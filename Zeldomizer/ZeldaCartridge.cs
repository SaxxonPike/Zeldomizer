using System;
using System.Collections.Generic;
using Zeldomizer.Engine.Graphics;
using Zeldomizer.Engine.Overworld;
using Zeldomizer.Engine.Overworld.Interfaces;
using Zeldomizer.Engine.Shops;
using Zeldomizer.Engine.Shops.Interfaces;
using Zeldomizer.Engine.Text;
using Zeldomizer.Engine.Text.Interfaces;
using Zeldomizer.Engine.Underworld;
using Zeldomizer.Engine.Underworld.Interfaces;
using Zeldomizer.Metal;

namespace Zeldomizer
{
    /// <summary>
    /// A data adapter for the Legend of Zelda NES cartridge ROM.
    /// </summary>
    /// <remarks>
    /// You *really* don't want to change any of the hex offsets in here, trust me.
    /// </remarks>
    public class ZeldaCartridge
    {
        private readonly ISource _source;

        /// <summary>
        /// Create a data adapter.
        /// </summary>
        public ZeldaCartridge(ISource source)
        {
            _source = source;
            var conversionTable = new Lazy<ITextConversionTable>(() => new TextConversionTable());
            var speechConverter = new Lazy<IStringConverter>(() => new SpeechStringConverter(conversionTable.Value));
            var textConverter = new Lazy<IStringConverter>(() => new TextStringConverter(conversionTable.Value));
            var fixedStringConverter = new Lazy<IFixedStringConverter>(() => new FixedStringConverter(conversionTable.Value));
            var speechFormatter = new Lazy<IStringFormatter>(() => new StringFormatter());

            // Character Text
            _characterText = new Lazy<IList<string>>(() => new CharacterText(
                new WordPointerTable(new SourceBlock(source, 0x4000), new SourceBlock(source, -0x4000), 0x26), 
                speechFormatter.Value, 
                speechConverter.Value, 
                0x556, 
                0x26));

            // Ending Text
            _endingText = new Lazy<IEndingText>(() => new EndingText(
                new StringData(new SourceBlock(source, 0xA959), speechConverter.Value, 38),
                new FixedStringData(new SourceBlock(source, 0xAB07), fixedStringConverter.Value, 8),
                new FixedStringData(new SourceBlock(source, 0xAB0F), fixedStringConverter.Value, 24),
                new FixedStringData(new SourceBlock(source, 0xAB27), fixedStringConverter.Value, 20)));

            // Menu Text
            _menuText = new Lazy<IMenuText>(() => new MenuText(
                new FixedStringData(new SourceBlock(source, 0x09D48), fixedStringConverter.Value, 17),
                new FixedStringData(new SourceBlock(source, 0x09D5E), fixedStringConverter.Value, 18),
                new FixedStringData(new SourceBlock(source, 0x09D70), fixedStringConverter.Value, 8),
                new FixedStringData(new SourceBlock(source, 0x09EEB), fixedStringConverter.Value, 5)));

            // Underworld
            _underworld = new Lazy<IUnderworld>(() =>
            {
                var underworldColumnPointers = new WordPointerTable(
                    new SourceBlock(source, 0x16704),
                    new SourceBlock(source, 0xC000), 10);
                var columnLibraries = new UnderworldColumnLibraryList(
                    underworldColumnPointers);
                var grids = new UnderworldGridList(
                    new SourceBlock(source, 0x18700),
                    4);
                var roomLayouts = new UnderworldRoomLayoutList(
                    new SourceBlock(source, 0x160DE),
                    42);
                var levels = new UnderworldLevelList(
                    new SourceBlock(source, 0x193FF),
                    9);

                return new Underworld
                {
                    ColumnLibraries = columnLibraries,
                    Grids = grids,
                    Levels = levels,
                    RoomLayouts = roomLayouts
                };
            });

            // Overworld
            _overworld = new Lazy<IOverworld>(() =>
            {
                var overworldColumnPointers = new WordPointerTable(
                    new SourceBlock(source, 0x19D0F),
                    new SourceBlock(source, 0x0C000),
                    16);
                var columnLibraries = new OverworldColumnLibraryList(
                    overworldColumnPointers);
                var grid = new OverworldGrid(
                    new SourceBlock(source, 0x18580));
                var roomLayouts = new OverworldRoomLayoutList(
                    new SourceBlock(source, 0x15418),
                    124);
                var tiles = new OverworldTileList(
                    new SourceBlock(source, 0x1697C));
                var detailTiles = new OverworldDetailTileList(
                    new SourceBlock(source, 0x169B4));
                var sprites = new OverworldSpriteList(
                    new SourceBlock(source, 0x0C93B));

                return new Overworld
                {
                    ColumnLibraries = columnLibraries,
                    Grid = grid,
                    DetailTiles = detailTiles,
                    RoomLayouts = roomLayouts,
                    Tiles = tiles,
                    Sprites = sprites
                };
            });

            // Shops
            _shops = new Lazy<IReadOnlyList<IShop>>(() => new ShopList(
                new SourceBlock(source, 0x18600),
                new SourceBlock(source, 0x045A2),
                new SourceBlock(source, 0x06E6F), 
                20));
        }

        private readonly Lazy<IList<string>> _characterText;
        private readonly Lazy<IEndingText> _endingText;
        private readonly Lazy<IOverworld> _overworld;
        private readonly Lazy<IUnderworld> _underworld;
        private readonly Lazy<IMenuText> _menuText;
        private readonly Lazy<IReadOnlyList<IShop>> _shops;

        public IList<string> CharacterText => _characterText.Value;
        public IEndingText EndingText => _endingText.Value;
        public IOverworld Overworld => _overworld.Value;
        public IUnderworld Underworld => _underworld.Value;
        public IMenuText MenuText => _menuText.Value;
        public IReadOnlyList<IShop> Shops => _shops.Value;
    }
}
