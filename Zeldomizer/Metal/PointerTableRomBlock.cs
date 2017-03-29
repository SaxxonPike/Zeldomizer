using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Zeldomizer.Metal
{
    public class WordPointerTable : IPointerTable
    {
        private readonly IRom _targetRom;

        // ReSharper disable once CollectionNeverUpdated.Local
        private readonly WordList _table;

        public WordPointerTable(IRom pointerTableRom, IRom targetRom, int count)
        {
            Count = count;
            _targetRom = targetRom;
            _table = new WordList(pointerTableRom, count);
        }

        public int Count { get; }

        public IRom this[int index] =>
            new RomBlock(_targetRom, _table[index]);

        public IEnumerator<IRom> GetEnumerator()
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
