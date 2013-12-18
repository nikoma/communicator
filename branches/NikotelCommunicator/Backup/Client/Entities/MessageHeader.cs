using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;
using System.Collections;
namespace Remwave.Nikotalkie
{
    /// <summary>
    /// Summary description for MessageHeader
    /// </summary>
    public class NMessageHeader 
    {

        public List<String> To;
        public String From;
        public DateTime Date;
        public Int32 Size;
        public String MsgID;
        public List<NMessageAttachementHeader> AttachementHeaders;
        public NMessageHeader()
        {
            Date = DateTime.Now;
            //
            // TODO: Add constructor logic here
            //
        }

        


    }

   
}