namespace Remwave.Client.Controls
{
    partial class NikotalkieItem
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
            this.LabelDate = new System.Windows.Forms.Label();
            this.LabelFrom = new System.Windows.Forms.Label();
            this.ItemIcon = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.ItemIcon)).BeginInit();
            this.SuspendLayout();
            // 
            // LabelDate
            // 
            this.LabelDate.AutoSize = true;
            this.LabelDate.BackColor = System.Drawing.Color.Transparent;
            this.LabelDate.Dock = System.Windows.Forms.DockStyle.Right;
            this.LabelDate.Font = new System.Drawing.Font("Trebuchet MS", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LabelDate.ForeColor = System.Drawing.Color.White;
            this.LabelDate.Location = new System.Drawing.Point(205, 0);
            this.LabelDate.Name = "LabelDate";
            this.LabelDate.Size = new System.Drawing.Size(95, 16);
            this.LabelDate.TabIndex = 1;
            this.LabelDate.Text = "2007.10.12 23:59";
            this.LabelDate.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.LabelDate.DoubleClick += new System.EventHandler(this.NikotalkieItem_DoubleClick);
            this.LabelDate.Click += new System.EventHandler(this.NikotalkieItem_Click);
            // 
            // LabelFrom
            // 
            this.LabelFrom.AutoSize = true;
            this.LabelFrom.BackColor = System.Drawing.Color.Transparent;
            this.LabelFrom.Font = new System.Drawing.Font("Trebuchet MS", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LabelFrom.ForeColor = System.Drawing.Color.White;
            this.LabelFrom.Location = new System.Drawing.Point(25, 3);
            this.LabelFrom.Name = "LabelFrom";
            this.LabelFrom.Size = new System.Drawing.Size(41, 18);
            this.LabelFrom.TabIndex = 0;
            this.LabelFrom.Text = "dzius";
            this.LabelFrom.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.LabelFrom.DoubleClick += new System.EventHandler(this.NikotalkieItem_DoubleClick);
            this.LabelFrom.Click += new System.EventHandler(this.NikotalkieItem_Click);
            // 
            // ItemIcon
            // 
            this.ItemIcon.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.ItemIcon.ErrorImage = null;
            this.ItemIcon.Image = global::Remwave.Client.Properties.Resources.NikotalkieIconMessage20;
            this.ItemIcon.InitialImage = null;
            this.ItemIcon.Location = new System.Drawing.Point(2, 2);
            this.ItemIcon.Margin = new System.Windows.Forms.Padding(0);
            this.ItemIcon.Name = "ItemIcon";
            this.ItemIcon.Size = new System.Drawing.Size(20, 20);
            this.ItemIcon.TabIndex = 2;
            this.ItemIcon.TabStop = false;
            this.ItemIcon.DoubleClick += new System.EventHandler(this.NikotalkieItem_DoubleClick);
            this.ItemIcon.Click += new System.EventHandler(this.NikotalkieItem_Click);
            // 
            // NikotalkieItem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.BackgroundImage = global::Remwave.Client.Properties.Resources.NikotalkieItemBackgroundBlack;
            this.Controls.Add(this.LabelDate);
            this.Controls.Add(this.LabelFrom);
            this.Controls.Add(this.ItemIcon);
            this.DoubleBuffered = true;
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "NikotalkieItem";
            this.Size = new System.Drawing.Size(300, 24);
            this.DoubleClick += new System.EventHandler(this.NikotalkieItem_DoubleClick);
            this.Click += new System.EventHandler(this.NikotalkieItem_Click);
            ((System.ComponentModel.ISupportInitialize)(this.ItemIcon)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.Label LabelFrom;
        public System.Windows.Forms.Label LabelDate;
        public System.Windows.Forms.PictureBox ItemIcon;
    }
}
