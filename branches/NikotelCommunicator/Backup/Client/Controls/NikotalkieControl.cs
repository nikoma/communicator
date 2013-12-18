using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Collections;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using Remwave.Nikotalkie;

namespace Remwave.Client.Controls
{
    public partial class NikotalkieControl : UserControl
    {
        enum ControlView
        {
            ComposeMessage, Inbox, Outbox, RecordingAudio
        }

        private Point mMousePosition;
        private Boolean mLoggedIn = false;


        private NSettings mSettings;
        private NStorage mStorage = new NStorage();
        private Configuration mConfiguration;
        private BackgroundWorker mBackgroundWorker;
        private bool mBackgroundWorkerAtWork = false;
        private DeliveryWorker mDeliveryWorker;




        private NikotalkieFolderView mInboxView;
        private NikotalkieFolderView mOutboxView;
        private NikotalkieComposeMessageView mComposeMessageView;
        private NikotalkieRecordingView mRecordingView;

        private AMRCodec mCodec = new AMRCodec();
        private DXSound mDXSound;


        #region Public Events
        public event EventHandler HideControl;
        internal void OnHideControl(object sender, EventArgs args)
        {
            if (HideControl != null)
            {
                HideControl(sender, args);
            }
        }

        public event EventHandler ShowControl;
        internal void OnShowControl(object sender, EventArgs args)
        {
            if (ShowControl != null)
            {
                ShowControl(sender, args);
            }
        }

        public event EventHandler IncomingMessage;
        internal void OnIncomingMessage(object sender, EventArgs args)
        {
            if (IncomingMessage != null)
            {
                IncomingMessage(sender, args);
            }
        }

        public event EventHandler AcceptButtonChanged;
        internal void OnAcceptButtonChanged(object sender, EventArgs args)
        {
            if (AcceptButtonChanged != null)
            {
                AcceptButtonChanged(sender, args);
            }
        }

        #endregion

        #region Background Worker
        private void InitBackgroundWorker()
        {
            mBackgroundWorker = new BackgroundWorker();
            mBackgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(mBackgroundWorker_RunWorkerCompleted);
            mBackgroundWorker.ProgressChanged += new ProgressChangedEventHandler(mBackgroundWorker_ProgressChanged);
            mBackgroundWorker.DoWork += new DoWorkEventHandler(mBackgroundWorker_DoWork);
            mBackgroundWorker.WorkerReportsProgress = true;
            mBackgroundWorker.WorkerSupportsCancellation = true;
        }

        void mBackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            if (!mLoggedIn) return;
            this.mBackgroundWorkerAtWork = true;
            UIUpdateSendReceiveProgress(true, 0, "");
            BackgroundWorker bw = (BackgroundWorker)sender;
            mDeliveryWorker = new DeliveryWorker(this.mConfiguration, this.mSettings, this.mStorage, bw);
        }

        void mBackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            UIUpdateSendReceiveProgress(true, e.ProgressPercentage, (string)e.UserState);
        }

        void mBackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (mDeliveryWorker != null && mDeliveryWorker.NewMessages > 0)
            {
                nikotalkieNotifyIcon.Text = "You have " + mDeliveryWorker.NewMessages.ToString() + " new messages.";
                nikotalkieNotifyIcon.ShowBalloonTip(1000, "nikotalkie", "You have " + mDeliveryWorker.NewMessages.ToString() + " new messages.", ToolTipIcon.Info);
                this.OnIncomingMessage(this, new EventArgs());
            }

            UIUpdateSendReceiveProgress(false, 0, "");
            this.mBackgroundWorkerAtWork = false;
        }

        #endregion

        public NikotalkieControl()
        {
            InitializeComponent();
            InitBackgroundWorker();
          
        }

        #region Presenter
        private void InitializeViews(NStorage storage)
        {
            mInboxView = new NikotalkieFolderView(storage.InboxIndex, this);
            mInboxView.Visible = false;
            this.Controls.Add(mInboxView);

            mOutboxView = new NikotalkieFolderView(storage.OutboxIndex, this);
            mOutboxView.Visible = false;
            this.Controls.Add(mOutboxView);

            mComposeMessageView = new NikotalkieComposeMessageView();
            mComposeMessageView.Visible = false;
            mComposeMessageView.TalkClicked += new EventHandler(mNewMessageView_TalkClicked);
            this.Controls.Add(mComposeMessageView);



            mRecordingView = new NikotalkieRecordingView();
            mRecordingView.Visible = false;
            mRecordingView.RecordingCanceled += new EventHandler(mRecordingView_RecordingCanceled);
            mRecordingView.RecordingSend += new EventHandler(mRecordingView_RecordingSend);
            mRecordingView.RecordingDone += new EventHandler(mRecordingView_RecordingDone);
            this.Controls.Add(mRecordingView);


            SwitchActiveView(ControlView.ComposeMessage);

        }


        private void SwitchActiveView(ControlView controlView)
        {
            try
            {
                mInboxView.Visible = (ControlView.Inbox == controlView);
                mOutboxView.Visible = (ControlView.Outbox == controlView);
                mComposeMessageView.Visible = (ControlView.ComposeMessage == controlView);
                mRecordingView.Visible = (ControlView.RecordingAudio == controlView);

                //set accept button

                switch (controlView)
                {
                    case ControlView.ComposeMessage:
                        OnAcceptButtonChanged(mComposeMessageView.buttonTalk, new EventArgs());
                        break;
                    case ControlView.Inbox:
                        OnAcceptButtonChanged(null, new EventArgs());
                        break;
                    case ControlView.Outbox:
                        OnAcceptButtonChanged(null, new EventArgs());
                        break;
                    case ControlView.RecordingAudio:
                        OnAcceptButtonChanged(mRecordingView.buttonStop, new EventArgs());
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
#if TRACE
                Console.WriteLine(" SwitchActiveView " + ex.Message);
#endif

            }
        }
        private delegate void UIUpdateSendReceiveProgressDelegate(bool visible, int progress, string text);
        private void UIUpdateSendReceiveProgress(bool visible, int progress, string text)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new UIUpdateSendReceiveProgressDelegate(this.UIUpdateSendReceiveProgress), new object[] { visible, progress, text });
                return;
            }

            progressBarDeliveryWorker.Value = progress;
            progressBarDeliveryWorker.Visible = visible;
            trayMenuSendReceive.Enabled = !visible;

        }
        private delegate void UpdateInboxViewDelegate();
        private void UpdateInboxView()
        {
            if (this.InvokeRequired)
            {
                BeginInvoke(new UpdateInboxViewDelegate(UpdateInboxView));
                return;
            }
            mInboxView.RefreshView();
        }
        #endregion

        #region Authorization
        public void Logout()
        {
            this.mLoggedIn = false;
        }

        public void Login(String username, String password, String url, String path)
        {
            this.mLoggedIn = false;

            if (mConfiguration != null) mConfiguration = null;

            //Authorize(username, password, type) type=XML
            Hashtable parameters = new Hashtable();
            parameters.Add("username", username);
            parameters.Add("password", password);
            parameters.Add("type", "XML");

            try
            {
                NResultAuthorize resultAuthorize = new NResultAuthorize();
                WebRequest requestAuthorize = WebRequest.Create(url + NTransport.BulildWebRequestQuery("XMLAuthorize", parameters));
                HttpWebResponse responseRequestAuthorize = (HttpWebResponse)requestAuthorize.GetResponse();
                Stream dataStream = responseRequestAuthorize.GetResponseStream();
                XmlSerializer des = new XmlSerializer(typeof(NResultAuthorize));
                resultAuthorize = (NResultAuthorize)des.Deserialize(new System.Xml.XmlTextReader(dataStream));
                dataStream.Close();

                if (resultAuthorize.Success)
                {
                    mStorage.InitializeStorage(path);
                    mConfiguration = new Configuration(path);
                    mConfiguration.Username = username;
                    mConfiguration.Password = password;
                    mConfiguration.LastMessageID = mStorage.InboxIndex.LastMessageId;
#if(DEBUG)
                    mConfiguration.Url = url;
#else
                     mConfiguration.Url  = resultAuthorize.WebServiceUrl;
#endif

                    mSettings = new NSettings(path);
                    InitializeViews(mStorage);
                    this.mLoggedIn = true;
                }
                else
                {
                    this.mLoggedIn = false;
                }
            }
            catch (Exception ex)
            {
#if(DEBUG)
                throw;
#endif
            }
        }
        #endregion

        #region Buttons Animation Events

        private void NikotalkieControl_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Parent.Location = new Point(System.Windows.Forms.Cursor.Position.X - 150, System.Windows.Forms.Cursor.Position.Y - 30);

            }
        }

        private void NikotalkieControl_MouseDown(object sender, MouseEventArgs e)
        {
            this.mMousePosition = e.Location;
        }

        private void btnContacts_MouseEnter(object sender, EventArgs e)
        {
            btnContacts.ImageKey = "NikotalkieButtonContactsPressed";
        }

        private void btnContacts_MouseLeave(object sender, EventArgs e)
        {
            btnContacts.ImageKey = "NikotalkieButtonContacts";
        }

        private void btnIncoming_MouseEnter(object sender, EventArgs e)
        {
            btnIncoming.ImageKey = "NikotalkieButtonInboxPressed";
        }

        private void btnIncoming_MouseLeave(object sender, EventArgs e)
        {
            btnIncoming.ImageKey = "NikotalkieButtonInbox";
        }

        private void buttonOutgoing_MouseEnter(object sender, EventArgs e)
        {
            buttonOutgoing.ImageKey = "NikotalkieButtonSentboxPressed";
        }

        private void buttonOutgoing_MouseLeave(object sender, EventArgs e)
        {
            buttonOutgoing.ImageKey = "NikotalkieButtonSentbox";
        }

        #endregion

        #region Buttons Events
        private void btnIncoming_Click(object sender, EventArgs e)
        {
            SwitchActiveView(ControlView.Inbox);
        }
        private void btnContacts_Click(object sender, EventArgs e)
        {
            SwitchActiveView(ControlView.ComposeMessage);
        }
        private void buttonOutgoing_Click(object sender, System.EventArgs e)
        {
            SwitchActiveView(ControlView.Outbox);
        }
        #endregion

        #region Context Menu Events
        private void trayMenuSendReceive_Click(object sender, EventArgs e)
        {
            if (!this.mBackgroundWorkerAtWork && !this.mBackgroundWorker.IsBusy)
            {
                try
                {
                    mBackgroundWorker.RunWorkerAsync();
                }
                catch (Exception)
                {
#if (DEBUG)
                    throw;
#endif
                }

            }
        }
        #endregion

        #region Views Events
        //ComposeMessage View
        void mNewMessageView_TalkClicked(object sender, EventArgs e)
        {
            SwitchActiveView(ControlView.RecordingAudio);
            mRecordingView.StartRecording();
        }
        void mRecordingView_RecordingSend(object sender, EventArgs e)
        {
            //simple version, wrap up what we have and send out;

            NMessage message = new NMessage();
            message.Header = new NMessageHeader();
            message.Header.From = mConfiguration.Username;
            message.Header.To = mComposeMessageView.Recipients;
            message.Header.AttachementHeaders = new List<NMessageAttachementHeader>();
            NMessageAttachementHeader attachement = new NMessageAttachementHeader();
            attachement.AttachementType = NMessageAttachementType.Audio;
            attachement.ID = 0;
            message.Header.AttachementHeaders.Add(attachement);
            message.Body = new NMessageBody();
            message.Body.AttachementBodys = new List<NMessageAttachementBody>();
            NMessageAttachementBody attachementBody = new NMessageAttachementBody();
            attachementBody.AttachementType = NMessageAttachementType.Audio;
            attachementBody.ID = 0;
            attachementBody.Name = "Voice Message";
            attachementBody.Data = mCodec.Encode(mRecordingView.CapturedStream).ToArray();
            attachementBody.Size = attachementBody.Data.Length;
            message.Body.AttachementBodys.Add(attachementBody);
            mStorage.SaveOutbox(message);
            SwitchActiveView(ControlView.ComposeMessage);
            mComposeMessageView.NewMessage();
        }
        void mRecordingView_RecordingCanceled(object sender, EventArgs e)
        {
            SwitchActiveView(ControlView.ComposeMessage);
        }
        void mRecordingView_RecordingDone(object sender, EventArgs e)
        {
            OnAcceptButtonChanged(mRecordingView.buttonSend, new EventArgs());
        }
        #endregion

        #region Timers
        private void timerDeliveryWorker_Tick(object sender, EventArgs e)
        {
            if (!this.mBackgroundWorkerAtWork && !this.mBackgroundWorker.IsBusy)
            {
                try
                {
                    mBackgroundWorker.RunWorkerAsync();
                }
                catch (Exception)
                {
#if (DEBUG)
                    throw;
#endif
                }

            }
        }
        #endregion


        #region Public Methods

        public void StartPlayingMessage(NMessage message)
        {
           if(mDXSound==null) mDXSound = new DXSound(this);
            NMessage tmpMessage = mStorage.GetMessage(message);
            if (tmpMessage == null || tmpMessage.Body.AttachementBodys.Count == 0) return;
            MemoryStream memoryStream = new MemoryStream(tmpMessage.Body.AttachementBodys[0].Data);
            mDXSound.StartPlaying(mCodec.Decode(memoryStream));
        }
        public void StopPlayingMessage()
        {
            if (mDXSound != null) mDXSound.StopPlaying();
        }

        public void MessageDelete(NMessage message)
        {
            mStorage.DeleteMessage(message);
        }

        public void MessageReply(NMessage message)
        {
            mComposeMessageView.SetRecipient(message.Header.From);
            SwitchActiveView(ControlView.ComposeMessage);
        }

        public void MessageReply(String recipient)
        {
            mComposeMessageView.SetRecipient(recipient);
            SwitchActiveView(ControlView.ComposeMessage);
        }

        #endregion

        private void nikotalkieNotifyIcon_Click(object sender, EventArgs e)
        {
            OnShowControl(this, new EventArgs());
        }

        private void nikotalkieNotifyIcon_DoubleClick(object sender, EventArgs e)
        {
            OnShowControl(this, new EventArgs());
        }

    }
}
