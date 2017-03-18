using System;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace Zeldomizer.Test
{
    public static class RomFile
    {
        public static byte[] Get()
        {
            var thisType = typeof(RomFile);
            var thisAssembly = thisType.Assembly;
            byte[] result;

            using (var mem = new MemoryStream())
            {
                var source = thisAssembly.GetManifestResourceStream($"{thisType.Namespace}.zelda.zip");
                source.CopyTo(mem);
                result = mem.ToArray();
            }

            if (result.Length == 0x20000)
                return result;
            if (result.Length == 0x20010)
                return result.Skip(0x10).ToArray();
            if (result.Length < 2)
                throw new Exception("Bad ROM file.");
            if (result[0] == 0x50 && result[1] == 0x4B)
                return Unzip(result);
            throw new Exception("Bad ROM file.");
        }

        private static byte[] Unzip(byte[] data)
        {
            using (var stream = new MemoryStream(data))
            using (var archive = new ZipArchive(stream))
            {
                var entry = archive.Entries.FirstOrDefault(e => e.Length == 0x20000 || e.Length == 0x20010);
                if (entry != null)
                {
                    using (var mem = new MemoryStream())
                    using (var entryStream = entry.Open())
                    {
                        entryStream.CopyTo(mem);
                        return mem.ToArray();
                    }
                }
            }
            throw new Exception("Couldn't find appropriately sized file in archive.");
        }
    }
}
