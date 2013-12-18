using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Remwave.Client.Controls
{
    public partial class ChatBox : UserControl
    {
        public ChatBox()
        {
            InitializeComponent();
            wbConversation.IsWebBrowserContextMenuEnabled = false;
        }

        public String JID;

        public delegate void FilesDropedHandler(ChatBox sender, String[] files);
        public event FilesDropedHandler FileDroped;
        internal void OnFileDroped(ChatBox sender, String[] files)
        {
            if (FileDroped != null)
            {
                FileDroped(sender, files);
            }
        }

        public delegate void LinkClickedHandler(ChatBox sender, String id, String url);
        public event LinkClickedHandler LinkClicked;
        internal void OnLinkClicked(ChatBox sender, String id, String url)
        {
            if (LinkClicked != null)
            {
                LinkClicked(sender, id, url);
            }
        }


        public WebBrowser ChatTabConversation
        {
            get { return wbConversation; }
        }

        public TextBox ChatTabMessage
        {
            get { return tbMessage; }
        }

        public void AttachEvents()
        {
            HtmlElementCollection links = wbConversation.Document.Links;

            for (int i = 0; i < links.Count; i++)
            {
                try
                {
                    links[i].DetachEventHandler("onclick", Link_Clicked);
                    links[i].AttachEventHandler("onclick", Link_Clicked);
                }
                catch (Exception)
                {
#if DEBUG
                    throw;
#endif
                }
            }

        }

        public void SetLink(String id, String innerText, bool disable)
        {
            HtmlElementCollection links = wbConversation.Document.Links;

            for (int i = 0; i < links.Count; i++)
            {
                try
                {
                    if (links[i].Id == id)
                    {
                        links[i].InnerText = innerText;
                        if (disable) links[i].Enabled = false;
                    }
                }
                catch (Exception)
                {
#if DEBUG
                    throw;
#endif
                }
            }
        }

        private void Link_Clicked(object sender, EventArgs e)
        {
            HtmlElement link = wbConversation.Document.ActiveElement;
            String url = link.GetAttribute("href");
            OnLinkClicked(this, link.Id, link.GetAttribute("href"));
        }

        internal void ResetHTML()
        {
            string defaultHtml = "<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\"     \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\"> <html xmlns=\"http://www.w3.org/1999/xhtml\"> <head> <title></title> </head> <body> <p></p> </body> </html>";
            if (wbConversation.Document == null) wbConversation.Navigate("about:blank");
            wbConversation.Document.OpenNew(true);
            wbConversation.Document.Write(defaultHtml);
        }

        private void tbMessage_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                try
                {
                    string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                    OnFileDroped(this, files);
                }
                catch (Exception)
                {
                    return;
                }

            }
        }

        private void tbMessage_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void wbContextMenu_Copy_Click(object sender, EventArgs e)
        {
            wbConversation.Document.ExecCommand("Copy", false, null);
        }

        private void wbContextMenu_SelectAll_Click(object sender, EventArgs e)
        {
            wbConversation.Document.ExecCommand("SelectAll", false, null);
        }
    }
}
