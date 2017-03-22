namespace Mimic.Devices
{
    public class RomDevice : BusDevice
    {
        protected readonly int Offset;
        protected readonly int Mask;
        protected readonly byte[] Ram;
        protected readonly int UpperBound;

        public RomDevice(string name, int size, int length, int offset, int mask)
            : base(name)
        {
            Offset = offset;
            Mask = mask;
            Ram = new byte[size];
            UpperBound = offset + length;
        }

        public override int CpuRead(int address) => Ram[address & Mask];
        public override int CpuPeek(int address) => CpuRead(address);
        public override int PpuRead(int address) => CpuRead(address);
        public override int PpuPeek(int address) => CpuRead(address);
        public override bool CpuReadChipSelect(int address) => address >= Offset && address < UpperBound;
        public override bool PpuReadChipSelect(int address) => CpuReadChipSelect(address);
    }
}
