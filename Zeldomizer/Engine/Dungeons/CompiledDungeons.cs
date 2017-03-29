using System.Collections.Generic;

namespace Zeldomizer.Engine.Dungeons
{
    public class CompiledDungeons
    {
        public IEnumerable<int> Columns { get; set; }
        public IEnumerable<int> Rooms { get; set; }
        public IEnumerable<int> ColumnOffsets { get; set; }
    }
}
