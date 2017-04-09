using System.Drawing;
using Zeldomizer.Engine.Graphics.Interfaces;
using Zeldomizer.Metal;

namespace Zeldomizer.Engine.Graphics
{
    public class SpriteRenderer
    {
        public SpriteRenderer()
        {
            Colors = new Color[4];
        }

        public Color[] Colors { get; }

        public Bitmap Render(ISprite sprite)
        {
            var output = new DirectBitmap(sprite.Width, sprite.Height);
            var count = sprite.Width * sprite.Height;

            // Copy bitmap using the palette
            for (var i = 0; i < count; i++)
                output[i] = Colors[sprite[i].Bits(1, 0)];

            return output.Bitmap;
        }

    }
}
