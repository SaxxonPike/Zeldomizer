namespace Zeldomizer.Metal
{
    public class WordList : FixedList<int>
    {
        private readonly byte[] _source;
        private readonly int _offset;

        public WordList(byte[] source, int offset, int capacity) : base(capacity)
        {
            _source = source;
            _offset = offset;
        }

        public override int this[int index]
        {
            get
            {
                return _source[(index << 1) + _offset] |
                    (_source[(index << 1) + _offset + 1] << 8);
            }
            set
            {
                _source[(index << 1) + _offset] = unchecked((byte)value);
                _source[(index << 1) + _offset + 1] = unchecked((byte)(value >> 8));
            }
        }
    }
}
