using Zeldomizer.Metal;

namespace Zeldomizer.Engine.Dungeons
{
    public class DungeonRoomLayout : ByteList
    {
        public DungeonRoomLayout(IRom source, int offset) : base(source, offset, 12)
        {
        }
    }
}
