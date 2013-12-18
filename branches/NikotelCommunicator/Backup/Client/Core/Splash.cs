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
            BrandComponent();
            lblVersion.Text = "Version : " + Application.ProductVersion;

            loadingProgressBar.Visible = true;
            loadingProgressBar.Enabled = true;
            loadingProgressBar.Value1 = 0;
            loadingProgressBar.Value2 = 0;
        }
        private void BrandComponent()
        {
#if BRAND_OURFREEPHONE
            lblVersion.BackColor = Color.Black;
#elif BRAND_NIKOTEL
            lblVersion.BackColor = Color.Transparent;
#endif
            this.Icon = Properties.Resources.desktop;
        }
        private void Splash_Load(object sender, EventArgs e)
        {
            
        }


        public void Tick()
        {
            if (loadingProgressBar.Value2 + loadingProgressBar.Step > loadingProgressBar.Maximum) loadingProgressBar.Value2 = 0;
            loadingProgressBar.Value2 += loadingProgressBar.Step;
            loadingProgressBar.Value1 = loadingProgressBar.Value2 - 10;
            lblVersion.Invalidate();
            lblVersion.Update();
            loadingProgressBar.Invalidate();
            loadingProgressBar.Update();
            
        }

        

        


       
    }
}