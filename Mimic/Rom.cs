using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mimic
{
    public class Rom : IBus
    {
        private readonly int _offset;
        private readonly int _mask;
        private readonly byte[] _ram;
        private readonly int _upperBound;

        public Rom(int capacity, int offset, int mask)
        {
            _offset = offset;
            _mask = mask;
            _ram = new byte[capacity];
            _upperBound = offset + capacity;
        }

        public int CpuRead(int address)
        {
            return _ram[address & _mask];
        }

        public void CpuWrite(int address, int value)
        {
            // Do nothing.
        }

        public int CpuPeek(int address) => CpuRead(address);

        public void CpuPoke(int address, int value) => CpuPoke(address, value);

        public int PpuRead(int address) => CpuRead(address);

        public int PpuPeek(int address) => CpuRead(address);

        public bool Rdy => true;

        public bool CpuAssertsRead(int address)
        {
            return address >= _offset && address < _upperBound;
        }

        public bool PpuAssertsRead(int address) => CpuAssertsRead(address);

        public bool AssertsRdy => false;

        public bool CpuAssertsWrite(int address)
        {
            return false;
        }

        public void Reset()
        {
        }
    }
}
