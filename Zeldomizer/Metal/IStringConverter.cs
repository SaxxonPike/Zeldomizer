namespace Zeldomizer.Metal
{
    public interface IStringConverter
    {
        int GetLength(ISource source, int offset);
        string Decode(ISource source, int offset);
        byte[] Encode(string text);
    }
}