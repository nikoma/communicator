namespace Remwave.Client
{
    partial class ArchiveWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ArchiveWindow));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tvUserList = new System.Windows.Forms.TreeView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.wbConversation = new System.Windows.Forms.WebBrowser();
            this.panelSearch = new System.Windows.Forms.Panel();
            this.tbxSearchText = new System.Windows.Forms.TextBox();
            this.btnSearchCancel = new System.Windows.Forms.Label();
            this.lblSearchIcon = new System.Windows.Forms.Label();
            this.timerSearchLauncher = new System.Windows.Forms.Timer(this.components);
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panelSearch.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.tvUserList);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.panel1);
            this.splitContainer1.Panel2.Controls.Add(this.panelSearch);
            this.splitContainer1.Size = new System.Drawing.Size(492, 386);
            this.splitContainer1.SplitterDistance = 164;
            this.splitContainer1.TabIndex = 0;
            // 
            // tvUserList
            // 
            this.tvUserList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvUserList.Location = new System.Drawing.Point(0, 0);
            this.tvUserList.Name = "tvUserList";
            this.tvUserList.ShowRootLines = false;
            this.tvUserList.Size = new System.Drawing.Size(164, 386);
            this.tvUserList.TabIndex = 0;
            this.tvUserList.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvUserList_AfterSelect);
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.wbConversation);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 30);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(324, 356);
            this.panel1.TabIndex = 4;
            // 
            // wbConversation
            // 
            this.wbConversation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wbConversation.IsWebBrowserContextMenuEnabled = false;
            this.wbConversation.Location = new System.Drawing.Point(0, 0);
            this.wbConversation.MinimumSize = new System.Drawing.Size(20, 20);
            this.wbConversation.Name = "wbConversation";
            this.wbConversation.Size = new System.Drawing.Size(320, 352);
            this.wbConversation.TabIndex = 0;
            // 
            // panelSearch
            // 
            this.panelSearch.BackColor = System.Drawing.SystemColors.Control;
            this.panelSearch.Controls.Add(this.tbxSearchText);
            this.panelSearch.Controls.Add(this.btnSearchCancel);
            this.panelSearch.Controls.Add(this.lblSearchIcon);
            this.panelSearch.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelSearch.Location = new System.Drawing.Point(0, 0);
            this.panelSearch.Name = "panelSearch";
            this.panelSearch.Size = new System.Drawing.Size(324, 30);
            this.panelSearch.TabIndex = 4;
            // 
            // tbxSearchText
            // 
            this.tbxSearchText.Font = new System.Drawing.Font("Trebuchet MS", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbxSearchText.Location = new System.Drawing.Point(25, 5);
            this.tbxSearchText.Name = "tbxSearchText";
            this.tbxSearchText.Size = new System.Drawing.Size(200, 21);
            this.tbxSearchText.TabIndex = 3;
            this.tbxSearchText.TextChanged += new System.EventHandler(this.tbxSearchText_TextChanged);
            // 
            // btnSearchCancel
            // 
            this.btnSearchCancel.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnSearchCancel.Image = global::Remwave.Client.Properties.Resources.iconCross;
            this.btnSearchCancel.Location = new System.Drawing.Point(308, 0);
            this.btnSearchCancel.Name = "btnSearchCancel";
            this.btnSearchCancel.Padding = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.btnSearchCancel.Size = new System.Drawing.Size(16, 30);
            this.btnSearchCancel.TabIndex = 2;
            this.btnSearchCancel.Click += new System.EventHandler(this.btnSearchCancel_Click);
            // 
            // lblSearchIcon
            // 
            this.lblSearchIcon.Image = global::Remwave.Client.Properties.Resources.iconMagnifier;
            this.lblSearchIcon.Location = new System.Drawing.Point(3, 0);
            this.lblSearchIcon.Name = "lblSearchIcon";
            this.lblSearchIcon.Padding = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.lblSearchIcon.Size = new System.Drawing.Size(16, 30);
            this.lblSearchIcon.TabIndex = 1;
            this.lblSearchIcon.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // timerSearchLauncher
            // 
            this.timerSearchLauncher.Enabled = true;
            this.timerSearchLauncher.Interval = 1000;
            this.timerSearchLauncher.Tick += new System.EventHandler(this.timerSearchLauncher_Tick);
            // 
            // ArchiveWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(492, 386);
            this.Controls.Add(this.splitContainer1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ArchiveWindow";
            this.Text = "Archive";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panelSearch.ResumeLayout(false);
            this.panelSearch.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.WebBrowser wbConversation;
        public System.Windows.Forms.TreeView tvUserList;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panelSearch;
        private System.Windows.Forms.TextBox tbxSearchText;
        private System.Windows.Forms.Label btnSearchCancel;
        private System.Windows.Forms.Label lblSearchIcon;
        private System.Windows.Forms.Timer timerSearchLauncher;
    }
}