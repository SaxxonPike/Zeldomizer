using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Zeldomizer.Metal;

namespace Zeldomizer.Engine.Dungeons
{
    public class DungeonList : IEnumerable<Dungeon>
    {
        private DungeonColumnLibraryList _columns;
        private DungeonRoomLayoutList _roomList;
        private DungeonRoomMapper _roomMapper;

        public DungeonList(IRom rom)
        {
            _columns = new DungeonColumnLibraryList(rom, 0x16704, 0xC000, 10);
            _roomList = new DungeonRoomLayoutList(rom, 0x160DE, 42);
            _roomMapper = new DungeonRoomMapper(_roomList, _columns);
        }

        private IEnumerable<Dungeon> GetDungeons()
        {
            yield break;
        }

        public IEnumerable<DungeonRoomLayoutMapper> Rooms
        {
            get { return _roomList.Select(r => new DungeonRoomLayoutMapper(r, _columns)); }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        public IEnumerator<Dungeon> GetEnumerator() =>
            GetDungeons().GetEnumerator();
    }
}
