using Zeldomizer.Metal;

namespace Zeldomizer.Engine.Overworld
{
    public class OverworldColumnTile
    {
        private readonly ISource _source;

        public OverworldColumnTile(ISource source)
        {
            _source = source;
        }

        public int Data
        {
            get { return _source[0]; }
            set { _source[0] = unchecked((byte) value); }
        }

        public int Id
        {
            get { return _source[0].Bits(5, 0) << 1; }
            set { _source[0] = _source[0].Bits(5, 0, value >> 1); }
        }

        public bool Double
        {
            get { return _source[0].Bit(6); }
            set { _source[0] = _source[0].Bit(6, value); }
        }

        public override string ToString()
        {
            return $"ID={Id} Double={Double}";
        }
    }
}
