using System.Collections.Generic;

namespace Zeldomizer.Engine
{
    /// <summary>
    /// Map data that has been compiled by a <see cref="MapCompiler"/>.
    /// </summary>
    public class CompiledMap
    {
        /// <summary>
        /// Raw column data.
        /// </summary>
        public IEnumerable<int> Columns { get; set; }

        /// <summary>
        /// Raw room data.
        /// </summary>
        public IEnumerable<int> Rooms { get; set; }

        /// <summary>
        /// Offsets in the data where column data starts.
        /// </summary>
        public IEnumerable<int> ColumnOffsets { get; set; }
    }
}
