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
    public partial class FormReorderNote : Form
    {
        private List<NotePart> _noteParts;
        private Note _note;

        public FormReorderNote(Note note)
        {
            this._note = note;
            InitializeComponent();

            this.CenterToParent();

            this._textBoxNote.Text = note.Value;
            this._textBoxOriginalSentence.Text = note.OriginalSentence.ToString();

            this._flowLayoutPanelActive.DragEnter += new DragEventHandler(FlowLayoutPanelActive_DragEnter);
            this._flowLayoutPanelActive.DragDrop += new DragEventHandler(FlowLayoutPanelActive_DragDrop);
            this._flowLayoutPanelActive.AllowDrop = true;

            this._flowLayoutPanelDeleted.DragEnter += new DragEventHandler(FlowLayoutPanelActive_DragEnter);
            this._flowLayoutPanelDeleted.DragDrop += new DragEventHandler(FlowLayoutPanelActive_DragDrop);
            this._flowLayoutPanelDeleted.AllowDrop = true;

            List<NotenizerDependency> noteDependencies = note.NoteDependencies;
            List<NotenizerAdvancedLabel> activeLabels = new List<NotenizerAdvancedLabel>();

            foreach (NotenizerDependency noteDependency in noteDependencies)
            {
                activeLabels.Add(new NotenizerAdvancedLabel(noteDependency));
            }

            foreach (NotePart notePartLoop in note.NoteParts)
            {
                activeLabels.Insert(notePartLoop.InitializedNoteParticles.Count, new NotenizerAdvancedLabel(NotenizerConstants.SentenceTerminator) { RepresentMode = RepresentMode.SentenceEnd });
            }

            this._flowLayoutPanelActive.Controls.AddRange(activeLabels.ToArray<Control>());

            foreach (NotenizerDependency originalSentenceDependencyLoop in note.UnusedDependencies)
            {
                NotenizerDependency clonedDependency = originalSentenceDependencyLoop.Clone();
                clonedDependency.TokenType = TokenType.Dependent;

                this._flowLayoutPanelDeleted.Controls.Add(new NotenizerAdvancedLabel(clonedDependency));
            }
        }

        public List<NotePart> NoteParts
        {
            get { return this._noteParts; }
        }

        #region Event Handlers

        private void FlowLayoutPanelActive_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.All;
        }

        private void FlowLayoutPanelActive_DragDrop(object sender, DragEventArgs e)
        {
            NotenizerAdvancedLabel data = (NotenizerAdvancedLabel)e.Data.GetData(typeof(NotenizerAdvancedLabel));
            FlowLayoutPanel destinationPanel = (FlowLayoutPanel)sender;
            FlowLayoutPanel sourcePanel = (FlowLayoutPanel)data.Parent;

            Point p = destinationPanel.PointToClient(new Point(e.X, e.Y));
            var item = destinationPanel.GetChildAtPoint(p);

            int index = -1;
            if (item == null)
            {
                for (int i = 0; i < destinationPanel.Controls.Count; i++)
                {
                    if (i + 1 < destinationPanel.Controls.Count)
                        if (destinationPanel.Controls[i].Bounds.Right < p.X && destinationPanel.Controls[i + 1].Bounds.Left > p.X)
                            index = i + 1;
                }
            }
            else
                index = destinationPanel.Controls.GetChildIndex(item, false);

            if (!destinationPanel.Controls.Contains(data))
                destinationPanel.Controls.Add(data);

            destinationPanel.Controls.SetChildIndex(data, index);
            destinationPanel.Invalidate();
            sourcePanel.Invalidate();
        }

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
    }
}
