namespace Zeldomizer.Metal
{
    public class WordList : FixedList<int>
    {
        private readonly ISource _source;

        public WordList(ISource source, int capacity) : base(capacity)
        {
            _source = source;
        }

        public override int this[int index]
        {
            get
            {
                return _source[index << 1] |
                    (_source[(index << 1) + 1] << 8);
            }
            set
            {
                _source[index << 1] = unchecked((byte)value);
                _source[(index << 1) + 1] = unchecked((byte)(value >> 8));
            }
        }
    }
}
