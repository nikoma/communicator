using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Threading;

namespace Remwave.Client
{
    static class Program
    {

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            string applicationName = Application.ProductName;
            string executablePath = Application.ExecutablePath;
            int[] portsToOpen = { };
            //hnetcfg.dll 
            Firewall.OpenFirewallPorts(executablePath, applicationName, portsToOpen);

            QualityAgentLogger Logger = new QualityAgentLogger();
            Console.SetError(Logger);

            //Allow to run only one instance of application

            // A boolean that indicates whether this application has
            // initial ownership of the Mutex.
            bool ownsMutex;

            // Attempt to create and take ownership of a Mutex named
            // MutexExample.
            using (Mutex mutex =
                       new Mutex(true, "Remwave-Client-Mutex", out ownsMutex))
            {
                // If the application owns the Mutex it can continue to execute;
                // otherwise, the application should exit. 
                if (ownsMutex)
                {
                    Console.WriteLine("Mutex:Started Client-Mutex to ensure only one instance run concurrently.");
                    Application.EnableVisualStyles();
                    AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
                    Application.ThreadException += new ThreadExceptionEventHandler(Application_ThreadException);
                    Application.SetCompatibleTextRenderingDefault(false);
                    try
                    {
                        Application.Run(new ClientForm());
                    }
                    catch (Exception ex)
                    {
                        HandleException(ex);
                      
                    } 

                 
                    // Release the mutex
                    mutex.ReleaseMutex();
                }
                else
                {
                    Console.WriteLine("Mutex:Another instance is already running. This instance of the application will terminate.");
                    Thread.Sleep(3000);
                }
            }


        }

        static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            HandleException(e.Exception as Exception);
        }

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            HandleException(e.ExceptionObject as Exception);
        }

        private static void HandleException(Exception ex)
        {
            if (ex == null)
                return;
            Console.Error.WriteLine(ex);
            Application.Exit();
        }
    }
}