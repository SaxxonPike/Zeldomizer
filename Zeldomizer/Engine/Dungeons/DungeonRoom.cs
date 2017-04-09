using System.Linq;
using Zeldomizer.Metal;

namespace Zeldomizer.Engine.Dungeons
{
    public class DungeonRoom
    {
        private readonly ISource _source;

        public DungeonRoom(ISource source)
        {
            _source = source;
        }

        public int Color0
        {
            get => _source[0x000];
            set => _source[0x000] = unchecked((byte)value);
        }

        public int Color1
        {
            get => _source[0x080];
            set => _source[0x080] = unchecked((byte)value);
        }

        public int Monsters
        {
            get => _source[0x100];
            set => _source[0x100] = unchecked((byte)value);
        }

        public int Room
        {
            get => _source[0x180].Bits(6, 0);
            set => _source[0x180] = _source[0x180].Bits(6, 0, value);
        }

        public int FloorItem
        {
            get => _source[0x200];
            set => _source[0x200] = unchecked((byte)value);
        }

        public int SpecialItem
        {
            get => _source[0x280];
            set => _source[0x280] = unchecked((byte)value);
        }

        public override string ToString()
        {
            return string.Join(" ", Enumerable.Range(0, 6).Select(i => $"{_source[i * 0x080]:X2}"));
        }
    }
}
