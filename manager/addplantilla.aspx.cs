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

public partial class manager_addplantilla : System.Web.UI.Page
{
    int fac_id = 0;
    int suc_id = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.QueryString["suc_id"] != null) {
            suc_id = int.Parse(Request.QueryString["suc_id"].ToString().Trim());
            lb_suc_id.Text = suc_id.ToString();
        } else {
            lb_suc_id.Text = "0";
        }
        if (Request.QueryString["fac_id"] != null) {
            fac_id = int.Parse(Request.QueryString["fac_id"].ToString().Trim());
            lb_fac_id.Text = fac_id.ToString();
        } else {
            lb_fac_id.Text = "0";
        }

    }
    protected void cmdLogin_Click(object sender, EventArgs e)
    {
        RE_GenericBean campobean = new RE_GenericBean();
        campobean.intC2 = int.Parse(lb_fac_id.Text.Trim());
        campobean.strC1=tb_campo.Text.Trim().ToUpper();
        campobean.intC3=int.Parse(tb_x.Text.Trim());
        campobean.intC4=int.Parse(tb_y.Text.Trim());
        campobean.intC5 = int.Parse(lb_suc_id.Text.Trim());

        int result = DB.InsertUpdateCampo(campobean);
        if (result == 0)
        {
            lb_msg.Text = "Existió un problema al tratar de guardar los datos del campo, por favor intente de nuevo";
            return;
        }
        else if (result == -100)
        {
            lb_msg.Text = "Existió un problema al tratar de guardar los datos del campo, por favor intente de nuevo";
            return;
        }
        string mensaje = "<script languaje=\"JavaScript\">";
        mensaje += "window.opener.location='addfactura.aspx?id=" + campobean.intC2 + "&suc_id="+campobean.intC5+"';";
        mensaje += "window.close();";
        mensaje += "</script>";
        Page.RegisterClientScriptBlock("closewindow", mensaje);
    }
}
