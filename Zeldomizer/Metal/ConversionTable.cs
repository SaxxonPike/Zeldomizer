using System.Collections.Generic;
using System.Linq;

namespace Zeldomizer.Metal
{
    public class ConversionTable : IConversionTable
    {
        private static readonly Dictionary<int, char> DecodeTable = new Dictionary<int, char>
        {
            { 0x00, '0' },
            { 0x01, '1' },
            { 0x02, '2' },
            { 0x03, '3' },
            { 0x04, '4' },
            { 0x05, '5' },
            { 0x06, '6' },
            { 0x07, '7' },
            { 0x08, '8' },
            { 0x09, '9' },
            { 0x0A, 'A' },
            { 0x0B, 'B' },
            { 0x0C, 'C' },
            { 0x0D, 'D' },
            { 0x0E, 'E' },
            { 0x0F, 'F' },
            { 0x10, 'G' },
            { 0x11, 'H' },
            { 0x12, 'I' },
            { 0x13, 'J' },
            { 0x14, 'K' },
            { 0x15, 'L' },
            { 0x16, 'M' },
            { 0x17, 'N' },
            { 0x18, 'O' },
            { 0x19, 'P' },
            { 0x1A, 'Q' },
            { 0x1B, 'R' },
            { 0x1C, 'S' },
            { 0x1D, 'T' },
            { 0x1E, 'U' },
            { 0x1F, 'V' },
            { 0x20, 'W' },
            { 0x21, 'X' },
            { 0x22, 'Y' },
            { 0x23, 'Z' },
            { 0x24, ' ' },
            { 0x28, ',' },
            { 0x29, '!' },
            { 0x2A, '\'' },
            { 0x2B, '&' },
            { 0x2C, '.' },
            { 0x2D, '"' },
            { 0x2E, '?' },
            { 0x2F, '-' },
        };

        private static readonly Dictionary<char, int> EncodeTable = DecodeTable
            .ToDictionary(kv => kv.Value, kv => kv.Key);

        public int? Encode(char input)
        {
            if (EncodeTable.ContainsKey(input))
                return EncodeTable[input];
            return null;
        }

        public char? Decode(int input)
        {
            if (DecodeTable.ContainsKey(input))
                return DecodeTable[input];
            return null;
        }
    }
}
