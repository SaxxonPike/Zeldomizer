using System.Collections;
using System.Collections.Generic;

namespace Zeldomizer.Metal
{
    public class OverlappingSourceReader : IEnumerable<ISource>
    {
        private readonly ISource _source;
        private readonly int _count;

        public OverlappingSourceReader(ISource source, int count)
        {
            _source = source;
            _count = count;
        }

        private IEnumerable<ISource> GetSources()
        {
            var i = 0;
            var remaining = _count;
            while (remaining > 0)
            {
                var input = _source[i];
                if ((input & 0x80) != 0)
                {
                    remaining--;
                    yield return new SourceBlock(_source, i);
                }
                i++;
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        public IEnumerator<ISource> GetEnumerator() => GetSources().GetEnumerator();
    }
}
