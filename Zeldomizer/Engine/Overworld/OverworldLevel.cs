﻿using System.Collections.Generic;
using Zeldomizer.Engine.Graphics;
using Zeldomizer.Metal;

namespace Zeldomizer.Engine.Overworld
{
    public class OverworldLevel
    {
        private readonly ISource _source;

        public OverworldLevel(ISource source)
        {
            _source = source;
        }

        public PaletteList RoomPalette =>
            new PaletteList(new SourceBlock(_source, 0x00), 4);
        public Palette LinkPalette =>
            new Palette(new SourceBlock(_source, 0x10));
        public PaletteList EnemyPalette =>
            new PaletteList(new SourceBlock(_source, 0x14), 3);
        public ByteList EnemyQuantities =>
            new ByteList(new SourceBlock(_source, 0x21), 4);

        public IList<ICoordinate> PushSecretRoomCoordinates =>
            new CoordinateList(new SourceBlock(_source, 0x26), 4, 16, 8);

        public ICoordinate EntranceRoom => 
            new Coordinate(new SourceBlock(_source, 0x2C), 16, 8);

        public int LevelNumber
        {
            get => _source[0x30];
            set => _source[0x30] = unchecked((byte)value);
        }

        public IList<ICoordinate> ShortcutRoomCoordinates =>
            new CoordinateList(new SourceBlock(_source, 0x31), 4, 16, 8);

        public Fade DeathFade =>
            new Fade(new SourceBlock(_source, 0xD9));

    }
}
