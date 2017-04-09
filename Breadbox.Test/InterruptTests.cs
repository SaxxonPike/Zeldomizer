using NUnit.Framework;

namespace Breadbox
{
    [Parallelizable(ParallelScope.Fixtures)]
    public class InterruptTests : BreadboxBaseTestFixture
    {
        [Test]
        public void CpuCanRunOneCycle()
        {
            Cpu.Clock();
        }
    }
}
