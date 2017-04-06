using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Zeldomizer.Metal;

namespace Zeldomizer.Engine.Overworld
{
    public class OverworldColumnLibraryList : IReadOnlyList<OverworldColumnLibrary>
    {
        private readonly ISource _source;
        private readonly IReadOnlyList<int> _offsets;

        public OverworldColumnLibraryList(ISource source, IReadOnlyList<int> offsets)
        {
            _source = source;
            _offsets = offsets;
        }

        public IEnumerator<OverworldColumnLibrary> GetEnumerator()
        {
            return Enumerable
                .Range(0, Count)
                .Select(i => this[i])
                .GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int Count => 16;

        public OverworldColumnLibrary this[int index] => 
            new OverworldColumnLibrary(new SourceBlock(_source, _offsets[index]));
    }
}
