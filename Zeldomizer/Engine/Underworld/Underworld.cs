using Zeldomizer.Engine.Underworld.Interfaces;

namespace Zeldomizer.Engine.Underworld
{
    public class Underworld : IUnderworld
    {
        public UnderworldColumnLibraryList ColumnLibraries { get; set; }
        public UnderworldGridList Grids { get; set; }
        public UnderworldRoomLayoutList RoomLayouts { get; set; }
    }
}
