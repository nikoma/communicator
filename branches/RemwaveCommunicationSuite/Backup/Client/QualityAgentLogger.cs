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
            private XmlDocument doc;
            private string FileLocation;

            // Constructor
            public QualityAgentLogger()
            {
                FileLocation = Directory.GetCurrentDirectory() + @"\RCS-1.1-Beta-ExceptionLog-" + DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString("D2") + DateTime.Now.Day.ToString("D2") + ".xml";

                Disposed = false;

                doc = new XmlDocument();
                FileInfo fiMXF = new FileInfo(FileLocation);
                if (fiMXF.Exists)
                {
                    doc.Load(FileLocation);
                }
                else
                {
                    XmlDeclaration xmldecl;
                    xmldecl = doc.CreateXmlDeclaration("1.0", null, null);
                    XmlElement oRoot = doc.CreateElement("error");
                    doc.AppendChild(xmldecl);
                    doc.AppendChild(oRoot);
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
                if (!Disposed)
                {
                    if (Disposing)
                    {
                    }

                    // Moved from Close to Dispose Method
                    doc.Save(FileLocation);

                    // Close file
                    doc = null;

                    // Disposed
                    Disposed = true;

                    // Parent disposing
                    base.Dispose(Disposing);
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
                catch (Exception)
                {
                    return;
                }

                XmlNode oRoot = doc.DocumentElement;//LastChild.NextSibling;

                XmlElement Event = doc.CreateElement("Event");

                XmlElement TimeElement = doc.CreateElement("Time");
                XmlElement StackTraceElement = doc.CreateElement("StackTrace");
                XmlElement ExceptionElement = doc.CreateElement("Exception");
                XmlElement MessageElement = doc.CreateElement("Message");


                XmlText TimeText = doc.CreateTextNode(DateTime.Now.ToString());
                XmlText StackTraceText = doc.CreateTextNode(Ex.StackTrace);
                XmlText ExceptionText = doc.CreateTextNode(Ex.ToString());
                XmlText MessageText = doc.CreateTextNode(Ex.Message);


                TimeElement.AppendChild(TimeText);
                Event.AppendChild(TimeElement);

                StackTraceElement.AppendChild(StackTraceText);
                Event.AppendChild(StackTraceElement);

                ExceptionElement.AppendChild(ExceptionText);
                Event.AppendChild(ExceptionElement);

                MessageElement.AppendChild(MessageText);
                Event.AppendChild(MessageElement);
                oRoot.AppendChild(Event);



                if (MessageBox.Show("An application error has occurred and an application error log is being generated.\nClick [ OK ] to start Quality Agent and send an error report to REMWAVE.", "REMWAVE -  Quality Agent", MessageBoxButtons.OKCancel, MessageBoxIcon.Error) == DialogResult.OK)
                {
                    doc.Save(FileLocation);
                    System.Diagnostics.Process.Start(Directory.GetCurrentDirectory() + @"\REMWAVEQA.exe", "\"" + FileLocation + "\"");

                }
            }
        }
    }

