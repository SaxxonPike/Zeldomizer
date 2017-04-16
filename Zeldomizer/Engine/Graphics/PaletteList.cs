using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Zeldomizer.Metal;

namespace Zeldomizer.Engine.Graphics
{
    public class PaletteList : IReadOnlyList<Palette>
    {
        private readonly ISource _source;

        public PaletteList(ISource source, int count)
        {
            _source = source;
            Count = count;
        }

        public IEnumerator<Palette> GetEnumerator()
        {
            return Enumerable
                .Range(0, Count)
                .Select(i => this[i])
                .GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public int Count { get; }

        public Palette this[int index] => 
            new Palette(new SourceBlock(_source, index << 2));
    }
}
