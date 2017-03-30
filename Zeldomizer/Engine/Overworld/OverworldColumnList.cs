using System.Collections;
using System.Collections.Generic;
using Zeldomizer.Metal;

namespace Zeldomizer.Engine.Overworld
{
    public class OverworldColumnList : IEnumerable<OverworldColumn>
    {
        private readonly ISource _source;

        public OverworldColumnList(ISource source)
        {
            _source = source;
        }

        private IEnumerable<OverworldColumn> GetColumns()
        {
            var i = 0;
            var count = 0;
            while (count < 256)
            {
                var input = _source[i];
                var startFlag = (input & 0x80) != 0;

                if (startFlag)
                {
                    yield return new OverworldColumn(new SourceBlock(_source, i));
                    count++;
                }

                i++;
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        public IEnumerator<OverworldColumn> GetEnumerator() => GetColumns().GetEnumerator();
    }
}
