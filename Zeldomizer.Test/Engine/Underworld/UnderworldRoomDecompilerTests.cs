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
            var decompiled = Subject.Decompile(cart.Overworld.ColumnLibraries, cart.Overworld.RoomLayouts);
        }
    }
}
