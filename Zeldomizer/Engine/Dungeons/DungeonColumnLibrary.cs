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
            var reader = new OverlappingSourceReader(_source, _count);
            return reader.Select(s => new DungeonColumn(s));
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
