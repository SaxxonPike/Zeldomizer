using System.Collections.Generic;

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
        bool VendorRemains { get; set; }
        bool CanObtainItems { get; set; }
        int MessageId { get; set; }
        int VendorId { get; set; }
    }
}
