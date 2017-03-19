using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zeldomizer.Metal
{
    public class StringData
    {
        private readonly IRom _source;
        private readonly IStringConverter _stringConverter;
        private readonly int _offset;
        private readonly int _maxLength;

        public StringData(IRom source, IStringConverter stringConverter, int offset, int maxLength)
        {
            _source = source;
            _stringConverter = stringConverter;
            _offset = offset;
            _maxLength = maxLength;
        }

        public string Text
        {
            get { return _stringConverter.Decode(_source, _offset); }
            set
            {
                if (value == null)
                    value = string.Empty;
                var encoded = _stringConverter.Encode(value);
                if (encoded.Length > _maxLength)
                    throw new Exception("Text is too long.");
                _source.Write(encoded, _offset);
            }
        }
    }
}
