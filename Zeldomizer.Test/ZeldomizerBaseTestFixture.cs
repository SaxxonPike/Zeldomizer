using System;
using System.Diagnostics.CodeAnalysis;
using NUnit.Framework;
using Testing;
using Zeldomizer.Metal;

namespace Zeldomizer
{
    /// <summary>
    /// Base test fixture for testing.
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    [Parallelizable(ParallelScope.Fixtures)]
    public abstract class ZeldomizerBaseTestFixture : BaseTestFixture
    {
        [SetUp]
        public void _InitializeRom()
        {
            _source = new Lazy<ISource>(RomFile.GetRom);
        }

        private Lazy<ISource> _source;

        /// <summary>
        /// Zelda ROM file.
        /// </summary>
        protected ISource Source => _source.Value;

        /// <summary>
        /// Export ROM file.
        /// </summary>
        protected byte[] ExportHeaderedRom()
        {
            if (Source is IExportable exportable)
                return exportable.Export();

            throw new Exception("Base source type is not exportable with header!");
        }
    }

    /// <summary>
    /// Typed base test fixture for testing.
    /// </summary>
    /// <typeparam name="TSubject">Type being tested.</typeparam>
    [ExcludeFromCodeCoverage]
    [Parallelizable(ParallelScope.Fixtures)]
    public abstract class ZeldomizerBaseTestFixture<TSubject> : ZeldomizerBaseTestFixture
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
