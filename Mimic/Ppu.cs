using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mimic
{
    public class Ppu : Bus
    {
        private readonly IBus _system;

        public Ppu(IBus system)
        {
            _system = system;
        }
    }
}
