using System.Collections.Generic;
using System.Linq;

namespace Zeldomizer.Metal
{
    public static class RomExtensions
    {
        public static void Write(this ISource source, IEnumerable<int> data, int destination)
        {
            source.Write(data.Select(d => unchecked((byte)d)).ToArray(), destination);
        }
    }
}
