using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Zeldomizer.Metal;

namespace Zeldomizer.Engine.Overworld
{
    /// <summary>
    /// Represents a list of overworld room layouts, in raw form.
    /// </summary>
    /// <remarks>
    /// Refer to <see cref="Underworld.UnderworldRoomLayoutList"/> for more information about
    /// room layout lists.
    /// </remarks>
    public class OverworldRoomLayoutList : IReadOnlyList<OverworldRoomLayout>
    {
        private readonly ISource _source;

        /// <summary>
        /// Initialize a list of overworld room layouts.
        /// </summary>
        public OverworldRoomLayoutList(ISource source, int count)
        {
            _source = source;
            Count = count;
        }

        /// <summary>
        /// Get all overworld room layouts.
        /// </summary>
        /// <returns></returns>
        private IEnumerable<OverworldRoomLayout> GetRooms()
        {
            return Enumerable
                .Range(0, Count)
                .Select(i => this[i]);
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        public IEnumerator<OverworldRoomLayout> GetEnumerator() => GetRooms().GetEnumerator();

        /// <summary>
        /// Get the number of room layouts.
        /// </summary>
        public int Count { get; }

        /// <summary>
        /// Get the room layout at the specified index in the list.
        /// </summary>
        public OverworldRoomLayout this[int index] =>
            new OverworldRoomLayout(new SourceBlock(_source, index << 4));
    }
}
