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

public partial class operations_liquidacion_anticipo_CCH : System.Web.UI.Page
{
    UsuarioBean user = null;
    DataTable dt = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            lb_fecha_hora.Text = DB.getDateTimeNow();
        }
        user = (UsuarioBean)Session["usuario"];
        RE_GenericBean proveedor = null;

        if (!Page.IsPostBack)
        {
            obtengo_listas();
            lb_bancos_SelectedIndexChanged(sender, e);
            lb_bancos0_SelectedIndexChanged(sender, e);            
        }
    }

    protected void obtengo_listas()
    {
        ArrayList arr = null;
        ListItem item = null;
        // obtengo las transacciones
        arr = (ArrayList)DB.getOpcionesMatriz("tbl_tipo_transaccion, tbl_usuario_operacion where uop_usu_id='" + user.ID + "' and uop_pai_id=" + user.PaisID + " and uop_ttt_id=ttt_id and ttt_template='liquidacion_cheques.aspx'");
        lb_transaccion.Items.Clear();
        foreach (RE_GenericBean rgb in arr)
        {
            item = new ListItem(rgb.strC1, rgb.intC1.ToString());
            lb_transaccion.Items.Add(item);
        }

        lb_moneda.Items.Clear();
        arr = (ArrayList)DB.getMonedasbyPais(user.PaisID, user.contaID);
        foreach (RE_GenericBean rgb in arr)
        {
            item = new ListItem(rgb.strC1, rgb.intC1.ToString());
            lb_moneda.Items.Add(item);
            lb_moneda0.Items.Add(item);
        }
        
        arr = (ArrayList)DB.getBancosXPais(user.PaisID,1);
        lb_bancos.Items.Clear();
        foreach (RE_GenericBean rgb in arr)
        {
            item = new ListItem(rgb.strC1, rgb.intC1.ToString());
            lb_bancos.Items.Add(item);
            lb_bancos0.Items.Add(item);
        }
        item = new ListItem("Seleccione...", "0");
        lb_cuentas_bancarias.Items.Add(item);
        lb_cuentas_bancarias0.Items.Add(item);
    }

    protected void bt_buscar_Click(object sender, EventArgs e)
    {
        user = (UsuarioBean)Session["usuario"];
        ArrayList Arr = null;
        string where = "";
        if (tb_codigo.Text.Equals("") && tb_nombreb.Text.Equals("") && tb_nitb.Text.Equals(""))
        {
            WebMsgBox.Show("Debe ingresar al menos un criterio de búsqueda");
            return;
        }       
        
            if (!tb_nitb.Text.Equals(""))
            {
                WebMsgBox.Show("No se puede buscar por nit usuario de caja chica");
                return;
            }
            if (!tb_nombreb.Text.Trim().Equals("") && tb_nombreb.Text != null) where += " and upper(rtrim(pw_gecos)) like '%" + tb_nombreb.Text.Trim().ToUpper() + "%'";
            if (!tb_codigo.Text.Trim().Equals("") && tb_codigo.Text != null)
                if (!where.Equals("")) where += " and id_usuario=" + tb_codigo.Text; else where += " and id_usuario=" + tb_codigo.Text;
            Arr = (ArrayList)DB.getProveedor_cajachica(where); //Caja_chica
            dt = (DataTable)Utility.fillGridView("CajaChica", Arr);

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
            int proveedorID = int.Parse(tb_agenteID.Text);
            int proveedortype = 8; //porque 8 es proveedor caja chica
            int monID = int.Parse(lb_moneda.SelectedValue);
            
                //obtengo el listado de cortes
                ArrayList Arr = null;
                Arr = DB.getCortesProveedor(proveedorID, proveedortype, monID, 5, user);
                dt = (DataTable)Utility.fillGridView("corteproveedor", Arr);
                gv_cortes.DataSource = dt;
                gv_cortes.DataBind();
    }
    protected void gv_proveedor_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            user = (UsuarioBean)Session["usuario"];
        }
    }

    protected void btn_buscar_Click(object sender, EventArgs e)
    {
        ArrayList Arr = null;
        string where = "";

        where = " tcg_ted_id not in (3,14) and tcg_cli_id=" + tb_agenteID.Text + " and tcg_tpi_id=8 and tcg_ttr_id=43 and tcg_pai_id=" + user.PaisID;
        if (!lb_cuentas_bancarias.SelectedValue.Trim().Equals("")) where += " and tcg_cuenta='" + lb_cuentas_bancarias.SelectedValue + "'";
        Arr = DB.getChequesAnticipo(where);
        dt = (DataTable)Utility.fillGridView("chequesanticipo", Arr);
        gv_cheques.DataSource = dt;
        gv_cheques.DataBind();
    }
    protected void lb_bancos_SelectedIndexChanged(object sender, EventArgs e)
    {
        CheckBox chk_cortes;
        #region Limpiar GV Cortes
        //Se agrego este procedimiento porque si se escoge un banco o una cuenta bancariaria diferente
        //Se debe recalcular el total dependiendo la moneda y el tipo de cambio
        if (gv_cortes.Rows.Count > 0)
        {
            foreach (GridViewRow grd_Row in gv_cortes.Rows)
            {
                chk_cortes = (CheckBox)grd_Row.FindControl("chk_seleccion");
                chk_cortes.Checked = false;
            }
        }
        #endregion
        UsuarioBean user;
        user = (UsuarioBean)Session["usuario"];
        ListItem item = null;
        lb_cuentas_bancarias.Items.Clear();
        item = new ListItem("Seleccione...", " ");
        item.Selected = true;
        int ExistDep = 0;
        lb_cuentas_bancarias.Items.Add(item);
        ArrayList arrCtas = (ArrayList)DB.getCuentasBancariasXpais(int.Parse(lb_bancos.SelectedValue), user.PaisID,1);
        foreach (RE_GenericBean rgb in arrCtas)
        {
            item = new ListItem(rgb.strC1, rgb.strC1);
            lb_cuentas_bancarias.Items.Add(item);
        }
    }
    protected void bt_guardar_Click(object sender, EventArgs e)
    {
        RE_GenericBean rgb = new RE_GenericBean();
        RE_GenericBean rgbdep = new RE_GenericBean();
        decimal total_cheques = 0;
        decimal total_cheques_eq = 0;
        string moneda_cheque = "";
        string moneda_corte = "";
        decimal sobrante = 0;
        decimal sobrante_eq = 0;
        decimal total_provisiones = 0;
        decimal total_provisiones_eq = 0;
        CheckBox chk_cheque, chk_cortes;
        int cantcorte = 0, cantcheque = 0;
        //*************************************************************BLOQUEO DE PERIODO*****************************************
        if ((tb_boleta.Text.ToString() != "") && (tb_sobrante.Text.ToString() != ""))
        {
            if (tb_fechadoc.Text.ToString() == "")
            {
                WebMsgBox.Show("Debe de ingresar una fecha");
                return;
            }

            #region bloqueo periodo
            RE_GenericBean bloqueo = null;
            bloqueo = (RE_GenericBean)DB.ObtengoPeriodoBloqueado(user.PaisID);
            DateTime fecha_ultimo_bloqueo = DateTime.Parse(bloqueo.strC1); //fecha del ultimo bloqueo    
            DateTime hoy = DateTime.Today; //fecha del dia de hoy
            int activo = bloqueo.intC3;
            #region formateo fechas
            DateTime fecha_doc = DateTime.Parse(DB.DateFormat(tb_fechadoc.Text.ToString().Substring(0, 10)));
            #endregion
            if ((fecha_doc <= fecha_ultimo_bloqueo) && (activo == 1))
            {
                WebMsgBox.Show("No se puede emitir documentos con fecha seleccionada: " + fecha_doc.ToString().Substring(0, 10) + " menores al cierre de periodo contable: " + fecha_ultimo_bloqueo.ToString().Substring(0, 10));
                return;
            }
            #endregion
        }
        foreach (GridViewRow row in gv_cheques.Rows)
        {
            chk_cheque = (CheckBox)row.FindControl("chk_cheque");
            if (chk_cheque.Checked) cantcheque++;
        }
        if (cantcheque > 1) {
            WebMsgBox.Show("Solo puede asociar 1 cheque");
            return;
        }
        foreach (GridViewRow grd_Row in gv_cortes.Rows)
        {
            chk_cortes = (CheckBox)grd_Row.FindControl("chk_seleccion");
            if (chk_cortes.Checked) cantcorte++;
        }

        if ((cantcheque==0) || (cantcorte==0)) {
            WebMsgBox.Show("Debe por lo menos seleccionar 1 cheque y 1 corte");
            return;
        }

        foreach (GridViewRow row in gv_cheques.Rows) {
            chk_cheque = (CheckBox)row.FindControl("chk_cheque");
            if (chk_cheque.Checked)
            {
                total_cheques += decimal.Parse(row.Cells[4].Text);
                total_cheques_eq += decimal.Parse(row.Cells[7].Text);
                moneda_cheque = row.Cells[6].Text;
                rgb.intC2 = int.Parse(row.Cells[1].Text);//No cheque
            }
        }

        foreach (GridViewRow grd_Row in gv_cortes.Rows)
        {
            chk_cortes = (CheckBox)grd_Row.FindControl("chk_seleccion");
            if (chk_cortes.Checked)
            {
                moneda_corte = grd_Row.Cells[6].Text;
                if (moneda_cheque == moneda_corte)
                {
                    total_provisiones += decimal.Parse(grd_Row.Cells[7].Text);
                    total_provisiones_eq += decimal.Parse(grd_Row.Cells[8].Text);
                }
                else 
                {
                    total_provisiones += decimal.Parse(grd_Row.Cells[8].Text);
                    total_provisiones_eq += decimal.Parse(grd_Row.Cells[7].Text);
                }
                
                if (rgb.arr2 == null) rgb.arr2 = new ArrayList();
                rgb.arr2.Add(grd_Row.Cells[1].Text);
            }
        }
        
        if (!tb_sobrante.Text.Equals("")) {
            if (lb_moneda.SelectedValue == lb_moneda0.SelectedValue)
            {
                sobrante = decimal.Parse(tb_sobrante.Text);
                if (lb_moneda.SelectedValue.Equals("8"))
                {
                    sobrante_eq = sobrante * user.pais.TipoCambio;
                }
                else
                {
                    sobrante_eq = sobrante / user.pais.TipoCambio;
                }
            }
            else
            {
                sobrante_eq = decimal.Parse(tb_sobrante.Text);
                if (lb_moneda.SelectedValue.Equals("8"))
                {
                    sobrante = sobrante_eq / user.pais.TipoCambio;
                }
                else
                {
                    sobrante = sobrante_eq * user.pais.TipoCambio;
                }
            }
        }            
            
        if (sobrante > 0) { 
            if (tb_boleta.Text == null || tb_boleta.Text.Equals("")) {
                WebMsgBox.Show("Es necesario que ingrese un numero de boleta");
                return;
            }
            if (tb_fechadoc.Text == null || tb_fechadoc.Text.Equals(""))
            {
                WebMsgBox.Show("Es necesario que ingrese la fecha del deposito");
                return;
            }
        }
        
        decimal monto_abonar = 0;
        monto_abonar=sobrante+total_provisiones;
        
        if (total_cheques != monto_abonar) {
            WebMsgBox.Show("La suma de los cortes y el deposito debe ser igual a la suma de los cheques");
            return;
        }

        //obteniendo la configuracion para guardar el deposito si hay un valor sobrante
        if (sobrante > 0) {
            rgbdep.intC1 = int.Parse(lb_bancos0.SelectedValue);
            rgbdep.strC1 = lb_cuentas_bancarias0.SelectedValue;
            rgbdep.boolC1 = true;//indica que es un ingreso a la cuenta
            rgbdep.strC2 = tb_boleta.Text;
            if (DB.existDeposito(rgbdep.intC1, rgbdep.strC1.Trim(), rgbdep.strC2.Trim()))
            {
                WebMsgBox.Show("No se puede ingresar este deposito ya que fue ingresado anteriormente");
                return;
            }
        }
        //Fin
        int tranID = int.Parse(lb_transaccion.SelectedValue);
        int monID = int.Parse(lb_moneda.SelectedValue);
        int paisID = user.PaisID;
        int contaID = int.Parse(Session["Contabilidad"].ToString());
        
        rgb.Fecha_Hora = lb_fecha_hora.Text;
        rgb.intC1 = int.Parse(lb_moneda.SelectedValue);//moneda
        rgb.intC6 = int.Parse(tb_agenteID.Text);
        rgb.intC5 = 8;
        rgb.decC1 = total_cheques;//valor
        rgb.decC2 = sobrante;//cargo adicional
        rgb.decC3 = total_cheques_eq;//valor equivalente
        rgb.intC3 = contaID;// tipo contabilidad
        rgb.intC4 = tranID;// tipo transaccion
        
        int matOpID = DB.getMatrizOperacionID(tranID, monID, paisID, contaID);
        ArrayList ctas = (ArrayList)DB.getMatrizConfiguracion(matOpID);
        rgb.arr3 = ctas;

        //Se obtiene el resto de la informacion para guardar el deposito si hay un valor sobrante
        if (sobrante > 0)
        {
            rgbdep.intC2 = int.Parse(lb_moneda0.SelectedValue);
            rgbdep.intC3 = contaID;
            rgbdep.intC4 = tranID;
            rgbdep.decC1 = decimal.Parse(tb_sobrante.Text);
            if (lb_moneda0.SelectedValue.Equals("8"))
            {
                rgbdep.decC2 = rgbdep.decC1 * user.pais.TipoCambio;
            }
            else
            {
                rgbdep.decC2 = rgbdep.decC1 / user.pais.TipoCambio;
            }
            rgbdep.strC3 = tb_fechadoc.Text;
        }
        //Fin

        ArrayList result = DB.LiquidoCheque(rgb, user, rgbdep);
        
        if (result == null || result.Count == 0 || int.Parse(result[1].ToString()) == 0 || int.Parse(result[0].ToString()) == -100)
        {
            WebMsgBox.Show("Existió un problema al tratar de guardar la información, por favor intente de nuevo");
            bt_guardar.Enabled = true;
            return;
        }
        else
        {
            WebMsgBox.Show("Se guardo la transaccion con el cheque " + result[1].ToString());
            gv_cortes.Enabled = false;
            bt_guardar.Enabled = false;

            //Mostrando la Partida contable generada
            gv_detalle_partida.DataSource = DB.getPolizaDiario(user, double.Parse(result[0].ToString()), 6, 44);
            if (gv_detalle_partida != null) { 
                gv_detalle_partida.DataBind();
                if (gv_detalle_partida.Rows.Count>0){
                    gv_detalle_partida.Rows[gv_detalle_partida.Rows.Count - 1].Font.Bold = true;
                }
            }
            return;
        }
    }
    
    protected void lb_cuentas_bancarias_SelectedIndexChanged(object sender, EventArgs e)
    {
        CheckBox chk_cortes;
        #region Limpiar GV Cortes y elimino todos los cheques generados en esa cuenta
        gv_cheques.DataSource = null;
        gv_cheques.DataBind();
        //Se agrego este procedimiento porque si se escoge un banco o una cuenta bancariaria diferente
        //Se debe recalcular el total dependiendo la moneda y el tipo de cambio
        if (gv_cortes.Rows.Count > 0)
        {
            foreach (GridViewRow grd_Row in gv_cortes.Rows)
            {
                chk_cortes = (CheckBox)grd_Row.FindControl("chk_seleccion");
                chk_cortes.Checked = false;
            }
        }
        #endregion
        if (!lb_cuentas_bancarias.SelectedItem.Text.Equals("Seleccione..."))
        {
            string cuentaID = lb_cuentas_bancarias.SelectedValue;
            RE_GenericBean datoscuenta = (RE_GenericBean)DB.GetMonedaIDbyCuentaBancaria(cuentaID);
            lb_moneda.SelectedValue = datoscuenta.intC1.ToString();
        }
    }

    protected void lb_bancos0_SelectedIndexChanged(object sender, EventArgs e)
    {
        UsuarioBean user;
        user = (UsuarioBean)Session["usuario"];
        ListItem item = null;
        lb_cuentas_bancarias0.Items.Clear();
        item = new ListItem("Seleccione...", " ");
        item.Selected = true;
        lb_cuentas_bancarias0.Items.Add(item);
        ArrayList arrCtas = (ArrayList)DB.getCuentasBancariasXpais(int.Parse(lb_bancos0.SelectedValue), user.PaisID,1);
        foreach (RE_GenericBean rgb in arrCtas)
        {
            item = new ListItem(rgb.strC1, rgb.strC1);
            lb_cuentas_bancarias0.Items.Add(item);
        }
    }
    protected void lb_cuentas_bancarias0_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (!lb_cuentas_bancarias0.SelectedItem.Text.Equals("Seleccione..."))
        {
            string cuentaID = lb_cuentas_bancarias0.SelectedValue;
            RE_GenericBean datoscuenta = (RE_GenericBean)DB.GetMonedaIDbyCuentaBancaria(cuentaID);
            lb_moneda0.SelectedValue = datoscuenta.intC1.ToString();
        }
    }
}
