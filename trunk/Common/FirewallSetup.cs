using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using NetFwTypeLib;
using Microsoft.Win32;

namespace Remwave.Client
{

    public class Firewall
    {
        protected INetFwProfile fwProfile;

        #region openFirewall
        ///
        /// Opens the given ports on windows firewall. Also opens entirely the given application on the firewall.
        /// Please remember that you need administrative rights to use this function.
        ///
        /// The path of the application. Please include the ending \
        /// The name of the executable to open
        /// The ports that you wish to open individually
        public void openFirewall(string executableFilePath, string applicationName, int[] portsToOpen)
        {
            ///////////// Firewall Authorize Application ////////////
            try
            {


                setProfile();
                INetFwAuthorizedApplications apps = fwProfile.AuthorizedApplications;
                INetFwAuthorizedApplication app = (INetFwAuthorizedApplication)getInstance("INetAuthApp");
                app.Name = applicationName;
                app.ProcessImageFileName = executableFilePath;
                apps.Add(app);
                apps = null;

                //////////////// Open Needed Ports /////////////////
                INetFwOpenPorts openports = fwProfile.GloballyOpenPorts;
                foreach (int port in portsToOpen)
                {
                    INetFwOpenPort openport = (INetFwOpenPort)getInstance("INetOpenPort");
                    openport.Port = port;
                    openport.Protocol = NET_FW_IP_PROTOCOL_.NET_FW_IP_PROTOCOL_UDP;
                    openport.Name = applicationName + " Operation Port (" + port.ToString() + ")";
                    openports.Add(openport);
                }
                openports = null;
            }
            catch (Exception)
            {


            }
            Console.WriteLine("Firewall : Open Ports");
        }
        #endregion // openFirewall

        #region closeFirewall
        ///
        /// Deletes the firewall ports specified from the windows firewall exclusions list. Also deletes
        /// the given application from the excluded apps.
        ///
        /// The path of the application. Please include the ending \
        /// The name of the executable to close
        /// The ports that you wish to close individually
        public void closeFirewall(string executableFilePath, string applicationName, int[] portsToClose)
        {
            try
            {


                setProfile();
                INetFwAuthorizedApplications apps = fwProfile.AuthorizedApplications;
                apps.Remove(executableFilePath);
                apps = null;
                INetFwOpenPorts ports = fwProfile.GloballyOpenPorts;
                foreach (int port in portsToClose)
                {
                    ports.Remove(port, NET_FW_IP_PROTOCOL_.NET_FW_IP_PROTOCOL_UDP);
                }
                ports = null;
            }
            catch (Exception)
            {



            }
            Console.WriteLine("Firewall : Close Ports");
        }
        #endregion

        #region setProfile
        protected void setProfile()
        {
            // Access INetFwMgr
            INetFwMgr fwMgr = (INetFwMgr)getInstance("INetFwMgr");
            INetFwPolicy fwPolicy = fwMgr.LocalPolicy;
            fwProfile = fwPolicy.CurrentProfile;
            fwMgr = null;
            fwPolicy = null;
        }
        #endregion

        #region getInstance
        protected Object getInstance(String typeName)
        {
            if (typeName == "INetFwMgr")
            {
                Type type = Type.GetTypeFromCLSID(
                new Guid("{304CE942-6E39-40D8-943A-B913C40C9CD4}"));
                return Activator.CreateInstance(type);
            }
            else if (typeName == "INetAuthApp")
            {
                Type type = Type.GetTypeFromCLSID(
                new Guid("{EC9846B3-2762-4A6B-A214-6ACB603462D2}"));
                return Activator.CreateInstance(type);
            }
            else if (typeName == "INetOpenPort")
            {
                Type type = Type.GetTypeFromCLSID(
                new Guid("{0CA545C6-37AD-4A6C-BF92-9F7610067EF5}"));
                return Activator.CreateInstance(type);
            }
            else return null;
        }
        #endregion

        #region OpenFirewallPorts
        ///
        /// Opens the given ports on windows firewall. Also opens entirely the given application on the firewall.
        /// Please remember that you need administrative rights to use this function.
        ///
        /// The path of the application. Please include the ending \
        /// The name of the executable to open
        /// The ports that you wish to open individually
        public static void OpenFirewallPorts(string executableFilePath, string applicationName, int[] portsToOpen)
        {
            Firewall fw = new Firewall();
            fw.openFirewall(executableFilePath, applicationName, portsToOpen);
        }
        #endregion

        #region CloseFirewallPorts
        ///
        /// Deletes the firewall ports specified from the windows firewall exclusions list. Also deletes
        /// the given application from the excluded apps.
        ///
        /// The path of the application. Please include the ending \
        /// The name of the executable to close
        /// The ports that you wish to close individually
        public static void CloseFirewallPorts(string executableFilePath, string applicationName, int[] portsToClose)
        {
            Firewall fw = new Firewall();
            fw.closeFirewall(executableFilePath, applicationName, portsToClose);
        }
        #endregion
    }
}

