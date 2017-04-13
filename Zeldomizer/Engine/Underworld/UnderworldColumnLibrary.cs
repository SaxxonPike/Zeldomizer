using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Zeldomizer.Metal;

namespace Zeldomizer.Engine.Underworld
{
    /// <summary>
    /// Represents a list of underworld columns, in raw form.
    /// </summary>
    /// <remarks>
    /// Due to the way columns are referenced in room definitions, it is possible
    /// to make use of data that was originally unintended. The original game will
    /// only reference the first 8 or 9 columns in a list, but theoretically you
    /// could use up to 16 and it should still work. Take note of where start markers
    /// are in the raw data if this is the route you're going, as they're treated
    /// like a stream, just like the game code.
    /// </remarks>
    public class UnderworldColumnLibrary : IReadOnlyList<UnderworldColumn>
    {
        private readonly ISource _source;
        private readonly int _count;

        /// <summary>
        /// Initialize a list of columns.
        /// </summary>
        public UnderworldColumnLibrary(ISource source, int count)
        {
            _source = source;
            _count = count;
        }

        /// <summary>
        /// Get the column at the specified index within the list.
        /// </summary>
        public UnderworldColumn this[int index] =>
            GetMacros().ElementAt(index);

        /// <summary>
        /// Get all the columns.
        /// </summary>
        private IEnumerable<UnderworldColumn> GetMacros()
        {
            var reader = new OverlappingSourceReader(_source, _count);
            return reader.Select(s => new UnderworldColumn(s));
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        public IEnumerator<UnderworldColumn> GetEnumerator() => GetMacros().GetEnumerator();

        /// <summary>
        /// Get the number of possible columns in the list.
        /// </summary>
        public int Count => 16;
    }
}
