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

/// <summary>
/// Summary description for ServiceProvider
/// </summary>
public class ServiceProvider
{
    private String _Name = "";

    public String Name
    {
        get { return _Name; }
        set { _Name = value; }
    }

    private String _Description = "";

    public String Description
    {
        get { return _Description; }
        set { _Description = value; }
    }


    private String _IMServerAddress = "";

    public String IMServerAddress
    {
        get { return _IMServerAddress; }
        set { _IMServerAddress = value; }
    }


    private int _IMServerPort = 5222;

    public int IMServerPort
    {
        get { return _IMServerPort; }
        set { _IMServerPort = value; }
    }


    private String _SIPProxyRealm = "";

    public String SIPProxyRealm
    {
        get { return _SIPProxyRealm; }
        set { _SIPProxyRealm = value; }
    }


    private String _SIPProxyAddress = "";

    public String SIPProxyAddress
    {
        get { return _SIPProxyAddress; }
        set { _SIPProxyAddress = value; }
    }


    private int _SIPProxyPort = 5060;

    public int SIPProxyPort
    {
        get { return _SIPProxyPort; }
        set { _SIPProxyPort = value; }
    }


    private String _VideoProxyAddress = "";

    public String VideoProxyAddress
    {
        get { return _VideoProxyAddress; }
        set { _VideoProxyAddress = value; }
    }


    private int _VideoProxyPort = 800;

    public int VideoProxyPort
    {
        get { return _VideoProxyPort; }
        set { _VideoProxyPort = value; }
    }

    private string _SignupLink = "";

    public string SignupLink
    {
        get { return _SignupLink; }
        set { _SignupLink = value; }
    }

    public ServiceProvider()
    {
    }

    public ServiceProvider(String name, String description, String imServerAddress, int imServerPort, String sipProxyRealm, String sipProxyAddress, int sipProxyPort, string videoProxyAddress, int videoProxyPort, String signupLink)
    {
        this._Name = name;
        this._Description = description;

        this._IMServerAddress = imServerAddress;
        this._IMServerPort = imServerPort;

        this._SIPProxyRealm = sipProxyRealm;
        this._SIPProxyAddress = sipProxyAddress;
        this._SIPProxyPort = sipProxyPort;

        this._VideoProxyAddress = videoProxyAddress;
        this._VideoProxyPort = videoProxyPort;

        this._SignupLink = signupLink;
    }
}