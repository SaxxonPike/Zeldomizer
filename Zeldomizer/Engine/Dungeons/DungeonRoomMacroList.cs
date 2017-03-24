using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Zeldomizer.Metal;

namespace Zeldomizer.Engine.Dungeons
{
    public class DungeonRoomMacroList : IEnumerable<DungeonRoomMacro>
    {
        private readonly IRom _rom;
        private readonly int _offset;
        private readonly int _count;

        public DungeonRoomMacroList(IRom rom, int offset, int count)
        {
            _rom = rom;
            _offset = offset;
            _count = count;
        }

        public DungeonRoomMacro this[int index] => 
            new DungeonRoomMacro(_rom, _offset + index * 12);

        public IEnumerator<DungeonRoomMacro> GetEnumerator()
        {
            return Enumerable
                .Range(0, _count)
                .Select(i => this[i])
                .GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
