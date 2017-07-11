using FluentAssertions;
using NUnit.Framework;

namespace Breadbox
{
    [Parallelizable(ParallelScope.Fixtures)]
    public class FlagOpcodeTests : BreadboxBaseTestFixture
    {
        [Test]
        public void Clc([Range(0, 1)] int c)
        {
            // Arrange
            Cpu.SetOpcode(0x18);
            Cpu.SetC(c != 0);

            // Act
            Cpu.ClockStep();

            // Assert
            Cpu.C.Should().BeFalse("C must be cleared");
        }

        [Test]
        public void Sec([Range(0, 1)] int c)
        {
            // Arrange
            Cpu.SetOpcode(0x38);
            Cpu.SetC(c != 0);

            // Act
            Cpu.ClockStep();

            // Assert
            Cpu.C.Should().BeTrue("C must be set");
        }

        [Test]
        public void Cli([Range(0, 1)] int i)
        {
            // Arrange
            Cpu.SetOpcode(0x58);
            Cpu.SetI(i != 0);

            // Act
            Cpu.ClockStep();

            // Assert
            Cpu.I.Should().BeFalse("I must be cleared");
        }

        [Test]
        public void Sei([Range(0, 1)] int i)
        {
            // Arrange
            Cpu.SetOpcode(0x78);
            Cpu.SetI(i != 0);

            // Act
            Cpu.ClockStep();

            // Assert
            Cpu.I.Should().BeTrue("I must be set");
        }

        [Test]
        public void Clv([Range(0, 1)] int v)
        {
            // Arrange
            Cpu.SetOpcode(0xB8);
            Cpu.SetV(v != 0);

            // Act
            Cpu.ClockStep();

            // Assert
            Cpu.V.Should().BeFalse("V must be cleared");
        }

        [Test]
        public void Cld([Range(0, 1)] int d)
        {
            // Arrange
            Cpu.SetOpcode(0xD8);
            Cpu.SetD(d != 0);

            // Act
            Cpu.ClockStep();

            // Assert
            Cpu.D.Should().BeFalse("D must be cleared");
        }

        [Test]
        public void Sed([Range(0, 1)] int d)
        {
            // Arrange
            Cpu.SetOpcode(0xF8);
            Cpu.SetD(d != 0);

            // Act
            Cpu.ClockStep();

            // Assert
            Cpu.D.Should().BeTrue("D must be set");
        }
    }
}
