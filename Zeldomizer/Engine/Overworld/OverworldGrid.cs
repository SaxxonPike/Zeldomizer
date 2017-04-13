using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Zeldomizer.Engine.Underworld;
using Zeldomizer.Metal;

namespace Zeldomizer.Engine.Overworld
{
    /// <summary>
    /// Represents a 16x8 grid of overworld rooms, in raw form.
    /// </summary>
    /// <remarks>
    /// Refer to <see cref="Underworld.UnderworldGrid"/> for more information about grids.
    /// </remarks>
    public class OverworldGrid : IReadOnlyList<OverworldRoom>
    {
        private readonly ISource _source;

        /// <summary>
        /// Initialize a grid of underworld rooms.
        /// </summary>
        public OverworldGrid(ISource source)
        {
            _source = source;
        }

        /// <summary>
        /// Get all dungeon rooms in the grid.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<OverworldRoom> GetEnumerator() =>
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
        public OverworldRoom this[int index] => new OverworldRoom(new SourceBlock(_source, index));

    }
}
