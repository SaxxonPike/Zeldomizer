using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Zeldomizer.Metal;

namespace Zeldomizer.Engine.Overworld
{
    public class OverworldRoomList : IEnumerable<OverworldRoom>
    {
        private readonly ISource _source;
        private readonly int _count;

        public OverworldRoomList(ISource source, int count)
        {
            _source = source;
            _count = count;
        }

        private IEnumerable<OverworldRoom> GetRooms()
        {
            return Enumerable
                .Range(0, _count)
                .Select(i => new OverworldRoom(new SourceBlock(_source, i << 4)));
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        public IEnumerator<OverworldRoom> GetEnumerator() => GetRooms().GetEnumerator();
    }
}
