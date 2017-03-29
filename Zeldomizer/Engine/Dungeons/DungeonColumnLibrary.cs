using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Zeldomizer.Metal;

namespace Zeldomizer.Engine.Dungeons
{
    public class DungeonColumnLibrary : IEnumerable<DungeonColumn>
    {
        private readonly IRom _rom;
        private readonly int _offset;
        private readonly int _count;

        public DungeonColumnLibrary(IRom rom, int offset, int count)
        {
            _rom = rom;
            _offset = offset;
            _count = count;
        }

        public DungeonColumn this[int index] =>
            GetMacros().ElementAt(index);

        private IEnumerable<DungeonColumn> GetMacros()
        {
            var i = _offset;
            var macrosRemaining = _count;

            while (macrosRemaining > 0)
            {
                var input = _rom[i];
                var bit7 = input.Bit(7);
                if (bit7)
                {
                    yield return new DungeonColumn(new RomBlock(_rom, i));
                    macrosRemaining--;
                }
                i++;
            }
        }


        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<DungeonColumn> GetEnumerator()
        {
            return GetMacros().GetEnumerator();
        }
    }
}
