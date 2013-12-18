using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Remwave.ChatController;
using System.IO;
using Remwave.Services;

namespace Remwave.Client
{
    public partial class ArchiveWindow : Form
    {
        private Storage mStorage;
        bool mLaunchSearch = false;
        Int32 mSearchDelay = 0;
        Int32 mMinimumSearchDelay = 1;
        public ArchiveWindow()
        {
            InitializeComponent();
        }

        public void Open(Storage storage, JabberUser selectedJabberUser)
        {
            this.mStorage = storage;
            wbConversation.DocumentText = "<HTML><BODY></BODY></HTML>";
            loadArchiveUsersList(selectedJabberUser.JID);
            tbxSearchText.Focus();
        }

        public void reloadArchive()
        {

        }

        public delegate void loadArchiveSelectedUserDelegate(string selectedUser, string search);
        public void loadArchiveSelectedUser(string selectedUser, string search)
        {
            if (this.InvokeRequired)
            {
                BeginInvoke(new loadArchiveSelectedUserDelegate(loadArchiveSelectedUser), new object[] { selectedUser, search });
                return;
            }
            wbConversation.Document.OpenNew(true);

            Emoticons myEmoticons = new Emoticons(Directory.GetCurrentDirectory() + "\\Emoticons\\");

            List<Remwave.Client.Storage.StorageMessage> list = mStorage.GetMessageFromArchive(selectedUser, search, 2048);
            ChatController.MessageTemplate tmplMessageTemplate = new MessageTemplate(MessageTemplateType.Notification);
            wbConversation.Document.Write(tmplMessageTemplate.Message);
            for (int i = list.Count - 1; i >= 0; i--)
            {
                JabberUser jabberUser = null;

                MessageStyle incomingStyle = new MessageStyle(Color.White, new System.Drawing.Font("Trebuchet MS", 8.5F, System.Drawing.FontStyle.Regular), Color.Black, Color.Red);
                MessageStyle outgoingStyle = new MessageStyle(Color.White, new System.Drawing.Font("Trebuchet MS", 8.5F, System.Drawing.FontStyle.Regular), Color.Black, Color.Blue);
                MessageTemplateType template = MessageTemplateType.Notification;
                MessageStyle style = new MessageStyle(Color.White, new System.Drawing.Font("Trebuchet MS", 8.5F, System.Drawing.FontStyle.Regular), Color.Gray, Color.Gray);

                String messageHTML = "";


                if (list[i].ContentHTML != "")
                {

                    messageHTML = list[i].ContentHTML;
                }
                else
                {
                    messageHTML = list[i].ContentText;
                }





                //Compatibility with legacy text only messages
                if (list[i].Direction == StorageItemDirection.In)
                {
                    style = incomingStyle;
                    template = MessageTemplateType.In;
                    jabberUser = new JabberUser(list[i].JID);
                }
                else
                {
                    style = outgoingStyle;
                    template = MessageTemplateType.Out;
                    jabberUser = new JabberUser(mStorage.Username);
                }
                IMMessage message = new IMMessage(jabberUser.Nick, messageHTML, list[i].GUID, list[i].Created, style, template, myEmoticons);
                wbConversation.Document.Write(message.HTML);
            }
        }

        public void loadArchiveUsersList(String selectedJID)
        {
            List<Storage.StorageMessageArchiveUsers> list = mStorage.GetMessageArchiveUsers();
            tvUserList.Nodes.Clear();

            foreach (Storage.StorageMessageArchiveUsers item in list)
            {
                JabberUser jabberUser = new JabberUser(item.JID);

                TreeNode node = new TreeNode(jabberUser.Nick);
                node.Tag = jabberUser.JID;
                node.ImageIndex = 0;
                tvUserList.Nodes.Add(node);
                if (selectedJID != null
                     && selectedJID == item.JID
                     && tvUserList.SelectedNode != null
                     && tvUserList.SelectedNode.Tag.ToString() != selectedJID)
                {
                    tvUserList.SelectedNode = node;
                }
            }
            if (tvUserList.Nodes.Count == 0)
            {
                tvUserList.Nodes.Clear();
                TreeNode node = new TreeNode(Properties.Localization.txtArchiveInfoNoHistoryFound);
                tvUserList.Nodes.Add(node);
            }
        }

        private void tvUserList_AfterSelect(object sender, TreeViewEventArgs e)
        {
            loadArchiveSelectedUser(e.Node.Tag.ToString(), "");
        }

        private void tbxSearchText_TextChanged(object sender, EventArgs e)
        {
            if (tvUserList.SelectedNode != null)
            {
                mLaunchSearch = true;
                mSearchDelay = 0;
            }
            tbxSearchText.Focus();
        }

        private void btnSearchCancel_Click(object sender, EventArgs e)
        {
            tbxSearchText.Text = "";
        }

        private void timerSearchLauncher_Tick(object sender, EventArgs e)
        {
            if (mLaunchSearch)
            {
                mSearchDelay++;
                if (mSearchDelay > mMinimumSearchDelay)
                {
                    mLaunchSearch = false;
                    mSearchDelay = 0;
                    loadArchiveSelectedUser(tvUserList.SelectedNode.Tag.ToString(), tbxSearchText.Text.Trim());
                }
            }
        }
    }
}
