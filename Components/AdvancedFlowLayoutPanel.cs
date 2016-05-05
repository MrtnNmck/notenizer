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
    /// <summary>
    /// Advanced FlowLayoutPanel.
    /// Allows drag and drop events.
    /// </summary>
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

        /// <summary>
        /// Handler for drop event.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnControlAdded(ControlEventArgs e)
        {
            this.SetFont(e.Control, this._controlsFont);

            if (e.Control is NotenizerAdvancedLabel)
                (e.Control as NotenizerAdvancedLabel).ResetToolTip();

            base.OnControlAdded(e);
        }

        /// <summary>
        /// Event handler for drag enter event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AdvancedFlowLayoutPanelActive_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.All;
        }

        /// <summary>
        /// Event handler for drag drop event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AdvancedFlowLayoutPanelActive_DragDrop(object sender, DragEventArgs e)
        {
            Point p;
            NotenizerAdvancedLabel data;
            AdvancedFlowLayoutPanel sourcePanel;
            AdvancedFlowLayoutPanel destinationPanel;

            data = (NotenizerAdvancedLabel)e.Data.GetData(typeof(NotenizerAdvancedLabel));
            destinationPanel = (AdvancedFlowLayoutPanel)sender;
            sourcePanel = (AdvancedFlowLayoutPanel)data.Parent;
            p = destinationPanel.PointToClient(new Point(e.X, e.Y));

            this.DoDragDrop(data, sourcePanel, destinationPanel, p);
        }

        #endregion Event Handlers

        #region Methods

        /// <summary>
        /// Initializes AdvnacedFlowLayoutPanel.
        /// </summary>
        private void Init()
        {
            DragEnter += new DragEventHandler(AdvancedFlowLayoutPanelActive_DragEnter);
            DragDrop += new DragEventHandler(AdvancedFlowLayoutPanelActive_DragDrop);
            AllowDrop = true;
        }

        /// <summary>
        /// Sets font to control.
        /// </summary>
        /// <param name="control"></param>
        /// <param name="font"></param>
        private void SetFont(Control control, Font font)
        {
            control.Font = font;
        }

        /// <summary>
        /// Does drag and drop from one AvancedFlowLayoutPanel to another.
        /// </summary>
        /// <param name="data">NotenizerAdvancedLabel that is being drag and droped</param>
        /// <param name="sourcePanel">Source panel of drag and drop event</param>
        /// <param name="destinationPanel">Destination panel of drag and drop event</param>
        /// <param name="destPanelPointToClient">Point on destination panel where drag and drop event finished</param>
        private void DoDragDrop(NotenizerAdvancedLabel data, AdvancedFlowLayoutPanel sourcePanel, AdvancedFlowLayoutPanel destinationPanel, Point destPanelPointToClient)
        {
            try
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
            catch (Exception ex)
            {
                throw new Exception("Error performing drag and drop." + Environment.NewLine + Environment.NewLine + ex.Message);
            }
        }

        #endregion Methods
    }
}
