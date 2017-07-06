using Zeldomizer.Metal;

namespace Zeldomizer.Engine.Graphics
{
    public class Fade
    {
        public Fade(ISource paletteSource)
        {
            Palette2 = new FadePaletteList(paletteSource, 4);
            Palette3 = new FadePaletteList(new SourceBlock(paletteSource, 4), 4);
        }

        public FadePaletteList Palette2 { get; }
        public FadePaletteList Palette3 { get; }
    }
}
