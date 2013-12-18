using System;
using System.Collections;
using System.Xml.Serialization;
using System.IO;
using System.Reflection;

namespace Remwave.Client
{
    /// <summary>
    /// Autor: michael koehler
    /// Summary description for ContactContainer.
    /// </summary>
    [Serializable()]
    public class ContactContainer
    {
        /// <summary>
        /// class attributes
        /// </summary>

        [XmlIgnore]
        public static bool serializing = false;

        [XmlIgnore]
        private ArrayList contactList;

        public ContactContainer()
        {
            contactList = new ArrayList();
        }

        public ArrayList Container
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

        public static ContactContainer DefaultInstance()
        {
            return new ContactContainer();
        }
    }

    [Serializable()]

    public class NTContact : ICloneable
    {
        private string AccountName = "";
        private string Anniversary = "";
        private string AssistantName = "";
        private string AssistantTelephoneNumber = "";
        private string Birthday = "";
        private string Body = "";
        private string Business2TelephoneNumber = "";
        private string BusinessAddressCity = "";
        private string BusinessAddressCountry = "";
        private string BusinessAddressPostalCode = "";
        private string BusinessAddressState = "";
        private string BusinessAddressStreet = "";
        private string BusinessFaxNumber = "";
        private string BusinessTelephoneNumber = "";
        private string CarTelephoneNumber = "";
        private string Categories = "";
        private string Children = "";
        private string CompanyName = "";
        private string CompanyTelephoneNumber = "";
        private string CustomerId = "";
        private string Department = "";
        private string Email1Address = "";
        private string Email2Address = "";
        private string Email3Address = "";
        private string FileAs = "";
        private string FirstName = "";
        private string GovernmentId = "";
        private string Home2TelephoneNumber = "";
        private string HomeAddressCity = "";
        private string HomeAddressCountry = "";
        private string HomeAddressPostalCode = "";
        private string HomeAddressState = "";
        private string HomeAddressStreet = "";
        private string HomeFaxNumber = "";
        private string HomeTelephoneNumber = "";
        private string IM1Address = "";
        private string IM2Address = "";
        private string IM3Address = "";
        private string ItemId = "";
        private string JobTitle = "";
        private string LastName = "";
        private string Manager = "";
        private string MiddleName = "";
        private string MobileTelephoneNumber = "";
        private string Nickname = "";
        private string OfficeLocation = "";
        private string OtherAddressCity = "";
        private string OtherAddressCountry = "";
        private string OtherAddressPostalCode = "";
        private string OtherAddressState = "";
        private string OtherAddressStreet = "";
        private string PagerNumber = "";
        private string Picture = "";
        private string Properties = "";
        private string RadioTelephoneNumber = "";
        private string RingTone = "";
        private string Spouse = "";
        private string Suffix = "";
        private string Title = "";
        private string WebPage = "";
        private string YomiCompanyName = "";
        private string YomiFirstName = "";
        private string YomiLastName = "";
        private string Username = "";
        private string SpeedDial = "";
        private string Deleted = "";
        private string VoIPTelephoneNumber = "";
        private string JabberID = "";

        [XmlIgnore]
        public int score;
        [XmlIgnore]
        public bool changed = false;

        public string FullName()
        {
            return FullName(true);
        }

        public string FullName(bool firstLast)
        {
            if (firstLast)
            {
                return FirstName + ' ' + LastName;
            }
            else
            {
                return LastName + ", " + FirstName;
            }
        }

        public string PrimaryPhoneNumbers()
        {
            string myPrimaryPhoneNumbers="";

            myPrimaryPhoneNumbers += HomeTelephoneNumber != "" ? HomeTelephoneNumber + "," : "";
            myPrimaryPhoneNumbers += MobileTelephoneNumber != "" ? "m " + MobileTelephoneNumber + "," : "";
            myPrimaryPhoneNumbers += BusinessTelephoneNumber != "" ? "b " + BusinessTelephoneNumber + "," : "";
            return myPrimaryPhoneNumbers;
        }

        public static string GenerateUniqueID()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            Random rnd = new Random();

            sb.Append("RSI");
            sb.Append("-");
            sb.Append(System.DateTime.Now.Ticks);
            sb.Append("-");
            sb.Append(rnd.Next());
            sb.Append(rnd.Next());
            sb.Append(rnd.Next());
            sb.Append("-");
            sb.Append(rnd.Next());
            sb.Append(rnd.Next());
            sb.Append(rnd.Next());
            sb.Append("-");
            sb.Append(rnd.Next());
            sb.Append(rnd.Next());
            sb.Append(rnd.Next());
            return sb.ToString();
        }


        public NTContact()
        {
            this.ItemId = GenerateUniqueID();
        }

        #region accessor members
        public string NTUsername { get { return Username; } set { Username = value!=null?value.Trim():""; } }
        public string NTSpeedDial { get { return SpeedDial; } set { SpeedDial = value != null ? value.Trim() : ""; } }
        public string NTDeleted { get { return Deleted; } set { Deleted = value != null ? value.Trim() : ""; } }
        public string NTVoIPTelephoneNumber { get { return VoIPTelephoneNumber; } set { VoIPTelephoneNumber = value != null ? value.Trim() : ""; } }

        public string NTAccountName { get { return AccountName; } set { AccountName = value != null ? value.Trim() : ""; } }
        public string NTAnniversary { get { return Anniversary; } set { Anniversary = value != null ? value.Trim() : ""; } }
        public string NTAssistantName { get { return AssistantName; } set { AssistantName = value != null ? value.Trim() : ""; } }
        public string NTAssistantTelephoneNumber { get { return AssistantTelephoneNumber; } set { AssistantTelephoneNumber = value != null ? value.Trim() : ""; } }
        public string NTBirthday { get { return Birthday; } set { Birthday = value != null ? value.Trim() : ""; } }
        public string NTBody { get { return Body; } set { Body = value != null ? value.Trim() : ""; } }
        public string NTBusiness2TelephoneNumber { get { return Business2TelephoneNumber; } set { Business2TelephoneNumber = value != null ? value.Trim() : ""; } }
        public string NTBusinessAddressCity { get { return BusinessAddressCity; } set { BusinessAddressCity = value != null ? value.Trim() : ""; } }
        public string NTBusinessAddressCountry { get { return BusinessAddressCountry; } set { BusinessAddressCountry = value != null ? value.Trim() : ""; } }
        public string NTBusinessAddressPostalCode { get { return BusinessAddressPostalCode; } set { BusinessAddressPostalCode = value != null ? value.Trim() : ""; } }
        public string NTBusinessAddressState { get { return BusinessAddressState; } set { BusinessAddressState = value != null ? value.Trim() : ""; } }
        public string NTBusinessAddressStreet { get { return BusinessAddressStreet; } set { BusinessAddressStreet = value != null ? value.Trim() : ""; } }
        public string NTBusinessFaxNumber { get { return BusinessFaxNumber; } set { BusinessFaxNumber = value != null ? value.Trim() : ""; } }
        public string NTBusinessTelephoneNumber { get { return BusinessTelephoneNumber; } set { BusinessTelephoneNumber = value != null ? value.Trim() : ""; } }
        public string NTCarTelephoneNumber { get { return CarTelephoneNumber; } set { CarTelephoneNumber = value != null ? value.Trim() : ""; } }
        public string NTCategories { get { return Categories; } set { Categories = value != null ? value.Trim() : ""; } }
        public string NTChildren { get { return Children; } set { Children = value != null ? value.Trim() : ""; } }
        public string NTCompanyName { get { return CompanyName; } set { CompanyName = value != null ? value.Trim() : ""; } }
        public string NTCompanyTelephoneNumber { get { return CompanyTelephoneNumber; } set { CompanyTelephoneNumber = value != null ? value.Trim() : ""; } }
        public string NTCustomerId { get { return CustomerId; } set { CustomerId = value != null ? value.Trim() : ""; } }
        public string NTDepartment { get { return Department; } set { Department = value != null ? value.Trim() : ""; } }
        public string NTEmail1Address { get { return Email1Address; } set { Email1Address = value != null ? value.Trim() : ""; } }
        public string NTEmail2Address { get { return Email2Address; } set { Email2Address = value != null ? value.Trim() : ""; } }
        public string NTEmail3Address { get { return Email3Address; } set { Email3Address = value != null ? value.Trim() : ""; } }
        public string NTFileAs { get { return FileAs; } set { FileAs = value != null ? value.Trim() : ""; } }
        public string NTFirstName { get { return FirstName; } set { FirstName = value != null ? value.Trim() : ""; } }
        public string NTGovernmentId { get { return GovernmentId; } set { GovernmentId = value != null ? value.Trim() : ""; } }
        public string NTHome2TelephoneNumber { get { return Home2TelephoneNumber; } set { Home2TelephoneNumber = value != null ? value.Trim() : ""; } }
        public string NTHomeAddressCity { get { return HomeAddressCity; } set { HomeAddressCity = value != null ? value.Trim() : ""; } }
        public string NTHomeAddressCountry { get { return HomeAddressCountry; } set { HomeAddressCountry = value != null ? value.Trim() : ""; } }
        public string NTHomeAddressPostalCode { get { return HomeAddressPostalCode; } set { HomeAddressPostalCode = value != null ? value.Trim() : ""; } }
        public string NTHomeAddressState { get { return HomeAddressState; } set { HomeAddressState = value != null ? value.Trim() : ""; } }
        public string NTHomeAddressStreet { get { return HomeAddressStreet; } set { HomeAddressStreet = value != null ? value.Trim() : ""; } }
        public string NTHomeFaxNumber { get { return HomeFaxNumber; } set { HomeFaxNumber = value != null ? value.Trim() : ""; } }
        public string NTHomeTelephoneNumber { get { return HomeTelephoneNumber; } set { HomeTelephoneNumber = value != null ? value.Trim() : ""; } }
        public string NTIM1Address { get { return IM1Address; } set { IM1Address = value != null ? value.Trim() : ""; } }
        public string NTIM2Address { get { return IM2Address; } set { IM2Address = value != null ? value.Trim() : ""; } }
        public string NTIM3Address { get { return IM3Address; } set { IM3Address = value != null ? value.Trim() : ""; } }
        public string NTItemId { get { return ItemId; } set { ItemId = value != null ? value.Trim() : ""; } }
        public string NTJobTitle { get { return JobTitle; } set { JobTitle = value != null ? value.Trim() : ""; } }
        public string NTLastName { get { return LastName; } set { LastName = value != null ? value.Trim() : ""; } }
        public string NTManager { get { return Manager; } set { Manager = value != null ? value.Trim() : ""; } }
        public string NTMiddleName { get { return MiddleName; } set { MiddleName = value != null ? value.Trim() : ""; } }
        public string NTMobileTelephoneNumber { get { return MobileTelephoneNumber; } set { MobileTelephoneNumber = value != null ? value.Trim() : ""; } }
        public string NTNickname { get { return Nickname; } set { Nickname = value != null ? value.Trim() : ""; } }
        public string NTOfficeLocation { get { return OfficeLocation; } set { OfficeLocation = value != null ? value.Trim() : ""; } }
        public string NTOtherAddressCity { get { return OtherAddressCity; } set { OtherAddressCity = value != null ? value.Trim() : ""; } }
        public string NTOtherAddressCountry { get { return OtherAddressCountry; } set { OtherAddressCountry = value != null ? value.Trim() : ""; } }
        public string NTOtherAddressPostalCode { get { return OtherAddressPostalCode; } set { OtherAddressPostalCode = value != null ? value.Trim() : ""; } }
        public string NTOtherAddressState { get { return OtherAddressState; } set { OtherAddressState = value != null ? value.Trim() : ""; } }
        public string NTOtherAddressStreet { get { return OtherAddressStreet; } set { OtherAddressStreet = value != null ? value.Trim() : ""; } }
        public string NTPagerNumber { get { return PagerNumber; } set { PagerNumber = value != null ? value.Trim() : ""; } }
        public string NTPicture { get { return Picture; } set { Picture = value != null ? value.Trim() : ""; } }
        public string NTProperties { get { return Properties; } set { Properties = value != null ? value.Trim() : ""; } }
        public string NTRadioTelephoneNumber { get { return RadioTelephoneNumber; } set { RadioTelephoneNumber = value != null ? value.Trim() : ""; } }
        public string NTRingTone { get { return RingTone; } set { RingTone = value != null ? value.Trim() : ""; } }
        public string NTSpouse { get { return Spouse; } set { Spouse = value != null ? value.Trim() : ""; } }
        public string NTSuffix { get { return Suffix; } set { Suffix = value != null ? value.Trim() : ""; } }
        public string NTTitle { get { return Title; } set { Title = value != null ? value.Trim() : ""; } }
        public string NTWebPage { get { return WebPage; } set { WebPage = value != null ? value.Trim() : ""; } }
        public string NTYomiCompanyName { get { return YomiCompanyName; } set { YomiCompanyName = value != null ? value.Trim() : ""; } }
        public string NTYomiFirstName { get { return YomiFirstName; } set { YomiFirstName = value != null ? value.Trim() : ""; } }
        public string NTYomiLastName { get { return YomiLastName; } set { YomiLastName = value != null ? value.Trim() : ""; } }
        public string NTJabberID { get { return JabberID; } set { JabberID = value != null ? value.Trim() : ""; } }
        #endregion

        #region ICloneable Members

        public object Clone()
        {
            NTContact copy = new NTContact();
            return (NTContact)this.MemberwiseClone();

        }

        #endregion
    }
}

