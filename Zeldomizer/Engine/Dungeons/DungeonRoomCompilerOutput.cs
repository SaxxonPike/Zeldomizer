using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zeldomizer.Engine.Dungeons
{
    public class DungeonRoomCompilerOutput
    {
        public IEnumerable<int> Columns { get; set; }
        public IEnumerable<int> Rooms { get; set; }
    }
}
