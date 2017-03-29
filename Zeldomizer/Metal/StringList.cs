using System;
using System.Linq;

namespace Zeldomizer.Metal
{
    public class StringList : FixedList<string>
    {
        private readonly ISource _source;
        private readonly IPointerTable _pointerTable;
        private readonly int _maxSize;
        private int _currentSize;
        private readonly IStringConverter _stringConverter;

        public StringList(ISource source, IPointerTable pointerTable, IStringConverter stringConverter, int maxSize, int capacity) : base(capacity)
        {
            _source = source;
            _pointerTable = pointerTable;
            _maxSize = maxSize;
            _stringConverter = stringConverter;

            var stringTableStart = pointerTable.Min(p => p.Offset);
            _currentSize = _pointerTable.Max(p => _stringConverter.GetLength(p) + p.Offset - stringTableStart);
        }

        protected virtual string Decode(int index)
        {
            return _stringConverter.Decode(_pointerTable[index]);
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
                var currentLength = _stringConverter.GetLength(_pointerTable[index]);
                var sizeChange = encoded.Length - currentLength;
                if (sizeChange + _currentSize > _maxSize)
                    throw new Exception("New string is too large to fit in the table.");

                // If the size has changed, move the following table data accordingly
                var currentOffset = _pointerTable[index].Offset;
                if (sizeChange != 0)
                {
                    var tableStart = _pointerTable.Min(p => p.Offset);
                    var sourceOffset = currentOffset - sizeChange;
                    var targetOffset = currentOffset;
                    var length = _maxSize - (_pointerTable[index].Offset - tableStart) - Math.Abs(sizeChange);

                    _source.Copy(sourceOffset, targetOffset, length);

                    // Adjust pointer table to account for new string offsets
                    for (var i = index + 1; i < Capacity; i++)
                    {
                        _pointerTable.SetPointer(i,_pointerTable.GetPointer(i) + sizeChange);
                    }

                    _currentSize += sizeChange;
                }

                // Insert the new value into the table
                _source.Write(encoded, currentOffset);
            }
        }
    }
}
