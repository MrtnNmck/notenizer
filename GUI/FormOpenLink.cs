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
    /// <summary>
    /// Window to get text from URL or wikipedia.
    /// </summary>
    public partial class FormOpenLink : Form
    {
        #region Variables

        private String _url = String.Empty;
        private String _country = String.Empty;

        #endregion Variables

        #region Constructors

        public FormOpenLink()
        {
            InitializeComponent();
            CenterToParent();
        }

        #endregion Constuctors

        #region Properties

        /// <summary>
        /// Url of wikipedia page.
        /// </summary>
        public String Url
        {
            get { return this._url; }
        }

        /// <summary>
        /// Country name on wikipedia.
        /// </summary>
        public String Country
        {
            get { return this._country; }
        }

        #endregion Properties

        #region Event Handlers

        /// <summary>
        /// Url chacked changed event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RadioButtonUrl_CheckedChanged(Object sender, EventArgs e)
        {
            if (this.radioButtonUrl.Checked)
                this.labelUrl.Text = "Link";
            else
                this.labelUrl.Text = "Country";
        }

        /// <summary>
        /// Country checked changed event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radioButtonCountry_CheckedChanged(Object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Confirm button click event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonConfirm_Click(Object sender, EventArgs e)
        {
            if (this.radioButtonUrl.Checked)
                this._url = this.textBoxUrl.Text;
            else
                this._country = this.textBoxUrl.Text;

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
            Dispose();
        }

        #endregion Event Handlers

        #region Methods

        #endregion Methods
    }
}
