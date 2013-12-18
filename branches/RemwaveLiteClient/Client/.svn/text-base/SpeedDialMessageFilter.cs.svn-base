using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Remwave.Client
{
    /*
            * Message filter, grabs all windows messaged within whole application
            * 
           SpeedDialMessageFilter messageFilter = new SpeedDialMessageFilter();
           messageFilter.KeyUp += new System.Windows.Forms.KeyEventHandler(this.tbxSearchText_KeyUp);
           messageFilter.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbxSearchText_KeyDown);
           messageFilter.Handle = tbxSearchText.Handle;

           Application.AddMessageFilter(messageFilter);
             
            */

    public class SpeedDialMessageFilter:IMessageFilter
    {
        public const int WM_KEYDOWN = 0x0100;
        public const int WM_KEYUP = 0x0101;
            public bool PreFilterMessage(ref Message aMessage)
            {
                if (aMessage.Msg == WM_KEYDOWN)
                {
                    /*
                    Message msg = aMessage;
                    msg.HWnd = this.Handle;

                    //DefWndProc(ref aMessage);
                     */
                    
                    if (KeyDown != null)
                    {
                        try
                        {
                            KeyEventArgs args = new KeyEventArgs((Keys)aMessage.WParam.ToInt32());
                            this.KeyDown(this, args);
                        }
                        catch (Exception)
                        {
                            
                            //throw;
                        }

                    }
                    
                }
                if (aMessage.Msg == WM_KEYUP)
                {
                    /*
                    Message msg = aMessage;
                    msg.HWnd = this.Handle;

                    DefWndProc(ref aMessage);
                    */

                    if (KeyUp != null)
                    {
                        try
                        {
                            KeyEventArgs args = new KeyEventArgs((Keys)aMessage.WParam.ToInt32());
                            this.KeyUp(this, args);
                        }
                        catch (Exception)
                        {
                            
                            //throw;
                        }

                    }
                }
                return false;
            }
         
        public KeyEventHandler KeyDown;
        public KeyEventHandler KeyUp;
        public IntPtr Handle = IntPtr.Zero;
        
    }
}
