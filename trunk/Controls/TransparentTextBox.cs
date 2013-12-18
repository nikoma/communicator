using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Remwave.Client.Controls
{
    public partial class TransparentTextBox :TextBox
    {
        private string DefaultText = "Tag it, click here.";
        public TransparentTextBox()
        {
            InitializeComponent();
            //Force transparency
            base.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            //base.BackColor = Color.Transparent;
            
        }
           
    

        

        private void TransparentTextBox_Enter(object sender, EventArgs e)
        {
           
            if (this.Text == this.DefaultText) this.Text = "";
        }

        private void TransparentTextBox_Leave(object sender, EventArgs e)
        {
           
            if (this.Text == null || this.Text.Trim().Length == 0) { this.Text = this.DefaultText; }
        }

        private void TransparentTextBox_TextChanged(object sender, EventArgs e)
        {
            if (!this.Focused)
            {
                if (this.Text == null || this.Text.Trim().Length == 0) { this.Text = this.DefaultText; }
            }
        }

    }
}
