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
    public ArrayList sucursal_arr = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["usuario"] == null)
        {
            Session["msg"] = "Usted debe generar una nueva sesion";
            Response.Redirect("../Default.aspx");
        }

        UsuarioBean user = (UsuarioBean)Session["usuario"];
        ArrayList Arr_Perfiles = (ArrayList)DB.getPerfilesxUsu(user.ID, user.PaisID);
        int Flag=0;
        foreach (PerfilesBean Perfil in Arr_Perfiles)
        {
            if ((Perfil.ID == 7) || (Perfil.ID == 19))
            {
                Flag++; 
            }
        }
        if (Flag == 0)
        {
            Response.Redirect("../operations/index.aspx");
        }   

        if (!Page.IsPostBack)
        {
            string sql = "";
            sql += "and suc_pai_id=" + user.PaisID;
            sucursal_arr = DB.getSucursales(sql);
        }
    }
}
