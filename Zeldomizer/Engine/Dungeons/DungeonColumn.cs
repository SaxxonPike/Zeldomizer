using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Zeldomizer.Metal;

namespace Zeldomizer.Engine.Dungeons
{
    public class DungeonColumn : IEnumerable<int>
    {
        private readonly IRom _rom;

        public DungeonColumn(IRom rom)
        {
            _rom = rom;
        }

        public int this[int index] =>
            GetBlocks().ElementAt(index);

        private IEnumerable<int> GetBlocks()
        {
            var i = 0;
            var tilesLeft = 7;

            while (true)
            {
                var input = _rom[i++];
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

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<int> GetEnumerator()
        {
            return GetBlocks().GetEnumerator();
        }
    }
}
