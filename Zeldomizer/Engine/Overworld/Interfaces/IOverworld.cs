using Zeldomizer.Engine.Graphics;

namespace Zeldomizer.Engine.Overworld.Interfaces
{
    public interface IOverworld
    {
        OverworldColumnLibraryList ColumnLibraries { get; }
        OverworldDetailTileList DetailTiles { get; }
        OverworldGrid Grid { get; }
        OverworldRoomLayoutList RoomLayouts { get; }
        OverworldTileList Tiles { get; }
        OverworldSpriteList Sprites { get; }
        OverworldLevel Level { get; }
        OverworldStart Start { get; }
    }
}