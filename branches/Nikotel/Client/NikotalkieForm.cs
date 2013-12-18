using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Remwave.Client
{
    public partial class NikotalkieForm : Form
    {
        ClientForm mOwner;

        public NikotalkieForm()
        {
            InitializeComponent();
        }

        public NikotalkieForm(ClientForm owner )
        {
            mOwner = owner;
            InitializeComponent();
            nikotalkieControl.ShowControl += new EventHandler(nikotalkieControl_ShowControl);
            nikotalkieControl.HideControl += new EventHandler(nikotalkieControl_HideControl);
            nikotalkieControl.IncomingMessage += new EventHandler(nikotalkieControl_IncomingMessage);
            nikotalkieControl.AcceptButtonChanged += new EventHandler(nikotalkieControl_AcceptButtonChanged);
        }

        void nikotalkieControl_AcceptButtonChanged(object sender, EventArgs e)
        {
            /*
            if (sender == null)
            {
                this.AcceptButton = null;
            }
            else
            {
                Button button = (Button)sender;
                this.AcceptButton = button;
            }
             * */
        }

        void nikotalkieControl_IncomingMessage(object sender, EventArgs e)
        {
            if (mOwner == null) return;
            mOwner.myClientEvents.RaiseEvent(Remwave.Client.Events.ClientEvent.IncomingInstantMessage);
        }

        void nikotalkieControl_HideControl(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        void nikotalkieControl_ShowControl(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
            this.Show();
            this.Activate();
        }

        private void NikotalkieForm_Load(object sender, EventArgs e)
        {
            this.ClientSize = new Size(320, 320);
        }

        private void NikotalkieForm_Shown(object sender, EventArgs e)
        {
            this.MinimumSize = this.RestoreBounds.Size;
        }

        private void NikotalkieForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.WindowState = FormWindowState.Minimized;
        }


        internal void StartComposing(String recipient)
        {
            this.WindowState = FormWindowState.Normal;
            this.Show();
            this.Activate();

            nikotalkieControl.MessageReply(recipient);
        }

        
    }
}