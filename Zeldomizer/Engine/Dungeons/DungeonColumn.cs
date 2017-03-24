using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Zeldomizer.Metal;

namespace Zeldomizer.Engine.Dungeons
{
    public class DungeonColumn : IEnumerable<int>
    {
        private readonly IRom _rom;
        private readonly int _offset;

        public DungeonColumn(IRom rom, int offset)
        {
            _rom = rom;
            _offset = offset;
        }

        public int this[int index] =>
            GetBlocks().ElementAt(index);

        private IEnumerable<int> GetBlocks()
        {
            var i = _offset;
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
                        Console.WriteLine("end macro");
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
