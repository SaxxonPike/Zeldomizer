using System.Collections.Generic;
using Zeldomizer.Randomization.Interfaces;

namespace Zeldomizer.Randomization
{
    public abstract class RandomizerModule : IRandomizerModule
    {
        private IReadOnlyList<IRandomizerParameter> _parameters;
        private IReadOnlyList<IRandomizerValidation> _validations;
        
        public bool Enabled { get; set; }
        public abstract string Name { get; }
        public abstract string Description { get; }

        public IReadOnlyList<IRandomizerParameter> Parameters => 
            _parameters ?? (_parameters = GetParameters());

        public IReadOnlyList<IRandomizerValidation> Validations =>
            _validations ?? (_validations = GetValidations());

        protected abstract IReadOnlyList<IRandomizerParameter> GetParameters();
        protected abstract IReadOnlyList<IRandomizerValidation> GetValidations();

        public void Activate(ZeldaCartridge cartridge, ISeededRandom random)
        {
            if (Enabled)
                DoActivate(cartridge, random);
        }

        protected abstract void DoActivate(ZeldaCartridge cartridge, ISeededRandom random);

        public void Validate()
        {
            if (!Enabled)
                return;

            foreach (var validation in Validations)
                validation.Validate();
        }
    }
}
