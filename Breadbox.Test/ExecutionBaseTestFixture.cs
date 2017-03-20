using Moq;

namespace Breadbox
{
    public abstract class ExecutionBaseTestFixture : BaseTestFixture
    {
        protected override void SetUpMocks()
        {
            base.SetUpMocks();
            MemoryMock = new Mock<IMemory>();
        }
    }
}
