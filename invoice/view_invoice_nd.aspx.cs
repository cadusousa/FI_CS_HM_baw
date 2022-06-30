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

public partial class invoice_view_invoice_nd : System.Web.UI.Page
{

    int factID = 0;
    UsuarioBean user = null;

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
            ////CAMBIAR PARA MANEJAR FISCAL USD Y FISCAL LOCAL
            //arr = null;
            //if (user.SucursalID == 15)
            //{
            //    arr = (ArrayList)DB.getSerieFactCreditos(user.SucursalID, 4, user, 0);//1 porque es el tipo de documento para facturacion
            //}
            //else
            //{
            //    arr = (ArrayList)DB.getSerieFactbySuc(user.SucursalID, 4, user, 0);//1 porque es el tipo de documento para facturacion
            //}
            //foreach (string valor in arr)
            //{
            //    item = new ListItem(valor, valor);
            //    lb_serie_factura.Items.Add(item);
            //}

            //arr = null;
            //arr = (ArrayList)DB.getSerieFactbySuc(user.SucursalID, 3, user, 1);//1 porque es el tipo de documento para facturacion
            //foreach (string valor in arr)
            //{
            //    item = new ListItem(valor, valor);
            //    lb_serieNC.Items.Add(item);
            //}
            //if (lb_serieNC.Items.Count == 0)
            //{

            //    WebMsgBox.Show("No hay Serie definida para esta sucursal");
            //    bt_Enviar.Enabled = false;
            //    return;
            //}
            arr = null;
            arr = (ArrayList)DB.Get_Series_By_Monedas_Conta(4, user, 1);
            item = new ListItem("Seleccione...", "0");
            lb_serie_factura.Items.Clear();
            lb_serie_factura.Items.Add(item);
            foreach (RE_GenericBean Bean_Serie in arr)
            {
                item = new ListItem(Bean_Serie.strC1, Bean_Serie.strC2);
                lb_serie_factura.Items.Add(item);
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
    protected void TextBox1_TextChanged(object sender, EventArgs e)
    {
        GridViewRow row;
        double descuento = 0, subtotal = 0, total = 0, impuesto = 0, descuento_subtotal = 0, descuento_impuesto = 0;
        double porc_impuesto = double.Parse(user.pais.Impuesto.ToString());
        int tipo_contabilidad = int.Parse(Session["Contabilidad"].ToString());
        TextBox t1, t2;

        for (int i = 0; i < dgw.Rows.Count; i++)
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
                    if ((rub.rubroImpuesto > 0) && (descuento > 0) && (int.Parse(LimitDays.Text) <= user.pais.diasimpuesto))
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
        impuesto = Math.Round(impuesto, 2);
        subtotal = Math.Round(subtotal, 2);
        total = Math.Round(total, 2);

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
            if (Request.QueryString["ndID"] != null && !Request.QueryString["ndID"].ToString().Equals(""))
            {
                factID = int.Parse(Request.QueryString["ndID"].ToString().Trim());
                //Obtengo los rubros de la factura
                RE_GenericBean factdata = (RE_GenericBean)DB.getNotaDebitoData(factID);
                #region Cargar Serie de Factura
                UsuarioBean Usuario_Temporal = new UsuarioBean();
                Usuario_Temporal.SucursalID = factdata.intC5;
                Usuario_Temporal.contaID = factdata.intC10;
                Usuario_Temporal.PaisID = factdata.intC2;
                Usuario_Temporal.Operacion = 1;
                arr = null;
                arr = (ArrayList)DB.Get_Series_By_Monedas_Conta(4, Usuario_Temporal, 0);
                item = new ListItem("Seleccione...", "0");
                lb_serie_factura.Items.Clear();
                lb_serie_factura.Items.Add(item);
                foreach (RE_GenericBean Bean_Serie in arr)
                {
                    item = new ListItem(Bean_Serie.strC1, Bean_Serie.strC2);
                    lb_serie_factura.Items.Add(item);
                }
                #endregion
                int Doc_ID = DB.getDocumentoID(factdata.intC5, factdata.strC28, 4, user);
                lb_serie_factura.SelectedValue = Doc_ID.ToString();//Serie
                tbCliCod.Text = factdata.douC1.ToString(); //YA
                tb_nombre.Text = factdata.strC2;// YA
                tb_nit.Text = factdata.strC1; //YA
                tb_razon.Text = factdata.strC34;//YA
                lb_imp_exp.SelectedValue = factdata.intC9.ToString();//YA------------
                lb_moneda.SelectedValue = factdata.intC4.ToString();//YA
                lb_tipocobro.Text = factdata.intC9.ToString();//YA---------------
                tb_direccion.Text = factdata.strC6;//YA
                tb_observaciones.Text = factdata.strC4;//YA
                tb_referencia.Text = factdata.strC11;//YA
                tb_naviera.Text = factdata.strC13;//YA
                tb_vapor.Text = factdata.strC14;//YA
                tb_contenedor.Text = factdata.strC9;//YA

                tb_hbl.Text = factdata.strC7;//YA
                tb_mbl.Text = factdata.strC8;//YA
                tb_routing.Text = factdata.strC12;//YA
                tb_contenedor.Text = factdata.strC9;//YA

                tb_shipper.Text = factdata.strC15;//YA
                tb_orden.Text = factdata.strC16;//YA
                tb_consignee.Text = factdata.strC17;//YA
                tb_comodity.Text = factdata.strC18;//YA

                tb_paquetes1.Text = factdata.strC19;//YA
                tb_paquetes2.Text = factdata.strC32;//YA

                tb_peso.Text = factdata.strC20;//YA
                tb_vol.Text = factdata.strC21;//YA
                tb_dua_ingreso.Text = factdata.strC22;//YA
                tb_dua_salida.Text = factdata.strC23;//YA
                tb_vendedor1.Text = factdata.strC24;//YA
                tb_vendedor2.Text = factdata.strC25;//YA
                tb_fecha.Text = factdata.strC3;//YA
                tb_doc.Text = "";
                if (Usuario_Temporal.PaisID == 5 || Usuario_Temporal.PaisID == 21)
                {
                    //tb_fecha.Text = factdata.strC58;//CR factura referencia fecha 2019-07-17
                    //tb_doc.Text = factdata.strC59;//CR factura referencia esignature 2019-07-17
                    tb_doc.Text = factdata.strC59 + "|" + factdata.strC58 + "|" + factdata.intC12.ToString() + "|" + factdata.strC57 + "|" + factdata.intC13.ToString();
                    // esignature(20,21) | fecha fac | id fac | serie fac | corr fac                    
                }                

                lb_serie_factura.SelectedValue = factdata.strC28;//YA
                lb_facid.Text = factdata.intC6.ToString();//YA
                lb_contribuyente.SelectedValue = factdata.strC55;
                dgw.DataSource = (DataTable)DB.getRubByDoc_xNC(factID, 4, 3);
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
                FacturaBean Estado_Factura = (FacturaBean)DB.getEstadoNotaDebito(factID);
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
                #endregion

                //Calculo de Dias del Documento para acreditar o no el IVA
                DateTime ahora = DateTime.Now;
                string[] f = tb_fecha.Text.Split('/');
                DateTime fecha = ahora;

                //---------------------------------dg-------------------------------

                TimeSpan RangeTax = ahora.Subtract(fecha);
                LimitDays.Text = RangeTax.Days.ToString();
            }
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

            RE_GenericBean rgb = new RE_GenericBean();
            rgb.Fecha_Hora = lb_fecha_hora.Text;
            rgb.intC1 = int.Parse(lb_tipo_transaccion.SelectedValue);//tipo transaccion
            rgb.intC2 = int.Parse(lb_moneda.SelectedValue);//tipo moneda
            rgb.intC3 = int.Parse(Session["Contabilidad"].ToString());//tipo de contabilidad
            rgb.intC4 = int.Parse(Request.QueryString["ndID"].ToString().Trim());
            rgb.intC5 = 3;//tipo de transaccion
            rgb.intC6 = 3;//tipo de persona
            rgb.intC7 = int.Parse(tbCliCod.Text.Trim());//Cliente ID
            rgb.decC1 = decimal.Parse(tb_total.Text.Trim());//monto a aplicar
            rgb.strC1 = tb_observaciones.Text;//observaciones
            rgb.strC2 = lb_serie_factura.SelectedItem.Text;//Serie Nota de Debito
            rgb.strC3 = lb_facid.Text;//Correlativo Nota de Debito

            try //deberia funcionar solo para CR
            {
                string[] tbdoc = tb_doc.Text.Split('|');
                rgb.Factura_Ref_ID = int.Parse(tbdoc[2]);           //2019-07-17 aqui deben ir los datos de la factura referenciada en ND
                rgb.Factura_Ref_Correlativo = int.Parse(tbdoc[4]);  //2019-07-17
                rgb.Factura_Ref_Serie = tbdoc[3];                   //2019-07-17
                rgb.Factura_Ref_Fecha = tbdoc[1];                   //tb_fecha.Text;    //fecha factura 2019-07-09
                rgb.Factura_Ref_Doc = tbdoc[0];                     //esignature 2019-07-11
            }
            catch (Exception var2) { 
                
            }

            rgb.strC20 = lb_serieNC.SelectedItem.Text;
            rgb.strC35 = tb_nit.Text;//nit
            rgb.strC9 = tb_hbl.Text;
            rgb.strC10 = tb_mbl.Text;
            rgb.strC11 = tb_contenedor.Text;
            rgb.strC12 = tb_routing.Text;
            rgb.strC13 = tb_poliza.Text.Trim();//Poliza Aduanal
            rgb.IP_Address = Request.ServerVariables["REMOTE_ADDR"].ToString();
            rgb.Factura_Electronica = int.Parse(lbl_tipo_serie.Text);
            if (tb_iva.Text.Equals(""))
            {
                rgb.decC3 = 0;
            }
            else
            {
                rgb.decC3 = decimal.Parse(tb_iva.Text);
            }

            rgb.Nombre_Cliente = tb_nombre.Text.Trim();//Nombre del Cliente
            rgb.Direccion = tb_direccion.Text.Trim();//Direccion del Cliente

            if (lb_moneda.SelectedValue.Equals("8"))
            {
                rgb.decC2 = Math.Round((rgb.decC1 * user.pais.TipoCambio), 2);
            }
            else
            {
                rgb.decC2 = Math.Round((rgb.decC1 / user.pais.TipoCambio), 2);
            }

            #region Validaciones Facturacion Electronica Costa Rica
            if (((user.PaisID == 5) || (user.PaisID == 21)) && (lbl_tipo_serie.Text == "1"))
            {
                if (drp_tipopersona.SelectedValue == "10")
                {
                    bt_Enviar.Enabled = false;
                    bt_nota_credito_virtual.Enabled = false;
                    WebMsgBox.Show("No es posible emitir Nota de Credito Intercompany a traves de una Serie Electronica, por favor pregunte al Contador de Costa Rica la forma correcta de proceder.");
                    return;
                }
                if (drp_tipo_identificacion_cliente.SelectedValue == "0")
                {
                    WebMsgBox.Show("La Nota de Credito no fue guardada, ni procesada por el Ministerio de Hacienda porque el Cliente.: " + tb_nombre.Text + " con ID.: " + tbCliCod.Text + " no tiene Asignado Tipo de Identificacion Tributaria en el Catalogo de Clientes, por favor contacte al personal de Contabilidad para que actualicen y asigen el Tipo en el Catalago de Clientes, posterior a ello genere nuevamente la Nota de Credito.");
                    return;
                }
                if (lbl_correo_documento_electronico.Text == "")
                {
                    WebMsgBox.Show("El Cliente.: " + tb_nombre.Text + " con ID.: " + tbCliCod.Text + " no tiene asignado correo electronico en el Catalogo de Clientes para recibir Notas de Credito Electronicas, por favor asigne una para que mas adelante las pueda recibir.");
                }
            }
            #endregion

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



            int matOpID = DB.getMatrizOperacionID(5, mat.monID, user.PaisID, mat.contaID);
            ArrayList ctas_cargo = (ArrayList)DB.getMatrizConfiguracion(matOpID);

            #region Definir Estado de la Nota de Debito
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
                rubro.rubroName = lb2.Text;
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
            }
            rgb.intC10 = 4; //nota debito
            DateTime fecha = DateTime.Parse(tb_fecha.Text);
            if (fecha.CompareTo(DateTime.Now) < 0) rgb.boolC2 = true;
            string Check_Existencia = DB.CheckExistDoc(rgb.Fecha_Hora, 3);
            if (Check_Existencia == "0")
            {
                ArrayList result = DB.InsertNotaCredito(rgb, user, rgb.intC3, ctas_cargo);
                if (result == null || result.Count == 0 || int.Parse(result[1].ToString()) == 0 || int.Parse(result[0].ToString()) == -100)
                {
                    #region Facturacion Electronica de Costa Rica
                    if (((user.PaisID == 5) || (user.PaisID == 21)) && (lbl_tipo_serie.Text == "1"))
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
                        //}
                    }
                    #endregion
                    WebMsgBox.Show("La Nota de Credito fue grabada exitosamente con el numero " + lb_serieNC.SelectedItem.Text + " - " + result[0].ToString());
                    tb_corrNC.Text = result[0].ToString();
                    bt_Enviar.Enabled = false;
                    bt_Imprimir.Enabled = true;
                    bt_nota_credito_virtual.Enabled = true;
                    factID = int.Parse(Request.QueryString["ndID"].ToString().Trim());
                    FacturaBean Estado_Factura = (FacturaBean)DB.getEstadoND(factID);
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
    protected void bt_Imprimir_Click(object sender, EventArgs e)
    {
        int ncID = DB.getCorrelativoDoc(user.SucursalID, 3, lb_serieNC.SelectedItem.Text, tb_corrNC.Text);
        String csname1 = "PopupScript";
        Type cstype = this.GetType();
        ClientScriptManager cs2 = Page.ClientScript;
        string script = "window.open('printersettings.aspx?fac_id=" + ncID.ToString() + "&tipo=3','Imprimir','toolbar=0, status=1, resizable=1, scrollbars=1, top=0, left=0, width=' + (window.screen.width -10) + ', height=' + (window.screen.height -75))";
        #region Seteo de Parametros de Impresion
        user = (UsuarioBean)Session["usuario"];
        user.ImpresionBean.Operacion = "1";
        user.ImpresionBean.Tipo_Documento = "3";
        user.ImpresionBean.Id = ncID.ToString();
        user.ImpresionBean.Impreso = false;
        Session["usuario"] = user;
        #endregion
        if (!cs2.IsStartupScriptRegistered(cstype, csname1))
        {
            cs2.RegisterStartupScript(cstype, csname1, script, true);
        }
        bt_Imprimir.Enabled = false;
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
                    int Moneda_Serie_Nota_Debito = 0;
                    int Moneda_Serie_Nota_Credito = 0;
                    Moneda_Serie_Nota_Debito = DB.Get_Serie_Moneda_By_ID(int.Parse(lb_serie_factura.SelectedValue));
                    Moneda_Serie_Nota_Credito = DB.Get_Serie_Moneda_By_ID(int.Parse(lb_serieNC.SelectedValue));
                    if (Moneda_Serie_Nota_Debito != Moneda_Serie_Nota_Credito)
                    {
                        lb_serieNC.SelectedValue = "0";
                        WebMsgBox.Show("Por Favor seleccione una Serie con la misma Moneda de la Nota de Debito");
                        return;
                    }
                    lb_moneda.SelectedValue = Moneda_Serie_Nota_Debito.ToString();

                    if ((user.PaisID == 5) || (user.PaisID == 21) || (user.PaisID == 1) || (user.PaisID == 15))
                    {
                        RE_GenericBean Data_Serie_ND = (RE_GenericBean)DB.getFactura(int.Parse(lb_serie_factura.SelectedValue), 4);
                        RE_GenericBean Data_Serie_NC = (RE_GenericBean)DB.getFactura(int.Parse(lb_serieNC.SelectedValue), 3);
                        if (Data_Serie_NC.intC14 == 1)
                        {
                            if (Data_Serie_ND.intC14 != 1)
                            {
                                bt_Enviar.Enabled = false;
                                bt_Imprimir.Enabled = false;
                                bt_nota_credito_virtual.Enabled = false;
                                WebMsgBox.Show("No se puede aplicar Nota de Credito Electronica a una Nota de Debito que no es Electronica.");
                                return;
                            }
                            else
                            {
                                bt_Enviar.Enabled = true;
                            }
                        }
                        if (Data_Serie_ND.intC14 == 1)
                        {
                            if (Data_Serie_NC.intC14 != 1)
                            {
                                bt_Enviar.Enabled = false;
                                bt_Imprimir.Enabled = false;
                                bt_nota_credito_virtual.Enabled = false;
                                WebMsgBox.Show("No se puede aplicar Nota de Credito normal a una Nota de Debito Electronica.");
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
            }
            else
            {
                t1.Text = "0.00";
            }
        }
        TextBox1_TextChanged(sender, e);
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
            ClientScriptManager cs2 = Page.ClientScript;
            string script = "window.open('../invoice/template_invoice.aspx?id=" + lbl_NC_ID.Text + "&transaccion=3','Imprimir','menubar=1,resizable=1,scrollbars=1,toolbar=1,width=750');";
            if (!cs2.IsStartupScriptRegistered(cstype, csname1))
            {
                cs2.RegisterStartupScript(cstype, csname1, script, true);
            }
        }
    }


    protected void tb_corrNC_TextChanged(object sender, EventArgs e)
    {

    }
    protected void dgw_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.Cells.Count > 0)
        {
            e.Row.Cells[10].Visible = false;
            e.Row.Cells[11].Visible = false;

        }
    }
    protected void Determinar_Tipo_Serie(int sucID, string serie)
    {
        if (serie == "Seleccione...")
        {
            return;
        }
        if ((user.PaisID == 1) || (user.PaisID == 6) || (user.PaisID == 25) || (user.PaisID == 5) || (user.PaisID == 21))
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
                if ((user.PaisID == 1) || (user.PaisID == 5) || (user.PaisID == 21))
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
}