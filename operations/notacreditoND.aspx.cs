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

public partial class operations_notacreditoND : System.Web.UI.Page
{
    UsuarioBean user;
    protected void Page_Load(object sender, EventArgs e)
    {
        lb_transaccion.Enabled = false;
        if (!IsPostBack)
        {
            lb_fecha_hora.Text = DB.getDateTimeNow();
        }
        if (Session["usuario"] == null)
        {
            Response.Redirect("../default.aspx");
        }

        user = (UsuarioBean)Session["usuario"];
        if (!user.Aplicaciones.Contains("6"))
            Response.Redirect("index.aspx");
        int permiso = int.Parse(user.Aplicaciones["6"].ToString());
        if (!((permiso & 8192) == 8192))
            Response.Redirect("index.aspx");

        if (!IsPostBack)
        {
            int tipo_contabilidad = int.Parse(Session["Contabilidad"].ToString());
            int moneda_inicial = 0;
            obtengo_listas(tipo_contabilidad);
            moneda_inicial = DB.Get_Moneda_Inicial_By_Empresa(user, 31);
            if (tipo_contabilidad == 2)
            {
                lb_moneda.SelectedValue = moneda_inicial.ToString();
                lb_moneda.Enabled = false;
            }
            else
            {
                lb_moneda.SelectedValue = moneda_inicial.ToString();
                lb_moneda.Enabled = false;

            }
        }

        #region Bloqueos Facturacion Electronica Costa Rica
        if ((user.PaisID == 5) || (user.PaisID == 21))
        {
            if (user.SucursalID == 117)
            {
                bt_imprimir.Visible = false;
                pnl_tipo_identificacion_cliente.Visible = true;
            }
        }
        #endregion

        if ((lb_serie_factura.Items.Count > 0) && (lb_serie_factura.SelectedValue != "0"))
        {
            Determinar_Tipo_Serie(user.SucursalID, lb_serie_factura.SelectedItem.Text);
        }

    }
    private void obtengo_listas(int tipo_contabilidad)
    {
        ListItem item = null;
        ArrayList arr = null;
        if (!Page.IsPostBack)
        {
            arr = (ArrayList)DB.getOpcionesMatriz("tbl_tipo_transaccion where ttt_template='notacreditond.aspx' and ttr_opcion=1 ");
            lb_transaccion.Items.Clear();
            item = new ListItem("Seleccione...", "0");
            lb_transaccion.Items.Add(item);
            foreach (RE_GenericBean rgb in arr)
            {
                item = new ListItem(rgb.strC1, rgb.intC1.ToString());
                lb_transaccion.Items.Add(item);
            }
            arr = null;
            arr = (ArrayList)DB.getMonedasbyPais(user.PaisID, user.contaID);
            foreach (RE_GenericBean rgb in arr)
            {
                item = new ListItem(rgb.strC1, rgb.intC1.ToString());
                lb_moneda.Items.Add(item);
            }
            //arr = null;
            //arr = (ArrayList)DB.getSerieFactbySuc(user.SucursalID, 31, user, 2);//
            //foreach (string valor in arr)
            //{
            //    item = new ListItem(valor, valor);
            //    lb_serie_factura.Items.Add(item);
            //}
            arr = (ArrayList)DB.Get_Series_By_Monedas_Conta(31, user, 1);
            item = new ListItem("Seleccione...", "0");
            lb_serie_factura.Items.Clear();
            lb_serie_factura.Items.Add(item);
            foreach (RE_GenericBean Bean_Serie in arr)
            {
                item = new ListItem(Bean_Serie.strC1, Bean_Serie.strC2);
                lb_serie_factura.Items.Add(item);
            }
            arr = null;
            arr = (ArrayList)DB.getOpcionesMatriz("tbl_tipo_contribuyente");
            foreach (RE_GenericBean rgb in arr)
            {
                item = new ListItem(rgb.strC1, rgb.intC1.ToString());
                lb_contribuyente.Items.Add(item);
            }
            arr = null;
            arr = (ArrayList)DB.getOpcionesMatriz("tbl_tipo_imp_exp");
            foreach (RE_GenericBean rgb in arr)
            {
                item = new ListItem(rgb.strC1, rgb.intC1.ToString());
                lb_imp_exp.Items.Add(item);
            }


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

    protected void Page_PreRender(object sender, EventArgs e)
    {
        int ndID = 0;
        if (!Page.IsPostBack)
        {
            if (Request.QueryString["ndID"] != null && !Request.QueryString["ndID"].ToString().Equals(""))
            {
                ndID = int.Parse(Request.QueryString["ndID"].ToString());
                RE_GenericBean ndData = (RE_GenericBean)DB.getNotaDebitoData(ndID);
                lb_fechacreacion.Text = ndData.strC3;//fecha de creacion
                lb_proveedorID.Text = ndData.douC1.ToString();
                lb_proveedorCCHID.Text = ndData.intC4.ToString();
                lb_provID.Text = ndData.douC1.ToString();
                tb_provID.Text = ndID.ToString();
                tb_provID.Enabled = false;
                tb_nota_debito.Text = ndData.strC28 + " - " + ndData.intC6.ToString();
                tb_nd_serie.Text = ndData.strC28;
                tb_nd_correlativo.Text = ndData.intC6.ToString();
                lb_tipopersona.SelectedValue = ndData.intC7.ToString();
                tb_hbl.Text = ndData.strC7;
                tb_mbl.Text = ndData.strC8;
                tb_routing.Text = ndData.strC12;
                tb_contenedor.Text = ndData.strC9;
                tb_direccion.Text = ndData.strC6;
                int proveedorCCHID = int.Parse(lb_proveedorCCHID.Text);
                RE_GenericBean proveedordata = null;
                int proveedorID = int.Parse(lb_proveedorID.Text);
                if (ndData.intC7 == 2)
                { // es un agente
                    lb_transaccion.SelectedValue = "99";
                    proveedordata = (RE_GenericBean)DB.getAgenteData(proveedorID, "");// obtengo los datos del agente
                    lb_contribuyente.SelectedValue = DB.getProveedorRegimen(ndData.intC11, proveedorID.ToString()).ToString();
                    tb_nit.Text = "";
                    if (proveedordata != null)
                    {
                        tb_nombre.Text = proveedordata.strC1;
                    }
                }
                else if (ndData.intC7 == 4)
                {// es un proveedor 
                    lb_transaccion.SelectedValue = "101";
                    proveedordata = (RE_GenericBean)DB.getProveedorData(proveedorID, ""); //obtengo los datos del proveedor
                    lb_contribuyente.SelectedValue = DB.getProveedorRegimen(ndData.intC7, proveedorID.ToString()).ToString();
                    tb_nit.Text = proveedordata.strC1;
                    tb_nombre.Text = proveedordata.strC2;
                }
                else if (ndData.intC7 == 5)
                {// naviera
                    lb_transaccion.SelectedValue = "100";
                    proveedordata = (RE_GenericBean)DB.getNavieraData(proveedorID); //obtengo los datos del proveedor
                    lb_contribuyente.SelectedValue = DB.getProveedorRegimen(ndData.intC7, proveedorID.ToString()).ToString();
                    tb_nit.Text = "";// proveedordata.strC1;
                    tb_nombre.Text = proveedordata.strC1;
                }
                else if (ndData.intC7 == 6)
                {// Linea aerea
                    lb_transaccion.SelectedValue = "102";
                    proveedordata = (RE_GenericBean)DB.getCarriersData(proveedorID); //obtengo los datos del proveedor
                    lb_contribuyente.SelectedValue = DB.getProveedorRegimen(ndData.intC7, proveedorID.ToString()).ToString();
                    tb_nit.Text = "";// proveedordata.strC1;
                    tb_nombre.Text = proveedordata.strC1;
                }
                else if (ndData.intC7 == 10)
                {// Intercompany
                    
                    string where = " and id_intercompany=" + proveedorID;
                    lb_transaccion.SelectedValue = "113";
                    ArrayList intecompanydata = (ArrayList)DB.Get_Intercompanys(where); //obtengo los datos del proveedor
                    foreach (RE_GenericBean rgb in intecompanydata)
                    {
                        tb_nombre.Text = rgb.strC1;
                        tb_nit.Text = rgb.strC2;
                    }
                    lb_contribuyente.SelectedValue = DB.getProveedorRegimen(ndData.intC7, proveedorID.ToString()).ToString();
                    drp_tipo_identificacion_cliente.SelectedValue = "3";//Tipo de Identificacion del Extranjero
                    
                }

                DataTable rubdt = (DataTable)DB.getRubByDoc_xNC(ndID, 4, 31);//4 = nota de debito, 12 de NC operaciones
                gv_detalle.DataSource = rubdt;
                gv_detalle.DataBind();
                
                lb_moneda.SelectedValue = ndData.intC4.ToString();
                FacturaBean Estado_Factura = (FacturaBean)DB.getEstadoNotaDebito(ndID);
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
                    gv_detalle.Enabled = false;
                    bt_Enviar.Enabled = false;
                }

                //Calculo de Dias del Documento para acreditar o no el IVA
                DateTime ahora = DateTime.Now;
                string[] f = lb_fechacreacion.Text.Split('/');
                DateTime fecha = ahora;

                TimeSpan RangeTax = ahora.Subtract(fecha);
                LimitDays.Text = RangeTax.Days.ToString();
            }
        }
    }

    protected void bt_Enviar_Click(object sender, EventArgs e)
    {

        int transID = int.Parse(lb_transaccion.SelectedValue);
        if (transID <= 0)
        {
            WebMsgBox.Show("Debe seleccionar una Transaccion");
            return;
        }
        if (lb_serie_factura.Items.Count <= 0)
        {
            WebMsgBox.Show("No se puede Guardar una Nota de Credito sin serie");
            return;
        }
        if (lb_serie_factura.SelectedValue == "0")
        {
            WebMsgBox.Show("Por favor seleccione la Serie a utilizar");
            return;
        }
        int contribuyente = int.Parse(lb_contribuyente.SelectedValue);
        int moneda = int.Parse(lb_moneda.SelectedValue);//moneda
        int tipopersona = int.Parse(lb_tipopersona.SelectedValue);//tipo de persona
        int imp_exp = int.Parse(lb_imp_exp.SelectedValue);
        int tipo_cobro = 1;//prepaid, collect
        int servicio = 0; //fcl, lcl, etc
        if (imp_exp == 1) { tipo_cobro = 1; } else { tipo_cobro = 2; } //si es 1=importacion cobro 1=collect,si es 2=exportacion cobro 2=prepaid

        int tipo_contabilidad = int.Parse(Session["Contabilidad"].ToString());//fiscal o financiera
        if (tipo_contabilidad == 1) { transID = 1; } else if (tipo_contabilidad == 2) { transID = 7; }

        decimal total = 0;
        total = decimal.Parse(tb_total.Text);

        if (total == 0)
        {
            WebMsgBox.Show("Debe realizar un descuento en algun rubro.");
            return;
        }

        user = (UsuarioBean)Session["usuario"];
        ////Pendiente el tema de si es menor de 2 meses
        RE_GenericBean rgb = new RE_GenericBean();
        rgb.Fecha_Hora = lb_fecha_hora.Text;
        rgb.intC1 = int.Parse(lb_transaccion.SelectedValue);//tipo transaccion
        rgb.intC2 = moneda;//tipo moneda
        rgb.intC3 = tipo_contabilidad;//tipo de contabilidad
        rgb.intC4 = int.Parse(tb_provID.Text);//ND ID
        rgb.intC5 = 31;//sys tipo de transaccion NC 
        rgb.intC6 = tipopersona;//tipo de persona
        rgb.intC7 = int.Parse(lb_proveedorID.Text);//Proveedor ID
        rgb.intC10 = 4; // tfr_tfa_sysref_id para la tabla tbl_factura_abono y no es 26(ND proveedor) porque en el libro diario se guarda como tdi_ttr_id=4
        rgb.decC1 = decimal.Parse(tb_total.Text.Trim());//monto a aplicar
        rgb.strC1 = tb_observaciones.Text;//observaciones
        rgb.strC2 = lb_serie_factura.SelectedItem.Text;
        //rgb.strC3 = tb_referencia.Text; //Referencia
        rgb.strC3 = tb_nd_correlativo.Text; //Correlativo Nota de Debito
        rgb.decC3 = decimal.Parse(tb_iva.Text.Trim());//monto a aplicar
        rgb.strC9 = tb_hbl.Text;
        rgb.strC10 = tb_mbl.Text;
        rgb.strC11 = tb_contenedor.Text;
        rgb.strC12 = tb_routing.Text;
        //rgb.Nombre_Cliente = tb_nombre.Text;//Nombre del Cliente
        //rgb.strC14 = tb_nit.Text;
        rgb.IP_Address = Request.ServerVariables["REMOTE_ADDR"].ToString();
        rgb.Factura_Electronica = int.Parse(lbl_tipo_serie.Text);
        rgb.strC36 = lbl_correo_documento_electronico.Text;
        rgb.strC37 = tb_referencia_correo.Text;
        rgb.strC38 = tb_referencia.Text; //Referencia
        rgb.Nombre_Cliente = tb_nombre.Text;//Nombre del Cliente
        rgb.strC35 = tb_nit.Text.Trim();//nit
        rgb.Direccion = tb_direccion.Text.Trim();//Direccion

        if (lb_moneda.SelectedValue.Equals("8"))
        {
            rgb.decC2 = Math.Round((rgb.decC1 * user.pais.TipoCambio), 2);
        }
        else
        {
            rgb.decC2 = Math.Round((rgb.decC1 / user.pais.TipoCambio), 2);
        }
        MatOpBean mat = new MatOpBean();
        mat.paiID = user.PaisID;
        mat.monID = int.Parse(lb_moneda.SelectedValue);
        mat.contaID = rgb.intC3;
        mat.impexpID = int.Parse(lb_imp_exp.SelectedValue);
        mat.cobroID = tipo_cobro;
        mat.contriID = int.Parse(lb_contribuyente.SelectedValue);

        int matOpID = DB.getMatrizOperacionID(int.Parse(lb_transaccion.Text), mat.monID, user.PaisID, mat.contaID);
        ArrayList ctas_cargo = (ArrayList)DB.getMatrizConfiguracion(matOpID);


        #region Definir Estado de Nota de Debito
        //Nota: El estado de la Nota de Debito siempre se queda en 1=Activo porque la Nota de Debito se matara con la Nota de Credito en el SOA
        int estadoTEMPORAL_ND = 0;
        if ((chk_anulacion.Visible == true) && (chk_anulacion.Checked == true))
        {
            rgb.Estado = 1;
            estadoTEMPORAL_ND = 3;
            rgb.Estado = estadoTEMPORAL_ND;
        }
        else if (decimal.Parse(tb_total.Text) == decimal.Parse(lbl_saldo.Text))
        {
            rgb.Estado = 1;
            estadoTEMPORAL_ND = 4;
            rgb.Estado = estadoTEMPORAL_ND;
        }
        else if (decimal.Parse(tb_total.Text) < decimal.Parse(lbl_saldo.Text))
        {
            rgb.Estado = 1;
            estadoTEMPORAL_ND = 2;
            rgb.Estado = estadoTEMPORAL_ND;
        }
        #endregion
        #region Validacion Anulacion de Intercompanys
        if (rgb.intC6 == 10)
        {
            #region Validar Transaccion Encadenada
            int tranID = 4;
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

                //WebMsgBox.Show("Transaccion encadenada (NOTA DEBITO " + Arr_Doc_Factura[1].ToString() + "-" + Arr_Doc_Factura[2].ToString() + "), no es posible aplicar Nota de Credito porque afectaria el Monto Total por Pagar a la Aseguradora, esta transaccion solo puede ser anulada al anular la Transaccion Padre.: PROVISION A LA ASEGURADORA Serie.: " + Arr_Doc_Padre[1].ToString() + " y Correlativo.: " + Arr_Doc_Padre[2].ToString() + ", al anular Transaccion Padre tambien se anulara Transaccion Hija.: PROVISION INTERCOMPANY Serie.: " + Arr_Doc_Hijo[1].ToString() + " y Correlativo.: " + Arr_Doc_Hijo[2].ToString() + " en la Empresa.: " + Empresa_Hija.Nombre_Sistema + " .");
                WebMsgBox.Show("Transaccion encadenada (NOTA DEBITO " + Arr_Doc_Factura[1].ToString() + "-" + Arr_Doc_Factura[2].ToString() + "), no es posible aplicar Nota de Credito porque afectaria el Monto Total por Pagar a la Aseguradora, esta transaccion solo puede ser anulada al anular la Transaccion Padre.: PROVISION A LA ASEGURADORA Serie.: " + Arr_Doc_Padre[1].ToString() + " y Correlativo.: " + Arr_Doc_Padre[2].ToString() + ".");
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
                        Arr_Temp = DB.getSerieCorrAndObsByDoc(int.Parse(Bean_Padre.strC2), int.Parse(Bean_Padre.strC3));
                        WebMsgBox.Show("Transaccion encadenada, esta transaccion solo puede ser anulada desde la Empresa.: " + Empresa_Padre.Nombre_Sistema + " al anular la Transaccion Padre.: " + Arr_Temp[0].ToString() + " Serie.: " + Arr_Temp[1].ToString() + " y Correlativo.: " + Arr_Temp[2].ToString() + "");
                        return;
                    }
                    #endregion
                }
                #endregion
                #region Validar Padre
                if (resultado_transaccion_encadenada == "PADRE")
                {
                    if (estadoTEMPORAL_ND == 3)//Si va a anular la Nota de Debito con Nota de Credito se debe anular tambien la Provision Intercompany en Destino
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
                                int resultado_anulacion = DB.Registrar_Anulacion(user, 4, rgb.intC4, "Anulacion a traves de Nota de Credito");
                            }
                            #endregion
                        }
                    }
                }
                #endregion
            }
            #endregion
        }
        #endregion

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
        foreach (GridViewRow row in gv_detalle.Rows)
        {
            lb1.Text = row.Cells[2].Text.Trim();  //rubro id
            lb2.Text = row.Cells[3].Text.Trim();  //rubro nombre
            lb6.Text = row.Cells[4].Text.Trim();
            lb3.Text = row.Cells[4].Text.Trim();  //rubro tipo
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
            rubro.rubtoType = lb3.Text;  
            rubro.rubroTot = double.Parse(t1.Text);
            rubro.rubroImpuesto = double.Parse(t2.Text);
            rubro.rubroSubTot = rubro.rubroTot - rubro.rubroImpuesto;
            rubro.rubroTypeID = Utility.TraducirServiciotoINT(rubro.rubtoType);
            if (lb_moneda.SelectedValue.Equals("8"))
            {
                rubro.rubroEquivalente = Math.Round((rubro.rubroTot * (double)user.pais.TipoCambio), 2);
            }
            else
            {
                rubro.rubroEquivalente = Math.Round((rubro.rubroTot / (double)user.pais.TipoCambio), 2);
            }
            servicio = rubro.rubroTypeID;
            rubro.cta_debe = (ArrayList)DB.getCtaContablebyRubro("debe", (int)rubro.rubroID, user.PaisID, transID, contribuyente, moneda, imp_exp, tipo_cobro, tipo_contabilidad, servicio);
            rubro.cta_haber = (ArrayList)DB.getCtaContablebyRubro("haber", (int)rubro.rubroID, user.PaisID, transID, contribuyente, moneda, imp_exp, tipo_cobro, tipo_contabilidad, servicio);
            if ((rubro.cta_debe == null && rubro.cta_haber == null) || (rubro.cta_debe.Count == 0 && rubro.cta_haber.Count == 0))
            {
                WebMsgBox.Show("El rubro " + rubro.rubroName + " no tiene configurada ninguna cuenta contable para la combinacion de cobro que esta realizando, por favor pongase en contacto con el Contador");
                return;
            }
            if (rgb.arr1 == null) rgb.arr1 = new ArrayList();
            if (rubro.rubroTot > 0)
                rgb.arr1.Add(rubro);
        }

        string Check_Existencia = DB.CheckExistDoc(rgb.Fecha_Hora, 31);
        if (Check_Existencia == "0")
        {
            ArrayList result = DB.InsertNotaCredito_NotaDebito(rgb, user, tipo_contabilidad, ctas_cargo);//4 ya que en la tabla tbl_tipo_persona 4=proveedor

            if (result == null || result.Count == 0 || int.Parse(result[1].ToString()) == 0 || int.Parse(result[0].ToString()) == -100)
            {
                #region Facturacion Electronica de Costa Rica
                if (((user.PaisID == 5) || (user.PaisID == 21)) && (lbl_tipo_serie.Text == "1"))
                {
                    if (result.Count == 4)
                    {
                        ArrayList Arr_Transmision_CR = (ArrayList)result[3];
                        if (Arr_Transmision_CR[0].ToString() == "0")
                        {
                            pnl_transmision_electronica.Visible = true;
                            tb_resultado_transmision.Text = Arr_Transmision_CR[1].ToString();
                            bt_Enviar.Enabled = false;
                            WebMsgBox.Show(Arr_Transmision_CR[1].ToString());
                            return;
                        }
                    }
                }
                #endregion
                WebMsgBox.Show("Existió un problema al tratar de guardar la información, por favor intente de nuevo");
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
                            tb_NCprov.Text = Arr_Transmision_CR[2].ToString();
                            #region Mostrando la Partida contable
                            //Mostrando la Partida contable generada
                            gv_detalle_partida.DataSource = DB.getPolizaDiario(user, double.Parse(result[0].ToString()), 31, 0);
                            gv_detalle_partida.DataBind();
                            gv_detalle_partida.Rows[gv_detalle_partida.Rows.Count - 1].Font.Bold = true;
                            #endregion
                            bt_Enviar.Enabled = false;
                            WebMsgBox.Show(Arr_Transmision_CR[1].ToString());
                            return;
                        }
                    }
                }
                #endregion

                WebMsgBox.Show("La Nota de Credito fue guardada exitosamente con Serie.: " + lb_serie_factura.SelectedItem.Text + " y Correlativo.: " + result[1].ToString());
                tb_NCprov.Text = result[1].ToString();
                bt_Enviar.Enabled = false;
                bt_imprimir.Enabled = true;

                //Mostrando la Partida contable generada
                gv_detalle_partida.DataSource = DB.getPolizaDiario(user, double.Parse(result[0].ToString()), 31, 0);
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
    protected void bt_imprimir_Click(object sender, EventArgs e)
    {
        String csname1 = "PopupScript";
        Type cstype = this.GetType();
        // Get a ClientScriptManager reference from the Page class.
        ClientScriptManager cs2 = Page.ClientScript;
        string script = "window.open('print_notadebito.aspx?fac_id=" + lb_provID.Text + "','Imprimir','menubar=1,resizable=1,scrollbars=1,toolbar=1,width=750');";

        if (!cs2.IsStartupScriptRegistered(cstype, csname1))
        {
            cs2.RegisterStartupScript(cstype, csname1, script, true);
        }

    }

    protected void TextBox1_TextChanged(object sender, EventArgs e)
    {
        GridViewRow row;
        double descuento = 0, subtotal = 0, total = 0, impuesto = 0, descuento_subtotal = 0, descuento_impuesto = 0;
        double porc_impuesto = double.Parse(user.pais.Impuesto.ToString());
        int tipo_contabilidad = int.Parse(Session["Contabilidad"].ToString());
        TextBox t1, t2;

        for (int i = 0; i < gv_detalle.Rows.Count; i++)
        {
            row = gv_detalle.Rows[i];
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
        //if (Math.Round(total,2) == double.Parse(lbl_total_factura.Text.ToString()))
        //{
        //    chk_anulacion.Checked = true;
        //}
        //else
        //{
        //    chk_anulacion.Checked = false;
        //}
    }
    protected void lb_transaccion_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (!lb_transaccion.SelectedItem.Text.Equals("Seleccione..."))
        {
            int tranID = int.Parse(lb_transaccion.SelectedValue);
            if (tranID == 54) { lb_tipopersona.SelectedValue = "2"; }// agentes
            else if (tranID == 57) { lb_tipopersona.SelectedValue = "8"; }// caja chica
            else if (tranID == 58) { lb_tipopersona.SelectedValue = "2"; }// agente
            else if (tranID == 56) { lb_tipopersona.SelectedValue = "6"; }// linea aerea
            else if (tranID == 25) { lb_tipopersona.SelectedValue = "4"; }// proveedores
            else if (tranID == 55) { lb_tipopersona.SelectedValue = "5"; }// naviera
            else if (tranID == 52) { lb_tipopersona.SelectedValue = "6"; }// Linea Aerea
        }
    }
    protected void chk_anulacion_CheckedChanged(object sender, EventArgs e)
    {
        Label lb1 = new Label();
        TextBox t1 = new TextBox();
        decimal descuento = 0, total = 0;
        foreach (GridViewRow row in gv_detalle.Rows)
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
    protected void gv_detalle_RowCreated(object sender, GridViewRowEventArgs e)
    {
        e.Row.Cells[11].Visible = false;
    }
    protected void lb_serie_factura_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (lb_serie_factura.Items.Count > 0)
        {
            if (lb_serie_factura.SelectedValue != "0")
            {
                //BAW FISCAL USD
                if ((user.PaisID == 1) || (user.PaisID == 5) || (user.PaisID == 7) || (user.PaisID == 15) || (user.PaisID == 21) || (user.PaisID == 4) || (user.PaisID == 11) || (user.PaisID == 12) || (user.PaisID == 13) || (user.PaisID == 24) || (user.PaisID == 3) || (user.PaisID == 23))//BAW FISCAL USD
                {
                    int Moneda_Serie_Nota_Debito = 0;
                    int Moneda_Serie_Nota_Credito = 0;
                    Moneda_Serie_Nota_Debito = int.Parse(lb_moneda.SelectedValue);
                    Moneda_Serie_Nota_Credito = DB.Get_Serie_Moneda_By_ID(int.Parse(lb_serie_factura.SelectedValue));
                    if (Moneda_Serie_Nota_Debito != Moneda_Serie_Nota_Credito)
                    {
                        lb_serie_factura.SelectedValue = "0";
                        WebMsgBox.Show("Por Favor seleccione una Serie con la misma Moneda de la Nota de Debito");
                        return;
                    }
                }
            }
        }
    }
    protected void Determinar_Tipo_Serie(int sucID, string serie)
    {
        int Doc_ID = DB.getDocumentoID(sucID, serie, 31, user);
        lbl_serie_id.Text = Doc_ID.ToString();
        RE_GenericBean Bean_Serie = (RE_GenericBean)DB.getFactura(Doc_ID, 31);
        if (Bean_Serie == null)
        {
            WebMsgBox.Show("Existio un error tratando de determinar el Tipo de Documento, por lo cual no fue transmitido");
            return;
        }
        else
        {
            lbl_tipo_serie.Text = Bean_Serie.intC14.ToString();
            if ((user.PaisID == 1) || (user.PaisID == 5))
            {
                pnl_documento_electronico.Visible = true;
                lbl_tipo_serie_caption.Font.Bold = true;
                lbl_correo_documento_electronico.Font.Bold = true;
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