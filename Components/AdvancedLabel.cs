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
    public partial class AdvancedLabel : Label
    {
        private Point _offset;
        private Color? _borderColor = null;
        private int _borderWidth = 1;

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

        private void Init()
        {
            this.Cursor = Cursors.Hand;

            this.Font = new Font(ComponentConstants.AdvancedLabelFontFamilyName, ComponentConstants.AdvancedLabelActiveFontSize);
            this.BorderStyle = BorderStyle.FixedSingle;

            this.TextAlign = ContentAlignment.MiddleCenter;
            this.AutoSize = true;
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                int dx = e.X - _offset.X;
                int dy = e.Y - _offset.Y;
                Location = new Point(Left + dx, Top + dy);
            }
            base.OnMouseMove(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            this.DoDragDrop(this, DragDropEffects.All);

            if (e.Button == MouseButtons.Left)
            {
                _offset = new Point(e.X, e.Y);
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (_borderColor.HasValue)
                e.Graphics.DrawRectangle(new Pen(_borderColor.Value, _borderWidth), new Rectangle(0, 0, this.Width - _borderWidth, this.Height - _borderWidth));
        }

        private void AdvancedLabel_GiveFeedback(Object sender, GiveFeedbackEventArgs e)
        {
            // Sets the custom cursor based upon the effect.
            e.UseDefaultCursors = false;
            if ((e.Effect & DragDropEffects.Move) == DragDropEffects.Move)
                Cursor.Current = Cursors.Hand;
        }

        public Color? BorderColor
        {
            get { return _borderColor; }
            set { _borderColor = value; }
        }

        public int BorderWidth
        {
            get { return _borderWidth; }
            set { _borderWidth = value; }
        }
    }
}
