using System;

namespace Mimic
{
    public sealed class Mmc1 : BusDevice
    {
        private readonly byte[] _romData;

        /// <summary>
        /// Serial shift register. Five shifts cause a write.
        /// </summary>
        private int _ssr;

        /// <summary>
        /// Mirror mode. 0=lower only, 1=upper only, 2=vertical, 3=horizontal
        /// </summary>
        private int _mirror;

        /// <summary>
        /// PRG ROM access mode. 0,1=32k mode, 2=16k/fixed lower, 3=16k/fixed upper
        /// </summary>
        private int _prgMode;

        /// <summary>
        /// CHR ROM access mode. 0=8k mode, 1=4k mode
        /// </summary>
        private int _chrMode;

        /// <summary>
        /// CHR ROM low bank selection.
        /// </summary>
        private int _chrBank0;

        /// <summary>
        /// CHR ROM high bank selection.
        /// </summary>
        private int _chrBank1;

        /// <summary>
        /// PRG bank selection.
        /// </summary>
        private int _prgBank;

        /// <summary>
        /// Save RAM disable bit.
        /// </summary>
        private bool _ramDisabled;

        /// <summary>
        /// RAM located at 6000-7FFF.
        /// </summary>
        private readonly IBusDevice _ram;

        /// <summary>
        /// Uppermost PRG bank.
        /// </summary>
        private readonly int _maxPrgBank;

        /// <summary>
        /// Consecutive writes don't work; only the first is considered.
        /// This flag is true if no read has happened since the last write.
        /// </summary>
        private bool _writeLatch;

        /// <summary>
        /// Reset value for the serial shift register.
        /// </summary>
        private const int SsrResetValue = 0b100000;

        /// <summary>
        /// Create a MMC1 mapped device.
        /// </summary>
        /// <param name="name">Name of the device.</param>
        /// <param name="romData">ROM data to pull from.</param>
        public Mmc1(string name, byte[] romData) : base(name)
        {
            _romData = romData;
            _ram = new Ram("SaveRam", 0x2000, 0x2000, 0x6000, 0x1FFF);
            _maxPrgBank = (_romData.Length >> 14) - 1;
        }

        /// <summary>
        /// Get the offset in the ROM data for the CPU based on PRG settings.
        /// </summary>
        /// <param name="address">Address to fetch for.</param>
        private int GetPrgOffset(int address)
        {
            switch (_prgMode)
            {
                case 0x2:
                    // Fixed low bank
                    return (((address & 0x4000) == 0 ? 0 : _prgBank) << 14) |
                           (address & 0x3FFF);
                case 0x3:
                    // Fixed high bank
                    return (((address & 0x4000) == 0 ? _prgBank : _maxPrgBank) << 14) |
                           (address & 0x3FFF);
                default:
                    // 32k mode
                    return ((_prgBank & ~1) << 14) |
                           (address & 0x7FFF);
            }
        }

        /// <summary>
        /// Get the offset in the ROM data for the PPU based on CHR settings.
        /// </summary>
        /// <param name="address">Address to fetch for.</param>
        private int GetChrOffset(int address)
        {
            switch (_chrMode)
            {
                case 0x1:
                    // 4k mode
                    return (((address & 0x1000) == 0 ? _chrBank0 : _chrBank1) << 12) |
                           (address & 0x0FFF);
                default:
                    // 8k mode
                    return ((_chrBank0 & ~1) << 12) |
                           (address & 0x1FFF);
            }
        }

        /// <summary>
        /// Retrieve data for the CPU.
        /// </summary>
        /// <param name="address">Address to read from.</param>
        public override int CpuRead(int address)
        {
            // Reading enables writes again
            _writeLatch = false;
            return CpuPeek(address);
        }

        /// <summary>
        /// Accept data from the CPU.
        /// </summary>
        /// <param name="address">Address to write to.</param>
        /// <param name="value">Data to write.</param>
        public override void CpuWrite(int address, int value)
        {
            // Ignore consecutive writes.
            if (_writeLatch)
                return;
            _writeLatch = true;

            if ((address & 0xE000) == 0x6000)
            {
                // Write to save RAM.
                _ram.CpuWrite(address, value);
            }
            else if ((address & 0x8000) != 0)
            {
                // Bit 7 clears registers
                if ((value & 0x80) != 0)
                {
                    WriteRegister(address, 0);
                    _ssr = SsrResetValue;
                }
                else
                {
                    // Otherwise, shift in like a 5 bit SSR
                    _ssr >>= 1;
                    _ssr |= (value & 1) << 5;
                    if ((_ssr & 1) != 0)
                    {
                        WriteRegister(address, _ssr >> 1);
                        _ssr = SsrResetValue;
                    }
                }
            }
        }

        /// <summary>
        /// Write to an internal MMC1 register.
        /// </summary>
        /// <param name="address">Address of the register.</param>
        /// <param name="data">Data to write to the register.</param>
        private void WriteRegister(int address, int data)
        {
            switch (address & 0xE000)
            {
                case 0x8000:
                    // Control register
                    _mirror = data & 0b00000011;
                    _prgMode = (data & 0b00001100) >> 2;
                    _chrMode = data >> 4;
                    break;
                case 0xA000:
                    // CHR low bank
                    _chrBank0 = data;
                    break;
                case 0xC000:
                    // CHR high bank
                    _chrBank1 = data;
                    break;
                case 0xE000:
                    // PRG bank + save RAM enable
                    _prgBank = data & 0b00001111;
                    _ramDisabled = (data >> 4) != 0;
                    break;
            }
        }

        /// <summary>
        /// Simulate a read at the specified address without triggering internal changes.
        /// </summary>
        /// <param name="address">Address to read from.</param>
        public override int CpuPeek(int address)
        {
            switch (address & 0xE000)
            {
                case 0x6000:
                    // Save RAM
                    return _ram.CpuPeek(address);
                case 0x8000:
                case 0xA000:
                case 0xC000:
                case 0xE000:
                    // ROM
                    return _romData[GetPrgOffset(address)];
            }
            return 0xFF;
        }

        /// <summary>
        /// Simulate a write to the specified address without triggering internal changes.
        /// </summary>
        /// <param name="address">Address to write to.</param>
        /// <param name="value">Data to write.</param>
        public override void CpuPoke(int address, int value)
        {
            switch (address & 0xE000)
            {
                case 0x6000:
                    // Save RAM
                    _ram.CpuPoke(address, value);
                    break;
                case 0x8000:
                case 0xA000:
                case 0xC000:
                case 0xE000:
                    // Internal registers
                    WriteRegister(address, value);
                    break;
            }
        }

        /// <summary>
        /// Returns true if the given address is valid for reading from this device.
        /// </summary>
        /// <param name="address">Address to check.</param>
        public override bool AssertsCpuRead(int address)
        {
            switch (address & 0xE000)
            {
                case 0x6000:
                    // Save RAM
                    return !_ramDisabled;
                case 0x8000:
                case 0xA000:
                case 0xC000:
                case 0xE000:
                    // ROM
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        /// Returns true if the given address is valid for writing to this device.
        /// </summary>
        /// <param name="address">Address to check.</param>
        public override bool AssertsCpuWrite(int address)
        {
            switch (address & 0xE000)
            {
                case 0x6000:
                case 0x8000:
                case 0xA000:
                case 0xC000:
                case 0xE000:
                    // Save RAM and internal registers
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        /// Reset internal registers.
        /// </summary>
        public override void Reset()
        {
            _ssr = SsrResetValue;
            _mirror = 0;
            _prgMode = 0;
            _chrMode = 0;
            _chrBank0 = 0;
            _chrBank1 = 0;
            _prgBank = 0;
        }
    }
}
