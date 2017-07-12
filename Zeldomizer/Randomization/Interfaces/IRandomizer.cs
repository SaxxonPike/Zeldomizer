using System.Collections.Generic;

namespace Zeldomizer.Randomization.Interfaces
{
    public interface IRandomizer
    {
        IReadOnlyList<IRandomizerModule> Modules { get; }
    }
}