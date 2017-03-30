using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zeldomizer.Metal
{
    public static class DebugPrettyPrint
    {
        public static string GetByteArray(IEnumerable<byte> data) =>
            string.Join(" ", data.Select(i => $"{i:X2}"));

        public static string GetByteArray(IEnumerable<int> data) =>
            GetByteArray(data.Select(d => unchecked((byte) d)));
    }
}
