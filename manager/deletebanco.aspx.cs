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

public partial class manager_deletesuc : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["usuario"] == null)
        {
            Session["msg"] = "Usted debe generar una nueva sesion";
            Response.Redirect("../Default.aspx");
        }
        if ((Request.QueryString["id"] != null) && (!Request.QueryString["id"].Equals("0")) && (!Request.QueryString["id"].Equals("")))
        {
            int id = int.Parse(Request.QueryString["id"]);
            int result = DB.deleteBanco(id);
            if ((result == 0) || (result == -100))
            {
                WebMsgBox.Show("Existio un problema al tratar de eliminar este registro, si el problema continua pongase en contacto con el administrador");
                return;
            }            
            Response.Redirect("searchbanco.aspx");
        }
        else
        {
            WebMsgBox.Show("Debe indicar que usuario desea eliminar, por favor intente de nuevo.");
            return;
        }

    }
}
