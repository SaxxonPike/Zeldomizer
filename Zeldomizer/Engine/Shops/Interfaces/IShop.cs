using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zeldomizer.Engine.Shops.Interfaces
{
    public interface IShop : IReadOnlyList<IShopItem>
    {
        int ShopType { get; set; }
        bool ShowPrices { get; set; }
        bool RevealPrice { get; set; }
        bool ShowMinusSign { get; set; }
    }
}
