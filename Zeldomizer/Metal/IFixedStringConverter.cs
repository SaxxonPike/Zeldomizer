namespace Zeldomizer.Metal
{
    public interface IFixedStringConverter
    {
        string Decode(ISource source, int offset, int length);
        byte[] Encode(string text, int length);
    }
}
