using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace nsComponents
{
    public partial class AdvancedFlowLayoutPanel : FlowLayoutPanel
    {
        private Font _controlsFont;

        public AdvancedFlowLayoutPanel(Font font)
        {
            InitializeComponent();
            Init();

            this._controlsFont = font;
        }

        private void Init()
        {
            DragEnter += new DragEventHandler(AdvancedFlowLayoutPanelActive_DragEnter);
            DragDrop += new DragEventHandler(AdvancedFlowLayoutPanelActive_DragDrop);
            AllowDrop = true;
        }

        protected override void OnControlAdded(ControlEventArgs e)
        {
            e.Control.Font = this._controlsFont;

            base.OnControlAdded(e);
        }


        private void AdvancedFlowLayoutPanelActive_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.All;
        }

        private void AdvancedFlowLayoutPanelActive_DragDrop(object sender, DragEventArgs e)
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

            if (!destinationPanel.Controls.Contains(data) && data.IsDeletable)
                destinationPanel.Controls.Add(data);

            destinationPanel.Controls.SetChildIndex(data, index);
            destinationPanel.Invalidate();
            sourcePanel.Invalidate();
        }
    }
}
