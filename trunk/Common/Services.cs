using System;
using System.Collections.Generic;
using System.Text;
using Remwave.Client;

namespace Remwave.Services
{
    public enum ConfigXMPPNetwork
    {
        Default,
        MSN,
        Yahoo,
        GaduGadu,
        IRC,
        AIM,
        ICQ
    }


    public class JabberUser
    {
        public String JID;
        public String Username;
        public String Nick;
        public ConfigXMPPNetwork Network;
        public String Domain;

        public JabberUser(String jid, String nick)
        {
            NormalizeJID(jid);
            this.Nick = nick;
        }

        public JabberUser(String jid)
        {
            NormalizeJID(jid);
        }
        public string EscapedUsername
        {
            get { return this.Username.Replace(@"\", @"\5c").Replace(@" ", @"\20").Replace("\"", @"\22").Replace(@"&", @"\26").Replace(@"'", @"\27").Replace(@"/", @"\2f").Replace(@":", @"\3a").Replace(@"<", @"\3c").Replace(@">", @"\3e").Replace(@"@", @"\40"); }
        }
        public string EscapedJID
        {
            get { return this.EscapedUsername + @"@" + this.Domain; }
        }
        private string Unescape(string str)
        {
            return str.Replace(@"\20", @" ").Replace(@"\26", @"&").Replace(@"\27", @"'").Replace(@"\2f", @"/").Replace(@"\3a", @":").Replace(@"\3c", @"<").Replace(@"\3e", @">").Replace(@"\40", @"@").Replace(@"\22", "\"").Replace(@"\5c", @"\");
        }
        private void NormalizeJID(String jid)
        {
            this.JID = Unescape(jid.ToLower());
            this.Username = GetUserName(this.JID);
            this.Nick = this.Username;
            this.Network = GetNetwork(this.JID);
            this.Domain = GetDomain(this.JID);
            //wrap all together
            this.JID = this.Username + @"@" + this.Domain;

        }

        private String GetDomain(String jid)
        {
            return ConfigIM.GetXMPPDomain(GetNetwork(jid));
        }



        private ConfigXMPPNetwork GetNetwork(String jid)
        {
            jid = (jid.ToLower().Split('/'))[0];
            if (jid.EndsWith(@"@" + ConfigIM.GG_IMServer)) return ConfigIM.GetXMPPNetwork(ConfigIM.GG_IMServer);
            if (jid.EndsWith(@"@" + ConfigIM.ICQ_IMServer)) return ConfigIM.GetXMPPNetwork(ConfigIM.ICQ_IMServer);
            if (jid.EndsWith(@"@" + ConfigIM.IRC_IMServer)) return ConfigIM.GetXMPPNetwork(ConfigIM.IRC_IMServer);
            if (jid.EndsWith(@"@" + ConfigIM.MSN_IMServer)) return ConfigIM.GetXMPPNetwork(ConfigIM.MSN_IMServer);
            if (jid.EndsWith(@"@" + ConfigIM.YAHOO_IMServer)) return ConfigIM.GetXMPPNetwork(ConfigIM.YAHOO_IMServer);
            if (jid.EndsWith(@"@" + ConfigIM.AIM_IMServer)) return ConfigIM.GetXMPPNetwork(ConfigIM.AIM_IMServer);
            return ConfigIM.GetXMPPNetwork(ConfigIM.IMServer);
        }

        private String GetUserName(String jid)
        {
            String username = "";
            jid = (jid.ToLower().Split('/'))[0];
            if (jid.EndsWith(@"@" + GetDomain(jid)))
            {
                username = jid.Replace(@"@" + GetDomain(jid), "");
            }
            else
            {
                username = jid;
            }
            return username;
        }

        public override string ToString()
        {
            return this.JID;
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

        public NTContact Contact;

        private JabberUser _jabberUser;
        public JabberUser JabberUser
        {
            get { return _jabberUser; }
        }

        public void Credentials(string username, string password)
        {
            this.Logout();
            _username = username;
            _password = password;
            _jabberUser = new JabberUser(username + @"@" + ConfigIM.IMServer);
            Contact = new NTContact();
            Contact.NTJabberID = _jabberUser.JID;
        }

        public void Login()
        {

            _authorized = true;

        }
        public void Logout()
        {
            _username = null;
            _password = null;
            _authorized = false;
            _jabberUser = null;
            Contact = null;
        }

        public bool LoggedIn
        {
            get { return _authorized; }
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
        AddContact,
        HostNikomeeting,
        JoinNikomeeting,
        NikotalkieMessage
    }

    public class Activity
    {
        public Activity()
        {
            ActivityOtherData = "";
            ActivityType = ActivityType.None;
            ActivityJabberUser = null;
        }

        public Activity(ActivityType activityType, JabberUser activityJabberUser, String activityOtherData)
        {
            ActivityType = activityType;
            ActivityOtherData = activityOtherData;
            ActivityJabberUser = activityJabberUser;
        }

        public ActivityType ActivityType = ActivityType.None;
        public JabberUser ActivityJabberUser;
        public String ActivityOtherData;
    }
}
