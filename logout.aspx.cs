using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

public partial class logout : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Session["usuario"] = null;
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        Response.Cookies.Clear();
        ClientScript.RegisterStartupScript(this.GetType(), "Nombre", "<script> refresh(); </script>");
    }
}
