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

public partial class operations_Depositos : System.Web.UI.Page
{
    UsuarioBean user;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["usuario"] == null)
        {
            Response.Redirect("../default.aspx");
        }

        user = (UsuarioBean)Session["usuario"];

        if (!Page.IsPostBack)
        {


            ArrayList arr = (ArrayList)DB.getMonedasbyPais(user.PaisID, 0);
            ListItem item = null;
            foreach (RE_GenericBean rgb in arr)
            {
                item = new ListItem(rgb.strC1, rgb.intC1.ToString());
                lb_moneda.Items.Add(item);
            }

            // obtengo las transacciones
            arr = (ArrayList)DB.getOpcionesMatriz("tbl_tipo_transaccion where ttt_template='Depositos.aspx'");
            lb_transaccion.Items.Clear();
            foreach (RE_GenericBean rgb in arr)
            {
                item = new ListItem(rgb.strC1, rgb.intC1.ToString());
                lb_transaccion.Items.Add(item);
            }
            lb_transaccion.SelectedIndex = 1;

            arr = (ArrayList)DB.getBancosXPais(user.PaisID,1);
            lb_bancos.Items.Clear();
            foreach (RE_GenericBean rgb in arr)
            {
                item = new ListItem(rgb.strC1, rgb.intC1.ToString());
                lb_bancos.Items.Add(item);
            }
            lb_bancos_SelectedIndexChanged(sender, e);
            lb_cuentas_bancarias_SelectedIndexChanged(sender,e);
            int tipo_contabilidad = int.Parse(Session["Contabilidad"].ToString());
        }
    }

    protected void lb_bancos_SelectedIndexChanged(object sender, EventArgs e)
    {
        ListItem item = null;
        lb_cuentas_bancarias.Items.Clear();
        ArrayList arrCtas = (ArrayList)DB.getCuentasBancarias_ByPaisMoneda(int.Parse(lb_bancos.SelectedValue), user,1);
        foreach (RE_GenericBean rgb in arrCtas)
        {
            item = new ListItem(rgb.strC1, rgb.strC1);
            lb_cuentas_bancarias.Items.Add(item);
        }
        lb_cuentas_bancarias_SelectedIndexChanged(sender, e);
    }

    protected void lb_cuentas_bancarias_SelectedIndexChanged(object sender, EventArgs e)
    {
        string cuentaID = lb_cuentas_bancarias.SelectedValue;
        RE_GenericBean monID = DB.GetMonedaIDbyCuentaBancaria(cuentaID);
        lb_moneda.SelectedValue = monID.intC1.ToString();
    }

    protected void btn_deposito_Click(object sender, EventArgs e)
    {
        if (tb_refNo.Text == null || tb_refNo.Text.Equals("")) {
            WebMsgBox.Show("Es obligatorio que ingrese el numero de referencia bancaria");
            return;
        }
        if (tb_fechadoc.Text == null || tb_fechadoc.Text.Equals(""))
        {
            WebMsgBox.Show("Es obligatorio que ingrese la Fecha del Deposito");
            return;
        }       

        if (tb_monto.Text == null || tb_monto.Text.Equals(""))
        {
            WebMsgBox.Show("Es obligatorio que ingrese el Monto del Deposito");
            return;
        }
        if (lb_bancos.Items.Count < 1)
        {
            WebMsgBox.Show("Debe seleccionar un Banco");
            return;
        }
        if (lb_cuentas_bancarias.Items.Count < 1)
        {
            WebMsgBox.Show("Debe seleccionar una Cuenta Bancaria");
            return;
        }
        //*************************************************************BLOQUEO DE PERIODO*****************************************
        #region bloqueo periodo
        RE_GenericBean bloqueo = null;
        bloqueo = (RE_GenericBean)DB.ObtengoPeriodoBloqueado(user.PaisID);
        DateTime fecha_ultimo_bloqueo = DateTime.Parse(bloqueo.strC1); //fecha del ultimo bloqueo    
        DateTime hoy = DateTime.Today; //fecha del dia de hoy
        int activo = bloqueo.intC3;
        #region formateo fechas
        //string fecha_documento = tb_fechadoc.Text.ToString();
        DateTime fecha_doc = DateTime.Parse(DB.DateFormat(tb_fechadoc.Text.ToString().Substring(0, 10)));
        //DateTime fecha_doc = DateTime.Parse(DB.DateFormat(fecha_documento));
        #endregion
        if ((fecha_doc <= fecha_ultimo_bloqueo) && (activo == 1))
        {
            WebMsgBox.Show("No se puede emitir documentos con fecha seleccionada: " + fecha_doc.ToString().Substring(0, 10) + " menores al cierre de periodo contable: " + fecha_ultimo_bloqueo.ToString().Substring(0, 10));
            return;
        }
        if (fecha_doc > DateTime.Today)
        {
            WebMsgBox.Show("No puede ingresar fechas Adelantadas a la fecha de hoy");
            return;
        }
        #endregion

        RE_GenericBean rgb = new RE_GenericBean();
        decimal monto = 0;
        decimal monto_ingresado = 0;
        decimal monto_equivalente = 0;

        rgb.intC1 = int.Parse(lb_bancos.SelectedValue);
        rgb.strC1 = lb_cuentas_bancarias.SelectedValue;
        rgb.boolC1 = true;//indica que es un ingreso a la cuenta
        rgb.strC2 = tb_refNo.Text.Trim();
        if (DB.existDeposito(rgb.intC1, rgb.strC1.Trim(), rgb.strC2.Trim()))
        {
            WebMsgBox.Show("No se puede ingresar este deposito ya que fue ingresado anteriormente");
            return;
        }
        //if (user.Moneda != int.Parse(lb_moneda.SelectedValue))
        //{
        //    monto_equivalente = Math.Round(decimal.Parse(tb_monto.Text.Trim()), 2);
        //    if (user.Moneda==8)
        //    {
        //        monto_ingresado = Math.Round(monto_equivalente / user.pais.TipoCambio, 2);
        //    }
        //    else
        //    {
        //        monto_ingresado = Math.Round(monto_equivalente * user.pais.TipoCambio, 2);
        //    }
        //}
        //else
        //{
        //    monto_ingresado = Math.Round(decimal.Parse(tb_monto.Text.Trim()), 2);
        //    if (user.Moneda == 8)
        //    {
        //        monto_equivalente = Math.Round(monto_ingresado * user.pais.TipoCambio, 2);
        //    }
        //    else
        //    {
        //        monto_equivalente = Math.Round(monto_ingresado / user.pais.TipoCambio, 2);
        //    }
        //}

        monto_ingresado = Math.Round(decimal.Parse(tb_monto.Text.Trim()), 2);
        if (int.Parse(lb_moneda.SelectedValue) == 8)
        {
            monto_equivalente = Math.Round(monto_ingresado * user.pais.TipoCambio, 2);
        }
        else
        {
            monto_equivalente = Math.Round(monto_ingresado / user.pais.TipoCambio, 2);
        }

        #region Realizar Calculos por Diferencial Cambiario
        GridViewRow row_Diferencial;
        CheckBox chk_Diferencial;
        for (int i = 0; i < gv_cortes.Rows.Count; i++)
        {
            row_Diferencial = gv_cortes.Rows[i];
            if (row_Diferencial.RowType == DataControlRowType.DataRow)
            {
                chk_Diferencial = (CheckBox)row_Diferencial.FindControl("chk_seleccion");
                if (chk_Diferencial.Checked)
                {
                    //if (rgb.arr1 == null) rgb.arr1 = new ArrayList();
                    //rgb.arr1.Add(row_Diferencial.Cells[1].Text);
                    //monto += decimal.Parse(row_Diferencial.Cells[11].Text);
                    string monedaRecibo = "";
                    double Tipo_CambioRECIBO = 0;
                    double Tipo_CambioDP = 0;
                    double valorFECHA_RE = 0;
                    double valorFECHA_DP = 0;
                    double valor_RE = 0;
                    int treID = 0;
                    double diferencial = 0;
                    monedaRecibo = row_Diferencial.Cells[12].Text;
                    treID = int.Parse(row_Diferencial.Cells[13].Text);
                    valor_RE = Convert.ToDouble(row_Diferencial.Cells[4].Text);
                    Tipo_CambioRECIBO = Convert.ToDouble(row_Diferencial.Cells[14].Text);
                    Tipo_CambioDP = Convert.ToDouble(tb_tipo_cambio.Text);
                    if (monedaRecibo == "USD")
                    {
                        valorFECHA_RE = Math.Round(valor_RE * Tipo_CambioRECIBO, 2);
                        valorFECHA_DP = Math.Round(valor_RE * Tipo_CambioDP, 2);
                    }
                    else
                    {
                        valorFECHA_RE = Math.Round(valor_RE / Tipo_CambioRECIBO, 2);
                        valorFECHA_DP = Math.Round(valor_RE / Tipo_CambioDP, 2);
                    }
                    diferencial = Math.Round(valorFECHA_DP - valorFECHA_RE, 2);
                    row_Diferencial.Cells[15].Text = diferencial.ToString();
                }
            }
        }
        #endregion

        GridViewRow row;
        CheckBox chk;
        ArrayList Arr_Diferencial = new ArrayList();
        RE_GenericBean Bean_Diferencial = null;
       
        //Validando valores en la moneda principal
        //if (user.Moneda == int.Parse(lb_moneda.SelectedValue))
        //{
        //    for (int i = 0; i < gv_cortes.Rows.Count; i++)
        //    {
        //        row = gv_cortes.Rows[i];
        //        if (row.RowType == DataControlRowType.DataRow)
        //        {
        //            chk = (CheckBox)row.FindControl("chk_seleccion");
        //            if (chk.Checked)
        //            {
        //                if (rgb.arr1 == null) rgb.arr1 = new ArrayList();
        //                rgb.arr1.Add(row.Cells[1].Text);
        //                monto += decimal.Parse(row.Cells[4].Text);
        //                #region Capturar valor del Diferencial Cambiario
        //                //DIFERENCIAL EN RECIBO PAGO
        //                //factbean.douC10 = Convert.ToDouble(row.Cells[14].Text);//Diferencial
        //                //factbean.intC10 = rgb.intC2;//Moneda Diferencial
        //                //LIBRO DIARIO
        //                Bean_Diferencial = new RE_GenericBean();
        //                Bean_Diferencial.douC10 = Convert.ToDouble(row.Cells[15].Text);//Diferencial
        //                Bean_Diferencial.intC10 = int.Parse(lb_moneda.SelectedValue);//Moneda Diferencial
        //                Bean_Diferencial.intC11 = int.Parse(row.Cells[1].Text);//Recibo Pago ID
        //                Bean_Diferencial.intC12 = int.Parse(row.Cells[13].Text);//Recibo ID


        //                if ((Bean_Diferencial.douC10 > 0) || (Bean_Diferencial.douC10 < 0))
        //                {
        //                    Arr_Diferencial.Add(Bean_Diferencial);
        //                    if (rgb.arr2 == null) rgb.arr2 = new ArrayList();
        //                    {
        //                        rgb.arr2.Add(Bean_Diferencial);
        //                    }
        //                }
        //                #endregion
        //            }
        //        }
        //    }
        //    if (monto > monto_ingresado)
        //    {
        //        WebMsgBox.Show("La suma de todos los montos no puede ser mayor al monto ingresado, por favor revise el monto del deposito o bien los recibos a los que desea aplicar");
        //        return;
        //    }
        //}
        //else //Validando valores en la moneda equivalente
        //{
        //    for (int i = 0; i < gv_cortes.Rows.Count; i++)
        //    {
        //        row = gv_cortes.Rows[i];
        //        if (row.RowType == DataControlRowType.DataRow)
        //        {
        //            chk = (CheckBox)row.FindControl("chk_seleccion");
        //            if (chk.Checked)
        //            {
        //                if (rgb.arr1 == null) rgb.arr1 = new ArrayList();
        //                rgb.arr1.Add(row.Cells[1].Text);
        //                monto += decimal.Parse(row.Cells[11].Text);
        //            }
        //        }
        //    }
        //    if (monto > monto_ingresado)
        //    {
        //        WebMsgBox.Show("La suma de todos los montos no puede ser mayor al monto ingresado, por favor revise el monto del deposito o bien los recibos a los que desea aplicar");
        //        return;
        //    }
        //}

        for (int i = 0; i < gv_cortes.Rows.Count; i++)
        {
            int monedaDEPOSITO = 0;
            int monedaRECIBO = 0;

            monedaDEPOSITO = int.Parse(lb_moneda.SelectedValue);
            row = gv_cortes.Rows[i];
            if (row.RowType == DataControlRowType.DataRow)
            {
                chk = (CheckBox)row.FindControl("chk_seleccion");
                if (chk.Checked)
                {
                    if (rgb.arr1 == null) rgb.arr1 = new ArrayList();
                    rgb.arr1.Add(row.Cells[1].Text);
                    monto += decimal.Parse(row.Cells[4].Text);
                    monedaRECIBO = int.Parse(row.Cells[16].Text);
                    #region Validar Moneda del Deposito vs Moneda del Recibo
                    if (monedaDEPOSITO != monedaRECIBO)
                    {
                        WebMsgBox.Show("No es posible Asociar un Recibo con Moneda .: " + Utility.TraducirMonedaInt(monedaRECIBO) + " a un Depostivo con Moneda .: " + Utility.TraducirMonedaInt(monedaDEPOSITO) + ",  Por Favor asocie recibo(s) con la misma Moneda del Deposito.");
                        return;
                    }
                    #endregion
                    #region Capturar valor del Diferencial Cambiario
                    //DIFERENCIAL EN RECIBO PAGO
                    //factbean.douC10 = Convert.ToDouble(row.Cells[14].Text);//Diferencial
                    //factbean.intC10 = rgb.intC2;//Moneda Diferencial
                    //LIBRO DIARIO
                    Bean_Diferencial = new RE_GenericBean();
                    Bean_Diferencial.douC10 = Convert.ToDouble(row.Cells[15].Text);//Diferencial
                    Bean_Diferencial.intC10 = int.Parse(lb_moneda.SelectedValue);//Moneda Diferencial
                    Bean_Diferencial.intC11 = int.Parse(row.Cells[1].Text);//Recibo Pago ID
                    Bean_Diferencial.intC12 = int.Parse(row.Cells[13].Text);//Recibo ID


                    if ((Bean_Diferencial.douC10 > 0) || (Bean_Diferencial.douC10 < 0))
                    {
                        Arr_Diferencial.Add(Bean_Diferencial);
                        if (rgb.arr2 == null) rgb.arr2 = new ArrayList();
                        {
                            rgb.arr2.Add(Bean_Diferencial);
                        }
                    }
                    #endregion
                }
            }
        }
        if (monto > monto_ingresado)
        {
            WebMsgBox.Show("La suma de todos los montos no puede ser mayor al monto ingresado, por favor revise el monto del deposito o bien los recibos a los que desea aplicar");
            return;
        }
        
        int monID = int.Parse(lb_moneda.SelectedValue);
        int tranID = int.Parse(lb_transaccion.SelectedValue);
        int contaID = int.Parse(Session["Contabilidad"].ToString());
        rgb.intC2 = monID;
        rgb.intC3 = contaID;
        rgb.intC4 = tranID;
        int matOpID = DB.getMatrizOperacionID(tranID, monID, user.PaisID, contaID);
        ArrayList ctas = (ArrayList)DB.getMatrizConfiguracion(matOpID);

        //Asignando los valores que se van a guardar en tabla recibos y libro_diario
        rgb.decC1 = monto;
        rgb.decC2 = Math.Round(decimal.Parse(tb_monto.Text.Trim()), 2); ;
        
        if (int.Parse(lb_moneda.SelectedValue) == 8)
        {
            rgb.decC3 = Math.Round(rgb.decC2 * user.pais.TipoCambio, 2);
        }
        else
        {
            rgb.decC3 = Math.Round(rgb.decC2 / user.pais.TipoCambio, 2);
        }        
        rgb.strC3 = DB.DateFormat(tb_fechadoc.Text);
        rgb.strC4 = tb_observaciones.Text;
        rgb.IP_Address = Request.ServerVariables["REMOTE_ADDR"].ToString();
        user = (UsuarioBean)Session["usuario"];
        int result = -1;
        if (lb_transaccion.SelectedValue != "104")
        {
             result = DB.aplicoDeposito(user, rgb, ctas, 1); //depositos cliente
        }
        else
        {
            result = DB.aplicoDepositoSOA(user, rgb, ctas, 1);//depositos soa
        }
        if (result < 0)
        {
            WebMsgBox.Show("Existio un problema al tratar de guardar la informacion por favor intente de nuevo");
            return;
        }
        else 
        {
            WebMsgBox.Show("El Deposito " + tb_refNo.Text +" se guardo exitosamente");
            lb_transaccion.Enabled = false;
            lb_bancos.Enabled = false;
            lb_cuentas_bancarias.Enabled = false;
            tb_refNo.ReadOnly = true;
            tb_fechadoc.ReadOnly = true;
            tb_monto.ReadOnly = true;
            tb_observaciones.ReadOnly = true;
            gv_cortes.Enabled = false;
            btn_deposito.Enabled = false;
            tb_buscar.Enabled = false;

            //Mostrando la Partida contable generada
            if (lb_transaccion.SelectedValue != "104")
            {
                gv_detalle_partida.DataSource = DB.getPolizaDiario(user, double.Parse(result.ToString()), 17, 0);
                gv_detalle_partida_diferencial.DataSource = DB.getPolizaDiferencial(user, 17, result);
            }
            else 
            {
                gv_detalle_partida.DataSource = DB.getPolizaDiario(user, double.Parse(result.ToString()), 28, 0);
                gv_detalle_partida_diferencial.DataSource = DB.getPolizaDiferencial(user, 28, result);
            }
            
            gv_detalle_partida.DataBind();
            gv_detalle_partida.Rows[gv_detalle_partida.Rows.Count - 1].Font.Bold = true;

            gv_detalle_partida_diferencial.DataBind();
            if (gv_detalle_partida_diferencial.Rows.Count > 0)
            {
                gv_detalle_partida_diferencial.Rows[gv_detalle_partida_diferencial.Rows.Count - 1].Font.Bold = true;
            }
        }
    }

    protected void gv_cortes_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.Cells.Count > 1)
        {
            e.Row.Cells[1].Visible = false;
            e.Row.Cells[5].Visible = false;
            e.Row.Cells[6].Visible = false;
            e.Row.Cells[12].Visible = false;
            e.Row.Cells[13].Visible = false;
            e.Row.Cells[15].Visible = false;
            e.Row.Cells[16].Visible = false;
        }
    }
    protected void gv_cortes_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        DataTable dt1;
        dt1 = (DataTable)ViewState["depositosdt"];
        gv_cortes.DataSource = dt1;
        gv_cortes.PageIndex = e.NewPageIndex;
        gv_cortes.DataBind();
    }
    protected void tb_buscar_Click(object sender, EventArgs e)
    {
        string serie = tb_serie.Text.Trim();
        string corr = tb_correlativo.Text.Trim();
        string where = "";

        if (lb_transaccion.SelectedValue != "104")
        {
            if (serie != null && !serie.Equals("")) where = " and tre_serie='" + serie + "'";
            if (corr != null && !corr.Equals("")) where += " and tre_correlativo='" + corr + "'";
            int tipo_contabilidad = int.Parse(Session["Contabilidad"].ToString());
            DataTable dt1 = (DataTable)DB.getRecibosPendDepositar(user.PaisID, tipo_contabilidad, where, 1);
            ViewState["depositosdt"] = dt1;
            gv_cortes.DataSource = dt1;
            gv_cortes.DataBind();
        }
        else 
        {
            if (serie != null && !serie.Equals("")) where = " and  trc_serie='" + serie + "'";
            if (corr != null && !corr.Equals("")) where += " and trc_correlativo='" + corr + "'";
            int tipo_contabilidad = int.Parse(Session["Contabilidad"].ToString());
            DataTable dt1 = (DataTable)DB.getRecibosPendDepositar(user.PaisID, tipo_contabilidad, where, 2);
            ViewState["depositosdt"] = dt1;
            gv_cortes.DataSource = dt1;
            gv_cortes.DataBind();
 
        }
    }
    protected void tb_fechadoc_TextChanged(object sender, EventArgs e)
    {
        tb_tipo_cambio.Text = DB.getTipoCambioByDay(user, DB.DateFormat(tb_fechadoc.Text)).ToString();
    }
    protected void Page_PreRender(object sender, EventArgs e)
    {
        int i;
        GridViewRow row;
        for (i = 0; i < gv_cortes.Rows.Count; i++)
        {
            row = gv_cortes.Rows[i];
            if (row.RowType == DataControlRowType.DataRow)
            {
                row.Cells[4].BackColor = System.Drawing.Color.FromName("#66CCFF");
                //row.Cells[11].BackColor = System.Drawing.Color.LightCyan;
                
                row.Cells[4].Font.Bold = true;
                row.Cells[11].Font.Bold = true;
                row.Cells[2].HorizontalAlign = HorizontalAlign.Center;
                row.Cells[3].HorizontalAlign = HorizontalAlign.Center;
                row.Cells[4].HorizontalAlign = HorizontalAlign.Right;
                row.Cells[11].HorizontalAlign = HorizontalAlign.Right;
                row.Cells[14].HorizontalAlign = HorizontalAlign.Right;
                row.Cells[15].HorizontalAlign = HorizontalAlign.Right;

                row.Cells[7].Font.Bold = true;
                row.Cells[7].HorizontalAlign = HorizontalAlign.Center;
                row.Cells[7].BackColor = System.Drawing.Color.FromName("#66CCFF");
            }
        }
    }
}

