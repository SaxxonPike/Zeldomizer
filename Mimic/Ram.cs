namespace Mimic
{
    public sealed class Ram : Rom
    {
        public Ram(int size, int length, int offset, int mask)
            : base(size, length, offset, mask)
        {
        }

        public override void CpuWrite(int address, int value) => Ram[address & Mask] = unchecked((byte) value);
        public override void CpuPoke(int address, int value) => CpuWrite(address, value);
        public override bool AssertsCpuWrite(int address) => AssertsCpuRead(address);
    }
}
