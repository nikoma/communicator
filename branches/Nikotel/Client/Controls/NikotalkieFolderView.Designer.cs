namespace Remwave.Client.Controls
{
    partial class NikotalkieFolderView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NikotalkieFolderView));
            this.VerticalScrollBar = new Telerik.WinControls.UI.RadVScrollBar();
            this.Items = new System.Windows.Forms.Panel();
            this.layoutActionButtons = new System.Windows.Forms.TableLayoutPanel();
            this.buttonReply = new System.Windows.Forms.Button();
            this.buttonPlay = new System.Windows.Forms.Button();
            this.buttonDelete = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.VerticalScrollBar)).BeginInit();
            this.layoutActionButtons.SuspendLayout();
            this.SuspendLayout();
            // 
            // VerticalScrollBar
            // 
            this.VerticalScrollBar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.VerticalScrollBar.DisableMouseEvents = false;
            this.VerticalScrollBar.Dock = System.Windows.Forms.DockStyle.Right;
            this.VerticalScrollBar.ImageList = null;
            this.VerticalScrollBar.Location = new System.Drawing.Point(298, 0);
            this.VerticalScrollBar.Margin = new System.Windows.Forms.Padding(0);
            this.VerticalScrollBar.Name = "VerticalScrollBar";
            // 
            // VerticalScrollBar.RootElement
            // 
            this.VerticalScrollBar.RootElement.AccessibleDescription = "";
            this.VerticalScrollBar.RootElement.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.VerticalScrollBar.RootElement.KeyTip = "";
            this.VerticalScrollBar.RootElement.ToolTipText = null;
            this.VerticalScrollBar.Size = new System.Drawing.Size(20, 198);
            this.VerticalScrollBar.SmallImageList = null;
            this.VerticalScrollBar.TabIndex = 2;
            this.VerticalScrollBar.ThemeName = "Office2007Black";
            this.VerticalScrollBar.Scroll += new System.Windows.Forms.ScrollEventHandler(this.VerticalScrollBar_Scroll);
            // 
            // Items
            // 
            this.Items.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Items.Location = new System.Drawing.Point(2, 0);
            this.Items.Name = "Items";
            this.Items.Size = new System.Drawing.Size(296, 198);
            this.Items.TabIndex = 3;
            this.Items.Resize += new System.EventHandler(this.Items_Resize);
            // 
            // layoutActionButtons
            // 
            this.layoutActionButtons.ColumnCount = 3;
            this.layoutActionButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.layoutActionButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.layoutActionButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.layoutActionButtons.Controls.Add(this.buttonReply, 0, 0);
            this.layoutActionButtons.Controls.Add(this.buttonPlay, 0, 0);
            this.layoutActionButtons.Controls.Add(this.buttonDelete, 0, 0);
            this.layoutActionButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.layoutActionButtons.Location = new System.Drawing.Point(2, 198);
            this.layoutActionButtons.Name = "layoutActionButtons";
            this.layoutActionButtons.RowCount = 1;
            this.layoutActionButtons.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 42F));
            this.layoutActionButtons.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.layoutActionButtons.Size = new System.Drawing.Size(316, 42);
            this.layoutActionButtons.TabIndex = 4;
            // 
            // buttonReply
            // 
            this.buttonReply.FlatAppearance.BorderSize = 0;
            this.buttonReply.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.buttonReply.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.buttonReply.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonReply.Image = ((System.Drawing.Image)(resources.GetObject("buttonReply.Image")));
            this.buttonReply.Location = new System.Drawing.Point(200, 0);
            this.buttonReply.Margin = new System.Windows.Forms.Padding(0);
            this.buttonReply.Name = "buttonReply";
            this.buttonReply.Size = new System.Drawing.Size(120, 42);
            this.buttonReply.TabIndex = 8;
            this.buttonReply.UseVisualStyleBackColor = true;
            this.buttonReply.MouseLeave += new System.EventHandler(this.buttonReply_MouseLeave);
            this.buttonReply.Click += new System.EventHandler(this.buttonReply_Click);
            this.buttonReply.MouseDown += new System.Windows.Forms.MouseEventHandler(this.buttonReply_MouseDown);
            this.buttonReply.MouseEnter += new System.EventHandler(this.buttonReply_MouseEnter);
            this.buttonReply.MouseUp += new System.Windows.Forms.MouseEventHandler(this.buttonReply_MouseUp);
            // 
            // buttonPlay
            // 
            this.buttonPlay.FlatAppearance.BorderSize = 0;
            this.buttonPlay.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.buttonPlay.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.buttonPlay.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonPlay.Image = global::Remwave.Client.Properties.Resources.NikotalkieButtonPlay;
            this.buttonPlay.Location = new System.Drawing.Point(100, 0);
            this.buttonPlay.Margin = new System.Windows.Forms.Padding(0);
            this.buttonPlay.Name = "buttonPlay";
            this.buttonPlay.Size = new System.Drawing.Size(100, 42);
            this.buttonPlay.TabIndex = 7;
            this.buttonPlay.UseVisualStyleBackColor = true;
            this.buttonPlay.MouseLeave += new System.EventHandler(this.buttonPlay_MouseLeave);
            this.buttonPlay.Click += new System.EventHandler(this.buttonPlay_Click);
            this.buttonPlay.MouseDown += new System.Windows.Forms.MouseEventHandler(this.buttonPlay_MouseDown);
            this.buttonPlay.MouseEnter += new System.EventHandler(this.buttonPlay_MouseEnter);
            this.buttonPlay.MouseUp += new System.Windows.Forms.MouseEventHandler(this.buttonPlay_MouseUp);
            // 
            // buttonDelete
            // 
            this.buttonDelete.FlatAppearance.BorderSize = 0;
            this.buttonDelete.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.buttonDelete.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.buttonDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonDelete.Image = ((System.Drawing.Image)(resources.GetObject("buttonDelete.Image")));
            this.buttonDelete.Location = new System.Drawing.Point(0, 0);
            this.buttonDelete.Margin = new System.Windows.Forms.Padding(0);
            this.buttonDelete.Name = "buttonDelete";
            this.buttonDelete.Size = new System.Drawing.Size(100, 42);
            this.buttonDelete.TabIndex = 6;
            this.buttonDelete.UseVisualStyleBackColor = true;
            this.buttonDelete.MouseLeave += new System.EventHandler(this.buttonDelete_MouseLeave);
            this.buttonDelete.Click += new System.EventHandler(this.buttonDelete_Click);
            this.buttonDelete.MouseDown += new System.Windows.Forms.MouseEventHandler(this.buttonDelete_MouseDown);
            this.buttonDelete.MouseEnter += new System.EventHandler(this.buttonDelete_MouseEnter);
            this.buttonDelete.MouseUp += new System.Windows.Forms.MouseEventHandler(this.buttonDelete_MouseUp);
            // 
            // NikotalkieFolderView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.Controls.Add(this.Items);
            this.Controls.Add(this.VerticalScrollBar);
            this.Controls.Add(this.layoutActionButtons);
            this.DoubleBuffered = true;
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "NikotalkieFolderView";
            this.Padding = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.Size = new System.Drawing.Size(320, 240);
            ((System.ComponentModel.ISupportInitialize)(this.VerticalScrollBar)).EndInit();
            this.layoutActionButtons.ResumeLayout(false);
            this.ResumeLayout(false);

        }

       

        #endregion

        private Telerik.WinControls.UI.RadVScrollBar VerticalScrollBar;
        public System.Windows.Forms.Panel Items;
        private System.Windows.Forms.TableLayoutPanel layoutActionButtons;
        private System.Windows.Forms.Button buttonPlay;
        private System.Windows.Forms.Button buttonDelete;
        private System.Windows.Forms.Button buttonReply;
    }
}
