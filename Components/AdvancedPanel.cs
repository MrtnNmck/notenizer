using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace nsComponents
{
    public partial class AdvancedPanel : Panel
    {
        private int _borderWidth;
        private Color _borderColor;

        public AdvancedPanel()
        {
            InitializeComponent();

            _borderWidth = 1;
            _borderColor = Color.Black;
        }

        public int BorderWidth
        {
            get { return _borderWidth; }
            set { _borderWidth = value; }
        }

        public Color BorderColor
        {
            get { return _borderColor; }
            set { _borderColor = value; }
        }

        private void On_Paint(object sender, PaintEventArgs e)
        {
            if (this.BorderStyle == BorderStyle.FixedSingle)
            {
                int halfThickness = _borderWidth / 2;

                using (Pen pen = new Pen(_borderColor, halfThickness))
                {
                    e.Graphics.DrawRectangle(pen, new Rectangle(halfThickness, halfThickness, this.ClientSize.Width - _borderWidth, this.ClientSize.Height - _borderWidth));
                }
            }
        }
    }
}
