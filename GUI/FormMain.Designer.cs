using nsComponents;

namespace nsGUI
{
    partial class FormMain
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
            this._labelCaption = new System.Windows.Forms.Label();
            this._panelHeader = new System.Windows.Forms.Panel();
            this._panelMain = new System.Windows.Forms.Panel();
            this._tableLayoutPanelMain = new nsComponents.AdvancedTableLayoutPanel();
            this._labelCoolumnCaption1 = new System.Windows.Forms.Label();
            this._labelColumnCaption2 = new System.Windows.Forms.Label();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.quitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clearToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._panelBottom = new System.Windows.Forms.Panel();
            this._advancedProgressBar = new nsComponents.AdvancedProgressBar(this.components);
            this.exportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.openFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._panelHeader.SuspendLayout();
            this._panelMain.SuspendLayout();
            this._tableLayoutPanelMain.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this._panelBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // _labelCaption
            // 
            this._labelCaption.AutoSize = true;
            this._labelCaption.Font = new System.Drawing.Font("Segoe UI Emoji", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._labelCaption.Location = new System.Drawing.Point(3, 0);
            this._labelCaption.Name = "_labelCaption";
            this._labelCaption.Size = new System.Drawing.Size(133, 37);
            this._labelCaption.TabIndex = 0;
            this._labelCaption.Text = "Notenizer";
            // 
            // _panelHeader
            // 
            this._panelHeader.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._panelHeader.Controls.Add(this._labelCaption);
            this._panelHeader.Location = new System.Drawing.Point(12, 27);
            this._panelHeader.Name = "_panelHeader";
            this._panelHeader.Size = new System.Drawing.Size(984, 50);
            this._panelHeader.TabIndex = 1;
            // 
            // _panelMain
            // 
            this._panelMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._panelMain.Controls.Add(this._tableLayoutPanelMain);
            this._panelMain.Location = new System.Drawing.Point(12, 83);
            this._panelMain.Name = "_panelMain";
            this._panelMain.Size = new System.Drawing.Size(984, 691);
            this._panelMain.TabIndex = 2;
            // 
            // _tableLayoutPanelMain
            // 
            this._tableLayoutPanelMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._tableLayoutPanelMain.AutoScroll = true;
            this._tableLayoutPanelMain.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this._tableLayoutPanelMain.ColumnCount = 2;
            this._tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this._tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this._tableLayoutPanelMain.Controls.Add(this._labelCoolumnCaption1, 0, 0);
            this._tableLayoutPanelMain.Controls.Add(this._labelColumnCaption2, 1, 0);
            this._tableLayoutPanelMain.Location = new System.Drawing.Point(3, 3);
            this._tableLayoutPanelMain.Name = "_tableLayoutPanelMain";
            this._tableLayoutPanelMain.RowCount = 1;
            this._tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this._tableLayoutPanelMain.Size = new System.Drawing.Size(978, 685);
            this._tableLayoutPanelMain.TabIndex = 0;
            // 
            // _labelCoolumnCaption1
            // 
            this._labelCoolumnCaption1.AutoSize = true;
            this._labelCoolumnCaption1.Dock = System.Windows.Forms.DockStyle.Fill;
            this._labelCoolumnCaption1.Font = new System.Drawing.Font("Segoe UI Emoji", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._labelCoolumnCaption1.Location = new System.Drawing.Point(4, 1);
            this._labelCoolumnCaption1.Name = "_labelCoolumnCaption1";
            this._labelCoolumnCaption1.Size = new System.Drawing.Size(478, 688);
            this._labelCoolumnCaption1.TabIndex = 0;
            this._labelCoolumnCaption1.Text = "Original sentences";
            this._labelCoolumnCaption1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // _labelColumnCaption2
            // 
            this._labelColumnCaption2.AutoSize = true;
            this._labelColumnCaption2.Dock = System.Windows.Forms.DockStyle.Fill;
            this._labelColumnCaption2.Font = new System.Drawing.Font("Segoe UI Emoji", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._labelColumnCaption2.Location = new System.Drawing.Point(492, 1);
            this._labelColumnCaption2.Name = "_labelColumnCaption2";
            this._labelColumnCaption2.Size = new System.Drawing.Size(478, 688);
            this._labelColumnCaption2.TabIndex = 1;
            this._labelColumnCaption2.Text = "Notes";
            this._labelColumnCaption2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1008, 24);
            this.menuStrip1.TabIndex = 3;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.openFileToolStripMenuItem,
            this.toolStripMenuItem3,
            this.exportToolStripMenuItem,
            this.toolStripMenuItem1,
            this.quitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.openToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.Menu_Open);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(149, 6);
            // 
            // quitToolStripMenuItem
            // 
            this.quitToolStripMenuItem.Name = "quitToolStripMenuItem";
            this.quitToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Q)));
            this.quitToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.quitToolStripMenuItem.Text = "Quit";
            this.quitToolStripMenuItem.Click += new System.EventHandler(this.Menu_Quit);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.clearToolStripMenuItem});
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
            this.editToolStripMenuItem.Text = "Edit";
            // 
            // clearToolStripMenuItem
            // 
            this.clearToolStripMenuItem.Name = "clearToolStripMenuItem";
            this.clearToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.C)));
            this.clearToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.clearToolStripMenuItem.Text = "Clear";
            this.clearToolStripMenuItem.Click += new System.EventHandler(this.Menu_Clear);
            // 
            // _panelBottom
            // 
            this._panelBottom.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._panelBottom.Controls.Add(this._advancedProgressBar);
            this._panelBottom.Location = new System.Drawing.Point(12, 780);
            this._panelBottom.Name = "_panelBottom";
            this._panelBottom.Size = new System.Drawing.Size(984, 29);
            this._panelBottom.TabIndex = 4;
            // 
            // _advancedProgressBar
            // 
            this._advancedProgressBar.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._advancedProgressBar.Location = new System.Drawing.Point(3, 3);
            this._advancedProgressBar.MarqueeAnimationSpeed = 0;
            this._advancedProgressBar.Name = "_advancedProgressBar";
            this._advancedProgressBar.Size = new System.Drawing.Size(978, 23);
            this._advancedProgressBar.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this._advancedProgressBar.TabIndex = 0;
            // 
            // exportToolStripMenuItem
            // 
            this.exportToolStripMenuItem.Name = "exportToolStripMenuItem";
            this.exportToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.exportToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.exportToolStripMenuItem.Text = "Export";
            this.exportToolStripMenuItem.Click += new System.EventHandler(this.Menu_Export);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(149, 6);
            // 
            // openFileToolStripMenuItem
            // 
            this.openFileToolStripMenuItem.Name = "openFileToolStripMenuItem";
            this.openFileToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.P)));
            this.openFileToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.openFileToolStripMenuItem.Text = "Open File";
            this.openFileToolStripMenuItem.Click += new System.EventHandler(this.Menu_OpenFile);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1008, 821);
            this.Controls.Add(this._panelBottom);
            this.Controls.Add(this._panelMain);
            this.Controls.Add(this._panelHeader);
            this.Controls.Add(this.menuStrip1);
            this.Name = "FormMain";
            this.Text = "Notenizer";
            this._panelHeader.ResumeLayout(false);
            this._panelHeader.PerformLayout();
            this._panelMain.ResumeLayout(false);
            this._tableLayoutPanelMain.ResumeLayout(false);
            this._tableLayoutPanelMain.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this._panelBottom.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label _labelCaption;
        private System.Windows.Forms.Panel _panelHeader;
        private System.Windows.Forms.Panel _panelMain;
        private AdvancedTableLayoutPanel _tableLayoutPanelMain;
        private System.Windows.Forms.Label _labelCoolumnCaption1;
        private System.Windows.Forms.Label _labelColumnCaption2;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem quitToolStripMenuItem;
        private System.Windows.Forms.Panel _panelBottom;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem clearToolStripMenuItem;
        private AdvancedProgressBar _advancedProgressBar;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem exportToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openFileToolStripMenuItem;
    }
}