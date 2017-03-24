using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zeldomizer.Engine.Dungeons
{
    public class DungeonRoomMapper
    {
        private readonly DungeonRoomLayoutList _dungeonRoomLayoutList;
        private readonly DungeonColumnLibraryList _dungeonColumnLibraryList;

        public DungeonRoomMapper(DungeonRoomLayoutList dungeonRoomLayoutList,
            DungeonColumnLibraryList dungeonColumnLibraryList)
        {
            _dungeonRoomLayoutList = dungeonRoomLayoutList;
            _dungeonColumnLibraryList = dungeonColumnLibraryList;
        }

        public IEnumerable<int> Map(DungeonRoom dungeonRoom) =>
            new DungeonRoomLayoutMapper(_dungeonRoomLayoutList[dungeonRoom.LayoutId], _dungeonColumnLibraryList);
    }
}
