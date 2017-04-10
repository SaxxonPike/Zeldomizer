using System;
using NUnit.Framework;

namespace Breadbox
{
    [Parallelizable(ParallelScope.Fixtures)]
    public class PerformanceTests : BreadboxBaseTestFixture
    {
        [Test]
        [Explicit("This test is to be profiled, not simply run.")]
        public void ProfileCpu()
        {
            // 64kb setup
            var memory = new byte[0x10000];

            // Program setup
            var program = new byte[]
            {
                // INC $0200,X
                0xFE, 0x00, 0x02,

                // DEX
                0xCA,

                // DEC $0400,Y
                0xDE, 0x00, 0x04,

                // INY
                0xC8,

                // LDA $0300
                0xAD, 0x00, 0x03,

                // ADC #$03
                0x69, 0x03,

                // STA $0300
                0x8D, 0x00, 0x03,

                // PHP
                0x08,

                // JMP $0000
                0x4C, 0x00, 0x00
            };

            // Load program into memory
            Array.Copy(program, memory, program.Length);

            // Cpu setup
            var cpu = new Mos6502(new Mos6502Configuration(0xFF, true, a => memory[a], (a, d) => memory[a] = unchecked((byte)d), () => true, () => false, () => false));

            // Run some cycles.
            cpu.SetPC(0x0000);
            cpu.ClockMultiple(10000000);
        }
    }
}
