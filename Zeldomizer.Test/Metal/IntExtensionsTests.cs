using FluentAssertions;
using NUnit.Framework;

// ReSharper disable InvokeAsExtensionMethod

namespace Zeldomizer.Metal
{
    [Parallelizable(ParallelScope.Fixtures)]
    public class IntExtensionsTests : ZeldomizerBaseTestFixture
    {
        [Test]
        [TestCase(0xFE, 0, false)]
        [TestCase(0x01, 0, true)]
        [TestCase(0xFD, 1, false)]
        [TestCase(0x02, 1, true)]
        [TestCase(0xFB, 2, false)]
        [TestCase(0x04, 2, true)]
        [TestCase(0xF7, 3, false)]
        [TestCase(0x08, 3, true)]
        [TestCase(0xEF, 4, false)]
        [TestCase(0x10, 4, true)]
        [TestCase(0xDF, 5, false)]
        [TestCase(0x20, 5, true)]
        [TestCase(0xBF, 6, false)]
        [TestCase(0x40, 6, true)]
        [TestCase(0x7F, 7, false)]
        [TestCase(0x80, 7, true)]
        public void Bit_GetsBitValue(int input, int bitNumber, bool expected)
        {
            var actual = IntExtensions.Bit(input, bitNumber);
            actual.Should().Be(expected);
        }

        [Test]
        [TestCase(0x10, 0, false, 0x10)]
        [TestCase(0x11, 0, false, 0x10)]
        [TestCase(0x10, 0, true, 0x11)]
        [TestCase(0x11, 0, true, 0x11)]
        [TestCase(0x20, 1, false, 0x20)]
        [TestCase(0x22, 1, false, 0x20)]
        [TestCase(0x20, 1, true, 0x22)]
        [TestCase(0x22, 1, true, 0x22)]
        [TestCase(0x40, 2, false, 0x40)]
        [TestCase(0x44, 2, false, 0x40)]
        [TestCase(0x40, 2, true, 0x44)]
        [TestCase(0x44, 2, true, 0x44)]
        [TestCase(0x80, 3, false, 0x80)]
        [TestCase(0x88, 3, false, 0x80)]
        [TestCase(0x80, 3, true, 0x88)]
        [TestCase(0x88, 3, true, 0x88)]
        public void Bit_SetsBitValue(int input, int bitNumber, bool newValue, int expected)
        {
            var actual = IntExtensions.Bit(input, bitNumber, newValue);
            actual.Should().Be(expected);
        }

        [Test]
        [TestCase(0xFF, 7, 0, 0xFF)]
        [TestCase(0xFF, 6, 0, 0x7F)]
        [TestCase(0xFF, 5, 0, 0x3F)]
        [TestCase(0xFF, 4, 0, 0x1F)]
        [TestCase(0xFF, 3, 0, 0x0F)]
        [TestCase(0xFF, 2, 0, 0x07)]
        [TestCase(0xFF, 1, 0, 0x03)]
        [TestCase(0xFF, 0, 0, 0x01)]
        [TestCase(0xFF, 7, 1, 0x7F)]
        [TestCase(0xFF, 6, 1, 0x3F)]
        [TestCase(0xFF, 5, 1, 0x1F)]
        [TestCase(0xFF, 4, 1, 0x0F)]
        [TestCase(0xFF, 3, 1, 0x07)]
        [TestCase(0xFF, 2, 1, 0x03)]
        [TestCase(0xFF, 1, 1, 0x01)]
        [TestCase(0xFF, 7, 2, 0x3F)]
        [TestCase(0xFF, 6, 2, 0x1F)]
        [TestCase(0xFF, 5, 2, 0x0F)]
        [TestCase(0xFF, 4, 2, 0x07)]
        [TestCase(0xFF, 3, 2, 0x03)]
        [TestCase(0xFF, 2, 2, 0x01)]
        [TestCase(0xFF, 7, 3, 0x1F)]
        [TestCase(0xFF, 6, 3, 0x0F)]
        [TestCase(0xFF, 5, 3, 0x07)]
        [TestCase(0xFF, 4, 3, 0x03)]
        [TestCase(0xFF, 3, 3, 0x01)]
        [TestCase(0x0F, 7, 0, 0x0F)]
        [TestCase(0x0F, 6, 0, 0x0F)]
        [TestCase(0x0F, 5, 0, 0x0F)]
        [TestCase(0x0F, 4, 0, 0x0F)]
        [TestCase(0x0F, 3, 0, 0x0F)]
        [TestCase(0x0F, 2, 0, 0x07)]
        [TestCase(0x0F, 1, 0, 0x03)]
        [TestCase(0x0F, 0, 0, 0x01)]
        [TestCase(0x0F, 7, 1, 0x07)]
        [TestCase(0x0F, 6, 1, 0x07)]
        [TestCase(0x0F, 5, 1, 0x07)]
        [TestCase(0x0F, 4, 1, 0x07)]
        [TestCase(0x0F, 3, 1, 0x07)]
        [TestCase(0x0F, 2, 1, 0x03)]
        [TestCase(0x0F, 1, 1, 0x01)]
        [TestCase(0x0F, 7, 2, 0x03)]
        [TestCase(0x0F, 6, 2, 0x03)]
        [TestCase(0x0F, 5, 2, 0x03)]
        [TestCase(0x0F, 4, 2, 0x03)]
        [TestCase(0x0F, 3, 2, 0x03)]
        [TestCase(0x0F, 2, 2, 0x01)]
        [TestCase(0x0F, 7, 3, 0x01)]
        [TestCase(0x0F, 6, 3, 0x01)]
        [TestCase(0x0F, 5, 3, 0x01)]
        [TestCase(0x0F, 4, 3, 0x01)]
        [TestCase(0x0F, 3, 3, 0x01)]
        public void Bits_GetsValue(int input, int bitNumber, int downTo, int expected)
        {
            var actual = IntExtensions.Bits(input, bitNumber, downTo);
            actual.Should().Be(expected);
        }

        [Test]
        [TestCase(0b11111111, 7, 0, 0b00000001, 0b00000001)]
        [TestCase(0b10111111, 3, 0, 0b00000011, 0b10110011)]
        [TestCase(0b11111110, 7, 1, 0b00000001, 0b00000010)]
        [TestCase(0b11011111, 3, 1, 0b00000001, 0b11010011)]
        [TestCase(0b11111111, 7, 2, 0b00000101, 0b00010111)]
        [TestCase(0b11111110, 3, 2, 0b00000001, 0b11110110)]
        [TestCase(0b11111111, 7, 3, 0b00000001, 0b00001111)]
        [TestCase(0b10111100, 3, 3, 0b00000001, 0b10111100)]
        public void Bits_SetsValue(int input, int bitNumber, int downTo, int newValue, int expected)
        {
            var actual = IntExtensions.Bits(input, bitNumber, downTo, newValue);
            actual.Should().Be(expected);
        }
    }
}
