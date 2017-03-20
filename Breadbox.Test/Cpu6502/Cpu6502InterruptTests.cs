using NUnit.Framework;

namespace Breadbox.Test.Cpu6502
{
    [TestFixture]
    public class Cpu6502InterruptTests : Cpu6502BaseTestFixture
    {
        [Test]
        public void CpuCanRunOneCycle()
        {
            Cpu.Clock();
        }
    }
}
