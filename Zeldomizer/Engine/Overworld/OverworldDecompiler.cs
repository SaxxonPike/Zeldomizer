﻿using System.Collections.Generic;
using System.Linq;

namespace Zeldomizer.Engine.Overworld
{
    public class OverworldDecompiler
    {
        public DecompiledOverworld Decompile(IEnumerable<IEnumerable<IEnumerable<int>>> columnLibraryList, IEnumerable<IEnumerable<int>> roomList, IEnumerable<int> tileList)
        {
            var columns = columnLibraryList.SelectMany(c => c.ToArray()).ToArray();
            var rooms = roomList.Select(r => r.ToArray()).ToArray();
            var tiles = tileList.ToArray();

            var translatedColumns = columns
                .Select(c => DecompileColumn(c, tiles).ToArray())
                .ToArray();

            return new DecompiledOverworld
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
            var output = new int[16 * 11];
            var i = 0;

            for (var y = 0; y < 11; y++)
            {
                for (var x = 0; x < 16; x++)
                {
                    output[i] = columns[room[x]][y];
                    i++;
                }
            }

            return output;
        }
    }
}
