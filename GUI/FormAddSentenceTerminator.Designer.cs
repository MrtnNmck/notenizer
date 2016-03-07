namespace nsGUI
{
    partial class FormAddSentenceTerminator
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this._comboBoxSentenceTerminator = new System.Windows.Forms.ComboBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this._buttonAccept = new System.Windows.Forms.Button();
            this._buttonCancel = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this._comboBoxSentenceTerminator);
            this.panel1.Location = new System.Drawing.Point(12, 45);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(274, 42);
            this.panel1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(102, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Sentence terminator";
            // 
            // _comboBoxSentenceTerminator
            // 
            this._comboBoxSentenceTerminator.FormattingEnabled = true;
            this._comboBoxSentenceTerminator.Location = new System.Drawing.Point(112, 10);
            this._comboBoxSentenceTerminator.Name = "_comboBoxSentenceTerminator";
            this._comboBoxSentenceTerminator.Size = new System.Drawing.Size(159, 21);
            this._comboBoxSentenceTerminator.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this._buttonAccept);
            this.panel2.Controls.Add(this._buttonCancel);
            this.panel2.Location = new System.Drawing.Point(12, 93);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(274, 28);
            this.panel2.TabIndex = 1;
            // 
            // _buttonAccept
            // 
            this._buttonAccept.Location = new System.Drawing.Point(114, 4);
            this._buttonAccept.Name = "_buttonAccept";
            this._buttonAccept.Size = new System.Drawing.Size(75, 23);
            this._buttonAccept.TabIndex = 1;
            this._buttonAccept.Text = "Accept";
            this._buttonAccept.UseVisualStyleBackColor = true;
            this._buttonAccept.Click += new System.EventHandler(this.AcceptButton_Click);
            // 
            // _buttonCancel
            // 
            this._buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._buttonCancel.Location = new System.Drawing.Point(195, 4);
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
            this.label2.Font = new System.Drawing.Font("Segoe UI Emoji", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(12, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(274, 30);
            this.label2.TabIndex = 2;
            this.label2.Text = "Choose sentence terminator";
            // 
            // FormAddSentenceTerminator
            // 
            this.AcceptButton = this._buttonAccept;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this._buttonCancel;
            this.ClientSize = new System.Drawing.Size(296, 133);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "FormAddSentenceTerminator";
            this.Text = "Add sentence terminator";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox _comboBoxSentenceTerminator;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button _buttonAccept;
        private System.Windows.Forms.Button _buttonCancel;
        private System.Windows.Forms.Label label2;
    }
}