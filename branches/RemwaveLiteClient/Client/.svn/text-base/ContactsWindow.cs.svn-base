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
        private RPhoneBook myRPhoneBook;
        private NTContact myNTContact;
        public ContactsWindow(ClientForm clientForm, NTContact selectedMyContact, Hashtable properties)
        {
            InitializeComponent();

            this.ClientSize = this.Size;
            this.MaximizeBox = false;
            this.MaximumSize = this.Size;

            myClientForm = clientForm;
            myRPhoneBook = myClientForm.myContactsBook;


            try
            {

            
            if (selectedMyContact != null)
            {
                myNTContact = selectedMyContact;
            }
            else
            {
                myNTContact = new NTContact();
            }

            myContactJabberIDListBox.Items.Clear();
            myContactJabberIDListBox.Items.Add("");

            myContactJabberGroupListBox.Items.Clear();
            myContactJabberGroupListBox.Items.Add("");

            

            myContactFirstNameInput.Text = myNTContact.NTFirstName;
            myContactMiddleNameInput.Text = myNTContact.NTMiddleName;
            myContactLastNameInput.Text = myNTContact.NTLastName;
            myContactPhoneHomeInput.Text = myNTContact.NTHomeTelephoneNumber;
            myContactPhoneMobileInput.Text = myNTContact.NTMobileTelephoneNumber;
            myContactPhoneBusinessInput.Text = myNTContact.NTBusinessTelephoneNumber;
            myContactPhoneVoIPInput.Text = myNTContact.NTVoIPTelephoneNumber;
            
                myContactJabberIDListBox.SelectedText = myNTContact.NTJabberID;

            myContactAddressStreetInput.Text = myNTContact.NTHomeAddressStreet;
            myContactAddressCityInput.Text = myNTContact.NTHomeAddressCity;
            myContactAddressZipCodeInput.Text = myNTContact.NTHomeAddressPostalCode;
            myContactAddressStateInput.Text = myNTContact.NTHomeAddressState;
            myContactAddressCountryInput.Text = myNTContact.NTHomeAddressCountry;

            myContactAddressEmailInput.Text = myNTContact.NTEmail1Address;

            if (properties != null)
            {
                myContactFirstNameInput.Text = properties["NTFirstName"] != null ? properties["NTFirstName"].ToString() : myNTContact.NTFirstName;
                myContactMiddleNameInput.Text = properties["NTMiddleName"] != null ? properties["NTMiddleName"].ToString() : myNTContact.NTMiddleName;
                myContactLastNameInput.Text = properties["NTLastName"] != null ? properties["NTLastName"].ToString() : myNTContact.NTLastName;
                myContactPhoneHomeInput.Text = properties["NTHomeTelephoneNumber"] != null ? properties["NTHomeTelephoneNumber"].ToString() : myNTContact.NTHomeTelephoneNumber;
                myContactPhoneMobileInput.Text = properties["NTMobileTelephoneNumber"] != null ? properties["NTMobileTelephoneNumber"].ToString() : myNTContact.NTMobileTelephoneNumber;
                myContactPhoneBusinessInput.Text = properties["NTBusinessTelephoneNumber"] != null ? properties["NTBusinessTelephoneNumber"].ToString() : myNTContact.NTBusinessTelephoneNumber;
                myContactPhoneVoIPInput.Text = properties["NTVoIPTelephoneNumber"] != null ? properties["NTVoIPTelephoneNumber"].ToString() : myNTContact.NTVoIPTelephoneNumber;
                myContactJabberIDListBox.SelectedText = properties["NTJabberID"] != null ? properties["NTJabberID"].ToString() : myNTContact.NTJabberID;

                myContactAddressStreetInput.Text = properties["NTHomeAddressStreet"] != null ? properties["NTHomeAddressStreet"].ToString() : myNTContact.NTHomeAddressStreet;
                myContactAddressCityInput.Text = properties["NTHomeAddressCity"] != null ? properties["NTHomeAddressCity"].ToString() : myNTContact.NTHomeAddressCity;
                myContactAddressZipCodeInput.Text = properties["NTHomeAddressPostalCode"] != null ? properties["NTHomeAddressPostalCode"].ToString() : myNTContact.NTHomeAddressPostalCode;
                myContactAddressStateInput.Text = properties["NTHomeAddressState"] != null ? properties["NTHomeAddressState"].ToString() : myNTContact.NTHomeAddressState;
                myContactAddressCountryInput.Text = properties["NTHomeAddressCountry"] != null ? properties["NTHomeAddressCountry"].ToString() : myNTContact.NTHomeAddressCountry;

                myContactAddressEmailInput.Text  = properties["NTEmail1Address"] != null ? properties["NTEmail1Address"].ToString() : myNTContact.NTEmail1Address;
            }

            foreach (DictionaryEntry buddy in myClientForm.myBuddyList)
            {
                myContactJabberIDListBox.Items.Add(buddy.Key.ToString());
                myContactJabberGroupListBox.Items.Add(buddy.Value.ToString());
                if (myContactJabberIDListBox.Text == buddy.Key.ToString())
                {
                    myContactJabberIDListBox.SelectedIndex = myContactJabberIDListBox.Items.Count - 1;
                    myContactJabberGroupListBox.SelectedIndex = myContactJabberGroupListBox.Items.Count - 1;
                }
            }
        }
        catch (Exception)
        {

          //  throw;
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

            myNTContact.NTEmail1Address = myContactAddressEmailInput.Text;

            if (myRPhoneBook.List.IndexOf(myNTContact) < 0)
            {
                myRPhoneBook.List.Add(myNTContact);
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