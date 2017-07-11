using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Zeldomizer.Metal;

namespace Zeldomizer.Engine.Overworld
{
    /// <summary>
    /// Represents a list of overworld detail tiles, in raw form.
    /// </summary>
    /// <remarks>
    /// Refer to <see cref="OverworldDetailTile"/> for more information about detail tiles.
    /// </remarks>
    public class OverworldDetailTileList : IReadOnlyList<OverworldDetailTile>
    {
        private readonly ISource _source;

        /// <summary>
        /// Initialize a list of overworld detail tiles.
        /// </summary>
        public OverworldDetailTileList(ISource source)
        {
            _source = source;
        }

        /// <summary>
        /// Get the overworld detail tile at the specified index.
        /// </summary>
        public OverworldDetailTile this[int index] => 
            new OverworldDetailTile(new SourceBlock(_source, index << 4));

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        /// <summary>
        /// Enumerate all detail tiles.
        /// </summary>
        public IEnumerator<OverworldDetailTile> GetEnumerator()
        {
            return Enumerable
                .Range(0, Count)
                .Select(i => this[i])
                .GetEnumerator();
        }

        /// <summary>
        /// Get the number of detail tiles.
        /// </summary>
        public int Count => 16;
    }
}
