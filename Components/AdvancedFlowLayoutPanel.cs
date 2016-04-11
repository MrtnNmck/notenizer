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
        #region Variables

        private Font _controlsFont;
        private bool _isDirty;

        #endregion Variables

        #region Constuctors

        public AdvancedFlowLayoutPanel(Font font)
        {
            InitializeComponent();
            Init();

            this._controlsFont = font;
            this._isDirty = false;
        }

        #endregion Constuctors

        #region Properties

        public bool IsDirty
        {
            get { return this._isDirty; }
        }

        #endregion Properties

        #region Event Handlers

        protected override void OnControlAdded(ControlEventArgs e)
        {
            this.SetFont(e.Control, this._controlsFont);

            base.OnControlAdded(e);
        }


        private void AdvancedFlowLayoutPanelActive_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.All;
        }

        private void AdvancedFlowLayoutPanelActive_DragDrop(object sender, DragEventArgs e)
        {
            NotenizerAdvancedLabel data = (NotenizerAdvancedLabel)e.Data.GetData(typeof(NotenizerAdvancedLabel));
            AdvancedFlowLayoutPanel destinationPanel = (AdvancedFlowLayoutPanel)sender;
            AdvancedFlowLayoutPanel sourcePanel = (AdvancedFlowLayoutPanel)data.Parent;
            Point p = destinationPanel.PointToClient(new Point(e.X, e.Y));

            this.DoDragDrop(data, sourcePanel, destinationPanel, p);
        }

        #endregion Event Hanlders

        #region Methods

        private void Init()
        {
            DragEnter += new DragEventHandler(AdvancedFlowLayoutPanelActive_DragEnter);
            DragDrop += new DragEventHandler(AdvancedFlowLayoutPanelActive_DragDrop);
            AllowDrop = true;
        }

        private void SetFont(Control control, Font font)
        {
            control.Font = font;
        }

        private void DoDragDrop(NotenizerAdvancedLabel data, AdvancedFlowLayoutPanel sourcePanel, AdvancedFlowLayoutPanel destinationPanel, Point destPanelPointToClient)
        {
            var item = destinationPanel.GetChildAtPoint(destPanelPointToClient);

            int index = -1;
            if (item == null)
            {
                for (int i = 0; i < destinationPanel.Controls.Count; i++)
                {
                    if (i + 1 < destinationPanel.Controls.Count)
                        if (destinationPanel.Controls[i].Bounds.Right < destPanelPointToClient.X && destinationPanel.Controls[i + 1].Bounds.Left > destPanelPointToClient.X)
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

            this._isDirty = true;
        }

        #endregion Methods
    }
}
