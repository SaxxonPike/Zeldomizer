using System.Linq;
using System.Text;

namespace Zeldomizer.Metal
{
    public class TextStringConverter : IStringConverter
    {
        private readonly IConversionTable _conversionTable;

        public TextStringConverter(IConversionTable conversionTable)
        {
            _conversionTable = conversionTable;
        }

        public int GetLength(IRom source, int offset)
        {
            var i = offset;
            var result = 0;
            while (true)
            {
                if (source[i] == 0xFF)
                    return result;
                result++;
                i++;
            }
        }

        public string Decode(IRom source, int offset)
        {
            var i = offset;
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
                        output.Append(_conversionTable.Decode(input & 0x3F));
                        break;
                }

                i++;
            }
        }

        public byte[] Encode(string text)
        {
            return (text ?? "")
                .Select(_conversionTable.Encode)
                .Select(c => unchecked((byte)(c ?? 0x24)))
                .Concat(new byte[]{ 0xFF })
                .ToArray();
        }
    }
}
