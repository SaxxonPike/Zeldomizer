using System.Linq;
using NUnit.Framework;

namespace Zeldomizer
{
    public class TestPlayground : BaseTestFixture
    {
        [Test]
        public void Test1()
        {
            var cart = new ZeldaCartridge(Rom);
            var rooms = cart.Dungeons.Rooms.ToArray();
        }

    }
}
