namespace Zeldomizer.Metal
{
    public class SourceBlock : ISource
    {
        public SourceBlock(ISource parent, int offset)
        {
            Parent = parent;
            Offset = offset;
        }

        public ISource Parent { get; }
        public int Offset { get; }
        public int Length { get; set; }
        public RomBlockType Type { get; set; }

        public byte this[int index]
        {
            get { return Parent[index + Offset]; }
            set { Parent[index + Offset] = value; }
        }

        public void Copy(int source, int destination, int length) =>
            Parent.Copy(source + Offset, destination + Offset, length);

        public byte[] Read(int offset, int length) =>
            Parent.Read(offset + Offset, length);

        public void Write(byte[] source, int destination) =>
            Parent.Write(source, destination + Offset);

        public byte[] ExportRaw() => 
            Parent.Read(Offset, Length > 0 ? Length : 0);
    }
}
