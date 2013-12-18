namespace Remwave.Client
{
    partial class SpeedDialWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SpeedDialWindow));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tbxSearchText = new Telerik.WinControls.UI.RadTextBox();
            this.lbxSearchResults = new Telerik.WinControls.UI.RadListBox();
            this.btnShowContacts = new Telerik.WinControls.UI.RadButton();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbxSearchText)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lbxSearchResults)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnShowContacts)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.BackColor = System.Drawing.Color.Black;
            this.tableLayoutPanel1.BackgroundImage = global::Remwave.Client.Properties.Resources.SearchUser;
            this.tableLayoutPanel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 52F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.tbxSearchText, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.lbxSearchResults, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.btnShowContacts, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(10, 10);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 48F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(300, 348);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // tbxSearchText
            // 
            this.tbxSearchText.AcceptsReturn = false;
            this.tbxSearchText.AcceptsTab = false;
            this.tbxSearchText.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.tbxSearchText.BackColor = System.Drawing.Color.White;
            this.tbxSearchText.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            this.tbxSearchText.DisableMouseEvents = false;
            this.tbxSearchText.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbxSearchText.HideSelection = true;
            this.tbxSearchText.ImageList = null;
            this.tbxSearchText.Lines = new string[0];
            this.tbxSearchText.Location = new System.Drawing.Point(60, 15);
            this.tbxSearchText.Margin = new System.Windows.Forms.Padding(3, 3, 6, 3);
            this.tbxSearchText.MaxLength = 32767;
            this.tbxSearchText.Modified = false;
            this.tbxSearchText.Multiline = false;
            this.tbxSearchText.Name = "tbxSearchText";
            this.tbxSearchText.NullText = "";
            this.tbxSearchText.PasswordChar = '\0';
            this.tbxSearchText.ReadOnly = false;
            // 
            // tbxSearchText.RootElement
            // 
            this.tbxSearchText.RootElement.AccessibleDescription = "";
            this.tbxSearchText.RootElement.AutoSizeMode = Telerik.WinControls.RadAutoSizeMode.WrapAroundChildren;
            this.tbxSearchText.RootElement.BackColor = System.Drawing.Color.White;
            this.tbxSearchText.RootElement.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbxSearchText.RootElement.KeyTip = "";
            this.tbxSearchText.RootElement.ToolTipText = null;
            this.tbxSearchText.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.tbxSearchText.SelectedText = global::Remwave.Client.Properties.Localization.txtBtnDialPadPound;
            this.tbxSearchText.SelectionLength = 0;
            this.tbxSearchText.SelectionStart = 0;
            this.tbxSearchText.ShortcutsEnabled = true;
            this.tbxSearchText.Size = new System.Drawing.Size(229, 17);
            this.tbxSearchText.SmallImageList = null;
            this.tbxSearchText.TabIndex = 1;
            this.tbxSearchText.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.tbxSearchText.ThemeName = "ControlDefault";
            this.tbxSearchText.WordWrap = true;
            this.tbxSearchText.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbxSearchText_KeyDown);
            this.tbxSearchText.KeyUp += new System.Windows.Forms.KeyEventHandler(this.tbxSearchText_KeyUp);
            // 
            // lbxSearchResults
            // 
            this.lbxSearchResults.BackColor = System.Drawing.Color.White;
            this.tableLayoutPanel1.SetColumnSpan(this.lbxSearchResults, 2);
            this.lbxSearchResults.DisableMouseEvents = false;
            this.lbxSearchResults.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbxSearchResults.FormatString = null;
            this.lbxSearchResults.ImageList = null;
            this.lbxSearchResults.Location = new System.Drawing.Point(0, 48);
            this.lbxSearchResults.Margin = new System.Windows.Forms.Padding(0);
            this.lbxSearchResults.Name = "lbxSearchResults";
            // 
            // lbxSearchResults.RootElement
            // 
            this.lbxSearchResults.RootElement.AccessibleDescription = "";
            this.lbxSearchResults.RootElement.BackColor = System.Drawing.Color.White;
            this.lbxSearchResults.RootElement.KeyTip = "";
            this.lbxSearchResults.RootElement.ToolTipText = null;
            this.lbxSearchResults.Size = new System.Drawing.Size(300, 300);
            this.lbxSearchResults.SmallImageList = null;
            this.lbxSearchResults.TabIndex = 2;
            this.lbxSearchResults.ThemeName = "Telerik";
            this.lbxSearchResults.KeyUp += new System.Windows.Forms.KeyEventHandler(this.lbxSearchResults_KeyUp);
            // 
            // btnShowContacts
            // 
            this.btnShowContacts.BackColor = System.Drawing.Color.Transparent;
            this.btnShowContacts.DisableMouseEvents = false;
            this.btnShowContacts.DisplayStyle = Telerik.WinControls.DisplayStyle.Image;
            this.btnShowContacts.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnShowContacts.Image = global::Remwave.Client.Properties.Resources.buttonContacts;
            this.btnShowContacts.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnShowContacts.ImageList = null;
            this.btnShowContacts.Location = new System.Drawing.Point(3, 3);
            this.btnShowContacts.Name = "btnShowContacts";
            // 
            // btnShowContacts.RootElement
            // 
            this.btnShowContacts.RootElement.AccessibleDescription = "";
            this.btnShowContacts.RootElement.BackColor = System.Drawing.Color.Transparent;
            this.btnShowContacts.RootElement.KeyTip = "";
            this.btnShowContacts.RootElement.ToolTipText = null;
            this.btnShowContacts.Size = new System.Drawing.Size(46, 42);
            this.btnShowContacts.SmallImageList = null;
            this.btnShowContacts.TabIndex = 3;
            this.btnShowContacts.Text = "radButton1";
            this.btnShowContacts.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnShowContacts.TextImageRelation = System.Windows.Forms.TextImageRelation.Overlay;
            this.btnShowContacts.ThemeName = "Plain";
            this.btnShowContacts.Click += new System.EventHandler(this.btnShowContacts_Click);
            // 
            // SpeedDialWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(254)))), ((int)(((byte)(254)))), ((int)(((byte)(254)))));
            this.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(152)))), ((int)(((byte)(152)))), ((int)(((byte)(152)))));
            this.ClientSize = new System.Drawing.Size(320, 368);
            this.Controls.Add(this.tableLayoutPanel1);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(320, 468);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(320, 68);
            this.Name = "SpeedDialWindow";
            this.Opacity = 0.99;
            this.Padding = new System.Windows.Forms.Padding(10);
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SpeedDialWindow";
            this.ThemeName = "Telerik";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.SpeedDialWindow_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbxSearchText)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lbxSearchResults)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnShowContacts)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private Telerik.WinControls.UI.RadTextBox tbxSearchText;
        private Telerik.WinControls.UI.RadListBox lbxSearchResults;
        private Telerik.WinControls.UI.RadButton btnShowContacts;
    }
}