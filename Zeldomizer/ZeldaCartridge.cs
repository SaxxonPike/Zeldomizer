using Zeldomizer.Engine.Dungeons;
using Zeldomizer.Engine.Music;
using Zeldomizer.Engine.Text;
using Zeldomizer.Metal;

namespace Zeldomizer
{
    public class ZeldaCartridge
    {
        public ZeldaCartridge(IRom source)
        {
            var conversionTable = new TextConversionTable();
            var speechConverter = new SpeechStringConverter(conversionTable);
            var textConverter = new TextStringConverter(conversionTable);
            var fixedStringConverter = new FixedStringConverter(conversionTable);
            var speechFormatter = new StringFormatter();

            MusicPointers = new MusicPointers(source);
            CharacterText = new CharacterText(source, speechFormatter, speechConverter);
            EndingText = new EndingText(source, speechConverter, textConverter, fixedStringConverter);
            Dungeons = new DungeonList(source);
        }

        public CharacterText CharacterText { get; }
        public MusicPointers MusicPointers { get; }
        public EndingText EndingText { get; }
        public DungeonList Dungeons { get; }
    }
}
