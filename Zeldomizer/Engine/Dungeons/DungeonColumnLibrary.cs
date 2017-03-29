using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Zeldomizer.Metal;

namespace Zeldomizer.Engine.Dungeons
{
    public class DungeonColumnLibrary : IEnumerable<DungeonColumn>
    {
        private readonly ISource _source;
        private readonly int _count;

        public DungeonColumnLibrary(ISource source, int count)
        {
            _source = source;
            _count = count;
        }

        public DungeonColumn this[int index] =>
            GetMacros().ElementAt(index);

        private IEnumerable<DungeonColumn> GetMacros()
        {
            var i = 0;
            var macrosRemaining = _count;

            while (macrosRemaining > 0)
            {
                var input = _source[i];
                var bit7 = input.Bit(7);
                if (bit7)
                {
                    yield return new DungeonColumn(new SourceBlock(_source, i));
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
