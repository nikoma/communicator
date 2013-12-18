namespace Remwave.Client
{
    partial class Phone2PhoneWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Phone2PhoneWindow));
            this.meNameLabel = new System.Windows.Forms.Label();
            this.selectedCallLabel = new System.Windows.Forms.Label();
            this.myTitleWhitePanel = new System.Windows.Forms.Panel();
            this.myContactWindowDescriptionLabel = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.selectedNameLabel = new System.Windows.Forms.Label();
            this.meCallLabel = new System.Windows.Forms.Label();
            this.myDialPadCallOrAnswerButton = new System.Windows.Forms.Button();
            this.myDialPadCallCancelButton = new System.Windows.Forms.Button();
            this.selectedContactComboBox = new Telerik.WinControls.UI.RadComboBox();
            this.tmpRadComboBoxItem = new Telerik.WinControls.UI.RadComboBoxItem();
            this.meContactComboBox = new Telerik.WinControls.UI.RadComboBox();
            this.radTitleBar1 = new Telerik.WinControls.UI.RadTitleBar();
            this.imageList2 = new System.Windows.Forms.ImageList(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.myTitleWhitePanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.selectedContactComboBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.meContactComboBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radTitleBar1)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // meNameLabel
            // 
            this.meNameLabel.AutoSize = true;
            this.meNameLabel.Location = new System.Drawing.Point(66, 73);
            this.meNameLabel.Name = "meNameLabel";
            this.meNameLabel.Size = new System.Drawing.Size(95, 13);
            this.meNameLabel.TabIndex = 2;
            this.meNameLabel.Text = "My Phone Number";
            // 
            // selectedCallLabel
            // 
            this.selectedCallLabel.AutoSize = true;
            this.selectedCallLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.selectedCallLabel.Location = new System.Drawing.Point(12, 19);
            this.selectedCallLabel.Name = "selectedCallLabel";
            this.selectedCallLabel.Size = new System.Drawing.Size(48, 15);
            this.selectedCallLabel.TabIndex = 3;
            this.selectedCallLabel.Text = "Call ...";
            // 
            // myTitleWhitePanel
            // 
            this.myTitleWhitePanel.BackColor = System.Drawing.Color.Transparent;
            this.myTitleWhitePanel.Controls.Add(this.myContactWindowDescriptionLabel);
            this.myTitleWhitePanel.Controls.Add(this.label4);
            this.myTitleWhitePanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.myTitleWhitePanel.ForeColor = System.Drawing.Color.White;
            this.myTitleWhitePanel.Location = new System.Drawing.Point(2, 24);
            this.myTitleWhitePanel.Name = "myTitleWhitePanel";
            this.myTitleWhitePanel.Size = new System.Drawing.Size(288, 45);
            this.myTitleWhitePanel.TabIndex = 8;
            // 
            // myContactWindowDescriptionLabel
            // 
            this.myContactWindowDescriptionLabel.AutoSize = true;
            this.myContactWindowDescriptionLabel.ForeColor = System.Drawing.Color.White;
            this.myContactWindowDescriptionLabel.Location = new System.Drawing.Point(4, 25);
            this.myContactWindowDescriptionLabel.Name = "myContactWindowDescriptionLabel";
            this.myContactWindowDescriptionLabel.Size = new System.Drawing.Size(248, 13);
            this.myContactWindowDescriptionLabel.TabIndex = 2;
            this.myContactWindowDescriptionLabel.Text = "Enter two phone numbers and we connect the call ";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(4, 4);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(118, 15);
            this.label4.TabIndex = 0;
            this.label4.Text = "Start nikotalk call";
            // 
            // selectedNameLabel
            // 
            this.selectedNameLabel.AutoSize = true;
            this.selectedNameLabel.Location = new System.Drawing.Point(66, 21);
            this.selectedNameLabel.Name = "selectedNameLabel";
            this.selectedNameLabel.Size = new System.Drawing.Size(134, 13);
            this.selectedNameLabel.TabIndex = 9;
            this.selectedNameLabel.Text = "John Doe\'s Phone Number";
            // 
            // meCallLabel
            // 
            this.meCallLabel.AutoSize = true;
            this.meCallLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.meCallLabel.Location = new System.Drawing.Point(13, 71);
            this.meCallLabel.Name = "meCallLabel";
            this.meCallLabel.Size = new System.Drawing.Size(47, 15);
            this.meCallLabel.TabIndex = 10;
            this.meCallLabel.Text = "and ...";
            // 
            // myDialPadCallOrAnswerButton
            // 
            this.myDialPadCallOrAnswerButton.Image = ((System.Drawing.Image)(resources.GetObject("myDialPadCallOrAnswerButton.Image")));
            this.myDialPadCallOrAnswerButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.myDialPadCallOrAnswerButton.Location = new System.Drawing.Point(90, 132);
            this.myDialPadCallOrAnswerButton.Name = "myDialPadCallOrAnswerButton";
            this.myDialPadCallOrAnswerButton.Size = new System.Drawing.Size(47, 31);
            this.myDialPadCallOrAnswerButton.TabIndex = 11;
            this.myDialPadCallOrAnswerButton.Text = "Call";
            this.myDialPadCallOrAnswerButton.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.myDialPadCallOrAnswerButton.UseVisualStyleBackColor = true;
            this.myDialPadCallOrAnswerButton.Click += new System.EventHandler(this.myDialPadCallOrAnswerButton_Click);
            // 
            // myDialPadCallCancelButton
            // 
            this.myDialPadCallCancelButton.Image = ((System.Drawing.Image)(resources.GetObject("myDialPadCallCancelButton.Image")));
            this.myDialPadCallCancelButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.myDialPadCallCancelButton.Location = new System.Drawing.Point(155, 132);
            this.myDialPadCallCancelButton.Name = "myDialPadCallCancelButton";
            this.myDialPadCallCancelButton.Size = new System.Drawing.Size(47, 31);
            this.myDialPadCallCancelButton.TabIndex = 12;
            this.myDialPadCallCancelButton.Text = "End";
            this.myDialPadCallCancelButton.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.myDialPadCallCancelButton.UseVisualStyleBackColor = true;
            this.myDialPadCallCancelButton.Click += new System.EventHandler(this.myDialPadCallCancelButton_Click);
            // 
            // selectedContactComboBox
            // 
            this.selectedContactComboBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.selectedContactComboBox.BackColor = System.Drawing.SystemColors.Control;
            this.selectedContactComboBox.DisableMouseEvents = false;
            this.selectedContactComboBox.DropDownMaxSize = new System.Drawing.Size(0, 0);
            this.selectedContactComboBox.DropDownMinSize = new System.Drawing.Size(0, 0);
            this.selectedContactComboBox.DropDownSizingMode = ((Telerik.WinControls.UI.SizingMode)((Telerik.WinControls.UI.SizingMode.RightBottom | Telerik.WinControls.UI.SizingMode.UpDown)));
            this.selectedContactComboBox.Font = new System.Drawing.Font("Trebuchet MS", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.selectedContactComboBox.ImageList = null;
            this.selectedContactComboBox.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.tmpRadComboBoxItem});
            this.selectedContactComboBox.Location = new System.Drawing.Point(69, 38);
            this.selectedContactComboBox.MaxLength = 32767;
            this.selectedContactComboBox.Name = "selectedContactComboBox";
            this.selectedContactComboBox.NullText = "";
            // 
            // selectedContactComboBox.RootElement
            // 
            this.selectedContactComboBox.RootElement.AccessibleDescription = "";
            this.selectedContactComboBox.RootElement.AutoSizeMode = Telerik.WinControls.RadAutoSizeMode.WrapAroundChildren;
            this.selectedContactComboBox.RootElement.BackColor = System.Drawing.SystemColors.Control;
            this.selectedContactComboBox.RootElement.Font = new System.Drawing.Font("Trebuchet MS", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.selectedContactComboBox.RootElement.KeyTip = "";
            this.selectedContactComboBox.RootElement.ToolTipText = null;
            this.selectedContactComboBox.Size = new System.Drawing.Size(200, 22);
            this.selectedContactComboBox.SmallImageList = null;
            this.selectedContactComboBox.TabIndex = 13;
            this.selectedContactComboBox.TabStop = false;
            this.selectedContactComboBox.ThemeName = "Office2007Black";
            // 
            // tmpRadComboBoxItem
            // 
            this.tmpRadComboBoxItem.AccessibleDescription = "";
            this.tmpRadComboBoxItem.DescriptionText = "Home";
            this.tmpRadComboBoxItem.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tmpRadComboBoxItem.Image = ((System.Drawing.Image)(resources.GetObject("tmpRadComboBoxItem.Image")));
            this.tmpRadComboBoxItem.KeyTip = "";
            this.tmpRadComboBoxItem.Text = "+1234567890";
            this.tmpRadComboBoxItem.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.tmpRadComboBoxItem.TextSeparatorVisibility = Telerik.WinControls.ElementVisibility.Visible;
            this.tmpRadComboBoxItem.ToolTipText = null;
            // 
            // meContactComboBox
            // 
            this.meContactComboBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.meContactComboBox.BackColor = System.Drawing.SystemColors.Control;
            this.meContactComboBox.DisableMouseEvents = false;
            this.meContactComboBox.DropDownMaxSize = new System.Drawing.Size(0, 0);
            this.meContactComboBox.DropDownMinSize = new System.Drawing.Size(0, 0);
            this.meContactComboBox.DropDownSizingMode = ((Telerik.WinControls.UI.SizingMode)((Telerik.WinControls.UI.SizingMode.RightBottom | Telerik.WinControls.UI.SizingMode.UpDown)));
            this.meContactComboBox.Font = new System.Drawing.Font("Trebuchet MS", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.meContactComboBox.FormatString = null;
            this.meContactComboBox.ImageList = null;
            this.meContactComboBox.Location = new System.Drawing.Point(69, 89);
            this.meContactComboBox.MaxLength = 32767;
            this.meContactComboBox.Name = "meContactComboBox";
            this.meContactComboBox.NullText = "";
            // 
            // meContactComboBox.RootElement
            // 
            this.meContactComboBox.RootElement.AccessibleDescription = "";
            this.meContactComboBox.RootElement.AutoSizeMode = Telerik.WinControls.RadAutoSizeMode.WrapAroundChildren;
            this.meContactComboBox.RootElement.BackColor = System.Drawing.SystemColors.Control;
            this.meContactComboBox.RootElement.Font = new System.Drawing.Font("Trebuchet MS", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.meContactComboBox.RootElement.KeyTip = "";
            this.meContactComboBox.RootElement.ToolTipText = null;
            this.meContactComboBox.Size = new System.Drawing.Size(200, 22);
            this.meContactComboBox.SmallImageList = null;
            this.meContactComboBox.TabIndex = 14;
            this.meContactComboBox.TabStop = false;
            this.meContactComboBox.ThemeName = "Office2007Black";
            // 
            // radTitleBar1
            // 
            this.radTitleBar1.BackColor = System.Drawing.Color.Black;
            this.radTitleBar1.Caption = "nikotalk call";
            this.radTitleBar1.DisableMouseEvents = false;
            this.radTitleBar1.Dock = System.Windows.Forms.DockStyle.Top;
            this.radTitleBar1.ImageList = this.imageList2;
            this.radTitleBar1.Location = new System.Drawing.Point(2, 1);
            this.radTitleBar1.Name = "radTitleBar1";
            // 
            // radTitleBar1.RootElement
            // 
            this.radTitleBar1.RootElement.AccessibleDescription = "";
            this.radTitleBar1.RootElement.BackColor = System.Drawing.Color.Black;
            this.radTitleBar1.RootElement.KeyTip = "";
            this.radTitleBar1.RootElement.ToolTipText = null;
            ((Telerik.WinControls.UI.RadImageButtonElement)(this.radTitleBar1.RootElement.GetChildAt(0).GetChildAt(2).GetChildAt(2).GetChildAt(1))).Visibility = Telerik.WinControls.ElementVisibility.Hidden;
            this.radTitleBar1.Size = new System.Drawing.Size(288, 23);
            this.radTitleBar1.SmallImageList = null;
            this.radTitleBar1.TabIndex = 15;
            this.radTitleBar1.TabStop = false;
            this.radTitleBar1.Text = "radTitleBar1";
            this.radTitleBar1.ThemeName = "Office2007Black";
            // 
            // imageList2
            // 
            this.imageList2.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList2.ImageStream")));
            this.imageList2.TransparentColor = System.Drawing.Color.Magenta;
            this.imageList2.Images.SetKeyName(0, "minimize.png");
            this.imageList2.Images.SetKeyName(1, "maximize.png");
            this.imageList2.Images.SetKeyName(2, "close.png");
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.Control;
            this.panel1.Controls.Add(this.myDialPadCallCancelButton);
            this.panel1.Controls.Add(this.meContactComboBox);
            this.panel1.Controls.Add(this.meNameLabel);
            this.panel1.Controls.Add(this.selectedContactComboBox);
            this.panel1.Controls.Add(this.selectedCallLabel);
            this.panel1.Controls.Add(this.selectedNameLabel);
            this.panel1.Controls.Add(this.myDialPadCallOrAnswerButton);
            this.panel1.Controls.Add(this.meCallLabel);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(2, 69);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(288, 179);
            this.panel1.TabIndex = 16;
            // 
            // Phone2PhoneWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(292, 250);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.myTitleWhitePanel);
            this.Controls.Add(this.radTitleBar1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Phone2PhoneWindow";
            this.Padding = new System.Windows.Forms.Padding(2, 1, 2, 2);
            this.Text = "nikotalk call";
            this.myTitleWhitePanel.ResumeLayout(false);
            this.myTitleWhitePanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.selectedContactComboBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.meContactComboBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radTitleBar1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label meNameLabel;
        private System.Windows.Forms.Label selectedCallLabel;
        private System.Windows.Forms.Panel myTitleWhitePanel;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label selectedNameLabel;
        private System.Windows.Forms.Label meCallLabel;
        private System.Windows.Forms.Button myDialPadCallOrAnswerButton;
        private System.Windows.Forms.Button myDialPadCallCancelButton;
        private Telerik.WinControls.UI.RadComboBox selectedContactComboBox;
        private Telerik.WinControls.UI.RadComboBox meContactComboBox;
        private Telerik.WinControls.UI.RadComboBoxItem tmplRadComboBoxItem;
        private Telerik.WinControls.UI.RadComboBoxItem tmpRadComboBoxItem;
        private Telerik.WinControls.UI.RadTitleBar radTitleBar1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label myContactWindowDescriptionLabel;
        private System.Windows.Forms.ImageList imageList2;
    }
}