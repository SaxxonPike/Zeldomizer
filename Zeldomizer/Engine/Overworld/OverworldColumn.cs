using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Zeldomizer.Metal;

namespace Zeldomizer.Engine.Overworld
{
    /// <summary>
    /// Represents a column of overworld tiles, in raw form.
    /// </summary>
    /// <remarks>
    /// Refer to <see cref="Underworld.UnderworldColumn"/> for more information about columns.
    /// </remarks>
    public class OverworldColumn : IReadOnlyList<int>
    {
        private readonly ISource _source;

        /// <summary>
        /// Initialize an overworld column.
        /// </summary>
        public OverworldColumn(ISource source)
        {
            _source = source;
        }

        /// <summary>
        /// Get all tiles from the column.
        /// </summary>
        private IEnumerable<int> GetTiles()
        {
            var i = 0;
            while (true)
            {
                var input = _source[i];
                var tile = input.Bits(5, 0);
                var doubleHeight = input.Bit(6);

                yield return tile;
                if (doubleHeight)
                    yield return tile;

                i++;
            }
        }

        /// <summary>
        /// Get a string representation of the column.
        /// </summary>
        public override string ToString() =>
            DebugPrettyPrint.GetByteArray(this);

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        public IEnumerator<int> GetEnumerator() => GetTiles().Take(Count).GetEnumerator();

        /// <summary>
        /// Get the number of tiles in the column.
        /// </summary>
        public int Count => 11;

        /// <summary>
        /// Get the tile at the specified Y position in the column.
        /// </summary>
        public int this[int index] => 
            GetTiles().ElementAt(index);
    }
}
