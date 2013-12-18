using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace QualityAgent
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] Args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new QualityAgentForm(Args));
        }
    }
}