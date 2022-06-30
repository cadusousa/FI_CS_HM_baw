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
    public partial class operations_provisiones : System.Web.UI.Page
    {
        DataTable dt = null;
        UsuarioBean user = null;
        string paginaanterior = "";
        public string queriestring = "";
        int excento = 0;
        int estado_provision = 0;

        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (Request.QueryString["provID"] != null)
            {
                MasterPageFile = "~/operations/blank.master";
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["usuario"] == null)
            {
                Response.Redirect("../default.aspx");
            }
            user = (UsuarioBean)Session["usuario"];
            if (!user.Aplicaciones.Contains("6"))
                Response.Redirect("index.aspx");
            int permiso = int.Parse(user.Aplicaciones["6"].ToString());
            if (!((permiso & 256) == 256))
                Response.Redirect("index.aspx");

            int tipo_contabilidad = int.Parse(Session["Contabilidad"].ToString());
            obtengo_listas(tipo_contabilidad);
            #region Seteo de Monedas
            //AQUI MODIFIQUE DE ULTIMO
            //if (tipo_contabilidad == 2)
            //{
            //    //lb_moneda.SelectedValue = "8";
            //    //lb_moneda2.SelectedValue = "8";
            //    lb_moneda.SelectedValue = user.Moneda.ToString();
            //    lb_moneda2.SelectedValue = user.Moneda.ToString();
            //    lb_moneda.Enabled = false;
            //}
            //else
            //{
            //    //lb_moneda.SelectedValue = user.pais.ID.ToString();
            //    //lb_moneda2.SelectedValue = user.pais.ID.ToString();
            //    lb_moneda.SelectedValue = user.Moneda.ToString();
            //    lb_moneda2.SelectedValue = user.Moneda.ToString();
            //    lb_moneda.Enabled = false;
            //}
            #endregion
            int provID = 0;
            if (!Page.IsPostBack)
            {
                ArrayList arr = null;
                ListItem item = null;
                arr = null;
                if (DB.Validar_Restriccion_Activa(user, 5, 24) == true)
                {
                    arr = (ArrayList)DB.Get_Tipos_Servicio_Permitidos(user.PaisID, user.contaID, 5,user.SucursalID);
                }
                else
                {
                    arr = (ArrayList)DB.getOpcionesMatriz("tbl_tipo_servicio");
                }
                foreach (RE_GenericBean rgb in arr)
                {
                    item = new ListItem(rgb.strC1, rgb.intC1.ToString());
                    lb_servicio.Items.Add(item);
                }
                if (lb_servicio.Items.Count > 0)
                {
                    lb_servicio.SelectedIndex = 1;
                    ArrayList rubros = (ArrayList)DB.getAllRubrosXPaisXServ(user.PaisID, int.Parse(lb_servicio.SelectedValue), "");
                    RE_GenericBean rubbean = null;
                    for (int a = 0; a < rubros.Count; a++)
                    {
                        rubbean = (RE_GenericBean)rubros[a];
                        item = new ListItem(rubbean.strC1, rubbean.intC1.ToString());
                        lb_rubro.Items.Add(item);
                    }
                }
                arr = null;
                arr = (ArrayList)DB.getMonedasbyPais(user.PaisID, user.contaID);
                foreach (RE_GenericBean rgb in arr)
                {
                    item = new ListItem(rgb.strC1, rgb.intC1.ToString());
                    lb_moneda2.Items.Add(item);
                    lb_moneda.Items.Add(item);
                }
                carga_ctas_provision();//carga las cuentas contables para esta provision

                if (Request.QueryString["provID"] != null)
                {
                    //Deshabilita el boton de agregar rubros 
                    bt_agregar.Enabled = false;
                    provID = int.Parse(Request.QueryString["provID"].ToString());
                    RE_GenericBean provData = (RE_GenericBean)Utility.getDetalleProvision(provID);
                    queriestring = Request.QueryString["queriesting"].ToString();
                    lb_proveedorID.Text = provData.intC3.ToString();
                    lb_proveedorCCHID.Text = provData.intC4.ToString();
                    tb_serie_provision.Text = provData.strC17.ToString();
                    tb_correlativo_provision.Text = provData.intC6.ToString();
                    lbl_ocid.Text = provData.strC35;
                    if ((provData.strC32 != "") && (provData.intC13 != 0) && (lbl_ocid.Text != "0"))
                    {
                        lb_correlativoOC.Visible = true;
                        lb_serieOC.Visible = true;
                        lb_or_compra.Visible = true;
                        lb_or_compra2.Visible = true;
                        lb_serieOC.Text = provData.strC32;//serie_OC
                        lb_correlativoOC.Text = provData.intC13.ToString();//correlativo_OC
                        RE_GenericBean rgb_oc = (RE_GenericBean)DB.getOCData(int.Parse(lbl_ocid.Text));
                        lbl_descripcion_bs.ToolTip = rgb_oc.strC3;
                        lbl_terminos_pago.ToolTip = rgb_oc.strC13;
                        lbl_observaciones_oc.ToolTip = rgb_oc.strC7;
                        pnl_oc.Visible = true;
                    }
                    else
                    {
                        lb_or_compra.Visible = false;
                        lb_or_compra2.Visible = false;
                        lb_correlativoOC.Visible = false;
                        lb_serieOC.Visible = false;
                        pnl_oc.Visible = false;
                    }
                    //Datos del Proveedor

                    tb_factura.Text = provData.strC3;
                    tb_factura_correlativo.Text = provData.strC31;
                    tb_fecha.Text = provData.strC4;

                    tb_fecha_emision.Text = provData.strC8;
                    tb_fecha_autorizacion.Text = provData.strC12;

                    lb_fechapago.Text = provData.strC5;
                    lb_moneda.SelectedValue = provData.intC5.ToString();
                    lb_moneda.Enabled = false;
                    tb_observacion.Text = provData.strC6;
                    tb_valor.Text = provData.decC1.ToString("#,#.00#;(#,#.00#)");
                    tb_afecto.Text = provData.decC2.ToString("#,#.00#;(#,#.00#)");
                    tb_noafecto.Text = provData.decC3.ToString("#,#.00#;(#,#.00#)");
                    tb_iva.Text = provData.decC4.ToString("#,#.00#;(#,#.00#)");
                    lb_imp_exp.SelectedValue = provData.intC10.ToString();
                    lbl_blID.Text = provData.Blid.ToString();
                    lbl_tipoOperacionID.Text = provData.Tipo_Operacion.ToString();
                    if ((provData.strC32 != "") && (provData.intC13 != 0) || (lbl_tipoOperacionID.Text != "8"))
                    {
                        //CAMBIOS
                        gv_detalle.Columns[0].Visible = false;
                        bt_agregar.Visible = false;

                    }
                    else
                    {
                        gv_detalle.Columns[0].Visible = true;
                        bt_agregar.Visible = true;
                    }
                    Validar_Restricciones("Load");
                    tb_hbl.Text = provData.strC13;
                    tb_mbl.Text = provData.strC14;
                    tb_routing.Text = provData.strC15;
                    tb_contenedor.Text = provData.strC16;
                    tb_poliza_seguros.Text = provData.strC42;

                    lb_serie_factura.SelectedValue = provData.strC37;

                    tb_oc_correlativo.Text = provData.strC38;
                    lb_provID.Text = provData.intC1.ToString();
                    lb_paiID.Text = provData.intC7.ToString();
                    lb_sucID.Text = provData.intC8.ToString();
                    lb_imp_exp.SelectedValue = provData.intC10.ToString();
                    lb_tpi.Text = provData.intC11.ToString();//TPI
                    tb_poliza.Text = provData.strC34;//Poliza Aduanal
                    lb_ruta_pais.SelectedValue = provData.strC10; //Ruta Pais Mayan Logistics
                    lb_ruta.SelectedValue = provData.strC11; //Ruta Mayan Logistics
                    ddl_tipo_documento.SelectedValue = provData.intC16.ToString();
                    drp_tipo_persona.SelectedValue = provData.intC11.ToString();
                    tb_codigo_proveedor.Text = provData.intC3.ToString();
                    if (provData.boolC1 == true)
                    {
                        tb_fecha_libro_compras.Text = provData.strC41;//Fecha_Libro_Compras_Formateada
                    }
                    #region Mostrar Estado
                    lbl_ted_nombre.Text = DB.Traducir_Estado_Documento(provData.intC12);
                    #endregion
                    #region Validar si el Documento esta Anulado
                    lbl_ted_id.Text = provData.intC12.ToString();
                    if (lbl_ted_id.Text == "3")
                    {
                        RE_GenericBean Anulacion_Bean = DB.Get_Detalle_Anulacion(user, 5, provID);
                        lbl_persona_anula.ToolTip = Anulacion_Bean.strC1;
                        lbl_motivo_anulacion.ToolTip = Anulacion_Bean.strC2;
                        lbl_fecha_anulacion.ToolTip = Anulacion_Bean.strC3;
                        lbl_hora_anulacion.ToolTip = Anulacion_Bean.strC4;
                        pnl_anulaciones.Visible = true;
                    }
                    else
                    {
                        pnl_anulaciones.Visible = false;
                    }
                    #endregion
                    if (provData.boolC1 == true)
                    {
                        Rb_Documento.Items[0].Selected = true;
                    }
                    else
                    {
                        Rb_Documento.Items[1].Selected = true;
                    }
                    RE_GenericBean proveedordata = null;
                    int excento = 0;
                    int proveedorID = int.Parse(lb_proveedorID.Text);
                    int proveedorCCHID = int.Parse(lb_proveedorCCHID.Text);
                    if (provData.intC11 == 2)
                    { // es un agente
                        lb_transaccion.SelectedValue = "15";
                        proveedordata = (RE_GenericBean)DB.getAgenteData(proveedorID, "");// obtengo los datos del agente
                        excento = DB.getProveedorRegimen(provData.intC11, proveedorID.ToString());
                        lb_contribuyente.SelectedValue = excento.ToString();
                        if (proveedordata != null)
                        {
                            tb_nombre.Text = proveedordata.strC1;
                            tb_nit.Text = proveedordata.strC6;
                        }
                        lb_transaccion.Enabled = false;
                    }
                    else if (provData.intC11 == 4)
                    {// es un proveedor 
                        lb_transaccion.SelectedValue = "8";
                        proveedordata = (RE_GenericBean)DB.getProveedorData(proveedorID, ""); //obtengo los datos del proveedor
                        excento = DB.getProveedorRegimen(provData.intC11, proveedorID.ToString());
                        lb_contribuyente.SelectedValue = excento.ToString();
                        tb_nit.Text = proveedordata.strC1;
                        tb_nombre.Text = proveedordata.strC2;
                        lb_transaccion.Enabled = false;
                    }
                    else if (provData.intC11 == 5)
                    {// naviera
                        lb_transaccion.SelectedValue = "17";
                        proveedordata = (RE_GenericBean)DB.getNavieraData(proveedorID); //obtengo los datos del proveedor
                        excento = DB.getProveedorRegimen(provData.intC11, proveedorID.ToString());
                        lb_contribuyente.SelectedValue = excento.ToString();
                        tb_nit.Text = proveedordata.strC2;
                        tb_nombre.Text = proveedordata.strC1;
                        lb_transaccion.Enabled = false;
                    }
                    else if (provData.intC11 == 6)
                    {// Linea aerea
                        lb_transaccion.SelectedValue = "18";
                        proveedordata = (RE_GenericBean)DB.getCarriersData(proveedorID); //obtengo los datos del proveedor
                        excento = DB.getProveedorRegimen(provData.intC11, proveedorID.ToString());
                        lb_contribuyente.SelectedValue = excento.ToString();
                        tb_nit.Text = proveedordata.strC2;
                        tb_nombre.Text = proveedordata.strC1;
                        lb_transaccion.Enabled = false;
                    }
                    else if (provData.intC11 == 8)
                    {// Caja chica
                        lb_transaccion.SelectedValue = "53";
                        proveedordata = (RE_GenericBean)DB.getProveedorData(proveedorCCHID, ""); //obtengo los datos del proveedor de caja chica
                        excento = DB.getProveedorRegimen(4, proveedorCCHID.ToString());
                        lb_contribuyente.SelectedValue = excento.ToString();
                        tb_nit.Text = proveedordata.strC1;
                        tb_nombre.Text = proveedordata.strC2;
                        lb_transaccion.Enabled = false;
                    }
                    else if (provData.intC11 == 10)
                    {// Intercompany
                        lb_transaccion.SelectedValue = "105";
                        proveedordata = (RE_GenericBean)DB.getIntercompanyData(proveedorID); //obtengo los datos del proveedor
                        excento = DB.getProveedorRegimen(provData.intC11, proveedorID.ToString());
                        lb_contribuyente.SelectedValue = excento.ToString();
                        tb_nit.Text = proveedordata.strC2;
                        tb_nombre.Text = proveedordata.strC7 + "   (" + proveedordata.strC1 + ")";
                        lb_transaccion.Enabled = false;
                    }
                    lb_contribuyente.SelectedValue = provData.Tipo_Contribuyente.ToString();
                    //DataTable rubdt = (DataTable)DB.getRubbyOC(provID, 5);//5=provision segun sys_tipo_referencia
                    DataTable rubdt = (DataTable)DB.getRubby_PRV(provID, 5);//5=provision segun sys_tipo_referencia
                    gv_detalle.DataSource = rubdt;
                    gv_detalle.DataBind();
                    tb_valor.Text = provData.decC1.ToString("#,#.00#;(#,#.00#)");
                    tb_afecto.Text = provData.decC2.ToString("#,#.00#;(#,#.00#)");
                    tb_noafecto.Text = provData.decC3.ToString("#,#.00#;(#,#.00#)");
                    tb_iva.Text = provData.decC4.ToString("#,#.00#;(#,#.00#)");
                    estado_provision = provData.intC12;

                }
                #region Seteo de Moneda
                //if (tipo_contabilidad == 2)
                //{
                //    //lb_moneda.SelectedValue = "8";
                //    lb_moneda.SelectedValue = user.Moneda.ToString();
                //    lb_moneda2.SelectedValue = user.Moneda.ToString();
                //    lb_moneda.Enabled = false;
                //}
                //else
                //{
                //    //lb_moneda.SelectedValue = user.pais.ID.ToString();
                //    lb_moneda.SelectedValue = user.Moneda.ToString();
                //    lb_moneda2.SelectedValue = user.Moneda.ToString();
                //    lb_moneda.Enabled = false;
                //}
                #endregion

                gv_detalle_partida.DataSource = DB.getPolizaDiario(user, double.Parse(provID.ToString()), 5, 0);
                gv_detalle_partida.DataBind();

                //Mostrando la Partida contable generada
                Label mi_variable;
                int i = 0;
                foreach (GridViewRow row in gv_detalle_partida.Rows)
                {
                    mi_variable = (Label)row.FindControl("lb_desc_cuenta");
                    if (mi_variable.Text.Equals("TOTAL"))
                    {
                        gv_detalle_partida.Rows[i].Font.Bold = true;
                    }
                    i++;
                }

                if (estado_provision != 1)
                {
                    bt_guardar.Enabled = false;
                    gv_detalle.Enabled = false;
                    bt_cancelar.Enabled = false;
                    lb_contribuyente.Enabled = false;
                    lb_imp_exp.Enabled = false;
                    bt_agregar.Enabled = false;

                    tb_fecha.ReadOnly = true;
                    lb_fechapago.ReadOnly = true;
                };

                //Desplegando Panel de Datos de Mayan
                if (user.PaisID == 13)
                {
                    Pnl_Mayan_Logistics.Visible = true;
                }
                if (Request.QueryString["provisionAutomatica"] != null && Request.QueryString["provisionAutomatica"].ToString() == "1")
                {
                    bt_guardar_Click(sender, e);
                }
            }
        }

        private void carga_ctas_provision()
        {

        }
        private void obtengo_listas(int tipoconta)
        {
            ListItem item = null;
            ArrayList arr = null;
            if (!Page.IsPostBack)
            {
                //arr = (ArrayList)DB.getOpcionesMatriz("tbl_tipo_transaccion, tbl_usuario_operacion where uop_usu_id='" + user.ID + "' and uop_pai_id=" + user.PaisID + " and uop_ttt_id=ttt_id and ttt_template='provisiones.aspx'");
                arr = (ArrayList)DB.getOpcionesMatriz("tbl_tipo_transaccion where ttt_template='provisiones.aspx'");
                foreach (RE_GenericBean rgb in arr)
                {
                    item = new ListItem(rgb.strC1, rgb.intC1.ToString());
                    lb_transaccion.Items.Add(item);

                }
                //lb_transaccion.SelectedIndex = 1;
                lb_transaccion.Enabled = true;
                arr = null;
                arr = (ArrayList)DB.getMonedasbyPais(user.PaisID, user.contaID);
                foreach (RE_GenericBean rgb in arr)
                {
                    item = new ListItem(rgb.strC1, rgb.intC1.ToString());
                    lb_moneda.Items.Add(item);
                    lb_moneda2.Items.Add(item);
                }
                arr = null;
                arr = (ArrayList)DB.getSerieFactbySuc(user.SucursalID, 8, user, 0);//1 porque es el tipo de documento para facturacion
                foreach (string valor in arr)
                {
                    item = new ListItem(valor, valor);
                    lb_serie_factura.Items.Add(item);
                }
                arr = null;
                arr = (ArrayList)DB.getOpcionesMatriz("tbl_tipo_imp_exp");
                foreach (RE_GenericBean rgb in arr)
                {
                    item = new ListItem(rgb.strC1, rgb.intC1.ToString());

                    lb_imp_exp.Items.Add(item);
                }

                arr = null;
                arr = (ArrayList)DB.getOpcionesMatriz("tbl_tipo_contribuyente");
                foreach (RE_GenericBean rgb in arr)
                {
                    item = new ListItem(rgb.strC1, rgb.intC1.ToString());
                    lb_contribuyente.Items.Add(item);
                }

                chklist_retencion.Visible = false;
                DataSet ds = (DataSet)DB.getRetencionesDataSet(user.PaisID);
                gv_retenciones.DataSource = ds.Tables["ret"];
                gv_retenciones.DataBind();


                arr = (ArrayList)DB.getTipoDocumentoSat(user, "");
                item = new ListItem("Seleccione...", "0");
                ddl_tipo_documento.Items.Add(item);
                foreach (RE_GenericBean rgb in arr)
                {

                    item = new ListItem(rgb.strC2 + "-" + rgb.strC1, rgb.intC1.ToString());
                    ddl_tipo_documento.Items.Add(item);
                }

                if (user.PaisID == 1) { ddl_tipo_documento.Visible = true; lb_tipo_docto.Visible = true; }
            }
        }
        protected void bt_cancelar_Click(object sender, EventArgs e)
        {
            ClientScript.RegisterStartupScript(typeof(Page), "closePage", "window.close();", true);
        }


        protected void bt_guardar_Click(object sender, EventArgs e)
        {
            #region Validar Permiso para Autorizar Transaccion de la Provision
            int bandera_ttt_id = 0;
            int provision_ttt_id = 0;
            provision_ttt_id = int.Parse(lb_transaccion.SelectedValue);
            ArrayList Arr_Transacciones_Usuario = (ArrayList)DB.getOpcionesMatriz("tbl_tipo_transaccion, tbl_usuario_operacion where uop_usu_id='" + user.ID + "' and uop_pai_id=" + user.PaisID + " and uop_ttt_id=ttt_id and ttt_template='provisiones.aspx'");
            foreach (RE_GenericBean Bean_Transaccion in Arr_Transacciones_Usuario)
            {
                if (Bean_Transaccion.intC1 == provision_ttt_id)
                {
                    bandera_ttt_id++;
                }
            }
            if (bandera_ttt_id == 0)
            {
                bt_guardar.Enabled = false;
                bt_cancelar.Enabled = false;
                bt_agregar.Enabled = false;
                gv_detalle.Enabled = false;
                WebMsgBox.Show("Estimado usuario, usted no tiene permiso para autorizar.: " + lb_transaccion.SelectedItem.Text);
                return;
            }
            #endregion
            if (tb_fecha_libro_compras.Text.Trim().Equals("") && (bool.Parse(Rb_Documento.SelectedValue) == true))
            {
                WebMsgBox.Show("Debe seleccionar la Fecha del Libro de Compras");
                return;
            }
            if (gv_detalle.Rows.Count == 0)
            {
                WebMsgBox.Show("No se puede autorizar una provision sin rubros");
                return;
            }
            int tipo_cobro = 1;//collect
            int transID = 15;
            switch (int.Parse(lb_transaccion.Text))
            {
                case 8:
                    transID = 8;
                    break;
                case 53:
                    transID = 8;
                    break;
            }
            int contribuyente = int.Parse(lb_contribuyente.SelectedValue);
            int moneda = int.Parse(lb_moneda.SelectedValue);
            int imp_exp = int.Parse(lb_imp_exp.SelectedValue);
            int tipo_contabilidad = int.Parse(Session["Contabilidad"].ToString());
            //int servicio = 0;
            int tipo_bienserv = int.Parse(rb_bienserv.SelectedValue);

            user = (UsuarioBean)Session["usuario"];
            int provID = int.Parse(lb_provID.Text);
            RE_GenericBean rgb = new RE_GenericBean();
            #region Validar si se cambia el Regimen
            if (Request.QueryString["provID"] != null)
            {
                int _provisionID = int.Parse(Request.QueryString["provID"].ToString());
                int _contribuyente = DB.Get_RegimenXProvision(_provisionID);
                if (_contribuyente != contribuyente)
                {
                    rgb.Modifica_Regimen = user.ID;
                }
            }
            #endregion
            rgb.intC8 = int.Parse(lb_provID.Text);
            rgb.intC9 = int.Parse(lb_tpi.Text);//TPI
            rgb.intC10 = int.Parse(tb_codigo_proveedor.Text);//Codigo de Proveedor
            rgb.intC1 = moneda;
            rgb.intC2 = tipo_contabilidad;
            rgb.decC1 = decimal.Parse(tb_iva.Text);
            rgb.intC4 = tipo_bienserv;
            rgb.decC2 = decimal.Parse(tb_valor.Text);
            rgb.decC3 = decimal.Parse(tb_afecto.Text);
            rgb.decC4 = decimal.Parse(tb_noafecto.Text);
            rgb.decC5 = decimal.Parse(tb_iva.Text);
            rgb.strC2 = user.ID;
            rgb.strC3 = tb_observacion.Text;
            rgb.Fiscal_No_Fiscal = bool.Parse(Rb_Documento.SelectedValue);
            rgb.Tipo_Contribuyente = contribuyente;



            if ((tb_factura.Text.Trim() != "") && (tb_factura_correlativo.Text.Trim() != "") && (tb_fecha.Text.Trim() != ""))
            {
                rgb.strC12 = tb_factura.Text.Trim();
                rgb.strC13 = tb_factura_correlativo.Text.Trim();
                rgb.strC14 = DB.DateFormat(tb_fecha.Text.Trim());
                rgb.strC15 = rgb.strC14;

                DateTime aux_fecha_emision = DateTime.Parse(rgb.strC15);
                double dias_credito = DB.getFechaMaxPago(rgb.intC8, rgb.intC9, user);
                DateTime aux_fecha_pago = aux_fecha_emision.AddDays(dias_credito);
                string fecha_pago_formateada = aux_fecha_pago.ToString();
                rgb.strC15 = DateTime.Parse(fecha_pago_formateada).ToString("yyyy/MM/dd", System.Globalization.CultureInfo.InvariantCulture);

            }
            else if ((tb_factura.Text.Trim() != "") || (tb_factura_correlativo.Text.Trim() != "") || (tb_fecha.Text.Trim() != ""))
            {
                WebMsgBox.Show("Debe Ingresar Todos los Campos del Documento de Proveedor");
                return;
            }
            if (rgb.strC14 == "")
            {
                rgb.strC14 = null;
                rgb.strC15 = null;
            }

            //Para Mayan se guardan el Pais y Ruta
            if (user.PaisID == 13)
            {
                rgb.strC10 = lb_ruta_pais.SelectedValue;
                rgb.strC11 = lb_ruta.SelectedValue;
            }

            string fecha_libro_compras = tb_fecha_libro_compras.Text;
            ArrayList Arr_Fechas = (ArrayList)DB.Formatear_Fechas(fecha_libro_compras, "");
            fecha_libro_compras = Arr_Fechas[0].ToString();
            rgb.strC4 = fecha_libro_compras;
            #region Obtener monto en dolares
            //// Si la moneda que estoy agregando es diferente a la que estoy facturando
            /*
            if (!lb_moneda.SelectedValue.Equals("8"))
            {
                if (!lb_moneda2.SelectedValue.Equals("8"))
                {
                    totalD = Math.Round(decimal.Parse(tb_valor.Text), 2);
                }
                else
                {
                    totalD = Math.Round((decimal.Parse(tb_valor.Text) * (decimal)user.pais.TipoCambio), 2);
                }
            }
            else
            {
                if (lb_moneda2.SelectedValue.Equals("8"))
                {
                    totalD = decimal.Parse(tb_valor.Text);
                }
                else
                {
                    totalD = Math.Round((decimal.Parse(tb_valor.Text) / (decimal)user.pais.TipoCambio), 2);
                }
            }
            rgb.decC6 = totalD;
            */
            #endregion
            //recorro el datagrid para armar la factura
            Label lb1, lb2, lb3, lb4, lb5, lb6, lb7, lb8, lb9, lb10;
            Rubros rubro;
            #region Construir Libro Diario
            foreach (GridViewRow row in gv_detalle.Rows)
            {
                lb1 = (Label)row.FindControl("lb_codigo");
                lb2 = (Label)row.FindControl("lb_rubro");
                lb3 = (Label)row.FindControl("lb_tipo");
                lb4 = (Label)row.FindControl("lb_subtotal");
                lb5 = (Label)row.FindControl("lb_impuesto");
                lb6 = (Label)row.FindControl("lb_total");
                lb7 = (Label)row.FindControl("lb_totaldolares");
                lb8 = (Label)row.FindControl("lb_monedatipo");
                rubro = new Rubros();
                rubro.rubroID = long.Parse(lb1.Text);
                rubro.rubroName = lb2.Text;
                rubro.rubtoType = lb3.Text;
                rubro.rubroTypeID = Utility.TraducirServiciotoINT(lb3.Text);
                rubro.rubroSubTot = double.Parse(lb4.Text);
                rubro.rubroImpuesto = double.Parse(lb5.Text);
                rubro.rubroTot = double.Parse(lb6.Text);
                rubro.rubroTotD = double.Parse(lb7.Text);
                rgb.decC8 += Convert.ToDecimal(rubro.rubroTotD);
                rubro.rubroMoneda = Utility.TraducirMonedaStr(lb8.Text);
                rubro.cta_debe = (ArrayList)DB.getCtaContablebyRubro("debe", (int)rubro.rubroID, user.PaisID, transID, contribuyente, moneda, imp_exp, tipo_cobro, tipo_contabilidad, rubro.rubroTypeID);
                //Sumando los Rubros Exentos
                if (rubro.rubroImpuesto == 0)
                {
                    rgb.decC7 += decimal.Parse(rubro.rubroTot.ToString());
                }

                if (Label1.Text == "1")
                {
                    rgb.decC6 += decimal.Parse(rubro.rubroTotD.ToString());
                }
                else if (Label1.Text == "0")
                {
                    rgb.decC6 += decimal.Parse(lb7.Text.ToString());

                }
                if ((rubro.cta_debe == null) || (rubro.cta_debe.Count == 0))
                {
                    WebMsgBox.Show("El rubro " + rubro.rubroName + " no tiene configurada ninguna cuenta contable para la combinacion de cobro que esta realizando, por favor pongase encontacto con el Contador");
                    return;
                }

                if (rgb.arr1 == null) rgb.arr1 = new ArrayList();
                rgb.arr1.Add(rubro);
            }
            #endregion

            #region Construir Detalle Facturacion
            foreach (GridViewRow row in gv_detalle.Rows)
            {
                lb1 = (Label)row.FindControl("lb_codigo");
                lb2 = (Label)row.FindControl("lb_rubro");
                lb3 = (Label)row.FindControl("lb_tipo");
                lb4 = (Label)row.FindControl("lb_subtotal");
                lb5 = (Label)row.FindControl("lb_impuesto");
                lb6 = (Label)row.FindControl("lb_total");
                lb7 = (Label)row.FindControl("lb_totaldolares");
                lb8 = (Label)row.FindControl("lb_monedatipo");
                lb9 = (Label)row.FindControl("lb_dfid");
                lb10 = (Label)row.FindControl("lb_costoid");
                if (lb9.Text == "0")
                {
                    rubro = new Rubros();
                    rubro.rubroID = long.Parse(lb1.Text);
                    rubro.rubroName = lb2.Text;
                    rubro.rubtoType = lb3.Text;
                    rubro.rubroTypeID = Utility.TraducirServiciotoINT(lb3.Text);
                    rubro.rubroSubTot = double.Parse(lb4.Text);
                    rubro.rubroImpuesto = double.Parse(lb5.Text);
                    rubro.rubroTot = double.Parse(lb6.Text);
                    rubro.rubroTotD = double.Parse(lb7.Text);
                    rubro.rubroMoneda = Utility.TraducirMonedaStr(lb8.Text);
                    rubro.cta_debe = (ArrayList)DB.getCtaContablebyRubro("debe", (int)rubro.rubroID, user.PaisID, transID, contribuyente, moneda, imp_exp, tipo_cobro, tipo_contabilidad, rubro.rubroTypeID);
                    rubro.RubroCostoID = double.Parse(lb10.Text);
                    //Sumando los Rubros Exentos
                    if (rubro.rubroImpuesto == 0)
                    {
                        rgb.decC7 += decimal.Parse(rubro.rubroTot.ToString());
                    }

                    if (Label1.Text == "1")
                    {
                        //if (moneda == 8)
                        //{
                        //    rubro.rubroTotD = Math.Round(rubro.rubroTot * (double)user.pais.TipoCambio, 2);
                        //}
                        //else
                        //{
                        //    rubro.rubroTotD = Math.Round(rubro.rubroTot / (double)user.pais.TipoCambio, 2);
                        //}
                    }
                    else if (Label1.Text == "0")
                    {
                        //rubro.rubroTotD = double.Parse(lb7.Text.ToString());
                    }
                    if ((rubro.cta_debe == null) || (rubro.cta_debe.Count == 0))
                    {
                        WebMsgBox.Show("El rubro " + rubro.rubroName + " no tiene configurada ninguna cuenta contable para la combinacion de cobro que esta realizando, por favor pongase encontacto con el Contador");
                        return;
                    }
                    if (rgb.arr3 == null) rgb.arr3 = new ArrayList();
                    rgb.arr3.Add(rubro);
                }
            }
            #endregion
            RE_GenericBean retencion = null;
            CheckBox chk = null;
            TextBox tb = null;
            Label lb = null;
            foreach (GridViewRow row in gv_retenciones.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    chk = (CheckBox)row.FindControl("chk_retencion");
                    if (chk.Checked)
                    {
                        tb = (TextBox)row.FindControl("tb_codigo");
                        lb = (Label)row.FindControl("lb_retencion");
                        lb2 = (Label)row.FindControl("lb_nombre");

                        
                        int id_ret = int.Parse(lb.Text);

                        if (user.PaisID == 1 && (id_ret == 119 || id_ret == 120 || id_ret == 121 || id_ret == 122 || id_ret == 123 || id_ret == 124 || id_ret == 126 || id_ret == 127 || id_ret == 128))
                        {
                            decimal monto_total = decimal.Parse(tb_monto.Text);

                            decimal monto_minimo = 0;
                            switch (id_ret)
                            {
                                case 119:
                                    monto_minimo = 2500;
                                    break;
                                case 120:
                                    monto_minimo = 2500;
                                    break;
                                case 121:
                                    monto_minimo = 2500;
                                    break;
                                case 122:
                                    monto_minimo = 2500;
                                    break;
                                case 123:
                                    monto_minimo = 30000;
                                    break;
                                case 124:
                                    monto_minimo = 0;
                                    break;
                                case 126:
                                    monto_minimo = 0;
                                    break;
                                case 127:
                                    monto_minimo = 2500;
                                    break;
                                case 128:
                                    monto_minimo = 2500;
                                    break;
                                default:
                                    monto_minimo = 0;
                                    break;
                            }
                            //monto_minimo = DB.Validar_Montominimo_Retencion(user, id_ret);

                            if (monto_total < monto_minimo)
                            {
                                WebMsgBox.Show("Para poder aplicar esta Retencion el monto total debe ser igual o mayor a " + monto_minimo.ToString());
                                return;
                            }
                        }

                        if (tb.Text.Trim() == "")
                        {
                            WebMsgBox.Show("Debe ingresar el numero de Referencia para la Retencion.: " + lb2.Text);
                            return;
                        }
                        else
                        {
                            int resultado_validacion = 0;
                            resultado_validacion = DB.Validar_Referencia_Retencion(user, provID, tb.Text.Trim());
                            if (resultado_validacion > 0)
                            {
                                WebMsgBox.Show("Numero de Retencion Repetido, porfavor ingrese un numero de Referencia Valido para la Retencion.: " + lb2.Text + "");
                                return;
                            }
                        }


                        retencion = new RE_GenericBean();
                        retencion = GeneroRetencion(int.Parse(lb.Text), moneda, tipo_contabilidad, provID);
                        retencion.strC1 = tb.Text.Trim();
                        rgb.douC1 += double.Parse(retencion.decC1.ToString());
                        if (rgb.arr2 == null) rgb.arr2 = new ArrayList();
                        rgb.arr2.Add(retencion);
                    }
                }
            }

            int matOpID = DB.getMatrizOperacionID(int.Parse(lb_transaccion.Text), moneda, user.PaisID, tipo_contabilidad);
            ArrayList ctas_cargo = (ArrayList)DB.getMatrizConfiguracion_ingreso_egreso(matOpID, "Abono");

            #region Cargar Rubros a Eliminar
                RE_GenericBean Anulaciones_Bean = null;
                int Tipo_Operacion = int.Parse(lbl_tipoOperacionID.Text);
                int Blid = int.Parse(lbl_blID.Text);
                string Id_Costo = "0";
                string DFID = "0";
                foreach (GridViewRow row in gv_detalle_anular.Rows)
                {
                    lb9 = (Label)row.FindControl("lb_dfid2");
                    lb10 = (Label)row.FindControl("lb_costoid2");
                    DFID = lb9.Text;
                    Id_Costo = lb10.Text;
                    Anulaciones_Bean = new RE_GenericBean();
                    Anulaciones_Bean.strC1 = user.pais.ISO.ToLower();
                    Anulaciones_Bean.intC1 = Tipo_Operacion;
                    Anulaciones_Bean.douC1 = double.Parse(Id_Costo.ToString());
                    Anulaciones_Bean.intC2 = Blid;
                    Anulaciones_Bean.strC2 = DFID;
                    rgb.arr4.Add(Anulaciones_Bean);
                }
            #endregion

            int tedID_actual = 0;
            tedID_actual = DB.getEstadoDocumento(rgb.intC8, 5);
            if (tedID_actual != 1)
            {
                bt_guardar.Enabled = false;
                bt_cancelar.Enabled = false;
                WebMsgBox.Show("El Documento ya fue Autorizado");
                return;
            }
            else
            {
                int result = DB.UpdateValidaProvision(rgb, user, transID, tipo_contabilidad, ctas_cargo, transID);
                if (result < 0)
                {
                    Session["factura"] = null;
                    Session["msg"] = "Error, Existió un problema al momento de guardar la provision, por favor intente de nuevo.";
                    Session["url"] = "index.aspx";
                    WebMsgBox.Show("Existio un error al tratar de autorizar la provision, porfavor intente de nuevo.");
                    bt_guardar.Enabled = false;
                    bt_cancelar.Enabled = false;
                    gv_detalle.Enabled = false;
                    return;
                }
                else
                {
                    //Mostrando la Partida contable generada
                    gv_detalle_partida.DataSource = DB.getPolizaDiario(user, double.Parse(rgb.intC8.ToString()), 5, 0);
                    gv_detalle_partida.DataBind();
                    int i = 0;
                    Label mi_variable;
                    foreach (GridViewRow row in gv_detalle_partida.Rows)
                    {
                        mi_variable = (Label)row.FindControl("lb_desc_cuenta");
                        if (mi_variable.Text.Equals("TOTAL"))
                        {
                            gv_detalle_partida.Rows[i].Font.Bold = true;
                        }
                        i++;
                    }
                    bt_guardar.Enabled = false;
                    bt_cancelar.Enabled = false;
                    bt_agregar.Enabled = false;
                    gv_detalle.Columns[0].Visible = false;

                    WebMsgBox.Show("La Provision fue Autorizada exitosamente",
                        Request.QueryString["provisionAutomatica"] != null && Request.QueryString["provisionAutomatica"].ToString() == "1" ?
                        WebMsgBox.TipoMensaje.SalirVentanaActual : WebMsgBox.TipoMensaje.SoloTexto);
                    
                    return;
                }
            }
            DataTable provListDT = null;
        }

        protected void bt_aceptar_Click(object sender, EventArgs e)
        {
            /*
            user = (UsuarioBean)Session["usuario"];
            int provID = int.Parse(Request.QueryString["provID"].ToString());
            RE_GenericBean rgb = new RE_GenericBean();
            rgb.intC1 = provID;
            rgb.decC4 = decimal.Parse(tb_iva.Text.Trim());

            DB.UpdateAceptaProvision(rgb, user);
            Response.Redirect("index.aspx");
             */ 
        }

        protected void tb_noafecto_TextChanged(object sender, EventArgs e)
        {
            if (decimal.Parse(tb_noafecto.Text.Trim()) <= decimal.Parse(tb_valor.Text.Trim()))
            {
                user = (UsuarioBean)Session["usuario"];
                decimal valor = decimal.Parse(tb_valor.Text);
                decimal noafecto = decimal.Parse(tb_noafecto.Text);
                decimal afecto = (valor - noafecto);
                decimal impuesto = Math.Round(((afecto / (user.pais.Impuesto + 1)) * user.pais.Impuesto), 2);
                afecto -= impuesto;
                //tb_afecto.Text = afecto.ToString();
                //tb_iva.Text = impuesto.ToString();
                //Eliminar Comas
                tb_afecto.Text = afecto.ToString("#,#.00#;(#,#.00#)");
                tb_iva.Text = impuesto.ToString("#,#.00#;(#,#.00#)");
            }
            else
            {
                tb_noafecto.Text = "0";
                WebMsgBox.Show("No puede ingresar un valor no afecto mayor al de la provision");
                return;
            }
        }

        protected void gv_detalle_DataBound(object sender, EventArgs e)
        {

            decimal impuesto = 0;
            decimal total = 0;
            double afecto = 0;
            double noafecto = 0;

            //decimal impuesto = decimal.Parse(tb_iva.Text);
            //decimal total = decimal.Parse(tb_valor.Text);
            //decimal afecto = decimal.Parse(tb_noafecto.Text);
            //decimal noafecto = decimal.Parse(tb_noafecto.Text);
            int tipo_contabilidad = int.Parse(Session["Contabilidad"].ToString());
            Label lb1, lb2, lb3, lb4, lb5, lb6;

            foreach (GridViewRow row in gv_detalle.Rows)
            {
                lb1 = (Label)row.FindControl("lb_subtotal");
                lb2 = (Label)row.FindControl("lb_impuesto");
                lb3 = (Label)row.FindControl("lb_total");
                lb4 = (Label)row.FindControl("lb_totaldolares");
                lb5 = (Label)row.FindControl("lb_monedatipo");
                lb6 = (Label)row.FindControl("lb_codigo");
                if (lb5.Text.Equals("1")) lb5.Text = "GTQ";
                if (lb5.Text.Equals("2")) lb5.Text = "SVC";
                if (lb5.Text.Equals("3")) lb5.Text = "HNL";
                if (lb5.Text.Equals("4")) lb5.Text = "NIC";
                if (lb5.Text.Equals("5")) lb5.Text = "CRC";
                if (lb5.Text.Equals("6")) lb5.Text = "PAB";
                if (lb5.Text.Equals("7")) lb5.Text = "BZD";
                if (lb5.Text.Equals("8")) lb5.Text = "USD";

                Rubros rubtemp = new Rubros();
                Rubros rub = new Rubros();
                //if (lb_contribuyente.SelectedValue.Equals("2"))
                //{  //significa que si es contribuyente
                rub.rubroID = int.Parse(lb6.Text);
                rub.rubroSubTot = double.Parse(lb1.Text);
                rub.rubroImpuesto = double.Parse(lb2.Text);
                rub.rubroTot = double.Parse(lb3.Text);

                rubtemp = (Rubros)DB.ExistRubroPais(rub, user.PaisID);
                if (rubtemp == null)
                {
                    WebMsgBox.Show("Error, El HBL no tiene todos los rubros registrados en contabilidad para este pais, por favor pongase en contacto con el departamento de contabilidad para agregarlos.");
                    return;
                }

                if (rubtemp.rubroImpuesto == 0)
                {
                    noafecto += rubtemp.rubroSubTot;
                }
                else
                {
                    afecto += rubtemp.rubroSubTot;
                }

                lb1.Text = rubtemp.rubroSubTot.ToString();
                lb2.Text = rubtemp.rubroImpuesto.ToString();
                lb3.Text = rubtemp.rubroTot.ToString();
                //}

                impuesto += Math.Round((decimal)rubtemp.rubroImpuesto, 2);
                total += Math.Round((decimal)rubtemp.rubroTot, 2);
            }

            tb_iva.Text = impuesto.ToString("#,#.00#;(#,#.00#)");
            tb_afecto.Text = afecto.ToString("#,#.00#;(#,#.00#)");
            tb_noafecto.Text = noafecto.ToString("#,#.00#;(#,#.00#)");
            //
            tb_valor.Text = total.ToString("#,#.00#;(#,#.00#)");
        }

        protected RE_GenericBean GeneroRetencion(int retID, int moneda, int tipo_contabilidad, int provID)
        {
            RE_GenericBean result = new RE_GenericBean();
            UsuarioBean user = (UsuarioBean)Session["usuario"];
            decimal valor = decimal.Parse(tb_valor.Text);
            decimal noafecto = decimal.Parse(tb_noafecto.Text);
            decimal afecto = (valor - noafecto);
            decimal impuesto = Math.Round(((afecto / (user.pais.Impuesto + 1)) * user.pais.Impuesto), 2);
            decimal basse = Math.Round(((afecto - impuesto) + noafecto), 2);
            decimal retencion = 0;
            decimal retencion_equivalente = 0;
            decimal temp = 0;
            int moneda_Temporal = 0;
            moneda_Temporal = int.Parse(lb_moneda.SelectedValue);
            //Si no existe valor afecto sobre el cual realizar la retencion y si solicita retencion, entonces se hace sobre el valor total
            if (basse == 0)
            {
                basse = Math.Round(valor, 2);
            }

            RE_GenericBean rgb = (RE_GenericBean)DB.getRetencionData(retID);

            //Obtengo el tipo de cambio cuando se genero la provision
            decimal tipo_cambio_prov = DB.getTipoCambioProv(provID);
            //int matOpID = DB.getMatrizOperacionID(rgb.intC5, moneda, 1, tipo_contabilidad);
            int matOpID = DB.getMatrizOperacionID(rgb.intC5, moneda_Temporal, 1, tipo_contabilidad);
            if (rgb.arr1 == null) rgb.arr1 = new ArrayList();
            rgb.arr1 = (ArrayList)DB.getMatrizConfiguracion_ingreso_egreso(matOpID, "Abono");

            if (rgb.intC2 == 1)
            { //Calculo sobre la base=total-impuesto
                if (basse >= rgb.decC1)
                {
                    temp = basse * rgb.decC2 / 100;
                    retencion = Math.Round(temp, 2);
                }
            }
            else if (rgb.intC2 == 2)
            { //Calculo sobre el impuesto
                if (impuesto >= rgb.decC1)
                {
                    temp = impuesto * rgb.decC2 / 100;
                    retencion = Math.Round(temp, 2);
                }
            }
            else if (rgb.intC2 == 3)
            { //Calculo sobre el total
                if (valor >= rgb.decC1)
                {
                    temp = valor * rgb.decC2 / 100;
                    retencion = Math.Round(temp, 2);
                }
            }
            else if (rgb.intC2 == 4)
            { //Calculo sobre el no afecto
                if (valor >= rgb.decC1)
                {
                    temp = noafecto * rgb.decC2 / 100;
                    retencion = Math.Round(temp, 2);
                }
            }
            else if (rgb.intC2 == 5)
            {//Calculo con un porcentaje inicial sobre el monto limite y un segundo porcentaje sobre el excedente
                if (basse >= rgb.decC1)
                {
                    if (basse <= rgb.decC5)
                    {
                        temp = basse * rgb.decC3 / 100;
                        retencion = Math.Round(temp, 2);
                    }
                    else
                    {
                        decimal excedente = basse - rgb.decC5;
                        temp = rgb.decC5 * rgb.decC3 / 100;
                        temp += excedente * rgb.decC4 / 100;
                        retencion = Math.Round(temp, 2);
                    }
                }
            }



            if (lb_moneda.SelectedValue.Equals("8"))
            {
                retencion_equivalente = Math.Round(retencion * tipo_cambio_prov, 2);
            }
            else
            {
                retencion_equivalente = Math.Round(retencion / tipo_cambio_prov, 2);
            }
            //obtengo las cuentas contables
            result.intC1 = retID;
            result.decC1 = retencion;
            result.decC2 = retencion_equivalente;
            result.decC3 = tipo_cambio_prov;
            result.intC5 = rgb.intC5;
            result.arr1 = rgb.arr1;
            return result;
        }

        protected void gv_detalle_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            Label lb1, lb2, lb3, lb4, lb5, lb6, lb7, lb8, lb9, lb10;
            #region Cargar Rubros del Grid a Autorizar
            #region Definir DataTable
            DataTable dt = new DataTable();
            dt.Columns.Add("ID");
            dt.Columns.Add("NOMBRE");
            dt.Columns.Add("TYPE");
            dt.Columns.Add("MONEDATYPE");
            dt.Columns.Add("TOTALD");
            dt.Columns.Add("SUBTOTAL");
            dt.Columns.Add("IMPUESTO");
            dt.Columns.Add("TOTAL");
            dt.Columns.Add("DFID");
            dt.Columns.Add("COSTOID");
            dt.Clear();
            #endregion
            foreach (GridViewRow row in gv_detalle.Rows)
            {
                lb1 = (Label)row.FindControl("lb_codigo");
                lb2 = (Label)row.FindControl("lb_rubro");
                lb3 = (Label)row.FindControl("lb_tipo");
                lb4 = (Label)row.FindControl("lb_monedatipo");
                lb5 = (Label)row.FindControl("lb_totaldolares");
                lb6 = (Label)row.FindControl("lb_subtotal");
                lb7 = (Label)row.FindControl("lb_impuesto");
                lb8 = (Label)row.FindControl("lb_total");
                lb9 = (Label)row.FindControl("lb_dfid");
                lb10 = (Label)row.FindControl("lb_costoid");
                object[] objArr = { lb1.Text, lb2.Text, lb3.Text, lb4.Text, lb5.Text, lb6.Text, lb7.Text, lb8.Text, lb9.Text, lb10.Text };
                dt.Rows.Add(objArr);
            }
            #endregion
            #region Eliminacion
            #region Definar DataTable
            DataTable dt_anulaciones = new DataTable();
            dt_anulaciones.Columns.Add("ID");
            dt_anulaciones.Columns.Add("NOMBRE");
            dt_anulaciones.Columns.Add("TYPE");
            dt_anulaciones.Columns.Add("MONEDATYPE");
            dt_anulaciones.Columns.Add("TOTALD");
            dt_anulaciones.Columns.Add("SUBTOTAL");
            dt_anulaciones.Columns.Add("IMPUESTO");
            dt_anulaciones.Columns.Add("TOTAL");
            dt_anulaciones.Columns.Add("DFID");
            dt_anulaciones.Columns.Add("COSTOID");
            dt_anulaciones.Clear();
            #endregion
            #region Obtener todos los Agregados al Grid Eliminacion Previamente
            foreach (GridViewRow row in gv_detalle_anular.Rows)
            {
                lb1 = (Label)row.FindControl("lb_codigo2");
                lb2 = (Label)row.FindControl("lb_rubro2");
                lb3 = (Label)row.FindControl("lb_tipo2");
                lb4 = (Label)row.FindControl("lb_monedatipo2");
                lb5 = (Label)row.FindControl("lb_totaldolares2");
                lb6 = (Label)row.FindControl("lb_subtotal2");
                lb7 = (Label)row.FindControl("lb_impuesto2");
                lb8 = (Label)row.FindControl("lb_total2");
                lb9 = (Label)row.FindControl("lb_dfid2");
                lb10 = (Label)row.FindControl("lb_costoid2");
                object[] objArr = { lb1.Text, lb2.Text, lb3.Text, lb4.Text, lb5.Text, lb6.Text, lb7.Text, lb8.Text, lb9.Text, lb10.Text };
                dt_anulaciones.Rows.Add(objArr);
            }
            #endregion
            #region Agregar al grid de eliminacion el que voy a Eliminar
            int indice_anular = 0;
            int ban_anular = 0;
            foreach (GridViewRow row in gv_detalle.Rows)
            {
                if (indice_anular == e.RowIndex)
                {
                    lb1 = (Label)row.FindControl("lb_codigo");
                    lb2 = (Label)row.FindControl("lb_rubro");
                    lb3 = (Label)row.FindControl("lb_tipo");
                    lb4 = (Label)row.FindControl("lb_monedatipo");
                    lb5 = (Label)row.FindControl("lb_totaldolares");
                    lb6 = (Label)row.FindControl("lb_subtotal");
                    lb7 = (Label)row.FindControl("lb_impuesto");
                    lb8 = (Label)row.FindControl("lb_total");
                    lb9 = (Label)row.FindControl("lb_dfid");
                    lb10 = (Label)row.FindControl("lb_costoid");
                    object[] objArr = { lb1.Text, lb2.Text, lb3.Text, lb4.Text, lb5.Text, lb6.Text, lb7.Text, lb8.Text, lb9.Text, lb10.Text };
                    bool Valida_Restriccion = Validar_Restricciones("gv_detalle_RowDeleting");
                    if (Valida_Restriccion == true)
                    {
                        if (lb10.Text != "0")
                        {
                            ban_anular = -100;
                        }
                        else
                        {
                            dt_anulaciones.Rows.Add(objArr);
                            ban_anular = 1;
                        }

                    }
                    else
                    {
                        dt_anulaciones.Rows.Add(objArr);
                        ban_anular = 1;
                    }
                }
                indice_anular++;
            }
            gv_detalle_anular.DataSource = dt_anulaciones;
            gv_detalle_anular.DataBind();
            #endregion
            //int Id_Costo = 0;
            //Id_Costo = int.Parse(gv_detalle.Rows[e.RowIndex].Cells[5].Text.ToString());
            if (ban_anular == -100)
            {
                WebMsgBox.Show("No se pueden eliminar rubros provisionados automaticamente, si desea eliminar o modificar rubros porfavor solicitelo al personal de Trafico");
                return;
            }
            else if (ban_anular == 1)
            {
                dt.Rows[e.RowIndex].Delete();
            }
            gv_detalle.DataSource = dt;
            gv_detalle.DataBind();
            #endregion
        }
        protected void Page_PreRender(object sender, EventArgs e)
        {
            //if (Page.IsPostBack)
            //{
            //    #region ReCalcular el Total
            //    decimal total = 0;
            //    Label lb1;
            //    foreach (GridViewRow row in gv_detalle.Rows)
            //    {
            //        lb1 = (Label)row.FindControl("lb_total");

            //        total += decimal.Parse(lb1.Text);
            //    }
            //    total = Math.Round(total, 2);
            //    tb_valor.Text = total.ToString();
            //    #endregion
            //    int tipo_contabilidad = int.Parse(Session["Contabilidad"].ToString());
            //    if (tipo_contabilidad == 1)
            //    {
            //        user = (UsuarioBean)Session["usuario"];
            //        decimal valor = decimal.Parse(tb_valor.Text);
            //        decimal noafecto = decimal.Parse(tb_noafecto.Text);
            //        decimal afecto = (valor - noafecto);
            //        decimal impuesto = Math.Round(((afecto / (user.pais.Impuesto + 1)) * user.pais.Impuesto), 2);
            //        afecto -= impuesto;
            //        //Eliminar Comas
            //        tb_afecto.Text = afecto.ToString("#,#.00#;(#,#.00#)");
            //        tb_iva.Text = impuesto.ToString("#,#.00#;(#,#.00#)");
            //        /*
            //        tb_afecto.Text = afecto.ToString();
            //        tb_iva.Text = impuesto.ToString();
            //        */
            //    }
            //    else if (tipo_contabilidad == 2)
            //    {
            //        tb_noafecto.Text = tb_valor.Text;
            //        tb_afecto.Text = "0.00";
            //        tb_iva.Text = "0.00";
            //    }
            //}
        }
        protected void gv_detalle_RowCreated(object sender, GridViewRowEventArgs e)
        {
            //if (e.Row.Cells.Count > 1)
            //e.Row.Cells[0].Visible = false;

        }
        protected void lb_servicio_SelectedIndexChanged(object sender, EventArgs e)
        {
            int servID = int.Parse(lb_servicio.SelectedValue);

            ArrayList rubros = (ArrayList)DB.getAllRubrosXPaisXServ(user.PaisID, servID, "");
            RE_GenericBean rubbean = null;
            ListItem item = null;
            lb_rubro.Items.Clear();
            for (int a = 0; a < rubros.Count; a++)
            {
                rubbean = (RE_GenericBean)rubros[a];
                item = new ListItem(rubbean.strC1, rubbean.intC1.ToString());
                lb_rubro.Items.Add(item);
            }
            btn_rubro_ModalPopupExtender.Show();
        }
        protected void buscar_rubro_Click(object sender, EventArgs e)
        {
            if (lb_servicio.Items.Count <= 0)
            {
                WebMsgBox.Show("No se existen Tipos de Servicio disponibles");
                return;
            }
            if (lb_rubro.Items.Count <= 0)
            {
                WebMsgBox.Show("No existen Rubros configurados");
                return;
            }
            if (tb_monto.Text == "0.00")
            {
                WebMsgBox.Show("No se puede agregar un rubro con valor cero");
                btn_rubro_ModalPopupExtender.Show();
                return;
            }
            Label1.Text = "1";
            user = (UsuarioBean)Session["usuario"];
            DataTable dt = new DataTable();
            dt.Columns.Add("ID");
            dt.Columns.Add("NOMBRE");
            dt.Columns.Add("TYPE");
            dt.Columns.Add("MONEDATYPE");
            dt.Columns.Add("TOTALD");
            dt.Columns.Add("SUBTOTAL");
            dt.Columns.Add("IMPUESTO");
            dt.Columns.Add("TOTAL");
            dt.Columns.Add("DFID");
            dt.Columns.Add("COSTOID");

            //dt.Columns.Add("Codigo");
            //dt.Columns.Add("Rubro");
            //dt.Columns.Add("Tipo");
            //dt.Columns.Add("Moneda");
            //dt.Columns.Add("Equivalente");
            //dt.Columns.Add("Subtotal");
            //dt.Columns.Add("Impuesto");
            //dt.Columns.Add("Total");
            //dt.Columns.Add("DFID");
            //dt.Columns.Add("COSTOID");

            dt.Clear();
            Label lb1, lb2, lb3, lb4, lb5, lb6, lb7, lb8, lb9, lb10;
            foreach (GridViewRow row in gv_detalle.Rows)
            {
                lb1 = (Label)row.FindControl("lb_codigo");
                lb2 = (Label)row.FindControl("lb_rubro");
                lb3 = (Label)row.FindControl("lb_tipo");
                lb4 = (Label)row.FindControl("lb_subtotal");
                lb5 = (Label)row.FindControl("lb_impuesto");
                lb6 = (Label)row.FindControl("lb_total");
                lb7 = (Label)row.FindControl("lb_totaldolares");
                lb8 = (Label)row.FindControl("lb_monedatipo");
                lb9 = (Label)row.FindControl("lb_dfid");
                lb10 = (Label)row.FindControl("lb_costoid");
                object[] objArr = { lb1.Text, lb2.Text, lb3.Text, lb8.Text, lb7.Text, lb4.Text, lb5.Text, lb6.Text, lb9.Text, lb10.Text };
                dt.Rows.Add(objArr);
            }

            //se jala el rubro y sus configuraciones
            Rubros rubro = new Rubros();
            rubro.rubroID = long.Parse(lb_rubro.SelectedValue);
            rubro.rubroName = lb_rubro.SelectedItem.Text;
            rubro.rubroMoneda = long.Parse(lb_moneda2.SelectedValue);

            //DEFINIR TIPO DE SERVICIO
            #region Backup
            //if (lb_servicio.SelectedItem.Text.Equals("FCL")) { rubro.rubtoType = "FCL"; }
            //if (lb_servicio.SelectedItem.Text.Equals("LCL")) { rubro.rubtoType = "LCL"; }
            //if (lb_servicio.SelectedItem.Text.Equals("AEREO")) { rubro.rubtoType = "AEREO"; }
            //if (lb_servicio.SelectedItem.Text.Equals("APL")) { rubro.rubtoType = "APL"; }
            //if (lb_servicio.SelectedItem.Text.Equals("TRANSPORTE T")) { rubro.rubtoType = "TRANSPORTE T"; }
            //if (lb_servicio.SelectedItem.Text.Equals("SEGUROS")) { rubro.rubtoType = "SEGUROS"; }
            //if (lb_servicio.SelectedItem.Text.Equals("PUERTOS")) { rubro.rubtoType = "PUERTOS"; }
            //if (lb_servicio.SelectedItem.Text.Equals("APL LOGISTICS")) { rubro.rubtoType = "APL LOGISTICS"; }
            //if (lb_servicio.SelectedItem.Text.Equals("ADUANAS")) { rubro.rubtoType = "ADUANAS"; }
            //if (lb_servicio.SelectedItem.Text.Equals("ALMACENADORA")) { rubro.rubtoType = "ALMACENADORA"; }
            //if (lb_servicio.SelectedItem.Text.Equals("INSPECTOR")) { rubro.rubtoType = "INSPECTOR"; }
            //if (lb_servicio.SelectedItem.Text.Equals("PO BOX")) { rubro.rubtoType = "PO BOX"; }
            //if (lb_servicio.SelectedItem.Text.Equals("ADMINISTRATIVO")) { rubro.rubtoType = "ADMINISTRATIVO"; }
            //if (lb_servicio.SelectedItem.Text.Equals("TERCEROS")) { rubro.rubtoType = "TERCEROS"; }
            //if (lb_servicio.SelectedItem.Text.Equals("INTERMODAL")) { rubro.rubtoType = "INTERMODAL"; }
            #endregion
            rubro.rubtoType = lb_servicio.SelectedItem.Text;


            rubro.rubroTot = double.Parse(tb_monto.Text);
            Rubros rubtemp = (Rubros)rubro;
            rubtemp = (Rubros)DB.ExistRubroPais(rubro, user.PaisID);
            if (rubtemp == null)
            {
                WebMsgBox.Show("Error, El HBL no tiene todos los rubros registrados en contabilidad para este pais, por favor pongase en contacto con el departamento de contabilidad para agregarlos.");
                return;
            }
            #region Backup
            //int tipo_contabilidad = int.Parse(Session["Contabilidad"].ToString());
            ////// Si la moneda que estoy agregando es diferente a la que estoy facturando
            //if (!lb_moneda.SelectedValue.Equals("8"))
            //{
            //    if (!lb_moneda2.SelectedValue.Equals("8"))
            //    {
            //        //rubro.rubroTot = Math.Round((double.Parse(tb_monto.Text) / (double)user.pais.TipoCambio), 2);
            //        rubtemp.rubroTot = Math.Round(double.Parse(tb_monto.Text), 2);
            //    }
            //    else
            //    {
            //        rubtemp.rubroTot = Math.Round((double.Parse(tb_monto.Text) * (double)user.pais.TipoCambio), 2);
            //    }
            //}
            //else
            //{
            //    if (lb_moneda2.SelectedValue.Equals("8"))
            //    {
            //        rubtemp.rubroTot = double.Parse(tb_monto.Text);
            //    }
            //    else
            //    {
            //        rubtemp.rubroTot = Math.Round((double.Parse(tb_monto.Text) / (double)user.pais.TipoCambio), 2);
            //    }
            //}

            //if ((rubtemp.CobIva == 1) && (tipo_contabilidad != 2) && (!lb_contribuyente.SelectedValue.Equals("1")))
            //{
            //    if (rubtemp.IvaInc == 1)
            //    {
            //        rubtemp.rubroSubTot = Math.Round(rubtemp.rubroTot * (double)(1 / (1 + user.pais.Impuesto)), 2);
            //        rubtemp.rubroImpuesto = Math.Round(rubtemp.rubroTot - rubtemp.rubroSubTot, 2);
            //    }
            //    else
            //    {
            //        rubtemp.rubroImpuesto = Math.Round(rubtemp.rubroTot * (double)user.pais.Impuesto, 2);
            //        rubtemp.rubroSubTot = Math.Round(rubtemp.rubroTot, 2);
            //        rubtemp.rubroTot = Math.Round(rubtemp.rubroSubTot + rubtemp.rubroImpuesto, 2);
            //    }
            //}
            //else
            //{
            //    rubtemp.rubroSubTot = rubtemp.rubroTot;
            //}
            //decimal tipoCambio = user.pais.TipoCambio;
            //double totalD = rubtemp.rubroTot;

            //if ((rubtemp.rubroMoneda == 8) && (lb_moneda.SelectedValue.Equals("8")))
            //{
            //    totalD = Math.Round(rubtemp.rubroTot * (double)tipoCambio, 2);
            //}
            //else if ((rubtemp.rubroMoneda == 8) && (!lb_moneda.SelectedValue.Equals("8")))
            //{
            //    rubtemp.rubroTot = Math.Round(rubtemp.rubroTot * (double)tipoCambio, 2);
            //}
            //else if ((rubtemp.rubroMoneda != 8) && (lb_moneda.SelectedValue.Equals("8")))
            //{
            //    rubtemp.rubroTot = Math.Round(rubtemp.rubroTot / (double)tipoCambio, 2);
            //}
            //else if ((rubtemp.rubroMoneda != 8) && (!lb_moneda.SelectedValue.Equals("8")))
            //{
            //    totalD = Math.Round(rubtemp.rubroTot / (double)tipoCambio, 2);
            //}

            //string simbolomoneda = "";
            //if (rubtemp.rubroMoneda == 1) simbolomoneda = "GTQ";
            //if (rubtemp.rubroMoneda == 2) simbolomoneda = "SVC";
            //if (rubtemp.rubroMoneda == 3) simbolomoneda = "HNL";
            //if (rubtemp.rubroMoneda == 4) simbolomoneda = "NIC";
            //if (rubtemp.rubroMoneda == 5) simbolomoneda = "CRC";
            //if (rubtemp.rubroMoneda == 6) simbolomoneda = "PAB";
            //if (rubtemp.rubroMoneda == 7) simbolomoneda = "BZD";
            //if (rubtemp.rubroMoneda == 8) simbolomoneda = "USD";
            //object[] obj = { rubtemp.rubroID.ToString(), rubtemp.rubroName.ToString(), rubtemp.rubtoType.ToString(), simbolomoneda, totalD.ToString("#,#.00#;(#,#.00#)"), rubtemp.rubroSubTot.ToString("#,#.00#;(#,#.00#)"), rubtemp.rubroImpuesto.ToString("#,#.00#;(#,#.00#)"), rubtemp.rubroTot.ToString("#,#.00#;(#,#.00#)"), "0", "0" };
            #endregion

            int tipo_contabilidad = int.Parse(Session["Contabilidad"].ToString());
            decimal tipoCambio = user.pais.TipoCambio;
            double totalD = rubtemp.rubroTot;
            if ((rubtemp.CobIva == 1) && (tipo_contabilidad != 2) && (!lb_contribuyente.SelectedValue.Equals("1")))//si debe cobrar iva y el rubro no esta en dolares y no es excento
            {
                #region Contribuyente
                if (rubtemp.IvaInc == 1)
                {
                    #region Iva Incluido
                    if ((rubtemp.rubroMoneda == 8) && (lb_moneda.SelectedValue.Equals("8")))
                    {
                        rubtemp.rubroSubTot = Math.Round(rubtemp.rubroTot * (double)(1 / (1 + user.pais.Impuesto)), 2);
                        rubtemp.rubroImpuesto = Math.Round(rubtemp.rubroTot - rubtemp.rubroSubTot, 2);
                        totalD = Math.Round(rubtemp.rubroTot * (double)tipoCambio, 2);
                    }
                    else if ((rubtemp.rubroMoneda == 8) && (!lb_moneda.SelectedValue.Equals("8")))
                    {
                        totalD = Math.Round(rubtemp.rubroTot * (double)tipoCambio, 2);
                        rubtemp.rubroSubTot = Math.Round(totalD * (double)(1 / (1 + user.pais.Impuesto)), 2);
                        rubtemp.rubroImpuesto = Math.Round(totalD - rubtemp.rubroSubTot, 2);
                        rubtemp.rubroTot = Math.Round(rubtemp.rubroSubTot + rubtemp.rubroImpuesto, 2);
                        totalD = Math.Round(rubtemp.rubroTot / (double)tipoCambio, 2);
                    }
                    else if ((rubtemp.rubroMoneda != 8) && (lb_moneda.SelectedValue.Equals("8")))
                    {
                        totalD = Math.Round(rubtemp.rubroTot / (double)tipoCambio, 2);
                        rubtemp.rubroSubTot = Math.Round(totalD * (double)(1 / (1 + user.pais.Impuesto)), 2);
                        rubtemp.rubroImpuesto = Math.Round(totalD - rubtemp.rubroSubTot, 2);
                        rubtemp.rubroTot = Math.Round(rubtemp.rubroSubTot + rubtemp.rubroImpuesto, 2);
                        totalD = Math.Round(rubtemp.rubroTot * (double)tipoCambio, 2);
                    }
                    else if ((rubtemp.rubroMoneda != 8) && (!lb_moneda.SelectedValue.Equals("8")))
                    {
                        rubtemp.rubroSubTot = Math.Round(rubtemp.rubroTot * (double)(1 / (1 + user.pais.Impuesto)), 2);
                        rubtemp.rubroImpuesto = Math.Round(rubtemp.rubroTot - rubtemp.rubroSubTot, 2);
                        totalD = Math.Round(rubtemp.rubroTot / (double)tipoCambio, 2);
                    }
                    #endregion
                }
                else
                {
                    #region No Incluye IVA
                    if ((rubtemp.rubroMoneda == 8) && (lb_moneda.SelectedValue.Equals("8")))
                    {
                        rubtemp.rubroImpuesto = Math.Round(rubtemp.rubroTot * (double)user.pais.Impuesto, 2);
                        rubtemp.rubroSubTot = Math.Round(rubtemp.rubroTot, 2);
                        rubtemp.rubroTot = Math.Round(rubtemp.rubroSubTot + rubtemp.rubroImpuesto, 2);
                        totalD = Math.Round(rubtemp.rubroTot * (double)tipoCambio, 2);
                    }
                    else if ((rubtemp.rubroMoneda == 8) && (!lb_moneda.SelectedValue.Equals("8")))
                    {
                        totalD = Math.Round(rubtemp.rubroTot * (double)tipoCambio, 2);
                        rubtemp.rubroImpuesto = Math.Round(totalD * (double)user.pais.Impuesto, 2);
                        rubtemp.rubroSubTot = Math.Round(totalD, 2);
                        rubtemp.rubroTot = Math.Round(rubtemp.rubroSubTot + rubtemp.rubroImpuesto, 2);
                        totalD = Math.Round(rubtemp.rubroTot / (double)tipoCambio, 2);
                    }
                    else if ((rubtemp.rubroMoneda != 8) && (lb_moneda.SelectedValue.Equals("8")))
                    {
                        totalD = Math.Round(rubtemp.rubroTot / (double)tipoCambio, 2);
                        rubtemp.rubroImpuesto = Math.Round(totalD * (double)user.pais.Impuesto, 2);
                        rubtemp.rubroSubTot = Math.Round(totalD, 2);
                        rubtemp.rubroTot = Math.Round(rubtemp.rubroSubTot + rubtemp.rubroImpuesto, 2);
                        totalD = Math.Round(rubtemp.rubroTot * (double)tipoCambio, 2);
                    }
                    else if ((rubtemp.rubroMoneda != 8) && (!lb_moneda.SelectedValue.Equals("8")))
                    {
                        rubtemp.rubroImpuesto = Math.Round(rubtemp.rubroTot * (double)user.pais.Impuesto, 2);
                        rubtemp.rubroSubTot = Math.Round(rubtemp.rubroTot, 2);
                        rubtemp.rubroTot = Math.Round(rubtemp.rubroSubTot + rubtemp.rubroImpuesto, 2);
                        totalD = Math.Round(rubtemp.rubroTot / (double)tipoCambio, 2);
                    }
                    #endregion
                }
                #endregion
            }
            else
            {
                #region Excento
                if ((rubtemp.rubroMoneda == 8) && (lb_moneda.SelectedValue.Equals("8")))
                {
                    totalD = Math.Round(rubtemp.rubroTot * (double)tipoCambio, 2);
                    rubtemp.rubroSubTot = rubtemp.rubroTot;
                }
                else if ((rubtemp.rubroMoneda == 8) && (!lb_moneda.SelectedValue.Equals("8")))
                {
                    rubtemp.rubroTot = Math.Round(rubtemp.rubroTot * (double)tipoCambio, 2);
                    rubtemp.rubroSubTot = rubtemp.rubroTot;
                    totalD = Math.Round(rubtemp.rubroTot / (double)tipoCambio, 2);
                }
                else if ((rubtemp.rubroMoneda != 8) && (lb_moneda.SelectedValue.Equals("8")))
                {
                    rubtemp.rubroTot = Math.Round(rubtemp.rubroTot / (double)tipoCambio, 2);
                    rubtemp.rubroSubTot = rubtemp.rubroTot;
                    totalD = Math.Round(rubtemp.rubroTot * (double)tipoCambio, 2);
                }
                else if ((rubtemp.rubroMoneda != 8) && (!lb_moneda.SelectedValue.Equals("8")))
                {
                    totalD = Math.Round(rubtemp.rubroTot / (double)tipoCambio, 2);
                    rubtemp.rubroSubTot = rubtemp.rubroTot;
                }
                #endregion
            }

            object[] obj = { rubtemp.rubroID.ToString(), rubtemp.rubroName.ToString(), rubtemp.rubtoType.ToString(), Utility.TraducirMonedaInt(Convert.ToInt32(lb_moneda.SelectedValue)), totalD.ToString("#,#.00#;(#,#.00#)"), rubtemp.rubroSubTot.ToString("#,#.00#;(#,#.00#)"), rubtemp.rubroImpuesto.ToString("#,#.00#;(#,#.00#)"), rubtemp.rubroTot.ToString("#,#.00#;(#,#.00#)"), "0", "0" };
            dt.Rows.Add(obj);
            if (user.contaID == 2)
            {
                //gv_detalle.Columns[4].HeaderText = "Total en Dolares";
                //gv_detalle.Columns[5].HeaderText = "Equivalente Local";
            }
            else
            {
                //gv_detalle.Columns[4].HeaderText = "Total local";
                //gv_detalle.Columns[5].HeaderText = "Equivalente en Dolares";
            }


            //gv_detalle.Columns[0].HeaderText = "Codigo";
            //gv_detalle.Columns[2].HeaderText = "Rubro";
            //gv_detalle.Columns[3].HeaderText = "Tipo";
            //gv_detalle.Columns[4].HeaderText = "Moneda";
            //gv_detalle.Columns[5].HeaderText = "Equivalente";
            //gv_detalle.Columns[6].HeaderText = "Subtotal";
            //gv_detalle.Columns[7].HeaderText = "Impuesto";
            //gv_detalle.Columns[8].HeaderText = "Total";

            gv_detalle.DataSource = dt;
            gv_detalle.DataBind();
            #region Validar Sender
            if (sender is Button)
            {
                Button btn_aux = (Button)sender;
                if (btn_aux.ID == "btn_siguiente_rubro")
                {
                    btn_rubro_ModalPopupExtender.Show();
                }
            }
            #endregion
            tb_monto.Text = "0.00";
        }
        protected void bt_agregar_Click(object sender, EventArgs e)
        {

        }
        protected bool Validar_Restricciones(string sender)
        {
            bool resultado = false;
            bool paiR = DB.Validar_Pais_Restringido(user);
            if (paiR == true)
            {
                #region ELIMINACION DE RUBROS TRAFICOS
                if (DB.Validar_Restriccion_Activa(user, 5, 20) == true)
                {
                    if (sender == "gv_detalle_RowDeleting")
                    {
                        resultado = true;
                    }
                }
                #endregion
                #region ELIMINAR Y AGREGAR RUBROS
                if (DB.Validar_Restriccion_Activa(user, 5, 21) == true)
                {
                    if (sender == "Load")
                    {
                        if (lbl_tipoOperacionID.Text != "8")
                        {
                            gv_detalle.Columns[0].Visible = false;
                            bt_agregar.Visible = false;
                        }
                    }
                }
                #endregion
            }
            else
            {
                resultado = true;
                return resultado;
            }
            return resultado;
        }

        protected void bt_ImprimirOC_Click(object sender, EventArgs e)
        {
            string script = "";
            String csname1 = "PopupScript";
            Type cstype = this.GetType();
            // Get a ClientScriptManager reference from the Page class.
            ClientScriptManager cs2 = Page.ClientScript;

            /*if (user.PaisID == 3)
            {
                script = "window.open('../invoice/template_retencion.aspx?id=" + lb_provID.Text.Trim() + "&tipo=5','Imprimir','menubar=1,resizable=1,scrollbars=1,toolbar=1,width=750');";
            }
            else
            {*/
                script = "window.open('../invoice/template_oc.aspx?id=" + lb_provID.Text.Trim() + "&tipo=5','Imprimir','menubar=1,resizable=1,scrollbars=1,toolbar=1,width=750');";
            //}
            
            if (!cs2.IsStartupScriptRegistered(cstype, csname1))
            {
                cs2.RegisterStartupScript(cstype, csname1, script, true);
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
        protected void tb_fecha_TextChanged(object sender, EventArgs e)
        {
            if (tb_fecha.Text.Trim() != "")
            {
                string Fecha_Factura = "";
                Fecha_Factura = DB.DateFormat(tb_fecha.Text.Trim());
                DateTime aux_fecha_emision = DateTime.Parse(Fecha_Factura);
                double dias_credito = DB.getFechaMaxPago(int.Parse(tb_codigo_proveedor.Text), int.Parse(drp_tipo_persona.SelectedValue), user);
                DateTime aux_fecha_pago = aux_fecha_emision.AddDays(dias_credito);
                string fecha_pago_formateada = aux_fecha_pago.ToString();
                lb_fechapago.Text = DateTime.Parse(fecha_pago_formateada).ToString("MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            }
            else
            {
                lb_fechapago.Text = "";
            }
        }
        protected void gv_detalle_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (Request.QueryString["provID"] != null)
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    Button bt = (Button)e.Row.Cells[0].Controls[0];
                    bt.Enabled = false;
                }
            }
        }
}
