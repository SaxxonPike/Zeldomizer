using System.Collections.Generic;

namespace Zeldomizer.Engine.Shops.Interfaces
{
    public interface IShop
    {
        IList<int> Items { get; }
        IList<int> Prices { get; }
    }
}