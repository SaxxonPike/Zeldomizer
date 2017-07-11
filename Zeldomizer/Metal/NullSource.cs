using System.Linq;

namespace Zeldomizer.Metal
{
    public class NullSource : ISource
    {
        public byte[] ExportRaw()
        {
            return new byte[0];
        }

        public byte this[int index]
        {
            get { return 0xFF; }
            set { }
        }

        public void Copy(int source, int destination, int length)
        {
        }

        public byte[] Read(int offset, int length) =>
            Enumerable.Range(0, length).Select(i => (byte)0xFF).ToArray();

        public void Write(byte[] source, int destination)
        {
        }

        public int Offset => 0;
        public int Length => int.MaxValue;
        public RomBlockType Type => RomBlockType.Unknown;
    }
}
