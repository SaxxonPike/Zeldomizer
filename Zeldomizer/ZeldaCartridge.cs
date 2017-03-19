using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zeldomizer.Engine;
using Zeldomizer.Metal;

namespace Zeldomizer
{
    public class ZeldaCartridge
    {
        public ZeldaCartridge(IRom source)
        {
            var conversionTable = new ConversionTable();
            var speechConverter = new SpeechStringConverter(conversionTable);
            var textConverter = new TextStringConverter(conversionTable);
            var fixedStringConverter = new FixedStringConverter(conversionTable);
            var speechFormatter = new StringFormatter();

            MusicPointers = new MusicPointers(source);
            CharacterText = new CharacterText(source, speechFormatter, speechConverter);
            EndingText = new EndingText(source, speechConverter, textConverter, fixedStringConverter);
        }

        public CharacterText CharacterText { get; }
        public MusicPointers MusicPointers { get; }
        public EndingText EndingText { get; }
    }
}
