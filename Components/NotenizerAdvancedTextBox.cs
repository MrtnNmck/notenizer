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
    /// <summary>
    /// AdvancedTextBox with Notenizer data.
    /// </summary>
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

            this._advancedTextBox.TextBox.Text = note.Text;
        }

        #endregion Constuctors

        #region Properties

        /// <summary>
        /// TextBox.
        /// </summary>
        public AdvancedTextBox AdvancedTextBox
        {
            get { return this._advancedTextBox; }
        }

        /// <summary>
        /// Edit button.
        /// </summary>
        public Button EditButton
        {
            get { return this._editButton; }
        }

        /// <summary>
        /// Nonizer note.
        /// </summary>
        public NotenizerNote Note
        {
            get { return this._note; }
        }

        /// <summary>
        /// Flag if component is deletable.
        /// </summary>
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

        /// <summary>
        /// Fires edit button clicked event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EditButton_Click(object sender, EventArgs e)
        {
            if (EditButtonClicked != null)
                EditButtonClicked(this, e);
        }

        /// <summary>
        /// Fires do mouse wheel event.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);

            if (Parent is IMousable)
                (Parent as IMousable).DoMouseWheel(e);
        }

        #endregion Event Handlers

        #region Methods

        /// <summary>
        /// Initializes NotenizerAdvancedTextBox.
        /// </summary>
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
            this.AdvancedTextBox.TextBox.SetToolTip(
                "Structure match: " + this._note.Rule.Match.Structure + "%." 
                + Environment.NewLine
                + "  Content match: " + this._note.Rule.Match.Content + "%.");

            this.Margin = new Padding(0);
            this.Padding = new Padding(0);
        }

        /// <summary>
        /// Adds new row with control into table layout.
        /// </summary>
        /// <param name="control"></param>
        /// <param name="sizeType"></param>
        /// <param name="size"></param>
        private void AddRowWithComponent(Control control, SizeType sizeType, float size)
        {
            try
            {
                this.RowCount += 1;
                this.RowStyles.Add(new RowStyle(sizeType, size));
                this.Controls.Add(control, 0, this.RowCount - 1);
            }
            catch (Exception ex)
            {
                throw new Exception("Error adding component to row in table layout." + Environment.NewLine + Environment.NewLine + ex.Message);
            }
        }

        /// <summary>
        /// Initilizes Edit button.
        /// </summary>
        private void InitEditButton()
        {
            this._editButton.Text = "Edit";
            this._editButton.Click += EditButton_Click;
        }

        /// <summary>
        /// Initializes child controls.
        /// </summary>
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
