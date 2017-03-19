using System;

namespace Zeldomizer.Metal
{
    public class StringData
    {
        private readonly IRom _source;
        private readonly IStringConverter _stringConverter;
        private readonly int _offset;

        public StringData(IRom source, IStringConverter stringConverter, int offset, int maxLength)
        {
            _source = source;
            _stringConverter = stringConverter;
            _offset = offset;
            MaxLength = maxLength;
        }

        public int MaxLength { get; }

        public string Text
        {
            get { return _stringConverter.Decode(_source, _offset); }
            set
            {
                if (value == null)
                    value = string.Empty;
                var encoded = _stringConverter.Encode(value);
                if (encoded.Length > MaxLength)
                    throw new Exception("Text is too long.");
                _source.Write(encoded, _offset);
            }
        }
    }
}
