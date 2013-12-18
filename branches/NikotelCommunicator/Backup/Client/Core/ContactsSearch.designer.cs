namespace Remwave.Client
{
    partial class ContactsSearch
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ContactsSearch));
            this.dataGridViewSearchResult = new System.Windows.Forms.DataGridView();
            this.usernameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.companyDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.firstNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lastNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.countryDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.regionDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.commentDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.userSearchResultBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnUserFind = new Telerik.WinControls.UI.RadButton();
            this.cbxSearchAgeRange = new System.Windows.Forms.ComboBox();
            this.lblAgeRange = new System.Windows.Forms.Label();
            this.rbtnSearchGenderNS = new System.Windows.Forms.RadioButton();
            this.rbtnSearchGenderFemale = new System.Windows.Forms.RadioButton();
            this.rbtnSearchGenderMale = new System.Windows.Forms.RadioButton();
            this.label5 = new System.Windows.Forms.Label();
            this.tbxSearchLanguage = new System.Windows.Forms.TextBox();
            this.lblLanguage = new System.Windows.Forms.Label();
            this.tbxSearchCity = new System.Windows.Forms.TextBox();
            this.lblCity = new System.Windows.Forms.Label();
            this.tbxSearchState = new System.Windows.Forms.TextBox();
            this.lblState = new System.Windows.Forms.Label();
            this.tbxSearchCountry = new System.Windows.Forms.TextBox();
            this.lblCountry = new System.Windows.Forms.Label();
            this.tbxSearchKeyWords = new System.Windows.Forms.TextBox();
            this.pbarSearchProgressBar = new Telerik.WinControls.UI.RadWaitingBar();
            this.myTitleWhitePanel = new System.Windows.Forms.Panel();
            this.myContactWindowDescriptionLabel = new System.Windows.Forms.Label();
            this.myContactWindowTitleLabel = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnUserAdd = new Telerik.WinControls.UI.RadButton();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewSearchResult)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.userSearchResultBindingSource)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnUserFind)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbarSearchProgressBar)).BeginInit();
            this.myTitleWhitePanel.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnUserAdd)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridViewSearchResult
            // 
            this.dataGridViewSearchResult.AllowUserToAddRows = false;
            this.dataGridViewSearchResult.AllowUserToDeleteRows = false;
            this.dataGridViewSearchResult.AllowUserToOrderColumns = true;
            this.dataGridViewSearchResult.AutoGenerateColumns = false;
            this.dataGridViewSearchResult.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewSearchResult.BackgroundColor = System.Drawing.Color.White;
            this.dataGridViewSearchResult.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridViewSearchResult.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewSearchResult.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.usernameDataGridViewTextBoxColumn,
            this.companyDataGridViewTextBoxColumn,
            this.firstNameDataGridViewTextBoxColumn,
            this.lastNameDataGridViewTextBoxColumn,
            this.countryDataGridViewTextBoxColumn,
            this.regionDataGridViewTextBoxColumn,
            this.commentDataGridViewTextBoxColumn});
            this.dataGridViewSearchResult.DataSource = this.userSearchResultBindingSource;
            this.dataGridViewSearchResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewSearchResult.Location = new System.Drawing.Point(0, 230);
            this.dataGridViewSearchResult.MultiSelect = false;
            this.dataGridViewSearchResult.Name = "dataGridViewSearchResult";
            this.dataGridViewSearchResult.ReadOnly = true;
            this.dataGridViewSearchResult.RowHeadersVisible = false;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.Black;
            this.dataGridViewSearchResult.RowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridViewSearchResult.RowTemplate.DefaultCellStyle.BackColor = System.Drawing.Color.White;
            this.dataGridViewSearchResult.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dataGridViewSearchResult.RowTemplate.DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
            this.dataGridViewSearchResult.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.dataGridViewSearchResult.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.dataGridViewSearchResult.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewSearchResult.Size = new System.Drawing.Size(632, 206);
            this.dataGridViewSearchResult.TabIndex = 0;
            this.dataGridViewSearchResult.SelectionChanged += new System.EventHandler(this.dataGridViewSearchResult_SelectionChanged);
            // 
            // usernameDataGridViewTextBoxColumn
            // 
            this.usernameDataGridViewTextBoxColumn.DataPropertyName = "Username";
            this.usernameDataGridViewTextBoxColumn.HeaderText = "Username";
            this.usernameDataGridViewTextBoxColumn.Name = "usernameDataGridViewTextBoxColumn";
            this.usernameDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // companyDataGridViewTextBoxColumn
            // 
            this.companyDataGridViewTextBoxColumn.DataPropertyName = "Company";
            this.companyDataGridViewTextBoxColumn.HeaderText = "Company";
            this.companyDataGridViewTextBoxColumn.Name = "companyDataGridViewTextBoxColumn";
            this.companyDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // firstNameDataGridViewTextBoxColumn
            // 
            this.firstNameDataGridViewTextBoxColumn.DataPropertyName = "FirstName";
            this.firstNameDataGridViewTextBoxColumn.HeaderText = "First Name";
            this.firstNameDataGridViewTextBoxColumn.Name = "firstNameDataGridViewTextBoxColumn";
            this.firstNameDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // lastNameDataGridViewTextBoxColumn
            // 
            this.lastNameDataGridViewTextBoxColumn.DataPropertyName = "LastName";
            this.lastNameDataGridViewTextBoxColumn.HeaderText = "Last Name";
            this.lastNameDataGridViewTextBoxColumn.Name = "lastNameDataGridViewTextBoxColumn";
            this.lastNameDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // countryDataGridViewTextBoxColumn
            // 
            this.countryDataGridViewTextBoxColumn.DataPropertyName = "Country";
            this.countryDataGridViewTextBoxColumn.HeaderText = "Country";
            this.countryDataGridViewTextBoxColumn.Name = "countryDataGridViewTextBoxColumn";
            this.countryDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // regionDataGridViewTextBoxColumn
            // 
            this.regionDataGridViewTextBoxColumn.DataPropertyName = "Region";
            this.regionDataGridViewTextBoxColumn.HeaderText = "Region";
            this.regionDataGridViewTextBoxColumn.Name = "regionDataGridViewTextBoxColumn";
            this.regionDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // commentDataGridViewTextBoxColumn
            // 
            this.commentDataGridViewTextBoxColumn.DataPropertyName = "Comment";
            this.commentDataGridViewTextBoxColumn.HeaderText = "Comment";
            this.commentDataGridViewTextBoxColumn.Name = "commentDataGridViewTextBoxColumn";
            this.commentDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // userSearchResultBindingSource
            // 
            this.userSearchResultBindingSource.DataSource = typeof(Remwave.Client.ContactsSearch.UserSearchResult);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this.btnUserFind);
            this.panel1.Controls.Add(this.cbxSearchAgeRange);
            this.panel1.Controls.Add(this.lblAgeRange);
            this.panel1.Controls.Add(this.rbtnSearchGenderNS);
            this.panel1.Controls.Add(this.rbtnSearchGenderFemale);
            this.panel1.Controls.Add(this.rbtnSearchGenderMale);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.tbxSearchLanguage);
            this.panel1.Controls.Add(this.lblLanguage);
            this.panel1.Controls.Add(this.tbxSearchCity);
            this.panel1.Controls.Add(this.lblCity);
            this.panel1.Controls.Add(this.tbxSearchState);
            this.panel1.Controls.Add(this.lblState);
            this.panel1.Controls.Add(this.tbxSearchCountry);
            this.panel1.Controls.Add(this.lblCountry);
            this.panel1.Controls.Add(this.tbxSearchKeyWords);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 60);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(632, 170);
            this.panel1.TabIndex = 12;
            // 
            // btnUserFind
            // 
            this.btnUserFind.BackColor = System.Drawing.Color.White;
            this.btnUserFind.DisableMouseEvents = false;
            this.btnUserFind.DisplayStyle = Telerik.WinControls.DisplayStyle.ImageAndText;
            this.btnUserFind.ImageAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnUserFind.ImageList = null;
            this.btnUserFind.Location = new System.Drawing.Point(313, 15);
            this.btnUserFind.Name = "btnUserFind";
            // 
            // btnUserFind.RootElement
            // 
            this.btnUserFind.RootElement.AccessibleDescription = "";
            this.btnUserFind.RootElement.BackColor = System.Drawing.Color.White;
            this.btnUserFind.RootElement.KeyTip = "";
            this.btnUserFind.RootElement.ToolTipText = null;
            this.btnUserFind.Size = new System.Drawing.Size(75, 23);
            this.btnUserFind.SmallImageList = null;
            this.btnUserFind.TabIndex = 9;
            this.btnUserFind.Text = "Find";
            this.btnUserFind.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnUserFind.TextImageRelation = System.Windows.Forms.TextImageRelation.Overlay;
            this.btnUserFind.ThemeName = "Telerik";
            this.btnUserFind.Click += new System.EventHandler(this.btnFind_Click);
            // 
            // cbxSearchAgeRange
            // 
            this.cbxSearchAgeRange.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxSearchAgeRange.Font = new System.Drawing.Font("Trebuchet MS", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbxSearchAgeRange.FormattingEnabled = true;
            this.cbxSearchAgeRange.Items.AddRange(new object[] {
            "15-18",
            "19-24",
            "25-30",
            "31-36",
            "37-45",
            "46-54",
            "55-60",
            "61-70",
            "71-90",
            "90-..."});
            this.cbxSearchAgeRange.Location = new System.Drawing.Point(406, 131);
            this.cbxSearchAgeRange.Name = "cbxSearchAgeRange";
            this.cbxSearchAgeRange.Size = new System.Drawing.Size(74, 26);
            this.cbxSearchAgeRange.TabIndex = 8;
            // 
            // lblAgeRange
            // 
            this.lblAgeRange.AutoSize = true;
            this.lblAgeRange.Font = new System.Drawing.Font("Trebuchet MS", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAgeRange.Location = new System.Drawing.Point(328, 134);
            this.lblAgeRange.Name = "lblAgeRange";
            this.lblAgeRange.Size = new System.Drawing.Size(72, 18);
            this.lblAgeRange.TabIndex = 27;
            this.lblAgeRange.Text = "Age range:";
            // 
            // rbtnSearchGenderNS
            // 
            this.rbtnSearchGenderNS.AutoSize = true;
            this.rbtnSearchGenderNS.Checked = true;
            this.rbtnSearchGenderNS.Font = new System.Drawing.Font("Trebuchet MS", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbtnSearchGenderNS.Location = new System.Drawing.Point(391, 100);
            this.rbtnSearchGenderNS.Name = "rbtnSearchGenderNS";
            this.rbtnSearchGenderNS.Size = new System.Drawing.Size(102, 22);
            this.rbtnSearchGenderNS.TabIndex = 7;
            this.rbtnSearchGenderNS.TabStop = true;
            this.rbtnSearchGenderNS.Text = "not specified";
            this.rbtnSearchGenderNS.UseVisualStyleBackColor = true;
            // 
            // rbtnSearchGenderFemale
            // 
            this.rbtnSearchGenderFemale.AutoSize = true;
            this.rbtnSearchGenderFemale.Font = new System.Drawing.Font("Trebuchet MS", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbtnSearchGenderFemale.Location = new System.Drawing.Point(391, 72);
            this.rbtnSearchGenderFemale.Name = "rbtnSearchGenderFemale";
            this.rbtnSearchGenderFemale.Size = new System.Drawing.Size(68, 22);
            this.rbtnSearchGenderFemale.TabIndex = 6;
            this.rbtnSearchGenderFemale.Text = "Female";
            this.rbtnSearchGenderFemale.UseVisualStyleBackColor = true;
            // 
            // rbtnSearchGenderMale
            // 
            this.rbtnSearchGenderMale.AutoSize = true;
            this.rbtnSearchGenderMale.Font = new System.Drawing.Font("Trebuchet MS", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbtnSearchGenderMale.Location = new System.Drawing.Point(391, 44);
            this.rbtnSearchGenderMale.Name = "rbtnSearchGenderMale";
            this.rbtnSearchGenderMale.Size = new System.Drawing.Size(53, 22);
            this.rbtnSearchGenderMale.TabIndex = 5;
            this.rbtnSearchGenderMale.Text = "Male";
            this.rbtnSearchGenderMale.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Trebuchet MS", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(328, 47);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(55, 18);
            this.label5.TabIndex = 23;
            this.label5.Text = "Gender:";
            // 
            // tbxSearchLanguage
            // 
            this.tbxSearchLanguage.Font = new System.Drawing.Font("Trebuchet MS", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbxSearchLanguage.Location = new System.Drawing.Point(87, 131);
            this.tbxSearchLanguage.Name = "tbxSearchLanguage";
            this.tbxSearchLanguage.Size = new System.Drawing.Size(220, 23);
            this.tbxSearchLanguage.TabIndex = 4;
            // 
            // lblLanguage
            // 
            this.lblLanguage.AutoSize = true;
            this.lblLanguage.Font = new System.Drawing.Font("Trebuchet MS", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLanguage.Location = new System.Drawing.Point(12, 134);
            this.lblLanguage.Name = "lblLanguage";
            this.lblLanguage.Size = new System.Drawing.Size(69, 18);
            this.lblLanguage.TabIndex = 21;
            this.lblLanguage.Text = "Language:";
            // 
            // tbxSearchCity
            // 
            this.tbxSearchCity.Font = new System.Drawing.Font("Trebuchet MS", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbxSearchCity.Location = new System.Drawing.Point(87, 102);
            this.tbxSearchCity.Name = "tbxSearchCity";
            this.tbxSearchCity.Size = new System.Drawing.Size(220, 23);
            this.tbxSearchCity.TabIndex = 3;
            // 
            // lblCity
            // 
            this.lblCity.AutoSize = true;
            this.lblCity.Font = new System.Drawing.Font("Trebuchet MS", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCity.Location = new System.Drawing.Point(12, 106);
            this.lblCity.Name = "lblCity";
            this.lblCity.Size = new System.Drawing.Size(36, 18);
            this.lblCity.TabIndex = 19;
            this.lblCity.Text = "City:";
            // 
            // tbxSearchState
            // 
            this.tbxSearchState.Font = new System.Drawing.Font("Trebuchet MS", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbxSearchState.Location = new System.Drawing.Point(87, 73);
            this.tbxSearchState.Name = "tbxSearchState";
            this.tbxSearchState.Size = new System.Drawing.Size(220, 23);
            this.tbxSearchState.TabIndex = 2;
            // 
            // lblState
            // 
            this.lblState.AutoSize = true;
            this.lblState.Font = new System.Drawing.Font("Trebuchet MS", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblState.Location = new System.Drawing.Point(12, 76);
            this.lblState.Name = "lblState";
            this.lblState.Size = new System.Drawing.Size(44, 18);
            this.lblState.TabIndex = 17;
            this.lblState.Text = "State:";
            // 
            // tbxSearchCountry
            // 
            this.tbxSearchCountry.Font = new System.Drawing.Font("Trebuchet MS", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbxSearchCountry.Location = new System.Drawing.Point(87, 44);
            this.tbxSearchCountry.Name = "tbxSearchCountry";
            this.tbxSearchCountry.Size = new System.Drawing.Size(220, 23);
            this.tbxSearchCountry.TabIndex = 1;
            // 
            // lblCountry
            // 
            this.lblCountry.AutoSize = true;
            this.lblCountry.Font = new System.Drawing.Font("Trebuchet MS", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCountry.Location = new System.Drawing.Point(12, 47);
            this.lblCountry.Name = "lblCountry";
            this.lblCountry.Size = new System.Drawing.Size(58, 18);
            this.lblCountry.TabIndex = 15;
            this.lblCountry.Text = "Country:";
            // 
            // tbxSearchKeyWords
            // 
            this.tbxSearchKeyWords.Font = new System.Drawing.Font("Trebuchet MS", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbxSearchKeyWords.Location = new System.Drawing.Point(15, 15);
            this.tbxSearchKeyWords.Name = "tbxSearchKeyWords";
            this.tbxSearchKeyWords.Size = new System.Drawing.Size(292, 23);
            this.tbxSearchKeyWords.TabIndex = 0;
            // 
            // pbarSearchProgressBar
            // 
            this.pbarSearchProgressBar.BackColor = System.Drawing.Color.White;
            this.pbarSearchProgressBar.DisableMouseEvents = false;
            this.pbarSearchProgressBar.ImageList = null;
            this.pbarSearchProgressBar.Location = new System.Drawing.Point(3, 10);
            this.pbarSearchProgressBar.Name = "pbarSearchProgressBar";
            // 
            // pbarSearchProgressBar.RootElement
            // 
            this.pbarSearchProgressBar.RootElement.AccessibleDescription = "";
            this.pbarSearchProgressBar.RootElement.BackColor = System.Drawing.Color.White;
            this.pbarSearchProgressBar.RootElement.KeyTip = "";
            this.pbarSearchProgressBar.RootElement.ToolTipText = null;
            this.pbarSearchProgressBar.Size = new System.Drawing.Size(200, 10);
            this.pbarSearchProgressBar.SmallImageList = null;
            this.pbarSearchProgressBar.TabIndex = 29;
            this.pbarSearchProgressBar.Text = "Searching ...";
            this.pbarSearchProgressBar.Visible = false;
            this.pbarSearchProgressBar.WaitingIndicatorWidth = 5;
            this.pbarSearchProgressBar.WaitingSpeed = 10;
            // 
            // myTitleWhitePanel
            // 
            this.myTitleWhitePanel.BackColor = System.Drawing.Color.White;
            this.myTitleWhitePanel.BackgroundImage = global::Remwave.Client.Properties.Resources.UserSearchHead;
            this.myTitleWhitePanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.myTitleWhitePanel.Controls.Add(this.myContactWindowDescriptionLabel);
            this.myTitleWhitePanel.Controls.Add(this.myContactWindowTitleLabel);
            this.myTitleWhitePanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.myTitleWhitePanel.Location = new System.Drawing.Point(0, 0);
            this.myTitleWhitePanel.Name = "myTitleWhitePanel";
            this.myTitleWhitePanel.Size = new System.Drawing.Size(632, 60);
            this.myTitleWhitePanel.TabIndex = 9;
            // 
            // myContactWindowDescriptionLabel
            // 
            this.myContactWindowDescriptionLabel.AutoSize = true;
            this.myContactWindowDescriptionLabel.BackColor = System.Drawing.Color.Transparent;
            this.myContactWindowDescriptionLabel.Location = new System.Drawing.Point(138, 30);
            this.myContactWindowDescriptionLabel.Name = "myContactWindowDescriptionLabel";
            this.myContactWindowDescriptionLabel.Size = new System.Drawing.Size(345, 13);
            this.myContactWindowDescriptionLabel.TabIndex = 1;
            this.myContactWindowDescriptionLabel.Text = "If you know your friend nick, e-mail or full name enter it in the box below.";
            // 
            // myContactWindowTitleLabel
            // 
            this.myContactWindowTitleLabel.AutoSize = true;
            this.myContactWindowTitleLabel.BackColor = System.Drawing.Color.Transparent;
            this.myContactWindowTitleLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.myContactWindowTitleLabel.Location = new System.Drawing.Point(138, 9);
            this.myContactWindowTitleLabel.Name = "myContactWindowTitleLabel";
            this.myContactWindowTitleLabel.Size = new System.Drawing.Size(112, 15);
            this.myContactWindowTitleLabel.TabIndex = 0;
            this.myContactWindowTitleLabel.Text = "Search for users";
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.White;
            this.panel2.Controls.Add(this.btnUserAdd);
            this.panel2.Controls.Add(this.pbarSearchProgressBar);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 436);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(632, 30);
            this.panel2.TabIndex = 13;
            // 
            // btnUserAdd
            // 
            this.btnUserAdd.BackColor = System.Drawing.Color.White;
            this.btnUserAdd.DisableMouseEvents = false;
            this.btnUserAdd.DisplayStyle = Telerik.WinControls.DisplayStyle.ImageAndText;
            this.btnUserAdd.Enabled = false;
            this.btnUserAdd.ImageAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnUserAdd.ImageList = null;
            this.btnUserAdd.Location = new System.Drawing.Point(545, 4);
            this.btnUserAdd.Name = "btnUserAdd";
            // 
            // btnUserAdd.RootElement
            // 
            this.btnUserAdd.RootElement.AccessibleDescription = "";
            this.btnUserAdd.RootElement.BackColor = System.Drawing.Color.White;
            this.btnUserAdd.RootElement.KeyTip = "";
            this.btnUserAdd.RootElement.ToolTipText = null;
            this.btnUserAdd.Size = new System.Drawing.Size(75, 23);
            this.btnUserAdd.SmallImageList = null;
            this.btnUserAdd.TabIndex = 0;
            this.btnUserAdd.Text = "Add User";
            this.btnUserAdd.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnUserAdd.TextImageRelation = System.Windows.Forms.TextImageRelation.Overlay;
            this.btnUserAdd.ThemeName = "Telerik";
            this.btnUserAdd.Click += new System.EventHandler(this.btnUserAdd_Click);
            // 
            // ContactsSearch
            // 
            this.AcceptButton = this.btnUserFind;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(632, 466);
            this.Controls.Add(this.dataGridViewSearchResult);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.myTitleWhitePanel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(640, 500);
            this.Name = "ContactsSearch";
            this.Text = "Contacts Search";
            this.Load += new System.EventHandler(this.ContactsSearch_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewSearchResult)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.userSearchResultBindingSource)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnUserFind)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbarSearchProgressBar)).EndInit();
            this.myTitleWhitePanel.ResumeLayout(false);
            this.myTitleWhitePanel.PerformLayout();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.btnUserAdd)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel myTitleWhitePanel;
        private System.Windows.Forms.Label myContactWindowDescriptionLabel;
        private System.Windows.Forms.Label myContactWindowTitleLabel;
        private System.Windows.Forms.DataGridView dataGridViewSearchResult;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox tbxSearchKeyWords;
        private System.Windows.Forms.RadioButton rbtnSearchGenderMale;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tbxSearchLanguage;
        private System.Windows.Forms.Label lblLanguage;
        private System.Windows.Forms.TextBox tbxSearchCity;
        private System.Windows.Forms.Label lblCity;
        private System.Windows.Forms.TextBox tbxSearchState;
        private System.Windows.Forms.Label lblState;
        private System.Windows.Forms.TextBox tbxSearchCountry;
        private System.Windows.Forms.Label lblCountry;
        private System.Windows.Forms.ComboBox cbxSearchAgeRange;
        private System.Windows.Forms.Label lblAgeRange;
        private System.Windows.Forms.RadioButton rbtnSearchGenderNS;
        private System.Windows.Forms.RadioButton rbtnSearchGenderFemale;
        private Telerik.WinControls.UI.RadWaitingBar pbarSearchProgressBar;
        private System.Windows.Forms.Panel panel2;
        private Telerik.WinControls.UI.RadButton btnUserFind;
        private Telerik.WinControls.UI.RadButton btnUserAdd;

        private System.Windows.Forms.BindingSource userSearchResultBindingSource;
        private System.Windows.Forms.DataGridViewTextBoxColumn usernameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn companyDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn firstNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn lastNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn countryDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn regionDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn commentDataGridViewTextBoxColumn;
    }
}