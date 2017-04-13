using Zeldomizer.Metal;

namespace Zeldomizer.Engine.Underworld
{
    public class UnderworldTileList : ByteList
    {
        private readonly ISource _source;

        public UnderworldTileList(ISource source) : base(source, 0x08)
        {
            _source = source;
        }
    }
}
