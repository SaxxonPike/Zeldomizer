using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Zeldomizer.Metal;

namespace Zeldomizer.Engine.Underworld
{
    public class UnderworldGrid : IReadOnlyList<UnderworldRoom>
    {
        private readonly ISource _source;

        public UnderworldGrid(ISource source)
        {
            _source = source;
        }

        public IEnumerator<UnderworldRoom> GetEnumerator() => 
            Enumerable.Range(0, 128).Select(i => this[i]).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() =>
            GetEnumerator();

        public int Count => 128;

        public UnderworldRoom this[int index] => new UnderworldRoom(new SourceBlock(_source, index));
    }
}
