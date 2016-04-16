using nsConstants;
using nsInterfaces;
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

        #endregion Event Handlers

        #region Methods

        public void ResizeToOriginal()
        {
            this.RowStyles.Clear();
            this.RowCount = 1;
            this.RowStyles.Add(new RowStyle(SizeType.Absolute, ComponentConstants.TableLayoutMainRowSize));
            this.AutoScroll = false;
            this.ResumeLayout(true);
            this.AutoScroll = true;
            this.ResumeLayout(true);
        }

        #endregion Methods
    }
}
