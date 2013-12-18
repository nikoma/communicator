using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Remwave.ChatController;
using System.IO;
using Telerik.WinControls.UI;
using Telerik.WinControls;
using System.Runtime.InteropServices;
using Remwave.Client.Controls;
using System.Threading;
using Remwave.Services;
using System.Collections;

namespace Remwave.Client
{

    public partial class ChatWindow : Form
    {
        private ClientForm myClientForm;
        private Hashtable myChatSessions = new Hashtable();
        private Emoticons myEmoticons = new Emoticons(Directory.GetCurrentDirectory() + "\\Emoticons\\");
        int mMessageInTimeout = -1;
        bool mLaunchSearch = false;
        Int32 mSearchDelay = 0;
        Int32 mMinimumSearchDelay = 1;
        [DllImport("user32.dll")]

        static extern bool FlashWindow(IntPtr hwnd, bool bInvert);

        public ChatWindow(ClientForm clientForm)
        {
            InitializeComponent();
            LocalizeComponent();
            BrandComponent();
            myClientForm = clientForm;
            this.myChatSessionsTabControl.ImageList = this.myClientForm.getPresenceImageList();
            this.ClientSize = this.Size;
        }

        private void LocalizeComponent()
        {
            myChatSessionsTabControl.Text = Properties.Localization.txtChatTitleControl;
            toolStripShortcutSearch.Text = Properties.Localization.txtChatCMenuSearch;
            myOpenFileDialog.Title = Properties.Localization.txtChatTitleOpenFileDialog;
            mySaveFileDialog.Title = Properties.Localization.txtChatTitleSaveFileDialog;
            Text = Properties.Localization.txtChatTitle;
        }
        private void BrandComponent()
        {
            this.Icon = Properties.Resources.desktop;
            this.myNotifyIcon.Icon = Properties.Resources.desktop;

#if !BRAND_JOCCOME
            this.myChatSessionsTabControl.ThemeName = "Office2007Black";
            this.BackColor = Color.FromArgb(82, 81, 82);
#endif

        }
        public void NewChat(JabberUser jabberUser, bool setFocus)
        {
            int presenceStatus = 0;
            if (myClientForm.myBuddyPresence.Contains(jabberUser.JID)) presenceStatus = (int)myClientForm.myBuddyPresence[jabberUser.JID];

            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ChatWindow));

            if (myChatSessions[jabberUser.JID] == null)
            {
                ChatSession tmplChatSession = new ChatSession();
                ChatBox tmplChatBox = new ChatBox();
                tmplChatBox.JID = jabberUser.JID;
                tmplChatBox.ResetHTML();

                tmplChatSession.LastStatus = presenceStatus;
                tmplChatBox.wbConversation.GotFocus += new EventHandler(wbConversation_GotFocus);

                this.tmplChatTab = new Telerik.WinControls.UI.TabItem();

                //
                // tmplChatTab
                this.tmplChatTab.AccessibleDescription = "";
                this.tmplChatTab.CanFocus = true;
                this.tmplChatTab.Class = "TabItem";
                this.tmplChatTab.Tag = jabberUser.JID;
                this.tmplChatTab.ImageIndex = presenceStatus;
                this.tmplChatTab.TextImageRelation = TextImageRelation.ImageBeforeText;
                //
                // tmplChatTab.ContentPanel
                //
                this.tmplChatTab.ContentPanel.BackColor = System.Drawing.Color.Transparent;
                this.tmplChatTab.ContentPanel.Font = new System.Drawing.Font("Trebuchet MS", 9F, System.Drawing.FontStyle.Regular);
                this.tmplChatTab.ContentPanel.Location = new System.Drawing.Point(1, 22);
                this.tmplChatTab.ContentPanel.Name = "ContentPanel";
                this.tmplChatTab.ContentPanel.Size = new System.Drawing.Size(490, 241);
                this.tmplChatTab.ContentPanel.Dock = DockStyle.Fill;
                this.tmplChatTab.ContentPanel.TabIndex = 0;
                this.tmplChatTab.Text = jabberUser.Nick;
                this.tmplChatTab.ToolTipText = null;

                this.myChatSessionsTabControl.Items.AddRange(new Telerik.WinControls.RadItem[] { this.tmplChatTab });
                this.tmplChatTab.ContentPanel.BackgroundImage = null;
                this.tmplChatTab.ContentPanel.BackColor = Color.White;
                this.tmplChatTab.ContentPanel.Controls.Add(tmplChatBox);
                tmplChatBox.Dock = DockStyle.Fill;
                this.tmplChatTab.ContentPanel.Invalidate();

                //wire up all events

                tmplChatBox.toolStripCallUser.Click += new EventHandler(tmplVoiceCallToolStripButton_Click); ;
                tmplChatBox.toolStripColor.Click += new EventHandler(tmplChangeColorToolStripButton_Click); ;
                tmplChatBox.toolStripEmotSmile.Click += new EventHandler(tmplEmoticonSmileToolStripButton_Click); ;
                tmplChatBox.toolStripEvilgrin.Click += new EventHandler(tmplEmoticonEvilGrinToolStripButton_Click); ;
                tmplChatBox.toolStripFont.Click += new EventHandler(tmplChangeFontToolStripButton_Click);
                tmplChatBox.toolStripGrin.Click += new EventHandler(tmplEmoticonGrinToolStripButton_Click); ;
                tmplChatBox.toolStripHappy.Click += new EventHandler(tmplEmoticonHappyToolStripButton_Click); ;
                tmplChatBox.toolStripOpenArchive.Click += new EventHandler(toolStripOpenArchive_Click); ;
                tmplChatBox.toolStripSendMessage.Click += new EventHandler(tmplSendMessageToolStripButton_Click); ;
                tmplChatBox.toolStripStartVideo.Click += new EventHandler(tmplVideoCallToolStripButton_Click); ;
                tmplChatBox.toolStripSurprised.Click += new EventHandler(tmplEmoticonSurprisedToolStripButton_Click); ;
                tmplChatBox.toolStripTongue.Click += new EventHandler(tmplEmoticonToungeToolStripButton_Click); ;
                tmplChatBox.toolStripUnhappy.Click += new EventHandler(tmplEmoticonUnhappyToolStripButton_Click); ;
                tmplChatBox.toolStripWaii.Click += new EventHandler(tmplEmoticonWaiiToolStripButton_Click); ;
                tmplChatBox.toolStripWink.Click += new EventHandler(tmplEmoticonWinkToolStripButton_Click); ;
                tmplChatBox.toolStripSendNudge.Click += new EventHandler(toolStripSendNudge_Click);
                tmplChatBox.toolStripSendFile.Click += new EventHandler(toolStripSendFile_Click);
                tmplChatBox.tbMessage.KeyUp += new KeyEventHandler(tmplChatTabMessage_KeyUp);

                //disable foreign networks features
                if (jabberUser.Network != ConfigXMPPNetwork.Default)
                {
                    tmplChatBox.toolStripCallUser.Enabled = false;
                    tmplChatBox.toolStripStartVideo.Enabled = false;
                    tmplChatBox.toolStripSendNudge.Enabled = false;
                    tmplChatBox.toolStripSendFile.Enabled = false;
                    tmplChatBox.toolStripCallUser.ToolTipText = Properties.Localization.txtChatFeatureOnlyHomeNetwork;
                    tmplChatBox.toolStripStartVideo.ToolTipText = Properties.Localization.txtChatFeatureOnlyHomeNetwork;
                    tmplChatBox.toolStripSendNudge.ToolTipText = Properties.Localization.txtChatFeatureOnlyHomeNetwork;
                    tmplChatBox.toolStripSendFile.ToolTipText = Properties.Localization.txtChatFeatureOnlyHomeNetwork;
                }

#if NOVIDEO
                tmplChatBox.toolStripStartVideo.Visible = false;
                tmplChatBox.toolStripStartVideo.Enabled = false;
#endif
#if NOFILESHARING
                tmplChatBox.toolStripSendFile.Visible = false;
                tmplChatBox.toolStripSendFile.Enabled = false;
#endif

                tmplChatBox.LinkClicked += new ChatBox.LinkClickedHandler(tmplChatBox_LinkClicked);
                tmplChatBox.FileDroped += new ChatBox.FilesDropedHandler(tmplChatBox_FileDroped);

                this.myChatSessionsTabControl.SelectedTab = this.tmplChatTab;

                this.tmplChatTab.ContentPanel.ResumeLayout(true);

                RadButtonElement tmplCloseButton = new RadButtonElement("x");
                tmplCloseButton.ToolTipText = Properties.Localization.txtChatInfoCloseTab;
                tmplCloseButton.Tag = jabberUser.JID;
                tmplCloseButton.Click += new EventHandler(tmplCloseButton_Click);

                tmplCloseButton.Alignment = ContentAlignment.TopRight;
                tmplChatTab.Children[2].Margin = new Padding(0, 0, 15, 0);
                tmplChatTab.Children.Add(tmplCloseButton);

                tmplChatSession.ChatTab = tmplChatTab;
                tmplChatSession.ChatBox = tmplChatBox;
                tmplChatSession.JabberUser = jabberUser;

                myChatSessions.Add(jabberUser.JID, tmplChatSession);
                Search("%");
                tmplChatSession.ChatBox.ChatTabMessage.Focus();
            }
            else
            {
                ChatSession tmplChatSession = (ChatSession)myChatSessions[jabberUser.JID];
                myChatSessionsTabControl.SelectedTab = tmplChatSession.ChatTab;
            }

            if (myClientForm.mUserAccount.LoggedIn)
            {
                ShowIt();
            }

        }

        void tmplChatBox_FileDroped(ChatBox sender, string[] files)
        {
            if (files == null) return;

            foreach (String file in files)
            {
                InitializeFileTransfer(sender.JID, file);
            }
        }

        void tmplChatBox_LinkClicked(ChatBox sender, string id, string url)
        {
            if (!url.Contains(":")) return;

            string action = url.Split(new string[] { ":" }, 2, StringSplitOptions.None)[0];
            string argument = url.Split(new string[] { ":" }, 2, StringSplitOptions.None)[1];
            ChatSession tmplChatSession = null;
            switch (action)
            {
                case "ft-accept":
                    sender.SetLink(id, "", true);
                    myClientForm.FileTransferProcessAccept(sender.JID, argument);
                    break;
                case "ft-decline":
                    sender.SetLink(id, "", true);
                    myClientForm.FileTransferProcessCancel(sender.JID, argument);
                    break;
                case "ar-open":
                    tmplChatSession = (ChatSession)myChatSessions[myChatSessionsTabControl.SelectedTab.Tag];
                    if (tmplChatSession != null) myClientForm.OpenArchiveWindow(tmplChatSession.JabberUser);
                    break;
                case "vc-accept":
                    sender.SetLink(id, "", true);
                    tmplChatSession = (ChatSession)myChatSessions[myChatSessionsTabControl.SelectedTab.Tag];
                    if (tmplChatSession != null) myClientForm.VideoSessionProcessInviationResult(sender.JID, argument,true);
                    break;
                case "vc-decline":
                    sender.SetLink(id, "", true);
                    tmplChatSession = (ChatSession)myChatSessions[myChatSessionsTabControl.SelectedTab.Tag];
                    if (tmplChatSession != null) myClientForm.VideoSessionProcessInviationResult(sender.JID, argument,false);
                    break;
            }
        }


        void toolStripSendFile_Click(object sender, EventArgs e)
        {
            ChatSession tmplChatSession = (ChatSession)myChatSessions[myChatSessionsTabControl.SelectedTab.Tag];
            if (tmplChatSession != null)
            {
                if (DialogResult.OK == myOpenFileDialog.ShowDialog())
                {
                    if (myOpenFileDialog.FileName != null && File.Exists(myOpenFileDialog.FileName))
                    {
                        InitializeFileTransfer(tmplChatSession.JabberUser.JID, myOpenFileDialog.FileName);

                    }
                }
            }
        }

        private void InitializeFileTransfer(string toJID, string file)
        {
            if (!File.Exists(file)) return;
            try
            {
                String ID = Guid.NewGuid().ToString();
                //PROCESS UPLOAD
                FileInfo fileInfo = new FileInfo(file);
                FileTransfer fileUpload = new FileTransfer();
                fileUpload.UploadCompleted += new EventHandler(fileTransfer_UploadCompleted);
                fileUpload.UploadProgressChanged += new EventHandler(fileUpload_UploadProgressChanged);
                SharedFile fileUploadInfo = fileUpload.Upload(file, myClientForm.mUserAccount.Username, myClientForm.mUserAccount.Password, toJID, ID);
                fileUploadInfo.Size = fileInfo.Length;
                myClientForm.FileTransferInvite(new JabberUser(fileUploadInfo.ToJID), fileUploadInfo.FileName, fileUploadInfo.ID, FileTransfer.FormatFileSize(fileUploadInfo.Size));
            }
            catch (Exception)
            {
#if (DEBUG)
                throw;
#endif
            }
        }

        void fileUpload_UploadProgressChanged(object sender, EventArgs e)
        {
            SharedFile sharedFile = (SharedFile)sender;
            ChatSession tmplChatSession = (ChatSession)myChatSessions[sharedFile.ToJID];
            if (tmplChatSession != null)
            {
                tmplChatSession.ChatBox.SetLink(IMMessage.NormalizeGUID(sharedFile.ID), sharedFile.FileName.ToString() + " " + sharedFile.GetSizeFormated() + " (" + (100 * sharedFile.Offset / sharedFile.Size) + @"%)", true);
            }
        }

        void fileDownload_DownloadProgressChanged(object sender, EventArgs e)
        {
            SharedFile sharedFile = (SharedFile)sender;
            ChatSession tmplChatSession = (ChatSession)myChatSessions[sharedFile.FromJID];
            if (tmplChatSession != null)
            {
                tmplChatSession.ChatBox.SetLink(IMMessage.NormalizeGUID(sharedFile.ID), sharedFile.FileName.ToString() + " " + sharedFile.GetSizeFormated() + " (" + (100 * sharedFile.Offset / sharedFile.Size) + @"%)", true);
            }
        }

        public void ProcessFileDownload(String jabberID, String filename, String fileTransferID)
        {
            FileTransfer fileDownload = new FileTransfer();
            fileDownload.DownloadCompleted += new EventHandler(fileDownload_DownloadCompleted);
            fileDownload.DownloadProgressChanged += new EventHandler(fileDownload_DownloadProgressChanged);
            SharedFile fileDownloadInfo = fileDownload.Download(filename, myClientForm.mUserAccount.Username, myClientForm.mUserAccount.Password, jabberID, fileTransferID);
        }



        void fileDownload_DownloadCompleted(object sender, EventArgs e)
        {
            SharedFile sharedFile = (SharedFile)sender;
            if (sharedFile.LocalFileName == null)
            {
                //transfer failed
                if (DialogResult.Retry == MessageBox.Show(
                    Properties.Localization.txtChatFileTransferFailedDesc,
                   Properties.Localization.txtChatFileTransferFailed,
                    MessageBoxButtons.RetryCancel,
                    MessageBoxIcon.Error,
                    MessageBoxDefaultButton.Button1
                    ))
                {
                    ProcessFileDownload(sharedFile.FromJID, sharedFile.FileName, sharedFile.ID);
                }
            }
            else
            {

                mySaveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                mySaveFileDialog.FileName = sharedFile.FileName;
                if (DialogResult.OK == mySaveFileDialog.ShowDialog())
                {
                    if (File.Exists(mySaveFileDialog.FileName)) File.Delete(mySaveFileDialog.FileName);
                    File.Move(sharedFile.LocalFileName, mySaveFileDialog.FileName);
                }
            }
        }
        void fileDownload_UploadProgressChanged(object sender, EventArgs e)
        {
            SharedFile sharedFile = (SharedFile)sender;
            ChatSession tmplChatSession = (ChatSession)myChatSessions[sharedFile.ToJID];
            if (tmplChatSession != null)
            {

                tmplChatSession.ChatBox.SetLink(IMMessage.NormalizeGUID(sharedFile.ID), sharedFile.Size.ToString() + " " + (sharedFile.Offset / sharedFile.Size), true);
            }
        }
        void fileTransfer_UploadCompleted(object sender, EventArgs e)
        {
            SharedFile sharedFile = (SharedFile)sender;
            if (sharedFile.LocalFileName != null)
            {
                myClientForm.FileTransferProcessDone(sharedFile.ToJID, sharedFile.FileName, sharedFile.ID, FileTransfer.FormatFileSize(sharedFile.Size));
            }
        }

        void toolStripSendNudge_Click(object sender, EventArgs e)
        {
            ChatSession tmplChatSession = (ChatSession)myChatSessions[myChatSessionsTabControl.SelectedTab.Tag];
            if (tmplChatSession != null)
            {
                if (tmplChatSession.NudgeTimeout > 0)
                {
                    AddNotification(
                        tmplChatSession.JabberUser.JID,
                        "INFO",
                        String.Format(Properties.Localization.txtChatInfoNudgeAgain, tmplChatSession.NudgeTimeout),
                        DateTime.Now,
                        Guid.NewGuid().ToString(),
                        false,
                        true
                        );
                }
                else
                {
                    tmplChatSession.NudgeTimeout = 10;
                    string jabberXevent = "<x xmlns='jabber:x:event'><event-nudge/><datetime>" + DateTime.Now.ToUniversalTime().ToBinary() + "</datetime><id>" + Guid.NewGuid().ToString() + "</id></x>";
                    bool msgSent = myClientForm.SendJabberXevent(tmplChatSession.JabberUser.JID, jabberXevent);
                    AddNotification(
                        tmplChatSession.JabberUser.JID,
                        "INFO",
                        Properties.Localization.txtChatInfoNudgeSent,
                        DateTime.Now,
                        Guid.NewGuid().ToString(),
                        false,
                        true
                        );
                }

            }
        }

        void wbConversation_GotFocus(object sender, EventArgs e)
        {
            if (panelSearch.Visible)
            {
                tbxSearchText.Focus();
            }
            else
            {
                ChatSession tmplChatSession = (ChatSession)myChatSessions[myChatSessionsTabControl.SelectedTab.Tag];
                tmplChatSession.ChatBox.ChatTabMessage.Focus();
            }
        }

        void toolStripOpenArchive_Click(object sender, EventArgs e)
        {
            ChatSession tmplChatSession = (ChatSession)myChatSessions[myChatSessionsTabControl.SelectedTab.Tag];
            if (tmplChatSession != null) myClientForm.OpenArchiveWindow(tmplChatSession.JabberUser);
        }



        void tmplVideoCallToolStripButton_Click(object sender, EventArgs e)
        {
            ChatSession tmplChatSession = (ChatSession)myChatSessions[myChatSessionsTabControl.SelectedTab.Tag];
            myClientForm.VideoSessionInvite(tmplChatSession.JabberUser);
        }

        void tmplVoiceCallToolStripButton_Click(object sender, EventArgs e)
        {
            ChatSession tmplChatSession = (ChatSession)myChatSessions[myChatSessionsTabControl.SelectedTab.Tag];
            myClientForm.StartNewCall(-1, tmplChatSession.JabberUser);
        }

        void tmplSendMessageToolStripButton_Click(object sender, EventArgs e)
        {
            SendMessage();
        }

        void tmplCloseButton_Click(object sender, EventArgs e)
        {
            if (myChatSessionsTabControl.Items.Count <= 1)
            {
                this.Close();
            }

            RadElement buttonElement = sender as RadElement;
            if (buttonElement.Tag != null)
            {
                try
                {
                    ChatSession tmplChatSession = (ChatSession)myChatSessions[buttonElement.Tag];
                    myChatSessionsTabControl.Items.Remove(tmplChatSession.ChatTab);
                    myChatSessions.Remove(buttonElement.Tag);
                }
                catch (Exception)
                {

                }
            }

            if (myChatSessionsTabControl.Items.Count > 0)
            {
                myChatSessionsTabControl.SelectLastVisibleItem();
            }
        }

        #region Emoticons

        void tmplEmoticonWinkToolStripButton_Click(object sender, EventArgs e)
        {
            ChatSession tmplChatSession = (ChatSession)myChatSessions[myChatSessionsTabControl.SelectedTab.Tag];
            tmplChatSession.ChatBox.ChatTabMessage.Text += " ;-)";
        }

        void tmplEmoticonWaiiToolStripButton_Click(object sender, EventArgs e)
        {
            ChatSession tmplChatSession = (ChatSession)myChatSessions[myChatSessionsTabControl.SelectedTab.Tag];
            tmplChatSession.ChatBox.ChatTabMessage.Text += " 8-)";
        }

        void tmplEmoticonUnhappyToolStripButton_Click(object sender, EventArgs e)
        {
            ChatSession tmplChatSession = (ChatSession)myChatSessions[myChatSessionsTabControl.SelectedTab.Tag];
            tmplChatSession.ChatBox.ChatTabMessage.Text += " :-(";
        }

        void tmplEmoticonToungeToolStripButton_Click(object sender, EventArgs e)
        {
            ChatSession tmplChatSession = (ChatSession)myChatSessions[myChatSessionsTabControl.SelectedTab.Tag];
            tmplChatSession.ChatBox.ChatTabMessage.Text += " :-p";
        }

        void tmplEmoticonSurprisedToolStripButton_Click(object sender, EventArgs e)
        {
            ChatSession tmplChatSession = (ChatSession)myChatSessions[myChatSessionsTabControl.SelectedTab.Tag];
            tmplChatSession.ChatBox.ChatTabMessage.Text += " :-o";
        }

        void tmplEmoticonHappyToolStripButton_Click(object sender, EventArgs e)
        {
            ChatSession tmplChatSession = (ChatSession)myChatSessions[myChatSessionsTabControl.SelectedTab.Tag];
            tmplChatSession.ChatBox.ChatTabMessage.Text += " XD";
        }

        void tmplEmoticonGrinToolStripButton_Click(object sender, EventArgs e)
        {
            ChatSession tmplChatSession = (ChatSession)myChatSessions[myChatSessionsTabControl.SelectedTab.Tag];
            tmplChatSession.ChatBox.ChatTabMessage.Text += " :-D";
        }

        void tmplEmoticonEvilGrinToolStripButton_Click(object sender, EventArgs e)
        {
            ChatSession tmplChatSession = (ChatSession)myChatSessions[myChatSessionsTabControl.SelectedTab.Tag];
            tmplChatSession.ChatBox.ChatTabMessage.Text += " B-)";
        }

        void tmplEmoticonSmileToolStripButton_Click(object sender, EventArgs e)
        {
            ChatSession tmplChatSession = (ChatSession)myChatSessions[myChatSessionsTabControl.SelectedTab.Tag];
            tmplChatSession.ChatBox.ChatTabMessage.Text += " :-)";
        }
        #endregion

        #region Message Style
        void tmplChangeColorToolStripButton_Click(object sender, EventArgs e)
        {
            ChatSession tmplChatSession = (ChatSession)myChatSessions[myChatSessionsTabControl.SelectedTab.Tag];
            myColorDialog.Color = tmplChatSession.ChatBox.ChatTabMessage.BackColor;
            if (myColorDialog.ShowDialog() != DialogResult.Cancel)
            {
                tmplChatSession.ChatBox.ChatTabMessage.BackColor = myColorDialog.Color;
                tmplChatSession.OutgoingStyle.BackColor = myColorDialog.Color;
            }
        }

        void tmplChangeFontToolStripButton_Click(object sender, EventArgs e)
        {
            ChatSession tmplChatSession = (ChatSession)myChatSessions[myChatSessionsTabControl.SelectedTab.Tag];
            myFontDialog.ShowColor = true;
            myFontDialog.Font = tmplChatSession.ChatBox.ChatTabMessage.Font;
            myFontDialog.Color = tmplChatSession.ChatBox.ChatTabMessage.ForeColor;

            if (myFontDialog.ShowDialog() != DialogResult.Cancel)
            {
                tmplChatSession.ChatBox.ChatTabMessage.Font = myFontDialog.Font;
                tmplChatSession.ChatBox.ChatTabMessage.ForeColor = myFontDialog.Color;
                tmplChatSession.OutgoingStyle.Font = myFontDialog.Font;
                tmplChatSession.OutgoingStyle.ForeColor = myFontDialog.Color;
            }
        }
        #endregion

        public delegate void UpdatePresenceDelegate(string jid, int buddyAvailability);

        public void UpdatePresence(string jid, int buddyAvailability)
        {
            if (InvokeRequired)
            {
                this.Invoke(new UpdatePresenceDelegate(this.UpdatePresence), new object[] { jid, buddyAvailability });
                return;
            }
            ChatSession tmplChatSession = (ChatSession)myChatSessions[jid];
            if (tmplChatSession != null)
            {
                switch (buddyAvailability)
                {
                    case 0: tmplChatSession.ChatTab.ImageIndex = 0; break;// 'offline
                    case 1: tmplChatSession.ChatTab.ImageIndex = 1; break;// 'online
                    case 2: tmplChatSession.ChatTab.ImageIndex = 2; break;// 'away
                    case 3: tmplChatSession.ChatTab.ImageIndex = 3; break;// 'extended away
                    case 4: tmplChatSession.ChatTab.ImageIndex = 4; break;// 'do not disturb
                }
            }
        }

        public delegate void AddNotificationDelegate(String jid, string senderEvent, string messageText, DateTime messageDateTime, string messageGUID, bool flashWindow, bool forceChat);
        public void AddNotification(String jid, String senderEvent, string messageText, DateTime messageDateTime, string messageGUID, bool flashWindow, bool forceChat)
        {
            if (InvokeRequired)
            {
                this.Invoke(new AddNotificationDelegate(this.AddNotification), new object[] { jid, senderEvent, messageText, messageDateTime, messageGUID, flashWindow });
                return;
            }

            //IMMessage parameters
            MessageStyle style = new MessageStyle(Color.White, new System.Drawing.Font("Trebuchet MS", 8.5F, System.Drawing.FontStyle.Regular), Color.Gray, Color.Gray);
            MessageTemplateType template = MessageTemplateType.Notification;
            ChatSession tmplChatSession = (ChatSession)myChatSessions[jid];
            JabberUser jabberUser = new JabberUser(jid);
            switch (senderEvent)
            {
                case "INFO":
                    senderEvent = "";
                    if (flashWindow) myClientForm.myNotifyIcon.ShowBalloonTip(10, jabberUser.Nick, messageText, ToolTipIcon.Info);
                    break;
                case "CLIENT":
                case "SERVER":
                case "PRESENCE":
                case "BUDDYUPDATE":
                    senderEvent = Properties.Localization.txtChatInfoSenderSystem;
                    break;
                case "NUDGE":
                    senderEvent = "";
                    myClientForm.mClientEvents.RaiseEvent(Remwave.Client.Events.ClientEvent.IncomingNudge);
                    FlashWindow(this.Handle, true);
                    #region Shake Window
                    Random rand = new Random();
                    int left = this.Left;
                    int top = this.Top;
                    for (int i = 0; i < 30; i++)
                    {
                        int randLeft = rand.Next(-10, 10);
                        int randTop = rand.Next(-10, 10);
                        this.Left = (left + randLeft) > 0 ? left + randLeft : 0;
                        this.Top = (top + randTop) > 0 ? left + randLeft : 0;
                        Thread.Sleep(50);
                    }
                    this.Left = left;
                    this.Top = top;
                    #endregion
                    break;
                case "INVITE":
                    senderEvent = Properties.Localization.txtChatInfoSenderUser;
                    myClientForm.mClientEvents.RaiseEvent(Remwave.Client.Events.ClientEvent.IncomingInstantMessage);
                    break;
            }

            if (forceChat && tmplChatSession == null)
            {
                try
                {
                    JabberUser chatJabberUser;
                    ContactList contactList = myClientForm.mContactBook.getCandidatesForJabberID(jid);
                    if (contactList.Count > 0)
                    {
                        NTContact ntContact = (NTContact)contactList[0];
                        chatJabberUser = new JabberUser(ntContact.NTJabberID, ntContact.NTNickname);
                    }
                    else
                    {
                        chatJabberUser = new JabberUser(jid);
                    }

                    NewChat(chatJabberUser, false);
                    tmplChatSession = (ChatSession)myChatSessions[chatJabberUser.JID];
                    if (tmplChatSession == null) return;
                }
                catch (Exception)
                {
                    return;
                }
            }

            //if chat session exist display content in conversation window
            if (tmplChatSession != null)
            {
                if (tmplChatSession.ChatBox.ChatTabConversation != null)
                {
                    IMMessage message = new IMMessage(senderEvent, messageText, messageGUID, messageDateTime, style, template, myEmoticons);
                    tmplChatSession.ChatBox.ChatTabConversation.Document.Body.InnerHtml += message.HTML;
                    tmplChatSession.ChatBox.ChatTabConversation.Document.Window.ScrollTo(0, tmplChatSession.ChatBox.ChatTabConversation.Document.Body.ScrollRectangle.Height);
                    tmplChatSession.ChatBox.AttachEvents();
                }
            }
        }

        public delegate void AddToConversationDelegate(String chatJID, String senderJID, string messageText, string messageHTML, DateTime messageDateTime, string messageGUID, bool forceEmpty, bool flashWindow);
        public void AddToConversation(String chatJID, String senderJID, string messageText, string messageHTML, DateTime messageDateTime, string messageGUID, bool forceEmpty, bool flashWindow)
        {

            if (InvokeRequired)
            {
                this.Invoke(new AddToConversationDelegate(this.AddToConversation), new object[] { chatJID, senderJID, messageText, messageDateTime, messageGUID, forceEmpty, flashWindow });
                return;
            }

            //IMMessage parameters

            MessageStyle style = new MessageStyle(Color.White, new System.Drawing.Font("Trebuchet MS", 8.5F, System.Drawing.FontStyle.Regular), Color.Gray, Color.Gray);
            MessageTemplateType template = MessageTemplateType.Notification;
            ChatSession tmplChatSession = (ChatSession)myChatSessions[chatJID];
            JabberUser chatJabberUser = null;
            JabberUser senderJabberUser = null;

            if (tmplChatSession == null)
            {
                try
                {
                    ContactList contactList = myClientForm.mContactBook.getCandidatesForJabberID(chatJID);
                    if (contactList.Count > 0)
                    {
                        NTContact ntContact = (NTContact)contactList[0];
                        chatJabberUser = new JabberUser(ntContact.NTJabberID, ntContact.NTNickname);
                    }
                    else
                    {
                        chatJabberUser = new JabberUser(chatJID);
                    }

                    NewChat(chatJabberUser, false);
                    tmplChatSession = (ChatSession)myChatSessions[chatJabberUser.JID];
                    if (tmplChatSession == null) return;
                }
                catch (Exception)
                {
                    return;
                }
            }
            if (forceEmpty)
            {
                tmplChatSession.ChatBox.ResetHTML();
            }
            if (senderJID == myClientForm.mUserAccount.JabberUser.JID)
            {
                //outgoing message
                senderJabberUser = myClientForm.mUserAccount.JabberUser;
                style = tmplChatSession.OutgoingStyle;
                template = MessageTemplateType.Out;

            }
            else
            { //incomming message
                senderJabberUser = tmplChatSession.JabberUser;
                if (flashWindow)
                {
                    FlashWindow(this.Handle, true);
                    myClientForm.mClientEvents.RaiseEvent(Remwave.Client.Events.ClientEvent.IncomingInstantMessage);
                    ShowIt();
                    mMessageInTimeout = 360;
                    myNotifyIcon.Visible = true;
                }
                style = tmplChatSession.IncomingStyle;
                template = MessageTemplateType.In;
            }

            //if chat session exist display content in conversation window
            if (tmplChatSession != null)
            {
                if (tmplChatSession.ChatBox.ChatTabConversation != null)
                {
                    SetComposing(false, senderJID);
                    if (messageHTML == "") messageHTML = messageText;

                    IMMessage message = new IMMessage(senderJabberUser.Nick, messageHTML, messageGUID, messageDateTime, style, template, myEmoticons);
                    tmplChatSession.ChatBox.ChatTabConversation.Document.Body.InnerHtml += message.HTML;
                    tmplChatSession.ChatBox.ChatTabConversation.Document.Window.ScrollTo(0, tmplChatSession.ChatBox.ChatTabConversation.Document.Body.ScrollRectangle.Height);
                    tmplChatSession.ChatBox.AttachEvents();
                }
            }
        }

        public void SetComposing(bool turnon, String jid)
        {
            //look to see who the message is from and send to the appropriate tab
            ChatSession tmplChatSession = (ChatSession)myChatSessions[jid];
            if (tmplChatSession != null)
            {
                if (tmplChatSession.ChatTab.ImageIndex == 0) return;
                //otherwise, either mark it as composing or nothing:
                if (turnon)
                {
                    tmplChatSession.ComposingTimeout = 30;
                    tmplChatSession.LastStatus = tmplChatSession.ChatTab.ImageIndex;
                    tmplChatSession.ChatTab.ImageIndex = 8;
                }
                else
                {
                    tmplChatSession.ChatTab.ImageIndex = tmplChatSession.LastStatus;
                }
            }
        }

        void tmplChatTabMessage_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (!e.Shift&&!e.Alt&&!e.Control)
                {
                    SendMessage();
                }
            }
            else
            {
                SendComposingEvent();
            }
        }

        private void SendComposingEvent()
        {
            ChatSession tmplChatSession = (ChatSession)myChatSessions[myChatSessionsTabControl.SelectedTab.Tag];
            if (tmplChatSession != null && tmplChatSession.SendComposingTimeout == 0)
            {
                //send composing notification
                tmplChatSession.SendComposingTimeout = 30;
                string jabberXevent = "<x xmlns='jabber:x:event'><composing/><datetime>" + DateTime.Now.ToUniversalTime().ToBinary() + "</datetime><id>" + Guid.NewGuid().ToString() + "</id></x>";
                bool msgSent = myClientForm.SendJabberXevent(tmplChatSession.JabberUser.JID, jabberXevent);
            }
        }

        private void SendMessage()
        {
            ChatSession tmplChatSession = (ChatSession)myChatSessions[myChatSessionsTabControl.SelectedTab.Tag];
            if (tmplChatSession != null)
            {

                if ((myClientForm.myBuddyPresence[tmplChatSession.JabberUser.JID] == null || (int)myClientForm.myBuddyPresence[tmplChatSession.JabberUser.JID] == 0))
                {
                    if (!tmplChatSession.OfflineMessageNotified)
                    {
                        tmplChatSession.OfflineMessageNotified = true;
                        AddNotification(
                             tmplChatSession.JabberUser.JID,
                             "INFO",
                             Properties.Localization.txtChatInfoUserOfflineMessage,
                             DateTime.Now,
                             Guid.NewGuid().ToString(),
                             false,
                             true
                             );
                    }
                }
                else
                {
                    tmplChatSession.OfflineMessageNotified = false;
                }

                IMMessage messageOutgoing = new IMMessage(tmplChatSession.JabberUser.Nick, tmplChatSession.ChatBox.ChatTabMessage.Text, Guid.NewGuid().ToString(),DateTime.Now, tmplChatSession.OutgoingStyle, MessageTemplateType.Outgoing, null);

                if (!myClientForm.SendMessage(tmplChatSession.JabberUser, messageOutgoing.Text, messageOutgoing.HTML)) return;

                AddToConversation(tmplChatSession.JabberUser.JID, 
                    myClientForm.mUserAccount.JabberUser.JID, 
                    tmplChatSession.ChatBox.ChatTabMessage.Text.Trim().Replace(@"&", @"&amp;").Replace(@"<", @"&lt;").Replace(@">", @"&gt;"),
                    "", 
                    DateTime.Now, 
                    Guid.NewGuid().ToString(), 
                    false, 
                    false
                    );
                tmplChatSession.ChatBox.ChatTabMessage.Text = "";
            }
        }

        private void Search(string search)
        {

            ChatSession tmplChatSession = (ChatSession)myChatSessions[myChatSessionsTabControl.SelectedTab.Tag];
            if (tmplChatSession != null)
            {
                tmplChatSession.ChatBox.ResetHTML();
                ChatController.MessageTemplate tmplMessageTemplate = new MessageTemplate(MessageTemplateType.Notification);
                tmplChatSession.ChatBox.AttachEvents();
                List<Remwave.Client.Storage.StorageMessage> list = myClientForm.myStorage.GetMessageFromArchive(tmplChatSession.JabberUser.JID, search, 50);

                if (list != null && list.Count > 0)
                {
                    for (int i = list.Count - 1; i >= 0; i--)
                    {
                        if (list[i].Direction == StorageItemDirection.In)
                        {
                            AddToConversation(tmplChatSession.JabberUser.JID, tmplChatSession.JabberUser.JID, list[i].ContentText, list[i].ContentHTML, list[i].Created, list[i].GUID, false, false);
                        }
                        else
                        {
                            AddToConversation(tmplChatSession.JabberUser.JID, myClientForm.mUserAccount.JabberUser.JID, list[i].ContentText, list[i].ContentHTML, list[i].Created, list[i].GUID, false, false);
                        }
                    }
                }
            }
        }

        private void btnSearchCancel_Click(object sender, EventArgs e)
        {
            tbxSearchText.Text = "";
            panelSearch.Visible = true;
            ChatSession tmplChatSession = (ChatSession)myChatSessions[myChatSessionsTabControl.SelectedTab.Tag];
            if (tmplChatSession != null)
            {
                tmplChatSession.ChatBox.ChatTabMessage.Focus();
            }
        }

        private void tbxSearchText_TextChanged(object sender, EventArgs e)
        {
            mLaunchSearch = true;
        }

        private void toolStripShortcutSearch_Click(object sender, EventArgs e)
        {
            panelSearch.Visible = true;
            tbxSearchText.Focus();
        }

        private void tbxSearchText_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                tbxSearchText.Text = "";
                panelSearch.Visible = true;
                ChatSession tmplChatSession = (ChatSession)myChatSessions[myChatSessionsTabControl.SelectedTab.Tag];
                if (tmplChatSession != null)
                {
                    tmplChatSession.ChatBox.ChatTabMessage.Focus();
                }
            }
        }

        private void timerSecond_Tick(object sender, EventArgs e)
        {
            try
            {
                foreach (object tmplChatSessionObj in myChatSessions.Values)
                {
                    ChatSession tmplChatSession = (ChatSession)tmplChatSessionObj;
                    if (tmplChatSession.ComposingTimeout > 0) tmplChatSession.ComposingTimeout--;
                    if (tmplChatSession.ComposingTimeout == 0)
                    {
                        SetComposing(false, tmplChatSession.JabberUser.JID);
                        tmplChatSession.ComposingTimeout--;
                    }
                    if (tmplChatSession.NudgeTimeout > 0) tmplChatSession.NudgeTimeout--;
                    if (tmplChatSession.SendComposingTimeout > 0) tmplChatSession.SendComposingTimeout--;
                }
                if (mMessageInTimeout > 0) mMessageInTimeout--;
                if (mMessageInTimeout == 0)
                {
                    myNotifyIcon.Visible = false;
                    mMessageInTimeout--;
                }
                if (mLaunchSearch)
                {
                    mSearchDelay++;
                    if (mSearchDelay > mMinimumSearchDelay)
                    {
                        mLaunchSearch = false;
                        mSearchDelay = 0;
                        Search(tbxSearchText.Text.Trim());
                    }
                }
            }
            catch (Exception ex)
            {
#if (TRACE)
                Console.WriteLine("timerSecond_Tick : " + ex.Message);
#endif
            }
        }

        private void ChatWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!myClientForm.mFormIsClosing)
            {
                e.Cancel = true;
                this.Hide();
            }
            else
            {
                timerSecond.Enabled = false;
            }
        }

        private void myNotifyIcon_MouseClick(object sender, MouseEventArgs e)
        {
            ShowIt();
        }

        private void myNotifyIcon_DoubleClick(object sender, EventArgs e)
        {
            ShowIt();
        }


        private void ShowIt()
        {
            mMessageInTimeout = 1;
            if (this.WindowState == FormWindowState.Minimized) this.WindowState = FormWindowState.Normal;
            this.Show();
            this.Activate();
            this.BringToFront();
        }

        internal void IncomingMessage(String jid, String messageText, String messageHTML)
        {
            if (messageText != "" || messageHTML != "")
            {
                JabberUser jabberUser = new JabberUser(jid);

                IMMessage messageIncoming = new IMMessage(jabberUser.Nick, messageText, Guid.NewGuid().ToString(), DateTime.Now, new MessageStyle(), MessageTemplateType.Incoming, null);
                AddToConversation(jabberUser.JID, jabberUser.JID, messageIncoming.Text, messageIncoming.HTML, messageIncoming.Time, messageIncoming.ID, false, true);
                myClientForm.myStorage.AddMessageToArchive(jabberUser.JID, myClientForm.myStorage.StorageGUID(), messageIncoming.Text, messageIncoming.HTML, false);
                SetComposing(false, jabberUser.JID);

#if(TRACE)
                Console.WriteLine("IM-Message:" + "(" + jabberUser.JID + ")" + messageIncoming.Text);
#endif
            }
        }
    }
}