using System.Collections.Generic;
using Zeldomizer.Engine.Graphics;
using Zeldomizer.Metal;

namespace Zeldomizer.Engine.Underworld
{
    public class UnderworldLevel
    {
        private readonly ISource _source;

        public UnderworldLevel(ISource source)
        {
            _source = source;
        }

        public PaletteList RoomPalette =>
            new PaletteList(new SourceBlock(_source, 0x00), 4);
        public PaletteList EnemyPalette =>
            new PaletteList(new SourceBlock(_source, 0x10), 4);

        public IList<int> ItemTiles =>
            new ByteList(new SourceBlock(_source, 0x26), 4);

        public int DrawnMapDataShift
        {
            get => _source[0x2A];
            set => _source[0x2A] = unchecked((byte) value);
        }

        public int ItemMapCursorShift
        {
            get => _source[0x2B];
            set => _source[0x2B] = unchecked((byte)value);
        }

        public int EntranceRoom
        {
            get => _source[0x2C];
            set => _source[0x2C] = unchecked((byte)value);
        }

        public int CompassTargetRoom
        {
            get => _source[0x2D];
            set => _source[0x2D] = unchecked((byte)value);
        }

        public int LevelNumber
        {
            get => _source[0x30];
            set => _source[0x30] = unchecked((byte)value);
        }

        public IList<int> StairwayData =>
            new ByteList(new SourceBlock(_source, 0x31), 10);

        public int BossRoom
        {
            get => _source[0x3B];
            set => _source[0x3B] = unchecked((byte)value);
        }

        public IList<int> DrawnMapData =>
            new ByteList(new SourceBlock(_source, 0x3C), 16);
        public IList<int> DungeonMapDisplay =>
            new ByteList(new SourceBlock(_source, 0x4C), 45);

        public UnderworldFade DownstairsPaletteFade =>
            new UnderworldFade(new SourceBlock(_source, 0x79));
        public UnderworldFade UpstairsPaletteFade =>
            new UnderworldFade(new SourceBlock(_source, 0x99));
        public UnderworldFade DarkRoomPaletteFade =>
            new UnderworldFade(new SourceBlock(_source, 0xB9));
        public UnderworldFade DeathPaletteFade =>
            new UnderworldFade(new SourceBlock(_source, 0xD9));
    }
}
