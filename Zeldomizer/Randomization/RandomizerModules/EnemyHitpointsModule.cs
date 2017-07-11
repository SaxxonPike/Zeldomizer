using System.Collections.Generic;

namespace Zeldomizer.Randomization.RandomizerModules
{
    public class EnemyHitpointsModule : IRandomizerModule
    {
        public string Name => "Enemy Hit Points";
        public string Description => "Set the number of hit points all enemies have.";
        public bool Enabled { get; set; }

        private readonly IRandomizerParameter _hitPointsParameter = new RandomizerParameter
        {
            Name = "Hit Points",
            DefaultValue = 1,
            Type = typeof(int)
        };

        public IReadOnlyList<IRandomizerParameter> Parameters => new[]
        {
            _hitPointsParameter
        };

        public void Activate(ZeldaCartridge cartridge)
        {
            // TODO
        }
    }
}
