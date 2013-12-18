using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Remwave.Client
{
    public partial class Splash : Form
    {
        public Splash()
        {
            InitializeComponent();

            lblVersion.Text = "Version : " + Application.ProductVersion;

            loadingProgressBar.Visible = true;
            loadingProgressBar.Enabled = true;
            loadingProgressBar.Value1 = 0;
            loadingProgressBar.Value2 = 0;
        }

        private void Splash_Load(object sender, EventArgs e)
        {
            
        }


        public void Tick()
        {
            if (loadingProgressBar.Value2 + loadingProgressBar.Step > loadingProgressBar.Maximum) loadingProgressBar.Value2 = 0;
            loadingProgressBar.Value2 += loadingProgressBar.Step;
            loadingProgressBar.Value1 = loadingProgressBar.Value2 - 10;
            loadingProgressBar.SeparatorColor1 = System.Drawing.Color.Gray;
            loadingProgressBar.SeparatorColor2 = System.Drawing.Color.White;
            loadingProgressBar.SeparatorWidth = 8;
            loadingProgressBar.ShowProgressIndicators = false;
            loadingProgressBar.Size = new System.Drawing.Size(318, 5);
            loadingProgressBar.StepWidth = 10;
            loadingProgressBar.SweepAngle = 135;

            lblVersion.Invalidate();
            lblVersion.Update();
            loadingProgressBar.Invalidate();
            loadingProgressBar.Update();
            
        }

        


       
    }
}
