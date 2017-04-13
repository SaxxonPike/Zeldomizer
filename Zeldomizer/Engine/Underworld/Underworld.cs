namespace Zeldomizer.Engine.Underworld
{
    public class Underworld
    {
        public Underworld(
            UnderworldColumnLibraryList columnLibraries, 
            UnderworldGridList grids, 
            UnderworldRoomLayoutList roomLayouts
            )
        {
            ColumnLibraries = columnLibraries;
            Grids = grids;
            RoomLayouts = roomLayouts;
        }

        public UnderworldColumnLibraryList ColumnLibraries { get; }
        public UnderworldGridList Grids { get; }
        public UnderworldRoomLayoutList RoomLayouts { get; }
    }
}
