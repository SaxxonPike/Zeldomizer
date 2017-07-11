namespace Zeldomizer.Engine.Underworld.Interfaces
{
    public interface IUnderworld
    {
        UnderworldColumnLibraryList ColumnLibraries { get; }
        UnderworldGridList Grids { get; }
        UnderworldLevelList Levels { get; }
        UnderworldRoomLayoutList RoomLayouts { get; }
    }
}