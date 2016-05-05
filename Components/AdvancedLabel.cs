using nsConstants;
using nsEnums;
using nsInterfaces;
using nsNotenizerObjects;
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
    /// Advanced Label.
    /// Allows drag and drop event.
    /// Allows border color and width change.
    /// </summary>
    public partial class AdvancedLabel : Label
    {
        #region Variables

        private Point _offset;
        private Color? _borderColor = null;
        private int _borderWidth = 1;

        #endregion Variables

        #region Constructors

        public AdvancedLabel()
        {
            InitializeComponent();
            Init();
        }

        public AdvancedLabel(Font font)
        {
            InitializeComponent();
            Init();

            this.Font = font;
        }

        #endregion Constuctors

        #region Properties

        /// <summary>
        /// Border's color.
        /// </summary>
        public Color? BorderColor
        {
            get { return _borderColor; }
            set { _borderColor = value; }
        }

        /// <summary>
        /// Border's width
        /// </summary>
        public int BorderWidth
        {
            get { return _borderWidth; }
            set { _borderWidth = value; }
        }

        #endregion Properties

        #region Event Handlers

        /// <summary>
        /// Event handler for mouse move event.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                int dx = e.X - _offset.X;
                int dy = e.Y - _offset.Y;
                Location = new Point(this.Left + dx, this.Top + dy);
            }

            base.OnMouseMove(e);
        }

        /// <summary>
        /// Event handler on mouse up event.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
        }

        /// <summary>
        /// Event handler for mouse down event.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            this.DoDragDrop(this, DragDropEffects.All);

            if (e.Button == MouseButtons.Left)
            {
                _offset = new Point(e.X, e.Y);
            }
        }

        /// <summary>
        /// Event handler to handle paint event.
        /// Changes border color and width.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            try
            {
                base.OnPaint(e);

                if (_borderColor.HasValue)
                {
                    using (Pen pen = new Pen(this._borderColor.Value, this._borderWidth))
                    {
                        e.Graphics.DrawRectangle(pen, new Rectangle(0, 0, this.Width - _borderWidth, this.Height - _borderWidth));
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

        /// <summary>
        /// Initializes AdvancedLabel
        /// </summary>
        private void Init()
        {
            this.Cursor = Cursors.Hand;

            this.Font = new Font(ComponentConstants.AdvancedLabelFontFamilyName, ComponentConstants.AdvancedLabelActiveFontSize);
            this.BorderStyle = BorderStyle.FixedSingle;

            this.TextAlign = ContentAlignment.MiddleCenter;
            this.AutoSize = true;
        }

        #endregion Methods
    }
}
