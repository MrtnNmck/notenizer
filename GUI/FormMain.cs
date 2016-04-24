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

            if (note.Rule.Match.Structure < NotenizerConstants.MaxMatchValue 
                && note.Rule.Match.Content < NotenizerConstants.MaxMatchValue
                && note.Rule.Match.Value < NotenizerConstants.MaxMatchValue)
            {
                // no match => we need to create new entries in all collections
                // INSERT
                // article is already in DB at this moment

                note.Rule.Structure = note.Structure;
                note.Rule.SentencesTerminators = note.SentencesTerminators;

                BsonDocument noteStructureDoc = DocumentCreator.CreateStructureDocument(note.Structure);
                BsonDocument sentenceStructureDoc = DocumentCreator.CreateStructureDocument(note.OriginalSentence);

                Object temp = DB.InsertToCollection(DBConstants.StructuresCollectionName, noteStructureDoc).Result;
                note.Rule.Structure = new NotenizerStructure(DocumentParser.ParseStructure(noteStructureDoc));

                BsonDocument newRuleDoc = DocumentCreator.CreateRuleDocument(note.Rule);
                note.Rule.ID = DB.InsertToCollection(DBConstants.NoteRulesCollectionName, newRuleDoc).Result;

                String andRuleId = String.Empty;
                if (andParserEnabled)
                {
                    NotenizerDependencies andParserDependencies = new NotenizerDependencies();

                    foreach (NoteParticle noteParticleLoop in andParserNotePart.InitializedNoteParticles)
                        andParserDependencies.Add(noteParticleLoop.NoteDependency);

                    if (note.AndRule == null)
                        note.AndRule = new NotenizerAndRule(andParserDependencies, andSetPosition, andParserDependencies.Count);
                    else
                    {
                        note.AndRule.Structure = new NotenizerStructure(andParserDependencies);
                        note.AndRule.SetsPosition = andSetPosition;
                        note.AndRule.SentenceTerminator = andParserDependencies.Count;
                    }

                    BsonDocument andRuleStructureDoc = DocumentCreator.CreateStructureDocument(note.AndRule.Structure);
                    temp = DB.InsertToCollection(DBConstants.StructuresCollectionName, andRuleStructureDoc);

                    note.AndRule.Structure.Structure = DocumentParser.ParseStructure(andRuleStructureDoc);
                    BsonDocument andRuleDoc = DocumentCreator.CreateRuleDocument(note.AndRule);
                    note.AndRule.ID = DB.InsertToCollection(DBConstants.AndParserRulesCollectionName, andRuleDoc).Result;
                }

                note.Note = new Note(note.Text);
                BsonDocument newNoteDoc = DocumentCreator.CreateNoteDocument(note, note.Rule.ID, note.AndRule.ID);
                temp = DB.InsertToCollection(DBConstants.NotesCollectionName, newNoteDoc);
                note.Note = DocumentParser.ParseNote(newNoteDoc);


                note.OriginalSentence.Sentence.StructureID = DB.InsertToCollection(DBConstants.StructuresCollectionName, sentenceStructureDoc).Result;
                BsonDocument newSentenceDocument = DocumentCreator.CreateSentenceDocument(note.OriginalSentence, note.OriginalSentence.Sentence.StructureID, note.OriginalSentence.Sentence.ArticleID,
                    note.Rule.ID, note.AndRule.ID, note.Note.ID);
                temp = DB.InsertToCollection(DBConstants.SentencesCollectionName, newSentenceDocument).Result;
                note.OriginalSentence.Sentence = DocumentParser.ParseSentence(newSentenceDocument);


                //note/*.Rule.*/Note = note.Note;
                note.Rule.Match = new Match(NotenizerConstants.MaxMatchValue);
                note.Rule.Sentence = note.OriginalSentence.Sentence;

                //note.AndRule.Note = note.Note;
                note.AndRule.Match = new Match(NotenizerConstants.MaxMatchValue);
                note.AndRule.Sentence = note.OriginalSentence.Sentence;
            }
            else if (note.Rule.Match.Structure == NotenizerConstants.MaxMatchValue
                     && note.Rule.Match.Value < NotenizerConstants.MaxMatchValue)
            {
                note.Rule.Structure = note.Structure;
                note.Rule.SentencesTerminators = note.SentencesTerminators;

                BsonDocument noteStructureDoc = DocumentCreator.CreateStructureDocument(note.Structure);
                Object temp = DB.InsertToCollection(DBConstants.StructuresCollectionName, noteStructureDoc).Result;
                note.Rule.Structure = new NotenizerStructure(DocumentParser.ParseStructure(noteStructureDoc));

                BsonDocument updateRuleDoc = DocumentCreator.CreateRuleDocument(note.Rule);
                note.Rule.ID = DB.ReplaceInCollection(DBConstants.NoteRulesCollectionName, note.Rule.ID, updateRuleDoc).Result;

                String andRuleId = String.Empty;
                if (andParserEnabled)
                {
                    NotenizerDependencies andParserDependencies = new NotenizerDependencies();

                    foreach (NoteParticle noteParticleLoop in andParserNotePart.InitializedNoteParticles)
                        andParserDependencies.Add(noteParticleLoop.NoteDependency);

                    if (note.AndRule == null)
                        note.AndRule = new NotenizerAndRule(andParserDependencies, andSetPosition, andParserDependencies.Count);
                    else
                    {
                        note.AndRule.Structure = new NotenizerStructure(andParserDependencies);
                        note.AndRule.SetsPosition = andSetPosition;
                        note.AndRule.SentenceTerminator = andParserDependencies.Count;
                    }

                    BsonDocument andRuleStructureDoc = DocumentCreator.CreateStructureDocument(note.AndRule.Structure);
                    temp = DB.InsertToCollection(DBConstants.StructuresCollectionName, andRuleStructureDoc);

                    note.AndRule.Structure.Structure = DocumentParser.ParseStructure(andRuleStructureDoc);
                    BsonDocument andRuleDoc = DocumentCreator.CreateRuleDocument(note.AndRule);
                    note.AndRule.ID = DB.ReplaceInCollection(DBConstants.AndParserRulesCollectionName, note.AndRule.ID, andRuleDoc).Result;
                }

                note.Note = new Note(note.Text);
                BsonDocument newNoteDoc = DocumentCreator.CreateNoteDocument(note, note.Rule.ID, note.AndRule.ID);
                temp = DB.InsertToCollection(DBConstants.NotesCollectionName, newNoteDoc);
                note.Note = DocumentParser.ParseNote(newNoteDoc);

                BsonDocument sentenceStructureDoc = DocumentCreator.CreateStructureDocument(note.OriginalSentence);
                note.OriginalSentence.Sentence.StructureID = DB.InsertToCollection(DBConstants.StructuresCollectionName, sentenceStructureDoc).Result;
                BsonDocument newSentenceDocument = DocumentCreator.CreateSentenceDocument(note.OriginalSentence, note.OriginalSentence.Sentence.StructureID, note.OriginalSentence.Sentence.ArticleID,
                    note.Rule.ID, note.AndRule.ID, note.Note.ID);
                temp = DB.InsertToCollection(DBConstants.SentencesCollectionName, newSentenceDocument).Result;
                note.OriginalSentence.Sentence = DocumentParser.ParseSentence(newSentenceDocument);

                //note.Rule.Note = note.Note;
                note.Rule.Match = new Match(NotenizerConstants.MaxMatchValue);
                note.Rule.Sentence = note.OriginalSentence.Sentence;

                //note.AndRule.Note = note.Note;
                note.AndRule.Match = new Match(NotenizerConstants.MaxMatchValue);
                note.AndRule.Sentence = note.OriginalSentence.Sentence;
            }
            else if (note.Rule.Match.Structure == NotenizerConstants.MaxMatchValue
                     && note.Rule.Match.Content == NotenizerConstants.MaxMatchValue
                     && note.Rule.Match.Value == NotenizerConstants.MaxMatchValue)
            {
                // SAME SENTENCE => UPDATE ALL
                note.Rule.SentencesTerminators = note.SentencesTerminators;

                // update rule
                note.Rule.SentencesTerminators = note.SentencesTerminators;
                note.Structure.Structure.Dependencies = note.Structure.Dependencies;
                note.Structure.Structure.ID = DB.ReplaceInCollection(
                    DBConstants.StructuresCollectionName,
                    note.Structure.Structure.ID,
                    DocumentCreator.CreateStructureDocument(
                        note.Structure,
                        true)).Result;

                note.Rule.ID = DB.ReplaceInCollection(
                    DBConstants.NoteRulesCollectionName,
                    note.Rule.ID,
                    DocumentCreator.CreateRuleDocument(
                        note.Rule)).Result;

                // update and-rule
                if (andParserEnabled)
                {
                    NotenizerDependencies andParserDependencies = new NotenizerDependencies();

                    foreach (NoteParticle noteParticleLoop in andParserNotePart.InitializedNoteParticles)
                        andParserDependencies.Add(noteParticleLoop.NoteDependency);

                    if (note.AndRule == null)
                    {
                        note.AndRule = new NotenizerAndRule(andParserDependencies, andSetPosition, andParserDependencies.Count);
                        note.AndRule.Structure.Structure.ID = DB.InsertToCollection(
                            DBConstants.StructuresCollectionName,
                            DocumentCreator.CreateStructureDocument(
                                note.AndRule.Structure)).Result;

                        note.AndRule.ID = DB.InsertToCollection(
                            DBConstants.AndParserRulesCollectionName,
                            DocumentCreator.CreateRuleDocument(
                                note.AndRule)).Result;
                    }
                    else
                    {
                        note.AndRule.Structure = new NotenizerStructure(andParserDependencies);
                        note.AndRule.Structure.Structure.Dependencies = andParserDependencies;
                        note.AndRule.SetsPosition = andSetPosition;
                        note.AndRule.SentenceTerminator = andParserDependencies.Count;

                        note.AndRule.Structure.Structure.ID = DB.ReplaceInCollection(
                            DBConstants.StructuresCollectionName,
                            note.AndRule.Structure.Structure.ID,
                            DocumentCreator.CreateStructureDocument(
                                note.AndRule.Structure)).Result;

                        note.AndRule.ID = DB.ReplaceInCollection(
                            DBConstants.AndParserRulesCollectionName,
                            note.AndRule.ID,
                            DocumentCreator.CreateRuleDocument(
                                note.AndRule)).Result;
                    }
                }

                // update note
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
        }

        #endregion Methods
    }
}
