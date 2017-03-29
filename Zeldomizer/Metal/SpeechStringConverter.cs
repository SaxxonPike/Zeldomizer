using System;
using System.Linq;
using System.Text;

namespace Zeldomizer.Metal
{
    public class SpeechStringConverter : IStringConverter
    {
        private readonly ITextConversionTable _textConversionTable;

        public SpeechStringConverter(ITextConversionTable textConversionTable)
        {
            _textConversionTable = textConversionTable;
        }

        private const int FastSpace = 0x25;
        private const int UnknownCharacter = 0x24;
        private const int EndCommand = 0xC0;

        private int Encode(char input)
        {
            return _textConversionTable.Encode(input) ?? UnknownCharacter;
        }

        private char Decode(int input)
        {
            return _textConversionTable.Decode(input) ?? ' ';
        }

        public int GetLength(ISource source)
        {
            var i = 0;
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

        public string Decode(ISource source)
        {
            var i = 0;
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
                var decoded = Decode(character);

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
                .Select(s => s.Select(Encode).ToArray())
                .ToArray();

            // Interpret the spaces around each line as fast spaces
            var encodedSpace = Encode(' ');
            foreach (var line in rawData)
            {
                for (var i = 0; i < line.Length; i++)
                {
                    if (line[i] != encodedSpace)
                        break;
                    line[i] = FastSpace;
                }
                for (var i = line.Length - 1; i >= 0; i--)
                {
                    if (line[i] != encodedSpace)
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
