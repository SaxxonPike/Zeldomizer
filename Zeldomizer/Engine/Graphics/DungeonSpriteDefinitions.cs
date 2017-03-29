using Zeldomizer.Metal;

namespace Zeldomizer.Engine.Graphics
{
    public class DungeonSpriteDefinitions : ByteList
    {
        public DungeonSpriteDefinitions(IRom source) : base(new RomBlock(source, 0x16718), 8)
        {
        }
    }
}
