using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mimic
{
    public class Ppu : IBus
    {
        public int CpuRead(int address)
        {
            throw new NotImplementedException();
        }

        public void CpuWrite(int address, int value)
        {
            throw new NotImplementedException();
        }

        public int CpuPeek(int address)
        {
            throw new NotImplementedException();
        }

        public void CpuPoke(int address, int value)
        {
            throw new NotImplementedException();
        }

        public int PpuRead(int address)
        {
            throw new NotImplementedException();
        }

        public int PpuPeek(int address)
        {
            throw new NotImplementedException();
        }

        public bool Rdy { get; }
        public bool CpuAssertsRead(int address)
        {
            throw new NotImplementedException();
        }

        public bool CpuAssertsWrite(int address)
        {
            throw new NotImplementedException();
        }

        public bool PpuAssertsRead(int address)
        {
            throw new NotImplementedException();
        }

        public bool AssertsRdy { get; }
        public void Reset()
        {
            throw new NotImplementedException();
        }
    }
}
