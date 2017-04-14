namespace Zeldomizer.Engine.Underworld
{
    public interface IUnderworld
    {
        UnderworldColumnLibraryList ColumnLibraries { get; }
        UnderworldGridList Grids { get; }
        UnderworldRoomLayoutList RoomLayouts { get; }
    }
}