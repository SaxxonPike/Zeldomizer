using Zeldomizer.Metal;

namespace Zeldomizer.Engine.Overworld
{
    public class OverworldStart
    {
        private readonly ISource _source;
        private readonly ICoordinate _coordinate;

        public OverworldStart(ISource source)
        {
            _source = source;
            _coordinate = new CoordinateSource(new SourceBlock(source, 0x07), 16, 8);
        }

        public int X
        {
            get => _coordinate.X;
            set => _coordinate.X = value;
        }

        public int Y
        {
            get => _coordinate.Y;
            set => _coordinate.Y = value;
        }

        public int RoomY
        {
            get => _source[0];
            set => _source[0] = unchecked((byte) value);
        }
    }
}
