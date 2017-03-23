using System.Windows.Forms;
using Autofac;

namespace Zeldomizer.UI.Core
{
    public class FormFactory : IFormFactory
    {
        private readonly IContainer _container;

        public FormFactory(IContainer container)
        {
            _container = container;
        }

        public TForm GetForm<TForm>() where TForm : Form
        {
            return _container.Resolve<TForm>();
        }
    }
}
