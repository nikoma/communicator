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
    public partial class NikotalkieItem : UserControl
    {
        public NikotalkieItem()
        {
            this.Dock = DockStyle.Top;
            InitializeComponent();
        }
        #region Public Events
        public event EventHandler ItemActionPlay;
        internal void OnItemActionPlay(object sender, EventArgs args)
        {
            if (ItemActionPlay != null)
            {
                ItemActionPlay(sender, args);
            }
        }

        public event EventHandler ItemSelected;
        internal void OnItemSelected(object sender, EventArgs args)
        {
            if (ItemSelected != null)
            {
                ItemSelected(sender, args);
            }
        }
        #endregion
        
        public NikotalkieItem(NMessage message)
        {
            this.Dock = DockStyle.Top;
            InitializeComponent();
            this.SetItem(message);
        }

        public void SetItem(NMessage message)
        {
            this.LabelFrom.Text = message.Header.From;
            if (DateTime.Now.ToShortDateString() == message.Header.Date.ToShortDateString())
            {//today
                this.LabelDate.Text = "Today, " + message.Header.Date.ToShortTimeString();
            }
            else if (DateTime.Now.AddDays(-7) < message.Header.Date)
            {//day of week
                this.LabelDate.Text = message.Header.Date.DayOfWeek.ToString() + ", " + message.Header.Date.ToShortTimeString();
            }
            else
            {
                this.LabelDate.Text = message.Header.Date.ToShortDateString();
            }
            
            this.Tag = message;
            this.Visible = true;   
        }

        public void UnSetItem()
        {
            this.Visible = false;
            this.LabelFrom.Text = "";
            this.LabelDate.Text = "";
            this.Tag = null;
        }

        public void SelectItem()
        {
            this.BackgroundImage = Properties.Resources.NikotalkieItemBackgroundBlackSelected;
        }

        public void UnSelectItem()
        {
            this.BackgroundImage = Properties.Resources.NikotalkieItemBackgroundBlack;
        }

        private void NikotalkieItem_Click(object sender, EventArgs e)
        {
            if (ItemSelected != null) OnItemSelected(this, e);
        }

        private void NikotalkieItem_DoubleClick(object sender, EventArgs e)
        {
            if (ItemActionPlay != null) OnItemActionPlay(this, e);
        }

    }
}
