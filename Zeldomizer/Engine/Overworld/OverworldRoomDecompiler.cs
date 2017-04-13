namespace Zeldomizer.Engine.Overworld
{
    /// <summary>
    /// A map decompiler for the overworld.
    /// </summary>
    public class OverworldRoomDecompiler : MapDecompiler
    {
        /// <summary>
        /// Width of a room, in tiles.
        /// </summary>
        protected override int RoomWidth => 16;

        /// <summary>
        /// Height of a room, in tiles.
        /// </summary>
        protected override int RoomHeight => 11;
    }
}
