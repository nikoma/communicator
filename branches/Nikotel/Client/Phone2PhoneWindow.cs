using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.UI;

namespace Remwave.Client
{
    public partial class Phone2PhoneWindow : ShapedForm
    {
        NTContact mySelectedNTContact;
        NTContact myMeNTContact;
        public bool StartCall = false;
        public string CallFrom = "";
        public string CallTo = "";

        public Phone2PhoneWindow(NTContact selectedContact, NTContact meContact)
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Phone2PhoneWindow));

            InitializeComponent();

            mySelectedNTContact = selectedContact;
            myMeNTContact = meContact;

            this.ClientSize = this.Size;
            this.MaximizeBox = false;
            this.MaximumSize = this.Size;


            selectedNameLabel.Text = mySelectedNTContact.FullName() + "'s Phone Number";
            selectedContactComboBox.Items.Clear();
            selectedContactComboBox.Items.Clear();
            meContactComboBox.Items.Clear();
            #region process mySelectedNTContact
            if (mySelectedNTContact.NTHomeTelephoneNumber != "")
            {
                this.tmplRadComboBoxItem = new Telerik.WinControls.UI.RadComboBoxItem();

                this.selectedContactComboBox.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.tmplRadComboBoxItem});

                // 
                // tmplRadComboBoxItem
                // 
                this.tmplRadComboBoxItem.AccessibleDescription = "";
                this.tmplRadComboBoxItem.CanFocus = true;
                this.tmplRadComboBoxItem.DescriptionText = "Home";
                this.tmplRadComboBoxItem.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                this.tmplRadComboBoxItem.Image = ((System.Drawing.Image)(resources.GetObject("tmpRadComboBoxItem.Image")));
                this.tmplRadComboBoxItem.Text = mySelectedNTContact.NTHomeTelephoneNumber; 
                this.tmplRadComboBoxItem.TextSeparatorVisibility = Telerik.WinControls.ElementVisibility.Visible;
                this.tmplRadComboBoxItem.ToolTipText = null;
                this.tmplRadComboBoxItem.DisplayStyle = DisplayStyle.ImageAndText;
                this.tmplRadComboBoxItem.TextImageRelation = TextImageRelation.ImageBeforeText;
                this.selectedContactComboBox.SelectedItem = this.tmplRadComboBoxItem;
            }
            if (mySelectedNTContact.NTMobileTelephoneNumber != "")
            {
                this.tmplRadComboBoxItem = new Telerik.WinControls.UI.RadComboBoxItem();

                this.selectedContactComboBox.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.tmplRadComboBoxItem});

                // 
                // tmplRadComboBoxItem
                // 
                this.tmplRadComboBoxItem.AccessibleDescription = "";
                this.tmplRadComboBoxItem.CanFocus = true;
                this.tmplRadComboBoxItem.DescriptionText = "Mobile";
                this.tmplRadComboBoxItem.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                this.tmplRadComboBoxItem.Image = ((System.Drawing.Image)(resources.GetObject("tmpRadComboBoxItem.Image")));
                this.tmplRadComboBoxItem.Text = mySelectedNTContact.NTMobileTelephoneNumber;
                this.tmplRadComboBoxItem.TextSeparatorVisibility = Telerik.WinControls.ElementVisibility.Visible;
                this.tmplRadComboBoxItem.ToolTipText = null;
                this.tmplRadComboBoxItem.DisplayStyle = DisplayStyle.ImageAndText;
                this.tmplRadComboBoxItem.TextImageRelation = TextImageRelation.ImageBeforeText;
                this.selectedContactComboBox.SelectedItem = this.tmplRadComboBoxItem;
            };
            if (mySelectedNTContact.NTBusinessTelephoneNumber != "")
            {
                this.tmplRadComboBoxItem = new Telerik.WinControls.UI.RadComboBoxItem();

                this.selectedContactComboBox.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.tmplRadComboBoxItem});

                // 
                // tmplRadComboBoxItem
                // 
                this.tmplRadComboBoxItem.AccessibleDescription = "";
                this.tmplRadComboBoxItem.CanFocus = true;
                this.tmplRadComboBoxItem.DescriptionText = "Work";
                this.tmplRadComboBoxItem.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                this.tmplRadComboBoxItem.Image = ((System.Drawing.Image)(resources.GetObject("tmpRadComboBoxItem.Image")));
                this.tmplRadComboBoxItem.Text = mySelectedNTContact.NTBusinessTelephoneNumber;
                this.tmplRadComboBoxItem.TextSeparatorVisibility = Telerik.WinControls.ElementVisibility.Visible;
                this.tmplRadComboBoxItem.ToolTipText = null;
                this.tmplRadComboBoxItem.DisplayStyle = DisplayStyle.ImageAndText;
                this.tmplRadComboBoxItem.TextImageRelation = TextImageRelation.ImageBeforeText;
                this.selectedContactComboBox.SelectedItem = this.tmplRadComboBoxItem;
            };
            if (selectedContactComboBox.Items.Count > 0) selectedContactComboBox.SelectedIndex = 0;
            #endregion

            #region process myMeNTContact
            if (myMeNTContact.NTHomeTelephoneNumber != "")
            {
                this.tmplRadComboBoxItem = new Telerik.WinControls.UI.RadComboBoxItem();

                this.meContactComboBox.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.tmplRadComboBoxItem});

                // 
                // tmplRadComboBoxItem
                // 
                this.tmplRadComboBoxItem.AccessibleDescription = "";
                this.tmplRadComboBoxItem.CanFocus = true;
                this.tmplRadComboBoxItem.DescriptionText = "Home";
                this.tmplRadComboBoxItem.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                this.tmplRadComboBoxItem.Image = ((System.Drawing.Image)(resources.GetObject("tmpRadComboBoxItem.Image")));
                this.tmplRadComboBoxItem.Text = myMeNTContact.NTHomeTelephoneNumber;
                this.tmplRadComboBoxItem.TextSeparatorVisibility = Telerik.WinControls.ElementVisibility.Visible;
                this.tmplRadComboBoxItem.ToolTipText = null;
                this.tmplRadComboBoxItem.DisplayStyle = DisplayStyle.ImageAndText;
                this.tmplRadComboBoxItem.TextImageRelation = TextImageRelation.ImageBeforeText;
                this.meContactComboBox.SelectedItem = this.tmplRadComboBoxItem;
            }
            if (myMeNTContact.NTMobileTelephoneNumber != "")
            {
                this.tmplRadComboBoxItem = new Telerik.WinControls.UI.RadComboBoxItem();

                this.meContactComboBox.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.tmplRadComboBoxItem});

                // 
                // tmplRadComboBoxItem
                // 
                this.tmplRadComboBoxItem.AccessibleDescription = "";
                this.tmplRadComboBoxItem.CanFocus = true;
                this.tmplRadComboBoxItem.DescriptionText = "Mobile";
                this.tmplRadComboBoxItem.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                this.tmplRadComboBoxItem.Image = ((System.Drawing.Image)(resources.GetObject("tmpRadComboBoxItem.Image")));
                this.tmplRadComboBoxItem.Text = myMeNTContact.NTMobileTelephoneNumber;
                this.tmplRadComboBoxItem.TextSeparatorVisibility = Telerik.WinControls.ElementVisibility.Visible;
                this.tmplRadComboBoxItem.ToolTipText = null;
                this.tmplRadComboBoxItem.DisplayStyle = DisplayStyle.ImageAndText;
                this.tmplRadComboBoxItem.TextImageRelation = TextImageRelation.ImageBeforeText;
                this.meContactComboBox.SelectedItem = this.tmplRadComboBoxItem;
            };
            if (myMeNTContact.NTBusinessTelephoneNumber != "")
            {
                this.tmplRadComboBoxItem = new Telerik.WinControls.UI.RadComboBoxItem();

                this.meContactComboBox.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.tmplRadComboBoxItem});

                // 
                // tmplRadComboBoxItem
                // 
                this.tmplRadComboBoxItem.AccessibleDescription = "";
                this.tmplRadComboBoxItem.CanFocus = true;
                this.tmplRadComboBoxItem.DescriptionText = "Work";
                this.tmplRadComboBoxItem.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                this.tmplRadComboBoxItem.Image = ((System.Drawing.Image)(resources.GetObject("tmpRadComboBoxItem.Image")));
                this.tmplRadComboBoxItem.Text = myMeNTContact.NTBusinessTelephoneNumber;
                this.tmplRadComboBoxItem.TextSeparatorVisibility = Telerik.WinControls.ElementVisibility.Visible;
                this.tmplRadComboBoxItem.ToolTipText = null;
                this.tmplRadComboBoxItem.DisplayStyle = DisplayStyle.ImageAndText;
                this.tmplRadComboBoxItem.TextImageRelation = TextImageRelation.ImageBeforeText;
                this.meContactComboBox.SelectedItem = this.tmplRadComboBoxItem;
            };
            if (meContactComboBox.Items.Count > 0) meContactComboBox.SelectedIndex = 0;
            #endregion

        }

        private void myDialPadCallCancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void myDialPadCallOrAnswerButton_Click(object sender, EventArgs e)
        {
            StartCall = true;
            CallFrom = meContactComboBox.Text;
            CallTo = selectedContactComboBox.Text;
            this.Close();
        }
    }
}