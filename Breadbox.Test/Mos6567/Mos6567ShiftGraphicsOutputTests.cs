using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentAssertions;
using NUnit.Framework;

namespace Breadbox.Test.Mos6567
{
    [TestFixture]
    [Parallelizable(ParallelScope.Self)]
    public class Mos6567ShiftGraphicsOutputTests : Mos6567BaseTestFixture
    {
        private static void Verify(GraphicsShiftOutput result, int expectedData, bool expectedMultiColorToggle)
        {
            var observedMultiColorToggle = result.MultiColorToggle;
            var observedData = result.ShiftRegisterData;

            observedData.Should().Be(expectedData, "shift register should be correct");
            observedMultiColorToggle.Should().Be(expectedMultiColorToggle, "multicolor toggle should be correct");
        }

        [Test]
        public void StandardTextMode([Range(0x00, 0xC0, 0x40)] int srData)
        {
            // Arrange
            var expectedData = srData << 1;
            const bool expectedMultiColorToggle = true;

            // Act
            var result = Vic.TestClockedGraphics(false, false, false, 0x000, srData);

            // Assert
            Verify(result, expectedData, expectedMultiColorToggle);
        }

        [Test]
        public void StandardBitmapMode([Range(0x00, 0xC0, 0x40)] int srData)
        {
            // Arrange
            var expectedData = srData << 1;
            const bool expectedMultiColorToggle = true;

            // Act
            var result = Vic.TestClockedGraphics(true, false, false, 0x000, srData);

            // Assert
            Verify(result, expectedData, expectedMultiColorToggle);
        }

        [Test]
        public void MultiColorTextMode([Range(0x00, 0xC0, 0x40)] int srData, [Range(0, 1)] int multiColorToggle, [Range(0x000, 0xC00, 0x400)] int c)
        {
            // Arrange
            var inputCharacterIsMultiColor = (c & 0x800) != 0;
            var inputMultiColorToggle = multiColorToggle != 0;
            var expectedData = inputCharacterIsMultiColor ? (inputMultiColorToggle ? srData : srData << 2) : (srData << 1);
            var expectedMultiColorToggle = !inputCharacterIsMultiColor || !inputMultiColorToggle;

            // Act
            var result = Vic.TestClockedGraphics(false, true, inputMultiColorToggle, c, srData);

            // Assert
            Verify(result, expectedData, expectedMultiColorToggle);
        }

        [Test]
        public void MultiColorBitmapMode([Range(0x00, 0xC0, 0x40)] int srData, [Range(0, 1)] int multiColorToggle)
        {
            // Arrange
            var inputMultiColorToggle = multiColorToggle != 0;
            var expectedData = inputMultiColorToggle ? srData : srData << 2;
            var expectedMultiColorToggle = !inputMultiColorToggle;

            // Act
            var result = Vic.TestClockedGraphics(true, true, inputMultiColorToggle, 0x000, srData);

            // Assert
            Verify(result, expectedData, expectedMultiColorToggle);
        }
    }
}
