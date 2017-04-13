using System.Collections.Generic;
using Zeldomizer.Metal;

namespace Zeldomizer.Engine.Overworld
{
    /// <summary>
    /// A map compiler for the overworld.
    /// </summary>
    public class OverworldRoomCompiler : MapCompiler
    {
        /// <summary>
        /// Width of a room, in tiles.
        /// </summary>
        protected override int RoomWidth => 16;

        /// <summary>
        /// Height of a room, in tiles.
        /// </summary>
        protected override int RoomHeight => 11;

        /// <summary>
        /// Remove the start bit.
        /// </summary>
        protected override int RemoveSpecialBits(int value) => value & 0b01111111;

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
                // Tile index is the lower 5 bits of the data.
                var tileInput = input.Bits(4, 0);

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
                // the buffer and refill it. Max 2 tiles in a run.
                if (count == 1 || tileInput.Bits(4, 0) != tile || marker)
                {
                    yield return (count << 6) | tile | (writeMarker ? 0x80 : 0x00);
                    writeMarker = marker;
                    count = 0;
                    tile = tileInput;
                    continue;
                }

                count++;
            }

            // Flush the buffer.
            yield return (count << 6) | tile | (writeMarker ? 0x80 : 0x00);
        }
    }
}
