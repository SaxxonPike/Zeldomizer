using System;

namespace Mimic
{
    public sealed class Mmc1 : Bus
    {
        private readonly byte[] _romData;

        private int _ssr;
        private int _mirror;
        private int _prgMode;
        private int _chrMode;
        private int _chrBank0;
        private int _chrBank1;
        private int _prgBank;
        private int _ramMode;
        private readonly IBus _ram;
        private readonly int _maxPrgBank;

        private const int SsrResetValue = 0b100000;

        public Mmc1(byte[] romData)
        {
            _romData = romData;
            _ram = new Ram(0x2000, 0x2000, 0x6000, 0x1FFF);
            _maxPrgBank = (_romData.Length >> 14) - 1;
        }

        private static int GetPrgOffset(int address, int prgBank)
        {
            return (address & 0x3FFF) | (prgBank << 14);
        }

        public override int CpuRead(int address)
        {
            switch (address & 0xE000)
            {
                case 0x6000:
                    return _ram.CpuRead(address);
                case 0x8000:
                case 0xA000:
                    return _romData[GetPrgOffset(address, _prgBank)];
                case 0xC000:
                case 0xE000:
                    return _romData[GetPrgOffset(address, _maxPrgBank)];
            }
            return 0xFF;
        }

        public override void CpuWrite(int address, int value)
        {
            if ((address & 0xE000) == 0x6000)
            {
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

        private void WriteRegister(int address, int data)
        {
            // todo
            switch (address & 0xE000)
            {
                case 0x8000:
                    _mirror = data & 0b00000011;
                    _prgMode = (data & 0b00001100) >> 2;
                    _chrMode = data >> 4;
                    break;
                case 0xA000:
                    _chrBank0 = data;
                    break;
                case 0xC000:
                    _chrBank1 = data;
                    break;
                case 0xE000:
                    _prgBank = data & 0b00001111;
                    _ramMode = data >> 4;
                    break;
            }
        }

        public override int CpuPeek(int address)
        {
            switch (address & 0xE000)
            {
                case 0x6000:
                    return _ram.CpuPeek(address);
                case 0x8000:
                case 0xA000:
                    return _romData[GetPrgOffset(address, _prgBank)];
                case 0xC000:
                case 0xE000:
                    return _romData[GetPrgOffset(address, _maxPrgBank)];
            }
            return 0xFF;
        }

        public override void CpuPoke(int address, int value)
        {
            switch (address & 0xE000)
            {
                case 0x6000:
                    _ram.CpuPoke(address, value);
                    break;
                case 0x8000:
                case 0xA000:
                case 0xC000:
                case 0xE000:
                    WriteRegister(address, value);
                    break;
            }
        }

        public override bool AssertsCpuRead(int address)
        {
            switch (address & 0xE000)
            {
                case 0x6000:
                    return _ramMode == 0;
                case 0x8000:
                case 0xA000:
                case 0xC000:
                case 0xE000:
                    return true;
                default:
                    return false;
            }
        }

        public override bool AssertsCpuWrite(int address)
        {
            switch (address & 0xE000)
            {
                case 0x6000:
                case 0x8000:
                case 0xA000:
                case 0xC000:
                case 0xE000:
                    return true;
                default:
                    return false;
            }
        }

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
