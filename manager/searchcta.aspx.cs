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

public partial class manager_searchcta : System.Web.UI.Page
{
    public ArrayList cta_arr = null;
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
        if (!((permiso & 8) == 8) && !((permiso & 16) == 16))
        {
            Response.Redirect("manager.aspx");
        }
        if (!Page.IsPostBack)
        {
            cta_arr = (ArrayList)DB.getCuenta("");
        }

    }
    protected void bt_Enviar_Click(object sender, EventArgs e)
    {
        RE_GenericBean ctabean = new RE_GenericBean();
        string sql = "";
        if (!tb_cta_madre.Text.Trim().Equals("")) { sql += " and cue_madre='"+tb_cta_madre.Text.Trim().ToUpper()+"'"; }
        if (!tb_nombre.Text.Trim().Equals("")) { sql += " and cue_nombre='"+tb_nombre.Text.Trim().ToUpper()+"'"; }
        if (!lb_clasificacion.SelectedValue.Equals("Todos")) { sql += " and cue_clasificacion="+lb_clasificacion.SelectedValue; }
        if (!lb_tipo.SelectedValue.Equals("0")) { sql += " and cue_tipo="+lb_tipo.SelectedValue; }
        cta_arr = (ArrayList)DB.getCuenta(sql);
    }
}
