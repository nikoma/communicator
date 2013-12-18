using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.IO;

namespace Remwave.Nikotalkie
{
    public enum NStorageFolder
    {
        Inbox, Archive, Outbox, Sent
    }

    public class NStorage
    {

        #region Properties
        private String _rootPath;
        public String RootPath
        {
            get { return _rootPath; }
            set
            {
                _rootPath = value;
                this.CreateDirectories();
            }
        }

        private String _inboxPath;
        public String InboxPath
        {
            get { return _inboxPath; }
        }

        public NStorageIndex InboxIndex;
        private String _archivePath;
        public String ArchivePath
        {
            get { return _archivePath; }
        }

        public NStorageIndex ArchiveIndex;
        private String _outboxPath;
        public String OutboxPath
        {
            get { return _outboxPath; }
        }

        public NStorageIndex OutboxIndex;
        private String _sentPath;
        public String SentPath
        {
            get { return _sentPath; }
        }

        public NStorageIndex SentIndex;
        #endregion
        public NStorage()
        {

        }

        public NStorage(String rootPath)
        {
            this.InitializeStorage(rootPath);
        }

        #region Helper Methods
        private string GenerateId()
        {
            long i = 1;
            foreach (byte b in Guid.NewGuid().ToByteArray())
            {
                i *= ((int)b + 1);
            }
            return string.Format("{0:x}", i - DateTime.Now.Ticks);
        }
        private static byte[] ReadAllBytes(String FileName)
        {

            FileInfo fileInfo = new FileInfo(FileName);
            if (!fileInfo.Exists) return null;

            int initialLength = (Int32)fileInfo.Length;

            FileStream stream = new FileStream(FileName, FileMode.Open);
            // If we've been passed an unhelpful initial length, just
            // use 32K.
            if (initialLength < 1)
            {
                initialLength = 32768;
            }

            byte[] buffer = new byte[initialLength];
            int read = 0;

            int chunk;
            while ((chunk = stream.Read(buffer, read, buffer.Length - read)) > 0)
            {
                read += chunk;

                // If we've reached the end of our buffer, check to see if there's
                // any more information
                if (read == buffer.Length)
                {
                    int nextByte = stream.ReadByte();

                    // End of stream? If so, we're done
                    if (nextByte == -1)
                    {
                        return buffer;
                    }

                    // Nope. Resize the buffer, put in the byte we've just
                    // read, and continue
                    byte[] newBuffer = new byte[buffer.Length * 2];
                    Array.Copy(buffer, newBuffer, buffer.Length);
                    newBuffer[read] = (byte)nextByte;
                    buffer = newBuffer;
                    read++;
                }
            }
            // Buffer is now too big. Shrink it.
            byte[] ret = new byte[read];
            Array.Copy(buffer, ret, read);
            return ret;
        }
        private void CreateDirectories()
        {
            if (!Directory.Exists(this._rootPath)) Directory.CreateDirectory(_rootPath);
            if (!Directory.Exists(this._inboxPath)) Directory.CreateDirectory(this._inboxPath);
            if (!Directory.Exists(this._archivePath)) Directory.CreateDirectory(this._archivePath);
            if (!Directory.Exists(this._outboxPath)) Directory.CreateDirectory(this._outboxPath);
            if (!Directory.Exists(this._sentPath)) Directory.CreateDirectory(this._sentPath);
        }
        #endregion
        #region Storage Index Methods


        public void UpdateIndex(NStorageFolder folder)
        {
            NStorageIndex storageIndex = null;
            String storagePath = null;
            switch (folder)
            {
                case NStorageFolder.Inbox:
                    storageIndex = InboxIndex;
                    break;
                case NStorageFolder.Archive:
                    storageIndex = ArchiveIndex;
                    break;
                case NStorageFolder.Outbox:
                    storageIndex = OutboxIndex;
                    break;
                case NStorageFolder.Sent:
                    storageIndex = SentIndex;
                    break;
                default:
                    break;
            }
            if (storageIndex == null || !Directory.Exists(storageIndex.Path)) return;

            String[] files = Directory.GetFiles(storageIndex.Path, "*.xml");

            //remove deleted
            
            for (int i = storageIndex.Messages.Count - 1; i >= 0; i--)
            {
                bool exists = false;
                for (int j = 0; j < files.Length; j++)
                {
                    if (files[j] == storageIndex.Messages[i].LocalFileName) exists = true;
                }
                if (!exists)
                {
                    storageIndex.Messages.RemoveAt(i);
                }
            }
            //add new
            for (int i = 0; i < files.Length; i++)
            {

                if (!storageIndex.ExistsFile(files[i]))
                {
                    NMessage message = new NMessage();
                    using (StreamReader sr = new StreamReader(files[i]))
                    {
                        if (File.Exists(files[i]))
                        {
                            XmlSerializer des = new XmlSerializer(typeof(NMessage));
                            message = (NMessage)des.Deserialize(new System.Xml.XmlTextReader(sr));
                            sr.Close();
                        }
                    }

                    if (message != null)
                    {
                        message.Body = null;
                        storageIndex.Messages.Add(message);
                        storageIndex.LastMessageId = message.Header.MsgID;
                    }

                }
            }

            //save index
            try
            {
                using (StreamWriter sw = new StreamWriter(storagePath + ".index"))
                {
                    XmlSerializer ser = new XmlSerializer(typeof(NStorageIndex));
                    ser.Serialize(sw, storageIndex);
                    sw.Close();
                }

            }
            catch (Exception ex)
            {
#if (DEBUG)
                        throw;
#endif
            }

            storageIndex.SortIndex();
            storageIndex.OnIndexChanged(storageIndex, new EventArgs());
        }
        private NStorageIndex OpenIndex(String storagePath)
        {

            NStorageIndex storageIndex = null;
            String storageIndexFile = storagePath + ".index";
            if (File.Exists(storageIndexFile))
            {
                try
                {
                    using (StreamReader sr = new StreamReader(storageIndexFile))
                    {
                        XmlSerializer des = new XmlSerializer(typeof(NStorageIndex));
                        storageIndex = (NStorageIndex)des.Deserialize(new System.Xml.XmlTextReader(sr));
                        storageIndex.Path = storagePath;
                        sr.Close();
                    }
                }
                catch (Exception)
                {
                    //index is corrupt get rid of it
                    File.Delete(storageIndexFile);
#if (DEBUG)
                throw;
#endif
                }
            }

            return storageIndex;
        }
        #endregion

        public void InitializeStorage(String rootPath)
        {
            this._rootPath = rootPath;
            this._inboxPath = rootPath + @"\Inbox\";
            this._archivePath = rootPath + @"\Archive\";
            this._outboxPath = rootPath + @"\Outbox\";
            this._sentPath = rootPath + @"\Sent\";
            this.CreateDirectories();

            InboxIndex = OpenIndex(InboxPath);
            if (InboxIndex == null)
            {
                InboxIndex = new NStorageIndex(InboxPath);
                UpdateIndex(NStorageFolder.Inbox);
            }
            ArchiveIndex = OpenIndex(ArchivePath);
            if (ArchiveIndex == null)
            {
                ArchiveIndex = new NStorageIndex(ArchivePath);
                UpdateIndex(NStorageFolder.Archive);
            }
            OutboxIndex = OpenIndex(OutboxPath);
            if (OutboxIndex == null)
            {
                OutboxIndex = new NStorageIndex(OutboxPath);
                UpdateIndex(NStorageFolder.Outbox);
            }
            SentIndex = OpenIndex(SentPath);
            if (SentIndex == null)
            {
                SentIndex = new NStorageIndex(SentPath);
                UpdateIndex(NStorageFolder.Sent);
            }
        }

        public void SaveOutbox(NMessage message)
        {
            try
            {
                String fileName = OutboxPath + GenerateId() + ".xml";
                using (StreamWriter sw = new StreamWriter(fileName))
                {
                    XmlSerializer ser = new XmlSerializer(typeof(NMessage));
                    ser.Serialize(sw, message);
                    sw.Close();
                }
                OutboxIndex.LastMessageId = message.Header.MsgID;
            }
            catch (Exception)
            {
#if (DEBUG)
                throw;
#endif
            }
        }

        public void SaveInbox(NMessage message)
        {
            try
            {
                String fileName = InboxPath + GenerateId() + ".xml";
                message.LocalFileName = fileName;
                using (StreamWriter sw = new StreamWriter(fileName))
                {
                    XmlSerializer ser = new XmlSerializer(typeof(NMessage));
                    ser.Serialize(sw, message);
                    sw.Close();
                }
                InboxIndex.LastMessageId = message.Header.MsgID;
            }
            catch (Exception)
            {
#if (DEBUG)
                throw;
#endif
            }

        }

        public void DeleteMessage(NMessage message)
        {
            NMessage tmpMessage = this.GetMessage(message);
            if (tmpMessage != null)
            {
                tmpMessage.Deleted = true;
                try
                {
                    if (File.Exists(message.LocalFileName)) File.Delete(message.LocalFileName);
                    String fileName = ArchivePath + GenerateId() + ".xml";
                    tmpMessage.LocalFileName = fileName;
                    using (StreamWriter sw = new StreamWriter(fileName))
                    {
                        XmlSerializer ser = new XmlSerializer(typeof(NMessage));
                        ser.Serialize(sw, tmpMessage);
                        sw.Close();
                    }

                }
                catch (Exception)
                {
#if (DEBUG)
                throw;
#endif
                }

            }
            this.UpdateIndex(NStorageFolder.Inbox);
            this.UpdateIndex(NStorageFolder.Archive);


        }

        public void MoveMessage(NStorageFolder destination, NMessage message)
        {

        }

        internal NMessage GetMessage(NMessage message)
        {
            NMessage tmpMessage = null;
            try
            {
                using (StreamReader sr = new StreamReader(message.LocalFileName))
                {
                    if (File.Exists(message.LocalFileName))
                    {
                        tmpMessage = new NMessage();
                        XmlSerializer des = new XmlSerializer(typeof(NMessage));
                        tmpMessage = (NMessage)des.Deserialize(new System.Xml.XmlTextReader(sr));
                        sr.Close();
                    }
                }
            }
            catch (Exception)
            {
#if(DEBUG)                  
                  throw;
#endif
            }
            return tmpMessage;
        }
    }
}
