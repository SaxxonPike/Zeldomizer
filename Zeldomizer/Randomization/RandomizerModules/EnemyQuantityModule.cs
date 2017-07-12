using System;
using System.Collections.Generic;
using Zeldomizer.Randomization.Interfaces;
using Zeldomizer.Randomization.RandomizerValidations;

namespace Zeldomizer.Randomization.RandomizerModules
{
    public class EnemyQuantityModule : RandomizerModule
    {
        public override string Name => "Enemy Quantity";
        public override string Description => "Set enemy quantities.";

        private readonly IRandomizerParameter _minimumQuantityParameter = new RandomizerParameter
        {
            Name = "Minimum",
            Description = "Minimum quantity for randomization.",
            DefaultValue = 1,
            Type = typeof(int)
        };

        private readonly IRandomizerParameter _maximumQuantityParameter = new RandomizerParameter
        {
            Name = "Maximum",
            Description = "Maximum quantity for randomization.",
            DefaultValue = 6,
            Type = typeof(int)
        };

        protected override IReadOnlyList<IRandomizerParameter> GetParameters() => new[]
        {
            _minimumQuantityParameter,
            _maximumQuantityParameter
        };

        protected override IReadOnlyList<IRandomizerValidation> GetValidations() => new[]
        {
            new RangeRandomizerValidation(_minimumQuantityParameter, _maximumQuantityParameter) {Min = 0, Max = 8}
        };

        protected override void DoActivate(ZeldaCartridge cartridge, ISeededRandom random)
        {
            var min = _minimumQuantityParameter.GetEffectiveValue<int>();
            var max = _maximumQuantityParameter.GetEffectiveValue<int>();

            foreach (var underworldLevel in cartridge.Underworld.Levels)
            {
                var underworldLength = underworldLevel.EnemyQuantities.Count;
                for (var i = 0; i < underworldLength; i++)
                {
                    underworldLevel.EnemyQuantities[i] = random.GetInt(min, max + 1);
                }
            }

            var overworldLevel = cartridge.Overworld.Level;
            var overworldLength = overworldLevel.EnemyQuantities.Count;
            for (var i = 0; i < overworldLength; i++)
            {
                overworldLevel.EnemyQuantities[i] = random.GetInt(min, max + 1);
            }
        }
    }
}
