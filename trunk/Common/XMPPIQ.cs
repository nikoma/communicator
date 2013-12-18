using System;
using System.Collections.Generic;
using System.Text;
using Remwave.Services;
using System.Security.Cryptography;
using System.Xml.Serialization;
using System.IO;
using System.Collections;

namespace Remwave.Client
{
    public class XMPPIQ
    {



        private int mCounter = 0;
        private String mTag = Guid.NewGuid().ToString().Substring(1, 6);
        private String mPresenceAvatarSHA1 = "";

        public void Connected()
        {

        }
        public void Disconnected()
        {
            mCounter = 0;
            mTag = Guid.NewGuid().ToString().Substring(1, 6);
            mPresenceAvatarSHA1 = "";
        }

        public enum PresenceShow
        {
            offline = 0,
            chat,
            away,
            xa,
            dnd
        }


        public class IQMessage
        {
            public String To;
            public String Message;
            public IQMessage(String to, String message)
            {
                this.To = to;
                this.Message = message;
            }
        }

        public event EventHandler SendIQMessage;
        internal void OnSendIQMessage(IQMessage sender, EventArgs args)
        {
            if (SendIQMessage != null)
            {
                mCounter++;
                SendIQMessage(sender, args);
            }
        }


        public event EventHandler RecivedVCard;
        internal void OnRecivedVCard(IQVCard sender, EventArgs args)
        {
            if (RecivedVCard != null)
            {

                if (sender.NTPicture.ImageBase64 != null & sender.NTPicture.ImageBase64.Length > 0) mPresenceAvatarSHA1 = SHA1_ComputeHexaHash(System.Convert.FromBase64String(sender.NTPicture.ImageBase64));
                RecivedVCard(sender, args);
            }
        }



        String mServer;

        #region Messages

        private IQMessage IQMessage_Session(String domain)
        {
            String message = "<iq id=\"" + mTag + "-" + mCounter.ToString() + "\"  type=\"set\"><session xmlns=\"urn:ietf:params:xml:ns:xmpp-session\"/></iq>";
            return new IQMessage(domain, message);

        }
        private IQMessage IQMessage_DiscoInfo(String domain)
        {
            String message = "<iq id=\"" + mTag + "-" + mCounter.ToString() + "\" to=\"" + domain + "\" type=\"get\"><query xmlns=\"http://jabber.org/protocol/disco#info\"></query></iq>";
            return new IQMessage(domain, message);
        }

        private IQMessage IQMessage_Register(String domain, String username, String password)
        {
            String message = "<iq id=\"" + mTag + "-" + mCounter.ToString() + "\" to=\"" + domain + "\" type=\"set\"><query xmlns=\"jabber:iq:register\"><username>" + username + "</username><password>" + password + "</password><x xmlns=\"jabber:iq:gateway:register\"/></query></iq>";
            return new IQMessage(domain, message);
        }

        private IQMessage IQMessage_UnRegister(String domain)
        {
            String message = "<iq id=\"" + mTag + "-" + mCounter.ToString() + "\" to=\"" + domain + "\" type=\"set\"><query xmlns=\"jabber:iq:register\"><remove></remove></query></iq>";
            return new IQMessage(domain, message);
        }

        private IQMessage IQMessage_Presence(String domain, PresenceShow presenceShow, String presenceStatus)
        {
            String message;
            String tagAvatar;
            String tagVCard;
            if (mPresenceAvatarSHA1 != "")
            {
                tagAvatar = "<x xmlns='jabber:x:avatar'><hash>" + mPresenceAvatarSHA1 + "</hash></x>";
                tagVCard = "<x xmlns='vcard-temp:x:update'><photo>" + mPresenceAvatarSHA1 + "</photo></x>";
            }
            else
            {
                tagAvatar = "<x xmlns='jabber:x:avatar'/>";
                tagVCard = "<x xmlns='vcard-temp:x:update'/>";
            }

            if (presenceShow == PresenceShow.offline)
            {
                message = "<presence id=\"" + mTag + "-" + mCounter.ToString() + "\" to=\"" + domain + "\"><show/><status>" + presenceStatus + "</status><priority>1</priority>" + tagAvatar + tagVCard + "</presence>";
            }
            else if (presenceShow == PresenceShow.chat)
            {
                message = "<presence id=\"" + mTag + "-" + mCounter.ToString() + "\" to=\"" + domain + "\"><show>" + presenceShow.ToString() + "</show><status>" + presenceStatus + "</status><priority>1</priority>" + tagAvatar + tagVCard + "</presence>";
            }
            else
            {
                message = "<presence id=\"" + mTag + "-" + mCounter.ToString() + "\" to=\"" + domain + "\"><show>" + presenceShow.ToString() + "</show><status>" + presenceStatus + "</status><priority>0</priority>" + tagAvatar + tagVCard + "</presence>";
            }
            return new IQMessage(domain, message);
        }

        private IQMessage IQMessage_Prompt(JabberUser jabberUser)
        {
            String message = "<iq id=\"" + mTag + "-" + mCounter.ToString() + "\" to=\"" + jabberUser.Domain + "\" type=\"set\"><query xmlns=\"jabber:iq:gateway\"><prompt>" + jabberUser.Username + "</prompt></query></iq>";
            return new IQMessage(jabberUser.Domain, message);
        }

        private IQMessage IQMessage_Roster(JabberUser jabberUser, String group)
        {
            String message = "<iq id=\"" + mTag + "-" + mCounter.ToString() + "\" type=\"set\"><query xmlns=\"jabber:iq:roster\"><item jid=\"" + jabberUser.EscapedJID + "\" name=\"" + jabberUser.Nick + "\" subscription=\"to\"><group>" + group + "</group></item></query></iq>";
            return new IQMessage(jabberUser.Domain, message);
        }

        private IQMessage IQMessage_Subscribe(JabberUser jabberUser)
        {
            String message = "<presence id=\"" + mTag + "-" + mCounter.ToString() + "\" to=\"" + jabberUser.EscapedJID + "\"  type=\"subscribe\"/>";
            return new IQMessage(jabberUser.Domain, message);
        }

        private IQMessage IQMessage_Remove(JabberUser jabberUser)
        {
            String message = "<iq id=\"" + mTag + "-" + mCounter.ToString() + "\" type=\"set\"><query xmlns=\"jabber:iq:roster\"><item jid=\"" + jabberUser.EscapedJID + "\" subscription=\"remove\"/></query></iq>";
            return new IQMessage(jabberUser.Domain, message);
        }



        private IQMessage IQMessage_StoreAvatar(JabberUser jabberUser, String imageBase64)
        {
            if (imageBase64 == null || imageBase64.Length > 0)
            {
                return null;
            }
            mPresenceAvatarSHA1 = SHA1_ComputeHexaHash(System.Convert.FromBase64String(imageBase64));
            String message = "<iq id=\"" + mTag + "-" + mCounter.ToString() + "\" type=\"set\" to=\"" + jabberUser.EscapedJID + "\"><query xmlns='storage:client:avatar'><data mimetype='image/jpeg'>" + imageBase64 + "</data></query></iq>";
            return new IQMessage(jabberUser.Domain, message);

        }

        private IQMessage IQMessage_RetriveAvatar(JabberUser jabberUser)
        {
            String message = "<iq id=\"" + mTag + "-" + mCounter.ToString() + "\" type='get' to=\"" + jabberUser.EscapedJID + "\"><query xmlns='storage:client:avatar'/></iq>";
            return new IQMessage(jabberUser.Domain, message);
        }

        private IQMessage IQMessage_RequestVCard(JabberUser jabberUser, bool remote)
        {
            String message;
            if (remote)
            {
                message = "<iq id=\"" + mTag + "-" + mCounter.ToString() + "\" to=\"" + jabberUser.EscapedJID + "\" type=\"get\"><vCard xmlns=\"vcard-temp\"/></iq>";
            }
            else
            {
                message = "<iq id=\"" + mTag + "-" + mCounter.ToString() + "\" from=\"" + jabberUser.EscapedJID + "\" type=\"get\"><vCard xmlns=\"vcard-temp\"/></iq>";
            }
            return new IQMessage(jabberUser.Domain, message);
        }

        private IQMessage IQMessage_StoreVCard(JabberUser jabberUser, NTContact contact)
        {
            String message = "<iq id=\"" + mTag + "-" + mCounter.ToString() + "\" from=\"" + jabberUser.EscapedJID + "\" type=\"set\"> <vCard xmlns=\"vcard-temp\"> <N> <FAMILY>{NTLastName}</FAMILY> <GIVEN>{NTFirstName}</GIVEN> <MIDDLE>{NTMiddleName}</MIDDLE> </N> <ORG> <ORGNAME/> <ORGUNIT/> </ORG> <FN>{NTFirstName} {NTMiddleName} {NTLastName}</FN> <URL/> <TITLE/> <NICKNAME>{NTNickname}</NICKNAME> <PHOTO> <TYPE>image/jpeg</TYPE> <BINVAL>{NTPicture}</BINVAL> </PHOTO> <EMAIL> <HOME/> <INTERNET/> <PREF/> <USERID>{NTEmail1Address}</USERID> </EMAIL> <TEL> <PAGER/> <WORK/> <NUMBER/> </TEL> <TEL> <CELL/> <WORK/> <NUMBER/> </TEL> <TEL> <VOICE/> <WORK/> <NUMBER>{NTBusinessTelephoneNumber}</NUMBER> </TEL> <TEL> <FAX/> <WORK/> <NUMBER/> </TEL> <TEL> <PAGER/> <HOME/> <NUMBER/> </TEL> <TEL> <CELL/> <HOME/> <NUMBER>{NTMobileTelephoneNumber}</NUMBER> </TEL> <TEL> <VOICE/> <HOME/> <NUMBER>{NTHomeTelephoneNumber}</NUMBER> </TEL> <TEL> <FAX/> <HOME/> <NUMBER/> </TEL> <ADR> <WORK/> <PCODE/> <REGION/> <STREET/> <CTRY/> <LOCALITY/> </ADR> <ADR> <HOME/> <PCODE>{NTHomeAddressPostalCode}</PCODE> <REGION>{NTHomeAddressState}</REGION> <STREET>{NTHomeAddressStreet}</STREET> <CTRY>{NTHomeAddressCountry}</CTRY> <LOCALITY>{NTHomeAddressCity}</LOCALITY> </ADR> </vCard> </iq> ";

            if (contact.NTPicture.Length > 0) mPresenceAvatarSHA1 = SHA1_ComputeHexaHash(System.Convert.FromBase64String(contact.NTPicture));

            message = message
                .Replace("{NTFirstName}", contact.NTFirstName)
                .Replace("{NTMiddleName}", contact.NTMiddleName)
                .Replace("{NTLastName}", contact.NTLastName)
                .Replace("{NTNickname}", contact.NTNickname)
                .Replace("{NTEmail1Address}", contact.NTEmail1Address)
                .Replace("{NTBusinessTelephoneNumber}", contact.NTBusinessTelephoneNumber)
                .Replace("{NTHomeTelephoneNumber}", contact.NTHomeTelephoneNumber)
                .Replace("{NTMobileTelephoneNumber}", contact.NTMobileTelephoneNumber)
                .Replace("{NTHomeAddressStreet}", contact.NTHomeAddressStreet)
                .Replace("{NTHomeAddressCity}", contact.NTHomeAddressCity)
                .Replace("{NTHomeAddressPostalCode}", contact.NTHomeAddressPostalCode)
                .Replace("{NTHomeAddressState}", contact.NTHomeAddressState)
                .Replace("{NTHomeAddressCountry}", contact.NTHomeAddressCountry)
                .Replace("{NTPicture}", contact.NTPicture);

            return new IQMessage(jabberUser.Domain, message);
        }

        #endregion


        public XMPPIQ(String server)
        {
            mServer = server;
        }



        #region Methods

        internal void ProcessIQ(String iqraw)
        {
            IQ iq = null;
            using (MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(iqraw)))
            {
                XmlSerializer des = new XmlSerializer(typeof(IQ));
                try
                {
                    iq = (IQ)des.Deserialize(stream);
                }
                catch (Exception ex)
                {
                    return;
                }
                stream.Close();
            }
            if (iq == null | iq.Items == null) return;

            if (iq.Type == IQType.Result)
            {

                foreach (object item in iq.Items)
                {
                    try
                    {
                        if (item.GetType() == typeof(IQBind)) return;
                        if (item.GetType() == typeof(IQAuth)) return;
                        if (item.GetType() == typeof(IQBrowse)) return;
                        if (item.GetType() == typeof(IQDiscoInfo)) return;
                        if (item.GetType() == typeof(IQDiscoItems)) return;
                        if (item.GetType() == typeof(IQRoster)) return;
                        if (item.GetType() == typeof(IQSession)) return;

                        if (item.GetType() == typeof(IQVCard))
                        {
                            IQVCard vcard = (IQVCard)item;
                            vcard.NTJabberID = iq.From != null ? iq.From : iq.To;
                            OnRecivedVCard(vcard, new EventArgs());
                        }
                    }

                    catch (Exception)
                    {
                        //   throw;
                    }
                }
            }

        }

        internal void Session(String domain)
        {
            this.OnSendIQMessage(IQMessage_Session(domain), new EventArgs());
        }

        internal void RegisterUser(String domain, String username, String password)
        {
            this.OnSendIQMessage(IQMessage_DiscoInfo(domain), new EventArgs());
            this.OnSendIQMessage(IQMessage_Register(domain, username, password), new EventArgs());
        }

        internal void UnRegisterUser(String domain)
        {
            this.OnSendIQMessage(IQMessage_DiscoInfo(domain), new EventArgs());
            this.OnSendIQMessage(IQMessage_UnRegister(domain), new EventArgs());
        }

        internal void PromptUser(JabberUser jabberUser)
        {
            this.OnSendIQMessage(IQMessage_Prompt(jabberUser), new EventArgs());
        }

        internal void Roster(JabberUser jabberUser, String group)
        {
            this.OnSendIQMessage(IQMessage_Roster(jabberUser, group), new EventArgs());
        }

        internal void SendPresence(string domain, PresenceShow presenceShow, String presenceStatus)
        {
            this.OnSendIQMessage(IQMessage_Presence(domain, presenceShow, presenceStatus), new EventArgs());
        }

        internal void AvatarStore(JabberUser jabberUser, String imageBase64)
        {
            this.OnSendIQMessage(IQMessage_StoreAvatar(jabberUser, imageBase64), new EventArgs());
        }

        internal void RequestVCard(JabberUser jabberUser, Boolean remote)
        {
            this.OnSendIQMessage(IQMessage_RequestVCard(jabberUser, remote), new EventArgs());
        }

        internal void StoreVCard(JabberUser jabberUser, NTContact contact)
        {
            this.OnSendIQMessage(IQMessage_StoreVCard(jabberUser, contact), new EventArgs());
        }

        internal void DiscoInfo(String domain)
        {
            this.OnSendIQMessage(IQMessage_DiscoInfo(domain), new EventArgs());
        }

        internal void Subscibe(JabberUser jabberUser)
        {
            this.OnSendIQMessage(IQMessage_Subscribe(jabberUser), new EventArgs());
        }

        internal void Remove(JabberUser jabberUser)
        {
            this.OnSendIQMessage(IQMessage_Remove(jabberUser), new EventArgs());
        }

        #endregion


        #region Helper Methods

        public static string SHA1_ComputeHexaHash(byte[] data)
        {
            // Gets the SHA1 hash for text
            SHA1 sha1 = new SHA1CryptoServiceProvider();
            byte[] hash = sha1.ComputeHash(data);
            // Transforms as hexa
            string hexaHash = "";
            foreach (byte b in hash)
            {
                hexaHash += String.Format("{0:x2}", b);
            }
            // Returns SHA1 hexa hash
            return hexaHash;
        }

        #endregion
    }
}
