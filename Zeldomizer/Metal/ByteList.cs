namespace Zeldomizer.Metal
{
    /// <summary>
    /// A fixed length list of bytes.
    /// </summary>
    public class ByteList : FixedList<int>
    {
        private readonly ISource _source;

        /// <summary>
        /// Initialize a fixed length list of bytes.
        /// </summary>
        public ByteList(ISource source, int capacity) : base(capacity)
        {
            _source = source;
        }

        /// <summary>
        /// Get or set the element at the specified index.
        /// </summary>
        public override int this[int index]
        {
            get => _source[index];
            set => _source[index] = unchecked((byte)value);
        }

        /// <summary>
        /// Get a string representation of this list.
        /// </summary>
        public override string ToString() => 
            DebugPrettyPrint.GetByteArray(this);
    }
}
