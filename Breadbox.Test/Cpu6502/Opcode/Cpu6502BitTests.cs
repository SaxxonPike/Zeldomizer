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
    public class Cpu6502BitTests : Cpu6502OpcodeBaseTestFixture
    {
        public Cpu6502BitTests() : base(0x24)
        {
        }

        [Test]
        public void Bit([Range(0x0, 0xF, 0x5)] int lowA, [Range(0x0, 0xF, 0x5)] int highA, [Range(0x0, 0xF, 0x5)] int lowData, [Range(0x0, 0xF, 0x5)] int highData)
        {
            // Arrange
            var a = lowA + (highA << 4);
            var data = lowData + (highData << 4);
            var expectedN = (data & 0x80) != 0;
            var expectedV = (data & 0x40) != 0;
            var expectedZ = (data & a) == 0;
            MemoryMock.Setup(m => m.Read(It.IsAny<int>())).Returns(data);
            Cpu.SetA(a);

            // Act
            Cpu.ClockStep();

            // Assert
            Cpu.A.Should().Be(a);
            Cpu.N.Should().Be(expectedN);
            Cpu.V.Should().Be(expectedV);
            Cpu.Z.Should().Be(expectedZ);
        }
    }
}
