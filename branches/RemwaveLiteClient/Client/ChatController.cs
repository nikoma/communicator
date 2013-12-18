using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace Remwave.ChatController
{
   class MessageTemplate
   {   //<ROW_STYLE>
       //<HEADER_STYLE>
       //<MESSAGE_STYLE>
       const string _DefaultMessage = "<TABLE WIDTH='100%' CELLPADDING='0' CELLSPACING='0' BORDER='0'><TR><TD ALING='LEFT' VALIGN='TOP' STYLE='<ROW_STYLE>'><SPAN STYLE='<HEADER_STYLE>'><B><HEADER_TEXT><B> : <SPAN STYLE='<MESSAGE_STYLE>'><MESSAGE_TEXT></TD></TR></TABLE>";
       const string _Emoticon = "<IMG SRC='<FILENAME>' WIDTH='<WIDTH>' HEIGHT='<HEIGHT>' ALIGN='ABSMIDDLE' BORDER='0'>";

       public string Emoticon
       {
           get { return _Emoticon; }
       } 

       public string DefaultMessage
       {
           get { return _DefaultMessage; }
       } 

   }

    class ChatSession
    {
        private string _jabberID;

        public string JabberID
        {
            get { return _jabberID; }
            set { _jabberID = value; }
        }
        private int _lastStatus = 0;

        public int LastStatus
        {
            get { return _lastStatus; }
            set { _lastStatus = value; }
        }

        private Telerik.WinControls.UI.TabItem _chatTab;

        public Telerik.WinControls.UI.TabItem ChatTab
        {
            get { return _chatTab; }
            set { _chatTab = value; }
        }
        private System.Windows.Forms.WebBrowser _chatTabConversation;

        public System.Windows.Forms.WebBrowser ChatTabConversation
        {
            get { return _chatTabConversation; }
            set { _chatTabConversation = value; }
        }
        private System.Windows.Forms.TextBox _chatTabMessage;

        public System.Windows.Forms.TextBox ChatTabMessage
        {
            get { return _chatTabMessage; }
            set { _chatTabMessage = value; }
        }
    }

    class ChatSessions
    {
        Hashtable _List = new Hashtable();

        public Hashtable List
        {
            get { return _List; }
            set { _List = value; }
        }


    }
}
