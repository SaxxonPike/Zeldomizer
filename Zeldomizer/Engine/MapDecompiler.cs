using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zeldomizer.Engine
{
    public abstract class MapDecompiler
    {
        protected abstract int RoomWidth { get; }
        protected abstract int RoomHeight { get; }

        public DecompiledMap Decompile(IEnumerable<IEnumerable<IEnumerable<int>>> columnLibraryList, IEnumerable<IEnumerable<int>> roomList)
        {
            var columns = columnLibraryList.SelectMany(c => c).Select(c => c.ToArray()).ToArray();
            var rooms = roomList.Select(r => r.ToArray()).ToArray();

            return new DecompiledMap
            {
                Rooms = rooms.Select(r => DecompileRoom(r, columns))
            };
        }

        private IEnumerable<int> DecompileRoom(IReadOnlyList<int> room, IReadOnlyList<IReadOnlyList<int>> columns)
        {
            var output = new int[RoomWidth * RoomHeight];
            var i = 0;

            for (var y = 0; y < RoomHeight; y++)
            {
                for (var x = 0; x < RoomWidth; x++)
                {
                    output[i] = columns[room[x]][y];
                    i++;
                }
            }

            return output;
        }
    }
}
