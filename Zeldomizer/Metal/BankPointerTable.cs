using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Zeldomizer.Metal
{
    public class BankPointerTable : IPointerTable
    {
        public BankPointerTable(ISource targetSource, int count)
        {
            Count = count;
            Source = targetSource;
        }

        public ISource Source { get; }

        public int Count { get; }

        public ISource this[int index] =>
            new SourceBlock(Source, GetPointer(index));

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
            return -0x8000 + index * 0x4000;
        }

        public void SetPointer(int index, int value)
        {
            throw new Exception("Bank pointers cannot be changed.");
        }
    }
}
