using Zeldomizer.Metal;

namespace Zeldomizer.Engine.Overworld.Interfaces
{
    public interface IOverworldLevel
    {
        ICoordinate StartCoordinate { get; }
        int StartYPosition { get; set; }
    }
}
