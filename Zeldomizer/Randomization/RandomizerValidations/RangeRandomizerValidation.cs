using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zeldomizer.Randomization.Interfaces;

namespace Zeldomizer.Randomization.RandomizerValidations
{
    public class RangeRandomizerValidation : IRandomizerValidation
    {
        public RangeRandomizerValidation(IRandomizerParameter min, IRandomizerParameter max)
        {
            MinParameter = min;
            MaxParameter = max;
        }
        
        public IRandomizerParameter MinParameter { get; }
        public IRandomizerParameter MaxParameter { get; }
        public int? Min { get; set; }
        public int? Max { get; set; }

        public void Validate()
        {
            var minValue = MinParameter.GetEffectiveValue<int>();
            
            if (Min.HasValue && minValue < Min.Value)
                throw new RandomizerModuleValidationException($"{MinParameter.Name} must not be less than {Min.Value}.");
            if (Max.HasValue && minValue > Max.Value)
                throw new RandomizerModuleValidationException($"{MinParameter.Name} must not be greater than {Max.Value}.");

            var maxValue = MaxParameter.GetEffectiveValue<int>();

            if (Min.HasValue && maxValue < Min.Value)
                throw new RandomizerModuleValidationException($"{MaxParameter.Name} must not be less than {Min.Value}.");
            if (Max.HasValue && maxValue > Max.Value)
                throw new RandomizerModuleValidationException($"{MaxParameter.Name} must not be greater than {Max.Value}.");
            
            if (minValue > maxValue)
                throw new RandomizerModuleValidationException($"{MinParameter.Name} must not be greater than {MaxParameter.Name}.");
        }
    }
}
