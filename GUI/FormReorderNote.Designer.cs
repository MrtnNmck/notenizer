using nsComponents;
using nsConstants;

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
            this.panel1 = new System.Windows.Forms.Panel();
            this._labelTitle = new System.Windows.Forms.Label();
            this._panelMain = new System.Windows.Forms.Panel();
            this._flowLayoutPanelDeleted = new AdvancedFlowLayoutPanel(new System.Drawing.Font(ComponentConstants.AdvancedLabelFontFamilyName, ComponentConstants.AdvancedLabelDeletedFontSize));
            this._labelSeparator = new System.Windows.Forms.Label();
            this._flowLayoutPanelActive = new AdvancedFlowLayoutPanel(new System.Drawing.Font(ComponentConstants.AdvancedLabelFontFamilyName, ComponentConstants.AdvancedLabelActiveFontSize));
            this._panelBottom = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this._panelMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this._labelTitle);
            this.panel1.Location = new System.Drawing.Point(13, 13);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(556, 46);
            this.panel1.TabIndex = 0;
            // 
            // _labelTitle
            // 
            this._labelTitle.AutoSize = true;
            this._labelTitle.Font = new System.Drawing.Font("Segoe UI Emoji", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._labelTitle.Location = new System.Drawing.Point(4, 4);
            this._labelTitle.Name = "_labelTitle";
            this._labelTitle.Size = new System.Drawing.Size(129, 37);
            this._labelTitle.TabIndex = 0;
            this._labelTitle.Text = "Edit Note";
            // 
            // _panelMain
            // 
            this._panelMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._panelMain.Controls.Add(this._flowLayoutPanelDeleted);
            this._panelMain.Controls.Add(this._labelSeparator);
            this._panelMain.Controls.Add(this._flowLayoutPanelActive);
            this._panelMain.Location = new System.Drawing.Point(13, 66);
            this._panelMain.Name = "_panelMain";
            this._panelMain.Size = new System.Drawing.Size(556, 381);
            this._panelMain.TabIndex = 1;
            // 
            // _flowLayoutPanelDeleted
            // 
            this._flowLayoutPanelDeleted.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._flowLayoutPanelDeleted.Location = new System.Drawing.Point(4, 195);
            this._flowLayoutPanelDeleted.Name = "_flowLayoutPanelDeleted";
            this._flowLayoutPanelDeleted.Size = new System.Drawing.Size(549, 183);
            this._flowLayoutPanelDeleted.TabIndex = 2;
            this._flowLayoutPanelDeleted.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            // 
            // _labelSeparator
            // 
            this._labelSeparator.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._labelSeparator.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._labelSeparator.Location = new System.Drawing.Point(3, 189);
            this._labelSeparator.Name = "_labelSeparator";
            this._labelSeparator.Size = new System.Drawing.Size(550, 2);
            this._labelSeparator.TabIndex = 1;
            // 
            // _flowLayoutPanelActive
            // 
            this._flowLayoutPanelActive.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._flowLayoutPanelActive.Location = new System.Drawing.Point(4, 4);
            this._flowLayoutPanelActive.Name = "_flowLayoutPanelActive";
            this._flowLayoutPanelActive.Size = new System.Drawing.Size(549, 182);
            this._flowLayoutPanelActive.TabIndex = 0;
            this._flowLayoutPanelActive.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            // 
            // _panelBottom
            // 
            this._panelBottom.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._panelBottom.Location = new System.Drawing.Point(13, 453);
            this._panelBottom.Name = "_panelBottom";
            this._panelBottom.Size = new System.Drawing.Size(556, 37);
            this._panelBottom.TabIndex = 2;
            // 
            // FormReorderNote
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(581, 502);
            this.Controls.Add(this._panelBottom);
            this.Controls.Add(this._panelMain);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "FormReorderNote";
            this.Text = "Edit Note";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this._panelMain.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label _labelTitle;
        private System.Windows.Forms.Panel _panelMain;
        private System.Windows.Forms.Label _labelSeparator;
        private AdvancedFlowLayoutPanel _flowLayoutPanelActive;
        private System.Windows.Forms.Panel _panelBottom;
        private AdvancedFlowLayoutPanel _flowLayoutPanelDeleted;
    }
}