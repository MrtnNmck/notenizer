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
    public partial class FormOpenLink : Form
    {
        private String _url = String.Empty;
        private String _country = String.Empty;

        public FormOpenLink()
        {
            InitializeComponent();
            CenterToParent();
        }

        private void RadioButtonUrl_CheckedChanged(Object sender, EventArgs e)
        {
            if (this.radioButtonUrl.Checked)
                this.labelUrl.Text = "Link";
            else
                this.labelUrl.Text = "Country";
        }

        private void radioButtonCountry_CheckedChanged(Object sender, EventArgs e)
        {

        }

        private void ButtonConfirm_Click(Object sender, EventArgs e)
        {
            if (this.radioButtonUrl.Checked)
                this._url = this.textBoxUrl.Text;
            else
                this._country = this.textBoxUrl.Text;

            this.DialogResult = DialogResult.OK;
        }

        private void ButtonCancel_Click(Object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            Dispose();
        }

        public String Url
        {
            get { return this._url; }
        }

        public String Country
        {
            get { return this._country; }
        }
    }
}
