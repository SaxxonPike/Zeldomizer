using System.Collections.Generic;

namespace Zeldomizer.Engine.Interfaces
{
    public interface ICompiledMap
    {
        int ColumnCount { get; }
        IEnumerable<int> ColumnData { get; }
        IEnumerable<int> ColumnOffsets { get; }
        int RoomCount { get; }
        IEnumerable<int> RoomData { get; }
    }
}