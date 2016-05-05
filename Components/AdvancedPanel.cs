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
    /// <summary>
    /// Advnaced Label.
    /// Allows change of border width and color.
    /// </summary>
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

        /// <summary>
        /// Border's width.
        /// </summary>
        public int BorderWidth
        {
            get { return _borderWidth; }
            set { _borderWidth = value; }
        }

        /// <summary>
        /// Border's color.
        /// </summary>
        public Color BorderColor
        {
            get { return _borderColor; }
            set { _borderColor = value; }
        }

        #endregion Properties

        #region Event Handlers

        /// <summary>
        /// Handles on paint event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnPaint(object sender, PaintEventArgs e)
        {
            try
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
            catch (Exception ex)
            {
                throw new Exception("Failed OnPaint event." + Environment.NewLine + Environment.NewLine + ex.Message);
            }
        }

        #endregion Event Handlers

        #region Methods

        #endregion Methods
    }
}
