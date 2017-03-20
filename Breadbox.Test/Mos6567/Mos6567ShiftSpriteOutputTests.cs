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
    public class Mos6567ShiftSpriteOutputTests : Mos6567BaseTestFixture
    {
        private static void Verify(SpriteShiftOutput result, int expectedData,
            bool expectedMultiColorToggle, bool expectedXExpansionToggle, bool expectedShiftRegisterEnable)
        {
            var observedShiftRegisterEnable = result.ShiftRegisterEnable;
            var observedMultiColorToggle = result.MultiColorToggle;
            var observedXExpansionToggle = result.XExpansionToggle;
            var observedData = result.ShiftRegisterData;

            observedData.Should().Be(expectedData, "shift register should be correct");
            observedShiftRegisterEnable.Should().Be(expectedShiftRegisterEnable, "shift register enable should be correct");
            observedMultiColorToggle.Should().Be(expectedMultiColorToggle, "multicolor toggle should be correct");
            observedXExpansionToggle.Should().Be(expectedXExpansionToggle, "x expansion toggle should be correct");
        }

        [Test]
        public void SingleColorSprite([Range(0x000000, 0xC00000, 0x400000)] int srData)
        {
            // Arrange
            var expectedData = srData << 1;
            const bool expectedMultiColorToggle = true;
            const bool expectedXExpansionToggle = true;
            const bool expectedShiftRegisterEnable = true;

            // Act
            var result = Vic.TestClockedSprite(0, 0, true, true, srData, false, false, false, false);

            // Assert
            Verify(result, expectedData, expectedMultiColorToggle, expectedXExpansionToggle, expectedShiftRegisterEnable);
        }

        [Test]
        public void ExpandedSingleColorSprite([Range(0x000000, 0xC00000, 0x400000)] int srData, [Range(0, 1)] int xExpandToggle)
        {
            // Arrange
            var inputXExpandToggle = xExpandToggle != 0;
            var expectedData = inputXExpandToggle ? srData : srData << 1;
            const bool expectedMultiColorToggle = true;
            var expectedXExpansionToggle = !inputXExpandToggle;
            const bool expectedShiftRegisterEnable = true;

            // Act
            var result = Vic.TestClockedSprite(0, 0, true, true, srData, false, false, true, inputXExpandToggle);

            // Assert
            Verify(result, expectedData, expectedMultiColorToggle, expectedXExpansionToggle, expectedShiftRegisterEnable);
        }

        [Test]
        public void MultiColorSprite([Range(0x000000, 0xC00000, 0x400000)] int srData, [Range(0, 1)] int multiColorToggle)
        {
            // Arrange
            var inputMultiColorToggle = multiColorToggle != 0;
            var expectedData = inputMultiColorToggle ? srData : srData << 2;
            var expectedMultiColorToggle = !inputMultiColorToggle;
            const bool expectedXExpansionToggle = true;
            const bool expectedShiftRegisterEnable = true;

            // Act
            var result = Vic.TestClockedSprite(0, 0, true, true, srData, true, inputMultiColorToggle, false, false);

            // Assert
            Verify(result, expectedData, expectedMultiColorToggle, expectedXExpansionToggle, expectedShiftRegisterEnable);
        }

        [Test]
        public void ExpandedMultiColorSprite([Range(0x000000, 0xC00000, 0x400000)] int srData, [Range(0, 1)] int multiColorToggle, [Range(0, 1)] int xExpandToggle)
        {
            // Arrange
            var inputMultiColorToggle = multiColorToggle != 0;
            var inputXExpandToggle = xExpandToggle != 0;
            var expectedData = inputXExpandToggle || inputMultiColorToggle ? srData : srData << 2;
            var expectedMultiColorToggle = !inputMultiColorToggle;
            var expectedXExpansionToggle = inputMultiColorToggle ? inputXExpandToggle : !inputXExpandToggle;
            const bool expectedShiftRegisterEnable = true;

            // Act
            var result = Vic.TestClockedSprite(0, 0, true, true, srData, true, inputMultiColorToggle, true, inputXExpandToggle);

            // Assert
            Verify(result, expectedData, expectedMultiColorToggle, expectedXExpansionToggle, expectedShiftRegisterEnable);
        }
    }
}
