using System;
using System.IO;
using System.Linq;

namespace Testing
{
    /// <summary>
    /// Provides access to the file system.
    /// </summary>
    public static class FileSystem
    {
        /// <summary>
        /// Combine paths.
        /// </summary>
        /// <param name="path">Paths to combine.</param>
        private static string CombinePaths(params string[] path) =>
            Path.Combine(path);

        /// <summary>
        /// Create a directory, if it does not already exist.
        /// </summary>
        /// <param name="path">Path of the directory to create.</param>
        public static void CreateDirectory(params string[] path)
        {
            if (!Directory.Exists(CombinePaths(path)))
                Directory.CreateDirectory(CombinePaths(path));
        }

        /// <summary>
        /// Get the desktop path with the optional relative paths.
        /// </summary>
        /// <param name="path">Relative path.</param>
        public static string GetDesktopPath(params string[] path) =>
            CombinePaths(new[] { Environment.GetFolderPath(Environment.SpecialFolder.Desktop) }
                .Concat(path)
                .ToArray());

        /// <summary>
        /// Write data to the specified file.
        /// </summary>
        /// <param name="bytes">Data to write.</param>
        /// <param name="path">Path to the file.</param>
        public static void WriteFile(byte[] bytes, params string[] path) => 
            File.WriteAllBytes(CombinePaths(path), bytes);
    }
}
