using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.IO;

namespace Remwave.Nikotalkie
{
    [XmlRoot("Settings")]
    public class NSettings
    {

        //Properties
        private String Path;
        private String FileName = @"\Settings.xml";
        [XmlElement("StoragePath")]
        public String StoragePath;
        [XmlElement("SendReceiveInterval")]
        public Int32 SendReceiveInterval = 10 * 60; //seconds
        [XmlElement("LocalIP")]
        public String LocalIP = "0.0.0.0";
        [XmlElement("LocalIP")]
        public Boolean IsConnected = false;

        public NSettings()
        {


        }
        //Constructor
        public NSettings(String path)
        {
            this.Path = path;
        }

        //Public Methods
        public void Load()
        {
            NSettings tmpSettings = null; ;
            try
            {
                using (StreamReader sr = new StreamReader(Path + FileName))
                {
                    if (File.Exists(Path + FileName))
                    {
                        XmlSerializer des = new XmlSerializer(typeof(NSettings));
                        tmpSettings = (NSettings)des.Deserialize(new System.Xml.XmlTextReader(sr));
                        sr.Close();
                    }
                }
            }
            catch (Exception)
            {
                if (File.Exists(Path + FileName))
                {

#if (DEBUG)
                    throw;
#else
 File.Delete(Path + FileName);
#endif

                }
            }
            if (tmpSettings != null)
            {
                this.StoragePath = tmpSettings.StoragePath;
            }
        }

        public void Save()
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(Path + FileName))
                {
                    XmlSerializer ser = new XmlSerializer(typeof(NSettings));
                    ser.Serialize(sw, this);
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
    }
}
