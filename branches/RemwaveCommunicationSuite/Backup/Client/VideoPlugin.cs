using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace Remwave.Client
{
    class VideoPlugin
    {
        private string _jabberID;
        public string JabberID
        {
            get { return _jabberID; }
            set { _jabberID = value; }
        }
        private bool _initialized = false;
        public bool Initialized
        {
            get { return _initialized; }
            set { _initialized = value; }
        }
        private bool _connected = false;
        public bool Connected
        {
            get { return _connected; }
            set { _connected = value; }
        }


        [DllImport("nvideo.npm")]
        private static extern bool InitializePlugin(Int32 idNumber);

        [DllImport("nvideo.npm")]
        private static extern void ShutdownPlugin();

        [DllImport("nvideo.npm")]
        private static extern void SetVisible(bool visible);

        [DllImport("nvideo.npm")]
        private static extern void Connect(bool bConnected, UInt32 dwNetworkUpstreamBandwidth,
             UInt32 dwNetworkDownstreamBandwidth, UInt32 dwReservedBandwidth,
             string address, bool bRemoteBehindFirewall, bool bAsymetricFlag,
             string conferenceID,
             string userID);

        [DllImport("nvideo.npm")]
        private static extern void Disconnect();

        public bool Initialize(Int32 idNumber)
        {
            this.Initialized = InitializePlugin(idNumber);
            return this.Initialized;
        }

        public void StartConference(bool bConnected, UInt32 dwNetworkUpstreamBandwidth,
             UInt32 dwNetworkDownstreamBandwidth, UInt32 dwReservedBandwidth,
             string address, bool bRemoteBehindFirewall, bool bAsymetricFlag,
             string conferenceID,
             string userID, string jabberID)
        {
            if (!this.Initialized)
            {
                this.Initialize(1);
            }

            if (this.Connected)
            {
                this.StopConference();
            }

            VideoPlugin.Connect(bConnected, dwNetworkUpstreamBandwidth,
              dwNetworkDownstreamBandwidth, dwReservedBandwidth,
                 address, bRemoteBehindFirewall, bAsymetricFlag, conferenceID, userID);
            this.JabberID = jabberID;
            this.Connected = true;
        }
        
        public void Show(bool visible)
        {
            if (!this.Initialized)
            {
                this.Initialize(1);
            }
            VideoPlugin.SetVisible(visible);

        }

        public void StopConference()
        {
            if (this.Initialized)
            {
                VideoPlugin.Disconnect();
                this.Connected = false;
                VideoPlugin.SetVisible(false);
            }
        }

        public void UnInitialize()
        {
              if (this.Initialized)
            {
            VideoPlugin.ShutdownPlugin();
        }
        }
    }
}