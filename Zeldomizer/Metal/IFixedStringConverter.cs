namespace Zeldomizer.Metal
{
    public interface IFixedStringConverter
    {
        string Decode(ISource source, int length);
        byte[] Encode(string text, int length);
        int SpaceCharacter { get; }
    }
}
