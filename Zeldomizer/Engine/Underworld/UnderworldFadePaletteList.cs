using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Zeldomizer.Engine.Graphics;
using Zeldomizer.Metal;

namespace Zeldomizer.Engine.Underworld
{
    public class UnderworldFadePaletteList : IReadOnlyList<Palette>
    {
        private readonly ISource _source;

        public UnderworldFadePaletteList(ISource source, int count)
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
            new Palette(new SourceBlock(_source, index << 3));
    }
}
