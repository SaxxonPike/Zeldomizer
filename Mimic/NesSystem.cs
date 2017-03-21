using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Breadbox;

namespace Mimic
{
    public class NesSystem
    {
        private readonly Cpu _cpu;

        public NesSystem()
        {
            Router = new Router();
            _cpu = new Cpu(Router);

            Router.Install(new Ram(0x800, 0x2000, 0x0000, 0x7FF));
            Router.Install(new Ram(0x2000, 0x2000, 0x6000, 0x1FFF));
            Router.Install(new Ppu(Router));
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
