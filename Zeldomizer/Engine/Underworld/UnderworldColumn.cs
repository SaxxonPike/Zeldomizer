using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Zeldomizer.Metal;

namespace Zeldomizer.Engine.Underworld
{
    public class UnderworldColumn : IEnumerable<int>
    {
        private readonly ISource _source;

        public UnderworldColumn(ISource source)
        {
            _source = source;
        }

        public int this[int index] =>
            GetBlocks().ElementAt(index);

        private IEnumerable<int> GetBlocks()
        {
            var i = 0;
            var tilesLeft = 7;

            while (true)
            {
                var input = _source[i++];
                var count = input.Bits(6, 4) + 1;
                var kind = input.Bits(2, 0);
                for (var j = 0; j < count; j++)
                {
                    yield return kind;
                    if (--tilesLeft <= 0)
                    {
                        yield break;
                    }
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        public IEnumerator<int> GetEnumerator() => GetBlocks().GetEnumerator();
    }
}
