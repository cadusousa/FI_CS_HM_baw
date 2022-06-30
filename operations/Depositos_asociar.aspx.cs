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
            
        }

       
    }

    protected void lb_bancos_SelectedIndexChanged(object sender, EventArgs e)
    {
        ListItem item = null;
        lb_cuentas_bancarias.Items.Clear();
        //ArrayList arrCtas = (ArrayList)DB.getCuentasBancariasXpais(int.Parse(lb_bancos.SelectedValue), user.PaisID);
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
        if (tb_monto.Text == null || tb_monto.Text.Equals(""))
        {
            WebMsgBox.Show("Es obligatorio que ingrese el monto bancario");
            return;
        }

        #region Validar Cierre de Peridos Contables
        //*************************************************************BLOQUEO DE PERIODO*****************************************
        #region bloqueo periodo
        RE_GenericBean bloqueo = null;
        bloqueo = (RE_GenericBean)DB.ObtengoPeriodoBloqueado(user.PaisID);
        DateTime fecha_ultimo_bloqueo = DateTime.Parse(bloqueo.strC1); //fecha del ultimo bloqueo    
        DateTime hoy = DateTime.Today; //fecha del dia de hoy
        int activo = bloqueo.intC3;
        #region formateo fechas
        DateTime fecha_doc = DateTime.Parse(DB.DateFormat(tb_fecha_deposito.Text.ToString().Substring(0, 10)));
        #endregion
        if ((fecha_doc <= fecha_ultimo_bloqueo) && (activo == 1))
        {
            WebMsgBox.Show("No se puede asociar el Deposito #: " + tb_refNo.Text + " con Fecha: de Emision: " + fecha_doc.ToString().Substring(0, 10) + ", porque el periodo contable se encuentra cerrado hasta la Fecha: " + fecha_ultimo_bloqueo.ToString().Substring(0, 10) + "");
            return;
        }
        #endregion
        #endregion

        RE_GenericBean rgb = new RE_GenericBean();
        decimal monto = 0, monto_ingresado=decimal.Parse(tb_monto.Text.Trim());
        rgb.intC1=int.Parse(lb_bancos.SelectedValue);
        rgb.strC1 = lb_cuentas_bancarias.SelectedValue;
        rgb.boolC1 = true;//indica que es un ingreso a la cuenta
        rgb.strC2 = tb_refNo.Text;
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

        rgb.decC1 = monto;
        user = (UsuarioBean)Session["usuario"];
        int result = -1;
        if (lb_transaccion.SelectedValue != "104")
        {
           result = DB.aplicoDeposito(user, rgb, ctas, 0);
        }
        else
        {
            result = DB.aplicoDepositoSOA(user, rgb, ctas, 0);
        }
        if (result < 0)
        {
            WebMsgBox.Show("Existio un problema al tratar de guardar la informacion por favor intente de nuevo");
            return;
        }
        else 
        {
            WebMsgBox.Show("El Deposito " + tb_refNo.Text + " se asigno exitosamente");
            gv_cortes.Enabled = false;
            btn_deposito.Enabled = false;

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
            if (serie != null && !serie.Equals("")) where = " and trc_serie='" + serie + "'";
            if (corr != null && !corr.Equals("")) where += " and trc_correlativo='" + corr + "'";
            int tipo_contabilidad = int.Parse(Session["Contabilidad"].ToString());
            DataTable dt1 = (DataTable)DB.getRecibosPendDepositar(user.PaisID, tipo_contabilidad, where, 2);
            ViewState["depositosdt"] = dt1;
            gv_cortes.DataSource = dt1;
            gv_cortes.DataBind();
        }

    }
    protected void btn_cargar_Click(object sender, EventArgs e)
    {
        int banID = int.Parse(lb_bancos.SelectedValue);
        string cta = lb_cuentas_bancarias.SelectedValue;
        string refID = tb_refNo.Text;
        if (lb_transaccion.SelectedValue != "104")
        {
            if (!DB.existDeposito(banID, cta, refID))
            {
                WebMsgBox.Show("El deposito solicitado no existe, por favor verifique que sea correcto");
                return;
            }
            else
            {
                RE_GenericBean Bean_Deposito = DB.getDepositoData_By_Cuenta(banID, cta, refID);
                tb_fecha_deposito.Text = Bean_Deposito.strC6;
                tb_tipo_cambio.Text = Bean_Deposito.strC8;
                DataTable dt1 = (DataTable)DB.getRecibosAplicaDeposito(banID, cta, refID, 1);
                ViewState["depositosdt"] = dt1;
                gv_recibosaplicados.DataSource = dt1;
                gv_recibosaplicados.DataBind();
                if ((user.PaisID == 1) || (user.PaisID == 5) || (user.PaisID == 7) || (user.PaisID == 15) || (user.PaisID == 21) || (user.PaisID == 4) || (user.PaisID == 11) || (user.PaisID == 12) || (user.PaisID == 13) || (user.PaisID == 24) || (user.PaisID == 3) || (user.PaisID == 23))//BAW FISCAL USD
                {
                    //Mostrar el Saldo del Deposito en la Moneda de la Cuenta Bancaria
                    tb_monto.Text = DB.getSaldoDeposito(banID, cta, refID, 0, 1).ToString();
                }
                else
                {
                    if (user.Moneda == int.Parse(lb_moneda.SelectedValue))
                    {
                        tb_monto.Text = DB.getSaldoDeposito(banID, cta, refID, 0, 1).ToString();
                    }
                    else
                    {
                        tb_monto.Text = DB.getSaldoDeposito(banID, cta, refID, 1, 1).ToString();
                    }
                }
                tb_usu_creador.Text = DB.getusuarioDeposito(banID, cta, refID,1).ToString();
            }
        }
        else
        {
            //SOA
            if (!DB.existDeposito(banID, cta, refID))
            {
                WebMsgBox.Show("El deposito solicitado no existe, por favor verifique que sea correcto");
                return;
            }
            else
            {
                RE_GenericBean Bean_Deposito = DB.getDepositoData_By_Cuenta(banID, cta, refID);
                tb_fecha_deposito.Text = Bean_Deposito.strC6;
                tb_tipo_cambio.Text = Bean_Deposito.strC8;
                DataTable dt1 = (DataTable)DB.getRecibosAplicaDeposito(banID, cta, refID, 2);
                ViewState["depositosdt"] = dt1;
                gv_recibosaplicados.DataSource = dt1;
                gv_recibosaplicados.DataBind();

                if ((user.PaisID == 1) || (user.PaisID == 5) || (user.PaisID == 7) || (user.PaisID == 15) || (user.PaisID == 21) || (user.PaisID == 4) || (user.PaisID == 11) || (user.PaisID == 12) || (user.PaisID == 13) || (user.PaisID == 24) || (user.PaisID == 3) || (user.PaisID == 23))//BAW FISCAL USD
                {
                    tb_monto.Text = DB.getSaldoDeposito(banID, cta, refID, 0, 2).ToString();
                }
                else
                {
                    if (user.Moneda == int.Parse(lb_moneda.SelectedValue))
                    {
                        tb_monto.Text = DB.getSaldoDeposito(banID, cta, refID, 0, 2).ToString();
                    }
                    else
                    {
                        tb_monto.Text = DB.getSaldoDeposito(banID, cta, refID, 1, 2).ToString();
                    }
                }
                tb_usu_creador.Text = DB.getusuarioDeposito(banID, cta, refID,2).ToString();
            }
        }
    }
    protected void lb_transaccion_SelectedIndexChanged(object sender, EventArgs e)
    {
        tb_refNo.Text = "";
        tb_monto.Text = "";
        tb_observaciones.Text = "";
        tb_usu_creador.Text = "";
        gv_recibosaplicados.DataSource = null;
        gv_recibosaplicados.DataBind();
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
