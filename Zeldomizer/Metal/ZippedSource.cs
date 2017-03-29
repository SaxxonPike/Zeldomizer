using System;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace Zeldomizer.Metal
{
    public class ZippedSource : Source
    {
        public ZippedSource(byte[] zipBytes)
        {
            using (var zipStream = new MemoryStream(zipBytes))
            using (var archive = new ZipArchive(zipStream, ZipArchiveMode.Read, true))
            {
                var entry = archive.Entries.FirstOrDefault(e => e.Length == RomSize || e.Length == HeaderedRomSize);
                if (entry != null)
                {
                    using (var mem = new MemoryStream())
                    using (var entryStream = entry.Open())
                    {
                        entryStream.CopyTo(mem);
                        LoadRom(mem.ToArray());
                    }
                }
                else
                {
                    throw new Exception("Couldn't find appropriately sized file in archive.");
                }
            }
        }
    }
}
