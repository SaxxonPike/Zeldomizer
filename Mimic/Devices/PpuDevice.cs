using Mimic.Interfaces;

namespace Mimic.Devices
{
    /// <summary>
    /// Graphics unit for the Famicom and Nintendo Entertainment System consoles.
    /// </summary>
    public sealed class PpuDevice : BusDevice
    {
        /// <summary>
        /// System to perform reads from.
        /// </summary>
        private readonly IBusDevice _system;

        /// <summary>
        /// Horizontal position of the raster counter.
        /// </summary>
        private int _rasterX;

        /// <summary>
        /// Vertical position of the raster counter.
        /// </summary>
        private int _rasterY;

        /// <summary>
        /// Number of units horizontally the raster counter will count.
        /// </summary>
        private const int RasterWidth = 341;

        /// <summary>
        /// Number of units vertically the raster counter will count.
        /// </summary>
        private const int RasterHeight = 262;

        /// <summary>
        /// Vertical raster position at the beginning of which blanking begins.
        /// </summary>
        private const int NmiStart = 241;

        /// <summary>
        /// Vertical raster position at the beginning of which blanking ends.
        /// </summary>
        private const int NmiEnd = 261;

        /// <summary>
        /// NMI output to the CPU.
        /// </summary>
        private bool _nmiOutput;

        /// <summary>
        /// If true, NMI generation is enabled during blanking.
        /// </summary>
        private bool _nmiEnabled;

        /// <summary>
        /// Value which is returned by the vertical blank register. When read, this flag is cleared.
        /// </summary>
        private bool _nmiRegister;

        /// <summary>
        /// Data previously present on the PPU bus.
        /// </summary>
        private int _lastData;

        /// <summary>
        /// Create a PPU device.
        /// </summary>
        /// <param name="name">Name of the device.</param>
        /// <param name="system">System to perform reads from.</param>
        public PpuDevice(string name, IBusDevice system) : base(name)
        {
            _system = system;
        }

        /// <summary>
        /// Execute three PPU clocks, which is equivalent timing to one CPU clock.
        /// </summary>
        public override void Clock()
        {
            for (var i = 3; i > 0; i--)
            {
                _rasterX++;
                if (_rasterX == RasterWidth)
                {
                    _rasterX = 0;
                    switch (++_rasterY)
                    {
                        case RasterHeight:
                            _rasterY = 0;
                            break;
                        case NmiStart:
                            _nmiRegister = true;
                            _nmiOutput = _nmiEnabled;
                            break;
                        case NmiEnd:
                            _nmiRegister = _nmiOutput = false;
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// If true, the specified address is valid when reading from the PPU.
        /// </summary>
        /// <param name="address">Address to check.</param>
        public override bool CpuReadChipSelect(int address) => address == 0x4014 || (address & 0xE000) == 0x2000;

        /// <summary>
        /// If true, the specified address is valid when writing to the PPU.
        /// </summary>
        /// <param name="address">Address to check.</param>
        public override bool CpuWriteChipSelect(int address) => CpuReadChipSelect(address);

        /// <summary>
        /// Retrieve data for the CPU.
        /// </summary>
        /// <param name="address">Address to read from.</param>
        public override int CpuRead(int address)
        {
            var value = CpuPeek(address);
            switch (address & 0x7)
            {
                case 0x2:
                    _nmiRegister = false;
                    break;
            }
            _lastData = value;
            return value;
        }

        /// <summary>
        /// Get data from the specified address without triggering internal side effects.
        /// </summary>
        /// <param name="address">Address to read from.</param>
        public override int CpuPeek(int address)
        {
            switch (address & 0x7)
            {
                case 0x2:
                    return (_nmiRegister ? 0x80 : 0x00) | (_lastData & 0x1F);
            }
            return _lastData;
        }

        /// <summary>
        /// Accept data from the CPU.
        /// </summary>
        /// <param name="address">Address to write to.</param>
        /// <param name="value">Data to write.</param>
        public override void CpuWrite(int address, int value)
        {
            CpuPoke(address, value);
            _lastData = value;
        }

        /// <summary>
        /// Write data to the specified address without triggering internal side effects.
        /// </summary>
        /// <param name="address">Address to write to.</param>
        /// <param name="value">Data to write.</param>
        public override void CpuPoke(int address, int value)
        {
            switch (address & 0x7)
            {
                case 0x0:
                    _nmiEnabled = (value & 0x80) != 0;
                    break;
            }
        }

        /// <summary>
        /// Returns true. The PPU can control the CPU's NMI.
        /// </summary>
        public override bool AssertsNmi => true;

        /// <summary>
        /// Retrieve the current NMI output state of the PPU.
        /// </summary>
        public override bool Nmi => _nmiOutput;
    }
}
