using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Zeldomizer.Metal;

namespace Zeldomizer.Engine.Dungeons
{
    public class DungeonRoomLayoutList : IEnumerable<DungeonRoomLayout>
    {
        private readonly ISource _source;
        private readonly int _count;

        public DungeonRoomLayoutList(ISource source, int count)
        {
            _source = source;
            _count = count;
        }

        public DungeonRoomLayout this[int index] => 
            new DungeonRoomLayout(new SourceBlock(_source, index * 12));

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
