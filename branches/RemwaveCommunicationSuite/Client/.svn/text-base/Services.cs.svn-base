using System;
using System.Collections.Generic;
using System.Text;

namespace Remwave.Services
{

    public static class ConfigProfile
    {
        private static bool _AllowConfiguration = true;

        public static bool AllowConfiguration
        {
            get { return _AllowConfiguration; }
            set { _AllowConfiguration = value; }
        }
        private static bool _RunFirstStartConfiguration = true;

        public static bool RunFirstStartConfiguration
        {
            get { return _RunFirstStartConfiguration; }
            set { _RunFirstStartConfiguration = value; }
        }
    }

    public static class ConfigWebLinks
    {
        private static string _RegistrationLink = "about:blank";

        public static string RegistrationLink
        {
            get { return _RegistrationLink; }
         
        }
        private static string _ResetPasswordLink = "about:blank";

        public static string ResetPasswordLink
        {
            get { return _ResetPasswordLink; }
        }
        private static string _MyAccountLink = "about:blank";

        public static string MyAccountLink
        {
            get { return _MyAccountLink; }
        }

    }

    public static class ConfigTestNetwork
    {
        private static string _HostIp = "localhost";
        public static string HostIp
        {
            get { return _HostIp; }
            set { _HostIp = value; }
        }
        private static int _HostPort = 5222;
        public static int HostPort
        {
            get { return _HostPort; }
            set { _HostPort = value; }
        }
    }

    public static class ConfigIM
    {

        private static string _IMServer = "localhost";

        public static string IMServer
        {
            get { return _IMServer; }
            set { _IMServer = value; }
        }
        private static int _IMPort = 5222;

        public static int IMPort
        {
            get { return _IMPort; }
            set { _IMPort = value; }
        }


    }

    public static class ConfigSIP
    {
        private static string _Realm = "*";
        //private static string _Realm = "69.239.32.235";
        public static string Realm
        {
            get { return _Realm; }
             set { _Realm = value; }
        }

        private static string _ProxyAddress = "localhost";
        //private static string _ProxyAddress = "69.239.32.235";

        public static string ProxyAddress
        {
            get { return _ProxyAddress; }
            set { _ProxyAddress = value; }
        }
        



    }

    public static class ConfigVideoProxy
    {
        private static string _ProxyAddress = "63.214.186.148";

        public static string ProxyAddress
        {
            get { return _ProxyAddress; }
            set { _ProxyAddress = value; }
        }




    }

    public class NetworkInfo
    {
        private string _LocalIP = "0.0.0.0";

        public string LocalIP
        {
            get { return _LocalIP; }
            set { _LocalIP = value; }
        }

        private int _ResponseTime = 0;

        public int ResponseTime
        {
            get { return _ResponseTime; }
            set { _ResponseTime = value; }
        }
        private bool _Online = false;

        public bool Online
        {
            get { return _Online; }
            set { _Online = value; }
        }



    }

    public class UserAccount
    {
        private string _username;

        public string Username
        {
            get { return _username; }
        }
        private string _password;

        public string Password
        {
            get { return _password; }
        }
        private bool _authorized = false;

        public void Login(string Username, string Password)
        {
            _username = Username;
            _password = Password;
            _authorized = true;
        }
        public void Logout()
        {
            _authorized = false;
            _username = null;
            _password = null;
            
        }

        public bool LoggedIn()
        {
            return _authorized;
        }
    }

    public enum ActivityType
    {
        None,
        Call,
        IM,
        VideoCall,
        ScreenSharing,
        Email,
        AddContact
    }

    public class Activity
    {
        public Activity()
        {
            _activityData = "";
            _activityType = ActivityType.None;
        }

        public Activity(ActivityType activityType, String activityData)
        {
            _activityType = activityType;
            _activityData = activityData;
        }

        private ActivityType _activityType = ActivityType.None;

        public ActivityType ActivityType
        {
            get { return _activityType; }
            set { _activityType = value; }
        }
        private String _activityData;

        public String ActivityData
        {
            get { return _activityData; }
            set { _activityData = value; }
        }



    }



}

