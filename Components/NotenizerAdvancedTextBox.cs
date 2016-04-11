using nsConstants;
using nsExtensions;
using nsInterfaces;
using nsNotenizerObjects;
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
    public partial class NotenizerAdvancedTextBox : TableLayoutPanel, INotenizerComponent
    {
        #region Variables

        private AdvancedTextBox _advancedTextBox = null;
        private Button _editButton = null;
        private FlowLayoutPanel _buttonPanel = null;
        private NotenizerNote _note = null;
        private bool _isDeletable = true;

        public delegate void ButtonClickHandler(NotenizerAdvancedTextBox sender, EventArgs e);

        public event ButtonClickHandler EditButtonClicked;

        #endregion Variables

        #region Constructors

        public NotenizerAdvancedTextBox()
        {
            Init();
        }

        public NotenizerAdvancedTextBox(String text)
        {
            Init();

            this._advancedTextBox.TextBox.Text = text;
        }

        public NotenizerAdvancedTextBox(NotenizerNote note)
        {
            this._note = note;

            Init();

            this._advancedTextBox.TextBox.Text = note.Value;
        }

        #endregion Constuctors

        #region Properties

        public AdvancedTextBox AdvancedTextBox
        {
            get { return this._advancedTextBox; }
        }

        public Button EditButton
        {
            get { return this._editButton; }
        }

        public NotenizerNote Note
        {
            get { return this._note; }
        }

        public Boolean IsDeletable
        {
            get
            {
                return this._isDeletable;
            }

            set
            {
                this._isDeletable = value;
            }
        }

        #endregion Properties

        #region Event Handlers

        private void EditButton_Click(object sender, EventArgs e)
        {
            if (EditButtonClicked != null)
                EditButtonClicked(this, e);
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);

            if (Parent is IMousable)
                (Parent as IMousable).DoMouseWheel(e);
        }

        #endregion Event Hanlders

        #region Methods

        public void Init()
        {
            InitializeComponent();
            InitControls();

            this.Dock = DockStyle.Fill;
            this.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));

            AddRowWithComponent(this._advancedTextBox, SizeType.Absolute, ComponentConstants.NotenizerAdvancedTextBoxTextBoxRowSize);
            AddRowWithComponent(this._buttonPanel, SizeType.Absolute, ComponentConstants.NotenizerAdvancedTextBoxButtonsRowSize);
            this._buttonPanel.Controls.Add(this._editButton);

            InitEditButton();
            this.AdvancedTextBox.TextBox.SetToolTip("Match: " + _note.Rule.Match + "%");

            this.Margin = new Padding(0);
            this.Padding = new Padding(0);
        }

        private void AddRowWithComponent(Control control, SizeType sizeType, float size)
        {
            this.RowCount += 1;
            this.RowStyles.Add(new RowStyle(sizeType, size));
            this.Controls.Add(control, 0, this.RowCount - 1);
        }

        private void InitEditButton()
        {
            this._editButton.Text = "Edit";
            this._editButton.Click += EditButton_Click;
        }

        private void InitControls()
        {
            this._advancedTextBox = new AdvancedTextBox();
            this._editButton = new Button();

            this._buttonPanel = new FlowLayoutPanel();
            this._buttonPanel.Dock = DockStyle.Fill;
        }

        #endregion Methods
    }
}
