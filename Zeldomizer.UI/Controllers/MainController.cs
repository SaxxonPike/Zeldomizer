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
        
        public MainController(MainModel model)
        {
            
        }

        public void CloseForm(FormClosingEventArgs args)
        {
            FormClosed?.Invoke(this, EventArgs.Empty);
        }

        public void SelectModule(IRandomizerModule nodeTag)
        {
            ModuleSelected?.Invoke(this, nodeTag);
        }

        public void SetParameterValue<TValue>(IRandomizerParameter parameter, TValue value)
        {
            parameter.SetValue(value);
            ParameterValueChanged?.Invoke(this, parameter);
        }

        public void SetParameterEnableType(IRandomizerParameter parameter, RandomizerParameterEnableType enableType)
        {
            parameter.EnableType = enableType;
            ParameterEnableTypeChanged?.Invoke(this, parameter);
        }
    }
}
