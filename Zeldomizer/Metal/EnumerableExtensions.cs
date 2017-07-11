using System.Collections.Generic;
using System.Linq;

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
