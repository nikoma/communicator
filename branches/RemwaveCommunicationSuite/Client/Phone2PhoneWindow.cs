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
        NTContact myNTContact;
        public bool StartCall = false;
        public string CallFrom = "";
        public string CallTo = "";
        public Phone2PhoneWindow(NTContact selectedContact)
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Phone2PhoneWindow));

            InitializeComponent();

            this.ClientSize = this.Size;
            this.MaximizeBox = false;
            this.MaximumSize = this.Size;


            myNTContact  = selectedContact;

            label3.Text = myNTContact.FullName() + "'s Phone Number";
            radComboBox1.Items.Clear();
            if (myNTContact.NTHomeTelephoneNumber != "")
            {
                this.tmplRadComboBoxItem = new Telerik.WinControls.UI.RadComboBoxItem();

                this.radComboBox1.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.tmplRadComboBoxItem});

                // 
                // tmplRadComboBoxItem
                // 
                this.tmplRadComboBoxItem.AccessibleDescription = "";
                this.tmplRadComboBoxItem.CanFocus = true;
                this.tmplRadComboBoxItem.DescriptionText = "Home";
                this.tmplRadComboBoxItem.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                this.tmplRadComboBoxItem.Image = ((System.Drawing.Image)(resources.GetObject("tmpRadComboBoxItem.Image")));
                this.tmplRadComboBoxItem.Text = myNTContact.NTHomeTelephoneNumber; 
                this.tmplRadComboBoxItem.TextSeparatorVisibility = Telerik.WinControls.ElementVisibility.Visible;
                this.tmplRadComboBoxItem.ToolTipText = null;
                this.tmplRadComboBoxItem.DisplayStyle = DisplayStyle.ImageAndText;
                this.tmplRadComboBoxItem.TextImageRelation = TextImageRelation.ImageBeforeText;
                this.radComboBox1.SelectedItem = this.tmplRadComboBoxItem;
            }
            if (myNTContact.NTMobileTelephoneNumber != "")
            {
                this.tmplRadComboBoxItem = new Telerik.WinControls.UI.RadComboBoxItem();

                this.radComboBox1.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.tmplRadComboBoxItem});

                // 
                // tmplRadComboBoxItem
                // 
                this.tmplRadComboBoxItem.AccessibleDescription = "";
                this.tmplRadComboBoxItem.CanFocus = true;
                this.tmplRadComboBoxItem.DescriptionText = "Mobile";
                this.tmplRadComboBoxItem.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                this.tmplRadComboBoxItem.Image = ((System.Drawing.Image)(resources.GetObject("tmpRadComboBoxItem.Image")));
                this.tmplRadComboBoxItem.Text = myNTContact.NTMobileTelephoneNumber;
                this.tmplRadComboBoxItem.TextSeparatorVisibility = Telerik.WinControls.ElementVisibility.Visible;
                this.tmplRadComboBoxItem.ToolTipText = null;
                this.tmplRadComboBoxItem.DisplayStyle = DisplayStyle.ImageAndText;
                this.tmplRadComboBoxItem.TextImageRelation = TextImageRelation.ImageBeforeText;
                this.radComboBox1.SelectedItem = this.tmplRadComboBoxItem;
            };
            if (myNTContact.NTBusinessTelephoneNumber != "")
            {
                this.tmplRadComboBoxItem = new Telerik.WinControls.UI.RadComboBoxItem();

                this.radComboBox1.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.tmplRadComboBoxItem});

                // 
                // tmplRadComboBoxItem
                // 
                this.tmplRadComboBoxItem.AccessibleDescription = "";
                this.tmplRadComboBoxItem.CanFocus = true;
                this.tmplRadComboBoxItem.DescriptionText = "Work";
                this.tmplRadComboBoxItem.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                this.tmplRadComboBoxItem.Image = ((System.Drawing.Image)(resources.GetObject("tmpRadComboBoxItem.Image")));
                this.tmplRadComboBoxItem.Text = myNTContact.NTBusinessTelephoneNumber;
                this.tmplRadComboBoxItem.TextSeparatorVisibility = Telerik.WinControls.ElementVisibility.Visible;
                this.tmplRadComboBoxItem.ToolTipText = null;
                this.tmplRadComboBoxItem.DisplayStyle = DisplayStyle.ImageAndText;
                this.tmplRadComboBoxItem.TextImageRelation = TextImageRelation.ImageBeforeText;
                this.radComboBox1.SelectedItem = this.tmplRadComboBoxItem;
            };
        }

        private void myDialPadCallCancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void myDialPadCallOrAnswerButton_Click(object sender, EventArgs e)
        {
            StartCall = true;
            CallFrom = radComboBox2.Text;
            CallTo = radComboBox1.Text;
            this.Close();
        }
    }
}