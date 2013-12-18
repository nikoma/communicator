using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Remwave_RVoIPLib;
using Remwave.Services;
using nsoftware.IPWorks;
using System.Collections;
using System.Net;
using System.IO;
using System.Diagnostics;
using System.Net.NetworkInformation;
using System.Text.RegularExpressions;
using Telerik.WinControls.UI;
using Remwave.Client.Serializable;
using System.Threading;
using System.Runtime.InteropServices;
using System.Net.Sockets;

namespace Remwave.Client
{

    public partial class ClientForm : ShapedForm
    {

        #region public

        public SIPPhone mySIPPhone = new SIPPhone();
        public UserAccount myUserAccount = new UserAccount();
        public NetworkInfo myNetworkInfo = new NetworkInfo();
        public ClientConfiguration myClientConfiguration = new ClientConfiguration();
        public Hashtable myBuddyGroups = new Hashtable();
        public Hashtable myBuddyList = new Hashtable();
        public Hashtable myBuddyPresence = new Hashtable();
        public WEBPhoneBook myContactsBook;
        public bool FormIsClosing;
        public bool UpdatedSync = false;
        Thread getContactsBookThread;

        #endregion

        #region private

       private int hitCount = 0;
        private long lastTick = -1;

        private LINE_STATE[] mySIPPhoneLines;

        private ConfigurationWindow myConfigurationWindow;
        private ContactsWindow myContactWindow;
        private SpeedDialWindow mySpeedDialWindow;
        private ClientCallBack oCallback;
        private Phone2PhoneWindow myPhone2PhoneWindow;
        private Remwave.Client.ChatWindow myChatWindow;
        private List<CallRecord> myCallHistoryRecords = new List<CallRecord>();
        private CallHistorySerializer myCallHistorySerializer = new CallHistorySerializer();
        private const string ENGINE_KEY = "4e56f54543a5530f939106e1ca717291095e99e65e10747f508546fdcd5e88f017a533c59fd1a63f59332a3d14cf0a3d420b025fc41ae852018cfbd1ac013032";
        private VideoPlugin myVideoPlugin = new VideoPlugin();
        #endregion

        [DllImport("user32.dll")]
        public static extern UInt32 RegisterWindowMessage([MarshalAs(UnmanagedType.LPTStr)] String lpString);

        [DllImport("user32.dll")]
        public static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vlc);

        [DllImport("user32.dll")]
        public static extern bool UnregisterHotKey(IntPtr hWnd, int id);


        uint WM_DIAL = RegisterWindowMessage(Guid.NewGuid().ToString());
        #region WndProc hook / meta key watchdog
        /// <summary>
        /// WndProc has following direction
        /// - counting ALT meta key presses inside a second
        /// </summary>


        protected override void WndProc(ref Message m)
        {
            if (myUserAccount.LoggedIn())
            {

                if (m.Msg == 74)
                {

                    string s = Marshal.PtrToStringAuto(m.LParam);

                }
                if (m.Msg == 0x0312)
                {
                    //				Console.WriteLine("Hitcount: {0}  Ticks: {1}",hitCount, System.DateTime.Now.Ticks);
                    if (lastTick == -1 || System.DateTime.Now.Ticks < (lastTick + 20000000))
                    {
                        //					Console.WriteLine("hit");
                        hitCount++;

                        if (hitCount == 2)
                        {
                            hitCount = 0;
                            //triggered
                            ShowHideSpeedDialWindow();
                            lastTick = -1;
                            base.WndProc(ref m);
                            return;
                        }
                    }
                    else
                    {
                        //					Console.WriteLine("no hit");
                        hitCount = 0;
                        lastTick = -1;
                        base.WndProc(ref m);
                        return;
                    }
                    lastTick = System.DateTime.Now.Ticks;
                }
            }
            base.WndProc(ref m);
        }
        #endregion


        public ClientForm()
        {

            // Add the handlers to the NetworkChange events.
            NetworkChange.NetworkAvailabilityChanged +=
                NetworkAvailabilityChanged;
            NetworkChange.NetworkAddressChanged +=
                NetworkAddressChanged;

            // initialize phone lines states
            mySIPPhoneLines = new LINE_STATE[4];
            mySIPPhoneLines[0] = new LINE_STATE();
            mySIPPhoneLines[1] = new LINE_STATE();
            mySIPPhoneLines[2] = new LINE_STATE();
            mySIPPhoneLines[3] = new LINE_STATE();

            InitializeComponent();

            //preset size
            this.ClientSize = new Size(300, 466);
            this.MaximizeBox = false;
            this.MaximumSize = this.Size;
            this.MinimumSize = this.Size;


            //open login panel and process login
            myMainWindowSplitContainer.Panel1Collapsed = false;
            myMainWindowSplitContainer.Panel2Collapsed = true;

            //Media Engine Callback
            oCallback = new ClientCallBack(this.Callback);

            //Initialize ChatWindow
            myChatWindow = new Remwave.Client.ChatWindow(this);

            //link Presence Images
            myRosterListTreeView.ImageList = myChatWindow.myPresenceImagesList;

            //select Default Phone Line Tab
            myPhoneLinesTabControl.SelectedTab = myPhoneLine0Tab;
            myDialPadTabControl.SelectedTab = tabItem1;

            RegisterHotKey(this.Handle, this.GetType().GetHashCode(), 1, 0);

        }


        private void ConfigureServices()
        {
            ConfigSIP.ProxyAddress = myClientConfiguration.SIPProxyAddress;
            ConfigSIP.Realm = myClientConfiguration.SIPProxyRealm;
            ConfigIM.IMServer = myClientConfiguration.IMServerAddress;
            ConfigTestNetwork.HostIp = myClientConfiguration.IMServerAddress;
            ConfigVideoProxy.ProxyAddress = myClientConfiguration.VideoProxyAddress;
        }

        private void ConfigureClient(bool ExitOnCancel)
        {

            myConfigurationWindow = new ConfigurationWindow(this);
            if (myConfigurationWindow.ShowDialog() == DialogResult.OK)
            {
                ConfigureServices();
                
            }
            else
            {
                if (ExitOnCancel) Application.Exit();
            }
        }

        #region Activities

        public void StartActivity(Activity activity)
        {
            mySpeedDialWindow.Hide();
            switch (activity.ActivityType)
            {
                case ActivityType.None:
                    break;
                case ActivityType.Call:
                    StartNewCall(-1, activity.ActivityData);
                    this.WindowState = FormWindowState.Normal;
                    this.Show();
                    this.Activate();
                    break;
                case ActivityType.IM:
                    StartNewIM(activity.ActivityData, true);
                    break;
                case ActivityType.VideoCall:
                    StartNewVideoCall(activity.ActivityData);
                    this.WindowState = FormWindowState.Normal;
                    this.Show();
                    this.Activate();
                    break;
                case ActivityType.ScreenSharing:
                    StartNewVideoCall(activity.ActivityData);
                    this.WindowState = FormWindowState.Normal;
                    this.Show();
                    this.Activate();
                    break;
                case ActivityType.Email:
                    StartNewEmail(activity.ActivityData);
                    break;
                case ActivityType.AddContact:
                    StartNewContact(activity.ActivityData);
                    break;
                default:
                    break;
            }



        }



        public void StartNewIM(String jabberID, bool setFocus)
        {
            try
            {
                myChatWindow.NewChat(jabberID, setFocus);
                myChatWindow.Show();
                xmppControl.ProbePresence(jabberID);
            }
            catch (Exception)
            {

                //throw;
            }

        }

        public void StartNewVideoCall(String jabberID)
        {
            VideoSessionInvite(jabberID);
        }

        public void StartNewEmail(String email)
        {
            try
            {
                System.Diagnostics.Process.Start(@"mailto:" + email);
            }
            catch (Exception)
            {

                //throw;
            }
        }

        public void StartNewContact(String jabberID)
        {
            Hashtable properties = new Hashtable();
            if (jabberID != null)
            {
                int dummy;
                if (Int32.TryParse(jabberID, out dummy))
                {
                    properties.Add("NTHomeTelephoneNumber", jabberID);
                }
                else
                {
                    properties.Add("NTJabberID", jabberID);
                }

                myContactWindow = new ContactsWindow(this, null, properties);
                myContactWindow.FormClosing += new FormClosingEventHandler(myContactWindow_FormClosing);
                myContactWindow.Show();
            }
        }

        #endregion
        private void Client_Load(object sender, EventArgs e)
        {
            try
            {
                ClientConfigurationSerializer serializer = new ClientConfigurationSerializer();
                myClientConfiguration = serializer.LoadClientConfiguration();

                lnChangeSettings.Visible = ConfigProfile.AllowConfiguration;
                //check the web service for update and newses

                try
                {

                    RemwaveCSWS.Service remwaveCSWS = new Remwave.Client.RemwaveCSWS.Service();

                    string[] version = remwaveCSWS.Version();

                    if (Application.ProductVersion != version[0])
                    {
                        //force update
                        string downloadlink = remwaveCSWS.Software();
                        if (MessageBox.Show(this, "New Version of nikotel client is  avaliable for download!\nClick [OK] to open the download page.", "Update avaliable.", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
                        {
                            System.Diagnostics.Process.Start(downloadlink);
                            Application.Exit();
                        };
                    }

                    if (myClientConfiguration.LastNews != version[1])
                    {
                        //display news
                        string newsLink = remwaveCSWS.News();
                        System.Diagnostics.Process.Start(newsLink);
                        myClientConfiguration.LastNews = version[1];

                    }

                }
                catch (Exception)
                {
                    // throw;
                    ;
                }



                if (ConfigProfile.AllowConfiguration)
                {
                    //check in services are configured
                    bool runConfiguration = false;
                    if (myClientConfiguration.SIPProxyAddress == "") runConfiguration = true;
                    if (myClientConfiguration.SIPProxyRealm == "") runConfiguration = true;
                    if (myClientConfiguration.VideoProxyAddress == "") runConfiguration = true;
                    if (myClientConfiguration.IMServerAddress == "") runConfiguration = true;
                    if (myClientConfiguration.RSIUrl == "") runConfiguration = true;
                    if (runConfiguration)
                    {
                        ConfigureClient(true);
                    }
                    ConfigureServices();
               }






                if (myClientConfiguration.RememberMe)
                {
                    myLoginRememberMeCheckBox.Checked = true;
                    myLoginPasswordInput.Text = myClientConfiguration.Password;
                    myLoginUsernameInput.Text = myClientConfiguration.Username;
                    if (myClientConfiguration.AutoLogin)
                    {
                        myLoginAutoLoginCheckBox.Checked = true;
                        myLoginButton.PerformClick();
                    }
                }

            }
            catch (Exception)
            {

                //
            }

        }


        private bool ReInitializeClient()
        {
            if ((!this.FormIsClosing) && (myUserAccount.LoggedIn()))
            {
                TELEPHONY_RETURN_VALUE result;
                for (; true; )
                {
                    DetectNetwork();
                    if (!myNetworkInfo.Online)
                    {
                        Console.WriteLine("Network:SIP Proxy Does not respond, network time out.");
                        myClientNotifyIcon.ShowBalloonTip(10, "Service not reachable", "Service does not respond or network is not avaliable, please check your network settings or try again later.", ToolTipIcon.Warning);
                        break;
                    }

                    if (!xmppControl.Connected)
                    {
                        //network detected, try authenticate at jabber
                        try
                        {
                            xmppControl.Connect(myUserAccount.Username, myUserAccount.Password);
                        }
                        catch (IPWorksException ipwime)
                        {
                            Console.WriteLine("IM:ReConnecting:(" + ipwime.Code + "):" + ipwime.Message);
                            break;
                        }
                    }

                    try
                    {
                        result = mySIPPhone.ReStartSip(myNetworkInfo.LocalIP);
                        if (result != TELEPHONY_RETURN_VALUE.SipSuccess)
                        {
                            Console.WriteLine("SIP:ReStartSip:" + result.ToString());
                            break;
                        }

                    }
                    catch (Exception)
                    {
                        myClientNotifyIcon.ShowBalloonTip(10, "Service not reachable", "Service does not respond or network is not avaliable, please check your network settings or try again later.", ToolTipIcon.Warning);
                        break;
                    }
                    myClientNotifyIcon.ShowBalloonTip(10, "Connected", "You are online!", ToolTipIcon.Info);
                    return true;
                }
            }
            return false;
        }

        private bool InitializeClient()
        {
            if (!this.FormIsClosing)
            {
                for (; true; )
                {
                    TELEPHONY_RETURN_VALUE result;
                    DetectNetwork();

                    if (!myNetworkInfo.Online)
                    {
                        Console.WriteLine("Network: SIP Proxy Does not respond, network time out.");
                        myClientNotifyIcon.ShowBalloonTip(10, "Service not reachable", "Service does not respond or network is not avaliable, please check your network settings or try again later.", ToolTipIcon.Warning);
                        break;
                    }

                    //network detected, try authenticate at jabber
                    try
                    {
                        if (xmppControl.Connected) xmppControl.Disconnect();
                        xmppControl.InvokeThrough = this;
                        xmppControl.Timeout = 7200;
                        xmppControl.IMServer = ConfigIM.IMServer;
                        xmppControl.IMPort = ConfigIM.IMPort;
                        xmppControl.Connect(myLoginUsernameInput.Text, myLoginPasswordInput.Text);
                    }
                    catch (IPWorksException ipwime)
                    {
                        Console.WriteLine("IM:Connect error:" + ipwime.Code.ToString() + " - " + ipwime.Message);
                        MessageBox.Show(this, "Username / Password invalid!\nMake sure you have your username typed in all lowercase letters and that your password is typed correctly.", "Authorization Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                    }


                    try
                    {
                        result = mySIPPhone.InitEngine(oCallback, myLoginUsernameInput.Text, myLoginPasswordInput.Text, ConfigSIP.Realm, ConfigSIP.ProxyAddress, myNetworkInfo.LocalIP, ENGINE_KEY);
                        if (result != TELEPHONY_RETURN_VALUE.SipSuccess)
                        {
                            xmppControl.Disconnect();
                            Console.WriteLine("SIP:InitEngine:" + result.ToString());
                            break;
                        }

                    }
                    catch (Exception)
                    {
                        myClientNotifyIcon.ShowBalloonTip(10, "Service not reachable", "Service does not respond or network is not avaliable, please check your network settings or try again later.", ToolTipIcon.Warning);
                        break;
                    }

                    myUserAccount.Login(myLoginUsernameInput.Text, myLoginPasswordInput.Text);

                    myClientNotifyIcon.ShowBalloonTip(10, "Connected", "You are online!", ToolTipIcon.Info);
                    return true;
                }
            }
            return false;
        }

        private void ClientForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {

                this.FormIsClosing = true;

                UnregisterHotKey(this.Handle, this.GetType().GetHashCode());

                CallHistory tmpCallHistory = new CallHistory(myUserAccount.Username);
                tmpCallHistory.CallRecords = myCallHistoryRecords.ToArray();
                myCallHistorySerializer.SaveCallHistory(tmpCallHistory, myUserAccount.Username);
                mySIPPhone.ShutdownEngine();
                myVideoPlugin.UnInitialize();
                Thread.Sleep(10000);

            }
            catch (Exception ex)
            {
                Console.WriteLine("Closing Form Exception : " + ex.Message);
                throw;
            }
        }

        #region Detect Network Avaliability Changes

        private void NetworkAvailabilityChanged(
             object sender, NetworkAvailabilityEventArgs e)
        {
            // Report whether the network is now available or unavailable.
            if (e.IsAvailable)
            {
                Console.WriteLine("Network Available");
                myNetworkInfo.Online = true;

            }
            else
            {
                Console.WriteLine("Network Unavailable");
                myNetworkInfo.Online = false;
                myClientNotifyIcon.ShowBalloonTip(10, "Network Unavaliable", "Remwave Communication Suite functionality will be limited, check your network.", ToolTipIcon.Warning);

            }
        }

        // Declare a method to handle NetworkAdressChanged events.
        private void NetworkAddressChanged(object sender, EventArgs e)
        {
            // IP has changed reinitialize client
            string curIP = myNetworkInfo.LocalIP;
            DetectNetwork();
            if (curIP != myNetworkInfo.LocalIP)
            {

                if (myUserAccount.LoggedIn())
                {
                    Console.WriteLine("Current IP Addresses:");
                    this.ReInitializeClient();
                }
            }
        }
        #endregion



        #region VoIP Media Engine Callback
        public void Callback(int PhoneLineID, int NotificationType, int TelephonyEvent, string EventMessage)
        {
            try
            {
                SIP_NOTIFY_TYPE not = (SIP_NOTIFY_TYPE)NotificationType;
                TELEPHONY_RETURN_VALUE retval = (TELEPHONY_RETURN_VALUE)TelephonyEvent;

                string sNot = not.ToString();
                string sRet = retval.ToString();

                if (not == SIP_NOTIFY_TYPE.GLOBAL_NOTIFICATION)
                {
                    Console.WriteLine("SIP|" + PhoneLineID.ToString() + " : " + sNot + " / " + sRet);
                    switch (retval)
                    {
                        case TELEPHONY_RETURN_VALUE.SipCallEngineReady:
                            // the telephony engine has been created.

                            break;

                        case TELEPHONY_RETURN_VALUE.SipCallEngineTerminated:
                            // the telephony engine has been terminated.
                            break;

                        case TELEPHONY_RETURN_VALUE.SipLineCapacityReached:
                            // the telephony engine detected a new incoming phone call
                            // but all phone lines were in use.
                            break;

                        case TELEPHONY_RETURN_VALUE.SipRegisterTrying:
                            // attempting to register with a proxy or
                            // sip registrar server.
                            break;

                        case TELEPHONY_RETURN_VALUE.SipRegisterSuccess:
                            // registration was successful.
                            break;

                        case TELEPHONY_RETURN_VALUE.SipRegistrationIntervalError:
                            // registration time interval that was specified was
                            // rejected by the server. the interval is too small.
                            // registration failed.
                            break;

                        case TELEPHONY_RETURN_VALUE.SipRegistrationTimeOut:
                            // we timed out waiting for the registration response to be
                            // sent back from the server.
                            break;

                        case TELEPHONY_RETURN_VALUE.SipRegisterErrorBadCredentials:

                            // we sent authorization credentials with the registration
                            // but they are no good. the configured user name and password
                            // for the domain are not correct.
                            break;

                        case TELEPHONY_RETURN_VALUE.SipRegisterError:
                            // unknown error. registration failed.
                            break;

                        default:
                            // do nothing.
                            break;
                    }
                }
                else if (not == SIP_NOTIFY_TYPE.IMMEDIATE_NOTIFICATION)
                {
                    //sip messages notification



                }
                else if (not == SIP_NOTIFY_TYPE.PHONE_LINE_NOTIFICATION)
                {

                    //update Line State
                    mySIPPhoneLines[PhoneLineID].State = mySIPPhone.GetPhoneLineState(PhoneLineID);
                    mySIPPhoneLines[PhoneLineID].CallDirection = mySIPPhone.GetPhoneLineCallDirection(PhoneLineID);
                    mySIPPhoneLines[PhoneLineID].CallRecordingActive = mySIPPhone.GetPhoneLineCallRecordingActive(PhoneLineID) == 1 ? true : false;

                    Console.WriteLine("SIP|" + PhoneLineID.ToString() + " : " + sNot + " / " + sRet);
                    Console.WriteLine("SIP-LS|" + PhoneLineID.ToString() + ":" + mySIPPhoneLines[PhoneLineID].State.ToString());

                    switch (retval)
                    {
                        //reset phone line state
                        case TELEPHONY_RETURN_VALUE.SipOnHook:
                            mySIPPhoneLines[PhoneLineID].OnHook(myCallHistoryRecords);
                            ((TabItem)myPhoneLinesTabControl.Items[PhoneLineID]).ImageIndex = 0;
                            break;

                        case TELEPHONY_RETURN_VALUE.SipIncomingCallStart:

                            string myCallFrom = mySIPPhone.GetIncomingCallDetails(PhoneLineID);
                            myClientNotifyIcon.ShowBalloonTip(10, "Incoming Call", "You have a call from : " + myCallFrom, ToolTipIcon.Info);
                            mySIPPhoneLines[PhoneLineID].LastDialedNumber = myCallFrom;

                            switch (PhoneLineID)
                            {
                                case 0: myPhoneLine0Tab.ImageIndex = 2;
                                    myPhoneLinesTabControl.SelectedTab = myPhoneLine0Tab;
                                    break;
                                case 1: myPhoneLine1Tab.ImageIndex = 2;
                                    myPhoneLinesTabControl.SelectedTab = myPhoneLine1Tab;
                                    break;
                                case 2: myPhoneLine2Tab.ImageIndex = 2;
                                    myPhoneLinesTabControl.SelectedTab = myPhoneLine2Tab;
                                    break;
                                case 3: myPhoneLine3Tab.ImageIndex = 2;
                                    myPhoneLinesTabControl.SelectedTab = myPhoneLine3Tab;
                                    break;
                            }

                            myDialPadTabControl.SelectedTab = tabItem1;

                            IncomingCall(PhoneLineID, myCallFrom);

                            break;

                        case TELEPHONY_RETURN_VALUE.SipOutgoingCallStart:


                            switch (PhoneLineID)
                            {
                                case 0: myPhoneLine0Tab.ImageIndex = 1;
                                    break;
                                case 1: myPhoneLine1Tab.ImageIndex = 1;
                                    break;
                                case 2: myPhoneLine2Tab.ImageIndex = 1;
                                    break;
                                case 3: myPhoneLine3Tab.ImageIndex = 1;
                                    break;
                            }

                            break;
                        case TELEPHONY_RETURN_VALUE.SipInCall:
                        case TELEPHONY_RETURN_VALUE.SipOffHook:
                            mySIPPhoneLines[PhoneLineID].CallActive = true;
                            switch (PhoneLineID)
                            {
                                case 0: myPhoneLine0Tab.ImageIndex = 2;
                                    break;
                                case 1: myPhoneLine1Tab.ImageIndex = 2;
                                    break;
                                case 2: myPhoneLine2Tab.ImageIndex = 2;
                                    break;
                                case 3: myPhoneLine3Tab.ImageIndex = 2;
                                    break;
                            }
                            break;


                        case TELEPHONY_RETURN_VALUE.SipInConference:
                        case TELEPHONY_RETURN_VALUE.SipInConferenceOn:
                            mySIPPhoneLines[PhoneLineID].CallConferenceActive = true;
                            switch (PhoneLineID)
                            {
                                case 0: myPhoneLine0Tab.ImageIndex = 5;
                                    break;
                                case 1: myPhoneLine1Tab.ImageIndex = 5;
                                    break;
                                case 2: myPhoneLine2Tab.ImageIndex = 5;
                                    break;
                                case 3: myPhoneLine3Tab.ImageIndex = 5;
                                    break;
                            };
                            break;
                        case TELEPHONY_RETURN_VALUE.SipInConferenceOff:
                            mySIPPhoneLines[PhoneLineID].CallConferenceActive = false;
                            break;
                        case TELEPHONY_RETURN_VALUE.SipCallHold:
                        case TELEPHONY_RETURN_VALUE.SipCallHoldOn:
                            mySIPPhoneLines[PhoneLineID].CallHoldActive = true;
                            switch (PhoneLineID)
                            {
                                case 0: myPhoneLine0Tab.ImageIndex = 4;
                                    break;
                                case 1: myPhoneLine1Tab.ImageIndex = 4;
                                    break;
                                case 2: myPhoneLine2Tab.ImageIndex = 4;
                                    break;
                                case 3: myPhoneLine3Tab.ImageIndex = 4;
                                    break;
                            };
                            break;
                        case TELEPHONY_RETURN_VALUE.SipCallHoldOff:
                            mySIPPhoneLines[PhoneLineID].CallHoldActive = false;
                            switch (PhoneLineID)
                            {
                                case 0: myPhoneLine0Tab.ImageIndex = 2;
                                    break;
                                case 1: myPhoneLine1Tab.ImageIndex = 2;
                                    break;
                                case 2: myPhoneLine2Tab.ImageIndex = 2;
                                    break;
                                case 3: myPhoneLine3Tab.ImageIndex = 2;
                                    break;
                            }
                            break;
                        case TELEPHONY_RETURN_VALUE.SipCallRecordActive:
                            mySIPPhoneLines[PhoneLineID].CallRecordingActive = true;
                            break;
                        case TELEPHONY_RETURN_VALUE.SipCallRecordComplete:
                            mySIPPhoneLines[PhoneLineID].CallRecordingActive = false;
                            break;
                        case TELEPHONY_RETURN_VALUE.SipTransferingCall:
                            mySIPPhoneLines[PhoneLineID].CallTransferActive = true;
                            break;


                        case TELEPHONY_RETURN_VALUE.SipFarEndIsBusy:
                            mySIPPhoneLines[PhoneLineID].LastErrorMessage = "Busy";
                            break;
                        case TELEPHONY_RETURN_VALUE.SipFarEndError:

                            if (EventMessage != null)
                            {
                                mySIPPhoneLines[PhoneLineID].LastErrorMessage = EventMessage;

                            }

                            break;

                        ///
                    }


                    UpdatePhoneLineButtons();
                }
                return;
            }
            catch (InvalidCastException)
            {

            }
            finally
            {

            }
        }

        public void StartNewCall(int PhoneLineID, string Destination)
        {
            TELEPHONY_RETURN_VALUE result;
            //sip:18585477597@examplevoip.com
            try
            {
                if (PhoneLineID == -1)
                {
                    PhoneLineID = mySIPPhone.GetFreePhoneLine();
                }

                //check if not already started

                for (int i = 0; i < 4; i++)
                {
                    if ((mySIPPhoneLines[i].LastDialedNumber == Destination) && (mySIPPhoneLines[i].State == TELEPHONY_RETURN_VALUE.SipInCall))
                    {
                        return;
                    }
                }

                mySIPPhoneLines[PhoneLineID].LastDialedNumber = Destination;
                myMainTabControl.SelectedTab = myMainDialPadTabPage;

                switch (PhoneLineID)
                {
                    case 0:
                        myPhoneLinesTabControl.SelectedTab = myPhoneLine0Tab;
                        break;
                    case 1:
                        myPhoneLinesTabControl.SelectedTab = myPhoneLine1Tab;
                        break;
                    case 2:
                        myPhoneLinesTabControl.SelectedTab = myPhoneLine2Tab;
                        break;
                    case 3:
                        myPhoneLinesTabControl.SelectedTab = myPhoneLine3Tab;
                        break;
                    default:
                        return;
                }
                myDialPadTabControl.SelectedTab = tabItem1;
                result = mySIPPhone.CallOrAnswer(PhoneLineID, "sip:" + Destination + "@" + ConfigSIP.Realm);
                Console.WriteLine("SIP" + result.ToString());
            }
            catch (Exception)
            {

                return;
            }
        }


        private delegate void IncomingCallDelegate(int PhoneLineID, String CallFrom);


        private void IncomingCall(int PhoneLineID, String CallFrom)
        {
            if (this.InvokeRequired)
            {
                BeginInvoke(new IncomingCallDelegate(IncomingCall), new object[] { PhoneLineID, CallFrom });
            }
            else
            {
                myMainTabControl.SelectedTab = myMainDialPadTabPage;
                myDialPadInfoCallingValueLabel.Text = CallFrom;
                myDialPadCallOrAnswerButton.Focus();
            }
        }

        private delegate void UpdatePhoneLineButtonsDelegate();
        private void UpdatePhoneLineButtons()
        {

            if (this.InvokeRequired)
            {
                BeginInvoke(new UpdatePhoneLineButtonsDelegate(UpdatePhoneLineButtons));
            }
            else
            {
                int PhoneLineID = 0;
                if (myPhoneLinesTabControl.SelectedTab.Tag != null)
                {
                    if (Int32.TryParse(myPhoneLinesTabControl.SelectedTab.Tag.ToString(), out PhoneLineID))
                    {
                        myDestinationTextBox.Text = mySIPPhoneLines[PhoneLineID].LastDialedNumber;

                        //update buttons
                        //conf
                        myPhoneLineCONFButton.Enabled = mySIPPhoneLines[PhoneLineID].CallActive;
                        myPhoneLineCONFButton.Image = mySIPPhoneLines[PhoneLineID].CallConferenceActive ? myOnOffIcons.Images[1] : myOnOffIcons.Images[0];

                        //hold
                        myPhoneLineHOLDButton.Enabled = mySIPPhoneLines[PhoneLineID].CallActive;
                        myPhoneLineHOLDButton.Image = mySIPPhoneLines[PhoneLineID].CallHoldActive ? myOnOffIcons.Images[1] : myOnOffIcons.Images[0];

                        //rec
                        myPhoneLineRECButton.Enabled = mySIPPhoneLines[PhoneLineID].CallActive;
                        myPhoneLineRECButton.Image = mySIPPhoneLines[PhoneLineID].CallRecordingActive ? myOnOffIcons.Images[1] : myOnOffIcons.Images[0];

                        //xfer
                        myPhoneLineXFERButton.Enabled = mySIPPhoneLines[PhoneLineID].CallActive;
                        myPhoneLineXFERButton.Image = mySIPPhoneLines[PhoneLineID].CallTransferActive ? myOnOffIcons.Images[1] : myOnOffIcons.Images[0];

                        switch (mySIPPhoneLines[PhoneLineID].State)
                        {

                            case TELEPHONY_RETURN_VALUE.SipIncomingCallStart:
                                myDialPadInfoStatusValueLabel.Text = "Incoming Call...";
                                myDialPadInfoCallingLabel.Text = "Call from :  ";
                                myDialPadInfoCallingValueLabel.Text = mySIPPhoneLines[PhoneLineID].LastDialedNumber;
                                myDialPadInfoCallingValueLabel.Visible = true;
                                myDialPadInfoCallingLabel.Visible = true;
                                break;
                            case TELEPHONY_RETURN_VALUE.SipOutgoingCallStart:
                            case TELEPHONY_RETURN_VALUE.SipDialing:
                                myDialPadInfoStatusValueLabel.Text = "Dialing... ";
                                myDialPadInfoCallingLabel.Text = "Calling : ";
                                myDialPadInfoCallingValueLabel.Text = mySIPPhoneLines[PhoneLineID].LastDialedNumber;
                                myDialPadInfoCallingValueLabel.Visible = true;
                                myDialPadInfoCallingLabel.Visible = true;
                                break;
                            case TELEPHONY_RETURN_VALUE.SipInCall:
                                myDialPadInfoStatusValueLabel.Text = "Connected";
                                myDialPadInfoCallingLabel.Text = "In call : ";
                                myDialPadInfoCallingValueLabel.Text = mySIPPhoneLines[PhoneLineID].LastDialedNumber;
                                myDialPadInfoCallingValueLabel.Visible = true;
                                myDialPadInfoCallingLabel.Visible = true;
                                break;
                            case TELEPHONY_RETURN_VALUE.SipOnHook:
                                myDialPadInfoStatusValueLabel.Text = "Ready";
                                myDialPadInfoCallingValueLabel.Text = "";
                                myDialPadInfoCallingLabel.Text = "";
                                myDialPadInfoCallingValueLabel.Visible = false;
                                myDialPadInfoCallingLabel.Visible = false;
                                break;
                            case TELEPHONY_RETURN_VALUE.SipFarEndIsBusy:
                            case TELEPHONY_RETURN_VALUE.SipFarEndError:
                                myDialPadInfoStatusValueLabel.Text = mySIPPhoneLines[PhoneLineID].LastErrorMessage;
                                break;
                        }
                    }
                }
            }
        }

        #endregion


        #region DialPad Events

        private void myDialPadButton_MouseDown(object sender, MouseEventArgs e)
        {
            TELEPHONY_RETURN_VALUE result;



            try
            {
                RadButton myRadButton = (RadButton)sender;
                string myDialedKey = (string)myRadButton.Tag;
                DTMF_TONE myDialedTone = DTMF_TONE.DtmfToneUndefined;

                int PhoneLineID = Int32.Parse(myPhoneLinesTabControl.SelectedTab.Tag.ToString());


                if (mySIPPhoneLines[PhoneLineID].State == TELEPHONY_RETURN_VALUE.SipInCall)
                {
                    if (myRadButton.Tag != null)
                    {
                        switch (myDialedKey)
                        {
                            case "1":
                                myDialedTone = DTMF_TONE.DtmfTone1;
                                break;
                            case "2":
                                myDialedTone = DTMF_TONE.DtmfTone2;
                                break;
                            case "3":
                                myDialedTone = DTMF_TONE.DtmfTone3;
                                break;
                            case "4":
                                myDialedTone = DTMF_TONE.DtmfTone4;
                                break;
                            case "5":
                                myDialedTone = DTMF_TONE.DtmfTone5;
                                break;
                            case "6":
                                myDialedTone = DTMF_TONE.DtmfTone6;
                                break;
                            case "7":
                                myDialedTone = DTMF_TONE.DtmfTone7;
                                break;
                            case "8":
                                myDialedTone = DTMF_TONE.DtmfTone8;
                                break;
                            case "9":
                                myDialedTone = DTMF_TONE.DtmfTone9;
                                break;
                            case "0":
                                myDialedTone = DTMF_TONE.DtmfTone0;
                                break;
                            case "*":
                                myDialedTone = DTMF_TONE.DtmfToneAsterisk;
                                break;
                            case "#":
                                myDialedTone = DTMF_TONE.DtmfTonePound;
                                break;

                        }
                        result = mySIPPhone.StartDTMF(PhoneLineID, myDialedTone);
                        Console.WriteLine("SIP StartDTMF :" + myDialedTone.ToString() + " - " + result.ToString());

                    }
                }
                else
                {
                    myDestinationTextBox.Text += myRadButton.Tag.ToString();
                }
            }
            catch (Exception)
            {

                return;
            }
        }

        private void myDialPadButton_MouseUp(object sender, MouseEventArgs e)
        {
            TELEPHONY_RETURN_VALUE result;



            try
            {
                int PhoneLineID = Int32.Parse(myPhoneLinesTabControl.SelectedTab.Tag.ToString());


                if (mySIPPhoneLines[PhoneLineID].State == TELEPHONY_RETURN_VALUE.SipInCall)
                {
                    result = mySIPPhone.StopDTMF(PhoneLineID);
                    Console.WriteLine("SIP StopDTMF : " + result.ToString());
                }
            }
            catch (Exception)
            {

                return;
            }
        }




        private void myDialPadCallCancelButton_Click(object sender, EventArgs e)
        {
            TELEPHONY_RETURN_VALUE result;
            try
            {
                int PhoneLineID = Int32.Parse(myPhoneLinesTabControl.SelectedTab.Tag.ToString());

                //cancel also video if its for same user

                if (mySIPPhoneLines[PhoneLineID].LastDialedNumber == myVideoPlugin.JabberID)
                {
                    myVideoPlugin.StopConference();
                }

                if (mySIPPhoneLines[PhoneLineID].State == TELEPHONY_RETURN_VALUE.SipOnHook)
                {
                    myDestinationTextBox.Text = "";
                }
                else
                {
                    result = mySIPPhone.CancelCall(PhoneLineID);
                    Console.WriteLine(result.ToString());
                }
            }
            catch (Exception)
            {
                return;
            }
        }


        private void myDialPadCallOrAnswerButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (myDestinationTextBox.Text != myUserAccount.Username)
                {
                    int PhoneLineID = 0;
                    if (myPhoneLinesTabControl.SelectedTab != null)
                    {
                        PhoneLineID = Int32.Parse(myPhoneLinesTabControl.SelectedTab.Tag.ToString());
                    }
                    else
                    {
                        PhoneLineID = mySIPPhone.GetFreePhoneLine();
                    }
                    StartNewCall(PhoneLineID, myDestinationTextBox.Text);
                }
            }
            catch (Exception)
            {

                return;
            }
        }


        private void myPhoneLineCONFButton_Click(object sender, EventArgs e)
        {
            TELEPHONY_RETURN_VALUE result;
            try
            {
                int PhoneLineID = Int32.Parse(myPhoneLinesTabControl.SelectedTab.Tag.ToString());
                result = mySIPPhone.ConferenceOnOffCall(PhoneLineID);
                Console.WriteLine("SIP|CONF:" + result.ToString());
            }
            catch (Exception)
            {

                return;
            }
        }
        private void myPhoneLineHOLDButton_Click(object sender, EventArgs e)
        {
            TELEPHONY_RETURN_VALUE result;
            try
            {
                int PhoneLineID = Int32.Parse(myPhoneLinesTabControl.SelectedTab.Tag.ToString());
                result = mySIPPhone.HoldOnOffCall(PhoneLineID);
                Console.WriteLine("SIP|HOLD:" + result.ToString());
            }
            catch (Exception)
            {

                return;
            }
        }

        private void myPhoneLineXFERButton_Click(object sender, EventArgs e)
        {
            TELEPHONY_RETURN_VALUE result;
            try
            {
                int PhoneLineID = Int32.Parse(myPhoneLinesTabControl.SelectedTab.Tag.ToString());
                result = mySIPPhone.XferCall(PhoneLineID, "sip:" + myDestinationTextBox.Text + "@" + ConfigSIP.Realm);
                Console.WriteLine("SIP|XFER:" + result.ToString());
            }
            catch (Exception)
            {

                return;
            }
        }

        private void myPhoneLineRECButton_Click(object sender, EventArgs e)
        {
            TELEPHONY_RETURN_VALUE result;
            try
            {
                int PhoneLineID = Int32.Parse(myPhoneLinesTabControl.SelectedTab.Tag.ToString());
                result = mySIPPhone.RecOnOffCall(PhoneLineID, Environment.GetFolderPath(Environment.SpecialFolder.Desktop));
                if (result == TELEPHONY_RETURN_VALUE.SipSuccess)
                {
                    mySIPPhoneLines[PhoneLineID].CallRecordingActive = !mySIPPhoneLines[PhoneLineID].CallRecordingActive;
                }
                Console.WriteLine("SIP|REC:" + result.ToString());
                UpdatePhoneLineButtons();
            }
            catch (Exception)
            {

                return;
            }
        }

        private void myDestinationTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (!myDestinationTextBox.Focused) { myDestinationTextBox.Focus(); }
                if (e.KeyCode == Keys.Enter)
                {
                    e.SuppressKeyPress = true;
                    myDialPadTabControl.SelectedTab = tabItem1;
                    myDialPadCallOrAnswerButton.PerformClick();
                    return;
                }
                int PhoneLineID = Int32.Parse(myPhoneLinesTabControl.SelectedTab.Tag.ToString());
                mySIPPhoneLines[PhoneLineID].LastDialedNumber = myDestinationTextBox.Text;
            }
            catch (Exception)
            {

                return;
            }


        }

        #endregion


        private void LoadContactsBook(ContactList myContactList)
        {
            myContactsListBox.Items.Clear();
            foreach (NTContact myNTContact in myContactList)
            {
                if (myNTContact.NTDeleted != "true")
                {
                    this.tmplContactListItem = new Telerik.WinControls.UI.RadListBoxItem();

                    this.myContactsListBox.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.tmplContactListItem});

                    // 
                    // tmplContactListItem
                    // 
                    this.tmplContactListItem.AccessibleDescription = myNTContact.FullName().Length > 64 ? myNTContact.FullName().Trim().Substring(0, 64) : myNTContact.FullName().Trim();
                    this.tmplContactListItem.CanFocus = true;
                    this.tmplContactListItem.DescriptionText = "» " + (myNTContact.PrimaryPhoneNumbers().Length > 64 ? myNTContact.PrimaryPhoneNumbers().Trim().Substring(0, 64) : myNTContact.PrimaryPhoneNumbers().Trim());
                    this.tmplContactListItem.ForeColor = System.Drawing.Color.White;
                    //this.tmplContactListItem.Image = ((System.Drawing.Image)(Properties.Resources.ContactBlank));
                    this.tmplContactListItem.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                    this.tmplContactListItem.Text = "» " + (myNTContact.FullName().Length > 64 ? myNTContact.FullName().Trim().Substring(0, 64) : myNTContact.FullName().Trim());
                    this.tmplContactListItem.Text += myNTContact.NTJabberID.Trim() != "" ? " (" + (myNTContact.NTJabberID.Trim().Length > 64 ? myNTContact.NTJabberID.Trim().Substring(0, 64) : myNTContact.NTJabberID.Trim()) + ")" : "";
                    this.tmplContactListItem.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
                    this.tmplContactListItem.TextSeparatorVisibility = Telerik.WinControls.ElementVisibility.Visible;
                    this.tmplContactListItem.ToolTipText = null;
                    this.tmplContactListItem.Font = new System.Drawing.Font("Trebuchet MS", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                    this.tmplContactListItem.ForeColor = System.Drawing.Color.White;
                    this.tmplContactListItem.DescriptionFont = new System.Drawing.Font("Trebuchet MS", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                    this.tmplContactListItem.TextSeparatorVisibility = Telerik.WinControls.ElementVisibility.Visible;
                    this.tmplContactListItem.DoubleClick += new EventHandler(tmplContactListItem_DoubleClick);
                    this.tmplContactListItem.Tag = myNTContact;
                }
            }
            if (myContactsListBox.Items.Count > 0) myContactsListBox.SelectedIndex = 0;


        }

        void tmplContactListItem_DoubleClick(object sender, EventArgs e)
        {
            Telerik.WinControls.UI.RadListBoxItem clickedItem = (Telerik.WinControls.UI.RadListBoxItem)sender;
            myContactWindow = new ContactsWindow(this, (NTContact)clickedItem.Tag, null);
            myContactWindow.FormClosing += new FormClosingEventHandler(myContactWindow_FormClosing);
            myContactWindow.Show();

        }



        private void DetectNetwork()
        {
            myNetworkInfo.Online = false;
            try
            {
                string HostName = System.Net.Dns.GetHostName();
                IPHostEntry thisHost = System.Net.Dns.GetHostEntry(HostName);

                for (int i = thisHost.AddressList.Length - 1; i >= 0; i--)
                //                for (int i = 0 ; i < thisHost.AddressList.Length; i++)
                {
                    if (!myNetworkInfo.Online)
                    {
                        IPAddress thisIpAddress = thisHost.AddressList[i];
                        if ((!thisIpAddress.ToString().Contains("127.0.0.1")) && (!thisIpAddress.ToString().Contains(":")))
                        {
                            try
                            {
                                Console.WriteLine("NETWORK-Detecting:" + thisIpAddress.ToString());
                                IPAddress remoteHostIP = System.Net.Dns.GetHostEntry(ConfigTestNetwork.HostIp).AddressList[0];
                                IPEndPoint remoteEndPoint = new IPEndPoint(remoteHostIP, ConfigTestNetwork.HostPort);
                                Socket remoteSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                                remoteSocket.Connect(remoteEndPoint);
                                myNetworkInfo.LocalIP = remoteSocket.LocalEndPoint.ToString().Split(new char[1] { ':' }, StringSplitOptions.RemoveEmptyEntries)[0];
                                myNetworkInfo.Online = true;
                                Console.WriteLine("NETWORK-Enabled:" + myNetworkInfo.LocalIP);
                                remoteSocket.Disconnect(false);

                                return;
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("NETWORK-Error:" + thisIpAddress.ToString() + " Error Message:" + ex.Message);
                                //There was a timeout try another interface
                            }
                        }
                    }
                }


            }
            catch (Exception ex)
            {
                return;
            }
        }

        private void LoggingInProgress(bool inProgress)
        {


            if (inProgress)
            {
                myLoginWaitingBar.StartWaiting();
                //  this.Cursor = Cursors.WaitCursor;
            }
            else
            {
                myLoginWaitingBar.EndWaiting();
                //   this.Cursor = Cursors.Default;
            }

            myLoginButton.Enabled = !inProgress;
            myLoginUsernameInput.Enabled = !inProgress;
            myLoginPasswordInput.Enabled = !inProgress;
            myLoginRememberMeCheckBox.Enabled = !inProgress;
            myLoginAutoLoginCheckBox.Enabled = !inProgress;
        }

        private void myLoginButton_Click(object sender, EventArgs e)
        {

            if (!myUserAccount.LoggedIn())
            {

                if (NetworkInterface.GetIsNetworkAvailable())
                {

                    LoggingInProgress(true);

                    try
                    {
                        myClientConfiguration.Username = "";
                        myClientConfiguration.Password = "";
                        myClientConfiguration.RememberMe = false;
                        myClientConfiguration.AutoLogin = false;

                        ClientConfigurationSerializer serializer = new ClientConfigurationSerializer();
                        if (myLoginRememberMeCheckBox.Checked)
                        {
                            myClientConfiguration.RememberMe = true;
                            myClientConfiguration.Username = myLoginUsernameInput.Text;
                            myClientConfiguration.Password = myLoginPasswordInput.Text;

                            if (myLoginAutoLoginCheckBox.Checked)
                            {

                                myClientConfiguration.AutoLogin = true;
                            }
                        }
                        serializer.SaveClientConfiguration(myClientConfiguration);
                    }
                    catch (Exception)
                    {

                        //
                    }

                    if (InitializeClient())
                    {

                        myMainWindowSplitContainer.Panel1Collapsed = true;
                        myMainWindowSplitContainer.Panel2Collapsed = false;

                        //initialize WebContactsBook

                        getContactsBook();
                        getPrepaidStatus();

                        CallHistory tmpCallHistory = myCallHistorySerializer.LoadCallHistory(myUserAccount.Username);
                        if (tmpCallHistory.CallRecords != null)
                        {
                            myCallHistoryRecords.AddRange(tmpCallHistory.CallRecords);
                        }
                    }
                    //open login panel and process login
                    LoggingInProgress(false);
                }
                else
                {
                    myClientNotifyIcon.ShowBalloonTip(10, "IM - Network Unavaliable", "Client functionality will be limited, check your network.", ToolTipIcon.Warning);
                }
            }
        }


        private void xmppControl_OnSubscriptionRequest(object sender, XmppSubscriptionRequestEventArgs e)
        {
            Console.WriteLine("IM-SubscriptionRequest:" + "(" + e.From + ")");
            if (MessageBox.Show("User " + e.From + " would like to add you to his/her contact list.  Do you wish to accept?", "Subscription Request", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
            {
                e.Accept = true;
            }
            if (MessageBox.Show("Would you like to add " + e.From + " to your own contact list?", "Manage Contacts", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
            {
                Hashtable properties = new Hashtable();
                properties.Add("NTJabberID", e.From);
                myContactWindow = new ContactsWindow(this, null, properties);
                myContactWindow.FormClosing += new FormClosingEventHandler(myContactWindow_FormClosing);
                myContactWindow.Show();
            }
        }


        public bool SendMessage(string jabberID, string messageText, string otherData)
        {
            try
            {
                xmppControl.MessageText = messageText.Trim();
                xmppControl.MessageType = XmppMessageTypes.mtChat;
                xmppControl.OtherData = otherData;
                xmppControl.SendMessage(jabberID);
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show("Error sending message: " + e.Message, "Error Sending Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            finally
            {

            }
            return false;
        }



        #region XmppControl Events

        // xmppControl Evevnt handling

        private TreeNode GetNode(TreeNode node, String jabberID)
        {
            if (node != null)
            {
                if (node.Text == jabberID)
                {
                    return node;
                }
                else
                {
                    if (node.FirstNode != null)
                    {
                        TreeNode newnode = GetNode(node.FirstNode, jabberID);
                        if (newnode != null)
                        {
                            return newnode;
                        }

                        else if (node.NextNode != null)
                        {
                            return GetNode(node.NextNode, jabberID);
                        }
                    }
                    else if (node.NextNode != null)
                    {
                        return GetNode(node.NextNode, jabberID);
                    }
                    return null;
                }
            }
            return null;
        }

        private string getCurrentGroup()
        {
            TreeNode node = myRosterListTreeView.SelectedNode;
            if (node == null) return "";

            if (myBuddyGroups.Contains(node.Text))
            {
                return node.Text;
            }
            else if (node.Parent != null)
            {
                if (myBuddyGroups.Contains(node.Parent.Text)) return node.Parent.Text;
            }
            return "";
        }



        private void xmppControl_OnSync(object sender, XmppSyncEventArgs e)
        {

            Console.WriteLine("IM-Sync:");
            try
            {
                myRosterListTreeView.Nodes.Clear();
                myBuddyGroups.Clear();
                myBuddyList.Clear();
                myChatWindow.AddToConversation("LOG", "INFO", "Sync");

                //get all groups
                int i;
                for (i = 1; i <= xmppControl.BuddyCount; i++)
                {
                    try
                    {
                        if (xmppControl.BuddyGroup[i] != "")
                        {
                            foreach (string item in xmppControl.BuddyGroup[i].ToString().Split(Char.Parse(",")))
                            {
                                if (!myBuddyGroups.Contains(item))
                                {
                                    myBuddyGroups.Add(item, item);
                                }

                            }

                        }
                        else
                        {
                            myBuddyGroups.Add("Other", xmppControl.BuddyGroup[i]);
                        }
                    }
                    catch (Exception)
                    {

                    }
                }

                //build roster
                foreach (DictionaryEntry group in myBuddyGroups)
                {

                    TreeNode groupNode;
                    //create groups
                    if (group.Value.ToString() != "")
                    {
                        groupNode = myRosterListTreeView.Nodes.Add(group.Value.ToString());
                    }
                    else
                    { //(some Buddies may not be in a group)
                        groupNode = myRosterListTreeView.Nodes.Add("Other");
                    }

                    groupNode.ImageIndex = 6; //group icon
                    groupNode.SelectedImageIndex = 6;


                    //add buddies to groups
                    for (i = 1; i <= xmppControl.BuddyCount; i++)
                    {


                        foreach (string item in xmppControl.BuddyGroup[i].Split(Char.Parse(",")))
                        {
                            if (group.Value.ToString() == item.ToString())
                            {
                                //format all of the buddyid's properly (strip out domain, resource)

                                String id = xmppControl.BuddyId[i];
                                id = id.Split('@')[0];


                                if (!UpdatedSync)
                                {
                                    UpdatedSync = true;
                                    switch (xmppControl.BuddySubscription[i])
                                    {
                                        case XmppBuddySubscriptions.stNone:
                                            xmppControl.SubscribeTo(id);
                                            break;
                                        case XmppBuddySubscriptions.stTo:
                                            xmppControl.SubscribeTo(id);
                                            break;
                                        case XmppBuddySubscriptions.stFrom:
                                            xmppControl.SubscribeTo(id);
                                            break;
                                        case XmppBuddySubscriptions.stBoth:
                                            break;
                                        case XmppBuddySubscriptions.stRemove:
                                            break;
                                    }

                                }

                                TreeNode newnode = new TreeNode(id);
                                newnode.Tag = id;
                                newnode.ImageIndex = 0;
                                groupNode.Nodes.Add(newnode);
                                groupNode.ImageIndex = 5;
                                groupNode.SelectedImageIndex = 5;



                                if (!myBuddyList.Contains(id))
                                {
                                    myBuddyList.Add(id, xmppControl.BuddyGroup[i]);
                                }
                                //xmppControl.ProbePresence(id);
                            }
                        }



                    }


                }


                myRosterListTreeView.ExpandAll();
            }
            catch (Exception)
            {

                //
            }
        }

        private void xmppControl_OnPresence(object sender, XmppPresenceEventArgs e)
        {
            Console.WriteLine("IM-Presence:" + "(" + e.User + ")" + e.Availability);
            if (!myBuddyPresence.Contains(e.User))
            {
                myBuddyPresence.Add(e.User, e.Availability);
            }
            else
            {
                myBuddyPresence[e.User] = e.Availability;
            }

            //update the roster imageindex for this buddy
            myChatWindow.AddToConversation(e.User, "PRESENCE", e.User + " is " + myChatWindow.PRESENCES[e.Availability]);
            myChatWindow.UpdatePresence(e.User, e.Availability);
            
            TreeNode node = GetNode(myRosterListTreeView.TopNode, e.User);
            if (node != null)
            {
                switch (e.Availability)
                {
                    case 0: node.ImageIndex = 0; node.SelectedImageIndex = 0; break;// 'offline
                    case 1: node.ImageIndex = 1; node.SelectedImageIndex = 1; break;// 'online
                    case 2: node.ImageIndex = 2; node.SelectedImageIndex = 2; break;// 'away
                    case 3: node.ImageIndex = 3; node.SelectedImageIndex = 3; break;// 'extended away
                    case 4: node.ImageIndex = 4; node.SelectedImageIndex = 4; break;// 'Do not disturb
                }
            }
        }


        private void xmppControl_OnBuddyUpdate(object sender, XmppBuddyUpdateEventArgs e)
        {
            Console.WriteLine("IM-BuddyUpdate:" + "(" + e.BuddyIdx.ToString() + ")");
            myChatWindow.AddToConversation(xmppControl.BuddyId[e.BuddyIdx], "BUDDYUPDATE", xmppControl.BuddyId[e.BuddyIdx] + " is " + xmppControl.BuddySubscription[e.BuddyIdx].ToString());
        }

        private void xmppControl_OnMessageIn(object sender, XmppMessageInEventArgs e)
        {
            Console.WriteLine("IM-Message:" + "(" + e.From + ")" + e.MessageText);
            if (e.MessageText != "")
            {
                myChatWindow.AddToConversation(e.From, e.From, e.MessageText);
            }


        }

        private void xmppControl_OnDisconnected(object sender, XmppDisconnectedEventArgs e)
        {
            Console.WriteLine("IM-Disconnected:" + "(" + e.StatusCode + ")" + e.StatusCode);
            if ((e.StatusCode == 0) || (e.StatusCode == 600))
            {
                if ((!xmppControl.Connected) && (myNetworkInfo.Online) && (myUserAccount.LoggedIn()))
                {
                    //network detected, try authenticate at jabber
                    try
                    {
                        Console.WriteLine("IM:Trying to ReConnect");
                        xmppControl.Connect(myUserAccount.Username, myUserAccount.Password);

                    }
                    catch (IPWorksException ipwime)
                    {
                        Console.WriteLine("IM:ReConnecting:(" + ipwime.Code + "):" + ipwime.Message);
                    }
                    if (!xmppControl.Connected)
                    {
                        //try to reconnect every 60 seconds
                        myServiceStateCheckTimer.Enabled = true;
                    }
                }
            }
        }

        private void xmppControl_OnConnected(object sender, XmppConnectedEventArgs e)
        {
            Console.WriteLine("IM-Connected:" + "(" + e.StatusCode + ")" + e.Description);
            myServiceStateCheckTimer.Enabled = false;
        }

        private void xmppControl_OnConnectionStatus(object sender, XmppConnectionStatusEventArgs e)
        {
            Console.WriteLine("IM-ConnectionStatus|" + e.ConnectionEvent + ": (" + e.StatusCode + ") " + e.Description);
        }
        private void xmppControl_OnPITrail(object sender, XmppPITrailEventArgs e)
        {
            Console.WriteLine("IM-PITrail:" + "(" + e.Pi + ")" + e.Direction.ToString());
            //Jabber X-Events are not supported by the component (they are not officially part of the protocol), however:
            if (IsXEvent(e.Pi)) HandleXEvent(e.Pi);

            String from = "";
            switch (e.Direction)
            {
                case 0: from = "CLIENT"; break;
                case 1: from = "SERVER"; break;
                case 2: from = "INFO"; break;
            }
            myChatWindow.AddToConversation("LOG", from, e.Pi);
        }
        #endregion



        #region "Jabber:X:Event"

        private bool IsXEvent(string Pi)
        {
            if (Pi.IndexOf("jabber:x:event") >= 0) return true;
            return false;
        }

        private string GetFromJabberID(string Pi)
        {
            int start = Pi.IndexOf("from=");
            if (start < 0) return xmppControl.User;
            string jabberID = Pi.Substring(start + 6);
            int stopi = jabberID.IndexOf("@");
            jabberID = jabberID.Remove(stopi, jabberID.Length - stopi);
            return jabberID;
        }

        private string GetVideoCallSessionID(string Pi)
        {
            int start = Pi.IndexOf("<id>");
            if (start < 0) return null;
            string sessionID = Pi.Substring(start + 4);
            int stopi = sessionID.IndexOf("</id>");
            sessionID = sessionID.Remove(stopi, sessionID.Length - stopi);
            return sessionID;
        }

        private void HandleXEvent(string Pi)
        {


            //Supported XEvents
            //composing - user typing notifications
            //video-invite - video conference invite
            //video-accept - accept, for previous video conference invite
            //video-cancel - cancel, for previous video conference invite



            string jabberID = GetFromJabberID(Pi);
            if (jabberID == xmppControl.User) return;

            //composing
            if ((Pi.IndexOf("<composing") >= 0) && (Pi.IndexOf("<body") >= 0))
            {
                //if there is a composing tag and a body tag - then this is a regular message
                //buddy is not composing
                Console.WriteLine("IM-XEvent|" + jabberID + " sent the message");
                myChatWindow.SetComposing(false, jabberID);
            }
            else if (Pi.IndexOf("<composing") >= 0)
            {
                //if there is a composing tag and NOT a body tag - then this is a composing message
                //buddy is composing
                Console.WriteLine("IM-XEvent|" + jabberID + " is composing");
                myChatWindow.SetComposing(true, jabberID);
            }



            //video-invite
            if (Pi.IndexOf("<video-invite") >= 0)
            {
                myChatWindow.AddToConversation(jabberID, "INVITE", jabberID + " invites you for video call");
                string VideoSessionID = GetVideoCallSessionID(Pi);
                Console.WriteLine("IM-XEvent| Video Call Invite - " + VideoSessionID);
                VideoSessionProcessInvite(jabberID, VideoSessionID);
            }


            //video-accept
            if (Pi.IndexOf("<video-accept") >= 0)
            {
                myChatWindow.AddToConversation(jabberID, "INVITE", jabberID + " accepted your invitation for video call");
                string VideoSessionID = GetVideoCallSessionID(Pi);
                Console.WriteLine("IM-XEvent| Video Call Accept - " + VideoSessionID);
                VideoSessionProcessAccept(jabberID, VideoSessionID);

            }

            //video-deny
            if (Pi.IndexOf("<video-cancel") >= 0)
            {
                myChatWindow.AddToConversation(jabberID, "INVITE", jabberID + " denied your invitation for video call");
                string VideoSessionID = GetVideoCallSessionID(Pi);
                Console.WriteLine("IM-XEvent| Video Call Cancel - " + VideoSessionID);
                VideoSessionProcessCancel(jabberID);
            }
        }
        #endregion

        #region VideoCall Processing

        public void VideoSessionConnect(string jabberID, string videoSessionID)
        {
            myVideoPlugin.StartConference(true, 0, 0, 0, ConfigVideoProxy.ProxyAddress, false, true, videoSessionID, DateTime.Now.ToString("yyyyMMdd|HHmmss|fff") + ";" + myUserAccount.Username, jabberID);
        }

        public void VideoSessionInvite(string jabberID)
        {
            string VideoSessionID = DateTime.Now.ToString("yyyyMMdd|hhmmss|fff") + "@" + myUserAccount.Username + ";" + jabberID;
            myVideoPlugin.Show(true);
            string jabberXevent = "<x xmlns='jabber:x:event'><video-invite/><id>" + VideoSessionID + "</id></x>";
            bool msgSent = SendMessage(jabberID, "", jabberXevent);
        }

        public void VideoSessionProcessInvite(string jabberID, string VideoSessionID)
        {
            if (MessageBox.Show("User " + jabberID + " would like to invite you for video call.  Do you wish to accept?", "Video Call Invitation", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
            {
                string jabberXevent = "<x xmlns='jabber:x:event'><video-accept/><id>" + VideoSessionID + "</id></x>";
                bool msgSent = SendMessage(jabberID, "", jabberXevent);
                VideoSessionConnect(jabberID, VideoSessionID);

            }
            else
            {
                string jabberXevent = "<x xmlns='jabber:x:event'><video-cancel/><id>" + VideoSessionID + "</id></x>";
                bool msgSent = SendMessage(jabberID, "", jabberXevent);
            }

        }
        public void VideoSessionProcessAccept(string jabberID, string VideoSessionID)
        {
            VideoSessionConnect(jabberID, VideoSessionID);
            StartNewCall(-1, jabberID);
        }


        public void VideoSessionProcessCancel(string jabberID)
        {
            //do nothing
        }

        public void VideoSessionDisconnect(string jabberID)
        {
            myVideoPlugin.StopConference();

        }
        #endregion



        #region xmpp change presence status
        private void myGoOfflineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            xmppControl.ChangePresence(0, myChatWindow.PRESENCES[0]);
            myPresenceStripSplitButton.Image = myGoOfflineToolStripMenuItem.Image;
        }

        private void myGoOnlineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            xmppControl.ChangePresence(1, myChatWindow.PRESENCES[1]);
            myPresenceStripSplitButton.Image = myGoOnlineToolStripMenuItem.Image;
        }

        private void myGoAwayToolStripMenuItem_Click(object sender, EventArgs e)
        {
            xmppControl.ChangePresence(2, myChatWindow.PRESENCES[2]);
            myPresenceStripSplitButton.Image = myGoAwayToolStripMenuItem.Image;
        }

        private void myGoExtendedAwayToolStripMenuItem_Click(object sender, EventArgs e)
        {
            xmppControl.ChangePresence(3, myChatWindow.PRESENCES[3]);
            myPresenceStripSplitButton.Image = myGoExtendedAwayToolStripMenuItem.Image;
        }

        private void myGoDoNotDisturbToolStripMenuItem_Click(object sender, EventArgs e)
        {
            xmppControl.ChangePresence(4, myChatWindow.PRESENCES[4]);
            myPresenceStripSplitButton.Image = myGoDoNotDisturbToolStripMenuItem.Image;
        }
        #endregion





        void myContactWindow_FormClosing(object sender, FormClosingEventArgs e)
        {

            try
            {


                ContactsWindow tmpContactWindow = (ContactsWindow)sender;

                if (tmpContactWindow != null)
                {
                    if (tmpContactWindow.Accepted)
                    {
                        myContactsBook.Save();
                        LoadContactsBook(myContactsBook.List);

                        if (tmpContactWindow.myContactJabberIDListBox.Text != "")
                        {
                            if (myBuddyList[tmpContactWindow.myContactJabberIDListBox.Text] == null)
                            {
                                //buddy not listed yet
                                xmppControl.Add(tmpContactWindow.myContactJabberIDListBox.Text, tmpContactWindow.myContactJabberAliasInput.Text, tmpContactWindow.myContactJabberGroupListBox.Text);
                                xmppControl.SubscribeTo(tmpContactWindow.myContactJabberIDListBox.Text);
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {

                //            throw;
            }
        }

        private void myPhoneLinesTabControl_TabSelected(object sender, Telerik.WinControls.UI.TabEventArgs args)
        {
            UpdatePhoneLineButtons();
        }

        private void myRosterListTreeView_DoubleClick(object sender, EventArgs e)
        {
            if (myRosterListTreeView.SelectedNode != null)
            {
                if (myRosterListTreeView.SelectedNode.Level == 1)
                {
                    StartNewIM(myRosterListTreeView.SelectedNode.Text, true);
                }
            }
        }

        private void sendMessageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (myRosterListTreeView.SelectedNode != null)
            {
                if (myRosterListTreeView.SelectedNode.Level == 1)
                {

                    StartNewIM(myRosterListTreeView.SelectedNode.Text, true);

                }
            }
        }

        private void callToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (myRosterListTreeView.SelectedNode != null)
            {
                if (myRosterListTreeView.SelectedNode.Level == 1)
                {

                    StartNewCall(-1, myRosterListTreeView.SelectedNode.Text);

                }
            }
        }

        private void myRosterListTreeView_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            if (e.Node.Level == 0)
            {
                myRosterContextMenuStrip.Enabled = false;
                myMenuChatToolStripButton.Enabled = false;
                myMenuCallToolStripButton.Enabled = false;
                myMenuPhone2PhoneCallToolStripButton.Enabled = false;
                myMenuVideoCallToolStripButton.Enabled = false;
            }
            else
            {
                myRosterContextMenuStrip.Enabled = true;
                myMenuChatToolStripButton.Enabled = true;
                myMenuCallToolStripButton.Enabled = true;
                myMenuPhone2PhoneCallToolStripButton.Enabled = true;
                myMenuVideoCallToolStripButton.Enabled = true;
            }
        }

        private void viewContactPropertiesToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (myRosterListTreeView.SelectedNode != null)
            {
                if (myRosterListTreeView.SelectedNode.Level == 1)
                {
                    Hashtable properties = new Hashtable();
                    properties.Add("NTJabberID", myRosterListTreeView.SelectedNode.Text);

                    myContactWindow = new ContactsWindow(this, myContactsBook.List.getCandidatesForJabberID(myRosterListTreeView.SelectedNode.Text).Count > 0 ? (NTContact)myContactsBook.List.getCandidatesForJabberID(myRosterListTreeView.SelectedNode.Text)[0] : null, properties);
                    myContactWindow.FormClosing += new FormClosingEventHandler(myContactWindow_FormClosing);
                    myContactWindow.Show();
                }
            }
        }

        private void myLoginForgotPasswordLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                ProcessStartInfo startProcess = new ProcessStartInfo();
                startProcess.FileName = ConfigWebLinks.ResetPasswordLink;
                startProcess.UseShellExecute = true;
                Process.Start(startProcess);
            }
            catch (Exception)
            {

                ;
            }
        }

        private void myLoginCreateAccountLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                ProcessStartInfo startProcess = new ProcessStartInfo();
                startProcess.FileName = ConfigWebLinks.RegistrationLink;
                startProcess.UseShellExecute = true;
                Process.Start(startProcess);
            }
            catch (Exception)
            {

                ;
            }
        }
        #region Contacts List context menu events
        private void sendMessageContactsToolStripMenuItem_Click(object sender, EventArgs e)
        {

            RadListBoxItem selectedItem = (RadListBoxItem)myContactsListBox.SelectedItem;
            if (selectedItem != null)
            {
                NTContact selectedContact = (NTContact)selectedItem.Tag;

                StartNewIM(selectedContact.NTJabberID, true);
            }
        }

        private void callComputerContactsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RadListBoxItem selectedItem = (RadListBoxItem)myContactsListBox.SelectedItem;
            if (selectedItem != null)
            {
                NTContact selectedContact = (NTContact)selectedItem.Tag;
                StartNewCall(-1, selectedContact.NTJabberID);
            }
        }

        private void callMobileContactsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RadListBoxItem selectedItem = (RadListBoxItem)myContactsListBox.SelectedItem;
            if (selectedItem != null)
            {
                NTContact selectedContact = (NTContact)selectedItem.Tag;
                StartNewCall(-1, selectedContact.NTMobileTelephoneNumber);
            }
        }

        private void callHomeContactsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RadListBoxItem selectedItem = (RadListBoxItem)myContactsListBox.SelectedItem;
            if (selectedItem != null)
            {
                NTContact selectedContact = (NTContact)selectedItem.Tag;
                StartNewCall(-1, selectedContact.NTHomeTelephoneNumber);
            }
        }

        private void callWorkContactsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RadListBoxItem selectedItem = (RadListBoxItem)myContactsListBox.SelectedItem;
            if (selectedItem != null)
            {
                NTContact selectedContact = (NTContact)selectedItem.Tag;
                StartNewCall(-1, selectedContact.NTBusinessTelephoneNumber);
            }
        }

        private void callVoIPContactsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RadListBoxItem selectedItem = (RadListBoxItem)myContactsListBox.SelectedItem;
            if (selectedItem != null)
            {
                NTContact selectedContact = (NTContact)selectedItem.Tag;
                StartNewCall(-1, selectedContact.NTVoIPTelephoneNumber);
            }
        }

        private void viewContactContactsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RadListBoxItem selectedItem = (RadListBoxItem)myContactsListBox.SelectedItem;
            if (selectedItem != null)
            {
                NTContact selectedContact = (NTContact)selectedItem.Tag;

                myContactWindow = new ContactsWindow(this, selectedContact, null);
                myContactWindow.FormClosing += new FormClosingEventHandler(myContactWindow_FormClosing);
                myContactWindow.Show();
            }
        }

        #endregion

        private void myContactsListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            RadListBoxItem selectedItem = (RadListBoxItem)myContactsListBox.SelectedItem;
            if (selectedItem != null)
            {
                NTContact selectedContact = (NTContact)selectedItem.Tag;
                if (selectedContact != null)
                {
                    sendMessageContactsToolStripMenuItem.Enabled = selectedContact.NTJabberID.Length > 0 ? true : false;
                    callComputerContactsToolStripMenuItem.Enabled = selectedContact.NTJabberID.Length > 0 ? true : false;
                    callHomeContactsToolStripMenuItem.Enabled = selectedContact.NTHomeTelephoneNumber.Length > 0 ? true : false;
                    callMobileContactsToolStripMenuItem.Enabled = selectedContact.NTMobileTelephoneNumber.Length > 0 ? true : false;
                    callWorkContactsToolStripMenuItem.Enabled = selectedContact.NTBusinessTelephoneNumber.Length > 0 ? true : false;
                    callVoIPContactsToolStripMenuItem.Enabled = selectedContact.NTVoIPTelephoneNumber.Length > 0 ? true : false;
                }

            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }



        private void myContactsShowAllButton_Click(object sender, EventArgs e)
        {

            if (myContactsBook != null)
            {
                LoadContactsBook(myContactsBook.List);
            }
        }

        private void myContactsShow1ABButton_Click(object sender, EventArgs e)
        {
            LoadContactsBook(myContactsBook.List.getCandidatesForName(new string[] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "0", "A", "B" }));
        }

        private void myContactsShowCDEButton_Click(object sender, EventArgs e)
        {
            LoadContactsBook(myContactsBook.List.getCandidatesForName(new string[] { "C", "D", "E" }));
        }

        private void myContactsShowFGHButton_Click(object sender, EventArgs e)
        {
            LoadContactsBook(myContactsBook.List.getCandidatesForName(new string[] { "F", "G", "H" }));
        }

        private void myContactsShowIJKButton_Click(object sender, EventArgs e)
        {
            LoadContactsBook(myContactsBook.List.getCandidatesForName(new string[] { "I", "J", "K" }));
        }

        private void myContactsShowLMNlButton_Click(object sender, EventArgs e)
        {
            LoadContactsBook(myContactsBook.List.getCandidatesForName(new string[] { "L", "M", "N" }));
        }

        private void myContactsShowOPQButton_Click(object sender, EventArgs e)
        {
            LoadContactsBook(myContactsBook.List.getCandidatesForName(new string[] { "O", "P", "Q" }));
        }

        private void myContactsShowRSTButton_Click(object sender, EventArgs e)
        {
            LoadContactsBook(myContactsBook.List.getCandidatesForName(new string[] { "R", "S", "T" }));
        }

        private void myContactsShowUVWButton_Click(object sender, EventArgs e)
        {
            LoadContactsBook(myContactsBook.List.getCandidatesForName(new string[] { "U", "V", "W" }));
        }

        private void myContactsShowXYZButton_Click(object sender, EventArgs e)
        {
            LoadContactsBook(myContactsBook.List.getCandidatesForName(new string[] { "X", "Y", "Z" }));
        }

        private void myContactsToolStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            try
            {
                foreach (object item in myContactsToolStrip.Items)
                {
                    if (((ToolStripButton)item).Name != e.ClickedItem.Name)
                    {
                        ((ToolStripButton)item).Checked = false;
                    }
                }
            }
            catch (Exception)
            {

                ;
            }

            myContactsShowAll();

        }

        private void myLoginWaitingBar_WaitingStarted(object sender, EventArgs e)
        {
            myLoginWaitingBar.Visible = true;
        }

        private void myLoginWaitingBar_WaitingEnded(object sender, EventArgs e)
        {
            myLoginWaitingBar.Visible = false;
        }

        private void myPrepaidAmountToolStripLabel_Click(object sender, EventArgs e)
        {
            getPrepaidStatus();
        }


        private void LoadCallsHistory()
        {
            myCallHistoryListBox.Items.Clear();


            foreach (CallRecord record in myCallHistoryRecords)
            {

                if (record.NumberOrUsername.Trim() != "")
                {
                    this.tmplCallRecordListItem = new Telerik.WinControls.UI.RadListBoxItem();

                    this.myCallHistoryListBox.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.tmplCallRecordListItem});

                    // 
                    // tmplContactListItem
                    // 
                    this.tmplCallRecordListItem.AccessibleDescription = record.NumberOrUsername;
                    this.tmplCallRecordListItem.CanFocus = true;
                    this.tmplCallRecordListItem.DescriptionText = record.CallDateTime.ToShortDateString() + " " + record.CallDateTime.ToShortTimeString() + " | (" + SecondsToHH24MISS(record.CallDuration) + ")";
                    this.tmplCallRecordListItem.ForeColor = System.Drawing.Color.White;
                    this.tmplCallRecordListItem.Image = ((System.Drawing.Image)(Properties.Resources.ResourceManager.GetObject(record.CallState.ToString())));
                    this.tmplCallRecordListItem.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                    this.tmplCallRecordListItem.Text = record.NumberOrUsername;
                    this.tmplCallRecordListItem.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
                    this.tmplCallRecordListItem.TextSeparatorVisibility = Telerik.WinControls.ElementVisibility.Visible;
                    this.tmplCallRecordListItem.ToolTipText = null;
                    this.tmplCallRecordListItem.Font = new System.Drawing.Font("Trebuchet MS", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                    this.tmplCallRecordListItem.ForeColor = System.Drawing.Color.White;
                    this.tmplCallRecordListItem.DescriptionFont = new System.Drawing.Font("Trebuchet MS", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                    this.tmplCallRecordListItem.TextSeparatorVisibility = Telerik.WinControls.ElementVisibility.Visible;
                    //this.tmplCallRecordListItem.DoubleClick += new EventHandler(tmplContactListItem_DoubleClick);
                    this.tmplCallRecordListItem.Tag = record;
                }
            }
            if (myCallHistoryListBox.Items.Count > 0) myCallHistoryListBox.SelectedIndex = 0;
        }

        private void radTabStrip1_TabSelected(object sender, TabEventArgs args)
        {
            if (args.TabItem == tabItem2)
            {
                LoadCallsHistory();

            }
        }

        private void myCallHistory_CallToolStripMenuItem_Click(object sender, EventArgs e)
        {


            RadListBoxItem selectedItem = (RadListBoxItem)myCallHistoryListBox.SelectedItem;
            if (selectedItem != null)
            {
                CallRecord selectedCallRecord = (CallRecord)selectedItem.Tag;
                if (selectedCallRecord != null)
                {
                    StartNewCall(-1, selectedCallRecord.NumberOrUsername);
                }

            }


        }

        private void myCallHistory_AddContactToolStripItem_Click(object sender, EventArgs e)
        {
            RadListBoxItem selectedItem = (RadListBoxItem)myCallHistoryListBox.SelectedItem;
            if (selectedItem != null)
            {
                CallRecord selectedCallRecord = (CallRecord)selectedItem.Tag;
                Hashtable properties = new Hashtable();
                if (selectedCallRecord != null)
                {
                    int dummy;
                    if (Int32.TryParse(selectedCallRecord.NumberOrUsername, out dummy))
                    {
                        properties.Add("NTHomeTelephoneNumber", selectedCallRecord.NumberOrUsername);
                    }
                    else
                    {
                        properties.Add("NTJabberID", selectedCallRecord.NumberOrUsername);
                    }

                    myContactWindow = new ContactsWindow(this, null, properties);
                    myContactWindow.FormClosing += new FormClosingEventHandler(myContactWindow_FormClosing);
                    myContactWindow.Show();
                }

            }
        }

        private void myCallHistory_DeleteToolStripItem_Click(object sender, EventArgs e)
        {
            RadListBoxItem selectedItem = (RadListBoxItem)myCallHistoryListBox.SelectedItem;
            if (selectedItem != null)
            {
                CallRecord selectedCallRecord = (CallRecord)selectedItem.Tag;
                if (selectedCallRecord != null)
                {
                    myCallHistoryRecords.Remove(selectedCallRecord);
                    selectedItem.Dispose();
                    LoadCallsHistory();
                }

            }
        }
        private string SecondsToHH24MISS(decimal myDuration)
        {
            decimal Hours = Math.Floor(myDuration / 3600);
            decimal Minutes = Math.Floor((myDuration % 3600) / 60);
            decimal Seconds = myDuration % 60;
            return Hours.ToString("00") + ":" + Minutes.ToString("00") + ":" + Seconds.ToString("00");

        }
        private void myOneSecondTickTimer_Tick(object sender, EventArgs e)
        {
            string tmpCallDuration = "00:00:00";
            int PhoneLineID = 0;

            foreach (LINE_STATE line in mySIPPhoneLines)
            {
                line.Tick();
                //update timer on screen for current selected line
            }
            if (Int32.TryParse(myPhoneLinesTabControl.SelectedTab.Tag.ToString(), out PhoneLineID))
            {
                tmpCallDuration = SecondsToHH24MISS(mySIPPhoneLines[PhoneLineID].CallDuration);
                if (mySIPPhoneLines[PhoneLineID].State == TELEPHONY_RETURN_VALUE.SipOkToAnswerCall)
                {
                    myDialPadCallOrAnswerButton.Select();
                    myDialPadCallOrAnswerButton.Image.RotateFlip(RotateFlipType.RotateNoneFlipY);
                    myDialPadCallOrAnswerButton.Refresh();
                    Thread current = Thread.CurrentThread;                    
                    Thread.Sleep(200);
                    myDialPadCallOrAnswerButton.Image.RotateFlip(RotateFlipType.RotateNoneFlipY);
                    myDialPadCallOrAnswerButton.Refresh();

                }
            }
            myDialPadInfoCallDurationLabel.Text = tmpCallDuration;
        }

        private void deleteContactContactsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RadListBoxItem selectedItem = (RadListBoxItem)myContactsListBox.SelectedItem;
            if (selectedItem != null)
            {
                NTContact selectedContact = (NTContact)selectedItem.Tag;
                selectedContact.NTDeleted = "true";
                LoadContactsBook(myContactsBook.List);
            }
        }

        private string NormalizePhoneNumber(string myPhoneNumber)
        {

            //remove special characters
            Regex _Regex1 = new Regex("([^a-z0-9._-p#*])+", RegexOptions.Singleline);
            myPhoneNumber = _Regex1.Replace(myPhoneNumber.ToLower(), "");

            //remove non alpha and non number form begining
            Regex _Regex2 = new Regex("([^a-z0-9])+", RegexOptions.Singleline);
            while ((myPhoneNumber.Length > 0) && (_Regex2.IsMatch(myPhoneNumber.Substring(0, 1))))
            {
                myPhoneNumber = myPhoneNumber.Remove(0, 1);
            }


            //format phone number
            Regex _Regex3 = new Regex("([^0-9p#*])+", RegexOptions.Singleline);
            if (myPhoneNumber.Length > 0)
            {
                int dummy;
                if (Int32.TryParse(myPhoneNumber.Substring(0, 1), out dummy))
                {
                    //number so remove all non  numeric
                    myPhoneNumber = _Regex3.Replace(myPhoneNumber, "");
                }
            }


            return myPhoneNumber;
        }

        private void myDestinationTextBox_TextChanged(object sender, EventArgs e)
        {
            int curPosition = myDestinationTextBox.SelectionStart;
            myDestinationTextBox.Text = NormalizePhoneNumber(myDestinationTextBox.Text);
            if (myDestinationTextBox.Text.Length > curPosition)
            {
                myDestinationTextBox.SelectionStart = curPosition;
            }
            else
            {
                myDestinationTextBox.SelectionStart = myDestinationTextBox.Text.Length;
            }
        }


        void myPhone2PhoneWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (myPhone2PhoneWindow.StartCall)
            {
                string result = "-1";
                try
                {
                    RSIFeaturesWS.Service nikotalkWS = new Remwave.Client.RSIFeaturesWS.Service(myClientConfiguration.RSIUrl);
                    result = nikotalkWS.clientCallNow(myUserAccount.Username, myUserAccount.Password, myPhone2PhoneWindow.CallFrom, myPhone2PhoneWindow.CallTo);
                }
                catch (Exception)
                {


                }
                if (result == "0")
                {
                    myClientNotifyIcon.ShowBalloonTip(10, "Phone 2 Phone Call", "Trying call  : " + myPhone2PhoneWindow.CallFrom + " to " + myPhone2PhoneWindow.CallTo, ToolTipIcon.Info);
                }
                else   if (result == "2")
                {
                    myClientNotifyIcon.ShowBalloonTip(10, "Phone 2 Phone Call", "Unable to start call - Payment Required : " + myPhone2PhoneWindow.CallFrom + " to " + myPhone2PhoneWindow.CallTo, ToolTipIcon.Error);
                }
                else
                {
                    myClientNotifyIcon.ShowBalloonTip(10, "Phone 2 Phone Call", "Unable to start call : " + myPhone2PhoneWindow.CallFrom + " to " + myPhone2PhoneWindow.CallTo, ToolTipIcon.Error);
                }
            }
        }

        private void callPhone2PhoneToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            RadListBoxItem selectedItem = (RadListBoxItem)myContactsListBox.SelectedItem;
            if (selectedItem != null)
            {
                NTContact selectedContact = (NTContact)selectedItem.Tag;
                myPhone2PhoneWindow = new Phone2PhoneWindow(selectedContact);
                myPhone2PhoneWindow.FormClosing += new FormClosingEventHandler(myPhone2PhoneWindow_FormClosing);
                myPhone2PhoneWindow.Show();
            }
        }

        private void myMenuVideoCallToolStripButton_Click(object sender, EventArgs e)
        {
            if (myRosterListTreeView.SelectedNode != null)
            {
                if (myRosterListTreeView.SelectedNode.Level == 1)
                {
                    VideoSessionInvite(myRosterListTreeView.SelectedNode.Text);
                }
            }
        }





        private void myClientNotifyIcon_BalloonTipClicked(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
            this.Activate();
        }

        private void myClientNotifyIcon_DoubleClick(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
            this.Activate();
        }

        private void myMenuAddContactToolStripButton_Click(object sender, EventArgs e)
        {
            myContactWindow = new ContactsWindow(this, null, null);
            myContactWindow.FormClosing += new FormClosingEventHandler(myContactWindow_FormClosing);
            myContactWindow.Show();
        }

        private void xmppControl_OnError(object sender, XmppErrorEventArgs e)
        {
            Console.WriteLine("IM-Error:" + "(" + e.ErrorCode + ")" + e.Description);
        }

        private void xmppControl_OnIQ(object sender, XmppIQEventArgs e)
        {
            Console.WriteLine("IM-IQ:" + "(" + e.Iq + ")");
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (myRosterListTreeView.SelectedNode != null)
            {
                if (myRosterListTreeView.SelectedNode.Level == 1)
                {

                    try
                    {
                        xmppControl.Remove(myRosterListTreeView.SelectedNode.Text, "", "");
                        xmppControl.Remove(myRosterListTreeView.SelectedNode.Text + "@" + ConfigIM.IMServer, "", "");
                        xmppControl.Remove(myRosterListTreeView.SelectedNode.Text + "@" + ConfigIM.IMServer, "", myRosterListTreeView.SelectedNode.Parent.Text);
                        xmppControl.UnsubscribeTo(myRosterListTreeView.SelectedNode.Text);
                    }
                    catch (Exception)
                    {


                    }



                }
            }
        }

        private void myServiceStateCheckTimer_Tick(object sender, EventArgs e)
        {
            if ((!xmppControl.Connected) && (myNetworkInfo.Online) && (myUserAccount.LoggedIn()))
            {
                //network detected, try authenticate at jabber
                try
                {
                    Console.WriteLine("IM:Trying to ReConnect");
                    xmppControl.Connect(myUserAccount.Username, myUserAccount.Password);
                }
                catch (IPWorksException ipwime)
                {
                    Console.WriteLine("IM:ReConnecting:(" + ipwime.Code + "):" + ipwime.Message);
                }
            }
        }

        private void myMenuPhone2PhoneCallToolStripButton_Click(object sender, EventArgs e)
        {
            NTContact selectedContact;
            if (myRosterListTreeView.SelectedNode != null)
            {
                if (myRosterListTreeView.SelectedNode.Level == 1)
                {

                    ContactList foundContactsList = myContactsBook.List.getCandidatesForJabberID(myRosterListTreeView.SelectedNode.Text);
                    if (foundContactsList.Count > 0)
                    {
                        selectedContact = (NTContact)foundContactsList[0];
                        myPhone2PhoneWindow = new Phone2PhoneWindow(selectedContact);
                        myPhone2PhoneWindow.FormClosing += new FormClosingEventHandler(myPhone2PhoneWindow_FormClosing);
                        myPhone2PhoneWindow.Show();

                    }
                }
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {

            getPrepaidStatus();
        }

        private void myMainTabControl_TabSelected(object sender, TabEventArgs args)
        {
            myOneSecondTickTimer.Enabled = false;
            if (args.TabItem == myMainClientTabPage)
            {

                if (DialogResult.Yes == MessageBox.Show("Would you like to open My Account area in your browser?", "Client Website", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                {

                    try
                    {


                        ProcessStartInfo startProcess = new ProcessStartInfo();
                        startProcess.FileName = ConfigWebLinks.MyAccountLink;
                        startProcess.UseShellExecute = true;
                        Process.Start(startProcess);
                    }
                    catch (Exception)
                    {

                        //                    throw;
                    }
                }

            }
            else if (args.TabItem == myMainContactsTabPage)
            {

                myContactsShowAllButton.PerformClick();
            }
            else if (args.TabItem == myMainDialPadTabPage)
            {
                myOneSecondTickTimer.Enabled = true;
                myDestinationTextBox.Focus();
            }
        }

        private void myMainMenuAccountItem_Click(object sender, EventArgs e)
        {
            if (DialogResult.Yes == MessageBox.Show("Would you like to open My Account area in your browser?", "REMWAVE Website", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
            {

                try
                {


                    ProcessStartInfo startProcess = new ProcessStartInfo();
                    startProcess.FileName = ConfigWebLinks.MyAccountLink;
                    startProcess.UseShellExecute = true;
                    Process.Start(startProcess);
                }
                catch (Exception)
                {

                    //                throw;
                }
            }
        }

        private void myMainMenuExitItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void myMainMenuSignOutItem_Click(object sender, EventArgs e)
        {

            //logout user
            //switch panels
            //disconnect xmppp
            //shutdown engine


            myUserAccount.Logout();

            //open login panel and process login
            myMainWindowSplitContainer.Panel1Collapsed = false;
            myMainWindowSplitContainer.Panel2Collapsed = true;



            xmppControl.Disconnect();
            mySIPPhone.ShutdownEngine();



        }

        private void myDialPadRadButtonDel_Click(object sender, EventArgs e)
        {
            int curPosition = myDestinationTextBox.SelectionStart;
            if (curPosition == 0) { curPosition = myDestinationTextBox.Text.Length; };

            if (!myDestinationTextBox.Focused) { myDestinationTextBox.Focus(); }

            if (myDestinationTextBox.Text.Length > 0)
            {
                myDestinationTextBox.Text = myDestinationTextBox.Text.Remove(curPosition - 1, 1);
                if (myDestinationTextBox.Text.Length > curPosition)
                {
                    myDestinationTextBox.SelectionStart = curPosition - 1;
                }
                else
                {
                    myDestinationTextBox.SelectionStart = myDestinationTextBox.Text.Length;
                }
            }
        }

        private void myMainMenuAboutItem_Click(object sender, EventArgs e)
        {
            //Initialize ChatWindow
            AboutBox myAboutBox = new Remwave.Client.AboutBox();
            myAboutBox.Show();
        }

        private void startVideoToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (myRosterListTreeView.SelectedNode != null)
            {
                if (myRosterListTreeView.SelectedNode.Level == 1)
                {
                    VideoSessionInvite(myRosterListTreeView.SelectedNode.Text);
                }
            }
        }

        private void lnChangeSettings_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ConfigureClient(false);
        }


        private void ShowHideSpeedDialWindow()
        {

            if (mySpeedDialWindow == null)
            {
                mySpeedDialWindow = new SpeedDialWindow(this);
            }
            mySpeedDialWindow.ShowHide();
        }

        private void myClientNotifyIcon_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
            this.Activate();
        }

        private void getContactsBook()
        {
            ThreadStart st = new ThreadStart(getContactsBookFunction);
            getContactsBookThread = new Thread(st);
            // start the thread
            getContactsBookThread.Start();




        }


        private void getContactsBookFunction()
        {
            try
            {
                setContactBook(new WEBPhoneBook(myUserAccount));
            }
            catch (Exception)
            {

                ;
            }
            Thread.CurrentThread.Abort();
        }

        private delegate void setContactBookDelegate(WEBPhoneBook webPhoneBook);

        private void setContactBook(WEBPhoneBook webPhoneBook)
        {

            if (this.InvokeRequired)
            {
                BeginInvoke(new setContactBookDelegate(setContactBook), new object[] { webPhoneBook });
            }
            else
            {
                myContactsBook = webPhoneBook;
            }

        }

        private void getPrepaidStatus()
        {
            myPrepaidAmountRefreshToolStripLabel.Visible = false;

            try
            {


                Remwave.Client.RSIFeaturesWS.Service ss = new Remwave.Client.RSIFeaturesWS.Service(myClientConfiguration.RSIUrl);

                ss.clientPrepaidAmountCompleted += new Remwave.Client.RSIFeaturesWS.clientPrepaidAmountCompletedEventHandler(ss_clientPrepaidAmountCompleted);
                ss.clientPrepaidAmountAsync(myUserAccount.Username, myUserAccount.Password);

            }
            catch (Exception)
            {
                ;
            }

        }

        void ss_clientPrepaidAmountCompleted(object sender, Remwave.Client.RSIFeaturesWS.clientPrepaidAmountCompletedEventArgs e)
        {
            try
            {
                myPrepaidAmountToolStripLabel.Text = e.Result.ToString();
            }
            catch (Exception)
            {
                myPrepaidAmountToolStripLabel.Text = "0.00";
                ;
            }
            myPrepaidAmountRefreshToolStripLabel.Visible = true;
        }



        private void myContactsShowAll()
        {
            if (myContactsBook != null)
            {
                LoadContactsBook(myContactsBook.List);
            }
        }  


    }
}
