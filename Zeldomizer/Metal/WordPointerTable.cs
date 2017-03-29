using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Zeldomizer.Metal
{
    public class WordPointerTable : IPointerTable
    {
        private readonly ISource _targetSource;

        // ReSharper disable once CollectionNeverUpdated.Local
        private readonly WordList _table;

        public WordPointerTable(ISource pointerTableSource, ISource targetSource, int count)
        {
            Count = count;
            _targetSource = targetSource;
            _table = new WordList(pointerTableSource, count);
        }

        public int Count { get; }

        public ISource this[int index] =>
            new SourceBlock(_targetSource, _table[index]);

        public IEnumerator<ISource> GetEnumerator()
        {
            return Enumerable
                .Range(0, Count)
                .Select(i => this[i])
                .GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() =>
            GetEnumerator();
    }
}
