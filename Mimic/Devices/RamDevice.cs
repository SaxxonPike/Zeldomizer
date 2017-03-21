namespace Mimic.Devices
{
    public sealed class RamDevice : RomDevice
    {
        public RamDevice(string name, int size, int length, int offset, int mask)
            : base(name, size, length, offset, mask)
        {
        }

        public override void CpuWrite(int address, int value) => Ram[address & Mask] = unchecked((byte) value);
        public override void CpuPoke(int address, int value) => CpuWrite(address, value);
        public override bool AssertsCpuWrite(int address) => AssertsCpuRead(address);
    }
}
