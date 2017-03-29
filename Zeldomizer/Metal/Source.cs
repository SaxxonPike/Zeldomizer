using System;
using System.Collections.Generic;
using System.Linq;

namespace Zeldomizer.Metal
{
    public class Source : ISource
    {
        protected byte[] Data { get; }

        public Source(IEnumerable<byte> data)
        {
            Data = data.ToArray();
        }

        public Source(IEnumerable<int> data)
        {
            Data = data.Select(d => unchecked((byte) d)).ToArray();
        }

        public Source(int size)
        {
            Data = new byte[size];
        }

        public byte this[int index]
        {
            get { return Data[index]; }
            set { Data[index] = value; }
        }

        public void Copy(int source, int destination, int length)
        {
            Array.Copy(Data, source, Data, destination, length);
        }

        public byte[] Read(int offset, int length)
        {
            var result = new byte[length];
            Array.Copy(Data, offset, result, 0, result.Length);
            return result;
        }

        public void Write(byte[] source, int destination)
        {
            Array.Copy(source, 0, Data, destination, source.Length);
        }

        public byte[] ExportRaw()
        {
            return Data.ToArray();
        }

        public int Offset => 0;
        public int Length => Data.Length;
        public RomBlockType Type => RomBlockType.Container;
    }
}
