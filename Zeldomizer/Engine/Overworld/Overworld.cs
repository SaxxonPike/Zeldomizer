namespace Zeldomizer.Engine.Overworld
{
    public class Overworld : IOverworld
    {
        public Overworld(
            OverworldColumnLibraryList columnLibraries,
            OverworldGrid grid, 
            OverworldRoomLayoutList roomLayouts,
            OverworldTileList tiles,
            OverworldDetailTileList detailTiles
            )
        {
            ColumnLibraries = columnLibraries;
            Grid = grid;
            RoomLayouts = roomLayouts;
            Tiles = tiles;
            DetailTiles = detailTiles;
        }

        public OverworldColumnLibraryList ColumnLibraries { get; }
        public OverworldGrid Grid { get; }
        public OverworldRoomLayoutList RoomLayouts { get; }
        public OverworldTileList Tiles { get; }
        public OverworldDetailTileList DetailTiles { get; }
    }
}
