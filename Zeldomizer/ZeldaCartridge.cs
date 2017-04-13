using Zeldomizer.Engine.Music;
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

            MusicPointers = new MusicPointers(source);
            CharacterText = new CharacterText(new WordPointerTable(new SourceBlock(source, 0x4000), new SourceBlock(source, -0x4000), 0x26), speechFormatter, speechConverter);
            EndingText = new EndingText(source, speechConverter, textConverter, fixedStringConverter);

            var underworldColumnPointers = new WordPointerTable(new SourceBlock(source, 0x16704), new SourceBlock(source, 0xC000), 10);
            var underworldColumnLibraries = new UnderworldColumnLibraryList(underworldColumnPointers);
            var underworldGrids = new UnderworldGridList(new SourceBlock(source, 0x18700), 4);
            var underworldRoomLayouts = new UnderworldRoomLayoutList(new SourceBlock(source, 0x160DE), 42);
            Underworld = new Underworld(underworldColumnLibraries, underworldGrids, underworldRoomLayouts);

            var overworldColumnPointers = new WordPointerTable(new SourceBlock(source, 0x19D0F), new SourceBlock(source, 0xC000), 16);
            var overworldColumnLibraries = new OverworldColumnLibraryList(overworldColumnPointers);
            var overworldGrid = new OverworldGrid(new SourceBlock(source, 0x18580));
            var overworldRoomLayouts = new OverworldRoomLayoutList(new SourceBlock(source, 0x15418), 124);
            var overworldTiles = new OverworldTileList(new SourceBlock(source, 0x1697C));
            var overworldDetailTiles = new OverworldDetailTileList(new SourceBlock(source, 0x169B4));
            Overworld = new Overworld(overworldColumnLibraries, overworldGrid, overworldRoomLayouts, overworldTiles, overworldDetailTiles);
        }

        public CharacterText CharacterText { get; }
        public MusicPointers MusicPointers { get; }
        public EndingText EndingText { get; }
        public Overworld Overworld { get; }
        public Underworld Underworld { get; }
    }
}
