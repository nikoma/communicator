using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Xml.Serialization;

namespace Remwave.Client.Controls
{
    public partial class VoiceMailEntry : UserControl
    {
        #region Member Variables
        WebVoiceMailControl mOwner = null;
        InfoFile mFileInfo = null;

        MailService.ServiceWse mDownloadService = null;
        MailService.ServiceWse mDeleteService = null;
        MailService.ServiceWse mArchiveService = null;

        bool mCached = false;
        bool mSelected = false;
        bool mDeleted = false;
        bool mDownloadInProgress = false;
        #endregion

        #region Setup / Initialization
        public VoiceMailEntry()
        {
            InitializeComponent();
            Dock = DockStyle.Top;
            mFileInfo = new InfoFile();
        }

        public VoiceMailEntry(WebVoiceMailControl owner, InfoFile info)
        {
            mOwner = owner;
            InitializeComponent();
            Dock = DockStyle.Top;
            this.progressBar.EndWaiting();
            this.progressBar.Enabled = false;
            this.progressBar.Visible = false;

            mFileInfo = info;
            lFrom.Text = mFileInfo.From;
            DateTime dateTime = new DateTime(1970, 1, 1);
            dateTime = dateTime.AddSeconds(mFileInfo.Time);
            lDate.Text = dateTime.ToShortDateString() + " " + dateTime.ToShortTimeString();
            tbTagIt.Text = mFileInfo.Tags;
            //Setup Color Scheme

            mCached = mFileInfo.Cached;
            if (mCached)
            {
                mFileInfo.FileName = Path.GetFileName(mFileInfo.FileName);               
            }

            this.Name = mFileInfo.FileName;
            this.toolTip.SetToolTip(this.lFrom, mFileInfo.From);
            this.toolTip.SetToolTip(this.pMailImage, mFileInfo.Location.ToString());
            ResetButtonDesign();
        }

        internal void ResetButtonDesign()
        {
            if (!mCached)
            {
                MailPictureImage = mOwner.MailNewInboxPicture == null ? Properties.Resources.listIconEmailNew : mOwner.MailNewInboxPicture;
            }
            else if (mFileInfo.Location == InfoFile.LocationType.ARCHIVE)
            {
                MailPictureImage = mOwner.MailArchivePicture == null ? Properties.Resources.listIconEmailOpen : mOwner.MailArchivePicture;
            }
            else
            {
                MailPictureImage = mOwner.MailInboxPicture == null ? Properties.Resources.listIconEmail : mOwner.MailInboxPicture;

            }

            if (mSelected)
            {
                BackColor = mOwner.SelectedMailBackgroundColor;
                BackgroundImage = mOwner.SelectedMailBackgroundImage;
                BackgroundImageLayout = mOwner.SelectedMailBackgroundImageLayout;
            }
            else
            {
                BackColor = mOwner.MailBackgroundColor;
                BackgroundImage = mOwner.MailBackgroundImage;
                BackgroundImageLayout = mOwner.MailBackgroundImageLayout;
            }
        }


        #endregion

        #region Download Mail Services

        public void DownloadMail()
        {
            if (!mCached)
            {
                mOwner.SetFileDownloadInProgress = true;

                this.progressBar.StartWaiting();
                this.progressBar.Enabled = true;
                this.progressBar.Visible = true;

                if (mDownloadService == null)
                {
                    mDownloadService = new MailService.ServiceWse();
                    mDownloadService.RequireMtom = true;
                    mDownloadService.VoiceMail_GetMessageCompleted += new MailService.VoiceMail_GetMessageCompletedEventHandler(mailService_VoiceMail_GetMessageCompleted);
                    mDownloadService.VoiceMail_GetMessageAsync(mOwner.UserName, mOwner.Password, mFileInfo.FileName, (MailService.LocationType)mFileInfo.Location);
                    mDownloadInProgress = true;
                   SelectControl();
                }
            }
        }

        void mailService_VoiceMail_GetMessageCompleted(object sender, MailService.VoiceMail_GetMessageCompletedEventArgs e)
        {
            if (e.Cancelled || e.Error != null || e.Result == null)
                return;

            this.Invoke(new DownloadCompleted(MailDownloadCompleted), new Object[] { e.Result });
        }

        delegate void DownloadCompleted(MailService.MessageReturn message);
        void MailDownloadCompleted(MailService.MessageReturn message)
        {
            if (message.Error == MailService.ReturnStatus.Success)
            {
                if (message.CompressedFormat == MailService.CompressionType.None)
                {
                    try
                    {
                        String newFileName = mOwner.BasePath + mFileInfo.Location.ToString() + @"\" + mFileInfo.FileName;
                        String newFileInfoName = newFileName + ".xml";
                        String tempFileName = Path.GetTempFileName();
                        String tempFileInfoName = Path.GetTempFileName();

                        File.WriteAllBytes(tempFileName, message.Bytes);

                        XmlSerializer ser = new XmlSerializer(typeof(InfoFile));
                        FileStream fs = File.OpenWrite(tempFileInfoName);
                        ser.Serialize(fs, mFileInfo);
                        fs.Close();

                        //Now move files
                        if (File.Exists(newFileName)) File.Delete(newFileName);
                        File.Move(tempFileName, newFileName);
                        if (File.Exists(newFileInfoName)) File.Delete(newFileInfoName);
                        File.Move(tempFileInfoName, newFileInfoName);

                        mCached = true;
                        ResetButtonDesign();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("MailDownloadCompleted :" + ex.Message);
                        //todo, maybe inform user??
                    }
                }
            }
            //Remove the service from ourself
            mDownloadService.VoiceMail_GetMessageCompleted -= new MailService.VoiceMail_GetMessageCompletedEventHandler(mailService_VoiceMail_GetMessageCompleted);
            mDownloadService = null;
            this.progressBar.Enabled = false;
            this.progressBar.Visible = false;
            mDownloadInProgress = false;
            if (Selected) { 
                mOwner.SetFileDownloadInProgress = false;
                SelectControl();
            }
            
        }

        #endregion

        #region Properties
        public string FullPath
        {
            get { return Path.Combine(Path.Combine(mOwner.BasePath, Enum.GetName(typeof(InfoFile.LocationType), mFileInfo.Location)), mFileInfo.FileName); }
        }
        public InfoFile Info
        {
            get { return mFileInfo; }
        }
        public Image MailPictureImage
        {
            set { pMailImage.Image = value; }
            get { return pMailImage.Image; }
        }
        public bool Cached
        {
            get { return mCached; }
        }
        public void SelectControl()
        {
            CancelEventArgs arg = new CancelEventArgs(false);
            mOwner.OnSelectedMailEntry(this, arg);
            if (arg.Cancel == false)
            {
                mSelected = true;
            }
        }
        public bool Selected
        {
            set
            {
                mSelected = value;


            }
            get { return mSelected; }
        }

        public bool DownloadInProgress
        {
            set
            {
                mDownloadInProgress = value;


            }
            get { return mDownloadInProgress; }
        }

        #endregion

        #region Form Events
        public void ArchiveLocal()
        {
            //Move local to archive if cached
            if (mCached && mFileInfo.Location == InfoFile.LocationType.INBOX)
            {
                File.Move(FullPath, mOwner.BasePath + @"ARCHIVE\" + mFileInfo.FileName);
                File.Move(FullPath + ".xml", mOwner.BasePath + @"ARCHIVE\" + mFileInfo.FileName + ".xml");
            }

            //Update location setting
            mFileInfo.Location = InfoFile.LocationType.ARCHIVE;
            this.toolTip.SetToolTip(this.pMailImage, mFileInfo.Location.ToString());

            ResetButtonDesign();
        }

        private void bVoiceMailEntry_Click(object sender, EventArgs e)
        {
            CancelEventArgs arg = new CancelEventArgs(false);
            mOwner.OnSelectedMailEntry(this, arg);
            if (arg.Cancel == false)
            {
                mSelected = true;
            }
        }
        #endregion

        #region Public Methods

        public void DeleteVoiceMail()
        {
            //Delete locally
            mDeleted = true;
            try
            {
                if(File.Exists(FullPath)) File.Delete(FullPath);
                if(File.Exists(FullPath + ".xml")) File.Delete(FullPath + ".xml");
            }
            catch
            {
            }

            //Delete Remotely
            this.Enabled = false;
            if (mDeleteService == null)
                mDeleteService = new MailService.ServiceWse();
            mDeleteService.RequireMtom = true;
            mDeleteService.VoiceMail_DeleteMessageAsync(mOwner.UserName, mOwner.Password, mFileInfo.FileName, (MailService.LocationType)mFileInfo.Location);
            mDeleteService.VoiceMail_DeleteMessageCompleted += new Remwave.Client.MailService.VoiceMail_DeleteMessageCompletedEventHandler(mDeleteService_VoiceMail_DeleteMessageCompleted);

            mOwner.Purge(this);
        }

        void mDeleteService_VoiceMail_DeleteMessageCompleted(object sender, Remwave.Client.MailService.VoiceMail_DeleteMessageCompletedEventArgs e)
        {
            if (e.Error != null || e.Cancelled)
            {
                Console.WriteLine("DeleteMessageCompleted : Failed" + e.Result.ToString());
            }
            mDeleteService.VoiceMail_DeleteMessageCompleted -= new Remwave.Client.MailService.VoiceMail_DeleteMessageCompletedEventHandler(mDeleteService_VoiceMail_DeleteMessageCompleted);
        }

        public void PlayVoiceMail()
        {
            if (mCached == false)
            {   //Start Download
                if (mOwner.FileDownloadInProgress == false)
                {
                    mOwner.SetFileDownloadInProgress = true;
                    DownloadMail();
                }
            }
            else
            {
                //Move to archive if needed
                try
                {
                    if (mFileInfo.Location == InfoFile.LocationType.INBOX)
                    {
                        ArchiveLocal();

                        //Move to remote archive
                        if (mArchiveService == null)
                            mArchiveService = new MailService.ServiceWse();
                        mDownloadService.RequireMtom = true;
                        //Do and forget
                        mArchiveService.VoiceMail_ArchiveMessageAsync(mOwner.UserName, mOwner.Password, mFileInfo.FileName);
                    }
                }
                catch
                {
                }
            }
        }
        #endregion

        #region Private methods
        void SaveFileInfo()
        {
            if (mDeleted) return;
            try
            {
                String newFileInfoName = mOwner.BasePath + mFileInfo.Location.ToString() + @"\" + mFileInfo.FileName + ".xml"; ;
                String tempFileInfoName = Path.GetTempFileName();
                XmlSerializer ser = new XmlSerializer(typeof(InfoFile));
                FileStream fs = File.OpenWrite(tempFileInfoName);
                ser.Serialize(fs, mFileInfo);
                fs.Close();
                //Now move file
                if (File.Exists(newFileInfoName)) File.Delete(newFileInfoName);
                if (File.Exists(tempFileInfoName)) File.Move(tempFileInfoName, newFileInfoName);
            }
            catch (Exception ex)
            {
                Console.WriteLine("VoiceMail Entry SaveFileInfo :" + ex.Message);
            }
        }
        #endregion

       
        private void tbTagIt_Leave(object sender, EventArgs e)
        {
            mFileInfo.Tags = tbTagIt.Text;
            SaveFileInfo();
        }
}
}
