using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zeldomizer.Engine;

namespace Zeldomizer.Metal
{
    public class StringList : FixedList<string>
    {
        private readonly IRom _source;
        private readonly int _pointerAdjustment;
        private readonly int _maxSize;
        private int _currentSize;
        private readonly IStringConverter _stringConverter;
        private readonly WordList _pointers;

        public StringList(IRom source, IStringConverter stringConverter, int pointerOffset, int pointerAdjustment, int maxSize, int capacity) : base(capacity)
        {
            _source = source;
            _pointerAdjustment = pointerAdjustment;
            _maxSize = maxSize;
            _stringConverter = stringConverter;
            _pointers = new WordList(source, pointerOffset, capacity);

            var stringTableStart = _pointers.Min();
            _currentSize = _pointers.Max(p => _stringConverter.GetLength(source, p + pointerAdjustment) + p - stringTableStart);
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
            get
            {
                return Decode(index);
            }
            set
            {
                // Determine if the new value will overflow the table
                var encoded = Encode(value);
                var currentLength = _stringConverter.GetLength(_source, _pointers[index] + _pointerAdjustment);
                var sizeChange = encoded.Length - currentLength;
                if (sizeChange + _currentSize > _maxSize)
                    throw new Exception("New string is too large to fit in the table.");

                // If the size has changed, move the following table data accordingly
                var currentOffset = _pointers[index] + _pointerAdjustment;
                if (sizeChange != 0)
                {
                    var tableStart = _pointers.Min();
                    var sourceOffset = currentOffset - sizeChange;
                    var targetOffset = currentOffset;
                    var length = _maxSize - (_pointers[index] - tableStart) - Math.Abs(sizeChange);

                    _source.Copy(sourceOffset, targetOffset, length);

                    // Adjust pointer table to account for new string offsets
                    for (var i = index + 1; i < Capacity; i++)
                    {
                        _pointers[i] += sizeChange;
                    }

                    _currentSize += sizeChange;
                }

                // Insert the new value into the table
                _source.Write(encoded, currentOffset);
            }
        }
    }
}
