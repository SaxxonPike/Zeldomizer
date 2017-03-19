using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeldomizer.Metal
{
    public class StringConverter : IStringConverter
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

        private const int FastSpace = 0x25;

        private const int EndCommand = 0xC0;

        private static readonly Dictionary<char, int> EncodeTable = DecodeTable
            .ToDictionary(kv => kv.Value, kv => kv.Key);

        public int GetLength(IRom source, int offset)
        {
            var i = offset;
            var result = 0;

            while (true)
            {
                var input = source[i];
                var commandBits = input >> 6;

                result++;

                if (commandBits == 0x3)
                    break;

                i++;
            }

            return result;
        }

        public string Decode(IRom source, int offset)
        {
            var i = offset;
            var currentLine = 0;
            var lines = Enumerable
                .Range(0, 3)
                .Select(x => new StringBuilder())
                .ToArray();
            var maxLine = 0;

            while (true)
            {
                var input = source[i];
                var commandBits = input >> 6;
                var character = input & 0x3F;
                var decoded = DecodeTable.ContainsKey(character)
                    ? DecodeTable[character]
                    : ' ';

                lines[currentLine].Append(decoded);

                switch (commandBits)
                {
                    case 0x1:
                        currentLine = 2;
                        break;
                    case 0x2:
                        currentLine = 1;
                        break;
                    case 0x3:
                        return string.Join(Environment.NewLine, lines.Take(maxLine + 1).Select(x => x.ToString()));
                }

                if (currentLine > maxLine)
                    maxLine = currentLine;

                i++;
            }
        }

        public byte[] Encode(string text)
        {
            // Break up existing lines.
            var input = text.Split(new[] {Environment.NewLine}, StringSplitOptions.None);
            if (input.Length > 3)
                throw new Exception("Too many lines.");

            // Encode each line of the string
            var rawData = input
                .Select(s => string.IsNullOrEmpty(s) ? " " : s.ToUpperInvariant())
                .Select(s => s.Select(c => EncodeTable.ContainsKey(c) ? EncodeTable[c] : EncodeTable[' ']).ToArray())
                .ToArray();

            // Interpret the spaces around each line as fast spaces
            foreach (var line in rawData)
            {
                for (var i = 0; i < line.Length; i++)
                {
                    if (line[i] != EncodeTable[' '])
                        break;
                    line[i] = FastSpace;
                }
                for (var i = line.Length - 1; i >= 0; i--)
                {
                    if (line[i] != EncodeTable[' '])
                        break;
                    line[i] = FastSpace;
                }
            }

            // Encode line endings
            var lineIndex = 0;
            foreach (var line in rawData)
            {
                line[line.Length - 1] |= (lineIndex >= rawData.Length - 1)
                    ? EndCommand
                    : (2 - lineIndex) << 6;
                lineIndex++;
            }

            // Concatenate and return result
            return rawData
                .SelectMany(d => d)
                .Select(b => unchecked((byte) b))
                .ToArray();
        }
    }
}
