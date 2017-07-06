using Zeldomizer.Metal;

namespace Zeldomizer.Engine.Overworld
{
    public class OverworldLevel
    {
        private readonly ISource _source;

        public OverworldLevel(ISource source)
        {
            _source = source;
        }
    }
}
