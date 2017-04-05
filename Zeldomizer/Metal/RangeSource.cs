using System.Linq;

namespace Zeldomizer.Metal
{
    public class RangeSource : ISource
    {
        private readonly ISource _source;
        private readonly ISource _fallbackSource;

        public RangeSource(ISource source, int offset, int length, ISource fallbackSource)
        {
            Offset = offset;
            Length = length;
            _source = source;
            _fallbackSource = fallbackSource;
        }

        public byte[] ExportRaw() => 
            Read(Offset, Length);

        private ISource GetSourceForOffset(int offset)
        {
            if (offset >= Offset && offset < Offset + Length)
                return _source;
            return _fallbackSource;
        }

        public byte this[int index]
        {
            get => GetSourceForOffset(index)[index];
            set => GetSourceForOffset(index)[index] = value;
        }

        public void Copy(int source, int destination, int length) => 
            Write(Read(source, length), destination);

        public byte[] Read(int offset, int length) => 
            Enumerable.Range(offset, length).Select(i => this[i]).ToArray();

        public void Write(byte[] source, int destination)
        {
            var offset = destination;
            foreach (var datum in source)
                this[offset++] = datum;
        }

        public int Offset { get; }
        public int Length { get; }
        public RomBlockType Type => RomBlockType.Container;
    }
}
