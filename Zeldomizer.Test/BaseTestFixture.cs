using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using NUnit.Framework;
using Ploeh.AutoFixture;
using Zeldomizer.Metal;

namespace Zeldomizer
{
    /// <summary>
    /// Base test fixture for testing.
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    [Parallelizable(ParallelScope.Fixtures)]
    public abstract class BaseTestFixture
    {
        [SetUp]
        public void _InitializeRom()
        {
            Fixture = new Fixture();
            _source = null;
        }

        private ISource _source;

        /// <summary>
        /// Zelda ROM file.
        /// </summary>
        protected ISource Source => _source ?? (_source = RomFile.GetRom());

        /// <summary>
        /// AutoFixture factory.
        /// </summary>
        protected Fixture Fixture { get; private set; }

        /// <summary>
        /// Generate a random value for type T.
        /// </summary>
        /// <typeparam name="T">Type to generate the random value for.</typeparam>
        /// <returns>Random value created by AutoFixture.</returns>
        protected T Random<T>()
        {
            return Fixture.Create<T>();
        }

        /// <summary>
        /// Generate multiple random values of type T.
        /// </summary>
        /// <typeparam name="T">Type to generate random values for.</typeparam>
        /// <returns>Random values created by AutoFixture.</returns>
        protected IEnumerable<T> ManyRandom<T>()
        {
            return Fixture.CreateMany<T>();
        }

        /// <summary>
        /// Write a file to the desktop.
        /// </summary>
        /// <param name="data">Data to write.</param>
        /// <param name="fileName">Name of the file.</param>
        protected static void WriteToDesktop(byte[] data, string fileName)
        {
            var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), fileName);
            File.WriteAllBytes(path, data);
        }

        /// <summary>
        /// Write a file to a folder on the desktop.
        /// </summary>
        /// <param name="data">Data to write.</param>
        /// <param name="path">Path on the desktop.</param>
        /// <param name="fileName">Name of the file.</param>
        protected static void WriteToDesktopPath(byte[] data, string path, string fileName)
        {
            var folder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), path);
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);
            WriteToDesktop(data, Path.Combine(path, fileName));
        }

        /// <summary>
        /// Newline string, environment specific.
        /// </summary>
        protected string NewLine => Environment.NewLine;
    }

    /// <summary>
    /// Typed base test fixture for testing.
    /// </summary>
    /// <typeparam name="TSubject">Type being tested.</typeparam>
    [ExcludeFromCodeCoverage]
    public abstract class BaseTestFixture<TSubject> : BaseTestFixture
    {
        [SetUp]
        public void _InitializeTestSubject()
        {
            Subject = GetTestSubject();
        }

        /// <summary>
        /// Subject being tested.
        /// </summary>
        public TSubject Subject { get; private set; }

        /// <summary>
        /// Create the test subject.
        /// </summary>
        /// <returns>Test subject.</returns>
        protected abstract TSubject GetTestSubject();
    }
}
