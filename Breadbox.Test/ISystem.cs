using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Breadbox
{
    public interface ISystem
    {
        int Read(int address);
        void Write(int address, int value);
        bool Ready { get; }
        bool Nmi { get; }
        bool Irq { get; }
    }
}
