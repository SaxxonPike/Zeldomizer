using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Zeldomizer.Metal
{
    public class WordPointerTable : IPointerTable
    {
        // ReSharper disable once CollectionNeverUpdated.Local
        private readonly WordList _table;

        public WordPointerTable(ISource pointerTableSource, ISource targetSource, int count)
        {
            Count = count;
            Source = targetSource;
            _table = new WordList(pointerTableSource, count);
        }

        public ISource Source { get; }

        public int Count { get; }

        public ISource this[int index] =>
            new SourceBlock(Source, _table[index]);

        public IEnumerator<ISource> GetEnumerator()
        {
            return Enumerable
                .Range(0, Count)
                .Select(i => this[i])
                .GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() =>
            GetEnumerator();

        public int GetPointer(int index)
        {
            return _table[index];
        }

        public void SetPointer(int index, int value)
        {
            _table[index] = value;
        }
    }
}
