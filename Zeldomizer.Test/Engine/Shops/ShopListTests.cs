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
            var shopItems = Subject[0x0D];
            shopItems[0].Item.Should().Be(0x1C);
            shopItems[1].Item.Should().Be(0x00);
            shopItems[2].Item.Should().Be(0x08);
        }

        [Test]
        public void Items_CanSetValue()
        {
            var shopItems = Subject[0x0D];
            shopItems[0].Item.Should().Be(0x1C);
            shopItems[0].Item = 0x12;
            shopItems[0].Item.Should().Be(0x12);
            shopItems[1].Item.Should().Be(0x00);
            shopItems[1].Item = 0x34;
            shopItems[1].Item.Should().Be(0x34);
            shopItems[2].Item.Should().Be(0x08);
            shopItems[2].Item = 0x56;
            shopItems[2].Item.Should().Be(0x16);
        }

        [Test]
        public void Prices_CanGetValue()
        {
            var shopItems = Subject[0x0D];
            shopItems[0].Price.Should().Be(130);
            shopItems[1].Price.Should().Be(20);
            shopItems[2].Price.Should().Be(80);
        }

        [Test]
        public void Prices_CanSetValue()
        {
            var shopItems = Subject[0x0D];
            shopItems[0].Price.Should().Be(130);
            shopItems[0].Price = 0x13;
            shopItems[0].Price.Should().Be(0x13);
            shopItems[1].Price.Should().Be(20);
            shopItems[1].Price = 0x24;
            shopItems[1].Price.Should().Be(0x24);
            shopItems[2].Price.Should().Be(80);
            shopItems[2].Price = 0x35;
            shopItems[2].Price.Should().Be(0x35);
        }
    }
}
