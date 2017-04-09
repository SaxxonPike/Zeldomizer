using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Zeldomizer.Metal;

namespace Zeldomizer.Engine.Dungeons
{
    public class DungeonGrid : IReadOnlyList<DungeonRoom>
    {
        private readonly ISource _source;

        public DungeonGrid(ISource source)
        {
            _source = source;
        }

        public IEnumerator<DungeonRoom> GetEnumerator() => 
            Enumerable.Range(0, 128).Select(i => this[i]).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() =>
            GetEnumerator();

        public int Count => 128;

        public DungeonRoom this[int index] => new DungeonRoom(new SourceBlock(_source, index));
    }
}
