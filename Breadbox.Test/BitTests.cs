using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace Breadbox
{
    [Parallelizable(ParallelScope.Fixtures)]
    public class BitTests : OpcodeBaseTestFixture
    {
        public BitTests() : base(0x24)
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
            System.Setup(m => m.Read(It.IsAny<int>())).Returns(data);
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
