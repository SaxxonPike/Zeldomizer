namespace Mimic
{
    public class NesSystem
    {
        private readonly Cpu _cpu;

        public NesSystem()
        {
            Router = new Router("Router");
            _cpu = new Cpu("Cpu", Router);

            Router.Install(new Ram("Ram", 0x800, 0x2000, 0x0000, 0x7FF));
            Router.Install(new Ppu("Ppu", Router));
            Router.Install(_cpu);

            Reset();
        }

        public Router Router { get; }

        public void Clock() => Router.Clock();

        public void Clock(int count)
        {
            for (var i = 0; i < count; i++)
                Router.Clock();
        }

        public void Reset() => Router.Reset();

        public void ClockToAddress(int address, int timeout)
        {
            while (timeout-- > 0 && _cpu.CpuPc != address)
                Router.Clock();
        }
    }
}
