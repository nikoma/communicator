using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Remwave_RVoIPLib;
using Remwave.Services;
using System.Collections;
using System.Net;
using System.IO;
using System.Diagnostics;
using System.Net.NetworkInformation;
using System.Text.RegularExpressions;
using Telerik.WinControls.UI;
using Remwave.Client.Serializable;
using System.Runtime.InteropServices;
using System.Threading;
using System.Net.Sockets;
using Telerik.WinControls;
using nsoftware.IPWorksSSL;
using Remwave.Client.Controls;
using Remwave.Client.Events;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Resources;
using System.Globalization;
using Microsoft.Win32;

namespace Remwave.Client
{
    public partial class ClientForm : Form
    {
        #region Localization
        private String mCultureInfoRegistryKey = @"SOFTWARE\" + Application.CompanyName + @"\" + Application.ProductName;


        #endregion

        #region public

        internal static String AppDataDir = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\" + Application.CompanyName + @"\" + ConfigBrand.ProductDirectoryName;
        internal SIPPhone mSIPPhone = new SIPPhone();
        internal UserAccount mUserAccount = new UserAccount();
        internal NetworkInfo myNetworkInfo = new NetworkInfo();
        internal ClientConfiguration myClientConfiguration = new ClientConfiguration();
        internal Hashtable myBuddyGroups = new Hashtable();
        internal Hashtable myBuddyList = new Hashtable();
        internal Hashtable myBuddyPresence = new Hashtable();
        internal Hashtable myFileTransfer = new Hashtable();
        internal ContactBook mContactBook = null;

        internal ClientEvents mClientEvents = null;

        internal bool mFormIsClosing = false;
        internal Size mClientSize = new Size(300, 480);

        internal XMPPIQ mXMPPIQ;

        Thread probePresenceThread;

        internal String[] PRESENCES;

        #endregion

        #region private

        private bool mUpdatedSync = false;
        private bool mReloadContactsList = false;
        private int mAltKeyHitCount = 0;
        private long mAltKeyLastTick = -1;
        private bool showApp = false;
        private DateTime mServiceDiscoveryTime = DateTime.Now;
        private XMPPIQ.PresenceShow mPresenceShow = XMPPIQ.PresenceShow.offline;

        //Window Forms
        private Remwave.Client.ChatWindow myChatWindow;
        private Remwave.Client.ContactsWindow myContactWindow;
        private Remwave.Client.ContactsSearch myContactsSearchWindow;
        private Remwave.Client.Phone2PhoneWindow myPhone2PhoneWindow;
        private Remwave.Client.SpeedDialWindow mySpeedDialWindow;
        internal Remwave.Client.ArchiveWindow myArchiveWindow;
        internal Remwave.Client.SettingsWindow mySettingsWindow;
        internal Remwave.Client.AboutBox myAboutBoxWindow;
#if !NONIKOTALKIE
        internal Remwave.Client.NikotalkieForm myNikotalkieWindow;
#endif
        //Media engine engine
        // v5.12.8.1 private const string ENGINE_KEY = "a73beb7f3673043275ed61778c03aadd3c2ad616563411b962a7a62e4bdb220791d3bbede455173dfcf08a8abdbda4603eead82e14e43b65b32b63218e7bfde4";
        private const string ENGINE_KEY = "a2b8fffe907db6109a6b03f0f751574d3ec03b70d14958c182bca5bdb2e60ceacbef6e494b04eeba24be99e1b3942bd778a136cefdc1b9cc73d4ccbe8fee70b5";
        // v5.12.4.5 private const string ENGINE_KEY = "a2b8fffe907db6109a6b03f0f751574d3ec03b70d14958c182bca5bdb2e60ceacbef6e494b04eeba24be99e1b3942bd778a136cefdc1b9cc73d4ccbe8fee70b5";
        
        private PhoneLineState[] mySIPPhoneLines;
        private ClientCallBack myMediaEngineCallback;

        //Video Plugin
        private VideoPlugin myVideoPlugin = new VideoPlugin("nvideo.npm", Application.ProductName);

        //Serializers
        private CallHistorySerializer myCallHistorySerializer = new CallHistorySerializer(AppDataDir);
        private ClientConfigurationSerializer myClientConfigurationSerializer = new ClientConfigurationSerializer(AppDataDir, @"\Configuration.dat");
        private ClientSettingsSerializer mClientSettingsSerializer = new ClientSettingsSerializer(AppDataDir);

        //AudioPlayback
        private AudioPlayer mAudioPlayer = null;

        //Other
        private List<CallRecord> myCallHistoryRecords = new List<CallRecord>();

        //Remwave Unified Storage
#if BRAND_JOCCOME
        private String mStorageFileName = @"\JOCCOme.rusa";
#elif BRAND_NIKOTEL
        private String mStorageFileName = @"\Nikotel.rusa";
#else
        private String mStorageFileName = @"\storage.rusa";
#endif
        internal Storage myStorage;

        //Client Settings
        private ClientSettings mClientSettings;

        //QualityAgentLogger
        private QualityAgentLogger mQualityAgentLogger;

        #endregion

        #region user32.dll imports
        [DllImport("user32.dll")]
        internal static extern UInt32 RegisterWindowMessage([MarshalAs(UnmanagedType.LPTStr)] String lpString);

        [DllImport("user32.dll")]
        internal static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vlc);

        [DllImport("user32.dll")]
        internal static extern bool UnregisterHotKey(IntPtr hWnd, int id);
        #endregion

        #region WndProc hook / meta key watchdog
        /// <summary>
        /// WndProc has following direction
        /// - counting ALT meta key presses inside a second
        /// </summary>

        uint WM_DIAL = RegisterWindowMessage(Guid.NewGuid().ToString());

        protected override void WndProc(ref Message m)
        {
            if (this.mFormIsClosing)
            {
                base.WndProc(ref m);
                return;
            }

            long maxRepeat = 1 * 1000 * 10000;
            try
            {
                if (mUserAccount.LoggedIn)
                {

                    if (m.Msg == 74)
                    {
                        // if (m.LParam == null) return;
                        string s = Marshal.PtrToStringAuto(m.LParam);

                    }
                    if (m.Msg == 0x0312)
                    {
                        if (System.DateTime.Now.Ticks < (mAltKeyLastTick + maxRepeat))
                        {
                            mAltKeyHitCount++;
                            if (mAltKeyHitCount == 2)
                            {
                                mAltKeyHitCount = 0;
                                ShowHideSpeedDialWindow();
                                base.WndProc(ref m);
                            }
                        }
                        else
                        {
                            mAltKeyHitCount = 1;
                            base.WndProc(ref m);
                        }
                        mAltKeyLastTick = System.DateTime.Now.Ticks;
                    }
                }
                base.WndProc(ref m);
            }
            catch (Exception ex)
            {
                Console.WriteLine("WndProc " + ex.Message);
            }
        }
        #endregion

        private void LocalizeComponent()
        {
            //set all controls default localization
            myStatusStrip.Text = Properties.Localization.txtTitleStatus;
            myGoOfflineToolStripMenuItem.Text = Properties.Localization.txtStatusOffline;
            myGoOnlineToolStripMenuItem.Text = Properties.Localization.txtStatusOnline;
            myGoAwayToolStripMenuItem.Text = Properties.Localization.txtStatusAway;
            myGoExtendedAwayToolStripMenuItem.Text = Properties.Localization.txtStatusExtendedAway;
            myGoDoNotDisturbToolStripMenuItem.Text = Properties.Localization.txtStatusDND;
            myMainMenuStrip.Text = Properties.Localization.txtMenu;
            plikToolStripMenuItem.Text = Properties.Localization.txtMenuFile;
            messageHistoryToolStripMenuItem.Text = Properties.Localization.txtMenuMessageHistroy;
            settingsToolStripMenuItem.Text = Properties.Localization.txtMenuSettings;
            myMainMenuSignOutItem.Text = Properties.Localization.txtMenuSignOut;
            exitToolStripMenuItem.Text = Properties.Localization.txtMenuExit;
            helpToolStripMenuItem.Text = Properties.Localization.txtMenuHelp;
            myMainMenuAboutItem.Text = Properties.Localization.txtMenuAbout;
            meToolStripMenuItem.Text = Properties.Localization.txtMenuMe;
            homenetToolStripMenuItem.Text = Properties.Localization.txtMenuIMDefault;
            msnToolStripMenuItem.Text = Properties.Localization.txtMenuIMMSN;
            yahooToolStripMenuItem.Text = Properties.Localization.txtMenuIMYahoo;
            aimToolStripMenuItem.Text = Properties.Localization.txtIMAIM;
            ggToolStripMenuItem.Text = Properties.Localization.txtMenuIMGG;
            icqToolStripMenuItem.Text = Properties.Localization.txtMenuIMICQ;
            sendMessageRosterContextMenuItem.Text = Properties.Localization.txtCMenuSendMessage;
            callRosterContextMenuItem.Text = Properties.Localization.txtCMenuCallComputer;
            callPhone2PhoneRosterContextMenuItem.Text = Properties.Localization.txtCMenuCallPhone2Phone;
            startVideoCallRosterContextMenuItem.Text = Properties.Localization.txtCMenuStartVideoCall;
            viewContactPropertiesRosterContextMenuItem.Text = Properties.Localization.txtCMenuViewContactProperties;
            deleteRosterContextMenuItem.Text = Properties.Localization.txtCMenuDelete;
            sendMessageContactContextMenuItem.Text = Properties.Localization.txtCMenuSendMessage;
            callContactContextMenuItem.Text = Properties.Localization.txtCMenuCallComputer;
            startVideoCallContactContextMenuItem.Text = Properties.Localization.txtCMenuStartVideoCall;
            callMobileContactContextMenuItem.Text = Properties.Localization.txtCMenuCallMobile;
            callHomeContactContextMenuItem.Text = Properties.Localization.txtCMenuCallHome;
            callWorkContactContextMenuItem.Text = Properties.Localization.txtCMenuCallWork;
            callVoIPContactContextMenuItem.Text = Properties.Localization.txtCmenuCallVoIP;
            callPhone2PhoneContactContextMenuItem.Text = Properties.Localization.txtCMenuCallPhone2Phone;
            viewContactPropertiesContactContextMenuItem.Text = Properties.Localization.txtCMenuViewContactProperties;
            deleteContactContextMenuItem.Text = Properties.Localization.txtCMenuDelete;
            myContactsToolStrip.Text = Properties.Localization.txtCMenuContacts;
            myContactsShowAllButton.Text = Properties.Localization.txtBtnShowAll;
            myContactsShow1ABButton.Text = Properties.Localization.txtBtnShow1AB;
            myContactsShowCDEButton.Text = Properties.Localization.txtBtnShowCDE;
            myContactsShowFGHButton.Text = Properties.Localization.txtBtnShowFGH;
            myContactsShowIJKButton.Text = Properties.Localization.txtBtnShowIJK;
            myContactsShowLMNButton.Text = Properties.Localization.txtBtnShowLMN;
            myContactsShowOPQButton.Text = Properties.Localization.txtBtnShowOPQ;
            myContactsShowRSTButton.Text = Properties.Localization.txtBtnShowRST;
            myContactsShowUVWButton.Text = Properties.Localization.txtBtnShowUVW;
            myContactsShowXYZButton.Text = Properties.Localization.txtBtnShowXYZ;
            myTabItemDialPad.Text = Properties.Localization.txtTitleDialPad;
            myPhoneHOLDButton.Text = Properties.Localization.txtBtnHold;
            myPhoneHoldLine1Button.Text = Properties.Localization.txtBtnHoldLine1;
            myPhoneHoldLine2Button.Text = Properties.Localization.txtBtnHoldLine2;
            myPhoneHoldLine3Button.Text = Properties.Localization.txtBtnHoldLine3;
            myPhoneHoldLine4Button.Text = Properties.Localization.txtBtnHoldLine4;
            myPhoneCONFButton.Text = Properties.Localization.txtBtnConf;
            myPhoneConfLine1Button.Text = Properties.Localization.txtBtnConfLine1;
            myPhoneConfLine2Button.Text = Properties.Localization.txtBtnConfLine2;
            myPhoneConfLine3Button.Text = Properties.Localization.txtBtnConfLine3;
            myPhoneConfLine4Button.Text = Properties.Localization.txtBtnConfLine4;
            myPhoneLineRECButton.Text = Properties.Localization.txtBtnRec;
            myPhoneLineXFERButton.Text = Properties.Localization.txtBtnXfer;
            myDialPadRadButtonPound.Text = Properties.Localization.txtBtnDialPadPound;
            myDialPadRadButton9.Text = Properties.Localization.txtBtnDialPad9;
            myDialPadRadButton8.Text = Properties.Localization.txtBtnDialPad8;
            myDialPadRadButton7.Text = Properties.Localization.txtBtnDialPad7;
            myDialPadRadButton6.Text = Properties.Localization.txtBtnDialPad6;
            myDialPadRadButton5.Text = Properties.Localization.txtBtnDialPad5;
            myDialPadRadButton4.Text = Properties.Localization.txtBtnDialPad4;
            myDialPadRadButton3.Text = Properties.Localization.txtBtnDialPad3;
            myDialPadRadButton2.Text = Properties.Localization.txtBtnDialPad2;
            myDialPadRadButton1.Text = Properties.Localization.txtBtnDialPad1;
            myDialPadRadButton0.Text = Properties.Localization.txtBtnDialPad0;
            myPhoneLinesTabControl.Text = Properties.Localization.txtTitleLines;
            myPhoneLine0Tab.Text = Properties.Localization.txtTabLine0;
            myPhoneLine1Tab.Text = Properties.Localization.txtTabLine1;
            myPhoneLine2Tab.Text = Properties.Localization.txtTabLine2;
            myPhoneLine3Tab.Text = Properties.Localization.txtTabLine3;
            myTabItemCallHistory.Text = Properties.Localization.txtTitleCallHistory;
            myCallHistory_CallToolStripMenuItem.Text = Properties.Localization.txtCMenuCall;
            myCallHistory_AddContactToolStripItem.Text = Properties.Localization.txtCMenuAddtoContacts;
            myCallHistory_DeleteToolStripItem.Text = Properties.Localization.txtCMenuDelete;
            myTabItemVoiceMail.Text = Properties.Localization.txtTitleVoiceMail;
            myDialPadInfoCallDurationLabel.Text = Properties.Localization.txtInfoCallDuration;
            myDialPadInfoCallingValueLabel.Text = Properties.Localization.txtInfoCallingValue;
            myDialPadInfoCallingLabel.Text = Properties.Localization.txtInfoCalling;
            myDialPadInfoStatusValueLabel.Text = Properties.Localization.txtInfoStatusValue;
            myDialPadInfoStatusLabel.Text = Properties.Localization.txtInfoStatusLable;
            myLoginButton.Text = Properties.Localization.txtBtnLogin;
            myLoginCreateAccountLink.Text = Properties.Localization.txtLinkCreateAccount;
            myLoginForgotPasswordLink.Text = Properties.Localization.txtLinkForgotPassword;
            myLoginAutoLoginCheckBox.Text = Properties.Localization.txtLinkLoginAutomatically;
            myLoginRememberMeCheckBox.Text = Properties.Localization.txtLinkRememberMe;
            myLoginPassowordLabel.Text = Properties.Localization.txtLinkPassword;
            myLoginUsernameLabel.Text = Properties.Localization.txtLinkUsername;
            myMainTabControl.Text = Properties.Localization.txtTitleMainTabControl;
            myMainOnlineTabPage.Text = Properties.Localization.txtTitleMainTabOnlinePage;
            myMainContactsTabPage.Text = Properties.Localization.txtTitleMainContactsTabPage;
            myMainDialPadTabPage.Text = Properties.Localization.txtTitleDialPadTabPage;
            myMenuAddContactToolStripButton.Text = Properties.Localization.txtCMenuAddContact;
            myMenuSearchContactToolStripButton.Text = Properties.Localization.txtCMenuSearchforUsers;
            myMenuChatToolStripButton.Text = Properties.Localization.txtCMenuChat;
            myMenuCallToolStripButton.Text = Properties.Localization.txtCMenuCall;
            myPrepaidAmountToolStripLabel.Text = Properties.Localization.txtCMenuPrepaidAmount;
            myMenuPhone2PhoneCallToolStripButton.Text = Properties.Localization.txtCMenuCallPhone2Phone;
            myPrepaidAmountRefreshToolStripLabel.Text = Properties.Localization.txtCMenuPrepaidAmountRefresh;
            myMenuVideoCallToolStripButton.Text = Properties.Localization.txtCMenuVideoCall;
            myNotifyIcon.Text = Properties.Localization.txtTitleNotifyIcon;




        }


        private void BrandComponent()
        {
            this.Text = Application.ProductName;
            this.LinkOnlineToolStripMenuItem.Text = Application.ProductName + " Online";
            this.homenetToolStripMenuItem.Text = ConfigBrand.ServiceCommonName;
            this.myNotifyIcon.BalloonTipTitle = Application.ProductName;
            this.myNotifyIcon.Text = Application.ProductName;
            this.xmppsControl.Resource = Application.ProductName;
            this.Icon = Properties.Resources.desktop;
            this.myNotifyIcon.Icon = Properties.Resources.desktop;
            this.myDialPadCallCancelButton.BackgroundImage = Properties.Resources.CallCancelButton;
            this.myDialPadCallOrAnswerButton.BackgroundImage = Properties.Resources.CallAnswerButton;
            this.homenetToolStripMenuItem.Text = ConfigBrand.ServiceCommonName;

            /*
            * PresenceImages
            0 Offline
            1 Online
            2 Away
            3 ExtAway
            4 Busy
            5 Node Opened
            6 Node Closed
            7 NodeSelected
            8 EventTyping
            */
            this.myPresenceImagesList.Images.Clear();
            this.myPresenceImagesList.Images.Add(Properties.Resources.statusOffline);
            this.myPresenceImagesList.Images.Add(Properties.Resources.statusOnline);
            this.myPresenceImagesList.Images.Add(Properties.Resources.statusAway);
            this.myPresenceImagesList.Images.Add(Properties.Resources.statusExtAway);
            this.myPresenceImagesList.Images.Add(Properties.Resources.statusBusy);
            this.myPresenceImagesList.Images.Add(Properties.Resources.NodeIconOpened);
            this.myPresenceImagesList.Images.Add(Properties.Resources.NodeIconClosed);
            this.myPresenceImagesList.Images.Add(Properties.Resources.NodeIconSelected);
            this.myPresenceImagesList.Images.Add(Properties.Resources.statusTyping);

            /*
            * myDialPadImageList
            0  DialPad_0
            1  DialPad_1
            2  DialPad_2
            3  DialPad_3
            4  DialPad_4
            5  DialPad_5
            6  DialPad_6
            7  DialPad_7
            8  DialPad_8
            9  DialPad_9
            10 DialPad_Ast
            11 DialPad_Pnd
             */
            this.myDialPadImageList.Images.Clear();
            this.myDialPadImageList.Images.Add(Properties.Resources.DialPad_0);
            this.myDialPadImageList.Images.Add(Properties.Resources.DialPad_1);
            this.myDialPadImageList.Images.Add(Properties.Resources.DialPad_2);
            this.myDialPadImageList.Images.Add(Properties.Resources.DialPad_3);
            this.myDialPadImageList.Images.Add(Properties.Resources.DialPad_4);
            this.myDialPadImageList.Images.Add(Properties.Resources.DialPad_5);
            this.myDialPadImageList.Images.Add(Properties.Resources.DialPad_6);
            this.myDialPadImageList.Images.Add(Properties.Resources.DialPad_7);
            this.myDialPadImageList.Images.Add(Properties.Resources.DialPad_8);
            this.myDialPadImageList.Images.Add(Properties.Resources.DialPad_9);
            this.myDialPadImageList.Images.Add(Properties.Resources.DialPad_Ast);
            this.myDialPadImageList.Images.Add(Properties.Resources.DialPad_Pnd);

            /*
            //Toolbar
            myMenuAddContactToolStripButton.Image
            myMenuSearchContactToolStripButton.Image
            myMenuChatToolStripButton.Image
            myMenuCallToolStripButton.Image
            myMenuPhone2PhoneCallToolStripButton.Image
            myMenuVideoCallToolStripButton.Image
            //Tabs
            myMainOnlineTabPage.Image
            myMainContactsTabPage.Image
            myMainDialPadTabPage.Image
            */
#if NOUSERSEARCH
            myMenuSearchContactToolStripButton.Visible = false;
#endif

#if NOVIDEO
            myMenuVideoCallToolStripButton.Visible = false;
            startVideoCallContactContextMenuItem.Visible = false;
            startVideoCallRosterContextMenuItem.Visible = false;
#endif

#if NOIMPEERING
            msnToolStripMenuItem.Visible = false;
            yahooToolStripMenuItem.Visible = false;
            aimToolStripMenuItem.Visible = false;
            ggToolStripMenuItem.Visible = false;
            icqToolStripMenuItem.Visible = false;
#endif

#if NOVOICEMAIL
            myTabItemVoiceMail.Visibility = ElementVisibility.Hidden;
#endif


#if !BRAND_JOCCOME
            this.myContactsListBox.ThemeName = "Office2007Black";
            this.myDialPadTabControl.ThemeName = "Office2007Black";
            this.myPhoneLinesTabControl.ThemeName = "Office2007Black";
            this.myCallHistoryListBox.ThemeName = "Office2007Black";
            this.myMainTabControl.ThemeName = "Office2007Black";
            this.myLoginButton.Image = null;
#endif

        }
        internal void ShowMinimized()
        {
            this.WindowState = FormWindowState.Minimized;
            this.Show();
        }

        public ClientForm()               
        {
            mClientEvents = new ClientEvents();
            //initialize client events
            EventNotification eventNotificationIncomingInstantMessage = new EventNotification();
            eventNotificationIncomingInstantMessage.Event = ClientEvent.IncomingInstantMessage;
            eventNotificationIncomingInstantMessage.NotificationData = Directory.GetCurrentDirectory() + @"\Sounds\im.mp3";
            eventNotificationIncomingInstantMessage.NotificationType = EventNotificationType.Sound;
            mClientEvents.AddEventNotification(eventNotificationIncomingInstantMessage);

            EventNotification eventNotificationIncomingNudge = new EventNotification();
            eventNotificationIncomingNudge.Event = ClientEvent.IncomingNudge;
            eventNotificationIncomingNudge.NotificationData = Directory.GetCurrentDirectory() + @"\Sounds\nudge.mp3";
            eventNotificationIncomingNudge.NotificationType = EventNotificationType.Sound;
            mClientEvents.AddEventNotification(eventNotificationIncomingNudge);

            EventNotification eventNotificationStartRinging = new EventNotification();
            eventNotificationStartRinging.Event = ClientEvent.IncomingCallStartRinging;
            eventNotificationStartRinging.NotificationData = Directory.GetCurrentDirectory() + @"\Sounds\ringin.mp3";
            eventNotificationStartRinging.NotificationType = EventNotificationType.RingingStart;
            mClientEvents.AddEventNotification(eventNotificationStartRinging);


            EventNotification eventNotificationStopRinging = new EventNotification();
            eventNotificationStopRinging.Event = ClientEvent.IncomingCallStopRinging;
            eventNotificationStopRinging.NotificationData = "";
            eventNotificationStopRinging.NotificationType = EventNotificationType.RingingStop;
            mClientEvents.AddEventNotification(eventNotificationStopRinging);

            //Apply Client Settings
            ApplyClientSettings();




            mContactBook = new ContactBook(new NTContactStore[]{
               new NTContactStore("vCard",NTContactStoreType.vCard,true),
               new NTContactStore(Properties.Localization.txtTitleStoreLocal,NTContactStoreType.Local,true),               
               new NTContactStore(Properties.Localization.txtTitleStoreOutlook, NTContactStoreType.Outlook, mClientSettings.ContactsEnableOutlookStore),
               new NTContactStore(Properties.Localization.txtTitleStoreServer, NTContactStoreType.Server, mClientSettings.ContactsEnableServerStore)                
                }, AppDataDir);

            mContactBook.UpdateCompleted += new EventHandler(mContactBook_UpdateCompleted);

            RegistryKey key = null;

            if (mClientSettings.ProgramLanguage != "" && mClientSettings.ProgramLanguage.Length == 5)
            {
                try
                {
                    CultureInfo currentCulture = new CultureInfo(mClientSettings.ProgramLanguage);
                    Thread.CurrentThread.CurrentUICulture = currentCulture;
                }
                catch (Exception ex)
                {
#if (TRACE)
                    Console.WriteLine("ClientForm Failed Set Culture From settings : " + ex.Message);
#endif
                }
            }
            else
            {
                key = Registry.CurrentUser.OpenSubKey(mCultureInfoRegistryKey);
                if (key != null)
                {
                    try
                    {
                        String value = key.GetValue("CultureInfo", "").ToString();
                        if (value.Length == 5)
                        {
                            CultureInfo currentCulture = new CultureInfo(value);
                            Thread.CurrentThread.CurrentUICulture = currentCulture;
                            mClientSettings.ProgramLanguage = value;
                        }
                    }
                    catch (Exception ex)
                    {
#if (TRACE)
                        Console.WriteLine("ClientForm Failed Set Culture From registry : " + ex.Message);
#endif
                    }
                    key.Close();
                }
            }

            //INITIALIZE STORAGE VERISIONING

            List<StorageDDL> storageDDL = new List<StorageDDL>();
            storageDDL.Add(new StorageDDL("1.0.0", Properties.Resources.fbDDL_1_0_0));
            storageDDL.Add(new StorageDDL("1.0.1", Properties.Resources.fbDDL_1_0_1));
            storageDDL.Add(new StorageDDL("1.0.2", Properties.Resources.fbDDL_1_0_2));
            storageDDL.Add(new StorageDDL("1.0.3", Properties.Resources.fbDDL_1_0_3));
            storageDDL.Add(new StorageDDL("1.0.4", Properties.Resources.fbDDL_1_0_4));
            storageDDL.Add(new StorageDDL("1.0.5", Properties.Resources.fbDDL_1_0_5));
            storageDDL.Add(new StorageDDL("1.0.6", Properties.Resources.fbDDL_1_0_6));

            myStorage = new Storage(storageDDL, "1.0.6");

            InitializeComponent();
            LocalizeComponent();
            BrandComponent();

            PRESENCES = new String[] { 
                Properties.Localization.txtStatusOffline, 
                Properties.Localization.txtStatusOnline, 
                Properties.Localization.txtStatusAway, 
                Properties.Localization.txtStatusExtendedAway, 
                Properties.Localization.txtStatusDND 
            };

            // Add the handlers to the NetworkChange events.
            NetworkChange.NetworkAvailabilityChanged +=
                NetworkAvailabilityChanged;
            NetworkChange.NetworkAddressChanged +=
                NetworkAddressChanged;





            //Make Sure that AppDataDir directory exists
            // 
            try
            {
                if (!Directory.Exists(AppDataDir)) Directory.CreateDirectory(AppDataDir);
            }
            catch (Exception ex)
            {
#if (TRACE)
                Console.WriteLine("ClientForm AppDataDir : " + ex.Message);
#endif
            }

            //open login panel and process login
            myMainWindowSplitContainer.Panel1Collapsed = false;
            myMainWindowSplitContainer.Panel2Collapsed = true;

            //Media Engine Callback
            myMediaEngineCallback = new ClientCallBack(this.Callback);

            //link Presence Images
            myRosterListTreeView.ImageList = this.myPresenceImagesList;


            //preset size
            this.ClientSize = mClientSize;
            this.MinimumSize = this.Size;
            this.MaximumSize = this.Size;
            this.MaximizeBox = false;

            bool result = RegisterHotKey(this.Handle, this.GetType().GetHashCode(), 1, 0);

            myVideoPlugin.VideoPluginExit += new EventHandler(myVideoPlugin_VideoPluginExit);

            initializeDialPadMenuItems();

            //Initialize mXMPPIQ
            mXMPPIQ = new XMPPIQ(ConfigIM.IMServer);
            mXMPPIQ.SendIQMessage += new EventHandler(mXMPPIQ_SendIQMessage);
            mXMPPIQ.RecivedVCard += new EventHandler(mXMPPIQ_RecivedVCard);

        }

       
        void mContactBook_UpdateCompleted(object sender, EventArgs e)
        {
            LoadContactsBook();
        }

        void mXMPPIQ_RecivedVCard(object sender, EventArgs e)
        {
            try
            {
                IQVCard vcard = (IQVCard)sender;
                if (vcard == null | vcard.NTJabberID == null) return;

                JabberUser jabberUser = new JabberUser(vcard.NTJabberID);
                if (mUserAccount.JabberUser.JID == jabberUser.JID)
                {
                    TranslateVCardToNTContact(vcard, mUserAccount.Contact);
                }
                else
                {
                    NTContact contact = null;
                    ContactList contacts = mContactBook.getCandidatesForJabberID(jabberUser.JID);

                    for (int i = contacts.Count - 1; i >= 0; i--)
                    {
                        if (contacts[i] != null & contacts[i].NTContactStore == NTContactStoreType.vCard)
                        {
                            contact = contacts[i];
                        }
                        else
                        {
                            return;
                        }
                    }

                    if (contact == null)
                    {
                        contact = new NTContact();
                        contact.NTJabberID = jabberUser.JID;
                        contact.NTContactStore = NTContactStoreType.vCard;
                        mContactBook.Add(contact);
                    }

                    TranslateVCardToNTContact(vcard, contact);
                    mContactBook.Modified();
                }
            }
            catch (Exception)
            {
                //  throw;
            }

        }

        void TranslateVCardToNTContact(IQVCard vcard, NTContact contact)
        {
            try
            {
                if (vcard == null | contact == null) return;


                contact.NTFirstName = vcard.NTName.NTFirstName != null ? vcard.NTName.NTFirstName : "";
                contact.NTLastName = vcard.NTName.NTLastName != null ? vcard.NTName.NTLastName : "";
                contact.NTMiddleName = vcard.NTName.NTMiddleName != null ? vcard.NTName.NTMiddleName : "";

                contact.NTNickname = vcard.NTNickname != null ? vcard.NTNickname : "";

                if (contact.NTPicture != null)
                {
                    contact.NTPicture = vcard.NTPicture.ImageBase64 != null ? vcard.NTPicture.ImageBase64 : "";
                }

                contact.NTEmail1Address = vcard.NTEmail.UserID;
                if (vcard.NTTel != null)
                {
                    foreach (IQVCard.Tel telItem in vcard.NTTel)
                    {
                        if (telItem.Voice != null & telItem.Work != null & telItem.Number != null) contact.NTBusinessTelephoneNumber = telItem.Number;
                        if (telItem.Cell != null & telItem.Home != null & telItem.Number != null) contact.NTMobileTelephoneNumber = telItem.Number;
                        if (telItem.Voice != null & telItem.Home != null & telItem.Number != null) contact.NTHomeTelephoneNumber = telItem.Number;
                    }
                }
                if (vcard.NTAddress != null)
                {
                    foreach (IQVCard.Adr adrItem in vcard.NTAddress)
                    {

                        if (adrItem.Home != null)
                        {
                            contact.NTHomeAddressPostalCode = adrItem.PCode != null ? adrItem.PCode : "";
                            contact.NTHomeAddressStreet = adrItem.Street != null ? adrItem.Street : "";
                            contact.NTHomeAddressState = adrItem.Region != null ? adrItem.Region : "";
                            contact.NTHomeAddressCity = adrItem.Locality != null ? adrItem.Locality : "";
                            contact.NTHomeAddressCountry = adrItem.Ctry != null ? adrItem.Ctry : "";
                        }
                    }
                }
            }
            catch (Exception)
            {
                //  throw;
            }

        }
        void mXMPPIQ_SendIQMessage(object sender, EventArgs e)
        {
            if (xmppsControl.Connected)
            {
                try
                {
                    XMPPIQ.IQMessage message = (XMPPIQ.IQMessage)sender;
                    xmppsControl.SendCommand(message.Message);
                }
                catch (Exception ex)
                {
#if (TRACE)
                    Console.WriteLine("mXMPPIQ_SendIQMessage : " + ex.Message);
#endif
                }
            }
        }

        #region Initialize Dial Pad Buttons Menu Items
        private void initializeDialPadMenuItems()
        {
            this.myPhoneConfLine1Button.Click += new EventHandler(myPhoneConfLine1Button_Click);
            this.myPhoneConfLine2Button.Click += new EventHandler(myPhoneConfLine2Button_Click);
            this.myPhoneConfLine3Button.Click += new EventHandler(myPhoneConfLine3Button_Click);
            this.myPhoneConfLine4Button.Click += new EventHandler(myPhoneConfLine4Button_Click);
            this.myPhoneHoldLine1Button.Click += new EventHandler(myPhoneHoldLine1Button_Click);
            this.myPhoneHoldLine2Button.Click += new EventHandler(myPhoneHoldLine2Button_Click);
            this.myPhoneHoldLine3Button.Click += new EventHandler(myPhoneHoldLine3Button_Click);
            this.myPhoneHoldLine4Button.Click += new EventHandler(myPhoneHoldLine4Button_Click);

        }

        void myPhoneHoldLine4Button_Click(object sender, EventArgs e)
        {
            try
            {
                TELEPHONY_RETURN_VALUE result;
                result = mSIPPhone.HoldOnOffCall(3);
#if (TRACE)
                Console.WriteLine("SIP|Line 4 HOLD:" + result.ToString());
#endif
            }
            catch (Exception)
            {
                return;
            }
        }

        void myPhoneHoldLine3Button_Click(object sender, EventArgs e)
        {
            try
            {
                TELEPHONY_RETURN_VALUE result;
                result = mSIPPhone.HoldOnOffCall(2);
#if (TRACE)
                Console.WriteLine("SIP|Line 3 HOLD:" + result.ToString());
#endif
            }
            catch (Exception)
            {
                return;
            }
        }

        void myPhoneHoldLine2Button_Click(object sender, EventArgs e)
        {
            try
            {
                TELEPHONY_RETURN_VALUE result;
                result = mSIPPhone.HoldOnOffCall(1);
#if (TRACE)
                Console.WriteLine("SIP|Line 2 HOLD:" + result.ToString());
#endif
            }
            catch (Exception)
            {
                return;
            }
        }

        void myPhoneHoldLine1Button_Click(object sender, EventArgs e)
        {
            try
            {
                TELEPHONY_RETURN_VALUE result;
                result = mSIPPhone.HoldOnOffCall(0);
#if (TRACE)
                Console.WriteLine("SIP|Line 1 HOLD:" + result.ToString());
#endif
            }
            catch (Exception)
            {
                return;
            }
        }


        void myPhoneConfLine4Button_Click(object sender, EventArgs e)
        {

            try
            {
                TELEPHONY_RETURN_VALUE result;
                result = mSIPPhone.ConferenceOnOffCall(3);
#if (TRACE)
                Console.WriteLine("SIP|Line 4 CONF:" + result.ToString());
#endif
            }
            catch (Exception)
            {
                return;
            }
        }

        void myPhoneConfLine3Button_Click(object sender, EventArgs e)
        {
            try
            {
                TELEPHONY_RETURN_VALUE result;
                result = mSIPPhone.ConferenceOnOffCall(2);
#if (TRACE)
                Console.WriteLine("SIP|Line 3 CONF:" + result.ToString());
#endif
            }
            catch (Exception)
            {
                return;
            }
        }

        void myPhoneConfLine2Button_Click(object sender, EventArgs e)
        {
            try
            {
                TELEPHONY_RETURN_VALUE result;
                result = mSIPPhone.ConferenceOnOffCall(1);
#if (TRACE)
                Console.WriteLine("SIP|Line 2 CONF:" + result.ToString());
#endif
            }
            catch (Exception)
            {
                return;
            }
        }

        void myPhoneConfLine1Button_Click(object sender, EventArgs e)
        {
            try
            {
                TELEPHONY_RETURN_VALUE result;
                result = mSIPPhone.ConferenceOnOffCall(0);
#if (TRACE)
                Console.WriteLine("SIP|Line 1 CONF:" + result.ToString());
#endif
            }
            catch (Exception)
            {
                return;
            }
        }
        #endregion
        private delegate void myVideoPlugin_VideoPluginExitDelegate(object sender, EventArgs e);
        private void myVideoPlugin_VideoPluginExit(object sender, EventArgs e)
        {
            if (mFormIsClosing) return;
            if (this.InvokeRequired)
            {
                this.Invoke(new myVideoPlugin_VideoPluginExitDelegate(this.myVideoPlugin_VideoPluginExit), new object[] { sender, e });
                return;
            }
#if (TRACE)
            Console.WriteLine("myVideoPlugin_VideoPluginExit");
#endif
        }

        #region Activities

        internal void StartActivity(Activity activity)
        {
            mySpeedDialWindow.Hide();
            switch (activity.ActivityType)
            {
                case ActivityType.None:
                    break;
                case ActivityType.Call:
                    StartNewCall(-1, activity.ActivityOtherData);
                    this.WindowState = FormWindowState.Normal;
                    this.Show();
                    this.Activate();
                    break;
                case ActivityType.IM:
                    StartNewIM(activity.ActivityJabberUser, true);
                    break;
                case ActivityType.VideoCall:
                    StartNewVideoCall(activity.ActivityJabberUser);
                    this.WindowState = FormWindowState.Normal;
                    this.Show();
                    this.Activate();
                    break;
                case ActivityType.ScreenSharing:
                    StartNewVideoCall(activity.ActivityJabberUser);
                    this.WindowState = FormWindowState.Normal;
                    this.Show();
                    this.Activate();
                    break;
                case ActivityType.Email:
                    StartNewEmail(activity.ActivityOtherData);
                    break;
                case ActivityType.AddContact:
                    StartNewContact(activity.ActivityOtherData);
                    break;

#if !NONIKOMEETING
                case ActivityType.HostNikomeeting:

                    try
                    {
                        String command = Directory.GetCurrentDirectory() + @"\nikomeeting.exe";
                        String arguments = "\"action=autohost;username=" + mUserAccount.Username + ";password=" + mUserAccount.Password + "\"";
                        System.Diagnostics.Process.Start(command, arguments);
                        if (this.WindowState != FormWindowState.Minimized) this.WindowState = FormWindowState.Minimized;
                    }
                    catch (Exception ex)
                    {
#if (DEBUG)
                        throw;
#else
                        Console.WriteLine("StartActivity : " + ex.Message);
#endif
                    }
                    break;
                case ActivityType.JoinNikomeeting:

                    try
                    {
                        String command = Directory.GetCurrentDirectory() + @"\nikomeeting.exe";
                        String arguments = "\"action=join;username=" + mUserAccount.Username + ";sessionid=" + activity.ActivityOtherData + "\"";
                        System.Diagnostics.Process.Start(command, arguments);
                        if (this.WindowState != FormWindowState.Minimized) this.WindowState = FormWindowState.Minimized;
                    }
                    catch (Exception ex)
                    {
#if (DEBUG)
                        throw;
#else
                        Console.WriteLine("StartActivity : " + ex.Message);
#endif
                    }
                    break;
#endif

#if !NONIKOTALKIE
                case ActivityType.NikotalkieMessage:
                    myNikotalkieWindow.StartComposing(activity.ActivityJabberUser.Username);
                    break;
#endif




                default:
                    break;
            }


        }

        internal void StartNewIM(String jid, bool setFocus)
        {
            JabberUser jaberUser = new JabberUser(jid);
            StartNewIM(jaberUser, setFocus);
        }

        internal void StartNewIM(JabberUser jabberUser, bool setFocus)
        {
            try
            {
                myChatWindow.NewChat(jabberUser, setFocus);
                xmppsControl.ProbePresence(jabberUser.EscapedJID);
            }
            catch (Exception ex)
            {
#if (TRACE)
                Console.WriteLine("StartNewIM : " + ex.Message);
#endif
            }
        }

        internal void StartNewVideoCall(String jid)
        {
            JabberUser jabberUser = new JabberUser(jid);
            StartNewVideoCall(jabberUser);
        }
        internal void StartNewVideoCall(JabberUser jabberUser)
        {
            VideoSessionInvite(jabberUser);
        }

        internal void StartNewEmail(String email)
        {
            try
            {
                System.Diagnostics.Process.Start(@"mailto:" + email);
            }
            catch (Exception ex)
            {
#if (TRACE)
                Console.WriteLine("StartNewEmail : " + ex.Message);
#endif
            }
        }

        internal void StartNewContact(String jabberID)
        {
            try
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

                    OpenContactWindow(null, properties);
                }
            }
            catch (Exception ex)
            {
#if (TRACE)
                Console.WriteLine("StartNewContact : " + ex.Message);
#endif
            }
        }

        internal void StartUserSearch()
        {
            if (myContactsSearchWindow == null || myContactsSearchWindow.Disposing)
            {
                myContactsSearchWindow = new ContactsSearch(this);

                myContactsSearchWindow.FormClosing += new FormClosingEventHandler(myContactsSearchWindow_FormClosing);
            }
            myContactsSearchWindow.selectedContact = null;
            myContactsSearchWindow.Activate();
            myContactsSearchWindow.Show();
        }

        void myContactsSearchWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!mFormIsClosing)
            {
                e.Cancel = true;
                myContactsSearchWindow.Visible = false;
                if (myContactsSearchWindow.selectedContact != null)
                {
                    OpenContactWindow(myContactsSearchWindow.selectedContact, null);
                }
            }
        }

        #endregion

        private void Client_Load(object sender, EventArgs e)
        {
            try
            {
               

                myClientConfiguration = myClientConfigurationSerializer.LoadClientConfiguration();

                //check the web service for update and newses

                try
                {
                    ClientUpdateWS.Service clientUpdateWS = new Remwave.Client.ClientUpdateWS.Service();
                    clientUpdateWS.VersionCompleted += new Remwave.Client.ClientUpdateWS.VersionCompletedEventHandler(clientUpdateWS_VersionCompleted);
                    clientUpdateWS.VersionAsync();
                }
                catch (Exception ex)
                {
#if (TRACE)
                    Console.WriteLine("Client_Load : " + ex.Message);
#endif
                }

                if (myClientConfiguration.RememberMe)
                {
                    myLoginRememberMeCheckBox.Checked = true;
                    myLoginPasswordInput.Text = myClientConfiguration.Password;
                    myLoginUsernameInput.Text = myClientConfiguration.Username;
                    if (myClientConfiguration.AutoLogin)
                    {
                        myLoginAutoLoginCheckBox.Checked = true;
                    }
                }
            }
            catch (Exception ex)
            {
#if (TRACE)
                Console.WriteLine("Client_Load : " + ex.Message);
#endif
            }
        }

        void clientUpdateWS_VersionCompleted(object sender, Remwave.Client.ClientUpdateWS.VersionCompletedEventArgs e)
        {
            try
            {
                if (e.Error != null) return;
                if (e.Result != null)
                {
                    string[] version = e.Result;
                    ClientUpdateWS.Service clientUpdateWS = new Remwave.Client.ClientUpdateWS.Service();
                    if (Application.ProductVersion.CompareTo(version[0]) < 0)
                    {
                        //force update
                        string downloadlink = clientUpdateWS.Software();
                        if (Disposing) return;
                        if (MessageBox.Show(Properties.Localization.txtInfoNewVersion,
                            Properties.Localization.txtInfoUpdateAvailable,
                            MessageBoxButtons.OKCancel,
                            MessageBoxIcon.Information
                            ) == DialogResult.OK)
                        {

                            try
                            {
                                System.Diagnostics.Process.Start(downloadlink);
                                Application.Exit();
                            }
                            catch (Exception ex)
                            {

#if (TRACE)
                                Console.WriteLine("clientUpdateWS_VersionCompleted Application.Exit: " + ex.Message);
#endif
                            }
                        };
                    }

                    if (myClientConfiguration.LastNews.CompareTo(version[1]) < 0)
                    {
                        //display news
                        string newsLink = clientUpdateWS.News();
                        System.Diagnostics.Process.Start(newsLink);
                        myClientConfiguration.LastNews = version[1];
                    }
                }
            }
            catch (Exception ex)
            {
#if (TRACE)
                Console.WriteLine("clientUpdateWS_VersionCompleted : " + ex.Message);
#endif
            }
        }


        private bool ReInitializeClient()
        {
            if (!this.mFormIsClosing & mUserAccount.LoggedIn)
            {
                TELEPHONY_RETURN_VALUE result;
                for (; true; )
                {
                    DetectNetwork();
                    if (!myNetworkInfo.Online)
                    {
#if (TRACE)
                        Console.WriteLine("Network:SIP Proxy Does not respond, network time out.");
#endif
                        myNotifyIcon.ShowBalloonTip(10,
                            Properties.Localization.txtInfoServiceNotReachable,
                            Properties.Localization.txtInfoServiceNotReachableDesc,
                            ToolTipIcon.Warning);
                        break;
                    }

                    if (!xmppsControl.Connected)
                    {
                        //network detected, try authenticate at jabber
                        try
                        {
                            myServiceStateCheckTimer.Tag = DateTime.Now;
                            xmppsControl.Connect(mUserAccount.Username, mUserAccount.Password);
                        }
                        catch (nsoftware.IPWorksSSL.IPWorksSSLException ex)
                        {
#if (TRACE)
                            Console.WriteLine("IM:ReConnecting:(" + ex.Code + "):" + ex.Message);
#endif
                            break;
                        }
                    }

                    try
                    {
                        result = mSIPPhone.ReStartSip(myNetworkInfo.LocalIP);
                        if (result != TELEPHONY_RETURN_VALUE.SipSuccess)
                        {
#if (TRACE)
                            Console.WriteLine("SIP:ReStartSip:" + result.ToString());
#endif
                            break;
                        }

                    }
                    catch (Exception)
                    {
                        myNotifyIcon.ShowBalloonTip(10,
                            Properties.Localization.txtInfoServiceNotReachable,
                            Properties.Localization.txtInfoServiceNotReachableDesc,
                            ToolTipIcon.Warning);
                        break;
                    }

                    myNotifyIcon.ShowBalloonTip(10,
                        Properties.Localization.txtInfoServiceConnected,
                        Properties.Localization.txtInfoServiceConnectedDesc,
                        ToolTipIcon.Info);
                    if (xmppsControl.Connected)
                    {
                        try
                        {
                            XMPP_ChangePresence(XMPPIQ.PresenceShow.chat);
                        }
                        catch (Exception)
                        {

                        }
                    }
                    return true;
                }
            }
            return false;
        }

        private bool InitializeClient()
        {
            if (!this.mFormIsClosing)
            {
                mUserAccount.Credentials(myLoginUsernameInput.Text, myLoginPasswordInput.Text);

                for (; true; )
                {
                    TELEPHONY_RETURN_VALUE result;
                    DetectNetwork();

                    if (!myNetworkInfo.Online)
                    {
                        try
                        {
                            myNotifyIcon.ShowBalloonTip(10,
                                Properties.Localization.txtInfoServiceNotReachable,
                                Properties.Localization.txtInfoServiceNotReachableDesc,
                                ToolTipIcon.Warning);
                        }
                        catch (Exception)
                        {

                        }
#if (TRACE)
                        Console.WriteLine("Network:SIP Proxy Does not respond, network time out.");
#endif
                        return false;
                    }

                    //Initialize ChatWindow
                    if (myChatWindow != null) myChatWindow.Dispose();
                    myChatWindow = new Remwave.Client.ChatWindow(this);
#if !NONIKOTALKIE
                    if (myNikotalkieWindow != null) myNikotalkieWindow.Dispose();
                    myNikotalkieWindow = new NikotalkieForm(this);
#endif

                    if (mySpeedDialWindow != null) mySpeedDialWindow.Dispose();
                    mySpeedDialWindow = new SpeedDialWindow(this);

                    
                    if (!myStorage.Open(AppDataDir, mStorageFileName, mUserAccount.Username, false))
                    {
                        if (Disposing) return false;
                        if (DialogResult.Yes == MessageBox.Show(
                            Properties.Localization.txtInfoStorageEngineErrorDesc,
                            Properties.Localization.txtInfoStorageEngineError,
                            MessageBoxButtons.YesNoCancel,
                            MessageBoxIcon.Error))
                        {
                            myStorage.Open(AppDataDir, mStorageFileName, mUserAccount.Username, true);
                        }
                    }
                    

                    //network detected, try authenticate at jabber
                    try
                    {
                        myServiceStateCheckTimer.Tag = DateTime.Now;
                        if (xmppsControl.Connected)
                        {
                            try
                            {
                                xmppsControl.Disconnect();
                            }
                            catch (Exception)
                            {

                            }
                        }

                        xmppsControl.InvokeThrough = this;
                        xmppsControl.Timeout = 7200;
                        xmppsControl.IMServer = ConfigIM.IMServer;
                        xmppsControl.IMPort = ConfigIM.IMPort;
                        xmppsControl.Connect(mUserAccount.Username, mUserAccount.Password);
                    }
                    catch (nsoftware.IPWorksSSL.IPWorksSSLException ex)
                    {
#if (TRACE)
                        Console.WriteLine("IM:Connect error:" + ex.Code.ToString() + " - " + ex.Message);
#endif

                        if (Disposing) return false;
                        MessageBox.Show(Properties.Localization.txtInfoWrongUsernamePasswordDesc,
                            Properties.Localization.txtInfoWrongUsernamePassword,
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error
                            );

                        return false;
                    }
                    catch (Exception ex)
                    {
                        return false;
                    }


                    try
                    {
                        result = mSIPPhone.InitEngine(myMediaEngineCallback, mUserAccount.Username, mUserAccount.Password, ConfigSIP.Realm, ConfigSIP.ProxyAddress, myNetworkInfo.LocalIP, ENGINE_KEY, String.Format(ConfigSIP.UserAgent, new object[] { Application.ProductName, Application.ProductVersion }), mClientSettings.PhoneEnableSIPDiagnostic ? 1 : 0, AppDataDir + @"\Sip.log");
                        result = TELEPHONY_RETURN_VALUE.SipSuccess;
                        if (result != TELEPHONY_RETURN_VALUE.SipSuccess)
                        {
                            try
                            {
                                xmppsControl.Disconnect();
                            }
                            catch (Exception)
                            {

                            }

#if (TRACE)
                            Console.WriteLine("SIP:InitEngine:" + result.ToString());
#endif
                            switch (result)
                            {
                                case TELEPHONY_RETURN_VALUE.SipAudioInFailure:
                                    try
                                    {
                                        myNotifyIcon.ShowBalloonTip(10,
                                            Properties.Localization.txtInfoSipEngineAudioInError,
                                            Properties.Localization.txtInfoSipEngineAudioInErrorDesc,
                                            ToolTipIcon.Warning);
                                    }
                                    catch (Exception)
                                    {

                                    }
                                    break;
                                case TELEPHONY_RETURN_VALUE.SipAudioOutFailure:
                                    try
                                    {
                                        myNotifyIcon.ShowBalloonTip(10,
                                            Properties.Localization.txtInfoSipEngineAudioOutError,
                                            Properties.Localization.txtInfoSipEngineAudioOutErrorDesc,
                                            ToolTipIcon.Warning);
                                    }
                                    catch (Exception)
                                    {
                                    }
                                    break;
                                default:
                                    try
                                    {
                                        myNotifyIcon.ShowBalloonTip(10,
                                            Properties.Localization.txtInfoSipEngineGeneralError,
                                            String.Format(Properties.Localization.txtInfoSipEngineGeneralErrorDesc, (int)result),
                                            ToolTipIcon.Warning);
                                    }
                                    catch (Exception)
                                    {
                                    }
                                    break;
                            }
                            break;
                        }
                    }
                    catch (Exception ex)
                    {
#if (TRACE)
                        Console.WriteLine("SIP:InitEngine:" + ex.InnerException.ToString());
#endif
                        try
                        {
                            myNotifyIcon.ShowBalloonTip(10,
                            Properties.Localization.txtInfoServiceNotReachable,
                            Properties.Localization.txtInfoServiceNotReachableDesc,
                            ToolTipIcon.Warning);
                        }
                        catch (Exception)
                        {
                        }
                        break;
                    }

#if !NONIKOTALKIE
                    myNikotalkieWindow.nikotalkieControl.Login(mUserAccount.Username, mUserAccount.Password, ConfigWebLinks.RestNikotalkieUrl, AppDataDir + @"\" + mUserAccount.Username);
#endif

#if !NOVOICEMAIL
                    myWebVoiceMailControl.IntializeControl(mUserAccount.Username, mUserAccount.Password, AppDataDir);
#endif

                    try
                    {
                        myNotifyIcon.ShowBalloonTip(10,
                            Properties.Localization.txtInfoServiceConnected,
                            Properties.Localization.txtInfoServiceConnectedDesc,
                            ToolTipIcon.Info
                            );
                    }
                    catch (Exception)
                    {
                    }

                    if (xmppsControl.Connected)
                    {
                        try
                        {
                            XMPP_ChangePresence(XMPPIQ.PresenceShow.chat);
                        }
                        catch (Exception)
                        {

                        }
                    }
                    mUserAccount.Login();
                    return true;
                }
            }
            return false;
        }

        private void ClientForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.mFormIsClosing == true) return;
            try
            {
                if (mClientSettings.ProgramConfirmClosing & e.CloseReason == CloseReason.UserClosing & !Disposing)
                {
                    if (DialogResult.Yes != MessageBox.Show(
                        Properties.Localization.txtInfoApplicationClosingDesc,
                         Properties.Localization.txtInfoApplicationClosing,
                         MessageBoxButtons.YesNo,
                         MessageBoxIcon.Question)
                        )
                    {
                        e.Cancel = true;
                        return;
                    }
                }

                this.mFormIsClosing = true;
                this.WindowState = FormWindowState.Minimized;
                
                UnregisterHotKey(this.Handle, this.GetType().GetHashCode());

                UserSignOut();

                if (xmppsControl.Connected)
                {
                    try
                    {
                        xmppsControl.Disconnect();
                    }
                    catch (Exception)
                    {

                    }
                }

             
                mClientEvents.StopEvents();
                myStorage.Close();
                myStorage = null;
                if (myVideoPlugin != null) myVideoPlugin.Dispose();

                mSIPPhone = null;

                Application.Exit();
            }
            catch (Exception ex)
            {
#if (TRACE)
                Console.WriteLine("ClientForm_FormClosing : " + ex.Message);
#endif

            }
        }

        private void UserSignOut()
        {
            try
            {
                //logout user
                //switch panels
                //disconnect xmppp
                //shutdown engine
                mContactBook.Save();

                if (mUserAccount.LoggedIn)
                {
                    CallHistory tmpCallHistory = new CallHistory(mUserAccount.Username);
                    tmpCallHistory.CallRecords = myCallHistoryRecords.ToArray();
                    myCallHistorySerializer.SaveCallHistory(tmpCallHistory, mUserAccount.Username);
                    mUserAccount.Logout();
                    myClientConfiguration.DeleteLoginOptions();
                }

                if (xmppsControl.Connected)
                    try
                    {
                        xmppsControl.Disconnect();
                    }
                    catch (Exception)
                    {

                    }

                ShutdownEngine();

                if (myChatWindow != null) myChatWindow.Dispose();
                if (myContactWindow != null) myContactWindow.Dispose();
                if (myArchiveWindow != null) myArchiveWindow.Dispose();
                if (mySpeedDialWindow != null) mySpeedDialWindow.Dispose();
#if !NONIKOTALKIE
                if (myNikotalkieWindow != null) myNikotalkieWindow.Dispose();
#endif

            }
            catch (Exception ex)
            {
#if (TRACE)
                Console.WriteLine("myMainMenuSignOutItem_Click : " + ex.Message);
#endif
            }
            finally
            {
                //open login panel and process login
                myMainWindowSplitContainer.Panel1Collapsed = false;
                myMainWindowSplitContainer.Panel2Collapsed = true;
            }
        }

        #region Detect Network Avaliability Changes

        private void NetworkAvailabilityChanged(
             object sender, NetworkAvailabilityEventArgs e)
        {
            // Report whether the network is now available or unavailable.
            if (e.IsAvailable)
            {
#if (TRACE)
                Console.WriteLine("NetworkAvailabilityChanged - Network Available");
#endif
                myNetworkInfo.Online = true;

            }
            else
            {
#if (TRACE)
                Console.WriteLine("NetworkAvailabilityChanged - Network Unavailable");
#endif

                myNetworkInfo.Online = false;
                myNotifyIcon.ShowBalloonTip(10,
                    Properties.Localization.txtInfoServiceUnavailable,
                    Properties.Localization.txtInfoServiceUnavailableDesc,
                    ToolTipIcon.Warning);

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
                if (mUserAccount.LoggedIn)
                {
#if (TRACE)
                    Console.WriteLine("NetworkAddressChanged - Current IP Address : " + myNetworkInfo.LocalIP);
#endif
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
#if (TRACE)
                    Console.WriteLine("SIP|" + PhoneLineID.ToString() + " : " + sNot + " / " + sRet);
#endif
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
                    switch (retval)
                    {
                        case TELEPHONY_RETURN_VALUE.SipCallRecordComplete:
                            try
                            {
                                String recordingPath =
                                    Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\" + Application.ProductName + @"\";
                                if (!Directory.Exists(recordingPath)) Directory.CreateDirectory(recordingPath);
                                File.Move(EventMessage, recordingPath + "Call - " + DateTime.Now.ToString("yyyyMMdd HHmm.wav"));
                            }
                            catch (Exception ex)
                            {
#if (TRACE)
                                Console.WriteLine("SipCallRecordComplete:" + ex.Message);
#endif
                            }
                            break;
                    }

                }
                else if (not == SIP_NOTIFY_TYPE.PHONE_LINE_NOTIFICATION)
                {

                    //update Line State                  
                    mySIPPhoneLines[PhoneLineID].CallDirection = mSIPPhone.GetPhoneLineCallDirection(PhoneLineID);
                    mySIPPhoneLines[PhoneLineID].CallRecordingActive = mSIPPhone.GetPhoneLineCallRecordingActive(PhoneLineID) == 1 ? true : false;
#if (TRACE)
                    Console.WriteLine("SIP|" + PhoneLineID.ToString() + " : " + sNot + " / " + sRet);
                    Console.WriteLine("SIP-LS|" + PhoneLineID.ToString() + ":" + mySIPPhoneLines[PhoneLineID].State.ToString());
#endif
                    switch (retval)
                    {
                        //reset phone line state
                        case TELEPHONY_RETURN_VALUE.SipOnHook:
                            mySIPPhoneLines[PhoneLineID].State = retval;
                            mClientEvents.RaiseEvent(ClientEvent.IncomingCallStopRinging);
                            CallOnHook(PhoneLineID);
                            ((TabItem)myPhoneLinesTabControl.Items[PhoneLineID]).ImageIndex = 0;

                            if (mySIPPhoneLines[PhoneLineID].CallForwardActive)
                            {
                                mySIPPhoneLines[PhoneLineID].CallForwardActive = false;
                                ForwardCall(PhoneLineID, "sip:" + mySIPPhoneLines[PhoneLineID].CallForwardContact + "@" + ConfigSIP.Realm);
                            }

                            break;
                        case TELEPHONY_RETURN_VALUE.SipIncomingCallStart:
                            mySIPPhoneLines[PhoneLineID].State = retval;
                            mySIPPhoneLines[PhoneLineID].LastDialedNumber = mSIPPhone.GetIncomingCallDetails(PhoneLineID);
                            mySIPPhoneLines[PhoneLineID].LastEnteredNumber = mySIPPhoneLines[PhoneLineID].LastDialedNumber;

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



                            IncomingCall(PhoneLineID, mySIPPhoneLines[PhoneLineID].LastDialedNumber);

                            break;

                        case TELEPHONY_RETURN_VALUE.SipOutgoingCallStart:
                            mySIPPhoneLines[PhoneLineID].State = retval;
                            mySIPPhoneLines[PhoneLineID].LastDialedNumber = mSIPPhone.GetOutgoingCallDetails(PhoneLineID);

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
                            mySIPPhoneLines[PhoneLineID].State = retval;
                            mClientEvents.RaiseEvent(ClientEvent.IncomingCallStopRinging);
                            mySIPPhoneLines[PhoneLineID].CallActive = true;
                            switch (PhoneLineID)
                            {
                                case 0: myPhoneLine0Tab.ImageIndex = 3;
                                    break;
                                case 1: myPhoneLine1Tab.ImageIndex = 3;
                                    break;
                                case 2: myPhoneLine2Tab.ImageIndex = 3;
                                    break;
                                case 3: myPhoneLine3Tab.ImageIndex = 3;
                                    break;
                            }
                            break;


                        case TELEPHONY_RETURN_VALUE.SipInConference:
                        case TELEPHONY_RETURN_VALUE.SipInConferenceOn:
                            mySIPPhoneLines[PhoneLineID].State = retval;
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
                            mySIPPhoneLines[PhoneLineID].State = retval;
                            mySIPPhoneLines[PhoneLineID].CallConferenceActive = false;
                            break;
                        case TELEPHONY_RETURN_VALUE.SipCallHold:
                        case TELEPHONY_RETURN_VALUE.SipCallHoldOn:
                            mySIPPhoneLines[PhoneLineID].State = retval;
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
                            mySIPPhoneLines[PhoneLineID].State = retval;
                            mySIPPhoneLines[PhoneLineID].CallHoldActive = false;
                            switch (PhoneLineID)
                            {
                                case 0: myPhoneLine0Tab.ImageIndex = 3;
                                    break;
                                case 1: myPhoneLine1Tab.ImageIndex = 3;
                                    break;
                                case 2: myPhoneLine2Tab.ImageIndex = 3;
                                    break;
                                case 3: myPhoneLine3Tab.ImageIndex = 3;
                                    break;
                            }
                            break;
                        case TELEPHONY_RETURN_VALUE.SipCallRecordActive:
                            mySIPPhoneLines[PhoneLineID].State = retval;
                            mySIPPhoneLines[PhoneLineID].CallRecordingActive = true;
                            switch (PhoneLineID)
                            {
                                case 0: myPhoneLine0Tab.ImageIndex = 6;
                                    break;
                                case 1: myPhoneLine1Tab.ImageIndex = 6;
                                    break;
                                case 2: myPhoneLine2Tab.ImageIndex = 6;
                                    break;
                                case 3: myPhoneLine3Tab.ImageIndex = 6;
                                    break;
                            }
                            break;
                        case TELEPHONY_RETURN_VALUE.SipCallRecordComplete:
                            mySIPPhoneLines[PhoneLineID].State = retval;
                            mySIPPhoneLines[PhoneLineID].CallRecordingActive = false;
                            switch (PhoneLineID)
                            {
                                case 0: myPhoneLine0Tab.ImageIndex = 3;
                                    break;
                                case 1: myPhoneLine1Tab.ImageIndex = 3;
                                    break;
                                case 2: myPhoneLine2Tab.ImageIndex = 3;
                                    break;
                                case 3: myPhoneLine3Tab.ImageIndex = 3;
                                    break;
                            }
                            break;
                        case TELEPHONY_RETURN_VALUE.SipTransferingCall:
                            mySIPPhoneLines[PhoneLineID].State = retval;
                            mySIPPhoneLines[PhoneLineID].CallTransferActive = true;
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
                        case TELEPHONY_RETURN_VALUE.SipFarEndIsBusy:
                            mySIPPhoneLines[PhoneLineID].State = retval;
                            mySIPPhoneLines[PhoneLineID].LastErrorMessage = Properties.Localization.txtInfoLineBusy;
                            break;
                        case TELEPHONY_RETURN_VALUE.SipFarEndError:
                            mySIPPhoneLines[PhoneLineID].State = retval;
                            if (EventMessage != null)
                            {
                                mySIPPhoneLines[PhoneLineID].LastErrorMessage = EventMessage;
                            } break;

                        ///
                        case TELEPHONY_RETURN_VALUE.SipStartIncomingRing:
                            mySIPPhoneLines[PhoneLineID].State = retval;
                            if (!mClientSettings.PhoneDefaultRingToneEnabled) mClientEvents.RaiseEvent(ClientEvent.IncomingCallStartRinging);
                            break;

                        case TELEPHONY_RETURN_VALUE.SipReceived302MovedTemporarily:
                            mySIPPhoneLines[PhoneLineID].State = retval;
                            mySIPPhoneLines[PhoneLineID].CallForwardActive = true;
                            mySIPPhoneLines[PhoneLineID].CallForwardContact = EventMessage;
                            CancelCall(PhoneLineID);
                            break;
                    }


                    UpdatePhoneLineButtons();
                }
                return;
            }
            catch (InvalidCastException)
            {

            }
        }

        internal void StartNewCall(int phoneLineID, JabberUser jabberUser)
        {
            if (jabberUser.Network == ConfigXMPPNetwork.Default)
            {
                StartNewCall(phoneLineID, jabberUser.Username);
            }
        }

        internal void StartNewCall(int phoneLineID, string Destination)
        {
            try
            {
                if (phoneLineID == -1)
                {
                    phoneLineID = mSIPPhone.GetFreePhoneLine();
                }

                Destination = NormalizePhoneNumber(Destination);
                mySIPPhoneLines[phoneLineID].LastDialedNumber = Destination;
                mySIPPhoneLines[phoneLineID].LastEnteredNumber = Destination;
                myMainTabControl.SelectedTab = myMainDialPadTabPage;
                myDialPadTabControl.SelectedTab = myTabItemDialPad;
                switch (phoneLineID)
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
                PlaceCall(phoneLineID, "sip:" + Destination + "@" + ConfigSIP.Realm);
            }
            catch (Exception ex)
            {
#if (TRACE)
                Console.WriteLine("StartNewCall : " + ex.Message);
#endif
                return;
            }
        }


        private delegate void PlaceCallDelegate(int PhoneLineID, String CallTo);
        private void PlaceCall(int PhoneLineID, String CallTo)
        {

            if (this.InvokeRequired)
            {
                BeginInvoke(new PlaceCallDelegate(PlaceCall), new object[] { PhoneLineID, CallTo });
            }
            else
            {
                TELEPHONY_RETURN_VALUE result;
                result = mSIPPhone.CallOrAnswer(PhoneLineID, CallTo);
#if (TRACE)
                Console.WriteLine("CallOrAnswer : " + result.ToString());
#endif
            }
        }


        private delegate void ShutdownEngineDelegate();
        private void ShutdownEngine()
        {
            if (this.InvokeRequired)
            {
                BeginInvoke(new ShutdownEngineDelegate(ShutdownEngine));
            }
            else
            {
                mSIPPhone.ShutdownEngine();
            }
        }

        private delegate void CancelCallDelegate(int PhoneLineID);
        private void CancelCall(int PhoneLineID)
        {
            if (this.InvokeRequired)
            {
                BeginInvoke(new CancelCallDelegate(CancelCall), new object[] { PhoneLineID });
            }
            else
            {
                mSIPPhone.CancelCall(PhoneLineID);
            }
        }

        private delegate void ForwardCallDelegate(int PhoneLineID, String CallTo);
        private void ForwardCall(int PhoneLineID, String CallTo)
        {
            if (this.InvokeRequired)
            {
                BeginInvoke(new ForwardCallDelegate(PlaceCall), new object[] { PhoneLineID, CallTo });
            }
            else
            {
                TELEPHONY_RETURN_VALUE result;
                result = mSIPPhone.CallOrAnswer(PhoneLineID, CallTo);
            }
        }

        private delegate void CallOnHookDelegate(int PhoneLineID);
        private void CallOnHook(int PhoneLineID)
        {
            if (this.InvokeRequired)
            {
                BeginInvoke(new CallOnHookDelegate(CallOnHook), new object[] { PhoneLineID });
            }
            else
            {
                mySIPPhoneLines[PhoneLineID].OnHook(myCallHistoryRecords);
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
                myNotifyIcon.ShowBalloonTip(10,
                              Properties.Localization.txtInfoIncomingCall,
                              String.Format(Properties.Localization.txtInfoIncomingCallDesc, CallFrom),
                              ToolTipIcon.Info);

                myDialPadTabControl.SelectedTab = myTabItemDialPad;
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
                        myDestinationTextBox.Text = mySIPPhoneLines[PhoneLineID].LastEnteredNumber;

                        //update buttons
                        //conf
                        myPhoneCONFButton.Image = mySIPPhoneLines[PhoneLineID].CallConferenceActive ? Properties.Resources.ButtonOn : Properties.Resources.ButtonOff;



                        myPhoneCONFButton.Items[0].Enabled = mySIPPhoneLines[0].CallActive;
                        myPhoneCONFButton.Items[1].Enabled = mySIPPhoneLines[1].CallActive;
                        myPhoneCONFButton.Items[2].Enabled = mySIPPhoneLines[2].CallActive;
                        myPhoneCONFButton.Items[3].Enabled = mySIPPhoneLines[3].CallActive;

                        myPhoneCONFButton.Items[0].Text = mySIPPhoneLines[0].CallActive ? Properties.Localization.txtTabLine0 + " " + mySIPPhoneLines[0].LastDialedNumber : Properties.Localization.txtTabLine0 + " " + Properties.Localization.txtInfoLineFree;
                        myPhoneCONFButton.Items[1].Text = mySIPPhoneLines[1].CallActive ? Properties.Localization.txtTabLine1 + " " + mySIPPhoneLines[1].LastDialedNumber : Properties.Localization.txtTabLine1 + " " + Properties.Localization.txtInfoLineFree;
                        myPhoneCONFButton.Items[2].Text = mySIPPhoneLines[2].CallActive ? Properties.Localization.txtTabLine2 + " " + mySIPPhoneLines[2].LastDialedNumber : Properties.Localization.txtTabLine2 + " " + Properties.Localization.txtInfoLineFree;
                        myPhoneCONFButton.Items[3].Text = mySIPPhoneLines[3].CallActive ? Properties.Localization.txtTabLine3 + " " + mySIPPhoneLines[3].LastDialedNumber : Properties.Localization.txtTabLine3 + " " + Properties.Localization.txtInfoLineFree;


                        ((RadMenuItem)(myPhoneCONFButton.Items[0])).IsChecked = mySIPPhoneLines[0].CallConferenceActive;
                        ((RadMenuItem)(myPhoneCONFButton.Items[1])).IsChecked = mySIPPhoneLines[1].CallConferenceActive;
                        ((RadMenuItem)(myPhoneCONFButton.Items[2])).IsChecked = mySIPPhoneLines[2].CallConferenceActive;
                        ((RadMenuItem)(myPhoneCONFButton.Items[3])).IsChecked = mySIPPhoneLines[3].CallConferenceActive;

                        //hold

                        myPhoneHOLDButton.Items[0].Enabled = mySIPPhoneLines[0].CallActive;
                        myPhoneHOLDButton.Items[1].Enabled = mySIPPhoneLines[1].CallActive;
                        myPhoneHOLDButton.Items[2].Enabled = mySIPPhoneLines[2].CallActive;
                        myPhoneHOLDButton.Items[3].Enabled = mySIPPhoneLines[3].CallActive;

                        myPhoneHOLDButton.Items[0].Text = mySIPPhoneLines[0].CallActive ? Properties.Localization.txtTabLine0 + " " + mySIPPhoneLines[0].LastDialedNumber : Properties.Localization.txtTabLine0 + " " + Properties.Localization.txtInfoLineFree;
                        myPhoneHOLDButton.Items[1].Text = mySIPPhoneLines[1].CallActive ? Properties.Localization.txtTabLine1 + " " + mySIPPhoneLines[1].LastDialedNumber : Properties.Localization.txtTabLine1 + " " + Properties.Localization.txtInfoLineFree;
                        myPhoneHOLDButton.Items[2].Text = mySIPPhoneLines[2].CallActive ? Properties.Localization.txtTabLine2 + " " + mySIPPhoneLines[2].LastDialedNumber : Properties.Localization.txtTabLine2 + " " + Properties.Localization.txtInfoLineFree;
                        myPhoneHOLDButton.Items[3].Text = mySIPPhoneLines[3].CallActive ? Properties.Localization.txtTabLine3 + " " + mySIPPhoneLines[3].LastDialedNumber : Properties.Localization.txtTabLine3 + " " + Properties.Localization.txtInfoLineFree;


                        ((RadMenuItem)(myPhoneHOLDButton.Items[0])).IsChecked = mySIPPhoneLines[0].CallHoldActive;
                        ((RadMenuItem)(myPhoneHOLDButton.Items[1])).IsChecked = mySIPPhoneLines[1].CallHoldActive;
                        ((RadMenuItem)(myPhoneHOLDButton.Items[2])).IsChecked = mySIPPhoneLines[2].CallHoldActive;
                        ((RadMenuItem)(myPhoneHOLDButton.Items[3])).IsChecked = mySIPPhoneLines[3].CallHoldActive;




                        //rec
                        myPhoneLineRECButton.Enabled = mySIPPhoneLines[PhoneLineID].CallActive;
                        myPhoneLineRECButton.Image = mySIPPhoneLines[PhoneLineID].CallRecordingActive ? Properties.Resources.ButtonOn : Properties.Resources.ButtonOff;

                        //xfer
                        myPhoneLineXFERButton.Enabled = mySIPPhoneLines[PhoneLineID].CallActive;
                        myPhoneLineXFERButton.Image = mySIPPhoneLines[PhoneLineID].CallTransferActive ? Properties.Resources.ButtonOn : Properties.Resources.ButtonOff;






                        switch (mySIPPhoneLines[PhoneLineID].State)
                        {

                            case TELEPHONY_RETURN_VALUE.SipIncomingCallStart:
                                myDialPadInfoStatusValueLabel.Text = Properties.Localization.txtInfoLineIncomingCall;
                                myDialPadInfoCallingLabel.Text = Properties.Localization.txtInfoLineCallFrom;
                                myDialPadInfoCallingValueLabel.Text = mySIPPhoneLines[PhoneLineID].LastDialedNumber;
                                myDialPadInfoCallingValueLabel.Visible = true;
                                myDialPadInfoCallingLabel.Visible = true;
                                break;
                            case TELEPHONY_RETURN_VALUE.SipOutgoingCallStart:
                            case TELEPHONY_RETURN_VALUE.SipDialing:
                                myDialPadInfoStatusValueLabel.Text = Properties.Localization.txtInfoLineDialing;
                                myDialPadInfoCallingLabel.Text = Properties.Localization.txtInfoLineCalling;
                                myDialPadInfoCallingValueLabel.Text = mySIPPhoneLines[PhoneLineID].LastDialedNumber;
                                myDialPadInfoCallingValueLabel.Visible = true;
                                myDialPadInfoCallingLabel.Visible = true;
                                break;
                            case TELEPHONY_RETURN_VALUE.SipInCall:
                                myDialPadInfoStatusValueLabel.Text = Properties.Localization.txtInfoLineConnected;
                                myDialPadInfoCallingLabel.Text = Properties.Localization.txtInfoLineInCall;
                                myDialPadInfoCallingValueLabel.Text = mySIPPhoneLines[PhoneLineID].LastDialedNumber;
                                myDialPadInfoCallingValueLabel.Visible = true;
                                myDialPadInfoCallingLabel.Visible = true;
                                break;
                            case TELEPHONY_RETURN_VALUE.SipOnHook:
                                myDialPadInfoStatusValueLabel.Text = Properties.Localization.txtInfoLineReady;
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
                        result = mSIPPhone.StartDTMF(PhoneLineID, myDialedTone);
#if (TRACE)
                        Console.WriteLine("SIP StartDTMF :" + myDialedTone.ToString() + " - " + result.ToString());
#endif
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
                    result = mSIPPhone.StopDTMF(PhoneLineID);
#if (TRACE)
                    Console.WriteLine("SIP StopDTMF : " + result.ToString());
#endif
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


                if (mySIPPhoneLines[PhoneLineID].State == TELEPHONY_RETURN_VALUE.SipOnHook)
                {
                    myDestinationTextBox.Text = "";
                }
                else
                {
                    result = mSIPPhone.CancelCall(PhoneLineID);
#if (TRACE)
                    Console.WriteLine("myDialPadCallCancelButton_Click : " + result.ToString());
#endif
                }

                //cancel also video if its for same user
                if (mySIPPhoneLines[PhoneLineID].LastDialedNumber == myVideoPlugin.JabberID)
                {
                    myVideoPlugin.StopConference();
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
                int PhoneLineID = 0;
                if (myPhoneLinesTabControl.SelectedTab != null)
                {
                    PhoneLineID = Int32.Parse(myPhoneLinesTabControl.SelectedTab.Tag.ToString());
                }
                else
                {
                    PhoneLineID = mSIPPhone.GetFreePhoneLine();
                }
                StartNewCall(PhoneLineID, myDestinationTextBox.Text);

            }
            catch (Exception)
            {
                return;
            }
        }


        private void myPhoneLineCONFButton_Click(object sender, EventArgs e)
        {
            //NOT USED
        }

        private void myPhoneLineHOLDButton_Click(object sender, EventArgs e)
        {
            //NOT USED SEE INITIALIZATION OF MENU BUTTONS
        }

        private void myPhoneLineXFERButton_Click(object sender, EventArgs e)
        {
            TELEPHONY_RETURN_VALUE result;
            try
            {
                int PhoneLineID = Int32.Parse(myPhoneLinesTabControl.SelectedTab.Tag.ToString());
                result = mSIPPhone.XferCall(PhoneLineID, "sip:" + myDestinationTextBox.Text + "@" + ConfigSIP.Realm);
#if (TRACE)
                Console.WriteLine("SIP|XFER:" + result.ToString());
#endif

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
                result = mSIPPhone.RecOnOffCall(PhoneLineID, Environment.GetFolderPath(Environment.SpecialFolder.Desktop));
                if (result == TELEPHONY_RETURN_VALUE.SipSuccess)
                {
                    mySIPPhoneLines[PhoneLineID].CallRecordingActive = !mySIPPhoneLines[PhoneLineID].CallRecordingActive;
                }
#if (TRACE)
                Console.WriteLine("SIP|REC:" + result.ToString());
#endif
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
                    myDialPadTabControl.SelectedTab = myTabItemDialPad;
                    myDialPadCallOrAnswerButton.PerformClick();
                    return;
                }
                int PhoneLineID = Int32.Parse(myPhoneLinesTabControl.SelectedTab.Tag.ToString());
                mySIPPhoneLines[PhoneLineID].LastEnteredNumber = myDestinationTextBox.Text;
            }
            catch (Exception)
            {
                return;
            }
        }

        #endregion

        private Telerik.WinControls.UI.RadListBoxItem BuildContactItem(NTContact contact)
        {

            this.tmplContactListItem = new Telerik.WinControls.UI.RadListBoxItem();

            JabberUser jabberUser = new JabberUser(contact.NTJabberID);

            this.tmplContactListItem.AutoSize = true;
            this.tmplContactListItem.AutoSizeMode = Telerik.WinControls.RadAutoSizeMode.Auto;

            // 
            // tmplContactListItem
            // 
            this.tmplContactListItem.AccessibleDescription = contact.FullName().Length > 64 ? contact.FullName().Trim().Substring(0, 64) : contact.FullName().Trim();
            this.tmplContactListItem.CanFocus = true;
            this.tmplContactListItem.DescriptionText = "» " + (contact.PrimaryPhoneNumbers().Length > 64 ? contact.PrimaryPhoneNumbers().Trim().Substring(0, 64) : contact.PrimaryPhoneNumbers().Trim());
            this.tmplContactListItem.ForeColor = System.Drawing.Color.Black;
            this.tmplContactListItem.ImageAlignment = ContentAlignment.MiddleCenter;
            try
            {
                if (contact.NTPicture != null & contact.NTPicture.Length > 0) this.tmplContactListItem.Image = ImageProcessing.FixedSize(ImageProcessing.FromString(contact.NTPicture), 42, 42);
            }
            catch (Exception)
            {

            }

            if (this.tmplContactListItem.Image == null) this.tmplContactListItem.Image = ImageProcessing.FixedSize(((System.Drawing.Image)(Properties.Resources.ContactBlank)), 42, 42);

            this.tmplContactListItem.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            this.tmplContactListItem.Text = "» " + (contact.FullName().Length > 64 ? contact.FullName().Trim().Substring(0, 64) : contact.FullName().Trim());
            this.tmplContactListItem.Text += jabberUser.Nick.Trim() != "" ? " (" + (jabberUser.Nick.Trim().Length > 64 ? jabberUser.Nick.Trim().Substring(0, 64) : jabberUser.Nick.Trim()) + ")" : "";
            this.tmplContactListItem.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.tmplContactListItem.TextSeparatorVisibility = Telerik.WinControls.ElementVisibility.Visible;
            this.tmplContactListItem.ToolTipText = null;
            this.tmplContactListItem.Font = new System.Drawing.Font("Trebuchet MS", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tmplContactListItem.ForeColor = System.Drawing.Color.Black;
            this.tmplContactListItem.DescriptionFont = new System.Drawing.Font("Trebuchet MS", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tmplContactListItem.TextSeparatorVisibility = Telerik.WinControls.ElementVisibility.Visible;
            this.tmplContactListItem.DoubleClick += new EventHandler(tmplContactListItem_DoubleClick);
            this.tmplContactListItem.Tag = contact;

            return tmplContactListItem;
        }

        private void LoadContactsBook()
        {
            try
            {
                myContactsListBox.Items.Clear();

                ContactList contactList = new ContactList(); ;
                Boolean filter = false;
                if (myContactsShow1ABButton.Checked)
                {
                    filter = true;
                    contactList.AddRange(mContactBook.getCandidatesForName(new string[] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "0", "A", "B" }));
                }

                if (myContactsShowCDEButton.Checked)
                {
                    filter = true;
                    contactList.AddRange(mContactBook.getCandidatesForName(new string[] { "C", "D", "E" }));
                }

                if (myContactsShowFGHButton.Checked)
                {
                    filter = true;
                    contactList.AddRange(mContactBook.getCandidatesForName(new string[] { "F", "G", "H" }));
                }

                if (myContactsShowIJKButton.Checked)
                {
                    filter = true;
                    contactList.AddRange(mContactBook.getCandidatesForName(new string[] { "I", "J", "K" }));
                }

                if (myContactsShowOPQButton.Checked)
                {
                    contactList.AddRange(mContactBook.getCandidatesForName(new string[] { "O", "P", "Q" }));
                }

                if (myContactsShowRSTButton.Checked)
                {
                    filter = true;
                    contactList.AddRange(mContactBook.getCandidatesForName(new string[] { "R", "S", "T" }));
                }
                if (myContactsShowLMNButton.Checked)
                {
                    filter = true;
                    contactList.AddRange(mContactBook.getCandidatesForName(new string[] { "L", "M", "N" }));
                }

                if (myContactsShowUVWButton.Checked)
                {
                    contactList.AddRange(mContactBook.getCandidatesForName(new string[] { "U", "V", "W" }));
                }

                if (myContactsShowXYZButton.Checked)
                {
                    filter = true;
                    contactList.AddRange(mContactBook.getCandidatesForName(new string[] { "X", "Y", "Z" }));
                }

                if (!filter) contactList = mContactBook.getContactList();

                if (contactList == null | contactList.Count == 0) return;

                lock (contactList)
                {
                    try
                    {
                        for (int i = 0; i < contactList.Count; i++)
                        {
                            if (contactList[i].NTDeleted == "true") continue;
                            this.myContactsListBox.Items.AddRange(new Telerik.WinControls.RadItem[] {
                    BuildContactItem(contactList[i])});
                        }
                    }
                    catch (Exception)
                    {
                        //                    throw;
                    }

                }
                if (myContactsListBox.Items.Count > 0) myContactsListBox.SelectedIndex = 0;
            }
            catch (Exception)
            {
                //   throw;
            }
        }

        void tmplContactListItem_DoubleClick(object sender, EventArgs e)
        {
            Telerik.WinControls.UI.RadListBoxItem clickedItem = (Telerik.WinControls.UI.RadListBoxItem)sender;
            OpenContactWindow((NTContact)clickedItem.Tag, null);
        }


        private void OpenContactWindow(NTContact selectedContact, Hashtable properties)
        {
            try
            {
                myContactWindow = new ContactsWindow(this, selectedContact, properties);
                if (properties != null && properties.ContainsKey("NTJabberID")) mXMPPIQ.RequestVCard(new JabberUser(properties["NTJabberID"].ToString()), true);
                myContactWindow.FormClosing += new FormClosingEventHandler(myContactWindow_FormClosing);
                myContactWindow.FormClosed += new FormClosedEventHandler(myContactWindow_FormClosed);
                myContactWindow.Show();
            }
            catch (Exception)
            {
                //   throw;
            }
        }

        void myContactWindow_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (this.mReloadContactsList)
            {
                this.mReloadContactsList = false;
            }

        }

        private void myContactsListBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (myContactsListBox.SelectedItem != null)
                {
                    Telerik.WinControls.UI.RadListBoxItem clickedItem = (Telerik.WinControls.UI.RadListBoxItem)myContactsListBox.SelectedItem;
                    OpenContactWindow((NTContact)clickedItem.Tag, null);
                }
            }
            else if (e.KeyCode == Keys.Delete)
            {
                RadListBoxItem selectedItem = (RadListBoxItem)myContactsListBox.SelectedItem;
                if (selectedItem != null)
                {
                    NTContact selectedContact = (NTContact)selectedItem.Tag;
                    selectedContact.NTDeleted = "true";
                    selectedContact.NTContactChanged = true;
                    mContactBook.Modified();
                }
            }
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
#if (TRACE)
                                Console.WriteLine("NETWORK-Detecting:" + thisIpAddress.ToString());
#endif
                                IPAddress remoteHostIP = System.Net.Dns.GetHostEntry(ConfigTestNetwork.HostIp).AddressList[0];
                                IPEndPoint remoteEndPoint = new IPEndPoint(remoteHostIP, ConfigTestNetwork.HostPort);
                                Socket remoteSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                                remoteSocket.Connect(remoteEndPoint);
                                myNetworkInfo.LocalIP = remoteSocket.LocalEndPoint.ToString().Split(new char[1] { ':' }, StringSplitOptions.RemoveEmptyEntries)[0];
                                myNetworkInfo.Online = true;
                                remoteSocket.Disconnect(false);
#if (TRACE)
                                Console.WriteLine("NETWORK-Enabled:" + myNetworkInfo.LocalIP);
#endif
                                return;
                            }
                            catch (Exception ex)
                            {
#if (TRACE)
                                Console.WriteLine("NETWORK-Error:" + thisIpAddress.ToString() + " Error Message:" + ex.Message);
#endif
                            }
                        }
                    }
                }


            }
            catch (Exception)
            {
                return;
            }
        }

        private void LoggingInProgress(bool inProgress)
        {
            if (inProgress)
            {
                myLoginWaitingBar.StartWaiting();
                // this.Cursor = Cursors.WaitCursor;
            }
            else
            {
                myLoginWaitingBar.EndWaiting();
                // this.Cursor = Cursors.Default;
            }

            myLoginButton.Enabled = !inProgress;
            myLoginUsernameInput.Enabled = !inProgress;
            myLoginPasswordInput.Enabled = !inProgress;
            myLoginRememberMeCheckBox.Enabled = !inProgress;
            myLoginAutoLoginCheckBox.Enabled = !inProgress;
        }

        private void myLoginButton_Click(object sender, EventArgs e)
        {


            if (!mUserAccount.LoggedIn)
            {
                ResetUserInterface();

                if (NetworkInterface.GetIsNetworkAvailable())
                {
                    LoggingInProgress(true);
                    try
                    {
                        myClientConfiguration = myClientConfigurationSerializer.LoadClientConfiguration();
                        myClientConfiguration.SetLoginOptions(myLoginUsernameInput.Text, myLoginPasswordInput.Text, myLoginAutoLoginCheckBox.Checked, myLoginRememberMeCheckBox.Checked);
                        myClientConfigurationSerializer.SaveClientConfiguration(myClientConfiguration);
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

                        try
                        {
                            CallHistory tmpCallHistory = myCallHistorySerializer.LoadCallHistory(mUserAccount.Username);
                            if (tmpCallHistory.CallRecords != null)
                            {
                                myCallHistoryRecords.Clear();
                                myCallHistoryRecords.AddRange(tmpCallHistory.CallRecords);
                            }
                        }
                        catch (Exception ex)
                        {
#if (TRACE)
                            Console.WriteLine("CallHistory : Failed to load - " + ex.Message);
#endif
                        }
                        mContactBook.LoadAsync(mUserAccount);

                        getPrepaidStatus();
                    }
                    //open login panel and process login
                    LoggingInProgress(false);
                }
                else
                {
                    myNotifyIcon.ShowBalloonTip(10,
                        Properties.Localization.txtInfoServiceUnavailable,
                        Properties.Localization.txtInfoServiceUnavailableDesc,
                        ToolTipIcon.Warning);
                }
            }
        }

        private void ResetUserInterface()
        {
            // initialize phone lines states
            mySIPPhoneLines = new PhoneLineState[4];
            mySIPPhoneLines[0] = new PhoneLineState();
            mySIPPhoneLines[1] = new PhoneLineState();
            mySIPPhoneLines[2] = new PhoneLineState();
            mySIPPhoneLines[3] = new PhoneLineState();

            myRosterListTreeView.Nodes.Clear();

            myRosterContextMenuStrip.Enabled = false;
            myMenuChatToolStripButton.Enabled = false;
            myMenuCallToolStripButton.Enabled = false;
            myMenuVideoCallToolStripButton.Enabled = false;

            //select Default Phone Line Tab
            myPhoneLinesTabControl.SelectedTab = myPhoneLine0Tab;
            myDialPadTabControl.SelectedTab = myTabItemDialPad;

            myDestinationTextBox.Text = "";

            UpdatePhoneLineButtons();

            myContactsListBox.Items.Clear();
            myCallHistoryListBox.Items.Clear();

            myMainTabControl.SelectedTab = myMainOnlineTabPage;

        }

        internal bool SendJabberXevent(String jid, string jabberXevent)
        {
            try
            {

                if (xmppsControl.Connected)
                {
                    JabberUser jabberUser = new JabberUser(jid);
                    jabberXevent = jabberXevent.Trim();
                    if (jabberXevent.Length == 0) return false;
                    xmppsControl.MessageType = nsoftware.IPWorksSSL.XmppsMessageTypes.mtChat;
                    xmppsControl.OtherData = jabberXevent;
                    xmppsControl.SendMessage(jabberUser.EscapedJID);
                    return true;
                }
            }
            catch (Exception e)
            {
                if (Disposing) return false;
                MessageBox.Show
                    (
                    String.Format(Properties.Localization.txtInfoIMMessageSendErrorDesc, e.Message),
                    Properties.Localization.txtInfoIMMessageSendError,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation
                    );
            }
            return false;
        }

        internal bool SendMessage(JabberUser jabberUser, string messageText, string messageHTML)
        {
            try
            {
                if (xmppsControl.Connected)
                {
                    messageText = messageText.Trim();
                    messageHTML = messageHTML.Trim();

                    String escapedMessageText = messageText;
                    String escapedMessageHTML = messageHTML.Replace(@"&", @"&amp;").Replace(@"<", @"&lt;").Replace(@">", @"&gt;");
                    if (messageText.Length == 0 || messageHTML.Length == 0) return false;
                    xmppsControl.MessageText = escapedMessageText;
                    if (jabberUser.Network == ConfigXMPPNetwork.Default)
                    {
                        //xmppsControl.MessageHTML = escapedMessageHTML;
                    }

                    xmppsControl.MessageType = nsoftware.IPWorksSSL.XmppsMessageTypes.mtChat;
                    xmppsControl.SendMessage(jabberUser.EscapedJID);
                    myStorage.AddMessageToArchive(jabberUser.JID, myStorage.StorageGUID(), messageText, messageHTML, true);

                    return true;
                }
            }
            catch (Exception e)
            {
                if (Disposing) return false;
                MessageBox.Show
                     (
                     String.Format(Properties.Localization.txtInfoIMMessageSendErrorDesc, e.Message),
                     Properties.Localization.txtInfoIMMessageSendError,
                     MessageBoxButtons.OK,
                     MessageBoxIcon.Exclamation
                     );
            }
            return false;
        }

        #region XmppControl Events

        private int GetBuddyId(String jid)
        {
            for (int i = 0; i < xmppsControl.BuddyCount; i++)
            {
                if (xmppsControl.BuddyId[i + 1] == jid) return i;
            }
            return 0;
        }

        private TreeNode RosterGetGroupNode(TreeNodeCollection nodes, String group)
        {
            try
            {

                foreach (TreeNode node in nodes)
                {
                    if (node.Level == 0 && node.Text == group) return node;
                }

            }
            catch (Exception ex)
            {
#if TRACE
                Console.WriteLine("RosterGetGroupNode : " + ex.Message);

#endif
            }
            return null;
        }

        private TreeNode RosterGetBuddyNode(TreeNodeCollection nodes, String jid)
        {
            if (nodes.Count > 0)
            {
                return RosterGetBuddyNode(nodes[0], jid);
            }
            return null;
        }
        private TreeNode RosterGetBuddyNode(TreeNode node, String jid)
        {
            if (node != null)
            {
                if (node.Tag != null && node.Tag.ToString() == jid)
                {
                    return node;
                }
                else
                {
                    if (node.FirstNode != null)
                    {
                        TreeNode newnode = RosterGetBuddyNode(node.FirstNode, jid);
                        if (newnode != null)
                        {
                            return newnode;
                        }
                        else if (node.NextNode != null)
                        {
                            return RosterGetBuddyNode(node.NextNode, jid);
                        }
                    }
                    else if (node.NextNode != null)
                    {
                        return RosterGetBuddyNode(node.NextNode, jid);
                    }
                    return null;
                }
            }
            return null;
        }
        private string RosterGetCurrentGroup()
        {
            TreeNode node = myRosterListTreeView.SelectedNode;
            if (node == null) return "";

            if (node.Level == 0)
            {
                if (myBuddyGroups.Contains(node.Text)) return node.Text;
            }
            else if (node.Level == 1)
            {
                if (myBuddyGroups.Contains(node.Parent.Text)) return node.Parent.Text;
            }
            return "";
        }

        internal ImageList getPresenceImageList()
        {
            ImageList il = new ImageList();
            il.ColorDepth = ColorDepth.Depth32Bit;
            il.Images.Add(Properties.Resources.statusOffline);
            il.Images.Add(Properties.Resources.statusOnline);
            il.Images.Add(Properties.Resources.statusAway);
            il.Images.Add(Properties.Resources.statusExtAway);
            il.Images.Add(Properties.Resources.statusBusy);
            il.Images.Add(Properties.Resources.NodeIconOpened);
            il.Images.Add(Properties.Resources.NodeIconClosed);
            il.Images.Add(Properties.Resources.NodeIconSelected);
            il.Images.Add(Properties.Resources.statusTyping);

            il.TransparentColor = System.Drawing.Color.Transparent;
            return il;
        }

        private delegate void ProbePresenceFunctionDelegate();
        private void ProbePresenceFunction()
        {
            if (this.InvokeRequired)
            {
                BeginInvoke(new ProbePresenceFunctionDelegate(ProbePresenceFunction));
                return;
            }

            try
            {
                //probe presence and subscribe
                Thread.Sleep(1000);
                for (int i = 1; i <= xmppsControl.BuddyCount; i++)
                {
                    JabberUser jabberUser = new JabberUser(xmppsControl.BuddyId[i]);

                    switch (xmppsControl.BuddySubscription[i])
                    {
                        case XmppsBuddySubscriptions.stNone:
                            if (!mUpdatedSync)
                            {
                                try
                                {
                                    xmppsControl.SubscribeTo(jabberUser.EscapedJID);
                                }
                                catch (Exception)
                                {

                                }
                            }
                            break;
                        case XmppsBuddySubscriptions.stTo:
                            try
                            {
                                xmppsControl.ProbePresence(jabberUser.EscapedJID);
                            }
                            catch (Exception)
                            {

                            }

                            break;
                        case XmppsBuddySubscriptions.stFrom:
                            if (!mUpdatedSync)
                            {
                                try
                                {
                                    xmppsControl.SubscribeTo(jabberUser.EscapedJID);
                                }
                                catch (Exception)
                                {

                                }
                            }
                            break;
                        case XmppsBuddySubscriptions.stBoth:
                            try
                            {
                                xmppsControl.ProbePresence(jabberUser.EscapedJID);
                            }
                            catch (Exception)
                            {

                            }
                            break;
                        case XmppsBuddySubscriptions.stRemove:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
#if (TRACE)
                Console.WriteLine("probePresenceFunction : " + ex.Message);
#endif
            }
            mUpdatedSync = true;
        }

        // xmppControl Evevnt handling
        private delegate void xmppControl_OnSSLStatusDelegate(object sender, XmppsSSLStatusEventArgs e);
        private void xmppControl_OnSSLStatus(object sender, XmppsSSLStatusEventArgs e)
        {
            if (this.InvokeRequired)
            {
                BeginInvoke(new xmppControl_OnSSLStatusDelegate(xmppControl_OnSSLStatus), new object[] { sender, e });
                return;
            }
#if (TRACE)
            Console.WriteLine("OnSSLStatus:" + "(" + e.Message + ")");
#endif

        }

        private delegate void xmppControl_OnSSLServerAuthenticationDelegate(object sender, XmppsSSLServerAuthenticationEventArgs e);
        private void xmppControl_OnSSLServerAuthentication(object sender, XmppsSSLServerAuthenticationEventArgs e)
        {
            if (this.InvokeRequired)
            {
                BeginInvoke(new xmppControl_OnSSLServerAuthenticationDelegate(xmppControl_OnSSLServerAuthentication), new object[] { sender, e });
                return;
            }
            e.Accept = true;
#if (TRACE)
            Console.WriteLine("OnSSLServerAuthentication:" + "(" + e.CertIssuer + "|" + e.CertSubject + ")");
#endif

        }

        private delegate void xmppControl_OnSubscriptionRequestDelegate(object sender, XmppsSubscriptionRequestEventArgs e);
        private void xmppControl_OnSubscriptionRequest(object sender, XmppsSubscriptionRequestEventArgs e)
        {
            if (this.InvokeRequired)
            {
                BeginInvoke(new xmppControl_OnSubscriptionRequestDelegate(xmppControl_OnSubscriptionRequest), new object[] { sender, e });
                return;
            }

            try
            {
                if (e == null || !xmppsControl.Connected) return;
#if (TRACE)
                Console.WriteLine("IM-SubscriptionRequest:" + "(" + e.From + ")");
#endif
                JabberUser jabberUser = new JabberUser(e.From + @"@" + e.Domain);
                if (mContactBook.getCandidatesForJabberID(jabberUser.JID).Count > 0)
                {
                    try
                    {
                        e.Accept = true;
                        mXMPPIQ.Subscibe(jabberUser);
                        xmppsControl.SubscribeTo(jabberUser.EscapedJID);
                    }
                    catch (Exception)
                    {

                    }
                }
                else
                {
                    if (Disposing) return;
                    if (MessageBox.Show(
                        String.Format(Properties.Localization.txtInfoIMSubcriptionRequestDesc, jabberUser.Username),
                        Properties.Localization.txtInfoIMSubcriptionRequest,
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question,
                        MessageBoxDefaultButton.Button1
                        ) == DialogResult.Yes)
                    {
                        try
                        {
                            e.Accept = true;
                            mXMPPIQ.Subscibe(jabberUser);
                            xmppsControl.SubscribeTo(jabberUser.EscapedJID);
                        }
                        catch (Exception)
                        {

                        }
                        if (Disposing) return;
                        if (MessageBox.Show(
                           String.Format(Properties.Localization.txtInfoAddToContactDesc, jabberUser.Username),
                           Properties.Localization.txtInfoAddToContact,
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Question,
                            MessageBoxDefaultButton.Button1
                            ) == DialogResult.Yes)
                        {
                            Hashtable properties = new Hashtable();
                            properties.Add("NTJabberID", jabberUser.JID);
                            OpenContactWindow(null, properties);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
#if (TRACE)
                Console.WriteLine("xmppControl_OnSubscriptionRequest : " + ex.Message);
#endif
            }
        }

        private delegate void xmppControl_OnSyncDelegate(object sender, XmppsSyncEventArgs e);
        private void xmppControl_OnSync(object sender, XmppsSyncEventArgs e)
        {
            if (this.InvokeRequired)
            {
                BeginInvoke(new xmppControl_OnSyncDelegate(xmppControl_OnSync), new object[] { sender, e });
                return;
            }
#if (TRACE)
            Console.WriteLine("IM-Sync:");
#endif
            try
            {
                myBuddyGroups.Clear();
                myBuddyList.Clear();
                //get all groups
                int i;
                for (i = 1; i <= xmppsControl.BuddyCount; i++)
                {
                    try
                    {
                        if (xmppsControl.BuddyGroup[i] != "")
                        {
                            foreach (string item in xmppsControl.BuddyGroup[i].ToString().Split(Char.Parse(",")))
                            {
                                if (!myBuddyGroups.Contains(item.Trim()))
                                {
                                    myBuddyGroups.Add(item, item.Trim());
                                }
                            }
                        }
                        else
                        {
                            if (!myBuddyGroups.Contains(Properties.Localization.txtOtherGroup)) myBuddyGroups.Add(Properties.Localization.txtOtherGroup, xmppsControl.BuddyGroup[i]);
                        }
                    }
                    catch (Exception)
                    {
#if (DEBUG)
                        throw;
#endif
                    }
                }
#if (TRACE)
                Console.WriteLine("IM-Sync: Got groups");
#endif
                //build roster
                foreach (DictionaryEntry group in myBuddyGroups)
                {

                    String groupName = group.Value.ToString() != "" ? group.Value.ToString() : Properties.Localization.txtOtherGroup;
                    TreeNode groupNode = RosterGetGroupNode(myRosterListTreeView.Nodes, groupName);
                    //create only missing groups
                    if (groupNode == null)
                    {
                        groupNode = myRosterListTreeView.Nodes.Add(groupName);
                        groupNode.Expand();
                    }

                    groupNode.ImageIndex = 5; //group icon
                    groupNode.SelectedImageIndex = 7;

                    //add buddies to groups
                    for (i = 1; i <= xmppsControl.BuddyCount; i++)
                    {
                        foreach (string item in xmppsControl.BuddyGroup[i].Split(Char.Parse(",")))
                        {
                            if (group.Value.ToString() == item.ToString())
                            {
                                //format all of the buddyid's properly (strip out domain, resource)
                                JabberUser jabberUser = new JabberUser(xmppsControl.BuddyId[i]);
                                TreeNode buddyNode = RosterGetBuddyNode(groupNode, jabberUser.JID);
                                if (buddyNode == null)
                                {
                                    buddyNode = new TreeNode(jabberUser.Nick);
                                    buddyNode.Tag = jabberUser.JID;
                                    if (myBuddyPresence.Contains(jabberUser.JID))
                                    {
                                        buddyNode.ImageIndex = (int)myBuddyPresence[jabberUser.JID];
                                    }
                                    else
                                    {
                                        buddyNode.ImageIndex = 0;
                                    }
                                    groupNode.Nodes.Add(buddyNode);
                                    groupNode.ImageIndex = 5;
                                    groupNode.SelectedImageIndex = 7;
                                    groupNode.Expand();
                                }

                                if (!myBuddyList.Contains(jabberUser.JID))
                                {
                                    if (xmppsControl.BuddyGroup[i] != "")
                                    {
                                        myBuddyList.Add(jabberUser.JID, xmppsControl.BuddyGroup[i]);
                                    }
                                    else
                                    {
                                        myBuddyList.Add(jabberUser.JID, Properties.Localization.txtOtherGroup);
                                    }
                                }
                                else if (myBuddyList[jabberUser.JID].ToString() != xmppsControl.BuddyGroup[i].ToString())
                                {
                                    if (xmppsControl.BuddyGroup[i] != "")
                                    {
                                        myBuddyList[jabberUser.JID] = xmppsControl.BuddyGroup[i];
                                    }
                                    else
                                    {
                                        myBuddyList[jabberUser.JID] = Properties.Localization.txtOtherGroup;
                                    }
                                }
                            }
                        }
                    }
                }

#if (TRACE)
                Console.WriteLine("IM-Sync: Updated roster");
#endif
                //cleanup
                foreach (TreeNode groupNode in myRosterListTreeView.Nodes)
                {
                    if (groupNode.Level == 0)
                    {
                        //group does not exists
                        if (!myBuddyGroups.Contains(groupNode.Text))
                        {
                            groupNode.Remove();
                        }
                        else
                        {
                            foreach (TreeNode node in groupNode.Nodes)
                            {
                                if (node.Level == 1)
                                {
                                    if (!myBuddyList.Contains(node.Tag.ToString()))
                                    {
                                        node.Remove();
                                    }
                                    else if (!myBuddyList[node.Tag.ToString()].ToString().Contains(node.Parent.Text))
                                    {
                                        node.Remove();
                                    }
                                }
                            }
                        }
                    }
                }
#if (TRACE)
                Console.WriteLine("IM-Sync: clean up");
#endif

                ThreadStart st = new ThreadStart(ProbePresenceFunction);
                probePresenceThread = new Thread(st);
                // start the thread
                probePresenceThread.Start();

#if (TRACE)
                Console.WriteLine("IM-Sync: started presence scan");
#endif
            }
            catch (Exception ex)
            {
#if (DEBUG)
                throw;
#endif
#if (TRACE)
                Console.WriteLine("xmppControl_OnSync : " + ex.Message);
#endif

            }
        }

        private void myRosterListTreeView_AfterExpand(object sender, TreeViewEventArgs e)
        {
            e.Node.ImageIndex = 5;
        }

        private void myRosterListTreeView_AfterCollapse(object sender, TreeViewEventArgs e)
        {
            e.Node.ImageIndex = 6;
        }
        private void myRosterListTreeView_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            e.Cancel = false;
            e.Node.ImageIndex = 5;
        }

        private void myRosterListTreeView_BeforeCollapse(object sender, TreeViewCancelEventArgs e)
        {
            e.Cancel = false;
            e.Node.ImageIndex = 6;
        }
        private delegate void xmppControl_OnPresenceDelegate(object sender, XmppsPresenceEventArgs e);
        private void xmppControl_OnPresence(object sender, XmppsPresenceEventArgs e)
        {
            if (this.InvokeRequired)
            {
                BeginInvoke(new xmppControl_OnPresenceDelegate(xmppControl_OnPresence), new object[] { sender, e });
                return;
            }

            JabberUser jaberUser = new JabberUser(e.User + "@" + e.Domain);
#if (TRACE)
            Console.WriteLine("IM-Presence:" + "(" + jaberUser.JID + ")" + e.Availability + "|" + e.Status);
#endif

            try
            {

                //update the roster imageindex for this buddy
                TreeNode node = RosterGetBuddyNode(myRosterListTreeView.Nodes, jaberUser.JID);
                if (node == null) return;

                if (myBuddyPresence[jaberUser.JID] == null || (int)myBuddyPresence[jaberUser.JID] != e.Availability)
                {
                    myBuddyPresence[jaberUser.JID] = e.Availability;
                    myChatWindow.AddNotification(
                        jaberUser.JID,
                        "PRESENCE",
                        String.Format(Properties.Localization.txtInfoPresenceNotificationDesc, new object[]{
                    jaberUser.Nick , PRESENCES[e.Availability] , e.Status}),
                        DateTime.Now,
                        Guid.NewGuid().ToString(),
                        false,
                        false
                        );
                    myChatWindow.UpdatePresence(jaberUser.JID, e.Availability);
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

                        node.Text = jaberUser.Nick + " (" + e.Status + ") ";
                    }
                }
                else
                {
                    myBuddyPresence[jaberUser.JID] = e.Availability;
                    if (node != null)
                    {
                        String statusText = jaberUser.Nick + " (" + e.Status + ") ";
                        if (node.Text != statusText) node.Text = statusText;
                    }
                }

            }
            catch (Exception ex)
            {
#if (DEBUG)
                throw;
#endif
#if (TRACE)
                Console.WriteLine("IM-Presence:" + ex.Message);
#endif

            }



        }

        private delegate void xmppControl_OnBuddyUpdateDelegate(object sender, XmppsBuddyUpdateEventArgs e);
        private void xmppControl_OnBuddyUpdate(object sender, XmppsBuddyUpdateEventArgs e)
        {
            if (this.InvokeRequired)
            {
                BeginInvoke(new xmppControl_OnBuddyUpdateDelegate(xmppControl_OnBuddyUpdate), new object[] { sender, e });
                return;
            }

            try
            {
#if (TRACE)
                Console.WriteLine("IM-BuddyUpdate:" + "(" + e.BuddyIdx.ToString() + ")");
#endif
                //stNone (0) no subscription  
                //stTo (1) the buddy has a subscription to this entity.  
                //stFrom (2) this entity has a subscription to the buddy  
                //stBoth (3) subscription is both to and from  
                //stRemove (4) the item is to be removed from the list 
                /*
                                myChatWindow.AddNotification(
                                    xmppControl.BuddyId[e.BuddyIdx], 
                                    "BUDDYUPDATE", 
                                    xmppControl.BuddyId[e.BuddyIdx] + " is " + xmppControl.BuddySubscription[e.BuddyIdx].ToString(), 
                                    "", 
                                    Guid.NewGuid().ToString(), 
                                    false);
                 */
            }
            catch (Exception ex)
            {
#if (TRACE)
                Console.WriteLine("xmppControl_OnBuddyUpdate : " + ex.Message);
#endif
            }
        }

        private delegate void xmppControl_OnMessageInDelegate(object sender, XmppsMessageInEventArgs e);
        private void xmppControl_OnMessageIn(object sender, XmppsMessageInEventArgs e)
        {
            if (this.InvokeRequired)
            {
                BeginInvoke(new xmppControl_OnMessageInDelegate(xmppControl_OnMessageIn), new object[] { sender, e });
                return;
            }

            try
            {
                //e.Domain, e.From, e.MessageHTML, e.MessageText, e.Resource
                String messageText = e.MessageText == null ? "" : e.MessageText.Trim().Replace(@"&", @"&amp;").Replace(@"<", @"&lt;").Replace(@">", @"&gt;");
                String messageHTML = e.MessageHTML == null || e.MessageHTML == "" ? messageText : e.MessageHTML.Trim();

                if (e.Domain == "")
                {//INFO
                    if (messageText != "")
                    {
                        myChatWindow.AddNotification(Properties.Localization.txtInfoIMNotification, "INFO", messageText, DateTime.Now, Guid.NewGuid().ToString(), false, false);
                    }
                }
                else
                {//MESSAGE
                    String jid = e.From + "@" + e.Domain;
                    if (messageText != "" || messageHTML != "")
                    {
                        myChatWindow.IncomingMessage(jid, messageText, messageHTML);
                    }
                }
            }
            catch (Exception ex)
            {
#if (TRACE)
                Console.WriteLine("xmppControl_OnMessageIn : " + ex.Message);
#endif
            }
        }

        private delegate void xmppControl_OnDisconnectedDelegate(object sender, XmppsDisconnectedEventArgs e);
        private void xmppControl_OnDisconnected(object sender, XmppsDisconnectedEventArgs e)
        {
            if (this.InvokeRequired)
            {
                BeginInvoke(new xmppControl_OnDisconnectedDelegate(xmppControl_OnDisconnected), new object[] { sender, e });
                return;
            }
#if (TRACE)
            Console.WriteLine("xmppControl_OnDisconnected:" + "(" + e.StatusCode + ")" + e.StatusCode);
            mXMPPIQ.Disconnected();
#endif
            myToolStripNetworkStatus.Image = Properties.Resources.networkDisconnected;
            if ((!this.mFormIsClosing) && (!xmppsControl.Connected) && (myNetworkInfo.Online) && (mUserAccount.LoggedIn))
            {
                //network detected, try authenticate at jabber
                try
                {
                    if (((DateTime)myServiceStateCheckTimer.Tag) < DateTime.Now.AddMinutes(-1))
                    {
#if (TRACE)
                        Console.WriteLine("xmppControl_OnDisconnected:Trying to ReConnect.");
#endif
                        xmppsControl.Connect(mUserAccount.Username, mUserAccount.Password);
                    }
                    else
                    {
#if (TRACE)
                        Console.WriteLine("xmppControl_OnDisconnected:Skiping ReConnection.");
#endif
                    }
                }
                catch (IPWorksSSLException ex)
                {
#if (TRACE)
                    Console.WriteLine("xmppControl_OnDisconnected:ReConnecting:(" + ex.Code + "):" + ex.Message);
#endif
                }
            }
        }

        private delegate void xmppControl_OnConnectedDelegate(object sender, XmppsConnectedEventArgs e);
        private void xmppControl_OnConnected(object sender, XmppsConnectedEventArgs e)
        {
            if (this.InvokeRequired)
            {
                BeginInvoke(new xmppControl_OnConnectedDelegate(xmppControl_OnConnected), new object[] { sender, e });
                return;
            }
            myToolStripNetworkStatus.Image = Properties.Resources.networkConnected;
#if (TRACE)
            Console.WriteLine("IM-Connected:" + "(" + e.StatusCode + ")" + e.Description);
#endif
        }

        private delegate void xmppControl_OnConnectionStatusDelegate(object sender, XmppsConnectionStatusEventArgs e);
        private void xmppControl_OnConnectionStatus(object sender, XmppsConnectionStatusEventArgs e)
        {
            if (this.InvokeRequired)
            {
                BeginInvoke(new xmppControl_OnConnectionStatusDelegate(xmppControl_OnConnectionStatus), new object[] { sender, e });
                return;
            }
#if (TRACE)
            Console.WriteLine("IM-ConnectionStatus|" + e.ConnectionEvent + ": (" + e.StatusCode + ") " + e.Description);
#endif
        }

        private delegate void xmppControl_OnPITrailDelegate(object sender, XmppsPITrailEventArgs e);
        private void xmppControl_OnPITrail(object sender, XmppsPITrailEventArgs e)
        {
            if (this.InvokeRequired)
            {
                BeginInvoke(new xmppControl_OnPITrailDelegate(xmppControl_OnPITrail), new object[] { sender, e });
                return;
            }


            //Jabber X-Events are not supported by the component (they are not officially part of the protocol), however:
            /*                
                0 (Client) Pi originates from the client.  
                1 (Server) Pi originates from the server.  
                2 (Info) Pi is an informative message originating from within the component. 
             */
            if (IsXEvent(e.Pi)) HandleXEvent(e.Pi);


#if (TRACE)

            String from = "";
            switch (e.Direction)
            {
                case 0: from = "CLIENT"; break;
                case 1: from = "SERVER"; break;
                case 2: from = "INFO"; break;
            }
            Console.WriteLine("IM-PITrail | From: " + from + " [" + e.Pi + "]");

#endif
            if (e.Pi == "Login complete.")
            {//Only when login is complete we can initialize other IM Networks
                try
                {
                    mXMPPIQ.Session(ConfigIM.IMServer);
                    XMPP_RegisterIMNetworks();
                    XMPP_DiscoverIMNetworks();
                    XMPP_SendPresenceIMNetworks();
                    mXMPPIQ.RequestVCard(mUserAccount.JabberUser, false);
                    xmppsControl.RetrieveRoster();
                }
                catch (Exception)
                {

                }


            }
        }

        private delegate void xmppControl_OnErrorDelegate(object sender, XmppsErrorEventArgs e);
        private void xmppControl_OnError(object sender, XmppsErrorEventArgs e)
        {
            if (this.InvokeRequired)
            {
                BeginInvoke(new xmppControl_OnErrorDelegate(xmppControl_OnError), new object[] { sender, e });
                return;
            }
#if (TRACE)
            Console.WriteLine("IM-Error:" + "(" + e.ErrorCode + ")" + e.Description);
#endif
        }

        private delegate void xmppControl_OnIQDelegate(object sender, XmppsIQEventArgs e);
        private void xmppControl_OnIQ(object sender, XmppsIQEventArgs e)
        {
            if (this.InvokeRequired)
            {
                BeginInvoke(new xmppControl_OnIQDelegate(xmppControl_OnIQ), new object[] { sender, e });
                return;
            }
            mXMPPIQ.ProcessIQ(e.Iq);
        }

        private void XMPP_ChangePresence(XMPPIQ.PresenceShow presence)
        {
            //xmppsControl.ChangePresence((int)presence, PRESENCES[(int)presence]);
            mPresenceShow = presence;
            XMPP_SendPresenceIMNetworks();

        }

        private void XMPP_DiscoverIMNetworks()
        {
            try
            {
                mXMPPIQ.DiscoInfo(ConfigIM.IMServer);
                mXMPPIQ.DiscoInfo(ConfigIM.MSN_IMServer);
                mXMPPIQ.DiscoInfo(ConfigIM.YAHOO_IMServer);
                mXMPPIQ.DiscoInfo(ConfigIM.GG_IMServer);
                mXMPPIQ.DiscoInfo(ConfigIM.IRC_IMServer);
                mXMPPIQ.DiscoInfo(ConfigIM.ICQ_IMServer);
                mXMPPIQ.DiscoInfo(ConfigIM.AIM_IMServer);
            }
            catch (Exception)
            {
#if (DEBUG)
                throw;
#endif
            }
        }

        private void XMPP_SendPresenceIMNetworks()
        {
            try
            {
                mXMPPIQ.SendPresence(ConfigIM.IMServer, mPresenceShow, PRESENCES[(int)mPresenceShow]);
                mXMPPIQ.SendPresence(ConfigIM.MSN_IMServer, mPresenceShow, PRESENCES[(int)mPresenceShow]);
                mXMPPIQ.SendPresence(ConfigIM.YAHOO_IMServer, mPresenceShow, PRESENCES[(int)mPresenceShow]);
                mXMPPIQ.SendPresence(ConfigIM.GG_IMServer, mPresenceShow, PRESENCES[(int)mPresenceShow]);
                mXMPPIQ.SendPresence(ConfigIM.IRC_IMServer, mPresenceShow, PRESENCES[(int)mPresenceShow]);
                mXMPPIQ.SendPresence(ConfigIM.ICQ_IMServer, mPresenceShow, PRESENCES[(int)mPresenceShow]);
                mXMPPIQ.SendPresence(ConfigIM.AIM_IMServer, mPresenceShow, PRESENCES[(int)mPresenceShow]);
            }
            catch (Exception)
            {
#if (DEBUG)
                throw;
#endif
            }
        }

        private void XMPP_RegisterIMNetworks()
        {
            // MSN, Yahoo, GaduGadu, IRC, AIM, ICQ
            //MSN
            mXMPPIQ.DiscoInfo(ConfigIM.MSN_IMServer);
            if (myClientConfiguration.MSN_Username != "" && myClientConfiguration.MSN_Password != "")
            {
                mXMPPIQ.RegisterUser(ConfigIM.MSN_IMServer, myClientConfiguration.MSN_Username, myClientConfiguration.MSN_Password);
                msnToolStripMenuItem.Image = Properties.Resources.im_msn;
            }
            else
            {
                msnToolStripMenuItem.Image = Properties.Resources.im_msn_offline;
            }

            //Yahoo
            mXMPPIQ.DiscoInfo(ConfigIM.YAHOO_IMServer);
            if (myClientConfiguration.Yahoo_Username != "" && myClientConfiguration.Yahoo_Password != "")
            {
                mXMPPIQ.RegisterUser(ConfigIM.YAHOO_IMServer, myClientConfiguration.Yahoo_Username, myClientConfiguration.Yahoo_Password);
                yahooToolStripMenuItem.Image = Properties.Resources.im_yahoo;
            }
            else
            {
                yahooToolStripMenuItem.Image = Properties.Resources.im_yahoo_offline;
            }

            //GaduGadu
            mXMPPIQ.DiscoInfo(ConfigIM.GG_IMServer);
            if (myClientConfiguration.GG_Username != "" && myClientConfiguration.GG_Password != "")
            {
                mXMPPIQ.RegisterUser(ConfigIM.GG_IMServer, myClientConfiguration.GG_Username, myClientConfiguration.GG_Password);
                ggToolStripMenuItem.Image = Properties.Resources.im_gadugadu;
            }
            else
            {
                ggToolStripMenuItem.Image = Properties.Resources.im_gadugadu_offline;
            }

            //IRC
            mXMPPIQ.DiscoInfo(ConfigIM.IRC_IMServer);
            if (myClientConfiguration.IRC_Username != "" && myClientConfiguration.IRC_Password != "")
            {
                mXMPPIQ.RegisterUser(ConfigIM.IRC_IMServer, myClientConfiguration.IRC_Username, myClientConfiguration.IRC_Password);
                //TODO
            }
            else
            {
                //TODO
            }

            //ICQ
            mXMPPIQ.DiscoInfo(ConfigIM.ICQ_IMServer);
            if (myClientConfiguration.ICQ_Username != "" && myClientConfiguration.ICQ_Password != "")
            {
                mXMPPIQ.RegisterUser(ConfigIM.ICQ_IMServer, myClientConfiguration.ICQ_Username, myClientConfiguration.ICQ_Password);
                icqToolStripMenuItem.Image = Properties.Resources.im_icq;
            }
            else
            {
                icqToolStripMenuItem.Image = Properties.Resources.im_icq_offline;
            }

            //AIM
            mXMPPIQ.DiscoInfo(ConfigIM.AIM_IMServer);
            if (myClientConfiguration.AIM_Username != "" && myClientConfiguration.AIM_Password != "")
            {
                mXMPPIQ.RegisterUser(ConfigIM.AIM_IMServer, myClientConfiguration.AIM_Username, myClientConfiguration.AIM_Password);
                aimToolStripMenuItem.Image = Properties.Resources.im_aim;
            }
            else
            {
                aimToolStripMenuItem.Image = Properties.Resources.im_aim_offline;
            }



        }
        #endregion

        #region "Jabber:X:Event"

        private bool IsXEvent(string Pi)
        {
            if (Pi.IndexOf("jabber:x:event") >= 0) return true;
            return false;
        }

        private DateTime GetDateTimeUTC(string Pi)
        {
            DateTime date = DateTime.Now.ToUniversalTime();
            long datebinary = 0;
            string stag = "<datetime>";
            string etag = "</datetime>";
            int start = Pi.IndexOf(stag);
            if (start < 0) return date
                ;
            string _value = Pi.Substring(start + stag.Length);
            int stopi = _value.IndexOf(etag);
            _value = _value.Remove(stopi, _value.Length - stopi);

            if (long.TryParse(_value, out datebinary))
            {
                date = DateTime.FromBinary(datebinary);
            }
            return date;
        }

        private String GetNormalizedID(String guid)
        {
            guid = guid.Replace("-", "");
            if (!guid.StartsWith("ID"))
            {
                guid = "ID" + guid;
            }
            return guid;
        }
        private string GetFromJID(string Pi)
        {
            int start = Pi.IndexOf("from=");
            if (start < 0) return xmppsControl.User;
            string jabberID = Pi.Substring(start + 6);
            int stopi = jabberID.IndexOf("/");
            jabberID = jabberID.Remove(stopi, jabberID.Length - stopi);
            return jabberID;
        }
        private string GetFileTransferID(string Pi)
        {
            string stag = "<id>";
            string etag = "</id>";
            int start = Pi.IndexOf(stag);
            if (start < 0) return null;
            string _value = Pi.Substring(start + stag.Length);
            int stopi = _value.IndexOf(etag);
            _value = _value.Remove(stopi, _value.Length - stopi);
            return _value;
        }
        private string GetFileTransferSize(string Pi)
        {
            string stag = "<size>";
            string etag = "</size>";
            int start = Pi.IndexOf(stag);
            if (start < 0) return null;
            string _value = Pi.Substring(start + stag.Length);
            int stopi = _value.IndexOf(etag);
            _value = _value.Remove(stopi, _value.Length - stopi);
            return _value;
        }
        private string GetFileTransferFileName(string Pi)
        {
            string stag = "<file>";
            string etag = "</file>";
            int start = Pi.IndexOf(stag);
            if (start < 0) return null;
            string _value = Pi.Substring(start + stag.Length);
            int stopi = _value.IndexOf(etag);
            _value = _value.Remove(stopi, _value.Length - stopi);
            return _value;
        }
        private string GetVideoCallSessionID(string Pi)
        {
            string stag = "<id>";
            string etag = "</id>";
            int start = Pi.IndexOf(stag);
            if (start < 0) return null;
            string _value = Pi.Substring(start + stag.Length);
            int stopi = _value.IndexOf(etag);
            _value = _value.Remove(stopi, _value.Length - stopi);
            return _value;
        }

        private void HandleXEvent(string Pi)
        {
            //Supported XEvents
            //composing - user typing notifications
            //video-invite - video conference invite
            //video-accept - accept, for previous video conference invite
            //video-cancel - cancel, for previous video conference invite

            string jid = GetFromJID(Pi);
            if (jid == mUserAccount.JabberUser.JID) return;

            //ignore old messages
            DateTime remoteDateTimeUTC = GetDateTimeUTC(Pi);
            DateTime localDateTimeUTC = DateTime.Now.ToUniversalTime();

            JabberUser jabberUser = new JabberUser(jid);
            //composing
            if ((Pi.IndexOf("<composing") >= 0) && (Pi.IndexOf("<body") >= 0))
            {
                //if there is a composing tag and a body tag - then this is a regular message
                //buddy is not composing
#if (TRACE)
                Console.WriteLine("IM-XEvent|" + jid + " sent the message");
#endif
                myChatWindow.SetComposing(false, jid);
            }
            else if (Pi.IndexOf("<composing") >= 0)
            {
                //if there is a composing tag and NOT a body tag - then this is a composing message
                //buddy is composing

                //ignore composing older than one minute
                if (remoteDateTimeUTC.AddMinutes(1) < localDateTimeUTC) return;
#if (TRACE)
                Console.WriteLine("IM-XEvent|" + jid + " is composing");
#endif
                myChatWindow.SetComposing(true, jid);
            }
            //file transfer
            if (Pi.IndexOf("<file-transfer-invite") >= 0)
            {
                String fileTransferId = GetFileTransferID(Pi);
                FileTransferInfo ftInfo = new FileTransferInfo(GetFileTransferID(Pi), GetFileTransferFileName(Pi), GetFileTransferSize(Pi));

                myFileTransfer[ftInfo.ID] = ftInfo;

                FileTransferProcessInvite(jabberUser, fileTransferId);
            }
            if (Pi.IndexOf("<file-transfer-done") >= 0)
            {
                //TO DO GET RUN DOWNLOAD ONLY IF ALREADY UPLOADED
            }
            if (Pi.IndexOf("<file-transfer-accept") >= 0)
            {
                myChatWindow.AddNotification(
                    jid,
                    "INVITE",
                    String.Format(Properties.Localization.txtInfoIMFileTransferAccepted, jabberUser.Nick),
                    DateTime.Now,
                    Guid.NewGuid().ToString(),
                    true,
                    true
                    );
                string fileTransferID = GetFileTransferFileName(Pi);

#if (TRACE)
                Console.WriteLine("IM-XEvent| File Transfer Accept - " + fileTransferID);
#endif
            }
            if (Pi.IndexOf("<file-transfer-cancel") >= 0)
            {
                myChatWindow.AddNotification(
                    jid,
                    "INVITE",
                    String.Format(Properties.Localization.txtInfoIMFileTransferDenied, jabberUser.Nick),
                    DateTime.Now,
                    Guid.NewGuid().ToString(),
                    true,
                    true
                    );
                string fileTransferID = GetFileTransferFileName(Pi);
#if (TRACE)
                Console.WriteLine("IM-XEvent| File Transfer Cancel - " + fileTransferID);
#endif
            }

            //nudge
            if (Pi.IndexOf("<event-nudge") >= 0)
            {
                //ignore nudge older than one minute
                if (remoteDateTimeUTC.AddMinutes(1) < localDateTimeUTC) return;
                myChatWindow.AddNotification(
                    jid,
                    "NUDGE",
                   String.Format(Properties.Localization.txtInfoIMSendYouNudge, jabberUser.Nick),
                    DateTime.Now,
                    Guid.NewGuid().ToString(),
                    true,
                    true
                    );
            }

            //video-invite
            if (Pi.IndexOf("<video-invite") >= 0)
            {
                //ignore video-invite older than one minute
                if (remoteDateTimeUTC.AddMinutes(1) < localDateTimeUTC) return;

                string VideoSessionID = GetVideoCallSessionID(Pi);
#if (TRACE)
                Console.WriteLine("IM-XEvent| Video Call Invite - " + VideoSessionID);
#endif
                VideoSessionProcessInvite(jabberUser, VideoSessionID);

            }

            //video-accept
            if (Pi.IndexOf("<video-accept") >= 0)
            {
                //ignore video-accept older than one minute
                if (remoteDateTimeUTC.AddMinutes(1) < localDateTimeUTC) return;

                myChatWindow.AddNotification(
                    jid,
                    "INVITE",
                    String.Format(Properties.Localization.txtInfoIMVideoCallAccepted, jabberUser.Nick),
                    DateTime.Now,
                    Guid.NewGuid().ToString(),
                    true,
                    true
                    );
                string VideoSessionID = GetVideoCallSessionID(Pi);
#if (TRACE)
                Console.WriteLine("IM-XEvent| Video Call Accept - " + VideoSessionID);
#endif

                VideoSessionProcessAccept(jabberUser, VideoSessionID);
            }

            //video-deny
            if (Pi.IndexOf("<video-cancel") >= 0)
            {
                //ignore video-deny older than one minute
                if (remoteDateTimeUTC.AddMinutes(1) < localDateTimeUTC) return;

                VideoSessionProcessCancel(jabberUser);

                string VideoSessionID = GetVideoCallSessionID(Pi);
#if (TRACE)
                Console.WriteLine("IM-XEvent| Video Call Cancel - " + VideoSessionID);
#endif

            }
        }


        #endregion

        #region VideoCall Processing

        internal void VideoSessionConnect(string jabberID, string videoSessionID)
        {
            myVideoPlugin.StartConference(true, 0, 0, 0, ConfigVideoProxy.ProxyAddress, false, true, videoSessionID, DateTime.Now.ToString("yyyyMMdd|HHmmss|fff") + ";" + mUserAccount.Username, jabberID);
        }

        internal void VideoSessionInvite(String jid)
        {
            JabberUser jabberUser = new JabberUser(jid);
            VideoSessionInvite(jabberUser);
        }

        internal void VideoSessionInvite(JabberUser jabberUser)
        {
            myChatWindow.AddNotification(
                    jabberUser.JID,
                    "INVITE",
                    String.Format(Properties.Localization.txtInfoIMVideoCallInvitationSent, new object[] { 
                        jabberUser.Nick
                    }),
                    DateTime.Now,
                    Guid.NewGuid().ToString(),
                    false,
                    true
                    );

            string VideoSessionID = Guid.NewGuid().ToString() + "@" + mUserAccount.Username + ";" + jabberUser;
            //myVideoPlugin.Show(true);
            string jabberXevent = "<x xmlns='jabber:x:event'><video-invite/><datetime>" + DateTime.Now.ToUniversalTime().ToBinary() + "</datetime><id>" + VideoSessionID + "</id></x>";
            bool msgSent = SendJabberXevent(jabberUser.JID, jabberXevent);
        }

        internal void VideoSessionProcessInvite(JabberUser jabberUser, string VideoSessionID)
        {

            String videoInvitationID = GetNormalizedID(Guid.NewGuid().ToString());
            myChatWindow.AddNotification(
                jabberUser.JID,
                "INVITE",
                String.Format(Properties.Localization.txtInfoIMVideoCallInvitationRecived, new object[] 
{ 
    
    jabberUser.Nick}) + @"  <a id='" + videoInvitationID + "A' href='vc-accept:" + VideoSessionID + "'>Accept</a> | <a id='" + videoInvitationID + "A' href='vc-decline:" + VideoSessionID + "'>Decline</a> ",

                DateTime.Now,
                Guid.NewGuid().ToString(),
                true,
                true
                );
        }

        internal void VideoSessionProcessInviationResult(String jid, String videoSessionID, Boolean accepted)
        {
            if (accepted)
            {
                string jabberXevent = "<x xmlns='jabber:x:event'><video-accept/><datetime>" + DateTime.Now.ToUniversalTime().ToBinary() + "</datetime><id>" + videoSessionID + "</id></x>";
                bool msgSent = SendJabberXevent(jid, jabberXevent);
                VideoSessionConnect(jid, videoSessionID);
            }
            else
            {
                string jabberXevent = "<x xmlns='jabber:x:event'><video-cancel/><datetime>" + DateTime.Now.ToUniversalTime().ToBinary() + "</datetime><id>" + videoSessionID + "</id></x>";
                bool msgSent = SendJabberXevent(jid, jabberXevent);
            }
        }

        internal void VideoSessionProcessAccept(JabberUser jabberUser, string VideoSessionID)
        {
            VideoSessionConnect(jabberUser.JID, VideoSessionID);
            StartNewCall(-1, jabberUser);
        }

        internal void VideoSessionProcessCancel(JabberUser jabberUser)
        {
            myChatWindow.AddNotification(
                        jabberUser.JID,
                        "INVITE",
                        String.Format(Properties.Localization.txtInfoIMVideoCallDenied, jabberUser.Nick),
                        DateTime.Now,
                        Guid.NewGuid().ToString(),
                        true,
                        true
                        );
        }

        internal void VideoSessionDisconnect(JabberUser jabberID)
        {
            myVideoPlugin.StopConference();

        }
        #endregion

        #region File Transfer Processing
        internal void FileTransferInvite(JabberUser jabberUser, string fileName, string fileTransferID, string fileSize)
        {
            FileTransferInfo ftInfo = new FileTransferInfo(fileTransferID, fileName, fileSize);

            myFileTransfer[ftInfo.ID] = ftInfo;
            String transferIdN = GetNormalizedID(ftInfo.ID);

            myChatWindow.AddNotification(
                    jabberUser.JID,
                    "INVITE",
                    String.Format(Properties.Localization.txtInfoIMFileTransferInitialized, new object[] { 
                        jabberUser.Nick, 
                        @"<a id='" + transferIdN + "' href='ft-upload:" + fileTransferID + "'> " + ftInfo.Filename + " " + fileSize + @"</a>"
                    }),
                    DateTime.Now,
                    Guid.NewGuid().ToString(),
                    false,
                    true
                    );
            string jabberXevent = "<x xmlns='jabber:x:event'><file-transfer-invite/><datetime>" + DateTime.Now.ToUniversalTime().ToBinary() + "</datetime><id>" + fileTransferID + "</id><file>" + fileName + "</file><size>" + fileSize + "</size></x>";
            bool msgSent = SendJabberXevent(jabberUser.JID, jabberXevent);
        }

        internal void FileTransferProcessInvite(JabberUser jabberUser, string fileTransferID)
        {
            FileTransferInfo ftInfo = (FileTransferInfo)myFileTransfer[fileTransferID];
            if (ftInfo == null) return;

            String transferIdN = GetNormalizedID(ftInfo.ID);
            myChatWindow.AddNotification(
                jabberUser.JID,
                "INVITE",
                String.Format(Properties.Localization.txtInfoIMFileTransfer, new object[] 
{ 
    
    jabberUser.Nick, @"<a id='" + transferIdN + "A' href='ft-accept:" + fileTransferID + "'>Accept</a> | <a id='" + transferIdN + "A' href='ft-decline:" + fileTransferID + "'>Decline</a> " + ftInfo.Filename + " " + ftInfo.Size }),

                DateTime.Now,
                Guid.NewGuid().ToString(),
                true,
                true
                );

        }
        internal void FileTransferProcessDone(String jid, string filename, string fileTransferID, string fileSize)
        {
            string jabberXevent = "<x xmlns='jabber:x:event'><file-transfer-done/><id>" + fileTransferID + "</id></x>";
            bool msgSent = SendJabberXevent(jid, jabberXevent);
        }
        internal void FileTransferProcessAccept(String jid, String fileTransferID)
        {
            FileTransferInfo ftInfo = (FileTransferInfo)myFileTransfer[fileTransferID];
            if (ftInfo == null) return;

            String transferIdN = GetNormalizedID(ftInfo.ID);

            string jabberXevent = "<x xmlns='jabber:x:event'><file-transfer-accept/><datetime>" + DateTime.Now.ToUniversalTime().ToBinary() + "</datetime><id>" + fileTransferID + "</id></x>";
            bool msgSent = SendJabberXevent(jid, jabberXevent);
            myChatWindow.ProcessFileDownload(jid, ftInfo.Filename, ftInfo.ID);
            myChatWindow.AddNotification(
                 jid,
                 "INVITE",
                 String.Format(Properties.Localization.txtInfoIMFileTransferDownloading, @"<a id='" + transferIdN + "' href='ft-download:" + fileTransferID + "'> " + ftInfo.Filename + " " + ftInfo.Size + @"</a>"),
                 DateTime.Now,
                 Guid.NewGuid().ToString(),
                 true,
                 true
                 );
        }

        internal void FileTransferProcessCancel(String jid, string fileTransferID)
        {
            string jabberXevent = "<x xmlns='jabber:x:event'><file-transfer-cancel/><datetime>" + DateTime.Now.ToUniversalTime().ToBinary() + "</datetime><id>" + fileTransferID + "</id></x>";
            bool msgSent = SendJabberXevent(jid, jabberXevent);
        }
        #endregion
        #region xmpp change presence status
        private void myGoOfflineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (xmppsControl.Connected)
            {
                try
                {
                    XMPP_ChangePresence(XMPPIQ.PresenceShow.offline);
                }
                catch (Exception)
                {

                }
                myPresenceStripSplitButton.Image = myGoOfflineToolStripMenuItem.Image;
            }
        }

        private void myGoOnlineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (xmppsControl.Connected)
            {
                try
                {
                    XMPP_ChangePresence(XMPPIQ.PresenceShow.chat);
                }
                catch (Exception)
                {

                }
                myPresenceStripSplitButton.Image = myGoOnlineToolStripMenuItem.Image;
            }
        }

        private void myGoAwayToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (xmppsControl.Connected)
            {
                try
                {
                    XMPP_ChangePresence(XMPPIQ.PresenceShow.away);
                }
                catch (Exception)
                {

                }
                myPresenceStripSplitButton.Image = myGoAwayToolStripMenuItem.Image;
            }
        }

        private void myGoExtendedAwayToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (xmppsControl.Connected)
            {
                try
                {
                    XMPP_ChangePresence(XMPPIQ.PresenceShow.xa);
                }
                catch (Exception)
                {

                }
                myPresenceStripSplitButton.Image = myGoExtendedAwayToolStripMenuItem.Image;
            }
        }

        private void myGoDoNotDisturbToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (xmppsControl.Connected)
            {
                try
                {
                    XMPP_ChangePresence(XMPPIQ.PresenceShow.dnd);
                }
                catch (Exception)
                {

                }
                myPresenceStripSplitButton.Image = myGoDoNotDisturbToolStripMenuItem.Image;
            }
        }
        private void myToolStripNetworkStatus_Click(object sender, System.EventArgs e)
        {
            if (xmppsControl.Connected)
            {
                try
                {
                    xmppsControl.Disconnect();
                }
                catch (Exception)
                {

                }
            }
            else
            {
                try
                {
                    xmppsControl.Connect(mUserAccount.Username, mUserAccount.Password);
                }
                catch (Exception)
                {

                }
            }
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


                        if (mUserAccount.JabberUser.JID == tmpContactWindow.mNTContact.NTJabberID)
                        {
                            mXMPPIQ.StoreVCard(mUserAccount.JabberUser, mUserAccount.Contact);
                            return;
                        }

                        this.mReloadContactsList = true;
                        mContactBook.Modified();
                        if (tmpContactWindow.mNTContact != null && tmpContactWindow.mNTContact.NTJabberID != mUserAccount.JabberUser.JID && tmpContactWindow.mNTContact.NTJabberID.Length > 0)
                        {
                            JabberUser jabberUser = new JabberUser(tmpContactWindow.mNTContact.NTJabberID);
                            XmppsBuddySubscriptions subscription = XmppsBuddySubscriptions.stNone;


                            for (int i = 1; i <= xmppsControl.BuddyCount; i++)
                            {
                                if (xmppsControl.BuddyId[i] == jabberUser.JID)
                                {
                                    subscription = xmppsControl.BuddySubscription[i];
                                    break;
                                }
                            }
                            try
                            {
                                mXMPPIQ.Roster(jabberUser, tmpContactWindow.myContactJabberGroupListBox.Text);
                                //xmppsControl.Add(jabberUser.EscapedJID, jabberUser.Username, tmpContactWindow.myContactJabberGroupListBox.Text);
                                switch (subscription)
                                {
                                    case XmppsBuddySubscriptions.stTo:
                                        //
                                        break;
                                    case XmppsBuddySubscriptions.stBoth:
                                        //
                                        break;

                                    case XmppsBuddySubscriptions.stFrom:
                                    case XmppsBuddySubscriptions.stRemove:
                                    case XmppsBuddySubscriptions.stNone:
                                    default:
                                        mXMPPIQ.Subscibe(jabberUser);
                                        xmppsControl.SubscribeTo(jabberUser.EscapedJID);
                                        mXMPPIQ.PromptUser(jabberUser);
                                        break;
                                }
                            }
                            catch (Exception)
                            {

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
#if (TRACE)
                Console.WriteLine("myContactWindow_FormClosing" + ex.Message);
#endif
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
                    StartNewIM(myRosterListTreeView.SelectedNode.Tag.ToString(), true);
                }
            }
        }

        private void sendMessageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (myMainContactsTabPage.IsSelected)
            {
                RadListBoxItem selectedItem = (RadListBoxItem)myContactsListBox.SelectedItem;
                if (selectedItem != null)
                {
                    NTContact selectedContact = (NTContact)selectedItem.Tag;
                    if (selectedContact.NTJabberID != "") StartNewIM(new JabberUser(selectedContact.NTJabberID), false);
                }
            }
            else
                if (myMainOnlineTabPage.IsSelected)
                {
                    if (myRosterListTreeView.SelectedNode != null)
                    {
                        if (myRosterListTreeView.SelectedNode.Level == 1)
                        {
                            StartNewIM(myRosterListTreeView.SelectedNode.Tag.ToString(), true);
                        }
                    }
                }


        }

        private void callToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (myMainContactsTabPage.IsSelected)
            {
                RadListBoxItem selectedItem = (RadListBoxItem)myContactsListBox.SelectedItem;
                if (selectedItem != null)
                {
                    NTContact selectedContact = (NTContact)selectedItem.Tag;
                    if (selectedContact.NTJabberID != "") StartNewCall(-1, new JabberUser(selectedContact.NTJabberID));
                }
            }
            else
                if (myMainOnlineTabPage.IsSelected)
                {
                    if (myRosterListTreeView.SelectedNode != null)
                    {
                        if (myRosterListTreeView.SelectedNode.Level == 1)
                        {
                            StartNewCall(-1, new JabberUser(myRosterListTreeView.SelectedNode.Tag.ToString()));
                        }
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
                myMenuVideoCallToolStripButton.Enabled = false;
            }
            else
            {
                myRosterContextMenuStrip.Enabled = true;
                myMenuChatToolStripButton.Enabled = true;
                myMenuCallToolStripButton.Enabled = true;
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
                    properties.Add("NTJabberID", myRosterListTreeView.SelectedNode.Tag.ToString());
                    OpenContactWindow(mContactBook.getCandidatesForJabberID(myRosterListTreeView.SelectedNode.Tag.ToString()).Count > 0 ? (NTContact)mContactBook.getCandidatesForJabberID(myRosterListTreeView.SelectedNode.Tag.ToString())[0] : null, properties);
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
                if (selectedContact.NTJabberID != "") StartNewIM(selectedContact.NTJabberID, true);
            }
        }

        private void callComputerContactsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RadListBoxItem selectedItem = (RadListBoxItem)myContactsListBox.SelectedItem;
            if (selectedItem != null)
            {
                NTContact selectedContact = (NTContact)selectedItem.Tag;
                if (selectedContact.NTJabberID != "") StartNewCall(-1, new JabberUser(selectedContact.NTJabberID));
            }
        }

        private void callMobileContactsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RadListBoxItem selectedItem = (RadListBoxItem)myContactsListBox.SelectedItem;
            if (selectedItem != null)
            {
                NTContact selectedContact = (NTContact)selectedItem.Tag;
                if (selectedContact.NTMobileTelephoneNumber != "") StartNewCall(-1, selectedContact.NTMobileTelephoneNumber);
            }
        }

        private void callHomeContactsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RadListBoxItem selectedItem = (RadListBoxItem)myContactsListBox.SelectedItem;
            if (selectedItem != null)
            {
                NTContact selectedContact = (NTContact)selectedItem.Tag;
                if (selectedContact.NTHomeTelephoneNumber != "") StartNewCall(-1, selectedContact.NTHomeTelephoneNumber);
            }
        }

        private void callWorkContactsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RadListBoxItem selectedItem = (RadListBoxItem)myContactsListBox.SelectedItem;
            if (selectedItem != null)
            {
                NTContact selectedContact = (NTContact)selectedItem.Tag;
                if (selectedContact.NTBusinessTelephoneNumber != "") StartNewCall(-1, selectedContact.NTBusinessTelephoneNumber);
            }
        }

        private void callVoIPContactsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RadListBoxItem selectedItem = (RadListBoxItem)myContactsListBox.SelectedItem;
            if (selectedItem != null)
            {
                NTContact selectedContact = (NTContact)selectedItem.Tag;
                if (selectedContact.NTVoIPTelephoneNumber != "") StartNewCall(-1, selectedContact.NTVoIPTelephoneNumber);
            }
        }

        private void viewContactContactsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RadListBoxItem selectedItem = (RadListBoxItem)myContactsListBox.SelectedItem;
            if (selectedItem != null)
            {
                NTContact selectedContact = (NTContact)selectedItem.Tag;
                OpenContactWindow(selectedContact, null);
            }
        }
        #endregion

        private void myContactsListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                RadListBoxItem selectedItem = (RadListBoxItem)myContactsListBox.SelectedItem;
                if (selectedItem != null)
                {
                    NTContact selectedContact = (NTContact)selectedItem.Tag;
                    if (selectedContact != null)
                    {
                        sendMessageContactContextMenuItem.Enabled = selectedContact.NTJabberID.Length > 0 ? true : false;
                        callContactContextMenuItem.Enabled = selectedContact.NTJabberID.Length > 0 ? true : false;
                        callHomeContactContextMenuItem.Enabled = selectedContact.NTHomeTelephoneNumber.Length > 0 ? true : false;
                        callMobileContactContextMenuItem.Enabled = selectedContact.NTMobileTelephoneNumber.Length > 0 ? true : false;
                        callWorkContactContextMenuItem.Enabled = selectedContact.NTBusinessTelephoneNumber.Length > 0 ? true : false;
                        callVoIPContactContextMenuItem.Enabled = selectedContact.NTVoIPTelephoneNumber.Length > 0 ? true : false;
                        startVideoCallContactContextMenuItem.Enabled = selectedContact.NTJabberID.Length > 0 ? true : false;
                    }
                }
            }
            catch (Exception)
            {

            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void myMainMenuAboutItem_Click(object sender, EventArgs e)
        {
            if (myAboutBoxWindow == null)
            {

                myAboutBoxWindow = new AboutBox(Application.ProductName, Application.ProductVersion, Application.CompanyName, ConfigWebLinks.CompanyLink, ConfigWebLinks.ProductLink); ;
                myAboutBoxWindow.Disposed += new EventHandler(myAboutBoxWindow_Disposed);
            }

            myAboutBoxWindow.Show();
            myAboutBoxWindow.Activate();
        }

        private void myAboutBoxWindow_Disposed(object sender, EventArgs e)
        {
            myAboutBoxWindow = null;
        }

        private void myContactsShowAll()
        {
            if (mContactBook != null)
            {
                LoadContactsBook();
            }
        }

        private void myContactsAddContactButton_Click(object sender, EventArgs e)
        {
            OpenContactWindow(null, null);
        }

        private void myContactsShow1ABButton_Click(object sender, EventArgs e)
        {
            LoadContactsBook();
        }

        private void myContactsShowCDEButton_Click(object sender, EventArgs e)
        {

            LoadContactsBook();
        }

        private void myContactsShowFGHButton_Click(object sender, EventArgs e)
        {
            LoadContactsBook();
        }

        private void myContactsShowIJKButton_Click(object sender, EventArgs e)
        {
            LoadContactsBook();
        }

        private void myContactsShowLMNButton_Click(object sender, EventArgs e)
        {
            LoadContactsBook();

        }

        private void myContactsShowOPQButton_Click(object sender, EventArgs e)
        {
            LoadContactsBook();

        }

        private void myContactsShowRSTButton_Click(object sender, EventArgs e)
        {
            LoadContactsBook();
        }

        private void myContactsShowUVWButton_Click(object sender, EventArgs e)
        {
            LoadContactsBook();

        }

        private void myContactsShowXYZButton_Click(object sender, EventArgs e)
        {

            LoadContactsBook();

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
        private void myPrepaidAmountRefreshToolStripLabel_Click(object sender, EventArgs e)
        {
            getPrepaidStatus();
        }

        private void LoadCallsHistory()
        {
            myCallHistoryListBox.Items.Clear();

            for (int i = myCallHistoryRecords.Count; i > 0; i--)
            {
                CallRecord record = myCallHistoryRecords[i - 1];
                if (record.NumberOrUsername.Trim() != "")
                {
                    // 
                    // tmplContactListItem
                    // 

                    this.tmplCallRecordListItem = new Telerik.WinControls.UI.RadListBoxItem();
                    this.tmplCallRecordListItem.AutoSize = true;
                    this.tmplCallRecordListItem.AutoSizeMode = Telerik.WinControls.RadAutoSizeMode.Auto;
                    this.tmplCallRecordListItem.AccessibleDescription = record.NumberOrUsername;
                    this.tmplCallRecordListItem.CanFocus = true;
                    this.tmplCallRecordListItem.DescriptionText = record.CallDateTime.ToShortDateString() + " " + record.CallDateTime.ToShortTimeString() + " | (" + SecondsToHH24MISS(record.CallDuration) + ")";
                    this.tmplCallRecordListItem.ForeColor = System.Drawing.Color.Black;
                    this.tmplCallRecordListItem.Image = ((System.Drawing.Image)(Properties.Resources.ResourceManager.GetObject(record.CallState.ToString())));
                    this.tmplCallRecordListItem.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                    this.tmplCallRecordListItem.Text = record.NumberOrUsername;
                    this.tmplCallRecordListItem.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
                    this.tmplCallRecordListItem.TextSeparatorVisibility = Telerik.WinControls.ElementVisibility.Visible;
                    this.tmplCallRecordListItem.ToolTipText = null;
                    this.tmplCallRecordListItem.Font = new System.Drawing.Font("Trebuchet MS", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                    this.tmplCallRecordListItem.ForeColor = System.Drawing.Color.Black;
                    this.tmplCallRecordListItem.DescriptionFont = new System.Drawing.Font("Trebuchet MS", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                    this.tmplCallRecordListItem.TextSeparatorVisibility = Telerik.WinControls.ElementVisibility.Visible;
                    //this.tmplCallRecordListItem.DoubleClick += new EventHandler(tmplContactListItem_DoubleClick);
                    this.tmplCallRecordListItem.Tag = record;

                    this.myCallHistoryListBox.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.tmplCallRecordListItem});
                }
            }
            if (myCallHistoryListBox.Items.Count > 0) myCallHistoryListBox.SelectedIndex = 0;
        }

        private void radTabStrip1_TabSelected(object sender, TabEventArgs args)
        {
            if (args.TabItem == myTabItemCallHistory)
            {
                LoadCallsHistory();
            }

            if (args.TabItem == myTabItemVoiceMail)
            {
                myWebVoiceMailControl.CheckForVoicemails();
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
                if (selectedCallRecord != null && selectedCallRecord.NumberOrUsername != "")
                {
                    int dummy;
                    if (Int32.TryParse(selectedCallRecord.NumberOrUsername.Trim(), out dummy))
                    {
                        properties.Add("NTHomeTelephoneNumber", selectedCallRecord.NumberOrUsername);
                    }
                    else
                    {
                        properties.Add("NTJabberID", selectedCallRecord.NumberOrUsername);
                    }

                    OpenContactWindow(null, properties);
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
            if (mySIPPhoneLines == null) return;
            foreach (PhoneLineState line in mySIPPhoneLines)
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
                selectedContact.NTContactChanged = true;
                mContactBook.Modified();
            }
        }

        private string NormalizePhoneNumber(string myPhoneNumber)
        {

            //remove special characters
            Regex _Regex1 = new Regex("([^a-z0-9._-p#*+])+", RegexOptions.Singleline);
            myPhoneNumber = _Regex1.Replace(myPhoneNumber.ToLower(), "");

            //remove non alpha and non number form begining
            Regex _Regex2 = new Regex("([^a-z0-9+])+", RegexOptions.Singleline);
            while ((myPhoneNumber.Length > 0) && (_Regex2.IsMatch(myPhoneNumber.Substring(0, 1))))
            {
                myPhoneNumber = myPhoneNumber.Remove(0, 1);
            }

            //format phone number
            Regex _Regex3 = new Regex("([^0-9p#*+])+", RegexOptions.Singleline);
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
                    RSIFeaturesWS.RSIService nikotalkWS = new Remwave.Client.RSIFeaturesWS.RSIService();
                    result = nikotalkWS.serviceCallNow(mUserAccount.Username, mUserAccount.Password, myPhone2PhoneWindow.CallFrom, myPhone2PhoneWindow.CallTo);
                }
                catch (Exception ex)
                {
#if TRACE
                    Console.Write("serviceCallNow Exception : " + ex.Message);
#endif

                }
                if (result == "0")
                {
                    myNotifyIcon.ShowBalloonTip(
                        10,
                        Properties.Localization.txtInfoPhone2Phone,
                        String.Format(Properties.Localization.txtInfoPhone2PhoneTrying, new object[] { myPhone2PhoneWindow.CallFrom, myPhone2PhoneWindow.CallTo }),

                        ToolTipIcon.Info
                        );
                }
                else if (result == "2")
                {
                    myNotifyIcon.ShowBalloonTip(
                        10,
                        Properties.Localization.txtInfoPhone2Phone,
                        String.Format(Properties.Localization.txtInfoPhone2PhonePaymentRequired, new object[] { myPhone2PhoneWindow.CallFrom, myPhone2PhoneWindow.CallTo }),
                        ToolTipIcon.Error);
                }
                else
                {
                    myNotifyIcon.ShowBalloonTip(
                        10,
                        Properties.Localization.txtInfoPhone2Phone,
                        String.Format(Properties.Localization.txtInfoPhone2PhoneUnableToStartCall, new object[] { myPhone2PhoneWindow.CallFrom, myPhone2PhoneWindow.CallTo }),
                        ToolTipIcon.Error);
                }
            }
        }

        private void callPhone2PhoneToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            RadListBoxItem selectedItem = (RadListBoxItem)myContactsListBox.SelectedItem;
            if (selectedItem != null)
            {
                NTContact selectedContact = (NTContact)selectedItem.Tag;
                myPhone2PhoneWindow = new Phone2PhoneWindow(selectedContact, mUserAccount.Contact);
                myPhone2PhoneWindow.FormClosing += new FormClosingEventHandler(myPhone2PhoneWindow_FormClosing);
                myPhone2PhoneWindow.Show();
            }
        }

        private void myMenuVideoCallToolStripButton_Click(object sender, EventArgs e)
        {
            if (myMainContactsTabPage.IsSelected)
            {
                RadListBoxItem selectedItem = (RadListBoxItem)myContactsListBox.SelectedItem;
                if (selectedItem != null)
                {
                    NTContact selectedContact = (NTContact)selectedItem.Tag;
                    if (selectedContact.NTJabberID != "") StartNewVideoCall(selectedContact.NTJabberID);
                }
            }
            else
                if (myMainOnlineTabPage.IsSelected)
                {
                    if (myRosterListTreeView.SelectedNode != null)
                    {
                        if (myRosterListTreeView.SelectedNode.Level == 1)
                        {
                            StartNewVideoCall(myRosterListTreeView.SelectedNode.Tag.ToString());
                        }
                    }
                }
        }

        private void myClientNotifyIcon_BalloonTipClicked(object sender, EventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
            this.Activate();
            this.BringToFront();
        }

        private void myClientNotifyIcon_DoubleClick(object sender, EventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
            this.Activate();
            this.BringToFront();
        }

        private void myMenuAddContactToolStripButton_Click(object sender, EventArgs e)
        {
            OpenContactWindow(null, null);
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (myRosterListTreeView.SelectedNode != null)
            {
                if (myRosterListTreeView.SelectedNode.Level == 1)
                {
                    try
                    {
                        JabberUser jabberUser = new JabberUser(myRosterListTreeView.SelectedNode.Tag.ToString());
                        String group = myRosterListTreeView.SelectedNode.Parent.Text == Properties.Localization.txtOtherGroup ? "" : myRosterListTreeView.SelectedNode.Parent.Text;
                        mXMPPIQ.Remove(jabberUser);
                    }
                    catch (Exception)
                    {

                    }
                }
            }
        }

        private void myServiceStateCheckTimer_Tick(object sender, EventArgs e)
        {
            if (!this.mFormIsClosing & !xmppsControl.Connected & myNetworkInfo.Online & mUserAccount.LoggedIn)
            {
                //network detected, try authenticate at jabber
                try
                {
                    myServiceStateCheckTimer.Tag = DateTime.Now;
#if (TRACE)
                    Console.WriteLine("IM:Trying to ReConnect");
#endif
                    xmppsControl.Connect(mUserAccount.Username, mUserAccount.Password);

                }
                catch (nsoftware.IPWorksSSL.IPWorksSSLException ex)
                {
#if (TRACE)
                    Console.WriteLine("IM:ReConnecting:(" + ex.Code + "):" + ex.Message);
#endif
                }
            }

            if (mServiceDiscoveryTime.AddMinutes(3) < DateTime.Now)
            {
                if (!xmppsControl.Connected) return;
                mServiceDiscoveryTime = DateTime.Now;
                XMPP_DiscoverIMNetworks();
                XMPP_SendPresenceIMNetworks();
            }
        }

        private void myMenuPhone2PhoneCallToolStripButton_Click(object sender, EventArgs e)
        {
            if (myMainContactsTabPage.IsSelected)
            {
                RadListBoxItem selectedItem = (RadListBoxItem)myContactsListBox.SelectedItem;
                if (selectedItem != null)
                {
                    NTContact selectedContact = (NTContact)selectedItem.Tag;

                    myPhone2PhoneWindow = new Phone2PhoneWindow(selectedContact, mUserAccount.Contact);
                    myPhone2PhoneWindow.FormClosing += new FormClosingEventHandler(myPhone2PhoneWindow_FormClosing);
                    myPhone2PhoneWindow.Show();
                }
            }
            else
                if (myMainOnlineTabPage.IsSelected)
                {
                    if (myRosterListTreeView.SelectedNode != null)
                    {
                        if (myRosterListTreeView.SelectedNode.Level == 1)
                        {

                            ContactList foundContactsList = mContactBook.getCandidatesForJabberID(myRosterListTreeView.SelectedNode.Tag.ToString());
                            NTContact selectedContact;
                            if (foundContactsList.Count > 0)
                            {
                                selectedContact = (NTContact)foundContactsList[0];
                            }
                            else
                            {
                                selectedContact = new NTContact();
                            }
                            myPhone2PhoneWindow = new Phone2PhoneWindow(selectedContact, mUserAccount.Contact);
                            myPhone2PhoneWindow.FormClosing += new FormClosingEventHandler(myPhone2PhoneWindow_FormClosing);
                            myPhone2PhoneWindow.Show();
                        }
                    }
                }
        }


        private void myMainMenuSignOutItem_Click(object sender, EventArgs e)
        {
            UserSignOut();
        }







        #region Mani Tab Control
        private void myMainTabControl_TabSelected(object sender, TabEventArgs args)
        {
            /*            if (args.TabItem == myMainAccountTabPage)
                        {
                            if (DialogResult.Yes == MessageBox.Show(
                                Properties.Localization.txtInfoWebsiteDesc,
                                Properties.Localization.txtInfoWebsite,
                                MessageBoxButtons.YesNo,
                                MessageBoxIcon.Question
                                ))
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

                                }
                            }
                        }
                        else */
            if (args.TabItem == myMainContactsTabPage)
            {
                myContactsShowAllButton.PerformClick();

                if (myContactsListBox.Items.Count > 0)
                {
                    myContactsListBox.SelectedIndex = 0;
                    myContactsListBox.Select();
                    myContactsListBox.Focus();
                    myContactsListBox.Items[0].Focus();
                }
            }
            else if (args.TabItem == myMainDialPadTabPage)
            {
                myDestinationTextBox.Focus();
            }















































        }
        #endregion















        private void ShowHideSpeedDialWindow()
        {
            if (mySpeedDialWindow == null)
            {
                mySpeedDialWindow = new SpeedDialWindow(this);
            }
#if(!BRAND_JOCCOME)
            mySpeedDialWindow.ShowHide();
#else
            else if (!mySpeedDialWindow.Visible && showApp)
            {
                if (!this.Visible)
                {
                    this.WindowState = FormWindowState.Normal;
                    this.Show();
                    this.Activate();
                    this.Focus();

                }
                else
                {
                    this.Hide();
                    showApp = false;
                }


            }
            else
            {
                this.Hide();
                mySpeedDialWindow.ShowHide();
                showApp = true;
            }
#endif

        }

        private void startVideoCallRosterContextMenuItem_Click(object sender, EventArgs e)
        {
            if (myRosterListTreeView.SelectedNode != null)
            {
                if (myRosterListTreeView.SelectedNode.Level == 1)
                {
                    VideoSessionInvite(myRosterListTreeView.SelectedNode.Tag.ToString());
                }
            }
        }

        private void startVideoCallContactContextMenuItem_Click(object sender, EventArgs e)
        {
            RadListBoxItem selectedItem = (RadListBoxItem)myContactsListBox.SelectedItem;
            if (selectedItem != null)
            {
                NTContact selectedContact = (NTContact)selectedItem.Tag;
                if (selectedContact.NTJabberID != "") VideoSessionInvite(selectedContact.NTJabberID);
            }
        }


        private void getPrepaidStatus()
        {
            myPrepaidAmountRefreshToolStripLabel.Visible = false;

            try
            {
                Remwave.Client.RSIFeaturesWS.RSIService ss = new Remwave.Client.RSIFeaturesWS.RSIService();
                ss.servicePrepaidAmountCompleted += new Remwave.Client.RSIFeaturesWS.servicePrepaidAmountCompletedEventHandler(ss_servicePrepaidAmountCompleted);
                ss.servicePrepaidAmountAsync(mUserAccount.Username, mUserAccount.Password);
            }
            catch (Exception)
            {
                ;
            }
        }

        void ss_servicePrepaidAmountCompleted(object sender, Remwave.Client.RSIFeaturesWS.servicePrepaidAmountCompletedEventArgs e)
        {
            try
            {
                myPrepaidAmountRefreshToolStripLabel.Visible = true;
                if (e.Error != null) return;
                myPrepaidAmountToolStripLabel.Text = e.Result.ToString();
            }
            catch (Exception)
            {
                myPrepaidAmountToolStripLabel.Text = "0.00";
            }

        }

        private void startVideoToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (myRosterListTreeView.SelectedNode != null)
            {
                if (myRosterListTreeView.SelectedNode.Level == 1)
                {
                    VideoSessionInvite(new JabberUser(myRosterListTreeView.SelectedNode.Tag.ToString()));
                }
            }
        }

        private void myClientNotifyIcon_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
            this.Activate();
            this.Show();
            this.BringToFront();
        }





















































        private delegate void PlayAudioFileDelegate(string fileName);
        private void PlayAudioFile(string fileName)
        {
            if (this.InvokeRequired)
            {
                BeginInvoke(new PlayAudioFileDelegate(PlayAudioFile), new object[] { fileName });
            }
            else
            {
                mAudioPlayer.Play(fileName);
            }
        }
        private delegate void StopPlayingAudioFileDelegate(string fileName);
        private void StopPlayingAudioFile(string fileName)
        {
            if (this.InvokeRequired)
            {
                BeginInvoke(new StopPlayingAudioFileDelegate(StopPlayingAudioFile), new object[] { fileName });
            }
            else
            {
                mAudioPlayer.Stop();
            }
        }



        private void myWebVoiceMailControl_StartPlayingVoiceMail(object sender, EventArgs e)
        {
            try
            {
                VoiceMailEntry vm = (VoiceMailEntry)sender;

                if (vm != null)
                {
                    PlayAudioFile(vm.FullPath);
                }
            }
            catch (Exception)
            {
                //
            }
        }

        private void myWebVoiceMailControl_StopPlayingVoiceMail(object sender, EventArgs e)
        {
            try
            {
                VoiceMailEntry vm = (VoiceMailEntry)sender;

                if (vm != null)
                {
                    StopPlayingAudioFile(vm.FullPath);
                }
            }
            catch (Exception)
            {
                //
            }
        }




        private void myMenuSearchContactToolStripButton_Click(object sender, EventArgs e)
        {
            StartUserSearch();
        }

        private void ClientForm_Shown(object sender, EventArgs e)
        {
            if (myLoginAutoLoginCheckBox.Checked) myLoginButton.PerformClick();
        }

        private void myPhoneCONFButton_Click(object sender, EventArgs e)
        {
            Console.WriteLine(sender.ToString());
        }

        private void myPhoneHOLDButton_Click(object sender, EventArgs e)
        {
            Console.WriteLine(sender.ToString());
        }



        private void homenetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenContactWindow(mUserAccount.Contact, null);
        }

        void imAccount_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = false;
            IMAccountWindow imAccount = (IMAccountWindow)sender;
            if (imAccount.Save)
            {
                try
                {


                    switch (ConfigIM.GetXMPPNetwork(imAccount.Domain))
                    {

                        case ConfigXMPPNetwork.MSN:
                            myClientConfiguration.SetMSNLoginOptions(imAccount.Username, imAccount.Password);
                            mXMPPIQ.RegisterUser(imAccount.Domain, imAccount.Username, imAccount.Password);
                            msnToolStripMenuItem.Image = Properties.Resources.im_msn;
                            break;
                        case ConfigXMPPNetwork.Yahoo:
                            myClientConfiguration.SetYahooLoginOptions(imAccount.Username, imAccount.Password);
                            mXMPPIQ.RegisterUser(imAccount.Domain, imAccount.Username, imAccount.Password);
                            yahooToolStripMenuItem.Image = Properties.Resources.im_yahoo;
                            break;
                        case ConfigXMPPNetwork.GaduGadu:
                            myClientConfiguration.SetGaduGaduLoginOptions(imAccount.Username, imAccount.Password);
                            mXMPPIQ.RegisterUser(imAccount.Domain, imAccount.Username, imAccount.Password);
                            ggToolStripMenuItem.Image = Properties.Resources.im_gadugadu;
                            break;
                        case ConfigXMPPNetwork.IRC:
                            myClientConfiguration.SetIRCLoginOptions(imAccount.Username, imAccount.Password);
                            mXMPPIQ.RegisterUser(imAccount.Domain, imAccount.Username, imAccount.Password);
                            //ircToolStripMenuItem.Image = Properties.Resources.im_irc; 
                            break;
                        case ConfigXMPPNetwork.AIM:
                            myClientConfiguration.SetAIMLoginOptions(imAccount.Username, imAccount.Password);
                            mXMPPIQ.RegisterUser(imAccount.Domain, imAccount.Username, imAccount.Password);
                            aimToolStripMenuItem.Image = Properties.Resources.im_aim;
                            break;
                        case ConfigXMPPNetwork.ICQ:
                            myClientConfiguration.SetICQLoginOptions(imAccount.Username, imAccount.Password);
                            mXMPPIQ.RegisterUser(imAccount.Domain, imAccount.Username, imAccount.Password);
                            icqToolStripMenuItem.Image = Properties.Resources.im_icq;
                            break;
                    }
                    XMPP_DiscoverIMNetworks();
                    XMPP_SendPresenceIMNetworks();

                    myClientConfigurationSerializer.SaveClientConfiguration(myClientConfiguration);
                }
                catch (Exception)
                {

                }
            }
            if (imAccount.Delete)
            {
                try
                {


                    mXMPPIQ.UnRegisterUser(imAccount.Domain);
                    switch (ConfigIM.GetXMPPNetwork(imAccount.Domain))
                    {

                        case ConfigXMPPNetwork.MSN:
                            myClientConfiguration.DeleteMSNLoginOptions();
                            msnToolStripMenuItem.Image = Properties.Resources.im_msn_offline;
                            break;
                        case ConfigXMPPNetwork.Yahoo:
                            myClientConfiguration.DeleteYahooLoginOptions();
                            yahooToolStripMenuItem.Image = Properties.Resources.im_yahoo_offline;
                            break;
                        case ConfigXMPPNetwork.GaduGadu:
                            myClientConfiguration.DeleteGaduGaduLoginOptions();
                            ggToolStripMenuItem.Image = Properties.Resources.im_gadugadu_offline;
                            break;
                        case ConfigXMPPNetwork.IRC:
                            myClientConfiguration.DeleteIRCLoginOptions();
                            //irToolStripMenuItem.Image = Properties.Resources.im_irc_offline; 
                            break;
                        case ConfigXMPPNetwork.AIM:
                            myClientConfiguration.DeleteAIMLoginOptions();
                            aimToolStripMenuItem.Image = Properties.Resources.im_aim_offline;
                            break;
                        case ConfigXMPPNetwork.ICQ:
                            myClientConfiguration.DeleteICQLoginOptions();
                            icqToolStripMenuItem.Image = Properties.Resources.im_icq_offline;
                            break;
                    }
                }
                catch (Exception)
                {

                }
            }
        }

        private void mSNToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IMAccountWindow imAccount = new IMAccountWindow(myClientConfiguration.MSN_Username, myClientConfiguration.MSN_Password, ConfigIM.MSN_IMServer, Properties.Resources.im_msn, Properties.Localization.txtTitleMSNNetwork);
            imAccount.FormClosing += new FormClosingEventHandler(imAccount_FormClosing);
            imAccount.Show();
        }

        private void yahooToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IMAccountWindow imAccount = new IMAccountWindow(myClientConfiguration.Yahoo_Username, myClientConfiguration.Yahoo_Password, ConfigIM.YAHOO_IMServer, Properties.Resources.im_yahoo, Properties.Localization.txtTitleYahooNetwork);
            imAccount.FormClosing += new FormClosingEventHandler(imAccount_FormClosing);
            imAccount.Show();
        }

        private void aimToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IMAccountWindow imAccount = new IMAccountWindow(myClientConfiguration.AIM_Username, myClientConfiguration.AIM_Password, ConfigIM.AIM_IMServer, Properties.Resources.im_aim, Properties.Localization.txtTitleAIMNetwork);
            imAccount.FormClosing += new FormClosingEventHandler(imAccount_FormClosing);
            imAccount.Show();
        }

        private void ggToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IMAccountWindow imAccount = new IMAccountWindow(myClientConfiguration.GG_Username, myClientConfiguration.GG_Password, ConfigIM.GG_IMServer, Properties.Resources.im_gadugadu, Properties.Localization.txtTitleGGNetwork);
            imAccount.FormClosing += new FormClosingEventHandler(imAccount_FormClosing);
            imAccount.Show();
        }

        private void icqToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IMAccountWindow imAccount = new IMAccountWindow(myClientConfiguration.ICQ_Username, myClientConfiguration.ICQ_Password, ConfigIM.ICQ_IMServer, Properties.Resources.im_icq, Properties.Localization.txtTitleICQNetwork);
            imAccount.FormClosing += new FormClosingEventHandler(imAccount_FormClosing);
            imAccount.Show();
        }

        private void myRosterListTreeView_BeforeLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            if (e.Node.Level != 0) e.CancelEdit = true;
        }

        private void myRosterListTreeView_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            if (e.Node.Level == 0 && e.Label != null)
            {
                try
                {
                    String oldGroup = e.Node.Text;
                    for (int i = 0; i < xmppsControl.BuddyCount; i++)
                    {
                        string[] groups = xmppsControl.BuddyGroup[i + 1].Split(Char.Parse(","));
                        if (Array.IndexOf(groups, oldGroup) >= 0)
                        {
                            //  xmppsControl.BuddyGroup[i+1] = e.Label;
                            mXMPPIQ.Roster(new JabberUser(xmppsControl.BuddyId[i + 1]), e.Label);
                        }
                    }
                }
                catch (Exception ex)
                {
#if TRACE
                    Console.Write("myRosterListTreeView_AfterLabelEdit : " + ex.Message);
#endif

                }
            }
            else
            {
                e.CancelEdit = true;
            }
        }

        private void myRosterListTreeView_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F2)
            {
                if ((myRosterListTreeView.SelectedNode != null) && (myRosterListTreeView.SelectedNode.Level == 0))
                {
                    myRosterListTreeView.SelectedNode.BeginEdit();
                }
            }
            else if (e.KeyCode == Keys.Delete)
            {
                if (myRosterListTreeView.SelectedNode != null)
                {
                    if (myRosterListTreeView.SelectedNode.Level == 1)
                    {
                        try
                        {
                            JabberUser jabberUser = new JabberUser(myRosterListTreeView.SelectedNode.Tag.ToString());
                            if (myRosterListTreeView.SelectedNode.Parent.Text == Properties.Localization.txtOtherGroup)
                            {
                                xmppsControl.Remove(jabberUser.EscapedJID, "", myRosterListTreeView.SelectedNode.Parent.Text);
                            }
                            else
                            {
                                xmppsControl.Remove(jabberUser.EscapedJID, "", "");
                            }

                        }
                        catch (Exception)
                        {

                        }
                    }
                }
            }
        }

        internal void OpenArchiveWindow(JabberUser jabberUser)
        {
            if (myArchiveWindow == null)
            {

                myArchiveWindow = new ArchiveWindow();
                myArchiveWindow.Disposed += new EventHandler(myArchiveWindow_Disposed);
            }

            myArchiveWindow.Open(myStorage, jabberUser);
            myArchiveWindow.tvUserList.ImageList = getPresenceImageList();

            myArchiveWindow.Show();
            myArchiveWindow.Activate();
        }

        private void myArchiveWindow_Disposed(object sender, EventArgs e)
        {
            myArchiveWindow = null;
        }

        private void messageHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenArchiveWindow(null);
        }

        private void OpenSettingsWindow()
        {
            if (mySettingsWindow == null)
            {

                mySettingsWindow = new SettingsWindow(AppDataDir);
                mySettingsWindow.Disposed += new EventHandler(mySettingsWindow_Disposed);
                mySettingsWindow.FormClosing += new FormClosingEventHandler(mySettingsWindow_FormClosing);
            }
            mySettingsWindow.Show();
            mySettingsWindow.Activate();
        }

        void mySettingsWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (mySettingsWindow.ApplyChanges)
            {
                ApplyClientSettings();
            }
            if (mySettingsWindow.AbortClosing) e.Cancel = true;
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenSettingsWindow();
        }

        private void mySettingsWindow_Disposed(object sender, EventArgs e)
        {
            mySettingsWindow = null;
        }

        private void ApplyClientSettings()
        {
            mClientSettings = mClientSettingsSerializer.Load();

            if (!mClientSettings.ProgramAtStartup)
            {
                try
                {
                    Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true).DeleteValue(Application.ProductName.ToString());
                }
                catch (Exception)
                {
                }
            }
            else
            {
                try
                {
                    Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true).SetValue(Application.ProductName.ToString(), "\"" + Application.ExecutablePath.ToString() + "\"" + " /tray");
                }
                catch (Exception)
                {
                }
            }

            if (mQualityAgentLogger != null)
            {
                mQualityAgentLogger.Enabled = mClientSettings.ProgramEnableQualityAgent;
            }

            //mSIPPhone.SetIncomingPhoneRingEnable(mClientSettings.PhoneDefaultRingToneEnabled ? 1 : 0);

            if (!mClientSettings.PhoneDefaultRingToneEnabled)
            {
                EventNotification eventNotificationStartRinging = new EventNotification();
                eventNotificationStartRinging.Event = ClientEvent.IncomingCallStartRinging;
                eventNotificationStartRinging.NotificationData = mClientSettings.PhoneCustomRingTone;
                eventNotificationStartRinging.NotificationType = EventNotificationType.RingingStart;
                mClientEvents.AddEventNotification(eventNotificationStartRinging);
            }

            //mSIPPhone.SetAudioVolume(mClientSettings.PhoneAudioSpeakerVolume, mClientSettings.PhoneAudioMicrophoneVolume);
            int[] mediaFormats = { 9, 9, 9, 9 };
            int i = 0;
            foreach (AudioCodec codec in mClientSettings.PhoneEnabledMediaFormats)
            {
                mediaFormats[i] = (int)codec.Format;
                if (i >= 3) break;
            }
            //mSIPPhone.SetMediaFormats(mClientSettings.PhoneEnabledMediaFormats.Count, mediaFormats[0], mediaFormats[1], mediaFormats[2], mediaFormats[3]);

        }


        internal void SetQualityAgentLogger(QualityAgentLogger qualityAgentLogger)
        {
            mQualityAgentLogger = qualityAgentLogger;
            mQualityAgentLogger.Enabled = mClientSettings.ProgramEnableQualityAgent;
        }

        private void linkOnlineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(ConfigWebLinks.MyAccountLink);
            }
            catch (Exception ex)
            {
#if (TRACE)
                Console.WriteLine("Link Online : " + ex.Message);
#endif
            }
        }



        private void myDialPadCallOrAnswerButton_MouseLeave(object sender, EventArgs e)
        {
            myDialPadCallOrAnswerButton.BackgroundImage = Properties.Resources.CallAnswerButton;

        }
        private void myDialPadCallOrAnswerButton_MouseEnter(object sender, EventArgs e)
        {
            myDialPadCallOrAnswerButton.BackgroundImage = Properties.Resources.CallAnswerButtonOver;
        }
        private void myDialPadCallOrAnswerButton_MouseHover(object sender, EventArgs e)
        {
            myDialPadCallOrAnswerButton.BackgroundImage = Properties.Resources.CallAnswerButtonOver;
        }
        private void myDialPadCallOrAnswerButton_MouseUp(object sender, MouseEventArgs e)
        {
            myDialPadCallOrAnswerButton.BackgroundImage = Properties.Resources.CallAnswerButtonOver;
        }
        private void myDialPadCallOrAnswerButton_MouseDown(object sender, MouseEventArgs e)
        {
            myDialPadCallOrAnswerButton.BackgroundImage = Properties.Resources.CallAnswerButtonDown;
        }


        private void myDialPadCallCancelButton_MouseLeave(object sender, EventArgs e)
        {
            myDialPadCallCancelButton.BackgroundImage = Properties.Resources.CallCancelButton;
        }

        private void myDialPadCallCancelButton_MouseEnter(object sender, EventArgs e)
        {
            myDialPadCallCancelButton.BackgroundImage = Properties.Resources.CallCancelButtonOver;
        }

        private void myDialPadCallCancelButton_MouseHover(object sender, EventArgs e)
        {
            myDialPadCallCancelButton.BackgroundImage = Properties.Resources.CallCancelButtonOver;
        }

        private void myDialPadCallCancelButton_MouseDown(object sender, MouseEventArgs e)
        {
            myDialPadCallCancelButton.BackgroundImage = Properties.Resources.CallCancelButtonDown;
        }

        private void myDialPadCallCancelButton_MouseUp(object sender, MouseEventArgs e)
        {
            myDialPadCallCancelButton.BackgroundImage = Properties.Resources.CallCancelButtonOver;
        }
    }
}
