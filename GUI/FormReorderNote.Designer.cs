using nsComponents;
using nsConstants;
using System.Drawing;

namespace nsGUI
{
    partial class FormReorderNote
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
            this._panelMain = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this._panelBottom = new System.Windows.Forms.Panel();
            this._buttonApply = new System.Windows.Forms.Button();
            this._buttonCancel = new System.Windows.Forms.Button();
            this._textBoxOriginalSentence = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this._textBoxNote = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this._flowLayoutPanelActive = new AdvancedFlowLayoutPanel(new Font(ComponentConstants.AdvancedTextBoxFontFamilyName, ComponentConstants.AdvancedLabelActiveFontSize));
            this._flowLayoutPanelDeleted = new AdvancedFlowLayoutPanel(new Font(ComponentConstants.AdvancedTextBoxFontFamilyName, ComponentConstants.AdvancedLabelDeletedFontSize));
            this._panelMain.SuspendLayout();
            this._panelBottom.SuspendLayout();
            this.groupBox1.SuspendLayout();
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
            this._panelMain.Location = new System.Drawing.Point(13, 184);
            this._panelMain.Name = "_panelMain";
            this._panelMain.Size = new System.Drawing.Size(556, 418);
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
            this._flowLayoutPanelDeleted.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._flowLayoutPanelDeleted.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._flowLayoutPanelDeleted.Location = new System.Drawing.Point(7, 227);
            this._flowLayoutPanelDeleted.Name = "_flowLayoutPanelDeleted";
            this._flowLayoutPanelDeleted.Size = new System.Drawing.Size(543, 188);
            this._flowLayoutPanelDeleted.TabIndex = 2;
            // 
            // _flowLayoutPanelActive
            // 
            this._flowLayoutPanelActive.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._flowLayoutPanelActive.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._flowLayoutPanelActive.Location = new System.Drawing.Point(7, 20);
            this._flowLayoutPanelActive.Name = "_flowLayoutPanelActive";
            this._flowLayoutPanelActive.Size = new System.Drawing.Size(543, 178);
            this._flowLayoutPanelActive.TabIndex = 0;
            // 
            // _panelBottom
            // 
            this._panelBottom.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._panelBottom.Controls.Add(this._buttonApply);
            this._panelBottom.Controls.Add(this._buttonCancel);
            this._panelBottom.Location = new System.Drawing.Point(13, 608);
            this._panelBottom.Name = "_panelBottom";
            this._panelBottom.Size = new System.Drawing.Size(556, 33);
            this._panelBottom.TabIndex = 2;
            // 
            // _buttonApply
            // 
            this._buttonApply.Location = new System.Drawing.Point(397, 3);
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
            this._buttonCancel.Location = new System.Drawing.Point(478, 3);
            this._buttonCancel.Name = "_buttonCancel";
            this._buttonCancel.Size = new System.Drawing.Size(75, 23);
            this._buttonCancel.TabIndex = 0;
            this._buttonCancel.Text = "Cancel";
            this._buttonCancel.UseVisualStyleBackColor = true;
            this._buttonCancel.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // _textBoxOriginalSentence
            // 
            this._textBoxOriginalSentence.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._textBoxOriginalSentence.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._textBoxOriginalSentence.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this._textBoxOriginalSentence.Location = new System.Drawing.Point(9, 34);
            this._textBoxOriginalSentence.Multiline = true;
            this._textBoxOriginalSentence.Name = "_textBoxOriginalSentence";
            this._textBoxOriginalSentence.ReadOnly = true;
            this._textBoxOriginalSentence.Size = new System.Drawing.Size(541, 50);
            this._textBoxOriginalSentence.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI Emoji", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(6, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(93, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Original sentence";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI Emoji", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(6, 87);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(31, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Note";
            // 
            // _textBoxNote
            // 
            this._textBoxNote.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._textBoxNote.Location = new System.Drawing.Point(9, 103);
            this._textBoxNote.Multiline = true;
            this._textBoxNote.Name = "_textBoxNote";
            this._textBoxNote.ReadOnly = true;
            this._textBoxNote.Size = new System.Drawing.Size(541, 50);
            this._textBoxNote.TabIndex = 3;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this._textBoxOriginalSentence);
            this.groupBox1.Controls.Add(this._textBoxNote);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Font = new System.Drawing.Font("Segoe UI Emoji", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(13, 13);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(556, 165);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Information";
            // 
            // FormReorderNote
            // 
            this.AcceptButton = this._buttonApply;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this._buttonCancel;
            this.ClientSize = new System.Drawing.Size(581, 653);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this._panelBottom);
            this.Controls.Add(this._panelMain);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "FormReorderNote";
            this.Text = "Edit Note";
            this._panelMain.ResumeLayout(false);
            this._panelMain.PerformLayout();
            this._panelBottom.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel _panelMain;
        private AdvancedFlowLayoutPanel _flowLayoutPanelActive;
        private System.Windows.Forms.Panel _panelBottom;
        private AdvancedFlowLayoutPanel _flowLayoutPanelDeleted;
        private System.Windows.Forms.TextBox _textBoxOriginalSentence;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox _textBoxNote;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button _buttonApply;
        private System.Windows.Forms.Button _buttonCancel;
    }
}