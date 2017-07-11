using Zeldomizer.Metal;

namespace Zeldomizer.Engine.Overworld
{
    /// <summary>
    /// Represents an overworld map detail tile, which allows specifying any four specific
    /// 8x8 graphics to use as tiles, in raw form.
    /// </summary>
    public class OverworldDetailTile : ByteList
    {
        /// <summary>
        /// Initialize an overworld detail tile.
        /// </summary>
        public OverworldDetailTile(ISource source) : base(source, 4)
        {
        }
    }
}