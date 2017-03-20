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
    public class Mos6567RawGraphicsOutputTests : Mos6567BaseTestFixture
    {
        [Test]
        public void StandardTextMode([Range(0x0, 0xF, 5)] int backgroundColor0, [Range(0x0, 0xF, 5)] int graphicsColor, [Range(0x00, 0xC0, 0x40)] int graphicsShiftRegister)
        {
            // Arrange
            var expectedColor = (graphicsShiftRegister & 0x80) != 0 ? graphicsColor : backgroundColor0;
            var expectedForeground = (graphicsShiftRegister & 0x80) != 0;
            var cFetch = graphicsColor << 8;

            // Act
            var output = Vic.TestRawGraphicsOutputStandardTextMode(backgroundColor0, cFetch, graphicsShiftRegister);

            // Assert
            output.Color.Should().Be(expectedColor, "color should be correct");
            output.Foreground.Should().Be(expectedForeground, "foreground should be correct");
        }

        [Test]
        public void MulticolorTextMode([Range(0x0, 0xF, 5)] int backgroundColor0, [Range(0x0, 0xF, 5)] int backgroundColor1, [Range(0x0, 0xF, 5)] int backgroundColor2, [Range(0x0, 0xF, 5)] int graphicsColor, [Range(0x00, 0xC0, 0x40)] int graphicsShiftRegister)
        {
            // Arrange
            var multicolor = (graphicsColor & 0x8) != 0;
            var expectedColor = -1;
            var cFetch = graphicsColor << 8;
            if (multicolor)
            {
                switch (graphicsShiftRegister & 0xC0)
                {
                    case 0x00: expectedColor = backgroundColor0; break;
                    case 0x40: expectedColor = backgroundColor1; break;
                    case 0x80: expectedColor = backgroundColor2; break;
                    case 0xC0: expectedColor = graphicsColor & 0x7; break;
                }
            }
            else
            {
                expectedColor = (graphicsShiftRegister & 0x80) != 0 ? graphicsColor : backgroundColor0;
            }

            var expectedForeground = (graphicsShiftRegister & 0x80) != 0;

            // Act
            var output = Vic.TestRawGraphicsOutputMulticolorTextMode(backgroundColor0, backgroundColor1, backgroundColor2, cFetch, graphicsShiftRegister);

            // Assert
            output.Color.Should().Be(expectedColor, "color should be correct");
            output.Foreground.Should().Be(expectedForeground, "foreground should be correct");
        }

        [Test]
        public void StandardBitmapMode([Range(0x0, 0xF, 5)] int backgroundColor, [Range(0x0, 0xF, 5)] int foregroundColor, [Range(0x00, 0xC0, 0x40)] int graphicsShiftRegister)
        {
            // Arrange
            var expectedColor = (graphicsShiftRegister & 0x80) != 0 ? foregroundColor : backgroundColor;
            var expectedForeground = (graphicsShiftRegister & 0x80) != 0;
            var cFetch = (foregroundColor << 4) | backgroundColor;

            // Act
            var output = Vic.TestRawGraphicsOutputStandardBitmapMode(cFetch, graphicsShiftRegister);

            // Assert
            output.Color.Should().Be(expectedColor, "color should be correct");
            output.Foreground.Should().Be(expectedForeground, "foreground should be correct");
        }

        [Test]
        public void MulticolorBitmapMode([Range(0x0, 0xF, 5)] int backgroundColor, [Range(0x0, 0xF, 5)] int color1, [Range(0x0, 0xF, 5)] int color2, [Range(0x0, 0xF, 5)] int color3, [Range(0x00, 0xC0, 0x40)] int graphicsShiftRegister)
        {
            // Arrange
            var expectedColor = -1;
            switch (graphicsShiftRegister & 0xC0)
            {
                case 0x00: expectedColor = backgroundColor; break;
                case 0x40: expectedColor = color1; break;
                case 0x80: expectedColor = color2; break;
                case 0xC0: expectedColor = color3; break;
            }
            var expectedForeground = (graphicsShiftRegister & 0x80) != 0;
            var cFetch = (color3 << 8) | (color1 << 4) | color2;

            // Act
            var output = Vic.TestRawGraphicsOutputMulticolorBitmapMode(backgroundColor, cFetch, graphicsShiftRegister);

            // Assert
            output.Color.Should().Be(expectedColor, "color should be correct");
            output.Foreground.Should().Be(expectedForeground, "foreground should be correct");
        }

        [Test]
        public void ExtraColorMode([Range(0x0, 0xF, 5)] int backgroundColor0, [Range(0x0, 0xF, 5)] int backgroundColor1, [Range(0x0, 0xF, 5)] int backgroundColor2, [Range(0x0, 0xF, 5)] int backgroundColor3, [Range(0x0, 0xF, 5)] int foregroundColor, [Range(0x00, 0x80, 0x80)] int graphicsShiftRegister, [Range(0x00, 0xC0, 0x40)] int cFetchBase)
        {
            // Arrange
            var expectedColor = -1;
            var cFetch = cFetchBase | (foregroundColor << 8);
            if ((graphicsShiftRegister & 0x80) == 0)
            {
                switch (cFetchBase)
                {
                    case 0x00: expectedColor = backgroundColor0; break;
                    case 0x40: expectedColor = backgroundColor1; break;
                    case 0x80: expectedColor = backgroundColor2; break;
                    case 0xC0: expectedColor = backgroundColor3; break;
                }
            }
            else
            {
                expectedColor = foregroundColor;
            }
            var expectedForeground = (graphicsShiftRegister & 0x80) != 0;

            // Act
            var output = Vic.TestRawGraphicsOutputExtraColorMode(backgroundColor0, backgroundColor1, backgroundColor2, backgroundColor3, cFetch, graphicsShiftRegister);

            // Assert
            output.Color.Should().Be(expectedColor, "color should be correct");
            output.Foreground.Should().Be(expectedForeground, "foreground should be correct");
        }



    }
}
