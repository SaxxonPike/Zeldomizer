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
            var columnPointerTable = new WordPointerTable(new SourceBlock(Source, 0x16704), new SourceBlock(Source, 0xC000), 10);
            var columns = new DungeonColumnLibraryList(columnPointerTable);
            var roomList = new DungeonRoomLayoutList(new SourceBlock(Source, 0x160DE), 42);
            var tileList = new DungeonTileList(new SourceBlock(Source, 0x16718));

            var observed = Subject.Decompile(columns, roomList);
        }
    }
}
