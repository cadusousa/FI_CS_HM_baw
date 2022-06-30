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

public partial class invoice_showrcpt : System.Web.UI.Page
{
    UsuarioBean user = null;
    int rcpt = 0;
    decimal abono = 0;
    string simbolomoneda = "USD"; 
    string simboloequivalente="GTQ"; 
    protected void Page_Load(object sender, EventArgs e)
    {
        
        RE_GenericBean rgb = null;
        if (Session["usuario"] == null)
        {
            Response.Redirect("../default.aspx");
        }
        user = (UsuarioBean)Session["usuario"];
        btn_imprimir.Enabled = true;
        abono = decimal.Parse(tb_monto.Text.Trim());
        if (!Page.IsPostBack)
        {
            lb_banco.Items.Clear();

            ListItem item = null;
            // Cargo los tipos de operacion
            ArrayList arr = (ArrayList)DB.getOpcionesMatriz("tbl_tipo_transaccion where ttt_template='showrcpt.aspx'");

            //Cargo los bancos disponibles
            ArrayList banco_arr = (ArrayList)DB.getBancos(null, user);
            item = new ListItem("Seleccione", "0");
            lb_banco.Items.Add(item);
            foreach (RE_GenericBean rgb1 in banco_arr)
            {
                item = new ListItem(rgb1.strC1, rgb1.intC1.ToString());
                lb_banco.Items.Add(item);
            }
            // Cargo las monedas del pais
            arr = (ArrayList)DB.getMonedasbyPais(user.PaisID, user.contaID);
            foreach (RE_GenericBean rgb1 in arr)
            {
                item = new ListItem(rgb1.strC1, rgb1.intC1.ToString());
                lbMoneda.Items.Add(item);
            }
            if ((Request.QueryString["rcptID"] != null) && (!Request.QueryString["rcptID"].ToString().Equals("")))
            {
                rcpt = int.Parse(Request.QueryString["rcptID"].ToString().Trim());
                rcptID.Text = rcpt.ToString();
                rgb = (RE_GenericBean)DB.getRcptData(rcpt); //obtengo los datos del recibo
                tb_serierecibo.Text = rgb.strC32;
                tb_correlativo.Text = rgb.strC33;
                lbMoneda.SelectedValue = rgb.intC4.ToString();
                if (rgb.intC4==8) { simbolomoneda = "USD"; simboloequivalente = "LOCAL"; }
                else if (rgb.intC4 != 1) { simbolomoneda = "GTQ"; simboloequivalente = "USD"; }
                else if (rgb.intC4 != 2) { simbolomoneda = "SVC"; simboloequivalente = "USD"; }
                else if (rgb.intC4 != 3) { simbolomoneda = "HNL"; simboloequivalente = "USD"; }
                else if (rgb.intC4 != 4) { simbolomoneda = "NIC"; simboloequivalente = "USD"; }
                else if (rgb.intC4 != 5) { simbolomoneda = "CRC"; simboloequivalente = "USD"; }
                else if (rgb.intC4 != 6) { simbolomoneda = "PAB"; simboloequivalente = "USD"; }
                else if (rgb.intC4 != 7) { simbolomoneda = "BZD"; simboloequivalente = "USD"; }
                
                rgb.decC1 = DB.getTotalAbonadoXRcpt(rcpt, 2);//obtengo todo lo abonado por el recibo tipo2 porque es recibo
                // Obtengo los pagos recibidos en el documento
                gv_pagos.DataSource = (DataTable)DB.getPagosRecibo(rcpt);
                gv_pagos.DataBind();                
                // Obtengo el arreglo de facturas que el recibo mato o aplico
                dgw_aplicada.DataSource = (DataTable)DB.getRcptFacturas(rcpt);
                dgw_aplicada.DataBind();
                // Obtengo el arreglo de NotasDebito que el recibo mato o aplico
                gv_nd_abonadas.DataSource = (DataTable)DB.getRcptNotaDebito(rcpt);
                gv_nd_abonadas.DataBind();

                //Obtengo el arreglo de facturas pendientes de este client
                //DataSet ds = (DataSet)DB.getFacturasbyCliente(rgb.douC1, false, int.Parse(lbMoneda.SelectedValue), user);//false indica que solo hala las activas
                DataSet ds = (DataSet)DB.Get_FacturasbyCliente_Conta_Moneda(rgb.douC1, false, int.Parse(lbMoneda.SelectedValue), user);//false indica que solo hala las que no estan pagadas ni anuladas
                dgw.DataSource = ds.Tables["fact"];
                dgw.DataBind();

                ////Obtengo el arreglo de Notas Debito pendientes de este cliente
                //ds = (DataSet)DB.getNotasDebitoforRcpt((int)rgb.douC1, 3, rgb.intC4, false, user); //3 porque el tpi es cliente
                ds = (DataSet)DB.Get_NotasDebitoforRcptBy_Conta_Moneda((int)rgb.douC1, 3, int.Parse(lbMoneda.SelectedValue), false, user); //3 porque el tpi es cliente
                gv_notadebito.DataSource = ds.Tables["fact"];
                gv_notadebito.DataBind();




                tb_fecha.Text = rgb.strC5;
                lb_banco.SelectedValue = rgb.intC3.ToString();
                abono = Math.Round(rgb.decC3 - rgb.decC1, 2);
                tb_monto.Text = abono.ToString();
                tb_nota.Text = rgb.strC7;
                tb_contenedor.Text = rgb.strC11;
                tb_hbl.Text = rgb.strC9;
                tb_referencia.Text = rgb.strC10;
                tb_roe.Text = rgb.strC34;

                //Obtengo los datos del cliente
                string crite = "a.id_cliente=" + rgb.douC1;
                ArrayList clientearr = (ArrayList)DB.getClientes(crite, user, "");
                if ((clientearr != null) && (clientearr.Count > 0))
                {
                    // si entro aqui es porque encontre datos del cliente
                    RE_GenericBean clienteBean = (RE_GenericBean)clientearr[0];
                    tb_clientid.Text = clienteBean.douC1.ToString();
                    tb_clientname.Text = clienteBean.strC1;
                    tb_recibonombre.Text = clienteBean.strC2;
                }

                int resultado = DB.getEstadoRecibo(rcpt);
                if (resultado == 3)
                {
                    bt_Enviar.Enabled = false;
                    btn_imprimir.Enabled = false;
                    btn_reimprimir.Enabled = false;
                    WebMsgBox.Show("Recibo Anulado");
                    return;
                }
                else if (resultado == -100)
                {
                    bt_Enviar.Enabled = false;
                    btn_imprimir.Enabled = false;
                    btn_reimprimir.Enabled = false;
                    WebMsgBox.Show("Se ha producido un error al tratar de determinar el estado del recibo");
                    return;
                }

                gv_detalle_partida.DataSource = DB.getPolizaDiario(user, double.Parse(rcptID.Text), 2, 0);
                gv_detalle_partida.DataBind();
                if (gv_detalle_partida.Rows.Count > 0)
                {
                    gv_detalle_partida.Rows[gv_detalle_partida.Rows.Count - 1].Font.Bold = true;
                }

                gv_detalle_partida_diferencial.DataSource = DB.getPolizaDiferencial(user, 2, int.Parse(rcptID.Text));
                gv_detalle_partida_diferencial.DataBind();
                if (gv_detalle_partida_diferencial.Rows.Count > 0)
                {
                    gv_detalle_partida_diferencial.Rows[gv_detalle_partida_diferencial.Rows.Count - 1].Font.Bold = true;
                }

            }
        }
    }

    protected void bt_Enviar_Click(object sender, EventArgs e)
    {
        int ban = 0;
        decimal monto_aplicar = 0;
        ArrayList Arrfact_aplicar = new ArrayList();
        ArrayList Arrfact_aplicar_ND = new ArrayList();
        ArrayList Arr_Diferencial = new ArrayList();
        RE_GenericBean Bean_Diferencial = null;
        RE_GenericBean factbean = null;
        GridViewRow row;
        TextBox t1;
        int contaID = int.Parse(Session["Contabilidad"].ToString());//tipo de contabilidad
        int cliID = int.Parse(tb_clientid.Text);
        decimal saldo = 0;
        decimal montoabonar = 0, dolaresactuales = 0, colonesactuales = 0;
        decimal totalfactdolar = 0;
        decimal totalfact = 0;
        decimal TipoCambioFactura = 0;
        decimal valoractual = 0, valorantiguo = 0;
        decimal diferencial = 0;
        decimal total_abonos = 0;
        int monedadiferencial = 0;
        int estado = 0;
         decimal saldo_auxiliar = 0;
                            decimal montoabonar_auxiliar = 0;
        if ((tb_clientid.Text.Trim() == "") || (tb_clientid.Text.Trim() == "0"))
        {
            WebMsgBox.Show("Debe indicar el Cliente del Recibo");
        }
        else if ((tb_monto.Text == "") || (tb_monto.Text == "0"))
        {
            WebMsgBox.Show("No hay monto disponible para Aplicar.");
        }
        else if ((tb_monto.Text.Trim() != "") || (tb_monto.Text.Trim() != "0"))
        {
            monto_aplicar = decimal.Parse(tb_monto.Text);
            for (int i = 0; i < dgw.Rows.Count; i++)
            {
                row = dgw.Rows[i];
                if (row.RowType == DataControlRowType.DataRow)
                {
                    t1 = (TextBox)row.FindControl("TextBox1");
                    if (t1.Text == "")
                    { t1.Text = "0"; }
                    if (decimal.Parse(t1.Text) > 0)
                    {
                        ban++;
                        if (lbMoneda.SelectedValue.Equals("8"))
                        {
                            if (decimal.Parse(t1.Text) <= decimal.Parse(row.Cells[9].Text))
                            {
                                monto_aplicar = monto_aplicar - decimal.Parse(t1.Text);
                                total_abonos += decimal.Parse(t1.Text);
                            }
                            else if (decimal.Parse(t1.Text) > decimal.Parse(row.Cells[9].Text))
                            {
                                WebMsgBox.Show("Esta aplicando un abono mayor que el Saldo");
                                return;
                            }
                        }
                        else
                        {
                            if (decimal.Parse(t1.Text) <= decimal.Parse(row.Cells[9].Text))
                            {
                                monto_aplicar = monto_aplicar - decimal.Parse(t1.Text);
                                total_abonos += decimal.Parse(t1.Text);
                            }
                            else if (decimal.Parse(t1.Text) > decimal.Parse(row.Cells[9].Text))
                            {
                                WebMsgBox.Show("Esta aplicando un abono mayor que el Saldo");
                                return;
                            }
                        }
                    }
                }
            }
            for (int i = 0; i < gv_notadebito.Rows.Count; i++)
            {
                row = gv_notadebito.Rows[i];
                if (row.RowType == DataControlRowType.DataRow)
                {
                    t1 = (TextBox)row.FindControl("tb_descND");
                    if (t1.Text == "")
                    { t1.Text = "0"; }
                    if (decimal.Parse(t1.Text) > 0)
                    {
                        ban++;
                        if (lbMoneda.SelectedValue.Equals("8"))
                        {
                            if (decimal.Parse(t1.Text) <= decimal.Parse(row.Cells[9].Text))
                            {
                                monto_aplicar = monto_aplicar - decimal.Parse(t1.Text);
                                total_abonos += decimal.Parse(t1.Text);
                            }
                            else if (decimal.Parse(t1.Text) > decimal.Parse(row.Cells[9].Text))
                            {
                                WebMsgBox.Show("Esta aplicando un abono mayor que el Saldo");
                                return;
                            }
                        }
                        else
                        {
                            if (decimal.Parse(t1.Text) <= decimal.Parse(row.Cells[9].Text))
                            {
                                monto_aplicar = monto_aplicar - decimal.Parse(t1.Text);
                                total_abonos += decimal.Parse(t1.Text);
                            }
                            else if (decimal.Parse(t1.Text) > decimal.Parse(row.Cells[9].Text))
                            {
                                WebMsgBox.Show("Esta aplicando un abono mayor que el Saldo");
                                return;
                            }
                        }


                    }
                }
            }
            if (ban == 0)
            {
                WebMsgBox.Show("Debe indicar al menos un documento para abonar");
            }
            else if (ban > 0)
            {
                if (monto_aplicar < 0)
                {
                    WebMsgBox.Show("Esta aplicando un monto mayor al disponible");
                }
                else if (monto_aplicar >= 0)
                {
                    rcpt = int.Parse(rcptID.Text.Trim());
                    #region Definir estado del Recibo
                    if (decimal.Parse(tb_monto.Text) > total_abonos)
                    {
                        estado = 2;
                    }
                    else if (decimal.Parse(tb_monto.Text) == total_abonos)
                    {
                        estado = 4;
                    }
                    #endregion
                    for (int i = 0; i < dgw.Rows.Count; i++)
                    {
                        row = dgw.Rows[i];
                        factbean = new RE_GenericBean();

                        #region Definir Estado de la Factura
                        t1 = (TextBox)row.FindControl("TextBox1");
                        montoabonar_auxiliar = decimal.Parse(t1.Text);
                        if (lbMoneda.SelectedValue.Equals("8"))
                        {
                            saldo_auxiliar = decimal.Parse(row.Cells[9].Text);
                        }
                        else
                        {
                            saldo_auxiliar = decimal.Parse(row.Cells[9].Text);
                        }
                        if (montoabonar_auxiliar == saldo_auxiliar)
                        {
                            factbean.Estado = 4;
                        }
                        else if (montoabonar_auxiliar < saldo_auxiliar)
                        {
                            factbean.Estado = 2;
                        }
                        #endregion
                        
                        if (row.RowType == DataControlRowType.DataRow)
                        {
                            totalfact = decimal.Parse(row.Cells[7].Text);
                            totalfactdolar = decimal.Parse(row.Cells[10].Text);

                            t1 = (TextBox)row.FindControl("TextBox1");

                            montoabonar = decimal.Parse(t1.Text);
                            factbean.intC1 = int.Parse(row.Cells[1].Text);//fact id
                            factbean.intC2 = Utility.TraducirMonedaStr(row.Cells[6].Text.Trim());//Moneda en que se facturo
                            TipoCambioFactura = DB.getTipoCambioFactura(factbean.intC1);

                            if (factbean.intC2 == 8)
                            { // si la factura o documento viene en dolares
                                if (factbean.intC2.ToString().Equals(lbMoneda.SelectedValue))
                                { // si lamoneda del docto SI es igual a la del recibo
                                    valorantiguo = TipoCambioFactura * montoabonar;
                                    valoractual = user.pais.TipoCambio * montoabonar;
                                    diferencial = valoractual - valorantiguo;
                                }
                                else
                                { // si la moneda del docto no es igual a la del recibo
                                    dolaresactuales = montoabonar / user.pais.TipoCambio;
                                    valorantiguo = dolaresactuales * TipoCambioFactura;
                                    diferencial = montoabonar - valorantiguo;
                                }
                                monedadiferencial = int.Parse(lbMoneda.SelectedValue);
                            }
                            else
                            { // si la factura o documento no viene en dolares
                                if (factbean.intC2.ToString().Equals(lbMoneda.SelectedValue))
                                { // si lamoneda del docto SI es igual a la del recibo
                                    valorantiguo = montoabonar / TipoCambioFactura;
                                    valoractual = montoabonar / user.pais.TipoCambio;
                                    diferencial = valoractual - valorantiguo;
                                }
                                else
                                { // si la moneda del docto no es igual a la del recibo
                                    colonesactuales = user.pais.TipoCambio * montoabonar;
                                    valorantiguo = colonesactuales / TipoCambioFactura;
                                    diferencial = montoabonar - valorantiguo;
                                }
                                monedadiferencial = 8;
                            }

                            if (lbMoneda.SelectedValue.Equals("8"))
                            { //si la moneda del recibo es dolares
                                if (lbMoneda.SelectedValue.Equals(factbean.intC2.ToString()))
                                { // si lamoneda del recibo es igual al documento
                                    factbean.decC1 = montoabonar; // monto abonar 
                                    factbean.decC2 = Math.Round((montoabonar * user.pais.TipoCambio), 2);//monto a abonar equivalente
                                    saldo = decimal.Parse(row.Cells[9].Text);
                                    if (decimal.Parse(t1.Text.Trim()) == saldo)
                                        factbean.boolC1 = true;
                                }
                                else
                                { // si la moneda del recibo no es igual al documento
                                    factbean.decC1 = Math.Round((montoabonar * user.pais.TipoCambio), 2);  // monto abonar 
                                    factbean.decC2 = montoabonar;//monto a abonar equivalente
                                    saldo = decimal.Parse(row.Cells[9].Text);
                                    if (decimal.Parse(t1.Text.Trim()) == saldo)
                                        factbean.boolC1 = true;
                                }
                            }
                            else
                            {
                                if (lbMoneda.SelectedValue.Equals(factbean.intC2.ToString()))
                                { // si lamoneda del recibo es igual al documento
                                    factbean.decC1 = montoabonar; // monto abonar 
                                    factbean.decC2 = Math.Round((montoabonar / user.pais.TipoCambio), 2);//monto a abonar equivalente
                                    saldo = decimal.Parse(row.Cells[9].Text);
                                    if (decimal.Parse(t1.Text.Trim()) == saldo)
                                        factbean.boolC1 = true;
                                }
                                else
                                { // si la moneda del recibo no es igual al documento
                                    factbean.decC1 = Math.Round((montoabonar / user.pais.TipoCambio), 2);  // monto abonar 
                                    factbean.decC2 = montoabonar;//monto a abonar equivalente
                                    saldo = decimal.Parse(row.Cells[9].Text);
                                    if (decimal.Parse(t1.Text.Trim()) == saldo)
                                        factbean.boolC1 = true;
                                }

                            }
                            factbean.decC3 = diferencial;
                            factbean.decC3 = 0;
                            factbean.intC3 = monedadiferencial;
                            #region Capturar valor del Diferencial Cambiario
                            //FACTURA ABONO
                            factbean.douC10 = Convert.ToDouble(row.Cells[14].Text);//Diferencial
                            factbean.intC10 = int.Parse(lbMoneda.SelectedValue);//Moneda Diferencial
                            //LIBRO DIARIO
                            Bean_Diferencial = new RE_GenericBean();
                            Bean_Diferencial.douC10 = Convert.ToDouble(row.Cells[14].Text);//Diferencial
                            Bean_Diferencial.intC10 = int.Parse(lbMoneda.SelectedValue);//Moneda Diferencial
                            #endregion
                        }
                        if (factbean.decC1 > 0)
                            Arrfact_aplicar.Add(factbean);

                        if ((Bean_Diferencial.douC10 > 0) || (Bean_Diferencial.douC10 < 0))
                        {
                            Arr_Diferencial.Add(Bean_Diferencial);
                        }

                    }

                    //**************************************************************** notas de debito
                    for (int i = 0; i < gv_notadebito.Rows.Count; i++)
                    {
                        row = gv_notadebito.Rows[i];
                        factbean = new RE_GenericBean();

                        #region Definir Estado de la Nota Debito
                        t1 = (TextBox)row.FindControl("tb_descND");
                        montoabonar_auxiliar = decimal.Parse(t1.Text);
                        if (lbMoneda.SelectedValue.Equals("8"))
                        {
                            saldo_auxiliar = decimal.Parse(row.Cells[9].Text);
                        }
                        else
                        {
                            saldo_auxiliar = decimal.Parse(row.Cells[9].Text);
                        }
                        if (montoabonar_auxiliar == saldo_auxiliar)
                        {
                            factbean.Estado = 4;
                        }
                        else if (montoabonar_auxiliar < saldo_auxiliar)
                        {
                            factbean.Estado = 2;
                        }
                        #endregion

                        if (row.RowType == DataControlRowType.DataRow)
                        {
                            totalfact = decimal.Parse(row.Cells[7].Text);
                            totalfactdolar = decimal.Parse(row.Cells[10].Text);

                            t1 = (TextBox)row.FindControl("tb_descND");
                            montoabonar = decimal.Parse(t1.Text);
                            factbean.intC1 = int.Parse(row.Cells[1].Text);//fact id
                            factbean.intC2 = Utility.TraducirMonedaStr(row.Cells[6].Text.Trim());//Moneda en que se facturo
                            TipoCambioFactura = DB.getTipoCambioNotaDebito(factbean.intC1);

                            if (factbean.intC2 == 8)
                            { // si la factura o documento viene en dolares
                                if (factbean.intC2.ToString().Equals(lbMoneda.SelectedValue))
                                { // si lamoneda del docto SI es igual a la del recibo
                                    valorantiguo = TipoCambioFactura * montoabonar;
                                    valoractual = user.pais.TipoCambio * montoabonar;
                                    diferencial = valoractual - valorantiguo;
                                }
                                else
                                { // si la moneda del docto no es igual a la del recibo
                                    dolaresactuales = montoabonar / user.pais.TipoCambio;
                                    valorantiguo = dolaresactuales * TipoCambioFactura;
                                    diferencial = montoabonar - valorantiguo;
                                }
                                monedadiferencial = int.Parse(lbMoneda.SelectedValue);
                            }
                            else
                            { // si la factura o documento no viene en dolares
                                if (factbean.intC2.ToString().Equals(lbMoneda.SelectedValue))
                                { // si lamoneda del docto SI es igual a la del recibo
                                    valorantiguo = montoabonar / TipoCambioFactura;
                                    valoractual = montoabonar / user.pais.TipoCambio;
                                    diferencial = valoractual - valorantiguo;
                                }
                                else
                                { // si la moneda del docto no es igual a la del recibo
                                    colonesactuales = user.pais.TipoCambio * montoabonar;
                                    valorantiguo = colonesactuales / TipoCambioFactura;
                                    diferencial = montoabonar - valorantiguo;
                                }
                                monedadiferencial = 8;
                            }

                            if (lbMoneda.SelectedValue.Equals("8"))
                            { //si la moneda del recibo es dolares
                                if (lbMoneda.SelectedValue.Equals(factbean.intC2.ToString()))
                                { // si lamoneda del recibo es igual al documento
                                    factbean.decC1 = montoabonar; // monto abonar 
                                    factbean.decC2 = Math.Round((montoabonar * user.pais.TipoCambio), 2);//monto a abonar equivalente
                                    saldo = decimal.Parse(row.Cells[9].Text);
                                    if (decimal.Parse(t1.Text.Trim()) == saldo)
                                        factbean.boolC1 = true;
                                }
                                else
                                { // si la moneda del recibo no es igual al documento
                                    factbean.decC1 = Math.Round((montoabonar * user.pais.TipoCambio), 2);  // monto abonar 
                                    factbean.decC2 = montoabonar;//monto a abonar equivalente
                                    saldo = decimal.Parse(row.Cells[9].Text);
                                    if (decimal.Parse(t1.Text.Trim()) == saldo)
                                        factbean.boolC1 = true;
                                }
                            }
                            else
                            {
                                if (lbMoneda.SelectedValue.Equals(factbean.intC2.ToString()))
                                { // si lamoneda del recibo es igual al documento
                                    factbean.decC1 = montoabonar; // monto abonar 
                                    factbean.decC2 = Math.Round((montoabonar / user.pais.TipoCambio), 2);//monto a abonar equivalente
                                    saldo = decimal.Parse(row.Cells[9].Text);
                                    if (decimal.Parse(t1.Text.Trim()) == saldo)
                                        factbean.boolC1 = true;
                                }
                                else
                                { // si la moneda del recibo no es igual al documento
                                    factbean.decC1 = Math.Round((montoabonar / user.pais.TipoCambio), 2);  // monto abonar 
                                    factbean.decC2 = montoabonar;//monto a abonar equivalente
                                    saldo = decimal.Parse(row.Cells[9].Text);
                                    if (decimal.Parse(t1.Text.Trim()) == saldo)
                                        factbean.boolC1 = true;
                                }

                            }
                            factbean.decC3 = diferencial;
                            factbean.decC3 = 0;
                            factbean.intC3 = monedadiferencial;

                            #region Capturar valor del Diferencial Cambiario
                            //FACTURA ABONO
                            factbean.douC10 = Convert.ToDouble(row.Cells[14].Text);//Diferencial
                            factbean.intC10 = int.Parse(lbMoneda.SelectedValue);//Moneda Diferencial
                            //LIBRO DIARIO
                            Bean_Diferencial = new RE_GenericBean();
                            Bean_Diferencial.douC10 = Convert.ToDouble(row.Cells[14].Text);//Diferencial
                            Bean_Diferencial.intC10 = int.Parse(lbMoneda.SelectedValue);//Moneda Diferencial
                            #endregion
                        }
                        if (factbean.decC1 > 0)
                            Arrfact_aplicar_ND.Add(factbean);

                        if ((Bean_Diferencial.douC10 > 0) || (Bean_Diferencial.douC10 < 0))
                        {
                            Arr_Diferencial.Add(Bean_Diferencial);
                        }

                    }
                    //********************************************************************************* fin notas debito

                    int result = DB.AplicoRecibo(Arrfact_aplicar, Arrfact_aplicar_ND, rcpt, user, contaID, cliID, int.Parse(lbMoneda.SelectedValue), estado, Arr_Diferencial);
                    if (result == 0)
                    {
                        Session["msg"] = "Error, No se pudieron grabar los datos que ingreso, por favor intente de nuevo.";
                        Session["url"] = "showrcpt.aspx?rcptID=" + rcpt;
                        Response.Redirect("message.aspx");
                    }
                    else if (result == -100)
                    {
                        Session["msg"] = "Error, existió un error al tratar de grabar los datos, por favor intente de nuevo.";
                        Session["url"] = "showrcpt.aspx?rcptID=" + rcpt;
                        Response.Redirect("message.aspx");
                    }
                    else
                    {
                        DataSet ds = (DataSet)DB.Get_FacturasbyCliente_Conta_Moneda(cliID, false, int.Parse(lbMoneda.SelectedValue), user);//false indica que solo hala las que no estan pagadas ni anuladas
                        dgw.DataSource = ds.Tables["fact"];
                        dgw.DataBind();

                        ds = (DataSet)DB.Get_NotasDebitoforRcptBy_Conta_Moneda((int)cliID, 3, int.Parse(lbMoneda.SelectedValue), false, user); //3 porque el tpi es cliente
                        gv_notadebito.DataSource = ds.Tables["fact"];
                        gv_notadebito.DataBind();

                        gv_detalle_partida.DataSource = DB.getPolizaDiario(user, double.Parse(rcpt.ToString()), 2, 0);
                        gv_detalle_partida.DataBind();
                        gv_detalle_partida.Rows[gv_detalle_partida.Rows.Count - 1].Font.Bold = true;



                        RE_GenericBean rgb = (RE_GenericBean)DB.getRcptData(rcpt);
                        rgb.decC1 = DB.getTotalAbonadoXRcpt(rcpt, 2);
                        tb_monto.Text = Math.Round(rgb.decC3 - rgb.decC1, 2).ToString();
                        bt_Enviar.Enabled = false;
                        dgw.Enabled = false;
                        gv_notadebito.Enabled = false;
                        gv_pagos.Enabled = false;

                        dgw_aplicada.DataSource = (DataTable)DB.getRcptFacturas(rcpt);
                        dgw_aplicada.DataBind();
                        gv_nd_abonadas.DataSource = (DataTable)DB.getRcptNotaDebito(rcpt);
                        gv_nd_abonadas.DataBind();

                        gv_detalle_partida_diferencial.DataSource = DB.getPolizaDiferencial(user, 2, rcpt);
                        gv_detalle_partida_diferencial.DataBind();
                        if (gv_detalle_partida_diferencial.Rows.Count > 0)
                        {
                            gv_detalle_partida_diferencial.Rows[gv_detalle_partida_diferencial.Rows.Count - 1].Font.Bold = true;
                        }

                        WebMsgBox.Show("El Pago fue Aplicado Exitosamente.");
                        return;
                    }
                    //Response.Redirect("showrcpt.aspx?rcptID=" + rcpt);
                }
            }
        }
    }
    protected void Page_PreRender(object sender, EventArgs e)
    {
        int i;
        GridViewRow row;
        TextBox t1;
        decimal saldo = 0;
        for (i = 0; i < dgw.Rows.Count; i++)
        {
            row = dgw.Rows[i];
            if (row.RowType == DataControlRowType.DataRow)
            {
                row.Cells[6].BackColor = System.Drawing.Color.LightCyan;
                row.Cells[7].BackColor = System.Drawing.Color.LightCyan;
                row.Cells[8].BackColor = System.Drawing.Color.LightCyan;
                row.Cells[9].BackColor = System.Drawing.Color.LightCyan;
                row.Cells[6].Font.Bold = true;
                row.Cells[7].Font.Bold = true;
                row.Cells[8].Font.Bold = true;
                row.Cells[9].Font.Bold = true;
                row.Cells[3].HorizontalAlign = HorizontalAlign.Center;
                row.Cells[6].HorizontalAlign = HorizontalAlign.Center;
                row.Cells[7].HorizontalAlign = HorizontalAlign.Right;
                row.Cells[8].HorizontalAlign = HorizontalAlign.Right;
                row.Cells[9].HorizontalAlign = HorizontalAlign.Right;
                row.Cells[10].HorizontalAlign = HorizontalAlign.Right;
                row.Cells[11].HorizontalAlign = HorizontalAlign.Right;
                row.Cells[12].HorizontalAlign = HorizontalAlign.Right;
                row.Cells[13].HorizontalAlign = HorizontalAlign.Right;
                row.Cells[14].HorizontalAlign = HorizontalAlign.Right;
            }
        }

        for (i = 0; i < gv_notadebito.Rows.Count; i++)
        {
            row = gv_notadebito.Rows[i];
            if (row.RowType == DataControlRowType.DataRow)
            {
                row.Cells[6].BackColor = System.Drawing.Color.LightCyan;
                row.Cells[7].BackColor = System.Drawing.Color.LightCyan;
                row.Cells[8].BackColor = System.Drawing.Color.LightCyan;
                row.Cells[9].BackColor = System.Drawing.Color.LightCyan;
                row.Cells[6].Font.Bold = true;
                row.Cells[7].Font.Bold = true;
                row.Cells[8].Font.Bold = true;
                row.Cells[9].Font.Bold = true;
                row.Cells[3].HorizontalAlign = HorizontalAlign.Center;
                row.Cells[6].HorizontalAlign = HorizontalAlign.Center;
                row.Cells[7].HorizontalAlign = HorizontalAlign.Right;
                row.Cells[8].HorizontalAlign = HorizontalAlign.Right;
                row.Cells[9].HorizontalAlign = HorizontalAlign.Right;
                row.Cells[10].HorizontalAlign = HorizontalAlign.Right;
                row.Cells[11].HorizontalAlign = HorizontalAlign.Right;
                row.Cells[12].HorizontalAlign = HorizontalAlign.Right;
                row.Cells[13].HorizontalAlign = HorizontalAlign.Right;
                row.Cells[14].HorizontalAlign = HorizontalAlign.Right;
            }
        }
        for (i = 0; i < gv_pagos.Rows.Count; i++)
        {
            row = gv_pagos.Rows[i];
            if (row.RowType == DataControlRowType.DataRow)
            {
                row.Cells[4].BackColor = System.Drawing.Color.LightCyan;
                row.Cells[5].BackColor = System.Drawing.Color.LightCyan;
                row.Cells[4].Font.Bold = true;
                row.Cells[5].Font.Bold = true;
                row.Cells[4].HorizontalAlign = HorizontalAlign.Right;
                row.Cells[5].HorizontalAlign = HorizontalAlign.Center;
            }
        }
    }
    protected void btn_imprimir_Click(object sender, EventArgs e)
    {
        UsuarioBean user = (UsuarioBean)Session["usuario"];
        ArrayList Arr_Perfiles = (ArrayList)DB.getPerfilesxUsu(user.ID, user.PaisID);
        int b_permiso = 0;
        foreach (PerfilesBean Bean in Arr_Perfiles)
        {
            if ((Bean.ID == 8) || (Bean.ID == 9) || (Bean.ID == 10) || (Bean.ID == 12) || (Bean.ID == 18) || (Bean.ID == 34))
            {
                b_permiso = 1;
            }
        }
        if (b_permiso == 0)
        {
            WebMsgBox.Show("Usted no tiene permisos para realizar esta operacion");
            return;
        }
        else
        {
            String csname1 = "PopupScript";
            Type cstype = this.GetType();

            // Get a ClientScriptManager reference from the Page class.
            ClientScriptManager cs2 = Page.ClientScript;
            string script = "window.open('print_2dorecibo.aspx?rcptID=" + rcptID.Text + "','Imprimir','menubar=1,resizable=1,scrollbars=1,toolbar=1,width=750');";

            if (!cs2.IsStartupScriptRegistered(cstype, csname1))
            {
                cs2.RegisterStartupScript(cstype, csname1, script, true);
            }
            btn_imprimir.Enabled = false;
        }
    }
    protected void btn_reimprimir_Click(object sender, EventArgs e)
    {
        //RE_GenericBean rgb = new RE_GenericBean();
        UsuarioBean user = (UsuarioBean)Session["usuario"];
        //rcpt = int.Parse(rcptID.Text.ToString());
       // rgb = (RE_GenericBean)DB.getRcptData(rcpt);
        //string path = DB.getpathImpresion(2, rgb.intC2, rgb.strC32);
        /*if ((user.PaisID == 1) && (path != ""))
        {
            string mensaje = "<script languaje=\"JavaScript\">";
            mensaje += "window.open('../plantillas/impresion.aspx?tipo=2&id=" + rcptID.Text.Trim() + "','Genera_Reporte','menubar=1,resizable=1,scrollbars=1,toolbar=1,width=750');";
            mensaje += "</script>";
            Page.RegisterClientScriptBlock("closewindow", mensaje);
        }
        else
        {*/
        
        ArrayList Arr_Perfiles = (ArrayList)DB.getPerfilesxUsu(user.ID, user.PaisID);
        int b_permiso = 0;
        foreach (PerfilesBean Bean in Arr_Perfiles)
        {
            if ((Bean.ID == 8) || (Bean.ID == 9) || (Bean.ID == 10) || (Bean.ID == 12) || (Bean.ID == 18) || (Bean.ID == 34))
            {
                b_permiso = 1;
            }
        }
        if (b_permiso == 0)
        {
            WebMsgBox.Show("Usted no tiene permisos para realizar esta operacion");
            return;
        }
        else
        {
            String csname1 = "PopupScript";
            Type cstype = this.GetType();

            // Get a ClientScriptManager reference from the Page class.
            ClientScriptManager cs2 = Page.ClientScript;
            string script = "window.open('re_print.aspx?fac_id=" + rcptID.Text.Trim() + "&s=" + tb_serierecibo.Text.Trim() + "&c=" + tb_correlativo.Text.Trim() + "&tipo=2','Imprimir','menubar=1,resizable=1,scrollbars=1,toolbar=1,width=750');";
            user.ImpresionBean.Operacion = "2";
            user.ImpresionBean.Tipo_Documento = "2";
            user.ImpresionBean.Id = rcptID.Text;
            user.ImpresionBean.Impreso = true;
            Session["usuario"] = user;
            if (!cs2.IsStartupScriptRegistered(cstype, csname1))
            {
                cs2.RegisterStartupScript(cstype, csname1, script, true);
            }
        }
        //}
    }
    protected void dgw_aplicada_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.Cells.Count > 1)
        {
            //e.Row.Cells[1].Visible = false;
            //e.Row.Cells[2].Visible = false;
        }

    }
    protected void dgw_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.Cells.Count > 1)
        {
            e.Row.Cells[1].Visible = false;
            e.Row.Cells[14].Visible = false;
        }

    }
    protected void gv_notadebito_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.Cells.Count > 1)
        {
            e.Row.Cells[1].Visible = false;
            e.Row.Cells[14].Visible = false;
        }
    }

    protected void btn_recibo_virtual_Click(object sender, EventArgs e)
    {
        UsuarioBean user = (UsuarioBean)Session["usuario"];
        ArrayList Arr_Perfiles = (ArrayList)DB.getPerfilesxUsu(user.ID, user.PaisID);
        int b_permiso = 0;
        foreach (PerfilesBean Bean in Arr_Perfiles)
        {
            if ((Bean.ID == 8) || (Bean.ID == 9) || (Bean.ID == 10) || (Bean.ID == 12) || (Bean.ID == 18) || (Bean.ID == 34))
            {
                b_permiso = 1;
            }
        }
        if (b_permiso == 0)
        {
            WebMsgBox.Show("Usted no tiene permisos para realizar esta operacion");
            return;
        }
        else
        {
            String csname1 = "PopupScript";
            Type cstype = this.GetType();
            // Get a ClientScriptManager reference from the Page class.
            ClientScriptManager cs2 = Page.ClientScript;
            string script = "window.open('../invoice/template_recibo.aspx?id=" + rcptID.Text.Trim() + "&transaccion=2','Imprimir','menubar=1,resizable=1,scrollbars=1,toolbar=1,width=750');";
            if (!cs2.IsStartupScriptRegistered(cstype, csname1))
            {
                cs2.RegisterStartupScript(cstype, csname1, script, true);
            }
        }
        /*user = (UsuarioBean)Session["usuario"];
        ArrayList Arr_Perfiles = (ArrayList)DB.getPerfilesxUsu(user.ID, user.PaisID);
        int b_permiso = 0;
        foreach (PerfilesBean Bean in Arr_Perfiles)
        {
            if ((Bean.ID == 8) || (Bean.ID == 9) || (Bean.ID == 10) || (Bean.ID == 12) || (Bean.ID == 18))
            {
                b_permiso = 1;
            }
        }
        if (b_permiso == 0)
        {
            WebMsgBox.Show("Usted no tiene permisos para realizar esta operacion");
            return;
        }
        else
        {
            
        }*/
    }
    //protected void TextBox1_TextChanged(object sender, EventArgs e)
    //{
    //    if (lbMoneda.SelectedValue == "8")
    //    {
    //        GridViewRowCollection gvrc = dgw.Rows;
    //        foreach (GridViewRow row in gvrc)
    //        {
    //            double abono = 0;
    //            double abonoHOY_FACTURA = 0;
    //            double abonoFECHA_FACTURA = 0;
    //            double Tipo_CambioRECIBO = 0;
    //            double Tipo_CambioFACTURA = 0;
    //            int monedaFACTURA = 0;
    //            double diferencial = 0;
    //            TextBox tb1 = (TextBox)row.FindControl("TextBox1");
    //            if (tb1.Text.Trim() != "")
    //            {
    //                abono = Convert.ToDouble(tb1.Text.Trim());
    //                if (abono > 0)
    //                {
    //                    Tipo_CambioFACTURA = Convert.ToDouble(row.Cells[13].Text.ToString());
    //                    Tipo_CambioRECIBO = Convert.ToDouble(tb_roe.Text);
    //                    monedaFACTURA = Utility.TraducirMonedaStr(row.Cells[6].Text.ToString());
    //                    if (monedaFACTURA == 8)
    //                    {
    //                        abonoFECHA_FACTURA = Math.Round(abono * Tipo_CambioFACTURA, 2);
    //                        abonoHOY_FACTURA = Math.Round(abono * Tipo_CambioRECIBO, 2);
    //                    }
    //                    else
    //                    {
    //                        abonoFECHA_FACTURA = Math.Round(abono / Tipo_CambioFACTURA, 2);
    //                        abonoHOY_FACTURA = Math.Round(abono / Tipo_CambioRECIBO, 2);
    //                    }
    //                    diferencial = Math.Round(abonoHOY_FACTURA - abonoFECHA_FACTURA, 2);
    //                    row.Cells[14].Text = diferencial.ToString();
    //                }
    //            }
    //        }
    //    }
    //}
    //protected void tb_descND_TextChanged(object sender, EventArgs e)
    //{
    //    if (lbMoneda.SelectedValue == "8")
    //    {
    //        GridViewRowCollection gvrc = gv_notadebito.Rows;
    //        foreach (GridViewRow row in gvrc)
    //        {
    //            double abono = 0;
    //            double abonoFECHA_RE = 0;
    //            double abonoFECHA_ND = 0;
    //            double Tipo_CambioRECIBO = 0;
    //            double Tipo_CambioND = 0;
    //            int monedaFACTURA = 0;
    //            double diferencial = 0;
    //            TextBox tb1 = (TextBox)row.FindControl("tb_descND");
    //            if (tb1.Text.Trim() != "")
    //            {
    //                abono = Convert.ToDouble(tb1.Text.Trim());
    //                if (abono > 0)
    //                {
    //                    Tipo_CambioND = Convert.ToDouble(row.Cells[13].Text.ToString());
    //                    Tipo_CambioRECIBO = Convert.ToDouble(tb_roe.Text);
    //                    monedaFACTURA = Utility.TraducirMonedaStr(row.Cells[6].Text.ToString());
    //                    if (monedaFACTURA == 8)
    //                    {
    //                        abonoFECHA_ND = Math.Round(abono * Tipo_CambioND, 2);
    //                        abonoFECHA_RE = Math.Round(abono * Tipo_CambioRECIBO, 2);
    //                    }
    //                    else
    //                    {
    //                        abonoFECHA_ND = Math.Round(abono / Tipo_CambioND, 2);
    //                        abonoFECHA_RE = Math.Round(abono / Tipo_CambioRECIBO, 2);
    //                    }
    //                    diferencial = Math.Round(abonoFECHA_RE - abonoFECHA_ND, 2);
    //                    row.Cells[14].Text = diferencial.ToString();
    //                }
    //            }
    //        }
    //    }
    //}
    protected void TextBox1_TextChanged(object sender, EventArgs e)
    {
        if (lbMoneda.SelectedValue == "8")
        {
            double TOTAL_abonos = 0;
            double TOTAL_disponible = 0;

            #region Obtener el Total en Abonos en Notas de Debito
            GridViewRowCollection gvrc_nd = gv_notadebito.Rows;
            foreach (GridViewRow row_nd in gvrc_nd)
            {
                TextBox tb_abonoND = (TextBox)row_nd.FindControl("tb_descND");
                if (tb_abonoND.Text.Trim() != "")
                {
                    TOTAL_abonos += Convert.ToDouble(tb_abonoND.Text.Trim());
                }
            }
            #endregion

            GridViewRowCollection gvrc = dgw.Rows;
            foreach (GridViewRow row in gvrc)
            {
                double abono = 0;
                double saldo_DOCUMENTO = 0;
                double abonoHOY_FACTURA = 0;
                double abonoFECHA_FACTURA = 0;
                double Tipo_CambioRECIBO = 0;
                double Tipo_CambioFACTURA = 0;
                int monedaFACTURA = 0;
                double diferencial = 0;
                TextBox tb1 = (TextBox)row.FindControl("TextBox1");

                #region Validar que hayan ingresado un abono valido
                if (tb1.Text.Trim() == "")
                {
                    tb1.Text = "0.00";
                    row.Cells[14].Text = "0.00";
                    //return;
                }
                #endregion

                abono = Convert.ToDouble(tb1.Text.Trim());
                saldo_DOCUMENTO = Convert.ToDouble(row.Cells[9].Text.ToString());

                #region Validar que el Total a Abonar sea Mayor a Cero
                if (abono <= 0)
                {
                    tb1.Text = "0.00";
                    row.Cells[14].Text = "0.00";
                    //return;
                }
                #endregion
                #region Validar que el Total de Abonos no sea Mayor al Total Disponible
                TOTAL_abonos += abono;
                TOTAL_disponible = Convert.ToDouble(tb_monto.Text);
                if (TOTAL_abonos > TOTAL_disponible)
                {
                    tb1.Text = "0.00";
                    row.Cells[14].Text = "0.00";
                    WebMsgBox.Show("El Total en Abonos.: (" + TOTAL_abonos.ToString() + ") no puede ser mayor al Monto Disponible para Aplicar.: (" + TOTAL_disponible + ") ");
                    return;
                }
                #endregion
                #region Validar que el Total a Abonar no se Mayor al Saldo del Documento
                if (abono > saldo_DOCUMENTO)
                {
                    tb1.Text = "0.00";
                    row.Cells[14].Text = "0.00";
                    WebMsgBox.Show("El valor del Abono.: (" + abono.ToString() + ") no puede ser mayor al Saldo del Documento.: (" + saldo_DOCUMENTO.ToString() + ") ");
                    return;
                }
                #endregion

                Tipo_CambioFACTURA = Convert.ToDouble(row.Cells[13].Text.ToString());
                Tipo_CambioRECIBO = Convert.ToDouble(tb_roe.Text);
                monedaFACTURA = Utility.TraducirMonedaStr(row.Cells[6].Text.ToString());
                if (monedaFACTURA == 8)
                {
                    abonoFECHA_FACTURA = Math.Round(abono * Tipo_CambioFACTURA, 2);
                    abonoHOY_FACTURA = Math.Round(abono * Tipo_CambioRECIBO, 2);
                }
                else
                {
                    abonoFECHA_FACTURA = Math.Round(abono / Tipo_CambioFACTURA, 2);
                    abonoHOY_FACTURA = Math.Round(abono / Tipo_CambioRECIBO, 2);
                }
                diferencial = Math.Round(abonoHOY_FACTURA - abonoFECHA_FACTURA, 2);
                row.Cells[14].Text = diferencial.ToString();
            }
        }
    }
    protected void tb_descND_TextChanged(object sender, EventArgs e)
    {
        if (lbMoneda.SelectedValue == "8")
        {
            double TOTAL_abonos = 0;
            double TOTAL_disponible = 0;

            #region Obtener el Total en Abonos en Facturas
            GridViewRowCollection gvrc_FA = dgw.Rows;
            foreach (GridViewRow row_FA in gvrc_FA)
            {
                TextBox tb_abonoFA = (TextBox)row_FA.FindControl("TextBox1");
                if (tb_abonoFA.Text.Trim() != "")
                {
                    TOTAL_abonos += Convert.ToDouble(tb_abonoFA.Text.Trim());
                }
            }
            #endregion

            GridViewRowCollection gvrc = gv_notadebito.Rows;
            foreach (GridViewRow row in gvrc)
            {
                double abono = 0;
                double saldo_DOCUMENTO = 0;
                double abonoFECHA_RE = 0;
                double abonoFECHA_ND = 0;
                double Tipo_CambioRECIBO = 0;
                double Tipo_CambioND = 0;
                int monedaFACTURA = 0;
                double diferencial = 0;
                TextBox tb1 = (TextBox)row.FindControl("tb_descND");

                #region Validar que hayan ingresado un abono valido
                if (tb1.Text.Trim() == "")
                {
                    tb1.Text = "0.00";
                    row.Cells[14].Text = "0.00";
                    return;
                }
                #endregion

                abono = Convert.ToDouble(tb1.Text.Trim());
                saldo_DOCUMENTO = Convert.ToDouble(row.Cells[9].Text.ToString());

                #region Validar que el Total a Abonar sea Mayor a Cero
                if (abono <= 0)
                {
                    tb1.Text = "0.00";
                    row.Cells[14].Text = "0.00";
                    return;
                }
                #endregion
                #region Validar que el Total de Abonos no sea Mayor al Total Disponible
                TOTAL_abonos += abono;
                TOTAL_disponible = Convert.ToDouble(tb_monto.Text);
                if (TOTAL_abonos > TOTAL_disponible)
                {
                    tb1.Text = "0.00";
                    row.Cells[14].Text = "0.00";
                    WebMsgBox.Show("El Total en Abonos.: (" + TOTAL_abonos.ToString() + ") no puede ser mayor al Monto Disponible para Aplicar.: (" + TOTAL_disponible + ") ");
                    return;
                }
                #endregion
                #region Validar que el Total a Abonar no se Mayor al Saldo del Documento
                if (abono > saldo_DOCUMENTO)
                {
                    tb1.Text = "0.00";
                    row.Cells[14].Text = "0.00";
                    WebMsgBox.Show("El valor del Abono.: (" + abono.ToString() + ") no puede ser mayor al Saldo del Documento.: (" + saldo_DOCUMENTO.ToString() + ") ");
                    return;
                }
                #endregion
                Tipo_CambioND = Convert.ToDouble(row.Cells[13].Text.ToString());
                Tipo_CambioRECIBO = Convert.ToDouble(tb_roe.Text);
                monedaFACTURA = Utility.TraducirMonedaStr(row.Cells[6].Text.ToString());
                if (monedaFACTURA == 8)
                {
                    abonoFECHA_ND = Math.Round(abono * Tipo_CambioND, 2);
                    abonoFECHA_RE = Math.Round(abono * Tipo_CambioRECIBO, 2);
                }
                else
                {
                    abonoFECHA_ND = Math.Round(abono / Tipo_CambioND, 2);
                    abonoFECHA_RE = Math.Round(abono / Tipo_CambioRECIBO, 2);
                }
                diferencial = Math.Round(abonoFECHA_RE - abonoFECHA_ND, 2);
                row.Cells[14].Text = diferencial.ToString();
            }
        }
    }
}
