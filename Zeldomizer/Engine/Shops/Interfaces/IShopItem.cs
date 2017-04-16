namespace Zeldomizer.Engine.Shops.Interfaces
{
    public interface IShopItem
    {
        int Item { get; set; }
        bool ItemBit6 { get; set; }
        bool ItemBit7 { get; set; }
        int Price { get; set; }
    }
}