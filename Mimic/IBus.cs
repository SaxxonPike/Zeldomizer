using System.Collections;
using System.Threading;

namespace Mimic
{
    public interface IBus
    {
        int CpuRead(int address);
        void CpuWrite(int address, int value);
        int CpuPeek(int address);
        void CpuPoke(int address, int value);
        int PpuRead(int address);
        int PpuPeek(int address);
        bool Rdy { get; }
        bool AssertsCpuRead(int address);
        bool AssertsCpuWrite(int address);
        bool AssertsPpuRead(int address);
        bool AssertsRdy { get; }
        void Reset();
        void Clock();
        bool AssertsIrq { get; }
        bool AssertsNmi { get; }
        bool Irq { get; }
        bool Nmi { get; }
    }
}
