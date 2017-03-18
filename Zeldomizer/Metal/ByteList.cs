namespace Zeldomizer.Metal
{
    public class ByteList : FixedList<int>
    {
        private readonly byte[] _source;
        private readonly int _offset;

        public ByteList(byte[] source, int offset, int capacity) : base(capacity)
        {
            _source = source;
            _offset = offset;
        }

        public override int this[int index]
        {
            get { return _source[index + _offset]; }
            set { _source[index + _offset] = unchecked((byte)value); }
        }
    }
}
