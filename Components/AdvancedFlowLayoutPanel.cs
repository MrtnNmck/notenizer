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

            this._controlsFont = font;
        }

        protected override void OnControlAdded(ControlEventArgs e)
        {
            e.Control.Font = this._controlsFont;

            base.OnControlAdded(e);
        }
    }
}
