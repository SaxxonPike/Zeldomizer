using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mimic
{
    public interface ITracer : IBus
    {
        bool Enabled { get; set; }
        Action<int, int> OnCpuRead { get; set; }
        Action<int, int> OnCpuWrite { get; set; }
        Action<int, int> OnPpuRead { get; set; }
    }
}
