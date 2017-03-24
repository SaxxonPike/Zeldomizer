using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zeldomizer.Metal;

namespace Zeldomizer.Engine.Dungeons
{
    public class DungeonRoomMacro : ByteList
    {
        public DungeonRoomMacro(IRom source, int offset) : base(source, offset, 12)
        {
        }
    }
}
