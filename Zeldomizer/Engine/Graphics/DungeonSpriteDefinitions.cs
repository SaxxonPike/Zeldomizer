using Zeldomizer.Metal;

namespace Zeldomizer.Engine.Graphics
{
    public class DungeonSpriteDefinitions : ByteList
    {
        public DungeonSpriteDefinitions(ISource source) : base(new SourceBlock(source, 0x16718), 8)
        {
        }
    }
}
