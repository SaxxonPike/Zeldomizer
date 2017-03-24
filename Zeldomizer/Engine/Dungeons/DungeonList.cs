using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zeldomizer.Metal;

namespace Zeldomizer.Engine.Dungeons
{
    public class DungeonList
    {
        public DungeonList(IRom rom)
        {
            Macros = new DungeonRoomMacroList(rom, 0x160DE, 42);
        }

        public DungeonRoomMacroList Macros { get; }
    }
}
