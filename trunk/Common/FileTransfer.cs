using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Remwave.Services;
using System.ComponentModel;
using System.Threading;

namespace Remwave.Client
{
    class FileTransferInfo
    {
        public String ID;
        public String Size;
        public String Filename;

        public FileTransferInfo(String id, String filename, String size)
        {
            this.ID = id;
            this.Filename = filename;
            this.Size = size;
        }
    }

    public class SharedFile
    {
        public String FileName;
        public String LocalFileName;
        public String FromJID;
        public String ID;
        public String ToJID;
        public long Size;
        public long Offset;
        public Int32 ChunkSize;
        public Int32 ErrorCount;
        public DateTime TimeMarker;
        public Int32 Progress;

        internal string GetSizeFormated()
        {
            return FileTransfer.FormatFileSize(this.Size);
        }
    }

    class FileTransfer
    {
        #region Private Properties
        private long mPreferredTransferDuration = 1500;
        private Int32 mPreferredChunkSize = 32 * 1024;
        private Int32 mMaxChunkSize = 4 * 1024 * 1024;
        private Int32 mMinChunkSize = 4 * 1024;
        private BackgroundWorker mUploadBackgroundWorker;
        private BackgroundWorker mDownloadBackgroundWorker;
        private SharedFile mUploadFile;
        private SharedFile mDownloadFile;

        #endregion

        #region Helper Classes

        
        public static string FormatFileSize(long size)
        {

            if (size >= 1024 * 1024 * 1024)
            {

                return string.Format("{0:########0.##} GB", (size) / (1024 * 1024 * 1024));

            }

            else if (size >= 1024 * 1024)
            {

                return string.Format("{0:####0.##} MB", (size) / (1024 * 1024));

            }

            else if (size >= 1024)
            {

                return string.Format("{0:####0.##} KB", (size) / 1024);

            }

            else
            {

                return string.Format("{0} bytes", size);

            }

        }
        protected int OptimizeChunkSize(long currentChunkSize, DateTime transferStartTime)
        {

            double transferTime = DateTime.Now.Subtract(transferStartTime).TotalMilliseconds;
            double averageBytesPerMilliSec = currentChunkSize / transferTime;
            double optimizedChunkSize = averageBytesPerMilliSec * this.mPreferredTransferDuration;
            return (int)Math.Min(this.mMaxChunkSize, Math.Max(this.mMinChunkSize, optimizedChunkSize));

        }
        #endregion

        #region Public Events


        public event EventHandler UploadCompleted;
        internal void OnUploadCompleted(SharedFile sender, EventArgs args)
        {
            if (UploadCompleted != null)
                UploadCompleted(sender, new EventArgs());
        }
        public event EventHandler DownloadCompleted;
        internal void OnDownloadCompleted(SharedFile sender, EventArgs args)
        {
            if (DownloadCompleted != null)
                DownloadCompleted(sender, new EventArgs());
        }

        public event EventHandler UploadProgressChanged;
        internal void OnUploadProgressChanged(SharedFile sender, EventArgs args)
        {
            if (UploadProgressChanged != null)
                UploadProgressChanged(sender, new EventArgs());
        }

        public event EventHandler DownloadProgressChanged;
        internal void OnDownloadProgressChanged(SharedFile sender, EventArgs args)
        {
            if (DownloadProgressChanged != null)
                DownloadProgressChanged(sender, new EventArgs());
        }
        #endregion

        #region Public Methods


        public SharedFile Upload(String filename, String username, String password, String toJID, String id)
        {
            try
            {
                mUploadFile = new SharedFile();
                mUploadFile.FileName = Path.GetFileName(filename);
                mUploadFile.LocalFileName = filename;
                mUploadFile.FromJID = username;
                mUploadFile.ToJID = toJID;
                mUploadFile.ID = id;
                mUploadFile.ChunkSize = 0;


                FileTransferWS.ServiceWse serviceWse = new Remwave.Client.FileTransferWS.ServiceWse();
                serviceWse.RequireMtom = true;
                serviceWse.PutFileSize(username, password, id, new FileInfo(mUploadFile.LocalFileName).Length);

                mUploadBackgroundWorker = new BackgroundWorker();
                mUploadBackgroundWorker.WorkerReportsProgress = true;
                mUploadBackgroundWorker.DoWork += new DoWorkEventHandler(mUploadBackgroundWorker_DoWork);
                mUploadBackgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(mUploadBackgroundWorker_RunWorkerCompleted);
                mUploadBackgroundWorker.ProgressChanged += new ProgressChangedEventHandler(mUploadBackgroundWorker_ProgressChanged);
                mUploadBackgroundWorker.RunWorkerAsync();
            }
            catch (Exception)
            {

            }
            return mUploadFile;
        }

        public SharedFile Download(String filename, String username, String password, String fromJID, String id)
        {
            try
            {
                mDownloadFile = new SharedFile();
                mDownloadFile.FileName = Path.GetFileName(filename);
                mDownloadFile.FromJID = fromJID;
                mDownloadFile.ToJID = username;
                mDownloadFile.ID = id;
                mDownloadFile.LocalFileName = Path.GetTempFileName();

                FileTransferWS.ServiceWse serviceWse = new Remwave.Client.FileTransferWS.ServiceWse();
                serviceWse.RequireMtom = true;

                mDownloadFile.Size = serviceWse.GetFileSize(username, password, id);

                mDownloadBackgroundWorker = new BackgroundWorker();
                mDownloadBackgroundWorker.WorkerReportsProgress = true;
                mDownloadBackgroundWorker.DoWork += new DoWorkEventHandler(mDownloadBackgroundWorker_DoWork);
                mDownloadBackgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(mDownloadBackgroundWorker_RunWorkerCompleted);
                mDownloadBackgroundWorker.ProgressChanged += new ProgressChangedEventHandler(mDownloadBackgroundWorker_ProgressChanged);
                mDownloadBackgroundWorker.RunWorkerAsync();
            }
            catch (Exception)
            {

            }
            return mDownloadFile;
        }

        #endregion


        void mUploadBackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            OnUploadProgressChanged(mUploadFile, new EventArgs());
        }

        void mUploadBackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            OnUploadCompleted(mUploadFile, new EventArgs());
        }

        void mUploadBackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            UploadPartialDo();
        }

        void UploadPartialDo()
        {
            FileTransferWS.ServiceWse serviceWse = new Remwave.Client.FileTransferWS.ServiceWse();
            serviceWse.RequireMtom = true;
            mUploadFile.ChunkSize = mPreferredChunkSize;
            using (FileStream stream = new FileStream(mUploadFile.LocalFileName, FileMode.Open))
            {
                mUploadFile.Size = stream.Length;
                while (mUploadFile.Size > mUploadFile.Offset && mUploadFile.ErrorCount < 60)
                {
                    byte[] buffer = new byte[mUploadFile.ChunkSize];

                    stream.Position = mUploadFile.Offset;
                    mUploadFile.ChunkSize = stream.Read(buffer, 0, buffer.Length);
                    Array.Resize(ref buffer, mUploadFile.ChunkSize);

                    try
                    {
                        mUploadFile.TimeMarker = DateTime.Now;
                        serviceWse.PutFileChunk(mUploadFile.FromJID, "", mUploadFile.ID, buffer, mUploadFile.Offset);
                        mUploadFile.Offset += mUploadFile.ChunkSize;
                        mUploadFile.ChunkSize = OptimizeChunkSize(mUploadFile.ChunkSize, mUploadFile.TimeMarker);
                    }
                    catch (Exception)
                    {
                        Thread.Sleep(5000);
                        mUploadFile.ErrorCount++;
                    }
                    this.mUploadFile.Progress = 100*Convert.ToInt32(mUploadFile.Offset / mUploadFile.Size);
                    mUploadBackgroundWorker.ReportProgress(this.mUploadFile.Progress);
                }

            }
        }


        void mDownloadBackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            OnDownloadProgressChanged(mDownloadFile, new EventArgs());
        }

        void mDownloadBackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            OnDownloadCompleted(mDownloadFile, new EventArgs());
        }

        void mDownloadBackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            DownloadPartialDo();
        }

        void DownloadPartialDo()
        {
            FileTransferWS.ServiceWse serviceWse = new Remwave.Client.FileTransferWS.ServiceWse();
            serviceWse.RequireMtom = true;
            mDownloadFile.ChunkSize = mPreferredChunkSize;
            using (FileStream stream = new FileStream(mDownloadFile.LocalFileName, FileMode.Create))
            {
                while (mDownloadFile.Size > mDownloadFile.Offset && mDownloadFile.ErrorCount < 60)
                {

                    if (mDownloadFile.Offset + mDownloadFile.ChunkSize > mDownloadFile.Size)
                    {
                        mDownloadFile.ChunkSize = Convert.ToInt32(mDownloadFile.Size - mDownloadFile.Offset);
                    }

                    byte[] buffer = new byte[mDownloadFile.ChunkSize];
                    try
                    {
                        stream.Position = mDownloadFile.Offset;

                        mDownloadFile.TimeMarker = DateTime.Now;
                        buffer = serviceWse.GetFileChunk(mDownloadFile.FromJID, "", mDownloadFile.ID, mDownloadFile.Offset, mDownloadFile.ChunkSize);
                        mDownloadFile.Offset += buffer.Length; //we might download less data than we want
                        mDownloadFile.ChunkSize = OptimizeChunkSize(buffer.Length, mDownloadFile.TimeMarker);

                        stream.Write(buffer, 0, buffer.Length);
                    }
                    catch (Exception)
                    {
                        Thread.Sleep(5000);
                        mDownloadFile.ErrorCount++;
                    }
                    this.mDownloadFile.Progress = 100*Convert.ToInt32( mDownloadFile.Offset / mDownloadFile.Size);
                    mDownloadBackgroundWorker.ReportProgress(this.mDownloadFile.Progress);
                }
            }
        }
    }
}
