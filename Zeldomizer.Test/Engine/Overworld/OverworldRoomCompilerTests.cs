using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using Zeldomizer.Metal;

namespace Zeldomizer.Engine.Overworld
{
    [Parallelizable(ParallelScope.Fixtures)]
    public class OverworldRoomCompilerTests : ZeldomizerBaseTestFixture<OverworldRoomCompiler>
    {
        protected override OverworldRoomCompiler GetTestSubject()
        {
            return new OverworldRoomCompiler();
        }

        [Test]
        public void Compile_CompilesOriginalDungeons()
        {
            // Load all data from the original cartridge
            var cart = new ZeldaCartridge(Source);
            var decompiler = new OverworldRoomDecompiler();
            var inRooms = decompiler.Decompile(cart.Overworld.ColumnLibraries, cart.Overworld.RoomLayouts);

            // Compile it
            var compiler = new OverworldRoomCompiler();
            var output = compiler.Compile(inRooms.Rooms);
            var columnOutput = output.ColumnData.ToArray();
            var roomOutput = output.RoomData.ToArray();
            var columnMem = new Source(columnOutput);
            var roomMem = new Source(roomOutput);

            // Read it out
            var outputLibraryList = new[] {new OverworldColumnLibrary(columnMem, output.ColumnOffsets.Count())};
            var outputRoomList = new OverworldRoomLayoutList(roomMem, roomOutput.Length >> 4);
            var decompiledOutput = decompiler.Decompile(outputLibraryList, outputRoomList);

            // Compare
            foreach (var room in inRooms.Rooms)
            {
                decompiledOutput.Rooms.Should().Contain(v => v.SequenceEqual(room));
            }
        }
    }
}
