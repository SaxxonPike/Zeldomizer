using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zeldomizer.Metal;

namespace Zeldomizer.Engine.Overworld
{
    public class OverworldGrid : ByteList
    {
        public OverworldGrid(ISource source) : base(source, 128)
        {
        }
    }
}
