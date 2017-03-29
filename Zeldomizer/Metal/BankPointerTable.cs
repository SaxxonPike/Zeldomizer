using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Zeldomizer.Metal
{
    public class BankPointerTable : IPointerTable
    {
        private readonly ISource _targetSource;

        public BankPointerTable(ISource targetSource, int count)
        {
            Count = count;
            _targetSource = targetSource;
        }

        public int Count { get; }

        public ISource this[int index] =>
            new SourceBlock(_targetSource, -0x8000 + index * 0x4000);

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
