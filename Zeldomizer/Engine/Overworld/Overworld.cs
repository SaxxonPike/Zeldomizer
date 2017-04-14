using Zeldomizer.Engine.Overworld.Interfaces;

namespace Zeldomizer.Engine.Overworld
{
    public class Overworld : IOverworld
    {
        public OverworldColumnLibraryList ColumnLibraries { get; set; }
        public OverworldGrid Grid { get; set; }
        public OverworldRoomLayoutList RoomLayouts { get; set; }
        public OverworldTileList Tiles { get; set; }
        public OverworldDetailTileList DetailTiles { get; set; }
    }
}
