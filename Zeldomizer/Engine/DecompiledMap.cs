using System.Collections.Generic;

namespace Zeldomizer.Engine
{
    /// <summary>
    /// Map data that has been decompiled by a <see cref="MapDecompiler"/>.
    /// </summary>
    public class DecompiledMap
    {
        /// <summary>
        /// Decompiled room data. Each record contains the individual tiles in the room.
        /// </summary>
        public IEnumerable<IEnumerable<int>> Rooms { get; set; }
    }
}
