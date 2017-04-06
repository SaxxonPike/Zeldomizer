using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Zeldomizer.Metal;

namespace Zeldomizer.Engine.Overworld
{
    public class OverworldColumn : IReadOnlyList<int>
    {
        private readonly ISource _source;

        public OverworldColumn(ISource source)
        {
            _source = source;
        }

        private IEnumerable<int> GetTiles()
        {
            var i = 0;
            while (true)
            {
                var input = _source[i];
                var tile = input.Bits(5, 0);
                var doubleHeight = input.Bit(6);

                yield return tile;
                if (doubleHeight)
                    yield return tile;

                i++;
            }
        }

        public override string ToString() =>
            DebugPrettyPrint.GetByteArray(this);

        IEnumerator IEnumerable.GetEnumerator() => 
            GetEnumerator();
        public IEnumerator<int> GetEnumerator() => 
            GetTiles().Take(Count).GetEnumerator();

        public int Count => 11;

        public int this[int index] => 
            GetTiles().ElementAt(index);
    }
}
