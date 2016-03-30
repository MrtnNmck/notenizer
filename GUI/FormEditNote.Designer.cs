using nsComponents;
using nsConstants;
using System.Drawing;

namespace nsGUI
{
    partial class FormEditNote
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this._panelMain = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this._panelBottom = new System.Windows.Forms.Panel();
            this._buttonApply = new System.Windows.Forms.Button();
            this._buttonCancel = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this._textBoxNote = new System.Windows.Forms.TextBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addSentenceTerminatorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this._gridNotes = new System.Windows.Forms.DataGridView();
            this.noteColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._textBoxOriginalSentence = new System.Windows.Forms.TextBox();
            this._flowLayoutPanelActive = new AdvancedFlowLayoutPanel(new Font(ComponentConstants.AdvancedLabelFontFamilyName, ComponentConstants.AdvancedLabelActiveFontSize));
            this._advancedFlowLayoutPanelAndParserActive = new AdvancedFlowLayoutPanel(new Font(ComponentConstants.AdvancedLabelFontFamilyName, ComponentConstants.AdvancedLabelActiveFontSize));
            this._flowLayoutPanelDeleted = new AdvancedFlowLayoutPanel(new Font(ComponentConstants.AdvancedLabelFontFamilyName, ComponentConstants.AdvancedLabelDeletedFontSize));
            this._advancedFlowLayoutPanelAndParsedDeleted = new AdvancedFlowLayoutPanel(new Font(ComponentConstants.AdvancedLabelFontFamilyName, ComponentConstants.AdvancedLabelDeletedFontSize));
            this._panelMain.SuspendLayout();
            this._panelBottom.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._gridNotes)).BeginInit();
            this.SuspendLayout();
            // 
            // _panelMain
            // 
            this._panelMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._panelMain.Controls.Add(this.label4);
            this._panelMain.Controls.Add(this.label3);
            this._panelMain.Controls.Add(this._flowLayoutPanelDeleted);
            this._panelMain.Controls.Add(this._flowLayoutPanelActive);
            this._panelMain.Location = new System.Drawing.Point(6, 75);
            this._panelMain.Name = "_panelMain";
            this._panelMain.Size = new System.Drawing.Size(579, 431);
            this._panelMain.TabIndex = 1;
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI Emoji", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(4, 210);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(148, 13);
            this.label4.TabIndex = 4;
            this.label4.Text = "Unused words from sentence";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI Emoji", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(4, 4);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(75, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Words in note";
            // 
            // _flowLayoutPanelDeleted
            // 
            this._flowLayoutPanelDeleted.AllowDrop = true;
            this._flowLayoutPanelDeleted.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._flowLayoutPanelDeleted.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._flowLayoutPanelDeleted.Location = new System.Drawing.Point(7, 227);
            this._flowLayoutPanelDeleted.Name = "_flowLayoutPanelDeleted";
            this._flowLayoutPanelDeleted.Size = new System.Drawing.Size(566, 201);
            this._flowLayoutPanelDeleted.TabIndex = 2;
            // 
            // _flowLayoutPanelActive
            // 
            this._flowLayoutPanelActive.AllowDrop = true;
            this._flowLayoutPanelActive.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._flowLayoutPanelActive.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._flowLayoutPanelActive.Location = new System.Drawing.Point(7, 20);
            this._flowLayoutPanelActive.Name = "_flowLayoutPanelActive";
            this._flowLayoutPanelActive.Size = new System.Drawing.Size(566, 178);
            this._flowLayoutPanelActive.TabIndex = 0;
            // 
            // _panelBottom
            // 
            this._panelBottom.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._panelBottom.Controls.Add(this._buttonApply);
            this._panelBottom.Controls.Add(this._buttonCancel);
            this._panelBottom.Location = new System.Drawing.Point(13, 677);
            this._panelBottom.Name = "_panelBottom";
            this._panelBottom.Size = new System.Drawing.Size(599, 33);
            this._panelBottom.TabIndex = 2;
            // 
            // _buttonApply
            // 
            this._buttonApply.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._buttonApply.Location = new System.Drawing.Point(440, 3);
            this._buttonApply.Name = "_buttonApply";
            this._buttonApply.Size = new System.Drawing.Size(75, 23);
            this._buttonApply.TabIndex = 1;
            this._buttonApply.Text = "Apply";
            this._buttonApply.UseVisualStyleBackColor = true;
            this._buttonApply.Click += new System.EventHandler(this.ApplyButton_Click);
            // 
            // _buttonCancel
            // 
            this._buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._buttonCancel.Location = new System.Drawing.Point(521, 3);
            this._buttonCancel.Name = "_buttonCancel";
            this._buttonCancel.Size = new System.Drawing.Size(75, 23);
            this._buttonCancel.TabIndex = 0;
            this._buttonCancel.Text = "Cancel";
            this._buttonCancel.UseVisualStyleBackColor = true;
            this._buttonCancel.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label2.Location = new System.Drawing.Point(10, 3);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(30, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Note";
            // 
            // _textBoxNote
            // 
            this._textBoxNote.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._textBoxNote.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this._textBoxNote.Location = new System.Drawing.Point(13, 19);
            this._textBoxNote.Multiline = true;
            this._textBoxNote.Name = "_textBoxNote";
            this._textBoxNote.ReadOnly = true;
            this._textBoxNote.Size = new System.Drawing.Size(566, 50);
            this._textBoxNote.TabIndex = 3;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.editToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(624, 24);
            this.menuStrip1.TabIndex = 5;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addSentenceTerminatorToolStripMenuItem});
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
            this.editToolStripMenuItem.Text = "Edit";
            // 
            // addSentenceTerminatorToolStripMenuItem
            // 
            this.addSentenceTerminatorToolStripMenuItem.Name = "addSentenceTerminatorToolStripMenuItem";
            this.addSentenceTerminatorToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.T)));
            this.addSentenceTerminatorToolStripMenuItem.Size = new System.Drawing.Size(246, 22);
            this.addSentenceTerminatorToolStripMenuItem.Text = "Add sentence terminator";
            this.addSentenceTerminatorToolStripMenuItem.Click += new System.EventHandler(this.AddSentenceMenuItem_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.tabControl1.Location = new System.Drawing.Point(13, 132);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(599, 539);
            this.tabControl1.TabIndex = 6;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this._textBoxNote);
            this.tabPage1.Controls.Add(this._panelMain);
            this.tabPage1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.tabPage1.Location = new System.Drawing.Point(4, 23);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(591, 512);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Reorder note";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.label7);
            this.tabPage2.Controls.Add(this.panel1);
            this.tabPage2.Controls.Add(this._gridNotes);
            this.tabPage2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.tabPage2.Location = new System.Drawing.Point(4, 23);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(591, 512);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "And Parser";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this._advancedFlowLayoutPanelAndParsedDeleted);
            this.panel1.Controls.Add(this._advancedFlowLayoutPanelAndParserActive);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.panel1.Location = new System.Drawing.Point(6, 188);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(580, 318);
            this.panel1.TabIndex = 4;
            // 
            // _advancedFlowLayoutPanelAndParsedDeleted
            // 
            this._advancedFlowLayoutPanelAndParsedDeleted.AllowDrop = true;
            this._advancedFlowLayoutPanelAndParsedDeleted.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._advancedFlowLayoutPanelAndParsedDeleted.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._advancedFlowLayoutPanelAndParsedDeleted.Location = new System.Drawing.Point(6, 175);
            this._advancedFlowLayoutPanelAndParsedDeleted.Name = "_advancedFlowLayoutPanelAndParsedDeleted";
            this._advancedFlowLayoutPanelAndParsedDeleted.Size = new System.Drawing.Size(567, 140);
            this._advancedFlowLayoutPanelAndParsedDeleted.TabIndex = 5;
            // 
            // _advancedFlowLayoutPanelAndParserActive
            // 
            this._advancedFlowLayoutPanelAndParserActive.AllowDrop = true;
            this._advancedFlowLayoutPanelAndParserActive.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._advancedFlowLayoutPanelAndParserActive.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._advancedFlowLayoutPanelAndParserActive.Location = new System.Drawing.Point(6, 21);
            this._advancedFlowLayoutPanelAndParserActive.Name = "_advancedFlowLayoutPanelAndParserActive";
            this._advancedFlowLayoutPanelAndParserActive.Size = new System.Drawing.Size(567, 135);
            this._advancedFlowLayoutPanelAndParserActive.TabIndex = 4;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(3, 159);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(133, 13);
            this.label5.TabIndex = 3;
            this.label5.Text = "Unused words in sentence";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(4, 4);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(73, 13);
            this.label6.TabIndex = 2;
            this.label6.Text = "Words in note";
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this._textBoxOriginalSentence);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.groupBox2.Location = new System.Drawing.Point(12, 27);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(600, 99);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Information";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(10, 3);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(35, 13);
            this.label7.TabIndex = 3;
            this.label7.Text = "Notes";
            this.label7.Click += new System.EventHandler(this.label7_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(6, 19);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(89, 13);
            this.label8.TabIndex = 2;
            this.label8.Text = "Original sentence";
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
            this._gridNotes.Location = new System.Drawing.Point(12, 19);
            this._gridNotes.MultiSelect = false;
            this._gridNotes.Name = "_gridNotes";
            this._gridNotes.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this._gridNotes.Size = new System.Drawing.Size(567, 163);
            this._gridNotes.TabIndex = 1;
            // 
            // noteColumn
            // 
            this.noteColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.Transparent;
            this.noteColumn.DefaultCellStyle = dataGridViewCellStyle2;
            this.noteColumn.HeaderText = "Note";
            this.noteColumn.Name = "noteColumn";
            // 
            // _textBoxOriginalSentenceAndParser
            // 
            this._textBoxOriginalSentence.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._textBoxOriginalSentence.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._textBoxOriginalSentence.Location = new System.Drawing.Point(6, 35);
            this._textBoxOriginalSentence.Multiline = true;
            this._textBoxOriginalSentence.Name = "_textBoxOriginalSentenceAndParser";
            this._textBoxOriginalSentence.ReadOnly = true;
            this._textBoxOriginalSentence.Size = new System.Drawing.Size(588, 58);
            this._textBoxOriginalSentence.TabIndex = 0;
            // 
            // FormReorderNote
            // 
            this.AcceptButton = this._buttonApply;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this._buttonCancel;
            this.ClientSize = new System.Drawing.Size(624, 722);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this._panelBottom);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "FormReorderNote";
            this.Text = "Edit Note";
            this._panelMain.ResumeLayout(false);
            this._panelMain.PerformLayout();
            this._panelBottom.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._gridNotes)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Panel _panelMain;
        private AdvancedFlowLayoutPanel _flowLayoutPanelActive;
        private System.Windows.Forms.Panel _panelBottom;
        private AdvancedFlowLayoutPanel _flowLayoutPanelDeleted;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox _textBoxNote;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button _buttonApply;
        private System.Windows.Forms.Button _buttonCancel;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addSentenceTerminatorToolStripMenuItem;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.DataGridView _gridNotes;
        private System.Windows.Forms.DataGridViewTextBoxColumn noteColumn;
        private System.Windows.Forms.TextBox _textBoxOriginalSentence;
        private AdvancedFlowLayoutPanel _advancedFlowLayoutPanelAndParsedDeleted;
        private AdvancedFlowLayoutPanel _advancedFlowLayoutPanelAndParserActive;
    }
}