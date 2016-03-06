namespace nsGUI
{
    partial class FormTextInputer
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
            this.components = new System.ComponentModel.Container();
            this._panelHeader = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this._panelButtons = new System.Windows.Forms.Panel();
            this._buttonConfirm = new System.Windows.Forms.Button();
            this._buttonCancel = new System.Windows.Forms.Button();
            this._panelMain = new System.Windows.Forms.Panel();
            this._notenizerTextBoxText = new nsComponents.AdvancedTextBox();
            this.components.Add(this._notenizerTextBoxText);
            this._panelHeader.SuspendLayout();
            this._panelButtons.SuspendLayout();
            this._panelMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // _panelHeader
            // 
            this._panelHeader.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._panelHeader.Controls.Add(this.label1);
            this._panelHeader.Location = new System.Drawing.Point(13, 13);
            this._panelHeader.Name = "_panelHeader";
            this._panelHeader.Size = new System.Drawing.Size(459, 45);
            this._panelHeader.TabIndex = 9999;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI Emoji", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(4, 4);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(304, 37);
            this.label1.TabIndex = 9999;
            this.label1.Text = "Input text for processing";
            // 
            // _panelButtons
            // 
            this._panelButtons.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._panelButtons.Controls.Add(this._buttonConfirm);
            this._panelButtons.Controls.Add(this._buttonCancel);
            this._panelButtons.Location = new System.Drawing.Point(13, 319);
            this._panelButtons.Name = "_panelButtons";
            this._panelButtons.Size = new System.Drawing.Size(459, 30);
            this._panelButtons.TabIndex = 1;
            // 
            // _buttonConfirm
            // 
            this._buttonConfirm.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._buttonConfirm.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this._buttonConfirm.Location = new System.Drawing.Point(300, 4);
            this._buttonConfirm.Name = "_buttonConfirm";
            this._buttonConfirm.Size = new System.Drawing.Size(75, 23);
            this._buttonConfirm.TabIndex = 0;
            this._buttonConfirm.Text = "Confirm";
            this._buttonConfirm.UseVisualStyleBackColor = true;
            this._buttonConfirm.Click += new System.EventHandler(this.ButtonConfirm_Click);
            // 
            // _buttonCancel
            // 
            this._buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._buttonCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this._buttonCancel.Location = new System.Drawing.Point(381, 4);
            this._buttonCancel.Name = "_buttonCancel";
            this._buttonCancel.Size = new System.Drawing.Size(75, 23);
            this._buttonCancel.TabIndex = 1;
            this._buttonCancel.Text = "Cancel";
            this._buttonCancel.UseVisualStyleBackColor = true;
            this._buttonCancel.Click += new System.EventHandler(this.ButtonCancel_Click);
            // 
            // _panelMain
            // 
            this._panelMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._panelMain.Controls.Add(this._notenizerTextBoxText);
            this._panelMain.Location = new System.Drawing.Point(13, 65);
            this._panelMain.Name = "_panelMain";
            this._panelMain.Size = new System.Drawing.Size(459, 248);
            this._panelMain.TabIndex = 0;
            // 
            // _notenizerTextBoxText
            // 
            this._notenizerTextBoxText.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._notenizerTextBoxText.Dock = System.Windows.Forms.DockStyle.Fill;
            this._notenizerTextBoxText.Location = new System.Drawing.Point(0, 0);
            this._notenizerTextBoxText.Name = "advancedTextBox1";
            this._notenizerTextBoxText.Size = new System.Drawing.Size(459, 248);
            this._notenizerTextBoxText.TabIndex = 0;
            this._notenizerTextBoxText.TextBox.ReadOnly = false;
            // 
            // FormTextInputer
            // 
            this.AcceptButton = this._buttonConfirm;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this._buttonCancel;
            this.ClientSize = new System.Drawing.Size(484, 361);
            this.Controls.Add(this._panelMain);
            this.Controls.Add(this._panelButtons);
            this.Controls.Add(this._panelHeader);
            this.Name = "FormTextInputer";
            this.Text = "Text for processing";
            this._panelHeader.ResumeLayout(false);
            this._panelHeader.PerformLayout();
            this._panelButtons.ResumeLayout(false);
            this._panelMain.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel _panelHeader;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel _panelButtons;
        private System.Windows.Forms.Button _buttonConfirm;
        private System.Windows.Forms.Button _buttonCancel;
        private System.Windows.Forms.Panel _panelMain;
        private nsComponents.AdvancedTextBox _notenizerTextBoxText;
    }
}