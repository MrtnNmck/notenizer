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
        public FormReorderNote(Note note)
        {
            Font deletedFlowLayoutLabelFont = new Font(ComponentConstants.AdvancedLabelFontFamilyName, ComponentConstants.AdvancedLabelDeletedFontSize);
            InitializeComponent();

            this.CenterToParent();

            this._flowLayoutPanelActive.DragEnter += new DragEventHandler(FlowLayoutPanelActive_DragEnter);
            this._flowLayoutPanelActive.DragDrop += new DragEventHandler(FlowLayoutPanelActive_DragDrop);
            this._flowLayoutPanelActive.AllowDrop = true;

            this._flowLayoutPanelDeleted.DragEnter += new DragEventHandler(FlowLayoutPanelActive_DragEnter);
            this._flowLayoutPanelDeleted.DragDrop += new DragEventHandler(FlowLayoutPanelActive_DragDrop);
            this._flowLayoutPanelDeleted.AllowDrop = true;

            foreach (NotePart notePartLoop in note.NoteParts)
            {
                foreach (NoteParticle noteParticleLoop in notePartLoop.InitializedNoteParticles)
                {
                    this._flowLayoutPanelActive.Controls.Add(new NotenizerAdvancedLabel(noteParticleLoop.NoteDependency));
                }
            }

            foreach (NotenizerDependency originalSentenceDependencyLoop in note.OriginalSentence.Dependencies)
            {
                NotenizerDependency clonedDependency = originalSentenceDependencyLoop.Clone();
                clonedDependency.TokenType = TokenType.Dependent;

                this._flowLayoutPanelDeleted.Controls.Add(new NotenizerAdvancedLabel(clonedDependency));//, deletedFlowLayoutLabelFont));
            }
        }

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
    }
}
