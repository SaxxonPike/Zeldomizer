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
        public ZeldaCartridge(IRom source, IStringFormatter stringFormatter, IStringConverter stringConverter)
        {
            MusicPointers = new MusicPointers(source);
            CharacterText = new CharacterText(source, stringFormatter, stringConverter);
        }

        public CharacterText CharacterText { get; }
        public MusicPointers MusicPointers { get; }
    }
}
