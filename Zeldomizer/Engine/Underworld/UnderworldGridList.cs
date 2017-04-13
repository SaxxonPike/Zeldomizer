using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Zeldomizer.Metal;

namespace Zeldomizer.Engine.Underworld
{
    public class UnderworldGridList : IReadOnlyList<UnderworldGrid>
    {
        private readonly ISource _source;

        public UnderworldGridList(ISource source, int count)
        {
            _source = source;
            Count = count;
        }

        public IEnumerator<UnderworldGrid> GetEnumerator() => 
            Enumerable.Range(0, Count).Select(i => this[i]).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => 
            GetEnumerator();

        public int Count { get; }

        public UnderworldGrid this[int index] =>
            new UnderworldGrid(new SourceBlock(_source, 0x300 * index));
    }
}
