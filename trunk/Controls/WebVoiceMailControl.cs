using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Web;
using System.Xml.Serialization;
using System.IO;
using Remwave.Common;
using System.Collections;

namespace Remwave.Client.Controls
{
    public partial class WebVoiceMailControl : UserControl
    {
        #region Member Variables
        //Used for keeping syncronized events happening
        System.Windows.Forms.Timer mTimer = new Timer();
        volatile bool mPerformingUpdate = false;
        volatile bool mDownloadingFile = false;

        const int UpdateRateInSeconds = 10;

        string mUserName = "";
        string mPassword = "";
        string mBaseDirectory = "";
        VoiceMailEntry mSelectedVoiceMailEntry;

        DateTime mlastUpdate = System.DateTime.MinValue;

        MailService.Service mMailSerive = new MailService.Service();

        #endregion

        #region Setup / Initialization

        public WebVoiceMailControl()
        {
            InitializeComponent();
            toolButtonPlay.Visible = false;
            toolButtonStop.Visible = false;
            toolButtonGet.Visible = false;
            toolButtonEmail.Visible = false;
            toolButtonDelete.Visible = false;
        }

        /// <summary>
        /// Initialize Control
        /// </summary>
        /// <param name="username">Username to pass to webservice</param>
        /// <param name="password">Password to pass to webservice</param>
        /// <param name="baseDirectory">Directory We use as a point to cache files</param>
        public void IntializeControl(string username, string password, string baseDirectory)
        {
            mUserName = username;
            mPassword = password;
            mBaseDirectory = baseDirectory + @"\" + mUserName + @"\";

            if (Directory.Exists(mBaseDirectory) == false)
                Directory.CreateDirectory(mBaseDirectory);

            if (Directory.Exists(mBaseDirectory + @"ARCHIVE\") == false)
                Directory.CreateDirectory(mBaseDirectory + @"ARCHIVE\");

            if (Directory.Exists(mBaseDirectory + @"INBOX\") == false)
                Directory.CreateDirectory(mBaseDirectory + @"INBOX\");

            //Configure Service
            mMailSerive.VoiceMail_GetMessageListCompleted += new MailService.VoiceMail_GetMessageListCompletedEventHandler(mailSerive_VoiceMail_GetMessageListCompleted);

            InitializeVoiceMailEntries();

            //Start update timers
            mTimer.Interval = UpdateRateInSeconds * 1000;
            mTimer.Tick += new EventHandler(mTimer_SyncTick);
            mTimer.Enabled = true;
        }

        private void InitializeVoiceMailEntries()
        {
            //Clear anything left over from a previous run
            pEmailEntries.Controls.Clear();

            List<InfoFile> localCachedFileInfo = new List<InfoFile>();

            //Scan current cache of voice mail messages
            ProcessFiles(localCachedFileInfo, mBaseDirectory + @"INBOX\", InfoFile.LocationType.INBOX);
            ProcessFiles(localCachedFileInfo, mBaseDirectory + @"ARCHIVE\", InfoFile.LocationType.ARCHIVE);

            //Add already stored files to view


            //IComparer<InfoFile> comparer = new InfoFileComparer();
            //localCachedFileInfo.Sort(comparer);
            foreach (InfoFile info in localCachedFileInfo)
            {
                pEmailEntries.Controls.Add(new VoiceMailEntry(this, info));
            }
            if (pEmailEntries.Controls.Count > 0)
            {
                ((VoiceMailEntry)pEmailEntries.Controls[pEmailEntries.Controls.Count - 1]).SelectControl();
            }
        }

        void ProcessFiles(List<InfoFile> list, String dir, InfoFile.LocationType type)
        {
            try
            {
                string[] XMLFile = Directory.GetFiles(dir, "*.xml");
                foreach (string file in XMLFile)
                {
                    FileStream fs = null;
                    try
                    {
                        try
                        {
                            XmlSerializer des = new XmlSerializer(typeof(InfoFile));
                            fs = File.OpenRead(file);
                            InfoFile infoFile = (InfoFile)des.Deserialize(fs);
                            fs.Close();
                            fs = null;
                            if (File.Exists(dir + infoFile.FileName)) infoFile.Cached = true;
                            infoFile.Location = type;
                            list.Add(infoFile);
                        }
                        catch
                        {
                        }

                    }
                    catch (Exception)
                    {
                        continue;
                    }
                }
            }
            catch
            {
            }
        }

        #endregion

        #region Registerable Events

        public event EventHandler StartPlayingVoiceMail;
        /// <summary>
        /// Event happens when user click Play button , sender will be VoiceMailEntry, where you can retrieve file info
        /// </summary>
        internal void OnStartPlayingVoiceMail(VoiceMailEntry sender, EventArgs args)
        {
            if (StartPlayingVoiceMail != null)
            {
                StartPlayingVoiceMail(sender, args);
            }
        }

        public event EventHandler StopPlayingVoiceMail;
        /// <summary>
        /// Event happens when user click Stop button , sender will be VoiceMailEntry, where you can retrieve file info
        /// </summary>
        internal void OnStopPlayingVoiceMail(VoiceMailEntry sender, EventArgs args)
        {
            if (StopPlayingVoiceMail != null)
            {
                StopPlayingVoiceMail(sender, args);
            }
        }
        /// 
        /// 
        /// <summary>
        /// Event happens when user selects VoiceMailEntry , sender will be VoiceMailEntry, where you can retrieve file info
        /// </summary>
        public event EventHandler SelectedMailEntry;
        internal void OnSelectedMailEntry(VoiceMailEntry sender, EventArgs args)
        {
            if (SelectedMailEntry != null)
                SelectedMailEntry(sender, args);

            if (mSelectedVoiceMailEntry != null && mSelectedVoiceMailEntry != sender)
            {
                mSelectedVoiceMailEntry.Selected = false;
                mSelectedVoiceMailEntry.BackgroundImage = MailBackgroundImage;
                mSelectedVoiceMailEntry.BackgroundImageLayout = MailBackgroundImageLayout;
                mSelectedVoiceMailEntry.BackColor = MailBackgroundColor;
            }
            mSelectedVoiceMailEntry = sender;
            mSelectedVoiceMailEntry.Selected = true;
            mSelectedVoiceMailEntry.BackgroundImage = SelectedMailBackgroundImage;
            mSelectedVoiceMailEntry.BackgroundImageLayout = SelectedMailBackgroundImageLayout;
            mSelectedVoiceMailEntry.BackColor = SelectedMailBackgroundColor;

            if (mSelectedVoiceMailEntry.Cached)
            {
                toolButtonPlay.Visible = true;
                toolButtonEmail.Visible = true;
                toolButtonDelete.Visible = true;
                toolButtonDelete.Enabled = true;

                toolButtonGet.Visible = false;
            }
            else
            {

                toolButtonGet.Visible = !this.Synchronize;
                toolButtonDelete.Visible = true;
                toolButtonGet.Enabled = !mSelectedVoiceMailEntry.DownloadInProgress;
                toolButtonDelete.Enabled = !mSelectedVoiceMailEntry.DownloadInProgress;

                toolButtonPlay.Visible = false;
                toolButtonEmail.Visible = false;
            }
        }

        /// <summary>
        /// Event Handler for new mail message
        /// </summary>
        /// <param name="sender">WebVoiceMailControl</param>
        /// <param name="newInboxMessageCount">Number of incoming inbox messages</param>
        /// <param name="newArchiveMessageCount">Number of incoming archive messages</param>
        public delegate void NewMailHandler(WebVoiceMailControl sender, int newInboxMessageCount, int newArchiveMessageCount);
        /// <summary>Event for new incoming messages </summary>
        public event NewMailHandler NewMailEvent;
        internal void OnNewMailEvent(WebVoiceMailControl sender, int newInboxMessageCount, int newArchiveMessageCount)
        {
            if (NewMailEvent != null)
                NewMailEvent(sender, newInboxMessageCount, newArchiveMessageCount);
        }

        #endregion

        #region Properties
        public bool Synchronize
        {
            get { return toolButtonKeepSync.Checked; }
            set
            {
                toolButtonKeepSync.Checked = value;
                toolButtonRefresh.Enabled = !value;

                //Update all controls:
                //foreach (Control control in pEmailEntries.Controls)
                //{
                //    VoiceMailEntry vm = (VoiceMailEntry)control;
                //}
            }
        }

        public bool FileDownloadInProgress
        {
            get { return mDownloadingFile; }
        }

        internal bool SetFileDownloadInProgress
        {
            set { mDownloadingFile = value; }
        }

        public string BasePath
        {
            get { return mBaseDirectory; }
        }

        internal string UserName
        {
            get { return mUserName; }
        }

        internal string Password
        {
            get { return mPassword; }
        }

        #endregion

        #region Design Properties
        #region Color
        Color mSelectedMailBackgroundColor = Color.DarkGreen;
        public Color SelectedMailBackgroundColor
        {
            get { return mSelectedMailBackgroundColor; }
            set
            {
                mSelectedMailBackgroundColor = value;
                foreach (Control control in this.pEmailEntries.Controls)
                {
                    if (((VoiceMailEntry)control).Info.Location == InfoFile.LocationType.INBOX)
                        control.BackColor = value;
                }
            }
        }

        Color mMailBackgroundColor = Color.DarkMagenta;
        public Color MailBackgroundColor
        {
            get { return mMailBackgroundColor; }
            set
            {
                mMailBackgroundColor = value;
                foreach (Control control in this.pEmailEntries.Controls)
                {
                    if (((VoiceMailEntry)control).Info.Location == InfoFile.LocationType.ARCHIVE)
                        control.BackColor = value;
                }
            }
        }
        #endregion

        #region Images
        Image mSelectedMailBackgroundImage = null;
        public Image SelectedMailBackgroundImage
        {
            get { return mSelectedMailBackgroundImage; }
            set
            {
                mSelectedMailBackgroundImage = value;
                foreach (Control control in this.pEmailEntries.Controls)
                {
                    if (((VoiceMailEntry)control).Info.Location == InfoFile.LocationType.INBOX)
                        control.BackgroundImage = value;
                }
            }
        }

        Image mMailBackgroundImage = null;
        public Image MailBackgroundImage
        {
            get { return mMailBackgroundImage; }
            set
            {
                mMailBackgroundImage = value;
                foreach (Control control in this.pEmailEntries.Controls)
                {
                    if (((VoiceMailEntry)control).Info.Location == InfoFile.LocationType.ARCHIVE)
                        control.BackgroundImage = value;
                }
            }
        }

        ImageLayout mSelectedMailBackgroundImageLayout = ImageLayout.Tile;
        public ImageLayout SelectedMailBackgroundImageLayout
        {
            get { return mSelectedMailBackgroundImageLayout; }
            set
            {
                mSelectedMailBackgroundImageLayout = value;
                foreach (Control control in this.pEmailEntries.Controls)
                {
                    if (((VoiceMailEntry)control).Info.Location == InfoFile.LocationType.INBOX)
                        control.BackgroundImageLayout = value;
                }
            }
        }

        ImageLayout mMailBackgroundImageLayout = ImageLayout.Tile;
        public ImageLayout MailBackgroundImageLayout
        {
            get { return mMailBackgroundImageLayout; }
            set
            {
                mMailBackgroundImageLayout = value;
                foreach (Control control in this.pEmailEntries.Controls)
                {
                    if (((VoiceMailEntry)control).Info.Location == InfoFile.LocationType.ARCHIVE)
                        control.BackgroundImageLayout = value;
                }
            }
        }

        Image mMailPictureInboxImage = null;
        public Image MailInboxPicture
        {
            get { return mMailPictureInboxImage; }
            set
            {
                mMailPictureInboxImage = value;
                foreach (Control control in this.pEmailEntries.Controls)
                {
                    if (((VoiceMailEntry)control).Info.Location == InfoFile.LocationType.INBOX)
                        ((VoiceMailEntry)control).MailPictureImage = value;
                }
            }
        }


        Image mMailNewInboxPicture = null;
        public Image MailNewInboxPicture
        {
            get { return mMailNewInboxPicture; }
            set
            {
                mMailNewInboxPicture = value;
                foreach (Control control in this.pEmailEntries.Controls)
                {
                    if (!((VoiceMailEntry)control).Cached)
                        ((VoiceMailEntry)control).MailPictureImage = value;
                }
            }
        }
        Image mMailPictureArchiveImage = null;
        public Image MailArchivePicture
        {
            get { return mMailPictureArchiveImage; }
            set
            {
                mMailPictureArchiveImage = value;
                foreach (Control control in this.pEmailEntries.Controls)
                {
                    if (((VoiceMailEntry)control).Info.Location == InfoFile.LocationType.ARCHIVE)
                        ((VoiceMailEntry)control).MailPictureImage = value;
                }
            }
        }

        #endregion

        #region Other
        public Panel MainBackPanel
        {
            get { return this.pEmailEntries; }
        }
        #endregion

        #endregion

        #region Mail Update Events

        void mTimer_SyncTick(object sender, EventArgs e)
        {
            if (Synchronize)
            {
                if (mDownloadingFile == false)
                {
                    foreach (Control control in pEmailEntries.Controls)
                    {
                        VoiceMailEntry vm = (VoiceMailEntry)control;
                        if (!vm.Cached)
                        {
                            vm.DownloadMail();
                            break;
                        }
                    }
                }

                //In this same timeout, we handle getting new data
                if (mPerformingUpdate)
                    return;
                mlastUpdate = System.DateTime.Now;
                mPerformingUpdate = true;
                mMailSerive.VoiceMail_GetMessageListAsync(mUserName, mPassword);
            }
            else
            {
                TimeSpan interval = System.DateTime.Now - mlastUpdate;

                if (interval.TotalSeconds >= UpdateRateInSeconds)
                {
                    toolButtonRefresh.Enabled = true;
                }
            }
        }

        #endregion

        #region Internal Methods
        void mailSerive_VoiceMail_GetMessageListCompleted(object sender, MailService.VoiceMail_GetMessageListCompletedEventArgs e)
        {
            mPerformingUpdate = false;

            if (e.Cancelled || e.Error != null || e.Result == null)
                return;

            this.Invoke(new MessageListUpdated(MessageListUpdate), new Object[] { e.Result });
        }

        delegate void MessageListUpdated(MailService.FileInfo[] list);
        void MessageListUpdate(MailService.FileInfo[] list)
        {
            int newInboxFiles = 0;
            int newArchiveFiles = 0;

            foreach (MailService.FileInfo info in list)
            {
                try
                {
                    //File was not in our local list, so add it
                    if (pEmailEntries.Controls.ContainsKey(info.FileName) == false)
                    {
                        if (info.Location == MailService.LocationType.INBOX)
                            ++newInboxFiles;
                        else if (info.Location == MailService.LocationType.ARCHIVE)
                            ++newArchiveFiles;

                        String newFileInfoName = this.BasePath + info.Location.ToString() + @"\" + info.FileName + ".xml"; ;
                        String tempFileInfoName = Path.GetTempFileName();

                        XmlSerializer ser = new XmlSerializer(typeof(InfoFile));
                        FileStream fs = File.OpenWrite(tempFileInfoName);

                        InfoFile infoFile = new InfoFile(info);
                        infoFile.Cached = File.Exists(mBaseDirectory + info.Location.ToString() + @"\" + info.FileName);

                        ser.Serialize(fs, infoFile);
                        fs.Close();

                        //Now move files
                        if (File.Exists(newFileInfoName)) File.Delete(newFileInfoName);
                        File.Move(tempFileInfoName, newFileInfoName);

                        pEmailEntries.Controls.Add(new VoiceMailEntry(this, infoFile));

                    }
                    else
                    {
                        //Check for a message which may have moved from Inbox to Archive
                        VoiceMailEntry entry = (VoiceMailEntry)pEmailEntries.Controls[info.FileName];
                        if ((MailService.LocationType)entry.Info.Location != info.Location && info.Location == MailService.LocationType.ARCHIVE)
                            entry.ArchiveLocal();
                    }
                }
                catch
                {
                }
            }

            if (newInboxFiles > 0 || newArchiveFiles > 0)
                OnNewMailEvent(this, newInboxFiles, newArchiveFiles);
        }

        /// <summary>
        /// Clear out menu item from our control list
        /// </summary>
        /// <param name="entry"></param>
        internal void Purge(VoiceMailEntry entry)
        {
            toolButtonPlay.Visible = false;
            toolButtonEmail.Visible = false;
            toolButtonDelete.Visible = false;
            toolButtonGet.Visible = false;

            if (pEmailEntries.Controls.IndexOf(entry) > 0)
            {
                OnSelectedMailEntry((VoiceMailEntry)pEmailEntries.Controls[pEmailEntries.Controls.IndexOf(entry) - 1], new EventArgs());
            }
            else
                if (pEmailEntries.Controls.Count > 1)
                {
                    OnSelectedMailEntry((VoiceMailEntry)pEmailEntries.Controls[1], new EventArgs());
                }
            pEmailEntries.Controls.Remove(entry);
        }
        #endregion


        #region Toolbar Events
        private void bRefresh_Click(object sender, EventArgs e)
        {
            CheckForVoicemails();
        }

        public void CheckForVoicemails()
        {
            if (Synchronize == true || mPerformingUpdate)
                return;

            TimeSpan interval = System.DateTime.Now - mlastUpdate;

            if (interval.TotalSeconds >= UpdateRateInSeconds)
            {
                mlastUpdate = System.DateTime.Now;
                mPerformingUpdate = true;
                mMailSerive.VoiceMail_GetMessageListAsync(mUserName, mPassword);
                toolButtonRefresh.Enabled = false;
            }
        }

        private void bKeepSyned_Click(object sender, EventArgs e)
        {
            Synchronize = toolButtonKeepSync.Checked;
        }

        private void toolButtonStop_Click(object sender, EventArgs e)
        {
            if (mSelectedVoiceMailEntry != null)
            {
                StopPlayingVoiceMail(mSelectedVoiceMailEntry, e);
                toolButtonPlay.Visible = true;
                toolButtonStop.Visible = false;
            }
        }

        private void toolButtonPlay_Click(object sender, EventArgs e)
        {
            if (mSelectedVoiceMailEntry != null && mSelectedVoiceMailEntry.Cached)
            {
                toolButtonPlay.Visible = false;
                toolButtonStop.Visible = true;
                StartPlayingVoiceMail(mSelectedVoiceMailEntry, e);
                mSelectedVoiceMailEntry.ArchiveLocal();
            }
        }

        private void toolButtonGet_Click(object sender, EventArgs e)
        {
            mSelectedVoiceMailEntry.DownloadMail();
        }

        private void toolButtonEmail_Click(object sender, EventArgs e)
        {
            MAPI mapi = new MAPI();
            mapi.AddAttachment(mSelectedVoiceMailEntry.FullPath);
            mapi.SendMailPopup("FW: Voicemail from " + mSelectedVoiceMailEntry.Info.From, "FW: Voicemail from " + mSelectedVoiceMailEntry.Info.From);
        }

        private void toolButtonDelete_Click(object sender, EventArgs e)
        {
            mSelectedVoiceMailEntry.DeleteVoiceMail();
        }
        #endregion






    }
}
