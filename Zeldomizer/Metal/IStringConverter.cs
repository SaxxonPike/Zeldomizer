namespace Zeldomizer.Metal
{
    public interface IStringConverter
    {
        int GetLength(ISource source);
        string Decode(ISource source);
        byte[] Encode(string text);
    }
}