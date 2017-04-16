using System.Collections.Generic;
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

        public IList<int> Palette0 =>
            new ByteList(new SourceBlock(_source, 0x00), 4);
        public IList<int> Palette1 =>
            new ByteList(new SourceBlock(_source, 0x04), 4);
        public IList<int> Palette2 =>
            new ByteList(new SourceBlock(_source, 0x08), 4);
        public IList<int> Palette3 =>
            new ByteList(new SourceBlock(_source, 0x0C), 4);
        public IList<int> EnemyPalette0 =>
            new ByteList(new SourceBlock(_source, 0x10), 4);
        public IList<int> EnemyPalette1 =>
            new ByteList(new SourceBlock(_source, 0x14), 4);
        public IList<int> EnemyPalette2 =>
            new ByteList(new SourceBlock(_source, 0x18), 4);
        public IList<int> EnemyPalette3 =>
            new ByteList(new SourceBlock(_source, 0x1C), 4);

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

        public IList<int> PaletteStairFadeOut =>
            new ByteList(new SourceBlock(_source, 0x79), 32);
        public IList<int> PaletteStairFadeIn =>
            new ByteList(new SourceBlock(_source, 0x99), 32);
        public IList<int> PaletteDarkFade =>
            new ByteList(new SourceBlock(_source, 0xB9), 32);
        public IList<int> PaletteDeathFade =>
            new ByteList(new SourceBlock(_source, 0xD9), 32);
    }
}
