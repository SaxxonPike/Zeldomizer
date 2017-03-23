using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Zeldomizer.Metal;
using Zeldomizer.UI.Annotations;

namespace Zeldomizer.UI.Core
{
    public class Context : IContext
    {
        private IRom _rom;
        private string _romFileName;

        public IRom Rom
        {
            get { return _rom; }
            private set
            {
                OnPropertyChanging();
                _rom = value;
                OnPropertyChanged();
            }
        }

        public string RomFileName
        {
            get { return _romFileName; }
            set
            {
                OnPropertyChanging();
                _romFileName = value;
                OnPropertyChanged();
            }
        }

        #region Data binding stuff

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangingEventHandler PropertyChanging;

        protected virtual void OnPropertyChanging([CallerMemberName] string propertyName = null)
        {
            PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(propertyName));
        }

        #endregion
    }
}
