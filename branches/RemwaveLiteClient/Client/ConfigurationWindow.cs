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

            try
            {
                cbServiceProviders.SelectedIndex = 0;

                RemwaveLiteWS.Service service = new Remwave.Client.RemwaveLiteWS.Service();
                RemwaveLiteWS.ServiceProvider[] serviceProviders = service.ServiceProviders(Application.ProductVersion);

                foreach (RemwaveLiteWS.ServiceProvider serviceProvider in serviceProviders)
                {
                    RadComboBoxItem cbItem = new RadComboBoxItem();

                    cbItem.AccessibleDescription = "";
                    cbItem.DescriptionFont = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                    cbItem.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                    cbItem.DescriptionText = serviceProvider.Description;
                    cbItem.KeyTip = "";
                    cbItem.Text = serviceProvider.Name;
                    cbItem.TextSeparatorVisibility = Telerik.WinControls.ElementVisibility.Visible;
                    cbItem.ToolTipText = null;
                    cbItem.Tag = serviceProvider;

                    cbServiceProviders.Items.Add(cbItem);
                }


            }
            catch (Exception)
            {

                throw;
            }

        }

        private void btnSaveChanges_Click(object sender, EventArgs e)
        {
            string[] separator = { ":" };
            //SIP Proxy Realm
            myClientForm.myClientConfiguration.SIPProxyRealm = tbSipRealm.Text;
            //SIP Proxy Address and Port
            if (tbSipProxyAddress.Text.Contains(separator[0]))
            {
                string[] resultSipProxy =  { "localhost", "" };
                resultSipProxy = tbSipProxyAddress.Text.Split(separator, 2, StringSplitOptions.RemoveEmptyEntries);
                int resultSipProxyPort;
                myClientForm.myClientConfiguration.SIPProxyAddress = resultSipProxy[0];
                myClientForm.myClientConfiguration.SIPProxyPort = Int32.TryParse(resultSipProxy[1], out resultSipProxyPort) ? resultSipProxyPort : 5060;
            }
            else
            {
                myClientForm.myClientConfiguration.SIPProxyAddress = tbSipProxyAddress.Text;
                myClientForm.myClientConfiguration.SIPProxyPort = 5060;
            }

            //IM Jabber Server Address and Port
            if (tbIMServerAddress.Text.Contains(separator[0]))
            {
                string[] resultIMServer =  { "localhost", "" };
                resultIMServer = tbIMServerAddress.Text.Split(separator, 2, StringSplitOptions.RemoveEmptyEntries);
                int resultIMServerPort;
                myClientForm.myClientConfiguration.IMServerAddress = resultIMServer[0];
                myClientForm.myClientConfiguration.IMServerPort = Int32.TryParse(resultIMServer[1], out resultIMServerPort) ? resultIMServerPort : 5222;
            }
            else
            {
                myClientForm.myClientConfiguration.IMServerAddress = tbIMServerAddress.Text;
                myClientForm.myClientConfiguration.IMServerPort = 5222;
            }


            //Video Proxy Server Address / use default port
            if (tbVideoProxyAddress.Text.Contains(":"))
            {
                string[] resultVideoProxy =  { "localhost", "" };
                resultVideoProxy = tbVideoProxyAddress.Text.Split(separator, 2, StringSplitOptions.RemoveEmptyEntries);
                int resultVideoProxyPort;
                myClientForm.myClientConfiguration.VideoProxyAddress = resultVideoProxy[0];
                myClientForm.myClientConfiguration.VideoProxyPort = Int32.TryParse(resultVideoProxy[1], out resultVideoProxyPort) ? resultVideoProxyPort : 800;
            }
            else
            {
                myClientForm.myClientConfiguration.VideoProxyAddress = tbVideoProxyAddress.Text;
            }

            this.DialogResult = DialogResult.OK;

        }

        private void cbServiceProviders_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (((RadComboBoxItem)cbServiceProviders.SelectedItem).Tag != null)
            {
                lnChangeSettings.Visible = true;

                try
                {
                    RemwaveLiteWS.ServiceProvider serviceProvider = new Remwave.Client.RemwaveLiteWS.ServiceProvider();
                    serviceProvider = ((RemwaveLiteWS.ServiceProvider)((RadComboBoxItem)cbServiceProviders.SelectedItem).Tag);
                    tbSipProxyAddress.Text = serviceProvider.SIPProxyAddress + ":" + serviceProvider.SIPProxyPort.ToString();
                    tbSipRealm.Text = serviceProvider.SIPProxyRealm;
                    tbIMServerAddress.Text = serviceProvider.IMServerAddress + ":" + serviceProvider.IMServerPort.ToString();
                    tbVideoProxyAddress.Text = serviceProvider.VideoProxyAddress + ":" + serviceProvider.VideoProxyPort.ToString();
                }
                catch (Exception)
                {
                 //   throw;
                }
            }
            else
            {
                lnChangeSettings.Visible = false;

                tbSipProxyAddress.Text = myClientForm.myClientConfiguration.SIPProxyAddress;
                tbSipRealm.Text = myClientForm.myClientConfiguration.SIPProxyRealm;
                tbIMServerAddress.Text = myClientForm.myClientConfiguration.IMServerAddress;
                tbVideoProxyAddress.Text = myClientForm.myClientConfiguration.VideoProxyAddress;
            }

        }

        private void lnChangeSettings_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (((RadComboBoxItem)cbServiceProviders.SelectedItem).Tag != null)
            {
                try
                {
                    RemwaveLiteWS.ServiceProvider serviceProvider = new Remwave.Client.RemwaveLiteWS.ServiceProvider();
                    serviceProvider = ((RemwaveLiteWS.ServiceProvider)((RadComboBoxItem)cbServiceProviders.SelectedItem).Tag);

                    System.Diagnostics.Process.Start(serviceProvider.SignupLink);
                }
                catch (Exception)
                {
                    // throw;
                }
            }
        }
    }
}