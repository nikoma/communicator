using System;
using System.Collections;
using System.Xml.Serialization;
using System.IO;
using Outlook = Microsoft.Office.Interop.Outlook;
using System.Reflection;
using System.Diagnostics;
using Remwave.Services;
using System.Threading;

namespace Remwave.Client
{

    public enum NTContactStoreType
    {
        Local, Server, Outlook, vCard
    }

    public class NTContactStore
    {
        public String Name;
        public NTContactStoreType StoreType;
        public Boolean Enabled;

        public NTContactStore(String name, NTContactStoreType storeType, Boolean enabled)
        {

            Name = name;
            StoreType = storeType;
            Enabled = enabled;
        }

        public override string ToString()
        {
            return this.Name;
        }
    }

    public class ContactBook : ContactList
    {

        #region Public Events
        public event EventHandler UpdateCompleted;
        internal void OnUpdateCompleted(ContactBook sender, EventArgs args)
        {
            if (UpdateCompleted != null)
                UpdateCompleted(sender, args);
        }
        #endregion

        #region Private Properties
        private Thread mLoadContactsBookThread;
        private string mStoreBaseDirectory = "";
        private string mStoreUserDirectory = "";
        private static string mStoreFilename = @"\Contacts.xml";
        private static string[] mProperties = {"NTItemId","NTCustomerID","NTFirstName", "NTLastName", "NTMiddleName", "NTHomeTelephoneNumber", "NTCompanyName", 
										"NTMobileTelephoneNumber", "NTVoIPTelephoneNumber", "NTDeleted", "NTJabberID","NTCompanyTelephoneNumber","NTBusinessTelephoneNumber", 
                                        "NTHomeAddressStreet","NTHomeAddressCity","NTHomeAddressPostalCode","NTHomeAddressState","NTHomeAddressCountry","NTDeleted","NTEmail1Address","NTPicture","NTNickname"};



        private static UserAccount mUserAccount;
        #endregion

        #region Public Properties
        public readonly bool UseLocalStore = false;
        public readonly bool UseServerStore = false;
        public readonly bool UseOutlookStore = false;
        public readonly NTContactStore[] ContactStores = null;
        public ContactList Contacts
        {
            get { return this.getContactList(); }
        }
        #endregion


        public ContactBook(NTContactStore[] stores, String storePath)
        {
            this.ContactStores = stores;
            this.mStoreBaseDirectory = storePath;
            foreach (NTContactStore store in stores)
            {
                switch (store.StoreType)
                {
                    case NTContactStoreType.Local:
                        if (store.Enabled) UseLocalStore = true;
                        break;
                    case NTContactStoreType.Server:
                        if (store.Enabled) UseServerStore = true;
                        break;
                    case NTContactStoreType.Outlook:
                        if (store.Enabled) UseOutlookStore = true;
                        break;
                }
            }
        }

        public void LoadAsync(UserAccount userAccount)
        {
            mStoreUserDirectory = mStoreBaseDirectory + @"\" + userAccount.Username;
            mUserAccount = userAccount;
            ThreadStart st = new ThreadStart(LoadContactsBookFunction);
            mLoadContactsBookThread = new Thread(st);
            // start the thread
            mLoadContactsBookThread.Start();
        }


        private void LoadContactsBookFunction()
        {
            try
            {
                Load(mUserAccount);
            }
            catch (Exception)
            {
                ;
            }
            Thread.CurrentThread.Abort();
        }

        public void Load(UserAccount userAccount)
        {
            mUserAccount = userAccount;

            #region Local Store
            if (UseLocalStore)
            {
                try
                {
                    XmlSerializer xser = new XmlSerializer(typeof(NTContact[]));
                    StreamReader sr = new StreamReader(mStoreUserDirectory + mStoreFilename);
                    NTContact[] contacts = (NTContact[])xser.Deserialize(sr);
                    sr.Close();
                    lock (this)
                    {
                        foreach (NTContact contact in contacts)
                        {
                            contact.NTContactStore = NTContactStoreType.Local;
                            if (!this.Contains(contact) && contact.NTDeleted != "true") this.Add(contact);
                        }
                    }
                }
                catch (Exception ex)
                {
#if (TRACE)
                    Console.WriteLine("Load Contacts Book | Local Store : Failed - " + ex.Message);
#endif
                }
            }
            #endregion

#if !(REMWAVE_LITE)
            #region Server Store
            if (UseServerStore)
            {
                try
                {
                    Remwave.Client.RSIFeaturesWS.RSIService ss = new Remwave.Client.RSIFeaturesWS.RSIService();
                    XmlSerializer xser = new XmlSerializer(typeof(NTContact[]));

                    string sout = ss.servicePhonebookGet(mUserAccount.Username, mUserAccount.Password, mProperties, null);
#if (TRACE)
                    Console.WriteLine("PHONEBOOK-GET: " + userAccount.Username + " Done");
#endif

                    StringReader sr = new StringReader(sout);
                    NTContact[] ntc = (NTContact[])xser.Deserialize(sr);
                    sr.Close();
                    lock (this)
                    {
                        foreach (NTContact contact in ntc)
                        {
                            contact.NTContactStore = NTContactStoreType.Server;
                            if (!this.Contains(contact)) this.Add(contact);
                        }
                    }

                }
                catch (Exception ex)
                {
#if (TRACE)
                    Console.WriteLine("Load Contacts Book | Server Store : Failed - " + ex.Message);
#endif
                }

            }
            #endregion

            #region Outlook Store
            if (UseOutlookStore)
            {

                Outlook.Application oApp = new Outlook.Application();
                Outlook.NameSpace oNS = oApp.GetNamespace("MAPI");
                Outlook.MAPIFolder oContactsFolder = oNS.GetDefaultFolder(Outlook.OlDefaultFolders.olFolderContacts);

                try
                {
                    string filter = "[MessageClass] = \"IPM.Contact\"";
                    Outlook.Items oContactItems = oContactsFolder.Items.Restrict(filter);
                    lock (this)
                    {
                        foreach (Outlook.ContactItem item in oContactItems)
                        {
                            try
                            {
                                NTContact contact = NTTranslator((Outlook.ContactItem)item);
                                contact.NTContactStore = NTContactStoreType.Outlook;
                                if (!this.Contains(contact)) this.Add(contact);
                            }
                            catch (Exception ex)
                            {
#if (TRACE)
                                Console.WriteLine("Load : UseOutlookStore - " + ex.Message);
#endif
                            }
                        }
                    }
                    oContactItems = null;
                }
                catch (Exception ex)
                {
#if (TRACE)
                    Console.WriteLine("Load Contacts Book | Outlook Store : Failed - " + ex.Message);
#endif
                }

                oApp = null;
                oNS = null;

            }
            #endregion
#endif
            OnUpdateCompleted(this, new EventArgs());
        }

        public void Save()
        {
            #region Local Store
            if (UseLocalStore)
            {
                try
                {
                    if (!Directory.Exists(mStoreUserDirectory))
                    {
                        Directory.CreateDirectory(mStoreUserDirectory);
                    }

                    NTContact[] contacts = new NTContact[this.Count];
                    int j = 0;
                    lock (this)
                    {
                        for (int i = this.Count - 1; i >= 0; i--)
                        {
                            if (this[i].NTContactStore == NTContactStoreType.Local && this[i].NTDeleted != "true")
                            {
                                contacts[j] = this[i];
                                j++;
                            }
                        }
                    }

                    try
                    {
                        Array.Resize(ref contacts, j);

                        XmlSerializer xser = new XmlSerializer(typeof(NTContact[]));
                        StreamWriter sw = new StreamWriter(mStoreUserDirectory + mStoreFilename);
                        xser.Serialize(sw, contacts);
                        sw.Close();
                    }
                    catch (Exception ex)
                    {

#if (TRACE)
                        Console.WriteLine("Save Contacts Book | Local Store : Failed - " + ex.Message);
#endif
                    }
                }
                catch (Exception)
                {
#if (DEBUG)
                    throw;
#endif
                }


            }
            #endregion
#if !(REMWAVE_LITE)
            #region Server Store
            if (UseServerStore)
            {
                try
                {
                    NTContact[] contacts = new NTContact[this.Count];
                    int j = 0;
                    lock (this)
                    {
                        for (int i = this.Count - 1; i >= 0; i--)
                        {
                            if (this[i].NTContactStore == NTContactStoreType.Server & (this[i].NTContactChanged | this[i].NTDeleted == "true"))
                            {
                                contacts[j] = this[i];
                                contacts[j].NTContactChanged = false;
                                j++;
                            }
                        }
                    }

                    Array.Resize(ref contacts, j);

                    Remwave.Client.RSIFeaturesWS.RSIService ss = new Remwave.Client.RSIFeaturesWS.RSIService();
                    XmlSerializer xser = new XmlSerializer(typeof(NTContact[]));
                    StringWriter sw = new StringWriter();
                    xser.Serialize(sw, contacts);
                    ss.servicePhonebookPutAsync(mUserAccount.Username, mUserAccount.Password, mProperties, sw.ToString());
                    sw.Close();
                }
                catch (Exception ex)
                {
#if (TRACE)
                    Console.WriteLine("Save Contacts Book | Server Store : Failed - " + ex.Message);
#endif
                }
            }
            #endregion

            #region Outlook Store
            if (UseOutlookStore)
            {
                try
                {
                    //TODO CREATE OR UPDATE
                    lock (this)
                    {
                        for (int i = this.Count - 1; i >= 0; i--)
                        {
                            if (this[i].NTContactStore == NTContactStoreType.Outlook && (this[i].NTContactChanged || this[i].NTDeleted == "true"))
                            {
                                try
                                {
                                    Outlook.ContactItem oItem = NTTranslator(this[i]);
                                    if (this[i].NTDeleted == "true")
                                    {
                                        oItem.Delete();
                                    }
                                    else
                                    {
                                        oItem.Save();
                                    }
                                }
                                catch (Exception ex)
                                {

#if (TRACE)
                                    Console.WriteLine("Save Contacts Book | Outlook Store : Failed - " + ex.Message);
#endif
                                }
                                this[i].NTContactChanged = false;
                            }

                        }
                    }


                }
                catch (Exception)
                {
#if (DEBUG)
                    throw;
#endif
                }


            }
            #endregion
#endif
        }

        public void Modified()
        {
            OnUpdateCompleted(this, new EventArgs());
        }


        ~ContactBook()
        {
            Save();
        }







        #region NTTranslators
        private NTContact NTTranslator(Outlook.ContactItem oItem)
        {
            NTContact item = new NTContact();
            item.NTContactStore = NTContactStoreType.Outlook;
            //oItem.PropertyChange += new Microsoft.Office.Interop.Outlook.ItemEvents_10_PropertyChangeEventHandler(oItem_PropertyChange);
            item.NTContactStore = NTContactStoreType.Outlook;
            item.NTUsername = oItem.User1;
            item.NTSpeedDial = "";
            item.NTDeleted = "";
            item.NTVoIPTelephoneNumber = "";
            item.NTAccountName = oItem.Account;
            item.NTAnniversary = oItem.Anniversary.ToString();
            item.NTAssistantName = oItem.AssistantName;
            item.NTAssistantTelephoneNumber = oItem.AssistantTelephoneNumber;
            item.NTBirthday = oItem.Birthday.ToString();
            item.NTBody = oItem.Body;
            item.NTBusiness2TelephoneNumber = oItem.Business2TelephoneNumber;
            item.NTBusinessAddressCity = oItem.BusinessAddressCity;
            item.NTBusinessAddressCountry = oItem.BusinessAddressCountry;
            item.NTBusinessAddressPostalCode = oItem.BusinessAddressPostalCode;
            item.NTBusinessAddressState = oItem.BusinessAddressState;
            item.NTBusinessAddressStreet = oItem.BusinessAddressStreet;
            item.NTBusinessFaxNumber = oItem.BusinessFaxNumber;
            item.NTBusinessTelephoneNumber = oItem.BusinessTelephoneNumber;
            item.NTCarTelephoneNumber = oItem.CarTelephoneNumber;
            item.NTCategories = oItem.Categories;
            item.NTChildren = oItem.Children;
            item.NTCompanyName = oItem.CompanyName;
            item.NTCompanyTelephoneNumber = oItem.CompanyMainTelephoneNumber;
            item.NTCustomerId = oItem.CustomerID;
            item.NTDepartment = oItem.Department;
            item.NTEmail1Address = oItem.Email1Address;
            item.NTEmail2Address = oItem.Email2Address;
            item.NTEmail3Address = oItem.Email3Address;
            item.NTFileAs = oItem.FileAs;
            item.NTFirstName = oItem.FirstName;
            item.NTGovernmentId = oItem.GovernmentIDNumber;
            item.NTHome2TelephoneNumber = oItem.Home2TelephoneNumber;
            item.NTHomeAddressCity = oItem.HomeAddressCity;
            item.NTHomeAddressCountry = oItem.HomeAddressCountry;
            item.NTHomeAddressPostalCode = oItem.HomeAddressPostalCode;
            item.NTHomeAddressState = oItem.HomeAddressState;
            item.NTHomeAddressStreet = oItem.HomeAddressStreet;
            item.NTHomeFaxNumber = oItem.HomeFaxNumber;
            item.NTHomeTelephoneNumber = oItem.HomeTelephoneNumber;
            item.NTIM1Address = oItem.IMAddress;
            item.NTIM2Address = "";
            item.NTIM3Address = "";
            item.NTItemId = oItem.EntryID;
            item.NTJobTitle = oItem.JobTitle;
            item.NTLastName = oItem.LastName;
            item.NTManager = oItem.ManagerName;
            item.NTMiddleName = oItem.MiddleName;
            item.NTMobileTelephoneNumber = oItem.MobileTelephoneNumber;
            item.NTNickname = oItem.NickName;
            item.NTOfficeLocation = oItem.OfficeLocation;
            item.NTOtherAddressCity = oItem.OtherAddressCity;
            item.NTOtherAddressCountry = oItem.OtherAddressCountry;
            item.NTOtherAddressPostalCode = oItem.OtherAddressPostalCode;
            item.NTOtherAddressState = oItem.OtherAddressState;
            item.NTOtherAddressStreet = oItem.OtherAddressStreet;
            item.NTPagerNumber = oItem.PagerNumber;
            item.NTPicture = "";
            item.NTProperties = "";
            item.NTRadioTelephoneNumber = oItem.RadioTelephoneNumber;
            item.NTRingTone = "";
            item.NTSpouse = oItem.Spouse;
            item.NTSuffix = oItem.Suffix;
            item.NTTitle = oItem.Title;
            item.NTWebPage = oItem.WebPage;
            item.NTYomiCompanyName = oItem.YomiCompanyName;
            item.NTYomiFirstName = oItem.YomiFirstName;
            item.NTYomiLastName = oItem.YomiLastName;
            item.NTJabberID = oItem.IMAddress;

            return item;
        }
        private Outlook.ContactItem NTTranslator(NTContact item)
        {
            Outlook.Application outlookApp = new Outlook.Application();
            Outlook.NameSpace oNS = outlookApp.GetNamespace("MAPI");
            Outlook.MAPIFolder oContactsFolder = oNS.GetDefaultFolder(Outlook.OlDefaultFolders.olFolderContacts);
            Outlook.ContactItem oItem = null;

            try
            {
                oItem = (Outlook.ContactItem)oNS.GetItemFromID(item.NTItemId, oContactsFolder.StoreID);
            }
            catch (Exception)
            {

            }
            if (oItem == null) oItem = (Outlook.ContactItem)outlookApp.CreateItem(Outlook.OlItemType.olContactItem);

            oItem.User1 = item.NTUsername;

            oItem.Account = item.NTAccountName;
            //item.NTAnniversary = oItem.Anniversary.ToString();
            //item.NTAssistantName = oItem.AssistantName;
            //item.NTAssistantTelephoneNumber = oItem.AssistantTelephoneNumber;
            //item.NTBirthday = oItem.Birthday.ToString();
            //item.NTBody = oItem.Body;
            //item.NTBusiness2TelephoneNumber = oItem.Business2TelephoneNumber;
            //item.NTBusinessAddressCity = oItem.BusinessAddressCity;
            //item.NTBusinessAddressCountry = oItem.BusinessAddressCountry;
            //item.NTBusinessAddressPostalCode = oItem.BusinessAddressPostalCode;
            //item.NTBusinessAddressState = oItem.BusinessAddressState;
            //item.NTBusinessAddressStreet = oItem.BusinessAddressStreet;
            //item.NTBusinessFaxNumber = oItem.BusinessFaxNumber;
            //item.NTBusinessTelephoneNumber = oItem.BusinessTelephoneNumber;
            //item.NTCarTelephoneNumber = oItem.CarTelephoneNumber;
            //item.NTCategories = oItem.Categories;
            //item.NTChildren = oItem.Children;
            //item.NTCompanyName = oItem.CompanyName;
            oItem.CompanyMainTelephoneNumber = item.NTCompanyTelephoneNumber;
            //item.NTCustomerId = oItem.CustomerID;
            //item.NTDepartment = oItem.Department;
            oItem.Email1Address = item.NTEmail1Address;
            //item.NTEmail2Address = oItem.Email2Address;
            //item.NTEmail3Address = oItem.Email3Address;
            //item.NTFileAs = oItem.FileAs;
            oItem.FirstName = item.NTFirstName;
            //item.NTGovernmentId = oItem.GovernmentIDNumber;
            //item.NTHome2TelephoneNumber = oItem.Home2TelephoneNumber;
            oItem.HomeAddressCity = item.NTHomeAddressCity;
            oItem.HomeAddressCountry = item.NTHomeAddressCountry;
            oItem.HomeAddressPostalCode = item.NTHomeAddressPostalCode;
            oItem.HomeAddressState = item.NTHomeAddressState;
            oItem.HomeAddressStreet = item.NTHomeAddressStreet;
            oItem.HomeFaxNumber = item.NTHomeFaxNumber;
            oItem.HomeTelephoneNumber = item.NTHomeTelephoneNumber;
            //item.NTIM2Address = "";
            //item.NTIM3Address = "";
            //oItem.EntryID =item.NTItemId;
            //item.NTJobTitle = oItem.JobTitle;
            oItem.LastName = item.NTLastName;
            //item.NTManager = oItem.ManagerName;
            oItem.MiddleName = item.NTMiddleName;
            oItem.MobileTelephoneNumber = item.NTMobileTelephoneNumber;
            //Item.NickName = item.NTNickname
            //item.NTOfficeLocation = oItem.OfficeLocation;
            //item.NTOtherAddressCity = oItem.OtherAddressCity;
            //item.NTOtherAddressCountry = oItem.OtherAddressCountry;
            //item.NTOtherAddressPostalCode = oItem.OtherAddressPostalCode;
            //item.NTOtherAddressState = oItem.OtherAddressState;
            //item.NTOtherAddressStreet = oItem.OtherAddressStreet;
            //item.NTPagerNumber = oItem.PagerNumber;
            //item.NTPicture = "";
            //item.NTProperties = "";
            //item.NTRadioTelephoneNumber = oItem.RadioTelephoneNumber;
            //item.NTRingTone = "";
            //item.NTSpouse = oItem.Spouse;
            //item.NTSuffix = oItem.Suffix;
            //item.NTTitle = oItem.Title;
            //item.NTWebPage = oItem.WebPage;
            //item.NTYomiCompanyName = oItem.YomiCompanyName;
            //item.NTYomiFirstName = oItem.YomiFirstName;
            //item.NTYomiLastName = oItem.YomiLastName;
            oItem.IMAddress = item.NTJabberID;

            return oItem;
        }
        #endregion
    }
}
