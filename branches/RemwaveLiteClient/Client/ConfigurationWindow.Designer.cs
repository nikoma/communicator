namespace Remwave.Client
{
    partial class ConfigurationWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConfigurationWindow));
            this.tbarConfiguration = new Telerik.WinControls.UI.RadTitleBar();
            this.imageList2 = new System.Windows.Forms.ImageList(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.lnChangeSettings = new System.Windows.Forms.LinkLabel();
            this.lblServiceProvider = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cbServiceProviders = new Telerik.WinControls.UI.RadComboBox();
            this.radComboBoxItem1 = new Telerik.WinControls.UI.RadComboBoxItem();
            this.tbVideoProxyAddress = new System.Windows.Forms.TextBox();
            this.lblVideoProxyAddress = new System.Windows.Forms.Label();
            this.lblVideoConferenceConfiguraiton = new System.Windows.Forms.Label();
            this.btnSaveChanges = new Telerik.WinControls.UI.RadButton();
            this.tbIMServerAddress = new System.Windows.Forms.TextBox();
            this.tbSipRealm = new System.Windows.Forms.TextBox();
            this.tbSipProxyAddress = new System.Windows.Forms.TextBox();
            this.lblIMServerAddress = new System.Windows.Forms.Label();
            this.lblIMConfiguration = new System.Windows.Forms.Label();
            this.lblSIPRealm = new System.Windows.Forms.Label();
            this.lblSIPProxyAddress = new System.Windows.Forms.Label();
            this.lblSIPConfiguration = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.tbarConfiguration)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cbServiceProviders)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnSaveChanges)).BeginInit();
            this.SuspendLayout();
            // 
            // tbarConfiguration
            // 
            this.tbarConfiguration.Caption = "Configuration";
            this.tbarConfiguration.DisableMouseEvents = false;
            this.tbarConfiguration.Dock = System.Windows.Forms.DockStyle.Top;
            this.tbarConfiguration.ImageList = this.imageList2;
            this.tbarConfiguration.Location = new System.Drawing.Point(0, 0);
            this.tbarConfiguration.Name = "tbarConfiguration";
            // 
            // tbarConfiguration.RootElement
            // 
            this.tbarConfiguration.RootElement.AccessibleDescription = "";
            this.tbarConfiguration.RootElement.KeyTip = "";
            this.tbarConfiguration.RootElement.ToolTipText = null;
            ((Telerik.WinControls.UI.RadImageButtonElement)(this.tbarConfiguration.RootElement.GetChildAt(0).GetChildAt(2).GetChildAt(2).GetChildAt(0))).Visibility = Telerik.WinControls.ElementVisibility.Hidden;
            ((Telerik.WinControls.UI.RadImageButtonElement)(this.tbarConfiguration.RootElement.GetChildAt(0).GetChildAt(2).GetChildAt(2).GetChildAt(1))).Visibility = Telerik.WinControls.ElementVisibility.Hidden;
            this.tbarConfiguration.Size = new System.Drawing.Size(300, 23);
            this.tbarConfiguration.SmallImageList = null;
            this.tbarConfiguration.TabIndex = 1;
            this.tbarConfiguration.TabStop = false;
            this.tbarConfiguration.Text = "Configuration";
            this.tbarConfiguration.ThemeName = "Office2007Black";
            // 
            // imageList2
            // 
            this.imageList2.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList2.ImageStream")));
            this.imageList2.TransparentColor = System.Drawing.Color.Magenta;
            this.imageList2.Images.SetKeyName(0, "minimize.png");
            this.imageList2.Images.SetKeyName(1, "maximize.png");
            this.imageList2.Images.SetKeyName(2, "close.png");
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Black;
            this.panel1.BackgroundImage = global::Remwave.Client.Properties.Resources.SkinBGSettings;
            this.panel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.panel1.Controls.Add(this.lnChangeSettings);
            this.panel1.Controls.Add(this.lblServiceProvider);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.cbServiceProviders);
            this.panel1.Controls.Add(this.tbVideoProxyAddress);
            this.panel1.Controls.Add(this.lblVideoProxyAddress);
            this.panel1.Controls.Add(this.lblVideoConferenceConfiguraiton);
            this.panel1.Controls.Add(this.btnSaveChanges);
            this.panel1.Controls.Add(this.tbIMServerAddress);
            this.panel1.Controls.Add(this.tbSipRealm);
            this.panel1.Controls.Add(this.tbSipProxyAddress);
            this.panel1.Controls.Add(this.lblIMServerAddress);
            this.panel1.Controls.Add(this.lblIMConfiguration);
            this.panel1.Controls.Add(this.lblSIPRealm);
            this.panel1.Controls.Add(this.lblSIPProxyAddress);
            this.panel1.Controls.Add(this.lblSIPConfiguration);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 23);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(300, 377);
            this.panel1.TabIndex = 2;
            // 
            // lnChangeSettings
            // 
            this.lnChangeSettings.ActiveLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.lnChangeSettings.AutoSize = true;
            this.lnChangeSettings.LinkColor = System.Drawing.Color.Gray;
            this.lnChangeSettings.Location = new System.Drawing.Point(117, 94);
            this.lnChangeSettings.Name = "lnChangeSettings";
            this.lnChangeSettings.Size = new System.Drawing.Size(80, 13);
            this.lnChangeSettings.TabIndex = 19;
            this.lnChangeSettings.TabStop = true;
            this.lnChangeSettings.Text = "Create account";
            this.lnChangeSettings.VisitedLinkColor = System.Drawing.Color.Gray;
            this.lnChangeSettings.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnChangeSettings_LinkClicked);
            // 
            // lblServiceProvider
            // 
            this.lblServiceProvider.AutoSize = true;
            this.lblServiceProvider.ForeColor = System.Drawing.Color.White;
            this.lblServiceProvider.Location = new System.Drawing.Point(30, 75);
            this.lblServiceProvider.Name = "lblServiceProvider";
            this.lblServiceProvider.Size = new System.Drawing.Size(52, 13);
            this.lblServiceProvider.TabIndex = 18;
            this.lblServiceProvider.Text = "Provider :";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(30, 52);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(113, 16);
            this.label1.TabIndex = 17;
            this.label1.Text = "Preset providers :";
            // 
            // cbServiceProviders
            // 
            this.cbServiceProviders.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.cbServiceProviders.BackColor = System.Drawing.Color.Black;
            this.cbServiceProviders.DisableMouseEvents = false;
            this.cbServiceProviders.DropDownMaxSize = new System.Drawing.Size(0, 0);
            this.cbServiceProviders.DropDownMinSize = new System.Drawing.Size(0, 0);
            this.cbServiceProviders.DropDownSizingMode = ((Telerik.WinControls.UI.SizingMode)((Telerik.WinControls.UI.SizingMode.RightBottom | Telerik.WinControls.UI.SizingMode.UpDown)));
            this.cbServiceProviders.ImageList = null;
            this.cbServiceProviders.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.radComboBoxItem1});
            this.cbServiceProviders.Location = new System.Drawing.Point(120, 72);
            this.cbServiceProviders.MaxLength = 32767;
            this.cbServiceProviders.Name = "cbServiceProviders";
            this.cbServiceProviders.NullText = "";
            // 
            // cbServiceProviders.RootElement
            // 
            this.cbServiceProviders.RootElement.AccessibleDescription = "";
            this.cbServiceProviders.RootElement.AutoSizeMode = Telerik.WinControls.RadAutoSizeMode.WrapAroundChildren;
            this.cbServiceProviders.RootElement.BackColor = System.Drawing.Color.Black;
            this.cbServiceProviders.RootElement.KeyTip = "";
            this.cbServiceProviders.RootElement.ToolTipText = null;
            this.cbServiceProviders.Size = new System.Drawing.Size(150, 19);
            this.cbServiceProviders.SmallImageList = null;
            this.cbServiceProviders.TabIndex = 16;
            this.cbServiceProviders.ThemeName = "ControlDefault";
            this.cbServiceProviders.SelectedIndexChanged += new System.EventHandler(this.cbServiceProviders_SelectedIndexChanged);
            // 
            // radComboBoxItem1
            // 
            this.radComboBoxItem1.AccessibleDescription = "";
            this.radComboBoxItem1.DescriptionFont = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radComboBoxItem1.DescriptionText = "Your customized configuration.";
            this.radComboBoxItem1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radComboBoxItem1.KeyTip = "";
            this.radComboBoxItem1.Text = "Custom";
            this.radComboBoxItem1.TextSeparatorVisibility = Telerik.WinControls.ElementVisibility.Visible;
            this.radComboBoxItem1.ToolTipText = null;
            // 
            // tbVideoProxyAddress
            // 
            this.tbVideoProxyAddress.Location = new System.Drawing.Point(120, 287);
            this.tbVideoProxyAddress.Name = "tbVideoProxyAddress";
            this.tbVideoProxyAddress.Size = new System.Drawing.Size(150, 20);
            this.tbVideoProxyAddress.TabIndex = 15;
            // 
            // lblVideoProxyAddress
            // 
            this.lblVideoProxyAddress.AutoSize = true;
            this.lblVideoProxyAddress.ForeColor = System.Drawing.Color.White;
            this.lblVideoProxyAddress.Location = new System.Drawing.Point(30, 290);
            this.lblVideoProxyAddress.Name = "lblVideoProxyAddress";
            this.lblVideoProxyAddress.Size = new System.Drawing.Size(84, 13);
            this.lblVideoProxyAddress.TabIndex = 14;
            this.lblVideoProxyAddress.Text = "Server address :";
            // 
            // lblVideoConferenceConfiguraiton
            // 
            this.lblVideoConferenceConfiguraiton.AutoSize = true;
            this.lblVideoConferenceConfiguraiton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVideoConferenceConfiguraiton.ForeColor = System.Drawing.Color.White;
            this.lblVideoConferenceConfiguraiton.Location = new System.Drawing.Point(30, 268);
            this.lblVideoConferenceConfiguraiton.Name = "lblVideoConferenceConfiguraiton";
            this.lblVideoConferenceConfiguraiton.Size = new System.Drawing.Size(233, 16);
            this.lblVideoConferenceConfiguraiton.TabIndex = 13;
            this.lblVideoConferenceConfiguraiton.Text = "Video and collaboration configuration:";
            // 
            // btnSaveChanges
            // 
            this.btnSaveChanges.BackColor = System.Drawing.Color.Black;
            this.btnSaveChanges.DisableMouseEvents = false;
            this.btnSaveChanges.DisplayStyle = Telerik.WinControls.DisplayStyle.ImageAndText;
            this.btnSaveChanges.ImageAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSaveChanges.ImageList = null;
            this.btnSaveChanges.Location = new System.Drawing.Point(195, 342);
            this.btnSaveChanges.Name = "btnSaveChanges";
            // 
            // btnSaveChanges.RootElement
            // 
            this.btnSaveChanges.RootElement.AccessibleDescription = "";
            this.btnSaveChanges.RootElement.BackColor = System.Drawing.Color.Black;
            this.btnSaveChanges.RootElement.KeyTip = "";
            this.btnSaveChanges.RootElement.ToolTipText = null;
            this.btnSaveChanges.Size = new System.Drawing.Size(75, 23);
            this.btnSaveChanges.SmallImageList = null;
            this.btnSaveChanges.TabIndex = 12;
            this.btnSaveChanges.Text = "Save";
            this.btnSaveChanges.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnSaveChanges.TextImageRelation = System.Windows.Forms.TextImageRelation.Overlay;
            this.btnSaveChanges.ThemeName = "Office2007Black";
            this.btnSaveChanges.Click += new System.EventHandler(this.btnSaveChanges_Click);
            // 
            // tbIMServerAddress
            // 
            this.tbIMServerAddress.Location = new System.Drawing.Point(120, 230);
            this.tbIMServerAddress.Name = "tbIMServerAddress";
            this.tbIMServerAddress.Size = new System.Drawing.Size(150, 20);
            this.tbIMServerAddress.TabIndex = 10;
            // 
            // tbSipRealm
            // 
            this.tbSipRealm.Location = new System.Drawing.Point(120, 178);
            this.tbSipRealm.Name = "tbSipRealm";
            this.tbSipRealm.Size = new System.Drawing.Size(150, 20);
            this.tbSipRealm.TabIndex = 9;
            // 
            // tbSipProxyAddress
            // 
            this.tbSipProxyAddress.Location = new System.Drawing.Point(120, 152);
            this.tbSipProxyAddress.Name = "tbSipProxyAddress";
            this.tbSipProxyAddress.Size = new System.Drawing.Size(150, 20);
            this.tbSipProxyAddress.TabIndex = 7;
            // 
            // lblIMServerAddress
            // 
            this.lblIMServerAddress.AutoSize = true;
            this.lblIMServerAddress.ForeColor = System.Drawing.Color.White;
            this.lblIMServerAddress.Location = new System.Drawing.Point(30, 233);
            this.lblIMServerAddress.Name = "lblIMServerAddress";
            this.lblIMServerAddress.Size = new System.Drawing.Size(84, 13);
            this.lblIMServerAddress.TabIndex = 5;
            this.lblIMServerAddress.Text = "Server address :";
            // 
            // lblIMConfiguration
            // 
            this.lblIMConfiguration.AutoSize = true;
            this.lblIMConfiguration.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblIMConfiguration.ForeColor = System.Drawing.Color.White;
            this.lblIMConfiguration.Location = new System.Drawing.Point(30, 211);
            this.lblIMConfiguration.Name = "lblIMConfiguration";
            this.lblIMConfiguration.Size = new System.Drawing.Size(107, 16);
            this.lblIMConfiguration.TabIndex = 4;
            this.lblIMConfiguration.Text = "IM configuration :";
            // 
            // lblSIPRealm
            // 
            this.lblSIPRealm.AutoSize = true;
            this.lblSIPRealm.ForeColor = System.Drawing.Color.White;
            this.lblSIPRealm.Location = new System.Drawing.Point(30, 181);
            this.lblSIPRealm.Name = "lblSIPRealm";
            this.lblSIPRealm.Size = new System.Drawing.Size(43, 13);
            this.lblSIPRealm.TabIndex = 3;
            this.lblSIPRealm.Text = "Realm :";
            // 
            // lblSIPProxyAddress
            // 
            this.lblSIPProxyAddress.AutoSize = true;
            this.lblSIPProxyAddress.ForeColor = System.Drawing.Color.White;
            this.lblSIPProxyAddress.Location = new System.Drawing.Point(30, 155);
            this.lblSIPProxyAddress.Name = "lblSIPProxyAddress";
            this.lblSIPProxyAddress.Size = new System.Drawing.Size(79, 13);
            this.lblSIPProxyAddress.TabIndex = 1;
            this.lblSIPProxyAddress.Text = "Proxy address :";
            // 
            // lblSIPConfiguration
            // 
            this.lblSIPConfiguration.AutoSize = true;
            this.lblSIPConfiguration.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSIPConfiguration.ForeColor = System.Drawing.Color.White;
            this.lblSIPConfiguration.Location = new System.Drawing.Point(30, 133);
            this.lblSIPConfiguration.Name = "lblSIPConfiguration";
            this.lblSIPConfiguration.Size = new System.Drawing.Size(114, 16);
            this.lblSIPConfiguration.TabIndex = 0;
            this.lblSIPConfiguration.Text = "SIP configuration :";
            // 
            // ConfigurationWindow
            // 
            this.AcceptButton = this.btnSaveChanges;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(300, 400);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.tbarConfiguration);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ConfigurationWindow";
            this.Text = "Configuration";
            ((System.ComponentModel.ISupportInitialize)(this.tbarConfiguration)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cbServiceProviders)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnSaveChanges)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Telerik.WinControls.UI.RadTitleBar tbarConfiguration;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblSIPConfiguration;
        private System.Windows.Forms.Label lblSIPProxyAddress;
        private System.Windows.Forms.Label lblSIPRealm;
        private System.Windows.Forms.Label lblIMConfiguration;
        private System.Windows.Forms.TextBox tbIMServerAddress;
        private System.Windows.Forms.TextBox tbSipRealm;
        private System.Windows.Forms.TextBox tbSipProxyAddress;
        private System.Windows.Forms.Label lblIMServerAddress;
        private System.Windows.Forms.Label lblVideoProxyAddress;
        private System.Windows.Forms.Label lblVideoConferenceConfiguraiton;
        private Telerik.WinControls.UI.RadButton btnSaveChanges;
        private System.Windows.Forms.TextBox tbVideoProxyAddress;
        private System.Windows.Forms.ImageList imageList2;
        private System.Windows.Forms.Label lblServiceProvider;
        private System.Windows.Forms.Label label1;
        private Telerik.WinControls.UI.RadComboBox cbServiceProviders;
        private Telerik.WinControls.UI.RadComboBoxItem radComboBoxItem1;
        private System.Windows.Forms.LinkLabel lnChangeSettings;
    }
}