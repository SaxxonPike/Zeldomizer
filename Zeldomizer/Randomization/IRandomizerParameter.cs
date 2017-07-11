using System;

namespace Zeldomizer.Randomization
{
    public interface IRandomizerParameter
    {
        RandomizerParameterEnableType EnableType { get; set; }
        string Name { get; }
        string Description { get; }
        Type Type { get; }

        TParameter GetDefaultValue<TParameter>();
        TParameter GetValue<TParameter>();
        void SetValue<TParameter>(TParameter newValue);
    }
}