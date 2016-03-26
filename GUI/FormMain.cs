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
using nsParsers;
using nsEnums;
using System.Drawing;
using nsExceptions;

namespace nsGUI
{
    public partial class FormMain : Form
    {
        private Notenizer _notenizer;
        private AndParser _andParser;

        public FormMain()
        {
            Init();
        }

        public FormMain(String text)
        {
            Init();
            ProcessText(text);
        }

        private void Init()
        {
            this.Icon = Properties.Resources.AppIcon;

            _notenizer = new Notenizer();
            _andParser = new AndParser();

            InitializeComponent();
            this.CenterToScreen();

            this.menuStrip1.BackColor = Color.FromArgb(255, 106, 77);
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
                nAdvTextBox.AndParseButtonClicked += AndParserButton_Click;

                this._tableLayoutPanelMain.PerformSafely(() => this._tableLayoutPanelMain.Controls.Add(nAdvTextBox, 1,
                    this._tableLayoutPanelMain.RowCount - 1));
                this._tableLayoutPanelMain.PerformSafely(() => this._tableLayoutPanelMain.RowStyles.Add(new RowStyle(SizeType.Absolute, ComponentConstants.NotenizerAdvancedTextBoxSize + 15F)));

                nAdvTextBox.PerformSafely(() => nAdvTextBox.IsAndParserButtonVisible = _andParser.IsParsableSentence(noteLoop.OriginalSentence));
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

        private void ProcessText(String text)
        {
            this._advancedProgressBar.Start();

            Task.Factory.StartNew(() =>
            {
                this._notenizer.RunCoreNLP(text);
            }).ContinueWith(delegate {
                ShowNotes(this._notenizer.Notes);
                this._advancedProgressBar.StopAndReset();
            }).ContinueWith(TaskExceptionHandler.Handle, TaskContinuationOptions.OnlyOnFaulted);
        }

        #region Event Handlers

        private void Menu_Quit(object sender, EventArgs e)
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

                ProcessText(textForProcessing);
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
                    String ruleId = null;
                    BsonDocument ruleDoc;

                    if (notenizerNote.Rule == null)
                        throw new Exception("NotenizerNote.Rule is null!");

                    notenizerNote.Rule.UpdatedAt = DateTime.Now;
                    if (notenizerNote.Rule.CreatedBy == nsEnums.CreatedBy.Notenizer || notenizerNote.Rule.Match.Structure < 100.0)    // insert
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
                        && notenizerNote.Rule.Match.Content == 100)
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

        private void AndParserButton_Click(object sender, EventArgs e)
        {
            NotenizerNote notenizerNote = (sender as NotenizerAdvancedTextBox).Note;
            NotenizerNote parsedAndNote;
            NotePart sourceNotePart;
            int andSetPosition;

            if (notenizerNote.AndParserRule != null)
            {
                parsedAndNote = _notenizer.ApplyRule(notenizerNote.OriginalSentence, notenizerNote.AndParserRule);
                sourceNotePart = parsedAndNote.NoteParts[0];
                andSetPosition = notenizerNote.AndParserRule.SetsPosition;
            }
            else
            {
                parsedAndNote = new NotenizerNote(notenizerNote.OriginalSentence);
                sourceNotePart = new NotePart(notenizerNote.OriginalSentence);
                andSetPosition = 0;
            }

            List<NotePart> noteParts = new List<NotePart>();
            List<NoteParticle> andSets = _andParser.GetAndSets(notenizerNote.OriginalSentence);

            foreach (NoteParticle andSetLoop in andSets)
            {
                NotePart notePart = sourceNotePart.Clone();

                if (notenizerNote.AndParserRule == null || andSetPosition == 0)
                    notePart.NoteParticles.Insert(andSetPosition, andSetLoop);
                else
                    notePart.NoteParticles.Insert(notePart.InitializedNoteParticles[andSetPosition - 1].NoteDependency.Position + 1, andSetLoop);

                noteParts.Add(notePart);
            }

            FormAndParser frmAndParser = new FormAndParser(parsedAndNote, noteParts, andSetPosition);
            if (frmAndParser.ShowDialog() == DialogResult.OK)
            {
                this._advancedProgressBar.Start();

                Task.Factory.StartNew(() =>
                {
                    String ruleId;
                    NotenizerDependencies dependencies = new NotenizerDependencies();

                    foreach (NoteParticle noteParticleLoop in frmAndParser.ActiveNotePart.InitializedNoteParticles)
                        dependencies.Add(noteParticleLoop.NoteDependency);

                    if (notenizerNote.AndParserRule == null)
                        notenizerNote.AndParserRule = new NotenizerAndParserRule(dependencies, CreatedBy.User, frmAndParser.AndSetsPosition, dependencies.Count);
                    else
                    {
                        notenizerNote.AndParserRule.RuleDependencies = dependencies;
                        notenizerNote.AndParserRule.SetsPosition = frmAndParser.AndSetsPosition;
                        notenizerNote.AndParserRule.SentenceEnd = dependencies.Count;
                    }
                    BsonDocument andParserRuleDoc = DocumentCreator.CreateAndParserRuleDocument(notenizerNote.AndParserRule);

                    if (notenizerNote.AndParserRule.ID == String.Empty)
                    {
                        ruleId = DB.InsertToCollection(DBConstants.AndParserRulesCollectionName, andParserRuleDoc).Result;
                        notenizerNote.AndParserRule.ID = ruleId;
                        // update v note kolekcii
                        String temp = DB.Update(DBConstants.NotesCollectionName, notenizerNote.Rule.Note.ID, DBConstants.AndParserRuleRefIdFieldName, ObjectId.Parse(ruleId)).Result;
                    }
                    else
                        ruleId = DB.ReplaceInCollection(DBConstants.AndParserRulesCollectionName, notenizerNote.AndParserRule.ID, andParserRuleDoc).Result;

                }).ContinueWith(delegate
                {
                    this._advancedProgressBar.StopAndReset();
                });
            }
        }

        #endregion Event Handlers
    }
}
