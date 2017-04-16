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
    public class Shop : IReadOnlyList<IShopItem>
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
    }
}
