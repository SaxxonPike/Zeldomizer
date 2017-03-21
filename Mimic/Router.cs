﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mimic
{
    public class Router : IBus
    {
        public Router()
        {
            Devices = new List<IBus>();
            Tracers = new Dictionary<IBus, ITracer>();
        }

        public void Install(IBus device)
        {
            if (!Tracers.ContainsKey(device) && !Devices.Contains(device))
                Devices.Insert(0, device);
        }

        public void Install(IEnumerable<IBus> devices)
        {
            foreach (var device in devices)
                Install(device);
        }

        public ITracer InstallTraced(IBus device)
        {
            if (Tracers.ContainsKey(device))
                return Tracers[device];

            if (Devices.Contains(device))
                Devices.Remove(device);

            var tracer = new Tracer(device);
            Tracers[device] = tracer;
            Devices.Insert(0, tracer);
            return tracer;
        }

        public IEnumerable<ITracer> InstallTraced(IEnumerable<IBus> devices) =>
            devices.Select(InstallTraced);

        public void Uninstall(IBus device)
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

        protected List<IBus> Devices { get; }

        protected Dictionary<IBus, ITracer> Tracers { get; }

        public int CpuRead(int address)
        {
            var result = Devices.Aggregate(0xFF,
                (value, device) => device.AssertsCpuRead(address)
                    ? device.CpuRead(address) & value
                    : value);
            if (EnableTracing)
                OnCpuRead?.Invoke(address, result);
            return result;
        }

        public void CpuWrite(int address, int value)
        {
            foreach (var device in Devices)
                if (device.AssertsCpuWrite(address))
                    device.CpuWrite(address, value);
            if (EnableTracing)
                OnCpuWrite?.Invoke(address, value);
        }

        public int CpuPeek(int address)
        {
            return Devices.Aggregate(0xFF,
                (value, device) => device.AssertsCpuRead(address)
                    ? device.CpuPeek(address) & value
                    : value);
        }

        public void CpuPoke(int address, int value)
        {
            foreach (var device in Devices)
                if (device.AssertsCpuWrite(address))
                    device.CpuPoke(address, value);
        }

        public int PpuRead(int address)
        {
            var result = Devices.Aggregate(0xFF,
                (value, device) => device.AssertsPpuRead(address)
                    ? device.PpuRead(address) & value
                    : value);
            if (EnableTracing)
                OnPpuRead?.Invoke(address, result);
            return result;
        }

        public int PpuPeek(int address)
        {
            return Devices.Aggregate(0xFF,
                (value, device) => device.AssertsPpuRead(address)
                    ? device.PpuPeek(address) & value
                    : value);
        }

        public bool Rdy => Devices
            .All(device => !device.AssertsRdy || device.Rdy);

        public bool AssertsCpuRead(int address) => Devices
            .Any(device => device.AssertsCpuRead(address));

        public bool AssertsCpuWrite(int address) => Devices
            .Any(device => device.AssertsCpuWrite(address));

        public bool AssertsPpuRead(int address) => Devices
            .Any(device => device.AssertsPpuRead(address));

        public bool AssertsRdy => Devices
            .Any(device => device.AssertsRdy);

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

        public bool AssertsIrq => Devices
            .Any(device => device.AssertsIrq);

        public bool AssertsNmi => Devices
            .Any(device => device.AssertsNmi);

        public bool Irq => Devices
            .Any(device => device.AssertsIrq && device.Irq);

        public bool Nmi => Devices
            .Any(device => device.AssertsNmi && device.Nmi);
    }
}
