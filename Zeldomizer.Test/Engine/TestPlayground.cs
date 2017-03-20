using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using Zeldomizer.Metal;
using Zeldomizer.Metal.Cpu;

namespace Zeldomizer.Engine
{
    public class TestPlayground : BaseTestFixture
    {
        [Test]
        [Explicit]
        public void Test1()
        {
            var codeBlock = new CodeBlock
            {
                AnalysisHintAddresses = new [] { 0x9825 },
                Offset = 0x0000,
                Length = 0x4000,
                Origin = 0x8000,
                Rom = Rom
            };

            var disassembler = new Disassembler();
            var analyzer = new StaticAnalyzer(disassembler);
            var analysis = analyzer.Analyze(codeBlock);
        }
    }
}
