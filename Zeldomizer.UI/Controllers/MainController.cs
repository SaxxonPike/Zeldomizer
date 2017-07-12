using System;
using System.Windows.Forms;
using Zeldomizer.Randomization;
using Zeldomizer.Randomization.Interfaces;
using Zeldomizer.UI.Models;

namespace Zeldomizer.UI.Controllers
{
    public class MainController
    {
        public event EventHandler FormClosed;
        public event EventHandler<IRandomizerModule> ModuleSelected;
        public event EventHandler<IRandomizerParameter> ParameterValueChanged;
        public event EventHandler<IRandomizerParameter> ParameterEnableTypeChanged;

        private int _suspendLevels = 0;
        
        public MainController(MainModel model)
        {
            
        }

        public void CloseForm(FormClosingEventArgs args)
        {
            if (_suspendLevels > 0)
                return;

            FormClosed?.Invoke(this, EventArgs.Empty);
        }

        public void SelectModule(IRandomizerModule nodeTag)
        {
            if (_suspendLevels > 0)
                return;
            
            ModuleSelected?.Invoke(this, nodeTag);
        }

        public void SetParameterValue<TValue>(IRandomizerParameter parameter, TValue value)
        {
            if (_suspendLevels > 0)
                return;

            parameter.SetValue(value);
            ParameterValueChanged?.Invoke(this, parameter);
        }

        public void SetParameterEnableType(IRandomizerParameter parameter, RandomizerParameterEnableType enableType)
        {
            if (_suspendLevels > 0)
                return;

            parameter.EnableType = enableType;
            ParameterEnableTypeChanged?.Invoke(this, parameter);
        }

        public void Suspend()
        {
            _suspendLevels++;
        }

        public void Resume()
        {
            if (_suspendLevels > 0)
                _suspendLevels--;
        }
    }
}
