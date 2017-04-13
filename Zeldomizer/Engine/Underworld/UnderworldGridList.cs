using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Zeldomizer.Metal;

namespace Zeldomizer.Engine.Underworld
{
    /// <summary>
    /// Represents a list of underworld grids, in raw form.
    /// </summary>
    public class UnderworldGridList : IReadOnlyList<UnderworldGrid>
    {
        private readonly ISource _source;

        /// <summary>
        /// Initialize a list of underworld grids.
        /// </summary>
        public UnderworldGridList(ISource source, int count)
        {
            _source = source;
            Count = count;
        }

        /// <summary>
        /// Get all underworld grids.
        /// </summary>
        public IEnumerator<UnderworldGrid> GetEnumerator() => 
            Enumerable.Range(0, Count).Select(i => this[i]).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => 
            GetEnumerator();

        /// <summary>
        /// Get the number of underworld grids in this list.
        /// </summary>
        public int Count { get; }

        /// <summary>
        /// Get the underworld grid at the specified index in the list.
        /// </summary>
        public UnderworldGrid this[int index] =>
            new UnderworldGrid(new SourceBlock(_source, 0x300 * index));
    }
}
