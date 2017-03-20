using NUnit.Framework;

namespace Breadbox
{
    [TestFixture]
    public class InterruptTests : BaseTestFixture
    {
        [Test]
        public void CpuCanRunOneCycle()
        {
            Cpu.Clock();
        }
    }
}
