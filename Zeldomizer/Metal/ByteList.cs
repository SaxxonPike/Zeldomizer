using System.Linq;

namespace Zeldomizer.Metal
{
    public class ByteList : FixedList<int>
    {
        private readonly IRom _source;

        public ByteList(IRom source, int capacity) : base(capacity)
        {
            _source = source;
        }

        public override int this[int index]
        {
            get { return _source[index]; }
            set { _source[index] = unchecked((byte)value); }
        }

        public override string ToString()
        {
            return string.Join(" ", this.Select(i => $"{i:x2}"));
        }
    }
}
