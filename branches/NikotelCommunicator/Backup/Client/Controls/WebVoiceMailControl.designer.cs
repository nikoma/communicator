namespace Remwave.Client.Controls
{
    partial class WebVoiceMailControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WebVoiceMailControl));
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.toolButtonPlay = new System.Windows.Forms.ToolStripButton();
            this.toolButtonStop = new System.Windows.Forms.ToolStripButton();
            this.toolButtonGet = new System.Windows.Forms.ToolStripButton();
            this.toolButtonEmail = new System.Windows.Forms.ToolStripButton();
            this.toolButtonDelete = new System.Windows.Forms.ToolStripButton();
            this.toolButtonOptions = new System.Windows.Forms.ToolStripDropDownButton();
            this.toolButtonRefresh = new System.Windows.Forms.ToolStripButton();
            this.toolButtonKeepSync = new System.Windows.Forms.ToolStripButton();
            this.pEmailEntries = new System.Windows.Forms.Panel();
            this.TopPanel = new System.Windows.Forms.Panel();
            this.toolStrip.SuspendLayout();
            this.TopPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip
            // 
            this.toolStrip.BackColor = System.Drawing.Color.White;
            this.toolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolButtonPlay,
            this.toolButtonStop,
            this.toolButtonGet,
            this.toolButtonEmail,
            this.toolButtonDelete,
            this.toolButtonOptions});
            this.toolStrip.Location = new System.Drawing.Point(0, 0);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.toolStrip.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.toolStrip.Size = new System.Drawing.Size(298, 25);
            this.toolStrip.TabIndex = 0;
            this.toolStrip.Text = "toolStrip1";
            // 
            // toolButtonPlay
            // 
            this.toolButtonPlay.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolButtonPlay.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolButtonPlay.Image = ((System.Drawing.Image)(resources.GetObject("toolButtonPlay.Image")));
            this.toolButtonPlay.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolButtonPlay.Name = "toolButtonPlay";
            this.toolButtonPlay.Size = new System.Drawing.Size(31, 22);
            this.toolButtonPlay.Text = "Play";
            this.toolButtonPlay.Visible = false;
            this.toolButtonPlay.Click += new System.EventHandler(this.toolButtonPlay_Click);
            // 
            // toolButtonStop
            // 
            this.toolButtonStop.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolButtonStop.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolButtonStop.Image = ((System.Drawing.Image)(resources.GetObject("toolButtonStop.Image")));
            this.toolButtonStop.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolButtonStop.Name = "toolButtonStop";
            this.toolButtonStop.Size = new System.Drawing.Size(33, 22);
            this.toolButtonStop.Text = "Stop";
            this.toolButtonStop.Visible = false;
            this.toolButtonStop.Click += new System.EventHandler(this.toolButtonStop_Click);
            // 
            // toolButtonGet
            // 
            this.toolButtonGet.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolButtonGet.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolButtonGet.Image = ((System.Drawing.Image)(resources.GetObject("toolButtonGet.Image")));
            this.toolButtonGet.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolButtonGet.Name = "toolButtonGet";
            this.toolButtonGet.Size = new System.Drawing.Size(28, 22);
            this.toolButtonGet.Text = "Get";
            this.toolButtonGet.Visible = false;
            this.toolButtonGet.Click += new System.EventHandler(this.toolButtonGet_Click);
            // 
            // toolButtonEmail
            // 
            this.toolButtonEmail.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolButtonEmail.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolButtonEmail.Image = ((System.Drawing.Image)(resources.GetObject("toolButtonEmail.Image")));
            this.toolButtonEmail.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolButtonEmail.Name = "toolButtonEmail";
            this.toolButtonEmail.Size = new System.Drawing.Size(35, 22);
            this.toolButtonEmail.Text = "Email";
            this.toolButtonEmail.Visible = false;
            this.toolButtonEmail.Click += new System.EventHandler(this.toolButtonEmail_Click);
            // 
            // toolButtonDelete
            // 
            this.toolButtonDelete.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolButtonDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolButtonDelete.Image = ((System.Drawing.Image)(resources.GetObject("toolButtonDelete.Image")));
            this.toolButtonDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolButtonDelete.Name = "toolButtonDelete";
            this.toolButtonDelete.Size = new System.Drawing.Size(42, 22);
            this.toolButtonDelete.Text = "Delete";
            this.toolButtonDelete.Visible = false;
            this.toolButtonDelete.Click += new System.EventHandler(this.toolButtonDelete_Click);
            // 
            // toolButtonOptions
            // 
            this.toolButtonOptions.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolButtonOptions.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolButtonRefresh,
            this.toolButtonKeepSync});
            this.toolButtonOptions.Image = ((System.Drawing.Image)(resources.GetObject("toolButtonOptions.Image")));
            this.toolButtonOptions.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolButtonOptions.Name = "toolButtonOptions";
            this.toolButtonOptions.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.toolButtonOptions.Size = new System.Drawing.Size(57, 22);
            this.toolButtonOptions.Text = "Options";
            // 
            // toolButtonRefresh
            // 
            this.toolButtonRefresh.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolButtonRefresh.CheckOnClick = true;
            this.toolButtonRefresh.Image = global::Remwave.Client.Properties.Resources.iconRefresh;
            this.toolButtonRefresh.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.toolButtonRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolButtonRefresh.Name = "toolButtonRefresh";
            this.toolButtonRefresh.Size = new System.Drawing.Size(65, 20);
            this.toolButtonRefresh.Text = "Refresh";
            this.toolButtonRefresh.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.toolButtonRefresh.Click += new System.EventHandler(this.bRefresh_Click);
            // 
            // toolButtonKeepSync
            // 
            this.toolButtonKeepSync.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolButtonKeepSync.CheckOnClick = true;
            this.toolButtonKeepSync.Image = global::Remwave.Client.Properties.Resources.iconSync;
            this.toolButtonKeepSync.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.toolButtonKeepSync.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolButtonKeepSync.Name = "toolButtonKeepSync";
            this.toolButtonKeepSync.Size = new System.Drawing.Size(85, 20);
            this.toolButtonKeepSync.Text = "Synchronize";
            this.toolButtonKeepSync.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.toolButtonKeepSync.ToolTipText = "Keep Email Syncronized";
            this.toolButtonKeepSync.Click += new System.EventHandler(this.bKeepSyned_Click);
            // 
            // pEmailEntries
            // 
            this.pEmailEntries.AutoScroll = true;
            this.pEmailEntries.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pEmailEntries.Location = new System.Drawing.Point(0, 25);
            this.pEmailEntries.Name = "pEmailEntries";
            this.pEmailEntries.Size = new System.Drawing.Size(298, 330);
            this.pEmailEntries.TabIndex = 1;
            // 
            // TopPanel
            // 
            this.TopPanel.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.TopPanel.Controls.Add(this.toolStrip);
            this.TopPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.TopPanel.Location = new System.Drawing.Point(0, 0);
            this.TopPanel.Name = "TopPanel";
            this.TopPanel.Size = new System.Drawing.Size(298, 25);
            this.TopPanel.TabIndex = 0;
            // 
            // WebVoiceMailControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pEmailEntries);
            this.Controls.Add(this.TopPanel);
            this.Name = "WebVoiceMailControl";
            this.Size = new System.Drawing.Size(298, 355);
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.TopPanel.ResumeLayout(false);
            this.TopPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pEmailEntries;
        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.Panel TopPanel;
        private System.Windows.Forms.ToolStripButton toolButtonStop;
        private System.Windows.Forms.ToolStripButton toolButtonPlay;
        private System.Windows.Forms.ToolStripButton toolButtonGet;
        private System.Windows.Forms.ToolStripButton toolButtonEmail;
        private System.Windows.Forms.ToolStripButton toolButtonDelete;
        private System.Windows.Forms.ToolStripDropDownButton toolButtonOptions;
        private System.Windows.Forms.ToolStripButton toolButtonRefresh;
        private System.Windows.Forms.ToolStripButton toolButtonKeepSync;

    }
}
