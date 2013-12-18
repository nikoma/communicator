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

    public class RPhoneBook : IPhoneBook
    {
        private ContactList _contactList;
        private static UserAccount _userAccount;
        private static string _clientConfigurationPath = Application.LocalUserAppDataPath;
        private static bool _storeOnline = false;

        #region con / desctructor

        public RPhoneBook(UserAccount userAccount, bool storeOnline)
        {
            _userAccount = userAccount;
            _contactList = Persistence;
            _storeOnline = storeOnline;

        }
        ~RPhoneBook()
        {
            Save();
        }

        #endregion

        #region RPhoneBook Members

        public void Save()
        {
            Persistence = _contactList;
        }

        public ContactList List
        {
            get
            {
                return _contactList;
            }
            set
            {
                _contactList = value;
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
                if(_storeOnline){
                    ContactList container = new ContactList();
                try
                {   /*
                    Remwave.Client.RSIFeaturesWS.RSIService ss = new Remwave.Client.RSIFeaturesWS.RSIService();
                    XmlSerializer xser = new XmlSerializer(typeof(NTContact[]));

                    //string[] items = {""};
                    string sout = ss.servicePhonebookGet(userAccount.Username, userAccount.Password, Properties, null);
                    Console.WriteLine("PHONEBOOK-GET:Done");
                    StringReader sr = new StringReader(sout);
                    NTContact[] ntc = (NTContact[])xser.Deserialize(sr);

                    container.AddRange(ntc);
                    sr.Close();
                     * */
                }
                catch (Exception e)
                {
                    Console.WriteLine("PHONEBOOK-GET:Failed - " + e.Message);
                    return new ContactList();
                }
                return container;
                }
                else
                {
                    ContactList container = new ContactList();
                try
                {
                    XmlSerializer xser = new XmlSerializer(typeof(NTContact[]));
                    StreamReader sr = new StreamReader(_clientConfigurationPath + _userAccount + ".dat");
                    container.AddRange((NTContact[])xser.Deserialize(sr));
                    sr.Close();
                }
                catch (System.IO.FileNotFoundException)
                {
                    return new ContactList();
                }
                return container;
                }
                
            }
            set
            {
                if (_storeOnline)
                {
                     try
                {
                         /*
                    Remwave.Client.RSIFeaturesWS.RSIService ss = new Remwave.Client.RSIFeaturesWS.RSIService();
                    XmlSerializer xser = new XmlSerializer(typeof(NTContact[]));
                    StringWriter sw = new StringWriter();

                    xser.Serialize(sw, (NTContact[])value.ToArray(typeof(NTContact)));

                    ss.servicePhonebookPut(userAccount.Username, userAccount.Password, Properties, sw.ToString());
                    Console.WriteLine("PHONEBOOK-PUT:Done");
                    sw.Close();
                          */
                }
                catch (Exception e)
                {
                    Console.WriteLine("PHONEBOOK-PUT:Failed - " + e.Message);
                }
                }
                else
                {
 if (!Directory.Exists(_clientConfigurationPath))
                {
                    Directory.CreateDirectory(_clientConfigurationPath);
                }

                XmlSerializer xser = new XmlSerializer(typeof(NTContact[]));
                StreamWriter sw = new StreamWriter(_clientConfigurationPath + _userAccount + ".dat");
                xser.Serialize(sw, (NTContact[])value.ToArray(typeof(NTContact)));
                sw.Close();
                }

               
            }
        }
        
       
        #endregion
    }

    
}
