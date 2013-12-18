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
using System.Collections;

namespace Remwave.Client
{
    public partial class ArchiveWindow : Form
    {
        private Storage mStorage;
        private bool mLaunchSearch = false;
        private Int32 mSearchDelay = 0;
        private Int32 mMinimumSearchDelay = 1;
        private ColumnHeader mColumnHeader;
        private void BrandComponent()
        {
            this.Icon = Properties.Resources.desktop;
        }

        public ArchiveWindow()
        {
            InitializeComponent();
            BrandComponent();
            mColumnHeader = new ColumnHeader();
            mColumnHeader.Text = "Date";
        }

        private void ArchiveWindow_Load(object sender, EventArgs e)
        {
            //initialize listview

            listView1.View = View.Details;
            listView1.Sorting = SortOrder.None;
            listView1.HeaderStyle = ColumnHeaderStyle.None;
            listView1.FullRowSelect = true;


           

            listView1.Columns.AddRange(new ColumnHeader[] { mColumnHeader });
        }

        public void Open(Storage storage, JabberUser selectedJabberUser)
        {
            this.mStorage = storage;
            ResetHTML();
            loadArchiveUsersList(selectedJabberUser);
            tbxSearchText.Focus();
        }

        internal void ResetHTML()
        {
            string defaultHtml = "<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\"     \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\"> <html xmlns=\"http://www.w3.org/1999/xhtml\"> <head> <title></title> </head> <body> <p></p> </body> </html>";
            if (wbConversation.Document == null) wbConversation.Navigate("about:blank");
            wbConversation.Document.OpenNew(true);
            wbConversation.Document.Write(defaultHtml);
        }

        public void reloadArchive()
        {

        }

        public delegate void loadArchiveSelectedUserDateDelegate(string selectedUser, string search, string date);
        public void loadArchiveSelectedUserDate(string selectedUser, string search, string date)
        {
            String HTMLResult = "<p></p>";
            if (this.InvokeRequired)
            {
                BeginInvoke(new loadArchiveSelectedUserDateDelegate(loadArchiveSelectedUserDate), new object[] { selectedUser, search });
                return;
            }
            ResetHTML();

            Emoticons myEmoticons = new Emoticons(Directory.GetCurrentDirectory() + "\\Emoticons\\");

            List<Remwave.Client.Storage.StorageMessage> list = mStorage.GetMessageFromArchiveByDate(selectedUser, search, 2048,date);
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
                HTMLResult += message.HTML;
            }
            wbConversation.Document.Body.InnerHtml += HTMLResult;
        }

        public delegate void loadArchiveDatesSelectedUserDelegate(string selectedUser, string search);
        public void loadArchiveDatesSelectedUser(string selectedUser, string search)
        {
            if (this.InvokeRequired)
            {
                BeginInvoke(new loadArchiveDatesSelectedUserDelegate(loadArchiveDatesSelectedUser), new object[] { selectedUser, search });
                return;
            }
            //create items


            listView1.Items.Clear();
            List<StorageMessageDate> storageMessageDateList = mStorage.GetMessageDatesFromArchive(selectedUser, search, 180);

            foreach (StorageMessageDate date in storageMessageDateList)
            {

                String dateFormatted = date.Date.ToShortDateString() + ", " + date.Date.DayOfWeek.ToString();
                

                if (date.DaysAway < 7)
                {
                    listView1.Items.Add(new ListViewItem(new string[] { dateFormatted, "This Week", date.Day }));
                }
                else
                    if (date.DaysAway < 14)
                    {
                        listView1.Items.Add(new ListViewItem(new string[] { dateFormatted, "Last Week", date.Day }));
                    }
                    else
                        if (date.DaysAway < 21)
                        {
                            listView1.Items.Add(new ListViewItem(new string[] { dateFormatted, "Two Weeks Ago", date.Day }));
                        }
                        else
                            if (date.DaysAway < 28)
                            {
                                listView1.Items.Add(new ListViewItem(new string[] { dateFormatted, "Three Weeks Ago", date.Day }));
                            }
                        else
                            if (date.DaysAway < 59)
                            {
                                listView1.Items.Add(new ListViewItem(new string[] { dateFormatted, "Last Month", date.Day }));
                            }
                            else
                            {
                                listView1.Items.Add(new ListViewItem(new string[] { dateFormatted, "Older", date.Day }));
                            }
            }

            listViewSetGroups(1, false);
            mColumnHeader.Width = -1;
        }

        private void listViewSetGroups(int column, bool letters)
        {
            Hashtable groups = new Hashtable();
            listView1.Groups.Clear();
            foreach (ListViewItem item in listView1.Items)
            {
                String subItemText = item.SubItems[column].Text;
                if (letters)
                {
                    subItemText = subItemText.Substring(0, 1);
                }

                if (!groups.ContainsKey(subItemText))
                {
                    ListViewGroup listGroup = new ListViewGroup(subItemText, HorizontalAlignment.Left);
                    groups.Add(subItemText, listGroup);
                    listView1.Groups.Add(listGroup);
                }

                item.Group = (ListViewGroup)groups[subItemText];
            }
            if (listView1.Items.Count > 0) listView1.Items[0].Selected = true;
            
        }


        public void loadArchiveUsersList(JabberUser selectedJabberUser)
        {
            List<Storage.StorageMessageArchiveUsers> list = mStorage.GetMessageArchiveUsers();
            tvUserList.Nodes.Clear();

            foreach (Storage.StorageMessageArchiveUsers item in list)
            {
                JabberUser jabberUser = new JabberUser(item.JID);

                TreeNode node = new TreeNode(jabberUser.Nick);
                node.Tag = jabberUser;
                node.ImageIndex = 0;
                tvUserList.Nodes.Add(node);
                if (selectedJabberUser != null && selectedJabberUser.JID == item.JID)
                {
                    if (tvUserList.SelectedNode != null && tvUserList.SelectedNode.Tag.ToString() == selectedJabberUser.JID)
                        continue;
                    tvUserList.SelectedNode = node;
                }
            }
            if (tvUserList.Nodes.Count == 0)
            {
                tvUserList.Nodes.Clear();
                TreeNode node = new TreeNode(Properties.Localization.txtArchiveInfoNoHistoryFound);
                tvUserList.Nodes.Add(node);
            }
            if (tvUserList.SelectedNode == null) tvUserList.SelectedNode = tvUserList.Nodes[0];

        }





        private void tvUserList_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node != null && e.Node.Tag != null)
            {
                loadArchiveDatesSelectedUser(e.Node.Tag.ToString(), "");
                label1.Text = e.Node.Text;
            }
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
                    loadArchiveDatesSelectedUser(tvUserList.SelectedNode.Tag.ToString(), tbxSearchText.Text.Trim());
                }
            }
        }

        private void tvUserList_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                if ((tvUserList.SelectedNode != null) && (tvUserList.SelectedNode.Tag != null))
                {
                    mStorage.DeleteUserHistory(tvUserList.SelectedNode.Tag.ToString());
                    if ((tvUserList.SelectedNode.PrevNode != null) && (tvUserList.SelectedNode.PrevNode.Tag != null))
                    {
                        loadArchiveUsersList((JabberUser)tvUserList.SelectedNode.PrevNode.Tag);
                    }
                    else if ((tvUserList.SelectedNode.NextNode != null) && (tvUserList.SelectedNode.NextNode.Tag != null))
                    {
                        loadArchiveUsersList((JabberUser)tvUserList.SelectedNode.NextNode.Tag);
                    }
                    else
                    {
                        loadArchiveUsersList(null);
                    }
                }
            }
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (tvUserList.SelectedNode != null && tvUserList.SelectedNode.Tag != null)
            {
               if (listView1.SelectedItems.Count > 0)
            {
                loadArchiveSelectedUserDate(tvUserList.SelectedNode.Tag.ToString(), tbxSearchText.Text, listView1.SelectedItems[0].SubItems[2].Text);
            }

            }
            
        }



    }
}