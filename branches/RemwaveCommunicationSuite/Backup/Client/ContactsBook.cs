using System;
using System.Collections;
using System.Xml.Serialization;
using System.IO;
using Remwave.Services;
using System.Windows.Forms;

namespace Remwave.Client
{

    [Serializable]
    public class ContactList : ArrayList
    {
        public ContactList getCandidatesForJabberID(string jabberID)
        {
            ContactList candidates = new ContactList();
            IEnumerator ie = GetEnumerator();
            NTContact e;

            while (ie.MoveNext())
            {
                e = (NTContact)ie.Current;
                if (e.NTJabberID.Trim().ToUpper() == jabberID.Trim().ToUpper())
                {
                    candidates.Add(e);
                }
            }
            return candidates;
        }

        public ContactList getCandidatesForName(string[] keys)
        {
            ContactList candidates = new ContactList();
            IEnumerator ie = GetEnumerator();
            NTContact e;

            foreach (string search in keys)
            {

            while (ie.MoveNext())
            {
                e = (NTContact)ie.Current;
                if (e.NTLastName.ToUpper().StartsWith(search.ToUpper()))
                {
                    candidates.Add(e);
                }
            }
            ie.Reset();
        }
            return candidates;
        }
        public ContactList getCandidatesForNumber(string number)
        {
            ContactList candidates = new ContactList();
            IEnumerator ie = GetEnumerator();
            NTContact e;
            number = number.ToUpper();

            while (ie.MoveNext())
            {
                e = (NTContact)ie.Current;
                if (e.NTHomeTelephoneNumber.StartsWith(number))
                {
                    candidates.Add(e);
                }
            }
            return candidates;
        }
    }

    public interface IPhoneBook
    {
        ContactList List
        {
            get;
            set;
        }
        void Save();
    }

    public class WEBPhoneBook : IPhoneBook
    {
        private ContactList contactList;
        private static UserAccount userAccount;
        private static string ClientConfigurationPath = Application.LocalUserAppDataPath;

        #region con / desctructor

        public WEBPhoneBook(UserAccount myUserAccount)
        {
            userAccount = myUserAccount;
            contactList = Persistence;

        }
        ~WEBPhoneBook()
        {
            Save();
        }

        #endregion

        #region PhoneBook Members

        public void Save()
        {
            Persistence = contactList;
        }

        public ContactList List
        {
            get
            {
                return contactList;
            }
            set
            {
                contactList = value;
            }
        }

        #endregion

        #region processor

        private static string[] Properties = {"NTItemId","NTCustomerID","NTFirstName", "NTLastName", "NTMiddleName", "NTHomeTelephoneNumber", "NTCompanyName", 
										"NTMobileTelephoneNumber", "NTVoIPTelephoneNumber", "NTDeleted", "NTJabberID","NTCompanyTelephoneNumber","NTBusinessTelephoneNumber", 
                                        "NTHomeAddressStreet","NTHomeAddressCity","NTHomeAddressPostalCode","NTHomeAddressState","NTHomeAddressCountry","NTDeleted"};

        private static ContactList Persistence
        {
            get
            {
                ContactList container = new ContactList();
                try
                {
                    XmlSerializer xser = new XmlSerializer(typeof(NTContact[]));
                    StreamReader sr = new StreamReader(ClientConfigurationPath + userAccount + ".dat");
                    container.AddRange((NTContact[])xser.Deserialize(sr));
                    sr.Close();
                }
                catch (System.IO.FileNotFoundException)
                {
                    return new ContactList();
                }
                return container;
            }
            set
            {
                if (!Directory.Exists(ClientConfigurationPath))
                {
                    Directory.CreateDirectory(ClientConfigurationPath);
                }

                XmlSerializer xser = new XmlSerializer(typeof(NTContact[]));
                StreamWriter sw = new StreamWriter(ClientConfigurationPath + userAccount + ".dat");
                xser.Serialize(sw, (NTContact[])value.ToArray(typeof(NTContact)));
                sw.Close();
            }
        }
        
        
        /*
        private static ContactList Persistence
        {
            get
            {
                ContactList container = new ContactList();
                try
                {
                    Remwave.Client.RSIFeaturesWS.RSIService ss = new Remwave.Client.RSIFeaturesWS.RSIService();
                    XmlSerializer xser = new XmlSerializer(typeof(NTContact[]));

                    //string[] items = {""};
                    string sout = ss.servicePhonebookGet(userAccount.Username, userAccount.Password, Properties, null);
                    Console.WriteLine("PHONEBOOK-GET:Done");
                    StringReader sr = new StringReader(sout);
                    NTContact[] ntc = (NTContact[])xser.Deserialize(sr);

                    container.AddRange(ntc);
                    sr.Close();
                }
                catch (Exception e)
                {
                    Console.WriteLine("PHONEBOOK-GET:Failed - " + e.Message);
                    return new ContactList();
                }
                return container;
            }
            set
            {
                try
                {
                    Remwave.Client.RSIFeaturesWS.RSIService ss = new Remwave.Client.RSIFeaturesWS.RSIService();
                    XmlSerializer xser = new XmlSerializer(typeof(NTContact[]));
                    StringWriter sw = new StringWriter();

                    xser.Serialize(sw, (NTContact[])value.ToArray(typeof(NTContact)));

                    ss.servicePhonebookPut(userAccount.Username, userAccount.Password, Properties, sw.ToString());
                    Console.WriteLine("PHONEBOOK-PUT:Done");
                    sw.Close();
                }
                catch (Exception e)
                {
                    Console.WriteLine("PHONEBOOK-PUT:Failed - " + e.Message);
                }
            }
        }
        */
        #endregion
    }

    /// <summary>
    /// Summary description for PhoneBook.
    /// </summary>
    public class XMLPhoneBook : IPhoneBook
    {
        private ContactList contactList;
        private static string directory = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "\\Remwave";
        private static string filenameContacts = "\\client-contacts.xml";

        #region con /destructor

        public XMLPhoneBook()
        {
            contactList = Persistence;
        }

        ~XMLPhoneBook()
        {
            Save();
        }

        #endregion

        #region PhoneBook Members

        public void Save()
        {
            Persistence = contactList;
        }

        public ContactList List
        {
            get
            {
                return contactList;
            }
            set
            {
                contactList = value;
            }
        }

        #endregion

        #region processor

        private static ContactList Persistence
        {
            get
            {
                ContactList container = new ContactList();
                try
                {
                    XmlSerializer xser = new XmlSerializer(typeof(NTContact[]));
                    StreamReader sr = new StreamReader(directory + filenameContacts);
                    container.AddRange((NTContact[])xser.Deserialize(sr));
                    sr.Close();
                }
                catch (System.IO.FileNotFoundException)
                {
                    return new ContactList();
                }
                return container;
            }
            set
            {
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                XmlSerializer xser = new XmlSerializer(typeof(NTContact[]));
                StreamWriter sw = new StreamWriter(directory + filenameContacts);
                xser.Serialize(sw, (NTContact[])value.ToArray(typeof(NTContact)));
                sw.Close();
            }
        }

        #endregion
    }
}
