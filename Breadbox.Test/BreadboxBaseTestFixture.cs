using System.Collections.Generic;
using System.Linq;
using Moq;
using Moq.Language;
using NUnit.Framework;
using Testing;

namespace Breadbox
{
    [Parallelizable(ParallelScope.Fixtures)]
    public abstract class BreadboxBaseTestFixture : BaseTestFixture
    {
        private Mos6502Configuration _config;
        protected Mos6502 Cpu { get; private set; }

        [SetUp]
        public void Initialize()
        {
            System.Setup(x => x.Ready).Returns(true);
            System.Setup(x => x.Nmi).Returns(false);
            System.Setup(x => x.Irq).Returns(false);
            System.Setup(x => x.Read(It.IsAny<int>())).Returns(0xFF);
            System.Setup(x => x.Write(It.IsAny<int>(), It.IsAny<int>()));

            _config = new Mos6502Configuration(0xFF, true, System.Object.Read, System.Object.Write, () => System.Object.Ready, () => System.Object.Irq, () => System.Object.Nmi);
            Cpu = new Mos6502(_config);
        }

        protected virtual Mos6502Configuration Config => _config;

        protected Mock<ISystem> System => Mock<ISystem>();

        protected IEnumerable<int> GetColdStartReadSequence(int address, params int[] sequence)
        {
            var coldStartSequence = new[] { 0x00, 0x00, 0x00, address & 0xFF, (address >> 8) & 0xFF };
            return coldStartSequence.Concat(sequence);
        }

        protected ISetupSequentialResult<int> MockColdStartReadSequence(int address, params int[] sequence)
        {
            var reads = GetColdStartReadSequence(address, sequence);
            return reads.Aggregate(System.SetupSequence(m => m.Read(It.IsAny<int>())),
                (seq, item) => seq.Returns(item));
        }
    }
}

