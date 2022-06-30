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

public partial class operations_reactivar : System.Web.UI.Page
{
    UsuarioBean user = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["usuario"].ToString() == null)
        {
            Response.Redirect("../default.aspx");
        }

        user = (UsuarioBean)Session["usuario"];
        if (!user.Aplicaciones.Contains("6"))
            Response.Redirect("index.aspx");
        int permiso = int.Parse(user.Aplicaciones["6"].ToString());
        if (!((permiso & 2097152) == 2097152))
            Response.Redirect("index.aspx");


        if (!Page.IsPostBack) {
            ArrayList arr = null;
            ListItem item = null;
            arr = (ArrayList)DB.getSucursalesbyuser(user.ID, user.PaisID);
            lb_sucursal.Items.Clear();
            foreach (SucursalBean suc in arr)
            {
                item = new ListItem(suc.Nombre, suc.ID.ToString());
                lb_sucursal.Items.Add(item);
            }

            arr = null;
            arr = (ArrayList)DB.getOpcionesMatriz("tbl_tipo_conta");
            lb_conta_erronea.Items.Clear();
            foreach (RE_GenericBean rgb in arr)
            {
                item = new ListItem(rgb.strC1, rgb.intC1.ToString());
                lb_conta_erronea.Items.Add(item);
            }
            lb_conta_erronea_SelectedIndexChanged(sender, e);

            arr = (ArrayList)DB.getBancosXPais(user.PaisID, 1);
            lb_bancos.Items.Clear();
            item = new ListItem("Seleccione...", "0");
            lb_bancos.Items.Add(item);
            foreach (RE_GenericBean rgb in arr)
            {
                item = new ListItem(rgb.strC1, rgb.intC1.ToString());
                lb_bancos.Items.Add(item);
            }
            item = new ListItem("Seleccione...", "0");
            lb_cuentas_bancarias.Items.Add(item);
        }
    }
    protected void lb_conta_erronea_SelectedIndexChanged(object sender, EventArgs e)
    {
        ArrayList arr = null;
        ListItem item = null;

        int contaTemp = user.contaID;
        user.contaID = int.Parse(lb_conta_erronea.SelectedValue);
        arr = (ArrayList)DB.getSerieFactbySucReactivar(int.Parse(lb_sucursal.SelectedValue), int.Parse(lb_tipo_doc.SelectedValue), user);
        user.contaID = contaTemp;
        lb_serie_erronea.Items.Clear();
        foreach (string valor in arr)
        {
            item = new ListItem(valor, valor);
            lb_serie_erronea.Items.Add(item);
        }
    }
    protected void lb_tipo_doc_SelectedIndexChanged(object sender, EventArgs e)
    {
        ArrayList arr = null;
        ListItem item = null;

        int contaTemp = user.contaID;
        user.contaID = int.Parse(lb_conta_erronea.SelectedValue);
        arr = (ArrayList)DB.getSerieFactbySucReactivar(int.Parse(lb_sucursal.SelectedValue), int.Parse(lb_tipo_doc.SelectedValue), user);
        user.contaID = contaTemp;
        lb_serie_erronea.Items.Clear();
        foreach (string valor in arr)
        {
            item = new ListItem(valor, valor);
            lb_serie_erronea.Items.Add(item);
        }
    }
    protected void lb_sucursal_SelectedIndexChanged(object sender, EventArgs e)
    {
        ArrayList arr = null;
        ListItem item = null;
        
        int contaTemp = user.contaID;
        user.contaID = int.Parse(lb_conta_erronea.SelectedValue);
        arr = (ArrayList)DB.getSerieFactbySucReactivar(int.Parse(lb_sucursal.SelectedValue), int.Parse(lb_tipo_doc.SelectedValue), user);
        user.contaID = contaTemp;
        lb_serie_erronea.Items.Clear();
        foreach (string valor in arr)
        {
            item = new ListItem(valor, valor);
            lb_serie_erronea.Items.Add(item);
        }
    }

    protected void lb_bancos_SelectedIndexChanged(object sender, EventArgs e)
    {
        CheckBox chk_cortes;
        UsuarioBean user;
        user = (UsuarioBean)Session["usuario"];
        ListItem item = null;
        lb_cuentas_bancarias.Items.Clear();
        item = new ListItem("Seleccione...", " ");
        item.Selected = true;
        lb_cuentas_bancarias.Items.Add(item);
        ArrayList arrCtas = (ArrayList)DB.getCuentasBancariasXpais(int.Parse(lb_bancos.SelectedValue), user.PaisID,1);
        foreach (RE_GenericBean rgb in arrCtas)
        {
            item = new ListItem(rgb.strC1, rgb.strC1);
            lb_cuentas_bancarias.Items.Add(item);
        }
    }

    protected void bt_aceptar_Click(object sender, EventArgs e)
    {
        bt_aceptar.Enabled = false;
        if (tb_correlativo.Text == null || tb_correlativo.Text.Equals(""))
        {
            WebMsgBox.Show("Debe ingresar el numero de correlativo erroneo");
            tb_correlativo.Focus();
            return;
        }
        if ((lb_tipo_doc.SelectedValue == "1") && (user.PaisID == 1))
        {
            WebMsgBox.Show("No puede Reactivar Facturas");
            return;
        }

        int tranID = int.Parse(lb_tipo_doc.SelectedValue);
        string serie_err = lb_serie_erronea.SelectedValue;
        //int corr_err = int.Parse(tb_correlativo.Text);
        int conta_err = int.Parse(lb_conta_erronea.SelectedValue);
        string sqlmodifico = "", sqlvalido = "";
        if (tranID == 1) {
            int corr_err = int.Parse(tb_correlativo.Text);
            sqlvalido = "select tfa_id from tbl_facturacion where tfa_serie='" + serie_err + "' and tfa_correlativo='" + corr_err + "' and tfa_ted_id=3 and tfa_conta_id=" + conta_err + " and tfa_pai_id=" + user.PaisID;
        } else if (tranID == 2) {
            int corr_err = int.Parse(tb_correlativo.Text);
            sqlvalido = "select tre_id from tbl_recibo where tre_serie='" + serie_err + "' and tre_correlativo='" + corr_err + "' and tre_ted_id=3 and tre_tcon_id=" + conta_err + " and tre_pai_id=" + user.PaisID;
        } else if (tranID == 3) {
            int corr_err = int.Parse(tb_correlativo.Text);
            sqlvalido = "select tnc_id from tbl_nota_credito where tnc_serie='" + serie_err + "' and tnc_correlativo=" + corr_err + " and tnc_ted_id=3 and tnc_tcon_id=" + conta_err + " and tnc_pai_id=" + user.PaisID;
        } else if (tranID == 4) {
            int corr_err = int.Parse(tb_correlativo.Text);
            sqlvalido = "select tnd_id from tbl_nota_debito where tnd_serie='" + serie_err + "' and tnd_correlativo=" + corr_err + " and tnd_ted_id=3 and tnd_tcon_id=" + conta_err + " and tnd_pai_id=" + user.PaisID;
        } else if (tranID == 5)
        {
            int corr_err = int.Parse(tb_correlativo.Text);
            int tto = 0;
            tto = DB.Validar_Tipo_Operacion_Reactivacion(user, tranID, serie_err, corr_err, int.Parse(lb_conta_erronea.SelectedValue), int.Parse(lb_sucursal.SelectedValue), "");
            if (tto == 8)
            {
                sqlvalido = "select tpr_prov_id from tbl_provisiones where tpr_serie='" + serie_err + "' and tpr_correlativo=" + corr_err + " and tpr_ted_id=3 and tpr_tcon_id=" + conta_err + " and tpr_pai_id=" + user.PaisID;
            }
            else if (tto == -100)
            {
                sqlvalido = "";
                WebMsgBox.Show("Existio un error al tratar de determinar el Tipo de Operacion");
                bt_aceptar.Enabled = true;
                return;
            }
            else if (tto == 0)
            {
                sqlvalido = "";
                bt_aceptar.Enabled = true;
                WebMsgBox.Show("No se encontro el documento o no se encuentra anulado");
                return;
            }
            else
            {
                sqlvalido = "";
                bt_aceptar.Enabled = true;
                WebMsgBox.Show("No se pueden reactivar Provisiones Automaticas");
                return;
            }

        } else if (tranID == 17) {
            sqlvalido = "select tmb_id from tbl_movimiento_bancario where tmb_tbc_cuenta_bancaria='" + lb_cuentas_bancarias.SelectedValue + "' and tmb_referencia='" + tb_correlativo.Text + "' and tmb_ted_id=3 and tmb_mon_id in (select tpm_mon_id from tbl_pais_moneda where tpm_pai_id=" + user.PaisID + ")";
        }

        int id = (int)DB.ExecuteQuery(sqlvalido);
        if (id==-1) {
            WebMsgBox.Show("No existe el documento que trata de reactivar no existe o bien no esta anulado, por favor revise");
            return;
        }

        int result = -1;
        if (tranID == 1) {
            result = (int)DB.ReactivarFactura(id, user, conta_err);
        } else if (tranID == 2) {
            result = (int)DB.ReactivarRecibo(id, user, conta_err);
        } else if (tranID == 3) {
            result = (int)DB.ReactivarNC(id, user, conta_err);
        } else if (tranID == 4) {
            result = (int)DB.ReactivarND(id, user, conta_err);
        } else if (tranID == 5) {
            result = (int)DB.ReactivarProvision(id, user, conta_err);
        } else if (tranID == 17) {
            result = (int)DB.ReactivarDepositos(id, user, conta_err);
        }
         
        if (result == 0) {
            WebMsgBox.Show("No se pudo activar el documento, por favor revise.");
            return;
        } else {
            WebMsgBox.Show("El documento fue reactivado éxitosamente");
            return;
        }
    }
}
