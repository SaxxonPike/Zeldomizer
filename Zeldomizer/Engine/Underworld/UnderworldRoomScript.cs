using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zeldomizer.Engine.Underworld
{
    public enum UnderworldRoomScript
    {
        ShowItem = 0,
        ShowItemAndOpenShutters = 1,
        ExplodingEnemies = 2,
        Ganon = 3,
        PushBlockToOpen = 4,
        PushBlockToStairs = 5,
        OpenAfterClear = 6,
        OpenAfterClearWithItem = 7
    }
}
