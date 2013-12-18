using System;
using System.Collections.Generic;
using System.Text;

namespace Remwave.Services
{

    public static class ConfigWebLinks
    {
        private static string _RegistrationLink = "http://www.nikotel.com/nikosoftsignup/";
        public static string RegistrationLink
        {
            get { return _RegistrationLink; }

        }

        private static string _ResetPasswordLink = "https://www.nikotel.com/mynikotel/forgot_password";
        public static string ResetPasswordLink
        {
            get { return _ResetPasswordLink; }
        }

        private static string _MyAccountLink = "https://www.nikotel.com/mynikotel/login";
        public static string MyAccountLink
        {
            get { return _MyAccountLink; }
        }


        private static string _RestNikotalkieUrl = "http://api.nikotel.com/rest-dev/nikotalkie-1.0.php";
        public static string RestNikotalkieUrl
        {
            get { return _RestNikotalkieUrl; }
        }
    }

    public static class ConfigTestNetwork
    {
        private static string _HostIp = "im.nikotel.com";
        public static string HostIp
        {
            get { return _HostIp; }
        }
        private static int _HostPort = 5223;
        public static int HostPort
        {
            get { return _HostPort; }
        }

    }



    public static class ConfigSIP
    {
        private static string _Realm = "nikotel.com";
        public static string Realm
        {
            get { return _Realm; }
        }

        private static string _ProxyAddress = "voip.nikotel.com";
        public static string ProxyAddress
        {
            get { return _ProxyAddress; }
        }

        private static string _UserAgent = "User-Agent: Remwave Communication Suite - Nikotel/2.1 (www.remwave.com)";
        public static string UserAgent
        {
            get { return _UserAgent; }
        }


    }

    public static class ConfigVideoProxy
    {
        private static string _ProxyAddress = "video.nikotel.com";

        public static string ProxyAddress
        {
            get { return _ProxyAddress; }
        }




    }



    public static class ConfigIM
    {

        public static string IMServer = "im.nikotel.com";
        public static int IMPort = 5223;

        public static string MSN_IMServer = "msn.im.nikotel.com";
        public static string ICQ_IMServer = "icq.im.nikotel.com";
        public static string AIM_IMServer = "aim.im.nikotel.com";
        public static string GG_IMServer = "gadugadu.im.nikotel.com";
        public static string IRC_IMServer = "irc.im.nikotel.com";
        public static string YAHOO_IMServer = "yahoo.im.nikotel.com";

        public static String GetXMPPDomain(ConfigXMPPNetwork imNetwork)
        {
            switch (imNetwork)
            {
                case ConfigXMPPNetwork.Nikotel:
                    return IMServer;                    
                case ConfigXMPPNetwork.MSN:
                    return MSN_IMServer;
                case ConfigXMPPNetwork.Yahoo:
                    return YAHOO_IMServer;
                case ConfigXMPPNetwork.GaduGadu:
                    return GG_IMServer;
                case ConfigXMPPNetwork.IRC:
                    return IRC_IMServer;
                case ConfigXMPPNetwork.AIM:
                    return AIM_IMServer;
                case ConfigXMPPNetwork.ICQ:
                    return ICQ_IMServer;
                default:
                    return IMServer;
            }
        }

        public static ConfigXMPPNetwork GetXMPPNetwork(String domain)
        {
            if (domain == IMServer)
            {
                return ConfigXMPPNetwork.Nikotel;
            }
            else
                if (domain == MSN_IMServer)
                {
                    return ConfigXMPPNetwork.MSN;
                }
                else
                    if (domain == ICQ_IMServer)
                    {
                        return ConfigXMPPNetwork.ICQ;
                    }
                    else
                        if (domain == AIM_IMServer)
                        {
                            return ConfigXMPPNetwork.AIM;
                        }
                        else
                            if (domain == GG_IMServer)
                            {
                                return ConfigXMPPNetwork.GaduGadu;
                            }
                            else
                                if (domain == IRC_IMServer)
                                {
                                    return ConfigXMPPNetwork.IRC;
                                }
                                else
                                    if (domain == YAHOO_IMServer)
                                    {
                                        return ConfigXMPPNetwork.Yahoo;
                                    }
                                    else
                                    {
                                        return ConfigXMPPNetwork.Nikotel;
                                    }
        }


    }
    public enum ConfigXMPPNetwork
    {
        Nikotel,
        MSN,
        Yahoo,
        GaduGadu,
        IRC,
        AIM,
        ICQ
    }

}
