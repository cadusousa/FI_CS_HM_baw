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
using Npgsql;

public partial class operations_cambio_fechas : System.Web.UI.Page
{
    UsuarioBean user = null;
    DataTable dt = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        ArrayList arr = null;
        ListItem item = null;
        if (Session["usuario"] == null)
            Response.Redirect("../default.aspx");

        user = (UsuarioBean)Session["usuario"];
        #region Backup Permisos
        //if (!user.Aplicaciones.Contains("6"))
        //    Response.Redirect("index.aspx");
        //int permiso = int.Parse(user.Aplicaciones["6"].ToString());
        //if (!((permiso & 262144) == 262144))
        //    Response.Redirect("index.aspx");
        #endregion
        ArrayList Arr_Perfiles = (ArrayList)DB.getPerfilesxUsu(user.ID, user.PaisID);
        int bandera_permiso = 0;
        foreach (PerfilesBean Perfil in Arr_Perfiles)
        {
            if ((Perfil.ID == 18) || (Perfil.ID == 8))
            {
                bandera_permiso++;
            }
        }
        if (bandera_permiso == 0)
        {
            Response.Redirect("index.aspx");
        }
        if (!Page.IsPostBack)
        {
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

            arr = null;
            arr = (ArrayList)DB.getMonedasbyPais(user.PaisID, user.contaID);
            item = null;
            foreach (RE_GenericBean rgb in arr)
            {
                item = new ListItem(rgb.strC1, rgb.intC1.ToString());
                lb_moneda.Items.Add(item);
            }
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
        ArrayList arrCtas = (ArrayList)DB.getCuentasBancariasXpais(int.Parse(lb_bancos.SelectedValue), user.PaisID,0);
        foreach (RE_GenericBean rgb in arrCtas)
        {
            item = new ListItem(rgb.strC1, rgb.strC1);
            lb_cuentas_bancarias.Items.Add(item);
        }
    }
    protected void bt_aceptar_Click(object sender, EventArgs e)
    {
        tb_id.Text = null;
        tb_serie.Text = null;
        tb_correlat.Text = null;
        tb_monto.Text = null;
        tb_tipo.Text = null;
        DataTable dt = null;
        int contaID = int.Parse(Session["Contabilidad"].ToString());
        user = (UsuarioBean)Session["usuario"];
        int tranID = int.Parse(lb_tipo_doc.SelectedValue);
        string serie = tb_refid.Text;
        string corr = tb_correlativo.Text;
        int banco = int.Parse(lb_bancos.SelectedValue);
        string cuenta = lb_cuentas_bancarias.SelectedValue;
        int cliID = 0;
        if (tb_agenteID.Text != null && !tb_agenteID.Text.Equals(""))
        {
            cliID = int.Parse(tb_agenteID.Text);
        }
        string sql = "";

        if (tranID == 1)
        {
            //sql = "select tfa_id, tfa_serie, tfa_correlativo, tfa_nombre, round(cast(tfa_total as numeric),2), tfa_moneda,to_char(tfa_fecha_emision, 'dd/MM/yyyy')  from tbl_facturacion where tfa_id>0  and tfa_pai_id=" + user.PaisID + " and tfa_moneda =" + user.Moneda + " and tfa_conta_id =" + contaID;
            sql = "select tfa_id, tfa_serie, tfa_correlativo, tfa_nombre, round(cast(tfa_total as numeric),2), tfa_moneda,to_char(tfa_fecha_emision, 'dd/MM/yyyy')  from tbl_facturacion where tfa_id>0  and tfa_pai_id=" + user.PaisID + " and tfa_moneda =" + lb_moneda.SelectedValue + " and tfa_conta_id =" + contaID;
            if (serie != null && !serie.Trim().Equals("")) sql += " and tfa_serie='" + serie.Trim() + "'";
            if (corr != null && !corr.Trim().Equals("")) sql += " and tfa_correlativo='" + corr.Trim() + "'";
            if (cliID > 0) sql += " and tfa_cli_id=" + cliID;
            dt = (DataTable)DB.getInfoAnular(sql);
            gv_resultado.DataSource = dt;
            gv_resultado.DataBind();
        }
        else if (tranID == 2)
        {
            //sql = "select tre_id, tre_serie, tre_correlativo, tre_cli_nombre, round(cast(tre_monto as numeric),2), tre_moneda_id, to_char(tre_fecha, 'dd/MM/yyyy') from tbl_recibo where tre_id>0  and tre_pai_id=" + user.PaisID + " and tre_moneda_id =" + user.Moneda + " and tre_tcon_id =" + contaID;
            sql = "select tre_id, tre_serie, tre_correlativo, tre_cli_nombre, round(cast(tre_monto as numeric),2), tre_moneda_id, to_char(tre_fecha, 'dd/MM/yyyy') from tbl_recibo where tre_id>0  and tre_pai_id=" + user.PaisID + " and tre_moneda_id =" + lb_moneda.SelectedValue + " and tre_tcon_id =" + contaID;
            if (serie != null && !serie.Trim().Equals("")) sql += " and tre_serie='" + serie.Trim() + "'";
            if (corr != null && !corr.Trim().Equals("")) sql += " and tre_correlativo='" + corr.Trim() + "'";
            if (cliID > 0) sql += " and tre_cli_id=" + cliID;
            dt = (DataTable)DB.getInfoAnular(sql);
            gv_resultado.DataSource = dt;
            gv_resultado.DataBind();
        }
        else if (tranID == 17)
        {
            if ((banco != 0) && (cuenta != " "))
            {
                sql = "select tmb_id, tmb_tbc_cuenta_bancaria, tmb_referencia, tba_nombre, round(cast(tmb_monto as numeric),2), tmb_mon_id,to_char(tmb_fecha, 'dd/MM/yyyy') from tbl_banco, tbl_movimiento_bancario where tmb_tba_id=tba_id  and tmb_id>0  and tmb_mon_id =  (select tbc_mon_id from tbl_banco_cuenta where tbc_cuenta_bancaria = '" + cuenta.Trim() + "') ";
                if (corr != null && !corr.Trim().Equals("")) sql += " and tmb_referencia='" + corr.Trim() + "'";
                if (cuenta != null && !cuenta.Trim().Equals("") && !cuenta.Trim().Equals("0")) sql += " and tmb_tbc_cuenta_bancaria='" + cuenta.Trim() + "'";
                if (banco != null && !banco.Equals(0)) sql += " and tmb_tba_id=" + banco; dt = (DataTable)DB.getInfoAnular(sql);
                gv_resultado.DataSource = dt;
                gv_resultado.DataBind();
            }
            else
            {
                WebMsgBox.Show("Debe Ingresar seleccionar una cuenta y un banco");
                return;
            }
        }
        else if (tranID == 28)
        {
            if ((banco != 0) && (cuenta != " "))
            {
                sql = "select tmbc_id, tmbc_tbc_cuenta_bancaria, tmbc_referencia, tba_nombre, round(cast(tmbc_monto as numeric),2), tmbc_mon_id,to_char(tmbc_fecha, 'dd/MM/yyyy') from tbl_banco, tbl_movimiento_bancario_corte where tmbc_tba_id=tba_id  and tmbc_id>0  and tmbc_mon_id =  (select tbc_mon_id from tbl_banco_cuenta where tbc_cuenta_bancaria = '" + cuenta.Trim() + "') ";
                if (corr != null && !corr.Trim().Equals("")) sql += " and tmbc_referencia='" + corr.Trim() + "'";
                if (cuenta != null && !cuenta.Trim().Equals("") && !cuenta.Trim().Equals("0")) sql += " and tmbc_tbc_cuenta_bancaria='" + cuenta.Trim() + "'";
                if (banco != null && !banco.Equals(0)) sql += " and tmbc_tba_id=" + banco; dt = (DataTable)DB.getInfoAnular(sql);
                gv_resultado.DataSource = dt;
                gv_resultado.DataBind();
            }
            else
            {
                WebMsgBox.Show("Debe Ingresar seleccionar una cuenta y un banco");
                return;
            }
        }
        else if ((tranID == 3) || (tranID == 12) || (tranID == 18) || (tranID == 31))
        {
            //sql = "select tnc_id, tnc_serie, tnc_correlativo, tnc_nombre, round(cast(tnc_monto as numeric),2), tnc_mon_id,to_char(tnc_fecha, 'dd/MM/yyyy') from tbl_nota_credito where tnc_ttr_id=" + tranID + "  and tnc_pai_id=" + user.PaisID + " and tnc_mon_id =" + user.Moneda + " and tnc_tcon_id =" + contaID;
            sql = "select tnc_id, tnc_serie, tnc_correlativo, tnc_nombre, round(cast(tnc_monto as numeric),2), tnc_mon_id,to_char(tnc_fecha, 'dd/MM/yyyy') from tbl_nota_credito where tnc_ttr_id=" + tranID + "  and tnc_pai_id=" + user.PaisID + " and tnc_mon_id =" + lb_moneda.SelectedValue + " and tnc_tcon_id =" + contaID;
            if (serie != null && !serie.Trim().Equals("")) sql += " and tnc_serie='" + serie.Trim() + "'";
            if (corr != null && !corr.Trim().Equals("")) sql += " and tnc_correlativo='" + corr.Trim() + "'";
            if (cliID > 0) sql += " and tnc_cli_id=" + cliID;
            dt = (DataTable)DB.getInfoAnular(sql);
            gv_resultado.DataSource = dt;
            gv_resultado.DataBind();
        }
        else if (tranID == 4)
        {
            //sql = "select tnd_id, tnd_serie, tnd_correlativo, tnd_nombre, round(cast(tnd_total as numeric),2), tnd_moneda,to_char(tnd_fecha_emision, 'dd/MM/yyyy') from tbl_nota_debito where tnd_id>0  and tnd_pai_id=" + user.PaisID + " and tnd_moneda =" + user.Moneda + " and tnd_tcon_id =" + contaID;
            sql = "select tnd_id, tnd_serie, tnd_correlativo, tnd_nombre, round(cast(tnd_total as numeric),2), tnd_moneda,to_char(tnd_fecha_emision, 'dd/MM/yyyy') from tbl_nota_debito where tnd_id>0  and tnd_pai_id=" + user.PaisID + " and tnd_moneda =" + lb_moneda.SelectedValue + " and tnd_tcon_id =" + contaID;
            if (serie != null && !serie.Trim().Equals("")) sql += " and tnd_serie='" + serie.Trim() + "'";
            if (corr != null && !corr.Trim().Equals("")) sql += " and tnd_correlativo='" + corr.Trim() + "'";
            if (cliID > 0) sql += " and tnd_cli_id=" + cliID;
            dt = (DataTable)DB.getInfoAnular(sql);
            gv_resultado.DataSource = dt;
            gv_resultado.DataBind();
        }
        else if (tranID == 5)
        {
            //sql = "select tpr_prov_id, tpr_serie, tpr_correlativo, tpr_nombre, round(cast(tpr_valor as numeric),2), tpr_mon_id,to_char(tpr_fecha_creacion, 'dd/MM/yyyy') from tbl_provisiones where tpr_prov_id>0  and tpr_pai_id=" + user.PaisID + " and tpr_mon_id =" + user.Moneda + " and tpr_tcon_id =" + contaID;
            sql = "select tpr_prov_id, tpr_serie, tpr_correlativo, tpr_nombre, round(cast(tpr_valor as numeric),2), tpr_mon_id,to_char(tpr_fecha_creacion, 'dd/MM/yyyy') from tbl_provisiones where tpr_prov_id>0  and tpr_pai_id=" + user.PaisID + " and tpr_mon_id =" + lb_moneda.SelectedValue + " and tpr_tcon_id =" + contaID;
            if (serie != null && !serie.Trim().Equals("")) sql += " and tpr_serie='" + serie.Trim() + "'";
            if (corr != null && !corr.Trim().Equals("")) sql += " and tpr_correlativo='" + corr.Trim() + "'";
            if (cliID > 0) sql += " and tpr_proveedor_id=" + cliID;
            dt = (DataTable)DB.getInfoAnular(sql);
            gv_resultado.DataSource = dt;
            gv_resultado.DataBind();
        }
        else if (tranID == 6)
        {
            sql = "select tcg_id, tcg_cuenta, tcg_numero, tcg_acreditado, round(cast(tcg_valor as numeric),2), tcg_ttm_id,to_char(tcg_fecha, 'dd/MM/yyyy') from tbl_cheques_generados where tcg_ttr_id in (select ttt_id from tbl_tipo_transaccion where ttt_template in ('pop_cheques.aspx', 'anticipos.aspx'))  and tcg_pai_id=" + user.PaisID + " and tcg_tcon_id =" + contaID + " and tcg_ttm_id in (select tbc_mon_id from tbl_banco_cuenta where tbc_cuenta_bancaria = '" + cuenta.Trim() + "')  ";
            if (corr != null && !corr.Trim().Equals("")) sql += " and tcg_numero='" + corr.Trim() + "'";
            if (cuenta != null && !cuenta.Trim().Equals("") && !cuenta.Trim().Equals("0")) sql += " and tcg_cuenta='" + cuenta.Trim() + "'";
            if (banco != null && !banco.Equals(0)) sql += " and tcg_cuenta in (select tbc_cuenta_bancaria from tbl_banco_cuenta where tbc_tba_id=" + banco + ")";
            if (cliID > 0) sql += " and tcg_cli_id=" + cliID;
            dt = (DataTable)DB.getInfoAnular(sql);
            gv_resultado.DataSource = dt;
            gv_resultado.DataBind();
        }
        else if (tranID == 9)
        {
            //sql = "select trp_id, trp_serie, trp_referencia, tpr_nombre, round(cast(trp_monto as numeric),2), trp_mon_id,to_char(trp_fecha_generacion, 'dd/MM/yyyy') from tbl_retencion_provision, tbl_provisiones where trp_tpr_prov_id=tpr_prov_id and trp_id>0  and trp_pai_id=" + user.PaisID + " and tpr_mon_id =" + user.Moneda + " and tpr_tcon_id =" + contaID;
            sql = "select trp_id, trp_serie, trp_referencia, tpr_nombre, round(cast(trp_monto as numeric),2), trp_mon_id,to_char(trp_fecha_generacion, 'dd/MM/yyyy') from tbl_retencion_provision, tbl_provisiones where trp_tpr_prov_id=tpr_prov_id and trp_id>0  and trp_pai_id=" + user.PaisID + " and tpr_mon_id =" + lb_moneda.SelectedValue + " and tpr_tcon_id =" + contaID;
            if (serie != null && !serie.Trim().Equals("")) sql += " and trp_serie='" + serie.Trim() + "'";
            if (corr != null && !corr.Trim().Equals("")) sql += " and trp_referencia='" + corr.Trim() + "'";
            if (cliID > 0) sql += " and tpr_proveedor_id=" + cliID;
            dt = (DataTable)DB.getInfoAnular(sql);
            gv_resultado.DataSource = dt;
            gv_resultado.DataBind();
        }
        else if (tranID == 15) // ajuste contable
        {
            //sql = " select tac_id, tac_serie, tac_correlativo, '', round(cast(tac_cargo as numeric),2), tac_mon_id, to_char(tac_fecha_creacion, 'dd/MM/yyyy') from tbl_ajuste_contable where tac_id>0  and tac_pai_id=" + user.PaisID + " and tac_mon_id =" + user.Moneda + " and tac_conta_id =" + contaID;
            sql = " select tac_id, tac_serie, tac_correlativo, '', round(cast(tac_cargo as numeric),2), tac_mon_id, to_char(tac_fecha_creacion, 'dd/MM/yyyy') from tbl_ajuste_contable where tac_id>0  and tac_pai_id=" + user.PaisID + " and tac_mon_id =" + lb_moneda.SelectedValue + " and tac_conta_id =" + contaID;
            if (serie != null && !serie.Trim().Equals("")) sql += " and tac_serie='" + serie.Trim() + "'";
            if (corr != null && !corr.Trim().Equals("")) sql += " and tac_correlativo='" + corr.Trim() + "'";
            //if (cliID > 0) sql += " and trc_cli_id=" + cliID;
            dt = (DataTable)DB.getInfoAnular(sql);
            gv_resultado.DataSource = dt;
            gv_resultado.DataBind();
        }
        else if (tranID == 19)
        {
            sql = "select tcg_id, tcg_cuenta, tcg_no_transferencia, tcg_acreditado, round(cast(tcg_valor as numeric),2), tcg_ttm_id,to_char(tcg_fecha, 'dd/MM/yyyy') from tbl_cheques_generados where tcg_ttr_id in (select ttt_id from tbl_tipo_transaccion where ttt_template in ('pop_transferencias_ag.aspx', 'transferencia_anticipos.aspx')) and tcg_pai_id=" + user.PaisID + " and tcg_tcon_id =" + contaID + " and tcg_ttm_id in (select tbc_mon_id from tbl_banco_cuenta where tbc_cuenta_bancaria = '" + cuenta.Trim() + "')";
            if (corr != null && !corr.Trim().Equals("")) sql += " and tcg_no_transferencia='" + corr.Trim() + "'";
            if (cuenta != null && !cuenta.Trim().Equals("") && !cuenta.Trim().Equals("0")) sql += " and tcg_cuenta='" + cuenta.Trim() + "'";
            if (banco != null && !banco.Equals(0)) sql += " and tcg_cuenta in (select tbc_cuenta_bancaria from tbl_banco_cuenta where tbc_tba_id=" + banco + ")";
            if (cliID > 0) sql += " and tcg_cli_id=" + cliID;
            dt = (DataTable)DB.getInfoAnular(sql);
            gv_resultado.DataSource = dt;
            gv_resultado.DataBind();
        }
        else if (tranID == 20)
        {
            sql = "select tndb_id, tndb_tbc_cuenta_bancaria, tndb_credito_no, '', round(cast(tndb_valor as numeric),2), tndb_tbc_mon_id,to_char(tndb_fecha, 'dd/MM/yyyy') from tbl_nota_debito_bancaria where  tndb_pai_id=" + user.PaisID + " and tndb_tbc_mon_id = (select tbc_mon_id from tbl_banco_cuenta where tbc_cuenta_bancaria = '" + cuenta.Trim() + "') ";
            if (corr != null && !corr.Trim().Equals("")) sql += " and tndb_credito_no='" + corr.Trim() + "'";
            if (cuenta != null && !cuenta.Trim().Equals("") && !cuenta.Trim().Equals("0")) sql += " and tndb_tbc_cuenta_bancaria='" + cuenta.Trim() + "'";
            if (banco != null && !banco.Equals(0)) sql += " and tndb_tbc_cuenta_bancaria in (select tbc_cuenta_bancaria from tbl_banco_cuenta where tbc_tba_id=" + banco + ")";
            dt = (DataTable)DB.getInfoAnular(sql);
            gv_resultado.DataSource = dt;
            gv_resultado.DataBind();
        }
        else if (tranID == 21)
        {
            sql = "select tncb_id, tncb_tbc_cuenta_bancaria, tncb_credito_no, '', round(cast(tncb_valor as numeric),2), tncb_tbc_mon_id,to_char(tncb_fecha_emision, 'dd/MM/yyyy') from tbl_nota_credito_bancaria where  tncb_pai_id=" + user.PaisID + " and tncb_tbc_mon_id = (select tbc_mon_id from tbl_banco_cuenta where tbc_cuenta_bancaria = '" + cuenta.Trim() + "') ";
            if (corr != null && !corr.Trim().Equals("")) sql += " and tncb_credito_no='" + corr.Trim() + "'";
            if (cuenta != null && !cuenta.Trim().Equals("") && !cuenta.Trim().Equals("0")) sql += " and tncb_tbc_cuenta_bancaria='" + cuenta.Trim() + "'";
            if (banco != null && !banco.Equals(0)) sql += " and tncb_tbc_cuenta_bancaria in (select tbc_cuenta_bancaria from tbl_banco_cuenta where tbc_tba_id=" + banco + ")";
            dt = (DataTable)DB.getInfoAnular(sql);
            gv_resultado.DataSource = dt;
            gv_resultado.DataBind() ;
        }

        else if (tranID == 22)//recibo soa
        {
            //sql = "select trc_id, trc_serie, trc_correlativo, trc_cli_nombre, round(cast(trc_monto as numeric),2), trc_moneda_id, to_char(trc_fecha, 'dd/MM/yyyy') from tbl_recibo_corte where trc_id>0  and trc_pai_id=" + user.PaisID + " and trc_moneda_id =" + user.Moneda + " and trc_tcon_id =" + contaID;
            sql = "select trc_id, trc_serie, trc_correlativo, trc_cli_nombre, round(cast(trc_monto as numeric),2), trc_moneda_id, to_char(trc_fecha, 'dd/MM/yyyy') from tbl_recibo_corte where trc_id>0  and trc_pai_id=" + user.PaisID + " and trc_moneda_id =" + lb_moneda.SelectedValue + " and trc_tcon_id =" + contaID;
            if (serie != null && !serie.Trim().Equals("")) sql += " and trc_serie='" + serie.Trim() + "'";
            if (corr != null && !corr.Trim().Equals("")) sql += " and trc_correlativo='" + corr.Trim() + "'";
            if (cliID > 0) sql += " and trc_cli_id=" + cliID;
            dt = (DataTable)DB.getInfoAnular(sql);
            gv_resultado.DataSource = dt;
            gv_resultado.DataBind();
        }

        else if (tranID == 23) //Corte Proveedor
        {
            //sql = "select tcp_id, tcp_serie, tcp_correlativo,'', round(cast(tcp_total as numeric),2), tcp_mon_id,to_char(tcp_fecha_creacion, 'dd/MM/yyyy') from tbl_corte_proveedor where tcp_id>0  and tcp_pai_id=" + user.PaisID + " and tcp_mon_id =" + user.Moneda + " and tcp_conta_id =" + contaID;
            sql = "select tcp_id, tcp_serie, tcp_correlativo,'', round(cast(tcp_total as numeric),2), tcp_mon_id,to_char(tcp_fecha_creacion, 'dd/MM/yyyy') from tbl_corte_proveedor where tcp_id>0  and tcp_pai_id=" + user.PaisID + " and tcp_mon_id =" + lb_moneda.SelectedValue + " and tcp_conta_id =" + contaID;
            if (serie != null && !serie.Trim().Equals("")) sql += " and tcp_serie='" + serie.Trim() + "'";
            if (corr != null && !corr.Trim().Equals("")) sql += " and tcp_correlativo='" + corr.Trim() + "'";
            if (cliID > 0) sql += " and tcp_cli_id=" + cliID;
            dt = (DataTable)DB.getInfoAnular(sql);
            gv_resultado.DataSource = dt;
            gv_resultado.DataBind();

        }
        else if (tranID == 24) //Liquidaciones
        {
            //sql = "select tli_id, tli_serie, tli_correlativo, '', round(cast(tli_monto as numeric),2), tli_moneda_id,to_char(tli_fecha, 'dd/MM/yyyy') from tbl_liquidaciones where tli_id>0  and tli_pai_id=" + user.PaisID + " and tli_moneda_id =" + user.Moneda + " and tli_conta_id =" + contaID;
            sql = "select tli_id, tli_serie, tli_correlativo, '', round(cast(tli_monto as numeric),2), tli_moneda_id,to_char(tli_fecha, 'dd/MM/yyyy') from tbl_liquidaciones where tli_id>0  and tli_pai_id=" + user.PaisID + " and tli_conta_id =" + contaID;
            if (serie != null && !serie.Trim().Equals("")) sql += " and tli_serie='" + serie.Trim() + "'";
            if (corr != null && !corr.Trim().Equals("")) sql += " and tli_correlativo='" + corr.Trim() + "'";
            if (cliID > 0) sql += " and tli_cli_id=" + cliID;
            dt = (DataTable)DB.getInfoAnular(sql);
            gv_resultado.DataSource = dt;
            gv_resultado.DataBind();

        }
       
    }
    protected void bt_guardar_Click(object sender, EventArgs e)
    {
        int tranID = int.Parse(lb_tipo_doc.SelectedValue);
        int Estado_Conciliado = 0;
        int consiliado = 0;
        int estado = 0;
        ArrayList movimiento = new ArrayList();
        ArrayList detalle = new ArrayList();
        if (tb_fecha.Text == "")
        {
            WebMsgBox.Show("Debe Especificar una Fecha");
            return;
        }
        string fecha_bloqueo = tb_fecha.Text;
        DateTime nueva_fecha = DB.DThhmm(fecha_bloqueo);
        string fecha_anterior = tb_fecha_anterior.Text;
        DateTime f_anterior = DB.DThhmm(fecha_anterior);
        RE_GenericBean bloqueo = null;
        bloqueo = (RE_GenericBean)DB.ObtengoPeriodoBloqueado(user.PaisID);
        DateTime fecha_ultimo_bloqueo = DB.DThhmm(bloqueo.strC1); //fecha del ultimo bloqueo   
        int activo = bloqueo.intC3; // activo
        int bandera = 1;
        int result = 0;
        int _docID = int.Parse(tb_id.Text.ToString());
        if (chb_deposito.Checked.ToString() == "True")
        {
            if ((tb_tipo.Text == "2") || (tb_tipo.Text == "22"))
            {
                movimiento = (ArrayList)DB.getMovimientosByRecibo(int.Parse(tb_id.Text.ToString()),int.Parse(tb_tipo.Text));
                foreach (RE_GenericBean arr in movimiento)
                {
                    if ((arr.intC2 == 3) || (arr.intC2 == 4))//DEPOSITO
                    {
                        if (tb_tipo.Text == "2")
                        {
                            tranID = 17;
                        }
                        else if (tb_tipo.Text == "22")
                        {
                            tranID = 28;
                        }
                    }
                    Estado_Conciliado = DB.Validar_Documento_Conciliado(arr.intC1,tranID);
                    if (Estado_Conciliado == 13)
                    {
                        consiliado = 13;
                    }
                }
                if (consiliado == 13)
                {
                    WebMsgBox.Show("No se Puede Cambiar la fecha porque el Deposito asociado al recibo esta Conciliado");
                    return;
                }
            }
        }
        Estado_Conciliado = 0;
        if ((int.Parse(tb_tipo.Text.ToString()) == 6) || (int.Parse(tb_tipo.Text.ToString()) == 17) || (int.Parse(tb_tipo.Text.ToString()) == 19) || (int.Parse(tb_tipo.Text.ToString()) == 20) || (int.Parse(tb_tipo.Text.ToString()) == 21) || (int.Parse(tb_tipo.Text.ToString()) == 28))
        {
            Estado_Conciliado = DB.Validar_Documento_Conciliado(int.Parse(tb_id.Text.ToString()), int.Parse(tb_tipo.Text.ToString()));
        }
        
        if (Estado_Conciliado == -100)
        {
            WebMsgBox.Show("Existion un error al tratar de validar si el documento esta conciliado o no");
            return;
        }
        else if (Estado_Conciliado == 13)
        {
            WebMsgBox.Show("No se Puede Cambiar fecha a un Documento Conciliado");
            return;
        }
        else
        {
            int Estado_Conciliado_Circulacion = 0;
            Estado_Conciliado_Circulacion = DB.Validar_Documento_Conciliado_Circulacion(int.Parse(tb_id.Text.ToString()), int.Parse(tb_tipo.Text.ToString()));
            if (Estado_Conciliado_Circulacion == -100)
            {
                bt_aceptar.Enabled = false;
                bt_guardar.Enabled = false;
                WebMsgBox.Show("Existion un error al tratar de validar si el documento esta conciliado o no.");
                return;
            }
            else if (Estado_Conciliado_Circulacion > 0)
            {
                bt_aceptar.Enabled = false;
                bt_guardar.Enabled = false;
                WebMsgBox.Show("No se puede cambiar la fecha  del Documento porque esta en circulacion en un periodo ya conciliado, para cambiar fecha primero debe botar la conciliacion respectiva.");
                return;
            }
        }

        if ((int.Parse(tb_tipo.Text.ToString()) == 23) || (int.Parse(tb_tipo.Text.ToString()) == 24))
        {
            //verificar si la fecha del encabezado de liquidacion y corte SOA es menor a la mayor fecha de sus detalles.
            detalle = (ArrayList)DB.getDetallesBydoc(int.Parse(tb_id.Text.ToString()), int.Parse(tb_tipo.Text));
            foreach (RE_GenericBean arr in detalle)
            {
                if (nueva_fecha < DB.DThhmm(arr.strC1))
                {
                    bandera = 0;
                }
            }
            if (bandera == 0)
            {
                WebMsgBox.Show("No se puede mover a una fecha menor a la fecha de emision de los documentos.");
                return;
            }
        }
       
        if (nueva_fecha >= DateTime.Today)
        {
            WebMsgBox.Show("No puede Guardar fechas iguales o mayores al de hoy.");
            return;
        }
        if (tb_motivo.Text == "")
        {
            WebMsgBox.Show("Debe de ingresar un motivo.");
            return;
        }
        if ((nueva_fecha <= fecha_ultimo_bloqueo) && (activo == 1))
        {
            WebMsgBox.Show("No puede poner una fecha que esta dentro de un periodo bloqueado ");
            return;
        }
        decimal tp = DB.getTipoCambioByDay(user, nueva_fecha.ToString("yyyy-MM-dd").Substring(0, 10));
        if ((tp == 0))
        {
            WebMsgBox.Show("No existe tipo de cambio para esta fecha de corte: " + nueva_fecha.ToString());
            return;
        }
        if (tb_tipo.Text.ToString() == "1")
        {
            #region Validar Transaccion Encadenada
            ArrayList Arr_Temp = null;
            string resultado_transaccion_encadenada = "";
            string mensaje = "";
            string mensaje_documentos_asociados = "";
            resultado_transaccion_encadenada = DB.Validar_Transaccion_Encadenada(tranID, _docID);
            if (resultado_transaccion_encadenada == "-100")
            {
                WebMsgBox.Show("Existio un error al tratar de Validar si la trasaccion estaba encadenada");
                return;
            }
            if (resultado_transaccion_encadenada == "PADRE")
            {
                ArrayList Arr_Transacciones_Hijas = (ArrayList)DB.Get_Transacciones_Encadenadas_Hijas(tranID, _docID);
                if (Arr_Transacciones_Hijas == null)
                {
                    WebMsgBox.Show("Existio un error al Tratar de Obtener las Transacciones Encadenadas Hijas");
                    return;
                }
                foreach (RE_GenericBean Hija in Arr_Transacciones_Hijas)
                {
                    PaisBean Empresa_Hijo = (PaisBean)DB.getPais(int.Parse(Hija.strC4));
                    Arr_Temp = DB.getSerieCorrAndObsByDoc(int.Parse(Hija.strC2), int.Parse(Hija.strC3));
                    mensaje_documentos_asociados += "* " + Arr_Temp[5].ToString() + " " + " Serie.: " + Arr_Temp[1].ToString() + " y Correlativo.: " + Arr_Temp[2].ToString() + " en la Empresa.: " + Empresa_Hijo.Nombre_Sistema + " \\n";
                }
                Arr_Temp = DB.getSerieCorrAndObsByDoc(tranID, _docID);
                mensaje = "No se puede Cambiar la Fecha de la Transaccion Padre  Serie.: " + Arr_Temp[1].ToString() + " y Correlativo.: " + Arr_Temp[2].ToString() + " porque tiene los siguientes Documentos Asociados.: \\n";
                mensaje += mensaje_documentos_asociados;
                WebMsgBox.Show(mensaje);
                return;
            }
            #endregion
        }
        if (tb_tipo.Text.ToString() == "5")
        {
            #region Validar Transaccion Encadenada
            string resultado_transaccion_encadenada = "";
            resultado_transaccion_encadenada = DB.Validar_Transaccion_Encadenada(tranID, _docID);
            if (resultado_transaccion_encadenada == "-100")
            {
                WebMsgBox.Show("Existio un error al tratar de Validar si la trasaccion estaba encadenada");
                return;
            }
            if (resultado_transaccion_encadenada == "HIJO")
            {
                RE_GenericBean Provision = (RE_GenericBean)Utility.getDetalleProvision(_docID);
                if (resultado_transaccion_encadenada == "HIJO")
                {
                    if (Provision.intC11 == 10)
                    {
                        RE_GenericBean Intercompany_Origen = null;
                        Intercompany_Origen = (RE_GenericBean)DB.Get_Intercompany_Data(Provision.intC3);
                        if (Intercompany_Origen.intC3 != user.PaisID)
                        {
                            WebMsgBox.Show("Transaccion encadenada, la fecha de esta Transaccion no puede ser modificada dado que tiene asociado el Documento Padre Serie.: " + Provision.strC3 + " y Correlativo.: " + Provision.strC31 + " en la contabilidad de " + Intercompany_Origen.strC5 + ".");
                            return;
                        }
                    }
                }
            }
            #endregion
        }
        if (tb_tipo.Text.ToString() == "4")
        {
            #region Validar Transaccion Encadenada
            ArrayList Arr_Temp = null;
            string resultado_transaccion_encadenada = "";
            string mensaje = "";
            string mensaje_documentos_asociados = "";
            resultado_transaccion_encadenada = DB.Validar_Transaccion_Encadenada(tranID, _docID);
            if (resultado_transaccion_encadenada == "-100")
            {
                WebMsgBox.Show("Existio un error al tratar de Validar si la trasaccion estaba encadenada");
                return;
            }
            if ((resultado_transaccion_encadenada == "PADRE") || (resultado_transaccion_encadenada == "HIJO"))
            {
                #region Validar Hijo
                if (resultado_transaccion_encadenada == "HIJO")
                {
                    //Se permite realizar cambio de Fecha a Nota de Debito Hija, cuando es un cobro al Cliente por cuenta de otro Intercompany
                }
                #endregion
                #region Validar Padre
                if (resultado_transaccion_encadenada == "PADRE")
                {
                    ArrayList Arr_Transacciones_Hijas = (ArrayList)DB.Get_Transacciones_Encadenadas_Hijas(tranID, _docID);
                    if (Arr_Transacciones_Hijas == null)
                    {
                        WebMsgBox.Show("Existio un error al Tratar de Obtener las Transacciones Encadenadas Hijas");
                        return;
                    }
                    foreach (RE_GenericBean Hija in Arr_Transacciones_Hijas)
                    {
                        PaisBean Empresa_Hijo = (PaisBean)DB.getPais(int.Parse(Hija.strC4));
                        Arr_Temp = DB.getSerieCorrAndObsByDoc(int.Parse(Hija.strC2), int.Parse(Hija.strC3));
                        mensaje_documentos_asociados += "* " + Arr_Temp[5].ToString() + " " + " Serie.: " + Arr_Temp[1].ToString() + " y Correlativo.: " + Arr_Temp[2].ToString() + " en la Empresa.: " + Empresa_Hijo.Nombre_Sistema + " \\n";
                    }
                    Arr_Temp = DB.getSerieCorrAndObsByDoc(tranID, _docID);
                    mensaje = "No se puede Cambiar la Fecha de la Transaccion Padre  Serie.: " + Arr_Temp[1].ToString() + " y Correlativo.: " + Arr_Temp[2].ToString() + " porque tiene los siguientes Documentos Asociados.: \\n";
                    mensaje += mensaje_documentos_asociados;
                    WebMsgBox.Show(mensaje);
                    return;
                }
                #endregion
            }
            #endregion
        }
        result = ModificaFechaDocumento(user, nueva_fecha.ToString("yyyy-MM-dd").Substring(0, 10), f_anterior.ToString("yyyy-MM-dd").Substring(0,10), tb_motivo.Text, int.Parse(tb_tipo.Text.ToString()), tb_serie.Text, tb_correlat.Text, int.Parse(tb_id.Text.ToString()), dd_tipo_op.SelectedValue,chb_deposito.Checked.ToString());
        if (result != -100)
        {
            WebMsgBox.Show("Fecha Modificada con Exito");
        }
        else
        {
            WebMsgBox.Show("Ocurrio un Error al momento de Guardar");
        }
        bt_guardar.Enabled = false;
        bt_aceptar.Enabled = false;
    }
    protected void gv_resultado_SelectedIndexChanged(object sender, EventArgs e)
    {
        bt_guardar.Enabled = true;
        bt_guardar.Visible = true;
        lb_id.Visible = true;
        lb_serie.Visible = true;
        lb_correlativo.Visible = true;
        lb_monto.Visible = true;
        lb_nueva_fecha.Visible = true;
        tb_id.Visible = true;
        tb_serie.Visible = true;
        tb_correlat.Visible = true;
        tb_monto.Visible = true;
        tb_fecha.Visible = true;
        tb_fecha_anterior.Visible = true;
        lb_fecha_anterior.Visible = true;
        lb_motivo.Visible = true;
        tb_motivo.Visible = true;
        lb_tipo_operacion.Visible = true;
        dd_tipo_op.Visible = true;
        int id = 0;
        string serie = "",correlativo = "",monto="",fanterior="";
        int tranID = int.Parse(lb_tipo_doc.SelectedValue);
        int index = int.Parse(gv_resultado.SelectedIndex.ToString());
        id = int.Parse(gv_resultado.Rows[index].Cells[1].Text);
        serie = gv_resultado.Rows[index].Cells[2].Text;
        correlativo = gv_resultado.Rows[index].Cells[3].Text;
        monto = gv_resultado.Rows[index].Cells[5].Text;
        fanterior = gv_resultado.Rows[index].Cells[7].Text;

        tb_id.Text = id.ToString();
        tb_tipo.Text = lb_tipo_doc.SelectedValue.ToString();
        tb_serie.Text = serie;
        tb_correlat.Text = correlativo;
        tb_monto.Text = monto;
        tb_fecha_anterior.Text =fanterior;
        if ((tb_tipo.Text == "2") || (tb_tipo.Text == "22"))
        {
            lb_aplica_deposito.Visible = true;
            chb_deposito.Visible = true;
        }
        gv_resultado.Enabled = false;
    }
    protected void gv_resultado_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DataRowView oDataRowView = (DataRowView)e.Row.DataItem;
            if (oDataRowView != null)
            {
                if (oDataRowView.Row["Fecha Emision"] != null)
                {
                    string fecha = oDataRowView.Row["Fecha Emision"].ToString().Trim();
                    int id_doc = int.Parse(oDataRowView.Row["ID"].ToString());
                    #region formateo fechas

                    DateTime fecha_doc = DB.DThhmm(fecha);

                    /*
                    DateTime fecha_doc = DateTime.Parse("01/01/1969");//Ticket#2020021931000081 — RE: Error 

                    string nfec = "";
                    try {
                        fecha_doc = DateTime.Parse(fecha);
                    }
                    catch (Exception ex)
                    {
                        try
                        {
                            //Ticket#2020073031000068 — Error en cambio de fecha 
                            nfec = fecha.Substring(3, 2) + "/" + fecha.Substring(0, 2) + "/" + fecha.Substring(6, 4);
                            fecha_doc = DateTime.Parse(nfec);
                        }
                        catch (Exception ex2)
                        {
                            Response.Write("<script>alert('Error en el formato de fecha del servidor : (" + fecha + ")(" + nfec + ")');</script>");//Ticket#2020021931000081 — RE: Error 
                        }
                    }
                    */

                    //*************************************************************BLOQUEO DE PERIODO*****************************************
                    #region bloqueo periodo
                    RE_GenericBean bloqueo = null;
                    bloqueo = (RE_GenericBean)DB.ObtengoPeriodoBloqueado(user.PaisID);
                    DateTime fecha_ultimo_bloqueo = DB.DThhmm(bloqueo.strC1); //fecha del ultimo bloqueo    
                    DateTime hoy = DateTime.Today; //fecha del dia de hoy
                    int activo = bloqueo.intC3;
                    #endregion
                    if ((fecha_doc <= fecha_ultimo_bloqueo) && (activo == 1))
                    {
                        //WebMsgBox.Show("No se puede emitir documentos con fecha seleccionada: " + fecha_doc.ToString().Substring(0, 10) + " menores al cierre de periodo contable: " + fecha_ultimo_bloqueo.ToString().Substring(0, 10));
                        //return;
                        e.Row.Cells[0].Enabled = false;
                    }
                    if ((lb_tipo_doc.SelectedValue == "1"))
                    {
                        #region Validar Documento Electronico FC
                        RE_GenericBean factdata = (RE_GenericBean)DB.getFacturaData(id_doc);
                        if ((factdata.strC50 == "1") || (factdata.strC50 == "2"))
                        {
                            e.Row.Cells[0].Enabled = false;
                        }
                        #endregion
                    }
                    if ( (lb_tipo_doc.SelectedValue == "3"))
                    {
                        #region Validar Documento Electronico FC
                        RE_GenericBean ncdata = (RE_GenericBean)DB.getNotaCreditoData(id_doc);
                        if ((ncdata.strC50 == "1") || (ncdata.strC50 == "2"))
                        {
                            e.Row.Cells[0].Enabled = false;
                        }
                        #endregion
                    }
                    if ((lb_tipo_doc.SelectedValue == "4"))
                    {
                        #region Validar Documento Electronico FC
                        RE_GenericBean nddata = (RE_GenericBean)DB.getNotaDebitoData(id_doc);
                        if ((nddata.strC50 == "1") || (nddata.strC50 == "2"))
                        {
                            e.Row.Cells[0].Enabled = false;
                        }
                        #endregion
                        
                    }
                    #endregion
                }
            }
        }
    }





    protected static int UpdateFechaLibroD(string fecha, string tdi_ttr_id, int tdi_ref_id, int PaisID, string fecha_anterior, NpgsqlConnection conn, NpgsqlCommand comm)
    {
        /*
        NpgsqlConnection conn1 = OpenConnection();
        NpgsqlDataReader reader1 = null;

        NpgsqlTransaction transaction = (NpgsqlTransaction)conn1.BeginTransaction();

        NpgsqlCommand comm1 = new NpgsqlCommand();
        comm1.Connection = conn1;
        comm1.Transaction = transaction;
        */

        int result = 0;

        try
        {
/*
            comm.Parameters.Clear();

            comm.CommandText = (@"UPDATE tbl_libro_diario SET                   
            tdi_tipo_cambio = tca_tcambio, tdi_fecha='" + fecha + @"', tdi_fecha_documento='" + fecha + @"', 
            tdi_debe_equivalente = CASE WHEN tdi_moneda_id = 8 THEN tdi_debe * tca_tcambio ELSE tdi_debe / tca_tcambio END, 
            tdi_haber_equivalente = CASE WHEN tdi_moneda_id = 8 THEN tdi_haber * tca_tcambio ELSE tdi_haber / tca_tcambio END                 
            FROM tbl_tipo_cambio b 
            WHERE tca_pai_id = tdi_pai_id and CAST(tca_fecha as text) = '" + fecha_anterior + @"' AND
            tdi_ttr_id " + tdi_ttr_id + " AND tdi_ref_id in (" + tdi_ref_id + ")");

            //comm.Parameters.Add("@fecha", NpgsqlTypes.NpgsqlDbType.Date).Value = DateTime.Parse(fecha);
            //comm.Parameters.Add("@fecha_anterior", NpgsqlTypes.NpgsqlDbType.Date).Value = DateTime.Parse(fecha_anterior);
            result = comm.ExecuteNonQuery();

            comm.Parameters.Clear();

            comm.CommandText = (@"INSERT INTO tbl_saldos_cambio_fecha (
            saldo_index,
            tdi_transaction_id,
            fecha_anterior,
            fecha_nueva,	 
            fecha_proceso,
            stat	
            ) 
            SELECT null, tdi_transaction_id, '" + fecha_anterior + "', '" + fecha + @"', NOW(), 1
            FROM tbl_libro_diario 
            WHERE tdi_ttr_id " + tdi_ttr_id + " AND tdi_ref_id in (" + tdi_ref_id + ")");
            result = comm.ExecuteNonQuery();
*/

            comm.CommandText = (@"UPDATE tbl_libro_diario SET                   
            tdi_tipo_cambio = tca_tcambio, tdi_fecha='" + fecha + @"',  

            tdi_debe = CASE WHEN tca_tcambio <> tdi_tipo_cambio AND tdi_moneda_id = 8 THEN tdi_debe_equivalente / tca_tcambio ELSE tdi_debe END, 
	        tdi_haber = CASE WHEN tca_tcambio <> tdi_tipo_cambio AND tdi_moneda_id = 8 THEN tdi_haber_equivalente / tca_tcambio ELSE tdi_haber END,   
	        
	        tdi_debe_equivalente = CASE WHEN tca_tcambio <> tdi_tipo_cambio AND tdi_moneda_id <> 8 THEN tdi_debe / tca_tcambio ELSE tdi_debe_equivalente END, 
	        tdi_haber_equivalente = CASE WHEN tca_tcambio <> tdi_tipo_cambio AND tdi_moneda_id <> 8 THEN tdi_haber / tca_tcambio ELSE tdi_haber_equivalente END,   

            tdi_fecha_documento='" + fecha + @"'
            
            FROM tbl_tipo_cambio b 
            WHERE tca_pai_id = tdi_pai_id and CAST(tca_fecha as text) = '" + fecha + @"' AND
            tdi_ttr_id " + tdi_ttr_id + " AND tdi_ref_id in (" + tdi_ref_id + @");
            INSERT INTO tbl_saldos_cambio_fecha (
            saldo_index,
            tdi_transaction_id,
            fecha_anterior,
            fecha_nueva,	 
            fecha_proceso,
            stat	
            ) 
            SELECT null, tdi_transaction_id, '" + fecha_anterior + "', '" + fecha + @"', NOW(), 1
            FROM tbl_libro_diario 
            WHERE tdi_ttr_id " + tdi_ttr_id + " AND tdi_ref_id in (" + tdi_ref_id + ")");

            result = comm.ExecuteNonQuery();
            comm.Parameters.Clear();

            /*
            if (result > 0)
                transaction.Commit();
            else
                transaction.Rollback();
            
            CloseObj_insert(comm1, conn1);
            */

        }
        catch (Exception e)
        {
            log4net ErrLog = new log4net();
            ErrLog.ErrorLog(e.Message);
        }

        return result;

    }

    public static int ModificaFechaDocumento(UsuarioBean user, string nueva_fecha, string fecha_anterior, string motivo, int tipo, string serie, string correlativo, int id, string tipo_operacion, string mov)
    {
        int resultado = 0;
        int id_movimiento = 0;
        int corte = 0;
        int retencion = 0;
        int tipo2 = 0, res = 0;
        string filtro1 = "", filtro2 = "", operacion = "", query = "";
        string corr = "";
        ArrayList movimiento = new ArrayList();
        NpgsqlTransaction transaction;
        NpgsqlConnection conn;

        NpgsqlDataReader reader = null;
        conn = DB.OpenConnection();
        transaction = conn.BeginTransaction();

        NpgsqlCommand comm = new NpgsqlCommand();
        comm.Connection = conn;
        comm.Transaction = transaction;

        NpgsqlCommand comm1 = new NpgsqlCommand();
        comm1.Connection = conn;
        comm1.Transaction = transaction;

        try
        {// tipos de poeracion(1=ORIGINAL, 2=ANULADAS, 3= AMBAS)
            if (tipo_operacion == "1") // OPERACION ORIGINAL
            {
                filtro1 = " = " + tipo + "  ";
                operacion = "Original";
            }
            else if (tipo_operacion == "2") //OPERACION ANULADA
            {
                tipo2 = tipo + 1000;
                filtro1 = " = " + tipo2 + " ";
                operacion = "Anulada";
            }
            else if (tipo_operacion == "3") //AMBAS ANULADA + ARIGINAL
            {
                tipo2 = tipo + 1000;
                filtro1 = " in (" + tipo + "," + tipo2 + ")  ";
                operacion = "Ambas";
            }
            comm.CommandType = CommandType.Text;
            switch (tipo)
            {
                case 1:   //--FACTURAS--
                    if (tipo_operacion != "2")
                    {
                        comm.Parameters.Clear();
                        comm.CommandText = " update tbl_facturacion set tfa_fecha_emision='" + nueva_fecha + "' where tfa_id =" + id + " ";
                        resultado = comm.ExecuteNonQuery();
                    }
                    comm.Parameters.Clear();

                    //comm.CommandText = " update tbl_libro_diario set tdi_fecha='" + nueva_fecha + "', tdi_fecha_documento='" + nueva_fecha + "' where tdi_ttr_id " + filtro1 + " and tdi_ref_id in (" + id + ")  ";
                    //resultado = comm.ExecuteNonQuery();
                    //comm.Parameters.Clear();
                    res = UpdateFechaLibroD(nueva_fecha, filtro1, id, user.PaisID, fecha_anterior, conn, comm1);

                    break;
                case 2:  //--RECIBO
                    comm.Parameters.Clear();
                    comm.CommandText = " select tpr_tmb_id from tbl_recibo_pago where tpr_tre_id=" + id + " and tpr_tipo_pago in (3,4) ";
                    reader = comm.ExecuteReader();
                    RE_GenericBean rgb = null;
                    while (reader.Read())
                    {
                        rgb = new RE_GenericBean();
                        if (!reader.IsDBNull(0)) rgb.intC1 = int.Parse(reader.GetValue(0).ToString()); //id
                        movimiento.Add(rgb);
                    }
                    reader.Close();
                    if (tipo_operacion != "2")
                    {
                        comm.Parameters.Clear();
                        comm.CommandText = " update tbl_recibo set tre_fecha='" + nueva_fecha + "', tre_fecha_emision='" + nueva_fecha + "' where tre_id = " + id + " ";
                        resultado = comm.ExecuteNonQuery();
                        comm.Parameters.Clear();
                        //actualizo su factura abono
                        comm.CommandText = " update tbl_factura_abono set tfr_fecha_abono ='" + nueva_fecha + "' where tfr_tre_id =" + id + " and tfr_sysref_id = 2 ";
                        resultado = comm.ExecuteNonQuery();
                    }
                    comm.Parameters.Clear();

                    //comm.CommandText = " update tbl_libro_diario set tdi_fecha='" + nueva_fecha + "', tdi_fecha_documento='" + nueva_fecha + "' where tdi_ttr_id " + filtro1 + " and tdi_ref_id = " + id + "  ";
                    //resultado = comm.ExecuteNonQuery();
                    //comm.Parameters.Clear();
                    res = UpdateFechaLibroD(nueva_fecha, filtro1, id, user.PaisID, fecha_anterior, conn, comm1);

                    if ((movimiento.Count != 0) && (mov == "True")) // si el recibo tiene movimiento bancario actualizo tambien.
                    {
                        foreach (RE_GenericBean arr in movimiento)
                        {
                            if (tipo_operacion != "2")
                            {
                                comm.CommandText = " update tbl_movimiento_bancario set tmb_fecha='" + nueva_fecha + "', tmb_fecha_emision='" + nueva_fecha + "' where tmb_id = " + arr.intC1 + "  ";
                                resultado = comm.ExecuteNonQuery();
                            }
                            comm.Parameters.Clear();
                            #region FILTROS 2
                            if (tipo_operacion == "1") // OPERACION ORIGINAL
                            {
                                filtro2 = " = 17 ";
                            }
                            else if (tipo_operacion == "2") //OPERACION ANULADA
                            {

                                filtro2 = " = 1017 ";
                            }
                            else if (tipo_operacion == "3") //AMBAS ANULADA + ARIGINAL
                            {

                                filtro2 = " in (17,1017)  ";
                            }
                            #endregion

                            //comm.CommandText = " update tbl_libro_diario set tdi_fecha='" + nueva_fecha + "', tdi_fecha_documento='" + nueva_fecha + "' where tdi_ttr_id " + filtro2 + " and tdi_ref_id = " + arr.intC1 + "  ";
                            //resultado = comm.ExecuteNonQuery();
                            //comm.Parameters.Clear();
                            res = UpdateFechaLibroD(nueva_fecha, filtro2, arr.intC1, user.PaisID, fecha_anterior, conn, comm1);

                            corr = correlativo.ToString();
                            comm.CommandText = " INSERT INTO tbl_log_cambio_fecha(lc_usuario,lc_ttr_id,lc_fecha_docto,lc_nueva_fecha,lc_fecha_accion,lc_doc_id,lc_doc_pais,lc_doc_serie,lc_motivo,lc_operacion,lc_doc_correlativo) ";
                            comm.CommandText += " VALUES('" + user.ID + "',17,'" + fecha_anterior + "','" + nueva_fecha + "',now(), " + arr.intC1 + " ," + user.PaisID + ",'" + serie + "','" + motivo + "','" + operacion + "','" + corr + "') ";
                            resultado = comm.ExecuteNonQuery();
                            comm.Parameters.Clear();
                        }
                    }

                    break;
                case 3: //Nota de credito
                    if (tipo_operacion != "2")
                    {
                        comm.CommandText = " update tbl_nota_credito set tnc_fecha='" + nueva_fecha + "', tnc_fecha_emision='" + nueva_fecha + "', tnc_fecha_libro_compras='" + nueva_fecha + "' where tnc_id =" + id + " ";
                        resultado = comm.ExecuteNonQuery();
                        comm.Parameters.Clear();
                        //actualizo su factura abono
                        comm.CommandText = " update tbl_factura_abono set tfr_fecha_abono = '" + nueva_fecha + "' where tfr_tre_id =" + id + " and tfr_sysref_id = 3 ";
                        resultado = comm.ExecuteNonQuery();
                    }
                    comm.Parameters.Clear();

                    //comm.CommandText = " update tbl_libro_diario set tdi_fecha='" + nueva_fecha + "', tdi_fecha_documento='" + nueva_fecha + "' where tdi_ttr_id " + filtro1 + " and tdi_ref_id = " + id + "  ";
                    //resultado = comm.ExecuteNonQuery();
                    //comm.Parameters.Clear();
                    res = UpdateFechaLibroD(nueva_fecha, filtro1, id, user.PaisID, fecha_anterior, conn, comm1);

                    //si tiene corte
                    comm.CommandText = " select b.tcp_id from tbl_corte_proveedor_detalle a INNER JOIN tbl_corte_proveedor b ON a.tcpd_tcp_id=b.tcp_id where a.tcpd_docto_id=" + id + " and tcpd_str_id in (3,12,31) ";
                    reader = comm.ExecuteReader();
                    if (reader.Read())
                    {
                        corte = int.Parse(reader.GetValue(0).ToString());
                    }
                    reader.Close();
                    comm.Parameters.Clear();
                    if ((corte != 0) && (tipo_operacion != "2")) // si tiene corte
                    {
                        //actualizo la fecha del detalle del corte
                        comm.CommandText = " update tbl_corte_proveedor_detalle set tcpd_fecha_docto ='" + nueva_fecha + "' where tcpd_tcp_id = " + corte + " and tcpd_docto_id =" + id + " and tcpd_str_id in (3,12,31)  ";
                        resultado = comm.ExecuteNonQuery();
                        comm.Parameters.Clear();
                    }

                    break;
                case 4://Nota de debito
                    comm.CommandText = " update tbl_nota_debito set tnd_fecha_emision='" + nueva_fecha + "', tnd_fecha_libro_compras='" + nueva_fecha + "' where tnd_id = " + id + "  ";
                    resultado = comm.ExecuteNonQuery();
                    comm.Parameters.Clear();

                    //comm.CommandText = " update tbl_libro_diario set tdi_fecha='" + nueva_fecha + "', tdi_fecha_documento='" + nueva_fecha + "' where tdi_ttr_id " + filtro1 + " and tdi_ref_id = " + id + "  ";
                    //resultado = comm.ExecuteNonQuery();
                    //comm.Parameters.Clear();
                    res = UpdateFechaLibroD(nueva_fecha, filtro1, id, user.PaisID, fecha_anterior, conn, comm1);

                    //si tiene corte
                    comm.CommandText = " select b.tcp_id from tbl_corte_proveedor_detalle a INNER JOIN tbl_corte_proveedor b ON a.tcpd_tcp_id=b.tcp_id where a.tcpd_docto_id=" + id + " and tcpd_str_id =4 ";
                    reader = comm.ExecuteReader();
                    if (reader.Read())
                    {
                        corte = int.Parse(reader.GetValue(0).ToString());
                    }
                    reader.Close();
                    comm.Parameters.Clear();
                    if ((corte != 0) && (tipo_operacion != "2")) // si tiene corte
                    {
                        //actualizo la fecha del detalle del corte
                        comm.CommandText = " update tbl_corte_proveedor_detalle set tcpd_fecha_docto ='" + nueva_fecha + "' where tcpd_tcp_id = " + corte + " and tcpd_docto_id =" + id + " and tcpd_str_id =4  ";
                        resultado = comm.ExecuteNonQuery();
                        comm.Parameters.Clear();
                    }
                    break;
                case 5://PROVISIONES
                    int estado = 0;
                    comm.Parameters.Clear();//PROVISION
                    if (tipo_operacion == "3") //Ambas operaciones
                    {
                        //si tiene anulacion
                        comm.CommandText = " select tpr_ted_id from tbl_provisiones where tpr_prov_id = " + id + " ";
                        reader = comm.ExecuteReader();
                        if (reader.Read())
                        {
                            estado = int.Parse(reader.GetValue(0).ToString());
                        }
                        reader.Close();
                        comm.Parameters.Clear();
                        if (estado == 3) //si esta anulada guardo tpr_fecha_anula tambien
                        {
                            query = " update tbl_provisiones set tpr_fecha_acepta='" + nueva_fecha + "', tpr_fecha_creacion='" + nueva_fecha + "', tpr_fecha_emision='" + nueva_fecha + "', tpr_fecha_libro_compras='" + nueva_fecha + "',tpr_fecha_anula='" + nueva_fecha + "' where tpr_prov_id = " + id + "  ";
                        }
                        else //sino no, no guardo el tpr_fecha_anula
                        {
                            comm.Parameters.Clear();
                            comm.CommandText = " select tdi_no_partida from tbl_libro_diario where tdi_ref_id = " + id + " and tdi_ttr_id = 1005  ";
                            reader = comm.ExecuteReader();
                            if (reader.Read())
                            {
                                query = " update tbl_provisiones set tpr_fecha_acepta='" + nueva_fecha + "', tpr_fecha_creacion='" + nueva_fecha + "', tpr_fecha_emision='" + nueva_fecha + "', tpr_fecha_libro_compras='" + nueva_fecha + "',tpr_fecha_anula='" + nueva_fecha + "' where tpr_prov_id = " + id + "  ";
                            }
                            else
                            {
                                query = " update tbl_provisiones set tpr_fecha_acepta='" + nueva_fecha + "', tpr_fecha_creacion='" + nueva_fecha + "', tpr_fecha_emision='" + nueva_fecha + "', tpr_fecha_libro_compras='" + nueva_fecha + "' where tpr_prov_id = " + id + "  ";
                            }
                            reader.Close();
                        }
                    }
                    else if (tipo_operacion == "2") // solo anuladas
                    {
                        comm.Parameters.Clear();
                        //si tiene anulacion
                        comm.CommandText = " select tpr_ted_id from tbl_provisiones where tpr_prov_id = " + id + " ";
                        reader = comm.ExecuteReader();
                        if (reader.Read())
                        {
                            estado = int.Parse(reader.GetValue(0).ToString());
                        }
                        reader.Close();
                        comm.Parameters.Clear();
                        if (estado == 3) //si esta anulada guardo tpr_fecha_anula tambien
                        {
                            query = " update tbl_provisiones set tpr_fecha_anula='" + nueva_fecha + "' where tpr_prov_id = " + id + "  ";
                        }
                        else if (estado != 3)
                        {
                            comm.Parameters.Clear();
                            //si tiene anulacion
                            comm.CommandText = " select tdi_no_partida from tbl_libro_diario where tdi_ref_id = " + id + " and tdi_ttr_id = 1005  ";
                            reader = comm.ExecuteReader();
                            if (reader.Read())
                            {
                                query = " update tbl_provisiones set tpr_fecha_anula='" + nueva_fecha + "' where tpr_prov_id = " + id + "  ";
                            }
                            reader.Close();
                        }
                    }
                    else if (tipo_operacion == "1") // original
                    {
                        query = " update tbl_provisiones set tpr_fecha_acepta='" + nueva_fecha + "', tpr_fecha_creacion='" + nueva_fecha + "', tpr_fecha_emision='" + nueva_fecha + "', tpr_fecha_libro_compras='" + nueva_fecha + "' where tpr_prov_id = " + id + "  ";
                    }

                    comm1.CommandText = query;
                    resultado = comm1.ExecuteNonQuery();
                    comm1.Parameters.Clear();//LIBRO DIARIO

                    //comm.CommandText = " update tbl_libro_diario set tdi_fecha='" + nueva_fecha + "', tdi_fecha_documento='" + nueva_fecha + "' where tdi_ttr_id " + filtro1 + " and tdi_ref_id = " + id + " ";
                    //resultado = comm.ExecuteNonQuery();
                    //comm.Parameters.Clear();
                    res = UpdateFechaLibroD(nueva_fecha, filtro1, id, user.PaisID, fecha_anterior, conn, comm1);
                    if (res > 0)
                    {
                        //si tiene corte
                        comm.CommandText = " select b.tcp_id from tbl_corte_proveedor_detalle a INNER JOIN tbl_corte_proveedor b ON a.tcpd_tcp_id=b.tcp_id where a.tcpd_docto_id=" + id + " and tcpd_str_id in (5,10) ";
                        reader = comm.ExecuteReader();
                        if (reader.Read())
                        {
                            corte = int.Parse(reader.GetValue(0).ToString());
                        }
                        reader.Close();
                        comm.Parameters.Clear();
                        if ((corte != 0) && (tipo_operacion != "2")) // si tiene corte y no es anulacion
                        {
                            //resultado = comm.ExecuteNonQuery();
                            comm.Parameters.Clear();//actualizo la fecha del detalle del corte
                            comm.CommandText = " update tbl_corte_proveedor_detalle set tcpd_fecha_docto ='" + nueva_fecha + "' where tcpd_tcp_id = " + corte + " and tcpd_docto_id =" + id + " and tcpd_str_id in (5,10)  ";
                            resultado = comm.ExecuteNonQuery();
                            comm.Parameters.Clear();
                        }
                        //si tiene retencion
                        comm.CommandText = " select trp_id from tbl_retencion_provision where trp_tpr_prov_id=" + id + "  ";
                        reader = comm.ExecuteReader();
                        if (reader.Read())
                        {
                            retencion = int.Parse(reader.GetValue(0).ToString());
                        }
                        reader.Close();
                        comm.Parameters.Clear();
                        if ((retencion != 0) && (tipo_operacion != "2")) //SI TIENE RETENCION y no es anulacion
                        {
                            comm.CommandText = " update tbl_retencion_provision set trp_fecha_generacion='" + nueva_fecha + "' where trp_id=" + retencion + "   ";
                            resultado = comm.ExecuteNonQuery();
                            comm.Parameters.Clear();

                            //comm.CommandText = " update tbl_libro_diario set tdi_fecha='" + nueva_fecha + "', tdi_fecha_documento='" + nueva_fecha + "' where tdi_ttr_id=9 and tdi_ref_id =" + retencion + "  ";
                            //resultado = comm.ExecuteNonQuery();
                            res = UpdateFechaLibroD(nueva_fecha, " = 9", retencion, user.PaisID, fecha_anterior, conn, comm1);

                        }
                    }
                    break;
                case 6: //CHEQUES
                    if (tipo_operacion != "2")
                    {
                        comm.Parameters.Clear();
                        comm.CommandText = " update tbl_cheques_generados set tcg_fecha='" + nueva_fecha + "', tcg_fecha_emision='" + nueva_fecha + "' where tcg_id = " + id + " ";
                        resultado = comm.ExecuteNonQuery();
                    }
                    comm.Parameters.Clear();

                    //comm.CommandText = " update tbl_libro_diario set tdi_fecha='" + nueva_fecha + "', tdi_fecha_documento='" + nueva_fecha + "' where tdi_ttr_id " + filtro1 + " and tdi_ref_id = " + id + "   ";
                    //resultado = comm.ExecuteNonQuery();
                    //comm.Parameters.Clear();
                    res = UpdateFechaLibroD(nueva_fecha, filtro1, id, user.PaisID, fecha_anterior, conn, comm1);

                    if (tipo_operacion != "2")
                    {
                        comm.CommandText = " update tbl_cheque_corte set corte_fecha='" + nueva_fecha + "' where corte_tcg_id=" + id + "    ";
                        resultado = comm.ExecuteNonQuery();
                        comm.Parameters.Clear();
                    }
                    //SI TIENE LIQUIDACION ACTUALIZO TAMBIEN.
                    comm.Parameters.Clear();
                    comm.CommandText = " update tbl_liquidaciones_detalle set tld_fecha='" + nueva_fecha + "'  where tld_ttr_id=6 and tld_ref_id=" + id + "  ";
                    resultado = comm.ExecuteNonQuery();
                    comm.Parameters.Clear();

                    break;
                case 9://RETENCIONES.
                    if (tipo_operacion != "2")
                    {
                        comm.CommandText = " update tbl_retencion_provision set trp_fecha_generacion='" + nueva_fecha + "' where trp_id=" + id + "   ";
                        resultado = comm.ExecuteNonQuery();
                        comm.Parameters.Clear();

                        //comm.CommandText = " update tbl_libro_diario set tdi_fecha='" + nueva_fecha + "', tdi_fecha_documento='" + nueva_fecha + "' where tdi_ttr_id " + filtro1 + " and tdi_ref_id = " + id + "  ";
                        //resultado = comm.ExecuteNonQuery();
                        res = UpdateFechaLibroD(nueva_fecha, filtro1, id, user.PaisID, fecha_anterior, conn, comm1);

                    }
                    break;
                case 12: //Nota de credito proveedor
                    if (tipo_operacion != "2")
                    {
                        comm.CommandText = " update tbl_nota_credito set tnc_fecha='" + nueva_fecha + "', tnc_fecha_emision='" + nueva_fecha + "', tnc_fecha_libro_compras='" + nueva_fecha + "' where tnc_id =" + id + " ";
                        resultado = comm.ExecuteNonQuery();
                    }
                    comm.Parameters.Clear();

                    //comm.CommandText = " update tbl_libro_diario set tdi_fecha='" + nueva_fecha + "', tdi_fecha_documento='" + nueva_fecha + "' where tdi_ttr_id " + filtro1 + " and tdi_ref_id = " + id + "  ";
                    //resultado = comm.ExecuteNonQuery();
                    //comm.Parameters.Clear();
                    res = UpdateFechaLibroD(nueva_fecha, filtro1, id, user.PaisID, fecha_anterior, conn, comm1);

                    //si tiene corte
                    comm.CommandText = " select b.tcp_id from tbl_corte_proveedor_detalle a INNER JOIN tbl_corte_proveedor b ON a.tcpd_tcp_id=b.tcp_id where a.tcpd_docto_id=" + id + " and tcpd_str_id in (3,12,31) ";
                    reader = comm.ExecuteReader();
                    if (reader.Read())
                    {
                        corte = int.Parse(reader.GetValue(0).ToString());
                    }
                    reader.Close();
                    comm.Parameters.Clear();
                    if ((corte != 0) && (tipo_operacion != "2")) // si tiene corte
                    {
                        //actualizo la fecha del detalle del corte
                        comm.CommandText = " update tbl_corte_proveedor_detalle set tcpd_fecha_docto ='" + nueva_fecha + "' where tcpd_tcp_id = " + corte + " and tcpd_docto_id =" + id + " and tcpd_str_id in (3,12,31)  ";
                        resultado = comm.ExecuteNonQuery();
                        comm.Parameters.Clear();
                    }
                    break;
                case 15: //Ajuste contable
                    if (tipo_operacion != "2")
                    {
                        comm.Parameters.Clear();
                        comm.CommandText = " update tbl_ajuste_contable set tac_fecha_ajustar='" + nueva_fecha + "', tac_fecha_creacion='" + nueva_fecha + "' where tac_id =" + id + " ";
                        resultado = comm.ExecuteNonQuery();
                    }
                    comm.Parameters.Clear();

                    //comm.CommandText = "  update tbl_libro_diario set tdi_fecha='" + nueva_fecha + "', tdi_fecha_documento='" + nueva_fecha + "' where tdi_ttr_id " + filtro1 + " and tdi_ref_id = " + id + "   ";
                    //resultado = comm.ExecuteNonQuery();
                    //comm.Parameters.Clear();
                    res = UpdateFechaLibroD(nueva_fecha, filtro1, id, user.PaisID, fecha_anterior, conn, comm1);


                    break;
                case 17: //Depositos
                    if (tipo_operacion != "2")
                    {
                        comm.Parameters.Clear();
                        comm.CommandText = " update tbl_movimiento_bancario set tmb_fecha='" + nueva_fecha + "', tmb_fecha_emision='" + nueva_fecha + "' where tmb_id =" + id + "  ";
                        resultado = comm.ExecuteNonQuery();
                    }
                    comm.Parameters.Clear();
                    comm.CommandText = " update tbl_liquidaciones_detalle set tld_fecha='" + nueva_fecha + "'  where tld_ttr_id=17 and tld_ref_id=" + id + "  ";
                    resultado = comm.ExecuteNonQuery();
                    comm.Parameters.Clear();

                    //comm.CommandText = " update tbl_libro_diario set tdi_fecha='" + nueva_fecha + "', tdi_fecha_documento='" + nueva_fecha + "' where tdi_ttr_id " + filtro1 + " and tdi_ref_id =" + id + " ";
                    //resultado = comm.ExecuteNonQuery();
                    //comm.Parameters.Clear();
                    res = UpdateFechaLibroD(nueva_fecha, filtro1, id, user.PaisID, fecha_anterior, conn, comm1);


                    break;
                case 18: //Nota de credito AJUSTE
                    if (tipo_operacion != "2")
                    {
                        comm.CommandText = " update tbl_nota_credito set tnc_fecha='" + nueva_fecha + "', tnc_fecha_emision='" + nueva_fecha + "', tnc_fecha_libro_compras='" + nueva_fecha + "' where tnc_id =" + id + " ";
                        resultado = comm.ExecuteNonQuery();
                        comm.Parameters.Clear();
                        //actualizo su factura abono
                        comm.CommandText = " update tbl_factura_abono set tfr_fecha_abono = '" + nueva_fecha + "' where tfr_tre_id =" + id + " and tfr_sysref_id = 18 ";
                        resultado = comm.ExecuteNonQuery();
                    }
                    comm.Parameters.Clear();

                    //comm.CommandText = " update tbl_libro_diario set tdi_fecha='" + nueva_fecha + "', tdi_fecha_documento='" + nueva_fecha + "' where tdi_ttr_id " + filtro1 + " and tdi_ref_id = " + id + "  ";
                    //resultado = comm.ExecuteNonQuery();
                    //comm.Parameters.Clear();
                    res = UpdateFechaLibroD(nueva_fecha, filtro1, id, user.PaisID, fecha_anterior, conn, comm1);

                    ////si tiene corte
                    //comm.CommandText = " select b.tcp_id from tbl_corte_proveedor_detalle a INNER JOIN tbl_corte_proveedor b ON a.tcpd_tcp_id=b.tcp_id where a.tcpd_docto_id=" + id + " and tcpd_str_id in (3,12,31) ";
                    //reader = comm.ExecuteReader();
                    //if (reader.Read())
                    //{
                    //    corte = int.Parse(reader.GetValue(0).ToString());
                    //}
                    //reader.Close();
                    //comm.Parameters.Clear();
                    //if ((corte != 0) && (tipo_operacion != "2")) // si tiene corte
                    //{
                    //    //actualizo la fecha del detalle del corte
                    //    comm.CommandText = " update tbl_corte_proveedor_detalle set tcpd_fecha_docto ='" + nueva_fecha + "' where tcpd_tcp_id = " + corte + " and tcpd_docto_id =" + id + " and tcpd_str_id in (3,12,31)  ";
                    //    resultado = comm.ExecuteNonQuery();
                    //    comm.Parameters.Clear();
                    //}

                    break;
                case 19: //Transferencias
                    if (tipo_operacion != "2")
                    {
                        comm.Parameters.Clear();
                        comm.CommandText = " update tbl_cheques_generados set tcg_fecha='" + nueva_fecha + "', tcg_fecha_emision='" + nueva_fecha + "' where tcg_id = " + id + " ";
                        resultado = comm.ExecuteNonQuery();
                    }
                    comm.Parameters.Clear();

                    //comm.CommandText = " update tbl_libro_diario set tdi_fecha='" + nueva_fecha + "', tdi_fecha_documento='" + nueva_fecha + "' where tdi_ttr_id " + filtro1 + " and tdi_ref_id = " + id + "   ";
                    //resultado = comm.ExecuteNonQuery();
                    res = UpdateFechaLibroD(nueva_fecha, filtro1, id, user.PaisID, fecha_anterior, conn, comm1);

                    comm.Parameters.Clear();
                    if (tipo_operacion != "2")
                    {
                        comm.CommandText = " update tbl_cheque_corte set corte_fecha='" + nueva_fecha + "' where corte_tcg_id=" + id + "    ";
                        resultado = comm.ExecuteNonQuery();
                        comm.Parameters.Clear();
                    }
                    comm.Parameters.Clear();
                    comm.Parameters.Clear();
                    comm.CommandText = " update tbl_liquidaciones_detalle set tld_fecha='" + nueva_fecha + "'  where tld_ttr_id=19 and tld_ref_id=" + id + "  ";
                    resultado = comm.ExecuteNonQuery();
                    comm.Parameters.Clear();
                    break;
                case 20:  //ND Bancaria
                    if (tipo_operacion != "2")
                    {
                        comm.Parameters.Clear();
                        comm.CommandText = " update tbl_nota_debito_bancaria set tndb_fecha='" + nueva_fecha + "', tndb_fecha_emision='" + nueva_fecha + "' where tndb_id =" + id + " ";
                        resultado = comm.ExecuteNonQuery();
                    }
                    comm.Parameters.Clear();

                    //comm.CommandText = "  update tbl_libro_diario set tdi_fecha='" + nueva_fecha + "', tdi_fecha_documento='" + nueva_fecha + "' where tdi_ttr_id " + filtro1 + " and tdi_ref_id =" + id + "  ";
                    //resultado = comm.ExecuteNonQuery();
                    //comm.Parameters.Clear();
                    res = UpdateFechaLibroD(nueva_fecha, filtro1, id, user.PaisID, fecha_anterior, conn, comm1);

                    break;
                case 21:  //NC Bancaria
                    if (tipo_operacion != "2")
                    {
                        comm.Parameters.Clear();
                        comm.CommandText = " update tbl_nota_credito_bancaria set tncb_fecha='" + nueva_fecha + "', tncb_fecha_emision='" + nueva_fecha + "' where tncb_id = " + id + " ";
                        resultado = comm.ExecuteNonQuery();
                    }
                    comm.Parameters.Clear();

                    //comm.CommandText = "  update tbl_libro_diario set tdi_fecha='" + nueva_fecha + "', tdi_fecha_documento='" + nueva_fecha + "' where tdi_ttr_id " + filtro1 + " and tdi_ref_id = " + id + "  ";
                    //resultado = comm.ExecuteNonQuery();
                    //comm.Parameters.Clear();
                    res = UpdateFechaLibroD(nueva_fecha, filtro1, id, user.PaisID, fecha_anterior, conn, comm1);

                    break;
                case 22: // RECIBO CORTE
                    id_movimiento = 0;
                    int estado_conciliado = 0;
                    comm.Parameters.Clear();
                    comm.CommandText = " select tprc_tmb_id from tbl_recibo_pago_corte where tprc_tre_id=" + id + " and tprc_tipo_pago in (3,4)  ";
                    reader = comm.ExecuteReader();
                    rgb = null;
                    while (reader.Read())
                    {
                        rgb = new RE_GenericBean();
                        if (!reader.IsDBNull(0)) rgb.intC1 = int.Parse(reader.GetValue(0).ToString()); //id
                        movimiento.Add(rgb);
                    }
                    reader.Close();
                    comm.Parameters.Clear();
                    if (tipo_operacion != "2")
                    {
                        comm.CommandText = " update tbl_recibo_corte set trc_fecha='" + nueva_fecha + "', trc_fecha_emision='" + nueva_fecha + "' where trc_id=" + id + "  ";
                        resultado = comm.ExecuteNonQuery();
                    }
                    comm.Parameters.Clear();

                    //comm.CommandText = " update tbl_libro_diario set tdi_fecha='" + nueva_fecha + "', tdi_fecha_documento='" + nueva_fecha + "' where tdi_ttr_id " + filtro1 + " and tdi_ref_id=" + id + "    ";
                    //resultado = comm.ExecuteNonQuery();
                    //comm.Parameters.Clear();
                    res = UpdateFechaLibroD(nueva_fecha, filtro1, id, user.PaisID, fecha_anterior, conn, comm1);

                    if ((movimiento.Count != 0) && (mov == "True")) // si el recibo tiene movimiento bancario actualizo tambien.
                    {
                        foreach (RE_GenericBean arr in movimiento)
                        {
                            if (tipo_operacion != "2")
                            {
                                comm.Parameters.Clear();
                                comm.CommandText = " update tbl_movimiento_bancario_corte set tmbc_fecha='" + nueva_fecha + "', tmbc_fecha_emision='" + nueva_fecha + "' where tmbc_id = " + arr.intC1 + "   ";
                                resultado = comm.ExecuteNonQuery();
                            }
                            comm.Parameters.Clear();
                            corr = correlativo.ToString();
                            comm.CommandText = " INSERT INTO tbl_log_cambio_fecha(lc_usuario,lc_ttr_id,lc_fecha_docto,lc_nueva_fecha,lc_fecha_accion,lc_doc_id,lc_doc_pais,lc_doc_serie,lc_motivo,lc_operacion,lc_doc_correlativo) ";
                            comm.CommandText += " VALUES('" + user.ID + "',28,'" + fecha_anterior + "','" + nueva_fecha + "',now()," + arr.intC1 + "," + user.PaisID + ",'" + serie + "','" + motivo + "','" + operacion + "','" + corr + "') ";
                            resultado = comm.ExecuteNonQuery();
                            comm.Parameters.Clear();
                        }
                    }
                    break;
                case 23: //SOAS
                    if (tipo_operacion != "2")
                    {
                        comm.Parameters.Clear();
                        comm.CommandText = " update tbl_corte_proveedor set tcp_fecha_creacion='" + nueva_fecha + "'  where tcp_id=" + id + " ";
                        resultado = comm.ExecuteNonQuery();
                    }
                    comm.Parameters.Clear();

                    //comm.CommandText = " update tbl_libro_diario set tdi_fecha='" + nueva_fecha + "', tdi_fecha_documento='" + nueva_fecha + "' where tdi_ttr_id " + filtro1 + " and tdi_ref_id in (" + id + ")  ";
                    //resultado = comm.ExecuteNonQuery();
                    //comm.Parameters.Clear();
                    res = UpdateFechaLibroD(nueva_fecha, filtro1, id, user.PaisID, fecha_anterior, conn, comm1);

                    comm.CommandText = " update tbl_liquidaciones_detalle set tld_fecha='" + nueva_fecha + "' where tld_ttr_id " + filtro1 + " and tld_ref_id in (" + id + ")  ";
                    resultado = comm.ExecuteNonQuery();
                    comm.Parameters.Clear();
                    break;
                case 24: //LIQUIDACIONES.
                    if (tipo_operacion != "2")
                    {
                        comm.Parameters.Clear();
                        comm.CommandText = " update tbl_liquidaciones set tli_fecha='" + nueva_fecha + "'  where tli_id=" + id + " ";
                        resultado = comm.ExecuteNonQuery();
                    }
                    comm.Parameters.Clear();

                    //comm.CommandText = " update tbl_libro_diario set tdi_fecha='" + nueva_fecha + "', tdi_fecha_documento='" + nueva_fecha + "' where tdi_ttr_id " + filtro1 + " and tdi_ref_id in (" + id + ")  ";
                    //resultado = comm.ExecuteNonQuery();
                    //comm.Parameters.Clear();
                    res = UpdateFechaLibroD(nueva_fecha, filtro1, id, user.PaisID, fecha_anterior, conn, comm1);

                    break;
                case 28: //Depositos SOA
                    if (tipo_operacion != "2")
                    {
                        comm.Parameters.Clear();
                        comm.CommandText = " update tbl_movimiento_bancario_corte set tmbc_fecha='" + nueva_fecha + "', tmbc_fecha_emision='" + nueva_fecha + "' where tmbc_id =" + id + "  ";
                        resultado = comm.ExecuteNonQuery();
                    }
                    comm.Parameters.Clear();

                    //comm.CommandText = " update tbl_libro_diario set tdi_fecha='" + nueva_fecha + "', tdi_fecha_documento='" + nueva_fecha + "' where tdi_ttr_id " + filtro1 + " and tdi_ref_id =" + id + " ";
                    //resultado = comm.ExecuteNonQuery();
                    //comm.Parameters.Clear();
                    res = UpdateFechaLibroD(nueva_fecha, filtro1, id, user.PaisID, fecha_anterior, conn, comm1);

                    break;
                case 31: //Nota de a nd credito proveedor
                    if (tipo_operacion != "2")
                    {
                        comm.CommandText = " update tbl_nota_credito set tnc_fecha='" + nueva_fecha + "', tnc_fecha_emision='" + nueva_fecha + "', tnc_fecha_libro_compras='" + nueva_fecha + "' where tnc_id =" + id + " ";
                        resultado = comm.ExecuteNonQuery();
                    }
                    comm.Parameters.Clear();

                    //comm.CommandText = " update tbl_libro_diario set tdi_fecha='" + nueva_fecha + "', tdi_fecha_documento='" + nueva_fecha + "' where tdi_ttr_id " + filtro1 + " and tdi_ref_id = " + id + "  ";
                    //resultado = comm.ExecuteNonQuery();
                    //comm.Parameters.Clear();
                    res = UpdateFechaLibroD(nueva_fecha, filtro1, id, user.PaisID, fecha_anterior, conn, comm1);

                    //si tiene corte
                    comm.CommandText = " select b.tcp_id from tbl_corte_proveedor_detalle a INNER JOIN tbl_corte_proveedor b ON a.tcpd_tcp_id=b.tcp_id where a.tcpd_docto_id=" + id + " and tcpd_str_id in (3,12,31) ";
                    reader = comm.ExecuteReader();
                    if (reader.Read())
                    {
                        corte = int.Parse(reader.GetValue(0).ToString());
                    }
                    reader.Close();
                    comm.Parameters.Clear();
                    if ((corte != 0) && (tipo_operacion != "2")) // si tiene corte
                    {
                        //actualizo la fecha del detalle del corte
                        comm.CommandText = " update tbl_corte_proveedor_detalle set tcpd_fecha_docto ='" + nueva_fecha + "' where tcpd_tcp_id = " + corte + " and tcpd_docto_id =" + id + " and tcpd_str_id in (3,12,31)  ";
                        resultado = comm.ExecuteNonQuery();
                        comm.Parameters.Clear();
                    }
                    break;

            }

            corr = correlativo.ToString();          
            comm1.Parameters.Clear();
            comm1.CommandText = " INSERT INTO tbl_log_cambio_fecha(lc_usuario,lc_ttr_id,lc_fecha_docto,lc_nueva_fecha,lc_fecha_accion,lc_doc_id,lc_doc_pais,lc_doc_serie,lc_motivo,lc_operacion,lc_doc_correlativo) ";
            comm1.CommandText += " VALUES('" + user.ID + "'," + tipo + ",'" + fecha_anterior + "','" + nueva_fecha + "',now()," + id + "," + user.PaisID + ",'" + serie + "','" + motivo + "','" + operacion + "','" + corr + "') ";
            resultado = comm1.ExecuteNonQuery();
            comm1.Parameters.Clear();          

            if (2 == 1)
            {
                transaction.Rollback();
            }
            else {
                transaction.Commit();
            }

            DB.CloseObj_insert(comm, conn);

 
        }
        catch (Exception e)
        {
            transaction.Rollback();
            log4net ErrLog = new log4net();
            ErrLog.ErrorLog(e.Message);
            return -100;
        }
        return resultado;
    }



}