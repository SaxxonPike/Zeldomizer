using Zeldomizer.Metal;

namespace Zeldomizer.Engine.Underworld
{
    /// <summary>
    /// A mapping of underworld room tile values to graphics.
    /// </summary>
    public class UnderworldTileList : ByteList
    {
        /// <summary>
        /// Initialize an underworld tile graphic list.
        /// </summary>
        public UnderworldTileList(ISource source) : base(source, 0x08)
        {
        }
    }
}
