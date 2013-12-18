using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;

namespace Remwave.Client
{
    public partial class ContactsSearch : Form
    {
        private ClientForm myClientForm;
        public NTContact selectedContact;

        public ContactsSearch(ClientForm clientForm)
        {
            this.myClientForm = clientForm;
            InitializeComponent();
            LocalizeComponent();
            BrandComponent();
        }

        private void LocalizeComponent()
        {
            usernameDataGridViewTextBoxColumn.HeaderText = Properties.Localization.txtCSearchTitleUsername;
            companyDataGridViewTextBoxColumn.HeaderText = Properties.Localization.txtCSearchTitleCompany;
            firstNameDataGridViewTextBoxColumn.HeaderText = Properties.Localization.txtCSearchTitleFirstName;
            lastNameDataGridViewTextBoxColumn.HeaderText = Properties.Localization.txtCSearchTitleLastName;
            countryDataGridViewTextBoxColumn.HeaderText = Properties.Localization.txtCSearchTitleCountry;
            regionDataGridViewTextBoxColumn.HeaderText = Properties.Localization.txtCSearchTitleRegion;
            commentDataGridViewTextBoxColumn.HeaderText = Properties.Localization.txtCSearchTitleComment;
            btnUserFind.Text = Properties.Localization.txtCSearchTitleFind;
            lblAgeRange.Text = Properties.Localization.txtCSearchTitleAgeRange;
            rbtnSearchGenderNS.Text = Properties.Localization.txtCSearchInfoGenderNA;
            rbtnSearchGenderFemale.Text = Properties.Localization.txtCSearchInfoGenderFemale;
            rbtnSearchGenderMale.Text = Properties.Localization.txtCSearchInfoGenderMale;
            lblLanguage.Text = Properties.Localization.txtCSearchTitleLanguage;
            lblCity.Text = Properties.Localization.txtCSearchTitleCity;
            lblState.Text = Properties.Localization.txtCSearchTitleState;
            lblCountry.Text = Properties.Localization.txtCSearchTitleCountry;
            pbarSearchProgressBar.Text = Properties.Localization.txtCSearchInfoSearching;
            myContactWindowDescriptionLabel.Text = Properties.Localization.txtCSearchInfoDesc;
            myContactWindowTitleLabel.Text = Properties.Localization.txtCSearchInfo;
            btnUserAdd.Text = Properties.Localization.txtCSearchBtnAddUser;
            Text = Properties.Localization.txtCSearchTilte;
        }

        private void BrandComponent()
        {
            this.Icon = Properties.Resources.desktop;
        }

        public List<UserSearchResult> mySearchUserResult = new List<UserSearchResult>();
        public class UserSearchResult
        {
            //first_name, last_name, city, company, country_name, username, comment_data
            private string _username;
            public string Username
            {
                get { return _username; }
                set { _username = value; }
            }
            private string _company;
            public string Company
            {
                get { return _company; }
                set { _company = value; }
            }
            private string _firstName;
            public string FirstName
            {
                get { return _firstName; }
                set { _firstName = value; }
            }
            private string _lastName;
            public string LastName
            {
                get { return _lastName; }
                set { _lastName = value; }
            }
            private string _region;
            public string Region
            {
                get { return _region; }
                set { _region = value; }
            }
            private string _country;
            public string Country
            {
                get { return _country; }
                set { _country = value; }
            }
            private string _comment;
            public string Comment
            {
                get { return _comment; }
                set { _comment = value; }
            }
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            btnUserFind.Enabled = false;
            pbarSearchProgressBar.Visible = true;
            pbarSearchProgressBar.StartWaiting();
            try
            {
                RSIFeaturesWS.RSIService searchService = new Remwave.Client.RSIFeaturesWS.RSIService();
                //U.USERNAME || ' '  || C.COMPANY || ' '|| C.LAST_NAME || ' ' || C.FIRST_NAME || ' ' || C.COMMENT_DATA|| ' ' || C.CITY || ' ' || CTR.NAME || ' '
                string keywords = "";
                keywords += "%"+tbxSearchKeyWords.Text.Replace("%","").Replace("?","") + "%";
                keywords += tbxSearchCity.Text.Replace("%", "").Replace("?", "") + " %";
                keywords += tbxSearchCountry.Text.Replace("%", "").Replace("?", "") + " %";
                searchService.utilsSearchUserCompleted += new Remwave.Client.RSIFeaturesWS.utilsSearchUserCompletedEventHandler(searchService_utilsSearchUserCompleted);
                searchService.utilsSearchUserAsync(myClientForm.mUserAccount.Username, myClientForm.mUserAccount.Password, keywords);
            }
            catch (Exception)
            {
                btnUserFind.Enabled = true;
                pbarSearchProgressBar.Visible = false;
                pbarSearchProgressBar.EndWaiting();
                
            }
        }

        void searchService_utilsSearchUserCompleted(object sender, Remwave.Client.RSIFeaturesWS.utilsSearchUserCompletedEventArgs e)
        {
            try
            {
                btnUserFind.Enabled = true;
                pbarSearchProgressBar.Visible = false;
                pbarSearchProgressBar.EndWaiting();

                List<UserSearchResult> searchResult = new List<UserSearchResult>();

                if (e.Error != null) return;
                if (e.Result != null)
                {
                    string[] result = e.Result;
                    if (result[0].Length<7) return;
                    foreach (string item in result)
                    {
                        UserSearchResult resultItem = new UserSearchResult();
                        try
                        {
                            string[] items = item.Split(new string[] { ";" }, StringSplitOptions.None);
                            //$first_name.";".$last_name.";".$city.";".$company.";".$country_name.";".$username.";".$comment_data
                            resultItem.Company = items[3] == null ? "" : items[3];
                            resultItem.FirstName = items[0] == null ? "" : items[0];
                            resultItem.LastName = items[1] == null ? "" : items[1];
                            resultItem.Region = items[2] == null ? "" : items[2];
                            resultItem.Country = items[4] == null ? "" : items[4];
                            resultItem.Username = items[5] == null ? "" : items[5];
                            resultItem.Comment = items[6] == null ? "" : items[6];
                            searchResult.Add(resultItem);
                        }
                        catch (Exception)
                        {
                           
                        }
                    }
                    DisplayResults(searchResult);
                }
            }
            catch (Exception)
            {
                
            }
        }

        private delegate void DisplayResultsDelegate(List<UserSearchResult> resultList);


        private void DisplayResults(List<UserSearchResult> resultList)
        {
            if (this.InvokeRequired)
            {
                BeginInvoke(new DisplayResultsDelegate(DisplayResults), new object[] { resultList });
            }
            else
            {
                try
                {
                mySearchUserResult = resultList;
                userSearchResultBindingSource.DataSource = mySearchUserResult;
                dataGridViewSearchResult.ForeColor = Color.Black;
                dataGridViewSearchResult.DataSource = null;
                dataGridViewSearchResult.DataSource = userSearchResultBindingSource;

            }
            catch (Exception)
            {
               
            }
            }
        }



        private void ContactsSearch_Load(object sender, EventArgs e)
        {
            cbxSearchAgeRange.SelectedIndex = 1;
        }

        private void dataGridViewSearchResult_SelectionChanged(object sender, EventArgs e)
        {
            btnUserAdd.Enabled = false;
            if (dataGridViewSearchResult.SelectedRows.Count > 0)
            {
                btnUserAdd.Enabled = true;
            }
        }

        private void btnUserAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridViewSearchResult.SelectedRows.Count > 0)
                {
                    UserSearchResult selected = mySearchUserResult[dataGridViewSearchResult.SelectedRows[0].Index];
                    selectedContact = new NTContact();
                    selectedContact.NTCompanyName = selected.Company;
                    selectedContact.NTJabberID = selected.Username;
                    selectedContact.NTFirstName = selected.FirstName;
                    selectedContact.NTLastName = selected.LastName;
                    selectedContact.NTHomeAddressCity = selected.Region;
                    selectedContact.NTHomeAddressCountry = selected.Country;
                }
            }
            catch (Exception)
            {
                
            }
            this.Close();
        }
    }
}