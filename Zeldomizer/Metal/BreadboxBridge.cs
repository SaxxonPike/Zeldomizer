using Breadbox;

namespace Zeldomizer.Metal
{
    public class BreadboxBridge : IMemory
    {
        private readonly ISource _source;

        public BreadboxBridge(ISource source)
        {
            _source = source;
        }

        public int Read(int obj0) => _source[obj0];
        public void Write(int obj0, int obj1) => _source[obj0] = unchecked((byte)obj1);
    }
}
