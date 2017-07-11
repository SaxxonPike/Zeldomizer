using System.Collections.Generic;
using Zeldomizer.Engine.Interfaces;

namespace Zeldomizer.Engine
{
    /// <summary>
    /// Map data that has been compiled by a <see cref="MapCompiler"/>.
    /// </summary>
    public class CompiledMap : ICompiledMap
    {
        /// <summary>
        /// Raw column data.
        /// </summary>
        public IEnumerable<int> ColumnData { get; set; }

        /// <summary>
        /// Number of columns.
        /// </summary>
        public int ColumnCount { get; set; }

        /// <summary>
        /// Raw room data.
        /// </summary>
        public IEnumerable<int> RoomData { get; set; }

        /// <summary>
        /// Number of rooms.
        /// </summary>
        public int RoomCount { get; set; }

        /// <summary>
        /// Offsets in the data where column data starts.
        /// </summary>
        public IEnumerable<int> ColumnOffsets { get; set; }
    }
}
