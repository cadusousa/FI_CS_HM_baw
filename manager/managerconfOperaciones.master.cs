using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;

public partial class admin : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            lb_contabilidad.Items.Clear();
            // Cargo los tipos de contabilidad
            ArrayList arr = (ArrayList)DB.getOpcionesMatriz("tbl_tipo_conta");
            ListItem item = null;
            foreach (RE_GenericBean rgb in arr)
            {
                item = new ListItem(rgb.strC1, rgb.intC1.ToString());
                lb_contabilidad.Items.Add(item);
            }
            if (Session["Contabilidad"] != null && !Session["Contabilidad"].ToString().Equals(""))
                lb_contabilidad.SelectedValue = Session["Contabilidad"].ToString();
        }
    }
    protected void lb_contabilidad_SelectedIndexChanged(object sender, EventArgs e)
    {
        Session["Contabilidad"] = lb_contabilidad.SelectedValue;
        Response.Redirect("manager.aspx");
    }
}
