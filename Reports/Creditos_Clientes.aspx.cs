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

public partial class Reports_Creditos_Clientes : System.Web.UI.Page
{
    UsuarioBean user = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        Session["Contabilidad"] = null;
        if (Session["usuario"] == null)
            Response.Redirect("../default.aspx");
        user = (UsuarioBean)Session["usuario"];

        ArrayList arr = null;
        ListItem item = null;
        if (!Page.IsPostBack)
        {
            arr = (ArrayList)DB.getPaisesbyUser(user.ID);
            foreach (PaisBean pais in arr)
            {
                item = new ListItem(pais.Nombre, pais.ID.ToString());
                lb_pais.Items.Add(item);
            }
            lb_pais.SelectedValue = user.PaisID.ToString();
        }
    }
    protected void btn_generar_Click(object sender, EventArgs e)
    {
        string mensaje = "<script languaje=\"JavaScript\">";
        mensaje += "window.open('reports.aspx?reptype=13&monID=0','Genera_Reporte','menubar=1,resizable=1,scrollbars=1,toolbar=1,width=750');";
        mensaje += "</script>";
        Page.RegisterClientScriptBlock("closewindow", mensaje);
    }
}