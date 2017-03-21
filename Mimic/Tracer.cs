using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mimic
{
    public class Tracer : IBus
    {
        private readonly Action<int> _onRead;
        private readonly Action<int, int> _onWrite;

        public Tracer(Action<int> onRead, Action<int, int> onWrite)
        {
            _onRead = onRead;
            _onWrite = onWrite;
            Enabled = true;
        }

        public bool Enabled { get; set; }

        public int CpuRead(int address)
        {
            return 0xFF;
        }

        public void CpuWrite(int address, int value)
        {
            if (Enabled)
                _onWrite(address, value);
        }

        public int CpuPeek(int address)
        {
            return 0xFF;
        }

        public void CpuPoke(int address, int value)
        {
        }

        public int PpuRead(int address)
        {
            return 0xFF;
        }

        public int PpuPeek(int address)
        {
            return 0xFF;
        }

        public bool Rdy => true;

        public bool CpuAssertsRead(int address)
        {
            if (Enabled)
                _onRead(address);
            return false;
        }

        public bool CpuAssertsWrite(int address) => true;

        public bool PpuAssertsRead(int address) => false;

        public bool AssertsRdy => false;

        public void Reset()
        {
        }
    }
}
