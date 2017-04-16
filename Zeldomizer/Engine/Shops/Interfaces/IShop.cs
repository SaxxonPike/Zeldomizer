using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zeldomizer.Engine.Shops.Interfaces
{
    public interface IShop : IReadOnlyList<IShopItem>
    {
        bool ShowPrices { get; set; }
        bool RevealPrice { get; set; }
        bool ShowMinusSign { get; set; }
        bool RequireHearts { get; set; }
        bool PayToTalk { get; set; }
        bool ShowItems { get; set; }
    }
}
