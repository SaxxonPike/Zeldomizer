using System;
using System.Diagnostics.CodeAnalysis;
using Testing;
using Zeldomizer.Metal;

namespace Zeldomizer
{
    /// <summary>
    /// Helper class for getting the ROM out of the test resources.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class RomFile
    {
        /// <summary>
        /// Get the test ROM.
        /// </summary>
        /// <returns>Test ROM.</returns>
        public static ISource GetRom()
        {
            var resource = TestResourceProvider.GetTestDataZip();
            if (resource.Length == 0x20000 || resource.Length == 0x20010)
                return new RomSource(resource);
            if (resource.Length < 2)
                throw new Exception("Bad ROM file.");
            if (resource[0] == 0x50 && resource[1] == 0x4B)
                return new ZippedRomSource(resource);
            throw new Exception("Bad ROM file.");
        }
    }
}
