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
        bool CpuAssertsRead(int address);
        bool CpuAssertsWrite(int address);
        bool PpuAssertsRead(int address);
        bool AssertsRdy { get; }
        void Reset();
    }
}
