using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Zeldomizer.Metal;

namespace Zeldomizer.Engine.Dungeons
{
    public class DungeonList : IEnumerable<Dungeon>
    {
        private readonly DungeonColumnLibraryList _columns;
        private readonly DungeonRoomLayoutList _roomList;
        private DungeonRoomMapper _roomMapper;

        public DungeonList(IRom rom)
        {
            var columnPointerTable = new WordPointerTable(new RomBlock(rom, 0x16704), new RomBlock(rom, 0xC000), 10);
            _columns = new DungeonColumnLibraryList(columnPointerTable);
            _roomList = new DungeonRoomLayoutList(new RomBlock(rom, 0x160DE), 42);
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
