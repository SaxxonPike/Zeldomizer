using System.Linq;

namespace Zeldomizer.Metal
{
    public class ByteList : FixedList<int>
    {
        private readonly IRom _source;
        private readonly int _offset;

        public ByteList(IRom source, int offset, int capacity) : base(capacity)
        {
            _source = source;
            _offset = offset;
        }

        public override int this[int index]
        {
            get { return _source[index + _offset]; }
            set { _source[index + _offset] = unchecked((byte)value); }
        }

        public override string ToString()
        {
            return string.Join(" ", this.Select(i => $"{i:x2}"));
        }
    }
}
