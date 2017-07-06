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
        public Palette LinkPalette =>
            new Palette(new SourceBlock(_source, 0x10));
        public PaletteList EnemyPalette =>
            new PaletteList(new SourceBlock(_source, 0x14), 3);

        public IList<int> ItemTiles =>
            new ByteList(new SourceBlock(_source, 0x26), 4);

        public ICoordinate DrawnMapDataShift =>
            new Coordinate(new SourceBlock(_source, 0x2A), 16, 8);

        public ICoordinate ItemMapCursorShift =>
            new Coordinate(new SourceBlock(_source, 0x2B), 16, 8);

        public ICoordinate EntranceRoom =>
            new Coordinate(new SourceBlock(_source, 0x2C), 16, 8);

        public ICoordinate CompassTargetRoom =>
            new Coordinate(new SourceBlock(_source, 0x2D), 16, 8);

        public int LevelNumber
        {
            get => _source[0x30];
            set => _source[0x30] = unchecked((byte)value);
        }

        public IList<ICoordinate> StairwayRoomCoordinates =>
            new CoordinateList(new SourceBlock(_source, 0x31), 10, 16, 8);

        public ICoordinate BossRoom =>
            new Coordinate(new SourceBlock(_source, 0x3B), 16, 8);

        public IList<int> DrawnMapData =>
            new ByteList(new SourceBlock(_source, 0x3C), 16);
        public IList<int> DungeonMapDisplay =>
            new ByteList(new SourceBlock(_source, 0x4C), 45);

        public Fade DownstairsFade =>
            new Fade(new SourceBlock(_source, 0x79));
        public Fade UpstairsFade =>
            new Fade(new SourceBlock(_source, 0x99));
        public Fade DarkRoomFade =>
            new Fade(new SourceBlock(_source, 0xB9));
        public Fade DeathFade =>
            new Fade(new SourceBlock(_source, 0xD9));
    }
}
