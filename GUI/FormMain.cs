using nsComponents;
using nsNotenizer;
using nsNotenizerObjects;
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
    public partial class FormMain : Form
    {
        private Notenizer _notenizer;

        public FormMain()
        {
            _notenizer = new Notenizer();

            InitializeComponent();
            //Test();
            this.CenterToScreen();
        }

        private void Test()
        {
            for (int i = 0; i < 20; i++)
            {
                this._tableLayoutPanelMain.RowCount += 1;
                this._tableLayoutPanelMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
                this._tableLayoutPanelMain.Controls.Add(new NotenizerTextBox() { Text = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa" },
                    0, this._tableLayoutPanelMain.RowCount - 1);
                this._tableLayoutPanelMain.Controls.Add(new NotenizerTextBox() { Text = "bbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbb" },
                    1, this._tableLayoutPanelMain.RowCount - 1);
            }
        }

        #region Event Handlers

        private void Menu_Quit(object sendet, EventArgs e)
        {
            if (MessageBox.Show("Do you really want to quit?", "Quit confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                this.Dispose();
        }

        private void Menu_Open(Object sender, EventArgs e)
        {
            FormTextInputer frmTextInputer = new FormTextInputer();

            if (frmTextInputer.ShowDialog() == DialogResult.OK)
            {
                string textForProcessing = frmTextInputer.TextForProcessing;
                this._progressBar.Show();
                this._progressBar.Style = ProgressBarStyle.Marquee;

                this._notenizer.RunCoreNLP(textForProcessing);

                foreach (Note noteLoop in this._notenizer.Notes)
                {
                    this._tableLayoutPanelMain.RowCount += 1;
                    this._tableLayoutPanelMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
                    this._tableLayoutPanelMain.Controls.Add(new NotenizerTextBox() { Text = noteLoop.OriginalSentence.ToString() },
                        0, this._tableLayoutPanelMain.RowCount - 1);
                    this._tableLayoutPanelMain.Controls.Add(new NotenizerTextBox() { Text = noteLoop.Value },
                        1, this._tableLayoutPanelMain.RowCount - 1);
                }

                this._progressBar.Hide();
            }
        }

        #endregion Event Handlers
    }
}
