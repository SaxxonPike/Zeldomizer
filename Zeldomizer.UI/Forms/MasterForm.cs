using System.Windows.Forms;
using Zeldomizer.UI.Core;

namespace Zeldomizer.UI.Forms
{
    public partial class MasterForm : Form
    {
        private readonly IFormFactory _formFactory;
        private readonly IContext _context;

        public MasterForm(IFormFactory formFactory, IContext context)
        {
            _formFactory = formFactory;
            _context = context;
            InitializeComponent();
        }
    }
}
