using NUnit.Framework;

namespace Breadbox
{
    [Parallelizable(ParallelScope.Fixtures)]
    public abstract class OpcodeBaseTestFixture : BreadboxBaseTestFixture
    {
        private readonly int _opcode;

        protected OpcodeBaseTestFixture(int opcode)
        {
            _opcode = opcode;
        }

        [SetUp]
        public void SetupOpcode()
        {
            if (_opcode >= 0)
                Cpu.SetOpcode(_opcode);
        }
    }
}
