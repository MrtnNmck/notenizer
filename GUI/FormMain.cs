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
using System.IO;
using System.Linq;

namespace nsGUI
{
    public partial class FormMain : Form
    {
        #region Variables

        private Notenizer _notenizer;
        private AndParser _andParser;
        private List<NotenizerAdvancedTextBox> _noteTextBoxes;

        #endregion Variables

        #region Constructors

        public FormMain()
        {
            Init();
        }

        public FormMain(String text)
        {
            Init();
            ProcessText(text);
        }

        #endregion Constuctors

        #region Properties

        #endregion Properties

        #region Event Handlers

        private void Menu_Quit(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you really want to quit?", "Quit confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                this.Dispose();
        }

        private void Menu_Open(object sender, EventArgs e)
        {
            ProcessText(new FormTextInputer());
        }

        private void Menu_Clear(object sender, EventArgs e)
        {
            Clear();
        }

        private void Menu_Export(Object sender, EventArgs e)
        {
            String fileToSavePath = GetSaveFileLocation("txt files (*.txt)|*.txt");

            if (fileToSavePath.IsNullOrEmpty())
                return;

            ExportToTextFile(fileToSavePath, this._noteTextBoxes.Select(x => x.AdvancedTextBox.TextBox.Text));
        }

        private void Menu_OpenFile(Object sender, EventArgs e)
        {
            String fileToOpenPath = GetOpenFileLocation("txt files (*.txt)|*.txt");

            if (fileToOpenPath.IsNullOrEmpty())
                return;

            ProcessText(new FormTextInputer(GetTextFromFile(fileToOpenPath)));
        }

        private void NoteEditButton_Click(object sender, EventArgs e)
        {
            NotenizerNote notenizerNote = (sender as NotenizerAdvancedTextBox).Note;
            FormEditNote formEditNote = CreateFormEditNote(notenizerNote);

            if (formEditNote.ShowDialog() == DialogResult.OK)
            {
                Task.Factory.StartNew(() =>
                {
                    SaveData(notenizerNote, formEditNote);
                })
                .ContinueWith(delegate
                {
                    (sender as NotenizerAdvancedTextBox).PerformSafely(() => (sender as NotenizerAdvancedTextBox).AdvancedTextBox.TextBox.Text = notenizerNote.Value);
                    this._advancedProgressBar.StopAndReset();
                })
                .ContinueWith(TaskExceptionHandler.Handle, TaskContinuationOptions.OnlyOnFaulted);
            }
        }

        #endregion Event Handlers

        #region Methods

        private void Init()
        {
            this.Icon = Properties.Resources.AppIcon;

            this._notenizer = new Notenizer();
            this._andParser = new AndParser();
            this._noteTextBoxes = new List<NotenizerAdvancedTextBox>();

            InitializeComponent();
            this.CenterToScreen();

            this.menuStrip1.BackColor = Color.FromArgb(255, 106, 77);
        }

        private void ShowNotes(List<NotenizerNote> notes)
        {
            NotenizerAdvancedTextBox nAdvTextBox;

            foreach (NotenizerNote noteLoop in notes)
            {
                nAdvTextBox = new NotenizerAdvancedTextBox(noteLoop);
                nAdvTextBox.EditButtonClicked += NoteEditButton_Click;
                this._noteTextBoxes.Add(nAdvTextBox);

                this._tableLayoutPanelMain.PerformSafely(() => this._tableLayoutPanelMain.RowCount += 1);
                this._tableLayoutPanelMain.PerformSafely(() => this._tableLayoutPanelMain.Controls.Add(
                    new AdvancedTextBox(noteLoop.OriginalSentence.ToString()),
                    0, 
                    this._tableLayoutPanelMain.RowCount - 1));

                this._tableLayoutPanelMain.PerformSafely(() => this._tableLayoutPanelMain.Controls.Add(
                    nAdvTextBox, 
                    1,
                    this._tableLayoutPanelMain.RowCount - 1));

                this._tableLayoutPanelMain.PerformSafely(() => this._tableLayoutPanelMain.RowStyles.Add(
                    new RowStyle(SizeType.Absolute, ComponentConstants.NotenizerAdvancedTextBoxSize + 15F)));
            }
        }

        private void Clear()
        {
            int row;
            Control control;
            this._tableLayoutPanelMain.SuspendLayout();

            // do not remove first row
            while (this._tableLayoutPanelMain.RowCount > 1)
            {
                row = this._tableLayoutPanelMain.RowCount - 1;

                for (int i = 0; i < this._tableLayoutPanelMain.ColumnCount; i++)
                {
                    control = this._tableLayoutPanelMain.GetControlFromPosition(i, row);
                    this._tableLayoutPanelMain.Controls.Remove(control);
                    control.Dispose();
                }

                this._tableLayoutPanelMain.RowStyles.RemoveAt(row);
                this._tableLayoutPanelMain.RowCount--;
            }

            this._noteTextBoxes.Clear();
            this._noteTextBoxes = null;

            // show new changes
            this._tableLayoutPanelMain.ResumeLayout(false);
            this._tableLayoutPanelMain.PerformLayout();
        }

        private void ProcessText(FormTextInputer formTextInputer)
        {
            if (formTextInputer.ShowDialog() == DialogResult.OK)
                ProcessText(formTextInputer.TextForProcessing);
        }

        private void ProcessText(String text)
        {
            this._advancedProgressBar.Start();

            Task.Factory.StartNew(() =>
            {
                this._notenizer.RunCoreNLP(text);
            })
            .ContinueWith(delegate
            {
                ShowNotes(this._notenizer.Notes);
                this._advancedProgressBar.StopAndReset();
            })
            .ContinueWith(TaskExceptionHandler.Handle, TaskContinuationOptions.OnlyOnFaulted);
        }

        private FormEditNote CreateFormEditNote(NotenizerNote note)
        {
            NotenizerNote parsedAndNote;
            NotePart sourceNotePart;
            int andSetPosition;
            FormEditNote formEditNote;

            if (note.AndParserRule != null)
            {
                parsedAndNote = _notenizer.ApplyRule(note.OriginalSentence, note.AndParserRule);
                sourceNotePart = parsedAndNote.NoteParts[0];
                andSetPosition = note.AndParserRule.SetsPosition;
            }
            else
            {
                parsedAndNote = new NotenizerNote(note.OriginalSentence);
                sourceNotePart = new NotePart(note.OriginalSentence);
                andSetPosition = 0;
            }

            if (_andParser.IsParsableSentence(note.OriginalSentence))
            {
                List<NotePart> noteParts = new List<NotePart>();
                List<NoteParticle> andSets = _andParser.GetAndSets(note.OriginalSentence);

                foreach (NoteParticle andSetLoop in andSets)
                {
                    NotePart notePart = sourceNotePart.Clone();

                    if (note.AndParserRule == null || andSetPosition == 0)
                        notePart.NoteParticles.Insert(andSetPosition, andSetLoop);
                    else
                        notePart.NoteParticles.Insert(notePart.InitializedNoteParticles[andSetPosition - 1].NoteDependency.Position + 1, andSetLoop);

                    noteParts.Add(notePart);
                }

                formEditNote = new FormEditNote(note, parsedAndNote, noteParts, andSetPosition);
            }
            else
            {
                formEditNote = new FormEditNote(note);
            }

            return formEditNote;
        }

        private void SaveData(NotenizerNote note, FormEditNote formEditNote)
        {
            SaveData(note, formEditNote.NoteNoteParts, formEditNote.AndParserEnabled, formEditNote.AndParserNotePart, formEditNote.AndSetPosition);
        }

        private void SaveData(NotenizerNote note, List<NotePart> noteNoteParts, bool andParserEnabled, NotePart andParserNotePart, int andSetPosition)
        {
            // UPDATE only user-created rules, not Notenizer-created!
            // which value of originalSentence is same as value persisted in DB
            // We do not want to update note in DB, if we processed SIMILAR but not THE SAME sentence

            note.Replace(noteNoteParts);

            this._advancedProgressBar.Start();

            String noteId;
            String ruleId = null;
            String andParserRuleId = String.Empty;
            BsonDocument ruleDoc;

            if (note.Rule == null)
                throw new Exception("NotenizerNote.Rule is null!");

            note.Rule.UpdatedAt = DateTime.Now;
            if (note.Rule.CreatedBy == nsEnums.CreatedBy.Notenizer || note.Rule.Match.Structure < 100.0)    // insert
            {
                note.Rule.CreatedBy = nsEnums.CreatedBy.User;

                ruleDoc = DocumentCreator.CreateNoteRuleDocument(note.Rule, note);
                ruleId = DB.InsertToCollection(DBConstants.NoteRulesCollectionName, ruleDoc).Result;
            }
            else                                                                                            // update
            {
                ruleDoc = DocumentCreator.CreateNoteRuleDocument(note.Rule, note);
                ruleId = DB.ReplaceInCollection(DBConstants.NoteRulesCollectionName, note.Rule.ID, ruleDoc).Result;
            }

            if (andParserEnabled)
            {
                NotenizerDependencies andParserDependencies = new NotenizerDependencies();

                foreach (NoteParticle noteParticleLoop in andParserNotePart.InitializedNoteParticles)
                    andParserDependencies.Add(noteParticleLoop.NoteDependency);

                if (note.AndParserRule == null)
                    note.AndParserRule = new NotenizerAndParserRule(andParserDependencies, CreatedBy.User, andSetPosition, andParserDependencies.Count);
                else
                {
                    note.AndParserRule.RuleDependencies = andParserDependencies;
                    note.AndParserRule.SetsPosition = andSetPosition;
                    note.AndParserRule.SentenceEnd = andParserDependencies.Count;
                }

                BsonDocument andParserRuleDoc = DocumentCreator.CreateAndParserRuleDocument(note.AndParserRule);

                if (note.AndParserRule.ID == String.Empty)
                    andParserRuleId = DB.InsertToCollection(DBConstants.AndParserRulesCollectionName, andParserRuleDoc).Result;
                else
                    andParserRuleId = DB.ReplaceInCollection(DBConstants.AndParserRulesCollectionName, note.AndParserRule.ID, andParserRuleDoc).Result;
            }

            if (note.Rule.CreatedBy == nsEnums.CreatedBy.User
                && note.Rule.Match.Content == 100)
            {
                note.UpdatedAt = DateTime.Now;
                noteId = DB.ReplaceInCollection(DBConstants.NotesCollectionName, note.Rule.Note.ID, DocumentCreator.CreateNoteDocument(note, String.Empty, ruleId, andParserRuleId)).Result;
            }
            else
            {
                note.CreatedBy = nsEnums.CreatedBy.User;
                note.CreatedAt = DateTime.Now;
                note.UpdatedAt = note.CreatedAt;

                BsonDocument noteDoc = DocumentCreator.CreateNoteDocument(note, String.Empty, ruleId, andParserRuleId);
                noteId = DB.InsertToCollection(DBConstants.NotesCollectionName, noteDoc).Result;
                note.Rule = DocumentParser.ParseNoteRule(ruleDoc);
                note.Rule.Match = new Match(100, 100);
                note.Rule.Note = DocumentParser.ParseNote(noteDoc);
            }
        }
        
        private String GetSaveFileLocation(String filter)
        {
            SaveFileDialog dialog = new SaveFileDialog();

            dialog.Filter = filter;
            dialog.Title = "Choose file to save";

            if (dialog.ShowDialog() == DialogResult.OK)
                return dialog.FileName;

            return null;
        }

        private String GetOpenFileLocation(String filter)
        {
            OpenFileDialog dialog = new OpenFileDialog();

            dialog.Filter = filter;
            dialog.Title = "Choose file to open";

            if (dialog.ShowDialog() == DialogResult.OK)
                return dialog.FileName;

            return null;
        }

        private void ExportToTextFile(String filePath, IEnumerable<String> values)
        {
            File.WriteAllLines(filePath, values);

            if (MessageBox.Show(
                "Do you want to open saved file?",
                "Open file",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question) == DialogResult.Yes)
                System.Diagnostics.Process.Start(filePath);
        }

        private String GetTextFromFile(String filePath)
        {
            return File.ReadAllText(filePath);
        }

        #endregion Methods
    }
}
