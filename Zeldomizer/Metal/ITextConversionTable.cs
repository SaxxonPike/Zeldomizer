namespace Zeldomizer.Metal
{
    /// <summary>
    /// Text conversion table.
    /// </summary>
    public interface ITextConversionTable
    {
        /// <summary>
        /// Encode a character to the Zelda character set.
        /// </summary>
        int? Encode(char input);

        /// <summary>
        /// Decode a character from the Zelda character set.
        /// </summary>
        char? Decode(int input);

        /// <summary>
        /// Space character in the Zelda character set.
        /// </summary>
        int SpaceCharacter { get; }

        /// <summary>
        /// Space character that does not force a delay when displayed, in the Zelda character set.
        /// </summary>
        int PaddingCharacter { get; }
    }
}