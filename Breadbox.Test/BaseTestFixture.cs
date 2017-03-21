using System.Collections.Generic;
using System.Linq;
using Moq;
using Moq.Language;
using NUnit.Framework;

namespace Breadbox
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public abstract class BaseTestFixture
    {
        private Mos6502Configuration _config;
        protected Mock<IMemory> MemoryMock;
        protected Mock<IReadySignal> ReadySignalMock;
        protected Mock<IIrqSignal> IrqSignalMock;
        protected Mock<INmiSignal> NmiSignalMock;
        protected Mos6502 Cpu { get; private set; }

        [SetUp]
        public void Initialize()
        {
            SetUpMocks();

            var memory = MemoryMock != null ? MemoryMock.Object : new MemoryNull();
            var ready = ReadySignalMock != null ? ReadySignalMock.Object : new ReadySignalNull();
            var irq = IrqSignalMock != null ? IrqSignalMock.Object : new IrqSignalNull();
            var nmi = NmiSignalMock != null ? NmiSignalMock.Object : new NmiSignalNull();

            _config = new Mos6502Configuration(0xFF, true, memory, ready, irq, nmi);
            Cpu = new Mos6502(_config);
        }

        protected virtual Mos6502Configuration Config
        {
            get { return _config; }
        }

        protected virtual void SetUpMocks()
        {
        }

        protected IEnumerable<int> GetColdStartReadSequence(int address, params int[] sequence)
        {
            var coldStartSequence = new[] {0x00, 0x00, 0x00, address & 0xFF, (address >> 8) & 0xFF};
            return coldStartSequence.Concat(sequence);
        }

        protected ISetupSequentialResult<int> MockColdStartReadSequence(int address, params int[] sequence)
        {
            var reads = GetColdStartReadSequence(address, sequence);
            return reads.Aggregate(MemoryMock.SetupSequence(m => m.Read(It.IsAny<int>())),
                (seq, item) => seq.Returns(item));
        }
    }
}
