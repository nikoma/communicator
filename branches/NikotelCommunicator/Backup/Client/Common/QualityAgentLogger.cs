using System;
using System.IO;
using System.Xml;
using System.Text;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace Remwave.Client
{



    /// <summary>
    /// ErrorLogWriter class
    /// </summary>
    public class QualityAgentLogger : TextWriter
    {
        // Variables
        private bool Disposed;
        private XmlDocument mXMLDocument;
        private readonly string mLogPath = Path.GetTempPath();
        private string mLogFile;
        private string mQualityAgentLocation;

        public Boolean Enabled = true;

        // Constructor
        public QualityAgentLogger(String qualityAgentLocation)
        {
            try
            {
                this.mQualityAgentLocation = qualityAgentLocation;
                mLogFile = mLogPath + Application.ProductName + "-ExceptionLog-" + DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString("D2") + DateTime.Now.Day.ToString("D2") + ".xml";
                Disposed = false;
                mXMLDocument = new XmlDocument();
                FileInfo fiMXF = new FileInfo(mLogFile);
                if (fiMXF.Exists)
                {
                    mXMLDocument.Load(mLogFile);
                }
                else
                {
                    XmlDeclaration xmldecl;
                    xmldecl = mXMLDocument.CreateXmlDeclaration("1.0", null, null);
                    XmlElement oRoot = mXMLDocument.CreateElement("error");
                    mXMLDocument.AppendChild(xmldecl);
                    mXMLDocument.AppendChild(oRoot);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("QualityAgentLogger : " + ex.Message);
            }
        }

        // Destructor (equivalent to Finalize() without the need to call base.Finalize())
        ~QualityAgentLogger()
        {
            Dispose(false);
        }


        // Free resources immediately
        protected override void Dispose(bool Disposing)
        {
            try
            {
                if (!Disposed)
                {
                    if (Disposing)
                    {
                    }
                    // Moved from Close to Dispose Method
                    mXMLDocument.Save(mLogFile);

                    // Close file
                    mXMLDocument = null;

                    // Disposed
                    Disposed = true;

                    // Parent disposing
                    base.Dispose(Disposing);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("QualityAgentLogger.Dispose : " + ex.Message);
            }
        }

        // Close the file
        public override void Close()
        {
            // Free resources
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // Implement Encoding() method from TextWriter
        public override Encoding Encoding
        {
            get
            {
                return (Encoding.Unicode);
            }
        }

        // Implement WriteLine() method from TextWriter (remove MethodImpl attribute for single-threaded app)
        // Use stack trace and reflection to get the calling class and method
        [MethodImpl(MethodImplOptions.Synchronized)]
        public override void WriteLine(object obj)
        {
            Exception Ex;

            try
            {
                Ex = obj as Exception;
            }
            catch
            {
                return;
            }
            try
            {

                XmlNode oRoot = mXMLDocument.DocumentElement;//LastChild.NextSibling;

                XmlElement Event = mXMLDocument.CreateElement("Event");
                Event.SetAttribute("ProductName", Application.ProductName);
                Event.SetAttribute("ProductVersion", Application.ProductVersion);
                Event.SetAttribute("OSVersion", Environment.OSVersion.VersionString);
                XmlElement TimeElement = mXMLDocument.CreateElement("Time");
                XmlElement SourceElement = mXMLDocument.CreateElement("Source");
                XmlElement StackTraceElement = mXMLDocument.CreateElement("StackTrace");
                XmlElement ExceptionElement = mXMLDocument.CreateElement("Exception");
                XmlElement MessageElement = mXMLDocument.CreateElement("Message");

                XmlText TimeText = mXMLDocument.CreateTextNode(DateTime.Now.ToString());
                XmlText SourceText = mXMLDocument.CreateTextNode(Ex.Source);
                XmlText StackTraceText = mXMLDocument.CreateTextNode(Ex.StackTrace);
                XmlText ExceptionText = mXMLDocument.CreateTextNode(Ex.ToString());
                XmlText MessageText = mXMLDocument.CreateTextNode(Ex.Message);
                
                SourceElement.AppendChild(SourceText);
                Event.AppendChild(SourceElement);

                TimeElement.AppendChild(TimeText);
                Event.AppendChild(TimeElement);

                StackTraceElement.AppendChild(StackTraceText);
                Event.AppendChild(StackTraceElement);

                ExceptionElement.AppendChild(ExceptionText);
                Event.AppendChild(ExceptionElement);

                MessageElement.AppendChild(MessageText);
                Event.AppendChild(MessageElement);
                oRoot.AppendChild(Event);



                if (this.Enabled)
                {
                    if (MessageBox.Show("An application error has occurred and an application error log is being generated.\nClick [ OK ] to start Quality Agent and send an error report.", "Quality Agent Logger", MessageBoxButtons.OKCancel, MessageBoxIcon.Error) == DialogResult.OK)
                    {                      
                        try
                        {
                            mXMLDocument.Save(mLogFile);
                            System.Diagnostics.Process.Start(mQualityAgentLocation, "\"" + mLogFile + "\"");
                        }
                        catch (Exception)
                        {

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("QualityAgentLogger.WriteLine : " + ex.Message);
            }
        }

    }
}

