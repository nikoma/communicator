using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using Telerik.WinControls.UI;

namespace Remwave.Client
{
    public partial class ContactsWindow : ShapedForm
    {
        public bool Accepted = false;
        private ClientForm myClientForm;
        private WEBPhoneBook myWebPhonebook;
        private NTContact myNTContact;
        public ContactsWindow(ClientForm clientForm, NTContact selectedMyContact, Hashtable properties)
        {
            InitializeComponent();

            this.ClientSize = this.Size;
            this.MaximizeBox = false;
            this.MaximumSize = this.Size;

            myClientForm = clientForm;
            myWebPhonebook = myClientForm.myContactsBook;
            if (selectedMyContact != null)
            {
                myNTContact = selectedMyContact;
            }
            else
            {
                myNTContact = new NTContact();
            }


            myContactFirstNameInput.Text = myNTContact.NTFirstName;
            myContactMiddleNameInput.Text = myNTContact.NTMiddleName;
            myContactLastNameInput.Text = myNTContact.NTLastName;
            myContactPhoneHomeInput.Text = myNTContact.NTHomeTelephoneNumber;
            myContactPhoneMobileInput.Text = myNTContact.NTMobileTelephoneNumber;
            myContactPhoneBusinessInput.Text = myNTContact.NTBusinessTelephoneNumber;
            myContactPhoneVoIPInput.Text = myNTContact.NTVoIPTelephoneNumber;

            myContactAddressStreetInput.Text = myNTContact.NTHomeAddressStreet;
            myContactAddressCityInput.Text = myNTContact.NTHomeAddressCity;
            myContactAddressZipCodeInput.Text = myNTContact.NTHomeAddressPostalCode;
            myContactAddressStateInput.Text = myNTContact.NTHomeAddressState;
            myContactAddressCountryInput.Text = myNTContact.NTHomeAddressCountry;

            if (properties != null)
            {
                myContactFirstNameInput.Text = properties["NTFirstName"] != null ? properties["NTFirstName"].ToString() : myNTContact.NTFirstName;
                myContactMiddleNameInput.Text = properties["NTMiddleName"] != null ? properties["NTMiddleName"].ToString() : myNTContact.NTMiddleName;
                myContactLastNameInput.Text = properties["NTLastName"] != null ? properties["NTLastName"].ToString() : myNTContact.NTLastName;
                myContactPhoneHomeInput.Text = properties["NTHomeTelephoneNumber"] != null ? properties["NTHomeTelephoneNumber"].ToString() : myNTContact.NTHomeTelephoneNumber;
                myContactPhoneMobileInput.Text = properties["NTMobileTelephoneNumber"] != null ? properties["NTMobileTelephoneNumber"].ToString() : myNTContact.NTMobileTelephoneNumber;
                myContactPhoneBusinessInput.Text = properties["NTBusinessTelephoneNumber"] != null ? properties["NTBusinessTelephoneNumber"].ToString() : myNTContact.NTBusinessTelephoneNumber;
                myContactPhoneVoIPInput.Text = properties["NTVoIPTelephoneNumber"] != null ? properties["NTVoIPTelephoneNumber"].ToString() : myNTContact.NTVoIPTelephoneNumber;
                myContactJabberIDListBox.Text = properties["NTJabberID"] != null ? properties["NTJabberID"].ToString() : myNTContact.NTJabberID;

                myContactAddressStreetInput.Text = properties["NTHomeAddressStreet"] != null ? properties["NTHomeAddressStreet"].ToString() : myNTContact.NTHomeAddressStreet;
                myContactAddressCityInput.Text = properties["NTHomeAddressCity"] != null ? properties["NTHomeAddressCity"].ToString() : myNTContact.NTHomeAddressCity;
                myContactAddressZipCodeInput.Text = properties["NTHomeAddressPostalCode"] != null ? properties["NTHomeAddressPostalCode"].ToString() : myNTContact.NTHomeAddressPostalCode;
                myContactAddressStateInput.Text = properties["NTHomeAddressState"] != null ? properties["NTHomeAddressState"].ToString() : myNTContact.NTHomeAddressState;
                myContactAddressCountryInput.Text = properties["NTHomeAddressCountry"] != null ? properties["NTHomeAddressCountry"].ToString() : myNTContact.NTHomeAddressCountry;
            }

            myContactJabberIDListBox.Items.Clear();
            myContactJabberIDListBox.Items.Add("");

            myContactJabberGroupListBox.Items.Clear();
            myContactJabberGroupListBox.Items.Add("");

            foreach (DictionaryEntry buddy in myClientForm.myBuddyList)
            {
                myContactJabberIDListBox.Items.Add(buddy.Key.ToString());
                myContactJabberGroupListBox.Items.Add(buddy.Value.ToString());
                if(myNTContact.NTJabberID == buddy.Key.ToString())
                {
                     myContactJabberIDListBox.SelectedIndex = myContactJabberIDListBox.Items.Count - 1;
                     myContactJabberGroupListBox.SelectedIndex = myContactJabberGroupListBox.Items.Count - 1;
                }
            }

        }

        private void myContactSaveCloseButton_Click(object sender, EventArgs e)
        {
            Accepted = true;
            myNTContact.NTFirstName = myContactFirstNameInput.Text;
            myNTContact.NTMiddleName = myContactMiddleNameInput.Text;
            myNTContact.NTLastName = myContactLastNameInput.Text;

            myNTContact.NTHomeTelephoneNumber = myContactPhoneHomeInput.Text;
            myNTContact.NTMobileTelephoneNumber = myContactPhoneMobileInput.Text;
            myNTContact.NTBusinessTelephoneNumber = myContactPhoneBusinessInput.Text;
            myNTContact.NTVoIPTelephoneNumber = myContactPhoneVoIPInput.Text; ;

            myNTContact.NTJabberID = myContactJabberIDListBox.Text;


            myNTContact.NTHomeAddressStreet = myContactAddressStreetInput.Text;
            myNTContact.NTHomeAddressCity = myContactAddressCityInput.Text;
            myNTContact.NTHomeAddressPostalCode = myContactAddressZipCodeInput.Text;
            myNTContact.NTHomeAddressState = myContactAddressStateInput.Text;
            myNTContact.NTHomeAddressCountry = myContactAddressCountryInput.Text;




            if (myWebPhonebook.List.IndexOf(myNTContact) < 0)
            {
                myWebPhonebook.List.Add(myNTContact);
            }
            this.Close();
        }

        private void myContactFirstNameInput_TextChanged(object sender, EventArgs e)
        {
            myContactJabberAliasInput.Text = myContactFirstNameInput.Text + " " + myContactLastNameInput.Text;
        }

        private void myContactLastNameInput_TextChanged(object sender, EventArgs e)
        {
            myContactJabberAliasInput.Text = myContactFirstNameInput.Text + " " + myContactLastNameInput.Text;
        }
    }
}