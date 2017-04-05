using System.Diagnostics.CodeAnalysis;
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
    [ExcludeFromCodeCoverage]
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
            var tiles = new OverworldTileList(new SourceBlock(Source, 0x1697C)).ToList();
            var detailTiles = new OverworldDetailTileList(new SourceBlock(Source, 0x169B4));

            var decompiler = new OverworldDecompiler();
            var decompiledRooms = decompiler.Decompile(columns, rooms, tiles);

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
            foreach (var room in decompiledRooms.Rooms.Select(r => r.ToArray()))
            {
                using (var roomBitmap = new Bitmap(256, 168))
                using (var g = Graphics.FromImage(roomBitmap))
                using (var mem = new MemoryStream())
                {
                    var tileIndex = 0;

                    for (var y = 0; y < 11; y++)
                    {
                        for (var x = 0; x < 16; x++)
                        {
                            var tile = room[tileIndex];
                            var plotX = x << 4;
                            var plotY = y << 4;
                            if (tile < 0x70 || tile > 0xF1)
                                tile = 0;
                            else
                                tile -= 0x70;
                            g.DrawImage(spriteBitmaps[tile], plotX, plotY);
                            g.DrawImage(spriteBitmaps[tile + 1], plotX + 8, plotY);
                            g.DrawImage(spriteBitmaps[tile + 2], plotX, plotY + 8);
                            g.DrawImage(spriteBitmaps[tile + 3], plotX + 8, plotY + 8);
                            tileIndex++;
                        }
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

        [Test]
        [Explicit]
        public void Test3()
        {
            //var system = new NesSystem();
            //var cartridge = new Mmc1CartridgeDevice("Cart", Source.ExportRaw());

            //cartridge.PrgBank = 5;
            //cartridge.PrgMode = 3;
            //system.Router.Install(cartridge);

            //system.CpuPc = 0xAA59;
            //system.Router.CpuPoke(0x0004, 0xD8); // low data
            //system.Router.CpuPoke(0x0005, 0x9B); // high data
            //system.Router.CpuPoke(0x7276, 0x60); // RTS
            //system.Router.CpuPoke(0x728C, 0x60); // RTS

            //for (var i = 0; i < 100; i++)
            //    system.Clock();

        }
    }
}
