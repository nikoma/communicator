using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls.UI;

namespace Remwave.Client
{
    public partial class ConfigurationWindow : ShapedForm
    {
        private ClientForm myClientForm;

        public ConfigurationWindow(ClientForm clientForm)
        {
            myClientForm = clientForm;

            InitializeComponent();

            tbSipProxyAddress.Text = myClientForm.myClientConfiguration.SIPProxyAddress;
            tbSipRealm.Text = myClientForm.myClientConfiguration.SIPProxyRealm;
            tbIMServerAddress.Text = myClientForm.myClientConfiguration.IMServerAddress;
            tbVideoProxyAddress.Text = myClientForm.myClientConfiguration.VideoProxyAddress;
            tbWebServiceUrl.Text = myClientForm.myClientConfiguration.RSIUrl;
        }

        private void btnSaveChanges_Click(object sender, EventArgs e)
        {

           myClientForm.myClientConfiguration.SIPProxyAddress =  tbSipProxyAddress.Text;
           myClientForm.myClientConfiguration.SIPProxyRealm =  tbSipRealm.Text;
           myClientForm.myClientConfiguration.IMServerAddress =  tbIMServerAddress.Text;
           myClientForm.myClientConfiguration.VideoProxyAddress = tbVideoProxyAddress.Text;
           myClientForm.myClientConfiguration.RSIUrl = tbWebServiceUrl.Text;
           this.DialogResult = DialogResult.OK;

        }
    }
}