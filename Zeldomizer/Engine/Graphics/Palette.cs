using Zeldomizer.Metal;

namespace Zeldomizer.Engine.Graphics
{
    public class Palette : ByteList
    {
        public Palette(ISource source) : base(source, 4)
        {
        }
    }
}
