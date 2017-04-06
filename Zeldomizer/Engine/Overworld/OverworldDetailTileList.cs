using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Zeldomizer.Metal;

namespace Zeldomizer.Engine.Overworld
{
    public class OverworldDetailTileList : IReadOnlyList<OverworldDetailTile>
    {
        private readonly ISource _source;

        public OverworldDetailTileList(ISource source)
        {
            _source = source;
        }

        public OverworldDetailTile this[int index] => 
            new OverworldDetailTile(new SourceBlock(_source, index << 4));

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public IEnumerator<OverworldDetailTile> GetEnumerator()
        {
            return Enumerable
                .Range(0, Count)
                .Select(i => this[i])
                .GetEnumerator();
        }

        public int Count => 16;
    }
}
