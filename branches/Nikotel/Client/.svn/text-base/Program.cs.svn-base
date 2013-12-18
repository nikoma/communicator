using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Threading;

namespace Remwave.Client
{
    static class Program
    {
		static volatile bool msShowSplash = true;
		
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

            QualityAgentLogger Logger = new QualityAgentLogger(Application.StartupPath+@"\QualityAgent.exe");
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
                    
					//Splash screen
                    Thread thread = new Thread(new ThreadStart(Program.DoSplash));
                    thread.Priority = ThreadPriority.Normal;
                    thread.Start();
                    try
                    {
                        Form MainForm = new ClientForm();
                        MainForm.Show();
                        msShowSplash = false;
                        Application.Run(MainForm);
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
                    Thread.Sleep(1000);
                }
            }
        }

        static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            msShowSplash = false;
            HandleException(e.Exception as Exception);
        }

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            msShowSplash = false;
            Thread.Sleep(300);
            HandleException(e.ExceptionObject as Exception);
        }

        private static void HandleException(Exception ex)
        {
            if (ex == null)
                return;

#if (DEBUG) 
            throw(ex);
#else
            Console.Error.WriteLine(ex);
            Application.Exit();
#endif
        }
		
		static void DoSplash()
        {
            Splash msSplash = new Splash();
            //Show splash screen
            msSplash.Show();

            //Show splash for a few more secs
           while (msShowSplash == true)
            {
                msSplash.Tick();
                Thread.Sleep(250);
            }
            
            msSplash.Close();
            msSplash.Dispose();
            msSplash = null;
        }
    }
}