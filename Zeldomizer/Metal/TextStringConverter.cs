using System.Linq;
using System.Text;

namespace Zeldomizer.Metal
{
    public class TextStringConverter : IStringConverter
    {
        private readonly ITextConversionTable _textConversionTable;

        public TextStringConverter(ITextConversionTable textConversionTable)
        {
            _textConversionTable = textConversionTable;
        }

        public int GetLength(ISource source)
        {
            var i = 0;
            var result = 0;
            while (true)
            {
                if (source[i] == 0xFF)
                    return result;
                result++;
                i++;
            }
        }

        public string Decode(ISource source)
        {
            var i = 0;
            var output = new StringBuilder();

            while (true)
            {
                var input = source[i];
                switch (input)
                {
                    case 0xFF:
                        return output.ToString();
                    case 0x63:
                        output.Append(' ');
                        break;
                    default:
                        output.Append(_textConversionTable.Decode(input.Bits(5, 0)));
                        break;
                }

                i++;
            }
        }

        public byte[] Encode(string text)
        {
            return (text ?? "")
                .Select(_textConversionTable.Encode)
                .Select(c => unchecked((byte)(c ?? _textConversionTable.SpaceCharacter)))
                .Concat(new byte[]{ 0xFF })
                .ToArray();
        }
    }
}
