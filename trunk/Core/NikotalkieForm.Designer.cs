namespace Remwave.Client
{
    partial class NikotalkieForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NikotalkieForm));
            this.nikotalkieControl = new Remwave.Client.Controls.NikotalkieControl();
            this.SuspendLayout();
            // 
            // nikotalkieControl
            // 
            this.nikotalkieControl.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.nikotalkieControl.BackgroundImage = global::Remwave.Client.Properties.Resources.NikotalkieControlBackground;
            this.nikotalkieControl.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.nikotalkieControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nikotalkieControl.Location = new System.Drawing.Point(0, 0);
            this.nikotalkieControl.Margin = new System.Windows.Forms.Padding(0);
            this.nikotalkieControl.Name = "nikotalkieControl";
            this.nikotalkieControl.Padding = new System.Windows.Forms.Padding(0, 60, 0, 0);
            this.nikotalkieControl.Size = new System.Drawing.Size(314, 444);
            this.nikotalkieControl.TabIndex = 0;
            // 
            // NikotalkieForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.ClientSize = new System.Drawing.Size(314, 444);
            this.Controls.Add(this.nikotalkieControl);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "NikotalkieForm";
            this.Text = "Nikotalkie";
            this.WindowState = System.Windows.Forms.FormWindowState.Minimized;
            this.Shown += new System.EventHandler(this.NikotalkieForm_Shown);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.NikotalkieForm_FormClosing);
            this.Load += new System.EventHandler(this.NikotalkieForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        public Remwave.Client.Controls.NikotalkieControl nikotalkieControl;
    }
}