using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Zeldomizer.Engine.Shops.Interfaces;
using Zeldomizer.Metal;

namespace Zeldomizer.Engine.Shops
{
    /// <summary>
    /// Represents a shop, in raw form.
    /// </summary>
    public class Shop : IShop
    {
        private readonly ISource _itemSource;
        private readonly ISource _priceSource;

        /// <summary>
        /// Initialize a shop.
        /// </summary>
        public Shop(ISource itemSource, ISource priceSource)
        {
            _itemSource = itemSource;
            _priceSource = priceSource;
        }

        /// <summary>
        /// Enumerate items in the shop.
        /// </summary>
        public IEnumerator<IShopItem> GetEnumerator()
        {
            return Enumerable.Range(0, 3)
                .Select(i => this[i])
                .GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        /// <summary>
        /// Number of possible items in the shop.
        /// </summary>
        public int Count => 3;

        /// <summary>
        /// Items in the shop.
        /// </summary>
        public IShopItem this[int index] => new ShopItem(
            new SourceBlock(_priceSource, index),
            new SourceBlock(_itemSource, index));

        /// <summary>
        /// Get or set the shop type.
        /// </summary>
        public int ShopType
        {
            get
            {
                var bit2 = _itemSource[0].Bit(6) ? 0x4 : 0x0;
                var bit1 = _itemSource[1].Bit(6) ? 0x2 : 0x0;
                var bit0 = _itemSource[2].Bit(6) ? 0x1 : 0x0;
                return bit2 | bit1 | bit0;
            }
            set
            {
                _itemSource[0] = _itemSource[0].Bit(6, (value & 0x4) != 0);
                _itemSource[1] = _itemSource[1].Bit(6, (value & 0x2) != 0);
                _itemSource[2] = _itemSource[2].Bit(6, (value & 0x1) != 0);
            }
        }

        /// <summary>
        /// If true, price numbers are shown in the shop.
        /// </summary>
        public bool ShowPrices
        {
            get => _itemSource[2].Bit(7);
            set => _itemSource[2] = _itemSource[2].Bit(7, value);
        }

        /// <summary>
        /// If true, the actual value is revealed when an item is touched.
        /// </summary>
        public bool RevealPrice
        {
            get => _itemSource[1].Bit(7);
            set => _itemSource[1] = _itemSource[1].Bit(7, value);
        }

        /// <summary>
        /// If true, display a minus sign in front of each price.
        /// </summary>
        public bool ShowMinusSign
        {
            get => _itemSource[0].Bit(7);
            set => _itemSource[0] = _itemSource[0].Bit(7, value);
        }

        /// <summary>
        /// Get a string representation of this shop.
        /// </summary>
        public override string ToString()
        {
            return $"Shop type {ShopType}. Showing prices: {ShowPrices}. Showing minus sign: {ShowMinusSign}. Revealed price: {RevealPrice}. ";
        }
    }
}
