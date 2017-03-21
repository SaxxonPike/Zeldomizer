using System;

namespace Mimic.Interfaces
{
    public interface ITracer : IBusDevice
    {
        bool Enabled { get; set; }
        Action<int, int> OnCpuRead { get; set; }
        Action<int, int> OnCpuWrite { get; set; }
        Action<int, int> OnPpuRead { get; set; }
    }
}
