using nsComponents;
using nsNotenizer;
using nsNotenizerObjects;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;
using nsExtensions;
using nsConstants;
using nsDB;

namespace nsGUI
{
    public partial class FormMain : Form
    {
        private Notenizer _notenizer;

        public FormMain()
        {
            _notenizer = new Notenizer();

            InitializeComponent();
            this.CenterToScreen();
        }

        private void ShowNotes(List<Note> notes)
        {
            foreach (Note noteLoop in notes)
            {
                this._tableLayoutPanelMain.PerformSafely(() => this._tableLayoutPanelMain.RowCount += 1);

                this._tableLayoutPanelMain.PerformSafely(() => this._tableLayoutPanelMain.Controls.Add(new AdvancedTextBox(noteLoop.OriginalSentence.ToString()),
                    0, this._tableLayoutPanelMain.RowCount - 1));

                NotenizerAdvancedTextBox nAdvTextBox = new NotenizerAdvancedTextBox(noteLoop);
                nAdvTextBox.EditButtonClicked += NoteEditButton_Click;

                this._tableLayoutPanelMain.PerformSafely(() => this._tableLayoutPanelMain.Controls.Add(nAdvTextBox, 1,
                    this._tableLayoutPanelMain.RowCount - 1));
                this._tableLayoutPanelMain.PerformSafely(() => this._tableLayoutPanelMain.RowStyles.Add(new RowStyle(SizeType.Absolute, ComponentConstants.NotenizerAdvancedTextBoxSize + 15F)));
            }
        }

        private void Clear()
        {
            this._tableLayoutPanelMain.SuspendLayout();

            // do not remove first row
            while (this._tableLayoutPanelMain.RowCount > 1)
            {
                int row = this._tableLayoutPanelMain.RowCount - 1;

                for (int i = 0; i < this._tableLayoutPanelMain.ColumnCount; i++)
                {
                    Control control = this._tableLayoutPanelMain.GetControlFromPosition(i, row);
                    this._tableLayoutPanelMain.Controls.Remove(control);
                    control.Dispose();
                }

                this._tableLayoutPanelMain.RowStyles.RemoveAt(row);
                this._tableLayoutPanelMain.RowCount--;
            }

            // show new changed
            this._tableLayoutPanelMain.ResumeLayout(false);
            this._tableLayoutPanelMain.PerformLayout();
        }

        #region Event Handlers

        private void Menu_Quit(object sendet, EventArgs e)
        {
            if (MessageBox.Show("Do you really want to quit?", "Quit confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                this.Dispose();
        }

        private void Menu_Open(object sender, EventArgs e)
        {
            FormTextInputer frmTextInputer = new FormTextInputer();

            if (frmTextInputer.ShowDialog() == DialogResult.OK)
            {
                string textForProcessing = frmTextInputer.TextForProcessing;
                this._advancedProgressBar.Start();

                Task.Factory.StartNew(() =>
                {
                    this._notenizer.RunCoreNLP(textForProcessing);
                }).ContinueWith(delegate {
                    ShowNotes(this._notenizer.Notes);
                    this._advancedProgressBar.StopAndReset();
                });
            }
        }

        private void Menu_Clear(object sender, EventArgs e)
        {
            Clear();
        }

        private void NoteEditButton_Click(object sender, EventArgs e)
        {
            Note note = (sender as NotenizerAdvancedTextBox).Note;
            FormReorderNote frmReorderNote = new FormReorderNote(note);

            if (frmReorderNote.ShowDialog() == DialogResult.OK)
            {
                note.Replace(frmReorderNote.NoteParts);
                note.CreatedBy = nsEnums.CreatedBy.User;
                note.CreatedAt = DateTime.Now;

                this._advancedProgressBar.Start();

                Task.Factory.StartNew(() =>
                {
                    String _id = DB.InsertToCollection(DBConstants.NotesCollectionName, DocumentCreator.CreateNoteDocument(note, -1)).Result;
                }).ContinueWith(delegate
                {
                    (sender as NotenizerAdvancedTextBox).PerformSafely(() => (sender as NotenizerAdvancedTextBox).AdvancedTextBox.TextBox.Text = note.Value);
                    this._advancedProgressBar.StopAndReset();
                });
            }
        }

        #endregion Event Handlers
    }
}
