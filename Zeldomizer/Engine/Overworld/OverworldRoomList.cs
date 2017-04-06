using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Zeldomizer.Metal;

namespace Zeldomizer.Engine.Overworld
{
    public class OverworldRoomList : IReadOnlyList<OverworldRoom>
    {
        private readonly ISource _source;

        public OverworldRoomList(ISource source, int count)
        {
            _source = source;
            Count = count;
        }

        private IEnumerable<OverworldRoom> GetRooms()
        {
            return Enumerable
                .Range(0, Count)
                .Select(i => this[i]);
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        public IEnumerator<OverworldRoom> GetEnumerator() => GetRooms().GetEnumerator();
        public int Count { get; }

        public OverworldRoom this[int index] =>
            new OverworldRoom(new SourceBlock(_source, index << 4));
    }
}
