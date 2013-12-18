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
            this.myMessageMenuImagesList = new System.Windows.Forms.ImageList(this.components);
            this.myColorDialog = new System.Windows.Forms.ColorDialog();
            this.myFontDialog = new System.Windows.Forms.FontDialog();
            this.myChatSessionsTabControl = new Telerik.WinControls.UI.RadTabStrip();
            this.menuStripShortcut = new System.Windows.Forms.MenuStrip();
            this.toolStripShortcutSearch = new System.Windows.Forms.ToolStripMenuItem();
            this.panelSearch = new System.Windows.Forms.Panel();
            this.tbxSearchText = new System.Windows.Forms.TextBox();
            this.btnSearchCancel = new System.Windows.Forms.Label();
            this.lblSearchIcon = new System.Windows.Forms.Label();
            this.timerSecond = new System.Windows.Forms.Timer(this.components);
            this.myOpenFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.mySaveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.myNotifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.myChatSessionsTabControl)).BeginInit();
            this.myChatSessionsTabControl.SuspendLayout();
            this.menuStripShortcut.SuspendLayout();
            this.panelSearch.SuspendLayout();
            this.SuspendLayout();
            // 
            // myMessageMenuImagesList
            // 
            this.myMessageMenuImagesList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("myMessageMenuImagesList.ImageStream")));
            this.myMessageMenuImagesList.TransparentColor = System.Drawing.Color.Transparent;
            this.myMessageMenuImagesList.Images.SetKeyName(0, "font.png");
            this.myMessageMenuImagesList.Images.SetKeyName(1, "color_swatch.png");
            this.myMessageMenuImagesList.Images.SetKeyName(2, "emoticon_smile.png");
            this.myMessageMenuImagesList.Images.SetKeyName(3, "emoticon_grin.png");
            this.myMessageMenuImagesList.Images.SetKeyName(4, "emoticon_evilgrin.png");
            this.myMessageMenuImagesList.Images.SetKeyName(5, "emoticon_wink.png");
            this.myMessageMenuImagesList.Images.SetKeyName(6, "emoticon_happy.png");
            this.myMessageMenuImagesList.Images.SetKeyName(7, "emoticon_surprised.png");
            this.myMessageMenuImagesList.Images.SetKeyName(8, "emoticon_tongue.png");
            this.myMessageMenuImagesList.Images.SetKeyName(9, "emoticon_unhappy.png");
            this.myMessageMenuImagesList.Images.SetKeyName(10, "emoticon_waii.png");
            this.myMessageMenuImagesList.Images.SetKeyName(11, "email_go.png");
            this.myMessageMenuImagesList.Images.SetKeyName(12, "iconPhone.png");
            this.myMessageMenuImagesList.Images.SetKeyName(13, "iconWebcam.png");
            // 
            // myChatSessionsTabControl
            // 
            this.myChatSessionsTabControl.BackColor = System.Drawing.Color.WhiteSmoke;
            this.myChatSessionsTabControl.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.myChatSessionsTabControl.Controls.Add(this.menuStripShortcut);
            this.myChatSessionsTabControl.DisableMouseEvents = false;
            this.myChatSessionsTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.myChatSessionsTabControl.ImageList = null;
            this.myChatSessionsTabControl.ItemsOffset = 10;
            this.myChatSessionsTabControl.Location = new System.Drawing.Point(0, 5);
            this.myChatSessionsTabControl.Margin = new System.Windows.Forms.Padding(0);
            this.myChatSessionsTabControl.Name = "myChatSessionsTabControl";
            this.myChatSessionsTabControl.Padding = new System.Windows.Forms.Padding(0, 26, 0, 0);
            // 
            // myChatSessionsTabControl.RootElement
            // 
            this.myChatSessionsTabControl.RootElement.AccessibleDescription = "";
            this.myChatSessionsTabControl.RootElement.BackColor = System.Drawing.Color.WhiteSmoke;
            this.myChatSessionsTabControl.RootElement.KeyTip = "";
            this.myChatSessionsTabControl.RootElement.ToolTipText = null;
            this.myChatSessionsTabControl.ScrollOffsetStep = 5;
            this.myChatSessionsTabControl.ShowOverFlowButton = false;
            this.myChatSessionsTabControl.Size = new System.Drawing.Size(492, 266);
            this.myChatSessionsTabControl.SmallImageList = null;
            this.myChatSessionsTabControl.TabIndex = 0;
            this.myChatSessionsTabControl.TabScrollButtonsPosition = Telerik.WinControls.UI.TabScrollButtonsPosition.RightBottom;
            this.myChatSessionsTabControl.Text = "Chat Sessions";
            this.myChatSessionsTabControl.ThemeName = "Telerik";
            // 
            // menuStripShortcut
            // 
            this.menuStripShortcut.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.menuStripShortcut.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripShortcutSearch});
            this.menuStripShortcut.Location = new System.Drawing.Point(0, 242);
            this.menuStripShortcut.Name = "menuStripShortcut";
            this.menuStripShortcut.Size = new System.Drawing.Size(492, 24);
            this.menuStripShortcut.TabIndex = 1;
            this.menuStripShortcut.Visible = false;
            // 
            // toolStripShortcutSearch
            // 
            this.toolStripShortcutSearch.Name = "toolStripShortcutSearch";
            this.toolStripShortcutSearch.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F)));
            this.toolStripShortcutSearch.Size = new System.Drawing.Size(52, 20);
            this.toolStripShortcutSearch.Text = "Search";
            this.toolStripShortcutSearch.Click += new System.EventHandler(this.toolStripShortcutSearch_Click);
            // 
            // panelSearch
            // 
            this.panelSearch.Controls.Add(this.tbxSearchText);
            this.panelSearch.Controls.Add(this.btnSearchCancel);
            this.panelSearch.Controls.Add(this.lblSearchIcon);
            this.panelSearch.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelSearch.Location = new System.Drawing.Point(0, 271);
            this.panelSearch.Name = "panelSearch";
            this.panelSearch.Size = new System.Drawing.Size(492, 25);
            this.panelSearch.TabIndex = 1;
            // 
            // tbxSearchText
            // 
            this.tbxSearchText.Font = new System.Drawing.Font("Trebuchet MS", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbxSearchText.Location = new System.Drawing.Point(28, 2);
            this.tbxSearchText.Name = "tbxSearchText";
            this.tbxSearchText.Size = new System.Drawing.Size(200, 21);
            this.tbxSearchText.TabIndex = 3;
            this.tbxSearchText.TextChanged += new System.EventHandler(this.tbxSearchText_TextChanged);
            this.tbxSearchText.KeyUp += new System.Windows.Forms.KeyEventHandler(this.tbxSearchText_KeyUp);
            // 
            // btnSearchCancel
            // 
            this.btnSearchCancel.Image = global::Remwave.Client.Properties.Resources.cancel;
            this.btnSearchCancel.Location = new System.Drawing.Point(473, 4);
            this.btnSearchCancel.Name = "btnSearchCancel";
            this.btnSearchCancel.Size = new System.Drawing.Size(16, 16);
            this.btnSearchCancel.TabIndex = 2;
            this.btnSearchCancel.Visible = false;
            this.btnSearchCancel.Click += new System.EventHandler(this.btnSearchCancel_Click);
            // 
            // lblSearchIcon
            // 
            this.lblSearchIcon.Image = global::Remwave.Client.Properties.Resources.magnifier;
            this.lblSearchIcon.Location = new System.Drawing.Point(4, 4);
            this.lblSearchIcon.Name = "lblSearchIcon";
            this.lblSearchIcon.Size = new System.Drawing.Size(16, 16);
            this.lblSearchIcon.TabIndex = 1;
            this.lblSearchIcon.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // timerSecond
            // 
            this.timerSecond.Enabled = true;
            this.timerSecond.Interval = 1000;
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
            this.myNotifyIcon.DoubleClick += new System.EventHandler(this.myNotifyIcon_DoubleClick);
            this.myNotifyIcon.MouseClick += new System.Windows.Forms.MouseEventHandler(this.myNotifyIcon_MouseClick);
            // 
            // ChatWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(492, 296);
            this.Controls.Add(this.myChatSessionsTabControl);
            this.Controls.Add(this.panelSearch);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStripShortcut;
            this.Name = "ChatWindow";
            this.Padding = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.Text = "Chat";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ChatWindow_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.myChatSessionsTabControl)).EndInit();
            this.myChatSessionsTabControl.ResumeLayout(false);
            this.myChatSessionsTabControl.PerformLayout();
            this.menuStripShortcut.ResumeLayout(false);
            this.menuStripShortcut.PerformLayout();
            this.panelSearch.ResumeLayout(false);
            this.panelSearch.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Telerik.WinControls.UI.TabItem tmplChatTab;

        private System.Windows.Forms.ColorDialog myColorDialog;
        private System.Windows.Forms.FontDialog myFontDialog;

        private Telerik.WinControls.UI.RadTabStrip myChatSessionsTabControl;
        
        private System.Windows.Forms.ImageList myMessageMenuImagesList;
        private System.Windows.Forms.Panel panelSearch;
        private System.Windows.Forms.Label lblSearchIcon;
        private System.Windows.Forms.Label btnSearchCancel;
        private System.Windows.Forms.MenuStrip menuStripShortcut;
        private System.Windows.Forms.ToolStripMenuItem toolStripShortcutSearch;
        private System.Windows.Forms.TextBox tbxSearchText;
        private System.Windows.Forms.Timer timerSecond;
        private System.Windows.Forms.OpenFileDialog myOpenFileDialog;
        private System.Windows.Forms.SaveFileDialog mySaveFileDialog;
        private System.Windows.Forms.NotifyIcon myNotifyIcon;     
    }
}