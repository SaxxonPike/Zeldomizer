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
        private readonly ISource _source;

        /// <summary>
        /// Initialize a list of shops.
        /// </summary>
        public ShopList(ISource source, int count)
        {
            Count = count;
            _source = source;
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
        public IShop this[int index] => 
            new Shop(new SourceBlock(_source, index * 3), new SourceBlock(_source, (index + Count) * 3));

        /// <summary>
        /// Number of shops.
        /// </summary>
        public int Count { get; }
    }
}
