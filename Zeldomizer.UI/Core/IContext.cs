using System.ComponentModel;
using Zeldomizer.Metal;

namespace Zeldomizer.UI.Core
{
    public interface IContext : INotifyPropertyChanged, INotifyPropertyChanging
    {
        IRom Rom { get; }
        string RomFileName { get; set; }
    }
}