using Zeldomizer.Metal;

namespace Zeldomizer.Engine.Overworld
{
    /// <summary>
    /// Represents a 16x8 grid of overworld rooms, in raw form.
    /// </summary>
    /// <remarks>
    /// Refer to <see cref="Underworld.UnderworldGrid"/> for more information about grids.
    /// </remarks>
    public class OverworldGrid : ByteList
    {
        /// <summary>
        /// Initialize a 16x8 grid of overworld rooms.
        /// </summary>
        public OverworldGrid(ISource source) : base(source, 128)
        {
        }
    }
}
