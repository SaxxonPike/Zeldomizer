using System;
using System.Collections.Generic;
using System.Text;

namespace Zeldomizer.Metal
{
    public class StringList : FixedList<string>
    {
        private readonly byte[] _source;
        private readonly int _pointerAdjustment;
        private readonly int _maxSize;
        private readonly StringConverter _stringConverter;
        private readonly WordList _pointers;

        public StringList(byte[] source, int pointerOffset, int pointerAdjustment, int maxSize, int capacity) : base(capacity)
        {
            _source = source;
            _pointerAdjustment = pointerAdjustment;
            _maxSize = maxSize;
            _stringConverter = new StringConverter();
            _pointers = new WordList(source, pointerOffset, capacity);
        }

        protected virtual string Decode(int index)
        {
            return _stringConverter.Decode(_source, _pointers[index] + _pointerAdjustment);
        }

        protected virtual byte[] Encode(string text)
        {
            return _stringConverter.Encode(text);
        }

        public override string this[int index]
        {
            get { return Decode(index); }
            set
            {
                throw new Exception("Can't encode yet.");
            }
        }
    }
}
