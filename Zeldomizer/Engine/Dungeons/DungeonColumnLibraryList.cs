using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Zeldomizer.Metal;

namespace Zeldomizer.Engine.Dungeons
{
    public class DungeonColumnLibraryList : IEnumerable<DungeonColumnLibrary>
    {
        private readonly IRom _rom;
        private readonly int _offsetAdjust;

        // ReSharper disable once CollectionNeverUpdated.Local
        private readonly WordList _libraryOffsetList;

        public DungeonColumnLibraryList(IRom rom, int offset, int offsetAdjust, int count)
        {
            _rom = rom;
            _offsetAdjust = offsetAdjust;
            _libraryOffsetList = new WordList(_rom, offset, count);
        }

        public DungeonColumnLibrary this[int index] =>
            new DungeonColumnLibrary(_rom, _libraryOffsetList[index] + _offsetAdjust, 16);

        private IEnumerable<DungeonColumnLibrary> GetLibraries()
        {
            return _libraryOffsetList
                .Select(w => new DungeonColumnLibrary(_rom, w + _offsetAdjust, 16));
        }

        public IEnumerator<DungeonColumnLibrary> GetEnumerator()
        {
            return GetLibraries().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
