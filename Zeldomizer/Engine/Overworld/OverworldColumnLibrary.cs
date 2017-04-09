using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zeldomizer.Metal;

namespace Zeldomizer.Engine.Overworld
{
    public class OverworldColumnLibrary : IReadOnlyList<OverworldColumn>
    {
        private readonly ISource _source;

        public OverworldColumnLibrary(ISource source)
        {
            _source = source;
        }

        public OverworldColumn this[int index] =>
            GetColumns().ElementAt(index);

        private IEnumerable<OverworldColumn> GetColumns()
        {
            var reader = new OverlappingSourceReader(_source, 16);
            return reader.Select(s => new OverworldColumn(s));
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        public IEnumerator<OverworldColumn> GetEnumerator() => GetColumns().GetEnumerator();
        public int Count => 256;

    }
}
