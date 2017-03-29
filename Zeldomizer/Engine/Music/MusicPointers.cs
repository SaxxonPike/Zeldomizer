using Zeldomizer.Metal;

namespace Zeldomizer.Engine.Music
{
    public class MusicPointers : ByteList
    {
        public MusicPointers(ISource source) : base(new SourceBlock(source, 0x00D60), 0x24)
        {
        }
    }
}
