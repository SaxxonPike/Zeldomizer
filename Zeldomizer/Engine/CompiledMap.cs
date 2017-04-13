using System.Collections.Generic;

namespace Zeldomizer.Engine
{
    public class CompiledMap
    {
        public IEnumerable<int> Columns { get; set; }
        public IEnumerable<int> Rooms { get; set; }
        public IEnumerable<int> ColumnOffsets { get; set; }
    }
}
