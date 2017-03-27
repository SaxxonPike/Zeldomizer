using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zeldomizer.Engine.Dungeons
{
    public class DungeonRoomCompiler
    {
        private class ColumnCompressionInfo
        {
            public List<int> Sequence { get; set; }
            public int Offset { get; set; }
            public int MapToIndex { get; set; }
            public bool Enabled { get; set; }
            public int Index { get; set; }

            public int[] GetTiles()
            {
                var count = 0;
                return Sequence.Skip(Offset).TakeWhile(c =>
                {
                    if (count >= 7)
                        return false;
                    count += ((c >> 4) & 0x7) + 1;
                    return true;
                }).ToArray();
            }
        }

        private class ColumnItemComparer : EqualityComparer<int>
        {
            public override bool Equals(int x, int y) =>
                (x & ~0x80) == (y & ~0x80);

            public override int GetHashCode(int obj) =>
                obj.GetHashCode();

            public static ColumnItemComparer Instance { get; } = new ColumnItemComparer();
        }

        private class ColumnComparer : EqualityComparer<IEnumerable<int>>
        {
            public override bool Equals(IEnumerable<int> x, IEnumerable<int> y) =>
                x.SequenceEqual(y, ColumnItemComparer.Instance);

            public override int GetHashCode(IEnumerable<int> obj) => obj.GetHashCode();

            public static ColumnComparer Instance { get; } = new ColumnComparer();
        }

        private class ColumnMatchResult
        {
            public Action Operation { get; set; }
            public int Score { get; set; }
        }

        public DungeonRoomCompilerOutput Compile(IEnumerable<IEnumerable<int>> data)
        {
            var dungeons = data.ToArray();
            var dungeonColumns = dungeons
                .SelectMany(d => Enumerable.Range(0, 12)
                    .Select(x => Enumerable.Range(0, 7)
                        .Select(y => d.ElementAt(x + y * 12)).ToArray()))
                        .ToArray();
            var compressedColumns = CompressColumns(dungeonColumns)
                .ToArray();
            var uniqueColumns = compressedColumns
                .GroupBy(g => g.MapToIndex)
                .Select(g => g.OrderBy(s => s.Index).First().Sequence)
                .Distinct()
                .ToArray();
            var outputColumns = uniqueColumns
                .SelectMany(c => c)
                .ToArray();
            var encodedColumns = EncodeColumn(outputColumns).ToArray();
            var encodedIndices = compressedColumns.Select(c => c.MapToIndex).Distinct().OrderBy(c => c).ToList();
            var columnMap = compressedColumns.ToDictionary(c => c.Index, c => encodedIndices.IndexOf(c.MapToIndex));

            return new DungeonRoomCompilerOutput
            {
                Columns = encodedColumns,
                Rooms = dungeonColumns.Select((e, i) => columnMap[i]).ToArray()
            };
        }

        private static IEnumerable<ColumnCompressionInfo> CompressColumns(IEnumerable<IEnumerable<int>> columns)
        {
            var modified = true;
            var columnInfo = columns.Select((e, i) => new ColumnCompressionInfo
            {
                Offset = 0,
                Sequence = e.ToList(),
                MapToIndex = i,
                Enabled = true,
                Index = i
            }).ToList();

            // Dedupe.
            while (modified)
            {
                modified = false;
                foreach (var column in columnInfo)
                {
                    foreach (var otherColumn in columnInfo.Except(new[] { column }))
                    {
                        if (!ReferenceEquals(column.Sequence, otherColumn.Sequence) && column.Sequence.SequenceEqual(otherColumn.Sequence))
                        {
                            column.MapToIndex = otherColumn.Index;
                            column.Sequence = otherColumn.Sequence;
                            column.Offset = otherColumn.Offset;
                            modified = true;
                        }
                    }
                }
            }

            modified = true;

            // Find where columns overlap.
            while (modified)
            {
                modified = false;

                foreach (var column in columnInfo)
                {
                    var columnTiles = column.GetTiles();
                    var matchResult = new List<ColumnMatchResult>();

                    foreach (var sequence in columnInfo.Select(ci => ci.Sequence).Distinct())
                    {
                        if (column.Sequence == sequence)
                            continue;

                        // Search for full match within other patterns
                        for (var k = 0; k <= sequence.Count - columnTiles.Length; k++)
                        {
                            var match = sequence.Skip(k).Take(columnTiles.Length);
                            if (!match.SequenceEqual(columnTiles, ColumnItemComparer.Instance))
                                continue;

                            var offset = k;
                            matchResult.Add(new ColumnMatchResult
                            {
                                Operation = () =>
                                {
                                    var sourceSequence = column.Sequence;
                                    var sourceOffset = column.Offset;
                                    foreach (var targetColumn in columnInfo)
                                    {
                                        if (!ReferenceEquals(targetColumn.Sequence, sourceSequence) ||
                                            targetColumn.Offset != sourceOffset)
                                            continue;
                                        column.Sequence = sequence;
                                        column.Offset = offset;
                                    }
                                },
                                Score = columnTiles.Length
                            });

                            break;
                        }

                        // No need to try for partial matches if we found a complete one
                        if (matchResult.Any(mr => mr.Score > 0))
                            break;

                        // Don't try to relocate sequences in the middle
                        if (column.Offset != 0)
                            break;

                        // Search for partial matches on pre-pattern boundaries
                        for (var k = 1; k < columnTiles.Length; k++)
                        {
                            var preMatch = columnTiles.Skip(k).ToArray();
                            if (preMatch.SequenceEqual(sequence.Take(preMatch.Length), ColumnItemComparer.Instance))
                            {
                                matchResult.Add(new ColumnMatchResult
                                {
                                    Operation = () =>
                                    {
                                        var insertLength = columnTiles.Length - preMatch.Length;
                                        foreach (var otherColumn in columnInfo.Where(ci => ReferenceEquals(ci.Sequence, sequence)))
                                            otherColumn.Offset += insertLength;

                                        sequence.InsertRange(0, columnTiles.Take(insertLength));

                                        var sourceSequence = column.Sequence;
                                        var sourceOffset = column.Offset;
                                        foreach (var targetColumn in columnInfo)
                                        {
                                            if (!ReferenceEquals(targetColumn.Sequence, sourceSequence) ||
                                                targetColumn.Offset != sourceOffset)
                                                continue;
                                            column.Sequence = sequence;
                                            column.Offset = 0;
                                            modified = true;
                                        }
                                    },
                                    Score = preMatch.Length
                                });

                                // If we find one, we already have the best score for this sequence
                                break;
                            }
                        }

                        // Search for partial matches on post-pattern boundaries
                        for (var k = Math.Min(columnTiles.Length - 1, sequence.Count - 1); k > 0; k--)
                        {
                            var postMatch = columnTiles.Skip(sequence.Count - k).ToArray();
                            if (postMatch.SequenceEqual(sequence.Take(postMatch.Length), ColumnItemComparer.Instance))
                            {
                                matchResult.Add(new ColumnMatchResult
                                {
                                    Operation = () =>
                                    {
                                        var targetOffset = sequence.Count;
                                        sequence.AddRange(columnTiles.Skip(postMatch.Length));
                                        var sourceSequence = column.Sequence;
                                        var sourceOffset = column.Offset;
                                        foreach (var targetColumn in columnInfo)
                                        {
                                            if (!ReferenceEquals(targetColumn.Sequence, sourceSequence) ||
                                                targetColumn.Offset != sourceOffset)
                                                continue;
                                            column.Sequence = sequence;
                                            column.Offset = targetOffset;
                                            modified = true;
                                        }
                                    },
                                    Score = postMatch.Length
                                });

                                // If we find one, we already have the best score for this sequence
                                break;
                            }
                        }
                    }

                    // Evaluate scores
                    var bestMatch = matchResult.Where(mr => mr.Score > 0).OrderByDescending(mr => mr.Score).FirstOrDefault();
                    bestMatch?.Operation();
                }
            }

            foreach (var column in columnInfo)
            {
                column.Sequence[column.Offset] |= 0x80;
                foreach (var otherColumn in columnInfo
                    .Where(ci => ReferenceEquals(ci.Sequence, column.Sequence) && ci.Offset == column.Offset)
                    .Except(new []{column}))
                {
                    otherColumn.Enabled = false;
                    otherColumn.MapToIndex = column.Index;
                }
            }

            return columnInfo;
        }

        private static IEnumerable<int> EncodeColumn(IEnumerable<int> data)
        {
            var init = true;
            var tile = 0;
            var count = 0;
            var writeMarker = true;

            foreach (var input in data)
            {
                var tileInput = input & 0x07;
                var marker = (input & 0x80) != 0;

                if (init)
                {
                    init = false;
                    tile = tileInput;
                    continue;
                }

                if ((tileInput & 0x7) != tile || marker)
                {
                    yield return (count << 4) | tile | (writeMarker ? 0x80 : 0x00);
                    writeMarker = marker;
                    count = 0;
                    tile = tileInput;
                    continue;
                }

                count++;
            }

            yield return (count << 4) | tile | (writeMarker ? 0x80 : 0x00);
        }
    }
}
