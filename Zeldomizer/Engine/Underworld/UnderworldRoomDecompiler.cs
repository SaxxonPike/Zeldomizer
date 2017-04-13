using System.Collections.Generic;
using System.Linq;

namespace Zeldomizer.Engine.Underworld
{
    public class UnderworldRoomDecompiler
    {
        public DecompiledUnderworld Decompile(IEnumerable<IEnumerable<IEnumerable<int>>> columnList, IEnumerable<IEnumerable<int>> roomList)
        {
            var columns = columnList.SelectMany(c => c).Select(c => c.ToArray()).ToArray();
            var rooms = roomList.Select(r => r.ToArray()).ToArray();

            return new DecompiledUnderworld
            {
                Rooms = rooms.Select(r => DecompileRoom(r, columns))
            };
        }

        private static IEnumerable<int> DecompileRoom(IReadOnlyList<int> room, IReadOnlyList<IReadOnlyList<int>> columns)
        {
            var output = new int[12 * 7];
            var i = 0;

            for (var y = 0; y < 7; y++)
            {
                for (var x = 0; x < 12; x++)
                {
                    output[i] = columns[room[x]][y];
                    i++;
                }
            }

            return output;
        }
    }
}
