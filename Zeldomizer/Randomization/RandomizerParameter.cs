using System;
using Zeldomizer.Randomization.Interfaces;

namespace Zeldomizer.Randomization
{
    public class RandomizerParameter : IRandomizerParameter
    {
        public Type Type { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public RandomizerParameterEnableType EnableType { get; set; }
        public object DefaultValue { get; set; }
        public object Value { get; set; }

        public TParameter GetDefaultValue<TParameter>()
        {
            if (typeof(TParameter).IsClass)
                return (TParameter)Convert.ChangeType(DefaultValue, typeof(TParameter));
            return (TParameter)Convert.ChangeType(DefaultValue ?? default(TParameter), typeof(TParameter));
        }

        public TParameter GetValue<TParameter>()
        {
            if (typeof(TParameter).IsClass)
                return (TParameter) Convert.ChangeType(Value, typeof(TParameter));
            return (TParameter) Convert.ChangeType(Value ?? default(TParameter), typeof(TParameter));
        }

        public object GetEffectiveValue()
        {
            return EnableType == RandomizerParameterEnableType.Disabled
                ? DefaultValue
                : Value;
        }

        public TParameter GetEffectiveValue<TParameter>()
        {
            return EnableType == RandomizerParameterEnableType.Disabled
                ? GetDefaultValue<TParameter>()
                : GetValue<TParameter>();
        }

        public void SetValue<TParameter>(TParameter newValue)
        {
            Value = typeof(TParameter) == Type 
                ? newValue 
                : Convert.ChangeType(typeof(TParameter), Type);
        }

        public IRandomizerValidation Validation { get; set; }
    }
}
