using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Zeldomizer.Metal;

namespace Zeldomizer.Engine.Dungeons
{
    public class DungeonRoomLayoutList : IEnumerable<DungeonRoomLayout>
    {
        private readonly IRom _rom;
        private readonly int _count;

        public DungeonRoomLayoutList(IRom rom, int count)
        {
            _rom = rom;
            _count = count;
        }

        public DungeonRoomLayout this[int index] => 
            new DungeonRoomLayout(new RomBlock(_rom, index * 12));

        public IEnumerator<DungeonRoomLayout> GetEnumerator()
        {
            return Enumerable
                .Range(0, _count)
                .Select(i => this[i])
                .GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
