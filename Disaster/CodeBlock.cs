using System.Collections.Generic;

namespace Disaster
{
    public class CodeBlock : ICodeBlock
    {
        public int Length { get; set; }
        public int Offset { get; set; }
        public int Origin { get; set; }
        public IRom Rom { get; set; }
    }
}
