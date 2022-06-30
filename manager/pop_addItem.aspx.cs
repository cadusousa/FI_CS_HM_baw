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

public partial class manager_pop_addItem : System.Web.UI.Page
{
    string id;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack) {
            if (Request.QueryString["id"] != null)
            {
                id = Request.QueryString["id"].ToString();
                lbID.Text = id;
            }
        }
    }
    protected void cmdLogin_Click(object sender, EventArgs e)
    {
        string tabla = null;
        string prefijo = "";
        id = lbID.Text.Trim();
        if (id.Equals("Servicio")) { tabla = "tbl_tipo_servicio"; prefijo = "tts"; }
        if (id.Equals("Contribuyente")) { tabla = "tbl_tipo_contribuyente"; prefijo = "tti"; }
        if (id.Equals("Moneda")) { tabla = "tbl_tipo_moneda"; prefijo = "ttm"; }
        if (id.Equals("Cobro")) { tabla = "tbl_tipo_cobro"; prefijo = "ttc"; }
        if (id.Equals("Conta")) { tabla = "tbl_tipo_conta"; prefijo = "tcon"; }
        if (id.Equals("ImpExp")) { tabla = "tbl_tipo_imp_exp"; prefijo = "tie"; }
        string nombre = tbNombre.Text.Trim().ToUpper();
        string descrip = tbDescrip.Text.Trim();
        int result = DB.InsertItem(nombre, descrip, tabla, prefijo);
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
        mensaje += "window.opener.location='matriz.aspx';";
        mensaje += "window.close();";
        mensaje += "</script>";
        Page.RegisterClientScriptBlock("closewindow", mensaje);

    }
}
