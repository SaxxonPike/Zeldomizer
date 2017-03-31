using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zeldomizer.Metal;

namespace Zeldomizer.Engine.Overworld
{
    public class OverworldDetailTileList
    {
        private readonly ISource _source;

        public OverworldDetailTileList(ISource source)
        {
            _source = source;
        }
    }
}
