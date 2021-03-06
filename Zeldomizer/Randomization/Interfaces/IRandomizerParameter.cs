﻿using System;

namespace Zeldomizer.Randomization.Interfaces
{
    public interface IRandomizerParameter
    {
        RandomizerParameterEnableType EnableType { get; set; }
        string Name { get; }
        string Description { get; }
        Type Type { get; }
        object Value { get; set; }
        object DefaultValue { get; set; }

        object GetEffectiveValue();
        TParameter GetEffectiveValue<TParameter>();
        TParameter GetDefaultValue<TParameter>();
        TParameter GetValue<TParameter>();
        
        void SetValue<TParameter>(TParameter newValue);
    }
}