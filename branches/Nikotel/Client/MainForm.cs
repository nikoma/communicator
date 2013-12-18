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
using Microsoft.DirectX.AudioVideoPlayback;
using Remwave.Client.Controls;
using Remwave.Client.Events;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Resources;
using System.Globalization;
using Microsoft.Win32;

namespace Remwave.Client
{
    public partial class ClientForm : ShapedForm
    {
        #region Localization
        private String mCultureInfoRegistryKey = @"SOFTWARE\" + Application.CompanyName + @"\" + Application.ProductName;                                   
        private CultureInfo mEnglishCulture = new CultureInfo("en-US");
        private CultureInfo mFrenchCulture = new CultureInfo("fr-FR");
        private CultureInfo mSpanishCulture = new CultureInfo("es-ES");
        private CultureInfo mPolishCulture = new CultureInfo("pl-PL");
        private CultureInfo mGermanCulture = new CultureInfo("de-DE");

        #endregion

        #region public

        public static String AppDataDir = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\" + Application.CompanyName + @"\" + Application.ProductName;
        public SIPPhone mySIPPhone = new SIPPhone();
        public UserAccount mUserAccount = new UserAccount();
        public NetworkInfo myNetworkInfo = new NetworkInfo();
        public ClientConfiguration myClientConfiguration = new ClientConfiguration();
        public Hashtable myBuddyGroups = new Hashtable();
        public Hashtable myBuddyList = new Hashtable();
        public Hashtable myBuddyPresence = new Hashtable();
        public ContactBook mContactBook = new ContactBook(new NTContactStore[] { NTContactStore.Local, NTContactStore.Outlook, NTContactStore.Server });

        public ClientEvents myClientEvents = new ClientEvents();

        public bool mFormIsClosing;
        public Size myClientSize = new Size(300, 466);

        public XMPPIQ mXMPPIQ;
        Thread probePresenceThread;

        public String[] PRESENCES;

        #endregion

        #region private

        private bool UpdatedSync = false;

        private int hitCount = 0;
        private long lastTick = -1;
        private bool showApp = false;

        //Window Forms
        private Remwave.Client.ChatWindow myChatWindow;
        private Remwave.Client.ContactsWindow myContactWindow;
        //TO DO SEARCH WINDOW
        private Remwave.Client.Phone2PhoneWindow myPhone2PhoneWindow;
        private Remwave.Client.SpeedDialWindow mySpeedDialWindow;
        public Remwave.Client.ArchiveWindow myArchiveWindow;
        public Remwave.Client.NikotalkieForm myNikotalkieWindow;
        //Media engine engine
        private const string ENGINE_KEY = "a2b8fffe907db6109a6b03f0f751574d3ec03b70d14958c182bca5bdb2e60ceacbef6e494b04eeba24be99e1b3942bd778a136cefdc1b9cc73d4ccbe8fee70b5";
        private PhoneLineState[] mySIPPhoneLines;
        private ClientCallBack myMediaEngineCallback;

        //Video Plugin 
        private VideoPlugin myVideoPlugin = new VideoPlugin("nvideo.npm", "nikomeeting lite");

        //Serializers
        private CallHistorySerializer myCallHistorySerializer = new CallHistorySerializer();
        private ClientConfigurationSerializer myClientConfigurationSerializer = new ClientConfigurationSerializer(Application.ProductName + @".dat");

        //AudioPlayback
        //TO DO WHAT ABOUT  Audio Playback

        //Other
        private List<CallRecord> myCallHistoryRecords = new List<CallRecord>();

        //Remwave Unified Storage
        private String mStorageFileName = @"\Nikotel.rusa";
        public Storage myStorage;

        #endregion

        #region user32.dll imports
        [DllImport("user32.dll")]
        public static extern UInt32 RegisterWindowMessage([MarshalAs(UnmanagedType.LPTStr)] String lpString);

        [DllImport("user32.dll")]
        public static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vlc);

        [DllImport("user32.dll")]
        public static extern bool UnregisterHotKey(IntPtr hWnd, int id);
        #endregion

        #region WndProc hook / meta key watchdog
        /// <summary>
        /// WndProc has following direction
        /// - counting ALT meta key presses inside a second
        /// </summary>

        uint WM_DIAL = RegisterWindowMessage(Guid.NewGuid().ToString());

        protected override void WndProc(ref Message m)
        {
            try
            {
                if (mUserAccount.LoggedIn())
                {

                    if (m.Msg == 74)
                    {
                        // if (m.LParam == null) return;
                        string s = Marshal.PtrToStringAuto(m.LParam);

                    }
                    if (m.Msg == 0x0312)
                    {
                        if (lastTick == -1 || System.DateTime.Now.Ticks < (lastTick + 20000000))
                        {
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
            catch (Exception ex)
            {
                Console.WriteLine("WndProc " + ex.Message);
            }
        }
        #endregion
		
        private void LocalizeComponent()
        {
            //set all controls default localization
            statusStrip1.Text = Properties.Localization.txtTitleStatus;
            myGoOfflineToolStripMenuItem.Text = Properties.Localization.txtStatusOffline;
            myGoOnlineToolStripMenuItem.Text = Properties.Localization.txtStatusOnline;
            myGoAwayToolStripMenuItem.Text = Properties.Localization.txtStatusAway;
            myGoExtendedAwayToolStripMenuItem.Text = Properties.Localization.txtStatusExtendedAway;
            myGoDoNotDisturbToolStripMenuItem.Text = Properties.Localization.txtStatusDND;
            myMainMenuStrip.Text = Properties.Localization.txtMenu;
		    plikToolStripMenuItem.Text = Properties.Localization.txtMenuFile;
            myMainMenuSignOutItem.Text = Properties.Localization.txtMenuSignOut;
            myMainMenuExitItem.Text = Properties.Localization.txtMenuExit;
            helpToolStripMenuItem.Text = Properties.Localization.txtMenuHelp;
            myMainMenuAboutItem.Text = Properties.Localization.txtMenuAbout;
            meToolStripMenuItem.Text = Properties.Localization.txtMenuMe;
            homenetToolStripMenuItem.Text = Properties.Localization.txtMenuIMJOCCOme;
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
            //myTabItemVoiceMail.Text = Properties.Localization.txtTitleVoiceMail;
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
            myMainAccountTabPage.Text = Properties.Localization.txtTitleMainJOCCOmeTabPage;
            myMenuAddContactToolStripButton.Text = Properties.Localization.txtCMenuAddContact;
            //myMenuSearchContactToolStripButton.Text = Properties.Localization.txtCMenuSearchforUsers;
            myMenuChatToolStripButton.Text = Properties.Localization.txtCMenuChat;
            myMenuCallToolStripButton.Text = Properties.Localization.txtCMenuCall;
            myPrepaidAmountToolStripLabel.Text = Properties.Localization.txtCMenuPrepaidAmount;
            myMenuPhone2PhoneCallToolStripButton.Text = Properties.Localization.txtCMenuCallPhone2Phone;
            myPrepaidAmountRefreshToolStripLabel.Text = Properties.Localization.txtCMenuPrepaidAmountRefresh;
            myMenuVideoCallToolStripButton.Text = Properties.Localization.txtCMenuVideoCall;
            myNotifyIcon.Text = Properties.Localization.txtTitleNotifyIcon;




        }
		
		
		
        public ClientForm()
        {
            RegistryKey key = Registry.LocalMachine.OpenSubKey(mCultureInfoRegistryKey);
            if (key!=null)
            {
                try
                {
                    String value = key.GetValue("CultureInfo", "").ToString();
                    if (value.Length == 5)
                    {
                        CultureInfo currentCulture = new CultureInfo(value);
                        Thread.CurrentThread.CurrentUICulture = currentCulture;
                    }
                }
                catch (Exception)
                {
                    
                   
                }
                 key.Close();
			}


            //INITIALIZE STORAGE VERISIONING

            List<StorageDDL> storageDDL = new List<StorageDDL>();
            storageDDL.Add(new StorageDDL("1.0.0", Properties.Resources.fbDDL_1_0_0));
            storageDDL.Add(new StorageDDL("1.0.1", Properties.Resources.fbDDL_1_0_1));
            storageDDL.Add(new StorageDDL("1.0.2", Properties.Resources.fbDDL_1_0_2));
            storageDDL.Add(new StorageDDL("1.0.3", Properties.Resources.fbDDL_1_0_3));
           
            myStorage = new Storage(storageDDL, "1.0.3");

            InitializeComponent();
            LocalizeComponent();

            PRESENCES = new String[] { Properties.Localization.txtStatusOffline, Properties.Localization.txtStatusOnline, Properties.Localization.txtStatusAway, Properties.Localization.txtStatusExtendedAway, Properties.Localization.txtStatusDND };

            // Add the handlers to the NetworkChange events.
            NetworkChange.NetworkAvailabilityChanged +=
                NetworkAvailabilityChanged;
            NetworkChange.NetworkAddressChanged +=
                NetworkAddressChanged;

            // initialize phone lines states
            mySIPPhoneLines = new PhoneLineState[4];
            mySIPPhoneLines[0] = new PhoneLineState();
            mySIPPhoneLines[1] = new PhoneLineState();
            mySIPPhoneLines[2] = new PhoneLineState();
            mySIPPhoneLines[3] = new PhoneLineState();

            //initialize client events
            EventNotification eventNotificationIncomingInstantMessage = new EventNotification();
            eventNotificationIncomingInstantMessage.Event = ClientEvent.IncomingInstantMessage;
            eventNotificationIncomingInstantMessage.NotificationData = Directory.GetCurrentDirectory() + @"\Sounds\im.mp3";
            eventNotificationIncomingInstantMessage.NotificationType = EventNotificationType.Sound;
            myClientEvents.AddEventNotification(eventNotificationIncomingInstantMessage);

            EventNotification eventNotificationIncomingNudge = new EventNotification();
            eventNotificationIncomingNudge.Event = ClientEvent.IncomingNudge;
            eventNotificationIncomingNudge.NotificationData = Directory.GetCurrentDirectory() + @"\Sounds\nudge.mp3";
            eventNotificationIncomingNudge.NotificationType = EventNotificationType.Sound;
            myClientEvents.AddEventNotification(eventNotificationIncomingNudge);

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

            //set border and background colors











            //open login panel and process login
            myMainWindowSplitContainer.Panel1Collapsed = false;
            myMainWindowSplitContainer.Panel2Collapsed = true;

            //Media Engine Callback
            myMediaEngineCallback = new ClientCallBack(this.Callback);

            //link Presence Images
            myRosterListTreeView.ImageList = this.myPresenceImagesList;

            //select Default Phone Line Tab
            myPhoneLinesTabControl.SelectedTab = myPhoneLine0Tab;
            myDialPadTabControl.SelectedTab = myTabItemDialPad;

            //preset size
            this.ClientSize = myClientSize;
            this.MinimumSize = this.Size;
            this.MaximumSize = this.Size;
            this.MaximizeBox = false;

            bool result = RegisterHotKey(this.Handle, this.GetType().GetHashCode(), 1, 0);

            myVideoPlugin.VideoPluginExit += new EventHandler(myVideoPlugin_VideoPluginExit);

            initializeDialPadMenuItems();

            //Initialize mXMPPIQ
            mXMPPIQ = new XMPPIQ(ConfigIM.IMServer);
            mXMPPIQ.SendIQMessage += new EventHandler(mXMPPIQ_SendIQMessage);

        }

        void mXMPPIQ_SendIQMessage(object sender, EventArgs e)
        {
            if (xmppControl.Connected)
            {
                XMPPIQ.IQMessage message = (XMPPIQ.IQMessage)sender;
                xmppControl.SendCommand(message.Message);
#if (TRACE)
                Console.WriteLine("To:" + message.To + "|" + "Message:" + message.Message);
#endif
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
                result = mySIPPhone.HoldOnOffCall(3);
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
                result = mySIPPhone.HoldOnOffCall(2);
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
                result = mySIPPhone.HoldOnOffCall(1);
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
                result = mySIPPhone.HoldOnOffCall(0);
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
                result = mySIPPhone.ConferenceOnOffCall(3);
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
                result = mySIPPhone.ConferenceOnOffCall(2);
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
                result = mySIPPhone.ConferenceOnOffCall(1);
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
                result = mySIPPhone.ConferenceOnOffCall(0);
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

        public void StartActivity(Activity activity)
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
                case ActivityType.NikotalkieMessage:
                    myNikotalkieWindow.StartComposing(activity.ActivityJabberUser.Username);
                    break;
                default:
                    break;
            }


        }
		
        public void StartNewIM(String jid, bool setFocus)
        {
            JabberUser jaberUser = new JabberUser(jid);
            StartNewIM(jaberUser, setFocus);
        }

        public void StartNewIM(JabberUser jabberUser, bool setFocus)
        {
            try
            {
                myChatWindow.NewChat(jabberUser, setFocus);
                xmppControl.ProbePresence(jabberUser.JID);
            }
            catch (Exception ex)
            {
#if (TRACE)
                Console.WriteLine("StartNewIM : " + ex.Message);
#endif
            }
        }

        public void StartNewVideoCall(String jid)
        {
            JabberUser jabberUser = new JabberUser(jid);
            StartNewVideoCall(jabberUser);
        }
        public void StartNewVideoCall(JabberUser jabberUser)
        {
            VideoSessionInvite(jabberUser);
        }

        public void StartNewEmail(String email)
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

        public void StartNewContact(String jabberID)
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

                    myContactWindow = new ContactsWindow(this, null, properties);
                    myContactWindow.FormClosing += new FormClosingEventHandler(myContactWindow_FormClosing);
                    myContactWindow.Show();
                }
            }
            catch (Exception ex)
            {
#if (TRACE)
                Console.WriteLine("StartNewContact : " + ex.Message);
#endif
            }
        }



        //TO DO : STart User Search

























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
                    Console.WriteLine("Amitelo_Load JOCCOmeWS : " + ex.Message);
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
                        if (MessageBox.Show(this,
                            Properties.Localization.txtInfoNewVersion,
                            Properties.Localization.txtInfoUpdateAvaliable,
                            MessageBoxButtons.OKCancel,
                            MessageBoxIcon.Information
                            ) == DialogResult.OK)
                        {
                            System.Diagnostics.Process.Start(downloadlink);
                            Application.Exit();
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
            if ((!this.mFormIsClosing) && (mUserAccount.LoggedIn()))
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

                    if (!xmppControl.Connected)
                    {
                        //network detected, try authenticate at jabber
                        try
                        {
                            myServiceStateCheckTimer.Tag = DateTime.Now;
                            xmppControl.Connect(mUserAccount.Username, mUserAccount.Password);
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
                        result = mySIPPhone.ReStartSip(myNetworkInfo.LocalIP);
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
                    if (xmppControl.Connected) xmppControl.ChangePresence(1, PRESENCES[1]);

                    return true;
                }
            }
            return false;
        }

        private bool InitializeClient()
        {
            if (!this.mFormIsClosing)
            {
                for (; true; )
                {
                    TELEPHONY_RETURN_VALUE result;
                    DetectNetwork();

                    if (!myNetworkInfo.Online)
                    {
                        myNotifyIcon.ShowBalloonTip(10,
                            Properties.Localization.txtInfoServiceNotReachable,
                            Properties.Localization.txtInfoServiceNotReachableDesc,
                            ToolTipIcon.Warning);
#if (TRACE)
                        Console.WriteLine("Network:SIP Proxy Does not respond, network time out.");
#endif
                        return false;
                    }

                    //Initialize ChatWindow
                    if (myChatWindow != null) myChatWindow.Dispose();
                    myChatWindow = new Remwave.Client.ChatWindow(this);

                    if (myNikotalkieWindow != null) myNikotalkieWindow.Dispose();
                    myNikotalkieWindow = new NikotalkieForm(this);

                    //network detected, try authenticate at jabber
                    try
                    {
                        myServiceStateCheckTimer.Tag = DateTime.Now;
                        if (xmppControl.Connected) xmppControl.Disconnect();
                        xmppControl.InvokeThrough = this;
                        xmppControl.Timeout = 7200;
                        xmppControl.IMServer = ConfigIM.IMServer;
                        xmppControl.IMPort = ConfigIM.IMPort;
                        xmppControl.Connect(myLoginUsernameInput.Text, myLoginPasswordInput.Text);
                    }
                    catch (nsoftware.IPWorksSSL.IPWorksSSLException ex)
                    {
#if (TRACE)
                                          Console.WriteLine("IM:Connect error:" + ex.Code.ToString() + " - " + ex.Message);
#endif


                        MessageBox.Show(this,
                            Properties.Localization.txtInfoWrongUsernamePasswordDesc,
                            Properties.Localization.txtInfoWrongUsernamePassword,
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error
                            );
                        return false;
                    }
                    if (!myStorage.Open(AppDataDir, mStorageFileName, myLoginUsernameInput.Text, false))
                    {
                        if (DialogResult.Yes == MessageBox.Show(
                            Properties.Localization.txtInfoStorageEngineErrorDesc,
                            Properties.Localization.txtInfoStorageEngineError,
                            MessageBoxButtons.YesNoCancel, MessageBoxIcon.Error))
                        {
                            myStorage.Open(AppDataDir, mStorageFileName, myLoginUsernameInput.Text, true);
                        }
                    }
                    try
                    {
                        result = mySIPPhone.InitEngine(myMediaEngineCallback, myLoginUsernameInput.Text, myLoginPasswordInput.Text, ConfigSIP.Realm, ConfigSIP.ProxyAddress, myNetworkInfo.LocalIP, ENGINE_KEY, ConfigSIP.UserAgent);
                        if (result != TELEPHONY_RETURN_VALUE.SipSuccess)
                        {
                            xmppControl.Disconnect();
#if (TRACE)
                            Console.WriteLine("SIP:InitEngine:" + result.ToString());                 
#endif

                            switch (result)
                            {
                                case TELEPHONY_RETURN_VALUE.SipAudioInFailure:
                                    myNotifyIcon.ShowBalloonTip(10,
                                        Properties.Localization.txtInfoSipEngineAudioInError,
                                        Properties.Localization.txtInfoSipEngineAudioInErrorDesc,
                                        ToolTipIcon.Warning);
                                    break;
                                case TELEPHONY_RETURN_VALUE.SipAudioOutFailure:
                                    myNotifyIcon.ShowBalloonTip(10,
                                        Properties.Localization.txtInfoSipEngineAudioOutError,
                                        Properties.Localization.txtInfoSipEngineAudioOutErrorDesc,
                                        ToolTipIcon.Warning);
                                    break;
                                default:
                                    myNotifyIcon.ShowBalloonTip(10,
                                        Properties.Localization.txtInfoSipEngineGeneralError,
                                        String.Format(Properties.Localization.txtInfoSipEngineGeneralErrorDesc, (int)result),
                                        ToolTipIcon.Warning);
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
                        myNotifyIcon.ShowBalloonTip(10,
                        Properties.Localization.txtInfoServiceNotReachable,
                        Properties.Localization.txtInfoServiceNotReachableDesc,
                        ToolTipIcon.Warning);
                        break;
                    }

                    mUserAccount.Login(myLoginUsernameInput.Text, myLoginPasswordInput.Text);
                    myNikotalkieWindow.nikotalkieControl.Login(myLoginUsernameInput.Text, myLoginPasswordInput.Text, ConfigWebLinks.RestNikotalkieUrl, AppDataDir + @"\" + myLoginUsernameInput.Text);
                    //TO DO VoicemAil Initialization
                    myNotifyIcon.ShowBalloonTip(10,
                        Properties.Localization.txtInfoServiceConnected,
                        Properties.Localization.txtInfoServiceConnectedDesc,
                        ToolTipIcon.Info
                        );
                    if (xmppControl.Connected) xmppControl.ChangePresence(1, PRESENCES[1]);
                    return true;
                }
            }
            return false;
        }

        private void ClientForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (e.CloseReason == CloseReason.UserClosing)
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
                if (xmppControl.Connected) xmppControl.Disconnect();
                mySIPPhone.ShutdownEngine();

                UnregisterHotKey(this.Handle, this.GetType().GetHashCode());

                CallHistory tmpCallHistory = new CallHistory(mUserAccount.Username);
                tmpCallHistory.CallRecords = myCallHistoryRecords.ToArray();
                myCallHistorySerializer.SaveCallHistory(tmpCallHistory, mUserAccount.Username);
                myStorage.Close();

                if (myVideoPlugin != null) myVideoPlugin.Dispose();
            }
            catch (Exception ex)
            {
#if (TRACE)
                Console.WriteLine("AmiteloForm_FormClosing : " + ex.Message);     
#endif

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
                    Properties.Localization.txtInfoServiceUnavaliable,
                    Properties.Localization.txtInfoServiceUnavaliableDesc,
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
                if (mUserAccount.LoggedIn())
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
                    mySIPPhoneLines[PhoneLineID].State = mySIPPhone.GetPhoneLineState(PhoneLineID);
                    mySIPPhoneLines[PhoneLineID].CallDirection = mySIPPhone.GetPhoneLineCallDirection(PhoneLineID);
                    mySIPPhoneLines[PhoneLineID].CallRecordingActive = mySIPPhone.GetPhoneLineCallRecordingActive(PhoneLineID) == 1 ? true : false;
#if (TRACE)
                    Console.WriteLine("SIP|" + PhoneLineID.ToString() + " : " + sNot + " / " + sRet);
                    Console.WriteLine("SIP-LS|" + PhoneLineID.ToString() + ":" + mySIPPhoneLines[PhoneLineID].State.ToString());
#endif
                    switch (retval)
                    {
                        //reset phone line state
                        case TELEPHONY_RETURN_VALUE.SipOnHook:
                            mySIPPhoneLines[PhoneLineID].OnHook(myCallHistoryRecords);
                            ((TabItem)myPhoneLinesTabControl.Items[PhoneLineID]).ImageIndex = 0;
                            break;

                        case TELEPHONY_RETURN_VALUE.SipIncomingCallStart:

                            string myCallFrom = mySIPPhone.GetIncomingCallDetails(PhoneLineID);

                            myNotifyIcon.ShowBalloonTip(10,
                                Properties.Localization.txtInfoIncomingCall,
                                String.Format(Properties.Localization.txtInfoIncomingCallDesc, myCallFrom),
                                ToolTipIcon.Info);

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

                            myDialPadTabControl.SelectedTab = myTabItemDialPad;

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
                            mySIPPhoneLines[PhoneLineID].LastErrorMessage = Properties.Localization.txtInfoLineBusy;
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
        }
		
        public void StartNewCall(int phoneLineID, JabberUser jabberUser)
        {
            if (jabberUser.Network == ConfigXMPPNetwork.Nikotel)
            {
                StartNewCall(phoneLineID, jabberUser.Username);
            }
        }

        public void StartNewCall(int phoneLineID, string Destination)
        {
            TELEPHONY_RETURN_VALUE result;
            //sip:18585477597@examplevoip.com
            try
            {
                if (phoneLineID == -1)
                {
                    phoneLineID = mySIPPhone.GetFreePhoneLine();
                }

                //check if not already started
                for (int i = 0; i < 4; i++)
                {
                    if ((mySIPPhoneLines[i].LastDialedNumber == Destination)
                        && (mySIPPhoneLines[i].State == TELEPHONY_RETURN_VALUE.SipInCall)
                        && (mySIPPhoneLines[phoneLineID].State == TELEPHONY_RETURN_VALUE.SipOnHook)
                        )
                    {
                        return;
                    }
                }

                mySIPPhoneLines[phoneLineID].LastDialedNumber = Destination;
                myMainTabControl.SelectedTab = myMainDialPadTabPage;

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
                myDialPadTabControl.SelectedTab = myTabItemDialPad;
                result = mySIPPhone.CallOrAnswer(phoneLineID, "sip:" + Destination + "@" + ConfigSIP.Realm);
#if (TRACE)
                Console.WriteLine("CallOrAnswer : " + result.ToString());
#endif

            }
            catch (Exception ex)
            {
#if (TRACE)
                Console.WriteLine("StartNewCall : " + ex.Message);
#endif
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
                        myPhoneCONFButton.Image = mySIPPhoneLines[PhoneLineID].CallConferenceActive ? Properties.Resources.ButtonOn : Properties.Resources.ButtonOff;



                        myPhoneCONFButton.Items[0].Enabled = mySIPPhoneLines[0].CallActive;
                        myPhoneCONFButton.Items[1].Enabled = mySIPPhoneLines[1].CallActive;
                        myPhoneCONFButton.Items[2].Enabled = mySIPPhoneLines[2].CallActive;
                        myPhoneCONFButton.Items[3].Enabled = mySIPPhoneLines[3].CallActive;

                        myPhoneCONFButton.Items[0].Text = mySIPPhoneLines[0].CallActive ? Properties.Localization.txtTabLine0 + mySIPPhoneLines[0].LastDialedNumber : Properties.Localization.txtTabLine0 + Properties.Localization.txtInfoLineBusy;
                        myPhoneCONFButton.Items[1].Text = mySIPPhoneLines[1].CallActive ? Properties.Localization.txtTabLine1 + mySIPPhoneLines[1].LastDialedNumber : Properties.Localization.txtTabLine1 + Properties.Localization.txtInfoLineBusy;
                        myPhoneCONFButton.Items[2].Text = mySIPPhoneLines[2].CallActive ? Properties.Localization.txtTabLine2 + mySIPPhoneLines[2].LastDialedNumber : Properties.Localization.txtTabLine2 + Properties.Localization.txtInfoLineBusy;
                        myPhoneCONFButton.Items[3].Text = mySIPPhoneLines[3].CallActive ? Properties.Localization.txtTabLine3 + mySIPPhoneLines[3].LastDialedNumber : Properties.Localization.txtTabLine3 + Properties.Localization.txtInfoLineBusy;


                        ((RadMenuItem)(myPhoneCONFButton.Items[0])).IsChecked = mySIPPhoneLines[0].CallConferenceActive;
                        ((RadMenuItem)(myPhoneCONFButton.Items[1])).IsChecked = mySIPPhoneLines[1].CallConferenceActive;
                        ((RadMenuItem)(myPhoneCONFButton.Items[2])).IsChecked = mySIPPhoneLines[2].CallConferenceActive;
                        ((RadMenuItem)(myPhoneCONFButton.Items[3])).IsChecked = mySIPPhoneLines[3].CallConferenceActive;

                        //hold

                        myPhoneHOLDButton.Items[0].Enabled = mySIPPhoneLines[0].CallActive;
                        myPhoneHOLDButton.Items[1].Enabled = mySIPPhoneLines[1].CallActive;
                        myPhoneHOLDButton.Items[2].Enabled = mySIPPhoneLines[2].CallActive;
                        myPhoneHOLDButton.Items[3].Enabled = mySIPPhoneLines[3].CallActive;

                        myPhoneHOLDButton.Items[0].Text = mySIPPhoneLines[0].CallActive ? Properties.Localization.txtTabLine0 + mySIPPhoneLines[0].LastDialedNumber : Properties.Localization.txtTabLine0 + Properties.Localization.txtInfoLineBusy;
                        myPhoneHOLDButton.Items[1].Text = mySIPPhoneLines[1].CallActive ? Properties.Localization.txtTabLine1 + mySIPPhoneLines[1].LastDialedNumber : Properties.Localization.txtTabLine1 + Properties.Localization.txtInfoLineBusy;
                        myPhoneHOLDButton.Items[2].Text = mySIPPhoneLines[2].CallActive ? Properties.Localization.txtTabLine2 + mySIPPhoneLines[2].LastDialedNumber : Properties.Localization.txtTabLine2 + Properties.Localization.txtInfoLineBusy;
                        myPhoneHOLDButton.Items[3].Text = mySIPPhoneLines[3].CallActive ? Properties.Localization.txtTabLine3 + mySIPPhoneLines[3].LastDialedNumber : Properties.Localization.txtTabLine3 + Properties.Localization.txtInfoLineBusy;


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
                        result = mySIPPhone.StartDTMF(PhoneLineID, myDialedTone);
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
                    result = mySIPPhone.StopDTMF(PhoneLineID);
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
#if (TRACE)
                   Console.WriteLine(result.ToString());         
#endif
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
                if (myDestinationTextBox.Text != mUserAccount.Username)
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
                result = mySIPPhone.XferCall(PhoneLineID, "sip:" + myDestinationTextBox.Text + "@" + ConfigSIP.Realm);
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
                result = mySIPPhone.RecOnOffCall(PhoneLineID, Environment.GetFolderPath(Environment.SpecialFolder.Desktop));
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
                mySIPPhoneLines[PhoneLineID].LastDialedNumber = myDestinationTextBox.Text;
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
                        if(contact.NTPicture!=null & contact.NTPicture.Length>0) this.tmplContactListItem.Image = ImageProcessing.FixedSize(ImageProcessing.FromString(contact.NTPicture), 60, 60);
                    }
                    catch (Exception)
                    {
                      
                    }

                    if (this.tmplContactListItem.Image == null) this.tmplContactListItem.Image = ((System.Drawing.Image)(Properties.Resources.ContactBlank)); 

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

        private void LoadContactsBook(ContactList contactList)
        {
            myContactsListBox.Items.Clear();
          
                
            
            foreach (NTContact contact in contactList)
            {
                if (contact.NTDeleted != "true" && contact.NTJabberID != mUserAccount.Username)
                {
                    this.myContactsListBox.Items.AddRange(new Telerik.WinControls.RadItem[] {
                    BuildContactItem(contact)});
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

        private void myContactsListBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (myContactsListBox.SelectedItem != null)
                {
                    Telerik.WinControls.UI.RadListBoxItem clickedItem = (Telerik.WinControls.UI.RadListBoxItem)myContactsListBox.SelectedItem;
                    myContactWindow = new ContactsWindow(this, (NTContact)clickedItem.Tag, null);
                    myContactWindow.FormClosing += new FormClosingEventHandler(myContactWindow_FormClosing);
                    myContactWindow.Show();
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
            if (!mUserAccount.LoggedIn())
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
                        Properties.Localization.txtInfoServiceUnavaliable,
                        Properties.Localization.txtInfoServiceUnavaliableDesc,
                        ToolTipIcon.Warning);
                }
            }
        }

        public bool SendJabberXevent(String jid, string jabberXevent)
        {
            try
            {
                jabberXevent = jabberXevent.Trim();
                if (jabberXevent.Length == 0) return false;
                xmppControl.MessageType = nsoftware.IPWorksSSL.XmppsMessageTypes.mtChat;
                xmppControl.OtherData = jabberXevent;
                xmppControl.SendMessage(jid);
                return true;
            }
            catch (Exception e)
            {
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
		
        public bool SendMessage(JabberUser jabberUser, string messageText, string messageHTML)
        {
            try
            {
                messageText = messageText.Trim();
                messageHTML = messageHTML.Trim();

                String escapedMessageText = messageText.Replace(@"&", @"&amp;").Replace(@"<", @"&lt;").Replace(@">", @"&gt;");
                String escapedMessageHTML = messageHTML.Replace(@"&", @"&amp;").Replace(@"<", @"&lt;").Replace(@">", @"&gt;");
                if (messageText.Length == 0 || messageHTML.Length == 0) return false;
                xmppControl.MessageText = escapedMessageHTML;
                xmppControl.MessageHTML = escapedMessageHTML;
                xmppControl.MessageType = nsoftware.IPWorksSSL.XmppsMessageTypes.mtChat;
                xmppControl.SendMessage(jabberUser.JID);
                myStorage.AddMessageToArchive(jabberUser.JID, myStorage.StorageGUID(), messageText, messageHTML, true);

                return true;
            }
            catch (Exception e)
            {
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

        private TreeNode RosterGetNode(TreeNode node, String jid)
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
                        TreeNode newnode = RosterGetNode(node.FirstNode, jid);
                        if (newnode != null)
                        {
                            return newnode;
                        }
                        else if (node.NextNode != null)
                        {
                            return RosterGetNode(node.NextNode, jid);
                        }
                    }
                    else if (node.NextNode != null)
                    {
                        return RosterGetNode(node.NextNode, jid);
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
        private delegate void probePresenceFunctionDelegate();
        private void probePresenceFunction()
        {
            if (this.InvokeRequired)
            {
                BeginInvoke(new probePresenceFunctionDelegate(probePresenceFunction));
                return;
            }

            try
            {
                //probe presence and subscribe
                Thread.Sleep(1000);
                for (int i = 1; i <= xmppControl.BuddyCount; i++)
                {
                    String id = xmppControl.BuddyId[i];

                    switch (xmppControl.BuddySubscription[i])
                    {
                        case XmppsBuddySubscriptions.stNone:
                            if (!UpdatedSync) xmppControl.SubscribeTo(id);
                            break;
                        case XmppsBuddySubscriptions.stTo:
                            xmppControl.ProbePresence(id);
                            break;
                        case XmppsBuddySubscriptions.stFrom:
                            if (!UpdatedSync) xmppControl.SubscribeTo(id);
                            break;
                        case XmppsBuddySubscriptions.stBoth:
                            xmppControl.ProbePresence(id);
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
            UpdatedSync = true;
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
                if (e == null || !xmppControl.Connected) return;
#if (TRACE)
                Console.WriteLine("IM-SubscriptionRequest:" + "(" + e.From + ")");     
#endif

                if (mContactBook.getCandidatesForJabberID(e.From + @"@" + e.Domain).Count > 0)
                {
                    e.Accept = true;
                    xmppControl.SubscribeTo(e.From + @"@" + e.Domain);
                }
                else
                {
                    if (MessageBox.Show(
                       String.Format(Properties.Localization.txtInfoIMSubcriptionRequestDesc, e.From),
                        Properties.Localization.txtInfoIMSubcriptionRequest,
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question,
                        MessageBoxDefaultButton.Button1
                        ) == DialogResult.Yes)
                    {
                        e.Accept = true;
                    }
                    if (MessageBox.Show(
                        String.Format(Properties.Localization.txtInfoAddToContactDesc, e.From),
                       Properties.Localization.txtInfoAddToContact,
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question,
                        MessageBoxDefaultButton.Button1
                        ) == DialogResult.Yes)
                    {
                        Hashtable properties = new Hashtable();
                        properties.Add("NTJabberID", e.From + @"@" + e.Domain);
                        myContactWindow = new ContactsWindow(this, null, properties);
                        myContactWindow.FormClosing += new FormClosingEventHandler(myContactWindow_FormClosing);
                        myContactWindow.Show();
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
                myRosterListTreeView.Nodes.Clear();
                myBuddyGroups.Clear();
                myBuddyList.Clear();

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
                            myBuddyGroups.Add(Properties.Localization.txtOtherGroup, xmppControl.BuddyGroup[i]);
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
                        groupNode = myRosterListTreeView.Nodes.Add(Properties.Localization.txtOtherGroup);
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
                                JabberUser jabberUser = new JabberUser(xmppControl.BuddyId[i]);

                                TreeNode newnode = new TreeNode(jabberUser.Nick);
                                newnode.Tag = jabberUser.JID;
                                if (myBuddyPresence.Contains(jabberUser.JID))
                                {
                                    newnode.ImageIndex = (int)myBuddyPresence[jabberUser.JID];
                                }
                                else
                                {
                                    newnode.ImageIndex = 0;
                                }
                                groupNode.Nodes.Add(newnode);
                                groupNode.ImageIndex = 5;
                                groupNode.SelectedImageIndex = 5;

                                if (!myBuddyList.Contains(jabberUser.JID))
                                {
                                    myBuddyList.Add(jabberUser.JID, xmppControl.BuddyGroup[i]);
                                }
                            }
                        }
                    }
                }

                myRosterListTreeView.ExpandAll();
                ThreadStart st = new ThreadStart(probePresenceFunction);
                probePresenceThread = new Thread(st);
                // start the thread
                probePresenceThread.Start();
            }
            catch (Exception ex)
            {
#if (TRACE)
                     Console.WriteLine("xmppControl_OnSync" + ex.Message);
#endif

            }
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
            
            //update the roster imageindex for this buddy
            if (myBuddyPresence.Contains(jaberUser.JID) && (int)myBuddyPresence[jaberUser.JID] != e.Availability)
            {

                myChatWindow.AddNotification(
                    jaberUser.JID,
                    "PRESENCE",
                    String.Format(Properties.Localization.txtInfoPresenceNotificationDesc, new object[]{
                    jaberUser.Nick , PRESENCES[e.Availability] , e.Status}),
                    "",
                    Guid.NewGuid().ToString(),
                    false
                    );
                myChatWindow.UpdatePresence(jaberUser.JID, e.Availability);
            }
            myBuddyPresence.Remove(jaberUser.JID);
            myBuddyPresence.Add(jaberUser.JID, e.Availability);

            TreeNode node = RosterGetNode(myRosterListTreeView.Nodes[0], jaberUser.JID);
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
                String messageText = e.MessageText == null ? "" : e.MessageText;
                String messageHTML = e.MessageHTML == null || e.MessageHTML == "" ? messageText : e.MessageHTML;

                messageText = messageText.Trim().Replace(@"&lt;", @"<").Replace(@"&gt;", @">").Replace(@"&amp;", @"&");
                messageHTML = messageHTML.Trim().Replace(@"&lt;", @"<").Replace(@"&gt;", @">").Replace(@"&amp;", @"&");

                if (e.Domain == "")
                {//INFO
                    if (messageText != "")
                    {
                        myChatWindow.AddNotification(Properties.Localization.txtInfoIMNotification, "INFO", messageText, "", Guid.NewGuid().ToString(), false);
                    }
                }
                else
                {//MESSAGE
                    String jid = e.From + "@" + e.Domain;
                    if (messageText != "" || messageHTML != "")
                    {
                        myChatWindow.IncomingMessage(jid, messageText, messageHTML);
                    }
#if(TRACE) 
                    Console.WriteLine("IM-Message:" + "(" + jid + ")" + e.MessageText);
#endif
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
#endif
            if ((!this.mFormIsClosing) && (!xmppControl.Connected) && (myNetworkInfo.Online) && (mUserAccount.LoggedIn()))
            {
                //network detected, try authenticate at jabber
                try
                {
                    if (((DateTime)myServiceStateCheckTimer.Tag) < DateTime.Now.AddMinutes(-1))
                    {
#if (TRACE)
                        Console.WriteLine("xmppControl_OnDisconnected:Trying to ReConnect.");
#endif
                        xmppControl.Connect(mUserAccount.Username, mUserAccount.Password);
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
                if (!xmppControl.Connected)
                {
                    //try to reconnect every 60 seconds
                    myServiceStateCheckTimer.Enabled = true;
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
#if (TRACE)
            Console.WriteLine("IM-Connected:" + "(" + e.StatusCode + ")" + e.Description);
#endif
            myServiceStateCheckTimer.Enabled = false;
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

                InitializeIMNetworks();

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
#if (TRACE)
            Console.WriteLine("IM-IQ:" + "(" + e.Iq + ")");
#endif
        }

        private void InitializeIMNetworks()
        {
            // MSN, Yahoo, GaduGadu, IRC, AIM, ICQ
            //MSN
            mXMPPIQ.DiscoInfo(ConfigIM.MSN_IMServer);
            if (myClientConfiguration.MSN_Username != "" && myClientConfiguration.MSN_Password != "")
            {
                mXMPPIQ.RegisterUser(ConfigIM.MSN_IMServer, myClientConfiguration.MSN_Username, myClientConfiguration.MSN_Password);
                mXMPPIQ.SendPresence(ConfigIM.MSN_IMServer);
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
                mXMPPIQ.SendPresence(ConfigIM.YAHOO_IMServer);
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
                mXMPPIQ.SendPresence(ConfigIM.GG_IMServer);
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
                mXMPPIQ.SendPresence(ConfigIM.IRC_IMServer);
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
                mXMPPIQ.SendPresence(ConfigIM.ICQ_IMServer);
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
                mXMPPIQ.SendPresence(ConfigIM.AIM_IMServer);
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

        private string GetFromJID(string Pi)
        {
            int start = Pi.IndexOf("from=");
            if (start < 0) return xmppControl.User;
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
#if (TRACE)
                Console.WriteLine("IM-XEvent|" + jid + " is composing");
#endif
                myChatWindow.SetComposing(true, jid);
            }
            //file transfer
            if (Pi.IndexOf("<file-transfer-invite") >= 0)
            {
                myChatWindow.AddNotification(
                    jid,
                    "INVITE",
                    String.Format(Properties.Localization.txtInfoIMFileTransfer, new object[] { jabberUser.Nick, GetFileTransferFileName(Pi) + " " + GetFileTransferSize(Pi) }),
                    "",
                    Guid.NewGuid().ToString(),
                    true
                    );
                FileTransferProcessInvite(jabberUser, GetFileTransferFileName(Pi), GetFileTransferID(Pi), GetFileTransferSize(Pi));
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
                    "", 
                    Guid.NewGuid().ToString(), 
                    true
                    );
                string fileTransferID = GetFileTransferFileName(Pi);

#if (TRACE)
		                 Console.WriteLine("IM-XEvent| File Transfer Accept - " + fileTransferID);
#endif

                FileTransferProcessAccept(jabberUser, fileTransferID);
            }
            if (Pi.IndexOf("<file-transfer-cancel") >= 0)
            {
                myChatWindow.AddNotification(
                    jid, 
                    "INVITE", 
                    String.Format(Properties.Localization.txtInfoIMFileTransferDenied,jabberUser.Nick), 
                    "", 
                    Guid.NewGuid().ToString(), 
                    true
                    );
                string fileTransferID = GetFileTransferFileName(Pi);
#if (TRACE)
                Console.WriteLine("IM-XEvent| File Transfer Cancel - " + fileTransferID);
#endif
                FileTransferProcessCancel(jabberUser, fileTransferID);
            }

            //nudge
            if (Pi.IndexOf("<event-nudge") >= 0)
            {
                myChatWindow.AddNotification(
                    jid,
                    "NUDGE",
                   String.Format(Properties.Localization.txtInfoIMSendYouNudge, jabberUser.Nick),
                    "",
                    Guid.NewGuid().ToString(),
                    true
                    );
            }

            //video-invite
            if (Pi.IndexOf("<video-invite") >= 0)
            {
                myChatWindow.AddNotification(
                    jid,
                    "INVITE",
                    String.Format(Properties.Localization.txtInfoIMVideoCallInvite, jabberUser.Nick),
                    "",
                    Guid.NewGuid().ToString(),
                    true
                    );
                string VideoSessionID = GetVideoCallSessionID(Pi);
#if (TRACE)
                Console.WriteLine("IM-XEvent| Video Call Invite - " + VideoSessionID);
#endif
                VideoSessionProcessInvite(jabberUser, VideoSessionID);
            }

            //video-accept
            if (Pi.IndexOf("<video-accept") >= 0)
            {
                myChatWindow.AddNotification(
                    jid,
                    "INVITE",
                    String.Format(Properties.Localization.txtInfoIMVideoCallAccepted, jabberUser.Nick),
                    "",
                    Guid.NewGuid().ToString(),
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
                myChatWindow.AddNotification(
                        jid,
                        "INVITE",
                        String.Format(Properties.Localization.txtInfoIMVideoCallDenied, jabberUser.Nick),
                        "",
                        Guid.NewGuid().ToString(),
                        true
                        );
                string VideoSessionID = GetVideoCallSessionID(Pi);
#if (TRACE)
		       Console.WriteLine("IM-XEvent| Video Call Cancel - " + VideoSessionID);
#endif
                VideoSessionProcessCancel(jabberUser);
            }
        }
        #endregion

        #region VideoCall Processing

        public void VideoSessionConnect(string jabberID, string videoSessionID)
        {
            myVideoPlugin.StartConference(true, 0, 0, 0, ConfigVideoProxy.ProxyAddress, false, true, videoSessionID, DateTime.Now.ToString("yyyyMMdd|HHmmss|fff") + ";" + mUserAccount.Username, jabberID);
        }

        public void VideoSessionInvite(String jid)
        {
            JabberUser jabberUser = new JabberUser(jid);
            VideoSessionInvite(jabberUser);
        }
        
		public void VideoSessionInvite(JabberUser jabberUser)
        {
            string VideoSessionID = DateTime.Now.ToString("yyyyMMdd|hhmmss|fff") + "@" + mUserAccount.Username + ";" + jabberUser;
            //myVideoPlugin.Show(true);
            string jabberXevent = "<x xmlns='jabber:x:event'><video-invite/><id>" + VideoSessionID + "</id></x>";
            bool msgSent = SendJabberXevent(jabberUser.JID, jabberXevent);
        }

        public void VideoSessionProcessInvite(JabberUser jabberUser, string VideoSessionID)
        {
            if (MessageBox.Show(
                String.Format(Properties.Localization.txtInfoIMVideoCallInvitationDesc, jabberUser.Nick),
                Properties.Localization.txtInfoIMVideoCallInvitation, 
                MessageBoxButtons.YesNo, 
                MessageBoxIcon.Question, 
                MessageBoxDefaultButton.Button1
                ) == DialogResult.Yes)
            {
                string jabberXevent = "<x xmlns='jabber:x:event'><video-accept/><id>" + VideoSessionID + "</id></x>";
                bool msgSent = SendJabberXevent(jabberUser.JID, jabberXevent);
                VideoSessionConnect(jabberUser.JID, VideoSessionID);
            }
            else
            {
                string jabberXevent = "<x xmlns='jabber:x:event'><video-cancel/><id>" + VideoSessionID + "</id></x>";
                bool msgSent = SendJabberXevent(jabberUser.JID, jabberXevent);
            }
        }
		
        public void VideoSessionProcessAccept(JabberUser jabberUser, string VideoSessionID)
        {
            VideoSessionConnect(jabberUser.JID, VideoSessionID);
            StartNewCall(-1, jabberUser);
        }

        public void VideoSessionProcessCancel(JabberUser jabberID)
        {
            //do nothing
        }

        public void VideoSessionDisconnect(JabberUser jabberID)
        {
            myVideoPlugin.StopConference();

        }
        #endregion

        #region File Transfer Processing
        public void FileTransferInvite(JabberUser jabberUser, string fileName, string fileTransferID, string fileSize)
        {
            myChatWindow.AddNotification(
                    jabberUser.JID,
                    "INVITE",
                    String.Format(Properties.Localization.txtInfoIMFileTransferInitialized, new object[] { jabberUser.Nick, fileName, fileSize }),
                    "",
                    Guid.NewGuid().ToString(),
                    false
                    );
            string jabberXevent = "<x xmlns='jabber:x:event'><file-transfer-invite/><id>" + fileTransferID + "</id><file>" + fileName + "</file><size>" + fileSize + "</size></x>";
            bool msgSent = SendJabberXevent(jabberUser.JID, jabberXevent);
        }

        public void FileTransferProcessInvite(JabberUser jabberUser, string filename, string fileTransferID, string fileSize)
        {
            if (MessageBox.Show(
                String.Format(Properties.Localization.txtInfoIMFileTransferInvitationDesc, new object[]{jabberUser.Nick,filename,fileSize}), 
                Properties.Localization.txtInfoIMFileTransferInvitation,
                MessageBoxButtons.YesNo, 
                MessageBoxIcon.Question, 
                MessageBoxDefaultButton.Button1
                ) == DialogResult.Yes)
            {
                string jabberXevent = "<x xmlns='jabber:x:event'><file-transfer-accept/><id>" + fileTransferID + "</id></x>";
                bool msgSent = SendJabberXevent(jabberUser.JID, jabberXevent);
                myChatWindow.ProcessFileTransfer(jabberUser.JID, filename, fileTransferID);
                myChatWindow.AddNotification(
                    jabberUser.JID, 
                    "INVITE", 
                    String.Format(Properties.Localization.txtInfoIMFileTransferDownloading,filename), 
                    "", 
                    Guid.NewGuid().ToString(), 
                    true
                    );
            }
            else
            {
                string jabberXevent = "<x xmlns='jabber:x:event'><file-transfer-cancel/><id>" + fileTransferID + "</id></x>";
                bool msgSent = SendJabberXevent(jabberUser.JID, jabberXevent);
            }
        }
        public void FileTransferProcessDone(String jid, string filename, string fileTransferID, string fileSize)
        {
            string jabberXevent = "<x xmlns='jabber:x:event'><file-transfer-done/><id>" + fileTransferID + "</id></x>";
            bool msgSent = SendJabberXevent(jid, jabberXevent);
        }
        public void FileTransferProcessAccept(JabberUser jabberUser, string fileTransferID)
        {
            //TO DO SHOW PROGESS OF UPLOAD 
        }

        public void FileTransferProcessCancel(JabberUser jabberUser, string fileTransferID)
        {
            //TO DO CANCEL UPLOAD
        }
        #endregion
        #region xmpp change presence status
        private void myGoOfflineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (xmppControl.Connected)
            {
                xmppControl.ChangePresence(0, PRESENCES[0]);
                myPresenceStripSplitButton.Image = myGoOfflineToolStripMenuItem.Image;
            }
        }

        private void myGoOnlineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (xmppControl.Connected)
            {
                xmppControl.ChangePresence(1, PRESENCES[1]);
                myPresenceStripSplitButton.Image = myGoOnlineToolStripMenuItem.Image;
            }
        }

        private void myGoAwayToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (xmppControl.Connected)
            {
                xmppControl.ChangePresence(2, PRESENCES[2]);
                myPresenceStripSplitButton.Image = myGoAwayToolStripMenuItem.Image;
            }
        }

        private void myGoExtendedAwayToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (xmppControl.Connected)
            {
                xmppControl.ChangePresence(3, PRESENCES[3]);
                myPresenceStripSplitButton.Image = myGoExtendedAwayToolStripMenuItem.Image;
            }
        }

        private void myGoDoNotDisturbToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (xmppControl.Connected)
            {
                xmppControl.ChangePresence(4, PRESENCES[4]);
                myPresenceStripSplitButton.Image = myGoDoNotDisturbToolStripMenuItem.Image;
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
                        mContactBook.Save();
                        LoadContactsBook(mContactBook);

                        if (tmpContactWindow.myNTContact != null && tmpContactWindow.myNTContact.NTJabberID != mUserAccount.JabberUser.JID)
                        {
                            if (myBuddyList[tmpContactWindow.myNTContact.NTJabberID] == null)
                            {

                                JabberUser jabberUser = new JabberUser(tmpContactWindow.myNTContact.NTJabberID);

                                xmppControl.Add(jabberUser.JID, jabberUser.Username, tmpContactWindow.myContactJabberGroupListBox.Text);
                                xmppControl.SubscribeTo(jabberUser.JID);

                                mXMPPIQ.PromptUser(jabberUser.Domain, jabberUser.Username);
                                mXMPPIQ.Roster(jabberUser.Domain, jabberUser.Username, jabberUser.Nick, tmpContactWindow.myContactJabberGroupListBox.Text);
                                mXMPPIQ.Subscibe(jabberUser.Domain, jabberUser.Username);
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
            if (myRosterListTreeView.SelectedNode != null)
            {
                if (myRosterListTreeView.SelectedNode.Level == 1)
                {
                    StartNewIM(myRosterListTreeView.SelectedNode.Tag.ToString(), true);
                }
            }
        }

        private void callToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (myRosterListTreeView.SelectedNode != null)
            {
                if (myRosterListTreeView.SelectedNode.Level == 1)
                {
                    StartNewCall(-1, new JabberUser(myRosterListTreeView.SelectedNode.Tag.ToString()));
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
                    properties.Add("NTJabberID", myRosterListTreeView.SelectedNode.Tag.ToString());
                    myContactWindow = new ContactsWindow(this, mContactBook.getCandidatesForJabberID(myRosterListTreeView.SelectedNode.Tag.ToString()).Count > 0 ? (NTContact)mContactBook.getCandidatesForJabberID(myRosterListTreeView.SelectedNode.Tag.ToString())[0] : null, properties);
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
            catch (Exception ex)
            {
                //Console.WriteLine("myContactsListBox_SelectedIndexChanged : " + ex.Message);
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }


        private void myMainMenuAboutItem_Click(object sender, EventArgs e)
        {
            //Initialize ChatWindow
            AboutBox myAboutBox = new Remwave.Client.AboutBox();
            myAboutBox.Show();
        }








        private void myContactsShowAll()
        {
            if (mContactBook != null)
            {
                LoadContactsBook(mContactBook);
            }
        }

        private void myContactsAddContactButton_Click(object sender, EventArgs e)
        {
            myContactWindow = new ContactsWindow(this, null, null);
            myContactWindow.FormClosing += new FormClosingEventHandler(myContactWindow_FormClosing);
            myContactWindow.Show();
        }

        private void myContactsShow1ABButton_Click(object sender, EventArgs e)
        {
            if (myContactsShow1ABButton.Checked)
            {
                LoadContactsBook(mContactBook.getCandidatesForName(new string[] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "0", "A", "B" }));
            }
            else
            {
                myContactsShowAll();
            }
        }

        private void myContactsShowCDEButton_Click(object sender, EventArgs e)
        {

            if (myContactsShowCDEButton.Checked)
            {
                LoadContactsBook(mContactBook.getCandidatesForName(new string[] { "C", "D", "E" }));
            }
            else
            {
                myContactsShowAll();
            }
        }

        private void myContactsShowFGHButton_Click(object sender, EventArgs e)
        {
            if (myContactsShowFGHButton.Checked)
            {
                LoadContactsBook(mContactBook.getCandidatesForName(new string[] { "F", "G", "H" }));
            }
            else
            {
                myContactsShowAll();
            }
        }

        private void myContactsShowIJKButton_Click(object sender, EventArgs e)
        {
            if (myContactsShowIJKButton.Checked)
            {
                LoadContactsBook(mContactBook.getCandidatesForName(new string[] { "I", "J", "K" }));
            }
            else
            {
                myContactsShowAll();
            }


        }

        private void myContactsShowLMNButton_Click(object sender, EventArgs e)
        {
            if (myContactsShowLMNButton.Checked)
            {
                LoadContactsBook(mContactBook.getCandidatesForName(new string[] { "L", "M", "N" }));
            }
            else
            {
                myContactsShowAll();
            }

        }

        private void myContactsShowOPQButton_Click(object sender, EventArgs e)
        {
            if (myContactsShowOPQButton.Checked)
            {
                LoadContactsBook(mContactBook.getCandidatesForName(new string[] { "O", "P", "Q" }));
            }
            else
            {
                myContactsShowAll();
            }

        }

        private void myContactsShowRSTButton_Click(object sender, EventArgs e)
        {
            if (myContactsShowRSTButton.Checked)
            {
                LoadContactsBook(mContactBook.getCandidatesForName(new string[] { "R", "S", "T" }));
            }
            else
            {
                myContactsShowAll();
            }
        }

        private void myContactsShowUVWButton_Click(object sender, EventArgs e)
        {
            if (myContactsShowUVWButton.Checked)
            {
                LoadContactsBook(mContactBook.getCandidatesForName(new string[] { "U", "V", "W" }));
            }
            else
            {
                myContactsShowAll();
            }


        }

        private void myContactsShowXYZButton_Click(object sender, EventArgs e)
        {
            if (myContactsShowXYZButton.Checked)
            {
                LoadContactsBook(mContactBook.getCandidatesForName(new string[] { "X", "Y", "Z" }));
            }
            else
            {
                myContactsShowAll();
            }

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
            if (args.TabItem == myTabItemCallHistory)
            {
                LoadCallsHistory();
            }


            //TO DO :  VOICE MAIL CONTROL


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
                    myDialPadCallOrAnswerButton.BackgroundImage = Properties.Resources.PhoneAnswerOn;
                    myDialPadCallOrAnswerButton.Refresh();
                    Thread.Sleep(500);
                    myDialPadCallOrAnswerButton.BackgroundImage = Properties.Resources.PhoneAnswer;
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
                LoadContactsBook(mContactBook.getContactList());
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
                   
                }
                if (result == "0")
                {
                    myNotifyIcon.ShowBalloonTip(
                        10,
                        Properties.Localization.txtInfoPhone2Phone, 
                        String.Format(Properties.Localization.txtInfoPhone2PhoneTrying, new object[]{ myPhone2PhoneWindow.CallFrom , myPhone2PhoneWindow.CallTo}), 
                        
                        ToolTipIcon.Info
                        );
                }
                else if (result == "2")
                {
                    myNotifyIcon.ShowBalloonTip(
                        10, 
                        Properties.Localization.txtInfoPhone2Phone, 
                        String.Format(Properties.Localization.txtInfoPhone2PhonePaymentRequired, new object[]{ myPhone2PhoneWindow.CallFrom , myPhone2PhoneWindow.CallTo}), 
                        ToolTipIcon.Error);
                }
                else
                {
                    myNotifyIcon.ShowBalloonTip(
                        10, 
                        Properties.Localization.txtInfoPhone2Phone, 
                        String.Format(Properties.Localization.txtInfoPhone2PhoneUnableToStartCall, new object[]{ myPhone2PhoneWindow.CallFrom , myPhone2PhoneWindow.CallTo}), 
                        ToolTipIcon.Error);
                }
            }
        }

        private void callPhone2PhoneToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            RadListBoxItem selectedItem = (RadListBoxItem)myContactsListBox.SelectedItem;
            if (selectedItem != null)
            {
                NTContact meContact;
                if (mContactBook.getCandidatesForJabberID(mUserAccount.Username).Count > 0)
                {
                    meContact = (NTContact)mContactBook.getCandidatesForJabberID(mUserAccount.Username)[0];
                }
                else
                {
                    meContact = new NTContact();
                }
                NTContact selectedContact = (NTContact)selectedItem.Tag;
                myPhone2PhoneWindow = new Phone2PhoneWindow(selectedContact, meContact);
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
                    StartNewVideoCall(myRosterListTreeView.SelectedNode.Tag.ToString());
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
            myContactWindow = new ContactsWindow(this, null, null);
            myContactWindow.FormClosing += new FormClosingEventHandler(myContactWindow_FormClosing);
            myContactWindow.Show();
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (myRosterListTreeView.SelectedNode != null)
            {
                if (myRosterListTreeView.SelectedNode.Level == 1)
                {
                    try
                    {
                        xmppControl.UnsubscribeTo(myRosterListTreeView.SelectedNode.Tag.ToString());
                        xmppControl.Remove(myRosterListTreeView.SelectedNode.Tag.ToString(), "", "");
                    }
                    catch (Exception)
                    {
                        
                    }
                }
            }
        }

        private void myServiceStateCheckTimer_Tick(object sender, EventArgs e)
        {
            if ((!this.mFormIsClosing) && (!xmppControl.Connected) && (myNetworkInfo.Online) && (mUserAccount.LoggedIn()))
            {
                //network detected, try authenticate at jabber
                try
                {
                    myServiceStateCheckTimer.Tag = DateTime.Now;
#if (TRACE)
                    Console.WriteLine("IM:Trying to ReConnect");         
#endif
                    xmppControl.Connect(mUserAccount.Username, mUserAccount.Password);

                }
                catch (nsoftware.IPWorksSSL.IPWorksSSLException ipwime)
                {
#if (TARCE)
                    Console.WriteLine("IM:ReConnecting:(" + ipwime.Code + "):" + ipwime.Message);         
#endif
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
                    NTContact meContact;
                    if (mContactBook.getCandidatesForJabberID(mUserAccount.Username).Count > 0)
                    {
                        meContact = (NTContact)mContactBook.getCandidatesForJabberID(mUserAccount.Username)[0];
                    }
                    else
                    {
                        meContact = new NTContact();
                    }
                    ContactList foundContactsList = mContactBook.getCandidatesForJabberID(myRosterListTreeView.SelectedNode.Tag.ToString());
                    if (foundContactsList.Count > 0)
                    {
                        selectedContact = (NTContact)foundContactsList[0];
                    }
                    else
                    {
                        selectedContact = new NTContact();
                    }
                    myPhone2PhoneWindow = new Phone2PhoneWindow(selectedContact, meContact);
                    myPhone2PhoneWindow.FormClosing += new FormClosingEventHandler(myPhone2PhoneWindow_FormClosing);
                    myPhone2PhoneWindow.Show();
                }
            }
        }


        private void myMainMenuSignOutItem_Click(object sender, EventArgs e)
        {
            try
            {
                //logout user
                //switch panels
                //disconnect xmppp
                //shutdown engine

                mUserAccount.Logout();

                //open login panel and process login
                myMainWindowSplitContainer.Panel1Collapsed = false;
                myMainWindowSplitContainer.Panel2Collapsed = true;

                xmppControl.Disconnect();
                if (myChatWindow != null) myChatWindow.Dispose();
                if (myContactWindow != null) myContactWindow.Dispose();

                mySIPPhone.ShutdownEngine();
            }
            catch (Exception ex)
            {
#if (TRACE)
                Console.WriteLine("myMainMenuSignOutItem_Click" + ex.Message);     
#endif
            }
        }







        #region Mani Tab Control
        private void myMainTabControl_TabSelected(object sender, TabEventArgs args)
        {
            if (args.TabItem == myMainAccountTabPage)
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
            else if (args.TabItem == myMainContactsTabPage)
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
                myOneSecondTickTimer.Enabled = true;
                myDestinationTextBox.Focus();
            }

            if (args.TabItem == myMainOnlineTabPage)
            {
                myMainOnlineTabPage.Image = Properties.Resources.buttonOnlineOn;
            }
            else
            {
                myMainOnlineTabPage.Image = Properties.Resources.buttonOnline;
            }

            if (args.TabItem == myMainContactsTabPage)
            {
                myContactsShowAll();
                myMainContactsTabPage.Image = Properties.Resources.buttonContactsOn;
            }
            else
            {
                myMainContactsTabPage.Image = Properties.Resources.buttonContacts;
            }
            if (args.TabItem == myMainDialPadTabPage)
            {
                myOneSecondTickTimer.Enabled = true;
                myDestinationTextBox.Focus();
                myMainDialPadTabPage.Image = Properties.Resources.buttonPhoneOn;
            }
            else
            {
                myMainDialPadTabPage.Image = Properties.Resources.buttonPhone;
            }
        }

        private void myMainTabControl_TabHovered(object sender, TabEventArgs args)
        {

            if (args.TabItem == myMainOnlineTabPage && !myMainOnlineTabPage.IsSelected)
            {
                myMainOnlineTabPage.Image = args.TabItem.IsMouseOver ? Properties.Resources.buttonOnlineOver : Properties.Resources.buttonOnline;
            }

            if (args.TabItem == myMainContactsTabPage && !myMainContactsTabPage.IsSelected)
            {
                myMainContactsTabPage.Image = args.TabItem.IsMouseOver ? Properties.Resources.buttonContactsOver : Properties.Resources.buttonContacts;
            }

            if (args.TabItem == myMainDialPadTabPage && !myMainDialPadTabPage.IsSelected)
            {
                myMainDialPadTabPage.Image = args.TabItem.IsMouseOver ? Properties.Resources.buttonPhoneOver : Properties.Resources.buttonPhone;
            }
        }
#endregion















        private void ShowHideSpeedDialWindow()
        {
            if (mySpeedDialWindow == null)
            {
                mySpeedDialWindow = new SpeedDialWindow(this);
            }
            mySpeedDialWindow.ShowHide();
        /*else if (!mySpeedDialWindow.Visible && showApp)
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
            }*/       
        
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
                VideoSessionInvite(selectedContact.NTJabberID);
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
                if (e.Error != null) return;
                myPrepaidAmountToolStripLabel.Text = e.Result.ToString();
            }
            catch (Exception)
            {
                myPrepaidAmountToolStripLabel.Text = "0.00";
            }
            myPrepaidAmountRefreshToolStripLabel.Visible = true;
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

        private void myMainMenuAccountItem_Click(object sender, EventArgs e)
        {
            if (DialogResult.Yes == MessageBox.Show("Would you like to open My Account area in your browser?", "Open Website", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
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
                    //throw;
                }
            }
        }

        private void myMainMenuExitItem_Click(object sender, EventArgs e)
        {
            this.Close();
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

        

        private void ClientForm_Shown(object sender, EventArgs e)
        {
            if (myLoginAutoLoginCheckBox.Checked) myLoginButton.PerformClick();
        }




























































        private void homenetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (mContactBook.getCandidatesForJabberID(mUserAccount.JabberUser.JID).Count > 0)
            {
                myContactWindow = new ContactsWindow(this, (NTContact)mContactBook.getCandidatesForJabberID(mUserAccount.JabberUser.JID)[0], null);
                myContactWindow.FormClosing += new FormClosingEventHandler(myContactWindow_FormClosing);
                myContactWindow.Show();
            }
            else
            {
                StartNewContact(mUserAccount.Username);
            }
        }

        void imAccount_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = false;
            IMAccountWindow imAccount = (IMAccountWindow)sender;
            if (imAccount.Save)
            {
                switch (ConfigIM.GetXMPPNetwork(imAccount.Domain))
                {

                    case ConfigXMPPNetwork.MSN:
                        myClientConfiguration.SetMSNLoginOptions(imAccount.Username, imAccount.Password);
                        mXMPPIQ.RegisterUser(imAccount.Domain, imAccount.Username, imAccount.Password);
                        mXMPPIQ.SendPresence(imAccount.Domain);
                        msnToolStripMenuItem.Image = Properties.Resources.im_msn;
                        break;
                    case ConfigXMPPNetwork.Yahoo:
                        myClientConfiguration.SetYahooLoginOptions(imAccount.Username, imAccount.Password);
                        mXMPPIQ.RegisterUser(imAccount.Domain, imAccount.Username, imAccount.Password);
                        mXMPPIQ.SendPresence(imAccount.Domain);
                        yahooToolStripMenuItem.Image = Properties.Resources.im_yahoo;
                        break;
                    case ConfigXMPPNetwork.GaduGadu:
                        myClientConfiguration.SetGaduGaduLoginOptions(imAccount.Username, imAccount.Password);
                        mXMPPIQ.RegisterUser(imAccount.Domain, imAccount.Username, imAccount.Password);
                        mXMPPIQ.SendPresence(imAccount.Domain);
                        ggToolStripMenuItem.Image = Properties.Resources.im_gadugadu;
                        break;
                    case ConfigXMPPNetwork.IRC:
                        myClientConfiguration.SetIRCLoginOptions(imAccount.Username, imAccount.Password);
                        mXMPPIQ.RegisterUser(imAccount.Domain, imAccount.Username, imAccount.Password);
                        mXMPPIQ.SendPresence(imAccount.Domain);
                        //ircToolStripMenuItem.Image = Properties.Resources.im_irc; 
                        break;
                    case ConfigXMPPNetwork.AIM:
                        myClientConfiguration.SetAIMLoginOptions(imAccount.Username, imAccount.Password);
                        mXMPPIQ.RegisterUser(imAccount.Domain, imAccount.Username, imAccount.Password);
                        mXMPPIQ.SendPresence(imAccount.Domain);
                        aimToolStripMenuItem.Image = Properties.Resources.im_aim;
                        break;
                    case ConfigXMPPNetwork.ICQ:
                        myClientConfiguration.SetICQLoginOptions(imAccount.Username, imAccount.Password);
                        mXMPPIQ.RegisterUser(imAccount.Domain, imAccount.Username, imAccount.Password);
                        mXMPPIQ.SendPresence(imAccount.Domain);
                        icqToolStripMenuItem.Image = Properties.Resources.im_icq;
                        break;


                }

                myClientConfigurationSerializer.SaveClientConfiguration(myClientConfiguration);
            }
            if (imAccount.Delete)
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

      
    }
}
