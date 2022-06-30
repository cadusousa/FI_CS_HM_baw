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

public partial class invoice_newrcpt : System.Web.UI.Page
{
    UsuarioBean user = null;
    decimal abono = 0;
    private DataTable dt1;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            lb_fecha_hora.Text = DB.getDateTimeNow();
        }
        if (Session["usuario"] == null)
        {
            Response.Redirect("../default.aspx");
        }

        user = (UsuarioBean)Session["usuario"];
        if (!user.Aplicaciones.Contains("5"))
        {
            Response.Redirect("index.aspx");
        }
        int permiso = int.Parse(user.Aplicaciones["5"].ToString());
        if (!((permiso & 32) == 32) && !((permiso & 64) == 64))
        {
            Response.Redirect("index.aspx");
        }

        double cliID=0;
        ArrayList clientearr=null; //el maximo es de 1 ya que solo 1 cliente debe haber
        RE_GenericBean clienteBean=null;//tendra los datos del cliente;
        string criterio = "";
        
        abono=decimal.Parse(tb_monto.Text.Trim());
        user = (UsuarioBean)Session["usuario"];
        tb_fecha.Text = DateTime.Now.ToShortDateString();
        if (!Page.IsPostBack)
        {
            lb_tipopago.Items.Clear();
            lb_bancos.Items.Clear();

            // Cargo las monedas del pais para Pagos
            ArrayList arr = (ArrayList)DB.getMonedasbyPais(user.PaisID, 0);
            ListItem item = null;
            foreach (RE_GenericBean rgb in arr)
            {
                item = new ListItem(rgb.strC1, rgb.intC1.ToString());
                lb_moneda.Items.Add(item);
                lbMoneda.Items.Add(item);
            }
            #region Setear Moneda de Inicio
            if ((user.PaisID == 1) || (user.PaisID == 14) || (user.PaisID == 15) || (user.PaisID == 16) || (user.PaisID == 18))
            {
                if (user.contaID == 1)
                {
                    lb_moneda.SelectedValue = "1";
                    lbMoneda.SelectedValue = "1";
                }
                else
                {
                    lb_moneda.SelectedValue = "8";
                    lbMoneda.SelectedValue = "8";
                }
            }
            if ((user.PaisID == 2) || (user.PaisID == 9) || (user.PaisID == 26))
            {
                lb_moneda.SelectedValue = "8";
                lbMoneda.SelectedValue = "8";
            }
            if ((user.PaisID == 3) || (user.PaisID == 23))
            {
                if (user.contaID == 1)
                {
                    lb_moneda.SelectedValue = "3";
                    lbMoneda.SelectedValue = "3";
                }
                else
                {
                    lb_moneda.SelectedValue = "8";
                    lbMoneda.SelectedValue = "8";
                }
            }
            if ((user.PaisID == 4) || (user.PaisID == 24))
            {
                lb_moneda.SelectedValue = "8";
                lbMoneda.SelectedValue = "8";
            }
            if ((user.PaisID == 5) || (user.PaisID == 20) || (user.PaisID == 21))
            {
                if (user.contaID == 1)
                {
                    lb_moneda.SelectedValue = "5";
                    lbMoneda.SelectedValue = "5";
                }
                else
                {
                    lb_moneda.SelectedValue = "8";
                    lbMoneda.SelectedValue = "8";
                }
            }
            if ((user.PaisID == 6) || (user.PaisID == 25))
            {
                lb_moneda.SelectedValue = "8";
                lbMoneda.SelectedValue = "8";
            }
            if ((user.PaisID == 7) || (user.PaisID == 22))
            {
                if (user.contaID == 1)
                {
                    lb_moneda.SelectedValue = "7";
                    lbMoneda.SelectedValue = "7";
                }
                else
                {
                    lb_moneda.SelectedValue = "8";
                    lbMoneda.SelectedValue = "8";
                }
            }
            if (user.PaisID == 11)
            {
                if (user.contaID == 1)
                {
                    lb_moneda.SelectedValue = "4";
                    lbMoneda.SelectedValue = "4";
                }
                else
                {
                    lb_moneda.SelectedValue = "8";
                    lbMoneda.SelectedValue = "8";
                }
            }
            if ((user.PaisID == 12) || (user.PaisID == 13))
            {
                lb_moneda.SelectedValue = "8";
                lbMoneda.SelectedValue = "8";
            }
            #endregion
            item = null;
            // Cargo las opciones de pago
            arr = (ArrayList)DB.getOpcionesMatriz("tbl_tipo_transaccion where ttt_template='newrcpt.aspx'");
            foreach (RE_GenericBean rgb in arr)
            {
                item = new ListItem(rgb.strC1, rgb.intC1.ToString());
                lb_tipopago.Items.Add(item);
            }            

            int tipo_contabilidad = int.Parse(Session["Contabilidad"].ToString());
            arr = (ArrayList)DB.getBancos_By_PaisMonedaID(user.PaisID, int.Parse(lb_moneda.SelectedValue), 0);
            lb_bancos.Items.Clear();
            item = new ListItem("Seleccione...", " ");
            lb_bancos.Items.Add(item);
            foreach (RE_GenericBean rgb in arr)
            {
                item = new ListItem(rgb.strC1, rgb.intC1.ToString());
                lb_bancos.Items.Add(item);
            }

            // Cargo el tipo de cambio
            arr = (ArrayList)DB.getTipoCambio(user.PaisID);
            RE_GenericBean rgb1 = (RE_GenericBean)arr[0];
            tb_roe.Text = rgb1.decC1.ToString();
            //Obtengo la series de Recibos sin FISCAL USD ************************************
            //arr = null;
            //if ((user.SucursalID == 15)  || (user.SucursalID == 26) || (user.SucursalID == 47) || (user.SucursalID == 41) || (user.SucursalID == 71))
            //{
            //    arr = (ArrayList)DB.getSerieFactCreditos(user.SucursalID, 2, user, 1);//2 porque es el tipo de documento para Recibos
            //}
            //else
            //{
            //    arr = (ArrayList)DB.getSerieFactbySuc(user.SucursalID, 2, user, 1);//2 porque es el tipo de documento para Recibos
            //}
            //foreach (string valor in arr)
            //{
            //    item = new ListItem(valor, valor);
            //    lb_serie_factura.Items.Add(item);
            //}
            //Obtengo la series de Recibos CON FISCAL USD *************************
            arr = null;
            arr = (ArrayList)DB.Get_Series_By_Monedas_Conta(2, user, 1);
            item = new ListItem("Seleccione...", "0");
            lb_serie_factura.Items.Clear();
            lb_serie_factura.Items.Add(item);
            foreach (RE_GenericBean Bean_Serie in arr)
            {
                item = new ListItem(Bean_Serie.strC1, Bean_Serie.strC2);
                lb_serie_factura.Items.Add(item);
            }

            //Obtengo el codigo del cliente
            if (Request.QueryString["cliID"] != null)
            {
                cliID = double.Parse(Request.QueryString["cliID"].Trim());
                //Obtengo los datos del cliente
                criterio = "a.id_cliente=" + cliID;
                clientearr = (ArrayList)DB.getClientes(criterio, user, "");
                if ((clientearr != null) && (clientearr.Count > 0))
                {
                    clienteBean = (RE_GenericBean)clientearr[0];
                    tb_clientid.Text = clienteBean.douC1.ToString();
                    tb_clientname.Text = clienteBean.strC1;
                    tb_recibonombre.Text = clienteBean.strC2;

                    DataSet ds = (DataSet)DB.Get_FacturasbyCliente_Conta_Moneda(cliID, false, int.Parse(lbMoneda.SelectedValue), user);//false indica que solo hala las que no estan pagadas ni anuladas
                    dgw.DataSource = ds.Tables["fact"];
                    dgw.DataBind();

                    ds = (DataSet)DB.Get_NotasDebitoforRcptBy_Conta_Moneda((int)cliID, 3, int.Parse(lbMoneda.SelectedValue), false, user); //3 porque el tpi es cliente
                    gv_notadebito.DataSource = ds.Tables["fact"];
                    gv_notadebito.DataBind();
                }
            }
        }
        //BAW FISCAL USD
        if ((user.PaisID == 1) || (user.PaisID == 5) || (user.PaisID == 7) || (user.PaisID == 15) || (user.PaisID == 21) || (user.PaisID == 4) || (user.PaisID == 11) || (user.PaisID == 12) || (user.PaisID == 13) || (user.PaisID == 24) || (user.PaisID == 3) || (user.PaisID == 23))//BAW FISCAL USD
        {
            lb_moneda.Enabled = false;
        }
        else
        {
            lb_moneda.Enabled = true;
        }
    }
    protected void bt_Enviar_Click(object sender, EventArgs e)
    {
        GridViewRow row;
        TextBox t1;
        int ban = 0;
        decimal monto_aplicar = 0;
        decimal total_abonos = 0;
        if (lb_serie_factura.Items.Count == 0)
        {
            WebMsgBox.Show("No existe serie definida para esta sucursal");
            return;
        }
        if (lb_serie_factura.SelectedValue == "0")
        {
            WebMsgBox.Show("Por favor seleccione la Serie a utilizar");
            return;
        }
        if (chk_anticipo.Checked == false)
        {
            if ((tb_clientid.Text.Trim() == "") || (tb_clientid.Text.Trim() == "0"))
            {
                WebMsgBox.Show("Debe indicar el Cliente del Recibo");
            }
            else if ((tb_monto.Text == "") || (tb_monto.Text == "0") || (tb_monto.Text == "0.00"))
            {
                WebMsgBox.Show("Debe Indicar monto a Aplicar");
            }
            else if ((tb_monto.Text.Trim() != "") && (tb_monto.Text.Trim() != "0") && (tb_monto.Text != "0.00"))
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
                        //Validacion para Almacen HN, no debe permitir pagos mayores al valor de las facturas
                        if (user.SucursalID==42) {
                            if (monto_aplicar > 0) {
                                WebMsgBox.Show("Esta aplicando un monto mayor al necesario por los documentos");
                                return;
                            }
                        }
                        if (monto_aplicar < 0)
                        {
                            WebMsgBox.Show("Esta aplicando un monto mayor al disponible");
                        }
                        else if (monto_aplicar >= 0)
                        {
                            RE_GenericBean rgb = new RE_GenericBean();
                            #region Definir estado del Recibo
                            if (decimal.Parse(tb_monto.Text) > total_abonos)
                            {
                                rgb.Estado = 2;
                            }
                            else if (decimal.Parse(tb_monto.Text) == total_abonos)
                            {
                                rgb.Estado = 4;
                            }
                            #endregion
                            rgb.Fecha_Hora = lb_fecha_hora.Text;
                            rgb.intC2 = int.Parse(lbMoneda.SelectedValue);//tipo moneda
                            rgb.strC1 = tb_fecha.Text.Trim();//fecha
                            rgb.decC1 = decimal.Parse(tb_roe.Text.Trim());//tipo de cambio
                            rgb.douC1 = double.Parse(tb_clientid.Text.Trim());//id del cliente
                            rgb.strC2 = tb_clientname.Text.Trim().ToUpper();//nombre del cliente
                            rgb.strC3 = tb_recibonombre.Text.Trim().ToUpper();//nombre del recibo
                            rgb.decC2 = decimal.Parse(tb_monto.Text.Trim());//monto a aplicar
                            rgb.strC5 = tb_nota.Text.Trim();//Nota | Observaciones
                            bool anticipo = chk_anticipo.Checked; // anticipo
                            rgb.strC6 = tb_hbl.Text.Trim().ToUpper();// master
                            rgb.strC7 = tb_mbl.Text.Trim().ToUpper(); //hbl
                            rgb.strC8 = tb_routing.Text.Trim().ToUpper(); //Routing
                            rgb.strC10 = tb_contenedor.Text.Trim();//Contenedor
                            rgb.strC9 = lb_serie_factura.SelectedItem.Text;
                            rgb.IP_Address = Request.ServerVariables["REMOTE_ADDR"].ToString();
                            if (lbMoneda.SelectedValue.Equals("8"))
                            {
                                rgb.decC2 = decimal.Parse(tb_monto.Text.Trim()); // monto abonar en moneda a facturar
                                rgb.decC3 = Math.Round((decimal.Parse(tb_monto.Text.Trim()) * user.pais.TipoCambio), 2);//monto a abonar en otra moneda
                            }
                            else
                            {
                                rgb.decC2 = decimal.Parse(tb_monto.Text.Trim()); // monto abonar en moneda a facturar
                                rgb.decC3 = Math.Round((decimal.Parse(tb_monto.Text.Trim()) / user.pais.TipoCambio), 2);//monto a abonar en otra moneda
                            }
                            ArrayList Arrfact_aplicar = new ArrayList();
                            ArrayList Arrfact_aplicar_ND = new ArrayList();
                            ArrayList Arr_Diferencial = new ArrayList();
                            RE_GenericBean factbean = null;
                            RE_GenericBean Bean_Diferencial = null;
                            decimal saldo = 0;
                            decimal saldo_auxiliar = 0;
                            decimal montoabonar_auxiliar = 0;
                            decimal montoabonar = 0, dolaresactuales = 0, colonesactuales = 0;
                            decimal totalfactdolar = 0;
                            decimal totalfact = 0;
                            decimal TipoCambioFactura = 0;
                            decimal valoractual = 0, valorantiguo = 0;
                            decimal diferencial = 0;
                            int monedadiferencial = 0;

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
                                    totalfact = decimal.Parse(row.Cells[7].Text);////////////////////////////////////
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
                                    factbean.intC10 = rgb.intC2;//Moneda Diferencial
                                    //LIBRO DIARIO
                                    Bean_Diferencial = new RE_GenericBean();
                                    Bean_Diferencial.douC10 = Convert.ToDouble(row.Cells[14].Text);//Diferencial
                                    Bean_Diferencial.intC10 = rgb.intC2;//Moneda Diferencial
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
                                #region Definir Estado de la Nota de Debito
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
                                    factbean.intC10 = rgb.intC2;//Moneda Diferencial
                                    //LIBRO DIARIO
                                    Bean_Diferencial = new RE_GenericBean();
                                    Bean_Diferencial.douC10 = Convert.ToDouble(row.Cells[14].Text);//Diferencial
                                    Bean_Diferencial.intC10 = rgb.intC2;//Moneda Diferencial
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
                            
                            //AGREGANDO ARREGLO DE DOCUMENTOS DEL RECIBO
                            rgb.arr1 = Arrfact_aplicar;
                            rgb.arr3 = Arrfact_aplicar_ND;
                            rgb.arr4 = Arr_Diferencial;


                            int contaID = int.Parse(Session["Contabilidad"].ToString());//tipo de contabilidad
                            rgb.intC4 = contaID;//tipo de contabilidad


                            //************************************************** obtengo las cuentas *****************
                            GridViewRowCollection gvr = gv_pagos.Rows;
                            ArrayList pagosarr = new ArrayList();
                            RE_GenericBean pagobean = null;
                            foreach (GridViewRow row1 in gvr)
                            {
                                pagobean = new RE_GenericBean();
                                if (chk_anticipo.Checked)// Obtengo los datos de anticipos
                                {
                                    if (row1.Cells[1].Text.Trim().Equals("Efectivo"))
                                    {
                                        pagobean.intC1 = 1;
                                        pagobean.decC1 = decimal.Parse(row1.Cells[7].Text);//Monto
                                        pagobean.decC2 = decimal.Parse(row1.Cells[8].Text);//Monto Equivalente
                                        pagobean.strC1 = "";
                                        pagobean.strC2 = "";
                                        pagobean.intC2 = 12;
                                        pagobean.intC3 = 12;
                                        pagobean.intC10 = Utility.TraducirMonedaStr(row1.Cells[5].Text);//Moneda Recepcion
                                        pagobean.intC11 = Utility.TraducirMonedaStr(row1.Cells[6].Text);//Moneda de Aplicacion
                                        int matOpID = DB.getMatrizOperacionID(12, rgb.intC2, user.PaisID, contaID);
                                        pagobean.arr1 = (ArrayList)DB.getMatrizConfiguracion(matOpID);
                                    }
                                    else if (row1.Cells[1].Text.Trim().Equals("Cheque"))
                                    {
                                        pagobean.intC1 = 2;
                                        pagobean.decC1 = decimal.Parse(row1.Cells[7].Text);//Monto
                                        pagobean.decC2 = decimal.Parse(row1.Cells[8].Text);//Monto Equivalente
                                        pagobean.strC1 = Page.Server.HtmlDecode(row1.Cells[2].Text);//Banco
                                        pagobean.strC2 = "";
                                        pagobean.strC3 = row1.Cells[4].Text;//Referencia
                                        pagobean.intC2 = 12;
                                        pagobean.intC3 = 30;
                                        pagobean.intC10 = Utility.TraducirMonedaStr(row1.Cells[5].Text);//Moneda Recepcion
                                        pagobean.intC11 = Utility.TraducirMonedaStr(row1.Cells[6].Text);//Moneda de Aplicacion
                                        int matOpID = DB.getMatrizOperacionID(30, rgb.intC2, user.PaisID, contaID);
                                        pagobean.arr1 = (ArrayList)DB.getMatrizConfiguracion(matOpID);
                                    }
                                    else if (row1.Cells[1].Text.Trim().Equals("Deposito bancario"))
                                    {
                                        pagobean.intC1 = 3;
                                        pagobean.decC1 = decimal.Parse(row1.Cells[7].Text);//Monto
                                        pagobean.decC2 = decimal.Parse(row1.Cells[8].Text);//Monto Equivalente
                                        pagobean.strC1 = Page.Server.HtmlDecode(row1.Cells[2].Text);//Banco
                                        pagobean.strC2 = row1.Cells[3].Text;//Cuenta
                                        pagobean.strC3 = row1.Cells[4].Text;//Referencia
                                        pagobean.intC2 = 11;
                                        pagobean.intC3 = 31;
                                        pagobean.intC10 = Utility.TraducirMonedaStr(row1.Cells[5].Text);//Moneda Recepcion
                                        pagobean.intC11 = Utility.TraducirMonedaStr(row1.Cells[6].Text);//Moneda de Aplicacion
                                        int matOpID = DB.getMatrizOperacionID(31, rgb.intC2, user.PaisID, contaID);
                                        pagobean.arr1 = (ArrayList)DB.getMatrizConfiguracion(matOpID);
                                    }
                                    else if (row1.Cells[1].Text.Trim().Equals("Transferencia bancaria"))
                                    {
                                        pagobean.intC1 = 4;
                                        pagobean.decC1 = decimal.Parse(row1.Cells[7].Text);//Monto
                                        pagobean.decC2 = decimal.Parse(row1.Cells[8].Text);//Monto Equivalente
                                        pagobean.strC1 = Page.Server.HtmlDecode(row1.Cells[2].Text);//Banco
                                        pagobean.strC2 = row1.Cells[3].Text;//Cuenta
                                        pagobean.strC3 = row1.Cells[4].Text;//Referencia
                                        pagobean.intC2 = 11;
                                        pagobean.intC3 = 32;
                                        pagobean.intC10 = Utility.TraducirMonedaStr(row1.Cells[5].Text);//Moneda Recepcion
                                        pagobean.intC11 = Utility.TraducirMonedaStr(row1.Cells[6].Text);//Moneda de Aplicacion
                                        int matOpID = DB.getMatrizOperacionID(32, rgb.intC2, user.PaisID, contaID);
                                        pagobean.arr1 = (ArrayList)DB.getMatrizConfiguracion(matOpID);
                                    }
                                    else if (row1.Cells[1].Text.Trim().Equals("Retencion IVA"))
                                    {
                                        pagobean.intC1 = 5;
                                        pagobean.decC1 = decimal.Parse(row1.Cells[7].Text);//Monto
                                        pagobean.decC2 = decimal.Parse(row1.Cells[8].Text);//Monto Equivalente
                                        pagobean.strC1 = Page.Server.HtmlDecode(row1.Cells[2].Text);//Banco
                                        pagobean.strC2 = Page.Server.HtmlDecode(row1.Cells[3].Text);//Cuenta
                                        pagobean.strC3 = Page.Server.HtmlDecode(row1.Cells[4].Text);//Referencia
                                        pagobean.intC2 = 11;
                                        pagobean.intC3 = 71;
                                        pagobean.intC10 = Utility.TraducirMonedaStr(row1.Cells[5].Text);//Moneda Recepcion
                                        pagobean.intC11 = Utility.TraducirMonedaStr(row1.Cells[6].Text);//Moneda de Aplicacion
                                        int matOpID = DB.getMatrizOperacionID(71, rgb.intC2, user.PaisID, contaID);
                                        pagobean.arr1 = (ArrayList)DB.getMatrizConfiguracion(matOpID);
                                    }
                                    else if (row1.Cells[1].Text.Trim().Equals("Retencion ISR"))
                                    {
                                        pagobean.intC1 = 6;
                                        pagobean.decC1 = decimal.Parse(row1.Cells[7].Text);//Monto
                                        pagobean.decC2 = decimal.Parse(row1.Cells[8].Text);//Monto Equivalente
                                        pagobean.strC1 = Page.Server.HtmlDecode(row1.Cells[2].Text);//Banco
                                        pagobean.strC2 = Page.Server.HtmlDecode(row1.Cells[3].Text);//Cuenta
                                        pagobean.strC3 = Page.Server.HtmlDecode(row1.Cells[4].Text);//Referencia
                                        pagobean.intC2 = 11;
                                        pagobean.intC3 = 72;
                                        pagobean.intC10 = Utility.TraducirMonedaStr(row1.Cells[5].Text);//Moneda Recepcion
                                        pagobean.intC11 = Utility.TraducirMonedaStr(row1.Cells[6].Text);//Moneda de Aplicacion
                                        int matOpID = DB.getMatrizOperacionID(72, rgb.intC2, user.PaisID, contaID);
                                        pagobean.arr1 = (ArrayList)DB.getMatrizConfiguracion(matOpID);
                                    }
                                    else if (row1.Cells[1].Text.Trim().Equals("Retencion CLIENTES"))
                                    {
                                        pagobean.intC1 = 7;
                                        pagobean.decC1 = decimal.Parse(row1.Cells[7].Text);//Monto
                                        pagobean.decC2 = decimal.Parse(row1.Cells[8].Text);//Monto Equivalente
                                        pagobean.strC1 = Page.Server.HtmlDecode(row1.Cells[2].Text);//Banco
                                        pagobean.strC2 = Page.Server.HtmlDecode(row1.Cells[3].Text);//Cuenta
                                        pagobean.strC3 = Page.Server.HtmlDecode(row1.Cells[4].Text);//Rerencia
                                        pagobean.intC2 = 11;
                                        pagobean.intC3 = 73;
                                        pagobean.intC10 = Utility.TraducirMonedaStr(row1.Cells[5].Text);//Moneda Recepcion
                                        pagobean.intC11 = Utility.TraducirMonedaStr(row1.Cells[6].Text);//Moneda de Aplicacion
                                        int matOpID = DB.getMatrizOperacionID(73, rgb.intC2, user.PaisID, contaID);
                                        pagobean.arr1 = (ArrayList)DB.getMatrizConfiguracion(matOpID);
                                    }
                                    else if (row1.Cells[1].Text.Trim().Equals("Compensacion"))
                                    {
                                        pagobean.intC1 = 8;
                                        pagobean.decC1 = decimal.Parse(row1.Cells[7].Text);//Monto
                                        pagobean.decC2 = decimal.Parse(row1.Cells[8].Text);//Monto Equivalente
                                        pagobean.strC1 = Page.Server.HtmlDecode(row1.Cells[2].Text);//Banco
                                        pagobean.strC2 = Page.Server.HtmlDecode(row1.Cells[3].Text);//Cuenta
                                        pagobean.strC3 = Page.Server.HtmlDecode(row1.Cells[4].Text);//Referencia
                                        pagobean.intC2 = 11;
                                        pagobean.intC3 = 70;
                                        pagobean.intC10 = Utility.TraducirMonedaStr(row1.Cells[5].Text);//Moneda Recepcion
                                        pagobean.intC11 = Utility.TraducirMonedaStr(row1.Cells[6].Text);//Moneda de Aplicacion
                                        int matOpID = DB.getMatrizOperacionID(70, rgb.intC2, user.PaisID, contaID);
                                        pagobean.arr1 = (ArrayList)DB.getMatrizConfiguracion(matOpID);
                                    }
                                    else if (row1.Cells[1].Text.Trim().Equals("Ajuste periodo anterior"))
                                    {
                                        pagobean.intC1 = 8;
                                        pagobean.decC1 = decimal.Parse(row1.Cells[7].Text);//Monto
                                        pagobean.decC2 = decimal.Parse(row1.Cells[8].Text);//Monto Equivalente
                                        pagobean.strC1 = Page.Server.HtmlDecode(row1.Cells[2].Text);//Banco
                                        pagobean.strC2 = Page.Server.HtmlDecode(row1.Cells[3].Text);//Cuenta
                                        pagobean.strC3 = Page.Server.HtmlDecode(row1.Cells[4].Text);//Referencia
                                        pagobean.intC2 = 11;
                                        pagobean.intC3 = 87;
                                        pagobean.intC10 = Utility.TraducirMonedaStr(row1.Cells[5].Text);//Moneda Recepcion
                                        pagobean.intC11 = Utility.TraducirMonedaStr(row1.Cells[6].Text);//Moneda de Aplicacion
                                        int matOpID = DB.getMatrizOperacionID(87, rgb.intC2, user.PaisID, contaID);
                                        pagobean.arr1 = (ArrayList)DB.getMatrizConfiguracion(matOpID);
                                    }
                                    else if (row1.Cells[1].Text.Trim().Equals("Diferencial Cambiario"))
                                    {
                                        pagobean.intC1 = 8;
                                        pagobean.decC1 = decimal.Parse(row1.Cells[7].Text);//Monto
                                        pagobean.decC2 = decimal.Parse(row1.Cells[8].Text);//Monto Equivalente
                                        pagobean.strC1 = Page.Server.HtmlDecode(row1.Cells[2].Text);//Banco
                                        pagobean.strC2 = Page.Server.HtmlDecode(row1.Cells[3].Text);//Cuenta
                                        pagobean.strC3 = Page.Server.HtmlDecode(row1.Cells[4].Text);//Referencia
                                        pagobean.intC2 = 11;
                                        pagobean.intC3 = 88;
                                        pagobean.intC10 = Utility.TraducirMonedaStr(row1.Cells[5].Text);//Moneda Recepcion
                                        pagobean.intC11 = Utility.TraducirMonedaStr(row1.Cells[6].Text);//Moneda de Aplicacion
                                        int matOpID = DB.getMatrizOperacionID(88, rgb.intC2, user.PaisID, contaID);
                                        pagobean.arr1 = (ArrayList)DB.getMatrizConfiguracion(matOpID);
                                    }
                                }
                                else
                                {
                                    if (row1.Cells[1].Text.Trim().Equals("Efectivo"))
                                    {
                                        pagobean.intC1 = 1;
                                        pagobean.decC1 = decimal.Parse(row1.Cells[7].Text);//Monto
                                        pagobean.decC2 = decimal.Parse(row1.Cells[8].Text);//Monto Equivalente
                                        pagobean.strC1 = "";
                                        pagobean.strC2 = "";
                                        pagobean.intC2 = 12;
                                        pagobean.intC3 = 2;
                                        pagobean.intC10 = Utility.TraducirMonedaStr(row1.Cells[5].Text);//Moneda Recepcion
                                        pagobean.intC11 = Utility.TraducirMonedaStr(row1.Cells[6].Text);//Moneda de Aplicacion
                                        int matOpID = DB.getMatrizOperacionID(2, rgb.intC2, user.PaisID, contaID);
                                        pagobean.arr1 = (ArrayList)DB.getMatrizConfiguracion(matOpID);
                                    }
                                    else if (row1.Cells[1].Text.Trim().Equals("Cheque"))
                                    {
                                        pagobean.intC1 = 2;
                                        pagobean.decC1 = decimal.Parse(row1.Cells[7].Text);//Monto
                                        pagobean.decC2 = decimal.Parse(row1.Cells[8].Text);//Monto Equivalente
                                        pagobean.strC1 = Page.Server.HtmlDecode(row1.Cells[2].Text);//Banco
                                        pagobean.strC2 = Page.Server.HtmlDecode(row1.Cells[3].Text);//Cuenta
                                        pagobean.strC3 = Page.Server.HtmlDecode(row1.Cells[4].Text);//Referencia
                                        pagobean.intC2 = 12;
                                        pagobean.intC3 = 26;
                                        pagobean.intC10 = Utility.TraducirMonedaStr(row1.Cells[5].Text);//Moneda Recepcion
                                        pagobean.intC11 = Utility.TraducirMonedaStr(row1.Cells[6].Text);//Moneda de Aplicacion
                                        int matOpID = DB.getMatrizOperacionID(26, rgb.intC2, user.PaisID, contaID);
                                        pagobean.arr1 = (ArrayList)DB.getMatrizConfiguracion(matOpID);
                                    }
                                    else if (row1.Cells[1].Text.Trim().Equals("Deposito bancario"))
                                    {
                                        pagobean.intC1 = 3;
                                        pagobean.decC1 = decimal.Parse(row1.Cells[7].Text);//Monto
                                        pagobean.decC2 = decimal.Parse(row1.Cells[8].Text);//Monto Equivalente
                                        pagobean.strC1 = Page.Server.HtmlDecode(row1.Cells[2].Text);//Banco
                                        pagobean.strC2 = Page.Server.HtmlDecode(row1.Cells[3].Text);//Cuenta
                                        pagobean.strC3 = Page.Server.HtmlDecode(row1.Cells[4].Text);//Referencia
                                        pagobean.intC2 = 11;
                                        pagobean.intC3 = 27;
                                        pagobean.intC10 = Utility.TraducirMonedaStr(row1.Cells[5].Text);//Moneda Recepcion
                                        pagobean.intC11 = Utility.TraducirMonedaStr(row1.Cells[6].Text);//Moneda de Aplicacion
                                        int matOpID = DB.getMatrizOperacionID(27, rgb.intC2, user.PaisID, contaID);
                                        pagobean.arr1 = (ArrayList)DB.getMatrizConfiguracion(matOpID);
                                    }
                                    else if (row1.Cells[1].Text.Trim().Equals("Transferencia bancaria"))
                                    {
                                        pagobean.intC1 = 4;
                                        pagobean.decC1 = decimal.Parse(row1.Cells[7].Text);//Monto
                                        pagobean.decC2 = decimal.Parse(row1.Cells[8].Text);//Monto Equivalente
                                        pagobean.strC1 = Page.Server.HtmlDecode(row1.Cells[2].Text);//Banco
                                        pagobean.strC2 = Page.Server.HtmlDecode(row1.Cells[3].Text);//Cuenta
                                        pagobean.strC3 = Page.Server.HtmlDecode(row1.Cells[4].Text);//Referencia
                                        pagobean.intC2 = 11;
                                        pagobean.intC3 = 28;
                                        pagobean.intC10 = Utility.TraducirMonedaStr(row1.Cells[5].Text);//Moneda Recepcion
                                        pagobean.intC11 = Utility.TraducirMonedaStr(row1.Cells[6].Text);//Moneda de Aplicacion
                                        int matOpID = DB.getMatrizOperacionID(28, rgb.intC2, user.PaisID, contaID);
                                        pagobean.arr1 = (ArrayList)DB.getMatrizConfiguracion(matOpID);
                                    }
                                    else if (row1.Cells[1].Text.Trim().Equals("Retencion IVA"))
                                    {
                                        pagobean.intC1 = 5;
                                        pagobean.decC1 = decimal.Parse(row1.Cells[7].Text);//Monto
                                        pagobean.decC2 = decimal.Parse(row1.Cells[8].Text);//Monto Equivalente
                                        pagobean.strC1 = Page.Server.HtmlDecode(row1.Cells[2].Text);//Banco
                                        pagobean.strC2 = Page.Server.HtmlDecode(row1.Cells[3].Text);//Cuenta
                                        pagobean.strC3 = Page.Server.HtmlDecode(row1.Cells[4].Text);//Referencia
                                        pagobean.intC2 = 11;
                                        pagobean.intC3 = 29;
                                        pagobean.intC10 = Utility.TraducirMonedaStr(row1.Cells[5].Text);//Moneda Recepcion
                                        pagobean.intC11 = Utility.TraducirMonedaStr(row1.Cells[6].Text);//Moneda de Aplicacion
                                        int matOpID = DB.getMatrizOperacionID(29, rgb.intC2, user.PaisID, contaID);
                                        pagobean.arr1 = (ArrayList)DB.getMatrizConfiguracion(matOpID);
                                    }
                                    else if (row1.Cells[1].Text.Trim().Equals("Retencion ISR"))
                                    {
                                        pagobean.intC1 = 6;
                                        pagobean.decC1 = decimal.Parse(row1.Cells[7].Text);//Monto
                                        pagobean.decC2 = decimal.Parse(row1.Cells[8].Text);//Monto Equivalente
                                        pagobean.strC1 = Page.Server.HtmlDecode(row1.Cells[2].Text);//Banco
                                        pagobean.strC2 = Page.Server.HtmlDecode(row1.Cells[3].Text);//Cuenta
                                        pagobean.strC3 = Page.Server.HtmlDecode(row1.Cells[4].Text);//Referencia
                                        pagobean.intC2 = 11;
                                        pagobean.intC3 = 46;
                                        pagobean.intC10 = Utility.TraducirMonedaStr(row1.Cells[5].Text);//Moneda Recepcion
                                        pagobean.intC11 = Utility.TraducirMonedaStr(row1.Cells[6].Text);//Moneda de Aplicacion
                                        int matOpID = DB.getMatrizOperacionID(46, rgb.intC2, user.PaisID, contaID);
                                        pagobean.arr1 = (ArrayList)DB.getMatrizConfiguracion(matOpID);
                                    }
                                    else if (row1.Cells[1].Text.Trim().Equals("Retencion CLIENTES"))
                                    {
                                        pagobean.intC1 = 7;
                                        pagobean.decC1 = decimal.Parse(row1.Cells[7].Text);//Monto
                                        pagobean.decC2 = decimal.Parse(row1.Cells[8].Text);//Monto Equivalente
                                        pagobean.strC1 = Page.Server.HtmlDecode(row1.Cells[2].Text);//Banco
                                        pagobean.strC2 = Page.Server.HtmlDecode(row1.Cells[3].Text);//Cuenta
                                        pagobean.strC3 = Page.Server.HtmlDecode(row1.Cells[4].Text);//Referencia
                                        pagobean.intC2 = 11;
                                        pagobean.intC3 = 69;
                                        pagobean.intC10 = Utility.TraducirMonedaStr(row1.Cells[5].Text);//Moneda Recepcion
                                        pagobean.intC11 = Utility.TraducirMonedaStr(row1.Cells[6].Text);//Moneda de Aplicacion
                                        int matOpID = DB.getMatrizOperacionID(69, rgb.intC2, user.PaisID, contaID);
                                        pagobean.arr1 = (ArrayList)DB.getMatrizConfiguracion(matOpID);
                                    }
                                    else if (row1.Cells[1].Text.Trim().Equals("Compensacion"))
                                    {
                                        pagobean.intC1 = 8;
                                        pagobean.decC1 = decimal.Parse(row1.Cells[7].Text);//Monto
                                        pagobean.decC2 = decimal.Parse(row1.Cells[8].Text);//Monto Equivalente
                                        pagobean.strC1 = Page.Server.HtmlDecode(row1.Cells[2].Text);//Banco
                                        pagobean.strC2 = Page.Server.HtmlDecode(row1.Cells[3].Text);//Cuenta
                                        pagobean.strC3 = Page.Server.HtmlDecode(row1.Cells[4].Text);//Referencia
                                        pagobean.intC2 = 11;
                                        pagobean.intC3 = 70;
                                        pagobean.intC10 = Utility.TraducirMonedaStr(row1.Cells[5].Text);//Moneda Recepcion
                                        pagobean.intC11 = Utility.TraducirMonedaStr(row1.Cells[6].Text);//Moneda de Aplicacion
                                        int matOpID = DB.getMatrizOperacionID(70, rgb.intC2, user.PaisID, contaID);
                                        pagobean.arr1 = (ArrayList)DB.getMatrizConfiguracion(matOpID);
                                    }
                                    else if (row1.Cells[1].Text.Trim().Equals("Ajuste periodo anterior"))
                                    {
                                        pagobean.intC1 = 8;
                                        pagobean.decC1 = decimal.Parse(row1.Cells[7].Text);//Monto
                                        pagobean.decC2 = decimal.Parse(row1.Cells[8].Text);//Monto Equivalente
                                        pagobean.strC1 = Page.Server.HtmlDecode(row1.Cells[2].Text);//Banco
                                        pagobean.strC2 = Page.Server.HtmlDecode(row1.Cells[3].Text);//Cuenta
                                        pagobean.strC3 = Page.Server.HtmlDecode(row1.Cells[4].Text);//Referencia
                                        pagobean.intC2 = 11;
                                        pagobean.intC3 = 87;
                                        pagobean.intC10 = Utility.TraducirMonedaStr(row1.Cells[5].Text);//Moneda Recepcion
                                        pagobean.intC11 = Utility.TraducirMonedaStr(row1.Cells[6].Text);//Moneda de Aplicacion
                                        int matOpID = DB.getMatrizOperacionID(87, rgb.intC2, user.PaisID, contaID);
                                        pagobean.arr1 = (ArrayList)DB.getMatrizConfiguracion(matOpID);
                                    }
                                    else if (row1.Cells[1].Text.Trim().Equals("Diferencial Cambiario"))
                                    {
                                        pagobean.intC1 = 8;
                                        pagobean.decC1 = decimal.Parse(row1.Cells[7].Text);//Monto
                                        pagobean.decC2 = decimal.Parse(row1.Cells[8].Text);//Monto Equivalente
                                        pagobean.strC1 = Page.Server.HtmlDecode(row1.Cells[2].Text);//Banco
                                        pagobean.strC2 = Page.Server.HtmlDecode(row1.Cells[3].Text);//Cuenta
                                        pagobean.strC3 = Page.Server.HtmlDecode(row1.Cells[4].Text);//Referencia
                                        pagobean.intC2 = 11;
                                        pagobean.intC3 = 88;
                                        pagobean.intC10 = Utility.TraducirMonedaStr(row1.Cells[5].Text);//Moneda Recepcion
                                        pagobean.intC11 = Utility.TraducirMonedaStr(row1.Cells[6].Text);//Moneda de Aplicacion
                                        int matOpID = DB.getMatrizOperacionID(88, rgb.intC2, user.PaisID, contaID);
                                        pagobean.arr1 = (ArrayList)DB.getMatrizConfiguracion(matOpID);
                                    }
                                }
                                pagosarr.Add(pagobean);
                            }
                            rgb.arr2 = pagosarr;
                            string Check_Existencia = DB.CheckExistDoc(rgb.Fecha_Hora, 2);
                            if (Check_Existencia == "0")
                            {
                                ArrayList result = DB.PagoRecibo(rgb, user, contaID);
                                if (result == null || result.Count == 0 || int.Parse(result[0].ToString()) == 0 || int.Parse(result[0].ToString()) == -100)
                                {
                                    WebMsgBox.Show("Error, existió un error al tratar de grabar los datos, por favor intente de nuevo.");
                                    return;
                                }
                                else
                                {
                                    lb_rcpt.Text = result[1].ToString();
                                    lb_rcptID.Text = result[0].ToString();
                                    //WebMsgBox.Show("El recibo fue grabado exitosamente con el correlativo " + lb_serie_factura.SelectedValue + result[1].ToString());
                                    WebMsgBox.Show("El Recibo fue grabado exitosamente con Serie y Correlativo.: " + lb_serie_factura.SelectedItem.Text + " - " + result[1].ToString());
                                    int rcpt = int.Parse(result[0].ToString());
                                    dgw_aplicada.DataSource = (DataTable)DB.getRcptFacturas(rcpt);
                                    dgw_aplicada.DataBind();
                                    // Obtengo el arreglo de NotasDebito que el recibo mato o aplico
                                    gv_nd_abonadas.DataSource = (DataTable)DB.getRcptNotaDebito(rcpt);
                                    gv_nd_abonadas.DataBind();

                                    int cliID = int.Parse(tb_clientid.Text);
                                    //Obtengo el arreglo de facturas pendientes de este cliente
                                    //DataSet ds = (DataSet)DB.getFacturasbyCliente(cliID, false, int.Parse(lbMoneda.SelectedValue), user);//false indica que solo hala las que no estan pagadas ni anuladas
                                    DataSet ds = (DataSet)DB.Get_FacturasbyCliente_Conta_Moneda(cliID, false, int.Parse(lbMoneda.SelectedValue), user);//false indica que solo hala las que no estan pagadas ni anuladas
                                    dgw.DataSource = ds.Tables["fact"];
                                    dgw.DataBind();
                                    //Obtengo el arreglo de Notas Debito pendientes de este cliente
                                    //ds = (DataSet)DB.getNotasDebitoforRcpt((int)cliID, 3, int.Parse(lbMoneda.SelectedValue), false, user); //3 porque el tpi es cliente
                                    ds = (DataSet)DB.Get_NotasDebitoforRcptBy_Conta_Moneda((int)cliID, 3, int.Parse(lbMoneda.SelectedValue), false, user); //3 porque el tpi es cliente
                                    gv_notadebito.DataSource = ds.Tables["fact"];
                                    gv_notadebito.DataBind();
                                    btn_imprimir.Enabled = true;
                                    btn_recibo_virtual.Enabled = true;
                                    bt_Enviar.Enabled = false;
                                    lbl_flag.Text = "1";
                                    lb_serie_factura.Enabled = false;

                                    gv_detalle_partida.DataSource = DB.getPolizaDiario(user, double.Parse(lb_rcptID.Text), 2, 0);
                                    gv_detalle_partida.DataBind();
                                    gv_detalle_partida.Rows[gv_detalle_partida.Rows.Count - 1].Font.Bold = true;

                                    gv_detalle_partida_diferencial.DataSource = DB.getPolizaDiferencial(user, 2, int.Parse(lb_rcptID.Text));
                                    gv_detalle_partida_diferencial.DataBind();
                                    if (gv_detalle_partida_diferencial.Rows.Count > 0)
                                    {
                                        gv_detalle_partida_diferencial.Rows[gv_detalle_partida_diferencial.Rows.Count - 1].Font.Bold = true;
                                    }


                                    dgw.Enabled = false;
                                    gv_notadebito.Enabled = false;
                                    gv_pagos.Enabled = false;
                                    tb_agregarpago.Enabled = false;
                                    chk_anticipo.Enabled = false;
                                    return;
                                }
                            }
                            else
                            {
                                bt_Enviar.Enabled = false;
                                return;
                            }
                        }
                    }
            }
        }
        else if (chk_anticipo.Checked == true)
        {
            if ((tb_clientid.Text.Trim() == "") || (tb_clientid.Text.Trim() == "0"))
            {
                WebMsgBox.Show("Debe indicar el Cliente del Recibo");
            }
            else if ((tb_monto.Text == "") || (tb_monto.Text == "0") || (tb_monto.Text == "0.00"))
            {
                WebMsgBox.Show("Debe Indicar monto a Aplicar");
            }
            else if ((tb_monto.Text.Trim() != "") && (tb_monto.Text.Trim() != "0") && (tb_monto.Text != "0.00"))
            {
                RE_GenericBean rgb = new RE_GenericBean();
                ///
                rgb.Estado = 1;
                rgb.Fecha_Hora = lb_fecha_hora.Text;
                rgb.intC2 = int.Parse(lbMoneda.SelectedValue);//tipo moneda
                rgb.strC1 = tb_fecha.Text.Trim();//fecha
                rgb.decC1 = decimal.Parse(tb_roe.Text.Trim());//tipo de cambio
                rgb.douC1 = double.Parse(tb_clientid.Text.Trim());//id del cliente
                rgb.strC2 = tb_clientname.Text.Trim().ToUpper();//nombre del cliente
                rgb.strC3 = tb_recibonombre.Text.Trim().ToUpper();//nombre del recibo
                rgb.decC2 = decimal.Parse(tb_monto.Text.Trim());//monto a aplicar
                rgb.strC5 = tb_nota.Text.Trim();//Nota | Observaciones
                bool anticipo = chk_anticipo.Checked; // anticipo
                rgb.strC6 = tb_hbl.Text.Trim().ToUpper();// master
                rgb.strC7 = tb_mbl.Text.Trim().ToUpper(); //hbl
                rgb.strC8 = tb_routing.Text.Trim().ToUpper(); //referencia
                rgb.strC9 = lb_serie_factura.SelectedItem.Text;
                rgb.IP_Address = Request.ServerVariables["REMOTE_ADDR"].ToString();
                if (lbMoneda.SelectedValue.Equals("8"))
                {
                    rgb.decC2 = decimal.Parse(tb_monto.Text.Trim()); // monto abonar en moneda a facturar
                    rgb.decC3 = Math.Round((decimal.Parse(tb_monto.Text.Trim()) * user.pais.TipoCambio), 2);//monto a abonar en otra moneda
                }
                else
                {
                    rgb.decC2 = decimal.Parse(tb_monto.Text.Trim()); // monto abonar en moneda a facturar
                    rgb.decC3 = Math.Round((decimal.Parse(tb_monto.Text.Trim()) / user.pais.TipoCambio), 2);//monto a abonar en otra moneda
                }
                ArrayList Arrfact_aplicar = new ArrayList();
                ArrayList Arrfact_aplicar_ND = new ArrayList();
                RE_GenericBean factbean = null;
                //GridViewRow row;
                //TextBox t1;
                decimal saldo = 0;
                decimal montoabonar = 0, dolaresactuales = 0, colonesactuales = 0;
                decimal totalfactdolar = 0;
                decimal totalfact = 0;
                decimal TipoCambioFactura = 0;
                decimal valoractual = 0, valorantiguo = 0;
                decimal diferencial = 0;
                int monedadiferencial = 0;

                for (int i = 0; i < dgw.Rows.Count; i++)
                {
                    row = dgw.Rows[i];
                    factbean = new RE_GenericBean();
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
                        factbean.douC10 = Convert.ToDouble(row.Cells[14].Text);//Diferencial
                        factbean.intC10 = 8;//Moneda Diferencial
                        #endregion
                    }
                    if (factbean.decC1 > 0)
                        Arrfact_aplicar.Add(factbean);
                }

                //**************************************************************** notas de debito
                for (int i = 0; i < gv_notadebito.Rows.Count; i++)
                {
                    row = gv_notadebito.Rows[i];
                    factbean = new RE_GenericBean();
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
                        factbean.douC10 = Convert.ToDouble(row.Cells[14].Text);//Diferencial
                        factbean.intC10 = 8;//Moneda Diferencial
                        #endregion
                    }
                    if (factbean.decC1 > 0)
                        Arrfact_aplicar_ND.Add(factbean);
                }
                //********************************************************************************* fin notas debito

                rgb.arr1 = Arrfact_aplicar;
                rgb.arr3 = Arrfact_aplicar_ND;
                int contaID = int.Parse(Session["Contabilidad"].ToString());//tipo de contabilidad
                rgb.intC4 = contaID;//tipo de contabilidad


                //************************************************** obtengo las cuentas *****************
                GridViewRowCollection gvr = gv_pagos.Rows;
                ArrayList pagosarr = new ArrayList();
                RE_GenericBean pagobean = null;
                foreach (GridViewRow row1 in gvr)
                {
                    pagobean = new RE_GenericBean();
                    if (chk_anticipo.Checked)// Obtengo los datos de anticipos
                    {
                        if (row1.Cells[1].Text.Trim().Equals("Efectivo"))
                        {
                            pagobean.intC1 = 1;
                            pagobean.decC1 = decimal.Parse(row1.Cells[7].Text);//Monto
                            pagobean.decC2 = decimal.Parse(row1.Cells[8].Text);//Monto Equivalente
                            pagobean.strC1 = "";
                            pagobean.strC2 = "";
                            pagobean.intC2 = 12;
                            pagobean.intC3 = 12;
                            int matOpID = DB.getMatrizOperacionID(12, rgb.intC2, user.PaisID, contaID);
                            pagobean.arr1 = (ArrayList)DB.getMatrizConfiguracion(matOpID);
                        }
                        else if (row1.Cells[1].Text.Trim().Equals("Cheque"))
                        {
                            pagobean.intC1 = 2;
                            pagobean.decC1 = decimal.Parse(row1.Cells[7].Text);//Monto
                            pagobean.decC2 = decimal.Parse(row1.Cells[8].Text);//Monto Equivalente
                            pagobean.strC1 = Page.Server.HtmlDecode(row1.Cells[2].Text);//Banco
                            pagobean.strC2 = "";
                            pagobean.strC3 = row1.Cells[4].Text;//Referencia
                            pagobean.intC2 = 12;
                            pagobean.intC3 = 30;
                            int matOpID = DB.getMatrizOperacionID(30, rgb.intC2, user.PaisID, contaID);
                            pagobean.arr1 = (ArrayList)DB.getMatrizConfiguracion(matOpID);
                        }
                        else if (row1.Cells[1].Text.Trim().Equals("Deposito bancario"))
                        {
                            pagobean.intC1 = 3;
                            pagobean.decC1 = decimal.Parse(row1.Cells[7].Text);//Monto
                            pagobean.decC2 = decimal.Parse(row1.Cells[8].Text);//Monto Equivalente
                            pagobean.strC1 = Page.Server.HtmlDecode(row1.Cells[2].Text);//Banco
                            pagobean.strC2 = row1.Cells[3].Text;//Cuenta
                            pagobean.strC3 = row1.Cells[4].Text;//Referencia
                            pagobean.intC2 = 11;
                            pagobean.intC3 = 31;
                            int matOpID = DB.getMatrizOperacionID(31, rgb.intC2, user.PaisID, contaID);
                            pagobean.arr1 = (ArrayList)DB.getMatrizConfiguracion(matOpID);
                        }
                        else if (row1.Cells[1].Text.Trim().Equals("Transferencia bancaria"))
                        {
                            pagobean.intC1 = 4;
                            pagobean.decC1 = decimal.Parse(row1.Cells[7].Text);//Monto
                            pagobean.decC2 = decimal.Parse(row1.Cells[8].Text);//Monto Equivalente
                            pagobean.strC1 = Page.Server.HtmlDecode(row1.Cells[2].Text);//Banco
                            pagobean.strC2 = row1.Cells[3].Text;//Cuenta
                            pagobean.strC3 = row1.Cells[4].Text;//Referencia
                            pagobean.intC2 = 11;
                            pagobean.intC3 = 32;
                            int matOpID = DB.getMatrizOperacionID(32, rgb.intC2, user.PaisID, contaID);
                            pagobean.arr1 = (ArrayList)DB.getMatrizConfiguracion(matOpID);
                        }
                        else if (row1.Cells[1].Text.Trim().Equals("Retencion IVA"))
                        {
                            pagobean.intC1 = 5;
                            pagobean.decC1 = decimal.Parse(row1.Cells[7].Text);//Monto
                            pagobean.decC2 = decimal.Parse(row1.Cells[8].Text);//Monto Equivalente
                            pagobean.strC1 = Page.Server.HtmlDecode(row1.Cells[2].Text);//Banco
                            pagobean.strC2 = Page.Server.HtmlDecode(row1.Cells[3].Text);//Cuenta
                            pagobean.strC3 = Page.Server.HtmlDecode(row1.Cells[4].Text);//Referencia
                            pagobean.intC2 = 11;
                            pagobean.intC3 = 71;
                            int matOpID = DB.getMatrizOperacionID(71, rgb.intC2, user.PaisID, contaID);
                            pagobean.arr1 = (ArrayList)DB.getMatrizConfiguracion(matOpID);
                        }
                        else if (row1.Cells[1].Text.Trim().Equals("Retencion ISR"))
                        {
                            pagobean.intC1 = 6;
                            pagobean.decC1 = decimal.Parse(row1.Cells[7].Text);//Monto
                            pagobean.decC2 = decimal.Parse(row1.Cells[8].Text);//Monto Equivalente
                            pagobean.strC1 = Page.Server.HtmlDecode(row1.Cells[2].Text);//Banco
                            pagobean.strC2 = Page.Server.HtmlDecode(row1.Cells[3].Text);//Cuenta
                            pagobean.strC3 = Page.Server.HtmlDecode(row1.Cells[4].Text);//Referencia
                            pagobean.intC2 = 11;
                            pagobean.intC3 = 72;
                            int matOpID = DB.getMatrizOperacionID(72, rgb.intC2, user.PaisID, contaID);
                            pagobean.arr1 = (ArrayList)DB.getMatrizConfiguracion(matOpID);
                        }
                        else if (row1.Cells[1].Text.Trim().Equals("Retencion CLIENTES"))
                        {
                            pagobean.intC1 = 7;
                            pagobean.decC1 = decimal.Parse(row1.Cells[7].Text);//Monto
                            pagobean.decC2 = decimal.Parse(row1.Cells[8].Text);//Monto Equivalente
                            pagobean.strC1 = Page.Server.HtmlDecode(row1.Cells[2].Text);//Banco
                            pagobean.strC2 = Page.Server.HtmlDecode(row1.Cells[3].Text);//Cuenta
                            pagobean.strC3 = Page.Server.HtmlDecode(row1.Cells[4].Text);//Rerencia
                            pagobean.intC2 = 11;
                            pagobean.intC3 = 73;
                            int matOpID = DB.getMatrizOperacionID(73, rgb.intC2, user.PaisID, contaID);
                            pagobean.arr1 = (ArrayList)DB.getMatrizConfiguracion(matOpID);
                        }
                        else if (row1.Cells[1].Text.Trim().Equals("Compensacion"))
                        {
                            pagobean.intC1 = 8;
                            pagobean.decC1 = decimal.Parse(row1.Cells[7].Text);//Monto
                            pagobean.decC2 = decimal.Parse(row1.Cells[8].Text);//Monto Equivalente
                            pagobean.strC1 = Page.Server.HtmlDecode(row1.Cells[2].Text);//Banco
                            pagobean.strC2 = Page.Server.HtmlDecode(row1.Cells[3].Text);//Cuenta
                            pagobean.strC3 = Page.Server.HtmlDecode(row1.Cells[4].Text);//Referencia
                            pagobean.intC2 = 11;
                            pagobean.intC3 = 70;
                            int matOpID = DB.getMatrizOperacionID(70, rgb.intC2, user.PaisID, contaID);
                            pagobean.arr1 = (ArrayList)DB.getMatrizConfiguracion(matOpID);
                        }
                        else if (row1.Cells[1].Text.Trim().Equals("Ajuste periodo anterior"))
                        {
                            pagobean.intC1 = 8;
                            pagobean.decC1 = decimal.Parse(row1.Cells[7].Text);//Monto
                            pagobean.decC2 = decimal.Parse(row1.Cells[8].Text);//Monto Equivalente
                            pagobean.strC1 = Page.Server.HtmlDecode(row1.Cells[2].Text);//Banco
                            pagobean.strC2 = Page.Server.HtmlDecode(row1.Cells[3].Text);//Cuenta
                            pagobean.strC3 = Page.Server.HtmlDecode(row1.Cells[4].Text);//Referencia
                            pagobean.intC2 = 11;
                            pagobean.intC3 = 87;
                            int matOpID = DB.getMatrizOperacionID(87, rgb.intC2, user.PaisID, contaID);
                            pagobean.arr1 = (ArrayList)DB.getMatrizConfiguracion(matOpID);
                        }
                        else if (row1.Cells[1].Text.Trim().Equals("Diferencial Cambiario"))
                        {
                            pagobean.intC1 = 8;
                            pagobean.decC1 = decimal.Parse(row1.Cells[7].Text);//Monto
                            pagobean.decC2 = decimal.Parse(row1.Cells[8].Text);//Monto Equivalente
                            pagobean.strC1 = Page.Server.HtmlDecode(row1.Cells[2].Text);//Banco
                            pagobean.strC2 = Page.Server.HtmlDecode(row1.Cells[3].Text);//Cuenta
                            pagobean.strC3 = Page.Server.HtmlDecode(row1.Cells[4].Text);//Referencia
                            pagobean.intC2 = 11;
                            pagobean.intC3 = 88;
                            int matOpID = DB.getMatrizOperacionID(88, rgb.intC2, user.PaisID, contaID);
                            pagobean.arr1 = (ArrayList)DB.getMatrizConfiguracion(matOpID);
                        }
                    }
                    else
                    {
                        if (row1.Cells[1].Text.Trim().Equals("Efectivo"))
                        {
                            pagobean.intC1 = 1;
                            pagobean.decC1 = decimal.Parse(row1.Cells[7].Text);//Monto
                            pagobean.decC2 = decimal.Parse(row1.Cells[8].Text);//Monto Equivalente
                            pagobean.strC1 = "";
                            pagobean.strC2 = "";
                            pagobean.intC2 = 12;
                            pagobean.intC3 = 2;
                            int matOpID = DB.getMatrizOperacionID(2, rgb.intC2, user.PaisID, contaID);
                            pagobean.arr1 = (ArrayList)DB.getMatrizConfiguracion(matOpID);
                        }
                        else if (row1.Cells[1].Text.Trim().Equals("Cheque"))
                        {
                            pagobean.intC1 = 2;
                            pagobean.decC1 = decimal.Parse(row1.Cells[7].Text);//Monto
                            pagobean.decC2 = decimal.Parse(row1.Cells[8].Text);//Monto Equivalente
                            pagobean.strC1 = Page.Server.HtmlDecode(row1.Cells[2].Text);//Banco
                            pagobean.strC2 = Page.Server.HtmlDecode(row1.Cells[3].Text);//Cuenta
                            pagobean.strC3 = Page.Server.HtmlDecode(row1.Cells[4].Text);//Referencia
                            pagobean.intC2 = 12;
                            pagobean.intC3 = 26;
                            int matOpID = DB.getMatrizOperacionID(26, rgb.intC2, user.PaisID, contaID);
                            pagobean.arr1 = (ArrayList)DB.getMatrizConfiguracion(matOpID);
                        }
                        else if (row1.Cells[1].Text.Trim().Equals("Deposito bancario"))
                        {
                            pagobean.intC1 = 3;
                            pagobean.decC1 = decimal.Parse(row1.Cells[7].Text);//Monto
                            pagobean.decC2 = decimal.Parse(row1.Cells[8].Text);//Monto Equivalente
                            pagobean.strC1 = Page.Server.HtmlDecode(row1.Cells[2].Text);//Banco
                            pagobean.strC2 = Page.Server.HtmlDecode(row1.Cells[3].Text);//Cuenta
                            pagobean.strC3 = Page.Server.HtmlDecode(row1.Cells[4].Text);//Referencia
                            pagobean.intC2 = 11;
                            pagobean.intC3 = 27;
                            int matOpID = DB.getMatrizOperacionID(27, rgb.intC2, user.PaisID, contaID);
                            pagobean.arr1 = (ArrayList)DB.getMatrizConfiguracion(matOpID);
                        }
                        else if (row1.Cells[1].Text.Trim().Equals("Transferencia bancaria"))
                        {
                            pagobean.intC1 = 4;
                            pagobean.decC1 = decimal.Parse(row1.Cells[7].Text);//Monto
                            pagobean.decC2 = decimal.Parse(row1.Cells[8].Text);//Monto Equivalente
                            pagobean.strC1 = Page.Server.HtmlDecode(row1.Cells[2].Text);//Banco
                            pagobean.strC2 = Page.Server.HtmlDecode(row1.Cells[3].Text);//Cuenta
                            pagobean.strC3 = Page.Server.HtmlDecode(row1.Cells[4].Text);//Referencia
                            pagobean.intC2 = 11;
                            pagobean.intC3 = 28;
                            int matOpID = DB.getMatrizOperacionID(28, rgb.intC2, user.PaisID, contaID);
                            pagobean.arr1 = (ArrayList)DB.getMatrizConfiguracion(matOpID);
                        }
                        else if (row1.Cells[1].Text.Trim().Equals("Retencion IVA"))
                        {
                            pagobean.intC1 = 5;
                            pagobean.decC1 = decimal.Parse(row1.Cells[7].Text);//Monto
                            pagobean.decC2 = decimal.Parse(row1.Cells[8].Text);//Monto Equivalente
                            pagobean.strC1 = Page.Server.HtmlDecode(row1.Cells[2].Text);//Banco
                            pagobean.strC2 = Page.Server.HtmlDecode(row1.Cells[3].Text);//Cuenta
                            pagobean.strC3 = Page.Server.HtmlDecode(row1.Cells[4].Text);//Referencia
                            pagobean.intC2 = 11;
                            pagobean.intC3 = 29;
                            int matOpID = DB.getMatrizOperacionID(29, rgb.intC2, user.PaisID, contaID);
                            pagobean.arr1 = (ArrayList)DB.getMatrizConfiguracion(matOpID);
                        }
                        else if (row1.Cells[1].Text.Trim().Equals("Retencion ISR"))
                        {
                            pagobean.intC1 = 6;
                            pagobean.decC1 = decimal.Parse(row1.Cells[7].Text);//Monto
                            pagobean.decC2 = decimal.Parse(row1.Cells[8].Text);//Monto Equivalente
                            pagobean.strC1 = Page.Server.HtmlDecode(row1.Cells[2].Text);//Banco
                            pagobean.strC2 = Page.Server.HtmlDecode(row1.Cells[3].Text);//Cuenta
                            pagobean.strC3 = Page.Server.HtmlDecode(row1.Cells[4].Text);//Referencia
                            pagobean.intC2 = 11;
                            pagobean.intC3 = 46;
                            int matOpID = DB.getMatrizOperacionID(46, rgb.intC2, user.PaisID, contaID);
                            pagobean.arr1 = (ArrayList)DB.getMatrizConfiguracion(matOpID);
                        }
                        else if (row1.Cells[1].Text.Trim().Equals("Retencion CLIENTES"))
                        {
                            pagobean.intC1 = 7;
                            pagobean.decC1 = decimal.Parse(row1.Cells[7].Text);//Monto
                            pagobean.decC2 = decimal.Parse(row1.Cells[8].Text);//Monto Equivalente
                            pagobean.strC1 = Page.Server.HtmlDecode(row1.Cells[2].Text);//Banco
                            pagobean.strC2 = Page.Server.HtmlDecode(row1.Cells[3].Text);//Cuenta
                            pagobean.strC3 = Page.Server.HtmlDecode(row1.Cells[4].Text);//Referencia
                            pagobean.intC2 = 11;
                            pagobean.intC3 = 69;
                            int matOpID = DB.getMatrizOperacionID(69, rgb.intC2, user.PaisID, contaID);
                            pagobean.arr1 = (ArrayList)DB.getMatrizConfiguracion(matOpID);
                        }
                        else if (row1.Cells[1].Text.Trim().Equals("Compensacion"))
                        {
                            pagobean.intC1 = 8;
                            pagobean.decC1 = decimal.Parse(row1.Cells[7].Text);//Monto
                            pagobean.decC2 = decimal.Parse(row1.Cells[8].Text);//Monto Equivalente
                            pagobean.strC1 = Page.Server.HtmlDecode(row1.Cells[2].Text);//Banco
                            pagobean.strC2 = Page.Server.HtmlDecode(row1.Cells[3].Text);//Cuenta
                            pagobean.strC3 = Page.Server.HtmlDecode(row1.Cells[4].Text);//Referencia
                            pagobean.intC2 = 11;
                            pagobean.intC3 = 70;
                            int matOpID = DB.getMatrizOperacionID(70, rgb.intC2, user.PaisID, contaID);
                            pagobean.arr1 = (ArrayList)DB.getMatrizConfiguracion(matOpID);
                        }
                        else if (row1.Cells[1].Text.Trim().Equals("Ajuste periodo anterior"))
                        {
                            pagobean.intC1 = 8;
                            pagobean.decC1 = decimal.Parse(row1.Cells[7].Text);//Monto
                            pagobean.decC2 = decimal.Parse(row1.Cells[8].Text);//Monto Equivalente
                            pagobean.strC1 = Page.Server.HtmlDecode(row1.Cells[2].Text);//Banco
                            pagobean.strC2 = Page.Server.HtmlDecode(row1.Cells[3].Text);//Cuenta
                            pagobean.strC3 = Page.Server.HtmlDecode(row1.Cells[4].Text);//Referencia
                            pagobean.intC2 = 11;
                            pagobean.intC3 = 87;
                            int matOpID = DB.getMatrizOperacionID(87, rgb.intC2, user.PaisID, contaID);
                            pagobean.arr1 = (ArrayList)DB.getMatrizConfiguracion(matOpID);
                        }
                        else if (row1.Cells[1].Text.Trim().Equals("Diferencial Cambiario"))
                        {
                            pagobean.intC1 = 8;
                            pagobean.decC1 = decimal.Parse(row1.Cells[7].Text);//Monto
                            pagobean.decC2 = decimal.Parse(row1.Cells[8].Text);//Monto Equivalente
                            pagobean.strC1 = Page.Server.HtmlDecode(row1.Cells[2].Text);//Banco
                            pagobean.strC2 = Page.Server.HtmlDecode(row1.Cells[3].Text);//Cuenta
                            pagobean.strC3 = Page.Server.HtmlDecode(row1.Cells[4].Text);//Referencia
                            pagobean.intC2 = 11;
                            pagobean.intC3 = 88;
                            int matOpID = DB.getMatrizOperacionID(88, rgb.intC2, user.PaisID, contaID);
                            pagobean.arr1 = (ArrayList)DB.getMatrizConfiguracion(matOpID);
                        }
                    }
                    pagosarr.Add(pagobean);

                }
                rgb.arr2 = pagosarr;
                string Check_Existencia = DB.CheckExistDoc(rgb.Fecha_Hora, 2);
                if (Check_Existencia == "0")
                {
                    ArrayList result = DB.PagoRecibo(rgb, user, contaID);
                    if (result == null || result.Count == 0 || int.Parse(result[0].ToString()) == 0 || int.Parse(result[0].ToString()) == -100)
                    {
                        WebMsgBox.Show("Error, existió un error al tratar de grabar los datos, por favor intente de nuevo.");
                        return;
                    }
                    else
                    {
                        lb_rcpt.Text = result[1].ToString();
                        lb_rcptID.Text = result[0].ToString();
                        WebMsgBox.Show("El Recibo fue grabado exitosamente con Serie y Correlativo.: " + lb_serie_factura.SelectedItem.Text + " - " + result[1].ToString());
                        int rcpt = int.Parse(result[0].ToString());
                        dgw_aplicada.DataSource = (DataTable)DB.getRcptFacturas(rcpt);
                        dgw_aplicada.DataBind();
                        // Obtengo el arreglo de NotasDebito que el recibo mato o aplico
                        gv_nd_abonadas.DataSource = (DataTable)DB.getRcptNotaDebito(rcpt);
                        gv_nd_abonadas.DataBind();

                        int cliID = int.Parse(tb_clientid.Text);
                        DataSet ds = (DataSet)DB.Get_FacturasbyCliente_Conta_Moneda(cliID, false, int.Parse(lbMoneda.SelectedValue), user);//false indica que solo hala las que no estan pagadas ni anuladas
                        dgw.DataSource = ds.Tables["fact"];
                        dgw.DataBind();
                        //Obtengo el arreglo de Notas Debito pendientes de este cliente
                        ds = (DataSet)DB.Get_NotasDebitoforRcptBy_Conta_Moneda((int)cliID, 3, int.Parse(lbMoneda.SelectedValue), false, user); //3 porque el tpi es cliente
                        gv_notadebito.DataSource = ds.Tables["fact"];
                        gv_notadebito.DataBind();
                        btn_imprimir.Enabled = true;
                        btn_recibo_virtual.Enabled = true;
                        bt_Enviar.Enabled = false;
                        gv_detalle_partida.DataSource = DB.getPolizaDiario(user, double.Parse(lb_rcptID.Text), 2, 0);
                        gv_detalle_partida.DataBind();
                        gv_detalle_partida.Rows[gv_detalle_partida.Rows.Count - 1].Font.Bold = true;
                        return;
                    }
                }
                else
                {
                    bt_Enviar.Enabled = false;
                    return;
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
        if (lbl_flag.Text != "1")
        {
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
                    //if (lbMoneda.SelectedValue.Equals("8"))
                    //{
                    //    saldo = decimal.Parse(row.Cells[12].Text);
                    //}
                    //else
                    //{
                    //    saldo = decimal.Parse(row.Cells[9].Text);
                    //}
                    //t1 = (TextBox)row.FindControl("TextBox1");
                    //if ((abono - saldo > 0) && (!chk_anticipo.Checked))
                    //{
                    //    t1.Text = saldo.ToString();
                    //    abono = abono - saldo;
                    //}
                    //else
                    //{
                    //    if ((abono > 0) && (!chk_anticipo.Checked))
                    //    {
                    //        t1.Text = abono.ToString();
                    //        abono = 0;
                    //    }
                    //    else
                    //    {
                    //        t1.Text = "0";
                    //    }
                    //}
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

                    //if (lbMoneda.SelectedValue.Equals("8"))
                    //{
                    //    saldo = decimal.Parse(row.Cells[12].Text);
                    //}
                    //else
                    //{
                    //    saldo = decimal.Parse(row.Cells[9].Text);
                    //}
                    //t1 = (TextBox)row.FindControl("tb_descND");
                    //if ((abono - saldo > 0) && (!chk_anticipo.Checked))
                    //{
                    //    t1.Text = saldo.ToString();
                    //    abono = abono - saldo;
                    //}
                    //else
                    //{
                    //    if ((abono > 0) && (!chk_anticipo.Checked))
                    //    {
                    //        t1.Text = abono.ToString();
                    //        abono = 0;
                    //    }
                    //    else
                    //    {
                    //        t1.Text = "0";
                    //    }
                    //}
                }
            }
            for (i = 0; i < gv_pagos.Rows.Count; i++)
            {
                row = gv_pagos.Rows[i];
                if (row.RowType == DataControlRowType.DataRow)
                {
                    row.Cells[6].BackColor = System.Drawing.Color.LightCyan;
                    row.Cells[7].BackColor = System.Drawing.Color.LightCyan;
                    row.Cells[6].Font.Bold = true;
                    row.Cells[7].Font.Bold = true;
                    row.Cells[5].HorizontalAlign = HorizontalAlign.Center;
                    row.Cells[6].HorizontalAlign = HorizontalAlign.Center;
                    row.Cells[7].HorizontalAlign = HorizontalAlign.Right;
                    row.Cells[8].HorizontalAlign = HorizontalAlign.Right;
                    row.Cells[9].HorizontalAlign = HorizontalAlign.Right;
                }
            }
        }
        else
        {
            //Gridview Facturas Pendientes de Pago
            for (i = 0; i < dgw.Rows.Count; i++)
            {
                row = dgw.Rows[i];
                if (row.RowType == DataControlRowType.DataRow)
                {
                    t1 = (TextBox)row.FindControl("TextBox1");
                    t1.Text = "0.00";
                }
            }
            //Gridview Notas de Debito Pendientes de Pago
            for (i = 0; i < gv_notadebito.Rows.Count; i++)
            {
                row = gv_notadebito.Rows[i];
                if (row.RowType == DataControlRowType.DataRow)
                {
                    t1 = (TextBox)row.FindControl("tb_descND");
                    t1.Text = "0.00";
                }
            }
        }
    }
    protected void chk_anticipo_CheckedChanged(object sender, EventArgs e)
    {
        GridViewRow row;
        TextBox t1;
        int i = 0;
        if (chk_anticipo.Checked)
        {
            //Gridview Facturas Pendientes de Pago
            dgw.Enabled = false;
            for (i = 0; i < dgw.Rows.Count; i++)
            {
                row = dgw.Rows[i];
                if (row.RowType == DataControlRowType.DataRow)
                {
                    t1 = (TextBox)row.FindControl("TextBox1");
                    t1.Text = "0.00";
                }
            }
            //Gridview Notas de Debito Pendientes de Pago
            gv_notadebito.Enabled = false;
            for (i = 0; i < gv_notadebito.Rows.Count; i++)
            {
                row = gv_notadebito.Rows[i];
                if (row.RowType == DataControlRowType.DataRow)
                {
                    t1 = (TextBox)row.FindControl("tb_descND");
                    t1.Text = "0.00";                    
                }
            }
            if ((DB.Validar_Restriccion_Activa(user, 2, 28) == true))
            {
                #region Validacion Grupo Empresas
                if ((user.SucursalID == 9) || (user.SucursalID == 71))
                {
                }
                else if (user.pais.Grupo_Empresas == 1)
                {
                    if (lbl_tipo_cliente.Text == "True")
                    {
                        WebMsgBox.Show("No se puede Emitir un Recibo de Anticipo al cliente.:  " + tb_clientname.Text.Trim() + ", porque es un Cliente COLOADER");
                        tb_clientid.Text = "";
                        tb_clientname.Text = "";
                        tb_recibonombre.Text = "";
                        chk_anticipo.Checked = false;
                        dgw.DataBind();
                        gv_notadebito.DataBind();
                        return;
                    }
                }
                else if (user.pais.Grupo_Empresas == 2)
                {
                    if (lbl_tipo_cliente.Text == "False")
                    {
                        WebMsgBox.Show("No se puede Emitir un Recibo de Anticipo al cliente.: " + tb_clientname.Text.Trim() + ", porque es un Cliente DIRECTO");
                        tb_clientid.Text = "";
                        tb_clientname.Text = "";
                        tb_recibonombre.Text = "";
                        chk_anticipo.Checked = false;
                        dgw.DataBind();
                        gv_notadebito.DataBind();
                        return;
                    }
                }
                else if (user.pais.Grupo_Empresas == 3)
                {
                }
                #endregion
            }
        }
        else 
        {
            dgw.Enabled = true;
            gv_notadebito.Enabled = true;
            bt_Enviar.Enabled = true;
        }
    }

    protected void refresh() { 

    }

    protected void gv_clientes_SelectedIndexChanged(object sender, EventArgs e)
    {
        GridViewRow row = gv_clientes.SelectedRow;
        double cliID = 0;

        cliID = double.Parse(row.Cells[1].Text);
        //Obtengo los datos del cliente
        string criterio = "a.id_cliente=" + cliID;
        ArrayList clientearr = (ArrayList)DB.getClientes(criterio, user, "");
        RE_GenericBean clienteBean;
        if ((clientearr != null) && (clientearr.Count > 0))
        {
            // si entro aqui es porque encontre datos del cliente
            clienteBean = (RE_GenericBean)clientearr[0];
            tb_clientid.Text = clienteBean.douC1.ToString();
            tb_clientname.Text = clienteBean.strC1;
            tb_recibonombre.Text = clienteBean.strC2;
            //tb_nit.Text = clienteBean.strC3;
            cliID = double.Parse(tb_clientid.Text);
            //Obtengo los datos del cliente
            criterio = "a.id_cliente=" + cliID;
            clientearr = (ArrayList)DB.getClientes(criterio, user, "");
            if ((clientearr != null) && (clientearr.Count > 0))
            {
                // si entro aqui es porque encontre datos del cliente
                clienteBean = (RE_GenericBean)clientearr[0];
                tb_clientid.Text = clienteBean.douC1.ToString();
                tb_clientname.Text = clienteBean.strC1;
                tb_recibonombre.Text = clienteBean.strC2;
                lbl_tipo_cliente.Text = clienteBean.strC8;
                //Obtengo el arreglo de facturas pendientes de este cliente
                //DataSet ds = (DataSet)DB.getFacturasbyCliente(cliID, false, int.Parse(lbMoneda.SelectedValue), user);//false indica que solo hala las que no estan pagadas ni anuladas
                DataSet ds = (DataSet)DB.Get_FacturasbyCliente_Conta_Moneda(cliID, false, int.Parse(lbMoneda.SelectedValue), user);//false indica que solo hala las que no estan pagadas ni anuladas
                if (ds != null)
                {
                    dgw.DataSource = ds.Tables["fact"];
                }
                dgw.DataBind();
                //Obtengo el arreglo de Notas Debito pendientes de este cliente
                //ds = (DataSet)DB.getNotasDebitoforRcpt((int)cliID, 3, int.Parse(lbMoneda.SelectedValue), false, user); //3 porque el tpi es cliente
                ds = (DataSet)DB.Get_NotasDebitoforRcptBy_Conta_Moneda((int)cliID, 3, int.Parse(lbMoneda.SelectedValue), false, user); //3 porque el tpi es cliente
                if (ds != null)
                {
                    gv_notadebito.DataSource = ds.Tables["fact"];
                }
                gv_notadebito.DataBind();
            }
        }
        ViewState["dt"] = null;
        gv_clientes.DataSource = null;
        gv_clientes.DataBind();
    }
    protected void Button4_Click(object sender, EventArgs e)
    {
        string codigo = tb_codigo.Text.Trim();
        string nombre = tb_nombre_cliente.Text.Trim().Replace(' ', '%').ToUpper();
        string nit = tb_nit_cliente.Text.Trim().ToUpper();
        string criterio = "";
        if ((codigo.Equals("")) && (nombre.Equals("")) && (nit.Equals("")))
        {
            WebMsgBox.Show("Debe ingresar al menos un criterio de busqueda");
            return;
        }
        if ((codigo != null) && (!codigo.Equals("")))
        {
            criterio += "a.id_cliente=" + codigo;
        }
        if ((nombre != null) && (!nombre.Equals("")))
            if (!criterio.Equals(""))
                criterio += " and nombre_cliente like ('%" + nombre + "%')";
            else
                criterio += "nombre_cliente like ('%" + nombre + "%')";
        if ((nit != null) && (!nit.Equals("")))
            if (!criterio.Equals(""))
                criterio += " and codigo_tributario='" + nit + "'";
            else
                criterio = "codigo_tributario='" + nit + "'";
        ArrayList arr = (ArrayList)DB.getClientes(criterio, user, "");

        DataTable dt = new DataTable("cliente");
        dt.Columns.Add("ID");
        dt.Columns.Add("Nombre");
        dt.Columns.Add("Tipo");
        foreach (RE_GenericBean rgb in arr)
        {
            object[] objArr = { rgb.douC1, rgb.strC1, rgb.strC9 };
            dt.Rows.Add(objArr);
        }
        gv_clientes.DataSource = dt;
        gv_clientes.DataBind();
        ViewState["dt"] = dt;
        modalcliente.Show();
    }

    protected void gv_clientes_PageIndexChanged(object sender, EventArgs e)
    {
        if (Page.IsPostBack)
        {
            dt1 = (DataTable)ViewState["dt"];
        }
    }
    protected void gv_clientes_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        dt1 = (DataTable)ViewState["dt"];
        gv_clientes.DataSource = dt1;
        gv_clientes.PageIndex = e.NewPageIndex;
        //gv_cuenta.DataSource = gv_cuenta.DataSource;
        gv_clientes.DataBind();
        modalcliente.Show();
    }

    protected void bt_search_Click(object sender, EventArgs e)
    {
        
        UsuarioBean user = (UsuarioBean)Session["usuario"];
        int lista = 0;
        
        if (lb_tipo.SelectedValue.Equals("LCL"))
        {
            DataSet ds = DB.getBL_LCL(lista, tb_criterio.Text, user);
            this.dgw1.DataSource = ds.Tables["bl_list"];
            this.dgw1.DataBind();
            ViewState["dt"] = ds.Tables["bl_list"];
        }
        else if (lb_tipo.SelectedValue.Equals("FCL"))
        {
            DataSet ds = DB.getBL_FCL(lista, tb_criterio.Text, user);
            this.dgw1.DataSource = ds.Tables["bl_list"];
            this.dgw1.DataBind();
            ViewState["dt"] = ds.Tables["bl_list"];
        }
        else if (lb_tipo.SelectedValue.Equals("ALMACENADORA"))
        {
            string codDistribucion = Utility.CodigoDistribucionPicking(user.PaisID);
            DataSet ds = DB.getBL_ALMACENADORA(lista, tb_criterio.Text, user.pais.schema, codDistribucion, user);
            this.dgw1.DataSource = ds.Tables["bl_list"];
            this.dgw1.DataBind();
            ViewState["dt"] = ds.Tables["bl_list"];
        }
        else if (lb_tipo.SelectedValue.Equals("AEREO"))
        {
            DataSet ds = DB.getBL_AEREO(lista, tb_criterio.Text, user.pais.schema);
            this.dgw1.DataSource = ds.Tables["bl_list"];
            this.dgw1.DataBind();
            ViewState["dt"] = ds.Tables["bl_list"];
        }
        else if (lb_tipo.SelectedValue.Equals("TERRESTRE T"))
        {
            string paisISO = Utility.InttoISOPais(user.PaisID);
            DataSet ds = DB.getBL_TERRESTRE(lista, tb_criterio.Text, paisISO, user);
            this.dgw1.DataSource = ds.Tables["bl_list"];
            this.dgw1.DataBind();
            ViewState["dt"] = ds.Tables["bl_list"];
        }
        tb_cuenta_ModalPopupExtender.Show();
    }
    protected void dgw1_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Seleccionar")
        {
            int index = Convert.ToInt32(e.CommandArgument);
            tb_hbl.Text = dgw1.Rows[index].Cells[2].Text;
            tb_mbl.Text = dgw1.Rows[index].Cells[3].Text;
            tb_routing.Text = dgw1.Rows[index].Cells[4].Text;
            tb_contenedor.Text = dgw1.Rows[index].Cells[5].Text;
        }
    }
    protected void dgw1_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        DataTable dt1;
        dt1 = (DataTable)ViewState["dt"];
        dgw1.DataSource = dt1;
        dgw1.PageIndex = e.NewPageIndex;
        dgw1.DataBind();
        tb_cuenta_ModalPopupExtender.Show();
    }
    protected void btn_imprimir_Click(object sender, EventArgs e)
    {
        UsuarioBean user = (UsuarioBean)Session["usuario"];
        String csname1 = "PopupScript";
        Type cstype = this.GetType();
        string script = "";
        // Get a ClientScriptManager reference from the Page class.
        ClientScriptManager cs2 = Page.ClientScript;
        RE_GenericBean rgb = new RE_GenericBean();
        int rcpt = int.Parse(lb_rcptID.Text.ToString());
        rgb = (RE_GenericBean)DB.getRcptData(rcpt);
        string path = DB.getpathImpresion(2, rgb.intC2, rgb.strC32);
        script = "window.open('../ImpresionDocumentos.html?fac_id=" + lb_rcptID.Text + "&tipo=2&pais=" + user.PaisID.ToString() + "&idioma=" + user.Idioma.ToString() + "&tipo_cambio=" + user.pais.TipoCambio.ToString() + "&contaId=" + user.contaID.ToString() + "','Re-Imprimir','toolbar=0, status=1, resizable=1, scrollbars=1, top=0, left=0, width=' + (window.screen.width -10) + ', height=' + (window.screen.height -75))";
        #region Seteo de Parametros de Impresion
        //UsuarioBean user = (UsuarioBean)Session["usuario"];
        user.ImpresionBean.Operacion = "1";
        user.ImpresionBean.Tipo_Documento = "2";
        user.ImpresionBean.Id = lb_rcptID.Text;
        user.ImpresionBean.Impreso = false;
        Session["usuario"] = user;
        #endregion
        if (!cs2.IsStartupScriptRegistered(cstype, csname1))
        {
            cs2.RegisterStartupScript(cstype, csname1, script, true);
        }
    }
    
    protected void tb_agregarpago_Click(object sender, EventArgs e)
    {
        #region Validaciones
        if (tb_montoparcial.Text.Equals("0"))
        {
            WebMsgBox.Show("Debe ingresar un monto mayor a 0 para poder agregarlo");
            return;
        }
        if (lb_tipo_pago.SelectedValue.Equals("Efectivo"))
        {
            if (!lb_bancos.SelectedValue.Trim().Equals(""))
            {
                WebMsgBox.Show("Si el pago es en efectivo no debe llenar los campos de banco ni documento de referencia");
                lb_bancos.SelectedIndex = 0;
                return;
            }
        }
        else if (lb_tipo_pago.SelectedValue.Equals("Cheque"))
        {
            if (lb_bancos.SelectedValue.Equals(" "))
            {
                WebMsgBox.Show("Si el pago es con Cheque debe seleccionar el Banco");
                return;
            }
            else if (tb_referencia.Text.Trim() == "")
            {
                WebMsgBox.Show("Si el pago es con Cheque debe llenar el campo referencia");
                return;
            }
        }
        else if (lb_tipo_pago.SelectedValue.Equals("Deposito bancario") || (lb_tipo_pago.SelectedValue.Equals("Transferencia bancaria")))
        {
            if (lb_bancos.SelectedValue.Equals(" "))
            {
                WebMsgBox.Show("Si el pago es un Deposito Bancario o una Transferencia Bancaria debe seleccionar el Banco");
                return;
            }
            else if (lb_cuentas_bancarias.SelectedValue.Trim().Equals(""))
            {
                WebMsgBox.Show("Si el pago es un Deposito Bancario o una Transferencia Bancaria debe seleccionar el No. de Cuenta");
                return;
            }
            else if (tb_referencia.Text.Trim() == "")
            {
                WebMsgBox.Show("Debe llenar el campo de documento referencia");
                return;
            }
            if (DB.existDeposito(int.Parse(lb_bancos.SelectedValue), lb_cuentas_bancarias.SelectedValue, tb_referencia.Text.Trim()))
            {
                WebMsgBox.Show("No se puede ingresar este deposito ya que fue ingresado anteriormente");
                return;
            }
        }
        else if (lb_tipo_pago.SelectedValue.Equals("Retencion"))
        {
            if (tb_referencia.Text.Trim() == "")
            {
                WebMsgBox.Show("Debe llenar el campo de documento referencia");
                return;
            }
        }
        #endregion
        #region Validar que no se agregue el mismo pago al Grid
        int ban_existencia = 0;
        string pago = "";
        string banco = "";
        string no_cuenta = "";
        string no_referencia = "";
        pago = lb_tipo_pago.SelectedValue;
        if (lb_bancos.SelectedItem.Text != "Seleccione...")
        {
            banco = lb_bancos.SelectedItem.Text;
        }
        else
        {
            banco = "&nbsp;";
        }
        if (lb_cuentas_bancarias.Items.Count > 0)
        {
            if (lb_cuentas_bancarias.SelectedItem.Text != "Seleccione...")
            {
                no_cuenta = lb_cuentas_bancarias.SelectedItem.Text;
            }
            else
            {
                no_cuenta = "&nbsp;";
            }
        }
        else
        {
            no_cuenta = "&nbsp;";
        }
        no_referencia = tb_referencia.Text.Trim();
        if (pago == "Efectivo")
        {
            banco = "&nbsp;";
            no_cuenta = "&nbsp;";
            no_referencia = "&nbsp;";
        }
        if (pago == "Retencion")
        {
            banco = "&nbsp;";
            no_cuenta = "&nbsp;";
        }
        GridViewRowCollection gvp = gv_pagos.Rows;
        foreach (GridViewRow row in gvp)
        {
            if (pago != "Efectivo")
            {
                if (row.Cells[1].Text == pago)
                {
                    if ((row.Cells[2].Text == banco) || (row.Cells[2].Text == "&#160;"))
                    {
                        if ((row.Cells[3].Text == no_cuenta) || (row.Cells[3].Text == "&#160;"))
                        {
                            if (row.Cells[4].Text == no_referencia)
                            {
                                ban_existencia++;
                            }
                        }
                    }
                }
            }
        }
        if (ban_existencia > 0)
        {
            WebMsgBox.Show("El pago que esta tratando de aplicar ya fue ingresado en este u otro Recibo");
            return;
        }
        #endregion
        #region Validar que no se agregue el mismo pago en la BD con el mismo No de Referencia
        int tipo_pago = 0;
        if (lb_tipo_pago.SelectedValue == "Cheque")
        {
            tipo_pago = 2;
        }
        if (lb_tipo_pago.SelectedValue == "Deposito bancario")
        {
            tipo_pago = 3;
        }
        if ((tipo_pago == 2) || (tipo_pago == 3))
        {
            int resultado = 0;
            if (tipo_pago == 2)
            {
                resultado = DB.Check_Pago(tipo_pago, int.Parse(lb_bancos.SelectedValue), "", tb_referencia.Text);
            }
            if (tipo_pago == 3)
            {
                resultado = DB.Check_Pago(tipo_pago, int.Parse(lb_bancos.SelectedValue), lb_cuentas_bancarias.SelectedItem.Text, tb_referencia.Text);
            }
            if (resultado == -100)
            {
                WebMsgBox.Show("Ha existido un error con el tipo de pago ingresado por favor revise de nuevo");
                return;
            }
            else if (resultado > 0)
            {
                WebMsgBox.Show("El pago que esta tratando de aplicar ya fue ingresado en este u otro Recibo");
                return;
            }
        }
        #endregion
        decimal total = 0;
        decimal total_equivalente = 0;
        decimal montoparcial = 0;
        decimal montoparcial_equivalente = 0;
        decimal monto_recibido = 0;
        string moneda_recibida = "";
        string moneda_aplicacion = "";
        DataTable dt_temp = new DataTable();
        dt_temp.Columns.Add("Tipo");
        dt_temp.Columns.Add("Banco");
        dt_temp.Columns.Add("Cuenta");
        dt_temp.Columns.Add("Referencia");
        dt_temp.Columns.Add("Moneda Recepcion");
        dt_temp.Columns.Add("Moneda Aplicacion");
        dt_temp.Columns.Add("Monto Aplicar");
        dt_temp.Columns.Add("Equivalente");
        dt_temp.Columns.Add("T.C.");
        GridViewRowCollection gvr = gv_pagos.Rows;
        dt_temp.Clear();
        foreach (GridViewRow row in gvr)
        {
            total += decimal.Parse(row.Cells[7].Text.Trim());
            total_equivalente += decimal.Parse(row.Cells[8].Text.Trim());
            object[] objArr = { Page.Server.HtmlDecode(row.Cells[1].Text), Page.Server.HtmlDecode(row.Cells[2].Text), Page.Server.HtmlDecode(row.Cells[3].Text), Page.Server.HtmlDecode(row.Cells[4].Text), Page.Server.HtmlDecode(row.Cells[5].Text), Page.Server.HtmlDecode(row.Cells[6].Text), Page.Server.HtmlDecode(row.Cells[7].Text), Page.Server.HtmlDecode(row.Cells[8].Text), Page.Server.HtmlDecode(row.Cells[9].Text) };
            dt_temp.Rows.Add(objArr);
        }
        monto_recibido = Math.Round(decimal.Parse(tb_montoparcial.Text.Trim()), 2);
        moneda_recibida = lb_moneda.SelectedItem.Text.Substring(0, 3);
        moneda_aplicacion = lbMoneda.SelectedItem.Text.Substring(0, 3);
        if (lbMoneda.SelectedValue != lb_moneda.SelectedValue)
        {
            if (lbMoneda.SelectedValue.Equals("8"))
            {
                montoparcial_equivalente = Math.Round(decimal.Parse(tb_montoparcial.Text.Trim()), 2);
                montoparcial = Math.Round(montoparcial_equivalente / user.pais.TipoCambio, 2);
            }
            else
            {
                montoparcial_equivalente = Math.Round(decimal.Parse(tb_montoparcial.Text.Trim()), 2);
                montoparcial = Math.Round(montoparcial_equivalente * user.pais.TipoCambio, 2);
            }
        }
        else
        {
            if (lbMoneda.SelectedValue.Equals("8"))
            {
                montoparcial = Math.Round(decimal.Parse(tb_montoparcial.Text.Trim()), 2);
                montoparcial_equivalente = Math.Round(montoparcial * user.pais.TipoCambio, 2);
            }
            else
            {
                montoparcial = Math.Round(decimal.Parse(tb_montoparcial.Text.Trim()), 2);
                montoparcial_equivalente = Math.Round(montoparcial / user.pais.TipoCambio, 2);
            }
        }

        total += montoparcial;
        total_equivalente += montoparcial_equivalente;
        if (lb_bancos.SelectedItem.Text != "Seleccione...")
        {
            object[] objArr1 = { lb_tipo_pago.SelectedValue, lb_bancos.SelectedItem.Text, lb_cuentas_bancarias.SelectedValue.Trim(), tb_referencia.Text.Trim(), moneda_recibida, moneda_aplicacion, montoparcial.ToString(), montoparcial_equivalente.ToString(), user.pais.TipoCambio.ToString() };
            dt_temp.Rows.Add(objArr1);
        }
        else
        {
            object[] objArr1 = { lb_tipo_pago.SelectedValue, "", "", "", moneda_recibida, moneda_aplicacion, montoparcial.ToString(), montoparcial_equivalente.ToString(), user.pais.TipoCambio.ToString() };
            dt_temp.Rows.Add(objArr1);
        }

        tb_monto.Text = total.ToString();
        tb_monto_equivalente.Text = total_equivalente.ToString();
        abono = decimal.Parse(tb_monto.Text.Trim());
        gv_pagos.DataSource = dt_temp;
        gv_pagos.DataBind();

        tb_montoparcial.Text = "0";
        tb_referencia.Text = "";
        lb_bancos.SelectedIndex = 0;
        lb_cuentas_bancarias.Items.Clear();
    }
    protected void gv_pagos_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        decimal total = 0;
        decimal total_equivalente = 0;
        if (e.CommandName == "Eliminar")
        {
            int index = Convert.ToInt32(e.CommandArgument);
            DataTable dt_temp = new DataTable();
            dt_temp.Columns.Add("Tipo");
            dt_temp.Columns.Add("Banco");
            dt_temp.Columns.Add("Cuenta");
            dt_temp.Columns.Add("Referencia");
            dt_temp.Columns.Add("Moneda Recepcion");
            dt_temp.Columns.Add("Moneda Aplicacion");
            dt_temp.Columns.Add("Monto Aplicar");
            dt_temp.Columns.Add("Equivalente");
            dt_temp.Columns.Add("T.C.");
            GridViewRowCollection gvr = gv_pagos.Rows;
            dt_temp.Clear();
            foreach (GridViewRow row in gvr)
            {
                total += decimal.Parse(row.Cells[7].Text.Trim());
                total_equivalente += decimal.Parse(row.Cells[8].Text.Trim());
                object[] objArr = { Page.Server.HtmlDecode(row.Cells[1].Text), Page.Server.HtmlDecode(row.Cells[2].Text), Page.Server.HtmlDecode(row.Cells[3].Text), Page.Server.HtmlDecode(row.Cells[4].Text), Page.Server.HtmlDecode(row.Cells[5].Text), Page.Server.HtmlDecode(row.Cells[6].Text), Page.Server.HtmlDecode(row.Cells[7].Text), Page.Server.HtmlDecode(row.Cells[8].Text), Page.Server.HtmlDecode(row.Cells[9].Text) };
                dt_temp.Rows.Add(objArr);
            }
            total -= decimal.Parse(dt_temp.Rows[index][6].ToString());
            total_equivalente -= decimal.Parse(dt_temp.Rows[index][7].ToString());
            tb_monto.Text = total.ToString();
            tb_monto_equivalente.Text = total_equivalente.ToString();
            abono = decimal.Parse(tb_monto.Text.Trim());
            dt_temp.Rows[index].Delete();
            gv_pagos.DataSource = dt_temp;
            gv_pagos.DataBind();

            #region Limpiar Abonos y Diferencial
            GridViewRowCollection gvrc = dgw.Rows;
            foreach (GridViewRow row in gvrc)
            {
                TextBox tb1 = (TextBox)row.FindControl("TextBox1");
                tb1.Text = "0.00";
                row.Cells[14].Text = "0.00";
            }

            GridViewRowCollection gvrc_nd = gv_notadebito.Rows;
            foreach (GridViewRow row_nd in gvrc_nd)
            {
                TextBox tb_abonoND = (TextBox)row_nd.FindControl("tb_descND");
                tb_abonoND.Text = "0.00";
                row_nd.Cells[14].Text = "0.00";
            }
            #endregion

        }
    }

    protected void lb_bancos_SelectedIndexChanged1(object sender, EventArgs e)
    {
        ListItem item = null;
        lb_cuentas_bancarias.Items.Clear();
        item = new ListItem("Seleccione...", " ");
        lb_cuentas_bancarias.Items.Add(item);
        if (!lb_bancos.SelectedValue.Trim().Equals(""))
        {
            ArrayList arrCtas = (ArrayList)DB.getCuentasBancarias_ByPaisMoneda(int.Parse(lb_bancos.SelectedValue), user, 1);
            foreach (RE_GenericBean rgb in arrCtas)
            {
                item = new ListItem(rgb.strC1, rgb.strC1);
                lb_cuentas_bancarias.Items.Add(item);
            }
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
    protected void lb_tipo_pago_SelectedIndexChanged(object sender, EventArgs e)
    {
        int tipo_pago = 0;
        ListItem item = null;
        if (lb_tipo_pago.SelectedValue == "Efectivo")
        {
            tipo_pago = 1;
        }
        else if (lb_tipo_pago.SelectedValue == "Cheque")
        {
            tipo_pago = 2;
        }
        else if (lb_tipo_pago.SelectedValue == "Deposito bancario")
        {
            tipo_pago = 3;
        }
        else if (lb_tipo_pago.SelectedValue == "Transferencia bancaria")
        {
            tipo_pago = 4;
        }
        else if (lb_tipo_pago.SelectedValue == "Retencion")
        {
            tipo_pago = 5;
        }
        ArrayList arr = (ArrayList)DB.getBancos_By_PaisMonedaID(user.PaisID, int.Parse(lbMoneda.SelectedValue), tipo_pago);
        lb_bancos.Items.Clear();
        item = new ListItem("Seleccione...", " ");
        lb_bancos.Items.Add(item);
        foreach (RE_GenericBean rgb in arr)
        {
            item = new ListItem(rgb.strC1, rgb.intC1.ToString());
            lb_bancos.Items.Add(item);
        }
        lb_cuentas_bancarias.Items.Clear();
    }
    protected void btn_recibo_virtual_Click(object sender, EventArgs e)
    {
        String csname1 = "PopupScript";
        Type cstype = this.GetType();
        // Get a ClientScriptManager reference from the Page class.
        ClientScriptManager cs2 = Page.ClientScript;
        string script = "window.open('../invoice/template_recibo.aspx?id=" + lb_rcptID.Text + "&transaccion=2','Imprimir','menubar=1,resizable=1,scrollbars=1,toolbar=1,width=750');";
        if (!cs2.IsStartupScriptRegistered(cstype, csname1))
        {
            cs2.RegisterStartupScript(cstype, csname1, script, true);
        }
    }
    protected void lb_serie_factura_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (lb_serie_factura.Items.Count > 0)
        {
            if (lb_serie_factura.SelectedValue != "0")
            {
                int moneda_Serie = 0;
                moneda_Serie = DB.Get_Serie_Moneda_By_ID(int.Parse(lb_serie_factura.SelectedValue));
                if (moneda_Serie.ToString() != lbMoneda.SelectedValue)
                {
                    tb_monto.Text = "0.00";
                    tb_monto_equivalente.Text = "0.00";
                    lbMoneda.SelectedValue = moneda_Serie.ToString();//Moneda de la Transaccion
                    lb_moneda.SelectedValue = moneda_Serie.ToString();//Moneda del Rubro
                    lbMoneda.Enabled = false;
                    gv_pagos.DataBind();
                    if ((tb_clientid.Text.Trim() != "") && (tb_clientid.Text.Trim() != " ") && (tb_clientid.Text.Trim() != "0"))
                    {
                        double cliID = 0;
                        cliID = double.Parse(tb_clientid.Text);
                        DataSet ds = (DataSet)DB.Get_FacturasbyCliente_Conta_Moneda(cliID, false, int.Parse(lbMoneda.SelectedValue), user);//false indica que solo hala las que no estan pagadas ni anuladas
                        if (ds != null)
                        {
                            dgw.DataSource = ds.Tables["fact"];
                        }
                        dgw.DataBind();
                        ds = (DataSet)DB.Get_NotasDebitoforRcptBy_Conta_Moneda((int)cliID, 3, int.Parse(lbMoneda.SelectedValue), false, user); //3 porque el tpi es cliente
                        if (ds != null)
                        {
                            gv_notadebito.DataSource = ds.Tables["fact"];
                        }
                        gv_notadebito.DataBind();
                    }
                }
                lb_tipo_pago.SelectedValue = "Efectivo";
                lb_tipo_pago_SelectedIndexChanged(sender, e);
            }
            else
            {
                tb_monto.Text = "0.00";
                tb_monto_equivalente.Text = "0.00";
                gv_pagos.DataBind();
                dgw.DataBind();
                gv_notadebito.DataBind();
                WebMsgBox.Show("Por Favor seleccione la Serie a utilizar");
                return;
            }
        }
    }
    protected void TextBox1_TextChanged(object sender, EventArgs e)
    {
        #region Validaciones Facturacion Electronica de Costa Rica
        if ((user.PaisID == 5) || (user.PaisID == 21))
        {
            int _ID_FA = 0; ;
            TextBox tb_descuento = sender as TextBox;
            GridViewRow row_descuento = tb_descuento.NamingContainer as GridViewRow;
            int rowIndex = row_descuento.RowIndex;
            _ID_FA = int.Parse(row_descuento.Cells[1].Text);
            RE_GenericBean Data_Factura_Temporal = (RE_GenericBean)DB.getFacturaData(_ID_FA);
            if (Data_Factura_Temporal.strC50 == "1")
            {
                //Serie Electronica
                if ((Data_Factura_Temporal.strC1 == "0") || (Data_Factura_Temporal.strC49 == "-"))
                {
                    TextBox tb1 = (TextBox)row_descuento.FindControl("TextBox1");
                    tb1.Text = "0.00";
                    WebMsgBox.Show("No es posible aplicar un abono a la Factura Electronica seleccionada, dado que la misma no ha sido Firmada por GTI (Proveedor de Facturas Electronicas) ni tiene un correlativo asignado , por favor intente mas tarde y valide que los Datos del Cliente fueron ingresados correctamente en el Catalogo de Clientes.");
                    return;
                }
            }
        }
        #endregion

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
                TOTAL_abonos = Math.Round(TOTAL_abonos, 2);
                TOTAL_disponible = Math.Round(TOTAL_disponible, 2);
                saldo_DOCUMENTO = Math.Round(saldo_DOCUMENTO, 2);
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
                Tipo_CambioRECIBO = Convert.ToDouble(user.pais.TipoCambio);
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

        #region Validaciones Facturacion Electronica de Costa Rica
        if ((user.PaisID == 5) || (user.PaisID == 21))
        {
            int _ID_ND = 0; ;
            TextBox tb_descuento = sender as TextBox;
            GridViewRow row_descuento = tb_descuento.NamingContainer as GridViewRow;
            int rowIndex = row_descuento.RowIndex;
            _ID_ND = int.Parse(row_descuento.Cells[1].Text);
            RE_GenericBean Data_Nota_Debito_Temporal = (RE_GenericBean)DB.getNotaDebitoData(_ID_ND);
            if (Data_Nota_Debito_Temporal.strC50 == "1")
            {
                //Serie Electronica
                if ((Data_Nota_Debito_Temporal.intC6.ToString() == "0") || (Data_Nota_Debito_Temporal.strC49 == "-"))
                {
                    TextBox tb1 = (TextBox)row_descuento.FindControl("tb_descND");
                    tb1.Text = "0.00";
                    WebMsgBox.Show("No es posible aplicar un abono a la Nota de Debito Electronica seleccionada, dado que la misma no ha sido Firmada por GTI (Proveedor de Facturas Electronicas) ni tiene un correlativo asignado , por favor intente mas tarde y valide que los Datos del Cliente fueron ingresados correctamente en el Catalogo de Clientes.");
                    return;
                }
            }
        }
        #endregion

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
                TOTAL_abonos = Math.Round(TOTAL_abonos, 2);
                TOTAL_disponible = Math.Round(TOTAL_disponible, 2);
                saldo_DOCUMENTO = Math.Round(saldo_DOCUMENTO, 2);
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
                Tipo_CambioRECIBO = Convert.ToDouble(user.pais.TipoCambio);
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
