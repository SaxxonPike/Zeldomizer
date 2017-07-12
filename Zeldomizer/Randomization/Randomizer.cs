using System.Collections.Generic;
using Zeldomizer.Randomization.Interfaces;
using Zeldomizer.Randomization.RandomizerModules;

namespace Zeldomizer.Randomization
{
    public class Randomizer : IRandomizer
    {
        public IReadOnlyList<IRandomizerModule> Modules { get; } = new IRandomizerModule[]
        {
            new EnemyHitpointsModule(),
            new EnemyQuantityModule()
        };
    }
}
