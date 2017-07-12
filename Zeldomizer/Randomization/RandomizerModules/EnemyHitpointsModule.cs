using System.Collections.Generic;
using Zeldomizer.Randomization.Interfaces;
using Zeldomizer.Randomization.RandomizerValidations;

namespace Zeldomizer.Randomization.RandomizerModules
{
    public class EnemyHitpointsModule : RandomizerModule
    {
        public override string Name => "Enemy Hit Points";
        public override string Description => "Set the number of hit points all enemies have.";

        private readonly IRandomizerParameter _minimumHitPointsParameter = new RandomizerParameter
        {
            Name = "Minimum",
            Description = "Minimum hit points for randomization.",
            DefaultValue = 0,
            Type = typeof(int)
        };

        private readonly IRandomizerParameter _maximumHitPointsParameter = new RandomizerParameter
        {
            Name = "Maximum",
            Description = "Maximum hit points for randomization.",
            DefaultValue = 250,
            Type = typeof(int)
        };
        
        private readonly IRandomizerParameter _includeEnemiesParameter = new RandomizerParameter
        {
            Name = "Enemies",
            Description = "If enabled, enemy hit points will be modified.",
            DefaultValue = true,
            Type = typeof(bool)
        };

        private readonly IRandomizerParameter _includeBossesParameter = new RandomizerParameter
        {
            Name = "Bosses",
            Description = "If enabled, boss hit points will be modified.",
            DefaultValue = false,
            Type = typeof(bool)
        };

        private readonly IRandomizerParameter _includeGanonParameter = new RandomizerParameter
        {
            Name = "Ganon",
            Description = "If enabled, Ganon's initial hit points will be modified.",
            DefaultValue = false,
            Type = typeof(bool)
        };

        protected override IReadOnlyList<IRandomizerParameter> GetParameters() => new[]
        {
            _minimumHitPointsParameter,
            _maximumHitPointsParameter,
            _includeEnemiesParameter,
            _includeBossesParameter,
            _includeGanonParameter
        };

        protected override IReadOnlyList<IRandomizerValidation> GetValidations() => new[]
        {
            new RangeRandomizerValidation(_minimumHitPointsParameter, _maximumHitPointsParameter) {Min = 0, Max = 255}
        };

        protected override void DoActivate(ZeldaCartridge cartridge, ISeededRandom random)
        {
            var min = _minimumHitPointsParameter.GetEffectiveValue<int>();
            var max = _maximumHitPointsParameter.GetEffectiveValue<int>();
            var includeEnemies = _includeEnemiesParameter.GetEffectiveValue<bool>();
            var includeBosses = _includeBossesParameter.GetEffectiveValue<bool>();
            var includeGanon = _includeGanonParameter.GetEffectiveValue<bool>();

            if (includeEnemies)
            {
                for (var i = 0x00; i < 0x19; i++)
                    cartridge.HitPointTable[i] = random.GetInt(min, max + 1);
            }

            if (includeBosses)
            {
                for (var i = 0x19; i < 0x25; i++)
                    if (i != 0x1F)
                        cartridge.HitPointTable[i] = random.GetInt(min, max + 1);
            }

            if (includeGanon)
            {
                cartridge.HitPointTable[0x1F] = random.GetInt(min, max + 1);
            }
        }
    }
}
