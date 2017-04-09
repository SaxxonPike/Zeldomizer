using System;
using System.Linq;
using System.Text;

namespace Zeldomizer.Metal
{
    public class FixedStringConverter : IFixedStringConverter
    {
        private readonly ITextConversionTable _textConversionTable;

        public FixedStringConverter(ITextConversionTable textConversionTable)
        {
            _textConversionTable = textConversionTable;
        }

        public string Decode(ISource source, int length)
        {
            var output = new StringBuilder();

            for (var i = 0; i < length; i++)
            {
                var input = source[i];
                switch (input)
                {
                    case 0xFF:
                        return output.ToString().Trim();
                    case 0x63:
                        output.Append(' ');
                        break;
                    default:
                        output.Append(_textConversionTable.Decode(input.Bits(5, 0)));
                        break;
                }
            }

            return output.ToString().Trim();
        }

        public byte[] Encode(string text, int length)
        {
            // Convert all newlines into spaces.
            var input = string.Join(" ", (text ?? "")
                .Split(new []{Environment.NewLine}, StringSplitOptions.None))
                .ToUpperInvariant();

            // Encode the string.
            var encoded = input
                .Select(_textConversionTable.Encode)
                .Select(c => unchecked((byte)(c ?? _textConversionTable.SpaceCharacter)))
                .ToArray();

            // Pad the string with spaces.
            if (encoded.Length < length)
                return encoded
                    .Concat(Enumerable.Repeat((byte)_textConversionTable.PaddingCharacter, length - encoded.Length))
                    .ToArray();

            return encoded;
        }

        public int SpaceCharacter => _textConversionTable.SpaceCharacter;
    }
}
