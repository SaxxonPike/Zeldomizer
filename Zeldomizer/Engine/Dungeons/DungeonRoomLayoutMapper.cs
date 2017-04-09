using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Zeldomizer.Metal;

namespace Zeldomizer.Engine.Dungeons
{
    public class DungeonRoomLayoutMapper : IEnumerable<int>
    {
        private readonly DungeonRoomLayout _dungeonRoomLayout;
        private readonly DungeonColumnLibraryList _dungeonColumnLibraryList;

        public DungeonRoomLayoutMapper(DungeonRoomLayout dungeonRoomLayout, DungeonColumnLibraryList dungeonColumnLibraryList)
        {
            _dungeonRoomLayout = dungeonRoomLayout;
            _dungeonColumnLibraryList = dungeonColumnLibraryList;
        }

        private IEnumerable<int> GetTiles()
        {
            var columns = _dungeonRoomLayout
                .Select(columnId => _dungeonColumnLibraryList[columnId.Bits(7, 4)][columnId.Bits(3, 0)].ToArray());
            return Enumerable.Range(0, 7)
                .SelectMany(rowId => columns.Select(column => column[rowId]));
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        public IEnumerator<int> GetEnumerator() => GetTiles().GetEnumerator();
    }
}
