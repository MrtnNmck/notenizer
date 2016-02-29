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
    public partial class NotenizerTextBox : TextBox
    {
        public NotenizerTextBox() : base()
        {
            InitializeComponent();
            Init();
        }

        public NotenizerTextBox(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

        private void Init()
        {
            this.Dock = DockStyle.Fill;
            this.BorderStyle = BorderStyle.FixedSingle;
            this.Font = new System.Drawing.Font("Consolas", this.Font.Size);
            this.Multiline = true;
            this.ReadOnly = true;
        }
    }
}
