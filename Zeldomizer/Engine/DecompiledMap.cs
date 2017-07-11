using System.Collections.Generic;
using Zeldomizer.Engine.Interfaces;

namespace Zeldomizer.Engine
{
    /// <summary>
    /// Map data that has been decompiled by a <see cref="MapDecompiler"/>.
    /// </summary>
    public class DecompiledMap : IDecompiledMap
    {
        /// <summary>
        /// Decompiled room data. Each record contains the individual tiles in the room.
        /// </summary>
        public IEnumerable<IEnumerable<int>> Rooms { get; set; }
    }
}
