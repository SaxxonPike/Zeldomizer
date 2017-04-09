using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Zeldomizer.Metal;

namespace Zeldomizer.Engine.Dungeons
{
    public class DungeonGridList : IReadOnlyList<DungeonGrid>
    {
        private readonly ISource _source;

        public DungeonGridList(ISource source, int count)
        {
            _source = source;
            Count = count;
        }

        public IEnumerator<DungeonGrid> GetEnumerator() => 
            Enumerable.Range(0, Count).Select(i => this[i]).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => 
            GetEnumerator();

        public int Count { get; }

        public DungeonGrid this[int index] =>
            new DungeonGrid(new SourceBlock(_source, 0x300 * index));
    }
}
