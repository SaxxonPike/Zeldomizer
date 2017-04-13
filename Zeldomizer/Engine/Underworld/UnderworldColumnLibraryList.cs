using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Zeldomizer.Metal;

namespace Zeldomizer.Engine.Underworld
{
    /// <summary>
    /// Represents a list of lists of columns, in raw form.
    /// </summary>
    /// <remarks>
    /// The original game code will reference up to 9 of these, but since the
    /// value is 4 bits wide, you can theoritcally reference up to 16. Be careful
    /// when you do this; these reference pointers from a table and could point
    /// to I/O mapped memory if you go out of range.
    /// </remarks>
    public class UnderworldColumnLibraryList : IReadOnlyList<UnderworldColumnLibrary>
    {
        private readonly IPointerTable _pointerTable;

        /// <summary>
        /// Initialize a list of lists of columns.
        /// </summary>
        public UnderworldColumnLibraryList(IPointerTable pointerTable)
        {
            _pointerTable = pointerTable;
        }

        /// <summary>
        /// Get a list of columns at the specified index.
        /// </summary>
        public UnderworldColumnLibrary this[int index] =>
            new UnderworldColumnLibrary(_pointerTable[index], 16);

        public IEnumerator<UnderworldColumnLibrary> GetEnumerator()
        {
            return Enumerable
                .Range(0, Count)
                .Select(i => this[i])
                .GetEnumerator();
        }

        /// <summary>
        /// Number of lists of columns.
        /// </summary>
        public int Count => _pointerTable.Count;

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
