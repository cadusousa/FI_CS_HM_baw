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

public partial class manager_addrubro : System.Web.UI.Page
{
    public ArrayList rubropais_arr = null;
    public ArrayList rubrocuenta_arr = null;
    public int rubid = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        int id = 0;
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
        if (!Page.IsPostBack)
        {
            if (Request.QueryString["id"] != null)
            {
                id = int.Parse(Request.QueryString["id"].ToString());
                RE_GenericBean rubean = (RE_GenericBean)DB.getRubro(id);
                if (rubean == null)
                {
                    Session["msg"] = "Error, No se pudieron obtener los datos de este registro, por favor intente de nuevo.";
                    Session["url"] = "addrubro.aspx?id=" + id;
                    Response.Redirect("message.aspx");
                }
                lb_id.Text = rubean.intC1.ToString();
                rubid = id;
                tb_nombre.Text = rubean.strC1;

                rubropais_arr = (ArrayList)DB.getRubroPais(rubean.intC1);
                rubrocuenta_arr = (ArrayList)DB.getRubroCuenta(rubean.intC1);
            }
            else
            {
                lb_id.Text = "0";
            }
        } else {
            lb_id.Text = "0";
        }

    }
    protected void bt_Enviar_Click(object sender, EventArgs e)
    {
        RE_GenericBean rubean = new RE_GenericBean();
        rubean.intC1 = int.Parse(lb_id.Text.Trim());
        rubean.strC1 = tb_nombre.Text.Trim().ToUpper();
        int result = DB.InsertUpdateRubro(rubean);
        if (result == 0)
        {
            Session["msg"] = "Error, No se pudieron grabar los datos que ingreso, por favor intente de nuevo.";
            Session["url"] = "addrubro.aspx?id=" + rubean.intC1;
            Response.Redirect("message.aspx");
        }
        else if (result == -100)
        {
            Session["msg"] = "Error, existió un error al tratar de grabar los datos, por favor intente de nuevo.";
            Session["url"] = "addrubro.aspx?id=" + rubean.intC1;
            Response.Redirect("message.aspx");
        }
        Response.Redirect("searchrubro.aspx");

    }
}
