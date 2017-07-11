using Zeldomizer.Metal;

namespace Zeldomizer.Engine.Overworld
{
    public class OverworldRoom
    {
        private readonly ISource _source;

        /// <summary>
        /// Initialize an overworld room's properties.
        /// </summary>
        public OverworldRoom(ISource source)
        {
            _source = source;
        }

        /// <summary>
        /// Get or set the overworld room's layout index.
        /// </summary>
        public int Layout
        {
            get => _source[0x000].Bits(6, 0);
            set => _source[0x000] = _source[0x000].Bits(6, 0, value);
        }

        /// <summary>
        /// Unknown.
        /// </summary>
        public bool LayoutFlag
        {
            get => _source[0x000].Bit(7);
            set => _source[0x000] = _source[0x000].Bit(7, value);
        }
    }
}
