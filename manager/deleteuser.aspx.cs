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

public partial class manager_deleteuser : System.Web.UI.Page
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
            string id = Request.QueryString["id"];
            int result = DB.deleteUser(id);
            if (result == 0) {
                Session["msg"] = "Error<br /><br /> Existió un error al tratar de eliminar los datos que deseaba, por favor intente de nuevo.";
                Session["url"] = "searchuser.aspx";
                Response.Redirect("message.aspx");
            } else if (result == -100) {
                Session["msg"] = "Error<br /><br />Existió un error al tratar de eliminar los datos, por favor intente de nuevo.";
                Session["url"] = "searchuser.aspx";
                Response.Redirect("message.aspx");
            }
            Response.Redirect("searchuser.aspx");  
        } else {
            Session["msg"] = "Error<br /><br /> Debe indicar que usuario desea eliminar, por favor intente de nuevo.";
            Session["url"] = "searchuser.aspx";
            Response.Redirect("message.aspx");
        }
    }
}
