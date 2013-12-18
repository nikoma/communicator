using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Collections;
using System.Threading;
using System.Xml.Serialization;
using System.ComponentModel;

namespace Remwave.Nikotalkie
{
    public class DeliveryWorker
    {
        private bool mCancel = false;

        private NSettings mSettings;
        private NStorage mStorage;
        private Configuration mConfiguration;
        public int NewMessages = 0;
        public DeliveryWorker(Configuration configuration, NSettings settings, NStorage storage, BackgroundWorker backgroundWorker)
        {
            try
            {
                this.NewMessages = 0;
                int i = 0;
                mConfiguration = configuration;
                mSettings = settings;
                mStorage = storage;
                Boolean updateIndex = false;
                NTransport mTransport = new NTransport(this.mConfiguration);

                backgroundWorker.ReportProgress(0);

                #region RECEIVING
                //Receiving Headers
                List<NMessageHeader> messageHeaders = mTransport.ReciveHeaders();

                if (messageHeaders != null && !mCancel)
                {
                    i = 0;
                    foreach (NMessageHeader messageHeader in messageHeaders)
                    {


                        if (!mStorage.InboxIndex.ExistsID(messageHeader.MsgID) && !mStorage.ArchiveIndex.ExistsID(messageHeader.MsgID)) 
                       {   //skip already downloaded
                            NMessage message = mTransport.ReciveMessage(messageHeader);
                            if(message!=null)  mStorage.SaveInbox(message);
                            updateIndex = true;
                            NewMessages++;
                       }
                        backgroundWorker.ReportProgress(100 * i / (messageHeaders.Count));
                      i++;
                      
                    }
                    
                }

                if (updateIndex)
                {
                    mStorage.UpdateIndex(NStorageFolder.Inbox);
                }
              
                #endregion

                #region SENDING
                //Sending
                updateIndex = false;
                string[] mFileList = Directory.GetFiles(mStorage.OutboxPath, "*.xml");
                i = 1;
                if (mFileList.Length > 0)
                {
                    backgroundWorker.ReportProgress(100 * i / (mFileList.Length + 1));
                    foreach (String file in mFileList)
                    {
                        if (mCancel) break;
                        NMessage message = new NMessage();
                        using (StreamReader sr = new StreamReader(file))
                        {
                            if (File.Exists(file))
                            {
                                XmlSerializer des = new XmlSerializer(typeof(NMessage));
                                message = (NMessage)des.Deserialize(new System.Xml.XmlTextReader(sr));
                                sr.Close();
                            }
                        }

                        if (message != null)
                        {
                            NResultSend resultSend = mTransport.SendMessage(message);
                            if (resultSend.Success)
                            {
                                if (File.Exists(storage.SentPath + file)) File.Delete(storage.SentPath + file);
                                File.Move(file, storage.SentPath + Path.GetFileName(file));
                                updateIndex = true;
                            };
                        }
                        i++;
                        backgroundWorker.ReportProgress(100 * i / (mFileList.Length + 1));
                    }
                    if (updateIndex)
                    {
                        mStorage.UpdateIndex(NStorageFolder.Outbox);
                        mStorage.UpdateIndex(NStorageFolder.Sent);
                    }
                }
                #endregion

            }
            catch (Exception)
            {

#if (DEBUG)
                throw;
#endif
            }
            backgroundWorker.ReportProgress(100);
        }
    }
}
