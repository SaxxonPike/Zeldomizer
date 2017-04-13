using System.Linq;
using FluentAssertions;
using NUnit.Framework;

// ReSharper disable CollectionNeverUpdated.Local

namespace Zeldomizer.Engine.Underworld
{
    public class UnderworldRoomDecompilerTests : ZeldomizerBaseTestFixture<UnderworldRoomDecompiler>
    {
        protected override UnderworldRoomDecompiler GetTestSubject()
        {
            return new UnderworldRoomDecompiler();
        }

        [Test]
        public void Test1()
        {
            var cart = new ZeldaCartridge(Source);
            var dungeonGrids = cart.UnderworldGrids.ToArray();

            dungeonGrids[0][0x00].Layout.Should().Be(0x26);
        }
    }
}
