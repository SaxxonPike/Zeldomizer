using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Zeldomizer.Engine.Shops.Interfaces;
using Zeldomizer.Metal;

namespace Zeldomizer.Engine.Shops
{
    /// <summary>
    /// Represents a list of shops, in raw form.
    /// </summary>
    public class ShopList : IReadOnlyList<IShop>
    {
        private readonly ISource _shopSource;
        private readonly ISource _messageSource;
        private readonly ISource _vendorSource;

        /// <summary>
        /// Initialize a list of shops.
        /// </summary>
        public ShopList(
            ISource shopSource, 
            ISource messageSource, 
            ISource vendorSource,
            int count)
        {
            Count = count;
            _shopSource = shopSource;
            _messageSource = messageSource;
            _vendorSource = vendorSource;
        }

        /// <summary>
        /// Enumerate all shops.
        /// </summary>
        public IEnumerator<IShop> GetEnumerator()
        {
            return Enumerable
                .Range(0, Count)
                .Select(i => this[i])
                .GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        /// <summary>
        /// Get the shop at the specified index.
        /// </summary>
        public IShop this[int index] => new Shop(
            new SourceBlock(_shopSource, index * 3), 
            new SourceBlock(_shopSource, (index + Count) * 3),
            new SourceBlock(_messageSource, index),
            new SourceBlock(_vendorSource, index));

        /// <summary>
        /// Number of shops.
        /// </summary>
        public int Count { get; }
    }
}
