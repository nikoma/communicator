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
            this.loadingProgressBar.Location = new System.Drawing.Point(0, 265);
            this.loadingProgressBar.Name = "loadingProgressBar";
            this.loadingProgressBar.ProgressOrientation = Telerik.WinControls.ProgressOrientation.Left;
            // 
            // loadingProgressBar.RootElement
            // 
            this.loadingProgressBar.RootElement.AccessibleDescription = "";
            this.loadingProgressBar.RootElement.BackColor = System.Drawing.Color.White;
            this.loadingProgressBar.RootElement.KeyTip = "";
            this.loadingProgressBar.RootElement.ToolTipText = null;
            this.loadingProgressBar.SeparatorColor1 = System.Drawing.Color.FromArgb(((int)(((byte)(190)))), ((int)(((byte)(205)))), ((int)(((byte)(205)))), ((int)(((byte)(205)))));
            this.loadingProgressBar.SeparatorColor2 = System.Drawing.Color.White;
            this.loadingProgressBar.SeparatorWidth = 9;
            this.loadingProgressBar.ShowProgressIndicators = false;
            this.loadingProgressBar.Size = new System.Drawing.Size(570, 3);
            this.loadingProgressBar.SmallImageList = null;
            this.loadingProgressBar.StepWidth = 18;
            this.loadingProgressBar.SweepAngle = 45;
            this.loadingProgressBar.TabIndex = 0;
            this.loadingProgressBar.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.loadingProgressBar.ThemeName = "Dash1";
            this.loadingProgressBar.Value1 = 60;
            // 
            // lblVersion
            // 
            this.lblVersion.AutoSize = true;
            this.lblVersion.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(121)))), ((int)(((byte)(0)))));
            this.lblVersion.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVersion.ForeColor = System.Drawing.Color.White;
            this.lblVersion.Location = new System.Drawing.Point(24, 155);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new System.Drawing.Size(81, 13);
            this.lblVersion.TabIndex = 1;
            this.lblVersion.Text = "Version : 2.2.11";
            // 
            // Splash
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImage = global::Remwave.Client.Properties.Resources.splashImage;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ClientSize = new System.Drawing.Size(570, 268);
            this.Controls.Add(this.lblVersion);
            this.Controls.Add(this.loadingProgressBar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(570, 268);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(570, 268);
            this.Name = "Splash";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Splash";
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