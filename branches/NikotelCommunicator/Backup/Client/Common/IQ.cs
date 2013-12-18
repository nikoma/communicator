using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.Collections;

namespace Remwave.Client
{
    [Serializable]
    //[XmlTypeAttribute(Namespace = "jabber:client")]
    public enum IQType
    {
        [XmlEnumAttribute("error")]
        Error,
        [XmlEnumAttribute("get")]
        Get,
        [XmlEnumAttribute("result")]
        Result,
        [XmlEnumAttribute("set")]
        Set,
    }

    [Serializable]
    //[XmlType(Namespace = "jabber:client")]
    [XmlRoot("iq", IsNullable = false)]
    public class IQ
    {
        private ArrayList items;

        #region  Properties

        //[XmlElement("bind", Type = typeof(IQBind), Namespace = "urn:ietf:params:xml:ns:xmpp-bind")]
        //[XmlElement("query", Type = typeof(IQRoster), Namespace = "jabber:iq:roster")]
        //[XmlElement("query", Type = typeof(IQAuth), Namespace = "jabber:iq:auth")]
        //[XmlElement("query", Type = typeof(IQDiscoInfo), Namespace = "http://jabber.org/protocol/disco#info")]
        //[XmlElement("query", Type = typeof(IQDiscoItems), Namespace = "http://jabber.org/protocol/disco#items")]
        //[XmlElement("query", Type = typeof(IQBrowse), Namespace = "jabber:iq:browse")]
        //[XmlElement("session", Type = typeof(IQSession), Namespace = "urn:ietf:params:xml:ns:xmpp-session")]
        [XmlElement("vCard", Type = typeof(IQVCard), Namespace = "vcard-temp")]

        public ArrayList Items
        {
            get { return this.items; }
        }

        [XmlAttributeAttribute("from")]
        public String From;

        [XmlAttributeAttribute("to")]
        public String To;

        [XmlAttributeAttribute("type")]
        public IQType Type;

        [XmlAttributeAttribute("id")]
        public String ID;


        public IQ()
        {
            this.items = new System.Collections.ArrayList();
        }

        #endregion

    }


    [Serializable]
    [XmlType(Namespace = "urn:ietf:params:xml:ns:xmpp-bind")]
    [XmlRoot("iq", Namespace = "urn:ietf:params:xml:ns:xmpp-bind", IsNullable = false)]
    public class IQBind
    {
        [XmlAttributeAttribute("xmlns")]
        public String Namespace;
    }

    [Serializable]
    [XmlType(Namespace = "jabber:iq:roster")]
    [XmlRoot("iq", Namespace = "jabber:iq:roster", IsNullable = false)]
    public class IQRoster
    {
        [XmlAttributeAttribute("xmlns")]
        public String Namespace;
    }

    [Serializable]
    [XmlType(Namespace = "jabber:iq:auth")]
    [XmlRoot("iq", Namespace = "jabber:iq:auth", IsNullable = false)]
    public class IQAuth
    {
        [XmlAttributeAttribute("xmlns")]
        public String Namespace;
    }

    [Serializable]
    [XmlType(Namespace = "http://jabber.org/protocol/disco#info")]
    [XmlRoot("iq", Namespace = "http://jabber.org/protocol/disco#info", IsNullable = false)]
    public class IQDiscoInfo
    {
        [XmlAttributeAttribute("xmlns")]
        public String Namespace;
    }

    [Serializable]
    [XmlType(Namespace = "http://jabber.org/protocol/disco#items")]
    [XmlRoot("iq", Namespace = "http://jabber.org/protocol/disco#items", IsNullable = false)]
    public class IQDiscoItems
    {
        [XmlAttributeAttribute("xmlns")]
        public String Namespace;
    }

    [Serializable]
    [XmlType(Namespace = "jabber:iq:browse")]
    [XmlRoot("iq", Namespace = "jabber:iq:browse", IsNullable = false)]
    public class IQBrowse
    {
        [XmlAttributeAttribute("xmlns")]
        public String Namespace;
    }

    [Serializable]
    [XmlType(Namespace = "urn:ietf:params:xml:ns:xmpp-session")]
    [XmlRoot("iq", Namespace = "urn:ietf:params:xml:ns:xmpp-session", IsNullable = false)]
    public class IQSession
    {
        [XmlAttributeAttribute("xmlns")]
        public String Namespace;
    }

    [Serializable]
    [XmlType(Namespace = "vcard-temp")]
    [XmlRoot("vCard", Namespace = "vcard-temp", IsNullable = false)]
    public class IQVCard
    {
        [XmlAttributeAttribute("xmlns")]
        public String Namespace;

        public struct Names
        {
            [XmlElement("FAMILY")]
            public String NTLastName;
            [XmlElement("GIVEN")]
            public String NTFirstName;
            [XmlElement("MIDDLE")]
            public String NTMiddleName;
        }

        public struct Email
        {
            [XmlElement("HOME")]
            public String Home;
            [XmlElement("INTERNET")]
            public String Internet;
            [XmlElement("PREF")]
            public String Pref;
            [XmlElement("USERID")]
            public String UserID;
        }

        public struct Tel
        {
            [XmlElement("PAGER")]
            public String Pager;
            [XmlElement("CELL")]
            public String Cell;
            [XmlElement("VOICE")]
            public String Voice;
            [XmlElement("FAX")]
            public String Fax;
            [XmlElement("WORK")]
            public String Work;
            [XmlElement("HOME")]
            public String Home;
            [XmlElement("NUMBER")]
            public String Number;
        }

        public struct Adr
        {
            [XmlElement("WORK")]
            public String Work;
            [XmlElement("HOME")]
            public String Home;
            [XmlElement("PCODE")]
            public String PCode;
            [XmlElement("STREET")]
            public String Street;
            [XmlElement("REGION")]
            public String Region;
            [XmlElement("LOCALITY")]
            public String Locality;
            [XmlElement("CTRY")]
            public String Ctry;
        }

        public struct Photo
        {
            [XmlElement("TYPE")]
            public String MimeType;

            [XmlElement("BINVAL")]
            public String ImageBase64;
        }

        #region  Properties

        [XmlElement("N")]
        public Names NTName;

        [XmlElement("NICKNAME")]
        public String NTNickname;

        [XmlElement("JABBERID")]
        public String NTJabberID;

        [XmlElement("PHOTO")]
        public Photo NTPicture;

        [XmlElement("EMAIL")]
        public Email NTEmail;

        [XmlElement("TEL")]
        public List<Tel> NTTel;

        [XmlElement("ADR")]
        public List<Adr> NTAddress;

        #endregion
    }
}
