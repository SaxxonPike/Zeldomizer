using System.Collections.Generic;
using Zeldomizer.Engine;
using Zeldomizer.Engine.Overworld.Interfaces;
using Zeldomizer.Engine.Scenes.Interfaces;
using Zeldomizer.Engine.Shops.Interfaces;
using Zeldomizer.Engine.Text.Interfaces;
using Zeldomizer.Engine.Underworld.Interfaces;

namespace Zeldomizer
{
    public interface IZeldaCartridge
    {
        IList<string> CharacterText { get; }
        IEndingText EndingText { get; }
        IOverworld Overworld { get; }
        IUnderworld Underworld { get; }
        IMenuText MenuText { get; }
        IReadOnlyList<IShop> Shops { get; }
        IScene IntroScene { get; }
        IScene TitleScene { get; }
        HitPointTable HitPointTable { get; }
    }
}