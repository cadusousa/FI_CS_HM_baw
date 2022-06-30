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

public partial class manager_searchRubro : System.Web.UI.Page
{
    public ArrayList rubro_arr = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["usuario"] == null)
        {
            Session["msg"] = "Usted debe generar una nueva sesion";
            Response.Redirect("../Default.aspx");
        }
        UsuarioBean user1 = (UsuarioBean)Session["usuario"];
        if (!user1.Aplicaciones.Contains("7"))
        {
            Response.Redirect("manager.aspx");
        }
        int permiso = int.Parse(user1.Aplicaciones["7"].ToString());
        if (!((permiso & 32) == 32) && !((permiso & 64) == 64))
        {
            Response.Redirect("manager.aspx");
        }
        if (!Page.IsPostBack) {
            rubro_arr = DB.getRubros("");
        }
    }
    protected void bt_Enviar_Click(object sender, EventArgs e)
    {
        rubro_arr = DB.getRubros(tb_nombre.Text.Trim().ToUpper());
    }
}
