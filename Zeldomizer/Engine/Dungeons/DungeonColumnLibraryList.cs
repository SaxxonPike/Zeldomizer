using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Zeldomizer.Metal;

namespace Zeldomizer.Engine.Dungeons
{
    public class DungeonColumnLibraryList : IEnumerable<DungeonColumnLibrary>
    {
        private readonly IPointerTable _pointerTable;

        public DungeonColumnLibraryList(IPointerTable pointerTable)
        {
            _pointerTable = pointerTable;
        }

        public DungeonColumnLibrary this[int index] =>
            new DungeonColumnLibrary(_pointerTable[index], 16);

        public IEnumerator<DungeonColumnLibrary> GetEnumerator()
        {
            return Enumerable
                .Range(0, _pointerTable.Count)
                .Select(i => this[i])
                .GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
