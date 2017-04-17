using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zeldomizer.Metal;

namespace Zeldomizer.Engine.Overworld
{
    public class OverworldLevel
    {
        private readonly ISource _source;

        public OverworldLevel(ISource source)
        {
            _source = source;
        }
    }
}
