using Zeldomizer.Metal;

namespace Zeldomizer.Engine.Graphics
{
    public class DungeonSpriteDefinitions : ByteList
    {
        public DungeonSpriteDefinitions(IRom source, int offset, int capacity) : base(source, 0x16718, 8)
        {
        }
    }
}
