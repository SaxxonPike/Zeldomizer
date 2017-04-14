namespace Zeldomizer.Engine.Underworld.Interfaces
{
    public interface IUnderworld
    {
        UnderworldColumnLibraryList ColumnLibraries { get; }
        UnderworldGridList Grids { get; }
        UnderworldRoomLayoutList RoomLayouts { get; }
    }
}