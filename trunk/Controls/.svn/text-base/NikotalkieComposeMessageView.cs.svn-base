using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Remwave.Nikotalkie;

namespace Remwave.Client.Controls
{
    public partial class NikotalkieComposeMessageView : UserControl
    {

        #region Public Properties
        private List<String> mRecipients;
        public List<String> Recipients
        {
            get
            {
                mRecipients = new List<String>();
                String[] recipients = tbDestination.Text.Split(';');
                foreach (String recipient in recipients)
                {
                    mRecipients.Add(recipient);
                }
                return mRecipients;
            }
        }
        private Nikotalkie.NMessageAttachementHeader mAttachementHeader;

        public Nikotalkie.NMessageAttachementHeader AttachementHeader
        {
            get { return mAttachementHeader; }
        }

        #endregion



        public event EventHandler TalkClicked;
        internal void OnTalkClicked(object sender, EventArgs args)
        {
            if (TalkClicked != null)
            {
                TalkClicked(sender, args);
            }
        }

        public event EventHandler SendMessageClicked;
        internal void OnSendMessageClicked(object sender, EventArgs args)
        {
            if (SendMessageClicked != null)
            {
                SendMessageClicked(sender, args);
            }
        }

        public NikotalkieComposeMessageView()
        {
            this.Dock = DockStyle.Fill;
            InitializeComponent();
        }

        public void NewMessage()
        {
            mAttachementHeader = new NMessageAttachementHeader();
            buttonTalk.Visible = true;
            buttonTalk.Enabled = true;
        }

        private void buttonTalk_Click(object sender, EventArgs e)
        {
            tbDestination.Text = tbDestination.Text.Trim();
            if (tbDestination.Text.Length == 0)
            {
                MessageBox.Show("Please enter a destination email address or nikotalkie username  into \"Destination\" box", "Invalid destination", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                return;
            }
            OnTalkClicked(sender, e);
        }

        private void buttonSend_Click(object sender, EventArgs e)
        {
            OnSendMessageClicked(sender, e);
            if (!liDestinationHistory.Items.Contains(tbDestination.Text))
            {
                liDestinationHistory.Items.Add(tbDestination.Text);
            }
        }

        #region Button Animation Events

        private void buttonTalk_MouseEnter(object sender, EventArgs e)
        {
            this.buttonTalk.Image = Properties.Resources.NikotalkieButtonTalkOver;
        }

        private void buttonTalk_MouseLeave(object sender, EventArgs e)
        {
            this.buttonTalk.Image = Properties.Resources.NikotalkieButtonTalk;
        }

        private void buttonTalk_MouseDown(object sender, MouseEventArgs e)
        {
            this.buttonTalk.Image = Properties.Resources.NikotalkieButtonTalkDown;
        }

        private void buttonTalk_MouseUp(object sender, MouseEventArgs e)
        {
            this.buttonTalk.Image = Properties.Resources.NikotalkieButtonTalk;
        }

        #endregion

        internal void SetRecipient(string recipient)
        {
            tbDestination.Text = recipient;
        }
    }
}
