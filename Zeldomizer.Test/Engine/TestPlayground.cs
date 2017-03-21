using System;
using System.Collections.Generic;
using Disaster;
using Mimic;
using Mimic.Devices;
using Mimic.Systems;
using NUnit.Framework;

namespace Zeldomizer.Engine
{
    public class TestPlayground : BaseTestFixture
    {
        [Test]
        [Explicit]
        public void Test2()
        {
            var cartridge = new Mmc1CartridgeDevice("Cartridge", Rom.ExportRaw());
            var system = new NesSystem();
            var router = system.Router;

            router.Install(cartridge);

            router.OnCpuRead = (a, d) => Console.WriteLine($"READ   ${a:X4} -> ${d:X2}");
            router.OnCpuWrite = (a, d) => Console.WriteLine($"WRITE  ${a:X4} <- ${d:X2}");
            router.OnPpuRead = (a, d) => Console.WriteLine($"PPU    ${a:X4} -> ${d:X2}");
            router.EnableTracing = false;
            system.Clock(1000000);
            router.EnableTracing = true;
            system.Clock(1000);

            WriteToDesktop(router.DumpAddressible(), "mem.bin");
        }

        [Test]
        [Explicit]
        public void Test1()
        {
            var bank0 = new CodeBlock
            {
                Offset = 0x00000,
                Length = 0x04000,
                Origin = 0x8000,
                Rom = Rom
            };

            var bank0Hints = new int[]
            {
                0x9825, 0xBF50
            };

            var bank1 = new CodeBlock
            {
                Offset = 0x04000,
                Length = 0x04000,
                Origin = 0x8000,
                Rom = Rom
            };

            var bank1Hints = new int[]
            {
                0x8D47, 0x8D00
            };

            var bank2 = new CodeBlock
            {
                Offset = 0x08000,
                Length = 0x04000,
                Origin = 0x8000,
                Rom = Rom
            };

            var bank2Hints = new int[]
            {
                0x8012, 0x9000
            };

            var bank3 = new CodeBlock
            {
                Offset = 0x0C000,
                Length = 0x04000,
                Origin = 0x8000,
                Rom = Rom
            };

            var bank3Hints = new int[]
            {
                0x8044
            };

            var bank4 = new CodeBlock
            {
                Offset = 0x10000,
                Length = 0x04000,
                Origin = 0x8000,
                Rom = Rom
            };

            var bank4Hints = new int[]
            {

            };

            var bank5 = new CodeBlock
            {
                Offset = 0x14000,
                Length = 0x04000,
                Origin = 0x8000,
                Rom = Rom
            };

            var bank5Hints = new int[]
            {
                0x8929, 0xB05E, 0xB83A, 0x9328
            };

            var bank6 = new CodeBlock
            {
                Offset = 0x18000,
                Length = 0x04000,
                Origin = 0x8000,
                Rom = Rom
            };

            var bank6Hints = new int[]
            {

            };

            var bank7 = new CodeBlock
            {
                Offset = 0x1C000,
                Length = 0x04000,
                Origin = 0xC000,
                Rom = Rom
            };

            var bank7Hints = new int[]
            {
                0xE440, 0xE484, 0xFF50,
                0xEB96, 0xEBAA, 0xEBC0,
                0xEC1B, 0xEB62, 0xEB76,
                0xEB7E, 0xEB86, 0xEB8E,
                0xE94B, 0xE96F, 0xE977,
                0xE9D8, 0xEA6B, 0xE9A1,
                0xE9C3, 0xE9CB
            };

            var disassembler = new Disassembler();
            var analyzer = new StaticAnalyzer(disassembler);
            var bank0Analysis = analyzer.Analyze(new ICodeBlock[] { bank0, bank7 }, bank0Hints);
            var bank1Analysis = analyzer.Analyze(new ICodeBlock[] { bank1, bank7 }, bank1Hints);
            var bank2Analysis = analyzer.Analyze(new ICodeBlock[] { bank2, bank7 }, bank2Hints);
            var bank3Analysis = analyzer.Analyze(new ICodeBlock[] { bank3, bank7 }, bank3Hints);
            var bank4Analysis = analyzer.Analyze(new ICodeBlock[] { bank4, bank7 }, bank4Hints);
            var bank5Analysis = analyzer.Analyze(new ICodeBlock[] { bank5, bank7 }, bank5Hints);
            var bank6Analysis = analyzer.Analyze(new ICodeBlock[] { bank6, bank7 }, bank6Hints);
            var bank7Analysis = analyzer.Analyze(new ICodeBlock[] { bank7 }, bank7Hints);
        }
    }
}
