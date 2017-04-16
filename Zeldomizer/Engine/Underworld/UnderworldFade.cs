using Zeldomizer.Metal;

namespace Zeldomizer.Engine.Underworld
{
    public class UnderworldFade
    {
        public UnderworldFade(ISource paletteSource)
        {
            Palette2 = new UnderworldFadePaletteList(paletteSource, 4);
            Palette3 = new UnderworldFadePaletteList(new SourceBlock(paletteSource, 4), 4);
        }

        public UnderworldFadePaletteList Palette2 { get; }
        public UnderworldFadePaletteList Palette3 { get; }
    }
}
