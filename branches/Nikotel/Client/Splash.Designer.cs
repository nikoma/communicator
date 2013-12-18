namespace Remwave.Client
{
    partial class Splash
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Splash));
            this.loadingProgressBar = new Telerik.WinControls.UI.RadProgressBar();
            this.lblVersion = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.loadingProgressBar)).BeginInit();
            this.SuspendLayout();
            // 
            // loadingProgressBar
            // 
            this.loadingProgressBar.BackColor = System.Drawing.Color.White;
            this.loadingProgressBar.DisableMouseEvents = false;
            this.loadingProgressBar.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.loadingProgressBar.ImageList = null;
            this.loadingProgressBar.Location = new System.Drawing.Point(1, 155);
            this.loadingProgressBar.Name = "loadingProgressBar";
            this.loadingProgressBar.ProgressOrientation = Telerik.WinControls.ProgressOrientation.Left;
            // 
            // loadingProgressBar.RootElement
            // 
            this.loadingProgressBar.RootElement.AccessibleDescription = "";
            this.loadingProgressBar.RootElement.BackColor = System.Drawing.Color.White;
            this.loadingProgressBar.RootElement.KeyTip = "";
            this.loadingProgressBar.RootElement.ToolTipText = null;
            this.loadingProgressBar.SeparatorColor1 = System.Drawing.Color.FromArgb(((int)(((byte)(10)))), ((int)(((byte)(36)))), ((int)(((byte)(106)))));
            this.loadingProgressBar.SeparatorColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(10)))), ((int)(((byte)(36)))), ((int)(((byte)(106)))));
            this.loadingProgressBar.SeparatorWidth = 15;
            this.loadingProgressBar.ShowProgressIndicators = false;
            this.loadingProgressBar.Size = new System.Drawing.Size(318, 5);
            this.loadingProgressBar.SmallImageList = null;
            this.loadingProgressBar.StepWidth = 17;
            this.loadingProgressBar.SweepAngle = 135;
            this.loadingProgressBar.TabIndex = 0;
            this.loadingProgressBar.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.loadingProgressBar.ThemeName = "Classic";
            this.loadingProgressBar.Value1 = 60;
            // 
            // lblVersion
            // 
            this.lblVersion.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblVersion.AutoEllipsis = true;
            this.lblVersion.AutoSize = true;
            this.lblVersion.BackColor = System.Drawing.Color.Transparent;
            this.lblVersion.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVersion.ForeColor = System.Drawing.Color.White;
            this.lblVersion.Location = new System.Drawing.Point(4, 139);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new System.Drawing.Size(84, 13);
            this.lblVersion.TabIndex = 1;
            this.lblVersion.Text = "Version : 0.0.0.0";
            this.lblVersion.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Splash
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ClientSize = new System.Drawing.Size(320, 160);
            this.Controls.Add(this.lblVersion);
            this.Controls.Add(this.loadingProgressBar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(320, 160);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(320, 160);
            this.Name = "Splash";
            this.Padding = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "nikotel";
            this.Load += new System.EventHandler(this.Splash_Load);
            ((System.ComponentModel.ISupportInitialize)(this.loadingProgressBar)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Telerik.WinControls.UI.RadProgressBar loadingProgressBar;
        private System.Windows.Forms.Label lblVersion;

    }
}
