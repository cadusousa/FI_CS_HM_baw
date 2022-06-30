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

public partial class invoice_retencion_cliente : System.Web.UI.Page
{
    UsuarioBean user = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["usuario"] == null)
        {
            Response.Redirect("../default.aspx");
        }

        user = (UsuarioBean)Session["usuario"];
        int tipo_contabilidad = int.Parse(Session["Contabilidad"].ToString());
        obtengo_listas(tipo_contabilidad);
        if (tipo_contabilidad == 2)
        {
            lb_moneda.SelectedValue = "8";
            lb_moneda.Enabled = false;
        }
        else
        {
            lb_moneda.SelectedValue = user.pais.ID.ToString();
            lb_moneda.Enabled = false;
        }
        double cliID = 0;
    }

    private void obtengo_listas(int tipo_contabilidad)
    {
        ListItem item = null;
        ArrayList arr = null;
        if (!Page.IsPostBack)
        {
            arr = (ArrayList)DB.getOpcionesMatriz("tbl_tipo_transaccion where ttt_template='retencion_cliente.aspx'");
            foreach (RE_GenericBean rgb in arr)
            {
                item = new ListItem(rgb.strC1, rgb.intC1.ToString());
                lb_trans.Items.Add(item);
            }
            arr = null;
            arr = (ArrayList)DB.getMonedasbyPais(user.PaisID, user.contaID);
            foreach (RE_GenericBean rgb in arr)
            {
                item = new ListItem(rgb.strC1, rgb.intC1.ToString());
                lb_moneda.Items.Add(item);
            }
            arr = null;
            arr = (ArrayList)DB.getSerieFactbySuc(user.SucursalID, 9, user,0);//4 porque es el tipo de documento para nota debito segun sys_tipo_referencia
            foreach (string valor in arr)
            {
                item = new ListItem(valor, valor);
                lb_serie_factura.Items.Add(item);
            }
            
        }
    }
    
    protected void bt_Enviar_Click(object sender, EventArgs e)
    {

    }
    protected void bt_imprimir_Click(object sender, EventArgs e)
    {

    }
    protected void Button1_Click(object sender, EventArgs e)
    {

    }
    protected void lb_servicio_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
}
