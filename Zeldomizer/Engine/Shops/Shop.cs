﻿using System.Collections;
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
        private readonly ISource _messageSource;
        private readonly ISource _vendorSource;

        /// <summary>
        /// Initialize a shop.
        /// </summary>
        public Shop(
            ISource itemSource,
            ISource priceSource,
            ISource messageSource,
            ISource vendorSource)
        {
            _itemSource = itemSource;
            _priceSource = priceSource;
            _messageSource = messageSource;
            _vendorSource = vendorSource;
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
        /// If true, enough hearts must be obtained in order to get items.
        /// </summary>
        public bool RequireHearts
        {
            get => _itemSource[2].Bit(6);
            set => _itemSource[2] = _itemSource[2].Bit(6, value);
        }

        /// <summary>
        /// If true, this shop is Pay to Talk.
        /// </summary>
        public bool PayToTalk
        {
            get => _itemSource[1].Bit(6);
            set => _itemSource[1] = _itemSource[1].Bit(6, value);
        }

        /// <summary>
        /// If true, items are shown.
        /// </summary>
        public bool ShowItems
        {
            get => _itemSource[0].Bit(6);
            set => _itemSource[0] = _itemSource[0].Bit(6, value);
        }

        /// <summary>
        /// If true, the vendor will not disappear.
        /// </summary>
        public bool VendorRemains
        {
            get => _messageSource[0].Bit(7);
            set => _messageSource[0] = _messageSource[0].Bit(7, value);
        }

        /// <summary>
        /// If true, items can be obtained from the shop.
        /// </summary>
        public bool CanObtainItems
        {
            get => _messageSource[0].Bit(6);
            set => _messageSource[0] = _messageSource[0].Bit(6, value);
        }

        /// <summary>
        /// Get or set the message ID to show.
        /// </summary>
        public int MessageId
        {
            get => _messageSource[0].Bits(5, 1);
            set => _messageSource[0] = _messageSource[0].Bits(5, 1, value);
        }

        /// <summary>
        /// Get or set the vendor ID.
        /// </summary>
        public int VendorId
        {
            get => _vendorSource[0];
            set => _vendorSource[0] = unchecked((byte) value);
        }
    }
}
