using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Zeldomizer.Metal;

namespace Zeldomizer.Engine.Underworld
{
    public class UnderworldColumnLibraryList : IEnumerable<UnderworldColumnLibrary>
    {
        private readonly IPointerTable _pointerTable;

        public UnderworldColumnLibraryList(IPointerTable pointerTable)
        {
            _pointerTable = pointerTable;
        }

        public UnderworldColumnLibrary this[int index] =>
            new UnderworldColumnLibrary(_pointerTable[index], 16);

        public IEnumerator<UnderworldColumnLibrary> GetEnumerator()
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
