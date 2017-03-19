using System;
using System.Linq;

namespace Zeldomizer.Metal
{
    public class FixedStringData
    {
        private readonly IRom _source;
        private readonly IFixedStringConverter _fixedStringConverter;
        private readonly int _offset;

        public FixedStringData(IRom source, IFixedStringConverter fixedStringConverter, int offset, int length)
        {
            _source = source;
            _fixedStringConverter = fixedStringConverter;
            _offset = offset;
            Length = length;
        }

        public int Length { get; }

        public string Text
        {
            get
            {
                return _fixedStringConverter.Decode(_source, _offset, Length).Trim();
            }
            set
            {
                // Sanity check.
                if (value == null)
                    value = string.Empty;
                var encoded = _fixedStringConverter.Encode(value, Length);

                // Fail if too large.
                if (encoded.Length > Length)
                    throw new Exception("Text is too long.");

                // Pad if less than the length.
                if (encoded.Length < Length)
                    encoded = encoded
                        .Concat(Enumerable.Repeat((byte) 0x24, Length - encoded.Length))
                        .ToArray();

                // Plop the new string in.
                _source.Write(encoded, _offset);
            }
        }
    }
}
