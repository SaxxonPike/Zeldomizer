using Zeldomizer.Metal;

namespace Zeldomizer.Engine.Overworld
{
    /// <summary>
    /// A mapping of overworld room tile values to graphics.
    /// </summary>
    public class OverworldTileList : ByteList
    {
        /// <summary>
        /// Initialize an underworld tile graphic list.
        /// </summary>
        public OverworldTileList(ISource source) : base(source, 0x40)
        {
        }
    }
}
