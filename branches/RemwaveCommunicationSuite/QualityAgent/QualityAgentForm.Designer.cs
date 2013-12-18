namespace QualityAgent
{
    partial class QualityAgentForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(QualityAgentForm));
            this.ExceptionPreviewTextBox = new System.Windows.Forms.TextBox();
            this.MainToolStrip = new System.Windows.Forms.ToolStrip();
            this.OpenLogStripButton = new System.Windows.Forms.ToolStripButton();
            this.SendLogFileStripButton = new System.Windows.Forms.ToolStripButton();
            this.OpenLogFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.MainToolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // ExceptionPreviewTextBox
            // 
            this.ExceptionPreviewTextBox.AcceptsReturn = true;
            this.ExceptionPreviewTextBox.AcceptsTab = true;
            this.ExceptionPreviewTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ExceptionPreviewTextBox.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ExceptionPreviewTextBox.Location = new System.Drawing.Point(0, 25);
            this.ExceptionPreviewTextBox.MaxLength = 262144;
            this.ExceptionPreviewTextBox.Multiline = true;
            this.ExceptionPreviewTextBox.Name = "ExceptionPreviewTextBox";
            this.ExceptionPreviewTextBox.ReadOnly = true;
            this.ExceptionPreviewTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.ExceptionPreviewTextBox.Size = new System.Drawing.Size(592, 341);
            this.ExceptionPreviewTextBox.TabIndex = 0;
            this.ExceptionPreviewTextBox.TextChanged += new System.EventHandler(this.ExceptionPreviewTextBox_TextChanged);
            // 
            // MainToolStrip
            // 
            this.MainToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.OpenLogStripButton,
            this.SendLogFileStripButton});
            this.MainToolStrip.Location = new System.Drawing.Point(0, 0);
            this.MainToolStrip.Name = "MainToolStrip";
            this.MainToolStrip.Size = new System.Drawing.Size(592, 25);
            this.MainToolStrip.TabIndex = 1;
            this.MainToolStrip.Text = "toolStrip1";
            // 
            // OpenLogStripButton
            // 
            this.OpenLogStripButton.Image = ((System.Drawing.Image)(resources.GetObject("OpenLogStripButton.Image")));
            this.OpenLogStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.OpenLogStripButton.Name = "OpenLogStripButton";
            this.OpenLogStripButton.Size = new System.Drawing.Size(72, 22);
            this.OpenLogStripButton.Text = "Open &File";
            this.OpenLogStripButton.Click += new System.EventHandler(this.OpenLogStripButton_Click);
            // 
            // SendLogFileStripButton
            // 
            this.SendLogFileStripButton.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SendLogFileStripButton.Image = ((System.Drawing.Image)(resources.GetObject("SendLogFileStripButton.Image")));
            this.SendLogFileStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.SendLogFileStripButton.Name = "SendLogFileStripButton";
            this.SendLogFileStripButton.Size = new System.Drawing.Size(97, 22);
            this.SendLogFileStripButton.Text = "&Send Report";
            this.SendLogFileStripButton.Click += new System.EventHandler(this.SendLogFileStripButton_Click);
            // 
            // OpenLogFileDialog
            // 
            this.OpenLogFileDialog.FileName = "*.xml";
            this.OpenLogFileDialog.Filter = "XML Files|*.xml";
            // 
            // QualityAgentForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(592, 366);
            this.Controls.Add(this.ExceptionPreviewTextBox);
            this.Controls.Add(this.MainToolStrip);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "QualityAgentForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Quality Agent";
            this.Load += new System.EventHandler(this.QualityAgentForm_Load);
            this.MainToolStrip.ResumeLayout(false);
            this.MainToolStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox ExceptionPreviewTextBox;
        private System.Windows.Forms.ToolStrip MainToolStrip;
        private System.Windows.Forms.ToolStripButton SendLogFileStripButton;
        private System.Windows.Forms.ToolStripButton OpenLogStripButton;
        private System.Windows.Forms.OpenFileDialog OpenLogFileDialog;
    }
}

