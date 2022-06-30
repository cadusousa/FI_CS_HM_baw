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

public partial class operations_pop_transferencias : System.Web.UI.Page
{
    UsuarioBean user = null;
    ArrayList arrCtas = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        user = (UsuarioBean)Session["usuario"];
        if (!Page.IsPostBack)
        {
            ArrayList arr = null;
            ListItem item = null;
            lb_moneda.Items.Clear();
            arr = (ArrayList)DB.getMonedasbyPais(user.PaisID, user.contaID);
            foreach (RE_GenericBean rgb in arr)
            {
                item = new ListItem(rgb.strC1, rgb.intC1.ToString());
                lb_moneda.Items.Add(item);
            }
            // obtengo las transacciones
            arr = (ArrayList)DB.getOpcionesMatriz("tbl_tipo_transaccion, tbl_usuario_operacion where uop_ttt_id=ttt_id and ttt_template='pop_transferencias.aspx'");
            lb_transaccion.Items.Clear();
            foreach (RE_GenericBean rgb in arr)
            {
                item = new ListItem(rgb.strC1, rgb.intC1.ToString());
                lb_transaccion.Items.Add(item);
            }

            arrCtas = (ArrayList)Utility.getCuentasContables("Bancarias", "");
            lb_cuentas_bancarias.Items.Clear();
            lb_cuentas_bancarias2.Items.Clear();
            int cont = 0;
            foreach (RE_GenericBean rgb in arrCtas)
            {
                item = new ListItem(rgb.strC4, cont.ToString());
                lb_cuentas_bancarias.Items.Add(item);
                lb_cuentas_bancarias2.Items.Add(item);
                cont++;
            }

        }
    }
    protected void bt_guardar_Click(object sender, EventArgs e)
    {
        
    }

    protected void lb_cuentas_bancarias_SelectedIndexChanged(object sender, EventArgs e)
    {
        ListItem item = null;
        arrCtas = (ArrayList)Utility.getCuentasContables("Bancarias", "");
        RE_GenericBean rgb2 = (RE_GenericBean)arrCtas[int.Parse(lb_cuentas_bancarias.SelectedValue)];
        tb_cuenta_bancaria.Text = rgb2.strC5;
        tb_cuenta_contable.Text = rgb2.strC1;
        tb_cuenta_nombre.Text = rgb2.strC2;

    }
    protected void lb_cuentas_bancarias2_SelectedIndexChanged(object sender, EventArgs e)
    {
        ListItem item = null;
        arrCtas = (ArrayList)Utility.getCuentasContables("Bancarias", "");
        RE_GenericBean rgb2 = (RE_GenericBean)arrCtas[int.Parse(lb_cuentas_bancarias2.SelectedValue)];
        tb_cuenta_bancaria2.Text = rgb2.strC5;
        tb_cuenta_contable2.Text = rgb2.strC1;
        tb_cuenta_nombre2.Text = rgb2.strC2;

    }
}
