﻿using System;
using Breadbox;
using Mimic.Interfaces;

namespace Mimic.Devices
{
    /// <summary>
    /// A 6502 processor.
    /// </summary>
    public sealed class Cpu6502Device : BusDevice, IMemory, IReadySignal, IIrqSignal, INmiSignal
    {
        /// <summary>
        /// Device the CPU will use to perform reads and writes.
        /// </summary>
        private readonly IBusDevice _busDevice;

        /// <summary>
        /// Breadbox 6502 core.
        /// </summary>
        private readonly Mos6502 _cpu;
        private readonly Action<int, int> _busWrite;
        private readonly Func<int, int> _busRead;

        /// <summary>
        /// Create a CPU device using the specified device as a read/write target.
        /// </summary>
        /// <param name="name">Name of the CPU device.</param>
        /// <param name="busDevice">Device to target with read/write operations.</param>
        public Cpu6502Device(string name, IBusDevice busDevice) : base(name)
        {
            _busDevice = busDevice;
            _cpu = new Mos6502(new Mos6502Configuration(0xFF, false, this, this, this, this));
            _busWrite = busDevice.CpuWrite;
            _busRead = busDevice.CpuRead;
        }

        int IMemory.Read(int address) => _busRead(address);
        void IMemory.Write(int address, int value) => _busWrite(address, value);
        bool IReadySignal.ReadRdy() => _busDevice.Rdy;
        bool IIrqSignal.ReadIrq() => _busDevice.Irq;
        bool INmiSignal.ReadNmi() => _busDevice.Nmi;

        /// <summary>
        /// Perform a soft reset on the CPU.
        /// </summary>
        public override void Reset() => _cpu.SoftReset();

        /// <summary>
        /// Execute one clock cycle on the CPU.
        /// </summary>
        public override void Clock() => _cpu.Clock();

        /// <summary>
        /// Current PC, or Program Counter, which indicates where
        /// the CPU will run code from.
        /// </summary>
        public int Pc
        {
            get => _cpu.PC;
            set
            {
                _cpu.SetPC(value);
                _cpu.ForceOpcodeSync();
            }
        }

        /// <summary>
        /// Current value of the X register.
        /// </summary>
        public int A
        {
            get => _cpu.A;
            set => _cpu.SetA(value);
        }

        /// <summary>
        /// Current value of the X register.
        /// </summary>
        public int X
        {
            get => _cpu.X;
            set => _cpu.SetX(value);
        }

        /// <summary>
        /// Current value of the X register.
        /// </summary>
        public int Y
        {
            get => _cpu.Y;
            set => _cpu.SetY(value);
        }

        /// <summary>
        /// Retrieve the total number of cycles that the CPU has executed.
        /// </summary>
        public ulong TotalCycles => _cpu.TotalCycles;
    }
}
