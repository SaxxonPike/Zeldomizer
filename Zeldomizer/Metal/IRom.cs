namespace Zeldomizer.Metal
{
    public interface IRom : Disaster.Assembly.Interfaces.IRom, IRawExportable
    {
        void Copy(int source, int destination, int length);
        byte[] Read(int offset, int length);
        void Write(byte[] source, int destination);
        int Offset { get; }
        int Length { get; }
        RomBlockType Type { get; }
    }
}