using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.IO;
using Remwave.Services;

namespace Remwave.Client
{
    public partial class ContactsWindow : Form
    {
        public bool Accepted = false;
        private ClientForm myClientForm;
        private ContactBook mContactBook;
        public NTContact mNTContact;
        private Hashtable mProperties;

        private void BrandComponent()
        {
            this.Icon = Properties.Resources.desktop;
#if !BRAND_JOCCOME

            this.myContactCancelCloseButton.ThemeName = "Office2007Black";

            this.myContactSaveCloseButton.ThemeName = "Office2007Black";
#endif
        }

        public ContactsWindow(ClientForm clientForm, NTContact selectedMyContact, Hashtable properties)
        {
            myClientForm = clientForm;
            mContactBook = myClientForm.mContactBook;
            mContactBook.UpdateCompleted += new EventHandler(mContactBook_UpdateCompleted);
            mProperties = properties;

            InitializeComponent();
            LocalizeComponent();
            BrandComponent();

            this.ClientSize = this.Size;
            this.MaximizeBox = false;
            this.MaximumSize = this.Size;


            LoadContact(selectedMyContact);


        }

        private void LoadContact(NTContact selectedMyContact)
        {
            if (selectedMyContact != null)
            {
                mNTContact = selectedMyContact;
            }
            else
            {
                mNTContact = new NTContact();
            }

            myContactJabberGroupListBox.Items.Clear();
            myContactJabberGroupListBox.Items.Add("");
            myContactJabberIMNetworkListBox.DataSource = Enum.GetValues(typeof(ConfigXMPPNetwork));
            myContactJabberIMNetworkListBox.SelectedIndex = 0;
            myContactStoreComboBox.Items.Clear();

            foreach (NTContactStore store in myClientForm.mContactBook.ContactStores)
            {
                if (store.Enabled)
                {
                    myContactStoreComboBox.Items.Add(store);
                }
                if (mNTContact.NTContactStore == store.StoreType) myContactStoreComboBox.SelectedItem = store;
            }

            myContactJabberIDInput.Text = mNTContact.NTJabberID;

            myContactFirstNameInput.Text = mNTContact.NTFirstName;
            myContactMiddleNameInput.Text = mNTContact.NTMiddleName;
            myContactLastNameInput.Text = mNTContact.NTLastName;
            myContactPhoneHomeInput.Text = mNTContact.NTHomeTelephoneNumber;
            myContactPhoneMobileInput.Text = mNTContact.NTMobileTelephoneNumber;
            myContactPhoneBusinessInput.Text = mNTContact.NTBusinessTelephoneNumber;
            myContactPhoneVoIPInput.Text = mNTContact.NTVoIPTelephoneNumber;

            myContactJabberIDInput.Text = mNTContact.NTJabberID;

            myContactAddressStreetInput.Text = mNTContact.NTHomeAddressStreet;
            myContactAddressCityInput.Text = mNTContact.NTHomeAddressCity;
            myContactAddressZipCodeInput.Text = mNTContact.NTHomeAddressPostalCode;
            myContactAddressStateInput.Text = mNTContact.NTHomeAddressState;
            myContactAddressCountryInput.Text = mNTContact.NTHomeAddressCountry;

            myContactAddressEmailInput.Text = mNTContact.NTEmail1Address;

            try
            {
                myContactPicture.Image = myContactPicture.Image = ImageProcessing.FixedSize(ImageProcessing.FromString(mNTContact.NTPicture), 64, 64);
            }
            catch (Exception)
            {

            }


            if (mProperties != null)
            {
                myContactFirstNameInput.Text = mProperties["NTFirstName"] != null ? mProperties["NTFirstName"].ToString() : mNTContact.NTFirstName;
                myContactMiddleNameInput.Text = mProperties["NTMiddleName"] != null ? mProperties["NTMiddleName"].ToString() : mNTContact.NTMiddleName;
                myContactLastNameInput.Text = mProperties["NTLastName"] != null ? mProperties["NTLastName"].ToString() : mNTContact.NTLastName;
                myContactPhoneHomeInput.Text = mProperties["NTHomeTelephoneNumber"] != null ? mProperties["NTHomeTelephoneNumber"].ToString() : mNTContact.NTHomeTelephoneNumber;
                myContactPhoneMobileInput.Text = mProperties["NTMobileTelephoneNumber"] != null ? mProperties["NTMobileTelephoneNumber"].ToString() : mNTContact.NTMobileTelephoneNumber;
                myContactPhoneBusinessInput.Text = mProperties["NTBusinessTelephoneNumber"] != null ? mProperties["NTBusinessTelephoneNumber"].ToString() : mNTContact.NTBusinessTelephoneNumber;
                myContactPhoneVoIPInput.Text = mProperties["NTVoIPTelephoneNumber"] != null ? mProperties["NTVoIPTelephoneNumber"].ToString() : mNTContact.NTVoIPTelephoneNumber;
                myContactJabberIDInput.Text = mProperties["NTJabberID"] != null ? mProperties["NTJabberID"].ToString() : mNTContact.NTJabberID;

                myContactAddressStreetInput.Text = mProperties["NTHomeAddressStreet"] != null ? mProperties["NTHomeAddressStreet"].ToString() : mNTContact.NTHomeAddressStreet;
                myContactAddressCityInput.Text = mProperties["NTHomeAddressCity"] != null ? mProperties["NTHomeAddressCity"].ToString() : mNTContact.NTHomeAddressCity;
                myContactAddressZipCodeInput.Text = mProperties["NTHomeAddressPostalCode"] != null ? mProperties["NTHomeAddressPostalCode"].ToString() : mNTContact.NTHomeAddressPostalCode;
                myContactAddressStateInput.Text = mProperties["NTHomeAddressState"] != null ? mProperties["NTHomeAddressState"].ToString() : mNTContact.NTHomeAddressState;
                myContactAddressCountryInput.Text = mProperties["NTHomeAddressCountry"] != null ? mProperties["NTHomeAddressCountry"].ToString() : mNTContact.NTHomeAddressCountry;

                myContactAddressEmailInput.Text = mProperties["NTEmail1Address"] != null ? mProperties["NTEmail1Address"].ToString() : mNTContact.NTEmail1Address;
            }

            foreach (DictionaryEntry group in myClientForm.myBuddyGroups)
            {
                if (group.Value.ToString() != "" && !myContactJabberGroupListBox.Items.Contains(group.Value.ToString()))
                    myContactJabberGroupListBox.Items.Add(group.Value.ToString());
            }

            JabberUser jabberUser = null;
            if (myContactJabberIDInput.Text != "")
            {
                jabberUser = new JabberUser(myContactJabberIDInput.Text);
                myContactJabberIDInput.Text = jabberUser.Username;
                myContactJabberIMNetworkListBox.SelectedItem = jabberUser.Network;
                myContactJabberGroupListBox.Text = myClientForm.myBuddyList[jabberUser.JID] != null ? myClientForm.myBuddyList[jabberUser.JID].ToString() : Properties.Localization.txtOtherGroup;
            }





            if (myClientForm.mUserAccount.Username == myContactJabberIDInput.Text)
            {
                myContactJabberIDInput.Enabled = false;
                myContactJabberGroupBox.Visible = false;

                myContactStoreComboBox.Enabled = false;
                myContactStoreComboBox.Visible = false;

                labelStore.Visible = false;
            }

        }

        void mContactBook_UpdateCompleted(object sender, EventArgs e)
        {
            try
            {

                if (mProperties==null  | mProperties["NTJabberID"] == null) return;
                NTContact contact = null;
                ContactList contactList = mContactBook.getCandidatesForJabberID(mProperties["NTJabberID"].ToString());

                for (int i = contactList.Count - 1; i >= 0; i--)
                {
                    if (contactList[i].NTContactStore == NTContactStoreType.vCard)
                    {
                        contact = contactList[i];
                    }
                }

                if (contact != null)
                {
                    LoadContact(contact);
                }
            }
            catch (Exception)
            {

                throw;
            }

        }

        private void LocalizeComponent()
        {
            myContactNameDetailsGroupBox.Text = Properties.Localization.txtCFormTitleNameDetails;
            myContactLastNameLabel.Text = Properties.Localization.txtCFormTitleLastName;
            myContactMiddleNameLabel.Text = Properties.Localization.txtCFormTitleMiddleName;
            myContactFirstNameLabel.Text = Properties.Localization.txtCFormTitleFirstName;
            myContactPhonesGroupBox.Text = Properties.Localization.txtCFormTitlePhones;
            myContactPhoneVoIPLabel.Text = Properties.Localization.txtCFormTitlePhoneVoIP;
            myContactPhoneBusinessLabel.Text = Properties.Localization.txtCFormTitlePhoneBusiness;
            myContactPhoneMobileLabel.Text = Properties.Localization.txtCFormTitlePhoneMobile;
            myContactPhoneHomeLabel.Text = Properties.Localization.txtCFormTitlePhoneHome;
            myContactJabberGroupBox.Text = Properties.Localization.txtCFormTitleIMAddress;
            myContactJabberGroupLabel.Text = Properties.Localization.txtCFormTitleIMGroup;
            myContactJabberIDLabel.Text = Properties.Localization.txtCFormTitleIMUsername;
            myContactAddressGroupBox.Text = Properties.Localization.txtCFormTitleAddress;
            myContactAddressEmailLabel.Text = Properties.Localization.txtCFormTitleAddressEmail;
            myContactAddressZipCodeLabel.Text = Properties.Localization.txtCFormTitleAddressZipCode;
            myContactAddressCountryLabel.Text = Properties.Localization.txtCFormTitleAddressCountry;
            myContactAddressStateLabel.Text = Properties.Localization.txtCFormTitleAddressState;
            myContactAddressCityLabel.Text = Properties.Localization.txtCFormTitleAddressCity;
            myContactAddressStreetLabel.Text = Properties.Localization.txtCFormTitleAddressStreet;
            myContactSaveCloseButton.Text = Properties.Localization.txtCFormBtnSave;
            myContactPictureGroupBox.Text = Properties.Localization.txtCFormTitlePicture;
            btnChangePicture.Text = Properties.Localization.txtCFormTitlePictureChange;
            myContactWindowDescriptionLabel.Text = Properties.Localization.txtCFormInfoDesc;
            myContactWindowTitleLabel.Text = Properties.Localization.txtCFormInfo;
            myContactJabberIMNetworkLabel.Text = Properties.Localization.txtCFormTitleIMNetwork;
            Text = Properties.Localization.txtCFormTitle;
        }

        private void myContactSaveCloseButton_Click(object sender, EventArgs e)
        {
            Accepted = true;

            mNTContact.NTContactChanged = true;

            try
            {
                mNTContact.NTContactStore = ((NTContactStore)myContactStoreComboBox.SelectedItem).StoreType;
            }
            catch (Exception)
            {
                mNTContact.NTContactStore = NTContactStoreType.Local;
            }


            mNTContact.NTFirstName = myContactFirstNameInput.Text;
            mNTContact.NTMiddleName = myContactMiddleNameInput.Text;
            mNTContact.NTLastName = myContactLastNameInput.Text;

            mNTContact.NTHomeTelephoneNumber = myContactPhoneHomeInput.Text;
            mNTContact.NTMobileTelephoneNumber = myContactPhoneMobileInput.Text;
            mNTContact.NTBusinessTelephoneNumber = myContactPhoneBusinessInput.Text;
            mNTContact.NTVoIPTelephoneNumber = myContactPhoneVoIPInput.Text; ;


            mNTContact.NTJabberID = "";
            if (myContactJabberIDInput.Text.Trim() != "")
            {
                JabberUser jabberUser;
                ConfigXMPPNetwork selectedIMNetwork = ConfigXMPPNetwork.Default;

                try
                {
                    selectedIMNetwork = (ConfigXMPPNetwork)myContactJabberIMNetworkListBox.SelectedItem;
                }
                catch (Exception)
                {
                    selectedIMNetwork = ConfigXMPPNetwork.Default;
                }

                String domain = ConfigIM.GetXMPPDomain(selectedIMNetwork);
                jabberUser = new JabberUser(myContactJabberIDInput.Text.Trim() + @"@" + domain);


                mNTContact.NTJabberID = jabberUser.JID;
                mNTContact.NTUsername = jabberUser.Username;
                mNTContact.NTNickname = jabberUser.Nick;
            }

            mNTContact.NTHomeAddressStreet = myContactAddressStreetInput.Text;
            mNTContact.NTHomeAddressCity = myContactAddressCityInput.Text;
            mNTContact.NTHomeAddressPostalCode = myContactAddressZipCodeInput.Text;
            mNTContact.NTHomeAddressState = myContactAddressStateInput.Text;
            mNTContact.NTHomeAddressCountry = myContactAddressCountryInput.Text;

            mNTContact.NTEmail1Address = myContactAddressEmailInput.Text;

            if (mContactBook.IndexOf(mNTContact) < 0)
            {
                mContactBook.Add(mNTContact);
            }
            this.Close();
        }

        private void myContactFirstNameInput_TextChanged(object sender, EventArgs e)
        {
            //  myContactJabberAliasInput.Text = myContactFirstNameInput.Text + " " + myContactLastNameInput.Text;
        }

        private void myContactLastNameInput_TextChanged(object sender, EventArgs e)
        {
            //   myContactJabberAliasInput.Text = myContactFirstNameInput.Text + " " + myContactLastNameInput.Text;
        }

        private void btnChangePicture_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

            fileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);

            if (DialogResult.OK == fileDialog.ShowDialog())
            {
                if (fileDialog.FileName != null)
                {
                    //Processimge
                    try
                    {

                        myContactPicture.Image = ImageProcessing.FixedSize(new Bitmap(fileDialog.FileName), 64, 64);
                        mNTContact.NTPicture = ImageProcessing.ToString(myContactPicture.Image);
                    }
                    catch (Exception)
                    {
#if (DEBUG)
                        throw;
#endif
                    }
                }
            }
        }

        private void myContactCancelCloseButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ContactsWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            mContactBook.UpdateCompleted -= new EventHandler(mContactBook_UpdateCompleted);
        }


    }
}