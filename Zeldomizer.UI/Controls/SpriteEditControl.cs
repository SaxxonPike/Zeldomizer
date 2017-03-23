using System;
using System.Drawing;
using System.Windows.Forms;

namespace Zeldomizer.UI.Controls
{
    public partial class SpriteEditControl : UserControl
    {
        private int _zoom;

        public SpriteEditControl()
        {
            InitializeComponent();
        }

        public int Zoom
        {
            get
            {
                return _zoom;
            }
            set
            {
                _zoom = Math.Max(value, 1);
                spritePictureBox.Size = new Size(
                    (spritePictureBox.BackgroundImage?.Width ?? 8) * _zoom,
                    (spritePictureBox.BackgroundImage?.Height ?? 8) * _zoom);
            }
        }


    }
}
