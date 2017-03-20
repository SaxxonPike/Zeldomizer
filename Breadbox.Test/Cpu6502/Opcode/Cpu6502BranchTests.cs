using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace Breadbox.Test.Cpu6502.Opcode
{
    [TestFixture]
    [Parallelizable(ParallelScope.Self)]
    public class Cpu6502BranchTests : Cpu6502ExecutionBaseTestFixture
    {
        protected int CalculateBranch(int pc, int offset, bool taken)
        {
            if (taken)
            {
                return (pc + 2 + (offset >= 0x80 ? offset - 256 : offset)) & 0xFFFF;
            }
            return (pc + 2) & 0xFFFF;
        }

        [Test]
        public void Bpl([Range(0x20, 0xE0, 0x40)] int offset, [Range(0x0000, 0xFFFF, 0x5555)] int pc, [Range(0, 1)] int sign)
        {
            // Arrange
            Cpu.SetOpcode(0x10);
            Cpu.SetN(sign != 0);
            Cpu.SetPC(pc);
            MemoryMock.Setup(m => m.Read(It.IsAny<int>())).Returns(offset);
            var expectedPc = CalculateBranch(pc, offset, sign == 0);

            // Act
            Cpu.ClockStep();

            // Assert
            Cpu.PC.Should().Be(expectedPc);
        }

        [Test]
        public void Bmi([Range(0x20, 0xE0, 0x40)] int offset, [Range(0x0000, 0xFFFF, 0x5555)] int pc, [Range(0, 1)] int sign)
        {
            // Arrange
            Cpu.SetOpcode(0x30);
            Cpu.SetN(sign != 0);
            Cpu.SetPC(pc);
            MemoryMock.Setup(m => m.Read(It.IsAny<int>())).Returns(offset);
            var expectedPc = CalculateBranch(pc, offset, sign != 0);

            // Act
            Cpu.ClockStep();

            // Assert
            Cpu.PC.Should().Be(expectedPc);
        }

        [Test]
        public void Bvc([Range(0x20, 0xE0, 0x40)] int offset, [Range(0x0000, 0xFFFF, 0x5555)] int pc, [Range(0, 1)] int overflow)
        {
            // Arrange
            Cpu.SetOpcode(0x50);
            Cpu.SetV(overflow != 0);
            Cpu.SetPC(pc);
            MemoryMock.Setup(m => m.Read(It.IsAny<int>())).Returns(offset);
            var expectedPc = CalculateBranch(pc, offset, overflow == 0);

            // Act
            Cpu.ClockStep();

            // Assert
            Cpu.PC.Should().Be(expectedPc);
        }

        [Test]
        public void Bvs([Range(0x20, 0xE0, 0x40)] int offset, [Range(0x0000, 0xFFFF, 0x5555)] int pc, [Range(0, 1)] int overflow)
        {
            // Arrange
            Cpu.SetOpcode(0x70);
            Cpu.SetV(overflow != 0);
            Cpu.SetPC(pc);
            MemoryMock.Setup(m => m.Read(It.IsAny<int>())).Returns(offset);
            var expectedPc = CalculateBranch(pc, offset, overflow != 0);

            // Act
            Cpu.ClockStep();

            // Assert
            Cpu.PC.Should().Be(expectedPc);
        }

        [Test]
        public void Bcc([Range(0x20, 0xE0, 0x40)] int offset, [Range(0x0000, 0xFFFF, 0x5555)] int pc, [Range(0, 1)] int carry)
        {
            // Arrange
            Cpu.SetOpcode(0x90);
            Cpu.SetC(carry != 0);
            Cpu.SetPC(pc);
            MemoryMock.Setup(m => m.Read(It.IsAny<int>())).Returns(offset);
            var expectedPc = CalculateBranch(pc, offset, carry == 0);

            // Act
            Cpu.ClockStep();

            // Assert
            Cpu.PC.Should().Be(expectedPc);
        }

        [Test]
        public void Bcs([Range(0x20, 0xE0, 0x40)] int offset, [Range(0x0000, 0xFFFF, 0x5555)] int pc, [Range(0, 1)] int carry)
        {
            // Arrange
            Cpu.SetOpcode(0xB0);
            Cpu.SetC(carry != 0);
            Cpu.SetPC(pc);
            MemoryMock.Setup(m => m.Read(It.IsAny<int>())).Returns(offset);
            var expectedPc = CalculateBranch(pc, offset, carry != 0);

            // Act
            Cpu.ClockStep();

            // Assert
            Cpu.PC.Should().Be(expectedPc);
        }

        [Test]
        public void Beq([Range(0x20, 0xE0, 0x40)] int offset, [Range(0x0000, 0xFFFF, 0x5555)] int pc, [Range(0, 1)] int zero)
        {
            // Arrange
            Cpu.SetOpcode(0xD0);
            Cpu.SetZ(zero != 0);
            Cpu.SetPC(pc);
            MemoryMock.Setup(m => m.Read(It.IsAny<int>())).Returns(offset);
            var expectedPc = CalculateBranch(pc, offset, zero == 0);

            // Act
            Cpu.ClockStep();

            // Assert
            Cpu.PC.Should().Be(expectedPc);
        }

        [Test]
        public void Bne([Range(0x20, 0xE0, 0x40)] int offset, [Range(0x0000, 0xFFFF, 0x5555)] int pc, [Range(0, 1)] int zero)
        {
            // Arrange
            Cpu.SetOpcode(0xF0);
            Cpu.SetZ(zero != 0);
            Cpu.SetPC(pc);
            MemoryMock.Setup(m => m.Read(It.IsAny<int>())).Returns(offset);
            var expectedPc = CalculateBranch(pc, offset, zero != 0);

            // Act
            Cpu.ClockStep();

            // Assert
            Cpu.PC.Should().Be(expectedPc);
        }
    }
}
