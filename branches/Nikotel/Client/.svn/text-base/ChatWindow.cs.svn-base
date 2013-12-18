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

    public partial class ChatWindow : ShapedForm
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
            myClientForm = clientForm;
            this.myChatSessionsTabControl.ImageList = this.myClientForm.myPresenceImagesList;
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

        public void NewChat(JabberUser jabberUser, bool setFocus)
        {           
            int presenceStatus = 0;
            if (myClientForm.myBuddyPresence.Contains(jabberUser.JID)) presenceStatus = (int)myClientForm.myBuddyPresence[jabberUser.JID];

            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ChatWindow));

            if (myChatSessions[jabberUser.JID] == null)
            {
                ChatSession tmplChatSession = new ChatSession();
                ChatBox tmplChatBox = new ChatBox();
                tmplChatBox.wbConversation.DocumentText = "<HTML><BODY></BODY></HTML>";
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

                tmplChatBox.tbMessage.Focus();
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
                tmplChatSession.ChatTabConversation = tmplChatBox.wbConversation;
                tmplChatSession.ChatTabMessage = tmplChatBox.tbMessage;
                tmplChatSession.JabberUser = jabberUser;

                myChatSessions.Add(jabberUser.JID, tmplChatSession);
                Search("%");
                tmplChatSession.ChatTabMessage.Focus();
            }
            else
            {
                ChatSession tmplChatSession = (ChatSession)myChatSessions[jabberUser.JID];
                myChatSessionsTabControl.SelectedTab = tmplChatSession.ChatTab;
            }

            if (myClientForm.mUserAccount.LoggedIn())
            {
                ShowIt();
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
                        String ID = Guid.NewGuid().ToString();
                        //PROCESS UPLOAD
                        FileInfo fileInfo = new FileInfo(myOpenFileDialog.FileName);
                        FileTransfer fileUpload = new FileTransfer();
                        fileUpload.UploadCompleted += new EventHandler(fileTransfer_UploadCompleted);
                        FileTransfer.SharedFile fileUploadInfo = fileUpload.Upload(myOpenFileDialog.FileName, myClientForm.mUserAccount.Username, myClientForm.mUserAccount.Password, tmplChatSession.JabberUser.JID, ID);
                        fileUploadInfo.Size = fileInfo.Length;
                        myClientForm.FileTransferInvite(new JabberUser(fileUploadInfo.ToJID), fileUploadInfo.FileName, fileUploadInfo.ID, FileTransfer.FormatFileSize(fileUploadInfo.Size));

                    }
                }
            }
        }

        public void ProcessFileTransfer(String jabberID, String filename, String fileTransferID)
        {
            FileTransfer fileDownload = new FileTransfer();
            fileDownload.DownloadCompleted += new EventHandler(fileDownload_DownloadCompleted);
            FileTransfer.SharedFile fileDownloadInfo = fileDownload.Download(filename, myClientForm.mUserAccount.Username, myClientForm.mUserAccount.Password, jabberID, fileTransferID);
        }

        void fileDownload_DownloadCompleted(object sender, EventArgs e)
        {
            FileTransfer.SharedFile sharedFile = (FileTransfer.SharedFile)sender;
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
                    ProcessFileTransfer(sharedFile.FromJID, sharedFile.FileName, sharedFile.ID);
                }
            }
            else
            {

                mySaveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                mySaveFileDialog.FileName = sharedFile.FileName;
                if (DialogResult.OK == mySaveFileDialog.ShowDialog())
                {
                    File.Move(sharedFile.LocalFileName, mySaveFileDialog.FileName);
                }
            }
        }

        void fileTransfer_UploadCompleted(object sender, EventArgs e)
        {
            FileTransfer.SharedFile sharedFile = (FileTransfer.SharedFile)sender;
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
                        String.Format(Properties.Localization.txtChatInfoNudgeAgain,tmplChatSession.NudgeTimeout),
                        DateTime.Now.ToString("hh:mm:ss.fffffff"), 
                        Guid.NewGuid().ToString(), 
                        false
                        );
                }
                else
                {
                    tmplChatSession.NudgeTimeout = 10;
                    string jabberXevent = "<x xmlns='jabber:x:event'><event-nudge/><id>" + DateTime.Now.ToString("hh:mm:ss.fffffff") + "</id></x>";
                    bool msgSent = myClientForm.SendJabberXevent(tmplChatSession.JabberUser.JID, jabberXevent);
                    AddNotification(
                        tmplChatSession.JabberUser.JID, 
                        "INFO",
                        Properties.Localization.txtChatInfoNudgeSent,
                        DateTime.Now.ToString("hh:mm:ss.fffffff"), 
                        Guid.NewGuid().ToString(), 
                        false
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
                tmplChatSession.ChatTabMessage.Focus();
            }
        }

        void toolStripOpenArchive_Click(object sender, EventArgs e)
        {
            if (myClientForm.myArchiveWindow == null)
            {

                this.myClientForm.myArchiveWindow = new ArchiveWindow();
                this.myClientForm.myArchiveWindow.Disposed += new EventHandler(myArchiveWindow_Disposed);
            }
            ChatSession tmplChatSession = (ChatSession)myChatSessions[myChatSessionsTabControl.SelectedTab.Tag];
            if (tmplChatSession != null)
            {
                this.myClientForm.myArchiveWindow.Open(myClientForm.myStorage, tmplChatSession.JabberUser);
                this.myClientForm.myArchiveWindow.tvUserList.ImageList = this.myClientForm.myPresenceImagesList;
                this.myClientForm.myArchiveWindow.Show();
                this.myClientForm.myArchiveWindow.Activate();
            }
        }

        void myArchiveWindow_Disposed(object sender, EventArgs e)
        {
            myClientForm.myArchiveWindow = null;
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
            tmplChatSession.ChatTabMessage.Text += " ;-)";
        }

        void tmplEmoticonWaiiToolStripButton_Click(object sender, EventArgs e)
        {
            ChatSession tmplChatSession = (ChatSession)myChatSessions[myChatSessionsTabControl.SelectedTab.Tag];
            tmplChatSession.ChatTabMessage.Text += " 8-)";
        }

        void tmplEmoticonUnhappyToolStripButton_Click(object sender, EventArgs e)
        {
            ChatSession tmplChatSession = (ChatSession)myChatSessions[myChatSessionsTabControl.SelectedTab.Tag];
            tmplChatSession.ChatTabMessage.Text += " :-(";
        }

        void tmplEmoticonToungeToolStripButton_Click(object sender, EventArgs e)
        {
            ChatSession tmplChatSession = (ChatSession)myChatSessions[myChatSessionsTabControl.SelectedTab.Tag];
            tmplChatSession.ChatTabMessage.Text += " :-p";
        }

        void tmplEmoticonSurprisedToolStripButton_Click(object sender, EventArgs e)
        {
            ChatSession tmplChatSession = (ChatSession)myChatSessions[myChatSessionsTabControl.SelectedTab.Tag];
            tmplChatSession.ChatTabMessage.Text += " :-o";
        }

        void tmplEmoticonHappyToolStripButton_Click(object sender, EventArgs e)
        {
            ChatSession tmplChatSession = (ChatSession)myChatSessions[myChatSessionsTabControl.SelectedTab.Tag];
            tmplChatSession.ChatTabMessage.Text += " XD";
        }

        void tmplEmoticonGrinToolStripButton_Click(object sender, EventArgs e)
        {
            ChatSession tmplChatSession = (ChatSession)myChatSessions[myChatSessionsTabControl.SelectedTab.Tag];
            tmplChatSession.ChatTabMessage.Text += " :-D";
        }

        void tmplEmoticonEvilGrinToolStripButton_Click(object sender, EventArgs e)
        {
            ChatSession tmplChatSession = (ChatSession)myChatSessions[myChatSessionsTabControl.SelectedTab.Tag];
            tmplChatSession.ChatTabMessage.Text += " B-)";
        }

        void tmplEmoticonSmileToolStripButton_Click(object sender, EventArgs e)
        {
            ChatSession tmplChatSession = (ChatSession)myChatSessions[myChatSessionsTabControl.SelectedTab.Tag];
            tmplChatSession.ChatTabMessage.Text += " :-)";
        }
        #endregion

        #region Message Style
        void tmplChangeColorToolStripButton_Click(object sender, EventArgs e)
        {
            ChatSession tmplChatSession = (ChatSession)myChatSessions[myChatSessionsTabControl.SelectedTab.Tag];
            myColorDialog.Color = tmplChatSession.ChatTabMessage.BackColor;
            if (myColorDialog.ShowDialog() != DialogResult.Cancel)
            {
                tmplChatSession.ChatTabMessage.BackColor = myColorDialog.Color;
                tmplChatSession.OutgoingStyle.BackColor = myColorDialog.Color;
            }
        }

        void tmplChangeFontToolStripButton_Click(object sender, EventArgs e)
        {
            ChatSession tmplChatSession = (ChatSession)myChatSessions[myChatSessionsTabControl.SelectedTab.Tag];
            myFontDialog.ShowColor = true;
            myFontDialog.Font = tmplChatSession.ChatTabMessage.Font;
            myFontDialog.Color = tmplChatSession.ChatTabMessage.ForeColor;

            if (myFontDialog.ShowDialog() != DialogResult.Cancel)
            {
                tmplChatSession.ChatTabMessage.Font = myFontDialog.Font;
                tmplChatSession.ChatTabMessage.ForeColor = myFontDialog.Color;
                tmplChatSession.OutgoingStyle.Font = myFontDialog.Font;
                tmplChatSession.OutgoingStyle.ForeColor = myFontDialog.Color;
            }
        }
        #endregion

        public void UpdatePresence(string jid, int buddyAvailability)
        {
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

        public delegate void AddNotificationDelegate(String jid, string senderEvent, string messageText, string messageDateTime, string messageGUID, bool flashWindow);
        public void AddNotification(String jid, String senderEvent, string messageText, string messageDateTime, string messageGUID, bool flashWindow)
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
            
            switch (senderEvent)
            {
                case "INFO":
                    senderEvent = "";
                    myClientForm.myNotifyIcon.ShowBalloonTip(10, jid, messageText, ToolTipIcon.Info);
                    break;
                case "CLIENT":
                case "SERVER":
                case "PRESENCE":
                case "BUDDYUPDATE":
                    senderEvent = Properties.Localization.txtChatInfoSenderSystem;
                    break;
                case "NUDGE":
                    senderEvent = "";
                    myClientForm.myClientEvents.RaiseEvent(Remwave.Client.Events.ClientEvent.IncomingNudge);
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
                    myClientForm.myClientEvents.RaiseEvent(Remwave.Client.Events.ClientEvent.IncomingInstantMessage);
                    break;
            }
            //if chat session exist display content in conversation window
            if (tmplChatSession != null)
            {
                if (tmplChatSession.ChatTabConversation != null)
                {
                    IMMessage message = new IMMessage(senderEvent, messageText, messageGUID, messageDateTime, style, template, myEmoticons);
                    tmplChatSession.ChatTabConversation.Document.Write(message.HTML);
                    tmplChatSession.ChatTabConversation.Document.Window.ScrollTo(0, tmplChatSession.ChatTabConversation.Document.Body.ScrollRectangle.Height);
                }
            }
        }

        public delegate void AddToConversationDelegate(String chatJID, String senderJID, string messageText, string messageHTML, string messageDateTime, string messageGUID, bool forceEmpty, bool flashWindow);
        public void AddToConversation(String chatJID, String senderJID, string messageText, string messageHTML, string messageDateTime, string messageGUID, bool forceEmpty, bool flashWindow)
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
            if (forceEmpty)
            {
                tmplChatSession.ChatTabConversation.Document.OpenNew(true);
            }

            if (tmplChatSession == null)
            {
                try
                {
                    ContactList contactList = myClientForm.mContactBook.getCandidatesForJabberID(chatJID);
                    if (contactList.Count > 0)
                    {
                        NTContact ntContact = (NTContact)contactList[0];
                        chatJabberUser = new JabberUser(ntContact.NTJabberID, ntContact.NTUsername, ntContact.NTNickname);
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
                    myClientForm.myClientEvents.RaiseEvent(Remwave.Client.Events.ClientEvent.IncomingInstantMessage);
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
                if (tmplChatSession.ChatTabConversation != null)
                {
                    SetComposing(false, senderJID);
                    if (messageHTML == "")
                    {
                        messageHTML = messageText;
                    }
                        
                       
                    IMMessage message = new IMMessage(senderJabberUser.Nick, messageHTML, messageGUID, messageDateTime, style, template, myEmoticons);
                    tmplChatSession.ChatTabConversation.Document.Write(message.HTML);
                    tmplChatSession.ChatTabConversation.Document.Window.ScrollTo(0, tmplChatSession.ChatTabConversation.Document.Body.ScrollRectangle.Height);
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
                    tmplChatSession.ChatTab.ImageIndex = 7;
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
                if (!e.Control)
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
                string jabberXevent = "<x xmlns='jabber:x:event'><composing/><id>" + DateTime.Now.ToString("hh:mm:ss.fffffff") + "</id></x>";
                bool msgSent = myClientForm.SendJabberXevent(tmplChatSession.JabberUser.JID, jabberXevent);
            }
        }

        private void SendMessage()
        {
            ChatSession tmplChatSession = (ChatSession)myChatSessions[myChatSessionsTabControl.SelectedTab.Tag];
            if (tmplChatSession != null)
            {

                IMMessage messageOutgoing = new IMMessage(tmplChatSession.JabberUser.Nick, tmplChatSession.ChatTabMessage.Text, Guid.NewGuid().ToString(), "", tmplChatSession.OutgoingStyle, MessageTemplateType.Outgoing, null);

                if (!myClientForm.SendMessage(tmplChatSession.JabberUser, messageOutgoing.Text, messageOutgoing.HTML)) return;

                AddToConversation(tmplChatSession.JabberUser.JID, myClientForm.mUserAccount.JabberUser.JID, tmplChatSession.ChatTabMessage.Text, "", "", Guid.NewGuid().ToString(), false, false);
                tmplChatSession.ChatTabMessage.Text = "";
            }
        }

        private void Search(string search)
        {
            ChatSession tmplChatSession = (ChatSession)myChatSessions[myChatSessionsTabControl.SelectedTab.Tag];
            if (tmplChatSession != null)
            {
                tmplChatSession.ChatTabConversation.Document.OpenNew(true);
                ChatController.MessageTemplate tmplMessageTemplate = new MessageTemplate(MessageTemplateType.Notification);
                tmplChatSession.ChatTabConversation.Document.Write(tmplMessageTemplate.Message);
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
                tmplChatSession.ChatTabMessage.Focus();
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
                    tmplChatSession.ChatTabMessage.Focus();
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
                JabberUser jabberUser =  new JabberUser(jid);
                IMMessage messageIncoming = new IMMessage(jabberUser.Nick, messageText, Guid.NewGuid().ToString(), "", new MessageStyle(), MessageTemplateType.Incoming, null);
                AddToConversation(jid, jid, messageIncoming.Text, messageIncoming.HTML, messageIncoming.Time, messageIncoming.ID, false, true);
                myClientForm.myStorage.AddMessageToArchive(jid, myClientForm.myStorage.StorageGUID(), messageIncoming.Text, messageIncoming.HTML, false);
                SetComposing(false, jid);
            
#if(TRACE)
            Console.WriteLine("IM-Message:" + "(" + jid + ")" + messageIncoming.Text);
#endif
        }
    }
    }
}
