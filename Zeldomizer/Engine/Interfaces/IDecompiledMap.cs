using System.Collections.Generic;

namespace Zeldomizer.Engine.Interfaces
{
    public interface IDecompiledMap
    {
        IEnumerable<IEnumerable<int>> Rooms { get; set; }
    }
}