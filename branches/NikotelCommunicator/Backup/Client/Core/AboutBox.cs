using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls.UI;

namespace Remwave.Client
{
    public partial class AboutBox : Form
    {
        private void BrandComponent()
        {
            this.Icon = Properties.Resources.desktop;
        }
        public AboutBox()
        {
            InitializeComponent();            
        }

        public AboutBox(String productName, String productVersion, String copyrightNotice, String urlCompany, String urlProduct)
        {
            InitializeComponent();

            BrandComponent();
            this.ClientSize = this.Size;
            this.MaximizeBox = false;
            this.MaximumSize = this.Size;


            //labelBugsReports;
            labelCopyrightNotice.Text = String.Format(labelCopyrightNotice.Text,copyrightNotice);
            labelProductName.Text = productName;
            labelProductVersion.Text = String.Format(labelProductVersion.Text, productVersion); ;
            labelTitleProductName.Text = productName;
            linkCompany.Text = urlCompany;
            linkProduct.Text = urlProduct;
        }

        private void buttonCloseForm_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void linkCompany_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(linkCompany.Text);
            }
            catch (Exception ex)
            {
#if (TRACE)
                Console.WriteLine("StartNewEmail : " + ex.Message);
#endif
            }
        }

        private void linkProduct_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(linkProduct.Text);
            }
            catch (Exception ex)
            {
#if (TRACE)
                Console.WriteLine("StartNewEmail : " + ex.Message);
#endif
            }
        }
    }
}