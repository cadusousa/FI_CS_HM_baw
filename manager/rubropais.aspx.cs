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

public partial class manager_rubropais : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        int id = 0;
        ArrayList pais_arr = (ArrayList)DB.getPaises("");
        ListItem ite = null;
        foreach (PaisBean paisbean in pais_arr)
        {
            ite = new ListItem(paisbean.Nombre, paisbean.ID.ToString());
            lbpais.Items.Add(ite);
        }
        if (!Page.IsPostBack) {
            if (Request.QueryString["id"] != null)
            {
                id = int.Parse(Request.QueryString["id"].ToString());
                RE_GenericBean rubean = (RE_GenericBean)DB.getRubro(id);
                lb_rub_id.Text = rubean.intC1.ToString();
                tb_rubro.Text = rubean.strC1;
            }
        }
    }
    protected void cmdLogin_Click(object sender, EventArgs e)
    {
        RE_GenericBean rupaibean = new RE_GenericBean();
        rupaibean.intC1 = int.Parse(lb_rub_id.Text.Trim());
        rupaibean.intC2 = int.Parse(lbpais.SelectedValue);
        if (cobraiva.Checked) { rupaibean.intC3 = 1; } else { rupaibean.intC3 = 0; }
        if (ivaincluido.Checked) { rupaibean.intC4 = 1; } else { rupaibean.intC4 = 0; }
        int result = DB.InsertRubroPais(rupaibean);
        if (result == 0)
        {
            lb_msg.Text = "Existió un problema al tratar de asociar el rubro con el pais, por favor intente de nuevo";
            return;
        }
        else if (result == -100)
        {
            lb_msg.Text = "Existió un problema al tratar de asociar el rubro con el pais, por favor intente de nuevo";
            return;
        }
        string mensaje = "<script languaje=\"JavaScript\">";
        mensaje += "window.opener.location='addrubro.aspx?id=" + rupaibean.intC1 + "';";
        mensaje+="window.close();";
        mensaje+="</script>";
        Page.RegisterClientScriptBlock("closewindow", mensaje);
    }
    protected void cmdCancel_Click(object sender, EventArgs e)
    {
        string mensaje = "<script languaje=\"JavaScript\">";        
        mensaje += "window.close();";
        mensaje += "</script>";
        Page.RegisterClientScriptBlock("closewindow", mensaje);
    }
}
