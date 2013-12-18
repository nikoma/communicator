using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using Telerik.WinControls.UI;
using Remwave.Services;

namespace Remwave.Client
{
    public partial class ContactsWindow : ShapedForm
    {
        public bool Accepted = false;
        private ClientForm myClientForm;
        private ContactBook mContactBook;
        public NTContact myNTContact;
        public ContactsWindow(ClientForm clientForm, NTContact selectedMyContact, Hashtable properties)
        {
            InitializeComponent();
            LocalizeComponent();
            this.ClientSize = this.Size;
            this.MaximizeBox = false;
            this.MaximumSize = this.Size;

            myClientForm = clientForm;
            mContactBook = myClientForm.mContactBook;
            if (selectedMyContact != null)
            {
                myNTContact = selectedMyContact;
            }
            else
            {
                myNTContact = new NTContact();
            }

            myContactJabberGroupListBox.Items.Clear();
            myContactJabberIMNetworkListBox.DataSource = Enum.GetValues(typeof(ConfigXMPPNetwork));
            myContactJabberIMNetworkListBox.SelectedIndex = 0;

            comboBox1.DataSource = Enum.GetValues(typeof(NTContactStore));

            myContactJabberIDInput.Text = myNTContact.NTJabberID;
			
			
			myContactJabberGroupListBox.Items.Clear();
            myContactJabberGroupListBox.Items.Add("");         
            myContactFirstNameInput.Text = myNTContact.NTFirstName;
            myContactMiddleNameInput.Text = myNTContact.NTMiddleName;
            myContactLastNameInput.Text = myNTContact.NTLastName;
            myContactPhoneHomeInput.Text = myNTContact.NTHomeTelephoneNumber;
            myContactPhoneMobileInput.Text = myNTContact.NTMobileTelephoneNumber;
            myContactPhoneBusinessInput.Text = myNTContact.NTBusinessTelephoneNumber;
            myContactPhoneVoIPInput.Text = myNTContact.NTVoIPTelephoneNumber;

            myContactJabberIDInput.Text = myNTContact.NTJabberID;

            myContactAddressStreetInput.Text = myNTContact.NTHomeAddressStreet;
            myContactAddressCityInput.Text = myNTContact.NTHomeAddressCity;
            myContactAddressZipCodeInput.Text = myNTContact.NTHomeAddressPostalCode;
            myContactAddressStateInput.Text = myNTContact.NTHomeAddressState;
            myContactAddressCountryInput.Text = myNTContact.NTHomeAddressCountry;

            myContactAddressEmailInput.Text = myNTContact.NTEmail1Address;

            try
            {
                myContactPicture.Image = ImageProcessing.FromString(myNTContact.NTPicture);
            }
            catch (Exception)
            {
                
            }
                

            if (properties != null)
            {
                myContactFirstNameInput.Text = properties["NTFirstName"] != null ? properties["NTFirstName"].ToString() : myNTContact.NTFirstName;
                myContactMiddleNameInput.Text = properties["NTMiddleName"] != null ? properties["NTMiddleName"].ToString() : myNTContact.NTMiddleName;
                myContactLastNameInput.Text = properties["NTLastName"] != null ? properties["NTLastName"].ToString() : myNTContact.NTLastName;
                myContactPhoneHomeInput.Text = properties["NTHomeTelephoneNumber"] != null ? properties["NTHomeTelephoneNumber"].ToString() : myNTContact.NTHomeTelephoneNumber;
                myContactPhoneMobileInput.Text = properties["NTMobileTelephoneNumber"] != null ? properties["NTMobileTelephoneNumber"].ToString() : myNTContact.NTMobileTelephoneNumber;
                myContactPhoneBusinessInput.Text = properties["NTBusinessTelephoneNumber"] != null ? properties["NTBusinessTelephoneNumber"].ToString() : myNTContact.NTBusinessTelephoneNumber;
                myContactPhoneVoIPInput.Text = properties["NTVoIPTelephoneNumber"] != null ? properties["NTVoIPTelephoneNumber"].ToString() : myNTContact.NTVoIPTelephoneNumber;
                myContactJabberIDInput.Text = properties["NTJabberID"] != null ? properties["NTJabberID"].ToString() : myNTContact.NTJabberID;

                myContactAddressStreetInput.Text = properties["NTHomeAddressStreet"] != null ? properties["NTHomeAddressStreet"].ToString() : myNTContact.NTHomeAddressStreet;
                myContactAddressCityInput.Text = properties["NTHomeAddressCity"] != null ? properties["NTHomeAddressCity"].ToString() : myNTContact.NTHomeAddressCity;
                myContactAddressZipCodeInput.Text = properties["NTHomeAddressPostalCode"] != null ? properties["NTHomeAddressPostalCode"].ToString() : myNTContact.NTHomeAddressPostalCode;
                myContactAddressStateInput.Text = properties["NTHomeAddressState"] != null ? properties["NTHomeAddressState"].ToString() : myNTContact.NTHomeAddressState;
                myContactAddressCountryInput.Text = properties["NTHomeAddressCountry"] != null ? properties["NTHomeAddressCountry"].ToString() : myNTContact.NTHomeAddressCountry;

                myContactAddressEmailInput.Text = properties["NTEmail1Address"] != null ? properties["NTEmail1Address"].ToString() : myNTContact.NTEmail1Address;
            }

            JabberUser jabberUser = null;
            if (myContactJabberIDInput.Text != "")
            {
                jabberUser = new JabberUser(myContactJabberIDInput.Text);
                myContactJabberIDInput.Text = jabberUser.Username;
                myContactJabberIMNetworkListBox.SelectedItem = jabberUser.Network;
            }

            comboBox1.SelectedItem = myNTContact.NTContactStore;

            foreach (DictionaryEntry group in myClientForm.myBuddyGroups)
            {


                if (group.Value.ToString() != "" && !myContactJabberGroupListBox.Items.Contains(group.Value.ToString()))
                    myContactJabberGroupListBox.Items.Add(group.Value.ToString());



                if (jabberUser != null && jabberUser.Username == group.Key.ToString())
                {
                    myContactJabberGroupListBox.SelectedIndex = myContactJabberGroupListBox.Items.Count - 1;
                }
            }



            if (myClientForm.mUserAccount.Username == myContactJabberIDInput.Text)
            {
                myContactJabberGroupBox.Visible = false;
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

            myNTContact.NTContactChanged = true;
            myNTContact.NTContactStore = (NTContactStore)comboBox1.SelectedItem;

            myNTContact.NTFirstName = myContactFirstNameInput.Text;
            myNTContact.NTMiddleName = myContactMiddleNameInput.Text;
            myNTContact.NTLastName = myContactLastNameInput.Text;

            myNTContact.NTHomeTelephoneNumber = myContactPhoneHomeInput.Text;
            myNTContact.NTMobileTelephoneNumber = myContactPhoneMobileInput.Text;
            myNTContact.NTBusinessTelephoneNumber = myContactPhoneBusinessInput.Text;
            myNTContact.NTVoIPTelephoneNumber = myContactPhoneVoIPInput.Text; ;

           
            myNTContact.NTJabberID = "";
            if (myContactJabberIDInput.Text != "")
            {
                JabberUser jabberUser;
                ConfigXMPPNetwork selectedIMNetwork = ConfigXMPPNetwork.Nikotel;
                try
                {
                    selectedIMNetwork = (ConfigXMPPNetwork)myContactJabberIMNetworkListBox.SelectedItem;
                }
                catch (Exception)
                {

                    selectedIMNetwork = ConfigXMPPNetwork.Nikotel;
                }

                String domain = ConfigIM.GetXMPPDomain(selectedIMNetwork);
                jabberUser = new JabberUser(myContactJabberIDInput.Text.Replace(@"@", @"\40") + @"@" + domain);
           

            myNTContact.NTJabberID = jabberUser.JID;
            myNTContact.NTUsername = jabberUser.Username;
            myNTContact.NTNickname = jabberUser.Nick;
        }

            myNTContact.NTHomeAddressStreet = myContactAddressStreetInput.Text;
            myNTContact.NTHomeAddressCity = myContactAddressCityInput.Text;
            myNTContact.NTHomeAddressPostalCode = myContactAddressZipCodeInput.Text;
            myNTContact.NTHomeAddressState = myContactAddressStateInput.Text;
            myNTContact.NTHomeAddressCountry = myContactAddressCountryInput.Text;

            myNTContact.NTEmail1Address = myContactAddressEmailInput.Text;

            if (mContactBook.IndexOf(myNTContact) < 0)
            {
                mContactBook.Add(myNTContact);
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
                        myNTContact.NTPicture = ImageProcessing.ToString(myContactPicture.Image);
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
    }
}
