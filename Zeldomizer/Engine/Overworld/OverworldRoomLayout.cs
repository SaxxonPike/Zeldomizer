using Zeldomizer.Metal;

namespace Zeldomizer.Engine.Overworld
{
    /// <summary>
    /// Represents a room layout for the overworld, in raw form.
    /// </summary>
    /// <remarks>
    /// Refer to <see cref="Underworld.UnderworldRoomLayout"/> for more information about layouts.
    /// </remarks>
    public class OverworldRoomLayout : ByteList
    {
        /// <summary>
        /// Initialize a room layout for the overworld.
        /// </summary>
        public OverworldRoomLayout(ISource source) : base(source, 16)
        {
        }
    }
}
