using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Remwave.Nikotalkie
{
    [XmlRoot("StorageIndex")]
    public class NStorageIndex
    {
        [XmlElement("Changed")]
        public bool Changed = false;
        [XmlElement("Messages")]
        public List<NMessage> Messages = new List<NMessage>();
        [XmlElement("LastMessageId")]
        public String LastMessageId = "";
        [XmlElement("Path")]
        public String Path = "";
        public bool ExistsFile(String localFileName)
        {
            for (int i = 0; i < Messages.Count; i++)
            {
                if (this.Messages[i].LocalFileName == localFileName) return true;
            }
            return false;
        }
        public bool ExistsID(String ID)
        {
            for (int i = 0; i < Messages.Count; i++)
            {
                if (this.Messages[i].Header.MsgID == ID) return true;
            }
            return false;
        }
        public NMessage GetMessage(String localFileName)
        {
            for (int i = 0; i < Messages.Count; i++)
            {
                if (this.Messages[i].LocalFileName == localFileName) return this.Messages[i];
            }
            return null;
        }
        public NStorageIndex()
        {
          
        }
        public NStorageIndex(String path)
        {
            this.Path = path;
        }

        public void SortIndex()
        {
            NMessageComparer comparer = new NMessageComparer();
            Messages.Sort(comparer);
        }

        public event EventHandler IndexChanged;
        internal void OnIndexChanged(object sender, EventArgs args)
        {
            if (IndexChanged != null)
            {
                IndexChanged(sender, args);
            }
        }
    }
}
