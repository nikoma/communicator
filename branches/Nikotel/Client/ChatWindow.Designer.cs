namespace Remwave.Client
{
    partial class ChatWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ChatWindow));
            this.myColorDialog = new System.Windows.Forms.ColorDialog();
            this.myFontDialog = new System.Windows.Forms.FontDialog();
            this.myChatSessionsTabControl = new Telerik.WinControls.UI.RadTabStrip();
            this.radTitleBar1 = new Telerik.WinControls.UI.RadTitleBar();
            this.imageList2 = new System.Windows.Forms.ImageList(this.components);
            this.panelFormBg = new System.Windows.Forms.Panel();
            this.panelSearch = new System.Windows.Forms.Panel();
            this.tbxSearchText = new System.Windows.Forms.TextBox();
            this.btnSearchCancel = new System.Windows.Forms.Label();
            this.lblSearchIcon = new System.Windows.Forms.Label();
            this.toolStripShortcutSearch = new System.Windows.Forms.MenuStrip();
            this.toolStripMenuSearch = new System.Windows.Forms.ToolStripMenuItem();
            this.timerSecond = new System.Windows.Forms.Timer(this.components);
            this.myOpenFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.mySaveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.myNotifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.myChatSessionsTabControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radTitleBar1)).BeginInit();
            this.panelFormBg.SuspendLayout();
            this.panelSearch.SuspendLayout();
            this.toolStripShortcutSearch.SuspendLayout();
            this.SuspendLayout();
            // 
            // myChatSessionsTabControl
            // 
            this.myChatSessionsTabControl.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(83)))), ((int)(((byte)(83)))), ((int)(((byte)(83)))));
            this.myChatSessionsTabControl.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.myChatSessionsTabControl.DisableMouseEvents = false;
            this.myChatSessionsTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.myChatSessionsTabControl.ImageList = null;
            this.myChatSessionsTabControl.ItemsOffset = 10;
            this.myChatSessionsTabControl.Location = new System.Drawing.Point(2, 10);
            this.myChatSessionsTabControl.Margin = new System.Windows.Forms.Padding(0);
            this.myChatSessionsTabControl.Name = "myChatSessionsTabControl";
            this.myChatSessionsTabControl.Padding = new System.Windows.Forms.Padding(0, 26, 0, 0);
            // 
            // myChatSessionsTabControl.RootElement
            // 
            this.myChatSessionsTabControl.RootElement.AccessibleDescription = "";
            this.myChatSessionsTabControl.RootElement.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(83)))), ((int)(((byte)(83)))), ((int)(((byte)(83)))));
            this.myChatSessionsTabControl.RootElement.KeyTip = "";
            this.myChatSessionsTabControl.RootElement.ToolTipText = null;
            this.myChatSessionsTabControl.ScrollOffsetStep = 5;
            this.myChatSessionsTabControl.ShowOverFlowButton = false;
            this.myChatSessionsTabControl.Size = new System.Drawing.Size(496, 358);
            this.myChatSessionsTabControl.SmallImageList = null;
            this.myChatSessionsTabControl.TabIndex = 0;
            this.myChatSessionsTabControl.TabScrollButtonsPosition = Telerik.WinControls.UI.TabScrollButtonsPosition.RightBottom;
            this.myChatSessionsTabControl.Text = "Chat Sessions";
            this.myChatSessionsTabControl.ThemeName = "Office2007Black";
            // 
            // radTitleBar1
            // 
            this.radTitleBar1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(83)))), ((int)(((byte)(83)))), ((int)(((byte)(83)))));
            this.radTitleBar1.Caption = "Chat";
            this.radTitleBar1.DisableMouseEvents = false;
            this.radTitleBar1.Dock = System.Windows.Forms.DockStyle.Top;
            this.radTitleBar1.ImageList = this.imageList2;
            this.radTitleBar1.LeftImage = ((System.Drawing.Image)(resources.GetObject("radTitleBar1.LeftImage")));
            this.radTitleBar1.Location = new System.Drawing.Point(0, 0);
            this.radTitleBar1.Name = "radTitleBar1";
            // 
            // radTitleBar1.RootElement
            // 
            this.radTitleBar1.RootElement.AccessibleDescription = "";
            this.radTitleBar1.RootElement.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(83)))), ((int)(((byte)(83)))), ((int)(((byte)(83)))));
            this.radTitleBar1.RootElement.KeyTip = "";
            this.radTitleBar1.RootElement.ToolTipText = null;
            this.radTitleBar1.Size = new System.Drawing.Size(500, 23);
            this.radTitleBar1.SmallImageList = null;
            this.radTitleBar1.TabIndex = 1;
            this.radTitleBar1.TabStop = false;
            this.radTitleBar1.Text = "radTitleBar1";
            this.radTitleBar1.ThemeName = "Office2007Black";
            // 
            // imageList2
            // 
            this.imageList2.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList2.ImageStream")));
            this.imageList2.TransparentColor = System.Drawing.Color.Magenta;
            this.imageList2.Images.SetKeyName(0, "minimize.png");
            this.imageList2.Images.SetKeyName(1, "maximize.png");
            this.imageList2.Images.SetKeyName(2, "close.png");
            // 
            // panelFormBg
            // 
            this.panelFormBg.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(83)))), ((int)(((byte)(83)))), ((int)(((byte)(83)))));
            this.panelFormBg.Controls.Add(this.panelSearch);
            this.panelFormBg.Controls.Add(this.myChatSessionsTabControl);
            this.panelFormBg.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelFormBg.Location = new System.Drawing.Point(0, 23);
            this.panelFormBg.Name = "panelFormBg";
            this.panelFormBg.Padding = new System.Windows.Forms.Padding(2, 10, 2, 0);
            this.panelFormBg.Size = new System.Drawing.Size(500, 368);
            this.panelFormBg.TabIndex = 10;
            // 
            // panelSearch
            // 
            this.panelSearch.BackColor = System.Drawing.SystemColors.Control;
            this.panelSearch.Controls.Add(this.tbxSearchText);
            this.panelSearch.Controls.Add(this.btnSearchCancel);
            this.panelSearch.Controls.Add(this.lblSearchIcon);
            this.panelSearch.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelSearch.Location = new System.Drawing.Point(2, 343);
            this.panelSearch.Name = "panelSearch";
            this.panelSearch.Size = new System.Drawing.Size(496, 25);
            this.panelSearch.TabIndex = 2;
            // 
            // tbxSearchText
            // 
            this.tbxSearchText.Font = new System.Drawing.Font("Trebuchet MS", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbxSearchText.Location = new System.Drawing.Point(28, 2);
            this.tbxSearchText.Name = "tbxSearchText";
            this.tbxSearchText.Size = new System.Drawing.Size(200, 21);
            this.tbxSearchText.TabIndex = 3;
            this.tbxSearchText.TextChanged += new System.EventHandler(this.tbxSearchText_TextChanged);
            // 
            // btnSearchCancel
            // 
            this.btnSearchCancel.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnSearchCancel.Image = global::Remwave.Client.Properties.Resources.iconCross;
            this.btnSearchCancel.Location = new System.Drawing.Point(480, 0);
            this.btnSearchCancel.Name = "btnSearchCancel";
            this.btnSearchCancel.Padding = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.btnSearchCancel.Size = new System.Drawing.Size(16, 25);
            this.btnSearchCancel.TabIndex = 2;
            this.btnSearchCancel.Visible = false;
            this.btnSearchCancel.Click += new System.EventHandler(this.btnSearchCancel_Click);
            // 
            // lblSearchIcon
            // 
            this.lblSearchIcon.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblSearchIcon.Image = global::Remwave.Client.Properties.Resources.iconMagnifier;
            this.lblSearchIcon.Location = new System.Drawing.Point(0, 0);
            this.lblSearchIcon.Name = "lblSearchIcon";
            this.lblSearchIcon.Padding = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.lblSearchIcon.Size = new System.Drawing.Size(16, 25);
            this.lblSearchIcon.TabIndex = 1;
            this.lblSearchIcon.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // toolStripShortcutSearch
            // 
            this.toolStripShortcutSearch.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.toolStripShortcutSearch.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuSearch});
            this.toolStripShortcutSearch.Location = new System.Drawing.Point(0, 391);
            this.toolStripShortcutSearch.Name = "toolStripShortcutSearch";
            this.toolStripShortcutSearch.Size = new System.Drawing.Size(500, 24);
            this.toolStripShortcutSearch.TabIndex = 11;
            this.toolStripShortcutSearch.Text = "menuStrip1";
            this.toolStripShortcutSearch.Visible = false;
            // 
            // toolStripMenuSearch
            // 
            this.toolStripMenuSearch.Name = "toolStripMenuSearch";
            this.toolStripMenuSearch.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F)));
            this.toolStripMenuSearch.Size = new System.Drawing.Size(52, 20);
            this.toolStripMenuSearch.Text = "Search";
            this.toolStripMenuSearch.Click += new System.EventHandler(this.toolStripShortcutSearch_Click);
            // 
            // timerSecond
            // 
            this.timerSecond.Enabled = true;
            this.timerSecond.Tick += new System.EventHandler(this.timerSecond_Tick);
            // 
            // myOpenFileDialog
            // 
            this.myOpenFileDialog.FileName = "*.*";
            this.myOpenFileDialog.Filter = "All files (*.*)|*.*";
            this.myOpenFileDialog.RestoreDirectory = true;
            this.myOpenFileDialog.Title = "Transfer File";
            // 
            // mySaveFileDialog
            // 
            this.mySaveFileDialog.AddExtension = false;
            this.mySaveFileDialog.Filter = "All files (*.*)|*.*";
            this.mySaveFileDialog.RestoreDirectory = true;
            this.mySaveFileDialog.Title = "File Transfer";
            // 
            // myNotifyIcon
            // 
            this.myNotifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("myNotifyIcon.Icon")));
            this.myNotifyIcon.Text = "You\'ve got message.";
            this.myNotifyIcon.MouseClick += new System.Windows.Forms.MouseEventHandler(this.myNotifyIcon_MouseClick);
            this.myNotifyIcon.DoubleClick += new System.EventHandler(this.myNotifyIcon_DoubleClick);
            // 
            // ChatWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(83)))), ((int)(((byte)(83)))), ((int)(((byte)(83)))));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.BorderWidth = 2;
            this.ClientSize = new System.Drawing.Size(500, 420);
            this.Controls.Add(this.panelFormBg);
            this.Controls.Add(this.radTitleBar1);
            this.Controls.Add(this.toolStripShortcutSearch);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.toolStripShortcutSearch;
            this.Name = "ChatWindow";
            this.Padding = new System.Windows.Forms.Padding(0, 0, 0, 5);
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Chat";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ChatWindow_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.myChatSessionsTabControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radTitleBar1)).EndInit();
            this.panelFormBg.ResumeLayout(false);
            this.panelSearch.ResumeLayout(false);
            this.panelSearch.PerformLayout();
            this.toolStripShortcutSearch.ResumeLayout(false);
            this.toolStripShortcutSearch.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Telerik.WinControls.UI.TabItem tmplChatTab;
        private System.Windows.Forms.ColorDialog myColorDialog;
        private System.Windows.Forms.FontDialog myFontDialog;
        private Telerik.WinControls.UI.RadTabStrip myChatSessionsTabControl;
        private Telerik.WinControls.UI.RadTitleBar radTitleBar1;
        private System.Windows.Forms.ImageList imageList2;
        private System.Windows.Forms.Panel panelFormBg;
        private System.Windows.Forms.Panel panelSearch;
        private System.Windows.Forms.TextBox tbxSearchText;
        private System.Windows.Forms.Label btnSearchCancel;
        private System.Windows.Forms.Label lblSearchIcon;
        private System.Windows.Forms.MenuStrip toolStripShortcutSearch;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuSearch;
        private System.Windows.Forms.Timer timerSecond;
        private System.Windows.Forms.OpenFileDialog myOpenFileDialog;
        private System.Windows.Forms.SaveFileDialog mySaveFileDialog;
        private System.Windows.Forms.NotifyIcon myNotifyIcon;     
    }
}
