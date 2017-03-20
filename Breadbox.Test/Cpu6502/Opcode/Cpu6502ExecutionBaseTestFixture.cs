using Moq;

namespace Breadbox.Test.Cpu6502.Opcode
{
    public abstract class Cpu6502ExecutionBaseTestFixture : Cpu6502BaseTestFixture
    {
        protected override void SetUpMocks()
        {
            base.SetUpMocks();
            MemoryMock = new Mock<IMemory>();
        }
    }
}
