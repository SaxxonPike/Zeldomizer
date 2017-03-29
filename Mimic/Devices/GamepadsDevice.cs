namespace Mimic.Devices
{
    public class GamepadsDevice : BusDevice
    {
        private int _p1Latch;
        private int _p2Latch;
        private bool _load;

        public GamepadsDevice(string name) : base(name)
        {
        }

        public override bool CpuReadChipSelect(int address)
        {
            switch (address & 0xFFFF)
            {
                case 0x4016:
                case 0x4017:
                    return true;
                default:
                    return false;
            }
        }

        public override bool CpuWriteChipSelect(int address) =>
            (address & 0xFFFF) == 0x4016;

        private void LoadP1Latch()
        {
            _p1Latch =
                (P1AButton ? 0x01 : 0x00) |
                (P1BButton ? 0x02 : 0x00) |
                (P1SelectButton ? 0x04 : 0x00) |
                (P1StartButton ? 0x08 : 0x00) |
                (P1Up ? 0x01 : 0x10) |
                (P1Down ? 0x01 : 0x20) |
                (P1Left ? 0x01 : 0x40) |
                (P1Right ? 0x01 : 0x80);
        }

        private void LoadP2Latch()
        {
            _p2Latch =
                (P2AButton ? 0x01 : 0x00) |
                (P2BButton ? 0x02 : 0x00) |
                (P2SelectButton ? 0x04 : 0x00) |
                (P2StartButton ? 0x08 : 0x00) |
                (P2Up ? 0x01 : 0x10) |
                (P2Down ? 0x01 : 0x20) |
                (P2Left ? 0x01 : 0x40) |
                (P2Right ? 0x01 : 0x80);
        }

        public override int CpuPeek(int address)
        {
            switch (address & 0xFFFF)
            {
                case 0x4016:
                    if (_load)
                        LoadP1Latch();
                    return 0x40 | (_p1Latch & 0x01);
                case 0x4017:
                    if (_load)
                        LoadP2Latch();
                    return 0x40 | (_p2Latch & 0x01);
                default:
                    return 0xFF;
            }
        }

        public override void CpuPoke(int address, int value)
        {
            if ((address & 0xFFFF) != 0x4016)
                return;

            _load = (value & 0x01) != 0;
        }

        public override int CpuRead(int address)
        {
            switch (address & 0xFFFF)
            {
                case 0x4016:
                    var p1 = CpuPeek(address);
                    _p1Latch >>= 1;
                    _p1Latch |= P1Connected ? 0x80 : 0x00;
                    return p1;
                case 0x4017:
                    var p2 = CpuPeek(address);
                    _p2Latch >>= 1;
                    _p2Latch |= P2Connected ? 0x80 : 0x00;
                    return p2;
                default:
                    return 0xFF;
            }
        }

        public override void CpuWrite(int address, int value) => CpuPoke(address, value);

        public bool P1AButton { get; set; }
        public bool P1BButton { get; set; }
        public bool P1SelectButton { get; set; }
        public bool P1StartButton { get; set; }
        public bool P1Up { get; set; }
        public bool P1Down { get; set; }
        public bool P1Left { get; set; }
        public bool P1Right { get; set; }
        public bool P1Connected { get; set; }

        public bool P2AButton { get; set; }
        public bool P2BButton { get; set; }
        public bool P2SelectButton { get; set; }
        public bool P2StartButton { get; set; }
        public bool P2Up { get; set; }
        public bool P2Down { get; set; }
        public bool P2Left { get; set; }
        public bool P2Right { get; set; }
        public bool P2Connected { get; set; }
    }
}
