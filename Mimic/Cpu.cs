using System;
using Breadbox;

namespace Mimic
{
    public sealed class Cpu : Bus, IMemory, IReadySignal
    {
        private readonly IBus _bus;
        private readonly Mos6502 _cpu;

        public Cpu(IBus bus)
        {
            _bus = bus;
            _cpu = new Mos6502(new Mos6502Configuration(0xFF, false, this, this));
        }

        int IMemory.Read(int address) => _bus.CpuRead(address);
        void IMemory.Write(int address, int value) => _bus.CpuWrite(address, value);
        int IMemory.Peek(int address) => _bus.CpuPeek(address);
        void IMemory.Poke(int address, int value) => _bus.CpuPoke(address, value);
        bool IReadySignal.ReadRdy() => _bus.Rdy;

        public override void Reset() => _cpu.SoftReset();
        public override void Clock() => _cpu.Clock();
        public int CpuPc => _cpu.PC;
        public ulong TotalCycles => _cpu.TotalCycles;
    }
}
