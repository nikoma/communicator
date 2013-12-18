using System;
using System.IO;
using System.Xml.Serialization;
using System.Text;
using System.Security.Cryptography;
using System.Windows.Forms;
using System.Collections.Generic;

namespace Remwave.Client.Serializable
{


    #region Client Configuration

    internal class ClientConfigurationSerializer
    {
        private XmlSerializer serializer = new XmlSerializer(typeof(ClientConfiguration));
        private readonly string ClientConfigurationDir;
        private readonly string ClientConfigurationFile;

        public ClientConfigurationSerializer(string configPath, string configFilename)
        {
            this.ClientConfigurationDir = configPath;
            this.ClientConfigurationFile = configFilename;
        }


        public ClientConfiguration LoadClientConfiguration()
        {
            ClientConfiguration tmpClientConfiguration = new ClientConfiguration();
            try
            {
                if (!Directory.Exists(ClientConfigurationDir))
                {
                    Directory.CreateDirectory(ClientConfigurationDir);
                }

                byte[] data = File.ReadAllBytes(ClientConfigurationDir + ClientConfigurationFile);
                data = Unprotect(data);
                using (MemoryStream ms = new MemoryStream(data))
                {
                    tmpClientConfiguration = (ClientConfiguration)this.serializer.Deserialize(ms);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("LoadClientConfiguration : " + ex.Message);
                if (File.Exists(ClientConfigurationDir + ClientConfigurationFile))
                {
                    File.Delete(ClientConfigurationDir + ClientConfigurationFile);
                }
            }
            return tmpClientConfiguration;
        }

        public void SaveClientConfiguration(ClientConfiguration clientConfiguration)
        {
            byte[] data;
            ClientConfiguration tmpClientConfiguration = new ClientConfiguration();

            try
            {
                if (!Directory.Exists(ClientConfigurationDir))
                {
                    Directory.CreateDirectory(ClientConfigurationDir);
                }

                lock (clientConfiguration.NetworksConfiguration)
                {
                    foreach (ClientConfigurationNetworks networkcfg in clientConfiguration.NetworksConfiguration)
                    {
                        if (networkcfg.Username == clientConfiguration.CurrentUsername)
                        {
                            clientConfiguration.NetworksConfiguration.Remove(networkcfg);
                            break;
                        }
                    }
                }

                //to fix compatibility issues sanitize saved object
                ClientConfigurationNetworks imnetworkcfg = new ClientConfigurationNetworks();
                imnetworkcfg.Username = clientConfiguration.CurrentUsername;
                imnetworkcfg.AIM_Password = clientConfiguration.AIM_Password;
                imnetworkcfg.AIM_Username = clientConfiguration.AIM_Username;
                imnetworkcfg.GG_Password = clientConfiguration.GG_Password;
                imnetworkcfg.GG_Username = clientConfiguration.GG_Username;
                imnetworkcfg.ICQ_Password = clientConfiguration.ICQ_Password;
                imnetworkcfg.ICQ_Username = clientConfiguration.ICQ_Username;
                imnetworkcfg.IRC_Password = clientConfiguration.IRC_Password;
                imnetworkcfg.IRC_Username = clientConfiguration.IRC_Username;
                imnetworkcfg.MSN_Password = clientConfiguration.MSN_Password;
                imnetworkcfg.MSN_Username = clientConfiguration.MSN_Username;
                imnetworkcfg.Yahoo_Password = clientConfiguration.Yahoo_Password;
                imnetworkcfg.Yahoo_Username = clientConfiguration.Yahoo_Username;

                clientConfiguration.NetworksConfiguration.Add(imnetworkcfg);


                tmpClientConfiguration.AutoLogin = clientConfiguration.AutoLogin;
                tmpClientConfiguration.LastNews = clientConfiguration.LastNews;
                tmpClientConfiguration.Password = clientConfiguration.Password;
                tmpClientConfiguration.RememberMe = clientConfiguration.RememberMe;
                tmpClientConfiguration.Username = clientConfiguration.Username;
                tmpClientConfiguration.NetworksConfiguration = clientConfiguration.NetworksConfiguration;

                MemoryStream xmlStream = new MemoryStream();
                this.serializer.Serialize(xmlStream, tmpClientConfiguration);
                xmlStream.Position = 0;
                data = xmlStream.GetBuffer();
                data = Protect(data);
                File.WriteAllBytes(ClientConfigurationDir + ClientConfigurationFile, data);
            }
            catch (Exception ex)
            {
                Console.WriteLine("SaveClientConfiguration : " + ex.Message);
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

    [XmlRoot("ClientConfigurationNetworks")]
    public class ClientConfigurationNetworks
    {
        [XmlElement("Username")]
        public String Username = "";
        //ICQ
        [XmlElement("ICQ_Username")]
        public String ICQ_Username = "";
        [XmlElement("ICQ_Password")]
        public String ICQ_Password = "";
        //MSN
        [XmlElement("MSN_Username")]
        public String MSN_Username = "";
        [XmlElement("MSN_Password")]
        public String MSN_Password = "";
        //Yahoo
        [XmlElement("Yahoo_Username")]
        public String Yahoo_Username = "";
        [XmlElement("Yahoo_Password")]
        public String Yahoo_Password = "";
        //AIM
        [XmlElement("AIM_Username")]
        public String AIM_Username = "";
        [XmlElement("AIM_Password")]
        public String AIM_Password = "";
        //GaduGadu
        [XmlElement("GG_Username")]
        public String GG_Username = "";
        [XmlElement("GG_Password")]
        public String GG_Password = "";
        //IRC
        [XmlElement("IRC_Username")]
        public String IRC_Username = "";
        [XmlElement("IRC_Password")]
        public String IRC_Password = "";
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

        [XmlElement("LastNews")]
        public String LastNews = "1.0.0.0";

        [XmlIgnore]
        public String CurrentUsername = "";

        //ICQ
        [XmlElement("ICQ_Username")]
        public String ICQ_Username = "";
        [XmlElement("ICQ_Password")]
        public String ICQ_Password = "";
        //MSN
        [XmlElement("MSN_Username")]
        public String MSN_Username = "";
        [XmlElement("MSN_Password")]
        public String MSN_Password = "";
        //Yahoo
        [XmlElement("Yahoo_Username")]
        public String Yahoo_Username = "";
        [XmlElement("Yahoo_Password")]
        public String Yahoo_Password = "";
        //AIM
        [XmlElement("AIM_Username")]
        public String AIM_Username = "";
        [XmlElement("AIM_Password")]
        public String AIM_Password = "";
        //GaduGadu
        [XmlElement("GG_Username")]
        public String GG_Username = "";
        [XmlElement("GG_Password")]
        public String GG_Password = "";
        //IRC
        [XmlElement("IRC_Username")]
        public String IRC_Username = "";
        [XmlElement("IRC_Password")]
        public String IRC_Password = "";



        [XmlElement("NetworksConfiguration")]
        public List<ClientConfigurationNetworks> NetworksConfiguration = new List<ClientConfigurationNetworks>();

        public ClientConfiguration()
        {

        }


        public void SetLoginOptions(String username, String password, bool autoLogin, bool rememberMe)
        {

            if (rememberMe)
            {
                this.Username = username;
                this.Password = password;

            }
            else
            {
                this.Username = "";
                this.Password = "";
            }

            this.AutoLogin = autoLogin;
            this.RememberMe = rememberMe;
            this.CurrentUsername = username;

            try
            {
                lock (this.NetworksConfiguration)
                {
                    foreach (ClientConfigurationNetworks network in this.NetworksConfiguration)
                    {
                        if (network.Username == this.CurrentUsername)
                        {
                            this.AIM_Password = network.AIM_Password;
                            this.AIM_Username = network.AIM_Username;
                            this.GG_Password = network.GG_Password;
                            this.GG_Username = network.GG_Username;
                            this.ICQ_Password = network.ICQ_Password;
                            this.ICQ_Username = network.ICQ_Username;
                            this.IRC_Password = network.IRC_Password;
                            this.IRC_Username = network.IRC_Username;
                            this.MSN_Password = network.MSN_Password;
                            this.MSN_Username = network.MSN_Username;
                            this.Yahoo_Password = network.Yahoo_Password;
                            this.Yahoo_Username = network.Yahoo_Username;
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
        }
        public void DeleteICQLoginOptions()
        {
            this.ICQ_Username = "";
            this.ICQ_Password = "";
        }
        public void DeleteMSNLoginOptions()
        {
            this.MSN_Username = "";
            this.MSN_Password = "";
        }
        public void DeleteYahooLoginOptions()
        {
            this.Yahoo_Username = "";
            this.Yahoo_Password = "";
        }
        public void DeleteAIMLoginOptions()
        {
            this.AIM_Username = "";
            this.AIM_Password = "";
        }
        public void DeleteIRCLoginOptions()
        {
            this.IRC_Username = "";
            this.IRC_Password = "";
        }
        public void DeleteGaduGaduLoginOptions()
        {
            this.GG_Username = "";
            this.GG_Password = "";
        }

        public void SetICQLoginOptions(String username, String password)
        {
            this.ICQ_Username = username;
            this.ICQ_Password = password;
        }
        public void SetMSNLoginOptions(String username, String password)
        {
            this.MSN_Username = username;
            this.MSN_Password = password;
        }
        public void SetYahooLoginOptions(String username, String password)
        {
            this.Yahoo_Username = username;
            this.Yahoo_Password = password;
        }
        public void SetAIMLoginOptions(String username, String password)
        {
            this.AIM_Username = username;
            this.AIM_Password = password;
        }
        public void SetIRCLoginOptions(String username, String password)
        {
            this.IRC_Username = username;
            this.IRC_Password = password;
        }
        public void SetGaduGaduLoginOptions(String username, String password)
        {
            this.GG_Username = username;
            this.GG_Password = password;
        }


        public void DeleteLoginOptions()
        {
            this.Username = "";
            this.Password = "";
            this.AutoLogin = false;
            this.RememberMe = false;

            if (CurrentUsername == "") return;
            try
            {
                lock (this.NetworksConfiguration)
                {
                    foreach (ClientConfigurationNetworks networkcfg in this.NetworksConfiguration)
                    {
                        if (networkcfg.Username == this.CurrentUsername)
                        {
                            this.NetworksConfiguration.Remove(networkcfg);
                            break;
                        }
                    }
                }
            }
            catch (Exception)
            {

            }
            ClientConfigurationNetworks imnetworkcfg = new ClientConfigurationNetworks();

            imnetworkcfg.Username = this.CurrentUsername;
            imnetworkcfg.AIM_Password = this.AIM_Password;
            imnetworkcfg.AIM_Username = this.AIM_Username;
            imnetworkcfg.GG_Password = this.GG_Password;
            imnetworkcfg.GG_Username = this.GG_Username;
            imnetworkcfg.ICQ_Password = this.ICQ_Password;
            imnetworkcfg.ICQ_Username = this.ICQ_Username;
            imnetworkcfg.IRC_Password = this.IRC_Password;
            imnetworkcfg.IRC_Username = this.IRC_Username;
            imnetworkcfg.MSN_Password = this.MSN_Password;
            imnetworkcfg.MSN_Username = this.MSN_Username;
            imnetworkcfg.Yahoo_Password = this.Yahoo_Password;
            imnetworkcfg.Yahoo_Username = this.Yahoo_Username;

            this.NetworksConfiguration.Add(imnetworkcfg);
            this.CurrentUsername = "";
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

    internal class CallHistorySerializer
    {
        private String mPath;
        private XmlSerializer serializer = new XmlSerializer(typeof(CallHistory));
        private int maxsize = 100;

        public CallHistorySerializer(String path)
        {
            mPath = path;
        }

        public CallHistory LoadCallHistory(string username)
        {
            CallHistory tmpCallHistory = new CallHistory();

            try
            {
                FileStream fs = new FileStream(Path.Combine(mPath, "CallHistory-" + username + ".xml"), FileMode.OpenOrCreate);
                tmpCallHistory = (CallHistory)this.serializer.Deserialize(fs);
                fs.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("LoadCallHistory : " + ex.Message);
            }
            return tmpCallHistory;
        }

        public void SaveCallHistory(CallHistory callHistory, string username)
        {


            try
            {


                if (callHistory.CallRecords.Length > maxsize)
                {
                    Array.Reverse(callHistory.CallRecords);
                    Array.Resize(ref callHistory.CallRecords, maxsize);
                    Array.Reverse(callHistory.CallRecords);
                }

                FileStream fs = new FileStream(Path.Combine(mPath, "CallHistory-" + username + ".xml"), FileMode.Create);
                this.serializer.Serialize(fs, callHistory);

            }
            catch (Exception ex)
            {
                Console.WriteLine("LoadCallHistory : " + ex.Message);
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

    [XmlRoot("CallRecord")]
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
            this.CallState = callState;
            this.NumberOrUsername = numberOrUsername;
            this.CallDuration = callDuration;
        }

    }
    #endregion
}
