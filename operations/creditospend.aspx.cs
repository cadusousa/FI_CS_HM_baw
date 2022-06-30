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

public partial class operations_creditospend : System.Web.UI.Page
{
    UsuarioBean user=null;
    protected void Page_Load(object sender, EventArgs e)
    {
        UsuarioBean user = null;
        if (Session["usuario"] == null)
        {
            Response.Redirect("../default.aspx");
        }
        user = (UsuarioBean)Session["usuario"];
        //if ((user.PaisID == 1) || (user.PaisID == 15) || (user.PaisID == 18) || (user.PaisID == 2) || (user.PaisID == 26))
        //{
            int permiso_autorizar = 0;
            ArrayList Arr_Perfiles = (ArrayList)DB.getPerfilesxUsu(user.ID, user.PaisID);
            foreach (PerfilesBean Perfil in Arr_Perfiles)
            {
                if ((Perfil.ID == 8) || (Perfil.ID == 29))
                {
                    permiso_autorizar++;
                }
            }
            if (permiso_autorizar == 0)
            {
                Response.Redirect("index.aspx");
            }
        //}
        //else
        //{
        //    if (!user.Aplicaciones.Contains("6"))
        //        Response.Redirect("index.aspx");
        //    int permiso = int.Parse(user.Aplicaciones["6"].ToString());
        //    if (!((permiso & 262144) == 262144))
        //        Response.Redirect("index.aspx");
        //}
        cargo_gv();
    }

    private void cargo_gv() {
        user = (UsuarioBean)Session["usuario"];
        DataTable dt1 = (DataTable)DB.getCreditosPend(user.PaisID);
        gv_pendaut.DataSource = dt1;
        gv_pendaut.DataBind();
    }
    protected void gv_pendaut_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Autorizar")
        {
            int index = Convert.ToInt32(e.CommandArgument);
            Response.Write("<script languaje='JavaScript'>window.location.href='aut_credito.aspx?id=" + gv_pendaut.Rows[index].Cells[1].Text + "';</script>");
        }
    }
    protected void gv_pendaut_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gv_pendaut.PageIndex = e.NewPageIndex;
        gv_pendaut.DataBind();
    }
}
