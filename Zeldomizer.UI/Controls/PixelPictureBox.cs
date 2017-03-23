using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Zeldomizer.UI.Controls
{
    public class PixelPictureBox : PictureBox
    {
        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
            pevent.Graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
            pevent.Graphics.PixelOffsetMode = PixelOffsetMode.Half;
            base.OnPaintBackground(pevent);
        }
    }
}
