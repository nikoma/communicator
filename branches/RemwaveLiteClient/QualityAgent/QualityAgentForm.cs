using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.IO;

namespace QualityAgent
{
    public partial class QualityAgentForm : Form
    {
        string[] Args;

        public QualityAgentForm(string [] args)
        {
            this.Args = args;
            InitializeComponent();
        }

        private void LoadFile(string FileLocation)
        {
            bool fileOpened = false;
            try
            {
                XmlDocument doc = new XmlDocument();
                FileInfo fiMXF = new FileInfo(FileLocation);
                if (fiMXF.Exists)
                {
                    doc.Load(FileLocation);
                    ExceptionPreviewTextBox.Text = doc.InnerXml;
                    fileOpened = true;
                }
            }
            catch (Exception)
            {
                
            }

            if (!fileOpened)
            {
                MessageBox.Show("Report file could not be opened:\n" + FileLocation + "\n", "REMWAVE Quality Agent", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }


        private void QualityAgentForm_Load(object sender, EventArgs e)
        {

            if (Args.Length > 0)
            {
                LoadFile(Args[0]);
            }
            
        }

        private void OpenLogStripButton_Click(object sender, EventArgs e)
        {
            if (this.OpenLogFileDialog.ShowDialog() == DialogResult.OK)
            {
                LoadFile(this.OpenLogFileDialog.FileName);

            }
        }
        private void SendLogFileStripButton_Click(object sender, EventArgs e)
        {
            bool reportDelivered = false;
            try
            {
                QualityAgentWebService.Service QAService = new QualityAgent.QualityAgentWebService.Service();
                reportDelivered = QAService.ReceiveLogFile(ExceptionPreviewTextBox.Text);
            }
            catch (Exception)
            {
                
            }

            if (reportDelivered)
            {
                MessageBox.Show("Report has been successfully sent.", "Quality Agent Report Delivery Status", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Application.Exit();
            }
            else
            {
                MessageBox.Show("Report delivery failed!", "Quality Agent Report Delivery Status", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ExceptionPreviewTextBox_TextChanged(object sender, EventArgs e)
        {
            ExceptionPreviewTextBox.DeselectAll();
        }
    }
}