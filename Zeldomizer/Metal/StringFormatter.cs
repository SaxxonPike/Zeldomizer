using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeldomizer.Metal
{
    public class StringFormatter : IStringFormatter
    {
        private const int TextWidth = 24;

        public string Format(string input)
        {
            // Identify words from the input.
            var words = input
                .Split(new[] {Environment.NewLine}, StringSplitOptions.None)
                .SelectMany(s => s.Split(new[] {' '}, StringSplitOptions.None))
                .Select(s => s.Trim());
            var output = new List<StringBuilder>();
            var builder = new StringBuilder();

            // Perform word wrap.
            foreach (var word in words)
            {
                // Account for a space, if needed.
                var wordLength = word.Length;
                if (builder.Length > 0)
                    wordLength++;

                // If the word is too long, add new line.
                if (builder.Length + wordLength > TextWidth)
                {
                    output.Add(builder);
                    builder = new StringBuilder();
                }

                // Only add the space if other words are present on this line.
                if (builder.Length > 0)
                    builder.Append(" ");
                builder.Append(word);
            }

            // Our final line needs to be added to the output.
            if (builder.Length > 0)
                output.Add(builder);

            // Center the lines.
            var centeredLines = output
                .Select(sb => sb.ToString())
                .Select(s =>
                {
                    var spacesNeeded = TextWidth - s.Length;
                    return $"{new string(' ', spacesNeeded / 2)}{s}";
                })
                .ToArray();

            // Concatenate the resulting lines.
            return string.Join(Environment.NewLine, centeredLines);
        }

        public string UnFormat(string input)
        {
            var decodedText = input.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
            var trimmedText = decodedText.Select(s => s.Trim());
            return string.Join(" ", trimmedText);
        }
    }
}
