using System;
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
        /// <summary>
        /// Initialize a shop.
        /// </summary>
        public Shop(ISource itemSource, ISource priceSource)
        {
            Items = new ByteList(itemSource, 3);
            Prices = new ByteList(priceSource, 3);
        }

        /// <summary>
        /// List of items available at the shop.
        /// </summary>
        public IList<int> Items { get; }

        /// <summary>
        /// List of prices for the items.
        /// </summary>
        public IList<int> Prices { get; }

        /// <summary>
        /// Get a string representation of this shop.
        /// </summary>
        public override string ToString()
        {
            return string.Join(", ", Enumerable.Range(0, 3).Select(i => $"Item:{Items[i]:X2} Price:{Prices[i]}"));
        }
    }
}
