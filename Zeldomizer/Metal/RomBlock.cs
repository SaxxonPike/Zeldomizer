namespace Zeldomizer.Metal
{
    public class RomBlock : IRom
    {
        public RomBlock(IRom parent, int offset)
        {
            Parent = parent;
            Offset = offset;
        }

        public IRom Parent { get; }
        public int Offset { get; }
        public int Length { get; set; }
        public RomBlockType Type { get; set; }

        public byte this[int index]
        {
            get { return Parent[index + Offset + Parent.Offset]; }
            set { Parent[index + Offset + Parent.Offset] = value; }
        }

        public void Copy(int source, int destination, int length) =>
            Parent.Copy(source + Offset + Parent.Offset, destination + Offset + Parent.Offset, length);

        public byte[] Read(int offset, int length) =>
            Parent.Read(offset + Offset + Parent.Offset, length);

        public void Write(byte[] source, int destination) =>
            Parent.Write(source, destination + Offset + Parent.Offset);

        public byte[] ExportRaw() => 
            Parent.Read(Offset + Parent.Offset, Length > 0 ? Length : 0);
    }
}
