using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zeldomizer.Metal;

namespace Zeldomizer.Engine.Graphics
{
    public class Palette : ByteList
    {
        public Palette(ISource source) : base(source, 4)
        {
        }
    }
}
