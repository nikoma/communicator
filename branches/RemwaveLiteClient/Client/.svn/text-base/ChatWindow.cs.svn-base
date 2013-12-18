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

namespace Remwave.Client
{

    public partial class ChatWindow : ShapedForm
    {
        private ClientForm myClientForm;
        private ChatSessions myChatSessions = new ChatSessions();
        private Emoticons myEmoticons = new Emoticons();

        public String[] PRESENCES = { "Offline", "Online", "Away", "Extended Away", "Do Not Disturb" };

        public ChatWindow(ClientForm clientForm)
        {
            InitializeComponent();
            myClientForm = clientForm;
            myEmoticons.LoadDefault(myMessageMenuImagesList);

            this.ClientSize = this.Size;
        }

        private string BuildStyle(Font font, Color fontColor, Color bgColor)
        {
            string style = "<FONT>color: <COLOR>;background-color:<BGCOLOR>";

            string fontStyle = "font-family: " + font.Name + ",Verdana ;"
                + "font-size: " + font.SizeInPoints.ToString() + " pt;"
                + "font-weight: " + (font.Bold ? "bold" : "normal") + ";"
                + "font-style: " + (font.Italic ? "italic" : "normal") + ";"
                + "text-decoration: " + (font.Underline ? "underline" : font.Strikeout ? "line-through" : "none") + ";";

            //apply font
            style = style.Replace("<FONT>", fontStyle)
            .Replace("<COLOR>", "#" + fontColor.R.ToString("X2") + fontColor.G.ToString("X2") + fontColor.B.ToString("X2"))
            .Replace("<BGCOLOR>", "#" + bgColor.R.ToString("X2") + bgColor.G.ToString("X2") + bgColor.B.ToString("X2"));



            return style;
        }

        private string ProcessEmoticons(string messageText)
        {
            //handle text processing for emoticons replacement:
            MessageTemplate myMessageTemplate = new MessageTemplate();


            if (messageText != null)
            {

                foreach (Emoticon myEmoticon in myEmoticons.List)
                {
                    messageText = messageText
                        //upper case
                         .Replace(myEmoticon.Tag.ToUpper(), myMessageTemplate.Emoticon
                         .Replace("<FILENAME>", Directory.GetCurrentDirectory() + "\\Emoticons\\" + myEmoticon.Filename)
                         .Replace("<WIDTH>", myEmoticon.Width.ToString())
                         .Replace("<HEIGHT>", myEmoticon.Height.ToString()))
                        //lower case
                         .Replace(myEmoticon.Tag.ToLower(), myMessageTemplate.Emoticon
                         .Replace("<FILENAME>", Directory.GetCurrentDirectory() + "\\Emoticons\\" + myEmoticon.Filename)
                         .Replace("<WIDTH>", myEmoticon.Width.ToString())
                         .Replace("<HEIGHT>", myEmoticon.Height.ToString()))
                         ;
                }



            }
            return messageText;
        }


        public void NewChat(string jabberID, bool setFocus)
        {

            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ChatWindow));

            if (myChatSessions.List[jabberID] == null)
            {
                ChatSession tmplChatSession = new ChatSession();
                this.tmplChatTab = new Telerik.WinControls.UI.TabItem();
                this.tmplChatTabSplitter = new System.Windows.Forms.SplitContainer();
                this.tmplChatTabConversation = new System.Windows.Forms.WebBrowser();
                this.tmplChatTabMessage = new System.Windows.Forms.TextBox();
                this.tmplChatTab.ContentPanel.SuspendLayout();
                this.tmplChatTabSplitter.Panel1.SuspendLayout();
                this.tmplChatTabSplitter.Panel2.SuspendLayout();
                this.tmplChatTabSplitter.SuspendLayout();
                this.myChatSessionsTabControl.Items.AddRange(new Telerik.WinControls.RadItem[] { this.tmplChatTab });
                //
                // tmplChatTab
                this.tmplChatTab.AccessibleDescription = "";
                this.tmplChatTab.CanFocus = true;
                this.tmplChatTab.Class = "TabItem";
                this.tmplChatTab.Tag = jabberID;
                this.tmplChatTab.ImageIndex = 0;
                this.tmplChatTab.TextImageRelation = TextImageRelation.ImageBeforeText;
                //
                // tmplChatTab.ContentPanel
                //
                this.tmplChatTab.ContentPanel.BackColor = System.Drawing.Color.Transparent;
                this.tmplChatTab.ContentPanel.Controls.Add(this.tmplChatTabSplitter);
                this.tmplChatTab.ContentPanel.Font = new System.Drawing.Font("Trebuchet MS", 9F, System.Drawing.FontStyle.Regular);
                this.tmplChatTab.ContentPanel.Location = new System.Drawing.Point(1, 22);
                this.tmplChatTab.ContentPanel.Name = "ContentPanel";
                this.tmplChatTab.ContentPanel.Size = new System.Drawing.Size(490, 241);
                this.tmplChatTab.ContentPanel.Dock = DockStyle.Fill;
                this.tmplChatTab.ContentPanel.TabIndex = 0;
                this.tmplChatTab.Text = jabberID;
                this.tmplChatTab.ToolTipText = null;
                //
                // tmplChatTabSplitter
                //
                this.tmplChatTabSplitter.Dock = System.Windows.Forms.DockStyle.Fill;
                this.tmplChatTabSplitter.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
                this.tmplChatTabSplitter.Location = new System.Drawing.Point(0, 0);
                this.tmplChatTabSplitter.Name = "tmplChatTabSplitter";
                this.tmplChatTabSplitter.Orientation = System.Windows.Forms.Orientation.Horizontal;
                //
                // tmplChatTabSplitter.Panel1
                //
                this.tmplChatTabSplitter.Panel1.Controls.Add(this.tmplChatTabConversation);
                //
                // tmplChatTabSplitter.Panel2
                //
                this.tmplChatTabSplitter.Panel2.Controls.Add(this.tmplChatTabMessage);
                this.tmplChatTabSplitter.Size = new System.Drawing.Size(459, 264);
                this.tmplChatTabSplitter.SplitterDistance = 153;
                this.tmplChatTabSplitter.TabIndex = 0;
                //
                // tmplChatTabConversation
                //
                this.tmplChatTabConversation.AllowNavigation = false;
                this.tmplChatTabConversation.AllowWebBrowserDrop = false;
                this.tmplChatTabConversation.Dock = System.Windows.Forms.DockStyle.Fill;
                this.tmplChatTabConversation.IsWebBrowserContextMenuEnabled = false;
                this.tmplChatTabConversation.Location = new System.Drawing.Point(0, 0);
                this.tmplChatTabConversation.MinimumSize = new System.Drawing.Size(20, 20);
                this.tmplChatTabConversation.Name = "tmplChatTabConversation";
                this.tmplChatTabConversation.ScriptErrorsSuppressed = true;
                this.tmplChatTabConversation.Size = new System.Drawing.Size(459, 153);
                this.tmplChatTabConversation.TabIndex = 0;
                this.tmplChatTabConversation.WebBrowserShortcutsEnabled = false;
                this.tmplChatTabConversation.DocumentText = "<HTML><BODY></BODY></HTML>";

                //ToolStrip Buttons
                this.tmplChatWindowToolStrip = new System.Windows.Forms.ToolStrip();
                this.tmplChangeFontToolStripButton = new System.Windows.Forms.ToolStripButton();
                this.tmplChangeColorToolStripButton = new System.Windows.Forms.ToolStripButton();
                this.tmplEmoticonSeparatorToolStripButton = new System.Windows.Forms.ToolStripSeparator();
                this.tmplEmoticonSmileToolStripButton = new System.Windows.Forms.ToolStripButton();
                this.tmplEmoticonGrinToolStripButton = new System.Windows.Forms.ToolStripButton();
                this.tmplEmoticonEvilGrinToolStripButton = new System.Windows.Forms.ToolStripButton();
                this.tmplEmoticonWinkToolStripButton = new System.Windows.Forms.ToolStripButton();
                this.tmplEmoticonHappyToolStripButton = new System.Windows.Forms.ToolStripButton();
                this.tmplEmoticonSurprisedToolStripButton = new System.Windows.Forms.ToolStripButton();
                this.tmplEmoticonToungeToolStripButton = new System.Windows.Forms.ToolStripButton();
                this.tmplEmoticonUnhappyToolStripButton = new System.Windows.Forms.ToolStripButton();
                this.tmplEmoticonWaiiToolStripButton = new System.Windows.Forms.ToolStripButton();
                this.tmplToolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
                this.tmplSendMessageToolStripButton = new System.Windows.Forms.ToolStripButton();

                this.tmplVideoCallToolStripButton = new System.Windows.Forms.ToolStripButton();
                this.tmplVoiceCallToolStripButton = new System.Windows.Forms.ToolStripButton();





                this.tmplChatWindowToolStrip.SuspendLayout();

                this.tmplChatTabSplitter.Panel2.Controls.Add(this.tmplChatWindowToolStrip);

                // 
                // tmplChatWindowToolStrip
                // 
                this.tmplChatWindowToolStrip.AllowItemReorder = true;
                this.tmplChatWindowToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
                this.tmplChatWindowToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tmplChangeFontToolStripButton,
            this.tmplChangeColorToolStripButton,
            this.tmplEmoticonSeparatorToolStripButton,
            this.tmplEmoticonSmileToolStripButton,
            this.tmplEmoticonGrinToolStripButton,
            this.tmplEmoticonEvilGrinToolStripButton,
            this.tmplEmoticonWinkToolStripButton,
            this.tmplEmoticonHappyToolStripButton,
            this.tmplEmoticonSurprisedToolStripButton,
            this.tmplEmoticonToungeToolStripButton,
            this.tmplEmoticonUnhappyToolStripButton,
            this.tmplEmoticonWaiiToolStripButton,
                       this.tmplVideoCallToolStripButton,
                this.tmplVoiceCallToolStripButton,
                 this.tmplSendMessageToolStripButton,
                 this.tmplToolStripSeparator
                 });
                this.tmplChatWindowToolStrip.Location = new System.Drawing.Point(0, 0);
                this.tmplChatWindowToolStrip.Name = "tmplChatWindowToolStrip";
                this.tmplChatWindowToolStrip.Size = new System.Drawing.Size(490, 25);
                this.tmplChatWindowToolStrip.TabIndex = 0;
                this.tmplChatWindowToolStrip.Text = "";
                // 
                // tmplChangeFontToolStripButton
                // 
                this.tmplChangeFontToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
                this.tmplChangeFontToolStripButton.Image = myMessageMenuImagesList.Images[0];
                this.tmplChangeFontToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
                this.tmplChangeFontToolStripButton.Name = "tmplChangeFontToolStripButton";
                this.tmplChangeFontToolStripButton.Size = new System.Drawing.Size(23, 22);
                this.tmplChangeFontToolStripButton.Text = "";
                this.tmplChangeFontToolStripButton.Click += new EventHandler(tmplChangeFontToolStripButton_Click);
                // 
                // tmplChangeColorToolStripButton
                // 
                this.tmplChangeColorToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
                this.tmplChangeColorToolStripButton.Image = myMessageMenuImagesList.Images[1];
                this.tmplChangeColorToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
                this.tmplChangeColorToolStripButton.Name = "tmplChangeColorToolStripButton";
                this.tmplChangeColorToolStripButton.Size = new System.Drawing.Size(23, 22);
                this.tmplChangeColorToolStripButton.Text = "";
                this.tmplChangeColorToolStripButton.Click += new EventHandler(tmplChangeColorToolStripButton_Click);
                // 
                // tmplEmoticonSeparatorToolStripButton
                // 
                this.tmplEmoticonSeparatorToolStripButton.Name = "tmplEmoticonSeparatorToolStripButton";
                this.tmplEmoticonSeparatorToolStripButton.Size = new System.Drawing.Size(6, 25);
                // 
                // tmplEmoticonSmileToolStripButton
                // 
                this.tmplEmoticonSmileToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
                this.tmplEmoticonSmileToolStripButton.Image = myMessageMenuImagesList.Images[2];
                this.tmplEmoticonSmileToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
                this.tmplEmoticonSmileToolStripButton.Name = "tmplEmoticonSmileToolStripButton";
                this.tmplEmoticonSmileToolStripButton.Size = new System.Drawing.Size(23, 22);
                this.tmplEmoticonSmileToolStripButton.ToolTipText = "smile";
                this.tmplEmoticonSmileToolStripButton.Click += new EventHandler(tmplEmoticonSmileToolStripButton_Click);
                // 
                // tmplEmoticonEvilGrinToolStripButton
                // 
                this.tmplEmoticonEvilGrinToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
                this.tmplEmoticonEvilGrinToolStripButton.Image = myMessageMenuImagesList.Images[4];
                this.tmplEmoticonEvilGrinToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
                this.tmplEmoticonEvilGrinToolStripButton.Name = "tmplEmoticonEvilGrinToolStripButton";
                this.tmplEmoticonEvilGrinToolStripButton.Size = new System.Drawing.Size(23, 22);
                this.tmplEmoticonEvilGrinToolStripButton.ToolTipText = "evilgrin";
                this.tmplEmoticonEvilGrinToolStripButton.Click += new EventHandler(tmplEmoticonEvilGrinToolStripButton_Click);
                // 
                // tmplEmoticonGrinToolStripButton
                // 
                this.tmplEmoticonGrinToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
                this.tmplEmoticonGrinToolStripButton.Image = myMessageMenuImagesList.Images[3];
                this.tmplEmoticonGrinToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
                this.tmplEmoticonGrinToolStripButton.Name = "tmplEmoticonGrinToolStripButton";
                this.tmplEmoticonGrinToolStripButton.Size = new System.Drawing.Size(23, 22);
                this.tmplEmoticonGrinToolStripButton.ToolTipText = "grin";
                this.tmplEmoticonGrinToolStripButton.Click += new EventHandler(tmplEmoticonGrinToolStripButton_Click);
                // 
                // tmplEmoticonHappyToolStripButton
                // 
                this.tmplEmoticonHappyToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
                this.tmplEmoticonHappyToolStripButton.Image = myMessageMenuImagesList.Images[6];
                this.tmplEmoticonHappyToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
                this.tmplEmoticonHappyToolStripButton.Name = "tmplEmoticonHappyToolStripButton";
                this.tmplEmoticonHappyToolStripButton.Size = new System.Drawing.Size(23, 22);
                this.tmplEmoticonHappyToolStripButton.ToolTipText = "happy";
                this.tmplEmoticonHappyToolStripButton.Click += new EventHandler(tmplEmoticonHappyToolStripButton_Click);
                // 
                // tmplEmoticonSurprisedToolStripButton
                // 
                this.tmplEmoticonSurprisedToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
                this.tmplEmoticonSurprisedToolStripButton.Image = myMessageMenuImagesList.Images[7];
                this.tmplEmoticonSurprisedToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
                this.tmplEmoticonSurprisedToolStripButton.Name = "tmplEmoticonSurprisedToolStripButton";
                this.tmplEmoticonSurprisedToolStripButton.Size = new System.Drawing.Size(23, 22);
                this.tmplEmoticonSurprisedToolStripButton.ToolTipText = "surprised";
                this.tmplEmoticonSurprisedToolStripButton.Click += new EventHandler(tmplEmoticonSurprisedToolStripButton_Click);
                // 
                // tmplEmoticonToungeToolStripButton
                // 
                this.tmplEmoticonToungeToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
                this.tmplEmoticonToungeToolStripButton.Image = myMessageMenuImagesList.Images[8];
                this.tmplEmoticonToungeToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
                this.tmplEmoticonToungeToolStripButton.Name = "tmplEmoticonToungeToolStripButton";
                this.tmplEmoticonToungeToolStripButton.Size = new System.Drawing.Size(23, 22);
                this.tmplEmoticonToungeToolStripButton.Text = "tongue";
                this.tmplEmoticonToungeToolStripButton.Click += new EventHandler(tmplEmoticonToungeToolStripButton_Click);
                // 
                // tmplEmoticonUnhappyToolStripButton
                // 
                this.tmplEmoticonUnhappyToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
                this.tmplEmoticonUnhappyToolStripButton.Image = myMessageMenuImagesList.Images[9];
                this.tmplEmoticonUnhappyToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
                this.tmplEmoticonUnhappyToolStripButton.Name = "tmplEmoticonUnhappyToolStripButton";
                this.tmplEmoticonUnhappyToolStripButton.Size = new System.Drawing.Size(23, 22);
                this.tmplEmoticonUnhappyToolStripButton.ToolTipText = "unhappy";
                this.tmplEmoticonUnhappyToolStripButton.Click += new EventHandler(tmplEmoticonUnhappyToolStripButton_Click);
                // 
                // tmplEmoticonWaiiToolStripButton
                // 
                this.tmplEmoticonWaiiToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
                this.tmplEmoticonWaiiToolStripButton.Image = myMessageMenuImagesList.Images[10];
                this.tmplEmoticonWaiiToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
                this.tmplEmoticonWaiiToolStripButton.Name = "tmplEmoticonWaiiToolStripButton";
                this.tmplEmoticonWaiiToolStripButton.Size = new System.Drawing.Size(23, 22);
                this.tmplEmoticonWaiiToolStripButton.ToolTipText = "waii";
                this.tmplEmoticonWaiiToolStripButton.Click += new EventHandler(tmplEmoticonWaiiToolStripButton_Click);
                // 
                // tmplEmoticonWinkToolStripButton
                // 
                this.tmplEmoticonWinkToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
                this.tmplEmoticonWinkToolStripButton.Image = myMessageMenuImagesList.Images[5];
                this.tmplEmoticonWinkToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
                this.tmplEmoticonWinkToolStripButton.Name = "tmplEmoticonWinkToolStripButton";
                this.tmplEmoticonWinkToolStripButton.Size = new System.Drawing.Size(23, 22);
                this.tmplEmoticonWinkToolStripButton.ToolTipText = "wink";
                this.tmplEmoticonWinkToolStripButton.Click += new EventHandler(tmplEmoticonWinkToolStripButton_Click);
                // 
                // tmplToolStripSeparator
                // 
                this.tmplToolStripSeparator.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
                this.tmplToolStripSeparator.Name = "tmplToolStripSeparator";
                this.tmplToolStripSeparator.Size = new System.Drawing.Size(6, 25);
                // 
                // tmplSendMessageToolStripButton
                // 
                this.tmplSendMessageToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.ImageAndText;
                this.tmplSendMessageToolStripButton.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
                this.tmplSendMessageToolStripButton.Image = myMessageMenuImagesList.Images[11];
                this.tmplSendMessageToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
                this.tmplSendMessageToolStripButton.Name = "tmplSendMessageToolStripButton";
                this.tmplSendMessageToolStripButton.Size = new System.Drawing.Size(23, 22);
                this.tmplSendMessageToolStripButton.ToolTipText = "Send message";
                this.tmplSendMessageToolStripButton.Text = "Send";
                this.tmplSendMessageToolStripButton.Click += new EventHandler(tmplSendMessageToolStripButton_Click);
                // 
                // tmplVoiceCallToolStripButton
                // 
                this.tmplVoiceCallToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.ImageAndText;
                this.tmplVoiceCallToolStripButton.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
                this.tmplVoiceCallToolStripButton.Image = myMessageMenuImagesList.Images[12];
                this.tmplVoiceCallToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
                this.tmplVoiceCallToolStripButton.Name = "tmplVoiceCallToolStripButton";
                this.tmplVoiceCallToolStripButton.Size = new System.Drawing.Size(23, 22);
                this.tmplVoiceCallToolStripButton.ToolTipText = "Start a PC to PC call";
                this.tmplVoiceCallToolStripButton.Text = "Call";
                this.tmplVoiceCallToolStripButton.Click += new EventHandler(tmplVoiceCallToolStripButton_Click);
                // 
                // tmplVideoCallToolStripButton
                // 
                this.tmplVideoCallToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.ImageAndText;
                this.tmplVideoCallToolStripButton.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
                this.tmplVideoCallToolStripButton.Image = myMessageMenuImagesList.Images[13];
                this.tmplVideoCallToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
                this.tmplVideoCallToolStripButton.Name = "tmplVideoCallToolStripButton";
                this.tmplVideoCallToolStripButton.Size = new System.Drawing.Size(23, 22);
                this.tmplVideoCallToolStripButton.ToolTipText = "Start video call";
                this.tmplVideoCallToolStripButton.Text = "Video";
                this.tmplVideoCallToolStripButton.Click += new EventHandler(tmplVideoCallToolStripButton_Click);
                //
                // tmplChatTabMessage
                //
                this.tmplChatTabMessage.Dock = System.Windows.Forms.DockStyle.Fill;
                this.tmplChatTabMessage.Location = new System.Drawing.Point(0, 25);
                this.tmplChatTabMessage.Multiline = true;
                this.tmplChatTabMessage.Name = "tmplChatTabMessage";
                this.tmplChatTabMessage.Size = new System.Drawing.Size(354, 82);
                this.tmplChatTabMessage.TabIndex = 0;
                this.tmplChatTabMessage.KeyUp += new KeyEventHandler(tmplChatTabMessage_KeyUp);

                this.tmplChatTab.ContentPanel.ResumeLayout(false);
                this.tmplChatTabSplitter.Panel1.ResumeLayout(false);
                this.tmplChatTabSplitter.Panel2.ResumeLayout(false);
                this.tmplChatTabSplitter.ResumeLayout(false);
                this.tmplChatWindowToolStrip.ResumeLayout(false);
                this.tmplChatWindowToolStrip.PerformLayout();
                this.myChatSessionsTabControl.SelectedTab = this.tmplChatTab;

                RadButtonElement tmplCloseButton = new RadButtonElement("x");
                tmplCloseButton.ToolTipText = "Close Tab";
                tmplCloseButton.Click += new EventHandler(tmplCloseButton_Click);

                tmplCloseButton.Alignment = ContentAlignment.TopRight;
                // Let's say that you want to place the close button in "tabItem1"                
                tmplChatTab.Children[2].Margin = new Padding(0, 0, 15, 0);
                tmplChatTab.Children.Add(tmplCloseButton);

                tmplChatSession.ChatTab = tmplChatTab;
                tmplChatSession.ChatTabConversation = tmplChatTabConversation;
                tmplChatSession.ChatTabMessage = tmplChatTabMessage;
                tmplChatSession.JabberID = jabberID;


                myChatSessions.List.Add(jabberID, tmplChatSession);
            }
            else
            {
                ChatSession tmplChatSession = (ChatSession)myChatSessions.List[jabberID];
                tmplChatSession.ChatTab.Visibility = ElementVisibility.Visible;

            }

            if (myClientForm.myUserAccount.LoggedIn())
            {
                this.Update();
                this.Show();
            }

        }

        void tmplVideoCallToolStripButton_Click(object sender, EventArgs e)
        {
            ChatSession tmplChatSession = (ChatSession)myChatSessions.List[myChatSessionsTabControl.SelectedTab.Tag];
            myClientForm.VideoSessionInvite(tmplChatSession.JabberID);

         
        }

        void tmplVoiceCallToolStripButton_Click(object sender, EventArgs e)
        {
            ChatSession tmplChatSession = (ChatSession)myChatSessions.List[myChatSessionsTabControl.SelectedTab.Tag];
            myClientForm.StartNewCall(-1, tmplChatSession.JabberID);

        }

        void tmplSendMessageToolStripButton_Click(object sender, EventArgs e)
        {
            SendMessage();
        }



        void tmplCloseButton_Click(object sender, EventArgs e)
        {
            RadElement buttonElement = sender as RadElement;
            buttonElement.Parent.Visibility = ElementVisibility.Collapsed;

            bool close = true;
            foreach (TabItem tab in myChatSessionsTabControl.Items)
            {
                if (tab.Visibility == ElementVisibility.Visible) { close = false; }
            }
            if (close) this.Close();
        }

        void tmplEmoticonWinkToolStripButton_Click(object sender, EventArgs e)
        {
            ChatSession tmplChatSession = (ChatSession)myChatSessions.List[myChatSessionsTabControl.SelectedTab.Tag];
            tmplChatSession.ChatTabMessage.Text += " ;-)";

        }

        void tmplEmoticonWaiiToolStripButton_Click(object sender, EventArgs e)
        {
            ChatSession tmplChatSession = (ChatSession)myChatSessions.List[myChatSessionsTabControl.SelectedTab.Tag];
            tmplChatSession.ChatTabMessage.Text += " 8-)";
        }

        void tmplEmoticonUnhappyToolStripButton_Click(object sender, EventArgs e)
        {
            ChatSession tmplChatSession = (ChatSession)myChatSessions.List[myChatSessionsTabControl.SelectedTab.Tag];
            tmplChatSession.ChatTabMessage.Text += " :-(";
        }

        void tmplEmoticonToungeToolStripButton_Click(object sender, EventArgs e)
        {
            ChatSession tmplChatSession = (ChatSession)myChatSessions.List[myChatSessionsTabControl.SelectedTab.Tag];
            tmplChatSession.ChatTabMessage.Text += " :-p";
        }

        void tmplEmoticonSurprisedToolStripButton_Click(object sender, EventArgs e)
        {
            ChatSession tmplChatSession = (ChatSession)myChatSessions.List[myChatSessionsTabControl.SelectedTab.Tag];
            tmplChatSession.ChatTabMessage.Text += " :-o";
        }

        void tmplEmoticonHappyToolStripButton_Click(object sender, EventArgs e)
        {
            ChatSession tmplChatSession = (ChatSession)myChatSessions.List[myChatSessionsTabControl.SelectedTab.Tag];
            tmplChatSession.ChatTabMessage.Text += " XD";
        }

        void tmplEmoticonGrinToolStripButton_Click(object sender, EventArgs e)
        {
            ChatSession tmplChatSession = (ChatSession)myChatSessions.List[myChatSessionsTabControl.SelectedTab.Tag];
            tmplChatSession.ChatTabMessage.Text += " :-D";
        }

        void tmplEmoticonEvilGrinToolStripButton_Click(object sender, EventArgs e)
        {
            ChatSession tmplChatSession = (ChatSession)myChatSessions.List[myChatSessionsTabControl.SelectedTab.Tag];
            tmplChatSession.ChatTabMessage.Text += " B-)";
        }

        void tmplEmoticonSmileToolStripButton_Click(object sender, EventArgs e)
        {
            ChatSession tmplChatSession = (ChatSession)myChatSessions.List[myChatSessionsTabControl.SelectedTab.Tag];
            tmplChatSession.ChatTabMessage.Text += " :-)";
        }

        void tmplChangeColorToolStripButton_Click(object sender, EventArgs e)
        {
            ChatSession tmplChatSession = (ChatSession)myChatSessions.List[myChatSessionsTabControl.SelectedTab.Tag];
            myColorDialog.Color = tmplChatSession.ChatTabMessage.BackColor;
            if (myColorDialog.ShowDialog() != DialogResult.Cancel)
            {
                tmplChatSession.ChatTabMessage.BackColor = myColorDialog.Color;
            }
        }

        void tmplChangeFontToolStripButton_Click(object sender, EventArgs e)
        {
            ChatSession tmplChatSession = (ChatSession)myChatSessions.List[myChatSessionsTabControl.SelectedTab.Tag];
            myFontDialog.ShowColor = true;

            myFontDialog.Font = tmplChatSession.ChatTabMessage.Font;
            myFontDialog.Color = tmplChatSession.ChatTabMessage.ForeColor;

            if (myFontDialog.ShowDialog() != DialogResult.Cancel)
            {
                tmplChatSession.ChatTabMessage.Font = myFontDialog.Font;
                tmplChatSession.ChatTabMessage.ForeColor = myFontDialog.Color;
            }
        }





        public void UpdatePresence(string jabberID, int buddyAvailability)
        {
            ChatSession tmplChatSession = (ChatSession)myChatSessions.List[jabberID];
            if (tmplChatSession != null)
            {
                switch (buddyAvailability)
                {
                    case 0: tmplChatSession.ChatTab.ImageIndex = 0; break;// 'offline
                    case 1: tmplChatSession.ChatTab.ImageIndex = 1; break;// 'online
                    case 2: tmplChatSession.ChatTab.ImageIndex = 2; break;// 'away
                    case 3: tmplChatSession.ChatTab.ImageIndex = 3; break;// 'extended away
                    case 4: tmplChatSession.ChatTab.ImageIndex = 4; break;// 'Do not disturb
                }
            }
        }

        public void AddToConversation(string chatSessionID, string jabberID, string messageText)
        {
            ChatSession tmplChatSession = (ChatSession)myChatSessions.List[chatSessionID];


            //remove whitespaces

            //preset font and bg color
            Color myHeaderColor = Color.Gray;
            Color myFontColor = Color.Black;
            Color myBgColor = Color.White;

            Font myFont = new System.Drawing.Font("Trebuchet MS", 9F, System.Drawing.FontStyle.Regular);


            switch (jabberID)
            {
                case "CLIENT":
                case "SERVER":
                case "INFO":
                case "PRESENCE":
                case "BUDDYUPDATE":
                    jabberID = "System";
                    if (tmplChatSession != null) myFontColor = Color.Gray; myHeaderColor = Color.Gray;
                    break;
                case "INVITE":
                    jabberID = "User";
                    if (tmplChatSession != null) myFontColor = Color.Gray; myHeaderColor = Color.Gray;
                    break;
                default:
                    if (tmplChatSession == null)
                    {
                        try
                        {
                            NewChat(jabberID, false);
                            tmplChatSession = (ChatSession)myChatSessions.List[chatSessionID];
                            if (tmplChatSession == null) return;
                        }
                        catch (Exception)
                        {
                            return;
                        }
                    }
                    if (jabberID == myClientForm.myUserAccount.Username)
                    {
                        //my message
                        myHeaderColor = Color.Blue;
                        myFont = tmplChatSession.ChatTabMessage.Font;
                        myFontColor = tmplChatSession.ChatTabMessage.ForeColor;
                        myBgColor = tmplChatSession.ChatTabMessage.BackColor;
                    }
                    else
                    { //incomming message
                        myHeaderColor = Color.Red;
                    }

                    break;

            }
            //if chat session exist display content in conversation window
            if (tmplChatSession != null)
            {
                //messageText = messageText.Trim().Replace("<", "&lt;").Replace(">", "&rt;").Replace("\"", "&quot;").Replace("&", "&amp;").Replace("\r\n", "<BR />").Replace("\n", "<BR />");
                messageText = messageText.Trim().Replace("\r\n", "<BR />").Replace("\n", "<BR />");
                ChatController.MessageTemplate tmplMessageTemplate = new MessageTemplate();
                //<ROW_STYLE>
                //<HEADER_STYLE>
                //<MESSAGE_STYLE>
                if (tmplChatSession.ChatTabConversation != null)
                {
                    tmplChatSession.ChatTabConversation.Document.Write(ProcessEmoticons(tmplMessageTemplate.DefaultMessage
                        .Replace("<ROW_STYLE>", BuildStyle(myFont, myFontColor, myBgColor))
                        .Replace("<HEADER_STYLE>", BuildStyle(myFont, myHeaderColor, myBgColor))
                        .Replace("<MESSAGE_STYLE>", BuildStyle(myFont, myFontColor, myBgColor))
                        .Replace("<HEADER_TEXT>", jabberID)
                        .Replace("<MESSAGE_TEXT>", messageText))

                    );
                    tmplChatSession.ChatTabConversation.Document.Window.ScrollTo(0, tmplChatSession.ChatTabConversation.Document.Body.ScrollRectangle.Height);
                }
            }
        }

        public void SetComposing(bool turnon, string jabberID)
        {
            //look to see who the message is from and send to the appropriate tab
            ChatSession tmplChatSession = (ChatSession)myChatSessions.List[jabberID];
            if (tmplChatSession != null)
            {
                if (tmplChatSession.ChatTab.ImageIndex == 0) return;
                //otherwise, either mark it as composing or nothing:
                if (turnon)
                {
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

        private void ChatWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }


        private void SendComposingEvent()
        {
            ChatSession tmplChatSession = (ChatSession)myChatSessions.List[myChatSessionsTabControl.SelectedTab.Tag];

            if (tmplChatSession != null)
            {
                //send composing notification
                string jabberXevent = "<x xmlns='jabber:x:event'><composing/><id>" + DateTime.Now.ToString("hh:mm:ss.fffffff") + "</id></x>";
                bool msgSent = myClientForm.SendMessage(tmplChatSession.JabberID, "", jabberXevent);
            }
        }

        private void SendMessage()
        {
            ChatSession tmplChatSession = (ChatSession)myChatSessions.List[myChatSessionsTabControl.SelectedTab.Tag];

            if (tmplChatSession != null)
            {
                //send composing notification
                string jabberXevent = "<x xmlns='jabber:x:event'><composing/><id>" + DateTime.Now.ToString("hh:mm:ss.fffffff")+"</id></x>";
                bool msgSent = myClientForm.SendMessage(tmplChatSession.JabberID, tmplChatSession.ChatTabMessage.Text, jabberXevent);
                if (msgSent)
                {
                    AddToConversation(tmplChatSession.JabberID, myClientForm.myUserAccount.Username, tmplChatSession.ChatTabMessage.Text);
                    tmplChatSession.ChatTabMessage.Text = "";
                }
            }
        }

    }
}