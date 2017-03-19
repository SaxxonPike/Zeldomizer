namespace Zeldomizer.Metal
{
    public interface IFixedStringConverter
    {
        string Decode(IRom source, int offset, int length);
        byte[] Encode(string text, int length);
    }
}
