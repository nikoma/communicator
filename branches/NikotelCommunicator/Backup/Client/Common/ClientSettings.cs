using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.IO;


namespace Remwave.Client
{
    [XmlRoot("AudioCodec")]
    public class AudioCodec
    {
        public enum AudioCodecFormat
        {
            G729 = 2,
            iLBC_30Ms = 5,
            uLaw_8k = 0,
            aLaw_8k = 1,
            undefined =9
        }

        public AudioCodec()
        {

        }

        public AudioCodec(AudioCodecFormat format, String name)
        {
            this.Format = format;
            this.Name = name;
        }

        public override string ToString()
        {
            return Name;
        }

        [XmlElement("Format")]
        public AudioCodecFormat Format;
        [XmlElement("Name")]
        public String Name;
    }


    [XmlRoot("Settings")]
    public class ClientSettings
    {
        //Constructor
        public ClientSettings()
        {
            PhoneAvailableMediaFormats = new List<AudioCodec>();
            PhoneAvailableMediaFormats.Add(new AudioCodec(AudioCodec.AudioCodecFormat.G729, "G729"));
            PhoneAvailableMediaFormats.Add(new AudioCodec(AudioCodec.AudioCodecFormat.iLBC_30Ms, "iLBC 30Ms"));
            PhoneAvailableMediaFormats.Add(new AudioCodec(AudioCodec.AudioCodecFormat.uLaw_8k, "uLaw 8k"));
            PhoneAvailableMediaFormats.Add(new AudioCodec(AudioCodec.AudioCodecFormat.aLaw_8k, "aLaw 8k"));
        }

        [XmlElement("ProgramAtStartup")]
        public Boolean ProgramAtStartup = true;
        [XmlElement("ProgramConfirmClosing")]
        public Boolean ProgramConfirmClosing = true;
        [XmlElement("ProgramEnableQualityAgent")]
        public Boolean ProgramEnableQualityAgent = true;
        [XmlElement("ProgramLanguage")]
        public String ProgramLanguage = "";

        [XmlElement("ContactsEnableOutlookStore")]
        public Boolean ContactsEnableOutlookStore = false;
        [XmlElement("ContactsEnableServerStore")]
        public Boolean ContactsEnableServerStore = true;

        [XmlElement("PhoneAudioMicrophoneVolume")]
        public Int32 PhoneAudioMicrophoneVolume = 512;
        [XmlElement("PhoneAudioSpeakerVolume")]
        public Int32 PhoneAudioSpeakerVolume = 512;
        [XmlElement("PhoneEnabledMediaFormat")]
        public List<AudioCodec> PhoneEnabledMediaFormats;

        [XmlIgnore]
        public readonly List<AudioCodec> PhoneAvailableMediaFormats;

        [XmlElement("PhoneCustomRingTone")]
        public String PhoneCustomRingTone;
        [XmlElement("PhoneDefaultRingToneEnabled")]
        public Boolean PhoneDefaultRingToneEnabled = true;
        [XmlElement("PhoneEnableSIPDiagnostic")]
        public Boolean PhoneEnableSIPDiagnostic = true;       
    }


    public class ClientSettingsSerializer {

        //Properties
        private string mPath;        
        private String mFileName = @"\Settings.xml";

        public ClientSettingsSerializer(String path)
        {
            mPath = path;
        }
        
        //Public Methods
        public ClientSettings Load()
        {
            ClientSettings clientSettings = new ClientSettings();
            try
            {
                using (StreamReader sr = new StreamReader(mPath + mFileName))
                {
                    if (File.Exists(mPath + mFileName))
                    {
                        XmlSerializer des = new XmlSerializer(typeof(ClientSettings));
                        clientSettings = (ClientSettings)des.Deserialize(new System.Xml.XmlTextReader(sr));
                        sr.Close();
                    }
                }
            }
            catch (Exception)
            {
                if (File.Exists(mPath + mFileName))
                {

#if (DEBUG)
                    throw;
#else
                    File.Delete(mPath + mFileName);
#endif

                }
            }
            if (clientSettings.PhoneEnabledMediaFormats == null || clientSettings.PhoneEnabledMediaFormats.Count == 0)
            {
                clientSettings.PhoneEnabledMediaFormats = new List<AudioCodec>();
                clientSettings.PhoneEnabledMediaFormats.Add(new AudioCodec(AudioCodec.AudioCodecFormat.G729, "G729"));
                clientSettings.PhoneEnabledMediaFormats.Add(new AudioCodec(AudioCodec.AudioCodecFormat.iLBC_30Ms, "iLBC 30Ms"));
                clientSettings.PhoneEnabledMediaFormats.Add(new AudioCodec(AudioCodec.AudioCodecFormat.uLaw_8k, "uLaw 8k"));
                clientSettings.PhoneEnabledMediaFormats.Add(new AudioCodec(AudioCodec.AudioCodecFormat.aLaw_8k, "aLaw 8k"));
            }

            

            return clientSettings;
         
        }

        public void Save(ClientSettings clientSettings)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(mPath + mFileName))
                {
                    XmlSerializer ser = new XmlSerializer(typeof(ClientSettings));
                    ser.Serialize(sw, clientSettings);
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
