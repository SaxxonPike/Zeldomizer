using System;
using System.Collections.Generic;
using System.Linq;
using Mimic.Interfaces;

namespace Mimic.Devices
{
    public sealed class DeviceRouter : IBusDevice
    {
        public DeviceRouter(string name)
        {
            Name = name;
            Devices = new List<IBusDevice>();
            Tracers = new Dictionary<IBusDevice, ITracer>();
        }

        public void Install(IBusDevice device)
        {
            if (!Tracers.ContainsKey(device) && !Devices.Contains(device))
                Devices.Insert(0, device);
        }

        public void Install(IEnumerable<IBusDevice> devices)
        {
            foreach (var device in devices)
                Install(device);
        }

        public ITracer InstallTraced(IBusDevice device)
        {
            if (Tracers.ContainsKey(device))
                return Tracers[device];

            if (Devices.Contains(device))
                Devices.Remove(device);

            var tracer = new DeviceTracer(device);
            Tracers[device] = tracer;
            Devices.Insert(0, tracer);
            return tracer;
        }

        public IEnumerable<ITracer> InstallTraced(IEnumerable<IBusDevice> devices) =>
            devices.Select(InstallTraced);

        public void Uninstall(IBusDevice device)
        {
            if (Tracers.ContainsKey(device))
            {
                var tracer = Tracers[device];
                Tracers.Remove(device);
                Devices.Remove(tracer);
            }
            else
            {
                Devices.Remove(device);
            }
        }

        public Action<int, int> OnCpuRead { get; set; }
        public Action<int, int> OnCpuWrite { get; set; }
        public Action<int, int> OnPpuRead { get; set; }
        public bool EnableTracing { get; set; }

        private List<IBusDevice> Devices { get; }

        private Dictionary<IBusDevice, ITracer> Tracers { get; }

        public int CpuRead(int address)
        {
            var result = 0xFF;
            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (var device in Devices)
                if (device.CpuReadChipSelect(address))
                    result &= device.CpuRead(address);
            if (EnableTracing)
                OnCpuRead?.Invoke(address, result);
            return result;
        }

        public void CpuWrite(int address, int value)
        {
            foreach (var device in Devices)
                if (device.CpuWriteChipSelect(address))
                    device.CpuWrite(address, value);
            if (EnableTracing)
                OnCpuWrite?.Invoke(address, value);
        }

        public int CpuPeek(int address)
        {
            var result = 0xFF;
            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (var device in Devices)
                if (device.CpuReadChipSelect(address))
                    result &= device.CpuPeek(address);
            return result;
        }

        public void CpuPoke(int address, int value)
        {
            foreach (var device in Devices)
                if (device.CpuWriteChipSelect(address))
                    device.CpuPoke(address, value);
        }

        public int PpuRead(int address)
        {
            var result = 0xFF;
            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (var device in Devices)
                if (device.CpuReadChipSelect(address))
                    result &= device.PpuRead(address);
            if (EnableTracing)
                OnPpuRead?.Invoke(address, result);
            return result;
        }

        public int PpuPeek(int address)
        {
            var result = 0xFF;
            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (var device in Devices)
                if (device.CpuReadChipSelect(address))
                    result &= device.PpuPeek(address);
            return result;
        }

        public bool Rdy
        {
            get
            {
                // ReSharper disable once LoopCanBeConvertedToQuery
                foreach (var device in Devices)
                    if (device.AssertsRdy && !device.Rdy)
                        return false;
                return true;
            }
        }

        public bool CpuReadChipSelect(int address)
        {
            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (var device in Devices)
                if (device.CpuReadChipSelect(address))
                    return true;
            return false;
        }

        public bool CpuWriteChipSelect(int address)
        {
            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (var device in Devices)
                if (device.CpuWriteChipSelect(address))
                    return true;
            return false;
        }

        public bool PpuReadChipSelect(int address)
        {
            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (var device in Devices)
                if (device.PpuReadChipSelect(address))
                    return true;
            return false;
        }

        public bool AssertsRdy
        {
            get
            {
                // ReSharper disable once LoopCanBeConvertedToQuery
                foreach (var device in Devices)
                    if (device.AssertsRdy)
                        return true;
                return false;
            }
        }


        public void Reset()
        {
            foreach (var device in Devices)
                device.Reset();
        }

        public void Clock()
        {
            foreach (var device in Devices)
                device.Clock();
        }

        public bool AssertsIrq
        {
            get
            {
                // ReSharper disable once LoopCanBeConvertedToQuery
                foreach (var device in Devices)
                    if (device.AssertsIrq)
                        return true;
                return false;
            }
        }

        public bool AssertsNmi
        {
            get
            {
                // ReSharper disable once LoopCanBeConvertedToQuery
                foreach (var device in Devices)
                    if (device.AssertsNmi)
                        return true;
                return false;
            }
        }


        public bool Irq
        {
            get
            {
                // ReSharper disable once LoopCanBeConvertedToQuery
                foreach (var device in Devices)
                    if (device.AssertsIrq && device.Irq)
                        return true;
                return false;
            }
        }


        public bool Nmi
        {
            get
            {
                // ReSharper disable once LoopCanBeConvertedToQuery
                foreach (var device in Devices)
                    if (device.AssertsNmi && device.Nmi)
                        return true;
                return false;
            }
        }


        public byte[] DumpAddressible()
        {
            return Enumerable
                .Range(0, 0x10000)
                .Select(i => unchecked((byte) CpuPeek(i)))
                .ToArray();
        }

        public string Name { get; }
    }
}
