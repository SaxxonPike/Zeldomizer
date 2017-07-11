using System;

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
            return (TParameter)Convert.ChangeType(Value, typeof(TParameter));
        }

        public TParameter GetValue<TParameter>()
        {
            return (TParameter)Convert.ChangeType(Value, typeof(TParameter));
        }

        public void SetValue<TParameter>(TParameter newValue)
        {
            Value = Convert.ChangeType(typeof(TParameter), Type);
        }
    }
}
