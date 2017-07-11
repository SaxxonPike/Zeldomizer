using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace Breadbox
{
    [Parallelizable(ParallelScope.Fixtures)]
    public class IncTests : BreadboxBaseTestFixture
    {
        [Test]
        public void Inc([Range(0x0, 0xF, 0x5)] int lowOperand, [Range(0x0, 0xF, 0x5)] int highOperand)
        {
            // Arrange
            var operand = lowOperand + (highOperand << 4);
            var expectedResult = (operand + 1);
            var expectedSign = (expectedResult & 0x80) != 0;
            var expectedZero = (expectedResult & 0xFF) == 0;
            expectedResult &= 0xFF;
            System.SetupSequence(m => m.Read(It.IsAny<int>()))
                .Returns(0x00)
                .Returns(operand);
            Cpu.SetOpcode(0xE6);

            // Act
            Cpu.ClockStep();

            // Assert
            Cpu.Z.Should().Be(expectedZero, "Z must be set correctly");
            Cpu.N.Should().Be(expectedSign, "N must be set correctly");
            System.Verify(m => m.Write(0, operand));
            System.Verify(m => m.Write(0, expectedResult));
        }

        [Test]
        public void Inx([Range(0x0, 0xF, 0x5)] int lowX, [Range(0x0, 0xF, 0x5)] int highX)
        {
            // Arrange
            var x = lowX + (highX << 4);
            var expectedResult = (x + 1);
            var expectedSign = (expectedResult & 0x80) != 0;
            var expectedZero = (expectedResult & 0xFF) == 0;
            expectedResult &= 0xFF;
            Cpu.SetOpcode(0xE8);
            Cpu.SetX(x);

            // Act
            Cpu.ClockStep();

            // Assert
            Cpu.Z.Should().Be(expectedZero, "Z must be set correctly");
            Cpu.N.Should().Be(expectedSign, "N must be set correctly");
            Cpu.X.Should().Be(expectedResult, "X must be set correctly");
        }

        [Test]
        public void Iny([Range(0x0, 0xF, 0x5)] int lowY, [Range(0x0, 0xF, 0x5)] int highY)
        {
            // Arrange
            var y = lowY + (highY << 4);
            var expectedResult = (y + 1);
            var expectedSign = (expectedResult & 0x80) != 0;
            var expectedZero = (expectedResult & 0xFF) == 0;
            expectedResult &= 0xFF;
            Cpu.SetOpcode(0xC8);
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
