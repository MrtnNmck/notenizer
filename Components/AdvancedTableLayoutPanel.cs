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
        #region Variables

        #endregion Variables

        #region Constructors

        public AdvancedTableLayoutPanel()
        {
            InitializeComponent();
        }

        #endregion Constuctors

        #region Properties

        #endregion Properties

        #region Event Handlers

        public void DoMouseWheel(EventArgs e)
        {
            base.OnMouseWheel((MouseEventArgs)e);
        }

        #endregion Event Hanlders

        #region Methods

        #endregion Methods
    }
}
