using System.Collections.Generic;

namespace Zeldomizer.Metal
{
    public class CodeBlock : ICodeBlock
    {
        public IEnumerable<int> AnalysisHintAddresses { get; set; }
        public int Length { get; set; }
        public int Offset { get; set; }
        public int Origin { get; set; }
        public IRom Rom { get; set; }
    }
}
