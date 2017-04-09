using System.Collections.Generic;

namespace Zeldomizer.Engine.Dungeons
{
    public class CompiledUnderworld
    {
        public IEnumerable<int> Columns { get; set; }
        public IEnumerable<int> Rooms { get; set; }
        public IEnumerable<int> ColumnOffsets { get; set; }
    }
}
