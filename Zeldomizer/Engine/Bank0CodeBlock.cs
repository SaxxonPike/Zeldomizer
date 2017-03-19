using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zeldomizer.Metal;

namespace Zeldomizer.Engine
{
    public class Bank0CodeBlock : CodeBlock
    {
        private static readonly IEnumerable<int> StaticAnalysisPoints = new[]
        {
            0x9825,
            0xBF50
        };

        public Bank0CodeBlock(IRom source, int offset, int length, int codeOffset, IEnumerable<int> staticAnalysisPoints)
            : base(source, offset, length, codeOffset, staticAnalysisPoints)
        {
        }
    }
}
