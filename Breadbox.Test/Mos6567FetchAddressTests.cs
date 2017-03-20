using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Breadbox.Test.Mos6567;
using FluentAssertions;
using NUnit.Framework;

namespace Breadbox.Test
{
    [TestFixture]
    public class Mos6567FetchAddressTests : Mos6567BaseTestFixture
    {
        [Test]
        public void Idle()
        {
            Vic.TestFetchIAddress().Should().Be(0x3FFF);
        }

        [Test]
        public void GraphicsText([Range(0, 1)] int idle, [Range(0, 1)] int ecm, [Random(0x00, 0xFF, 1)] int vc, [Range(0x0000, 0x3800, 0x0800)] int cb, [Range(0x00, 0xFF, 0x55)] int c, [Range(0x0, 0x7, 0x7)] int rc)
        {
            // Arrange
            var expectedAddress = (idle != 0 ? 0x3FFF : (cb | ((c & 0xFF) << 3) | rc)) & (ecm != 0 ? 0x39FF : 0x3FFF);

            // Act
            var observedAddress = Vic.TestFetchGAddress(idle != 0, ecm != 0, false, vc, cb, c, rc);

            // Assert
            observedAddress.Should().Be(expectedAddress);
        }

        [Test]
        public void GraphicsBitmap([Range(0, 1)] int idle, [Range(0, 1)] int ecm, [Range(0x00, 0x3FF, 0x55)] int vc, [Range(0x0000, 0x2000, 0x2000)] int cb, [Random(0x00, 0xFF, 1)] int c, [Range(0x0, 0x7, 0x7)] int rc)
        {
            // Arrange
            var expectedAddress = (idle != 0 ? 0x3FFF : (cb | (vc << 3) | rc)) & (ecm != 0 ? 0x39FF : 0x3FFF);

            // Act
            var observedAddress = Vic.TestFetchGAddress(idle != 0, ecm != 0, true, vc, cb, c, rc);

            // Assert
            observedAddress.Should().Be(expectedAddress);
        }

        [Test]
        public void Character([Range(0x0000,0x3C00,0x0400)] int vm, [Range(0x00, 0x3FF, 0x55)] int vc)
        {
            // Arrange
            var expectedAddress = vm | vc;

            // Act
            var observedAddress = Vic.TestFetchCAddress(vm, vc);

            // Assert
            observedAddress.Should().Be(expectedAddress);
        }

        [Test]
        public void Refresh([Range(0x00, 0xFF, 0x55)] int refreshCounter)
        {
            // Arrange
            var expectedAddress = 0x3F00 | refreshCounter;

            // Act
            var observedAddress = Vic.TestFetchRAddress(refreshCounter);

            // Assert
            observedAddress.Should().Be(expectedAddress);
        }

        [Test]
        public void SpritePointer([Range(0, 7)] int index, [Range(0x0000, 0x3C00, 0x0400)] int vm)
        {
            // Arrange
            var expectedAddress = vm | 0x3F8 | index;
            
            // Act
            var observedAddress = Vic.TestFetchPAddress(vm, index);

            // Assert
            observedAddress.Should().Be(expectedAddress);
        }

        [Test]
        public void SpriteData([Range(0x00, 0xFF, 0x55)] int mp, [Range(0x00, 0x3F, 0x05)] int mc)
        {
            // Arrange
            var expectedAddress = (mp << 6) | mc;

            // Act
            var observedAddress = Vic.TestFetchSAddress(mp << 6, mc);

            // Assert
            observedAddress.Should().Be(expectedAddress);
        }

    }
}
