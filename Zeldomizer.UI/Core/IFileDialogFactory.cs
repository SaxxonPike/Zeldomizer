using System.Windows.Forms;

namespace Zeldomizer.UI.Core
{
    public interface IFileDialogFactory
    {
        OpenFileDialog GetOpen(params (string extension, string name)[] filters);
        SaveFileDialog GetSave(params (string extension, string name)[] filters);
    }
}