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
    public class Cpu6502DecTests : Cpu6502ExecutionBaseTestFixture
    {
        [Test]
        public void Dec([Range(0x0, 0xF, 0x5)] int lowOperand, [Range(0x0, 0xF, 0x5)] int highOperand)
        {
            // Arrange
            var operand = lowOperand + (highOperand << 4);
            var expectedResult = (operand - 1);
            var expectedSign = (expectedResult & 0x80) != 0;
            var expectedZero = (expectedResult & 0xFF) == 0;
            expectedResult &= 0xFF;
            MemoryMock.SetupSequence(m => m.Read(It.IsAny<int>()))
                .Returns(0x00)
                .Returns(operand);
            Cpu.SetOpcode(0xC6);

            // Act
            Cpu.ClockStep();

            // Assert
            Cpu.Z.Should().Be(expectedZero, "Z must be set correctly");
            Cpu.N.Should().Be(expectedSign, "N must be set correctly");
        }

        [Test]
        public void Dex([Range(0x0, 0xF, 0x5)] int lowX, [Range(0x0, 0xF, 0x5)] int highX)
        {
            // Arrange
            var x = lowX + (highX << 4);
            var expectedResult = (x - 1);
            var expectedSign = (expectedResult & 0x80) != 0;
            var expectedZero = (expectedResult & 0xFF) == 0;
            expectedResult &= 0xFF;
            Cpu.SetOpcode(0xCA);
            Cpu.SetX(x);

            // Act
            Cpu.ClockStep();

            // Assert
            Cpu.Z.Should().Be(expectedZero, "Z must be set correctly");
            Cpu.N.Should().Be(expectedSign, "N must be set correctly");
            Cpu.X.Should().Be(expectedResult, "X must be set correctly");
        }

        [Test]
        public void Dey([Range(0x0, 0xF, 0x5)] int lowY, [Range(0x0, 0xF, 0x5)] int highY)
        {
            // Arrange
            var y = lowY + (highY << 4);
            var expectedResult = (y - 1);
            var expectedSign = (expectedResult & 0x80) != 0;
            var expectedZero = (expectedResult & 0xFF) == 0;
            expectedResult &= 0xFF;
            Cpu.SetOpcode(0x88);
            Cpu.SetY(y);

            // Act
            Cpu.ClockStep();

            // Assert
            Cpu.Z.Should().Be(expectedZero, "Z must be set correctly");
            Cpu.N.Should().Be(expectedSign, "N must be set correctly");
            Cpu.Y.Should().Be(expectedResult, "Y must be set correctly");
        }
    }
}
