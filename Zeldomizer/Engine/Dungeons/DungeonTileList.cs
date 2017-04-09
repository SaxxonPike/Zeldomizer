using Zeldomizer.Metal;

namespace Zeldomizer.Engine.Dungeons
{
    public class DungeonTileList : ByteList
    {
        private readonly ISource _source;

        public DungeonTileList(ISource source) : base(source, 0x08)
        {
            _source = source;
        }
    }
}
