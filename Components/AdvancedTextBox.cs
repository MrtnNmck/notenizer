using nsConstants;
using nsInterfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace nsComponents
{
    public partial class AdvancedTextBox : Panel
    {
        #region Variables

        private RichTextBox _richTextBox = null;

        #endregion Variables

        #region Constructors

        public AdvancedTextBox() : base()
        {
            InitializeComponent();
            Init();
        }

        public AdvancedTextBox(String text)
        {
            InitializeComponent();
            Init();
            this._richTextBox.Text = text;
        }

        #endregion Constuctors

        #region Properties

        public RichTextBox TextBox
        {
            get { return this._richTextBox; }
        }

        #endregion Properties

        #region Event Handlers

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);

            if (Parent is IMousable)
                (Parent as IMousable).DoMouseWheel(e);
        }

        #endregion Event Handlers

        #region Methods

        private void Init()
        {
            this._richTextBox = new RichTextBox();

            this.Dock = DockStyle.Fill;
            this.BorderStyle = BorderStyle.FixedSingle;

            this._richTextBox.Font = new System.Drawing.Font(ComponentConstants.AdvancedTextBoxFontFamilyName, this.Font.Size);
            this._richTextBox.Multiline = true;
            this._richTextBox.ReadOnly = true;
            this._richTextBox.BorderStyle = BorderStyle.None;
            this._richTextBox.Dock = DockStyle.Fill;
            this._richTextBox.ShortcutsEnabled = true;

            this.Controls.Add(this._richTextBox);
        }

        #endregion Methods
    }
}
