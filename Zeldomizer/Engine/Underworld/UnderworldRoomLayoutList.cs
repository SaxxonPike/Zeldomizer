using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Zeldomizer.Metal;

namespace Zeldomizer.Engine.Underworld
{
    public class UnderworldRoomLayoutList : IEnumerable<UnderworldRoomLayout>
    {
        private readonly ISource _source;
        private readonly int _count;

        public UnderworldRoomLayoutList(ISource source, int count)
        {
            _source = source;
            _count = count;
        }

        public UnderworldRoomLayout this[int index] => 
            new UnderworldRoomLayout(new SourceBlock(_source, index * 12));

        public IEnumerator<UnderworldRoomLayout> GetEnumerator()
        {
            return Enumerable
                .Range(0, _count)
                .Select(i => this[i])
                .GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
