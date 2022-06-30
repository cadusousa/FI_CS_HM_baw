using System;
using System.Data;
using System.Collections;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

/// <summary>
/// Summary description for AppBean
/// </summary>
public class AppBean
{
    private int app_id = 0;
    private string app_nombre = "";
    private string app_descripcion = "";
    private ArrayList arr_op;
	public AppBean()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    public int appID
    {
        get { return app_id; }
        set { app_id = value; }
    }
    public string appNombre
    {
        get { return app_nombre; }
        set { app_nombre = value; }
    }
    public string appDescripcion
    {
        get { return app_descripcion; }
        set { app_descripcion = value; }
    }
    public ArrayList arrOp
    {
        get { return arr_op; }
        set { arr_op = value; }
    }
}
