using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Zeldomizer.Metal;

namespace Zeldomizer.Engine.Underworld
{
    /// <summary>
    /// Represents a list of room layouts, in raw form.
    /// </summary>
    /// <remarks>
    /// Each room layout is defined by a list of columns indices. This way,
    /// room layouts that share significant portions of their makeup can
    /// also save space by sharing entire columns.
    /// </remarks>
    public class UnderworldRoomLayoutList : IReadOnlyList<UnderworldRoomLayout>
    {
        private readonly ISource _source;

        /// <summary>
        /// Initialize a list of room layouts.
        /// </summary>
        public UnderworldRoomLayoutList(ISource source, int count)
        {
            _source = source;
            Count = count;
        }

        /// <summary>
        /// Get the room layout at the specified index.
        /// </summary>
        public UnderworldRoomLayout this[int index] => 
            new UnderworldRoomLayout(new SourceBlock(_source, index * 12));

        /// <summary>
        /// Enumerate all room layouts.
        /// </summary>
        public IEnumerator<UnderworldRoomLayout> GetEnumerator()
        {
            return Enumerable
                .Range(0, Count)
                .Select(i => this[i])
                .GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        /// <summary>
        /// Get the number of room layouts.
        /// </summary>
        public int Count { get; }
    }
}
