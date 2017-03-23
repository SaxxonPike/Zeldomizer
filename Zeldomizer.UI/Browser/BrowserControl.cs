using System.Windows.Forms;

namespace Zeldomizer.UI.Browser
{
    public class BrowserControl : WebBrowser
    {
        private readonly Router _router;
        private bool _navigating;

        public BrowserControl(Router router)
        {
            _router = router;
            IsWebBrowserContextMenuEnabled = false;
            AllowWebBrowserDrop = false;
        }

        protected override void OnNavigating(WebBrowserNavigatingEventArgs e)
        {
            if (e.Url == Url)
            {
                base.OnNavigating(e);
                return;
            }

            if (_navigating)
            {
                base.OnNavigating(e);
                return;
            }

            _navigating = true;
            e.Cancel = true;
            base.OnNavigating(e);
            //if (Document != null)
            //    Document.Write(string.Empty);
            DocumentStream = _router.Open(e.Url.AbsolutePath);
            _navigating = false;
        }
    }
}
