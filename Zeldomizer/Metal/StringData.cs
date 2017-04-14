using System;

namespace Zeldomizer.Metal
{
    public class StringData : IStringData
    {
        private readonly ISource _source;
        private readonly IStringConverter _stringConverter;

        public StringData(ISource source, IStringConverter stringConverter, int maxLength)
        {
            _source = source;
            _stringConverter = stringConverter;
            MaxLength = maxLength;
        }

        public int MaxLength { get; }

        public string Text
        {
            get => _stringConverter.Decode(_source);
            set
            {
                if (value == null)
                    value = string.Empty;
                var encoded = _stringConverter.Encode(value);
                if (encoded.Length > MaxLength)
                    throw new Exception("Text is too long.");
                _source.Write(encoded, 0);
            }
        }
    }

    public interface IStringData
    {
        int MaxLength { get; }
        string Text { get; set; }
    }
}
