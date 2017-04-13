using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Zeldomizer.Metal;

namespace Zeldomizer.Engine.Underworld
{
    /// <summary>
    /// Represents a column of underworld tiles, in raw form.
    /// </summary>
    /// <remarks>
    /// Columns are RLE encoded in order to save space where there will be long runs
    /// of the same tile.
    /// </remarks>
    public class UnderworldColumn : IReadOnlyList<int>
    {
        private readonly ISource _source;

        /// <summary>
        /// Initialize an underworld column.
        /// </summary>
        public UnderworldColumn(ISource source)
        {
            _source = source;
        }

        /// <summary>
        /// Get the tile at the specified Y position in the column.
        /// </summary>
        public int this[int index] =>
            GetBlocks().ElementAt(index);

        /// <summary>
        /// Get all the tiles in the column.
        /// </summary>
        private IEnumerable<int> GetBlocks()
        {
            var i = 0;
            var tilesLeft = Count;

            while (true)
            {
                var input = _source[i++];

                // Three bits for run length, range of 1-8
                var count = input.Bits(6, 4) + 1;

                // Lower three bits for tile type.
                var kind = input.Bits(2, 0);

                for (var j = 0; j < count; j++)
                {
                    yield return kind;
                    if (--tilesLeft <= 0)
                    {
                        yield break;
                    }
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        public IEnumerator<int> GetEnumerator() => GetBlocks().GetEnumerator();

        /// <summary>
        /// Get the number of rows in the column.
        /// </summary>
        public int Count => 7;
    }
}
