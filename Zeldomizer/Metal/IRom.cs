namespace Zeldomizer.Metal
{
    public interface IRom
    {
        byte this[int index] { get; set; }

        void Copy(int source, int destination, int length);
        byte[] Read(int offset, int length);
        void Write(byte[] source, int destination);
        byte[] Export();
    }
}