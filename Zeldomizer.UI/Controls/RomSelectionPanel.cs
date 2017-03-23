using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Zeldomizer.UI.Core;

namespace Zeldomizer.UI.Controls
{
    public partial class RomSelectionPanel : UserControl
    {
        private readonly IContext _context;

        public RomSelectionPanel(IContext context)
        {
            _context = context;
            InitializeComponent();
        }
    }
}
