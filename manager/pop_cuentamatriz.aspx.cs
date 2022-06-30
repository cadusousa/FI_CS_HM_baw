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

public partial class manager_cuentamatriz : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        int id = 0;
        int pai_id = 0;
        if (!Page.IsPostBack)
        {
            if (Request.QueryString["ref_id"] != null)
            {
                id = int.Parse(Request.QueryString["ref_id"].ToString());
                lb_ref.Text = id.ToString();
            }
            if (Request.QueryString["pai_id"] != null)
            {
                pai_id = int.Parse(Request.QueryString["pai_id"].ToString());
                lb_pai_id.Text = pai_id.ToString();
            }
            ListItem item = null;
            ArrayList arr = (ArrayList)DB.getCuenta(null);
            item = new ListItem("No aplica", "");
            lb_debe.Items.Add(item);
            lb_haber.Items.Add(item);
            foreach (RE_GenericBean rgb in arr) {
                item = new ListItem(rgb.strC2, rgb.strC1);
                lb_debe.Items.Add(item);
                lb_haber.Items.Add(item);
            }
        }
    }
    protected void cmdLogin_Click(object sender, EventArgs e)
    {
        string ctaDebe = lb_debe.SelectedValue;
        string ctaHaber = lb_haber.SelectedValue;
        int pocDebe=0;
        int pocHaber=0;
        if ((tb_debe!=null) && (!tb_debe.Equals("")))
            pocDebe=int.Parse(tb_debe.Text.Trim());
        if ((tb_haber.Text!=null) && (!tb_haber.Text.Trim().Equals("")))
            pocHaber=int.Parse(tb_haber.Text.Trim());

        RE_GenericBean rgb = new RE_GenericBean();
        rgb.strC1 = ctaDebe;
        if (!ctaDebe.Equals("") && ctaDebe != null) { rgb.intC1 = pocDebe; } else { rgb.intC1 = 0; } 
        rgb.strC2 = ctaHaber;
        if (!ctaHaber.Equals("") && ctaHaber != null) { rgb.intC2 = pocHaber; } else { rgb.intC2 = 0; }
        rgb.intC3 = int.Parse(lb_ref.Text.Trim());

        int result = DB.InsertItem(rgb);
        if (result == 0)
        {
            lb_msg.Text = "Existió un problema al tratar de guardar la informacion, por favor intente de nuevo";
            return;
        }
        else if (result == -100)
        {
            lb_msg.Text = "Existió un problema al tratar de guardar la informacion, por favor intente de nuevo";
            return;
        }
        string mensaje = "<script languaje=\"JavaScript\">";
        mensaje += "window.opener.location='matriz.aspx?id="+rgb.intC3+"&pai_id="+lb_pai_id.Text.Trim()+"';";
        mensaje += "window.close();";
        mensaje += "</script>";
        Page.RegisterClientScriptBlock("closewindow", mensaje);
        
    }
}
