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
using System.Xml;
using System.IO;

public partial class invoice_viewrcpt : System.Web.UI.Page
{
    int factID = 0;
    UsuarioBean user = null;
    bool Recuperacion_Iva = true;
    protected void Page_Load(object sender, EventArgs e)
    {
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
        if (!((permiso & 128) == 128))
        {
            Response.Redirect("index.aspx");
        }

        if (!IsPostBack)
        {
            lb_fecha_hora.Text = DB.getDateTimeNow();
            int moneda_inicial = 0;
            moneda_inicial = DB.Get_Moneda_Inicial_By_Empresa(user, 1);
            lb_moneda.SelectedValue = moneda_inicial.ToString();
        }

        int impo_expo = 0;
        int tipo_contabilidad = int.Parse(Session["Contabilidad"].ToString());
        obtengo_listas(tipo_contabilidad, impo_expo, user.PaisID);

        if (lb_serieNC.Items.Count == 0)
        {
            WebMsgBox.Show("No hay Serie definida para esta sucursal");
            bt_Enviar.Enabled = false;
            return;
        }

        if (lb_imp_exp.Items.Count > 0)
            impo_expo = int.Parse(lb_imp_exp.SelectedValue);
        if (lb_serie_factura.Items.Count > 0)
        {
            Determinar_Tipo_Serie(user.SucursalID, lb_serieNC.SelectedItem.Text);
        }

        #region Bloqueos Facturacion Electronica Costa Rica
        if ((user.PaisID == 5) || (user.PaisID == 21))
        {
            if (user.SucursalID == 117)
            {
                bt_Imprimir.Visible = false;
                pnl_tipo_identificacion_cliente.Visible = true;
            }
        }
        #endregion

    }
    private void obtengo_listas(int tipoconta, int impo_expo, int paiID)
    {
        ListItem item = null;
        ArrayList arr = null;
        if (!Page.IsPostBack)
        {
            arr = (ArrayList)DB.getOpcionesMatriz("tbl_tipo_transaccion where ttt_template='pop_notacredito.aspx'");
            foreach (RE_GenericBean rgb in arr)
            {
                item = new ListItem(rgb.strC1, rgb.intC1.ToString());
                lb_tipo_transaccion.Items.Add(item);
            }
            if (lb_tipo_transaccion.Items.Count > 0)
            {
                lb_tipo_transaccion.SelectedValue = "5";
            }
            arr = null;
            arr = (ArrayList)DB.getMonedasbyPais(paiID, user.contaID);
            foreach (RE_GenericBean rgb in arr)
            {
                item = new ListItem(rgb.strC1, rgb.intC1.ToString());
                lb_moneda.Items.Add(item);
            }
            arr = null;
            arr = (ArrayList)DB.getOpcionesMatriz("tbl_tipo_imp_exp");
            foreach (RE_GenericBean rgb in arr)
            {
                item = new ListItem(rgb.strC1, rgb.intC1.ToString());
                if (impo_expo == rgb.intC1) { item.Selected = true; lb_imp_exp.Enabled = false; }
                lb_imp_exp.Items.Add(item);
            }
            arr = null;
            arr = (ArrayList)DB.getOpcionesMatriz("tbl_tipo_contribuyente");
            foreach (RE_GenericBean rgb in arr)
            {
                item = new ListItem(rgb.strC1, rgb.intC1.ToString());
                lb_contribuyente.Items.Add(item);
            }
            
            arr = null;
            arr = (ArrayList)DB.Get_Series_By_Monedas_Conta(3, user, 1);
            item = new ListItem("Seleccione...", "0");
            lb_serieNC.Items.Clear();
            lb_serieNC.Items.Add(item);
            foreach (RE_GenericBean Bean_Serie in arr)
            {
                item = new ListItem(Bean_Serie.strC1, Bean_Serie.strC2);
                lb_serieNC.Items.Add(item);
            }
            lb_serieNC.SelectedValue = "0";

            arr = null;
            arr = (ArrayList)DB.Obtener_Tipos_Identificacion_Tributaria();
            drp_tipo_identificacion_cliente.Items.Clear();
            item = new ListItem("No Asignada", "0");
            drp_tipo_identificacion_cliente.Items.Add(item);
            foreach (RE_GenericBean rgb in arr)
            {
                item = new ListItem(rgb.strC1, rgb.intC1.ToString());
                drp_tipo_identificacion_cliente.Items.Add(item);
            }
        }
    }
    protected void chk_anulacion_CheckedChanged(object sender, EventArgs e)
    {

        Label lb1 = new Label();
        TextBox t1 = new TextBox();
        decimal descuento = 0, total = 0;
        foreach (GridViewRow row in dgw.Rows)
        {
            t1 = (TextBox)row.FindControl("TextBox1");
            lb1.Text = row.Cells[9].Text.Trim();
            if (chk_anulacion.Checked == true)
            {
                t1.Text = Math.Round(decimal.Parse(lb1.Text), 2).ToString("#,#.00#;(#,#.00#)");
                //descuento = decimal.Parse(t1.Text.Trim());
            }
            else
            {
                t1.Text = "0.00";
                //descuento = decimal.Parse(t1.Text.Trim());
            }
            //total += descuento;
        }
        TextBox1_TextChanged(sender, e);
    }
    protected void TextBox1_TextChanged(object sender, EventArgs e)
    {
        GridViewRow row;
        double descuento=0, subtotal=0, total=0, impuesto=0, descuento_subtotal=0, descuento_impuesto=0;
        double porc_impuesto=double.Parse(user.pais.Impuesto.ToString());
        int tipo_contabilidad = int.Parse(Session["Contabilidad"].ToString());
        TextBox t1, t2;
        
        for (int i=0; i < dgw.Rows.Count; i++)
        {
            row = dgw.Rows[i];
            if (row.RowType == DataControlRowType.DataRow)
            {
                t1 = (TextBox)row.FindControl("TextBox1");
                t2 = (TextBox)row.FindControl("TextBox2");                            
                if (t1.Text.Trim() == "")
                {
                    t1.Text = "0.00";
                    t2.Text = "0.00";
                }
                else
                {
                    descuento = double.Parse(t1.Text.Trim());
                    if (descuento > double.Parse(row.Cells[12].Text.Trim()))
                    {
                        WebMsgBox.Show("El descuento no puede ser mayor al Saldo de este rubro");
                        descuento = 0;
                        t1.Text = "0.00";
                        t2.Text = "0.00";
                    }

                    //*********************************************************************************************
                    Rubros rub = new Rubros();
                    //significa que si es contribuyente
                    rub.rubroSubTot = double.Parse(row.Cells[7].Text.Trim());
                    rub.rubroImpuesto = double.Parse(row.Cells[8].Text.Trim());
                    rub.rubroTot = double.Parse(row.Cells[9].Text.Trim());

                    //si se cobro Iva, y esta dentro del periodo de recuperacion permitido se descuenta el IVA
                    Recuperacion_Iva = DB.Validar_Periodo_Recuperacion_Impuesto(user, tb_fecha.Text);
                    //if ((rub.rubroImpuesto > 0) && (descuento > 0) && (int.Parse(LimitDays.Text) <= user.pais.diasimpuesto))
                    if ((rub.rubroImpuesto > 0) && (descuento > 0) && (Recuperacion_Iva == true))
                    {
                        descuento_subtotal = descuento / (1 + porc_impuesto);
                        descuento_impuesto = Math.Round(descuento - descuento_subtotal, 2);
                        t2.Text = descuento_impuesto.ToString();
                        impuesto += Math.Round(descuento_impuesto, 2);
                        subtotal += Math.Round(descuento_subtotal, 2);
                        total += Math.Round(descuento, 2);
                    }
                    else if (descuento > 0) 
                    {
                        subtotal += Math.Round(descuento, 2);
                        total += Math.Round(descuento, 2);
                    }
                    else
                    {
                        t1.Text = "0.00";
                        t2.Text = "0.00";
                    }
                }
            }
        }
        tb_iva.Text = impuesto.ToString("#,#.00#;(#,#.00#)");
        tb_subtotal.Text = subtotal.ToString("#,#.00#;(#,#.00#)");
        tb_total.Text = total.ToString("#,#.00#;(#,#.00#)");
        
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        ListItem item = null;
        ArrayList arr = null;
        if (!Page.IsPostBack)
        {
            if (Request.QueryString["factID"] != null && !Request.QueryString["factID"].ToString().Equals(""))
            {
                factID = int.Parse(Request.QueryString["factID"].ToString().Trim());
                //Obtengo los rubros de la factura
                RE_GenericBean factdata = (RE_GenericBean)DB.getFacturaData(factID);
                #region Cargar Serie de Factura
                UsuarioBean Usuario_Temporal = new UsuarioBean();
                Usuario_Temporal.SucursalID = factdata.intC2;
                Usuario_Temporal.contaID = factdata.intC9;
                Usuario_Temporal.PaisID = factdata.intC8;
                Usuario_Temporal.Operacion = 1;
                arr = null;
                arr = (ArrayList)DB.Get_Series_By_Monedas_Conta(1, Usuario_Temporal, 0);
                item = new ListItem("Seleccione...", "0");
                lb_serie_factura.Items.Clear();
                lb_serie_factura.Items.Add(item);
                foreach (RE_GenericBean Bean_Serie in arr)
                {
                    item = new ListItem(Bean_Serie.strC1, Bean_Serie.strC2);
                    lb_serie_factura.Items.Add(item);
                }
                #endregion
                int Doc_ID = DB.getDocumentoID(factdata.intC2, factdata.strC28, 1, user);
                lb_serie_factura.SelectedValue = Doc_ID.ToString();//Serie
                tbCliCod.Text=factdata.intC3.ToString();
                tb_nombre.Text=factdata.strC3;
                tb_nit.Text=factdata.strC2;
                tb_razon.Text = factdata.strC26;
                lb_imp_exp.SelectedValue = factdata.intC6.ToString();
                lb_moneda.SelectedValue = factdata.intC4.ToString();
                lb_tipocobro.Text = factdata.intC7.ToString();
                tb_direccion.Text = factdata.strC4;
                tb_observaciones.Text = factdata.strC7;
                tb_referencia.Text = factdata.strC27;
                tb_naviera.Text = factdata.strC13;
                tb_vapor.Text = factdata.strC14;
                tb_contenedor.Text = factdata.strC11;
                tb_hbl.Text = factdata.strC9;
                tb_mbl.Text = factdata.strC10;
                tb_routing.Text = factdata.strC12;
                tb_contenedor.Text = factdata.strC11;
                tb_shipper.Text = factdata.strC15;
                tb_orden.Text = factdata.strC16;
                tb_consignee.Text = factdata.strC17;
                tb_comodity.Text = factdata.strC18;
                tb_paquetes1.Text = factdata.strC19;
                tb_paquetes2.Text = factdata.strC32;
                tb_peso.Text = factdata.strC20;
                tb_vol.Text = factdata.strC21;
                tb_dua_ingreso.Text = factdata.strC22;
                tb_dua_salida.Text = factdata.strC23;
                tb_vendedor1.Text = factdata.strC24;
                tb_vendedor2.Text = factdata.strC25;
                tb_fecha.Text = factdata.strC5;
                tb_doc.Text = "";
                if (factdata.strC49.Length > 41 && (Usuario_Temporal.PaisID == 5 || Usuario_Temporal.PaisID == 21))   
                {
                    tb_doc.Text = factdata.strC49.Substring(21, 20).ToString(); //2019-07-11
                }              
                lb_serie_factura.SelectedValue = factdata.strC28;
                lb_facid.Text = factdata.strC1;
                tb_factura_total.Text = factdata.decC3.ToString();
                tb_factura_impuesto.Text = factdata.decC2.ToString();
                tb_factura_subtotal.Text = factdata.decC1.ToString();
                tb_allin.Text = factdata.strC30;
                lb_contribuyente.SelectedValue = factdata.strC55;
                drp_tipopersona.SelectedValue = factdata.strC58;
                if (drp_tipopersona.SelectedValue == "10")
                {
                    lb_tipo_transaccion.SelectedValue = "114";
                }
                dgw.DataSource = (DataTable)DB.getRubByDoc_xNC(factID, 1, 3);
                dgw.DataBind();
                #region Definir Moneda y serie del Documento en Base al tipo de Contabilidad
                int tipo_contabilidad = int.Parse(Session["Contabilidad"].ToString());
                if (tipo_contabilidad == 2)
                {
                    lb_moneda.SelectedValue = user.Moneda.ToString();
                }
                else
                {
                    lb_moneda.SelectedValue = user.Moneda.ToString();
                }
                #endregion
                FacturaBean Estado_Factura = (FacturaBean)DB.getEstadoFactura(factID);
                lbl_total_factura.Text = Estado_Factura.Total.ToString();
                lbl_total_abonos.Text = Estado_Factura.SubTot.ToString();
                lbl_saldo.Text = Math.Round((Estado_Factura.Total - Estado_Factura.SubTot), 2).ToString();
                if (decimal.Parse(lbl_total_abonos.Text) == 0)
                {
                    chk_anulacion.Visible = true;
                    lbl_anulacion.Visible = true;
                }
                else if (decimal.Parse(lbl_total_abonos.Text) > 0)
                {
                    chk_anulacion.Visible = false;
                    lbl_anulacion.Visible = false;
                }
                if (decimal.Parse(lbl_saldo.Text) == 0)
                {
                    dgw.Enabled = false;
                    bt_Enviar.Enabled = false;
                }
                if (drp_tipopersona.SelectedValue == "3")
                {
                    #region Cargar Datos Adicionales del Cliente
                    string criterio = " a.id_cliente=" + tbCliCod.Text;
                    ArrayList clientearr = (ArrayList)DB.getClientes(criterio, user, "");
                    RE_GenericBean clienteBean;
                    if ((clientearr != null) && (clientearr.Count > 0))
                    {
                        clienteBean = (RE_GenericBean)clientearr[0];
                        lbl_correo_documento_electronico.Text = clienteBean.strC7;
                        drp_tipo_identificacion_cliente.SelectedValue = clienteBean.strC10;
                    }
                }
                else if (drp_tipopersona.SelectedValue == "10")
                {
                    drp_tipo_identificacion_cliente.SelectedValue = "10";//Tipo de Identificacion del Extranjero para Intercompanys
                }
                #endregion
                //Calculo de Dias del Documento para acreditar o no el IVA
                DateTime ahora = DateTime.Now;
                string[] f = tb_fecha.Text.Split('/');
                DateTime fecha = new DateTime(int.Parse(f[0]), int.Parse(f[1]), int.Parse(f[2]));
                TimeSpan RangeTax = ahora.Subtract(fecha);
                LimitDays.Text = RangeTax.Days.ToString();
                Recuperacion_Iva = DB.Validar_Periodo_Recuperacion_Impuesto(user, tb_fecha.Text);
                #region Cargar Serie de Notas de Credito en base a Moneda de la Factura
                //ArrayList Arr_Temp = null;
                //Arr_Temp = (ArrayList)DB.Get_Series_By_Monedas(1, user, 8, 1);
                //ListItem Item_Temp = new ListItem("Seleccione...", "0");
                //lb_serieNC.Items.Clear();
                //lb_serieNC.Items.Add(Item_Temp);
                //foreach (RE_GenericBean Bean_Serie in Arr_Temp)
                //{
                //    Item_Temp = new ListItem(Bean_Serie.strC1, Bean_Serie.strC2);
                //    lb_serieNC.Items.Add(Item_Temp);
                //}
                #endregion
            }
        }
        if (Math.Round(double.Parse(tb_total.Text), 2) == double.Parse(lbl_total_factura.Text.ToString()))
        {
            chk_anulacion.Checked = true;
        }
        else
        {
            chk_anulacion.Checked = false;
        }
    }

    protected void bt_Enviar_Click(object sender, EventArgs e)
    {
        if (lb_serieNC.Items.Count > 0)
        {
            if (lb_serieNC.SelectedValue == "0")
            {
                WebMsgBox.Show("Por favor seleccione la Serie a utilizar");
                return;
            }
        }
        if ((tb_total.Text.Trim().Equals("")) || (tb_total.Text.Trim().Equals("0")))
        {
            WebMsgBox.Show("Debe indicar el valor del Credito a Aplicar.");
        }
        else
        {

            decimal total = 0, saldo = 0, abono = 0, descuento_aplicar = 0;
            total = decimal.Parse(tb_total.Text);
            if (total > decimal.Parse(lbl_saldo.Text))
            {
                WebMsgBox.Show("No puedo aplicar un credito mayor al saldo disponible");
                return;
            }
            int servicio = 0;
            user = (UsuarioBean)Session["usuario"];
            FacturaBean Estado_Factura = null;
            RE_GenericBean rgb = new RE_GenericBean();
            rgb.Fecha_Hora = lb_fecha_hora.Text;
            rgb.intC1 = int.Parse(lb_tipo_transaccion.SelectedValue);//tipo transaccion
            rgb.intC2 = int.Parse(lb_moneda.SelectedValue);//tipo moneda
            rgb.intC3 = int.Parse(Session["Contabilidad"].ToString());//tipo de contabilidad
            rgb.intC4 = int.Parse(Request.QueryString["factID"].ToString().Trim());
            rgb.intC5 = 3;//tipo de transaccion
            rgb.intC6 = int.Parse(drp_tipopersona.SelectedValue);
            rgb.intC7 = int.Parse(tbCliCod.Text.Trim());//Cliente ID
            rgb.strC1 = tb_observaciones.Text;//observaciones
            rgb.strC2 = lb_serie_factura.SelectedItem.Text;//Serie Factura
            rgb.Factura_Ref_Fecha = tb_fecha.Text; //fecha factura 2019-07-09
            rgb.Factura_Ref_Doc = tb_doc.Text; //esignature 2019-07-11
            rgb.strC3 = lb_facid.Text;//Correlativo Factura
            rgb.strC20 = lb_serieNC.SelectedItem.Text;
            rgb.strC35 = tb_nit.Text;//nit
            rgb.strC36 = lbl_correo_documento_electronico.Text;
            rgb.strC37 = tb_referencia_correo.Text;
            rgb.strC38 = tb_allin.Text;
            rgb.strC9 = tb_hbl.Text;
            rgb.strC10 = tb_mbl.Text;
            rgb.strC11 = tb_contenedor.Text;
            rgb.strC12 = tb_routing.Text;
            rgb.strC13 = tb_poliza.Text.Trim();//Poliza Aduanal
            rgb.decC1 = decimal.Parse(tb_total.Text.Trim());//tnc_total
            rgb.IP_Address = Request.ServerVariables["REMOTE_ADDR"].ToString();
            if ((rgb.decC1 == 0) && (user.PaisID == 1))
            {
                WebMsgBox.Show("No se puede emitir una Nota de Credito con Valor cero");
                return;
            }
            if (tb_iva.Text.Equals(""))
            {
                rgb.decC3 = 0;
            }
            else
            {
                rgb.decC3 = decimal.Parse(tb_iva.Text);//tnc_impuesto                
            }
            rgb.decC4 = rgb.decC1 - rgb.decC3;//tnc_subtotal
            rgb.decC5 = decimal.Parse(tb_factura_subtotal.Text);
            rgb.decC6 = decimal.Parse(tb_factura_impuesto.Text);
            rgb.decC7 = decimal.Parse(tb_factura_total.Text);
            rgb.intC8 = int.Parse(lbl_serie_id.Text);
            rgb.Nombre_Cliente = tb_nombre.Text.Trim();//Nombre del Cliente
            if (lb_moneda.SelectedValue.Equals("8"))
            {
                rgb.decC2 = Math.Round((rgb.decC1 * user.pais.TipoCambio), 2);
            }
            else
            {
                rgb.decC2 = Math.Round((rgb.decC1 / user.pais.TipoCambio), 2);
            }
            #region Validacion Longitud del Nombre del Cliente
            if ((user.PaisID == 1) || (user.PaisID == 15))
            {
                tb_nombre.Text = tb_nombre.Text.Trim();
                if (tb_nombre.Text.Length > 150)
                {
                    WebMsgBox.Show("El nombre del cliente tiene mas de 150 caracteres, favor corregir en el Catalogo de Clientes");
                    return;
                }
                else
                {
                    rgb.Nombre_Cliente = tb_nombre.Text.Trim();
                }
            }
            else
            {
                rgb.Nombre_Cliente = tb_nombre.Text.Trim();
            }
            #endregion
            #region Validacion Longitud de Direccion
            if (((user.PaisID == 1) || (user.PaisID == 15)) && (rgb.intC2 == 1))
            {
                tb_direccion.Text = tb_direccion.Text.Trim();
                if (tb_direccion.Text.Length > 160)
                {
                    WebMsgBox.Show("La direccion del cliente tiene mas de 80 caracteres, favor corregir en el Catalogo de Clientes");
                    return;
                }
                else
                {
                    rgb.Direccion = tb_direccion.Text.Trim();
                }
            }
            else
            {
                rgb.Direccion = tb_direccion.Text.Trim();
            }
            #endregion
            #region Validaciones Facturacion Electronica Costa Rica
            if (((user.PaisID == 5) || (user.PaisID == 21)) && (lbl_tipo_serie.Text == "1"))
            {
                if (drp_tipopersona.SelectedValue == "10")
                {
                    //bt_Enviar.Enabled = false;
                    //bt_nota_credito_virtual.Enabled = false;
                    //WebMsgBox.Show("No es posible emitir Nota de Credito Intercompany a traves de una Serie Electronica, por favor pregunte al Contador de Costa Rica la forma correcta de proceder.");
                    //return;
                }
                if (drp_tipo_identificacion_cliente.SelectedValue == "0")
                {
                    WebMsgBox.Show("La Nota de Credito no fue guardada, ni procesada por el Ministerio de Hacienda porque el Cliente.: " + tb_nombre.Text + " con ID.: " + tbCliCod.Text + " no tiene Asignado Tipo de Identificacion Tributaria en el Catalogo de Clientes, por favor contacte al personal de Contabilidad para que actualicen y asigen el Tipo en el Catalago de Clientes, posterior a ello genere nuevamente la Nota de Credito.");
                    return;
                }
                if ((lbl_correo_documento_electronico.Text == "") && (drp_tipopersona.SelectedValue == "3"))
                {
                    WebMsgBox.Show("La Nota de Credito no fue guardada, el Cliente.: " + tb_nombre.Text + " con ID.: " + tbCliCod.Text + " no tiene asignado correo electronico en el Catalogo de Clientes para recibir Notas de Credito Electronicas, por favor asigne para poder guardar la Nota de Credito.");
                    return;
                }
            }
            #endregion
            bool valida_cliente = Validar_Restricciones("btn_enviar_cliente");
            if (valida_cliente == true)
            {
                return;
            }
            bool valida_nit = Validar_Restricciones("send_nit");
            if (valida_nit == true)
            {
                return;
            }
            ////************************************************** obtengo las cuentas *****************
            MatOpBean mat = new MatOpBean();
            mat.paiID = user.PaisID;
            mat.monID = int.Parse(lb_moneda.SelectedValue);
            mat.contaID = rgb.intC3;
            if (mat.contaID == 1)
            {
                mat.tranID = 1;// Fiscal Factura
            }
            else
            {
                mat.tranID = 7;// Financiera Invoice
            }
            mat.impexpID = int.Parse(lb_imp_exp.SelectedValue);
            mat.cobroID = int.Parse(lb_tipocobro.Text);
            mat.contriID = int.Parse(lb_contribuyente.SelectedValue);
            int matOpID = DB.getMatrizOperacionID(int.Parse(lb_tipo_transaccion.SelectedValue), mat.monID, user.PaisID, mat.contaID);
            ArrayList ctas_cargo = (ArrayList)DB.getMatrizConfiguracion(matOpID);
            #region Definir Estado de Factura
            if ((chk_anulacion.Visible == true) && (chk_anulacion.Checked == true))
            {
                rgb.Estado = 3;
            }
            else if (decimal.Parse(tb_total.Text) == decimal.Parse(lbl_saldo.Text))
            {
                rgb.Estado = 4;
            }
            else if (decimal.Parse(tb_total.Text) < decimal.Parse(lbl_saldo.Text))
            {
                rgb.Estado = 2;
            }
            #endregion
            #region Validacion Anulacion de Intercompanys
            if (rgb.Estado == 3)
            {
                #region Validar Transaccion Encadenada
                int tranID = 1;
                int id = rgb.intC4;
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

                    WebMsgBox.Show("Transaccion encadenada (FACTURA " + Arr_Doc_Factura[1].ToString() + "-" + Arr_Doc_Factura[2].ToString() + "), no es posible aplicar Nota de Credito porque afectaria el Monto Total por Pagar a la Aseguradora, esta transaccion solo puede ser anulada al anular la Transaccion Padre.: PROVISION A LA ASEGURADORA Serie.: " + Arr_Doc_Padre[1].ToString() + " y Correlativo.: " + Arr_Doc_Padre[2].ToString() + " .");
                    return;
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
                        if (rgb.intC6 == 10)
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
                                DB.Anular_Documentos(int.Parse(Hija.strC2), int.Parse(Hija.strC3), user, user.contaID);
                                DB.Desactivar_Transaccion_Encadenada_Log(int.Parse(Hija.strC1), user);
                                //Aqui me quede
                                int resultado_anulacion = DB.Registrar_Anulacion(user, 1, rgb.intC4, "Anulacion a traves de Nota de Credito");
                            }
                            #endregion
                        }
                    }
                    #endregion
                }
                #endregion
            }
            #endregion
            #region Validacion de Serie de Exportacion
            if (user.PaisID == 1)
            {
                if (Request.QueryString["factID"] != null && !Request.QueryString["factID"].ToString().Equals(""))
                {
                    int tfaID = 0;
                    tfaID = int.Parse(Request.QueryString["factID"].ToString().Trim());
                    RE_GenericBean Factura_Bean = (RE_GenericBean)DB.getFacturaData(tfaID);
                    int Serie_Factura_ID = DB.getDocumentoID(Factura_Bean.intC2, Factura_Bean.strC28, 1, user);
                    RE_GenericBean Serie_Bean = (RE_GenericBean)DB.getFactura(Serie_Factura_ID, 1);
                    if (Serie_Bean.strC9 == "FACE72")
                    {
                        rgb.Exportacion_Nombre_Comercial = Factura_Bean.strC3;
                        rgb.Exportacion_Identificacion_Tributaria = Factura_Bean.strC2;
                    }
                }
            }
            #endregion
            //****************************************** Recorro el grid para contabilizar los rubros
            //recorro el datagrid para armar la factura
            Label lb1 = new Label();
            Label lb2 = new Label();
            Label lb3 = new Label();
            Label lb4 = new Label();
            Label lb5 = new Label();
            Label lb6 = new Label();
            TextBox t1 = new TextBox();
            TextBox t2 = new TextBox();
            Rubros rubro;
            #region Obtener Descuentos
            XML_Bean Bean_IVA = new XML_Bean();
            XML_Bean Bean_IVACERO = new XML_Bean();
            foreach (GridViewRow row in dgw.Rows)
            {
                lb1.Text = row.Cells[2].Text.Trim();
                lb2.Text = row.Cells[3].Text.Trim();
                lb6.Text = row.Cells[4].Text.Trim();
                lb3.Text = row.Cells[7].Text.Trim();
                lb4.Text = row.Cells[8].Text.Trim();
                lb5.Text = row.Cells[9].Text.Trim();
                t1 = (TextBox)row.FindControl("TextBox1");
                t2 = (TextBox)row.FindControl("TextBox2");
                if (t1.Text == "")
                { t1.Text = "0.00"; }
                if (t2.Text == "")
                { t2.Text = "0.00"; }
                rubro = new Rubros();
                rubro.rubroID = long.Parse(lb1.Text); ;
                rubro.rubroName = lb2.Text.Trim();
                rubro.rubroTot = double.Parse(t1.Text);
                rubro.rubroImpuesto = double.Parse(t2.Text);
                rubro.rubroSubTot = rubro.rubroTot - rubro.rubroImpuesto;
                rubro.rubroTypeID = Utility.TraducirServiciotoINT(lb6.Text);
                rubro.rubroMoneda = int.Parse(lb_moneda.SelectedValue);
                if (lb_moneda.SelectedValue.Equals("8"))
                {
                    rubro.rubroEquivalente = Math.Round((rubro.rubroTot * (double)user.pais.TipoCambio), 2);
                }
                else
                {
                    rubro.rubroEquivalente = Math.Round((rubro.rubroTot / (double)user.pais.TipoCambio), 2);
                }
                servicio = rubro.rubroTypeID;
                rubro.cta_debe = (ArrayList)DB.getCtaContablebyRubro("debe", (int)rubro.rubroID, user.PaisID, mat.tranID, mat.contriID, mat.monID, mat.impexpID, mat.cobroID, mat.contaID, servicio);
                rubro.cta_haber = (ArrayList)DB.getCtaContablebyRubro("haber", (int)rubro.rubroID, user.PaisID, mat.tranID, mat.contriID, mat.monID, mat.impexpID, mat.cobroID, mat.contaID, servicio);
                if ((rubro.cta_debe == null && rubro.cta_haber == null) || (rubro.cta_debe.Count == 0 && rubro.cta_haber.Count == 0))
                {
                    WebMsgBox.Show("El rubro " + rubro.rubroName + " no tiene configurada ninguna cuenta contable para la combinacion de cobro que esta realizando, por favor pongase en contacto con el Contador");
                    return;
                }
                if (rgb.arr1 == null) rgb.arr1 = new ArrayList();
                if (rubro.rubroTot > 0)
                    rgb.arr1.Add(rubro);
                if (rgb.arr2 == null) rgb.arr2 = new ArrayList();
                #region Tasas de Impuesto
                if (t1.Text != "0.00")
                {
                    if (rubro.rubroImpuesto > 0)
                    {
                        Bean_IVA.stC1 = "IVA";
                        Bean_IVA.decC1 += decimal.Parse(rubro.rubroSubTot.ToString());//Base
                        Bean_IVA.stC2 = "12";//Tasa
                        Bean_IVA.decC2 += decimal.Parse(rubro.rubroImpuesto.ToString());//Monto
                        rgb.TotalIngresosNG += rubro.rubroSubTot;
                    }
                    else if (rubro.rubroImpuesto == 0)
                    {
                        Bean_IVACERO.stC1 = "IVA";
                        Bean_IVACERO.decC1 += decimal.Parse(rubro.rubroSubTot.ToString());//Base
                        Bean_IVACERO.stC2 = "0";//Tasa
                        Bean_IVACERO.decC2 += decimal.Parse(rubro.rubroImpuesto.ToString());//Monto
                    }
                }
                #endregion
            }
            #endregion
            #region Validacion ALL-IN
            if (tb_allin.Text.Trim() != "")
            {
                rubro = new Rubros();
                rubro.rubroID = long.Parse("0");
                rubro.rubroName = tb_allin.Text;
                rubro.rubtoType = "Servicio";
                rubro.rubroSubTot = double.Parse(tb_subtotal.Text);
                rubro.rubroImpuesto = double.Parse(tb_iva.Text);
                rubro.rubroTot = double.Parse(tb_total.Text);
                rubro.rubroTotD = 0;
                rubro.rubroCommentario = "";
                rubro.rubroMoneda = long.Parse(lb_moneda.SelectedValue);
                rubro.rubroTypeID = 0;
                if (rgb.arr3 == null) rgb.arr3 = new ArrayList();
                rgb.arr3.Add(rubro);
            }
            #endregion
            if (Bean_IVA.decC2 > 0)
            {
                rgb.arr2.Add(Bean_IVA);
            }
            if (Bean_IVACERO.stC1 == "IVA")
            {
                rgb.arr2.Add(Bean_IVACERO);
            }
            rgb.Factura_Electronica = int.Parse(lbl_tipo_serie.Text);
            rgb.intC10 = 1; //Factura
            DateTime fecha = DateTime.Parse(tb_fecha.Text);
            if (fecha.CompareTo(DateTime.Now) < 0) rgb.boolC2 = true;
            string Check_Existencia = DB.CheckExistDoc(rgb.Fecha_Hora, 3);
            if (Check_Existencia == "0")
            {
                ArrayList result = DB.InsertNotaCredito(rgb, user, rgb.intC3, ctas_cargo);
                if (result == null || result.Count == 0 || int.Parse(result[1].ToString()) == 0 || int.Parse(result[0].ToString()) == -100)
                {
                    #region Facturacion Electronica de Costa Rica
                    if (((user.PaisID == 5) || (user.PaisID == 21)) && (lbl_tipo_serie.Text == "1") && (result != null))
                    {
                        //bool valida_einvoice = Validar_Restricciones("send_einvoice");
                        //if (valida_einvoice == true)
                        //{
                            if (result.Count == 4)
                            {
                                ArrayList Arr_Transmision_CR = (ArrayList)result[3];
                                if (Arr_Transmision_CR[0].ToString() == "0")
                                {
                                    pnl_transmision_electronica.Visible = true;
                                    tb_resultado_transmision.Text = Arr_Transmision_CR[1].ToString();
                                    bt_Enviar.Enabled = false;
                                    bt_nota_credito_virtual.Enabled = false;
                                    WebMsgBox.Show(Arr_Transmision_CR[1].ToString());
                                    return;
                                }
                            }
                        //}
                    }
                    #endregion
                    WebMsgBox.Show("Existió un problema al tratar de guardar la Nota de Credito, por favor intente de nuevo");
                    return;
                }
                else
                {
                    bool valida_einvoice = Validar_Restricciones("send_enc");
                    if (valida_einvoice == true)
                    {
                        #region Transmitir EInvoice
                        if ((user.PaisID == 1) || (user.PaisID == 15))
                        {
                            #region Facturacion Electronica Guatemala
                            string Referencia_Interna = "";
                            Referencia_Interna = result[2].ToString();
                            if ((lbl_tipo_serie.Text == "1") || (lbl_tipo_serie.Text == "2"))
                            {
                                ArrayList Arr_Data = new ArrayList();
                                #region Definir Referencia Interna
                                if (Referencia_Interna == "0")
                                {
                                    WebMsgBox.Show("Existio un error al momento de definir la referencia Interna, porfavor intente de nuevo");
                                    return;
                                }
                                //Referencia_Interna = tbCliCod.Text.Trim() + "0003" + result[1].ToString();
                                rgb.Referencia_Interna = Referencia_Interna;
                                rgb.Correlativo = result[0].ToString();
                                Arr_Data.Add(rgb);
                                Arr_Data.Add(Referencia_Interna);
                                #endregion

                                var fel = DB.DataFEL(user.PaisID, "", "");
                                if (fel.isFELdate) {
                                
		                            //registrar la nota de credito
		                            #region FEL 2019-05-14
		                            try
		                            {
		                                string ncide = result[1].ToString().Trim();
		                                		                                
		                                fel_101017.WebService1 proceso = new fel_101017.WebService1();
		                                user = (UsuarioBean)Session["usuario"];
		                                
                                        var resultado = proceso.Proceso_07_Completo("", "NC", ncide, user.Email, "3", user.PaisID.ToString()); 
		                                if (resultado[3] == "")
		                                {
                                            string iStr = "";
                                            iStr = "Se Transmitio correctamente la Nota de Credito. ";
                                            iStr += "FEL Serie " + resultado[1] + " y Numero " + resultado[2] + ". ";
                                            iStr += "Interno Serie " + lb_serieNC.SelectedItem.Text + " y Correlativo " + result[0].ToString() + ".";
                                            
                                            setButtons("1", iStr, result, resultado[0], Referencia_Interna, iStr, resultado[5]);
		                                    return;
		                                }
		                                else
		                                {		                                    
		                                    setButtons("2", "Existio un error durante la Transmision del Documento con FEL y no se obtuvo firma electronica, La Nota de Credito fue grabada exitosamente con la Serie.: " + lb_serieNC.SelectedItem.Text + " y Folio.: " + result[0].ToString(), result, "-", Referencia_Interna, resultado[3] + " ", "");
		                                    return;
		                                }
		                                //resultado[3] = "Key : " + resultado[0] + " Serie : " + resultado[1] + " Correlativo :" + resultado[2];
		                            }
		                            catch (Exception ex)
		                            {		                                
		                                setButtons("3", "FEL (SAT) se encuentra fuera de linea y no se obtuvo firma electronica. " + ex.Message + ". La Nota de Credito fue grabada exitosamente en el Sistema con la Serie.: " + lb_serieNC.SelectedItem.Text + " y Folio.: " + result[0].ToString() + " y sera retransmitida mediante un proceso automatico en un lapso de 24 horas", result, "-", Referencia_Interna, "", "");
		                                return;
		                            }
		                            #endregion
		                           
		                            //Boolean chk1 = chk_anulacion.Checked;
		                            /*if (chk_anulacion.Checked)
		                            {
		                                //registrar la anulacion
		                                #region FEL 2019-04-23
		                                try
		                                {   //falta validar que esto lo realice cuando anulen la factura checkbox
		                                    string facide = Request.QueryString["factID"].ToString().Trim();

		                                    //2019-04-23
		                                    //http://10.10.1.7:9191/WebService1.asmx
		                                    fel_101017.WebService1 proceso = new fel_101017.WebService1();
		                                    user = (UsuarioBean)Session["usuario"];
		                                    var resultado = proceso.Proceso_09_AnularDocumentoXML("", "", facide, user.ID, "1", 1);
		                                    if (resultado[3] == "")
		                                    {
		                                        //ExmlDoc.InnerXml = DB.Base64String_String(Tag.ResponseData.ResponseData1); string Signature = resultado[0]; //string Signature = DB.Get_Signature(user, ExmlDoc);
		                                        //resultado[3] = "Key : " + resultado[0] + " Serie : " + resultado[1] + " Correlativo :" + resultado[2];
		                                        string iStr = "Se Transmitio correctamente la Anulacion de ID " + facide; //"Serie " + resultado[1] + " y Folio " + resultado[2];
		                                        //setButtons("1", "Se Transmitio correctamente la Nota de Credito Serie " + lb_serieNC.SelectedItem.Text + " y Folio " + result[0].ToString(), result, Signature, Referencia_Interna, resultado[0], resultado[1], "Firma Electronica.: " + Signature + " \n" + "Se Transmitio correctamente la Nota de Credito Serie " + resultado[1] + " y Folio " + resultado[2]);
		                                        setButtons("1", iStr, result, resultado[0], Referencia_Interna, resultado[0], resultado[1], "Firma Electronica.: " + resultado[0] + " \n" + iStr);
		                                        return;
		                                    }
		                                    else
		                                    {                                             
		                                        setButtons("2", "Existio un error durante la Transmision del Documento con FEL y no se obtuvo firma electronica, La Nota de Credito fue grabada exitosamente con la Serie.: " + lb_serieNC.SelectedItem.Text + " y Folio.: " + result[0].ToString(), result, "-", Referencia_Interna, "0", result[1].ToString(), resultado[3] + " ");
		                                        return;
		                                    }

		                                }
		                                catch (Exception ex)
		                                {
		                                    setButtons("3", "FEL (SAT) se encuentra fuera de linea y no se obtuvo firma electronica. " + ex.Message + ". La Nota de Credito fue grabada exitosamente en el Sistema con la Serie.: " + lb_serieNC.SelectedItem.Text + " y Folio.: " + result[0].ToString() + " y sera retransmitida mediante un proceso automatico en un lapso de 24 horas", result, "-", Referencia_Interna, "0", result[1].ToString(), "");
		                                    return;
		                                }
		                                #endregion
		                            }
                                    */
		                        } else {
                                /*2019-10-11
                                #region Generar XML
                                string Query = " and tn_pai_id=" + user.PaisID + " and tn_ttr_id=3 and tn_conta_id=" + user.contaID + " order by tn_nivel, tn_posicion asc  ";
                                XmlDocument ExmlDoc = (XmlDocument)DB.Generar_Xml(user, Arr_Data, Query, 3);
                                #endregion
                                if (ExmlDoc != null)
                                {
                                    #region Guardar XML Transmitido
                                    //ExmlDoc.Save("D:\\EINVOICE\\3" + Referencia_Interna + ".xml");
                                    #endregion
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                        #region Conectar a GFACE
                                    int intentos_transmision = 0;
                                    GFACEWEBSERVICE.TransactionTag Tag = new GFACEWEBSERVICE.TransactionTag();
                                    do
                                    {
                                        Tag = (GFACEWEBSERVICE.TransactionTag)DB.Transmitir_Documento_Electronico(user, ExmlDoc, Referencia_Interna);
                                        intentos_transmision += 1;
                                    } while (((Tag == null)) && (intentos_transmision <= 3));

                                    if (Tag == null)
                                    {
                                        #region Transmision Fallida
                                        WebMsgBox.Show("El GFACE (SAT) se encuentra fuera de linea y no se obtuvo firma electronica, La Nota de Credito fue grabada exitosamente en el Sistema con la Serie.: " + lb_serieNC.SelectedItem.Text + " y Folio.: " + result[0].ToString() + "  y sera retransmitida mediante un proceso automatico en un lapso de 24 horas");
                                        #region Registrar Referencia Interna
                                        int result_signature = 0;
                                        ArrayList EArr = new ArrayList();
                                        EArr.Add(3);
                                        EArr.Add("-");
                                        EArr.Add(int.Parse(lbl_tipo_serie.Text));
                                        EArr.Add(int.Parse(result[1].ToString()));
                                        EArr.Add(Referencia_Interna);
                                        EArr.Add("0");
                                        EArr.Add(result[0].ToString());
                                        EArr.Add(ExmlDoc);
                                        result_signature = DB.Update_Signature(user, EArr);
                                        #endregion
                                        tb_corrNC.Text = result[0].ToString();
                                        bt_Enviar.Enabled = false;
                                        //Toque Aqui
                                        bt_Imprimir.Enabled = false;
                                        btn_ver_pdf.Visible = false;
                                        bt_nota_credito_virtual.Enabled = true;
                                        factID = int.Parse(Request.QueryString["factID"].ToString().Trim());
                                        Estado_Factura = (FacturaBean)DB.getEstadoFactura(factID);
                                        lbl_total_factura.Text = Estado_Factura.Total.ToString();
                                        lbl_total_abonos.Text = Estado_Factura.SubTot.ToString();
                                        lbl_saldo.Text = Math.Round((Estado_Factura.Total - Estado_Factura.SubTot), 2).ToString();
                                        #region Mostrando la Partida Contable
                                        gv_detalle_partida.DataSource = DB.getPolizaDiario(user, double.Parse(result[1].ToString()), 3, 0);
                                        lbl_NC_ID.Text = result[1].ToString();
                                        gv_detalle_partida.DataBind();
                                        gv_detalle_partida.Rows[gv_detalle_partida.Rows.Count - 1].Font.Bold = true;
                                        #endregion
                                        if (decimal.Parse(lbl_total_abonos.Text) == 0)
                                        {
                                            chk_anulacion.Visible = true;
                                            lbl_anulacion.Visible = true;
                                        }
                                        else if (decimal.Parse(lbl_total_abonos.Text) > 0)
                                        {
                                            chk_anulacion.Visible = false;
                                            lbl_anulacion.Visible = false;
                                        }
                                        if (decimal.Parse(lbl_saldo.Text) == 0)
                                        {
                                            dgw.Enabled = false;
                                            bt_Enviar.Enabled = false;
                                        }
                                        return;
                                        #endregion
                                    }
                                    else if (Tag.Response.Result == false)
                                    {
                                        #region Transmision Fallida
                                        WebMsgBox.Show("Existio un error durante la Transmision del Documento con el GFACE y no se obtuvo firma electronica, La Nota de Credito fue grabada exitosamente con la Serie.: " + lb_serieNC.SelectedItem.Text + " y Folio.: " + result[0].ToString());
                                        #region Registrar Referencia Interna
                                        int result_signature = 0;
                                        ArrayList EArr = new ArrayList();
                                        EArr.Add(3);
                                        EArr.Add("-");
                                        EArr.Add(int.Parse(lbl_tipo_serie.Text));
                                        EArr.Add(int.Parse(result[1].ToString()));
                                        EArr.Add(Referencia_Interna);
                                        EArr.Add("0");
                                        EArr.Add(result[0].ToString());
                                        EArr.Add(ExmlDoc);
                                        result_signature = DB.Update_Signature(user, EArr);
                                        #endregion
                                        tb_resultado_transmision.Text = Tag.Response.Hint + " " + Tag.Response.Description + " " + Tag.Response.Data + " ";
                                        pnl_transmision_electronica.Visible = true;
                                        tb_corrNC.Text = result[0].ToString();
                                        bt_Enviar.Enabled = false;
                                        //Toque Aqui
                                        bt_Imprimir.Enabled = false;
                                        btn_ver_pdf.Visible = false;
                                        bt_nota_credito_virtual.Enabled = true;
                                        factID = int.Parse(Request.QueryString["factID"].ToString().Trim());
                                        Estado_Factura = (FacturaBean)DB.getEstadoFactura(factID);
                                        lbl_total_factura.Text = Estado_Factura.Total.ToString();
                                        lbl_total_abonos.Text = Estado_Factura.SubTot.ToString();
                                        lbl_saldo.Text = Math.Round((Estado_Factura.Total - Estado_Factura.SubTot), 2).ToString();
                                        #region Mostrando la Partida Contable
                                        gv_detalle_partida.DataSource = DB.getPolizaDiario(user, double.Parse(result[1].ToString()), 3, 0);
                                        lbl_NC_ID.Text = result[1].ToString();
                                        gv_detalle_partida.DataBind();
                                        gv_detalle_partida.Rows[gv_detalle_partida.Rows.Count - 1].Font.Bold = true;
                                        #endregion
                                        if (decimal.Parse(lbl_total_abonos.Text) == 0)
                                        {
                                            chk_anulacion.Visible = true;
                                            lbl_anulacion.Visible = true;
                                        }
                                        else if (decimal.Parse(lbl_total_abonos.Text) > 0)
                                        {
                                            chk_anulacion.Visible = false;
                                            lbl_anulacion.Visible = false;
                                        }
                                        if (decimal.Parse(lbl_saldo.Text) == 0)
                                        {
                                            dgw.Enabled = false;
                                            bt_Enviar.Enabled = false;
                                        }
                                        return;
                                        #endregion
                                    }
                                    else if (Tag.Response.Result == true)
                                    {
                                        #region Transmision Exitosa
                                        WebMsgBox.Show("Se Transmitio correctamente la Nota de Credito Serie " + lb_serieNC.SelectedItem.Text + " y Folio " + result[0].ToString());
                                        #region Registrar Firma Electronica
                                        string Signature = "";
                                        int result_signature = 0;
                                        ArrayList EArr = new ArrayList();
                                        ExmlDoc.InnerXml = DB.Base64String_String(Tag.ResponseData.ResponseData1);
                                        Signature = DB.Get_Signature(user, ExmlDoc);
                                        EArr.Add(3);
                                        EArr.Add(Signature);
                                        EArr.Add(int.Parse(lbl_tipo_serie.Text));
                                        EArr.Add(int.Parse(result[1].ToString()));
                                        EArr.Add(Referencia_Interna);
                                        EArr.Add(Tag.Response.Identifier.DocumentGUID);
                                        EArr.Add(Tag.Response.Identifier.Serial);
                                        EArr.Add(null);
                                        result_signature = DB.Update_Signature(user, EArr);
                                        pnl_transmision_electronica.Visible = true;
                                        tb_resultado_transmision.Text = "Firma Electronica.: " + Signature + " \n" + "Se Transmitio correctamente la Nota de Credito Serie " + Tag.Response.Identifier.Batch + " y Folio " + Tag.Response.Identifier.Serial;
                                        lbl_internal_reference.Text = Referencia_Interna;
                                        #endregion
                                        tb_corrNC.Text = result[0].ToString();
                                        bt_Enviar.Enabled = false;
                                        //Toque Aqui
                                        bt_Imprimir.Enabled = true;
                                        btn_ver_pdf.Visible = false;
                                        bt_nota_credito_virtual.Enabled = true;
                                        factID = int.Parse(Request.QueryString["factID"].ToString().Trim());
                                        Estado_Factura = (FacturaBean)DB.getEstadoFactura(factID);
                                        lbl_total_factura.Text = Estado_Factura.Total.ToString();
                                        lbl_total_abonos.Text = Estado_Factura.SubTot.ToString();
                                        lbl_saldo.Text = Math.Round((Estado_Factura.Total - Estado_Factura.SubTot), 2).ToString();
                                        #region Mostrando la Partida Contable
                                        gv_detalle_partida.DataSource = DB.getPolizaDiario(user, double.Parse(result[1].ToString()), 3, 0);
                                        lbl_NC_ID.Text = result[1].ToString();
                                        gv_detalle_partida.DataBind();
                                        gv_detalle_partida.Rows[gv_detalle_partida.Rows.Count - 1].Font.Bold = true;
                                        #endregion
                                        if (decimal.Parse(lbl_total_abonos.Text) == 0)
                                        {
                                            chk_anulacion.Visible = true;
                                            lbl_anulacion.Visible = true;
                                        }
                                        else if (decimal.Parse(lbl_total_abonos.Text) > 0)
                                        {
                                            chk_anulacion.Visible = false;
                                            lbl_anulacion.Visible = false;
                                        }
                                        if (decimal.Parse(lbl_saldo.Text) == 0)
                                        {
                                            dgw.Enabled = false;
                                            bt_Enviar.Enabled = false;
                                        }
                                        return;
                                        #endregion
                                    }
                                    #endregion
                                    }
                                    else
                                    {
                                        WebMsgBox.Show("Existio un error en la Generacion del XML, porfavor intente de nuevo");
                                        return;
                                    }*/
                                }    
                            }
                            #endregion
                        }
                        else if (user.PaisID == 5)
                        {
                            #region BK Facturacion Electronica Costa Rica
                            //if (lbl_tipo_serie.Text == "1")
                            //{
                            //    EInvoice_CR EInvoice = new EInvoice_CR();
                            //    ArrayList Arr_Transmision_CR = EInvoice.Generar_Firma_Electronica(user, 3, 0, null);
                            //    if (Arr_Transmision_CR[0].ToString() == "0")
                            //    {
                            //        WebMsgBox.Show(Arr_Transmision_CR[1].ToString());
                            //        pnl_transmision_electronica.Visible = true;
                            //        tb_resultado_transmision.Text = Arr_Transmision_CR[1].ToString();
                            //    }
                            //    else if (Arr_Transmision_CR[0].ToString() == "1")
                            //    {
                            //        WebMsgBox.Show(Arr_Transmision_CR[1].ToString());
                            //        pnl_transmision_electronica.Visible = true;
                            //        tb_resultado_transmision.Text = Arr_Transmision_CR[1].ToString();
                            //        tb_corrNC.Text = Arr_Transmision_CR[2].ToString();
                            //    }
                            //    lbl_NC_ID.Text = result[1].ToString();
                            //    user = (UsuarioBean)Session["usuario"];
                            //    #region Mostrando la Partida contable
                            //    gv_detalle_partida.DataSource = DB.getPolizaDiario(user, double.Parse(result[1].ToString()), 3, 0);
                            //    lbl_NC_ID.Text = result[1].ToString();
                            //    gv_detalle_partida.DataBind();
                            //    gv_detalle_partida.Rows[gv_detalle_partida.Rows.Count - 1].Font.Bold = true;
                            //    #endregion
                            //    bt_Enviar.Enabled = false;
                            //    bt_nota_credito_virtual.Enabled = true;
                            //    return;
                            //}
                            #endregion
                        }
                        #endregion
                    }

                    #region Facturacion Electronica de Costa Rica
                    if (((user.PaisID == 5) || (user.PaisID == 21)) && (lbl_tipo_serie.Text == "1"))
                    {
                        if (result.Count == 4)
                        {
                            ArrayList Arr_Transmision_CR = (ArrayList)result[3];
                            if (Arr_Transmision_CR[0].ToString() == "1")
                            {
                                pnl_transmision_electronica.Visible = true;
                                tb_resultado_transmision.Text = Arr_Transmision_CR[1].ToString();
                                tb_corrNC.Text = Arr_Transmision_CR[2].ToString();
                                lbl_NC_ID.Text = result[0].ToString();
                                #region Mostrando la Partida contable
                                //Mostrando la Partida contable generada
                                gv_detalle_partida.DataSource = DB.getPolizaDiario(user, double.Parse(result[1].ToString()), 3, 0);
                                lbl_NC_ID.Text = result[1].ToString();
                                gv_detalle_partida.DataBind();
                                gv_detalle_partida.Rows[gv_detalle_partida.Rows.Count - 1].Font.Bold = true;
                                #endregion
                                bt_Enviar.Enabled = false;
                                bt_nota_credito_virtual.Enabled = true;
                                WebMsgBox.Show(Arr_Transmision_CR[1].ToString());
                                return;
                            }
                        }
                    }
                    #endregion

                    WebMsgBox.Show("La Nota de Credito fue guardada exitosamente con Serie.: " + lb_serieNC.SelectedItem.Text + " y Correlativo.: " + result[0].ToString());
                    tb_corrNC.Text = result[0].ToString();
                    bt_Enviar.Enabled = false;
                    bt_Imprimir.Enabled = true;
                    bt_nota_credito_virtual.Enabled = true;
                    factID = int.Parse(Request.QueryString["factID"].ToString().Trim());
                    Estado_Factura = (FacturaBean)DB.getEstadoFactura(factID);
                    lbl_total_factura.Text = Estado_Factura.Total.ToString();
                    lbl_total_abonos.Text = Estado_Factura.SubTot.ToString();
                    lbl_saldo.Text = Math.Round((Estado_Factura.Total - Estado_Factura.SubTot), 2).ToString();

                    //Mostrando la Partida contable generada
                    gv_detalle_partida.DataSource = DB.getPolizaDiario(user, double.Parse(result[1].ToString()), 3, 0);
                    lbl_NC_ID.Text = result[1].ToString();
                    gv_detalle_partida.DataBind();
                    gv_detalle_partida.Rows[gv_detalle_partida.Rows.Count - 1].Font.Bold = true;

                    if (decimal.Parse(lbl_total_abonos.Text) == 0)
                    {
                        chk_anulacion.Visible = true;
                        lbl_anulacion.Visible = true;
                    }
                    else if (decimal.Parse(lbl_total_abonos.Text) > 0)
                    {
                        chk_anulacion.Visible = false;
                        lbl_anulacion.Visible = false;
                    }
                    if (decimal.Parse(lbl_saldo.Text) == 0)
                    {
                        dgw.Enabled = false;
                        bt_Enviar.Enabled = false;
                    }
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


    protected void setButtons(string tipo, string msg, ArrayList result, string guid, string Referencia_Interna, string result_str, string esignature)
    {
        WebMsgBox.Show(msg); 

        switch (tipo)
        {
            case "1": //ok
                pnl_transmision_electronica.Visible = true;
                tb_resultado_transmision.Text = result_str; //"Firma Electronica.: " + Signature + " \n" + "Se Transmitio correctamente la Factura Serie " + Tag.Response.Identifier.Batch + " y Folio " + Tag.Response.Identifier.Serial;
                lbl_internal_reference.Text = Referencia_Interna;
                var fel = DB.DataFEL(user.PaisID, esignature, guid);
                if (fel.isFEL) 
                    lbl_internal_reference.Text = esignature;
                bt_Imprimir.Enabled = true;
                break;

            case "2": //error
                pnl_transmision_electronica.Visible = true;
                tb_resultado_transmision.Text = result_str; //Tag.Response.Hint + " " + Tag.Response.Description + " " + Tag.Response.Data + " ";
                bt_Imprimir.Enabled = false;
                break;

            case "3": //no connect
                bt_Imprimir.Enabled = false;
                break;

        }

        tb_corrNC.Text = result[0].ToString();
        bt_Enviar.Enabled = false;
        btn_ver_pdf.Visible = false;
        bt_nota_credito_virtual.Enabled = true;
        factID = int.Parse(Request.QueryString["factID"].ToString().Trim());
        FacturaBean Estado_Factura = (FacturaBean)DB.getEstadoFactura(factID);
        lbl_total_factura.Text = Estado_Factura.Total.ToString();
        lbl_total_abonos.Text = Estado_Factura.SubTot.ToString();
        lbl_saldo.Text = Math.Round((Estado_Factura.Total - Estado_Factura.SubTot), 2).ToString();
        gv_detalle_partida.DataSource = DB.getPolizaDiario(user, double.Parse(result[1].ToString()), 3, 0);
        lbl_NC_ID.Text = result[1].ToString();
        gv_detalle_partida.DataBind();
        gv_detalle_partida.Rows[gv_detalle_partida.Rows.Count - 1].Font.Bold = true;
        if (decimal.Parse(lbl_total_abonos.Text) == 0)
        {
            chk_anulacion.Visible = true;
            lbl_anulacion.Visible = true;
        }
        else if (decimal.Parse(lbl_total_abonos.Text) > 0)
        {
            chk_anulacion.Visible = false;
            lbl_anulacion.Visible = false;
        }
        if (decimal.Parse(lbl_saldo.Text) == 0)
        {
            dgw.Enabled = false;
            bt_Enviar.Enabled = false;
        }
    }

    protected void bt_Imprimir_Click(object sender, EventArgs e)
    {
        int ncID = DB.getCorrelativoDoc(user.SucursalID, 3, lb_serieNC.SelectedItem.Text, tb_corrNC.Text);
        String csname1 = "PopupScript";
        Type cstype = this.GetType();
        // Get a ClientScriptManager reference from the Page class.
        ClientScriptManager cs2 = Page.ClientScript;
        #region Seteo de Parametros de Impresion
        user = (UsuarioBean)Session["usuario"];
        string script = "";
        user.ImpresionBean.Operacion = "1";
        user.ImpresionBean.Tipo_Documento = "3";
        user.ImpresionBean.Id = ncID.ToString();
        user.ImpresionBean.Impreso = false;
        Session["usuario"] = user;
        #endregion


        if (((user.PaisID == 6) || (user.PaisID == 25)) && (lbl_tipo_serie.Text == "1"))
        { //En el caso de Panama utiliza la impresion especial para Impresora Fiscal Tally1125, parametro op=0=impresion
            script = "window.open('printersettingsPA.aspx?fac_id=" + ncID.ToString() + "&tipo=3','Imprimir','toolbar=0, status=1, resizable=1, scrollbars=1, top=0, left=0, width=' + (window.screen.width -10) + ', height=' + (window.screen.height -75))";
        }
        else
        {
            #region Impresiones Electronicas
            if (((user.PaisID == 1) || (user.PaisID == 15)) && (lbl_tipo_serie.Text == "1"))
            {
                if (!DB.DownloadFEL(ncID.ToString(), "3", "PDF", Response, lbl_internal_reference.Text, user.PaisID, true, true)) //1:registra - 3:nota credito
                    DB.OpenGFACEpdf(lbl_internal_reference.Text, Response);
            }
            else
            {
                script = "window.open('../ImpresionDocumentos.html?fac_id=" + ncID.ToString() + "&tipo=3&pais=" + user.PaisID.ToString() + "&idioma=" + user.Idioma.ToString() + "&tipo_cambio=" + user.pais.TipoCambio.ToString() + "&contaId=" + user.contaID.ToString() + "','Re-Imprimir','toolbar=0, status=1, resizable=1, scrollbars=1, top=0, left=0, width=' + (window.screen.width -10) + ', height=' + (window.screen.height -75))";

            }
            #endregion
        }

        #region Backup
        //if (user.PaisID != 6) //Si no es Panama, realiza impresion estandar
        //{
        //    //Toque Aqui
        //    #region Impresiones Electronicas
        //    if ((user.PaisID == 1) && (lbl_tipo_serie.Text == "1"))
        //    {
        //        string FilePath = "D:\\EINVOICE\\" + lbl_internal_reference.Text + ".pdf";
        //        if (File.Exists(FilePath) == true)
        //        {
        //            #region Descargar Archivo
        //            Response.Clear();
        //            Response.ContentType = "application/octet-stream";
        //            Response.AddHeader("Content-Disposition", "attachment; filename=" + lbl_internal_reference.Text + ".pdf");
        //            Response.Flush();
        //            Response.WriteFile(FilePath);
        //            Response.End();
        //            #endregion
        //        }
        //        else
        //        {
        //            WebMsgBox.Show("Ruta de acceso al archivo de impresion invalida");
        //        }
        //    }
        //    else
        //    {
        //        script = script = "window.open('printersettings.aspx?fac_id=" + ncID.ToString() + "&tipo=3','Imprimir','toolbar=0, status=1, resizable=1, scrollbars=1, top=0, left=0, width=' + (window.screen.width -10) + ', height=' + (window.screen.height -75))";
        //    }
        //    #endregion
        //    //script = script = "window.open('printersettings.aspx?fac_id=" + ncID.ToString() + "&tipo=3','Imprimir','toolbar=0, status=1, resizable=1, scrollbars=1, top=0, left=0, width=' + (window.screen.width -10) + ', height=' + (window.screen.height -75))";
        //}
        //else if ((user.PaisID == 6) && (lbl_tipo_serie.Text == "1"))
        //{ //En el caso de Panama utiliza la impresion especial para Impresora Fiscal Tally1125, parametro op=0=impresion
        //    script = "window.open('printersettingsPA.aspx?fac_id=" + ncID.ToString() + "&tipo=3','Imprimir','toolbar=0, status=1, resizable=1, scrollbars=1, top=0, left=0, width=' + (window.screen.width -10) + ', height=' + (window.screen.height -75))";
        //}
        //else
        //{
        //    script = "window.open('printersettings.aspx?fac_id=" + lb_facid.Text + "&tipo=1','Imprimir','toolbar=0, status=1, resizable=1, scrollbars=1, top=0, left=0, width=' + (window.screen.width -10) + ', height=' + (window.screen.height -75))";
        //}
        #endregion


        if (!cs2.IsStartupScriptRegistered(cstype, csname1))
        {
            cs2.RegisterStartupScript(cstype, csname1, script, true);
        }
    }
    protected void lb_serieNC_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (lb_serieNC.Items.Count > 0)
        {
            if (lb_serieNC.SelectedValue != "0")
            {
                //BAW FISCAL USD
                if ((user.PaisID == 1) || (user.PaisID == 5) || (user.PaisID == 7) || (user.PaisID == 15) || (user.PaisID == 21) || (user.PaisID == 4) || (user.PaisID == 11) || (user.PaisID == 12) || (user.PaisID == 13) || (user.PaisID == 24) || (user.PaisID == 3) || (user.PaisID == 23))//BAW FISCAL USD
                {
                    int Moneda_Serie_Factura = 0;
                    int Moneda_Serie_Nota_Credito = 0;
                    Moneda_Serie_Factura = DB.Get_Serie_Moneda_By_ID(int.Parse(lb_serie_factura.SelectedValue));
                    Moneda_Serie_Nota_Credito = DB.Get_Serie_Moneda_By_ID(int.Parse(lb_serieNC.SelectedValue));
                    if (Moneda_Serie_Factura != Moneda_Serie_Nota_Credito)
                    {
                        lb_serieNC.SelectedValue = "0";
                        WebMsgBox.Show("Por Favor seleccione una Serie con la misma Moneda de la Factura");
                        return;
                    }
                    lb_moneda.SelectedValue = Moneda_Serie_Factura.ToString();
                    if ((user.PaisID == 5) || (user.PaisID == 21) || (user.PaisID == 1) || (user.PaisID == 15))
                    {
                        RE_GenericBean Data_Serie_FA = (RE_GenericBean)DB.getFactura(int.Parse(lb_serie_factura.SelectedValue), 1);
                        RE_GenericBean Data_Serie_NC = (RE_GenericBean)DB.getFactura(int.Parse(lb_serieNC.SelectedValue), 3);
                        if (Data_Serie_NC.intC14 == 1)
                        {
                            if (Data_Serie_FA.intC14 != 1)
                            {
                                bt_Enviar.Enabled = false;
                                bt_Imprimir.Enabled = false;
                                bt_nota_credito_virtual.Enabled = false;
                                WebMsgBox.Show("No se puede aplicar Nota de Credito Electronica a una Factura que no es Electronica.");
                                return;
                            }
                            else
                            {
                                bt_Enviar.Enabled = true;
                            }
                        }
                        if (Data_Serie_FA.intC14 == 1)
                        {
                            if (Data_Serie_NC.intC14 != 1)
                            {
                                bt_Enviar.Enabled = false;
                                bt_Imprimir.Enabled = false;
                                bt_nota_credito_virtual.Enabled = false;
                                WebMsgBox.Show("No se puede aplicar Nota de Credito normal a una Factura Electronica.");
                                return;
                            }
                            else
                            {
                                bt_Enviar.Enabled = true;
                            }
                        }
                    }
                }
                else
                {
                    int tipo_contabilidad = int.Parse(Session["Contabilidad"].ToString());
                    if (tipo_contabilidad != 2)
                    {
                        RE_GenericBean rgb = (RE_GenericBean)DB.getFacturabySerie(lb_serieNC.SelectedItem.Text, 3, user.SucursalID);
                        lb_moneda.SelectedValue = rgb.intC5.ToString();
                    }
                }
            }
        }
    }
    
    protected void bt_nota_credito_virtual_Click(object sender, EventArgs e)
    {
        user = (UsuarioBean)Session["usuario"];
        int permiso = int.Parse(user.Aplicaciones["6"].ToString());
        if (!((permiso & 32) == 32))
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
            string script = "window.open('../invoice/template_invoice.aspx?id=" + lbl_NC_ID.Text + "&transaccion=3','Imprimir','menubar=1,resizable=1,scrollbars=1,toolbar=1,width=750');";
            if (!cs2.IsStartupScriptRegistered(cstype, csname1))
            {
                cs2.RegisterStartupScript(cstype, csname1, script, true);
            }
        }
    }
    protected bool Validar_Restricciones(string sender)
    {
        bool resultado = false;
        bool paiR = DB.Validar_Pais_Restringido(user);
        if (paiR == true)
        {
            #region FACTURACION ELECTRONICA
            if (DB.Validar_Restriccion_Activa(user, 3, 14) == true)
            {
                if (sender == "send_enc")
                {
                    resultado = true;
                    return resultado;
                }
            }
            #endregion
            #region COBRO UNICAMENTE A CLIENTES LOCALES
            if (DB.Validar_Restriccion_Activa(user, 3, 15) == true)
            {
                if (sender == "btn_enviar_cliente")
                {
                    double cliID = 0;
                    cliID = double.Parse(tbCliCod.Text.Trim().ToString());
                    RE_GenericBean Cliente_Bean = DB.getDataClient(cliID);
                    if (Cliente_Bean.strC7 == user.pais.ISO)
                    {
                        resultado = false;
                    }
                    else
                    {
                        WebMsgBox.Show("No se puede emitir Cobro a clientes Internacionales");
                        bt_Enviar.Enabled = false;
                        resultado = true;
                    }
                    return resultado;
                }
            }
            #endregion
            #region Validar Nit
            if (DB.Validar_Restriccion_Activa(user, 3, 16) == true)
            {
                if (sender == "send_nit")
                {
                    resultado = DB.ValidarNIT(tb_nit.Text, user.pais.ISO);
                    if (resultado == false)
                    {
                        WebMsgBox.Show("Nit invalido, porfavor solicite la correcion en el Catalogo de Clientes");
                        bt_Enviar.Enabled = false;
                        dgw.Enabled = false;
                        resultado = true;
                    }
                    else
                    {
                        resultado = false;
                    }
                    return resultado;
                }
            }
            #endregion
        }
        return resultado;
        #region Backup
        //bool resultado = false;
        //bool paiR = DB.Validar_Pais_Restringido(user);
        //int restriccionID = 0;
        //if (paiR == true)
        //{
        //    ArrayList Arr_Restricciones = (ArrayList)DB.Get_Restricciones_XPais_Tipo(user, user.contaID, 3, user.PaisID, " and a.tbrp_suc_id=" + user.SucursalID + "");
        //    if (Arr_Restricciones.Count > 0)
        //    {
        //        foreach (RE_GenericBean Bean in Arr_Restricciones)
        //        {
        //            restriccionID = int.Parse(Bean.strC2);
        //            switch (restriccionID)
        //            {
        //                case 14:
        //                    #region FACTURACION ELECTRONICA
        //                    if (sender == "send_enc")
        //                    {
        //                        resultado = true;
        //                        return resultado;
        //                    }
        //                    #endregion
        //                    break;
        //                case 15:
        //                    #region COBRO UNICAMENTE A CLIENTES LOCALES
        //                    if (sender == "btn_enviar_cliente")
        //                    {
        //                        double cliID = 0;
        //                        cliID = double.Parse(tbCliCod.Text.Trim().ToString());
        //                        RE_GenericBean Cliente_Bean = DB.getDataClient(cliID);
        //                        if (Cliente_Bean.strC7 == user.pais.ISO)
        //                        {
        //                            resultado = false;
        //                        }
        //                        else
        //                        {
        //                            WebMsgBox.Show("No se puede emitir Cobro a clientes Internacionales");
        //                            bt_Enviar.Enabled = false;
        //                            resultado = true;
        //                        }
        //                        return resultado;
        //                    }
        //                    #endregion
        //                    break;
        //                case 16:
        //                    #region Validar Nit
        //                    if (sender == "send_nit")
        //                    {
        //                        resultado = DB.ValidarNIT(tb_nit.Text, user.pais.ISO);
        //                        if (resultado == false)
        //                        {
        //                            WebMsgBox.Show("Nit invalido, porfavor solicite la correcion en el Catalogo de Clientes");
        //                            bt_Enviar.Enabled = false;
        //                            dgw.Enabled = false;
        //                            resultado = true;
        //                        }
        //                        else
        //                        {
        //                            resultado = false;
        //                        }
        //                        return resultado;
        //                    }
        //                    #endregion
        //                    break;
        //            }
        //        }
        //    }
        //    else
        //    {
        //        //NO TIENE NINGUNA RESTRICCION ASIGNADA
        //        resultado = false;
        //        return resultado;
        //    }
        //}
        //else
        //{
        //    //PAIS SIN RESTRICCIONES
        //    resultado = false;
        //    return resultado;
        //}
        //return resultado;
        #endregion
    }
    protected void Determinar_Tipo_Serie(int sucID, string serie)
    {
        if (serie == "Seleccione...")
        {
            return;
        }
        if ((user.PaisID == 1) || (user.PaisID == 15) || (user.PaisID == 6) || (user.PaisID == 25) || (user.PaisID == 5) || (user.PaisID == 21))
        {
            int Doc_ID = DB.getDocumentoID(sucID, serie, 3, user);
            lbl_serie_id.Text = Doc_ID.ToString();
            RE_GenericBean Bean_Serie = (RE_GenericBean)DB.getFactura(Doc_ID, 3);
            if (Bean_Serie == null)
            {
                WebMsgBox.Show("Existio un error tratando de determinar el Tipo de Documento, por lo cual no fue transmitido");
                return;
            }
            else
            {
                lbl_tipo_serie.Text = Bean_Serie.intC14.ToString();
                lbl_tipo_serie_caption.Font.Bold = true;
                lbl_correo_documento_electronico.Font.Bold = true;
                if ((user.PaisID == 1) || (user.PaisID == 15) || (user.PaisID == 5) || (user.PaisID == 21))
                {
                    pnl_documento_electronico.Visible = true;
                    if (lbl_tipo_serie.Text == "0")
                    {
                        lbl_tipo_serie_caption.Text = "Serie Estandard";
                    }
                    else if (lbl_tipo_serie.Text == "1")
                    {
                        lbl_tipo_serie_caption.Text = "Serie Electronica";
                    }
                    else if (lbl_tipo_serie.Text == "2")
                    {
                        lbl_tipo_serie_caption.Text = "Serie en Copia";
                    }
                }
            }
        }
    }

    protected void btn_ver_pdf_Click(object sender, EventArgs e)
    {
        if (!DB.DownloadFEL(Request.QueryString["ncID"].ToString().Trim(), "3", "PDF", Response, lbl_internal_reference.Text, user.PaisID, true, true)) 
            DB.OpenGFACEpdf(lbl_internal_reference.Text, Response);
    }
    protected void dgw_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.Cells.Count > 0)
        {
            e.Row.Cells[10].Visible = false;
            e.Row.Cells[11].Visible = false;

        }
    }
}
