﻿using nsConstants;
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
        private AdvancedTextBox _advancedTextBox = null;
        private Button _editButton = null;
        private Note _note = null;

        public delegate void ButtonClickHandler(NotenizerAdvancedTextBox sender, EventArgs e);

        public event ButtonClickHandler EditButtonClicked;

        public NotenizerAdvancedTextBox()
        {
            Init();
        }

        public NotenizerAdvancedTextBox(String text)
        {
            Init();

            this._advancedTextBox.TextBox.Text = text;
        }

        public NotenizerAdvancedTextBox(Note note)
        {
            Init();

            this._note = note;
            this._advancedTextBox.TextBox.Text = note.Value;
        }

        public void Init()
        {
            InitializeComponent();
            InitControls();

            this.Dock = DockStyle.Fill;
            this.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));

            AddRowWithComponent(this._advancedTextBox, SizeType.Absolute, ComponentConstants.NotenizerAdvancedTextBoxTextBoxRowSize);
            AddRowWithComponent(this._editButton, SizeType.Absolute, ComponentConstants.NotenizerAdvancedTextBoxButtonsRowSize);

            InitEditButton();
            this.Margin = new Padding(0);
            this.Padding = new Padding(0);
        }

        private void EditButton_Click(object sender, EventArgs e)
        {
            if (EditButtonClicked != null)
                EditButtonClicked(this, e);
        }

        public AdvancedTextBox AdvancedTextBox
        {
            get { return this._advancedTextBox; }
        }

        public Button EditButton
        {
            get { return this._editButton; }
        }

        public Note Note
        {
            get { return this._note; }
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
        }
    }
}