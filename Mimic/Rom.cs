﻿namespace Mimic
{
    public class Rom : BusDevice
    {
        protected readonly int Offset;
        protected readonly int Mask;
        protected readonly byte[] Ram;
        protected readonly int UpperBound;

        public Rom(string name, int size, int length, int offset, int mask)
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
        public override bool AssertsCpuRead(int address) => address >= Offset && address < UpperBound;
        public override bool AssertsPpuRead(int address) => AssertsCpuRead(address);
    }
}
