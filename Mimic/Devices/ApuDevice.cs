namespace Mimic.Devices
{
    public sealed class ApuDevice : BusDevice
    {
        public ApuDevice(string name) : base(name)
        {
        }

        public override bool CpuReadChipSelect(int address)
        {
            return (address & 0xFFFF) == 0x4015;
        }

        public override bool CpuWriteChipSelect(int address)
        {
            var maskedAddress = address & 0xFFFF;
            return maskedAddress >= 0x4000 && maskedAddress < 0x4018;
        }

        public override int CpuPeek(int address)
        {
            switch (address & 0x1F)
            {
                case 0x15:
                    // Control register. No IRQ/DMA active, not implemented yet
                    return 0x20;
                default:
                    return 0xFF;
            }
        }

        public override int CpuRead(int address) => CpuPeek(address);
    }
}
