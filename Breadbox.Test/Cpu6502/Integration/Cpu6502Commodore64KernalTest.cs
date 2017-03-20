using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Breadbox.System;
using Breadbox.Test.Properties;
using Moq;
using NUnit.Framework;

namespace Breadbox.Test.Cpu6502.Integration
{
    [TestFixture]
    [Parallelizable(ParallelScope.Self)]
    public class Cpu6502Commodore64KernalTest
    {
        protected bool TraceEnabled { get; set; }

        private class MockSignals : ILoRamSignal, IHiRamSignal, IGameSignal, IExRomSignal, ICharenSignal, IVicBank
        {
            public bool ReadLoRam()
            {
                return true;
            }

            public bool ReadHiRam()
            {
                return true;
            }

            public bool ReadGame()
            {
                return true;
            }

            public bool ReadExRom()
            {
                return true;
            }

            public bool ReadCharen()
            {
                return true;
            }

            public int ReadVicBank()
            {
                return 0x0000;
            }
        }

        private void TraceRead(int address)
        {
            if (TraceEnabled)
            {
                Console.WriteLine("READ   ${0:x4}", address);
            }
        }

        private void TraceWrite(int address, int value)
        {
            if (TraceEnabled)
            {
                Console.WriteLine("WRITE  ${0:x4} <- #${1:x2}", address, value);
            }
        }

        [Test]
        [Explicit("Manual only.")]
        public void Test1()
        {
            // Arrange
            var signalMock = new MockSignals();
            var ioMock = new Mock<IMemory>();
            var rasterLineRequests = 0;

            ioMock.Setup(m => m.Read(It.IsInRange(0xD400, 0xDFFF, Range.Inclusive))).Returns(0xFF);
            ioMock.Setup(m => m.Read(It.IsIn(0xD012))).Returns(() =>
            {
                rasterLineRequests = (rasterLineRequests + 1) & 0xFF;
                return rasterLineRequests;
            });

            var ram = new RamChip(16, 8);
            var color = new RamChip(10, 4);
            var basic = new RomChip(13, 8);
            var kernal = new RomChip(13, 8);
            var chargen = new RomChip(12, 8);
            var roml = new MemoryNull();
            var romh = new MemoryNull();

            kernal.Flash(Resources.kernal_901227_03.Select(i => (int)i).ToArray());
            basic.Flash(Resources.basic_901226_01.Select(i => (int)i).ToArray());

            TraceEnabled = false;
            var plaConfig = new Commodore64SystemPlaConfiguration(
                signalMock,
                signalMock,
                signalMock,
                signalMock,
                signalMock,
                ram,
                color,
                basic,
                kernal,
                chargen,
                roml,
                romh,
                ioMock.Object,
                signalMock
                );
            var pla = new Commodore64SystemPla(plaConfig);
            var bus = pla.SystemBus;
            var tracer = new MemoryTrace(bus, TraceRead, TraceWrite);

            var cpu = new Mos6502(new Mos6502Configuration(0xFF, true, null, tracer, new ReadySignalNull()));

            // Act
            cpu.ClockMultiple(3000000);

            // Assert
            var dump = ram.Dump();
            File.WriteAllBytes(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "dump.bin"), dump);
        }
    }
}
