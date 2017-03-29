using System.Collections;
using System.Collections.Generic;
using Zeldomizer.Metal;

namespace Zeldomizer.Engine.Dungeons
{
    public class Dungeon : IEnumerable<DungeonRoom>
    {
        private readonly ISource _source;
        private readonly IEnumerable<DungeonRoomLayoutMapper> _roomMappers;
        private readonly IEnumerable<DungeonColumnLibrary> _dungeonColumnLibraryList;

        public Dungeon(ISource source, IEnumerable<DungeonRoomLayoutMapper> roomMappers,
            IEnumerable<DungeonColumnLibrary> dungeonColumnLibraryList)
        {
            _source = source;
            _roomMappers = roomMappers;
            _dungeonColumnLibraryList = dungeonColumnLibraryList;
        }

        private IEnumerable<DungeonRoom> GetDungeonRooms()
        {
            yield break;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public IEnumerator<DungeonRoom> GetEnumerator() =>
            GetDungeonRooms().GetEnumerator();
    }
}
