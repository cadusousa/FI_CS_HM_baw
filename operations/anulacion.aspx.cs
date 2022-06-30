
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

public partial class operations_anulacion : System.Web.UI.Page
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
        }
        if (lb_tipo_doc.SelectedValue == "9")
        {
            lb_id.Visible = true;
            tb_id.Visible = true;
        }
        else
        {
            lb_id.Visible = false;
            tb_id.Visible = false;
        }
        if (!Page.IsPostBack)
        {
            arr = null;
            arr = (ArrayList)DB.getMonedasbyPais(user.PaisID, user.contaID);
            item = null;
            foreach (RE_GenericBean rgb in arr)
            {
                item = new ListItem(rgb.strC1, rgb.intC1.ToString());
                lb_moneda.Items.Add(item);
            }
            //Definir Moneda Inicial*************************************
            int moneda_inicial = 0;
            moneda_inicial = DB.Get_Moneda_Inicial_By_Empresa(user, 1);
            lb_moneda.SelectedValue = moneda_inicial.ToString();
        }
    }

    protected void lb_bancos_SelectedIndexChanged(object sender, EventArgs e)
    {
        ListItem item = null;
        if (lb_tipo_doc.SelectedValue.ToString() != "29")
        {
            CheckBox chk_cortes;
            UsuarioBean user;
            user = (UsuarioBean)Session["usuario"];
            lb_cuentas_bancarias.Items.Clear();
            item = new ListItem("Seleccione...", " ");
            item.Selected = true;
            lb_cuentas_bancarias.Items.Add(item);
            ArrayList arrCtas = (ArrayList)DB.getCuentasBancariasXpais(int.Parse(lb_bancos.SelectedValue), user.PaisID, 0);
            foreach (RE_GenericBean rgb in arrCtas)
            {
                item = new ListItem(rgb.strC1, rgb.strC1);
                lb_cuentas_bancarias.Items.Add(item);
            }
        }
        else if (lb_tipo_doc.SelectedValue.ToString() == "29")
        {
            lb_cuentas_bancarias.Items.Clear();
            item = new ListItem("Seleccione...", "0");
            lb_cuentas_bancarias.Items.Add(item);
            ArrayList arrCtas = (ArrayList)DB.getCuentasBancarias_comisiones(int.Parse(lb_bancos.SelectedValue), user);
            foreach (RE_GenericBean rgb in arrCtas)
            {
                item = new ListItem(rgb.strC1, rgb.strC1);
                lb_cuentas_bancarias.Items.Add(item);
            }
        }
    }

    protected void bt_buscar_Click(object sender, EventArgs e)
    {
        user = (UsuarioBean)Session["usuario"];
        ArrayList Arr = null;
        string where = "";
        if (tb_codigo.Text.Equals("") && tb_nombreb.Text.Equals("") && tb_nitb.Text.Equals(""))
        {
            WebMsgBox.Show("Debe ingresar al menos un criterio de busqueda");
            return;
        }
        if (!tb_nombreb.Text.Trim().Equals("") && tb_nombreb.Text != null) where += " upper(rtrim(a.nombre_cliente)) like '%" + tb_nombreb.Text.Trim().ToUpper() + "%'";
        if (!tb_codigo.Text.Trim().Equals("") && tb_codigo.Text != null)
            if (!where.Equals("")) where += " and a.id_cliente=" + tb_codigo.Text; else where += "a.id_cliente=" + tb_codigo.Text;
        if (!tb_nitb.Text.Trim().Equals("") && tb_nitb.Text != null)
            if (!where.Equals("")) where += " and a.codigo_tributario='" + tb_nitb.Text + "'"; else where += "a.codigo_tributario='" + tb_nitb.Text + "'";
        Arr = (ArrayList)DB.getClientes(where, user, ""); //Proveedor
        dt = (DataTable)Utility.fillGridView("cliente", Arr);


        ViewState["proveedordt"] = dt;
        gv_proveedor.DataSource = dt;
        gv_proveedor.DataBind();
        mpeSeleccion.Show();
    }

    protected void gv_proveedor_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        DataTable dt1;
        dt1 = (DataTable)ViewState["proveedordt"];
        gv_proveedor.DataSource = dt1;
        gv_proveedor.PageIndex = e.NewPageIndex;
        gv_proveedor.DataBind();
        mpeSeleccion.Show();
    }
    protected void gv_proveedor_SelectedIndexChanged(object sender, EventArgs e)
    {
        GridViewRow row = gv_proveedor.SelectedRow;
 
        tb_agenteID.Text = Page.Server.HtmlDecode(row.Cells[1].Text);
        tb_agentenombre.Text = Page.Server.HtmlDecode(row.Cells[2].Text);



    }
    protected void gv_proveedor_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            user = (UsuarioBean)Session["usuario"];
            ArrayList Arr = null;
            string where = "";
        }
    }

    protected void bt_aceptar_Click(object sender, EventArgs e)
    {
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

        gv_detalle.DataSource = dt;
        gv_detalle.DataBind();

        string sql = "";

        if (tranID == 1)
        {
            //sql = "select tfa_id, tfa_serie, tfa_correlativo, tfa_nombre, round(cast(tfa_total as numeric),2), tfa_moneda,to_char(tfa_fecha_emision, 'yyyy/mm/dd') from tbl_facturacion where tfa_id>0 and tfa_ted_id<>3 and tfa_pai_id=" + user.PaisID + " and tfa_moneda =" + user.Moneda + " and tfa_conta_id =" + contaID;
            sql = "select tfa_id, tfa_serie, tfa_correlativo, tfa_nombre, round(cast(tfa_total as numeric),2), tfa_moneda,to_char(tfa_fecha_emision, 'yyyy/mm/dd') from tbl_facturacion where tfa_id>0 and tfa_ted_id<>3 and tfa_pai_id=" + user.PaisID + " and tfa_moneda =" + lb_moneda.SelectedValue + " and tfa_conta_id =" + contaID;
            if (serie != null && !serie.Trim().Equals("")) sql += " and tfa_serie='" + serie.Trim() + "'";
            if (corr != null && !corr.Trim().Equals("")) sql += " and tfa_correlativo='" + corr.Trim() + "'";
            if (cliID > 0) sql += " and tfa_cli_id=" + cliID;
            dt = (DataTable)DB.getInfoAnular(sql);
            gv_resultado.DataSource = dt;
            gv_resultado.DataBind();
        }
        else if (tranID == 2)
        {
            //sql = "select tre_id, tre_serie, tre_correlativo, tre_cli_nombre, round(cast(tre_monto as numeric),2), tre_moneda_id,to_char(tre_fecha, 'yyyy/mm/dd') from tbl_recibo where tre_id>0 and tre_ted_id<>3 and tre_pai_id=" + user.PaisID + " and tre_moneda_id =" + user.Moneda + " and tre_tcon_id =" + contaID;
            sql = "select tre_id, tre_serie, tre_correlativo, tre_cli_nombre, round(cast(tre_monto as numeric),2), tre_moneda_id,to_char(tre_fecha, 'yyyy/mm/dd') from tbl_recibo where tre_id>0 and tre_ted_id<>3 and tre_pai_id=" + user.PaisID + " and tre_moneda_id =" + lb_moneda.SelectedValue + " and tre_tcon_id =" + contaID;
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
                sql = "select tmb_id, tmb_tbc_cuenta_bancaria, tmb_referencia, tba_nombre, round(cast(tmb_monto as numeric),2), tmb_mon_id,to_char(tmb_fecha, 'yyyy/mm/dd') from tbl_banco, tbl_movimiento_bancario where tmb_tba_id=tba_id and tmb_ted_id<>3 and tmb_id>0  and tmb_mon_id =  (select tbc_mon_id from tbl_banco_cuenta where tbc_cuenta_bancaria = '" + cuenta.Trim() + "') ";
            if (corr != null && !corr.Trim().Equals("")) sql += " and tmb_referencia='" + corr.Trim() + "'";
            if (cuenta != null && !cuenta.Trim().Equals("") && !cuenta.Trim().Equals("0")) sql += " and tmb_tbc_cuenta_bancaria='" + cuenta.Trim() + "'";
            if (banco != null && !banco.Equals(0)) sql += " and tmb_tba_id=" + banco;
            string sql2 ="";//uniendo los depositos que estan en otra modeda que no sea la del banco y su tipo de pago con recibo pago sea 3 o 4 y no tenga partida de creacion.
                sql2 = " select a.tmb_id, a.tmb_tbc_cuenta_bancaria, a.tmb_referencia, tba_nombre, round(cast(a.tmb_monto as numeric),2), a.tmb_mon_id,to_char(tmb_fecha, 'yyyy/mm/dd')  ";
                sql2 += " from tbl_banco, tbl_movimiento_bancario a where a.tmb_tba_id=tba_id and a.tmb_ted_id<>3 and a.tmb_id>0 and a.tmb_id in  (select b.tpr_tmb_id from tbl_recibo_pago b where b.tpr_tipo_pago in (3,4) and b.tpr_tmb_id=a.tmb_id ) ";
                sql2 += " and a.tmb_id not in (select c.tdi_ref_id from tbl_libro_diario c where c.tdi_ref_id=a.tmb_id and c.tdi_ttr_id = 17) ";
                sql2 += " and a.tmb_mon_id <> (select tbc_mon_id from tbl_banco_cuenta where tbc_cuenta_bancaria = '" + cuenta.Trim() + "') ";
                if (corr != null && !corr.Trim().Equals("")) sql2 += " and a.tmb_referencia='" + corr.Trim() + "'";
                if (cuenta != null && !cuenta.Trim().Equals("") && !cuenta.Trim().Equals("0")) sql2 += " and a.tmb_tbc_cuenta_bancaria='" + cuenta.Trim() + "'";
                if (banco != null && !banco.Equals(0)) sql2 += " and a.tmb_tba_id=" + banco;
                sql = sql + " union all  " + sql2;
            dt = (DataTable)DB.getInfoAnular(sql);
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
                sql = "select tmbc_id, tmbc_tbc_cuenta_bancaria, tmbc_referencia, tba_nombre, round(cast(tmbc_monto as numeric),2), tmbc_mon_id,to_char(tmbc_fecha, 'yyyy/mm/dd') from tbl_banco, tbl_movimiento_bancario_corte where tmbc_tba_id=tba_id and tmbc_ted_id<>3 and tmbc_id>0  and tmbc_mon_id =  (select tbc_mon_id from tbl_banco_cuenta where tbc_cuenta_bancaria = '" + cuenta.Trim() + "') ";
                if (corr != null && !corr.Trim().Equals("")) sql += " and tmbc_referencia='" + corr.Trim() + "'";
                if (cuenta != null && !cuenta.Trim().Equals("") && !cuenta.Trim().Equals("0")) sql += " and tmbc_tbc_cuenta_bancaria='" + cuenta.Trim() + "'";
                if (banco != null && !banco.Equals(0)) sql += " and tmbc_tba_id=" + banco;
                dt = (DataTable)DB.getInfoAnular(sql);
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
            //sql = "select tnc_id, tnc_serie, tnc_correlativo, tnc_nombre, round(cast(tnc_monto as numeric),2), tnc_mon_id,to_char(tnc_fecha, 'yyyy/mm/dd') from tbl_nota_credito where tnc_ttr_id=" + tranID + " and tnc_ted_id<>3 and tnc_pai_id=" + user.PaisID + " and tnc_mon_id =" + user.Moneda + " and tnc_tcon_id =" + contaID;
            sql = "select tnc_id, tnc_serie, tnc_correlativo, tnc_nombre, round(cast(tnc_monto as numeric),2), tnc_mon_id,to_char(tnc_fecha, 'yyyy/mm/dd') from tbl_nota_credito where tnc_ttr_id=" + tranID + " and tnc_ted_id<>3 and tnc_pai_id=" + user.PaisID + " and tnc_mon_id =" + lb_moneda.SelectedValue + " and tnc_tcon_id =" + contaID;
            if (serie != null && !serie.Trim().Equals("")) sql += " and tnc_serie='" + serie.Trim() + "'";
            if (corr != null && !corr.Trim().Equals("")) sql += " and tnc_correlativo='" + corr.Trim() + "'";
            if (cliID > 0) sql += " and tnc_cli_id=" + cliID;
            dt = (DataTable)DB.getInfoAnular(sql);
            gv_resultado.DataSource = dt;
            gv_resultado.DataBind();
        }
        else if (tranID == 4)
        {
            //sql = "select tnd_id, tnd_serie, tnd_correlativo, tnd_nombre, round(cast(tnd_total as numeric),2), tnd_moneda,to_char(tnd_fecha_emision, 'yyyy/mm/dd') from tbl_nota_debito where tnd_id>0 and tnd_ted_id<>3 and tnd_pai_id=" + user.PaisID + " and tnd_moneda =" + user.Moneda + " and tnd_tcon_id =" + contaID;
            sql = "select tnd_id, tnd_serie, tnd_correlativo, tnd_nombre, round(cast(tnd_total as numeric),2), tnd_moneda,to_char(tnd_fecha_emision, 'yyyy/mm/dd') from tbl_nota_debito where tnd_id>0 and tnd_ted_id<>3 and tnd_pai_id=" + user.PaisID + " and tnd_moneda =" + lb_moneda.SelectedValue + " and tnd_tcon_id =" + contaID;
            if (serie != null && !serie.Trim().Equals("")) sql += " and tnd_serie='" + serie.Trim() + "'";
            if (corr != null && !corr.Trim().Equals("")) sql += " and tnd_correlativo='" + corr.Trim() + "'";
            if (cliID > 0) sql += " and tnd_cli_id=" + cliID;
            dt = (DataTable)DB.getInfoAnular(sql);
            gv_resultado.DataSource = dt;
            gv_resultado.DataBind();
        }
        else if (tranID == 5)
        {
            //sql = "select tpr_prov_id, tpr_serie, tpr_correlativo, tpr_nombre, round(cast(tpr_valor as numeric),2), tpr_mon_id,to_char(tpr_fecha_creacion, 'yyyy/mm/dd') from tbl_provisiones where tpr_prov_id>0 and tpr_ted_id<>3 and tpr_pai_id=" + user.PaisID + " and tpr_mon_id =" + user.Moneda + " and tpr_tcon_id =" + contaID;
            sql = "select tpr_prov_id, tpr_serie, tpr_correlativo, tpr_nombre, round(cast(tpr_valor as numeric),2), tpr_mon_id,to_char(tpr_fecha_creacion, 'yyyy/mm/dd') from tbl_provisiones where tpr_prov_id>0 and tpr_ted_id<>3 and tpr_pai_id=" + user.PaisID + " and tpr_mon_id =" + lb_moneda.SelectedValue + " and tpr_tcon_id =" + contaID;
            if (serie != null && !serie.Trim().Equals("")) sql += " and tpr_serie='" + serie.Trim() + "'";
            if (corr != null && !corr.Trim().Equals("")) sql += " and tpr_correlativo='" + corr.Trim() + "'";
            if (cliID > 0) sql += " and tpr_proveedor_id=" + cliID;
            dt = (DataTable)DB.getInfoAnular(sql);
            gv_resultado.DataSource = dt;
            gv_resultado.DataBind();
        }
        else if (tranID == 9)
        {
            //sql = "select trp_id, trp_serie, trp_referencia, tpr_nombre, round(cast(trp_monto as numeric),2), trp_mon_id,to_char(trp_fecha_generacion, 'yyyy/mm/dd') from tbl_retencion_provision, tbl_provisiones where trp_tpr_prov_id=tpr_prov_id and trp_id>0 and trp_ted_id<>3 and trp_pai_id=" + user.PaisID + " and tpr_mon_id =" + user.Moneda + " and tpr_tcon_id =" + contaID;
            sql = "select trp_id, trp_serie, trp_referencia, tpr_nombre, round(cast(trp_monto as numeric),2), trp_mon_id,to_char(trp_fecha_generacion, 'yyyy/mm/dd') from tbl_retencion_provision, tbl_provisiones where trp_tpr_prov_id=tpr_prov_id and trp_id>0 and trp_ted_id<>3 and trp_pai_id=" + user.PaisID + " and tpr_mon_id =" + lb_moneda.SelectedValue + " and tpr_tcon_id =" + contaID;
            if (serie != null && !serie.Trim().Equals("")) sql += " and trp_serie='" + serie.Trim() + "'";
            if (corr != null && !corr.Trim().Equals("")) sql += " and trp_referencia='" + corr.Trim() + "'";
            if (cliID > 0) sql += " and tpr_proveedor_id=" + cliID;
            if (!tb_id.Text.Trim().Equals("")) sql += " and trp_id=" + int.Parse(tb_id.Text.Trim());
            dt = (DataTable)DB.getInfoAnular(sql);
            gv_resultado.DataSource = dt;
            gv_resultado.DataBind();
        }
        else if (tranID == 6)
        {
            sql = "select tcg_id, tcg_cuenta, tcg_numero, tcg_acreditado, round(cast(tcg_valor as numeric),2), tcg_ttm_id,to_char(tcg_fecha, 'yyyy/mm/dd') from tbl_cheques_generados where tcg_ttr_id in (select ttt_id from tbl_tipo_transaccion where ttt_template in ('pop_cheques.aspx', 'anticipos.aspx')) and tcg_ted_id<>3 and tcg_pai_id=" + user.PaisID + " and tcg_tcon_id =" + contaID + " and tcg_ttm_id in (select tbc_mon_id from tbl_banco_cuenta where tbc_cuenta_bancaria = '" + cuenta.Trim() + "')  ";  
            //sql = "select tcg_id, tcg_cuenta, tcg_numero, tcg_acreditado, round(cast(tcg_valor as numeric),2), tcg_ttm_id,to_char(tcg_fecha, 'yyyy/mm/dd') from tbl_cheques_generados where tcg_ttr_id in (select ttt_id from tbl_tipo_transaccion where ttt_template in ('pop_cheques.aspx', 'anticipos.aspx')) and tcg_ted_id<>3 and tcg_pai_id=" + user.PaisID + " and tcg_tcon_id =" + contaID + " and tcg_ttm_id in (" + lb_moneda.SelectedValue + ")  ";
                if (corr != null && !corr.Trim().Equals("")) sql += " and tcg_numero='" + corr.Trim() + "'";
                if (cuenta != null && !cuenta.Trim().Equals("") && !cuenta.Trim().Equals("0")) sql += " and tcg_cuenta='" + cuenta.Trim() + "'";
                if (banco != null && !banco.Equals(0)) sql += " and tcg_cuenta in (select tbc_cuenta_bancaria from tbl_banco_cuenta where tbc_tba_id=" + banco + ")";
                if (cliID > 0) sql += " and tcg_cli_id=" + cliID;
            dt = (DataTable)DB.getInfoAnular(sql);
            gv_resultado.DataSource = dt;
            gv_resultado.DataBind();
        }
        else if (tranID == 19)
        {
            sql = "select tcg_id, tcg_cuenta, tcg_no_transferencia, tcg_acreditado, round(cast(tcg_valor as numeric),2), tcg_ttm_id,to_char(tcg_fecha, 'yyyy/mm/dd') from tbl_cheques_generados where tcg_ttr_id in (select ttt_id from tbl_tipo_transaccion where ttt_template in ('pop_transferencias_ag.aspx', 'transferencia_anticipos.aspx')) and tcg_ted_id<>3 and tcg_pai_id=" + user.PaisID + " and tcg_tcon_id =" + contaID + " and tcg_ttm_id in (select tbc_mon_id from tbl_banco_cuenta where tbc_cuenta_bancaria = '" + cuenta.Trim() + "')";
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
            sql = "select tndb_id, tndb_tbc_cuenta_bancaria, tndb_credito_no, '', round(cast(tndb_valor as numeric),2), tndb_tbc_mon_id,to_char(tndb_fecha, 'yyyy/mm/dd') from tbl_nota_debito_bancaria where tndb_ted_id<>3 and tndb_pai_id=" + user.PaisID + " and tndb_tbc_mon_id = (select tbc_mon_id from tbl_banco_cuenta where tbc_cuenta_bancaria = '" + cuenta.Trim() + "') ";
            if (corr != null && !corr.Trim().Equals("")) sql += " and tndb_credito_no='" + corr.Trim() + "'";
            if (cuenta != null && !cuenta.Trim().Equals("") && !cuenta.Trim().Equals("0")) sql += " and tndb_tbc_cuenta_bancaria='" + cuenta.Trim() + "'";
            if (banco != null && !banco.Equals(0)) sql += " and tndb_tbc_cuenta_bancaria in (select tbc_cuenta_bancaria from tbl_banco_cuenta where tbc_tba_id=" + banco + ")";
            dt = (DataTable)DB.getInfoAnular(sql);
            gv_resultado.DataSource = dt;
            gv_resultado.DataBind();
        }
        else if (tranID == 21)
        {
            sql = "select tncb_id, tncb_tbc_cuenta_bancaria, tncb_credito_no, '', round(cast(tncb_valor as numeric),2), tncb_tbc_mon_id,to_char(tncb_fecha, 'yyyy/mm/dd') from tbl_nota_credito_bancaria where tncb_ted_id<>3 and tncb_pai_id=" + user.PaisID + " and tncb_tbc_mon_id = (select tbc_mon_id from tbl_banco_cuenta where tbc_cuenta_bancaria = '" + cuenta.Trim() + "') ";
            if (corr != null && !corr.Trim().Equals("")) sql += " and tncb_credito_no='" + corr.Trim() + "'";
            if (cuenta != null && !cuenta.Trim().Equals("") && !cuenta.Trim().Equals("0")) sql += " and tncb_tbc_cuenta_bancaria='" + cuenta.Trim() + "'";
            if (banco != null && !banco.Equals(0)) sql += " and tncb_tbc_cuenta_bancaria in (select tbc_cuenta_bancaria from tbl_banco_cuenta where tbc_tba_id=" + banco + ")";
            dt = (DataTable)DB.getInfoAnular(sql);
            gv_resultado.DataSource = dt;
            gv_resultado.DataBind();
        }
        else if (tranID == 22)
        {
            //sql = "select trc_id, trc_serie, trc_correlativo, trc_cli_nombre, round(cast(trc_monto as numeric),2), trc_moneda_id,to_char(trc_fecha, 'yyyy/mm/dd') from tbl_recibo_corte where trc_id>0 and trc_ted_id<>3 and trc_pai_id=" + user.PaisID + " and trc_moneda_id =" + user.Moneda + " and trc_tcon_id =" + contaID;
            sql = "select trc_id, trc_serie, trc_correlativo, trc_cli_nombre, round(cast(trc_monto as numeric),2), trc_moneda_id,to_char(trc_fecha, 'yyyy/mm/dd') from tbl_recibo_corte where trc_id>0 and trc_ted_id<>3 and trc_pai_id=" + user.PaisID + " and trc_moneda_id =" + lb_moneda.SelectedValue + " and trc_tcon_id =" + contaID;
            if (serie != null && !serie.Trim().Equals("")) sql += " and trc_serie='" + serie.Trim() + "'";
            if (corr != null && !corr.Trim().Equals("")) sql += " and trc_correlativo='" + corr.Trim() + "'";
            if (cliID > 0) sql += " and trc_cli_id=" + cliID;
            dt = (DataTable)DB.getInfoAnular(sql);
            gv_resultado.DataSource = dt;
            gv_resultado.DataBind();
        }
        else if (tranID == 23) //Corte Proveedor, no se encuentra en la tabla Sys_referencia
        {
            //sql = "select tcp_id, tcp_serie, tcp_correlativo, '', round(cast(tcp_total as numeric),2), tcp_mon_id,to_char(tcp_fecha_creacion, 'yyyy/mm/dd'), tcp_tpi_id, tcp_cli_id from tbl_corte_proveedor where tcp_id>0 and tcp_ted_id<>3 and tcp_pai_id=" + user.PaisID + " and tcp_mon_id =" + user.Moneda + " and tcp_conta_id =" + contaID;
            sql = "select tcp_id, tcp_serie, tcp_correlativo, '', round(cast(tcp_total as numeric),2), tcp_mon_id,to_char(tcp_fecha_creacion, 'yyyy/mm/dd'), tcp_tpi_id, tcp_cli_id from tbl_corte_proveedor where tcp_id>0 and tcp_ted_id<>3 and tcp_pai_id=" + user.PaisID + " and tcp_mon_id =" + lb_moneda.SelectedValue + " and tcp_conta_id =" + contaID;
            if (serie != null && !serie.Trim().Equals("")) sql += " and tcp_serie='" + serie.Trim() + "'";
            if (corr != null && !corr.Trim().Equals("")) sql += " and tcp_correlativo='" + corr.Trim() + "'";
            if (cliID > 0) sql += " and tcp_cli_id=" + cliID;
            dt = (DataTable)DB.getInfoAnular(sql);
            gv_resultado.DataSource = dt;
            gv_resultado.DataBind();
        }
        if (tranID == 14)
        {
            //sql = "select tfa_id, tfa_serie, tfa_correlativo, tfa_nombre, round(cast(tfa_total as numeric),2), tfa_moneda,to_char(tfa_fecha_emision, 'yyyy/mm/dd') from tbl_facturacion_proforma where tfa_id>0 and tfa_ted_id in (1) and tfa_pai_id=" + user.PaisID + " and tfa_moneda =" + user.Moneda + " and tfa_conta_id =" + contaID;
            sql = "select tfa_id, tfa_serie, tfa_correlativo, tfa_nombre, round(cast(tfa_total as numeric),2), tfa_moneda,to_char(tfa_fecha_emision, 'yyyy/mm/dd') from tbl_facturacion_proforma where tfa_id>0 and tfa_ted_id in (1) and tfa_pai_id=" + user.PaisID + " and tfa_moneda =" + lb_moneda.SelectedValue + " and tfa_conta_id =" + contaID;
            if (serie != null && !serie.Trim().Equals("")) sql += " and tfa_serie='" + serie.Trim() + "'";
            if (corr != null && !corr.Trim().Equals("")) sql += " and tfa_correlativo='" + corr.Trim() + "'";
            if (cliID > 0) sql += " and tfa_cli_id=" + cliID;
            dt = (DataTable)DB.getInfoAnular(sql);
            gv_resultado.DataSource = dt;
            gv_resultado.DataBind();
        }
        if (tranID == 15)
        {
            //sql = "select tac_id, tac_serie, tac_correlativo,substring(tac_observaciones from 0 for 50),round(cast(tac_cargo as numeric),2), tac_mon_id,to_char(tac_fecha_creacion, 'yyyy/mm/dd') from tbl_ajuste_contable where tac_id >0 and tac_pai_id =" + user.PaisID + " and tac_mon_id = " + user.Moneda + " and tac_ted_id not in (3) and tac_conta_id = " + contaID;
            sql = "select tac_id, tac_serie, tac_correlativo,substring(tac_observaciones from 0 for 50),round(cast(tac_cargo as numeric),2), tac_mon_id,to_char(tac_fecha_creacion, 'yyyy/mm/dd') from tbl_ajuste_contable where tac_id >0 and tac_pai_id =" + user.PaisID + " and tac_mon_id = " + lb_moneda.SelectedValue + " and tac_ted_id not in (3) and tac_conta_id = " + contaID;
            if (serie != null && !serie.Trim().Equals("")) sql += " and tac_serie='" + serie.Trim() + "'";
            if (corr != null && !corr.Trim().Equals("")) sql += " and tac_correlativo='" + corr.Trim() + "'";
            //if (cliID > 0) sql += " and tfa_cli_id=" + cliID;
            dt = (DataTable)DB.getInfoAnular(sql);
            gv_resultado.DataSource = dt;
            gv_resultado.DataBind();
        }
        else if (tranID == 29)
        {
            sql = "select tchc_id, tchc_cuenta, tchc_numero, tchc_acreditado, round(cast(tchc_monto as numeric),2), tchc_moneda_id,to_char(tchc_fecha_emision, 'yyyy/mm/dd') from tbl_cheques_comisiones where   tchc_ted_id<>3 and tchc_pais_id=" + user.PaisID + " and tchc_conta_id =" + contaID + " and tchc_moneda_id  in (select tbcc_mon_id from tbl_banco_cuenta_comision where tbcc_cuenta_bancaria = '" + cuenta.Trim() + "')  ";
            if (corr != null && !corr.Trim().Equals("")) sql += " and tchc_numero='" + corr.Trim() + "'";
            if (cuenta != null && !cuenta.Trim().Equals("") && !cuenta.Trim().Equals("0")) sql += " and tchc_cuenta='" + cuenta.Trim() + "'";
            if (banco != null && !banco.Equals(0)) sql += " and tchc_cuenta in (select tbcc_cuenta_bancaria from tbl_banco_cuenta_comision where tbcc_tbac_id=" + banco + ")";
            if (cliID > 0) sql += " and tchc_id=" + cliID;
            dt = (DataTable)DB.getInfoAnular(sql);
            gv_resultado.DataSource = dt;
            gv_resultado.DataBind();
        }
        else if (tranID == 30) //Corte Comision
        {
            sql = "select tcc_id, tcc_serie, tcc_correlativo, '', round(cast(tcc_total as numeric),2), tcc_moneda_id,to_char(tcc_fecha_creacion, 'yyyy/mm/dd'), tcc_vendedor_id from tbl_corte_comisiones where tcc_ted_id<>3 and tcc_pais_id=" + user.PaisID + " and tcc_moneda_id =" + user.Moneda + " and tcc_conta_id =" + contaID;
            if (serie != null && !serie.Trim().Equals("")) sql += " and tcc_serie='" + serie.Trim() + "'";
            if (corr != null && !corr.Trim().Equals("")) sql += " and tcc_correlativo='" + corr.Trim() + "'";
            if (cliID > 0) sql += " and tcc_vendedor_id=" + cliID;
            dt = (DataTable)DB.getInfoAnular(sql);
            gv_resultado.DataSource = dt;
            gv_resultado.DataBind();
        }
        tb_sql.Text = sql;
        //WebMsgBox.Show(sql); //Debug para ver el Query generado
    }


    protected void Anular(int tranID, int id, int ttrID_PADRE, int refID_PADRE)
    {
        int anulacion = 0;
        int EstadoDocumento = 9;
        int contaID = int.Parse(Session["Contabilidad"].ToString());
        user = (UsuarioBean)Session["usuario"];
        string motivo = "";
        string sql = "";
        if (tb_motivo.Text.Trim().Equals(""))
        {
            WebMsgBox.Show("Debe Ingresar el Motivo de la Anulacion");
            return;
        }
        if (tb_motivo.Text.Trim().Length < 15)
        {
            WebMsgBox.Show("Debe ingresar una razon mas descriptiva");
            return;
        }
        motivo = tb_motivo.Text;
        EstadoDocumento = DB.getEstadoDocumento(id, tranID);
            if (EstadoDocumento == 9)
            {
                WebMsgBox.Show("El documento no se puede anular porque esta Cortado Debe anular el Corte primero.");
                sql = "select tcp_id,'CORTE', tcp_serie,tcp_correlativo,tcp_total from tbl_corte_proveedor_detalle, tbl_corte_proveedor where tcpd_str_id=" + tranID + " and tcpd_docto_id=" + id + " and tcpd_tcp_id=tcp_id";
                dt = (DataTable)DB.getInfoAnular2(sql);
                gv_detalle.DataSource = dt;
                gv_detalle.DataBind();
                return;
            }
            else if (EstadoDocumento == 4)
            {
                sql = "select tcp_id,'CORTE', tcp_serie,tcp_correlativo,tcp_total from tbl_corte_proveedor_detalle, tbl_corte_proveedor where tcpd_str_id=" + tranID + " and tcpd_docto_id=" + id + " and tcpd_tcp_id=tcp_id";
                dt = (DataTable)DB.getInfoAnular2(sql);
                if (dt.Rows.Count > 0) 
                {
                    WebMsgBox.Show("El documento no se puede anular porque esta Cortado Debe anular el Corte primero.");
                    gv_detalle.DataSource = dt;
                    gv_detalle.DataBind();
                    return;
                }  
            }
        if (tranID == 1)
        {
            #region Validar Documento Electronico
            RE_GenericBean factdata = (RE_GenericBean)DB.getFacturaData(id);
            if ((factdata.strC50 == "1") || (factdata.strC50 == "2"))
            {
                WebMsgBox.Show("No se pueden Anular Facturas Electronicas, porfavor anule a traves de Nota de Credito");
                return;
            }
            #endregion
            #region Validar Transaccion Encadenada
            ArrayList Arr_Temp = null;
            string resultado_transaccion_encadenada = "";
            int documentos_con_amarre = 0;
            int documentos_con_periodo_bloqueado = 0;
            int estado_hijo = 0;
            string mensaje = "";
            string mensaje_documentos_asociados = "";
            string mensaje_documentos_periodo_cerrado = "";
            resultado_transaccion_encadenada = DB.Validar_Transaccion_Encadenada(tranID, id);
            if (resultado_transaccion_encadenada == "-100")
            {
                WebMsgBox.Show("Existio un error al tratar de Validar si la trasaccion estaba encadenada");
                return;
            }
            if ((resultado_transaccion_encadenada == "HIJO,PADRE") || (resultado_transaccion_encadenada == "PADRE,HIJO"))
            {
                #region Transaccion Doblemente Encadenada porque es Hija y Padre
                if ((ttrID_PADRE == 0) || (refID_PADRE == 0))//Esto Significa que se esta Anuldo desde Transaccion ella misma y no desde el Padre
                {
                    ArrayList Arr_Padre = DB.Get_Transaccion_Padre_By_Transaccion_Hija(tranID, id);
                    RE_GenericBean Bean_Padre = (RE_GenericBean)Arr_Padre[0];
                    PaisBean Empresa_Padre = (PaisBean)DB.getPais(int.Parse(Bean_Padre.strC4));
                    ArrayList Arr_Doc_Padre = null;
                    Arr_Doc_Padre = DB.getSerieCorrAndObsByDoc(int.Parse(Bean_Padre.strC2), int.Parse(Bean_Padre.strC3));

                    ArrayList Arr_Hija = DB.Get_Transaccion_Hija_By_Transaccion_Padre(tranID, id);
                    RE_GenericBean Bean_Hija = (RE_GenericBean)Arr_Hija[0];
                    PaisBean Empresa_Hija = (PaisBean)DB.getPais(int.Parse(Bean_Hija.strC4));
                    ArrayList Arr_Doc_Hijo = null;
                    Arr_Doc_Hijo = DB.getSerieCorrAndObsByDoc(int.Parse(Bean_Hija.strC2), int.Parse(Bean_Hija.strC3));

                    ArrayList Arr_Doc_Factura = null;
                    Arr_Doc_Factura = DB.getSerieCorrAndObsByDoc(tranID, id);

                    WebMsgBox.Show("Transaccion encadenada (FACTURA " + Arr_Doc_Factura[1].ToString() + "-" + Arr_Doc_Factura[2].ToString() + "), esta transaccion solo puede ser anulada al anular la Transaccion Padre.: PROVISION A LA ASEGURADORA Serie.: " + Arr_Doc_Padre[1].ToString() + " y Correlativo.: " + Arr_Doc_Padre[2].ToString() + ", al anular Transaccion Padre tambien se anulara Transaccion Hija.: PROVISION INTERCOMPANY Serie.: " + Arr_Doc_Hijo[1].ToString() + " y Correlativo.: " + Arr_Doc_Hijo[2].ToString() + " en la Empresa.: " + Empresa_Hija.Nombre_Sistema + " .");
                    return;
                }
                #endregion
            }
            else if ((resultado_transaccion_encadenada == "PADRE") || (resultado_transaccion_encadenada == "HIJO"))
            {
                #region Validar Hijo
                if (resultado_transaccion_encadenada == "HIJO")
                {
                    ArrayList Arr_Padre = DB.Get_Transaccion_Padre_By_Transaccion_Hija(tranID, id);
                    RE_GenericBean Bean_Padre = (RE_GenericBean)Arr_Padre[0];
                    PaisBean Empresa_Padre = (PaisBean)DB.getPais(int.Parse(Bean_Padre.strC4));
                    #region Validacion Intercompany
                    if (factdata.strC58 == "10")
                    {
                        if (Empresa_Padre.ID != user.PaisID)
                        {
                            Arr_Temp = DB.getSerieCorrAndObsByDoc(int.Parse(Bean_Padre.strC2), int.Parse(Bean_Padre.strC3));
                            WebMsgBox.Show("Transaccion encadenada, esta transaccion solo puede ser anulada desde la Empresa.: " + Empresa_Padre.Nombre_Sistema + " durante el dia de emision al anular la Transaccion Padre.: " + Arr_Temp[0].ToString() + " Serie.: " + Arr_Temp[1].ToString() + " y Correlativo.: " + Arr_Temp[2].ToString() + "");
                            return;
                        }
                    }
                    #endregion
                }
                #endregion
                #region Validar Padre
                if (resultado_transaccion_encadenada == "PADRE")
                {
                    ArrayList Arr_Transacciones_Hijas = (ArrayList)DB.Get_Transacciones_Encadenadas_Hijas(tranID, id);
                    if (Arr_Transacciones_Hijas == null)
                    {
                        WebMsgBox.Show("Existio un error al Tratar de Obtener las Transacciones Encadenadas Hijas");
                        return;
                    }
                    foreach (RE_GenericBean Hija in Arr_Transacciones_Hijas)
                    {
                        PaisBean Empresa_Hijo = (PaisBean)DB.getPais(int.Parse(Hija.strC4));
                        Arr_Temp = DB.getSerieCorrAndObsByDoc(int.Parse(Hija.strC2), int.Parse(Hija.strC3));
                        #region Validar Estado Documento
                        estado_hijo = DB.getEstadoDocumento(int.Parse(Hija.strC3), int.Parse(Hija.strC2));
                        if ((estado_hijo == 2) || (estado_hijo == 4) || (estado_hijo == 9))
                        {
                            documentos_con_amarre++;
                            mensaje_documentos_asociados += "* " + Arr_Temp[5].ToString() + " " + " Serie.: " + Arr_Temp[1].ToString() + " y Correlativo.: " + Arr_Temp[2].ToString() + " en la Empresa.: " + Empresa_Hijo.Nombre_Sistema + " \\n";
                        }
                        #endregion
                        #region Validar Bloqueo Periodo
                        RE_GenericBean bloqueo = null;
                        DateTime fecha_ultimo_bloqueo;
                        DateTime hoy;
                        DateTime fecha_doc = DateTime.Parse(DB.getFechaDocumento(int.Parse(Hija.strC3), int.Parse(Hija.strC2)));
                        bloqueo = (RE_GenericBean)DB.ObtengoPeriodoBloqueado(int.Parse(Hija.strC4));
                        fecha_ultimo_bloqueo = DateTime.Parse(bloqueo.strC1); //fecha del ultimo bloqueo    
                        hoy = DateTime.Today; //fecha del dia de hoy
                        int activo = bloqueo.intC3;
                        if ((fecha_doc <= fecha_ultimo_bloqueo) && (activo == 1))
                        {
                            documentos_con_periodo_bloqueado++;
                            mensaje_documentos_periodo_cerrado += "* " + Arr_Temp[5].ToString() + " " + " Serie.: " + Arr_Temp[1].ToString() + " y Correlativo.: " + Arr_Temp[2].ToString() + " en la Empresa.: " + Empresa_Hijo.Nombre_Sistema + " \\n";
                        }
                        #endregion
                    }
                    if ((documentos_con_amarre > 0)||(documentos_con_periodo_bloqueado>0))
                    {
                        #region Documento con Amarres o Bloqueos
                        if (documentos_con_amarre > 0)
                        {
                            #region Crear Mensaje Documentos con Amarre
                            mensaje_documentos_asociados = "-- Transacciones Hijas con documentos asociandos .: \\n" + mensaje_documentos_asociados;
                            #endregion
                        }
                        if (documentos_con_periodo_bloqueado > 0)
                        {
                            #region Crear Mensaje Documentos con Periodo Bloqueado
                            mensaje_documentos_periodo_cerrado = "-- Transacciones Hijas dentro de un periodo cerrado .: \\n" + mensaje_documentos_periodo_cerrado;
                            #endregion
                        }
                        Arr_Temp = DB.getSerieCorrAndObsByDoc(tranID, id);
                        mensaje = "No se puede Anular la Transaccion Padre  Serie.: " + Arr_Temp[1].ToString() + " y Correlativo.: " + Arr_Temp[2].ToString() + " por el siguiente motivo.: \\n";
                        mensaje += mensaje_documentos_asociados;
                        mensaje += mensaje_documentos_periodo_cerrado;
                        WebMsgBox.Show(mensaje);
                        return;
                        #endregion
                    }
                    else
                    {
                        #region Documento sin Amarres o Bloqueos se procede a Anular Hijos
                        foreach (RE_GenericBean Hija in Arr_Transacciones_Hijas)
                        {
                            Anular(int.Parse(Hija.strC2), int.Parse(Hija.strC3), 0, 0);
                            DB.Desactivar_Transaccion_Encadenada_Log(int.Parse(Hija.strC1), user);
                        }
                        #endregion
                    }
                }
                #endregion
            }
            #endregion
            anulacion = DB.AnulacionFactura(id, user, contaID);
            if (anulacion == -100)
            {
                WebMsgBox.Show("No se puede anular el Documento");
                return;
            }
        }
        else if (tranID == 2)
        {
            anulacion = DB.AnulacionRecibo(id, user, contaID);
            if (anulacion == -100)
            {
                WebMsgBox.Show("No se puede anular el Documento");
                return;
            }
           
        }
        else if (tranID == 17)
        {
            int Estado_Conciliado = 0;
            Estado_Conciliado = DB.Validar_Documento_Conciliado(id, 17);
            if (Estado_Conciliado == -100)
            {
                WebMsgBox.Show("Existion un error al tratar de validar si el documento esta conciliado o no");
                return;
            }
            else if (Estado_Conciliado == 13)
            {
                WebMsgBox.Show("No se Puede Anular un Deposito Conciliado");
                return;
            }
            else
            {
                int Estado_Conciliado_Circulacion = 0;
                Estado_Conciliado_Circulacion = DB.Validar_Documento_Conciliado_Circulacion(id, 17);
                if (Estado_Conciliado_Circulacion == -100)
                {
                    WebMsgBox.Show("Existion un error al tratar de validar si el documento esta conciliado o no.");
                    return;
                }
                else if (Estado_Conciliado_Circulacion > 0)
                {
                    WebMsgBox.Show("No se puede anular el Deposito porque esta en circulacion en un periodo ya conciliado, para anular primero debe botar la conciliacion respectiva.");
                    return;
                }
                else
                {
                    string recibo = DB.verifica_tipo_pago_RC(id);
                    anulacion = DB.AnulacionDepositos(id, user, contaID);
                    if ((recibo != "") && (anulacion != -100))
                    {
                        WebMsgBox.Show("El Deposito anulado, tenia un recibo asociado, el cual tambien debe de ser anulado, Favor proceder a realizar su anulacion respectiva, " + recibo);
                    }
                }
            }
        }
        else if (tranID == 28)
        {
            int Estado_Conciliado = 0;
            Estado_Conciliado = DB.Validar_Documento_Conciliado(id, 28);
            if (Estado_Conciliado == -100)
            {
                WebMsgBox.Show("Existion un error al tratar de validar si el documento esta conciliado o no");
                return;
            }
            else if (Estado_Conciliado == 13)
            {
                WebMsgBox.Show("No se Puede Anular un Deposito Conciliado");
                return;
            }
            else
            {
                int Estado_Conciliado_Circulacion = 0;
                Estado_Conciliado_Circulacion = DB.Validar_Documento_Conciliado_Circulacion(id, 28);
                if (Estado_Conciliado_Circulacion == -100)
                {
                    WebMsgBox.Show("Existion un error al tratar de validar si el documento esta conciliado o no.");
                    return;
                }
                else if (Estado_Conciliado_Circulacion > 0)
                {
                    WebMsgBox.Show("No se puede anular el Deposito porque esta en circulacion en un periodo ya conciliado, para anular primero debe botar la conciliacion respectiva.");
                    return;
                }
                else
                {
                    anulacion = DB.AnulacionDepositosSoa(id, user, contaID);
                }
            }
        }
        else if ((tranID == 3) || (tranID == 12) || (tranID == 18) || (tranID == 31))
        { 
            #region Validar Documento Electronico
            RE_GenericBean NCData = (RE_GenericBean)DB.getNotaCreditoData(id);
            if ((NCData.strC50 == "1") || (NCData.strC50 == "2"))
            {
                WebMsgBox.Show("No se pueden Anular Notas de Credito Electronicas");
                return;
            }
            #endregion
            anulacion = DB.AnulacionNC(id, user, contaID, tranID);
            if (anulacion == -100)
            {
                WebMsgBox.Show("No se puede anular el Documento");
                return;
            }
        }
        else if (tranID == 4)
        {

            RE_GenericBean factdata = (RE_GenericBean)DB.getNotaDebitoData(id);
            #region Validar Transaccion Encadenada
            ArrayList Arr_Temp = null;
            string resultado_transaccion_encadenada = "";
            int documentos_con_amarre = 0;
            int documentos_con_periodo_bloqueado = 0;
            int estado_hijo = 0;
            string mensaje = "";
            string mensaje_documentos_asociados = "";
            string mensaje_documentos_periodo_cerrado = "";
            resultado_transaccion_encadenada = DB.Validar_Transaccion_Encadenada(tranID, id);
            if (resultado_transaccion_encadenada == "-100")
            {
                WebMsgBox.Show("Existio un error al tratar de Validar si la trasaccion estaba encadenada");
                return;
            }
            if ((resultado_transaccion_encadenada == "HIJO,PADRE") || (resultado_transaccion_encadenada == "PADRE,HIJO"))
            {
                #region Transaccion Doblemente Encadenada porque es Hija y Padre
                if ((ttrID_PADRE == 0) || (refID_PADRE == 0))//Esto Significa que se esta Anuldo desde Transaccion ella misma y no desde el Padre
                {
                    ArrayList Arr_Padre = DB.Get_Transaccion_Padre_By_Transaccion_Hija(tranID, id);
                    RE_GenericBean Bean_Padre = (RE_GenericBean)Arr_Padre[0];
                    PaisBean Empresa_Padre = (PaisBean)DB.getPais(int.Parse(Bean_Padre.strC4));
                    ArrayList Arr_Doc_Padre = null;
                    Arr_Doc_Padre = DB.getSerieCorrAndObsByDoc(int.Parse(Bean_Padre.strC2), int.Parse(Bean_Padre.strC3));

                    ArrayList Arr_Hija = DB.Get_Transaccion_Hija_By_Transaccion_Padre(tranID, id);
                    RE_GenericBean Bean_Hija = (RE_GenericBean)Arr_Hija[0];
                    PaisBean Empresa_Hija = (PaisBean)DB.getPais(int.Parse(Bean_Hija.strC4));
                    ArrayList Arr_Doc_Hijo = null;
                    Arr_Doc_Hijo = DB.getSerieCorrAndObsByDoc(int.Parse(Bean_Hija.strC2), int.Parse(Bean_Hija.strC3));

                    ArrayList Arr_Doc_Factura = null;
                    Arr_Doc_Factura = DB.getSerieCorrAndObsByDoc(tranID, id);

                    WebMsgBox.Show("Transaccion encadenada (NOTA DEBITO " + Arr_Doc_Factura[1].ToString() + "-" + Arr_Doc_Factura[2].ToString() + "), esta transaccion solo puede ser anulada al anular la Transaccion Padre.: PROVISION A LA ASEGURADORA Serie.: " + Arr_Doc_Padre[1].ToString() + " y Correlativo.: " + Arr_Doc_Padre[2].ToString() + ", al anular Transaccion Padre tambien se anulara Transaccion Hija.: PROVISION INTERCOMPANY Serie.: " + Arr_Doc_Hijo[1].ToString() + " y Correlativo.: " + Arr_Doc_Hijo[2].ToString() + " en la Empresa.: " + Empresa_Hija.Nombre_Sistema + " .");
                    return;
                }
                #endregion
            }
            else if ((resultado_transaccion_encadenada == "PADRE") || (resultado_transaccion_encadenada == "HIJO"))
            {
                #region Validar Hijo
                if (resultado_transaccion_encadenada == "HIJO")
                {
                    ArrayList Arr_Padre = DB.Get_Transaccion_Padre_By_Transaccion_Hija(tranID, id);
                    RE_GenericBean Bean_Padre = (RE_GenericBean)Arr_Padre[0];
                    PaisBean Empresa_Padre = (PaisBean)DB.getPais(int.Parse(Bean_Padre.strC4));
                    #region Validacion Intercompany
                    if (Empresa_Padre.ID != user.PaisID)
                    {
                        Arr_Temp = DB.getSerieCorrAndObsByDoc(int.Parse(Bean_Padre.strC2), int.Parse(Bean_Padre.strC3));
                        WebMsgBox.Show("Transaccion encadenada, esta transaccion solo puede ser anulada desde la Empresa.: " + Empresa_Padre.Nombre_Sistema + " durante el dia de emision al anular la Transaccion Padre.: " + Arr_Temp[0].ToString() + " Serie.: " + Arr_Temp[1].ToString() + " y Correlativo.: " + Arr_Temp[2].ToString() + "");
                        return;
                    }
                    #endregion
                }
                #endregion
                #region Validar Padre
                if (resultado_transaccion_encadenada == "PADRE")
                {
                    ArrayList Arr_Transacciones_Hijas = (ArrayList)DB.Get_Transacciones_Encadenadas_Hijas(tranID, id);
                    if (Arr_Transacciones_Hijas == null)
                    {
                        WebMsgBox.Show("Existio un error al Tratar de Obtener las Transacciones Encadenadas Hijas");
                        return;
                    }
                    foreach (RE_GenericBean Hija in Arr_Transacciones_Hijas)
                    {
                        PaisBean Empresa_Hijo = (PaisBean)DB.getPais(int.Parse(Hija.strC4));
                        Arr_Temp = DB.getSerieCorrAndObsByDoc(int.Parse(Hija.strC2), int.Parse(Hija.strC3));
                        #region Validar Estado Documento
                        estado_hijo = DB.getEstadoDocumento(int.Parse(Hija.strC3), int.Parse(Hija.strC2));
                        if ((estado_hijo == 2) || (estado_hijo == 4) || (estado_hijo == 9))
                        {
                            documentos_con_amarre++;
                            mensaje_documentos_asociados += "* " + Arr_Temp[5].ToString() + " " + " Serie.: " + Arr_Temp[1].ToString() + " y Correlativo.: " + Arr_Temp[2].ToString() + " en la Empresa.: " + Empresa_Hijo.Nombre_Sistema + " \\n";
                        }
                        #endregion
                        #region Validar Bloqueo Periodo
                        RE_GenericBean bloqueo = null;
                        DateTime fecha_ultimo_bloqueo;
                        DateTime hoy;
                        DateTime fecha_doc = DateTime.Parse(DB.getFechaDocumento(int.Parse(Hija.strC3), int.Parse(Hija.strC2)));
                        bloqueo = (RE_GenericBean)DB.ObtengoPeriodoBloqueado(int.Parse(Hija.strC4));
                        fecha_ultimo_bloqueo = DateTime.Parse(bloqueo.strC1); //fecha del ultimo bloqueo    
                        hoy = DateTime.Today; //fecha del dia de hoy
                        int activo = bloqueo.intC3;
                        if ((fecha_doc <= fecha_ultimo_bloqueo) && (activo == 1))
                        {
                            documentos_con_periodo_bloqueado++;
                            mensaje_documentos_periodo_cerrado += "* " + Arr_Temp[5].ToString() + " " + " Serie.: " + Arr_Temp[1].ToString() + " y Correlativo.: " + Arr_Temp[2].ToString() + " en la Empresa.: " + Empresa_Hijo.Nombre_Sistema + " \\n";
                        }
                        #endregion
                    }
                    if ((documentos_con_amarre > 0) || (documentos_con_periodo_bloqueado > 0))
                    {
                        #region Documento con Amarres o Bloqueos
                        if (documentos_con_amarre > 0)
                        {
                            #region Crear Mensaje Documentos con Amarre
                            mensaje_documentos_asociados = "-- Transacciones Hijas con documentos asociandos .: \\n" + mensaje_documentos_asociados;
                            #endregion
                        }
                        if (documentos_con_periodo_bloqueado > 0)
                        {
                            #region Crear Mensaje Documentos con Periodo Bloqueado
                            mensaje_documentos_periodo_cerrado = "-- Transacciones Hijas dentro de un periodo cerrado .: \\n" + mensaje_documentos_periodo_cerrado;
                            #endregion
                        }
                        Arr_Temp = DB.getSerieCorrAndObsByDoc(tranID, id);
                        mensaje = "No se puede Anular la Transaccion Padre  Serie.: " + Arr_Temp[1].ToString() + " y Correlativo.: " + Arr_Temp[2].ToString() + " por el siguiente motivo.: \\n";
                        mensaje += mensaje_documentos_asociados;
                        mensaje += mensaje_documentos_periodo_cerrado;
                        WebMsgBox.Show(mensaje);
                        return;
                        #endregion
                    }
                    else
                    {
                        #region Documento sin Amarres o Bloqueos se procede a Anular Hijos
                        foreach (RE_GenericBean Hija in Arr_Transacciones_Hijas)
                        {
                            Anular(int.Parse(Hija.strC2), int.Parse(Hija.strC3), 0, 0);
                            DB.Desactivar_Transaccion_Encadenada_Log(int.Parse(Hija.strC1), user);
                        }
                        #endregion
                    }
                }
                #endregion
            }
            #endregion
            anulacion = DB.AnulacionND(id, user, contaID);
            if (anulacion == -100)
            {
                WebMsgBox.Show("No se puede anular el Documento");
                return;
            }
        }
        else if (tranID == 5) // provision
        {
            #region Validar Transaccion Encadenada
            ArrayList Arr_Temp = null;
            string resultado_transaccion_encadenada = "";
            resultado_transaccion_encadenada = DB.Validar_Transaccion_Encadenada(tranID, id);
            if (resultado_transaccion_encadenada == "-100")
            {
                WebMsgBox.Show("Existio un error al tratar de Validar si la trasaccion estaba encadenada");
                return;
            }
            if ((resultado_transaccion_encadenada == "PADRE") || (resultado_transaccion_encadenada == "HIJO"))
            {
                RE_GenericBean Provision = (RE_GenericBean)Utility.getDetalleProvision(id);
                if (resultado_transaccion_encadenada == "HIJO")
                {
                    #region Provision Encadenada como Hija
                    ArrayList Arr_Padre = DB.Get_Transaccion_Padre_By_Transaccion_Hija(tranID, id);
                    RE_GenericBean Bean_Padre = (RE_GenericBean)Arr_Padre[0];
                    PaisBean Empresa_Padre = (PaisBean)DB.getPais(int.Parse(Bean_Padre.strC4));
                    if (Provision.intC11 == 10)
                    {
                        if ((ttrID_PADRE != 0) && (refID_PADRE != 0))//Esto Significa que se esta Anuldo Transaccion Hija desde Transaccion Padre
                        {
                            #region Provision Hija Anulada desde Empresa Padre
                            #endregion
                        }
                        else
                        {
                            #region Provision Hija Anulada desde Empresa Hija
                            RE_GenericBean Intercompany_Origen = null;
                            Intercompany_Origen = (RE_GenericBean)DB.Get_Intercompany_Data(Provision.intC3);
                            if (Intercompany_Origen.intC3 != user.PaisID)
                            {
                                WebMsgBox.Show("Transaccion encadenada (PROVISION " + Provision.strC17 + " -" + Provision.intC6.ToString() + " ), esta transaccion solo puede ser anulada desde la Empresa Origen.: " + Empresa_Padre.Nombre_Sistema + " al anular la Transaccion Padre Serie.: " + Provision.strC3 + " y Correlativo.: " + Provision.strC31 + " ");
                                return;
                            }
                            #endregion
                        }
                    }
                    #endregion
                   }
                else if (resultado_transaccion_encadenada == "PADRE")
                {
                    if (Provision.Tipo_Operacion == 9)
                    {
                        #region Provision de Seguros Doblemente Encadenada
                        int documentos_con_amarre = 0;
                        int documentos_con_periodo_bloqueado = 0;
                        int estado_hijo = 0;
                        string mensaje = "";
                        string mensaje_documentos_asociados = "";
                        string mensaje_documentos_periodo_cerrado = "";
                        ArrayList Arr_Transacciones_Hijas = (ArrayList)DB.Get_Transacciones_Encadenadas_Hijas(tranID, id);
                        ArrayList Arr_Transacciones_Nietas = null;
                        if (Arr_Transacciones_Hijas == null)
                        {
                            WebMsgBox.Show("Existio un error al Tratar de Obtener las Transacciones Encadenadas Hijas");
                            return;
                        }
                        foreach (RE_GenericBean Hija in Arr_Transacciones_Hijas)
                        {
                            Arr_Transacciones_Nietas = (ArrayList)DB.Get_Transacciones_Encadenadas_Hijas(int.Parse(Hija.strC2), int.Parse(Hija.strC3));
                        }
                        foreach (RE_GenericBean Nieta in Arr_Transacciones_Nietas)
                        {
                            Arr_Transacciones_Hijas.Add(Nieta);
                        }
                        foreach (RE_GenericBean Hija in Arr_Transacciones_Hijas)
                        {
                            PaisBean Empresa_Hijo = (PaisBean)DB.getPais(int.Parse(Hija.strC4));
                            Arr_Temp = DB.getSerieCorrAndObsByDoc(int.Parse(Hija.strC2), int.Parse(Hija.strC3));
                            #region Validar Estado Documento
                            estado_hijo = DB.getEstadoDocumento(int.Parse(Hija.strC3), int.Parse(Hija.strC2));
                            if ((estado_hijo == 2) || (estado_hijo == 4) || (estado_hijo == 9))
                            {
                                documentos_con_amarre++;
                                mensaje_documentos_asociados += "* " + Arr_Temp[5].ToString() + " " + " Serie.: " + Arr_Temp[1].ToString() + " y Correlativo.: " + Arr_Temp[2].ToString() + " en la Empresa.: " + Empresa_Hijo.Nombre_Sistema + " \\n";
                            }
                            #endregion
                            #region Validar Bloqueo Periodo
                            RE_GenericBean bloqueo = null;
                            DateTime fecha_ultimo_bloqueo;
                            DateTime hoy;
                            DateTime fecha_doc = DateTime.Parse(DB.getFechaDocumento(int.Parse(Hija.strC3), int.Parse(Hija.strC2)));
                            bloqueo = (RE_GenericBean)DB.ObtengoPeriodoBloqueado(int.Parse(Hija.strC4));
                            fecha_ultimo_bloqueo = DateTime.Parse(bloqueo.strC1); //fecha del ultimo bloqueo    
                            hoy = DateTime.Today; //fecha del dia de hoy
                            int activo = bloqueo.intC3;
                            if ((fecha_doc <= fecha_ultimo_bloqueo) && (activo == 1))
                            {
                                documentos_con_periodo_bloqueado++;
                                mensaje_documentos_periodo_cerrado += "* " + Arr_Temp[5].ToString() + " " + " Serie.: " + Arr_Temp[1].ToString() + " y Correlativo.: " + Arr_Temp[2].ToString() + " en la Empresa.: " + Empresa_Hijo.Nombre_Sistema + " \\n";
                            }
                            #endregion
                        }
                        if ((documentos_con_amarre > 0) || (documentos_con_periodo_bloqueado > 0))
                        {
                            #region Documento con Amarres o Bloqueos
                            if (documentos_con_amarre > 0)
                            {
                                #region Crear Mensaje Documentos con Amarre
                                mensaje_documentos_asociados = "-- Transacciones Hijas con documentos asociandos .: \\n" + mensaje_documentos_asociados;
                                #endregion
                            }
                            if (documentos_con_periodo_bloqueado > 0)
                            {
                                #region Crear Mensaje Documentos con Periodo Bloqueado
                                mensaje_documentos_periodo_cerrado = "-- Transacciones Hijas dentro de un periodo cerrado .: \\n" + mensaje_documentos_periodo_cerrado;
                                #endregion
                            }
                            Arr_Temp = DB.getSerieCorrAndObsByDoc(tranID, id);
                            mensaje = "No se puede Anular la Transaccion Padre  Serie.: " + Arr_Temp[1].ToString() + " y Correlativo.: " + Arr_Temp[2].ToString() + " por el siguiente motivo.: \\n";
                            mensaje += mensaje_documentos_asociados;
                            mensaje += mensaje_documentos_periodo_cerrado;
                            WebMsgBox.Show(mensaje);
                            return;
                            #endregion
                        }
                        else
                        {
                            #region Documento sin Amarres o Bloqueos se procede a Anular Hijos
                            foreach (RE_GenericBean Hija in Arr_Transacciones_Hijas)
                            {
                                Anular(int.Parse(Hija.strC2), int.Parse(Hija.strC3), tranID, id);
                                DB.Desactivar_Transaccion_Encadenada_Log(int.Parse(Hija.strC1), user);
                            }
                            #endregion
                        }
                        #endregion
                    }
                }
            }
            #endregion
            anulacion = DB.AnulacionProvision(id, user, contaID);
            if (anulacion == -100)
            {
                WebMsgBox.Show("No se puede anular el Documento");
                return;
            }
        }
        else if (tranID == 6) // cheque
        {
            string Liquidacion = "";
            Liquidacion = DB.Validar_Documento_Liquidado(id, 6);
            if (Liquidacion != "")
            {
                WebMsgBox.Show("No se puede anular el Documento, porque tiene la siguiente Liquidacion asociada: " + Liquidacion);
                return;
            }
            int Estado_Conciliado = 0;
            Estado_Conciliado = DB.Validar_Documento_Conciliado(id, 6);
            if (Estado_Conciliado == -100)
            {
                WebMsgBox.Show("Existion un error al tratar de validar si el documento esta conciliado o no");
                return;
            }
            else if (Estado_Conciliado == 13)
            {
                WebMsgBox.Show("No se Puede Anular un Cheque Conciliado");
                return;
            }
            else
            {
                int Estado_Conciliado_Circulacion = 0;
                Estado_Conciliado_Circulacion = DB.Validar_Documento_Conciliado_Circulacion(id, 6);
                if (Estado_Conciliado_Circulacion == -100)
                {
                    WebMsgBox.Show("Existion un error al tratar de validar si el documento esta conciliado o no.");
                    return;
                }
                else if (Estado_Conciliado_Circulacion > 0)
                {
                    WebMsgBox.Show("No se puede anular el Cheque porque esta en circulacion en un periodo ya conciliado, para anular primero debe botar la conciliacion respectiva.");
                    return;
                }
                else
                {
                    anulacion = DB.AnulacionCheque(id, user, contaID);
                    if (anulacion == -100)
                    {
                        WebMsgBox.Show("No se puede anular el Documento");
                        return;
                    }
                }
            }
        }
        else if (tranID == 9)
        {
           anulacion = DB.AnulacionRetencion(id, user, contaID);
            if (anulacion == -100)
            {
                WebMsgBox.Show("No se puede anular el Documento");
                return;
            }
        }
        else if (tranID == 19) // transferencia
        {
            int Estado_Conciliado = 0;
            Estado_Conciliado = DB.Validar_Documento_Conciliado(id, 19);
            if (Estado_Conciliado == -100)
            {
                WebMsgBox.Show("Existion un error al tratar de validar si el documento esta conciliado o no");
                return;
            }
            else if (Estado_Conciliado == 13)
            {
                WebMsgBox.Show("No se Puede Anular una Transferencia Conciliada");
                return;
            }
            else
            {
                int Estado_Conciliado_Circulacion = 0;
                Estado_Conciliado_Circulacion = DB.Validar_Documento_Conciliado_Circulacion(id, 19);
                if (Estado_Conciliado_Circulacion == -100)
                {
                    WebMsgBox.Show("Existion un error al tratar de validar si el documento esta conciliado o no.");
                    return;
                }
                else if (Estado_Conciliado_Circulacion > 0)
                {
                    WebMsgBox.Show("No se puede anular la Transferencia porque esta en circulacion en un periodo ya conciliado, para anular primero debe botar la conciliacion respectiva.");
                    return;
                }
                else
                {
                    anulacion = DB.AnulacionTransferencia(id, user, contaID);
                    if (anulacion == -100)
                    {
                        WebMsgBox.Show("No se puede anular el Documento");
                        return;
                    }
                }
            }
        }
        else if (tranID == 20) // ND Bancaria
        {
            int Estado_Conciliado = 0;
            Estado_Conciliado = DB.Validar_Documento_Conciliado(id, 20);
            if (Estado_Conciliado == -100)
            {
                WebMsgBox.Show("Existion un error al tratar de validar si el documento esta conciliado o no");
                return;
            }
            else if (Estado_Conciliado == 13)
            {
                WebMsgBox.Show("No se Puede Anular una Nota de Debito Bancaria Conciliada");
                return;
            }
            else
            {
                int Estado_Conciliado_Circulacion = 0;
                Estado_Conciliado_Circulacion = DB.Validar_Documento_Conciliado_Circulacion(id, 20);
                if (Estado_Conciliado_Circulacion == -100)
                {
                    WebMsgBox.Show("Existion un error al tratar de validar si el documento esta conciliado o no.");
                    return;
                }
                else if (Estado_Conciliado_Circulacion > 0)
                {
                    WebMsgBox.Show("No se puede anular Nota de Debito Bancaria porque esta en circulacion en un periodo ya conciliado, para anular primero debe botar la conciliacion respectiva.");
                    return;
                }
                else
                {
                    anulacion = DB.AnulacionNDBancaria(id, user, contaID);
                    if (anulacion == -100)
                    {
                        WebMsgBox.Show("No se puede anular el Documento");
                        return;
                    }
                }
            }
        }
        else if (tranID == 21) // NCBancaria
        {
            int Estado_Conciliado = 0;
            Estado_Conciliado = DB.Validar_Documento_Conciliado(id, 21);
            if (Estado_Conciliado == -100)
            {
                WebMsgBox.Show("Existion un error al tratar de validar si el documento esta conciliado o no");
                return;
            }
            else if (Estado_Conciliado == 13)
            {
                WebMsgBox.Show("No se Puede Anular una Nota de Credito Bancaria Conciliada");
                return;
            }
            else
            {
                int Estado_Conciliado_Circulacion = 0;
                Estado_Conciliado_Circulacion = DB.Validar_Documento_Conciliado_Circulacion(id, 21);
                if (Estado_Conciliado_Circulacion == -100)
                {
                    WebMsgBox.Show("Existion un error al tratar de validar si el documento esta conciliado o no.");
                    return;
                }
                else if (Estado_Conciliado_Circulacion > 0)
                {
                    WebMsgBox.Show("No se puede anular Nota de Credito Bancaria porque esta en circulacion en un periodo ya conciliado, para anular primero debe botar la conciliacion respectiva.");
                    return;
                }
                else
                {
                    anulacion = DB.AnulacionNCBancaria(id, user, contaID, tranID);
                    if (anulacion == -100)
                    {
                        WebMsgBox.Show("No se puede anular el Documento");
                        return;
                    }
                }
            }
        }
        else if (tranID == 15) // Ajuste
        {
            anulacion = DB.AnulacionAjusteContable(id, user, contaID, tranID);
            if (anulacion == -100)
            {
                WebMsgBox.Show("No se puede anular el Documento");
                return;
            }
        }
        else if (tranID == 22)//RECIBO SOA
        {
            anulacion = DB.AnulacionReciboSOA(id, user, contaID);
            if (anulacion == -100)
            {
                WebMsgBox.Show("No se puede anular el Documento");
                return;
            }
        }
        else if (tranID == 23)
        {
            
           string liquidacion = DB.Validar_Documento_Liquidado(id, 23);
            if (liquidacion == "")
            {
                anulacion = DB.AnulacionCorte(id, user, contaID);
                if (anulacion == -100)
                {
                    WebMsgBox.Show("No se puede anular el Documento");
                    return;
                }
            }
            else
            {
                WebMsgBox.Show("No se puede anular el Documento, porque tiene una liquidacion: " + liquidacion);
                return;
            }
        }
        else if (tranID == 14)
        {
            anulacion = DB.AnulacionFacturaProforma(id, user, contaID);
            if (anulacion == -100)
            {
                WebMsgBox.Show("No se puede anular el Documento");
                return;
            }
        }
        else if (tranID == 29) // cheque Comision
        {
            anulacion = DB.AnulacionChequeComision(id, user, contaID);
            if (anulacion == -100)
            {
                WebMsgBox.Show("No se puede anular el Documento");
                return;
            }
        }
        else if (tranID == 30) //CORTE COMISION
        {

            anulacion = DB.AnulacionCorteComision(id, user, contaID);
            if (anulacion == -100)
            {
                WebMsgBox.Show("No se puede anular el Documento");
                return;
            }
        }


        int resultado_anulacion = DB.Registrar_Anulacion(user, tranID, id, motivo);
        if (resultado_anulacion == -100)
        {
            WebMsgBox.Show("Existio un Error al Tratar de Registrar la Anulacion");
            return;
        }
    }
    protected void gv_resultado_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        user = (UsuarioBean)Session["usuario"];
        int correlativo = 0, id = 0;
        string serie = "";
        int tranID = int.Parse(lb_tipo_doc.SelectedValue);
        string sql = "";
        int index = Convert.ToInt32(e.RowIndex);
        id = int.Parse(gv_resultado.Rows[index].Cells[1].Text);
        serie = gv_resultado.Rows[index].Cells[2].Text;
  
        if (tranID == 1)
        {
            sql = "select tfr_tre_id, 'Recibo', tre_serie, tre_correlativo, tfr_abono from tbl_factura_abono, tbl_recibo where tfr_tre_id=tre_id and tfr_sysref_id=2 and tfr_tfa_id=" + id + " and tfr_tfa_sysref_id=1 and tre_pai_id="+user.PaisID+" UNION select tfr_tre_id, 'Nota Credito', tnc_serie, cast(tnc_correlativo as text), tfr_abono from tbl_factura_abono, tbl_nota_credito where tfr_tre_id=tnc_id and tfr_sysref_id=3 and tfr_tfa_id=" + id + "  and tfr_tfa_sysref_id=1 and tnc_pai_id="+user.PaisID;
            dt = (DataTable)DB.getInfoAnular2(sql);
            gv_detalle.DataSource = dt;
            gv_detalle.DataBind();
        }
        else if (tranID == 2)
        {
            #region validar cheque anticipo
            sql = " select tcg_id,'CH-Ant.',tba_nombre,tcg_numero,tcg_valor from tbl_cheques_generados, tbl_banco,tbl_banco_cuenta where tcg_id in (select tfr_tfa_id from tbl_factura_abono where tfr_tre_id=" + id + " and tfr_tfa_sysref_id=40 and tfr_sysref_id=2)  and tbc_cuenta_bancaria=tcg_cuenta and tbc_tba_id=tba_id ";
            dt = (DataTable)DB.getInfoAnular2(sql);
            gv_detalle.DataSource = dt;
            gv_detalle.DataBind();
            #endregion
            if (dt.Rows.Count > 0)
            {
                WebMsgBox.Show("No se puede anular la transaccion debido a que tiene operaciones adicionales");
                return;
            }
            sql = "select tpr_tmb_id, 'Movimiento', tba_nombre, tmb_referencia, tpr_monto from tbl_recibo_pago, tbl_banco, tbl_movimiento_bancario where tpr_tmb_id=tmb_id and tmb_tba_id=tba_id and tpr_tre_id=" + id + " and tpr_tmb_id is not null ";
            dt = (DataTable)DB.getInfoAnular2(sql);
            gv_detalle.DataSource = dt;
            gv_detalle.DataBind();
            
        }
        else if (tranID == 17)
        {
            // como para eliminar un recibo no tengo que validar nada
            // hago dt=null
            dt = null;
        }
        else if (tranID == 28)
        {
            // como para eliminar un recibo no tengo que validar nada
            // hago dt=null
            dt = null;
        }
        else if ((tranID == 3) || (tranID == 12) || (tranID == 18) || (tranID == 31))
        {
            dt = null;
        }
        else if (tranID == 4)
        {
            sql = "select tfr_tre_id, 'Recibo', tre_serie, tre_correlativo, tfr_abono from tbl_factura_abono, tbl_recibo where tfr_tre_id=tre_id and tfr_tfa_id=" + id + " and tfr_sysref_id=2 and tfr_tfa_sysref_id=4 and tre_pai_id="+user.PaisID+" UNION select tfr_tre_id, 'Nota Credito', tnc_serie, cast(tnc_correlativo as text), tfr_abono from tbl_factura_abono, tbl_nota_credito where tfr_tre_id=tnc_id and tfr_tfa_id=" + id + " and tfr_sysref_id in (3,12) and tfr_tfa_sysref_id=4 and tnc_pai_id="+user.PaisID;
            dt = (DataTable)DB.getInfoAnular2(sql);
            gv_detalle.DataSource = dt;
            gv_detalle.DataBind();
        }
        else if (tranID == 5)
        { // provision
            sql = "select tcp_tcg_id, 'Cheque', tba_nombre||' '||tcg_cuenta, cast(tcg_numero as character varying), tcg_valor from tbl_cheques_generados, tbl_cheque_provision, tbl_banco, tbl_banco_cuenta, tbl_corte_proveedor_detalle where tcp_tcg_id=tcg_id and tcpd_docto_id=tcp_tpr_prov_id and tcpd_str_id in (5,10) and tbc_cuenta_bancaria=tcg_cuenta and tbc_tba_id=tba_id and tcp_tpr_prov_id=" + id + " and tcg_pai_id=" + user.PaisID + " and tcg_ttr_id in (select ttt_id from tbl_tipo_transaccion where ttt_template='pop_cheques.aspx') UNION select tcp_tcg_id, 'Transferencia', tba_nombre||' '||tcg_cuenta, cast(tcg_no_transferencia as character varying), tcg_valor from tbl_cheques_generados, tbl_cheque_provision, tbl_banco, tbl_banco_cuenta, tbl_corte_proveedor_detalle where tcp_tcg_id=tcg_id and tcpd_docto_id=tcp_tpr_prov_id and tcpd_str_id in (5,10) and tbc_cuenta_bancaria=tcg_cuenta and tbc_tba_id=tba_id and tcp_tpr_prov_id=" + id + " and tcg_pai_id=" + user.PaisID + " and tcg_ttr_id in (select ttt_id from tbl_tipo_transaccion where ttt_template='pop_transferencias_ag.aspx') UNION select tcp_id, 'Corte', tcp_serie, cast(tcp_correlativo as character varying), tpr_valor from tbl_corte_proveedor, tbl_corte_proveedor_detalle, tbl_provisiones where tpr_prov_id=tcpd_docto_id and tcpd_str_id in (5,10) and tcpd_tcp_id=tcp_id and tpr_prov_id=" + id + " and tpr_pai_id=" + user.PaisID + " UNION select trp_id, 'Retencion', trp_serie, trp_referencia, trp_monto from tbl_retencion_provision where trp_ted_id<>3 and trp_tpr_prov_id=" + id + " and trp_pai_id=" + user.PaisID;
            dt = (DataTable)DB.getInfoAnular2(sql);
            gv_detalle.DataSource = dt;
            gv_detalle.DataBind();
        }
        else if (tranID == 6)
        { // cheque
            dt = null;
        }
        if (tranID == 9)
        { // retencion proveedor
            dt = null;
        }
        else if (tranID == 19)
        { // transferencia
            dt = null;
        }
        else if (tranID == 20)
        { // ND Bancaria
            dt = null;
        }
        else if (tranID == 21)
        { // NC Bancaria
            dt = null;
        }
        else if (tranID == 15)
        { // Ajuste
            dt = null;
        }
        else if (tranID == 22)
        { // RECIBO SOA
            sql = "select tprc_tmb_id, 'Movimiento', tba_nombre, tmbc_referencia, tprc_monto from tbl_recibo_pago_corte, tbl_banco, tbl_movimiento_bancario_corte where tprc_tmb_id=tmbc_id and tmbc_tba_id=tba_id and tprc_tre_id=" + id + " and tprc_tmb_id is not null";
            dt = (DataTable)DB.getInfoAnular2(sql);
            gv_detalle.DataSource = dt;
            gv_detalle.DataBind();
        }
        else if (tranID == 23)
        { // corte proveedor
            sql = "select tcg_id, 'Cheque', tba_nombre||' '||tcg_cuenta, cast(tcg_numero as character varying), tcg_valor from tbl_cheques_generados, tbl_cheque_corte, tbl_banco, tbl_banco_cuenta where corte_tcg_id=tcg_id and tbc_cuenta_bancaria=tcg_cuenta and tbc_tba_id=tba_id and corte_tcp_id=" + id + " and tcg_ttr_id in (select ttt_id from tbl_tipo_transaccion where ttt_template='pop_cheques.aspx') UNION select tcg_id, 'Transferencia', tba_nombre||' '||tcg_cuenta, cast(tcg_no_transferencia as character varying), tcg_valor from tbl_cheques_generados, tbl_cheque_corte, tbl_banco, tbl_banco_cuenta where corte_tcg_id=tcg_id and tbc_cuenta_bancaria=tcg_cuenta and tbc_tba_id=tba_id and corte_tcp_id=" + id + " and tcg_ttr_id in (select ttt_id from tbl_tipo_transaccion where ttt_template='pop_transferencias_ag.aspx') UNION  select tcr_tre_id, 'Recibo Corte', trc_serie, trc_correlativo, tcr_abono from tbl_corte_abono, tbl_recibo_corte where tcr_tre_id=trc_id and tcr_sysref_id=22 and tcr_tcp_id=" + id + " and tcr_tcp_sysref_id=11 and trc_pai_id=" + user.PaisID;
            dt = (DataTable)DB.getInfoAnular2(sql);
            gv_detalle.DataSource = dt;
            gv_detalle.DataBind();
        }
        else if (tranID == 30)// CORTE COMISION
        {
            sql = "select tchc_id, 'Cheque-Comision', tbac_nombre||' '||tchc_cuenta, cast(tchc_numero as character varying), tchc_monto from tbl_cheques_comisiones, tbl_pago_cheque_comision, tbl_banco_comision, tbl_banco_cuenta_comision where tpcc_tchc_id=tchc_id and tbcc_cuenta_bancaria=tchc_cuenta and tbcc_tbac_id=tbac_id and tpcc_tcc_id=" + id + " and tpcc_ted_id=1 ";
            dt = (DataTable)DB.getInfoAnular2(sql);
            gv_detalle.DataSource = dt;
            gv_detalle.DataBind();
        }
      
        
        if (dt != null && dt.Rows.Count > 0)
        {
            WebMsgBox.Show("No se puede anular la transaccion debido a que tiene operaciones adicionales");
            return;
        }
        Anular(tranID, id, 0, 0);
        dt = null;
        dt = (DataTable)DB.getInfoAnular(tb_sql.Text);
        gv_resultado.DataSource = dt;
        gv_resultado.DataBind();
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
                    string fecha = oDataRowView.Row["Fecha Emision"].ToString();
                    int id_doc = int.Parse(oDataRowView.Row["ID"].ToString());
                    int bandera_electronico = 0; // la bandera para saber si es doc electronio o no 0 false 1 true.
                    #region formateo fechas
                    DateTime fecha_doc = DateTime.Parse(fecha);
                    //*************************************************************BLOQUEO DE PERIODO*****************************************
                    #region bloqueo periodo
                    RE_GenericBean bloqueo = null;
                    bloqueo = (RE_GenericBean)DB.ObtengoPeriodoBloqueado(user.PaisID);
                    DateTime fecha_ultimo_bloqueo = DateTime.Parse(bloqueo.strC1); //fecha del ultimo bloqueo    
                    DateTime hoy = DateTime.Today; //fecha del dia de hoy
                    int activo = bloqueo.intC3;
                    #endregion
                    //--------------------------------------------------------------------
                    if (lb_tipo_doc.SelectedValue == "1")
                    {
                        #region Validar Documento Electronico FC
                        RE_GenericBean factdata = (RE_GenericBean)DB.getFacturaData(id_doc);
                        if ((factdata.strC50 == "1") || (factdata.strC50 == "2"))
                        {
                            bandera_electronico = 1;
                        }
                        #endregion
                    }
                    else if (lb_tipo_doc.SelectedValue == "3")
                    {
                        #region Validar Documento Electronico FC
                        RE_GenericBean ncdata = (RE_GenericBean)DB.getNotaCreditoData(id_doc);
                        if ((ncdata.strC50 == "1") || (ncdata.strC50 == "2"))
                        {
                            bandera_electronico = 1;
                        }
                        #endregion
                    }
                    else if (lb_tipo_doc.SelectedValue == "4")
                    {
                        #region Validar Documento Electronico FC
                        RE_GenericBean nddata = (RE_GenericBean)DB.getNotaDebitoData(id_doc);
                        if ((nddata.strC50 == "1") || (nddata.strC50 == "2"))
                        {
                            bandera_electronico = 1;
                        }
                        #endregion
                    }
                    if (bandera_electronico == 1)//si es doc electronico.
                    {
                        e.Row.Cells[0].Enabled = false;
                    }
                    //si es guate, es factura  solo bloqueo si el mes de la factura es menor al mes actual no importa si el cierre de periodo es abierto o cerrado.
                    if ((user.PaisID == 1) && (lb_tipo_doc.SelectedValue == "1") && (fecha_doc.Month < hoy.Month))
                    {
                        e.Row.Cells[0].Enabled = false;
                    }
                    //si el pais no es guate y es factura y la fecha de la factura es menor a la del mes anterior bloqueo la anulacion.
                    //if ((user.PaisID != 1) && (lb_tipo_doc.SelectedValue == "1") && (fecha_doc.Month < hoy.Month) && (user.PaisID != 14)) 2019-11-14

                    //[Ticket#2019111131000099] ANULACION FX113-511 AIMAR LOGISTIC
                    if ((user.PaisID != 1 && user.PaisID != 14 && user.PaisID != 9) && (lb_tipo_doc.SelectedValue == "1") && (fecha_doc.Month < hoy.Month)) 
                    {
                        e.Row.Cells[0].Enabled = false;
                    }
                    if ((fecha_doc <= fecha_ultimo_bloqueo) && (activo == 1))
                    {
                        //WebMsgBox.Show("No se puede emitir documentos con fecha seleccionada: " + fecha_doc.ToString().Substring(0, 10) + " menores al cierre de periodo contable: " + fecha_ultimo_bloqueo.ToString().Substring(0, 10));
                        //return;
                        e.Row.Cells[0].Enabled = false;
                    }
                    #endregion
                }
            }
        }
    }
    protected void lb_tipo_doc_SelectedIndexChanged(object sender, EventArgs e)
    {
        ArrayList arr = new ArrayList();
        ListItem item = null;
        if (lb_tipo_doc.SelectedValue.ToString() == "29")
        {
            lb_bancos.Items.Clear();
            item = new ListItem("Seleccione...", "0");
            lb_bancos.Items.Add(item);
            arr = (ArrayList)DB.getBancos_comision(user);
            foreach (RE_GenericBean rgb in arr)
            {
                item = new ListItem(rgb.strC1, rgb.intC1.ToString());
                lb_bancos.Items.Add(item);
            }
        }
    }
    protected void gv_resultado_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            foreach (Button btn in e.Row.Cells[0].Controls.OfType<Button>())
            {
                if (btn.Text == "Anular")
                {
                    btn.Attributes["onclick"] = "if(!confirm('Esta seguro de querer Eliminar esta Transaccion?')){ return false; };";
                }
            }
        }
    }
    protected void gv_resultado_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
}
