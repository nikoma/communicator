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
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.listView1 = new System.Windows.Forms.ListView();
            this.wbConversation = new System.Windows.Forms.WebBrowser();
            this.panelSearch = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.btnSearchCancel = new System.Windows.Forms.Label();
            this.tbxSearchText = new System.Windows.Forms.TextBox();
            this.lblSearchIcon = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.timerSearchLauncher = new System.Windows.Forms.Timer(this.components);
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.panelSearch.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 30);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.tvUserList);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(632, 416);
            this.splitContainer1.SplitterDistance = 128;
            this.splitContainer1.TabIndex = 0;
            // 
            // tvUserList
            // 
            this.tvUserList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvUserList.FullRowSelect = true;
            this.tvUserList.HideSelection = false;
            this.tvUserList.Location = new System.Drawing.Point(0, 0);
            this.tvUserList.Name = "tvUserList";
            this.tvUserList.ShowRootLines = false;
            this.tvUserList.Size = new System.Drawing.Size(128, 416);
            this.tvUserList.TabIndex = 0;
            this.tvUserList.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvUserList_AfterSelect);
            this.tvUserList.KeyUp += new System.Windows.Forms.KeyEventHandler(this.tvUserList_KeyUp);
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.listView1);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.splitContainer2.Panel2.Controls.Add(this.wbConversation);
            this.splitContainer2.Panel2.Padding = new System.Windows.Forms.Padding(1);
            this.splitContainer2.Size = new System.Drawing.Size(500, 416);
            this.splitContainer2.SplitterDistance = 128;
            this.splitContainer2.TabIndex = 5;
            // 
            // listView1
            // 
            this.listView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView1.FullRowSelect = true;
            this.listView1.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.listView1.HideSelection = false;
            this.listView1.Location = new System.Drawing.Point(0, 0);
            this.listView1.MultiSelect = false;
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(128, 416);
            this.listView1.TabIndex = 0;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            this.listView1.SelectedIndexChanged += new System.EventHandler(this.listView1_SelectedIndexChanged);
            // 
            // wbConversation
            // 
            this.wbConversation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wbConversation.IsWebBrowserContextMenuEnabled = false;
            this.wbConversation.Location = new System.Drawing.Point(1, 1);
            this.wbConversation.MinimumSize = new System.Drawing.Size(20, 20);
            this.wbConversation.Name = "wbConversation";
            this.wbConversation.Size = new System.Drawing.Size(366, 414);
            this.wbConversation.TabIndex = 0;
            // 
            // panelSearch
            // 
            this.panelSearch.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(211)))), ((int)(((byte)(212)))), ((int)(((byte)(211)))));
            this.panelSearch.BackgroundImage = global::Remwave.Client.Properties.Resources.SearchUser;
            this.panelSearch.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.panelSearch.Controls.Add(this.tableLayoutPanel1);
            this.panelSearch.Controls.Add(this.label1);
            this.panelSearch.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelSearch.Location = new System.Drawing.Point(0, 0);
            this.panelSearch.Name = "panelSearch";
            this.panelSearch.Size = new System.Drawing.Size(632, 30);
            this.panelSearch.TabIndex = 4;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.btnSearchCancel, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.tbxSearchText, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblSearchIcon, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(378, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(254, 30);
            this.tableLayoutPanel1.TabIndex = 4;
            // 
            // btnSearchCancel
            // 
            this.btnSearchCancel.Image = global::Remwave.Client.Properties.Resources.iconCross;
            this.btnSearchCancel.Location = new System.Drawing.Point(235, 0);
            this.btnSearchCancel.Name = "btnSearchCancel";
            this.btnSearchCancel.Padding = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.btnSearchCancel.Size = new System.Drawing.Size(16, 30);
            this.btnSearchCancel.TabIndex = 2;
            this.btnSearchCancel.Click += new System.EventHandler(this.btnSearchCancel_Click);
            // 
            // tbxSearchText
            // 
            this.tbxSearchText.Font = new System.Drawing.Font("Trebuchet MS", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbxSearchText.Location = new System.Drawing.Point(27, 5);
            this.tbxSearchText.Margin = new System.Windows.Forms.Padding(5);
            this.tbxSearchText.Name = "tbxSearchText";
            this.tbxSearchText.Size = new System.Drawing.Size(200, 21);
            this.tbxSearchText.TabIndex = 0;
            this.tbxSearchText.TextChanged += new System.EventHandler(this.tbxSearchText_TextChanged);
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
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Trebuchet MS", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(3, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(0, 24);
            this.label1.TabIndex = 3;
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
            this.ClientSize = new System.Drawing.Size(632, 446);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.panelSearch);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ArchiveWindow";
            this.Text = "Archive";
            this.Load += new System.EventHandler(this.ArchiveWindow_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.ResumeLayout(false);
            this.panelSearch.ResumeLayout(false);
            this.panelSearch.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.WebBrowser wbConversation;
        public System.Windows.Forms.TreeView tvUserList;
        private System.Windows.Forms.Panel panelSearch;
        private System.Windows.Forms.TextBox tbxSearchText;
        private System.Windows.Forms.Label btnSearchCancel;
        private System.Windows.Forms.Label lblSearchIcon;
        private System.Windows.Forms.Timer timerSearchLauncher;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    }
}