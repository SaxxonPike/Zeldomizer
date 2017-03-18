using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zeldomizer.Engine;

namespace Zeldomizer
{
    public class ZeldaCartridge
    {
        public ZeldaCartridge(byte[] source)
        {
            MusicPointers = new MusicPointers(source);
        }

        public MusicPointers MusicPointers { get; }
    }
}
