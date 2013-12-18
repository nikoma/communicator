namespace Remwave.Client.Controls
{
    partial class ChatBox
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.splitContainerMain = new System.Windows.Forms.SplitContainer();
            this.wbConversation = new System.Windows.Forms.WebBrowser();
            this.tbMessage = new System.Windows.Forms.TextBox();
            this.tsToolbar = new System.Windows.Forms.ToolStrip();
            this.toolStripFont = new System.Windows.Forms.ToolStripButton();
            this.toolStripColor = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripEmotSmile = new System.Windows.Forms.ToolStripButton();
            this.toolStripGrin = new System.Windows.Forms.ToolStripButton();
            this.toolStripEvilgrin = new System.Windows.Forms.ToolStripButton();
            this.toolStripWink = new System.Windows.Forms.ToolStripButton();
            this.toolStripHappy = new System.Windows.Forms.ToolStripButton();
            this.toolStripSurprised = new System.Windows.Forms.ToolStripButton();
            this.toolStripTongue = new System.Windows.Forms.ToolStripButton();
            this.toolStripUnhappy = new System.Windows.Forms.ToolStripButton();
            this.toolStripWaii = new System.Windows.Forms.ToolStripButton();
            this.toolStripOpenArchive = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripCallUser = new System.Windows.Forms.ToolStripButton();
            this.toolStripSendFile = new System.Windows.Forms.ToolStripButton();
            this.toolStripStartVideo = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSendMessage = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSendNudge = new System.Windows.Forms.ToolStripButton();
            this.wbContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.wbContextMenu_Copy = new System.Windows.Forms.ToolStripMenuItem();
            this.wbContextMenu_SelectAll = new System.Windows.Forms.ToolStripMenuItem();
            this.splitContainerMain.Panel1.SuspendLayout();
            this.splitContainerMain.Panel2.SuspendLayout();
            this.splitContainerMain.SuspendLayout();
            this.tsToolbar.SuspendLayout();
            this.wbContextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainerMain
            // 
            this.splitContainerMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerMain.Location = new System.Drawing.Point(0, 0);
            this.splitContainerMain.Name = "splitContainerMain";
            this.splitContainerMain.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerMain.Panel1
            // 
            this.splitContainerMain.Panel1.Controls.Add(this.wbConversation);
            // 
            // splitContainerMain.Panel2
            // 
            this.splitContainerMain.Panel2.Controls.Add(this.tbMessage);
            this.splitContainerMain.Panel2.Controls.Add(this.tsToolbar);
            this.splitContainerMain.Size = new System.Drawing.Size(500, 300);
            this.splitContainerMain.SplitterDistance = 171;
            this.splitContainerMain.TabIndex = 0;
            // 
            // wbConversation
            // 
            this.wbConversation.AllowNavigation = false;
            this.wbConversation.AllowWebBrowserDrop = false;
            this.wbConversation.ContextMenuStrip = this.wbContextMenu;
            this.wbConversation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wbConversation.IsWebBrowserContextMenuEnabled = false;
            this.wbConversation.Location = new System.Drawing.Point(0, 0);
            this.wbConversation.MinimumSize = new System.Drawing.Size(20, 20);
            this.wbConversation.Name = "wbConversation";
            this.wbConversation.ScriptErrorsSuppressed = true;
            this.wbConversation.Size = new System.Drawing.Size(500, 171);
            this.wbConversation.TabIndex = 0;
            this.wbConversation.WebBrowserShortcutsEnabled = false;
            // 
            // tbMessage
            // 
            this.tbMessage.AcceptsReturn = true;
            this.tbMessage.AcceptsTab = true;
            this.tbMessage.AllowDrop = true;
            this.tbMessage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbMessage.Location = new System.Drawing.Point(0, 25);
            this.tbMessage.Multiline = true;
            this.tbMessage.Name = "tbMessage";
            this.tbMessage.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbMessage.Size = new System.Drawing.Size(500, 100);
            this.tbMessage.TabIndex = 0;
            this.tbMessage.DragDrop += new System.Windows.Forms.DragEventHandler(this.tbMessage_DragDrop);
            this.tbMessage.DragEnter += new System.Windows.Forms.DragEventHandler(this.tbMessage_DragEnter);
            // 
            // tsToolbar
            // 
            this.tsToolbar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripFont,
            this.toolStripColor,
            this.toolStripSeparator1,
            this.toolStripEmotSmile,
            this.toolStripGrin,
            this.toolStripEvilgrin,
            this.toolStripWink,
            this.toolStripHappy,
            this.toolStripSurprised,
            this.toolStripTongue,
            this.toolStripUnhappy,
            this.toolStripWaii,
            this.toolStripOpenArchive,
            this.toolStripSeparator3,
            this.toolStripCallUser,
            this.toolStripSendFile,
            this.toolStripStartVideo,
            this.toolStripSeparator2,
            this.toolStripSendMessage,
            this.toolStripSeparator4,
            this.toolStripSendNudge});
            this.tsToolbar.Location = new System.Drawing.Point(0, 0);
            this.tsToolbar.Name = "tsToolbar";
            this.tsToolbar.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.tsToolbar.Size = new System.Drawing.Size(500, 25);
            this.tsToolbar.TabIndex = 1;
            this.tsToolbar.Text = "toolStrip1";
            // 
            // toolStripFont
            // 
            this.toolStripFont.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripFont.Image = global::Remwave.Client.Properties.Resources.iconFont;
            this.toolStripFont.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripFont.Name = "toolStripFont";
            this.toolStripFont.Size = new System.Drawing.Size(23, 22);
            this.toolStripFont.Text = "Font";
            // 
            // toolStripColor
            // 
            this.toolStripColor.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripColor.Image = global::Remwave.Client.Properties.Resources.iconColorSwatch;
            this.toolStripColor.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripColor.Name = "toolStripColor";
            this.toolStripColor.Size = new System.Drawing.Size(23, 22);
            this.toolStripColor.Text = "Color";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripEmotSmile
            // 
            this.toolStripEmotSmile.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripEmotSmile.Image = global::Remwave.Client.Properties.Resources.emoticonSmile;
            this.toolStripEmotSmile.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripEmotSmile.Name = "toolStripEmotSmile";
            this.toolStripEmotSmile.Size = new System.Drawing.Size(23, 22);
            this.toolStripEmotSmile.Text = "Smile";
            // 
            // toolStripGrin
            // 
            this.toolStripGrin.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripGrin.Image = global::Remwave.Client.Properties.Resources.emoticonGrin;
            this.toolStripGrin.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripGrin.Name = "toolStripGrin";
            this.toolStripGrin.Size = new System.Drawing.Size(23, 22);
            this.toolStripGrin.Text = "Grin";
            // 
            // toolStripEvilgrin
            // 
            this.toolStripEvilgrin.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripEvilgrin.Image = global::Remwave.Client.Properties.Resources.emoticonEvilgrin;
            this.toolStripEvilgrin.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripEvilgrin.Name = "toolStripEvilgrin";
            this.toolStripEvilgrin.Size = new System.Drawing.Size(23, 22);
            this.toolStripEvilgrin.Text = "Evilgrin";
            // 
            // toolStripWink
            // 
            this.toolStripWink.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripWink.Image = global::Remwave.Client.Properties.Resources.emoticonWink;
            this.toolStripWink.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripWink.Name = "toolStripWink";
            this.toolStripWink.Size = new System.Drawing.Size(23, 22);
            this.toolStripWink.Text = "Wink";
            // 
            // toolStripHappy
            // 
            this.toolStripHappy.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripHappy.Image = global::Remwave.Client.Properties.Resources.emoticonHappy;
            this.toolStripHappy.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripHappy.Name = "toolStripHappy";
            this.toolStripHappy.Size = new System.Drawing.Size(23, 22);
            this.toolStripHappy.Text = "Happy";
            // 
            // toolStripSurprised
            // 
            this.toolStripSurprised.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripSurprised.Image = global::Remwave.Client.Properties.Resources.emoticonSurprised;
            this.toolStripSurprised.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripSurprised.Name = "toolStripSurprised";
            this.toolStripSurprised.Size = new System.Drawing.Size(23, 22);
            this.toolStripSurprised.Text = "Surprised";
            // 
            // toolStripTongue
            // 
            this.toolStripTongue.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripTongue.Image = global::Remwave.Client.Properties.Resources.emoticonTongue;
            this.toolStripTongue.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripTongue.Name = "toolStripTongue";
            this.toolStripTongue.Size = new System.Drawing.Size(23, 22);
            this.toolStripTongue.Text = "Tongue";
            // 
            // toolStripUnhappy
            // 
            this.toolStripUnhappy.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripUnhappy.Image = global::Remwave.Client.Properties.Resources.emoticonUnhappy;
            this.toolStripUnhappy.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripUnhappy.Name = "toolStripUnhappy";
            this.toolStripUnhappy.Size = new System.Drawing.Size(23, 22);
            this.toolStripUnhappy.Text = "Unhappy";
            // 
            // toolStripWaii
            // 
            this.toolStripWaii.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripWaii.Image = global::Remwave.Client.Properties.Resources.emoticonWaii;
            this.toolStripWaii.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripWaii.Name = "toolStripWaii";
            this.toolStripWaii.Size = new System.Drawing.Size(23, 22);
            this.toolStripWaii.Text = "Waii";
            // 
            // toolStripOpenArchive
            // 
            this.toolStripOpenArchive.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripOpenArchive.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripOpenArchive.Image = global::Remwave.Client.Properties.Resources.iconArchive;
            this.toolStripOpenArchive.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripOpenArchive.Name = "toolStripOpenArchive";
            this.toolStripOpenArchive.Size = new System.Drawing.Size(23, 22);
            this.toolStripOpenArchive.Text = "Open Archive";
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripCallUser
            // 
            this.toolStripCallUser.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripCallUser.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripCallUser.Image = global::Remwave.Client.Properties.Resources.listIconPhone;
            this.toolStripCallUser.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripCallUser.Name = "toolStripCallUser";
            this.toolStripCallUser.Size = new System.Drawing.Size(23, 22);
            this.toolStripCallUser.Text = "Call User";
            // 
            // toolStripSendFile
            // 
            this.toolStripSendFile.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripSendFile.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripSendFile.Image = global::Remwave.Client.Properties.Resources.attach;
            this.toolStripSendFile.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripSendFile.Name = "toolStripSendFile";
            this.toolStripSendFile.Size = new System.Drawing.Size(23, 22);
            this.toolStripSendFile.Text = "File transfer";
            // 
            // toolStripStartVideo
            // 
            this.toolStripStartVideo.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripStartVideo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripStartVideo.Image = global::Remwave.Client.Properties.Resources.listIconWebcam;
            this.toolStripStartVideo.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripStartVideo.Name = "toolStripStartVideo";
            this.toolStripStartVideo.Size = new System.Drawing.Size(23, 22);
            this.toolStripStartVideo.Text = "Video";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripSendMessage
            // 
            this.toolStripSendMessage.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripSendMessage.Image = global::Remwave.Client.Properties.Resources.iconMessage;
            this.toolStripSendMessage.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripSendMessage.Name = "toolStripSendMessage";
            this.toolStripSendMessage.Size = new System.Drawing.Size(51, 22);
            this.toolStripSendMessage.Text = "Send";
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripSendNudge
            // 
            this.toolStripSendNudge.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripSendNudge.Image = global::Remwave.Client.Properties.Resources.iconNudge;
            this.toolStripSendNudge.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripSendNudge.Name = "toolStripSendNudge";
            this.toolStripSendNudge.Size = new System.Drawing.Size(23, 22);
            this.toolStripSendNudge.Text = "Nudge";
            // 
            // wbContextMenu
            // 
            this.wbContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.wbContextMenu_Copy,
            this.wbContextMenu_SelectAll});
            this.wbContextMenu.Name = "wbContextMenu";
            this.wbContextMenu.Size = new System.Drawing.Size(153, 70);
            // 
            // wbContextMenu_Copy
            // 
            this.wbContextMenu_Copy.Name = "wbContextMenu_Copy";
            this.wbContextMenu_Copy.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            this.wbContextMenu_Copy.ShowShortcutKeys = false;
            this.wbContextMenu_Copy.Size = new System.Drawing.Size(152, 22);
            this.wbContextMenu_Copy.Text = "Copy";
            this.wbContextMenu_Copy.Click += new System.EventHandler(this.wbContextMenu_Copy_Click);
            // 
            // wbContextMenu_SelectAll
            // 
            this.wbContextMenu_SelectAll.Name = "wbContextMenu_SelectAll";
            this.wbContextMenu_SelectAll.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.A)));
            this.wbContextMenu_SelectAll.ShowShortcutKeys = false;
            this.wbContextMenu_SelectAll.Size = new System.Drawing.Size(152, 22);
            this.wbContextMenu_SelectAll.Text = "Select All";
            this.wbContextMenu_SelectAll.Click += new System.EventHandler(this.wbContextMenu_SelectAll_Click);
            // 
            // ChatBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainerMain);
            this.DoubleBuffered = true;
            this.Name = "ChatBox";
            this.Size = new System.Drawing.Size(500, 300);
            this.splitContainerMain.Panel1.ResumeLayout(false);
            this.splitContainerMain.Panel2.ResumeLayout(false);
            this.splitContainerMain.Panel2.PerformLayout();
            this.splitContainerMain.ResumeLayout(false);
            this.tsToolbar.ResumeLayout(false);
            this.tsToolbar.PerformLayout();
            this.wbContextMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainerMain;
        private System.Windows.Forms.ToolStrip tsToolbar;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        internal System.Windows.Forms.ToolStripButton toolStripSendNudge;
        internal System.Windows.Forms.ToolStripButton toolStripSendFile;
        internal System.Windows.Forms.WebBrowser wbConversation;
        internal System.Windows.Forms.TextBox tbMessage;
        internal System.Windows.Forms.ToolStripButton toolStripFont;
        internal System.Windows.Forms.ToolStripButton toolStripColor;
        internal System.Windows.Forms.ToolStripButton toolStripEmotSmile;
        internal System.Windows.Forms.ToolStripButton toolStripGrin;
        internal System.Windows.Forms.ToolStripButton toolStripEvilgrin;
        internal System.Windows.Forms.ToolStripButton toolStripWink;
        internal System.Windows.Forms.ToolStripButton toolStripHappy;
        internal System.Windows.Forms.ToolStripButton toolStripSurprised;
        internal System.Windows.Forms.ToolStripButton toolStripTongue;
        internal System.Windows.Forms.ToolStripButton toolStripUnhappy;
        internal System.Windows.Forms.ToolStripButton toolStripWaii;
        internal System.Windows.Forms.ToolStripButton toolStripSendMessage;
        internal System.Windows.Forms.ToolStripButton toolStripCallUser;
        internal System.Windows.Forms.ToolStripButton toolStripOpenArchive;
        internal System.Windows.Forms.ToolStripButton toolStripStartVideo;
        private System.Windows.Forms.ContextMenuStrip wbContextMenu;
        private System.Windows.Forms.ToolStripMenuItem wbContextMenu_Copy;
        private System.Windows.Forms.ToolStripMenuItem wbContextMenu_SelectAll;
    }
}
