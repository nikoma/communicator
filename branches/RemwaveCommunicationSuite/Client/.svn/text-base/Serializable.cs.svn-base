using System;
using System.IO;
using System.Xml.Serialization;
using System.Text;
using System.Security.Cryptography;
using System.Windows.Forms;
using Remwave.Services;

namespace Remwave.Client.Serializable
{


    #region Client Configuration

    public class ClientConfigurationSerializer
    {
        private XmlSerializer serializer = new XmlSerializer(typeof(ClientConfiguration));
        private readonly string ClientConfigurationPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\" + Application.CompanyName + @"\" + Application.ProductName + @"\RCS-Client.dat";

        public ClientConfiguration LoadClientConfiguration()
        {
            ClientConfiguration tmpClientConfiguration = new ClientConfiguration();
            try
            {
                byte[] data = File.ReadAllBytes(ClientConfigurationPath);
                data = Unprotect(data);
                using (MemoryStream ms = new MemoryStream(data))
                {
                    tmpClientConfiguration = (ClientConfiguration)this.serializer.Deserialize(ms);
                }
                
            }
            catch (Exception)
            {

            }
            return tmpClientConfiguration;
        }

        public void SaveClientConfiguration(ClientConfiguration clientConfiguration)
        {
            byte[] data;

            try
            {
                MemoryStream xmlStream = new MemoryStream();
                this.serializer.Serialize(xmlStream, clientConfiguration);
                xmlStream.Position = 0;
                data = xmlStream.GetBuffer();
                data = Protect(data);
                File.WriteAllBytes(ClientConfigurationPath, data);
            }
            catch (Exception)
            {

            }
        }

        static byte[] s_aditionalEntropy = { 177, 99, 155, 106, 108, 212, 133, 115, 104, 242, 211, 245, 85, 98, 160, 139 };

        public static byte[] Protect(byte[] data)
        {
            byte[] enc;
            try
            {
                // Encrypt the data using DataProtectionScope.CurrentUser. The result can be decrypted
                //  only by the same current user.
                enc = ProtectedData.Protect(data, s_aditionalEntropy, DataProtectionScope.CurrentUser);
                return enc;
            }
            catch (CryptographicException e)
            {
                Console.WriteLine("ConfigurationProtection:Data was not encrypted. An error occurred.");
                Console.WriteLine(e.ToString());
                return null;
            }
        }

        public static byte[] Unprotect(byte[] data)
        {
            byte[] dec;

            try
            {
                //Decrypt the data using DataProtectionScope.CurrentUser.
                dec = ProtectedData.Unprotect(data, s_aditionalEntropy, DataProtectionScope.CurrentUser);
                return dec;
            }
            catch (CryptographicException e)
            {
                Console.WriteLine("ConfigurationProtection:Data was not decrypted. An error occurred.");
                Console.WriteLine(e.ToString());
                return null;
            }
        }
    }
    [XmlRoot("ClientConfiguration")]
    public class ClientConfiguration
    {
        [XmlElement("Username")]
        public String Username = "";

        [XmlElement("Password")]
        public String Password = "";

        [XmlElement("AutoLogin")]
        public bool AutoLogin = false;

        [XmlElement("RememberMe")]
        public bool RememberMe = false;

        [XmlElement("IMServerAddress")]
        public String IMServerAddress = "";

        [XmlElement("IMServerPort")]
        public int IMServerPort = 5222;

        [XmlElement("SIPProxyRealm")]
        public String SIPProxyRealm = "";
        
        [XmlElement("SIPProxyAddress")]
        public String SIPProxyAddress = "";

        [XmlElement("SIPProxyPort")]
        public int SIPProxyPort = 5060;

        [XmlElement("VideoProxyAddress")]
        public String VideoProxyAddress = "";

        [XmlElement("VideoProxyPort")]
        public int VideoProxyPort = 800;


        [XmlElement("RSIUrl")]
        public string RSIUrl = global::Remwave.Client.Properties.Settings.Default.Remwave_RSIFeaturesWS_Service;

        [XmlElement("LastNews")]
        public string LastNews = "1.0.0.0";


        public ClientConfiguration()
        {
        }
        

        public void SetLoginOptions(String username, String password, bool autoLogin, bool rememberMe)
        {
            this.Username = username;
            this.Password = password;
            this.AutoLogin = autoLogin;
            this.RememberMe = rememberMe;
        }


        public void SetSIPConfiguration(String proxyAddress, int proxyPort, String proxyRealm)
        {
            this.SIPProxyAddress = proxyAddress;
            this.SIPProxyPort = proxyPort;
            this.SIPProxyRealm = proxyRealm;
        }

        public void SetIMConfiguration(String serverAddress, int serverPort)
        {
            this.IMServerAddress = serverAddress;
            this.IMServerPort = serverPort;
        }

        public void SetVideoProxyConfiguration(String proxyAddress, int proxyPort)
        {
            this.VideoProxyAddress = proxyAddress;
            this.VideoProxyPort = proxyPort;
        }

        public void SetRSIUrl(String rsiUrl)
        {
            this.RSIUrl = rsiUrl;
        }

    }
    #endregion

    #region CallHistory
    public enum CallStatus
    {   
       CallDialed = 0,
        CallReceived,
        CallMissed
    }

    public class CallHistorySerializer
    {
        private XmlSerializer serializer = new XmlSerializer(typeof(CallHistory));

        public CallHistory LoadCallHistory(string username)
        {
            CallHistory tmpCallHistory = new CallHistory();
          
            try
            {
                 FileStream fs = new FileStream("CallHistory-" + username + ".xml", FileMode.OpenOrCreate);
               tmpCallHistory =  (CallHistory)this.serializer.Deserialize(fs);
               fs.Close();
            }
            catch (Exception)
            {
                
              
            }
            return tmpCallHistory;
        }

        public void SaveCallHistory(CallHistory callHistory, string username)
        {

           
            try
            {
                 FileStream fs  = new FileStream("CallHistory-" + username + ".xml", FileMode.Create);
                this.serializer.Serialize(fs, callHistory);

            }
            catch (Exception)
            {


            }
        }
    }
    
    [XmlRoot("CallHistory")]
    public class CallHistory
    {


        [XmlElement("Username")]
        public String Username;

        [XmlArray("Calls")]
        [XmlArrayItem("Call")]
        public CallRecord[] CallRecords;


        public CallHistory()
        {

        }

        public CallHistory(String username)
        {
            this.Username = username;
        }

    }

    public class CallRecord
    {
        [XmlAttribute("Id")]
        public int Id;

        [XmlElement("CallDateTime")]
        public DateTime CallDateTime;

        [XmlElement("CallState")]
        public CallStatus CallState;

        [XmlElement("NumberOrUsername")]
        public String NumberOrUsername;

        [XmlElement("CallDuration")]
        public decimal CallDuration;

        public CallRecord()
        {

        }

        public CallRecord(DateTime callDateTime, CallStatus callState, String numberOrUsername, decimal callDuration)
        {
            this.CallDateTime = callDateTime;
            this.CallState =callState;
            this.NumberOrUsername = numberOrUsername;
            this.CallDuration = callDuration;
        }

    }
    #endregion
}
