using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mimic
{
    public class BusDevice : IBusDevice
    {
        public BusDevice(string name)
        {
            Name = name;
        }

        public virtual int CpuRead(int address) => 0xFF;
        public virtual void CpuWrite(int address, int value) { }
        public virtual int CpuPeek(int address) => 0xFF;
        public virtual void CpuPoke(int address, int value) { }
        public virtual int PpuRead(int address) => 0xFF;
        public virtual int PpuPeek(int address) => 0xFF;
        public virtual bool Rdy => true;
        public virtual bool AssertsCpuRead(int address) => false;
        public virtual bool AssertsCpuWrite(int address) => false;
        public virtual bool AssertsPpuRead(int address) => false;
        public virtual bool AssertsRdy => false;
        public virtual void Reset() { }
        public virtual void Clock() { }
        public virtual bool AssertsIrq => false;
        public virtual bool AssertsNmi => false;
        public virtual bool Irq => false;
        public virtual bool Nmi => false;
        public virtual string Name { get; }
    }
}
