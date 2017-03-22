using Mimic.Devices;

namespace Mimic.Systems
{
    public class NesSystem
    {
        private readonly Cpu6502Device _cpu6502Device;

        public NesSystem()
        {
            Router = new DeviceRouter("Router");
            _cpu6502Device = new Cpu6502Device("Cpu", Router);

            Router.Install(new RamDevice("Ram", 0x800, 0x2000, 0x0000, 0x7FF));
            Router.Install(new GamepadsDevice("Gamepads"));
            Router.Install(new ApuDevice("Apu"));
            Router.Install(new PpuDevice("Ppu", Router));
            Router.Install(_cpu6502Device);

            Reset();
        }

        public DeviceRouter Router { get; }

        public void Clock() => Router.Clock();

        public void Clock(int count)
        {
            for (var i = 0; i < count; i++)
                Router.Clock();
        }

        public void Reset() => Router.Reset();

        public void ClockToAddress(int address, int timeout)
        {
            while (timeout-- > 0 && _cpu6502Device.CpuPc != address)
                Router.Clock();
        }
    }
}
