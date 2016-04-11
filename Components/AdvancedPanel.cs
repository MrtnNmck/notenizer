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
        #region Variables

        private int _borderWidth;
        private Color _borderColor;

        #endregion Variables

        #region Constructors

        public AdvancedPanel()
        {
            InitializeComponent();

            _borderWidth = 1;
            _borderColor = Color.Black;
        }

        #endregion Constuctors

        #region Properties

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

        #endregion Properties

        #region Event Handlers

        private void On_Paint(object sender, PaintEventArgs e)
        {
            if (this.BorderStyle == BorderStyle.FixedSingle)
            {
                int halfThickness = _borderWidth / 2;

                using (Pen pen = new Pen(_borderColor, halfThickness))
                {
                    e.Graphics.DrawRectangle(pen, new Rectangle(halfThickness, halfThickness, this.ClientSize.Width - this._borderWidth, this.ClientSize.Height - this._borderWidth));
                }
            }
        }

        #endregion Event Hanlders

        #region Methods

        #endregion Methods
    }
}
