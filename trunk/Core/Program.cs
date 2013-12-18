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
        static void Main(string[] args)
        {
            Application.ApplicationExit += new EventHandler(Application_ApplicationExit);

            Boolean mMinimized = false;

            if (args != null)
            {
                foreach (string arg in args)
                {
                    if (arg == "/tray") mMinimized = true;
                }
            }

            string applicationName = Application.ProductName;
            string executablePath = Application.ExecutablePath;
            int[] portsToOpen = { };
			//hnetcfg.dll 
            Firewall.OpenFirewallPorts(executablePath, applicationName, portsToOpen);

            QualityAgentLogger Logger = new QualityAgentLogger(Application.StartupPath + @"\QualityAgent.exe");
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
                    Console.WriteLine("Mutex:Started Remwave-Client-Mutex to ensure only one instance run concurrently.");
                    Application.EnableVisualStyles();
                    AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
                    Application.ThreadException += new ThreadExceptionEventHandler(Application_ThreadException);
                    Application.SetCompatibleTextRenderingDefault(false);
                    
                   
                    if (!mMinimized)
                    {
                        //Splash screen
                        Thread thread = new Thread(new ThreadStart(Program.DoSplash));
                        thread.Priority = ThreadPriority.Normal;
                        thread.Start();
                    }
                    try
                    {
                       
                        ClientForm MainForm = new ClientForm();
                        MainForm.SetQualityAgentLogger(Logger);
                        if (mMinimized)
                        {
                            MainForm.ShowMinimized();
                        }
                        else
                        {
                            MainForm.Show();
                        }
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

        static void Application_ApplicationExit(object sender, EventArgs e)
        {
            Console.WriteLine("###Application_ApplicationExit###");
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

#if (DEBUG)
            Console.Error.WriteLine(ex);
#endif
            ExitApplication();
        }

        static void ExitApplication()
        {
            Application.Exit();
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