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

public partial class manager_search : System.Web.UI.Page
{
    public ArrayList banco_arr = null;
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
        if (!((permiso & 1) == 1) && !((permiso & 2) == 2))
        {
            Response.Redirect("manager.aspx");
        }
        if (!Page.IsPostBack)
        {
            banco_arr = DB.getBancos("");
        }
    }
    protected void bt_Enviar_Click(object sender, EventArgs e)
    {
        string nombre = tb_nombre.Text.Trim();
        string sql = "";
        banco_arr = null;
        if ((nombre != null) && (!nombre.Equals(""))) {
            sql = "and UPPER(tba_nombre) LIKE '%" + nombre + "%'";
        }
        banco_arr = DB.getBancos(sql);
    }
}
