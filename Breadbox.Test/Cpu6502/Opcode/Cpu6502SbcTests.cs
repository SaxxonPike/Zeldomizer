using System;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace Breadbox.Test.Cpu6502.Opcode
{
    [TestFixture]
    [Parallelizable(ParallelScope.Self)]
    public class Cpu6502SbcTests : Cpu6502OpcodeBaseTestFixture
    {
        public Cpu6502SbcTests() : base(0xE9)
        {
        }

        [Test]
        public void SbcNoDecimal([Range(0x0, 0xC, 0x4)] int lowA, [Range(0x0, 0xC, 0x4)] int highA, [Range(0x0, 0xC, 0x4)] int lowOperand, [Range(0x0, 0xC, 0x4)] int highOperand, [Range(0, 1)] int c)
        {
            // Arrange
            var a = lowA + (highA << 4);
            var operand = lowOperand + (highOperand << 4);
            var carry = c != 0;
            var expectedResult = a - operand - (carry ? 0 : 1);
            var expectedSign = (expectedResult & 0x80) != 0;
            var expectedZero = (expectedResult & 0xFF) == 0;
            var expectedOverflow = ((a ^ expectedResult) & (a ^ operand) & 0x80) != 0;
            var expectedCarry = expectedResult >= 0;
            Cpu.SetD(false);
            Cpu.SetC(carry);
            Cpu.SetA(a);
            MemoryMock.SetupSequence(m => m.Read(It.IsAny<int>()))
                .Returns(operand);

            // Act
            Cpu.ClockStep();

            // Assert
            Cpu.V.Should().Be(expectedOverflow, "V must be set correctly");
            Cpu.Z.Should().Be(expectedZero, "Z must be set correctly");
            Cpu.N.Should().Be(expectedSign, "N must be set correctly");
            Cpu.C.Should().Be(expectedCarry, "C must be set correctly");
            Cpu.A.Should().Be(expectedResult & 0xFF, "A must be set correctly");
        }

        [Test]
        public void SbcDecimal([Range(0x0, 0xF, 0x5)] int lowA, [Range(0x0, 0xF, 0x5)] int highA, [Range(0x0, 0xF, 0x5)] int lowOperand, [Range(0x0, 0xF, 0x5)] int highOperand, [Range(0, 1)] int c)
        {
            // Arrange
            var a = lowA + (highA << 4);
            var operand = lowOperand + (highOperand << 4);
            var carry = c != 0;

            var temp = a - operand - (carry ? 0 : 1);
            var temp2 = (a & 0xF) - (operand & 0xF) - (carry ? 0 : 1);
            if ((temp2 & 0x10) != 0)
                temp2 = ((temp2 - 6) & 0xF) | ((a & 0xF0) - (operand & 0xF0) - 0x10);
            else
                temp2 = (temp2 & 0xF) | ((a & 0xF0) - (operand & 0xF0));
            if ((temp2 & 0x100) != 0)
                temp2 -= 0x60;
            var expectedCarry = temp >= 0;
            var expectedZero = (temp & 0xFF) == 0;
            var expectedSign = (temp & 0x80) != 0;
            var expectedOverflow = ((a ^ temp) & (a ^ operand) & 0x80) != 0;
            var expectedResult = temp2 & 0xFF;
            Cpu.SetD(true);
            Cpu.SetC(carry);
            Cpu.SetA(a);
            MemoryMock.SetupSequence(m => m.Read(It.IsAny<int>()))
                .Returns(operand);

            // Act
            Cpu.ClockStep();

            // Assert
            Cpu.V.Should().Be(expectedOverflow, "V must be set correctly");
            Cpu.Z.Should().Be(expectedZero, "Z must be set correctly");
            Cpu.N.Should().Be(expectedSign, "N must be set correctly");
            Cpu.C.Should().Be(expectedCarry, "C must be set correctly");
            Cpu.A.Should().Be(expectedResult & 0xFF, "A must be set correctly");
        }
    }
}
