using NUnit.Framework;

namespace Zeldomizer.Test
{
    [TestFixture]
    public abstract class BaseTestFixture
    {
        [SetUp]
        public void _InitializeRom()
        {
            Rom = RomFile.Get();
        }

        protected byte[] Rom { get; private set; }
    }

    public abstract class BaseTestFixture<TSubject> : BaseTestFixture
    {
        [SetUp]
        public void _InitializeTestSubject()
        {
            Subject = GetTestSubject();
        }

        public TSubject Subject { get; private set; }

        protected abstract TSubject GetTestSubject();
    }
}
