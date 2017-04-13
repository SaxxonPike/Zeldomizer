using System.Collections.Generic;
using System.Linq;

namespace Zeldomizer.Engine
{
    public abstract class MapDecompiler
    {
        /// <summary>
        /// Width of the room, in tiles.
        /// </summary>
        protected abstract int RoomWidth { get; }

        /// <summary>
        /// Height of the room, in tiles.
        /// </summary>
        protected abstract int RoomHeight { get; }

        /// <summary>
        /// Decompile room data from the internal format used by the game.
        /// </summary>
        /// <param name="columnLibraryList">Libraries to reference in the decompilation, which contain column definitions.</param>
        /// <param name="roomList">Room definitions. Each record refers to a list of columns in the room.</param>
        public DecompiledMap Decompile(IEnumerable<IEnumerable<IEnumerable<int>>> columnLibraryList, IEnumerable<IEnumerable<int>> roomList)
        {
            var columns = columnLibraryList.SelectMany(c => c).Select(c => c.ToArray()).ToArray();
            var rooms = roomList.Select(r => r.ToArray()).ToArray();

            return new DecompiledMap
            {
                Rooms = rooms.Select(r => DecompileRoom(r, columns))
            };
        }

        /// <summary>
        /// Reorganize room data into a linear form.
        /// </summary>
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
