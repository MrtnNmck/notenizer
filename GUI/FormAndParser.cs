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
    public partial class FormAndParser : Form
    {
        #region Variables

        private List<NotePart> _parsedAndSets;
        private NotenizerNote _note;
        private int _andSetsPosition;
        private NotePart _activeNotePart;

        #endregion Variables

        #region Constructors

        public FormAndParser(NotenizerNote note, List<NotePart> noteParts, int andSetsPosition)
        {
            InitializeComponent();
            CenterToParent();

            _parsedAndSets = noteParts;
            _note = note;
            _andSetsPosition = andSetsPosition;

            Init();
        }

        #endregion Constuctors

        #region Properties

        public int AndSetsPosition
        {
            get { return _andSetsPosition; }
        }

        public NotePart ActiveNotePart
        {
            get { return _activeNotePart; }
        }

        #endregion Properties

        #region Event Handlers
        private void ApplyButton_Click(object sender, EventArgs e)
        {
            int depCounter = 0;
            _activeNotePart = new NotePart(_note.OriginalSentence);

            for (int i = 0; i < _flowLayoutPanelActive.Controls.Count; i++)
            {
                NotenizerAdvancedLabel label = _flowLayoutPanelActive.Controls[i] as NotenizerAdvancedLabel;

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

        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        #endregion Event Handlers

        #region Methods

        private void Init()
        {
            this.Icon = Properties.Resources.AppIcon;

            _buttonConfirm.Click += ApplyButton_Click;
            _buttonCancel.Click += CancelButton_Click;

            _textBoxOriginalSentence.Text = _note.OriginalSentence.ToString();
            List<NotenizerAdvancedLabel> activeLabels = new List<NotenizerAdvancedLabel>();

            foreach (NotenizerDependency noteDepenendencyLoop in _note.NoteDependencies)
                activeLabels.Add(new NotenizerAdvancedLabel(noteDepenendencyLoop));

            activeLabels.Insert(_andSetsPosition, new NotenizerAdvancedLabel(ComponentConstants.AndSetPositionConstant) { RepresentMode = RepresentMode.AndSet, IsDeletable = false });
            _flowLayoutPanelActive.Controls.AddRange(activeLabels.ToArray<Control>());

            foreach (NotenizerDependency originalSentenceDependencyLoop in _note.UnusedDependencies)
            {
                NotenizerDependency clonedDependency = originalSentenceDependencyLoop.Clone();

                if (!clonedDependency.Relation.IsNominalSubject())
                    clonedDependency.TokenType = TokenType.Dependent;

                this._flowLayoutPanelDeleted.Controls.Add(new NotenizerAdvancedLabel(clonedDependency));
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

        #endregion Methods
    }
}
