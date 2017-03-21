using System;
using System.IO;
using System.Linq;

namespace Zeldomizer.Metal
{
    public class Rom : IRom
    {
        private readonly byte[] _data;

        protected const int RomSize = 0x20000;
        private const int HeaderSize = 0x10;
        protected const int HeaderedRomSize = RomSize + HeaderSize;

        public Rom()
        {
            _data = new byte[RomSize];
        }

        public Rom(byte[] data) : this()
        {
            LoadRom(data);
        }

        protected void LoadRom(byte[] data)
        {
            byte[] input;
            if (data.Length == HeaderedRomSize)
                input = data.Skip(HeaderSize).ToArray();
            else if (_data.Length == RomSize)
                input = data;
            else
                throw new Exception("Rom is not 128k in size, headered or unheadered.");
            Array.Copy(input, _data, RomSize);
        }

        public byte this[int index]
        {
            get { return _data[index]; }
            set { _data[index] = value; }
        }

        public void Copy(int source, int destination, int length)
        {
            Array.Copy(_data, source, _data, destination, length);
        }

        public byte[] Read(int offset, int length)
        {
            var result = new byte[length];
            Array.Copy(_data, offset, result, 0, result.Length);
            return result;
        }

        public void Write(byte[] source, int destination)
        {
            Array.Copy(source, 0, _data, destination, source.Length);
        }

        /// <summary>
        /// Export a ROM file for use in an emulator.
        /// </summary>
        /// <returns>ROM file with header.</returns>
        public byte[] Export()
        {
            var emuData = new byte[]
            {
                0x4E, 0x45, 0x53, 0x1A,
                0x08, 0x00, 0x12, 0x00,
                0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00
            };

            using (var mem = new MemoryStream())
            {
                mem.Write(emuData, 0, emuData.Length);
                mem.Write(_data, 0, _data.Length);
                mem.Flush();
                return mem.ToArray();
            }
        }

        public byte[] ExportRaw()
        {
            return _data.ToArray();
        }
    }
}
