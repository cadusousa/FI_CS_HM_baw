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

public partial class manager_addcta : System.Web.UI.Page
{
    UsuarioBean user1 = null;
    
    protected void Page_Load(object sender, EventArgs e)
    {
        int id = 0;
        if (Session["usuario"] == null)
        {
            Session["msg"] = "Usted debe generar una nueva sesion";
            Response.Redirect("../Default.aspx");
        }
        
        user1 = (UsuarioBean)Session["usuario"];
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
            if (Request.QueryString["id"] != null)
            {
                id = int.Parse(Request.QueryString["id"].ToString());
                RE_GenericBean ctabean = (RE_GenericBean)DB.getCta(id);
                if (ctabean == null)
                {
                    Session["msg"] = "Error, No se pudieron obtener los datos de este registro, por favor intente de nuevo.";
                    Session["url"] = "searchcta.aspx";
                    Response.Redirect("message.aspx");
                }
                lb_ref.Text = ctabean.intC1.ToString();
                tb_cta.Text = ctabean.strC1.ToString();
                tb_nombre.Text = ctabean.strC2;
                tb_cta_madre.Text = ctabean.strC3;
                lb_nivel.SelectedValue=ctabean.intC2.ToString();
                lb_clasificacion.SelectedValue = ctabean.intC3.ToString();
                lb_tipo.SelectedValue = ctabean.intC4.ToString();
                tb_banco_nombre.Text = ctabean.strC4;
            } else {
                lb_ref.Text = "0";
            }
        }
    }
    protected void bt_enviar_Click(object sender, EventArgs e)
    {
        RE_GenericBean ctabean = new RE_GenericBean();
        ctabean.strC1 = user1.ID;
        ctabean.strC2 = tb_cta.Text.Trim().ToUpper();
        ctabean.strC3 = tb_nombre.Text.Trim().ToUpper();
        ctabean.strC4 = tb_cta_madre.Text.Trim().ToUpper();
        ctabean.strC5 = tb_banco_nombre.Text.Trim().ToUpper();
        ctabean.intC1 = int.Parse(lb_ref.Text.Trim().ToUpper());
        ctabean.intC2 = int.Parse(lb_clasificacion.SelectedValue);
        ctabean.intC3 = int.Parse(lb_nivel.SelectedValue);
        ctabean.intC4 = int.Parse(lb_tipo.SelectedValue);
        int result = DB.InsertUpdateCuenta(ctabean);
        if (result == 0)
        {
            Session["msg"] = "Error, No se pudieron grabar los datos que ingreso, por favor intente de nuevo.";
            Session["url"] = "addcta.aspx?id" + ctabean.intC1;
            Response.Redirect("message.aspx");
        }
        else if (result == -100)
        {
            Session["msg"] = "Error, existió un error al tratar de grabar los datos, por favor intente de nuevo.";
            Session["url"] = "addcta.aspx?id" + ctabean.intC1;
            Response.Redirect("message.aspx");
        }
        Response.Redirect("searchcta.aspx");
    }
}
