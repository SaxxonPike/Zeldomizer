using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace Testing
{
    /// <summary>
    /// Provides test resources.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class TestResourceProvider
    {
        /// <summary>
        /// Get a resource from the this assembly.
        /// </summary>
        /// <param name="name">Name of the resource.</param>
        /// <returns>Resource data.</returns>
        private static byte[] GetResource(string name)
        {
            var thisType = typeof(TestResourceProvider);
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
        /// Get the test data ZIP file.
        /// </summary>
        public static byte[] GetTestDataZip() => GetResource("TestData.zip");
    }
}
