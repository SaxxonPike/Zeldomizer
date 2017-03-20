using Moq;
using NUnit.Framework;

namespace Breadbox.Test.Cpu6502.Opcode
{
    public abstract class Cpu6502OpcodeBaseTestFixture : Cpu6502ExecutionBaseTestFixture
    {
        private readonly int _opcode;

        protected Cpu6502OpcodeBaseTestFixture(int opcode)
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
