using Zeldomizer.Randomization.Interfaces;

namespace Zeldomizer.Randomization.RandomizerValidations
{
    public class IntRandomizerValidation : IRandomizerValidation
    {
        public IntRandomizerValidation(IRandomizerParameter parameter)
        {
            Parameter = parameter;
        }
        
        public IRandomizerParameter Parameter { get; }
        public int? Min { get; set; }
        public int? Max { get; set; }
        
        public void Validate()
        {
            var value = Parameter.GetEffectiveValue<int>();
            
            if (Min.HasValue && value < Min.Value)
                throw new RandomizerModuleValidationException($"{Parameter.Name} must not be less than {Min.Value}.");
            if (Max.HasValue && value > Max.Value)
                throw new RandomizerModuleValidationException($"{Parameter.Name} must not be greater than {Max.Value}.");
        }
    }
}
