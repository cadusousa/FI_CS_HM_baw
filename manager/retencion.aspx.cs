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

public partial class manager_retencion : System.Web.UI.Page
{
    UsuarioBean user = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["usuario"] == null) Response.Redirect("../default.aspx");
        user = (UsuarioBean)Session["usuario"];
        if (!Page.IsPostBack) {
            ArrayList arr = (ArrayList)DB.getRetencionesList(user.PaisID);
            ListItem item = null;
            foreach (RE_GenericBean rgb in arr) {
                item = new ListItem(rgb.strC1, rgb.intC1.ToString());
                lb_retenciones.Items.Add(item);
            }

            
            // Cargo las opciones de pago
            arr = (ArrayList)DB.getOpcionesMatriz("tbl_tipo_transaccion where ttt_template='retencion_proveedores.aspx'");
            foreach (RE_GenericBean rgb in arr)
            {
                item = new ListItem(rgb.strC1, rgb.intC1.ToString());
                lb_tipo_retencion.Items.Add(item);
            }
        }
    }
    protected void bt_guardar_Click(object sender, EventArgs e)
    {

        if (Session["usuario"] == null) Response.Redirect("../default.aspx");
        user = (UsuarioBean)Session["usuario"];

        RE_GenericBean rgb = new RE_GenericBean();

        rgb.intC1=int.Parse(tb_id.Text);
        rgb.strC1 = tb_nombre.Text;
        rgb.decC1 = decimal.Parse(tb_minimo.Text);
        rgb.intC2 = int.Parse(tb_porcentaje.Text);
        rgb.intC3 = user.PaisID;
        rgb.intC4 = int.Parse(lb_tipo.SelectedValue);
        rgb.intC5 = int.Parse(lb_tipo_retencion.SelectedValue);
        int result = DB.InsertRetencion_data(rgb);
        if (result >= 0) { 
            WebMsgBox.Show("La retencion fue grabada exitosamente");
            return;
        }
    }
    protected void lb_retenciones_SelectedIndexChanged(object sender, EventArgs e)
    {
        int retID=int.Parse(lb_retenciones.SelectedValue);
        RE_GenericBean rgb = (RE_GenericBean)DB.getRetencionData(retID);
        tb_id.Text=rgb.intC1.ToString();
        tb_nombre.Text=rgb.strC1;
        tb_minimo.Text=rgb.decC1.ToString();
        tb_porcentaje.Text=rgb.intC3.ToString();
        lb_tipo.SelectedValue = rgb.intC2.ToString();
        if (rgb.intC5!=0)  lb_tipo_retencion.SelectedValue = rgb.intC5.ToString();
    }
}
