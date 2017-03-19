namespace Zeldomizer.Metal
{
    public interface IStringConverter
    {
        int GetLength(IRom source, int offset);
        string Decode(IRom source, int offset);
        byte[] Encode(string text);
    }
}