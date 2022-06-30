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

public partial class Reports_index : System.Web.UI.Page
{
    UsuarioBean user = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        int bandera = 0;
        if (Session["usuario"] == null)
        {
            Response.Redirect("../default.aspx");
        }
        user = (UsuarioBean)Session["usuario"];
        ArrayList Arr_Perfiles = (ArrayList)DB.getPerfilesxUsu(user.ID, user.PaisID);
        int ban_reporte_general = 0;
        foreach (PerfilesBean Perfil in Arr_Perfiles)
        {
            if ((Perfil.ID == 8) || (Perfil.ID == 18) || (Perfil.ID == 27) || (Perfil.ID == 25) || (Perfil.ID == 33))
            {
                ban_reporte_general++;
            }
        }
        if ((!user.Aplicaciones.Contains("9"))&&(ban_reporte_general==0))
        {
            if (user.Aplicaciones.Contains("6"))
                Response.Redirect("../operations/index.aspx");
            else
                if (user.Aplicaciones.Contains("5"))
                    Response.Redirect("../invoice/index.aspx");
                else
                    if ((user.Aplicaciones.Contains("1")) || (user.Aplicaciones.Contains("2")) || (user.Aplicaciones.Contains("3")) || (user.Aplicaciones.Contains("4")))
                        Response.Redirect("../manager/manager.aspx");
                    else
                    {
                        Session["msg"] = "Usted no tiene permisos para ver ninguna opcion";
                        Session["url"] = "/manager/countrychoice.aspx";
                        Response.Redirect("message.aspx");
                    }
        }
        //ArrayList Arr_Perfiles = (ArrayList)DB.getPerfilesxUsu(user.ID, user.PaisID);
        //foreach (PerfilesBean Perfil in Arr_Perfiles)
        //{
        //    if ((Perfil.ID == 18))//contabilidad
        //    {
        //        bandera = 1;
        //    }
        //}
        //if (bandera == 0)
        //{
        //    Session["msg"] = "Usted no tiene permisos para ver ninguna opcion";
        //    Session["url"] = "../manager/countrychoice.aspx";
        //    Response.Redirect("../invoice/index.aspx");
        //}

        if (Session["configurado"] == null)
        {
            WebMsgBox.Show("Debe configurar los parametros con los que va trabajar.");
            Response.Redirect("../manager/countrychoice.aspx");
        }
    }
}
