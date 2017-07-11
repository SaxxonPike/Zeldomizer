using Zeldomizer.Engine.Shops.Interfaces;
using Zeldomizer.Metal;

namespace Zeldomizer.Engine.Shops
{
    /// <summary>
    /// Represents a shop item, in raw form.
    /// </summary>
    public class ShopItem : IShopItem
    {
        private readonly ISource _priceSource;
        private readonly ISource _itemSource;

        /// <summary>
        /// Initialize a shop item.
        /// </summary>
        public ShopItem(ISource priceSource, ISource itemSource)
        {
            _priceSource = priceSource;
            _itemSource = itemSource;
        }

        /// <summary>
        /// Get or set the price of the item.
        /// </summary>
        public int Price
        {
            get => _priceSource[0];
            set => _priceSource[0] = unchecked((byte) value);
        }

        /// <summary>
        /// Get or set the item ID.
        /// </summary>
        public int Item
        {
            get => _itemSource[0].Bits(5, 0);
            set => _itemSource[0] = _itemSource[0].Bits(5, 0, value);
        }

        /// <summary>
        /// Get the string representation of the shop item.
        /// </summary>
        public override string ToString()
        {
            return $"Item {Item:X2} for ${Price}";
        }
    }
}
