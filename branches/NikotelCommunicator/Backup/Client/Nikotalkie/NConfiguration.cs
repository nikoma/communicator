using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.IO;

namespace Remwave.Nikotalkie
{
    [XmlRoot("Configuration")]
    public class Configuration
    {

        //Properties
        private String Path;
        private String FileName = @"\Configuration.xml";
        [XmlElement("Username")]
        public String Username;
        [XmlElement("Password")]
        public String Password;
        [XmlElement("LastMessageID")]
        public String LastMessageID;
        [XmlElement("Url")]
        public String Url;

        public Configuration()
        {

        }
        //Constructor
        public Configuration(String path)
        {
            this.Path = path;
        }

        //Public Methods
        public void Load()
        {
            Configuration tmpConfiguration = null;
            try
            {
                using (StreamReader sr = new StreamReader(Path + FileName))
                {
                    if (File.Exists(Path + FileName))
                    {
                        XmlSerializer des = new XmlSerializer(typeof(Configuration));
                        tmpConfiguration = (Configuration)des.Deserialize(new System.Xml.XmlTextReader(sr));
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
            if (tmpConfiguration != null)
            {
                this.Username = tmpConfiguration.Username;
                this.Password = tmpConfiguration.Password;
            }
        }

        public void Save()
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(Path + FileName))
                {
                    XmlSerializer ser = new XmlSerializer(typeof(Configuration));
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
