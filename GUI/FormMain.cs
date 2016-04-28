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
using System.Linq;
using nsServices.DBServices;
using nsServices.WebServices;
using nsServices.FileServices;

namespace nsGUI
{
    public partial class FormMain : Form
    {
        #region Variables

        private Notenizer _notenizer;
        private AndParser _andParser;
        private List<NotenizerAdvancedTextBox> _noteTextBoxes;
        private String _article;

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
            String fileToSavePath = FileManager.GetSaveFileLocation("txt files (*.txt)|*.txt");

            if (fileToSavePath.IsNullOrEmpty())
                return;

            FileManager.SaveTextToFile(fileToSavePath, this._noteTextBoxes.Select(x => x.AdvancedTextBox.TextBox.Text));
        }

        private void Menu_OpenFile(Object sender, EventArgs e)
        {
            String fileToOpenPath = FileManager.GetOpenFileLocation("txt files (*.txt)|*.txt");

            if (fileToOpenPath.IsNullOrEmpty())
                return;

            ProcessText(new FormTextInputer(FileManager.GetTextFromFile(fileToOpenPath)));
        }

        private void Menu_OpenLink(Object sender, EventArgs e)
        {
            FormOpenLink formOpenLink = new FormOpenLink();

            if (formOpenLink.ShowDialog() == DialogResult.OK)
            {
                if (formOpenLink.Url != String.Empty)
                {
                    if (!WebParser.UrlExists(formOpenLink.Url))
                        MessageBox.Show(formOpenLink.Url + " is not a valid URL.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    else
                    ProcessText(new FormTextInputer(WikiParser.Parse(formOpenLink.Url)));
                }
                else
                    ProcessText(new FormTextInputer(WikiParser.ParseCountry(formOpenLink.Country)));
            }
                
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
                    (sender as NotenizerAdvancedTextBox).PerformSafely(() => (sender as NotenizerAdvancedTextBox).AdvancedTextBox.TextBox.Text = notenizerNote.Text);
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

            // show new changes
            this._tableLayoutPanelMain.ResizeToOriginal();
        }

        private void ProcessText(FormTextInputer formTextInputer)
        {
            if (formTextInputer.ShowDialog() == DialogResult.OK)
                ProcessText(formTextInputer.TextForProcessing);
        }

        private void ProcessText(String text)
        {
            this._article = text;
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

            if (note.AndRule != null)
            {
                parsedAndNote = _notenizer.ApplyRule(note.OriginalSentence, note.AndRule);
                sourceNotePart = parsedAndNote.NoteParts[0];
                andSetPosition = note.AndRule.SetsPosition;
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

                    if (note.AndRule == null || andSetPosition == 0)
                        notePart.NoteParticles.Insert(andSetPosition, andSetLoop);
                    else
                    {
                        if (andSetPosition >= notePart.InitializedNoteParticles.Count)
                            andSetPosition = notePart.InitializedNoteParticles.Count == 0 ? 0 : notePart.InitializedNoteParticles.Count - 1;

                        if (andSetPosition == 0)
                            notePart.NoteParticles.Insert(andSetPosition, andSetLoop);
                        else
                            notePart.NoteParticles.Insert(notePart.InitializedNoteParticles[andSetPosition - 1].NoteDependency.Position + 1, andSetLoop);
                    }

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
            note.Replace(noteNoteParts);
            note.Rule.SentencesTerminators = note.SentencesTerminators;

            this._advancedProgressBar.Start();

            if (note.Rule.Match.Structure < NotenizerConstants.MaxMatchValue 
                && note.Rule.Match.Content < NotenizerConstants.MaxMatchValue
                && note.Rule.Match.Value < NotenizerConstants.MaxMatchValue)
            {
                InsertRule(note);

                if (andParserEnabled)
                    InsertAndRule(note, andParserNotePart, andSetPosition);

                InsertNote(note);
                note.OriginalSentence.Sentence.StructureID = InsertStructure(note.OriginalSentence.Structure);
                InsertSentence(note);

                note.Rule.Sentence = note.OriginalSentence.Sentence;
            }
            else if (note.Rule.Match.Structure == NotenizerConstants.MaxMatchValue
                     && note.Rule.Match.Value < NotenizerConstants.MaxMatchValue)
            {
                UpdateRule(note);

                if (andParserEnabled)
                    UpdateAndRule(note, andParserNotePart, andSetPosition);

                InsertNote(note);
                InsertSentence(note);

                note.Rule.Sentence = note.OriginalSentence.Sentence;
            }
            else if (note.Rule.Match.Structure == NotenizerConstants.MaxMatchValue
                     && note.Rule.Match.Content == NotenizerConstants.MaxMatchValue
                     && note.Rule.Match.Value == NotenizerConstants.MaxMatchValue)
            {
                UpdateRule(note);

                if (andParserEnabled)
                    UpdateAndRule(note, andParserNotePart, andSetPosition);

                UpdateNote(note);
            }

            note.Rule.Match = new Match(NotenizerConstants.MaxMatchValue);
        }

        private void UpdateNote(NotenizerNote note)
        {
            note.Note.UpdatedAt = DateTime.Now;
            note.Note.Text = note.Text;
            note.Note.AndRuleID = note.AndRule == null ? String.Empty : note.AndRule.ID;

            note.Note.ID = DB.ReplaceInCollection(
                DBConstants.NotesCollectionName,
                note.Note.ID,
                DocumentCreator.CreateNoteDocument(
                    note,
                    note.Note.RuleID,
                    note.Note.AndRuleID)).Result;
        }

        private void UpdateRule(NotenizerNote note)
        {
            note.Structure.Structure.UpdatedAt = DateTime.Now;
            note.Structure.Structure.Dependencies = note.Structure.Dependencies;
            note.Structure.Structure.ID = UpdateStructure(note.Structure, true);
            note.Rule.UpdatedAt = DateTime.Now;

            note.Rule.ID = DB.ReplaceInCollection(
                DBConstants.RulesCollectionName,
                note.Rule.ID,
                DocumentCreator.CreateRuleDocument(
                    note.Rule)).Result;
        }

        private void UpdateAndRule(NotenizerNote note, NotePart andParserNotePart, int andSetPosition)
        {
            NotenizerDependencies andParserDependencies = new NotenizerDependencies();

            foreach (NoteParticle noteParticleLoop in andParserNotePart.InitializedNoteParticles)
                andParserDependencies.Add(noteParticleLoop.NoteDependency);

            if (note.AndRule == null)
            {
                note.AndRule = new NotenizerAndRule(andParserDependencies, andSetPosition, andParserDependencies.Count);
                note.AndRule.CreatedAt = note.AndRule.UpdatedAt = DateTime.Now;
                note.AndRule.Structure.Structure.ID = InsertStructure(note.AndRule.Structure, true);

                note.AndRule.ID = DB.InsertToCollection(
                    DBConstants.AndRulesCollectionName,
                    DocumentCreator.CreateRuleDocument(
                        note.AndRule)).Result;
            }
            else
            {
                note.AndRule.UpdatedAt = DateTime.Now;
                note.AndRule.Structure.Structure.Dependencies = andParserDependencies;
                note.AndRule.Structure.Dependencies = andParserDependencies;
                note.AndRule.Structure.CompressedDependencies = new CompressedDependencies(andParserDependencies);
                note.AndRule.SetsPosition = andSetPosition;
                note.AndRule.SentenceTerminator = andParserDependencies.Count;

                note.AndRule.Structure.Structure.ID = UpdateStructure(note.AndRule.Structure, true);

                note.AndRule.ID = DB.ReplaceInCollection(
                    DBConstants.AndRulesCollectionName,
                    note.AndRule.ID,
                    DocumentCreator.CreateRuleDocument(
                        note.AndRule)).Result;
            }
        }

        private void InsertRule(NotenizerNote note)
        {
            note.Structure.Structure.CreatedAt = note.Structure.Structure.UpdatedAt = DateTime.Now;
            note.Rule.CreatedAt = note.Rule.UpdatedAt = DateTime.Now;
            note.Structure.Structure.Dependencies = note.Structure.Dependencies;
            note.Structure.Structure.ID = InsertStructure(note.Structure, true);

            note.Rule.ID = DB.InsertToCollection(
                DBConstants.RulesCollectionName,
                DocumentCreator.CreateRuleDocument(
                    note.Rule)).Result;
        }

        private void InsertAndRule(NotenizerNote note, NotePart andParserNotePart, int andSetPosition)
        {
            NotenizerDependencies andParserDependencies = new NotenizerDependencies();

            foreach (NoteParticle noteParticleLoop in andParserNotePart.InitializedNoteParticles)
                andParserDependencies.Add(noteParticleLoop.NoteDependency);

            note.AndRule = new NotenizerAndRule(andParserDependencies, andSetPosition, andParserDependencies.Count);
            note.AndRule.Structure.Structure.UpdatedAt = note.AndRule.Structure.Structure.CreatedAt = DateTime.Now;
            note.AndRule.CreatedAt = note.AndRule.UpdatedAt = DateTime.Now;
            note.AndRule.Structure.Structure.ID = InsertStructure(note.AndRule.Structure, true);

            note.AndRule.ID = DB.InsertToCollection(
                DBConstants.AndRulesCollectionName,
                DocumentCreator.CreateRuleDocument(
                    note.AndRule)).Result;
        }

        private void InsertNote(NotenizerNote note)
        {
            note.Note.CreatedAt = note.Note.UpdatedAt = DateTime.Now;
            note.Note.Text = note.Text;
            note.Note.AndRuleID = note.AndRule == null ? String.Empty : note.AndRule.ID;
            note.Note.RuleID = note.Rule.ID;

            note.Note.ID = DB.InsertToCollection(
                DBConstants.NotesCollectionName,
                DocumentCreator.CreateNoteDocument(
                    note,
                    note.Note.RuleID,
                    note.Note.AndRuleID)).Result;
        }

        private void InsertSentence(NotenizerNote note)
        {
            note.OriginalSentence.Sentence.CreatedAt = note.OriginalSentence.Sentence.UpdatedAt = DateTime.Now;
            note.OriginalSentence.Sentence.ID = DB.InsertToCollection(
                DBConstants.SentencesCollectionName,
                DocumentCreator.CreateSentenceDocument(
                    note.OriginalSentence,
                    note.OriginalSentence.Sentence.StructureID,
                    note.Rule.Sentence.Article.ID,
                    note.Rule.ID,
                    note.AndRule == null ? String.Empty : note.AndRule.ID,
                    note.Note.ID)).Result;
        }

        private String InsertStructure(NotenizerStructure structure, bool additionalInfo = false)
        {
            structure.Structure.CreatedAt = structure.Structure.UpdatedAt = DateTime.Now;

            return DB.InsertToCollection(
                        DBConstants.StructuresCollectionName,
                        DocumentCreator.CreateStructureDocument(
                            structure,
                            additionalInfo)).Result;
        }

        private String UpdateStructure(NotenizerStructure structure, bool additionalInfo = false)
        {
            structure.Structure.UpdatedAt = DateTime.Now;

            return DB.ReplaceInCollection(
                        DBConstants.StructuresCollectionName,
                        structure.Structure.ID,
                        DocumentCreator.CreateStructureDocument(
                            structure,
                            additionalInfo)).Result;
        }

        #endregion Methods
    }
}
