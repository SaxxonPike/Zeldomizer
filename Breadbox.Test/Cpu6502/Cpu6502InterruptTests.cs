using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace Breadbox.Test.Cpu6502
{
    [TestFixture]
    [Parallelizable(ParallelScope.Self)]
    public class Cpu6502InterruptTests : Cpu6502BaseTestFixture
    {
        [Test]
        public void CpuCanRunOneCycle()
        {
            Cpu.Clock();
        }
    }
}
