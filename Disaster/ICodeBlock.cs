using System.Collections.Generic;

namespace Disaster
{
    public interface ICodeBlock
    {
        int Length { get; set; }
        int Offset { get; set; }
        int Origin { get; set; }
        IRom Rom { get; set; }
    }
}