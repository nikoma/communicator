using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;

[WebService(Namespace = "http://rcs.remwave.com/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
public class Service : System.Web.Services.WebService
{
    public Service () {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    [WebMethod]
    public string[] Version() {
        //ProductVersion, News Version
        string[] version = { "1.1.0.0", "1.0.0.0" };
        return version;
    }

    [WebMethod]
    public string Software()
    {
        return "http://www.remwave.com/Downloads/tabid/212/Default.aspx";
    }

    [WebMethod]
    public string News()
    {
        return "http://www.remwave.com/Blogs/tabid/215/Default.aspx";
    }
}
