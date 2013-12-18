using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Threading;

namespace Remwave.Client
{
    public class VideoPlugin : IDisposable
    {

        private String mTitle = "online meeting";
        private IntPtr mLib = IntPtr.Zero;

        #region Constructor/Destructor and IDisposable implemenation
        [DllImport("kernel32.dll")]
        private extern static IntPtr LoadLibrary(string fileName);

        [DllImport("kernel32.dll")]
        private extern static bool FreeLibrary(IntPtr lib);

        public VideoPlugin(string fileName, string title)
        {
            mLib = LoadLibrary(fileName);
            mTitle = title;
        }

        ~VideoPlugin()
        {
            Dispose();
        }

        public void Dispose()
        {
            if (mLib == IntPtr.Zero) return;
            FreeLibrary(mLib);
            mLib = IntPtr.Zero;
        }

        protected IntPtr Library
        {
            get
            {
                if (mLib == IntPtr.Zero)
                    throw new InvalidOperationException("Plug-in library is not loaded");
                return mLib;
            }
        }

        #endregion

        #region Plugin DllImports
        /// <summary>
        /// Callback delegate for certain video plugin events
        /// </summary>
        /// <param name="idNumber">id number that you pass in for whatever reason</param>
        /// <param name="msg">Message of event</param>
        /// <param name="evtSenderHwnd">HWND of sender</param>
        /// <returns>depends on event type. For shutdown, returning 1 causes video plugin to shutdown, flase ignores</returns>
        public delegate UInt32 BoolCallbackHandler(Int32 idNumber, VideoPlugin.EventMessage msg, IntPtr evtSenderHwnd);

        [DllImport("nvideo.npm")]
        public static extern bool InitializePlugin(Int32 idNumber, IntPtr empty, BoolCallbackHandler handler);

       // [DllImport("nvideo.npm")]
        //public static extern void ShutdownPlugin();

        [DllImport("nvideo.npm")]
        public static extern void SetWindowActivated(bool active);

        [DllImport("nvideo.npm")]
        public static extern void SetVisible(bool visible);

        [DllImport("nvideo.npm")]
        public static extern void PluginConnect(bool bConnected, UInt32 dwNetworkUpstreamBandwidth, UInt32 dwNetworkDownstreamBandwidth, UInt32 dwReservedBandwidth, string address, bool bRemoteBehindFirewall, bool bAsymetricFlag, string conferenceID, string userID, bool bMultipartyConference, bool bPresenter, string pluginWindowTitle);

        [DllImport("nvideo.npm")]
        public static extern void PluginDisconnect();

        /// <summary>
        /// Allows / Disallows sending data from plugin
        /// </summary>
        /// <param name="bPlaying"></param>
        /// <returns>Same mode as requested if successful (returns what GetPlaying would)</returns>
        [DllImport("nvideo.npm")]
        public static extern bool SetPlaying(bool bPlaying);

       // [DllImport("nvideo.npm")]
        //public static extern bool GetPlaying();

        public enum EventMessage
        {
            /// <summary>
            /// Happens when user clicks X button of plugin window
            /// </summary>
            ShuttingDown = 0,
            AudioPushed = 1
        };

        #endregion

        #region Shutdow Thread
        private Boolean mShutdown = false;
        private Thread mShutdownThread;

        private void ShutdownThreadFunction()
        {
            try
            {
                Thread.Sleep(500);
                this.UnInitialize();
            }
            catch (Exception)
            {
                
            }
            Thread.CurrentThread.Abort();
        }
        #endregion

        #region Public Events
        public event EventHandler VideoPluginExit;
        internal void OnVideoPluginExit(object sender, EventArgs args)
        {
            if (VideoPluginExit != null)
            {
                VideoPluginExit(sender, args);
            }
        }

        #endregion


        public string JabberID;
        public bool Initialized;
        public bool Connected;


        BoolCallbackHandler mCallback = null;
        public UInt32 BoolCallbackFunction(Int32 idNumber, VideoPlugin.EventMessage msg, IntPtr evtSenderHwnd)
        {
            mShutdown = true;

            ThreadStart st = new ThreadStart(ShutdownThreadFunction);
            mShutdownThread = new Thread(st);
            mShutdownThread.Start();
 
            return 1;
        }

        public void Initialize(Int32 idNumber)
        {
            if (mShutdown) return;
            if (mCallback == null)
                mCallback = new BoolCallbackHandler(BoolCallbackFunction);
            this.Initialized = InitializePlugin(idNumber, IntPtr.Zero, mCallback);
            return;
        }

        public void StartConference(bool bConnected, UInt32 dwNetworkUpstreamBandwidth, UInt32 dwNetworkDownstreamBandwidth, UInt32 dwReservedBandwidth, string address, bool bRemoteBehindFirewall, bool bAsymetricFlag, string conferenceID, string userID, string jabberID)
        {
            if (mShutdown) return;

            if (!this.Initialized)
            {
                this.Initialize(1);
            }

            if (this.Connected)
            {
                this.StopConference();
            }

            VideoPlugin.PluginConnect(bConnected, dwNetworkUpstreamBandwidth,dwNetworkDownstreamBandwidth, dwReservedBandwidth, address, bRemoteBehindFirewall, bAsymetricFlag, conferenceID, userID, false, false, mTitle);
            VideoPlugin.SetPlaying(true);
            this.JabberID = jabberID;
            this.Connected = true;
            VideoPlugin.SetVisible(true);
        }

        public void StopConference()
        {
            if (mShutdown) return;
            if (this.Initialized)
            {
                VideoPlugin.PluginDisconnect();
                this.Connected = false;
                this.UnInitialize();
            }
        }

        public void UnInitialize()
        {
                if (this.Connected)
                {
                    this.StopConference();
                }
                if (this.Initialized)
                {
                    VideoPlugin.SetVisible(false);
                //    VideoPlugin.ShutdownPlugin();
                  //  this.Initialized = false;
                    OnVideoPluginExit(this, new EventArgs());
                }
                mShutdown = false;
        }
    }
}