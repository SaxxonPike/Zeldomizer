using System;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace Breadbox
{
    [Parallelizable(ParallelScope.Fixtures)]
    public class CompareTests : BreadboxBaseTestFixture
    {
        private void CompareFlags(int register, int data)
        {
            var compare = register - data;
            var expectedN = (compare & 0x80) != 0;
            var expectedZ = compare == 0;
            var expectedC = compare >= 0;
            Cpu.N.Should().Be(expectedN, "N must be set correctly");
            Cpu.Z.Should().Be(expectedZ, "Z must be set correctly");
            Cpu.C.Should().Be(expectedC, "C must be set correctly");
        }

        [Test]
        public void Cmp([Range(0x0, 0xF, 0x5)] int lowA, [Range(0x0, 0xF, 0x5)] int highA, [Range(0x0, 0xF, 0x5)] int lowData, [Range(0x0, 0xF, 0x5)] int highData)
        {
            // Arrange
            var a = lowA + (highA << 4);
            var data = lowData + (highData << 4);
            Cpu.SetA(a);
            Cpu.SetOpcode(0xC9);
            System.Setup(m => m.Read(It.IsAny<int>())).Returns(data);

            // Act
            Cpu.ClockStep();

            // Assert
            Console.WriteLine("A=${0:x2}; CMP #${1:x2}", a, data);
            Cpu.A.Should().Be(a, "A must not be modified");
            CompareFlags(a, data);
        }

        [Test]
        public void Cpx([Range(0x0, 0xF, 0x5)] int lowX, [Range(0x0, 0xF, 0x5)] int highX, [Range(0x0, 0xF, 0x5)] int lowData, [Range(0x0, 0xF, 0x5)] int highData)
        {
            // Arrange
            var x = lowX + (highX << 4);
            var data = lowData + (highData << 4);
            Cpu.SetX(x);
            Cpu.SetOpcode(0xE0);
            System.Setup(m => m.Read(It.IsAny<int>())).Returns(data);

            // Act
            Cpu.ClockStep();

            // Assert
            Console.WriteLine("X=${0:x2}; CPX #${1:x2}", x, data);
            Cpu.X.Should().Be(x, "X must not be modified");
            CompareFlags(x, data);
        }

        [Test]
        public void Cpy([Range(0x0, 0xF, 0x5)] int lowY, [Range(0x0, 0xF, 0x5)] int highY, [Range(0x0, 0xF, 0x5)] int lowData, [Range(0x0, 0xF, 0x5)] int highData)
        {
            // Arrange
            var y = lowY + (highY << 4);
            var data = lowData + (highData << 4);
            Cpu.SetY(y);
            Cpu.SetOpcode(0xC0);
            System.Setup(m => m.Read(It.IsAny<int>())).Returns(data);

            // Act
            Cpu.ClockStep();

            // Assert
            Console.WriteLine("Y=${0:x2}; CPY #${1:x2}", y, data);
            Cpu.Y.Should().Be(y, "Y must not be modified");
            CompareFlags(y, data);
        }

    }
}
