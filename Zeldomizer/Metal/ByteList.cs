﻿using System.Linq;

namespace Zeldomizer.Metal
{
    public class ByteList : FixedList<int>
    {
        private readonly ISource _source;

        public ByteList(ISource source, int capacity) : base(capacity)
        {
            _source = source;
        }

        public override int this[int index]
        {
            get => _source[index];
            set => _source[index] = unchecked((byte)value);
        }

        public override string ToString()
        {
            return DebugPrettyPrint.GetByteArray(this);
        }
    }
}
