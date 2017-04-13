using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using Zeldomizer.Engine.Underworld;
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
            var decompiler = new OverworldRoomDecompiler();
            var inColumnPointerTable = new WordPointerTable(new SourceBlock(Source, 0x16704), new SourceBlock(Source, 0xC000), 10);
            var inColumns = new UnderworldColumnLibraryList(inColumnPointerTable).ToArray();
            var inRoomList = new UnderworldRoomLayoutList(new SourceBlock(Source, 0x160DE), 42);
            var inRooms = decompiler.Decompile(inColumns, inRoomList);

            // Compile it
            var compiler = new UnderworldRoomCompiler();
            var output = compiler.Compile(inRooms.Rooms);
            var columnOutput = output.Columns.ToArray();
            var mem = new Source(columnOutput);

            // Read it out
            var library = new UnderworldColumnLibrary(mem, output.ColumnOffsets.Count());
            var columns = library.Select(dc => string.Join(string.Empty, dc.Select(t => $"{t:X1}"))).ToArray();
        }
    }
}
