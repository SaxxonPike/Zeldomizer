using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Zeldomizer.Metal;

namespace Zeldomizer.Engine.Underworld
{
    /// <summary>
    /// Represents a 16x8 grid of underworld rooms, in raw form.
    /// </summary>
    /// <remarks>
    /// Global coordinates are based off an index within a grid. There are a total of 5 grids in
    /// the entire game: two for the first quest dungeons, two for the second quest dungeons, and
    /// one for the world map which is shared between the two (although there are differences that
    /// are applied at runtime.)
    /// </remarks>
    public class UnderworldGrid : IReadOnlyList<UnderworldRoom>
    {
        private readonly ISource _source;

        /// <summary>
        /// Initialize a grid of underworld rooms.
        /// </summary>
        public UnderworldGrid(ISource source)
        {
            _source = source;
        }

        /// <summary>
        /// Get all dungeon rooms in the grid.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<UnderworldRoom> GetEnumerator() => 
            Enumerable.Range(0, 128).Select(i => this[i]).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() =>
            GetEnumerator();

        /// <summary>
        /// Total number of dungeon room spots in the grid.
        /// </summary>
        public int Count => 128;

        /// <summary>
        /// Get the dungeon room at the specified grid index.
        /// </summary>
        public UnderworldRoom this[int index] => new UnderworldRoom(new SourceBlock(_source, index));
    }
}
