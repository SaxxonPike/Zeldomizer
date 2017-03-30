using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Zeldomizer.Engine.Graphics;
using Zeldomizer.Engine.Overworld;
using Zeldomizer.Metal;

namespace Zeldomizer
{
    public class TestPlayground : BaseTestFixture
    {
        [Test]
        [Explicit]
        public void Test1()
        {
            var sprites = new OverworldSpriteList(new SourceBlock(Source, 0x0C93B)).ToList();
            var renderer = new SpriteRenderer();
            var palette = new NtscNesPalette();
            var columns = new OverworldColumnList(new SourceBlock(Source, 0x15BD8)).ToList();
            var rooms = new OverworldRoomList(new SourceBlock(Source, 0x15418), 124).ToList();

            // Grayscale
            renderer.Colors[0] = palette[0x0F];
            renderer.Colors[1] = palette[0x00];
            renderer.Colors[2] = palette[0x10];
            renderer.Colors[3] = palette[0x20];

            // Render out sprites
            var spriteBitmaps = sprites
                .Select(sprite => renderer.Render(sprite))
                .ToArray();

            // Render out rooms
            var roomIndex = 0;
            foreach (var room in rooms)
            {
                using (var roomBitmap = new Bitmap(256, 168))
                using (var g = Graphics.FromImage(roomBitmap))
                using (var mem = new MemoryStream())
                {
                    var x = 0;

                    foreach (var column in room)
                    {
                        var y = 0;

                        foreach (var tile in columns[column])
                        {
                            g.DrawImage(spriteBitmaps[tile], x, y);
                            g.DrawImage(spriteBitmaps[tile + 1], x + 8, y);
                            g.DrawImage(spriteBitmaps[tile + 2], x, y + 8);
                            g.DrawImage(spriteBitmaps[tile + 3], x + 8, y + 8);
                            y += 16;
                        }

                        x += 16;
                    }

                    roomBitmap.Save(mem, ImageFormat.Png);
                    WriteToDesktopPath(mem.ToArray(), "overworld", $"{roomIndex:X2}.png");
                }

                roomIndex++;
            }
        }

        [Test]
        [Explicit]
        public void WriteSprites()
        {
            var sprites = new OverworldSpriteList(new SourceBlock(Source, 0x0C93B)).ToList();
            var renderer = new SpriteRenderer();
            var palette = new NtscNesPalette();
            var index = 0x00;

            // Grayscale
            renderer.Colors[0] = palette[0x0F];
            renderer.Colors[1] = palette[0x00];
            renderer.Colors[2] = palette[0x10];
            renderer.Colors[3] = palette[0x20];

            // Render out sprites
            foreach (var sprite in sprites)
            {
                using (var bitmap = renderer.Render(sprite))
                using (var mem = new MemoryStream())
                {
                    bitmap.Save(mem, ImageFormat.Png);
                    WriteToDesktopPath(mem.ToArray(), "overworld", $"{index:X2}.png");
                }

                index++;
            }

        }
    }
}
