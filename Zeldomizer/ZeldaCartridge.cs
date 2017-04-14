using System;
using System.Collections.Generic;
using Zeldomizer.Engine.Overworld;
using Zeldomizer.Engine.Text;
using Zeldomizer.Engine.Underworld;
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
        /// <summary>
        /// Create a data adapter.
        /// </summary>
        public ZeldaCartridge(ISource source)
        {
            var conversionTable = new TextConversionTable();
            var speechConverter = new SpeechStringConverter(conversionTable);
            var textConverter = new TextStringConverter(conversionTable);
            var fixedStringConverter = new FixedStringConverter(conversionTable);
            var speechFormatter = new StringFormatter();

            // Character Text
            _characterText = new Lazy<IList<string>>(() => new CharacterText(
                new WordPointerTable(new SourceBlock(source, 0x4000), new SourceBlock(source, -0x4000), 0x26), 
                speechFormatter, 
                speechConverter, 
                0x556, 
                0x26));

            // Ending Text
            _endingText = new Lazy<IEndingText>(() => new EndingText(
                new StringData(new SourceBlock(source, 0xA959), speechConverter, 38),
                new FixedStringData(new SourceBlock(source, 0xAB07), fixedStringConverter, 8),
                new FixedStringData(new SourceBlock(source, 0xAB0F), fixedStringConverter, 24),
                new FixedStringData(new SourceBlock(source, 0xAB27), fixedStringConverter, 20)));

            // Menu Text
            _menuText = new Lazy<IMenuText>(() => new MenuText(new FixedStringData(new SourceBlock(source, 0x09D48), fixedStringConverter, 17),
                new FixedStringData(new SourceBlock(source, 0x09D5E), fixedStringConverter, 18),
                new FixedStringData(new SourceBlock(source, 0x09D70), fixedStringConverter, 8),
                new FixedStringData(new SourceBlock(source, 0x09EEB), fixedStringConverter, 5)));

            // Underworld
            _underworld = new Lazy<IUnderworld>(() =>
            {
                var underworldColumnPointers = new WordPointerTable(
                    new SourceBlock(source, 0x16704),
                    new SourceBlock(source, 0xC000), 10);
                var underworldColumnLibraries = new UnderworldColumnLibraryList(
                    underworldColumnPointers);
                var underworldGrids = new UnderworldGridList(
                    new SourceBlock(source, 0x18700),
                    4);
                var underworldRoomLayouts = new UnderworldRoomLayoutList(
                    new SourceBlock(source, 0x160DE),
                    42);
                return new Underworld(
                    underworldColumnLibraries,
                    underworldGrids,
                    underworldRoomLayouts);
            });

            // Overworld
            _overworld = new Lazy<IOverworld>(() =>
            {
                var overworldColumnPointers = new WordPointerTable(
                    new SourceBlock(source, 0x19D0F),
                    new SourceBlock(source, 0xC000),
                    16);
                var overworldColumnLibraries = new OverworldColumnLibraryList(
                    overworldColumnPointers);
                var overworldGrid = new OverworldGrid(new SourceBlock(
                    source,
                    0x18580));
                var overworldRoomLayouts = new OverworldRoomLayoutList(
                    new SourceBlock(source, 0x15418),
                    124);
                var overworldTiles = new OverworldTileList(
                    new SourceBlock(source, 0x1697C));
                var overworldDetailTiles = new OverworldDetailTileList(
                    new SourceBlock(source, 0x169B4));
                return new Overworld(
                    overworldColumnLibraries, 
                    overworldGrid, 
                    overworldRoomLayouts, 
                    overworldTiles, 
                    overworldDetailTiles);
            });
        }

        private readonly Lazy<IList<string>> _characterText;
        private readonly Lazy<IEndingText> _endingText;
        private readonly Lazy<IOverworld> _overworld;
        private readonly Lazy<IUnderworld> _underworld;
        private readonly Lazy<IMenuText> _menuText;

        public IList<string> CharacterText => _characterText.Value;
        public IEndingText EndingText => _endingText.Value;
        public IOverworld Overworld => _overworld.Value;
        public IUnderworld Underworld => _underworld.Value;
        public IMenuText MenuText => _menuText.Value;
    }
}
