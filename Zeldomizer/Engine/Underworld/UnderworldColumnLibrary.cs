using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Zeldomizer.Metal;

namespace Zeldomizer.Engine.Underworld
{
    public class UnderworldColumnLibrary : IEnumerable<UnderworldColumn>
    {
        private readonly ISource _source;
        private readonly int _count;

        public UnderworldColumnLibrary(ISource source, int count)
        {
            _source = source;
            _count = count;
        }

        public UnderworldColumn this[int index] =>
            GetMacros().ElementAt(index);

        private IEnumerable<UnderworldColumn> GetMacros()
        {
            var reader = new OverlappingSourceReader(_source, _count);
            return reader.Select(s => new UnderworldColumn(s));
        }


        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<UnderworldColumn> GetEnumerator()
        {
            return GetMacros().GetEnumerator();
        }
    }
}
