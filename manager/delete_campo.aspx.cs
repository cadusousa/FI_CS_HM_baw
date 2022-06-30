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

public partial class manager_delete_campo : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        int fac_id = 0;
        int suc_id = 0;
        if (Session["usuario"] == null)
        {
            Session["msg"] = "Usted debe generar una nueva sesion";
            Response.Redirect("../Default.aspx");
        }
        if ((Request.QueryString["suc_id"] != null) && (!Request.QueryString["suc_id"].Equals("0")) && (!Request.QueryString["suc_id"].Equals(""))) {
            suc_id = int.Parse(Request.QueryString["suc_id"].ToString());
        }
        if ((Request.QueryString["fac_id"] != null) && (!Request.QueryString["fac_id"].Equals("0")) && (!Request.QueryString["fac_id"].Equals(""))) {
            fac_id = int.Parse(Request.QueryString["fac_id"].ToString());
        }
        if ((Request.QueryString["cam_id"] != null) && (!Request.QueryString["cam_id"].Equals("0")) && (!Request.QueryString["cam_id"].Equals("")))
        {
            int cam_id = int.Parse(Request.QueryString["cam_id"].ToString());
            int result = DB.deleteCampo(cam_id);
            if (result == 0)
            {
                Session["msg"] = "Error<br /><br /> Existió un error al tratar de eliminar los datos que deseaba, por favor intente de nuevo.";
                Session["url"] = "addfactura.aspx?id="+fac_id+"&suc_id="+suc_id;
                Response.Redirect("message.aspx");
            }
            else if (result == -100)
            {
                Session["msg"] = "Error<br /><br />Existió un error al tratar de eliminar los datos, por favor intente de nuevo.";
                Session["url"] = "addfactura.aspx?id=" + fac_id + "&suc_id=" + suc_id;
                Response.Redirect("message.aspx");
            }
            Response.Redirect("addfactura.aspx?id="+fac_id+"&suc_id="+suc_id);
        }
        else
        {
            Session["msg"] = "Error<br /><br /> Debe indicar que campo desea eliminar, por favor intente de nuevo.";
            Session["url"] = "addfactura.aspx?id=" + fac_id + "&suc_id=" + suc_id;
            Response.Redirect("message.aspx");
        }
    }
}
