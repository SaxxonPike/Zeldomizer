using System;
using System.IO;
using Zeldomizer.Metal;

namespace Zeldomizer
{
    /// <summary>
    /// Helper class for getting the ROM out of the test resources.
    /// </summary>
    public static class RomFile
    {
        /// <summary>
        /// Get a resource from the this assembly.
        /// </summary>
        /// <param name="name">Name of the resource.</param>
        /// <returns>Resource data.</returns>
        private static byte[] GetResource(string name)
        {
            var thisType = typeof(RomFile);
            var thisAssembly = thisType.Assembly;

            using (var mem = new MemoryStream())
            {
                var source = thisAssembly
                    .GetManifestResourceStream($"{thisType.Namespace}.{name}");
                if (source == null)
                    throw new Exception($"Can't find resource {name}");
                source.CopyTo(mem);
                return mem.ToArray();
            }
        }

        /// <summary>
        /// Get the test ROM.
        /// </summary>
        /// <returns>Test ROM.</returns>
        public static IRom GetRom()
        {
            var resource = GetResource("TestData.zip");
            if (resource.Length == 0x20000 || resource.Length == 0x20010)
                return new Rom(resource);
            if (resource.Length < 2)
                throw new Exception("Bad ROM file.");
            if (resource[0] == 0x50 && resource[1] == 0x4B)
                return new ZippedRom(resource);
            throw new Exception("Bad ROM file.");
        }
    }
}
