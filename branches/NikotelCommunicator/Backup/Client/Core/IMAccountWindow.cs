using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Remwave.Services;

namespace Remwave.Client
{
    public partial class IMAccountWindow : Form
    {
        public Boolean Save = false;
        public Boolean Delete = false;
        public String Username;
        public String Password;
        public String Domain;
        private void BrandComponent()
        {
            this.Icon = Properties.Resources.desktop;
        }
        public IMAccountWindow()
        {
            InitializeComponent();
            
        }
        public IMAccountWindow(String username, String password, String domain, Image icon, String description )
        {
            this.Username = username;
            this.Password = password;
            this.Domain = domain;
            InitializeComponent();
            LocalizeComponent();
            BrandComponent();
            this.imgIMNetworkLogo.Image = icon;
            this.lblIMNetwork.Text = description;
            this.tbUsername.Text = username;
            this.tbPassword.Text = password;
        }

        private void LocalizeComponent()
        {
            this.btnSave.Text = Properties.Localization.txtIMAccountBtnSave;
            this.btnDelete.Text = Properties.Localization.txtIMAccountBtnDelete;
            this.lblUsername.Text = Properties.Localization.txtIMAccountTitleUsername;
            this.lblPassword.Text = Properties.Localization.txtIMAccountTitlePassword;
            this.lblIMNetwork.Text = Properties.Localization.txtIMAccountTitleNetwork;
            this.Text = Properties.Localization.txtIMAccountTitle;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            this.Delete = true;
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            this.Username = tbUsername.Text;
            this.Password = tbPassword.Text;
            this.Save = true;
            this.Close();
        }
    }
}
