using FluentAssertions;
using NUnit.Framework;

namespace Zeldomizer.Engine.Shops
{
    public class ShopListTests : ZeldomizerBaseTestFixture<ShopList>
    {
        protected override ShopList GetTestSubject()
        {
            var cart = new ZeldaCartridge(Source);
            return cart.Shops as ShopList;
        }

        [Test]
        public void Items_CanGetValue()
        {
            Subject[0x0D].Items[0].Should().Be(0x1C);
            Subject[0x0D].Items[1].Should().Be(0x00);
            Subject[0x0D].Items[2].Should().Be(0xC8);
        }

        [Test]
        public void Items_CanSetValue()
        {
            Subject[0x0D].Items[0].Should().Be(0x1C);
            Subject[0x0D].Items[0] = 0x12;
            Subject[0x0D].Items[0].Should().Be(0x12);
            Subject[0x0D].Items[1].Should().Be(0x00);
            Subject[0x0D].Items[1] = 0x34;
            Subject[0x0D].Items[1].Should().Be(0x34);
            Subject[0x0D].Items[2].Should().Be(0xC8);
            Subject[0x0D].Items[2] = 0x56;
            Subject[0x0D].Items[2].Should().Be(0x56);
        }

        [Test]
        public void Prices_CanGetValue()
        {
            Subject[0x0D].Prices[0].Should().Be(130);
            Subject[0x0D].Prices[1].Should().Be(20);
            Subject[0x0D].Prices[2].Should().Be(80);
        }

        [Test]
        public void Prices_CanSetValue()
        {
            Subject[0x0D].Prices[0].Should().Be(130);
            Subject[0x0D].Prices[0] = 0x13;
            Subject[0x0D].Prices[0].Should().Be(0x13);
            Subject[0x0D].Prices[1].Should().Be(20);
            Subject[0x0D].Prices[1] = 0x24;
            Subject[0x0D].Prices[1].Should().Be(0x24);
            Subject[0x0D].Prices[2].Should().Be(80);
            Subject[0x0D].Prices[2] = 0x35;
            Subject[0x0D].Prices[2].Should().Be(0x35);
        }
    }
}
