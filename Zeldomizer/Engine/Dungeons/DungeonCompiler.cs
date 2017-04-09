using System.Collections.Generic;
using System.Linq;
using Zeldomizer.Metal;

namespace Zeldomizer.Engine.Dungeons
{
    /// <summary>
    /// Encodes dungeon rooms and columns to the format used internally by the game.
    /// </summary>
    /// <remarks>
    /// The method used here doesn't compress data as tightly as the original. It favors
    /// speed over compression efficiency. The difference in compression efficiency has been
    /// measured to be roughly 10%. In order to be more efficient, the Compress step would
    /// need to be optimized to search for better ways columns can overlap.
    /// </remarks>
    public class DungeonCompiler
    {
        /// <summary>
        /// Compile dungeon room data to the format used internally.
        /// Outer enumerable is for dungeon rooms, and the inner enumerable is for tile data
        /// belonging to those rooms. Rooms are 12x7 in size.
        /// </summary>
        public CompiledDungeons Compile(IEnumerable<IEnumerable<int>> data)
        {
            var columns = ExtractColumns(data);
            var consolidatedMap = Consolidate(columns);
            var compressedMap = Compress(consolidatedMap);
            var compiledMap = Encode(compressedMap);

            var result = new CompiledDungeons
            {
                ColumnOffsets = compiledMap.Offsets,
                Columns = compiledMap.Repo,
                Rooms = consolidatedMap.Map.Select(cm => compressedMap.Map[cm.Value]).ToArray()
            };

            return result;
        }

        /// <summary>
        /// Extract columns from raw room data.
        /// </summary>
        private static IEnumerable<IEnumerable<int>> ExtractColumns(IEnumerable<IEnumerable<int>> data)
        {
            // Plot data to 12x7 map, then extract columns from it.
            return data
                .Select(d => d.Select(t => t.Bits(2, 0)).ToArray())
                .SelectMany(d => Enumerable.Range(0, 12)
                    .Select(x => Enumerable.Range(0, 7)
                        .Select(y => d[x + y * 12])));
        }

        /// <summary>
        /// Result from consolidation operation.
        /// </summary>
        private class ConsolidatedMap
        {
            public IDictionary<int, int> Map { get; set; }
            public IDictionary<int, int[]> Repo { get; set; }
        }

        /// <summary>
        /// Remove duplicate columns.
        /// </summary>
        private static ConsolidatedMap Consolidate(IEnumerable<IEnumerable<int>> dungeonColumns)
        {
            var dungeonColumnMap = new Dictionary<int, int>();
            var dungeonColumnRepo = new Dictionary<int, int[]>();
            var sourceIndex = 0;
            var repoIndex = 0;

            foreach (var dungeonColumn in dungeonColumns)
            {
                // Try and find existing matching column data
                var existingColumn = dungeonColumnRepo
                    .FirstOrDefault(kv => kv.Value.SequenceEqual(dungeonColumn));

                // If the column doesn't already exist, allocate
                if (existingColumn.Value == null)
                {
                    dungeonColumnRepo[repoIndex] = dungeonColumn.ToArray();
                    dungeonColumnMap[sourceIndex] = repoIndex;
                    repoIndex++;
                }
                else
                {
                    dungeonColumnMap[sourceIndex] = existingColumn.Key;
                }

                sourceIndex++;
            }

            return new ConsolidatedMap
            {
                Map = dungeonColumnMap,
                Repo = dungeonColumnRepo
            };
        }

        /// <summary>
        /// Result from compression operation.
        /// </summary>
        private class CompressedMap
        {
            public IDictionary<int, int> Offsets { get; set; }
            public IEnumerable<int> Repo { get; set; }
            public IDictionary<int, int> Map { get; set; }
        }

        /// <summary>
        /// Find overlapping portions of data and consolidate columns.
        /// </summary>
        private static CompressedMap Compress(ConsolidatedMap consolidatedMap)
        {
            var offsets = new Dictionary<int, int>();
            var repo = new List<int>();

            // Flatten all the data, keeping track where it is located.
            foreach (var entry in consolidatedMap.Repo)
            {
                offsets[entry.Key] = repo.Count;
                repo.AddRange(entry.Value.Select(v => v & 0b01110111));
            }

            var done = false;
            var attempts = 0;

            while (true)
            {
                // The iterations are not always stable, so we retry once.
                if (!done)
                    attempts = 2;
                else if (--attempts == 0)
                    break;

                // Find the first different occurrence of column data in the flattened data.
                var newOffsets = new Dictionary<int, int>();
                foreach (var entry in offsets)
                {
                    var key = entry.Key;
                    var value = entry.Value;
                    var term = repo.Skip(entry.Value).Take(7).ToArray();

                    for (var i = 0; i < repo.Count - 7; i++)
                    {
                        // If the occurrence is different than current, use it.
                        if (i != value && term.SequenceEqual(repo.Skip(i).Take(7)))
                        {
                            value = i;
                            break;
                        }
                    }

                    newOffsets[key] = value;
                }

                // Repopulate new flattened data, skipping over unused bytes
                // which may have resulted from the previous loop.
                var usedOffsets = newOffsets
                    .Select(kv => kv.Value)
                    .ToArray();
                var newRepo = new List<int>();
                var activeTiles = 0;
                for (var i = 0; i < repo.Count; i++)
                {
                    if (usedOffsets.Contains(i))
                        activeTiles = 7;

                    // Do not shift offsets if we are actively using the data.
                    if (activeTiles > 0)
                    {
                        activeTiles--;
                        newRepo.Add(repo[i]);
                        continue;
                    }

                    // Shift offsets due to bytes being skipped.
                    foreach (var affectedOffset in newOffsets
                        .Where(kv => kv.Value > newRepo.Count)
                        .Select(kv => kv.Key).ToArray())
                        newOffsets[affectedOffset]--;
                }

                // The iteration is successful if the flattened data shrunk.
                done = !(repo.Count < newRepo.Count);
                repo = newRepo;
                offsets = newOffsets;
            }

            return new CompressedMap
            {
                Offsets = offsets,
                Repo = repo,
                Map = GetCompressedMapMapping(offsets)
            };
        }

        /// <summary>
        /// Map a column/offset map to the order of columns in compressed data.
        /// </summary>
        private static Dictionary<int, int> GetCompressedMapMapping(Dictionary<int, int> offsets)
        {
            var output = new Dictionary<int, int>();
            var index = 0;

            foreach (var offset in offsets.OrderBy(kv => kv.Value))
            {
                output[offset.Key] = index;
                index++;
            }

            return output;
        }

        /// <summary>
        /// Result from encode operation.
        /// </summary>
        private class EncodedMap
        {
            public IEnumerable<int> Offsets { get; set; }
            public IEnumerable<int> Repo { get; set; }
        }

        /// <summary>
        /// Generate start points for decompression and apply encoding.
        /// </summary>
        private static EncodedMap Encode(CompressedMap compressedMap)
        {
            // Set bit 7 to indicate where sequences begin.
            var repo = compressedMap.Repo.ToArray();
            foreach (var entry in compressedMap.Offsets)
                repo[entry.Value] |= 0x80;

            var encodedRepo = EncodeSequence(repo)
                .ToArray();

            return new EncodedMap
            {
                Offsets = GetEncodedOffsets(encodedRepo),
                Repo = encodedRepo
            };
        }

        /// <summary>
        /// Get offsets to columns within raw column data by parsing sequence-start markers.
        /// </summary>
        private static IEnumerable<int> GetEncodedOffsets(IEnumerable<int> columns)
        {
            var offset = 0;
            foreach (var tile in columns)
            {
                // Bit 7 indicates the beginning of a sequence.
                if (tile.Bit(7))
                    yield return offset;
                offset++;
            }
        }

        /// <summary>
        /// Take existing column data and encode it using RLE.
        /// </summary>
        private static IEnumerable<int> EncodeSequence(IEnumerable<int> data)
        {
            var init = true;
            var tile = 0;
            var count = 0;
            var writeMarker = true;

            foreach (var input in data)
            {
                // Tile index is the lower 3 bits of the data.
                var tileInput = input.Bits(2, 0);

                // Bit 7 indicates the start of a sequence.
                var marker = input.Bit(7);

                // First run through will preload the buffer.
                if (init)
                {
                    init = false;
                    tile = tileInput;
                    continue;
                }

                // If the tile we found is different than the buffer, write out
                // the buffer and refill it.
                if (tileInput.Bits(2, 0) != tile || marker)
                {
                    yield return (count << 4) | tile | (writeMarker ? 0x80 : 0x00);
                    writeMarker = marker;
                    count = 0;
                    tile = tileInput;
                    continue;
                }

                count++;
            }

            // Flush the buffer.
            yield return (count << 4) | tile | (writeMarker ? 0x80 : 0x00);
        }
    }
}
