namespace Remwave.Client.Controls
{
    partial class NikotalkieComposeMessageView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NikotalkieComposeMessageView));
            this.buttonTalk = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label2 = new System.Windows.Forms.Label();
            this.tbDestination = new System.Windows.Forms.TextBox();
            this.liDestinationHistory = new System.Windows.Forms.ListBox();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonTalk
            // 
            this.buttonTalk.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.buttonTalk.FlatAppearance.BorderSize = 0;
            this.buttonTalk.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.buttonTalk.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.buttonTalk.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonTalk.Image = ((System.Drawing.Image)(resources.GetObject("buttonTalk.Image")));
            this.buttonTalk.Location = new System.Drawing.Point(0, 198);
            this.buttonTalk.Margin = new System.Windows.Forms.Padding(0);
            this.buttonTalk.Name = "buttonTalk";
            this.buttonTalk.Size = new System.Drawing.Size(320, 42);
            this.buttonTalk.TabIndex = 0;
            this.buttonTalk.UseVisualStyleBackColor = true;
            this.buttonTalk.MouseLeave += new System.EventHandler(this.buttonTalk_MouseLeave);
            this.buttonTalk.Click += new System.EventHandler(this.buttonTalk_Click);
            this.buttonTalk.MouseDown += new System.Windows.Forms.MouseEventHandler(this.buttonTalk_MouseDown);
            this.buttonTalk.MouseEnter += new System.EventHandler(this.buttonTalk_MouseEnter);
            this.buttonTalk.MouseUp += new System.Windows.Forms.MouseEventHandler(this.buttonTalk_MouseUp);
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(8, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(76, 16);
            this.label1.TabIndex = 1;
            this.label1.Text = "Destination:";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tbDestination, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.liDestinationHistory, 0, 3);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.Padding = new System.Windows.Forms.Padding(5);
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(320, 198);
            this.tableLayoutPanel1.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(8, 57);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(119, 16);
            this.label2.TabIndex = 3;
            this.label2.Text = "Destination History:";
            // 
            // tbDestination
            // 
            this.tbDestination.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbDestination.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbDestination.Location = new System.Drawing.Point(8, 32);
            this.tbDestination.Name = "tbDestination";
            this.tbDestination.Size = new System.Drawing.Size(304, 23);
            this.tbDestination.TabIndex = 2;
            // 
            // liDestinationHistory
            // 
            this.liDestinationHistory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.liDestinationHistory.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.liDestinationHistory.FormattingEnabled = true;
            this.liDestinationHistory.ItemHeight = 16;
            this.liDestinationHistory.Location = new System.Drawing.Point(8, 80);
            this.liDestinationHistory.Name = "liDestinationHistory";
            this.liDestinationHistory.Size = new System.Drawing.Size(304, 100);
            this.liDestinationHistory.TabIndex = 4;
            // 
            // NikotalkieComposeMessageView
            // 
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.buttonTalk);
            this.Name = "NikotalkieComposeMessageView";
            this.Size = new System.Drawing.Size(320, 240);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbDestination;
        private System.Windows.Forms.ListBox liDestinationHistory;
        public System.Windows.Forms.Button buttonTalk;
    }
}
