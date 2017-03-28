using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zeldomizer.Metal
{
    public static class RomExtensions
    {
        public static void Write(this IRom rom, IEnumerable<int> source, int destination)
        {
            rom.Write(source.Select(d => unchecked((byte)d)).ToArray(), destination);
        }
    }
}
