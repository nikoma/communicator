using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls.UI;
using Remwave.Services;

namespace Remwave.Client
{
    public partial class SpeedDialWindow : ShapedForm
    {

        private ClientForm myClientForm;
        private String previousSearchText = "";
        private bool displayingContacts = true;

        private int maxHeight = 468;

        private string defaultSearchText = "Enter a name or phone number";

        public SpeedDialWindow(ClientForm clientForm)
        {
            InitializeComponent();
            this.myClientForm = clientForm;
        }

        public void ShowHide()
        {
            if (!this.Visible)
            {
                myClientForm.WindowState = FormWindowState.Minimized;

                tbxSearchText.Text = "";
                lbxSearchResults.Items.Clear();
                AdjustFormSize(0);
                this.WindowState = FormWindowState.Normal;
                this.Show();
                this.Activate();
                this.Focus();
                tbxSearchText.Focus();

                
            }
            else
            {
                this.Hide();
            }
        }

        
        private void SpeedDialWindow_Load(object sender, EventArgs e)
        {
            //set default text to the search textbox
            tbxSearchText.NullText = defaultSearchText;
            lbxSearchResults.Visible = false;
            this.Size = new Size(320, 68);
        }

        private String GetContactName(NTContact contact)
        {
            //prepare contact name and username from NTContact
            String Name = "";
            String Username = "";
            if (contact != null)
            {
                Name = contact.FullName().Trim().Length > 64 ? contact.FullName().Trim().Substring(0, 64) : contact.FullName().Trim();
                Username = contact.NTJabberID.Trim().Length != 0 ? " (" + (contact.NTJabberID.Trim().Length > 64 ? contact.NTJabberID.Trim().Substring(0, 64) : contact.NTJabberID.Trim()) + ")" : "";
            }
            return Name + Username;
        }

        private String GetContactPrimaryPhoneNumbers(NTContact contact)
        {
            //prepare and format contact description, numbers, email, etc ...

            String Numbers = "";
            String Email = "";

            if (contact != null)
            {
                Numbers = (contact.PrimaryPhoneNumbers().Length > 64 ? contact.PrimaryPhoneNumbers().Trim().Substring(0, 64) : contact.PrimaryPhoneNumbers().Trim());
                Email = contact.NTEmail1Address.Length > 0 ? Email + contact.NTEmail1Address : "";
            }
            return Numbers + Email;
        }


        private RadListBoxItem BuildRadMenuContentItem(object tag, String name, String description, Image image, Size size, Font nameFont, Font descriptionFont, Telerik.WinControls.ElementVisibility separatorVisibility)
        {
            return BuildRadMenuContentItem(tag, name, description, image, size, nameFont, descriptionFont, separatorVisibility, true);
        }
        private RadListBoxItem BuildRadMenuContentItem(object tag, String name, String description, Image image, Size size, Font nameFont, Font descriptionFont, Telerik.WinControls.ElementVisibility separatorVisibility, bool selectable)
        {

            RadListBoxItem tmplContactListItem = new Telerik.WinControls.UI.RadListBoxItem();

            tmplContactListItem.AutoSize = true;
            tmplContactListItem.Size = size;
            tmplContactListItem.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            tmplContactListItem.DisplayStyle = Telerik.WinControls.DisplayStyle.ImageAndText;
            tmplContactListItem.Image = image;
            tmplContactListItem.ImageAlignment = ContentAlignment.MiddleCenter;
            //
            // tmplContactListItem
            //
            tmplContactListItem.AccessibleDescription = name;
            tmplContactListItem.CanFocus = true;
            tmplContactListItem.Text = " " + name.Trim();
            tmplContactListItem.DescriptionText = " " + description.Trim();

            tmplContactListItem.ForeColor = System.Drawing.Color.Black;

            tmplContactListItem.TextSeparatorVisibility = separatorVisibility;

            tmplContactListItem.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            tmplContactListItem.Font = nameFont;
            tmplContactListItem.DescriptionFont = descriptionFont;
            tmplContactListItem.Tag = tag;

            tmplContactListItem.ToolTipText = null;

            tmplContactListItem.AutoSizeMode = Telerik.WinControls.RadAutoSizeMode.WrapAroundChildren;

            tmplContactListItem.Enabled = selectable;

            tmplContactListItem.DoubleClick += new EventHandler(tmplContactListItem_DoubleClick);

            return tmplContactListItem;
        }


        private void LaunchSearchResultsActivity(bool launchActivity)
        {
            try
            {
                RadListBoxItem selectedItem = lbxSearchResults.SelectedItem as RadListBoxItem;
                if (selectedItem!=null && selectedItem.Tag != null)
                {
                    //is Activity
                    Activity activity = selectedItem.Tag as Activity;

                    if (activity != null)
                    {
                        if(launchActivity)myClientForm.StartActivity(activity);
                    }

                    NTContact contact = selectedItem.Tag as NTContact;

                    if (contact != null)
                    {
                        DisplaySelectionOptions(contact);
                    }
                };
            }
            catch (Exception)
            {
                return;
            }
        }


        void tmplContactListItem_DoubleClick(object sender, EventArgs e)
        {

            LaunchSearchResultsActivity(true);

            

        }

        private void DisplaySelectionOptions(NTContact contact)
        {
            int itemsHeight = 0;
            displayingContacts = false;
            //build sub menue

            Font listItemNameFont = new System.Drawing.Font("Trebuchet MS", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0))); ;
            Font listItemDescriptionFont = new System.Drawing.Font("Trebuchet MS", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));

            Font listSubItemNameFont = new System.Drawing.Font("Trebuchet MS", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0))); ;
            Font listSubItemDescriptionFont = new System.Drawing.Font("Trebuchet MS", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));

            Size listItemSize = new Size(300, 44);
            Size listSubItemSize = new Size(300, 44);

            lbxSearchResults.Items.Clear();

            lbxSearchResults.Items.Add(BuildRadMenuContentItem(
            contact,
            GetContactName(contact),
            GetContactPrimaryPhoneNumbers(contact),
            ((System.Drawing.Image)(Properties.Resources.listIconVcard)),
            listItemSize,
            listItemNameFont,
            listItemDescriptionFont,
            Telerik.WinControls.ElementVisibility.Visible,
            false
            ));

            itemsHeight += lbxSearchResults.Items[lbxSearchResults.Items.Count - 1].Size.Height;

            if (contact.NTJabberID != "")
            {
                //Call PC2PC
                Activity activityPC2PCCall = new Activity(ActivityType.Call, contact.NTJabberID);

                lbxSearchResults.Items.Add(
                BuildRadMenuContentItem(
                activityPC2PCCall,
                "Call PC2PC",
                contact.NTJabberID,
                ((System.Drawing.Image)(Properties.Resources.listIconComputer)),
                listSubItemSize,
                listSubItemNameFont,
                listSubItemDescriptionFont,
                Telerik.WinControls.ElementVisibility.Hidden
                )
                );
                itemsHeight += lbxSearchResults.Items[lbxSearchResults.Items.Count - 1].Size.Height;

                //Send Instant Message
                Activity activityIM = new Activity(ActivityType.IM, contact.NTJabberID);
                lbxSearchResults.Items.Add(
                BuildRadMenuContentItem(
                activityIM,
                "Send Instant Message",
                contact.NTJabberID,
                ((System.Drawing.Image)(Properties.Resources.listIconInstantMessage)),
                listSubItemSize,
                listSubItemNameFont,
                listSubItemDescriptionFont,
                Telerik.WinControls.ElementVisibility.Hidden
                )
                );
                itemsHeight += lbxSearchResults.Items[lbxSearchResults.Items.Count - 1].Size.Height;

                //Start Video Call
                Activity activityVideoCall = new Activity(ActivityType.VideoCall, contact.NTJabberID);
                lbxSearchResults.Items.Add(
                BuildRadMenuContentItem(
                activityVideoCall,
                "Start Video Call",
                contact.NTJabberID,
                ((System.Drawing.Image)(Properties.Resources.listIconWebcam)),
                listSubItemSize,
                listSubItemNameFont,
                listSubItemDescriptionFont,
                Telerik.WinControls.ElementVisibility.Hidden
                )
                );
                itemsHeight += lbxSearchResults.Items[lbxSearchResults.Items.Count - 1].Size.Height;

                //Start Screen Sharing
                Activity activityScreenSharing = new Activity(ActivityType.ScreenSharing, contact.NTJabberID);
                lbxSearchResults.Items.Add(
                BuildRadMenuContentItem(
                activityScreenSharing,
                "Start Screen Sharing",
                contact.NTJabberID,
                ((System.Drawing.Image)(Properties.Resources.listIconScreenSharing)),
                listSubItemSize,
                listSubItemNameFont,
                listSubItemDescriptionFont,
                Telerik.WinControls.ElementVisibility.Hidden
                )
                );
                itemsHeight += lbxSearchResults.Items[lbxSearchResults.Items.Count - 1].Size.Height;

            }

            if (contact.NTHomeTelephoneNumber != "")
            {
                //Call Home
                Activity activityCallHome = new Activity(ActivityType.Call, contact.NTHomeTelephoneNumber);
                lbxSearchResults.Items.Add(
                BuildRadMenuContentItem(
                activityCallHome,
                "Call Home",
                contact.NTHomeTelephoneNumber,
                ((System.Drawing.Image)(Properties.Resources.listIconPhone)),
                listSubItemSize,
                listSubItemNameFont,
                listSubItemDescriptionFont,
                Telerik.WinControls.ElementVisibility.Hidden
                )
                );
                itemsHeight += lbxSearchResults.Items[lbxSearchResults.Items.Count - 1].Size.Height;

            }

            if (contact.NTMobileTelephoneNumber != "")
            {
                //Call Mobile
                Activity activityCallMobile = new Activity(ActivityType.Call, contact.NTMobileTelephoneNumber);
                lbxSearchResults.Items.Add(
                BuildRadMenuContentItem(
                activityCallMobile,
                "Call Mobile",
                contact.NTMobileTelephoneNumber,
                ((System.Drawing.Image)(Properties.Resources.listIconPhone)),
                listSubItemSize,
                listSubItemNameFont,
                listSubItemDescriptionFont,
                Telerik.WinControls.ElementVisibility.Hidden
                )
                );
                itemsHeight += lbxSearchResults.Items[lbxSearchResults.Items.Count - 1].Size.Height;


            }

            if (contact.NTBusinessTelephoneNumber != "")
            {
                //Call Business
                Activity activityCallBusiness = new Activity(ActivityType.Call, contact.NTBusinessTelephoneNumber);
                lbxSearchResults.Items.Add(
                BuildRadMenuContentItem(
                activityCallBusiness,
                "Call Business",
                contact.NTBusinessTelephoneNumber,
                ((System.Drawing.Image)(Properties.Resources.listIconPhone)),
                listSubItemSize,
                listSubItemNameFont,
                listSubItemDescriptionFont,
                Telerik.WinControls.ElementVisibility.Hidden
                )
                );
                itemsHeight += lbxSearchResults.Items[lbxSearchResults.Items.Count - 1].Size.Height;

            }

            if (contact.NTVoIPTelephoneNumber != "")
            {
                //Call VoIP
                Activity activityCallVoIP = new Activity(ActivityType.Call, contact.NTVoIPTelephoneNumber);
                lbxSearchResults.Items.Add(
                BuildRadMenuContentItem(
                activityCallVoIP,
                "Call VoIP",
                contact.NTVoIPTelephoneNumber,
                ((System.Drawing.Image)(Properties.Resources.listIconPhone)),
                listSubItemSize,
                listSubItemNameFont,
                listSubItemDescriptionFont,
                Telerik.WinControls.ElementVisibility.Hidden
                )
                );
                itemsHeight += lbxSearchResults.Items[lbxSearchResults.Items.Count - 1].Size.Height;

            }

            if (contact.NTEmail1Address != "")
            {
                //Send Email
                Activity activitySendEmail = new Activity(ActivityType.Email, contact.NTEmail1Address);
                lbxSearchResults.Items.Add(
                BuildRadMenuContentItem(
                activitySendEmail,
                "Send Email",
                contact.NTEmail1Address,
                ((System.Drawing.Image)(Properties.Resources.listIconEmail)),
                listSubItemSize,
                listSubItemNameFont,
                listSubItemDescriptionFont,
                Telerik.WinControls.ElementVisibility.Hidden
                )
                );
                itemsHeight += lbxSearchResults.Items[lbxSearchResults.Items.Count - 1].Size.Height;

            }

            if (lbxSearchResults.Items.Count > 1) lbxSearchResults.SelectedIndex = 1;
            else if (lbxSearchResults.Items.Count > 0) lbxSearchResults.SelectedIndex = 0;

           AdjustFormSize(itemsHeight);
        }


        private void DisplaySearchResults(ContactList contactList, String searchText, bool showAll)
        {

            int itemsHeight = 0;
            displayingContacts = true;
            lbxSearchResults.Visible = true;
            Font listItemNameFont = new System.Drawing.Font("Trebuchet MS", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0))); ;
            Font listItemDescriptionFont = new System.Drawing.Font("Trebuchet MS", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));

            Font listSubItemNameFont = new System.Drawing.Font("Trebuchet MS", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0))); ;
            Font listSubItemDescriptionFont = new System.Drawing.Font("Trebuchet MS", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));

            Size listItemSize = new Size(300, 44);
            Size listSubItemSize = new Size(300, 44);

            lbxSearchResults.Items.Clear();

            searchText = searchText.Trim().ToLower();



            if (searchText != "")
            {
                Activity activityCall = new Activity(ActivityType.Call, searchText);

                lbxSearchResults.Items.Add(
                BuildRadMenuContentItem(
                activityCall,
                "Call",
                searchText,
                ((System.Drawing.Image)(Properties.Resources.listIconPhone)),
                listItemSize,
                listSubItemNameFont,
                listSubItemDescriptionFont,
                Telerik.WinControls.ElementVisibility.Hidden
                )
                );
                itemsHeight += lbxSearchResults.Items[lbxSearchResults.Items.Count - 1].Size.Height;
            }


            if (contactList != null)
            {
                foreach (NTContact myNTContact in contactList)
                {
                    if (myNTContact.NTDeleted != "true")
                    {
                        bool DisplayRecord = false;

                        if (showAll)
                        {
                            DisplayRecord = true;
                        }
                        else if (searchText.Length > 0 && searchText != defaultSearchText.ToLower())
                        {
                            foreach (string stringKey in searchText.Split(new String[] { " " }, StringSplitOptions.RemoveEmptyEntries))
                            {
                                if
                                (
                                myNTContact.NTFirstName.Trim().ToLower().StartsWith(stringKey)
                                ||
                                myNTContact.NTLastName.Trim().ToLower().StartsWith(stringKey)
                                ||
                                myNTContact.NTEmail1Address.Trim().ToLower().StartsWith(stringKey)
                                ||
                                myNTContact.NTJabberID.Trim().ToLower().StartsWith(stringKey)
                                ||
                                myNTContact.NTHomeTelephoneNumber.Trim().ToLower().StartsWith(stringKey)
                                ||
                                myNTContact.NTMobileTelephoneNumber.Trim().ToLower().StartsWith(stringKey)
                                ||
                                myNTContact.NTBusinessTelephoneNumber.Trim().ToLower().StartsWith(stringKey)
                                ||
                                myNTContact.NTVoIPTelephoneNumber.Trim().ToLower().StartsWith(stringKey)

                                )
                                {
                                    DisplayRecord = true;
                                    break;
                                }
                            }
                        }

                        if (DisplayRecord)
                        {
                            lbxSearchResults.Items.Add(BuildRadMenuContentItem(
                            myNTContact,
                            GetContactName(myNTContact),
                            GetContactPrimaryPhoneNumbers(myNTContact),
                            ((System.Drawing.Image)(Properties.Resources.listIconVcard)),
                            listItemSize,
                            listItemNameFont,
                            listItemDescriptionFont,
                            Telerik.WinControls.ElementVisibility.Visible
                            ));
                            itemsHeight += lbxSearchResults.Items[lbxSearchResults.Items.Count - 1].Size.Height;
                        }
                    }
                }
            }

            if (lbxSearchResults.Items.Count == 1)
            {
                Activity activityAddContact = new Activity(ActivityType.AddContact, searchText);

                lbxSearchResults.Items.Add(
                BuildRadMenuContentItem(
                activityAddContact,
                "No record found", "Add " + searchText+ " to your contacts.",
                ((System.Drawing.Image)(Properties.Resources.listIconVcardAdd)),
                listItemSize,
                listSubItemNameFont,
                listSubItemDescriptionFont,
                Telerik.WinControls.ElementVisibility.Hidden
                )

                );
                itemsHeight += lbxSearchResults.Items[lbxSearchResults.Items.Count - 1].Size.Height;



            }

            AdjustFormSize(itemsHeight);


            if (lbxSearchResults.Items.Count > 0) lbxSearchResults.SelectedIndex = 0;

        }

        private void AdjustFormSize(int listHeight)
        {
            if (listHeight == 0)
            {
                this.Size = new Size(320, 68);
            }
            else if ((listHeight + 68) >= maxHeight)
            {
                this.Size = new Size(320, maxHeight);
            }
            else
            {
                this.Size = new Size(320, listHeight + 68 + 20);
            }
        }














        //check below




















        private void tbxSearchText_KeyDown(object sender, KeyEventArgs e)
        {
            // Console.WriteLine("Down"+e.KeyCode.ToString());
        }



        private void tbxSearchText_KeyUp(object sender, KeyEventArgs e)
        {
            // Console.WriteLine("Up" + e.KeyCode.ToString());

            if (e.KeyCode == Keys.Escape) this.Hide();

            if (e.KeyCode == Keys.Down)
            {
                if (lbxSearchResults.Items.Count > 0)
                {
                    if (lbxSearchResults.SelectedIndex >= lbxSearchResults.Items.Count - 1)
                    {
                        lbxSearchResults.SelectedIndex = 0;
                    }
                    else
                    {
                        lbxSearchResults.SelectedIndex++;
                    }
                }
            }

            if (e.KeyCode == Keys.Up)
            {
                if (lbxSearchResults.Items.Count > 0)
                {
                    if (lbxSearchResults.SelectedIndex <= 0)
                    {
                        lbxSearchResults.SelectedIndex = lbxSearchResults.Items.Count - 1;
                    }
                    else
                    {
                        lbxSearchResults.SelectedIndex--;
                    }
                }
            }


            if (e.KeyCode == Keys.Return)
            {
                LaunchSearchResultsActivity(true);

            }else if(e.KeyCode == Keys.Right)
            {
                LaunchSearchResultsActivity(false);
            }


            if (e.KeyCode == Keys.Left || e.KeyCode == Keys.Back)
            {
                ShowMain(false, false);
            }


            if (previousSearchText != tbxSearchText.Text)
            {
                ShowMain(true, false);
            }

            previousSearchText = tbxSearchText.Text;
        }


        private void ShowMain(bool force, bool showAll)
        {
            if (!displayingContacts || force)
            {
                DisplaySearchResults(myClientForm.myContactsBook.List, tbxSearchText.Text, showAll);
            }
        }

        

        private void lbxSearchResults_KeyUp(object sender, KeyEventArgs e)
        {
            // Console.WriteLine(e.KeyCode.ToString());


            if (e.KeyCode == Keys.Return)
            {
                LaunchSearchResultsActivity(true);

            }
            else if (e.KeyCode == Keys.Right)
            {
                LaunchSearchResultsActivity(false);
            }


            if (e.KeyCode == Keys.Left || e.KeyCode == Keys.Back)
            {
                ShowMain(false,false);
            }




        }

        private void btnShowContacts_Click(object sender, EventArgs e)
        {
            ShowMain(true, true);
        }

    }
}