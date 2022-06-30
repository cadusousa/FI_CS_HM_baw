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

public partial class ventas_index : System.Web.UI.Page
{
    UsuarioBean user = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["usuario"] == null)
        {
            Response.Redirect("../default.aspx");
        }

        user = (UsuarioBean)Session["usuario"];

        if (!user.Aplicaciones.Contains("8"))
        {
            Session["msg"] = "Usted no tiene permisos para ver ninguna opcion";
            Session["url"] = "../manager/countrychoice.aspx";
            Response.Redirect("../invoice/index.aspx");
        }

        if (Session["configurado"] == null)
        {
            WebMsgBox.Show("Debe configurar los parametros con los que va trabajar.");
            Response.Redirect("../manager/countrychoice.aspx");
        }
    }
}
