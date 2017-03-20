using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentAssertions;
using NUnit.Framework;

namespace Breadbox.Test.Cpu6502.Opcode
{
    [TestFixture]
    [Parallelizable(ParallelScope.Self)]
    public class Cpu6502TransferTests : Cpu6502ExecutionBaseTestFixture
    {
        [Test]
        public void Tax([Random(0x00, 0x7F, 1)] int r0, [Random(0x80, 0xFF, 1)] int r1)
        {
            // Arrange
            Cpu.SetA(r0);
            Cpu.SetX(r1);
            Cpu.SetOpcode(0xAA);

            // Act
            Cpu.ClockStep();

            // Assert
            Cpu.X.Should().Be(r0);
        }

        [Test]
        public void Txa([Random(0x00, 0x7F, 1)] int r0, [Random(0x80, 0xFF, 1)] int r1)
        {
            // Arrange
            Cpu.SetA(r0);
            Cpu.SetX(r1);
            Cpu.SetOpcode(0x8A);

            // Act
            Cpu.ClockStep();

            // Assert
            Cpu.A.Should().Be(r1);
        }

        [Test]
        public void Tay([Random(0x00, 0x7F, 1)] int r0, [Random(0x80, 0xFF, 1)] int r1)
        {
            // Arrange
            Cpu.SetA(r0);
            Cpu.SetY(r1);
            Cpu.SetOpcode(0xA8);

            // Act
            Cpu.ClockStep();

            // Assert
            Cpu.Y.Should().Be(r0);
        }

        [Test]
        public void Tya([Random(0x00, 0x7F, 1)] int r0, [Random(0x80, 0xFF, 1)] int r1)
        {
            // Arrange
            Cpu.SetA(r0);
            Cpu.SetY(r1);
            Cpu.SetOpcode(0x98);

            // Act
            Cpu.ClockStep();

            // Assert
            Cpu.A.Should().Be(r1);
        }

        [Test]
        public void Tsx([Random(0x00, 0x7F, 1)] int r0, [Random(0x80, 0xFF, 1)] int r1)
        {
            // Arrange
            Cpu.SetS(r0);
            Cpu.SetX(r1);
            Cpu.SetOpcode(0xBA);

            // Act
            Cpu.ClockStep();

            // Assert
            Cpu.X.Should().Be(r0);
        }

        [Test]
        public void Txs([Random(0x00, 0x7F, 1)] int r0, [Random(0x80, 0xFF, 1)] int r1)
        {
            // Arrange
            Cpu.SetS(r0);
            Cpu.SetX(r1);
            Cpu.SetOpcode(0x9A);

            // Act
            Cpu.ClockStep();

            // Assert
            Cpu.S.Should().Be(r1);
        }
    }
}
