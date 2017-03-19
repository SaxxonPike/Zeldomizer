using Zeldomizer.Metal;

namespace Zeldomizer.Engine
{
    public class MusicPointers : ByteList
    {
        public MusicPointers(IRom source) : base(source, 0x00D60, 0x24)
        {
        }
    }
}
