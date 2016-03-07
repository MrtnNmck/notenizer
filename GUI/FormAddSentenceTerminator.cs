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
    public partial class FormAddSentenceTerminator : Form
    {
        private const String _memeberName = "sentenceTerminator";
        private String _selectedSentenceTerminator;

        public FormAddSentenceTerminator()
        {
            InitializeComponent();
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn(_memeberName));

            DataRow newRow = dt.NewRow();
            newRow[_memeberName] = ".";
            dt.Rows.Add(newRow);

            newRow = dt.NewRow();
            newRow[_memeberName] = "!";
            dt.Rows.Add(newRow);

            newRow = dt.NewRow();
            newRow[_memeberName] = "?";
            dt.Rows.Add(newRow);

            this._comboBoxSentenceTerminator.DataSource = dt;
            this._comboBoxSentenceTerminator.DisplayMember = _memeberName;
            this._comboBoxSentenceTerminator.ValueMember = _memeberName;
        }

        public String SelectedSentenceTerminator
        {
            get { return _selectedSentenceTerminator; }
        }

        private void AcceptButton_Click(Object sender, EventArgs e)
        {
            this._selectedSentenceTerminator = this._comboBoxSentenceTerminator.SelectedValue.ToString();

            this.DialogResult = DialogResult.OK;
        }

        private void CancelButton_Click(Object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }
}
