using Zeldomizer.Metal;

namespace Zeldomizer.Engine.Underworld
{
    /// <summary>
    /// Represents a room layout for the underworld, in raw form.
    /// </summary>
    public class UnderworldRoomLayout : ByteList
    {
        /// <summary>
        /// Initialize a room layout.
        /// </summary>
        public UnderworldRoomLayout(ISource source) : base(source, 12)
        {
        }
    }
}
