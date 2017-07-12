using System.Collections.Generic;
using Zeldomizer.Randomization.Interfaces;
using Zeldomizer.Randomization.RandomizerModules;

namespace Zeldomizer.Randomization
{
    public class Randomizer : IRandomizer
    {
        public Randomizer()
        {
            foreach (var module in Modules)
            {
                foreach (var parameter in module.Parameters)
                {
                    parameter.Value = parameter.DefaultValue;
                }
            }
        }
        
        public IReadOnlyList<IRandomizerModule> Modules { get; } = new IRandomizerModule[]
        {
            new EnemyHitpointsModule(),
            new EnemyQuantityModule()
        };
    }
}
