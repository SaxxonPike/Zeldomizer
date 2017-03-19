using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zeldomizer.Metal
{
    public class FixedStringData
    {
        private readonly IRom _source;
        private readonly IStringConverter _stringConverter;
        private readonly int _offset;
        private readonly int _length;

        public FixedStringData(IRom source, IStringConverter stringConverter, int offset, int length)
        {
            _source = source;
            _stringConverter = stringConverter;
            _offset = offset;
            _length = length;
        }

        public string Text
        {
            get
            {
                return _stringConverter.Decode(_source, _offset).Trim();
            }
            set
            {
                // Sanity check.
                if (value == null)
                    value = string.Empty;
                var encoded = _stringConverter.Encode(value);

                // Fail if too large.
                if (encoded.Length > _length)
                    throw new Exception("Text is too long.");

                // Pad if less than the length.
                if (encoded.Length < _length)
                    encoded = encoded
                        .Concat(Enumerable.Repeat((byte) 0x24, _length - encoded.Length))
                        .ToArray();

                // Plop the new string in.
                _source.Write(encoded, _offset);
            }
        }
    }
}
