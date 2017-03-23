using System.Windows.Forms;
using Autofac;

namespace Zeldomizer.UI.Browser
{
    public class BrowserUi : Form
    {
        public BrowserUi(IContainer container)
        {
            Visible = false;

            var browser = new BrowserControl(new Router())
            {
                Parent = this,
                Dock = DockStyle.Fill
            };

            browser.Navigated += (sender, args) => Visible = true;

            browser.Navigate("about:index");
        }
    }
}
