using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zeldomizer.Metal
{
    public static class EnumerableExtensions
    {
        public static byte[] ToByteArray(this IEnumerable<int> data)
        {
            return data.Select(d => unchecked((byte) d)).ToArray();
        }
    }
}
