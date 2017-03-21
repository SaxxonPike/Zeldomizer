using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Breadbox;

namespace Mimic
{
    public class NesSystem : IMemory, IReadySignal
    {
        private Mos6502 Cpu { get; }
        private IBus[] Devices { get; }
        private IBus Ppu { get; }
        private IBus WorkRam { get; }
        private IBus Sram { get; }

        public NesSystem(IEnumerable<IBus> devices)
        {
            WorkRam = new Ram(0x800, 0x2000, 0x0000, 0x7FF);
            Sram = new Ram(0x2000, 0x2000, 0x6000, 0x1FFF);
            Cpu = new Mos6502(new Mos6502Configuration(0xFF, false, this, this));
            Ppu = new Ppu();
            Devices = devices.Concat(new[] { WorkRam, Sram }).ToArray();

            Reset();
        }

        int IMemory.Read(int address)
        {
            foreach (var device in Devices)
                if (device.CpuAssertsRead(address))
                    return device.CpuRead(address);
            return 0xFF;
        }

        void IMemory.Write(int address, int value)
        {
            foreach (var device in Devices)
                if (device.CpuAssertsWrite(address))
                    device.CpuWrite(address, value);
        }

        int IMemory.Peek(int address)
        {
            foreach (var device in Devices)
                if (device.CpuAssertsRead(address))
                    return device.CpuPeek(address);
            return 0xFF;
        }

        void IMemory.Poke(int address, int value)
        {
            foreach (var device in Devices)
                if (device.CpuAssertsWrite(address))
                    device.CpuPoke(address, value);
        }

        bool IReadySignal.ReadRdy()
        {
            return Devices
                .All(device => !device.AssertsRdy || device.Rdy);
        }

        public int CpuPc => Cpu.PC;

        public ulong TotalCycles => Cpu.TotalCycles;

        public void Clock()
        {
            Cpu.Clock();
        }

        public void Clock(int count)
        {
            Cpu.ClockMultiple(count);
        }

        public void Reset()
        {
            foreach (var device in Devices)
                device.Reset();
        }

        public void ClockToAddress(int address, int timeout)
        {
            while (timeout-- > 0 && Cpu.PC != address)
                Cpu.Clock();
        }
    }
}
