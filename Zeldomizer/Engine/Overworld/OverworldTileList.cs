using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zeldomizer.Metal;

namespace Zeldomizer.Engine.Overworld
{
    public class OverworldTileList : ByteList
    {
        private readonly ISource _source;

        public OverworldTileList(ISource source) : base(source, 0x40)
        {
            _source = source;
        }
    }
}
