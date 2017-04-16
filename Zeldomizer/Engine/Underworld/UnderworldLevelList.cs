using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Zeldomizer.Metal;

namespace Zeldomizer.Engine.Underworld
{
    public class UnderworldLevelList : IReadOnlyList<UnderworldLevel>
    {
        private readonly ISource _source;

        public UnderworldLevelList(ISource source, int count)
        {
            _source = source;
            Count = count;
        }

        public IEnumerator<UnderworldLevel> GetEnumerator()
        {
            return Enumerable
                .Range(0, Count)
                .Select(i => this[i])
                .GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() => 
            GetEnumerator();

        public int Count { get; }

        public UnderworldLevel this[int index] => 
            new UnderworldLevel(new SourceBlock(_source, 0xFC * index));
    }
}
