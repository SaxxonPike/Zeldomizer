using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace Breadbox
{
    [Parallelizable(ParallelScope.Fixtures)]
    public class AdcTests : OpcodeBaseTestFixture
    {
        public AdcTests() : base(0x69)
        {
        }

        [Test]
        public void AdcNoDecimal([Range(0x0, 0xC, 0x4)] int lowA, [Range(0x0, 0xC, 0x4)] int highA, [Range(0x0, 0xC, 0x4)] int lowOperand, [Range(0x0, 0xC, 0x4)] int highOperand, [Range(0, 1)] int c)
        {
            // Arrange
            var a = lowA + (highA << 4);
            var operand = lowOperand + (highOperand << 4);
            var carry = c != 0;
            var expectedResult = a + operand + (carry ? 1 : 0);
            var expectedOverflow = ((a ^ expectedResult) & (a ^ operand) & 0x80) == 0;
            var expectedCarry = (expectedResult & 0x100) != 0;
            var expectedZero = (expectedResult & 0xFF) == 0;
            var expectedSign = (expectedResult & 0x80) != 0;
            Cpu.SetD(false);
            Cpu.SetC(carry);
            Cpu.SetA(a);
            System.SetupSequence(m => m.Read(It.IsAny<int>()))
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
        public void AdcDecimal([Range(0x0, 0xF, 0x5)] int lowA, [Range(0x0, 0xF, 0x5)] int highA, [Range(0x0, 0xF, 0x5)] int lowOperand, [Range(0x0, 0xF, 0x5)] int highOperand, [Range(0, 1)] int c)
        {
            // Arrange
            var a = lowA + (highA << 4);
            var operand = lowOperand + (highOperand << 4);
            var carry = c != 0;

            var lowTemp = lowA + lowOperand + c;
            if (lowTemp > 9)
                lowTemp += 6;
            var highTemp = highA + highOperand + (lowTemp > 0xF ? 1 : 0);
            var temp = (lowTemp & 0xF) + (highTemp << 4);

            var expectedZero = ((a + operand + c) & 0xFF) == 0;
            var expectedSign = (temp & 0x80) != 0;
            var expectedOverflow = ((a ^ temp) & (a ^ operand) & 0x80) == 0;
            if (temp > 0x99)
                temp += 96;
            var expectedCarry = temp > 0x99;
            var expectedResult = temp & 0xFF;
            Cpu.SetD(true);
            Cpu.SetC(carry);
            Cpu.SetA(a);
            System.SetupSequence(m => m.Read(It.IsAny<int>()))
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
