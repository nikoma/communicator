using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections;
namespace Remwave.Nikotalkie
{
    public struct NResultAuthorize
    {
        public String WebServiceUrl;
        public bool Success;
    }
    public struct NResultSend
    {
        public String MsgID;
        public bool Success;
    }

    /// <summary>
    /// Summary description for Message
    /// </summary>
    public class NMessage : IComparable
    {
        public Boolean Deleted = false;
        public NMessageHeader Header;
        public NMessageBody Body;
        public String LocalFileName;
        public NMessage()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public NMessage(NMessageHeader header, NMessageBody body)
        {
            this.Header = header;
            this.Body = body;
        }

        #region IComparable Members

        public int CompareTo(object obj)
        {
            if (obj is NMessage)
            {
                NMessage message = (NMessage)obj;

                return message.Header.Date.CompareTo(this.Header.Date);
            }
            else
                throw new ArgumentException("Object is not a NMessage.");
        }

        #endregion
        public int CompareTo(NMessage message)
        {

            return message.Header.Date.CompareTo(this.Header.Date);
        }
    }

    #region NMessageComparer

    public class NMessageComparer : System.Collections.Generic.IComparer<Remwave.Nikotalkie.NMessage>
    {

        #region IComparer Members

        public int Compare(NMessage x, NMessage y)
        {


            return x.CompareTo(y);
        }

        #endregion
    }

    #endregion
}
