namespace Zeldomizer.Metal
{
    public class CoordinateSource : ICoordinate
    {
        private readonly ISource _source;
        private readonly int _width;
        private readonly int _height;

        public CoordinateSource(ISource source, int width, int height)
        {
            _source = source;
            _width = width;
            _height = height;
        }

        public int X
        {
            get => _source[0] % _width;
            set => _source[0] = unchecked((byte)(_source[0] / _width * _width + value % _width));
        }

        public int Y
        {
            get => _source[0] / _width % _height;
            set => _source[0] = unchecked((byte)(_source[0] % _width + value % _height * _width));
        }

        public int Value
        {
            get => _source[0];
            set => _source[0] = unchecked((byte)value);
        }
    }
}
