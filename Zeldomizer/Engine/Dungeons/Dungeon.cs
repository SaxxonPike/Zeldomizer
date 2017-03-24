﻿using System.Collections;
using System.Collections.Generic;
using Zeldomizer.Metal;

namespace Zeldomizer.Engine.Dungeons
{
    public class Dungeon : IEnumerable<DungeonRoom>
    {
        private readonly IRom _rom;
        private readonly int _offset;
        private readonly IEnumerable<DungeonRoomLayoutMapper> _roomMappers;
        private readonly IEnumerable<DungeonColumnLibrary> _dungeonColumnLibraryList;

        public Dungeon(IRom rom, int offset, IEnumerable<DungeonRoomLayoutMapper> roomMappers,
            IEnumerable<DungeonColumnLibrary> dungeonColumnLibraryList)
        {
            _rom = rom;
            _offset = offset;
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