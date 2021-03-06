﻿using System;
using Mimic.Interfaces;

namespace Mimic.Devices
{
    public sealed class DeviceTracer : ITracer
    {
        private readonly IBusDevice _subject;

        public DeviceTracer(IBusDevice subject)
        {
            _subject = subject;
            Enabled = true;
        }

        public bool Enabled { get; set; }

        public Action<int, int> OnCpuRead { get; set; }
        public Action<int, int> OnCpuWrite { get; set; }
        public Action<int, int> OnPpuRead { get; set; }

        public int CpuRead(int address)
        {
            var value = _subject.CpuRead(address);
            if (Enabled)
                OnCpuRead?.Invoke(address, value);
            return value;
        }

        public void CpuWrite(int address, int value)
        {
            _subject.CpuWrite(address, value);
            if (Enabled)
                OnCpuWrite?.Invoke(address, value);
        }

        public int CpuPeek(int address) => _subject.CpuPeek(address);
        public void CpuPoke(int address, int value) => _subject.CpuPoke(address, value);

        public int PpuRead(int address)
        {
            var value = _subject.PpuRead(address);
            if (Enabled)
                OnPpuRead?.Invoke(address, value);
            return value;
        }

        public int PpuPeek(int address) => _subject.PpuPeek(address);
        public bool Rdy => _subject.Rdy;
        public bool CpuReadChipSelect(int address) => _subject.CpuReadChipSelect(address);
        public bool CpuWriteChipSelect(int address) => _subject.CpuWriteChipSelect(address);
        public bool PpuReadChipSelect(int address) => _subject.PpuReadChipSelect(address);
        public bool AssertsRdy => _subject.AssertsRdy;
        public void Reset() => _subject.Reset();
        public void Clock() => _subject.Clock();
        public bool AssertsIrq => _subject.AssertsIrq;
        public bool AssertsNmi => _subject.AssertsNmi;
        public bool Irq => _subject.Irq;
        public bool Nmi => _subject.Nmi;
        public string Name => _subject.Name;
    }
}
