using System.Linq;
using Zeldomizer.Engine.Dungeons;
using Zeldomizer.Engine.Music;
using Zeldomizer.Engine.Text;
using Zeldomizer.Metal;

namespace Zeldomizer
{
    public class ZeldaCartridge
    {
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
            DungeonGrids = new DungeonGridList(new SourceBlock(source, 0x18700), 4);

            var dungeonColumnPointers = new WordPointerTable(new SourceBlock(source, 0x16704), new SourceBlock(source, 0xC000), 10);
            DungeonColumnLibraries = new DungeonColumnLibraryList(dungeonColumnPointers);
        }

        public CharacterText CharacterText { get; }
        public MusicPointers MusicPointers { get; }
        public EndingText EndingText { get; }
        public DungeonGridList DungeonGrids { get; }
        public DungeonColumnLibraryList DungeonColumnLibraries { get; }
    }
}
