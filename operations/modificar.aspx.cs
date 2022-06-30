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

public partial class operations_modificar : System.Web.UI.Page
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
        if (!((permiso & 1048576) == 1048576))
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
            lb_conta_correcta.Items.Clear();
            lb_conta_erronea.Items.Clear();
            foreach (RE_GenericBean rgb in arr)
            {
                item = new ListItem(rgb.strC1, rgb.intC1.ToString());
                lb_conta_correcta.Items.Add(item);
                lb_conta_erronea.Items.Add(item);
            }
            lb_conta_erronea_SelectedIndexChanged(sender, e);
        }
    }
    protected void lb_conta_erronea_SelectedIndexChanged(object sender, EventArgs e)
    {
        ArrayList arr = null;
        ListItem item = null;
        int contaIDTemp = user.contaID;

        user.Operacion = 1;//Seteo temporalmente Operacion=1=Facturacion para obtener las series
        user.contaID = int.Parse(lb_conta_erronea.SelectedValue);
        arr = (ArrayList)DB.getSerieFactbySuc(int.Parse(lb_sucursal.SelectedValue), int.Parse(lb_tipo_doc.SelectedValue), user, 0);
        lb_serie_erronea.Items.Clear();
        foreach (string valor in arr)
        {
            item = new ListItem(valor, valor);
            lb_serie_erronea.Items.Add(item);
        }

        arr = null;
        user.contaID = int.Parse(lb_conta_correcta.SelectedValue);
        arr = (ArrayList)DB.getSerieFactbySuc(int.Parse(lb_sucursal.SelectedValue), int.Parse(lb_tipo_doc.SelectedValue), user, 0);
        lb_serie_correcta.Items.Clear();
        foreach (string valor in arr)
        {
            item = new ListItem(valor, valor);
            lb_serie_correcta.Items.Add(item);
        }
        user.contaID = contaIDTemp;
        user.Operacion = 2;//Regreso el Operacion=2=Operaciones
    }
    protected void lb_conta_correcta_SelectedIndexChanged(object sender, EventArgs e)
    {
        ArrayList arr = null;
        ListItem item = null;
        int contaIDTemp = user.contaID;
        
        user.Operacion = 1;//Seteo temporalmente Operacion=1=Facturacion para obtener las series
        user.contaID = int.Parse(lb_conta_erronea.SelectedValue);
        arr = (ArrayList)DB.getSerieFactbySuc(int.Parse(lb_sucursal.SelectedValue), int.Parse(lb_tipo_doc.SelectedValue), user, 0);
        lb_serie_erronea.Items.Clear();
        foreach (string valor in arr)
        {
            item = new ListItem(valor, valor);
            lb_serie_erronea.Items.Add(item);
        }

        arr = null;
        user.contaID = int.Parse(lb_conta_correcta.SelectedValue);
        arr = (ArrayList)DB.getSerieFactbySuc(int.Parse(lb_sucursal.SelectedValue), int.Parse(lb_tipo_doc.SelectedValue), user, 0);
        lb_serie_correcta.Items.Clear();
        foreach (string valor in arr)
        {
            item = new ListItem(valor, valor);
            lb_serie_correcta.Items.Add(item);
        }
        user.contaID = contaIDTemp; 
        user.Operacion = 2;//Regreso el Operacion=2=Operaciones
    }
    
    protected void lb_tipo_doc_SelectedIndexChanged(object sender, EventArgs e)
    {
        ArrayList arr = null;
        ListItem item = null;
        int contaIDTemp = user.contaID;

        user.Operacion = 1;//Seteo temporalmente Operacion=1=Facturacion para obtener las series
        user.contaID = int.Parse(lb_conta_erronea.SelectedValue);
        arr = (ArrayList)DB.getSerieFactbySuc(int.Parse(lb_sucursal.SelectedValue), int.Parse(lb_tipo_doc.SelectedValue), user, 0);
        lb_serie_erronea.Items.Clear();
        foreach (string valor in arr)
        {
            item = new ListItem(valor, valor);
            lb_serie_erronea.Items.Add(item);
        }

        arr = null;
        user.contaID = int.Parse(lb_conta_correcta.SelectedValue);
        arr = (ArrayList)DB.getSerieFactbySuc(int.Parse(lb_sucursal.SelectedValue), int.Parse(lb_tipo_doc.SelectedValue), user, 0);
        lb_serie_correcta.Items.Clear();
        foreach (string valor in arr)
        {
            item = new ListItem(valor, valor);
            lb_serie_correcta.Items.Add(item);
        }
        user.contaID = contaIDTemp;
        user.Operacion = 2;//Regreso el Operacion=2=Operaciones
    }
    protected void lb_sucursal_SelectedIndexChanged(object sender, EventArgs e)
    {
        ArrayList arr = null;
        ListItem item = null;
        int contaIDTemp = user.contaID;

        user.Operacion = 1;//Seteo temporalmente Operacion=1=Facturacion para obtener las series
        user.contaID = int.Parse(lb_conta_erronea.SelectedValue);
        arr = (ArrayList)DB.getSerieFactbySuc(int.Parse(lb_sucursal.SelectedValue), int.Parse(lb_tipo_doc.SelectedValue), user, 0);
        lb_serie_erronea.Items.Clear();
        foreach (string valor in arr)
        {
            item = new ListItem(valor, valor);
            lb_serie_erronea.Items.Add(item);
        }

        arr = null;
        user.contaID = int.Parse(lb_conta_correcta.SelectedValue);
        arr = (ArrayList)DB.getSerieFactbySuc(int.Parse(lb_sucursal.SelectedValue), int.Parse(lb_tipo_doc.SelectedValue), user, 0);
        lb_serie_correcta.Items.Clear();
        foreach (string valor in arr)
        {
            item = new ListItem(valor, valor);
            lb_serie_correcta.Items.Add(item);
        }
        user.contaID = contaIDTemp;
        user.Operacion = 2;//Regreso el Operacion=2=Operaciones
    }

    protected void bt_aceptar_Click(object sender, EventArgs e)
    {
        bt_aceptar.Enabled = false;
        if (tb_correlativo_erroneo.Text == null || tb_correlativo_erroneo.Text.Equals("")) {
            WebMsgBox.Show("Debe ingresar el numero de correlativo erroneo");
            tb_correlativo_erroneo.Focus();
            return;
        }
        if (tb_correlativo_correcto.Text == null || tb_correlativo_correcto.Text.Equals("")) {
            WebMsgBox.Show("Debe ingresar el numero de correlativo correcto");
            tb_correlativo_correcto.Focus();
            return;
        }

        int tranID = int.Parse(lb_tipo_doc.SelectedValue);
        string serie_err = lb_serie_erronea.SelectedValue;
        string serie_corr = lb_serie_correcta.SelectedValue;
        int corr_err = int.Parse(tb_correlativo_erroneo.Text);
        int corr_corr = int.Parse(tb_correlativo_correcto.Text);
        int conta_err = int.Parse(lb_conta_erronea.SelectedValue);
        int conta_corr = int.Parse(lb_conta_correcta.SelectedValue);
        int sucursalID = int.Parse(lb_sucursal.SelectedValue);
        string sqlmodifico = "", sqlvalido = "";
        if (tranID == 1) {
            sqlvalido = "select tfa_id from tbl_facturacion where tfa_serie='" + serie_err + "' and tfa_correlativo='" + corr_err + "' and tfa_conta_id=" + conta_err + " and tfa_suc_id=" + sucursalID + " and tfa_pai_id=" + user.PaisID;
            sqlmodifico = "update tbl_facturacion set tfa_serie='" + serie_corr + "', tfa_correlativo='" + corr_corr + "', tfa_conta_id="+conta_corr+" where tfa_serie='" + serie_err + "' and tfa_correlativo='" + corr_err + "' and tfa_conta_id=" + conta_err+ " and tfa_suc_id=" + sucursalID + " and tfa_pai_id=" + user.PaisID;
        } else if (tranID == 2) {
            sqlvalido = "select tre_id from tbl_recibo where tre_serie='" + serie_err + "' and tre_correlativo='" + corr_err + "' and tre_tcon_id=" + conta_err + " and tre_suc_id=" + sucursalID + " and tre_pai_id=" + user.PaisID;
            sqlmodifico = "update tbl_recibo set tre_serie='" + serie_corr + "', tre_correlativo='" + corr_corr + "', tre_tcon_id=" + conta_corr + " where tre_serie='" + serie_err + "' and tre_correlativo='" + corr_err + "' and tre_tcon_id=" + conta_err + " and tre_suc_id=" + sucursalID + " and tre_pai_id=" + user.PaisID;
        } else if (tranID == 3) {
            sqlvalido = "select tnc_id from tbl_nota_credito where tnc_serie='" + serie_err + "' and tnc_correlativo=" + corr_err + " and tnc_tcon_id=" + conta_err + " and tnc_suc_id=" + sucursalID + " and tnc_pai_id=" + user.PaisID;
            sqlmodifico = "update tbl_nota_credito set tnc_serie='" + serie_corr + "', tnc_correlativo=" + corr_corr + ", tnc_tcon_id=" + conta_corr +" where tnc_serie='" + serie_err + "' and tnc_correlativo=" + corr_err + " and tnc_tcon_id=" + conta_err + " and tnc_suc_id=" + sucursalID + " and tnc_pai_id=" + user.PaisID;
        } else if (tranID == 4) {
            sqlvalido = "select tnd_id from tbl_nota_debito where tnd_serie='" + serie_err + "' and tnd_correlativo=" + corr_err + " and tnd_tcon_id=" + conta_err + " and tnd_suc_id=" + sucursalID + " and tnd_pai_id=" + user.PaisID;
            sqlmodifico = "update tbl_nota_debito set tnd_serie='" + serie_corr + "', tnd_correlativo=" + corr_corr + ", tnd_tcon_id=" + conta_corr + " where tnd_serie='" + serie_err + "' and tnd_correlativo=" + corr_err + " and tnd_tcon_id=" + conta_err + " and tnd_suc_id=" + sucursalID + " and tnd_pai_id=" + user.PaisID;
        }
        int existedocumento = DB.ExisteDocumento(sqlvalido);
        if (existedocumento == 0)
        {
            WebMsgBox.Show("No existe el documento que trata de modificar, por favor revise");
            return;
        }
        else if (existedocumento == -100)
        {
            WebMsgBox.Show("Existio un error al tratar de comprobar la existencia del documento, por intente de nuevo");
            return;
        }
        else
        {
            if (tranID == 1)
            {
                #region Validar Factura Electronica
                RE_GenericBean factdata = (RE_GenericBean)DB.getFacturaData(existedocumento);
                if ((factdata.strC50 == "1") || (factdata.strC50 == "2"))
                {
                    WebMsgBox.Show("No se pueden Modificar Facturas Electronicas");
                    return;
                }
                #endregion
            }
            else if (tranID == 3)
            {
                #region Validar Documento Electronico
                RE_GenericBean NCData = (RE_GenericBean)DB.getNotaCreditoData(existedocumento);
                if ((NCData.strC50 == "1") || (NCData.strC50 == "2"))
                {
                    WebMsgBox.Show("No se pueden Modificar Notas de Credito Electronicas");
                    return;
                }
                #endregion
            }
        }
        int result=(int)DB.UpdateEstadoDocumento(sqlmodifico);
        if (result!=1){
            WebMsgBox.Show("No se pudo actualizar el documento, por favor revise.");
            return;
        } else {
            WebMsgBox.Show("El cambio fué realizado éxitosamente");
            return;
        }

    }
}
