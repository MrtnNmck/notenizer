using nsExtensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace nsGUI
{
    public partial class FormTextInputer : Form
    {
        #region Variables

        private String _textForProcessing;

        #endregion Variables

        #region Constructors

        public FormTextInputer()
        {
            InitializeComponent();
            this.Icon = Properties.Resources.AppIcon;
            this.CenterToParent();
        }

        public FormTextInputer(String text)
        {
            InitializeComponent();
            this.Icon = Properties.Resources.AppIcon;
            this.CenterToParent();
            this._notenizerTextBoxText.TextBox.Text = text;
        }

        #endregion Constuctors

        #region Properties

        /// <summary>
        /// Text for processing.
        /// </summary>
        public string TextForProcessing
        {
            get { return this._textForProcessing; }
        }

        #endregion Properties

        #region Event Handlers

        /// <summary>
        /// Confirm button click event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonConfirm_Click(Object sender, EventArgs e)
        {
            this._textForProcessing = this._notenizerTextBoxText.TextBox.Text.Trim().NormalizeWhiteSpaces();
            this.DialogResult = DialogResult.OK;
        }

        /// <summary>
        /// Cancel button click event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonCancel_Click(Object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        #endregion Event Handlers

        #region Methods

        #endregion Methods
    }
}
