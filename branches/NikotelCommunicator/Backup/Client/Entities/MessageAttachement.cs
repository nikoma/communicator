using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
namespace Remwave.Nikotalkie
{
    /// <summary>
    /// Summary description for MessageAttachement
    /// </summary>
    /// 
    public enum NMessageAttachementType
    {
        Audio, Image, Video, Text
    }

    public class NMessageAttachementHeader
    {
        public Int32 ID;
        public NMessageAttachementType AttachementType;

        public NMessageAttachementHeader()
        {


        }
    }

    public class NMessageAttachementBody
    {
        public Int32 ID;
        public Byte[] Data;
        public String Name;
        public Int32 Size;
        public NMessageAttachementType AttachementType;

        public NMessageAttachementBody()
        {


        }
    }
    public class NResponse
    {
        public bool Success = false;
        public String Message;
    }
}