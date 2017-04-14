namespace Zeldomizer.Engine.Overworld.Interfaces
{
    public interface IOverworld
    {
        OverworldColumnLibraryList ColumnLibraries { get; }
        OverworldDetailTileList DetailTiles { get; }
        OverworldGrid Grid { get; }
        OverworldRoomLayoutList RoomLayouts { get; }
        OverworldTileList Tiles { get; }
    }
}