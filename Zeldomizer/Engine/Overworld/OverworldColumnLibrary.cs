using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Zeldomizer.Metal;

namespace Zeldomizer.Engine.Overworld
{
    /// <summary>
    /// Represents a list of overworld columns, in raw form.
    /// </summary>
    /// <remarks>
    /// Refer to <see cref="Underworld.UnderworldColumnLibrary"/> for more information
    /// about column lists.
    /// </remarks>
    public class OverworldColumnLibrary : IReadOnlyList<OverworldColumn>
    {
        private readonly ISource _source;

        public OverworldColumnLibrary(ISource source)
        {
            _source = source;
        }

        /// <summary>
        /// Get the overworld column at the specified index.
        /// </summary>
        public OverworldColumn this[int index] =>
            GetColumns().ElementAt(index);

        /// <summary>
        /// Get all overworld columns.
        /// </summary>
        /// <returns></returns>
        private IEnumerable<OverworldColumn> GetColumns()
        {
            var reader = new OverlappingSourceReader(_source, 16);
            return reader.Select(s => new OverworldColumn(s));
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        public IEnumerator<OverworldColumn> GetEnumerator() => GetColumns().GetEnumerator();

        /// <summary>
        /// Number of overworld columns.
        /// </summary>
        public int Count => 256;

    }
}
