using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zeldomizer.Metal;

namespace Zeldomizer.Engine.Overworld
{
    public class OverworldColumnList
    {
        private readonly ISource _source;

        public OverworldColumnList(ISource source)
        {
            _source = source;
        }
    }
}
