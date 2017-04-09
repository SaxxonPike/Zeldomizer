using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using Zeldomizer.Metal;

// ReSharper disable CollectionNeverUpdated.Local

namespace Zeldomizer.Engine.Dungeons
{
    public class DungeonDecompilerTests : ZeldomizerBaseTestFixture<DungeonDecompiler>
    {
        protected override DungeonDecompiler GetTestSubject()
        {
            return new DungeonDecompiler();
        }

        [Test]
        public void Test1()
        {
            var cart = new ZeldaCartridge(Source);
            var dungeonGrids = cart.DungeonGrids.ToArray();

            dungeonGrids[0][0x00].Room.Should().Be(0x26);
        }
    }
}
