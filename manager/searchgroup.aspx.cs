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


public partial class manager_serchgroup : System.Web.UI.Page
{
    UsuarioBean user = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["usuario"] == null)
        {
            Response.Redirect("../default.aspx");
        }
        else
        {
            user = (UsuarioBean)Session["usuario"];
        }
        Cargar_Grupos();
    }
    protected void Cargar_Grupos()
    {
        ArrayList Arr_Grupos = new ArrayList();
        DataTable dt = new DataTable();
        dt.Columns.Add("ID");
        dt.Columns.Add("NOMBRE");
        dt.Columns.Add("DESCRIPCION");
        Arr_Grupos = (ArrayList)DB.Get_Grupos();
        foreach (RE_GenericBean Bean in Arr_Grupos)
        {
            object[] obj = { Bean.strC1, Bean.strC2, Bean.strC3 };
            dt.Rows.Add(obj);
        }
        gv_grupos.DataSource = dt;
        gv_grupos.DataBind();
    }
    protected void gv_grupos_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.Cells.Count > 1)
        {
            e.Row.Cells[0].Visible = false;
        }
    }
}