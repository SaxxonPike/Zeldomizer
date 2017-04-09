using System.Collections.Generic;
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
    public class TestPlayground : ZeldomizerBaseTestFixture
    {
        [Test]
        [Explicit]
        public void Test1()
        {
            var sprites = new OverworldSpriteList(new SourceBlock(Source, 0x0C93B));
            var renderer = new SpriteRenderer();
            var palette = new NtscNesPalette();
            var columns = new OverworldColumnLibraryList(new SourceBlock(Source, 0x14000 - 0x8000), new WordList(new SourceBlock(Source, 0x19D0F), 16));
            var rooms = new OverworldRoomList(new SourceBlock(Source, 0x15418), 124).ToList();
            var tiles = new OverworldTileList(new SourceBlock(Source, 0x1697C)).ToList();
            var detailTiles = new OverworldDetailTileList(new SourceBlock(Source, 0x169B4));
            var grid = new OverworldGrid(new SourceBlock(Source, 0x18580));

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

            var roomBitmaps = new Dictionary<int, Bitmap>();

            // Render out rooms
            var roomIndex = 0;
            foreach (var room in decompiledRooms.Rooms.Select(r => r.ToArray()))
            {
                var roomBitmap = new Bitmap(256, 168);

                using (var g = Graphics.FromImage(roomBitmap))
                using (var backgroundBrush = new SolidBrush(renderer.Colors[2]))
                using (var mem = new MemoryStream())
                {
                    g.FillRectangle(backgroundBrush, 0, 0, 256, 168);
                    var tileIndex = 0;

                    for (var y = 0; y < 11; y++)
                    {
                        for (var x = 0; x < 16; x++)
                        {
                            var tile = room[tileIndex];
                            var plotX = x << 4;
                            var plotY = y << 4;
                            tileIndex++;

                            var tileIds = tile < 0x10
                                ? detailTiles[tile]
                                : tile >= 0x70 && tile <= 0xF1
                                    ? Enumerable.Range(tile, 4)
                                    : null;

                            if (tileIds != null)
                            {
                                var tileIdArray = tileIds
                                    .Select(s => s < 0x70 || s > 0xF1 ? 0x7A : s - 0x70)
                                    .ToArray();

                                g.DrawImage(spriteBitmaps[tileIdArray[0]], plotX, plotY);
                                g.DrawImage(spriteBitmaps[tileIdArray[2]], plotX + 8, plotY);
                                g.DrawImage(spriteBitmaps[tileIdArray[1]], plotX, plotY + 8);
                                g.DrawImage(spriteBitmaps[tileIdArray[3]], plotX + 8, plotY + 8);
                            }

                            if (tile >= 0x85 && tile < 0xE5)
                            {
                                g.DrawRectangle(Pens.Red, plotX, plotY, 16, 16);
                            }
                        }
                    }

                    roomBitmap.Save(mem, ImageFormat.Png);
                    WriteToDesktopPath(room.ToByteArray(), "overworld", $"{roomIndex:X2}.bin");
                    WriteToDesktopPath(mem.ToArray(), "overworld", $"{roomIndex:X2}.png");
                }

                roomBitmaps[roomIndex] = roomBitmap;
                roomIndex++;
            }

            using (var outputMap = new Bitmap(256 * 16, 168 * 8))
            using (var g = Graphics.FromImage(outputMap))
            using (var mem = new MemoryStream())
            {
                var x = 0;
                var y = 0;
                foreach (var i in grid)
                {
                    while (x >= outputMap.Width)
                    {
                        x -= outputMap.Width;
                        y += 168;
                    }

                    g.DrawImage(roomBitmaps[i.Bits(6, 0)], x, y);
                    x += 256;
                }

                outputMap.Save(mem, ImageFormat.Png);
                WriteToDesktop(mem.ToArray(), "test.png");
            }

            foreach (var bitmap in roomBitmaps)
                bitmap.Value?.Dispose();
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
