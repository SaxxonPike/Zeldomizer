using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Moq;
using NUnit.Framework;
using AutoFixture;

namespace Testing
{
    /// <summary>
    /// Base test fixture for testing.
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    [Parallelizable(ParallelScope.Fixtures)]
    public abstract class BaseTestFixture
    {
        /// <summary>
        /// Initialize base test fixture.
        /// </summary>
        [SetUp]
        public void _InitializeBaseTestFixture()
        {
            _fixture = new Lazy<Fixture>(() => new Fixture());
            _mocks = new Dictionary<Type, object>();
        }

        /// <summary>
        /// Deferred AutoFixture factory.
        /// </summary>
        private Lazy<Fixture> _fixture;

        /// <summary>
        /// AutoFixture factory.
        /// </summary>
        protected Fixture Fixture => _fixture.Value;

        /// <summary>
        /// Mock repository.
        /// </summary>
        private Dictionary<Type, object> _mocks;

        /// <summary>
        /// Get a mock for specified type T. This will return the same mock for
        /// the specified type T on all requests.
        /// </summary>
        /// <typeparam name="T">Mocked type.</typeparam>
        /// <returns>Mock for type T.</returns>
        protected Mock<T> Mock<T>() where T : class
        {
            var mockType = typeof(T);
            if (!_mocks.ContainsKey(mockType))
                _mocks[mockType] = new Mock<T>();
            return (Mock<T>) _mocks[mockType];
        }

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
            var path = FileSystem.GetDesktopPath(fileName);
            FileSystem.WriteFile(data, path);
        }

        /// <summary>
        /// Write a file to a folder on the desktop.
        /// </summary>
        /// <param name="data">Data to write.</param>
        /// <param name="path">Path on the desktop.</param>
        /// <param name="fileName">Name of the file.</param>
        protected static void WriteToDesktopPath(byte[] data, string path, string fileName)
        {
            var folder = FileSystem.GetDesktopPath(path);
            FileSystem.CreateDirectory(folder);
            FileSystem.WriteFile(data, folder, fileName);
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
