using System.Collections.Generic;

namespace Zeldomizer.Metal
{
    public interface ICodeBlock
    {
        IEnumerable<int> AnalysisHintAddresses { get; set; }
        int Length { get; set; }
        int Offset { get; set; }
        int Origin { get; set; }
        IRom Rom { get; set; }
    }
}