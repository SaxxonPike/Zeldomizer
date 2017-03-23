using System.Windows.Forms;

namespace Zeldomizer.UI.Core
{
    public interface IFormFactory
    {
        TForm GetForm<TForm>()
            where TForm : Form;
    }
}