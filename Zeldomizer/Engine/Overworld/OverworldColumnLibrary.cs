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
            var offset = 0;
            var count = 0;
            while (count < 16)
            {
                var input = _source[offset];
                var startFlag = (input & 0x80) != 0;

                if (startFlag)
                {
                    yield return new OverworldColumn(new SourceBlock(_source, offset));
                    count++;
                }

                offset++;
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        public IEnumerator<OverworldColumn> GetEnumerator() => GetColumns().GetEnumerator();
        public int Count => 256;

    }
}
