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
    public partial class NikotalkieFolderView : UserControl
    {
        private NikotalkieControl mOwner;
        private int mItemHeight = 24;
        private int mOffset = 0;
        private NStorageIndex mNStorageIndex;
        private List<NikotalkieItem> mNikotalkieItems = new List<NikotalkieItem>();
        private NMessage mSelectedMessage;
        private Int32 mSelectedMessageIndex = 0;

        public NMessage SelectedMessage
        {
            get { return mSelectedMessage; }
            set
            {
                mSelectedMessage = value; 
            
            //TO DO UPDATE SELECTION
            
            }
        }
        

        public NikotalkieFolderView()
        {
            this.Dock = DockStyle.Fill;
            InitializeComponent();
        }

        public NikotalkieFolderView(NStorageIndex storageIndex, NikotalkieControl owner)
        {
            mOwner = owner;
            mNStorageIndex = storageIndex;
            mNStorageIndex.IndexChanged += new EventHandler(mNStorageIndex_IndexChanged);
            this.Dock = DockStyle.Fill;
            InitializeComponent();
            InitializeItems();
        }

        void mNStorageIndex_IndexChanged(object sender, EventArgs e)
        {
            RefreshView();
        }

        public void InitializeItems()
        {
            int itemsPerPage =Items.Height / mItemHeight;
           
            //only if needed 
            if (itemsPerPage  == mNikotalkieItems.Count) return;
            
            //add items
            if (itemsPerPage > mNikotalkieItems.Count)
            {

                for (int i = mNikotalkieItems.Count; i < itemsPerPage; i++)
                {
                    NikotalkieItem nikotalkieItem = new NikotalkieItem();
                    nikotalkieItem.ItemSelected += new EventHandler(nikotalkieItem_ItemSelected);
                    nikotalkieItem.ItemActionPlay += new EventHandler(nikotalkieItem_ItemActionPlay);
                    nikotalkieItem.Height = mItemHeight;
                    mNikotalkieItems.Add(nikotalkieItem);
                    this.Items.Controls.Add(nikotalkieItem);
                    nikotalkieItem.BringToFront();
                }
            }

            //remove items
            if (itemsPerPage < mNikotalkieItems.Count)
            {
                for (int i = mNikotalkieItems.Count-1; i > itemsPerPage-1; i--)
                {
                    mNikotalkieItems[i].Dispose();
                    mNikotalkieItems[i]= null;
                    mNikotalkieItems.RemoveAt(i);
                }
            }
            
            //refresh
            RefreshView();
        }

        void nikotalkieItem_ItemActionPlay(object sender, EventArgs e)
        {
            //TO DO PLAY SELECTED MESSAGE;
            if (mSelectedMessage == null) return;
           NikotalkieItem selectedNikotalkieItem = (NikotalkieItem)sender;
           this.mSelectedMessage = (NMessage)selectedNikotalkieItem.Tag;
                this.RefreshView();
                mOwner.StartPlayingMessage(this.mSelectedMessage);
        }

       private void nikotalkieItem_ItemSelected(object sender, EventArgs e)
        {
            NikotalkieItem selectedNikotalkieItem = (NikotalkieItem)sender;
            this.mSelectedMessage = (NMessage)selectedNikotalkieItem.Tag;
           this.mSelectedMessageIndex = mNikotalkieItems.IndexOf(selectedNikotalkieItem);
            this.RefreshView();
        }

        private delegate void RefreshViewDelegate();
        public void RefreshView()
        {
            if (this.InvokeRequired)
            {
                BeginInvoke(new RefreshViewDelegate(RefreshView));
                return;
            }
            if (!this.Visible || mNStorageIndex == null) return;
            for (int i = 0; i < mNikotalkieItems.Count; i++)
            {
                if (i + mOffset < mNStorageIndex.Messages.Count)
                {
                    mNikotalkieItems[i].SetItem(mNStorageIndex.Messages[i + mOffset]);
                    if (mSelectedMessage!=null && mSelectedMessage.Header.MsgID == mNStorageIndex.Messages[i + mOffset].Header.MsgID)
                    {

                        mNikotalkieItems[i].SelectItem();
                    }
                    else if (mSelectedMessage == null && i == mSelectedMessageIndex)
                    {
                        mNikotalkieItems[i].SelectItem();
                    }
                    else
                    {
                        mNikotalkieItems[i].UnSelectItem();
                    }
                }
                else
                {
                    mNikotalkieItems[i].UnSetItem();
                }
            }

            VerticalScrollBar.Minimum = 0;
            VerticalScrollBar.Maximum = mNStorageIndex.Messages.Count;
            VerticalScrollBar.LargeChange = mNikotalkieItems.Count;
            VerticalScrollBar.SmallChange=1;
        }

        private void VerticalScrollBar_Scroll(object sender, ScrollEventArgs e)
        {
            this.mOffset = e.NewValue;
            RefreshView();
        }

        private void Items_Resize(object sender, EventArgs e)
        {
            InitializeItems();
        }

        #region Buttons Events
        
        #endregion

        #region Buttons Animation Events


        private void buttonDelete_MouseEnter(object sender, EventArgs e)
        {
            buttonDelete.Image = Properties.Resources.NikotalkieButtonDeleteOver;
        }

        private void buttonDelete_MouseLeave(object sender, EventArgs e)
        {
            buttonDelete.Image = Properties.Resources.NikotalkieButtonDelete;
        }

        private void buttonDelete_MouseUp(object sender, MouseEventArgs e)
        {
            buttonDelete.Image = Properties.Resources.NikotalkieButtonDelete;
        }

        private void buttonDelete_MouseDown(object sender, MouseEventArgs e)
        {
            buttonDelete.Image = Properties.Resources.NikotalkieButtonDeleteDown;
        }

        private void buttonPlay_MouseDown(object sender, MouseEventArgs e)
        {
            buttonPlay.Image = Properties.Resources.NikotalkieButtonPlayDown;
        }

        private void buttonPlay_MouseEnter(object sender, EventArgs e)
        {
            buttonPlay.Image = Properties.Resources.NikotalkieButtonPlayOver;
        }

        private void buttonPlay_MouseLeave(object sender, EventArgs e)
        {
            buttonPlay.Image = Properties.Resources.NikotalkieButtonPlay;
        }

        private void buttonPlay_MouseUp(object sender, MouseEventArgs e)
        {
            buttonPlay.Image = Properties.Resources.NikotalkieButtonPlay;
        }

        private void buttonReply_MouseDown(object sender, MouseEventArgs e)
        {
            buttonReply.Image = Properties.Resources.NikotalkieButtonReplyDown;
        }

        private void buttonReply_MouseLeave(object sender, EventArgs e)
        {
            buttonReply.Image = Properties.Resources.NikotalkieButtonReply;
        }

        private void buttonReply_MouseEnter(object sender, EventArgs e)
        {
            buttonReply.Image = Properties.Resources.NikotalkieButtonReplyOver;
        }

        private void buttonReply_MouseUp(object sender, MouseEventArgs e)
        {
            buttonReply.Image = Properties.Resources.NikotalkieButtonReply;
        }
        #endregion

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            if (this.mSelectedMessage != null)
            {
                if (this.mSelectedMessage != null)
                {
                    mOwner.MessageDelete(this.mSelectedMessage);
                    this.SelectedMessage = null;
                }
            }
        }

        private void buttonPlay_Click(object sender, EventArgs e)
        {
            if (this.mSelectedMessage != null)
            {
                mOwner.StartPlayingMessage(this.mSelectedMessage);
            }
        }

        private void buttonReply_Click(object sender, EventArgs e)
        {
           if (this.mSelectedMessage != null)
           {
               mOwner.MessageReply(this.mSelectedMessage);
            }
        }
    }
}
