using System.Collections.Generic;

namespace Zeldomizer.Randomization
{
    public class Randomizer
    {
        public IReadOnlyList<IRandomizerModule> Modules { get; }
    }
}
