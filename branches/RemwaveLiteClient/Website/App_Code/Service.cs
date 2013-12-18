using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;

[WebService(Namespace = "http://lite.remwave.com/")]
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
        string[] version = { "1.0.0.2", "1.0.0.0" };
        return version;
    }
    [WebMethod]
    public string Software()
    {
        return "http://www.remwave.com/Softphone/SoftphoneUpdate/tabid/221/Default.aspx";
    }
    [WebMethod]
    public string News()
    {
        return "http://www.remwave.com/Blogs/tabid/215/Default.aspx";
    }
    
    [WebMethod]
    public ServiceProvider[] ServiceProviders(string version)
    {

        ServiceProvider[] listServiceProviders = new ServiceProvider[1];

        ServiceProvider nikotelServiceProvider = new ServiceProvider();


        nikotelServiceProvider.Name = "nikotel - the voip network";
        nikotelServiceProvider.Description = "Visit www.nikotel.com";

        nikotelServiceProvider.IMServerAddress = "im.nikotel.com";
        nikotelServiceProvider.IMServerPort = 5222;

        nikotelServiceProvider.SIPProxyRealm = "nikotel.com";
        nikotelServiceProvider.SIPProxyAddress = "voip.nikotel.com";
        nikotelServiceProvider.SIPProxyPort = 5060;
        
        nikotelServiceProvider.VideoProxyAddress = "video.nikotel.com";
        nikotelServiceProvider.VideoProxyPort = 800;

        nikotelServiceProvider.SignupLink = "https://www.nikotel.com/nikotel-signup/nikotalk/register";

        listServiceProviders[0] = nikotelServiceProvider;

        return listServiceProviders;

    }
}
