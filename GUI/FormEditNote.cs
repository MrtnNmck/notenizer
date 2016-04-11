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
        #region Variables

        private List<NotePart> _noteParts;
        private NotenizerNote _note;
        private NotenizerNote _andParserNote;
        private List<NotePart> _parsedAndSets;
        private int _andSetsPosition;
        private NotePart _andParserNotePart;
        private bool _andParserEnabled;

        #endregion Variables

        #region Constructors

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

        #endregion Constuctors

        #region Properties

        public List<NotePart> NoteNoteParts
        {
            get { return this._noteParts; }
        }

        public NotePart AndParserNotePart
        {
            get { return this._andParserNotePart; }
        }

        public int AndSetPosition
        {
            get { return this._andSetsPosition; }
        }

        public bool AndParserEnabled
        {
            get { return this._andParserEnabled; }
        }

        #endregion Properties

        #region Event Handlers

        private void ApplyButton_Click(Object sender, EventArgs e)
        {
            this._noteParts = CreateNoteParts();
            this._andParserNotePart = CreateAndParserNotePart();

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

        #endregion Event Hanlders

        #region Methods

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
            List<NotenizerAdvancedLabel> activeLabels = CreateLabels(this._note.NoteDependencies); ;
            List<NotenizerAdvancedLabel> unusedLabels = CreateUnusedLables(this._note.UnusedDependencies);

            InsertSentenceTerminators(activeLabels, this._note.NoteParts);

            this._flowLayoutPanelActive.Controls.AddRange(activeLabels.ToArray<Control>());
            this._flowLayoutPanelDeleted.Controls.AddRange(unusedLabels.ToArray<Control>());
        }

        private void InitControlsForAndParser()
        {
            List<NotenizerAdvancedLabel> activeLabels = CreateLabels(this._andParserNote.NoteDependencies);
            List<NotenizerAdvancedLabel> unusedLabels = CreateUnusedLables(this._andParserNote.UnusedDependencies);

            activeLabels.Insert(_andSetsPosition, new NotenizerAdvancedLabel(ComponentConstants.AndSetPositionConstant) { RepresentMode = RepresentMode.AndSet, IsDeletable = false });

            this._advancedFlowLayoutPanelAndParserActive.Controls.AddRange(activeLabels.ToArray<Control>());
            this._advancedFlowLayoutPanelAndParsedDeleted.Controls.AddRange(unusedLabels.ToArray<Control>());

            InitializeAndParserGrid();
        }

        private void DisableTabPage(TabPage tabPage)
        {
            foreach (Control controlLoop in tabPage.Controls)
                controlLoop.Enabled = false;
        }

        private List<NotePart> CreateNoteParts()
        {
            NotenizerAdvancedLabel label;
            List<NotePart> noteParts = new List<NotePart>();
            NotePart notePart = new NotePart(this._note.OriginalSentence);

            noteParts.Add(notePart);

            for (int i = 0; i < this._flowLayoutPanelActive.Controls.Count; i++)
            {
                label = (this._flowLayoutPanelActive.Controls[i] as NotenizerAdvancedLabel);

                if (label.RepresentMode == RepresentMode.SentenceEnd)
                {
                    notePart = new NotePart(_note.OriginalSentence);
                    noteParts.Add(notePart);
                    continue;
                }

                SetPositionAddToNotePart(label.Dependency, i, notePart);
            }

            if (notePart.InitializedNoteParticles.Count == 0)
                noteParts.Remove(notePart);

            return noteParts;
        }

        private NotePart CreateAndParserNotePart()
        {
            int depCounter = 0;
            NotenizerAdvancedLabel label;
            NotePart notePart = new NotePart(_note.OriginalSentence);

            for (int i = 0; i < this._advancedFlowLayoutPanelAndParserActive.Controls.Count; i++)
            {
                label = this._advancedFlowLayoutPanelAndParserActive.Controls[i] as NotenizerAdvancedLabel;

                if (label.RepresentMode == RepresentMode.AndSet)
                {
                    _andSetsPosition = i;
                    continue;
                }

                SetPositionAddToNotePart(label.Dependency, depCounter, notePart);
                depCounter++;
            }

            return notePart;
        }

        private void SetPositionAddToNotePart(NotenizerDependency dep, int position, NotePart destionationNotePart)
        {
            dep.Position = position;
            destionationNotePart.Add(new NoteParticle(dep));
        }

        private List<NotenizerAdvancedLabel> CreateLabels(NotenizerDependencies dependencies)
        {
            List<NotenizerAdvancedLabel> labels = new List<NotenizerAdvancedLabel>();

            foreach (NotenizerDependency noteDependency in dependencies)
                labels.Add(new NotenizerAdvancedLabel(noteDependency));

            return labels;
        }

        private void InsertSentenceTerminators(List<NotenizerAdvancedLabel> labels, List<NotePart> noteParts)
        {
            foreach (NotePart notePartLoop in noteParts)
                labels.Insert(notePartLoop.InitializedNoteParticles.Count, new NotenizerAdvancedLabel(NotenizerConstants.SentenceTerminator) { RepresentMode = RepresentMode.SentenceEnd });
        }

        private List<NotenizerAdvancedLabel> CreateUnusedLables(NotenizerDependencies depenencies)
        {
            NotenizerDependency clonedDependency;
            List<NotenizerAdvancedLabel> labels = new List<NotenizerAdvancedLabel>();

            foreach (NotenizerDependency originalSentenceDependencyLoop in depenencies)
            {
                clonedDependency = originalSentenceDependencyLoop.Clone();

                if (!clonedDependency.Relation.IsNominalSubject())
                    clonedDependency.TokenType = TokenType.Dependent;

                labels.Add(new NotenizerAdvancedLabel(clonedDependency));
            }

            return labels;
        }

        private void InitializeAndParserGrid()
        {
            DataRow row;
            DataTable dt = new DataTable();
            BindingSource bindingSource = new BindingSource();

            this._gridNotes.Columns.Clear();
            dt.Columns.Add(new DataColumn("noteColumn") { Caption = "Note" });

            foreach (NotePart parsedAndSetLoop in this. _parsedAndSets)
            {
                row = dt.NewRow();
                row["noteColumn"] = parsedAndSetLoop.AdjustedValue;
                dt.Rows.Add(row);
            }

            bindingSource.DataSource = dt;
            this._gridNotes.DataSource = bindingSource;
            this._gridNotes.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this._gridNotes.MultiSelect = false;
            this._gridNotes.ReadOnly = true;
            this._gridNotes.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this._gridNotes.Columns[0].HeaderText = "Note";
        }

        #endregion Methods
    }
}
