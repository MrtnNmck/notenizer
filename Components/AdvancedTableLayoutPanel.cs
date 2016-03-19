using nsInterfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace nsComponents
{
    public partial class AdvancedTableLayoutPanel : TableLayoutPanel, IMousable
    {
        public AdvancedTableLayoutPanel()
        {
            InitializeComponent();
        }

        public void DoMouseWheel(EventArgs e)
        {
            base.OnMouseWheel((MouseEventArgs)e);
        }
    }
}
