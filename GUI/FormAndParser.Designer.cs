using nsComponents;
using nsConstants;
using System.Drawing;

namespace nsGUI
{
    partial class FormAndParser
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this._gridNotes = new System.Windows.Forms.DataGridView();
            this.noteColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._textBoxOriginalSentence = new System.Windows.Forms.TextBox();
            this._buttonPanel = new System.Windows.Forms.Panel();
            this._buttonCancel = new System.Windows.Forms.Button();
            this._buttonConfirm = new System.Windows.Forms.Button();
            this._panelMain = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this._flowLayoutPanelActive = new AdvancedFlowLayoutPanel(new Font(ComponentConstants.AdvancedTextBoxFontFamilyName, ComponentConstants.AdvancedLabelActiveFontSize));
            this._flowLayoutPanelDeleted = new AdvancedFlowLayoutPanel(new Font(ComponentConstants.AdvancedTextBoxFontFamilyName, ComponentConstants.AdvancedLabelDeletedFontSize));
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._gridNotes)).BeginInit();
            this._buttonPanel.SuspendLayout();
            this._panelMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this._gridNotes);
            this.groupBox1.Controls.Add(this._textBoxOriginalSentence);
            this.groupBox1.Location = new System.Drawing.Point(13, 13);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(686, 286);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Information";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(7, 101);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(35, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Notes";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(7, 20);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(89, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Original sentence";
            // 
            // _gridNotes
            // 
            this._gridNotes.AllowUserToAddRows = false;
            this._gridNotes.AllowUserToDeleteRows = false;
            this._gridNotes.AllowUserToOrderColumns = true;
            this._gridNotes.AllowUserToResizeColumns = false;
            this._gridNotes.AllowUserToResizeRows = false;
            this._gridNotes.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._gridNotes.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this._gridNotes.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.noteColumn});
            this._gridNotes.Location = new System.Drawing.Point(7, 117);
            this._gridNotes.MultiSelect = false;
            this._gridNotes.Name = "_gridNotes";
            this._gridNotes.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this._gridNotes.Size = new System.Drawing.Size(673, 163);
            this._gridNotes.TabIndex = 1;
            // 
            // noteColumn
            // 
            this.noteColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.Transparent;
            this.noteColumn.DefaultCellStyle = dataGridViewCellStyle1;
            this.noteColumn.HeaderText = "Note";
            this.noteColumn.Name = "noteColumn";
            // 
            // _textBoxOriginalSentence
            // 
            this._textBoxOriginalSentence.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._textBoxOriginalSentence.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._textBoxOriginalSentence.Location = new System.Drawing.Point(6, 36);
            this._textBoxOriginalSentence.Multiline = true;
            this._textBoxOriginalSentence.Name = "_textBoxOriginalSentence";
            this._textBoxOriginalSentence.ReadOnly = true;
            this._textBoxOriginalSentence.Size = new System.Drawing.Size(673, 58);
            this._textBoxOriginalSentence.TabIndex = 0;
            // 
            // _buttonPanel
            // 
            this._buttonPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._buttonPanel.Controls.Add(this._buttonCancel);
            this._buttonPanel.Controls.Add(this._buttonConfirm);
            this._buttonPanel.Location = new System.Drawing.Point(13, 669);
            this._buttonPanel.Name = "_buttonPanel";
            this._buttonPanel.Size = new System.Drawing.Size(686, 31);
            this._buttonPanel.TabIndex = 1;
            // 
            // _buttonCancel
            // 
            this._buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._buttonCancel.Location = new System.Drawing.Point(608, 5);
            this._buttonCancel.Name = "_buttonCancel";
            this._buttonCancel.Size = new System.Drawing.Size(75, 23);
            this._buttonCancel.TabIndex = 1;
            this._buttonCancel.Text = "Cancel";
            this._buttonCancel.UseVisualStyleBackColor = true;
            // 
            // _buttonConfirm
            // 
            this._buttonConfirm.Location = new System.Drawing.Point(527, 5);
            this._buttonConfirm.Name = "_buttonConfirm";
            this._buttonConfirm.Size = new System.Drawing.Size(75, 23);
            this._buttonConfirm.TabIndex = 0;
            this._buttonConfirm.Text = "Apply";
            this._buttonConfirm.UseVisualStyleBackColor = true;
            // 
            // _panelMain
            // 
            this._panelMain.Controls.Add(this.label2);
            this._panelMain.Controls.Add(this.label1);
            this._panelMain.Controls.Add(this._flowLayoutPanelDeleted);
            this._panelMain.Controls.Add(this._flowLayoutPanelActive);
            this._panelMain.Location = new System.Drawing.Point(12, 305);
            this._panelMain.Name = "_panelMain";
            this._panelMain.Size = new System.Drawing.Size(687, 358);
            this._panelMain.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 174);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(133, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Unused words in sentence";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 4);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(73, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Words in note";
            // 
            // _flowLayoutPanelDeleted
            // 
            this._flowLayoutPanelDeleted.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._flowLayoutPanelDeleted.Location = new System.Drawing.Point(3, 190);
            this._flowLayoutPanelDeleted.Name = "_flowLayoutPanelDeleted";
            this._flowLayoutPanelDeleted.Size = new System.Drawing.Size(681, 165);
            this._flowLayoutPanelDeleted.TabIndex = 1;
            // 
            // _flowLayoutPanelActive
            // 
            this._flowLayoutPanelActive.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._flowLayoutPanelActive.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._flowLayoutPanelActive.Location = new System.Drawing.Point(3, 20);
            this._flowLayoutPanelActive.Name = "_flowLayoutPanelActive";
            this._flowLayoutPanelActive.Size = new System.Drawing.Size(681, 147);
            this._flowLayoutPanelActive.TabIndex = 0;
            // 
            // FormAndParser
            // 
            this.AcceptButton = this._buttonConfirm;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this._buttonCancel;
            this.ClientSize = new System.Drawing.Size(711, 712);
            this.Controls.Add(this._panelMain);
            this.Controls.Add(this._buttonPanel);
            this.Controls.Add(this.groupBox1);
            this.Name = "FormAndParser";
            this.Text = "FormAndParser";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._gridNotes)).EndInit();
            this._buttonPanel.ResumeLayout(false);
            this._panelMain.ResumeLayout(false);
            this._panelMain.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DataGridView _gridNotes;
        private System.Windows.Forms.TextBox _textBoxOriginalSentence;
        private System.Windows.Forms.Panel _buttonPanel;
        private System.Windows.Forms.Button _buttonCancel;
        private System.Windows.Forms.Button _buttonConfirm;
        private System.Windows.Forms.Panel _panelMain;
        private System.Windows.Forms.Label label1;
        private AdvancedFlowLayoutPanel _flowLayoutPanelDeleted;
        private AdvancedFlowLayoutPanel _flowLayoutPanelActive;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DataGridViewTextBoxColumn noteColumn;
    }
}