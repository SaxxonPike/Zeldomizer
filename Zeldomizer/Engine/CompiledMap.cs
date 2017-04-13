using System.Collections.Generic;

namespace Zeldomizer.Engine
{
    /// <summary>
    /// Map data that has been compiled by a <see cref="MapCompiler"/>.
    /// </summary>
    public class CompiledMap
    {
        public CompiledMap(
            IEnumerable<int> columnData,
            int columnCount,
            IEnumerable<int> roomData,
            int roomCount,
            IEnumerable<int> columnOffsets)
        {
            ColumnData = columnData;
            ColumnCount = columnCount;
            RoomData = roomData;
            RoomCount = roomCount;
            ColumnOffsets = columnOffsets;
        }

        /// <summary>
        /// Raw column data.
        /// </summary>
        public IEnumerable<int> ColumnData { get; }

        /// <summary>
        /// Number of columns.
        /// </summary>
        public int ColumnCount { get; }

        /// <summary>
        /// Raw room data.
        /// </summary>
        public IEnumerable<int> RoomData { get; }

        /// <summary>
        /// Number of rooms.
        /// </summary>
        public int RoomCount { get; }

        /// <summary>
        /// Offsets in the data where column data starts.
        /// </summary>
        public IEnumerable<int> ColumnOffsets { get; }
    }
}
