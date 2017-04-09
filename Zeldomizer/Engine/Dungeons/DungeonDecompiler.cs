using System.Collections.Generic;
using System.Linq;

namespace Zeldomizer.Engine.Dungeons
{
    public class DungeonDecompiler
    {
        public DecompiledUnderworld Decompile(IEnumerable<DungeonColumn> columnList, IEnumerable<DungeonRoomLayout> roomList, IEnumerable<int> tileList)
        {
            var columns = columnList.Select(c => c.ToArray()).ToArray();
            var rooms = roomList.Select(r => r.ToArray()).ToArray();
            var tiles = tileList.ToArray();

            var translatedColumns = columns
                .Select(c => DecompileColumn(c, tiles).ToArray())
                .ToArray();

            return new DecompiledUnderworld
            {
                Rooms = rooms.Select(r => DecompileRoom(r, translatedColumns))
            };
        }

        private static IEnumerable<int> DecompileColumn(IEnumerable<int> column, IReadOnlyList<int> tileList)
        {
            return column.Select(c => tileList[c]);
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
