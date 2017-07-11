using Mimic.Interfaces;

namespace Mimic.Devices
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
        public virtual bool CpuReadChipSelect(int address) => false;
        public virtual bool CpuWriteChipSelect(int address) => false;
        public virtual bool PpuReadChipSelect(int address) => false;
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
