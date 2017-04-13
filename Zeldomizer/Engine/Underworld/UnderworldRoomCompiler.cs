using System.Collections.Generic;
using System.Linq;
using Zeldomizer.Metal;

namespace Zeldomizer.Engine.Underworld
{
    /// <summary>
    /// A compiler for underworld rooms.
    /// </summary>
    public class UnderworldRoomCompiler : MapCompiler
    {
        /// <summary>
        /// Width of the room, in tiles.
        /// </summary>
        protected override int RoomWidth => 12;

        /// <summary>
        /// Height of the room, in tiles.
        /// </summary>
        protected override int RoomHeight => 7;

        /// <summary>
        /// Remove start bit and unused bit 3 of tile data.
        /// </summary>
        protected override int RemoveSpecialBits(int value) => value & 0b01110111;

        /// <summary>
        /// Take existing column data and encode it using RLE.
        /// </summary>
        protected override IEnumerable<int> EncodeSequence(IEnumerable<int> data)
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
