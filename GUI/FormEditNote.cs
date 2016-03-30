using nsComponents;
using nsConstants;
using nsEnums;
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
    public partial class FormEditNote : Form
    {
        private List<NotePart> _noteParts;
        private NotenizerNote _note;
        private NotenizerNote _andParserNote;
        private List<NotePart> _parsedAndSets;
        private int _andSetsPosition;
        private NotePart _activeNotePart;
        private bool _andParserEnabled;

        public FormEditNote(NotenizerNote note, NotenizerNote andParserNote, List<NotePart> noteParts, int andSetsPosition)
        {
            this._note = note;
            this._andParserNote = andParserNote;
            this._parsedAndSets = noteParts;
            this._andSetsPosition = andSetsPosition;
            this._andParserEnabled = true;

            InitializeComponent();
            Init();
            InitUniversalControls();
            InitControlsForNoteEdit();
            InitControlsForAndParser();
        }

        public FormEditNote(NotenizerNote note)
        {
            this._note = note;
            this._andParserEnabled = false;
            InitializeComponent();
            Init();
            InitUniversalControls();
            InitControlsForNoteEdit();

            DisableTabPage(this.tabPage2);
        }

        private void Init()
        {
            this.Icon = Properties.Resources.AppIcon;
            this.CenterToParent();
        }

        private void InitUniversalControls()
        {
            this._textBoxNote.Text = _note.Value;
            this._textBoxOriginalSentence.Text = _note.OriginalSentence.ToString();
        }

        private void InitControlsForNoteEdit()
        {
            List<NotenizerDependency> noteDependencies = _note.NoteDependencies;
            List<NotenizerAdvancedLabel> activeLabels = new List<NotenizerAdvancedLabel>();

            foreach (NotenizerDependency noteDependency in noteDependencies)
            {
                activeLabels.Add(new NotenizerAdvancedLabel(noteDependency));
            }

            foreach (NotePart notePartLoop in _note.NoteParts)
            {
                activeLabels.Insert(notePartLoop.InitializedNoteParticles.Count, new NotenizerAdvancedLabel(NotenizerConstants.SentenceTerminator) { RepresentMode = RepresentMode.SentenceEnd });
            }

            this._flowLayoutPanelActive.Controls.AddRange(activeLabels.ToArray<Control>());

            foreach (NotenizerDependency originalSentenceDependencyLoop in _note.UnusedDependencies)
            {
                NotenizerDependency clonedDependency = originalSentenceDependencyLoop.Clone();

                if (!clonedDependency.Relation.IsNominalSubject())
                    clonedDependency.TokenType = TokenType.Dependent;

                this._flowLayoutPanelDeleted.Controls.Add(new NotenizerAdvancedLabel(clonedDependency));
            }
        }

        private void InitControlsForAndParser()
        {
            List<NotenizerAdvancedLabel> activeLabels = new List<NotenizerAdvancedLabel>();

            foreach (NotenizerDependency noteDepenendencyLoop in _andParserNote.NoteDependencies)
                activeLabels.Add(new NotenizerAdvancedLabel(noteDepenendencyLoop));

            activeLabels.Insert(_andSetsPosition, new NotenizerAdvancedLabel(ComponentConstants.AndSetPositionConstant) { RepresentMode = RepresentMode.AndSet, IsDeletable = false });
            _advancedFlowLayoutPanelAndParserActive.Controls.AddRange(activeLabels.ToArray<Control>());

            foreach (NotenizerDependency originalSentenceDependencyLoop in _andParserNote.UnusedDependencies)
            {
                NotenizerDependency clonedDependency = originalSentenceDependencyLoop.Clone();

                if (!clonedDependency.Relation.IsNominalSubject())
                    clonedDependency.TokenType = TokenType.Dependent;

                this._advancedFlowLayoutPanelAndParsedDeleted.Controls.Add(new NotenizerAdvancedLabel(clonedDependency));
            }

            BindingSource bindingSource = new BindingSource();
            _gridNotes.Columns.Clear();

            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("noteColumn") { Caption = "Note" });

            foreach (NotePart parsedAndSetLoop in _parsedAndSets)
            {
                DataRow row = dt.NewRow();
                row["noteColumn"] = parsedAndSetLoop.AdjustedValue;
                dt.Rows.Add(row);
            }

            bindingSource.DataSource = dt;
            this._gridNotes.DataSource = bindingSource;
            _gridNotes.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            _gridNotes.MultiSelect = false;
            _gridNotes.ReadOnly = true;
            _gridNotes.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            _gridNotes.Columns[0].HeaderText = "Note";
        }

        private void DisableTabPage(TabPage tabPage)
        {
            foreach (Control controlLoop in tabPage.Controls)
                controlLoop.Enabled = false;
        }

        public List<NotePart> NoteParts
        {
            get { return this._noteParts; }
        }

        public NotePart AndParserNotePart
        {
            get { return this._activeNotePart; }
        }

        public int AndSetPosition
        {
            get { return this._andSetsPosition; }
        }

        public bool AndParserEnabled
        {
            get { return this._andParserEnabled; }
        }

        #region Event Handlers

        private void ApplyButton_Click(Object sender, EventArgs e)
        {
            this._noteParts = new List<NotePart>();
            NotePart notePart = new NotePart(_note.OriginalSentence);

            _noteParts.Add(notePart);

            for (int i = 0; i < this._flowLayoutPanelActive.Controls.Count; i++)
            {
                NotenizerAdvancedLabel label = (this._flowLayoutPanelActive.Controls[i] as NotenizerAdvancedLabel);

                if (label.RepresentMode == RepresentMode.SentenceEnd)
                {
                    notePart = new NotePart(_note.OriginalSentence);
                    _noteParts.Add(notePart);
                    continue;
                }

                NotenizerDependency dep = label.Dependency;
                dep.Position = i;

                NoteParticle noteParticle = new NoteParticle(dep);
                notePart.Add(noteParticle);
            }

            if (notePart.InitializedNoteParticles.Count == 0)
                _noteParts.Remove(notePart);


            int depCounter = 0;
            _activeNotePart = new NotePart(_note.OriginalSentence);

            for (int i = 0; i < _advancedFlowLayoutPanelAndParserActive.Controls.Count; i++)
            {
                NotenizerAdvancedLabel label = _advancedFlowLayoutPanelAndParserActive.Controls[i] as NotenizerAdvancedLabel;

                if (label.RepresentMode == RepresentMode.AndSet)
                {
                    _andSetsPosition = i;
                    continue;
                }

                NotenizerDependency dep = label.Dependency;
                dep.Position = depCounter;

                NoteParticle noteParticle = new NoteParticle(dep);
                _activeNotePart.Add(noteParticle);

                depCounter++;
            }

            this.DialogResult = DialogResult.OK;
        }

        private void CancelButton_Click(Object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void AddSentenceMenuItem_Click(Object sender, EventArgs e)
        {
            FormAddSentenceTerminator frmAddSentenceTerminator = new FormAddSentenceTerminator();

            if (frmAddSentenceTerminator.ShowDialog() == DialogResult.OK)
            {
                this._flowLayoutPanelDeleted.Controls.Add(new NotenizerAdvancedLabel(frmAddSentenceTerminator.SelectedSentenceTerminator) { RepresentMode = RepresentMode.SentenceEnd });
            }
        }

        #endregion Event Handlers

        private void label7_Click(Object sender, EventArgs e)
        {

        }
    }
}
