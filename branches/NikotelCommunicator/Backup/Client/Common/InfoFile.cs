using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Xml.Serialization;
using System.Collections;

namespace Remwave.Client
{
    // Create an IComparer for Inventory objects. 
    class InfoFileComparer : IComparer
    {
        // Implement the IComparable interface. 
        public int Compare(Object obj1, Object obj2)
        {

            InfoFile info1;
            InfoFile info2;

            if (obj1 is InfoFile)
                info1 = obj1 as InfoFile;
            else
                throw new ArgumentException("Object is not of type InfoFile.");

            if (obj2 is InfoFile)
                info2 = obj2 as InfoFile;
            else
                throw new ArgumentException("Object is not of type InfoFile.");

            return Math.Sign(info1.Time - info2.Time);

        }
    }


    /// <summary>
    /// Helper class used to load message configuration file
    /// </summary>
    [XmlRoot("InfoFile")]
    public class InfoFile
    {
        public enum LocationType
        {

            /// <remarks/>
            ARCHIVE,

            /// <remarks/>
            INBOX,
        }

        [XmlElement("Time")]
        public double Time = 0.0;//UTC (double precision)
        [XmlElement("Day")]
        public string Day = "";

        [XmlElement("Month")] //1-12
        public string Month = "";

        [XmlElement("Year")] //0000
        public string Year = "";

        [XmlElement("From")]// reciver from
        public string From = "";

        [XmlElement("Dead")]//expiration time stamp
        public string Dead = "";

        [XmlElement("Mailed")]//forwarded
        public bool Mailed = false;

        [XmlElement("Cached")]//downloaded localy
        public bool Cached = false;

        [XmlElement("FileName")]//filename
        public string FileName = "";

        [XmlElement("FileSize")]//filesize
        public long FileSize = 0;

        [XmlElement("Location")]//Location
        public LocationType Location = LocationType.INBOX;

        [XmlElement("Tags")]//Tags
        public string Tags = "";

        public InfoFile()
        {

        }

        public InfoFile(Remwave.Client.MailService.FileInfo info)
        {
            Time = info.Time;
            Day = info.Day;
            Month = info.Month;
            Year = info.Year;
            From = info.From;
            Tags = info.Tags;
            FileName = info.FileName;
            FileSize = info.FileSize;
        }

        #region IComparer Members




        #endregion

    }
}