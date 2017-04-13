using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using Zeldomizer.Metal;

namespace Zeldomizer.Engine.Underworld
{
    [Parallelizable(ParallelScope.Fixtures)]
    public class UnderworldRoomCompilerTests : ZeldomizerBaseTestFixture<UnderworldRoomCompiler>
    {
        protected override UnderworldRoomCompiler GetTestSubject()
        {
            return new UnderworldRoomCompiler();
        }

        [Test]
        public void Compile_CompilesOriginalDungeons()
        {
            var expected = new[]
            {
                "1111111", "0111110", "1111101", "0004000",
                "0001000", "0001111", "0011100", "0011101",
                "1014101", "1010101", "1011101", "1000001",
                "1000101", "1040101", "0011111", "0111111",
                "1111110", "0111011", "1100011", "1101011",
                "0000001", "1011111", "1111011", "1101110",
                "1110111", "1111611", "1101111", "1111010",
                "1011100", "0077211", "1177177", "1661111",
                "1111711", "1161111", "7111177", "1013101",
                "1012101", "0100113", "3777773", "3111113",
                "1111113", "0010011", "1101100", "0077311",
                "0000011", "0000000", "6666666", "1666661",
                "6666616", "1616161", "6161616", "1611161",
                "6166616", "6111116", "6166666", "1111112",
                "6661666", "1666666", "1661166", "7777777",
                "5555555", "1001001", "1777111", "1116111",
                "0100112", "2777772", "2111112", "0100111",
                "1112111", "1113111", "0777111", "1021101",
                "1031101", "1212121", "1212125", "1111155",
                "1111555", "1313131", "1313135"
            };

            // Load all data from the original cartridge
            var decompiler = new UnderworldRoomDecompiler();
            var inColumnPointerTable = new WordPointerTable(new SourceBlock(Source, 0x16704), new SourceBlock(Source, 0xC000), 10);
            var inColumns = new UnderworldColumnLibraryList(inColumnPointerTable);
            var inRoomList = new UnderworldRoomLayoutList(new SourceBlock(Source, 0x160DE), 42);
            var inRooms = decompiler.Decompile(inColumns, inRoomList);

            // Compile it
            var compiler = new UnderworldRoomCompiler();
            var output = compiler.Compile(inRooms.Rooms);
            var columnOutput = output.Columns.ToArray();
            var mem = new Source(columnOutput);

            // Read it out
            var library = new UnderworldColumnLibrary(mem, output.ColumnOffsets.Count());
            var columns = library.Select(dc => string.Join(string.Empty, dc.Select(t => $"{t:X1}")));
            columns.ShouldAllBeEquivalentTo(expected);
        }
    }
}
