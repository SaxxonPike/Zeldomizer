using NUnit.Framework;

namespace Breadbox
{
    public abstract class OpcodeBaseTestFixture : ExecutionBaseTestFixture
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
