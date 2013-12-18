using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Microsoft.DirectX.DirectSound;
using System.Collections;
using System.IO;

namespace Remwave.Client.Controls
{
    public partial class NikotalkieRecordingView : UserControl
    {
        public MemoryStream CapturedStream
        {
            get
            {
                if (mDXSound == null) return null;
                return mDXSound.CapturedStream;
            }
        }

        public event EventHandler RecordingSend;
        internal void OnRecordingSend(object sender, EventArgs args)
        {
            if (RecordingSend != null)
            {
                RecordingSend(sender, args);
            }
        }

        public event EventHandler RecordingDone;
        internal void OnRecordingDone(object sender, EventArgs args)
        {
            if (RecordingDone != null)
            {
                RecordingDone(sender, args);
            }
        }

        public event EventHandler RecordingCanceled;
        internal void OnRecordingCanceled(object sender, EventArgs args)
        {
            if (RecordingCanceled != null)
            {
                RecordingCanceled(sender, args);
            }
        }

        private DXSound mDXSound;

        public NikotalkieRecordingView()
        {
            this.Dock = DockStyle.Fill;
            InitializeComponent();
          
        }

        public void StartRecording()
        {
            if(mDXSound==null) mDXSound = new DXSound(this);
            mDXSound.StartRecording(0);
            buttonStop.Visible = true;
            layoutReviewButtons.Visible = false;
            layoutReviewButtons.Enabled = false;
        }

        public void StopRecording()
        {
         if(mDXSound!=null)  mDXSound.StopRecording();
            buttonStop.Visible = false;
            layoutReviewButtons.Visible = true;
            layoutReviewButtons.Enabled = true;
        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            this.StopRecording();
            OnRecordingDone(sender, e);
        }

        private void buttonReview_Click(object sender, EventArgs e)
        {
            if (mDXSound == null) mDXSound = new DXSound(this);
            mDXSound.StartPlaying(mDXSound.CapturedStream);
        }

        private void buttonSend_Click(object sender, EventArgs e)
        {
            if (mDXSound == null) mDXSound = new DXSound(this);
            mDXSound.StopPlaying();
            OnRecordingSend(sender, e);
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            if (mDXSound == null) mDXSound = new DXSound(this);
            mDXSound.StopPlaying();
            OnRecordingCanceled(sender, e);
        }

        #region  Buttons Animation Events

        //STOP
        private void buttonStop_MouseDown(object sender, MouseEventArgs e)
        {
            buttonStop.Image = Properties.Resources.NikotalkieButtonStopDown;
        }

        private void buttonStop_MouseUp(object sender, MouseEventArgs e)
        {
            buttonStop.Image = Properties.Resources.NikotalkieButtonStop;
        }

        private void buttonStop_MouseEnter(object sender, EventArgs e)
        {
            buttonStop.Image = Properties.Resources.NikotalkieButtonStopOver;
        }

        private void buttonStop_MouseLeave(object sender, EventArgs e)
        {
            buttonStop.Image = Properties.Resources.NikotalkieButtonStop;
        }

        //REVIEW
        private void buttonReview_MouseDown(object sender, MouseEventArgs e)
        {
            buttonReview.Image = Properties.Resources.NikotalkieButtonReviewDown;
        }

        private void buttonReview_MouseUp(object sender, MouseEventArgs e)
        {
            buttonReview.Image = Properties.Resources.NikotalkieButtonReview;
        }

        private void buttonReview_MouseEnter(object sender, EventArgs e)
        {
            buttonReview.Image = Properties.Resources.NikotalkieButtonReviewOver;
        }

        private void buttonReview_MouseLeave(object sender, EventArgs e)
        {
            buttonReview.Image = Properties.Resources.NikotalkieButtonReview;
        }

        //CANCEL
        private void buttonCancel_MouseDown(object sender, MouseEventArgs e)
        {
            buttonCancel.Image = Properties.Resources.NikotalkieButtonCancelDown;
        }

        private void buttonCancel_MouseEnter(object sender, EventArgs e)
        {
            buttonCancel.Image = Properties.Resources.NikotalkieButtonCancelOver;
        }

        private void buttonCancel_MouseLeave(object sender, EventArgs e)
        {
            buttonCancel.Image = Properties.Resources.NikotalkieButtonCancel;
        }

        private void buttonCancel_MouseUp(object sender, MouseEventArgs e)
        {
            buttonCancel.Image = Properties.Resources.NikotalkieButtonCancel;
        }

        //SEND
        private void buttonSend_MouseDown(object sender, MouseEventArgs e)
        {
            buttonSend.Image = Properties.Resources.NikotalkieButtonSendDown;
        }

        private void buttonSend_MouseEnter(object sender, EventArgs e)
        {
            buttonSend.Image = Properties.Resources.NikotalkieButtonSendOver;
        }

        private void buttonSend_MouseLeave(object sender, EventArgs e)
        {
            buttonSend.Image = Properties.Resources.NikotalkieButtonSend;
        }

        private void buttonSend_MouseUp(object sender, MouseEventArgs e)
        {
            buttonSend.Image = Properties.Resources.NikotalkieButtonSend;
        }
        #endregion

    }
}
