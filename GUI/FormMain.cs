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
using MongoDB.Bson;

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

        private void ShowNotes(List<NotenizerNote> notes)
        {
            foreach (NotenizerNote noteLoop in notes)
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
            NotenizerNote notenizerNote = (sender as NotenizerAdvancedTextBox).Note;
            FormReorderNote frmReorderNote = new FormReorderNote(notenizerNote);

            if (frmReorderNote.ShowDialog() == DialogResult.OK)
            {
                notenizerNote.Replace(frmReorderNote.NoteParts);

                this._advancedProgressBar.Start();

                Task.Factory.StartNew(() =>
                {
                    String noteId;
                    String ruleId;
                    BsonDocument ruleDoc;

                    if (notenizerNote.Rule == null)
                        throw new Exception("NotenizerNote.Rule is null!");

                    notenizerNote.Rule.UpdatedAt = DateTime.Now;
                    if (notenizerNote.Rule.CreatedBy == nsEnums.CreatedBy.Notenizer)    // insert
                    {
                        notenizerNote.Rule.CreatedBy = nsEnums.CreatedBy.User;

                        ruleDoc = DocumentCreator.CreateNoteRuleDocument(notenizerNote.Rule, notenizerNote);
                        ruleId = DB.InsertToCollection(DBConstants.NoteRulesCollectionName, ruleDoc).Result;
                    }
                    else                                                                // update
                    {
                        ruleDoc = DocumentCreator.CreateNoteRuleDocument(notenizerNote.Rule, notenizerNote);
                        ruleId = DB.ReplaceInCollection(DBConstants.NoteRulesCollectionName, notenizerNote.Rule.ID, ruleDoc).Result;
                    }

                    // UPDATE only user-created rules, not Notenizer-created!
                    // which value of originalSentence is same as value persisted in DB
                    // We do not want to update note in DB, if we processed SIMILAR but not THE SAME sentence
                    if (notenizerNote.Rule.CreatedBy == nsEnums.CreatedBy.User 
                        && notenizerNote.Rule.Note.OriginalSentence.Trim() == notenizerNote.OriginalSentence.ToString().Trim())
                    {
                        notenizerNote.UpdatedAt = DateTime.Now;
                        noteId = DB.ReplaceInCollection(DBConstants.NotesCollectionName, notenizerNote.Rule.Note.ID, DocumentCreator.CreateNoteDocument(notenizerNote, String.Empty, ruleId, String.Empty)).Result;
                    }
                    else
                    {
                        notenizerNote.CreatedBy = nsEnums.CreatedBy.User;
                        notenizerNote.CreatedAt = DateTime.Now;
                        notenizerNote.UpdatedAt = notenizerNote.CreatedAt;

                        BsonDocument noteDoc = DocumentCreator.CreateNoteDocument(notenizerNote, String.Empty, ruleId, String.Empty);
                        noteId = DB.InsertToCollection(DBConstants.NotesCollectionName, noteDoc).Result;
                        notenizerNote.Rule = DocumentParser.ParseNoteRule(ruleDoc);
                    }
                }).ContinueWith(delegate
                {
                    (sender as NotenizerAdvancedTextBox).PerformSafely(() => (sender as NotenizerAdvancedTextBox).AdvancedTextBox.TextBox.Text = notenizerNote.Value);
                    this._advancedProgressBar.StopAndReset();
                });
            }
        }

        #endregion Event Handlers
    }
}
