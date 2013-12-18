namespace Remwave.Client.Controls
{
    partial class VoiceMailEntry
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
            this.components = new System.ComponentModel.Container();
            this.lFrom = new System.Windows.Forms.Label();
            this.lDate = new System.Windows.Forms.Label();
            this.pMailImage = new System.Windows.Forms.PictureBox();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.progressBar = new Telerik.WinControls.UI.RadWaitingBar();
            this.tbTagIt = new Remwave.Client.Controls.TransparentTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.pMailImage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.progressBar)).BeginInit();
            this.SuspendLayout();
            // 
            // lFrom
            // 
            this.lFrom.AutoEllipsis = true;
            this.lFrom.AutoSize = true;
            this.lFrom.BackColor = System.Drawing.Color.Transparent;
            this.lFrom.Dock = System.Windows.Forms.DockStyle.Left;
            this.lFrom.Font = new System.Drawing.Font("Trebuchet MS", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lFrom.Location = new System.Drawing.Point(32, 0);
            this.lFrom.MaximumSize = new System.Drawing.Size(126, 16);
            this.lFrom.Name = "lFrom";
            this.lFrom.Size = new System.Drawing.Size(120, 16);
            this.lFrom.TabIndex = 1;
            this.lFrom.Text = "From: Nobodyyuuuuuuuyyyyy";
            this.lFrom.Click += new System.EventHandler(this.bVoiceMailEntry_Click);
            // 
            // lDate
            // 
            this.lDate.AutoEllipsis = true;
            this.lDate.AutoSize = true;
            this.lDate.BackColor = System.Drawing.Color.Transparent;
            this.lDate.Dock = System.Windows.Forms.DockStyle.Right;
            this.lDate.Font = new System.Drawing.Font("Trebuchet MS", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lDate.Location = new System.Drawing.Point(258, 0);
            this.lDate.Name = "lDate";
            this.lDate.Size = new System.Drawing.Size(98, 16);
            this.lDate.TabIndex = 1;
            this.lDate.Text = "Nov 3, 2003 00:00";
            this.lDate.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.lDate.Click += new System.EventHandler(this.bVoiceMailEntry_Click);
            // 
            // pMailImage
            // 
            this.pMailImage.BackColor = System.Drawing.Color.Transparent;
            this.pMailImage.Dock = System.Windows.Forms.DockStyle.Left;
            this.pMailImage.ErrorImage = null;
            this.pMailImage.Image = global::Remwave.Client.Properties.Resources.listIconEmail;
            this.pMailImage.Location = new System.Drawing.Point(0, 0);
            this.pMailImage.Margin = new System.Windows.Forms.Padding(0);
            this.pMailImage.Name = "pMailImage";
            this.pMailImage.Size = new System.Drawing.Size(32, 35);
            this.pMailImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pMailImage.TabIndex = 3;
            this.pMailImage.TabStop = false;
            this.pMailImage.Click += new System.EventHandler(this.bVoiceMailEntry_Click);
            // 
            // progressBar
            // 
            this.progressBar.BackColor = System.Drawing.SystemColors.Info;
            this.progressBar.DisableMouseEvents = false;
            this.progressBar.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.progressBar.ImageList = null;
            this.progressBar.Location = new System.Drawing.Point(0, 35);
            this.progressBar.Margin = new System.Windows.Forms.Padding(0);
            this.progressBar.Name = "progressBar";
            // 
            // progressBar.RootElement
            // 
            this.progressBar.RootElement.AccessibleDescription = "";
            this.progressBar.RootElement.BackColor = System.Drawing.SystemColors.Info;
            this.progressBar.RootElement.KeyTip = "";
            this.progressBar.RootElement.ToolTipText = null;
            this.progressBar.Size = new System.Drawing.Size(356, 3);
            this.progressBar.SmallImageList = null;
            this.progressBar.TabIndex = 4;
            this.progressBar.Text = "Downloading";
            this.progressBar.WaitingSpeed = 10;
            // 
            // tbTagIt
            // 
            this.tbTagIt.BackColor = System.Drawing.Color.Transparent;
            this.tbTagIt.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbTagIt.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tbTagIt.Font = new System.Drawing.Font("Trebuchet MS", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbTagIt.ForeColor = System.Drawing.Color.DarkGray;
            this.tbTagIt.Location = new System.Drawing.Point(32, 15);
            this.tbTagIt.Margin = new System.Windows.Forms.Padding(0);
            this.tbTagIt.MaxLength = 64;
            this.tbTagIt.Multiline = true;
            this.tbTagIt.Name = "tbTagIt";
            this.tbTagIt.Size = new System.Drawing.Size(324, 20);
            this.tbTagIt.TabIndex = 4;
            this.tbTagIt.Text = "Tag it, click here";
            this.tbTagIt.Click += new System.EventHandler(this.bVoiceMailEntry_Click);
            this.tbTagIt.Leave += new System.EventHandler(this.tbTagIt_Leave);
            // 
            // VoiceMailEntry
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Info;
            this.BackgroundImage = global::Remwave.Client.Properties.Resources.listBackgroundNormal39;
            this.Controls.Add(this.lDate);
            this.Controls.Add(this.lFrom);
            this.Controls.Add(this.tbTagIt);
            this.Controls.Add(this.pMailImage);
            this.Controls.Add(this.progressBar);
            this.DoubleBuffered = true;
            this.Name = "VoiceMailEntry";
            this.Padding = new System.Windows.Forms.Padding(0, 0, 0, 1);
            this.Size = new System.Drawing.Size(356, 39);
            this.Click += new System.EventHandler(this.bVoiceMailEntry_Click);
            ((System.ComponentModel.ISupportInitialize)(this.pMailImage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.progressBar)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lFrom;
        private System.Windows.Forms.Label lDate;
        private System.Windows.Forms.PictureBox pMailImage;
        private System.Windows.Forms.ToolTip toolTip;
        private Telerik.WinControls.UI.RadWaitingBar progressBar;
        private TransparentTextBox tbTagIt;
    }
}
