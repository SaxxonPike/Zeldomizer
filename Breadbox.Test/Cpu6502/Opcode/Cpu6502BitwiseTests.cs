using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace Breadbox.Test.Cpu6502.Opcode
{
    [Parallelizable(ParallelScope.Self)]
    public class Cpu6502BitwiseTests : Cpu6502ExecutionBaseTestFixture
    {
        [Test]
        public void And([Range(0x0, 0xF, 0x5)] int lowA, [Range(0x0, 0xF, 0x5)] int highA, [Range(0x0, 0xF, 0x5)] int lowOperand, [Range(0x0, 0xF, 0x5)] int highOperand)
        {
            // Arrange
            var a = lowA + (highA << 4);
            var operand = lowOperand + (highOperand << 4);
            var expectedResult = a & operand;
            var expectedSign = (expectedResult & 0x80) != 0;
            var expectedZero = (expectedResult & 0xFF) == 0;
            var expectedOverflow = Cpu.V;
            var expectedCarry = Cpu.C;
            Cpu.SetOpcode(0x29);
            Cpu.SetA(a);
            MemoryMock.SetupSequence(m => m.Read(It.IsAny<int>()))
                .Returns(operand);

            // Act
            Cpu.ClockStep();

            // Assert
            Cpu.V.Should().Be(expectedOverflow, "V must not be modified");
            Cpu.Z.Should().Be(expectedZero, "Z must be set correctly");
            Cpu.N.Should().Be(expectedSign, "N must be set correctly");
            Cpu.C.Should().Be(expectedCarry, "C must not be modified");
            Cpu.A.Should().Be(expectedResult & 0xFF, "A must be set correctly");
        }

        [Test]
        public void Eor([Range(0x0, 0xF, 0x5)] int lowA, [Range(0x0, 0xF, 0x5)] int highA, [Range(0x0, 0xF, 0x5)] int lowOperand, [Range(0x0, 0xF, 0x5)] int highOperand)
        {
            // Arrange
            var a = lowA + (highA << 4);
            var operand = lowOperand + (highOperand << 4);
            var expectedResult = a ^ operand;
            var expectedSign = (expectedResult & 0x80) != 0;
            var expectedZero = (expectedResult & 0xFF) == 0;
            var expectedOverflow = Cpu.V;
            var expectedCarry = Cpu.C;
            Cpu.SetOpcode(0x49);
            Cpu.SetA(a);
            MemoryMock.SetupSequence(m => m.Read(It.IsAny<int>()))
                .Returns(operand);

            // Act
            Cpu.ClockStep();

            // Assert
            Cpu.V.Should().Be(expectedOverflow, "V must not be modified");
            Cpu.Z.Should().Be(expectedZero, "Z must be set correctly");
            Cpu.N.Should().Be(expectedSign, "N must be set correctly");
            Cpu.C.Should().Be(expectedCarry, "C must not be modified");
            Cpu.A.Should().Be(expectedResult & 0xFF, "A must be set correctly");
        }

        [Test]
        public void Ora([Range(0x0, 0xF, 0x5)] int lowA, [Range(0x0, 0xF, 0x5)] int highA, [Range(0x0, 0xF, 0x5)] int lowOperand, [Range(0x0, 0xF, 0x5)] int highOperand)
        {
            // Arrange
            var a = lowA + (highA << 4);
            var operand = lowOperand + (highOperand << 4);
            var expectedResult = a | operand;
            var expectedSign = (expectedResult & 0x80) != 0;
            var expectedZero = (expectedResult & 0xFF) == 0;
            var expectedOverflow = Cpu.V;
            var expectedCarry = Cpu.C;
            Cpu.SetOpcode(0x09);
            Cpu.SetA(a);
            MemoryMock.SetupSequence(m => m.Read(It.IsAny<int>()))
                .Returns(operand);

            // Act
            Cpu.ClockStep();

            // Assert
            Cpu.V.Should().Be(expectedOverflow, "V must not be modified");
            Cpu.Z.Should().Be(expectedZero, "Z must be set correctly");
            Cpu.N.Should().Be(expectedSign, "N must be set correctly");
            Cpu.C.Should().Be(expectedCarry, "C must not be modified");
            Cpu.A.Should().Be(expectedResult & 0xFF, "A must be set correctly");
        }
    }
}
