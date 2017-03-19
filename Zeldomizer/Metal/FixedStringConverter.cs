using System;
using System.Linq;
using System.Text;

namespace Zeldomizer.Metal
{
    public class FixedStringConverter : IFixedStringConverter
    {
        private readonly IConversionTable _conversionTable;

        public FixedStringConverter(IConversionTable conversionTable)
        {
            _conversionTable = conversionTable;
        }

        public string Decode(IRom source, int offset, int length)
        {
            var output = new StringBuilder();

            for (var i = 0; i < length; i++)
            {
                var input = source[i + offset];
                switch (input)
                {
                    case 0xFF:
                        return output.ToString().Trim();
                    case 0x63:
                        output.Append(' ');
                        break;
                    default:
                        output.Append(_conversionTable.Decode(input & 0x3F));
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
                .Select(_conversionTable.Encode)
                .Select(c => unchecked((byte)(c ?? 0x24)))
                .ToArray();

            // Pad the string with spaces.
            if (encoded.Length < length)
                return encoded
                    .Concat(Enumerable.Repeat((byte)0x25, length - encoded.Length))
                    .ToArray();

            return encoded;
        }

    }
}
