using System;
using System.IO;
using System.Linq;

namespace Zeldomizer.Metal
{
    public class RomSource : Source, IExportable
    {
        protected const int RomSize = 0x20000;
        private const int HeaderSize = 0x10;
        protected const int HeaderedRomSize = RomSize + HeaderSize;

        protected RomSource() : base(RomSize)
        {
        }

        public RomSource(byte[] data) : this()
        {
            LoadRom(data);
        }

        protected void LoadRom(byte[] data)
        {
            byte[] input;
            if (data.Length == HeaderedRomSize)
                input = data.Skip(HeaderSize).ToArray();
            else if (Data.Length == RomSize)
                input = data;
            else
                throw new Exception("Rom is not 128k in size, headered or unheadered.");
            Array.Copy(input, Data, RomSize);
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
                mem.Write(Data, 0, Data.Length);
                mem.Flush();
                return mem.ToArray();
            }
        }
    }
}
