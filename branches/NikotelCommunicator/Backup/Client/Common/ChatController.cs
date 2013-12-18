using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Drawing;
using Remwave.Client;
using Remwave.Services;
using Remwave.Client.Controls;

namespace Remwave.ChatController
{



    public class IMMessage
    {
        public String ID;
        public DateTime Time;
        public String HTML;
        public String Text;
        public MessageStyle Style;

        public static String NormalizeGUID(String id)
        {
            id = id.Replace("-", "");
            if (!id.StartsWith("ID"))
            {
                id = "ID" + id;
            }
            return id;
        }

        public IMMessage(String senderName, String messageText, String messageGUID, DateTime messageDateTime, MessageStyle style, MessageTemplateType template, Emoticons emoticons)
        {
            if (messageDateTime == null || messageDateTime == DateTime.MinValue) messageDateTime = DateTime.Now;
            this.Time = messageDateTime;
            if (messageGUID == null) messageGUID = Guid.NewGuid().ToString();
            this.ID = NormalizeGUID(messageGUID);
            MessageTemplate tmplMessageTemplate = new MessageTemplate(template);
            tmplMessageTemplate.Message = tmplMessageTemplate.Message
                     .Replace("<HEADER_ROW_STYLE>", tmplMessageTemplate.BuildStyle(style.Font, style.ForeColor, Color.White))
                     .Replace("<DATETIME_STYLE>", tmplMessageTemplate.BuildStyle(style.Font, Color.Gray, Color.White))
                     .Replace("<HEADER_STYLE>", tmplMessageTemplate.BuildStyle(style.Font, style.HeaderColor, Color.White))
                     .Replace("<ROW_STYLE>", tmplMessageTemplate.BuildStyle(style.Font, style.ForeColor, style.BackColor))
                     .Replace("<MESSAGE_STYLE>", tmplMessageTemplate.BuildStyle(style.Font, style.ForeColor, style.BackColor))
                     .Replace("<HEADER_TEXT>", senderName != "" ? senderName : "")
                     .Replace("<DATETIME_TEXT>", messageDateTime.ToShortDateString() + " " + messageDateTime.ToShortTimeString())
                     .Replace("<GUID>", this.ID)
                     .Replace("<MESSAGE_TEXT>", messageText.Replace("\r\n", "<BR />").Replace("\n", "<BR />"));
            tmplMessageTemplate.ProcessEmoticons(emoticons);

            this.HTML = tmplMessageTemplate.Message;
            this.Text = messageText;
            this.Style = style;

        }
    }
    public enum MessageTemplateType
    {
        In,
        Out,
        Notification,
        Incoming,
        Outgoing
    }
    public class MessageTemplate
    {   //<ROW_STYLE>
        //<HEADER_STYLE>
        //<MESSAGE_STYLE>
        //<HEADER_ROW_STYLE>
        static string _DefaultMessageIn = @"<DIV ID='ID-<GUID>' WIDTH='100%' STYLE='CLEAR: BOTH;'><TABLE WIDTH='100%' CELLPADDING='0' CELLSPACING='0' BORDER='0'><TR><TD ALIGN='LEFT' VALIGN='TOP' STYLE='<HEADER_ROW_STYLE>'><SPAN STYLE='<HEADER_STYLE>'>:IN:  <B><HEADER_TEXT><B></TD><TD ALIGN='RIGHT' VALIGN='TOP' STYLE='<HEADER_ROW_STYLE>'><SPAN STYLE='<DATETIME_STYLE>'><SMALL><DATETIME_TEXT></TD></TR><TR><TD ALIGN='LEFT' VALIGN='TOP' STYLE='border-bottom-width: 1px;	border-bottom-style: dashed; border-bottom-color: #A0B0B9; <ROW_STYLE>' COLSPAN=2><SPAN STYLE='<MESSAGE_STYLE>'><MESSAGE_TEXT></TD></TR></TABLE></DIV>";
        static string _DefaultMessageOut = @"<DIV ID='ID-<GUID>' WIDTH='100%' STYLE='CLEAR: BOTH;'><TABLE WIDTH='100%' CELLPADDING='0' CELLSPACING='0' BORDER='0'><TR><TD ALIGN='LEFT' VALIGN='TOP' STYLE='<HEADER_ROW_STYLE>'><SPAN STYLE='<HEADER_STYLE>'>:OUT: <B><HEADER_TEXT><B></TD><TD ALIGN='RIGHT' VALIGN='TOP' STYLE='<HEADER_ROW_STYLE>'><SPAN STYLE='<DATETIME_STYLE>'><SMALL><DATETIME_TEXT></TD></TR><TR><TD ALIGN='LEFT' VALIGN='TOP' STYLE='border-bottom-width: 1px;	border-bottom-style: dashed; border-bottom-color: #A0B0B9; <ROW_STYLE>' COLSPAN=2><SPAN STYLE='<MESSAGE_STYLE>'><MESSAGE_TEXT></TD></TR></TABLE></DIV>";
        static string _DefaultMessageNotification = @"<DIV ID='ID-<GUID>' WIDTH='100%'><TABLE WIDTH='100%' CELLPADDING='0' CELLSPACING='1' BORDER='0'><TR><TD ALIGN='LEFT' VALIGN='TOP' STYLE='<ROW_STYLE>'><SPAN STYLE='<HEADER_STYLE>'><B><HEADER_TEXT></B> <SPAN STYLE='<MESSAGE_STYLE>'><MESSAGE_TEXT></TD></TR></TABLE></DIV>";
        static string _EmoticonTemplate = @"<IMG SRC='<FILENAME>' WIDTH='<WIDTH>' HEIGHT='<HEIGHT>' ALIGN='ABSMIDDLE' BORDER='0'>";
        static string _DefaultMessageIncoming = @"<TABLE WIDTH='100%' CELLPADDING='0' CELLSPACING='0' BORDER='0'><TR><TD ALIGN='LEFT' STYLE='<ROW_STYLE>'><SPAN STYLE='<MESSAGE_STYLE>'><MESSAGE_TEXT></SPAN></TD></TR></TABLE>";
        static string _DefaultMessageOutgoing = @"<TABLE WIDTH='100%' CELLPADDING='0' CELLSPACING='0' BORDER='0'><TR><TD ALIGN='LEFT' STYLE='<ROW_STYLE>'><SPAN STYLE='<MESSAGE_STYLE>'><MESSAGE_TEXT></SPAN></TD></TR></TABLE>";


        public MessageTemplate(MessageTemplateType type)
        {
            switch (type)
            {
                case MessageTemplateType.In:
                    _Message = _DefaultMessageIn;
                    break;
                case MessageTemplateType.Out:
                    _Message = _DefaultMessageOut;
                    break;
                case MessageTemplateType.Outgoing:
                    _Message = _DefaultMessageOutgoing;
                    break;
                case MessageTemplateType.Incoming:
                    _Message = _DefaultMessageIncoming;
                    break;
                default:
                    _Message = _DefaultMessageNotification;
                    break;
            }
        }

        public string EmoticonTemplate
        {
            get { return _EmoticonTemplate; }
        }

        private string _Message;
        public string Message
        {
            get { return _Message; }
            set { _Message = value; }
        }
        public string BuildStyle(Font font, Color fontColor, Color bgColor)
        {
            string style = "<FONT>color: <COLOR>;background-color:<BGCOLOR>";

            string fontStyle = "font-family: " + font.Name + ",Verdana ;"
                + "font-size: " + font.SizeInPoints.ToString() + " pt;"
                + "font-weight: " + (font.Bold ? "bold" : "normal") + ";"
                + "font-style: " + (font.Italic ? "italic" : "normal") + ";"
                + "text-decoration: " + (font.Underline ? "underline" : font.Strikeout ? "line-through" : "none") + ";";

            //apply font
            style = style.Replace("<FONT>", fontStyle)
            .Replace("<COLOR>", "#" + fontColor.R.ToString("X2") + fontColor.G.ToString("X2") + fontColor.B.ToString("X2"))
            .Replace("<BGCOLOR>", "#" + bgColor.R.ToString("X2") + bgColor.G.ToString("X2") + bgColor.B.ToString("X2"));

            return style;
        }

        public string ProcessEmoticons(Emoticons emoticons)
        {

            if (_Message != null && emoticons != null)
            {
                try
                {
                    foreach (Emoticon myEmoticon in emoticons.List)
                    {
                        _Message = _Message
                            //upper case
                             .Replace(myEmoticon.Tag.ToUpper(), this.EmoticonTemplate
                             .Replace("<FILENAME>", myEmoticon.Filename)
                             .Replace("<WIDTH>", myEmoticon.Width.ToString())
                             .Replace("<HEIGHT>", myEmoticon.Height.ToString()))
                            //lower case
                             .Replace(myEmoticon.Tag.ToLower(), this.EmoticonTemplate
                             .Replace("<FILENAME>", myEmoticon.Filename)
                             .Replace("<WIDTH>", myEmoticon.Width.ToString())
                             .Replace("<HEIGHT>", myEmoticon.Height.ToString()))
                             ;
                    }
                }
                catch (Exception)
                {

                    //throw;
                }
            }
            return _Message;
        }
    }
    public class MessageStyle
    {
        public Color BackColor;
        public Font Font;
        public Color ForeColor;
        public Color HeaderColor;

        public MessageStyle()
        {
            this.BackColor = Color.White;
            this.Font = new System.Drawing.Font("Trebuchet MS", 8.5F, System.Drawing.FontStyle.Regular);
            this.ForeColor = Color.Black;
            this.HeaderColor = Color.Gray;
        }

        public MessageStyle(Color backColor, Font font, Color foreColor, Color headerColor)
        {
            this.BackColor = backColor;
            this.Font = font;
            this.ForeColor = foreColor;
            this.HeaderColor = headerColor;
        }
    }
    public class ChatSession
    {
        public JabberUser JabberUser;
        public int LastStatus = 0;
        public Telerik.WinControls.UI.TabItem ChatTab;
        public ChatBox ChatBox;
        public int SendComposingTimeout = 0;
        public int ComposingTimeout = 0;
        public int NudgeTimeout = 0;
        public bool OfflineMessageNotified = false;
        public MessageStyle IncomingStyle = new MessageStyle();
        public MessageStyle OutgoingStyle = new MessageStyle();


        public ChatSession()
        {
            this.IncomingStyle = new MessageStyle(Color.White, new System.Drawing.Font("Trebuchet MS", 8.5F, System.Drawing.FontStyle.Regular), Color.Black, Color.Red);
            this.OutgoingStyle = new MessageStyle(Color.White, new System.Drawing.Font("Trebuchet MS", 8.5F, System.Drawing.FontStyle.Regular), Color.Black, Color.Blue);
        }
    }


}
