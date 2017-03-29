namespace Zeldomizer.Metal
{
    public interface ITextConversionTable
    {
        int? Encode(char input);
        char? Decode(int input);
    }
}