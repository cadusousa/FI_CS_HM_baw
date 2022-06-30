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

public partial class invoice_notadebito : System.Web.UI.Page
{
    UsuarioBean user;
    DataTable dt1;
    int ban_sucursales = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["usuario"] == null)
        {
            Response.Redirect("../default.aspx");
        }
        int tipo_contabilidad = int.Parse(Session["Contabilidad"].ToString());
        user = (UsuarioBean)Session["usuario"];        
        if (!IsPostBack)
        {
            lb_fecha_hora.Text = DB.getDateTimeNow();
            obtengo_listas(tipo_contabilidad);
            lb_trans_SelectedIndexChanged(sender, e);        
        }
        
        #region Habilitar TextBox por Sucursales
        ban_sucursales = DB.getSucursalesSinDocumentos(user.SucursalID);
        if (ban_sucursales > 0)
        {
            tb_hbl.ReadOnly = false;
            tb_mbl.ReadOnly = false;
            tb_routing.ReadOnly = false;
            tb_contenedor.ReadOnly = false;
        }
        else
        {
            tb_hbl.ReadOnly = true;
            tb_mbl.ReadOnly = true;
            tb_routing.ReadOnly = true;
            tb_contenedor.ReadOnly = true;
        }
        #endregion
        if (!user.Aplicaciones.Contains("6"))
            Response.Redirect("index.aspx");
        int permiso = int.Parse(user.Aplicaciones["6"].ToString());
        if (!((permiso & 16384) == 16384))
            Response.Redirect("index.aspx");

        if (!Page.IsPostBack)
        {
            int moneda_inicial = 0;
            moneda_inicial = DB.Get_Moneda_Inicial_By_Empresa(user, 4);
            if (tipo_contabilidad == 2)
            {
                lb_moneda.SelectedValue = moneda_inicial.ToString();
                lb_moneda2.SelectedValue = moneda_inicial.ToString();
                lb_moneda.Enabled = false;
            }
            else
            {
                lb_moneda.SelectedValue = moneda_inicial.ToString();
                lb_moneda2.SelectedValue = moneda_inicial.ToString();
                lb_moneda.Enabled = false;

            }
        }
        double cliID = 0;
        ArrayList clientearr = null; //el maximo es de 1 ya que solo 1 cliente debe haber
        RE_GenericBean clienteBean = null;//tendra los datos del cliente;
        string criterio = "";
        if (Request.QueryString["cliID"] != null)
        {
            cliID = double.Parse(Request.QueryString["cliID"].Trim());
            //Obtengo los datos del cliente
            criterio = "id_cliente=" + cliID;
            clientearr = (ArrayList)DB.getClientes(criterio, user, "");
            if ((clientearr != null) && (clientearr.Count > 0))
            {
                // si entro aqui es porque encontre datos del cliente
                clienteBean = (RE_GenericBean)clientearr[0];
                tbCliCod.Text = clienteBean.douC1.ToString();
                tb_nombre.Text = clienteBean.strC2;
                tb_direccion.Text = clienteBean.strC4;
            }
        }
        cargo_datos_BL();
        if (lbl_hbl_eliminado.Text != "")
        {
            tb_hbl.Text = "";
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
            item = new ListItem("Seleccione...", "0");
            lb_trans.Items.Add(item);
            arr = (ArrayList)DB.getOpcionesMatriz("tbl_tipo_transaccion where ttt_template='notadebito.aspx' and ttt_id<>6 order by ttt_nombre");
            foreach (RE_GenericBean rgb in arr)
            {
                item = new ListItem(rgb.strC1, rgb.intC1.ToString());
                lb_trans.Items.Add(item);
            }
            arr = null;
            arr = (ArrayList)DB.getMonedasbyPais(user.PaisID, user.contaID);
            foreach (RE_GenericBean rgb in arr)
            {
                item = new ListItem(rgb.strC1, rgb.intC1.ToString());
                lb_moneda.Items.Add(item);
                lb_moneda2.Items.Add(item);
            }
            
            arr = (ArrayList)DB.Get_Series_By_Monedas_Conta(4, user, 1);
            item = new ListItem("Seleccione...", "0");
            lb_serie_factura.Items.Clear();
            lb_serie_factura.Items.Add(item);
            foreach (RE_GenericBean Bean_Serie in arr)
            {
                item = new ListItem(Bean_Serie.strC1, Bean_Serie.strC2);
                lb_serie_factura.Items.Add(item);
            }

            ////SERIES DE FACTURA ELECTRONICA
            arr = null;
            arr = (ArrayList)DB.Get_Series_Electronicas(1, user, 1);
            item = new ListItem("Seleccione...", "");
            ddlSerieFacBusqueda.Items.Clear();
            ddlSerieFacBusqueda.Items.Add(item);
            foreach (RE_GenericBean Bean_Serie2 in arr)
            {
                item = new ListItem(Bean_Serie2.strC1, Bean_Serie2.strC2);
                ddlSerieFacBusqueda.Items.Add(item);
            }
            /////////

            arr = null;
            if (DB.Validar_Restriccion_Activa(user, 26, 24) == true)
            {
                arr = (ArrayList)DB.Get_Tipos_Servicio_Permitidos(user.PaisID, user.contaID, 26, user.SucursalID);
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

    protected void Button1_Click(object sender, EventArgs e)
    {
        if ((tb_monto.Text.Trim() == "") || (tb_monto.Text.Trim() == "0") || (tb_monto.Text.Trim() == "0.00"))
        {
            WebMsgBox.Show("Por favor ingrese el Total del Rubro");
            return;
        }
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
        if (tbCliCod.Text.Trim().Equals(""))
        {
            WebMsgBox.Show("Debe Ingresar el Codigo del Proveedor");
            return;
        }
        DataTable dt = new DataTable();

        dt.Columns.Add("ID");
        dt.Columns.Add("NOMBRE");
        dt.Columns.Add("TYPE");
        dt.Columns.Add("MONEDATYPE");
        dt.Columns.Add("SUBTOTAL");
        dt.Columns.Add("IMPUESTO");
        dt.Columns.Add("TOTAL");
        dt.Columns.Add("TOTALD");


        dt.Clear();
        Label lb1, lb2, lb3, lb4, lb5, lb6, lb7, lb8;
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
            object[] objArr = { lb1.Text, lb2.Text, lb3.Text, lb8.Text, lb4.Text, lb5.Text, lb6.Text, lb7.Text };
            dt.Rows.Add(objArr);
        }

        
        Rubros rubro = new Rubros();
        rubro.rubroID = long.Parse(lb_rubro.SelectedValue);
        rubro.rubroName = lb_rubro.SelectedItem.Text;
        int cliID = int.Parse(tbCliCod.Text);
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
        int tipo_contabilidad = int.Parse(Session["Contabilidad"].ToString());

        if ((rubtemp.CobIva == 1) && (tipo_contabilidad != 2) && (!lb_contribuyente.SelectedValue.Equals("1")))
        {
            if (rubtemp.IvaInc == 1)
            {
                rubtemp.rubroSubTot = Math.Round(rubtemp.rubroTot * (double)(1 / (1 + user.pais.Impuesto)), 2);
                rubtemp.rubroImpuesto = Math.Round(rubtemp.rubroTot - rubtemp.rubroSubTot, 2);
            }
            else
            {
                rubtemp.rubroImpuesto = Math.Round(rubtemp.rubroTot * (double)user.pais.Impuesto, 2);
                rubtemp.rubroSubTot = Math.Round(rubtemp.rubroTot, 2);
                rubtemp.rubroTot = Math.Round(rubtemp.rubroSubTot + rubtemp.rubroImpuesto, 2);
            }
        }
        else
        {
            rubtemp.rubroImpuesto = 0;
            rubtemp.rubroSubTot = rubtemp.rubroTot;
        }
        decimal tipoCambio = user.pais.TipoCambio;
        double totalD = rubtemp.rubroTot;
        if ((rubtemp.rubroMoneda == 8) && (lb_moneda.SelectedValue.Equals("8")))
        {
            totalD = Math.Round(rubtemp.rubroTot * (double)tipoCambio, 2);
        }
        else if ((rubtemp.rubroMoneda == 8) && (!lb_moneda.SelectedValue.Equals("8")))
        {
            rubtemp.rubroImpuesto = Math.Round(rubtemp.rubroImpuesto * (double)tipoCambio, 2);
            rubtemp.rubroSubTot = Math.Round(rubtemp.rubroSubTot * (double)tipoCambio, 2);
            rubtemp.rubroTot = Math.Round(rubtemp.rubroSubTot + rubtemp.rubroImpuesto, 2);
        }
        else if ((rubtemp.rubroMoneda != 8) && (lb_moneda.SelectedValue.Equals("8")))
        {
            rubtemp.rubroImpuesto = Math.Round(rubtemp.rubroImpuesto / (double)tipoCambio, 2);
            rubtemp.rubroSubTot = Math.Round(rubtemp.rubroSubTot / (double)tipoCambio, 2);
            rubtemp.rubroTot = Math.Round(rubtemp.rubroSubTot + rubtemp.rubroImpuesto, 2);
        }
        else if ((rubtemp.rubroMoneda != 8) && (!lb_moneda.SelectedValue.Equals("8")))
        {
            totalD = Math.Round(rubtemp.rubroTot / (double)tipoCambio, 2);
        }

        
        object[] obj = { rubtemp.rubroID.ToString(), rubtemp.rubroName.ToString(), rubtemp.rubtoType.ToString(), Utility.TraducirMonedaInt(Convert.ToInt32(lb_moneda.SelectedValue)), rubtemp.rubroSubTot.ToString("#,#.00#;(#,#.00#)"), rubtemp.rubroImpuesto.ToString("#,#.00#;(#,#.00#)"), rubtemp.rubroTot.ToString("#,#.00#;(#,#.00#)"), totalD.ToString("#,#.00#;(#,#.00#)") };
        dt.Rows.Add(obj);
        
        gv_detalle.DataSource = dt;
        gv_detalle.DataBind();
        tb_monto.Text = "0.00";
    }
    protected void gv_detalle_DataBound(object sender, EventArgs e)
    {
        decimal subtotal = 0;
        decimal impuesto = 0;
        decimal total = 0;
        decimal totalD = 0;

        Label lb1, lb2, lb3, lb4, lb6;
        foreach (GridViewRow row in gv_detalle.Rows)
        {
            lb1 = (Label)row.FindControl("lb_subtotal");
            lb2 = (Label)row.FindControl("lb_impuesto");
            lb3 = (Label)row.FindControl("lb_total");
            lb4 = (Label)row.FindControl("lb_totaldolares");
            lb6 = (Label)row.FindControl("lb_codigo");
            subtotal += Math.Round(decimal.Parse(lb1.Text), 2);
            totalD += Math.Round(decimal.Parse(lb4.Text), 2);

            Rubros rubtemp = new Rubros();
            Rubros rub = new Rubros();
            rub.rubroID = int.Parse(lb6.Text);
            rub.rubroSubTot = double.Parse(lb1.Text);
            rub.rubroImpuesto = double.Parse(lb2.Text);
            rub.rubroTot = double.Parse(lb3.Text);

            rubtemp = (Rubros)DB.ExistRubroPais(rub, user.PaisID);
            if (rubtemp == null)
            {
                WebMsgBox.Show("Error, no tiene todos los rubros registrados en contabilidad para este pais, por favor pongase en contacto con el departamento de contabilidad para agregarlos.");
                return;
            }


            lb1.Text = rubtemp.rubroSubTot.ToString();
            lb2.Text = rubtemp.rubroImpuesto.ToString();
            lb3.Text = rubtemp.rubroTot.ToString();

            impuesto += Math.Round((decimal)rubtemp.rubroImpuesto, 2);
            total += Math.Round((decimal)rubtemp.rubroTot, 2);

        }

        tb_subtotal.Text = subtotal.ToString("#,#.00#;(#,#.00#)");
        tb_impuesto.Text = impuesto.ToString("#,#.00#;(#,#.00#)");
        tb_total.Text = total.ToString("#,#.00#;(#,#.00#)");
        tb_totaldolares.Text = totalD.ToString("#,#.00#;(#,#.00#)");
    }
    protected void bt_Enviar_Click(object sender, EventArgs e)
    {
        int transID = int.Parse(lb_trans.SelectedValue);//tipo de transaccion factura, invoice
        if (transID <= 0)
        {
            WebMsgBox.Show("Debe seleccionar una Transaccion");
            return;
        }
        
        if (lb_serie_factura.Items.Count == 0)
        {
            WebMsgBox.Show("No se puede Guardar una Nota de Debito sin Serie");
            return;
        }
        if ((user.PaisID == 5) && (lbl_tipo_serie.Text == "1"))
        {
            if (tbSerieFacturaRef.Text == "" && tbCorrelativoFacturaRef.Text == "")
            {
                WebMsgBox.Show("Esta es un Nota de Debito Electronica. Debe ingresar la factura a referenciar");
                return;
            }
        }
        if (lb_serie_factura.SelectedValue == "0")
        {
            WebMsgBox.Show("Debe seleccionar la Serie a utilizar");
            return;
        }
        if ((tbCliCod.Text.Trim().Equals("") || (tbCliCod.Text.Trim().Equals("0"))))
        {
            WebMsgBox.Show("Es necesario que indique un proveedor");
            return;
        }
        if ((!tb_contenedor.Text.Trim().Equals("")) && (tb_contenedor.Text.Trim().Length > 3))
        {
            string resultado = "";
            resultado = DB.ValidarContenedor(tb_contenedor.Text.ToUpper());
            if (resultado == "0")
            {
                WebMsgBox.Show("El numero de Contenedor no es valido");
                return;
            }
            if (resultado == "")
            {
                WebMsgBox.Show("Existio un error porque el Numero de Contenedor no es valido");
                return;
            }
        }
        if ((gv_detalle.Rows.Count == 0) || (tb_total.Text.Trim().Equals("")))
        {
            WebMsgBox.Show("Debe ingresar al menos un rubro");
            return;
        }
        if (tb_fecha_libro_compras.Text.Trim().Equals("") && (bool.Parse(Rb_Documento.SelectedValue) == true))
        {
            WebMsgBox.Show("Debe seleccionar la Fecha del Libro de Compras");
            return;
        }

        #region Validar Observaciones de Interompanys
        if (lb_tipopersona.SelectedValue == "10")
        {
            if (tb_observaciones.Text.Trim() == "")
            {
                WebMsgBox.Show("Debe ingresar las Observaciones correspondientes para el Documento de Cobro");
                return;
            }
        }
        #endregion

        int contribuyente = 1;//excento
        int moneda = int.Parse(lb_moneda.SelectedValue);//moneda
        int imp_exp = int.Parse(lb_imp_exp.SelectedValue);
        int tipo_cobro = 1;//prepaid, collect
        if (imp_exp == 1) { tipo_cobro = 1; } else { tipo_cobro = 2; } //si es 1=importacion cobro 1=collect,si es 2=exportacion cobro 2=prepaid
        tipo_cobro = 1;
        int tipo_contabilidad = int.Parse(Session["Contabilidad"].ToString());//fiscal o financiera
        if (tipo_contabilidad == 1) { transID = 1; } else if (tipo_contabilidad == 2) { transID = 7; }//si es la fiscal, halo los datos de la factura, si es financiera halo los datos del invoice
        int servicio = 0; //fcl, lcl, etc

        FacturaBean factfinal = new FacturaBean();
        factfinal.Nit = tb_nit.Text;
        factfinal.Fecha_Hora = lb_fecha_hora.Text;
        factfinal.Nombre = tb_nombre.Text;
        factfinal.Direccion = tb_direccion.Text;
        factfinal.SubTot = double.Parse(tb_total.Text);
        factfinal.Total = double.Parse(tb_total.Text);
        factfinal.TotalDol = double.Parse(tb_totaldolares.Text);
        factfinal.SubTot = double.Parse(tb_subtotal.Text);
        factfinal.Impuesto = double.Parse(tb_impuesto.Text);
        if ((lb_moneda.SelectedValue == "8"))
        {

            factfinal.SubTotequivalente = Math.Round(factfinal.SubTot * (double)user.pais.TipoCambio, 2);
            factfinal.Impuesto_equivalente = Math.Round(factfinal.Impuesto * (double)user.pais.TipoCambio, 2);
        }
        else
        {
            factfinal.SubTotequivalente = Math.Round(factfinal.SubTot / (double)user.pais.TipoCambio, 2);
            factfinal.Impuesto_equivalente = Math.Round(factfinal.Impuesto / (double)user.pais.TipoCambio, 2);
        }
        factfinal.Observaciones = tb_observaciones.Text;
        factfinal.CliID = long.Parse(tbCliCod.Text);
        factfinal.MonedaID = moneda;
        factfinal.TedID = 1;//activo=1 tbl estado documento
        factfinal.UsuID = user.ID;
        factfinal.HBL = tb_hbl.Text;
        factfinal.MBL = tb_mbl.Text;
        factfinal.Contenedor = tb_contenedor.Text;
        factfinal.Routing = tb_routing.Text;
        factfinal.Referencia = tb_referencia.Text;
        factfinal.Serie = lb_serie_factura.SelectedItem.Text;
        DateTime fecha_emision = DateTime.Now;
        factfinal.Fecha_Emision = fecha_emision.ToString();
        factfinal.Fecha_Emision = DB.getDateTimeNow();
        factfinal.Tipo_Persona = int.Parse(lb_tipopersona.SelectedValue);
        factfinal.Fiscal_No_Fiscal = bool.Parse(Rb_Documento.SelectedValue);
        string fecha_libro_compras = tb_fecha_libro_compras.Text;
        ArrayList Arr_Fechas = (ArrayList)DB.Formatear_Fechas(fecha_libro_compras, "");
        fecha_libro_compras = Arr_Fechas[0].ToString();
        factfinal.Fecha_Libro_Compras = fecha_libro_compras;
        factfinal.Tipo_BienServicio = int.Parse(rb_bienserv.SelectedValue);
        factfinal.BlId = int.Parse(lbl_blID.Text);
        factfinal.ttoID = int.Parse(lbl_tipoOperacionID.Text);
        factfinal.tttID = int.Parse(lb_contribuyente.SelectedValue.ToString());
        factfinal.imp_exp = int.Parse(lb_imp_exp.SelectedValue);
        factfinal.IP_Address = Request.ServerVariables["REMOTE_ADDR"].ToString();
        factfinal.Correo_Electronico = lbl_correo_documento_electronico.Text;
        factfinal.Referencia_Correo = tb_referencia_correo.Text.Trim();
        factfinal.Factura_Electronica = int.Parse(lbl_tipo_serie.Text);

        #region Validaciones Facturacion Electronica Costa Rica
        if (((user.PaisID == 5) || (user.PaisID == 21)) && (lbl_tipo_serie.Text == "1"))
        {
            if (factfinal.Tipo_Persona == 10)
            {
                if (drp_tipo_identificacion_cliente.SelectedValue == "0")
                {
                    WebMsgBox.Show("La Nota de Debito no fue guardada, ni procesada por el Ministerio de Hacienda porque el Intercompany.: " + tb_nombre.Text + " con ID.: " + tbCliCod.Text + " no tiene Asignado Tipo de Identificacion Tributaria.");
                    return;
                }
            }
        }
        //Se agregan a la estructura los valores de la factura referenciada
        if ((user.PaisID == 5 || user.PaisID == 21) && (lbl_tipo_serie.Text == "1"))
        {
            factfinal.Factura_Ref_ID = int.Parse(hdIdFacturaRef.Value);
            factfinal.Factura_Ref_Serie = tbSerieFacturaRef.Text;
            factfinal.Factura_Ref_Correlativo = int.Parse(tbCorrelativoFacturaRef.Text);
            factfinal.Factura_Ref_Fecha = tbFechaFacturaRef.Text;
            factfinal.Factura_Ref_Doc = tbDocFacturaRef.Text;
        }
        else
        {
            factfinal.Factura_Ref_ID = 0;
            factfinal.Factura_Ref_Serie = "";
            factfinal.Factura_Ref_Correlativo = 0;
            factfinal.Factura_Ref_Fecha = "";
            factfinal.Factura_Ref_Doc = "";
        }
        #endregion
        bool valida_agente = Validar_Restricciones("Validar_Econocaribe");
        if (valida_agente == true)
        {
            return;
        }
        #region Validacion Intercompanys
        if (factfinal.Tipo_Persona == 10)
        {
            RE_GenericBean Intercompany_Origen = (RE_GenericBean)DB.Get_Intercompany_Data_By_Empresa(user.PaisID);
            if (Intercompany_Origen.intC1.ToString() == factfinal.CliID.ToString())
            {
                WebMsgBox.Show("No se puede Emitir Cobro a la misma Empresa");
                return;
            }
            RE_GenericBean Intercompany_Bean = (RE_GenericBean)DB.Get_Intercompany_Data(Convert.ToInt32(factfinal.CliID));
            if (Intercompany_Bean == null)
            {
                WebMsgBox.Show("Existio un error al Tratar de Obtener la Informacion del Intercompany");
                return;
            }
            int existe_configuracion = 0;
            string sql_existencia = " and tia_pais_origen=" + user.PaisID + " and tia_contabilidad_origen=" + user.contaID + " and tia_moneda_origen=" + factfinal.MonedaID + " and tia_tipo_operacion=" + 2 + " and tia_id_intercompany=" + factfinal.CliID.ToString() + " ";
            existe_configuracion = Contabilizacion_Automatica_CAD.Validar_Existencia_Intercompany_Administrativo(user, sql_existencia);
            if (existe_configuracion == -100)
            {
                WebMsgBox.Show("Existio un Error al Tratar de validar la configuracion del Intercompany");
                return;
            }
            else if (existe_configuracion > 0)
            {
                decimal Tipo_Cambio_Destino = 0;
                Tipo_Cambio_Destino = DB.getTipoCambioHoy(Intercompany_Bean.intC3);
                if (Tipo_Cambio_Destino == 0)
                {
                    PaisBean Pais_Temporal = (PaisBean)DB.getPais(Intercompany_Bean.intC3);
                    WebMsgBox.Show("No Existe Tipo de Cambio para realizar la Automatizacion en Sistema BAW - " + Pais_Temporal.Nombre_Sistema + ", favor notificar al personal de Contabilidad");
                    return;
                }
            }
        }
        #endregion
        //recorro el datagrid para aramar la factura
        Label lb1, lb2, lb3, lb4, lb5, lb6, lb7, lb8;
        Rubros rubro;
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
            rubro.rubroID = long.Parse(lb1.Text); ;
            rubro.rubroName = lb2.Text;
            rubro.rubtoType = lb3.Text;
            rubro.rubroSubTot = double.Parse(lb4.Text);
            rubro.rubroImpuesto = double.Parse(lb5.Text);
            rubro.rubroTot = double.Parse(lb6.Text);
            rubro.rubroTotD = double.Parse(lb7.Text);
            rubro.rubroTypeID = Utility.TraducirServiciotoINT(rubro.rubtoType);
            rubro.rubroMoneda = Utility.TraducirMonedaStr(lb8.Text);

            rubro.rubroTypeID = Utility.TraducirServiciotoINT(rubro.rubtoType);

            servicio = rubro.rubroTypeID;
            rubro.cta_debe = (ArrayList)DB.getCtaContablebyRubro("debe", (int)rubro.rubroID, user.PaisID, transID, int.Parse(lb_contribuyente.SelectedValue), moneda, imp_exp, tipo_cobro, tipo_contabilidad, servicio);
            rubro.cta_haber = (ArrayList)DB.getCtaContablebyRubro("haber", (int)rubro.rubroID, user.PaisID, transID, int.Parse(lb_contribuyente.SelectedValue), moneda, imp_exp, tipo_cobro, tipo_contabilidad, servicio);
            if ((rubro.cta_debe == null && rubro.cta_haber == null) || (rubro.cta_debe.Count == 0 && rubro.cta_haber.Count == 0))
            {
                WebMsgBox.Show("El rubro " + rubro.rubroName + " no tiene configurada ninguna cuenta contable para la combinacion de cobro que esta realizando, por favor pongase en contacto con el Contador");
                return;
            }

            if (factfinal.RubrosArr == null) factfinal.RubrosArr = new ArrayList();
            factfinal.RubrosArr.Add(rubro);

            #region Validacion de Cuentas Contables Intercompany Destino
            if (factfinal.Tipo_Persona == 10)
            {
                int _intercompanyID = 0;
                int _idpaisORIGEN = 0;
                int _idcontaORIGEN = 0;
                int _idmonedaORIGEN = 0;
                int _idOPERACION = 0;
                int _idpaisDESTINO = 0;
                int _idcontaDESTINO = 0;
                int _idmonedaDESTINO = 0;

                int existe_configuracion = 0;
                string sql_existencia = " and tia_pais_origen=" + user.PaisID + " and tia_contabilidad_origen=" + user.contaID + " and tia_moneda_origen=" + factfinal.MonedaID + " and tia_tipo_operacion=" + 2 + " and tia_id_intercompany=" + factfinal.CliID.ToString() + " ";
                existe_configuracion = Contabilizacion_Automatica_CAD.Validar_Existencia_Intercompany_Administrativo(user, sql_existencia);
                if (existe_configuracion == -100)
                {
                    WebMsgBox.Show("Existio un Error al Tratar de validar la configuracion del Intercompany");
                    return;
                }
                else if (existe_configuracion > 0)
                {
                    RE_GenericBean _Intercompany_Origen = null;
                    _intercompanyID = int.Parse(factfinal.CliID.ToString());
                    _idpaisORIGEN = user.PaisID;
                    _idcontaORIGEN = user.contaID;
                    _idmonedaORIGEN = factfinal.MonedaID;
                    _idOPERACION = 2;
                    RE_GenericBean Bean_Configuracion_Intercompany = Contabilizacion_Automatica_CN.Obtener_Configuracion_Intercompany_Administrativo(_intercompanyID, _idpaisORIGEN, _idcontaORIGEN, _idmonedaORIGEN, _idOPERACION);
                    _idpaisDESTINO = Bean_Configuracion_Intercompany.intC1;
                    _idcontaDESTINO = Bean_Configuracion_Intercompany.intC2;
                    _idmonedaDESTINO = Bean_Configuracion_Intercompany.intC3;
                    _Intercompany_Origen = (RE_GenericBean)DB.Get_Intercompany_Data_By_Empresa(_idpaisORIGEN);

                    int _tttID_Destino = 15;
                    Rubros Rubro_Destino = new Rubros();
                    Rubro_Destino.cta_debe = (ArrayList)DB.getCtaContablebyRubro("debe", (int)rubro.rubroID, _idpaisDESTINO, _tttID_Destino, contribuyente, _idmonedaDESTINO, imp_exp, tipo_cobro, _idcontaDESTINO, servicio);
                    if ((Rubro_Destino.cta_debe == null) || (Rubro_Destino.cta_debe.Count == 0))
                    {
                        WebMsgBox.Show("El rubro " + rubro.rubroName + " no tiene configurada ninguna cuenta contable para la Provision en Destino , por favor pongase en contacto con el Contador");
                        bt_Enviar.Enabled = false;
                        bt_imprimir.Enabled = false;
                        return;
                    }

                    int transID_Destino = 105;//Provision Intercompanys
                    int _matOpID_Destino = DB.getMatrizOperacionID(transID_Destino, _idmonedaDESTINO, _idpaisDESTINO, _idcontaDESTINO);
                    ArrayList ctas_cargo_Destino = (ArrayList)DB.getMatrizConfiguracion_ingreso_egreso(_matOpID_Destino, "Abono");
                    if ((ctas_cargo_Destino == null) || (ctas_cargo_Destino.Count == 0))
                    {
                        WebMsgBox.Show("No existe configuracion contable para la Provision en Destino , por favor pongase en contacto con el Contador");
                        return;
                    }
                }
            }
            #endregion

        }
        string Check_Existencia = DB.CheckExistDoc(factfinal.Fecha_Hora, 4);

        int matOpID = DB.getMatrizOperacionID(int.Parse(lb_trans.Text), moneda, user.PaisID, tipo_contabilidad);
        ArrayList ctas_cargo = (ArrayList)DB.getMatrizConfiguracion(matOpID);
        if (Check_Existencia == "0")
        {
            ArrayList result = DB.insertNotaDebito2(factfinal, user, int.Parse(lb_tipopersona.SelectedValue), tipo_contabilidad, ctas_cargo);//4 ya que en la tabla tbl_tipo_persona 4=proveedor
            if (result == null || result.Count == 0 || int.Parse(result[0].ToString()) == 0 || int.Parse(result[0].ToString()) == -100)
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
                            bt_nota_debito_virtual.Enabled = false;
                            WebMsgBox.Show(Arr_Transmision_CR[1].ToString());
                            return;
                        }
                    }
                }
                #endregion

                WebMsgBox.Show("Error, existió un error al tratar de grabar los datos, por favor intente de nuevo.");
                return;
            }
            else
            {
                bt_imprimir.Visible = true;
                bt_nota_debito_virtual.Visible = true;
                lb_facid.Text = result[0].ToString();

                //Mostrando la Partida contable generada
                gv_detalle_partida.DataSource = DB.getPolizaDiario(user, double.Parse(lb_facid.Text), 4, 0);
                gv_detalle_partida.DataBind();
                gv_detalle_partida.Rows[gv_detalle_partida.Rows.Count - 1].Font.Bold = true;

                //Intercompanys
                if (factfinal.Tipo_Persona == 10)
                {
                    tb_resultado_automatizacion.Text = result[2].ToString();
                    pnl_intercompanys.Visible = true;
                    //Notificacion Automatica de Intercompanys
                    bool resultado_notificacion = false;
                    resultado_notificacion = Contabilizacion_Automatica_CN.Generar_Notificacion_Automatica_Intercompany(4, int.Parse(lb_facid.Text), 1);
                    if (user.PaisID == 1 || user.PaisID == 15)
                    {
                        bt_nd_intrcompany.Visible = true;
                    }
                }

                if (lb_moneda.SelectedValue == "8")
                {
                    if (user.PaisID == 1 || user.PaisID == 15)
                    {
                        bt_impresion_debitnote.Visible = true;
                    }
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
                            tb_resultado_transmision.Text = Arr_Transmision_CR[1].ToString().Replace("\\n", "\n");
                            tb_correlativo.Text = Arr_Transmision_CR[2].ToString();
                            lb_facid.Text = result[0].ToString();
                            #region Mostrando la Partida contable
                            user = (UsuarioBean)Session["usuario"];
                            gv_detalle_partida.DataSource = DB.getPolizaDiario(user, double.Parse(lb_facid.Text), 4, 0);
                            gv_detalle_partida.DataBind();
                            gv_detalle_partida.Rows[gv_detalle_partida.Rows.Count - 1].Font.Bold = true;
                            #endregion
                            bt_Enviar.Enabled = false;
                            bt_nota_debito_virtual.Enabled = true;
                            WebMsgBox.Show(Arr_Transmision_CR[1].ToString());
                            return;
                        }
                    }
                }
                #endregion

                WebMsgBox.Show("La Nota de Debito fue grabada exitosamente con Serie.: " + lb_serie_factura.SelectedItem.Text + " y Correlativo.: " + result[1].ToString());
                tb_correlativo.Text = result[1].ToString();
                bt_Enviar.Enabled = false;
                factfinal = null;
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
        //string script = "window.open('print_notadebito.aspx?fac_id=" + lb_facid.Text + "','Imprimir','menubar=1,resizable=1,scrollbars=1,toolbar=1,width=750');";
        string script = "window.open('../ImpresionDocumentos.html?fac_id=" + lb_facid.Text + "&tipo=4&pais=" + user.PaisID.ToString() + "&idioma=" + user.Idioma.ToString() + "&tipo_cambio=" + user.pais.TipoCambio.ToString() + "&contaId=" + user.contaID.ToString() + "','Re-Imprimir','toolbar=0, status=1, resizable=1, scrollbars=1, top=0, left=0, width=' + (window.screen.width -10) + ', height=' + (window.screen.height -75))";

        if (!cs2.IsStartupScriptRegistered(cstype, csname1))
        {
            cs2.RegisterStartupScript(cstype, csname1, script, true);
        }

    }
    protected void lb_servicio_SelectedIndexChanged(object sender, EventArgs e)
    {
        int servID = int.Parse(lb_servicio.SelectedValue);
        int tipomoneda = int.Parse(lb_moneda.SelectedValue);
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
        tb_cuenta_ModalPopupExtender.Show();
    }
    protected void gv_detalle_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("ID");
        dt.Columns.Add("NOMBRE");
        dt.Columns.Add("TYPE");
        dt.Columns.Add("MONEDATYPE");
        dt.Columns.Add("SUBTOTAL");
        dt.Columns.Add("IMPUESTO");
        dt.Columns.Add("TOTAL");
        dt.Columns.Add("TOTALD");
        dt.Clear();
        Label lb1, lb2, lb3, lb4, lb5, lb6, lb7, lb8;
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
            object[] objArr = { lb1.Text, lb2.Text, lb3.Text, lb8.Text, lb4.Text, lb5.Text, lb6.Text, lb7.Text };
            dt.Rows.Add(objArr);
        }
        dt.Rows[e.RowIndex].Delete();
        gv_detalle.DataSource = dt;
        gv_detalle.DataBind();
    }
    protected void gv_clientes_SelectedIndexChanged(object sender, EventArgs e)
    {
        GridViewRow row = gv_clientes.SelectedRow;
        if (lb_tipopersona.SelectedValue.Equals("4")) //proveedor
        {
            tbCliCod.Text = Page.Server.HtmlDecode(row.Cells[1].Text);
            tb_nombre.Text = Page.Server.HtmlDecode(row.Cells[3].Text);
            tb_direccion.Text = Page.Server.HtmlDecode(row.Cells[7].Text);
            tb_telefono.Text = Page.Server.HtmlDecode(row.Cells[8].Text);
            tb_correoelectronico.Text = Page.Server.HtmlDecode(row.Cells[6].Text);
            tb_observaciones.Text = Page.Server.HtmlDecode(row.Cells[5].Text);
            drp_tipo_identificacion_cliente.SelectedValue = "0";//No Definida
        }
        else if (lb_tipopersona.SelectedValue.Equals("2")) //agente
        {
            if ((DB.Validar_Restriccion_Activa(user, 26, 29) == true))
            {
                #region Validacion Grupo Empresas
                if ((user.SucursalID == 9) || (user.SucursalID == 71))
                {
                }
                else if (user.pais.Grupo_Empresas == 1)
                {
                }
                else if (user.pais.Grupo_Empresas == 2)
                {
                    if (Page.Server.HtmlDecode(row.Cells[7].Text) == "NO NEUTRAL")
                    {
                        WebMsgBox.Show("No se puede utilizar el Agente.: " + Page.Server.HtmlDecode(row.Cells[1].Text) + " - " + Page.Server.HtmlDecode(row.Cells[2].Text) + ", porque es un Agente NO NEUTRAL");
                        return;
                    }
                }
                else if (user.pais.Grupo_Empresas == 3)
                {
                }
                #endregion
            }
            tbCliCod.Text = Page.Server.HtmlDecode(row.Cells[1].Text);
            tb_nombre.Text = Page.Server.HtmlDecode(row.Cells[2].Text);
            tb_direccion.Text = Page.Server.HtmlDecode(row.Cells[3].Text);
            tb_telefono.Text = Page.Server.HtmlDecode(row.Cells[4].Text);
            tb_correoelectronico.Text = Page.Server.HtmlDecode(row.Cells[5].Text);
            tb_nit.Text = Page.Server.HtmlDecode(row.Cells[9].Text);
            drp_tipo_identificacion_cliente.SelectedValue = "0";//No Definida
        }
        else if (lb_tipopersona.SelectedValue.Equals("5"))//naviera
        {
            tbCliCod.Text = Page.Server.HtmlDecode(row.Cells[1].Text);
            tb_nombre.Text = Page.Server.HtmlDecode(row.Cells[2].Text);
            tb_direccion.Text = "";
            tb_telefono.Text = "";
            tb_correoelectronico.Text = "";
            drp_tipo_identificacion_cliente.SelectedValue = "0";//No Definida
        }
        else if (lb_tipopersona.SelectedValue.Equals("6"))//Lineas aereas
        {
            tbCliCod.Text = Page.Server.HtmlDecode(row.Cells[1].Text);
            tb_nombre.Text = Page.Server.HtmlDecode(row.Cells[2].Text);
            //tb_contacto.Text = "";
            tb_direccion.Text = "";
            tb_telefono.Text = "";
            tb_correoelectronico.Text = "";
            drp_tipo_identificacion_cliente.SelectedValue = "0";//No Definida
        }
        else if (lb_tipopersona.SelectedValue.Equals("10"))//Intercompanys
        {
            tbCliCod.Text = Page.Server.HtmlDecode(row.Cells[1].Text);
            //tb_nombre.Text = Page.Server.HtmlDecode(row.Cells[2].Text);
            tb_nombre.Text = Page.Server.HtmlDecode(row.Cells[6].Text);
            tb_nit.Text = Page.Server.HtmlDecode(row.Cells[3].Text);
            tb_direccion.Text = Page.Server.HtmlDecode(row.Cells[5].Text);
            drp_tipo_identificacion_cliente.SelectedValue = "10";//Tipo de Identificacion del Extranjero
        }
        if (lb_tipopersona.SelectedValue == "10")
        {
            lb_contribuyente.SelectedValue = DB.getProveedorRegimen(int.Parse(lb_tipopersona.SelectedValue), tbCliCod.Text.ToString()).ToString();
        }
        else
        {
            lb_contribuyente.SelectedValue = "1";
        }
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
        gv_clientes.DataBind();
        modalcliente.Show();
    }
    protected void Button4_Click(object sender, EventArgs e)
    {
        DataTable dt = null;
        user = (UsuarioBean)Session["usuario"];
        ArrayList Arr = null;
        string where = "";
        if (tb_codigo.Text.Equals("") && tb_nombreb.Text.Equals("") && tb_nitb.Text.Equals(""))
        {
            WebMsgBox.Show("Debe ingresar al menos un criterio de búsqueda");
            return;
        }
        if (lb_tipopersona.SelectedValue.Equals("4"))
        {
            if (!tb_nombreb.Text.Trim().Equals("") && tb_nombreb.Text != null) where += " upper(rtrim(nombre)) like '%" + tb_nombreb.Text.Trim().ToUpper() + "%'";
            if (!tb_codigo.Text.Trim().Equals("") && tb_codigo.Text != null)
                if (!where.Equals("")) where += " and numero=" + tb_codigo.Text; else where += "numero=" + tb_codigo.Text;
            if (!tb_nitb.Text.Trim().Equals("") && tb_nitb.Text != null)
                if (!where.Equals("")) where += " and nit='" + tb_nitb.Text + "'"; else where += "nit='" + tb_nitb.Text + "'";
            Arr = (ArrayList)DB.getProveedor(where, ""); //Proveedor
            dt = (DataTable)Utility.fillGridView("Proveedor", Arr);
        }
        else if (lb_tipopersona.SelectedValue.Equals("2"))
        {
            if (!tb_nitb.Text.Equals(""))
            {
                WebMsgBox.Show("No se puede buscar por nit a un agente");
                return;
            }
            if (!tb_nombreb.Text.Trim().Equals("") && tb_nombreb.Text != null) where += " upper(rtrim(agente)) like '%" + tb_nombreb.Text.Trim().ToUpper() + "%'";
            if (!tb_codigo.Text.Trim().Equals("") && tb_codigo.Text != null)
                if (!where.Equals("")) where += " and agente_id=" + tb_codigo.Text; else where += "agente_id=" + tb_codigo.Text;
            Arr = (ArrayList)DB.getAgente(where, ""); //Agente
            dt = (DataTable)Utility.fillGridView("Agente", Arr);
        }
        else if (lb_tipopersona.SelectedValue.Equals("5"))
        {
            if (!tb_nitb.Text.Equals(""))
            {
                WebMsgBox.Show("No se puede buscar por nit una naviera");
                return;
            }
            if (!tb_nombreb.Text.Trim().Equals("") && tb_nombreb.Text != null) where += " upper(rtrim(nombre)) like '%" + tb_nombreb.Text.Trim().ToUpper() + "%'";
            if (!tb_codigo.Text.Trim().Equals("") && tb_codigo.Text != null)
                if (!where.Equals("")) where += " and id_naviera=" + tb_codigo.Text; else where += "id_naviera=" + tb_codigo.Text;
            Arr = (ArrayList)DB.getNavieras(where, ""); //Naviera
            dt = (DataTable)Utility.fillGridView("Naviera", Arr);
        }
        else if (lb_tipopersona.SelectedValue.Equals("6"))
        {
            if (!tb_nitb.Text.Equals(""))
            {
                WebMsgBox.Show("No se puede buscar por nit una línea aerea");
                return;
            }
            if (!tb_nombreb.Text.Trim().Equals("") && tb_nombreb.Text != null) where += " and upper(rtrim(name)) ilike '%" + tb_nombreb.Text.Trim().ToUpper() + "%'";
            if (!tb_codigo.Text.Trim().Equals("") && tb_codigo.Text != null)
                if (!where.Equals("")) where += " and carrier_id=" + tb_codigo.Text; else where += " and carrier_id=" + tb_codigo.Text;

            Arr = (ArrayList)DB.getCarriers(where); //Lineas aereas
            dt = (DataTable)Utility.fillGridView("LineasAereas", Arr);
        }
        else if (lb_tipopersona.SelectedValue.Equals("10"))
        {
            if (!tb_nombreb.Text.Trim().Equals("") && tb_nombreb.Text != null) where += " and upper(rtrim(nombre_comercial)) like '%" + tb_nombreb.Text.Trim().ToUpper() + "%'";
            if (!tb_codigo.Text.Trim().Equals("") && tb_codigo.Text != null)
                if (!where.Equals("")) where += " and id_intercompany=" + tb_codigo.Text; else where += " and id_intercompany=" + tb_codigo.Text;
            if (!tb_nitb.Text.Trim().Equals("") && tb_nitb.Text != null)
                if (!where.Equals("")) where += " and nit='" + tb_nitb.Text + "'"; else where += " and nit='" + tb_nitb.Text + "'";
            Arr = (ArrayList)DB.Get_Intercompanys(where);//Intercompany
            dt = (DataTable)Utility.fillGridView("Intercompany", Arr);
        }

        gv_clientes.DataSource = dt;
        gv_clientes.DataBind();
        ViewState["dt"] = dt;
        modalcliente.Show();
    }

    protected void dgw1_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        string tipo = lb_tipo.SelectedValue;
        if (e.CommandName == "Seleccionar")
        {
            int index = Convert.ToInt32(e.CommandArgument);
            if (tipo.Equals("FCL") || tipo.Equals("LCL"))
            {
                tb_hbl.Text = Page.Server.HtmlDecode(dgw1.Rows[index].Cells[2].Text);
                tb_mbl.Text = Page.Server.HtmlDecode(dgw1.Rows[index].Cells[3].Text);
                tb_routing.Text = Page.Server.HtmlDecode(dgw1.Rows[index].Cells[4].Text);
                tb_contenedor.Text = Page.Server.HtmlDecode(dgw1.Rows[index].Cells[5].Text);
            }
            else if (tipo.Equals("PICKING"))
            {
                tb_hbl.Text = Page.Server.HtmlDecode(dgw1.Rows[index].Cells[2].Text);
                tb_mbl.Text = Page.Server.HtmlDecode(dgw1.Rows[index].Cells[3].Text);
            }
            else if (tipo.Equals("AEREO"))
            {
                tb_hbl.Text = Page.Server.HtmlDecode(dgw1.Rows[index].Cells[2].Text);
                tb_mbl.Text = Page.Server.HtmlDecode(dgw1.Rows[index].Cells[3].Text);
            }
            else if (tipo.Equals("TERRESTRE T"))
            {
                tb_hbl.Text = Page.Server.HtmlDecode(dgw1.Rows[index].Cells[2].Text);
                tb_mbl.Text = Page.Server.HtmlDecode(dgw1.Rows[index].Cells[3].Text);
            }
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
    protected void bt_search_Click(object sender, EventArgs e)
    {
        ListItem item = null;
        UsuarioBean user = (UsuarioBean)Session["usuario"];
        int lista = 0;

        if (lb_tipo.SelectedValue.Equals("LCL"))
        {
            DataSet ds = DB.getBL_LCL(lista, tb_criterio.Text.Trim(), user);
            this.dgw1.DataSource = ds.Tables["bl_list"];
            this.dgw1.DataBind();
            ViewState["dt"] = ds.Tables["bl_list"];
        }
        else if (lb_tipo.SelectedValue.Equals("FCL"))
        {
            DataSet ds = DB.getBL_FCL(lista, tb_criterio.Text.Trim(), user);
            this.dgw1.DataSource = ds.Tables["bl_list"];
            this.dgw1.DataBind();
            ViewState["dt"] = ds.Tables["bl_list"];
        }
        else if (lb_tipo.SelectedValue.Equals("ALMACENADORA"))
        {
            string codDistribucion = Utility.CodigoDistribucionPicking(user.PaisID);
            DataSet ds = DB.getBL_ALMACENADORA(lista, tb_criterio.Text.Trim(), user.pais.schema, codDistribucion, user);
            this.dgw1.DataSource = ds.Tables["bl_list"];
            this.dgw1.DataBind();
            ViewState["dt"] = ds.Tables["bl_list"];
        }
        else if (lb_tipo.SelectedValue.Equals("AEREO"))
        {
            DataSet ds = DB.getBL_AEREO(lista, tb_criterio.Text.Trim(), user.pais.schema);
            this.dgw1.DataSource = ds.Tables["bl_list"];
            this.dgw1.DataBind();
            ViewState["dt"] = ds.Tables["bl_list"];
        }
        else if (lb_tipo.SelectedValue.Equals("TERRESTRE T"))
        {
            DataSet ds = DB.getBL_TERRESTRE(lista, tb_criterio.Text.Trim(), user.pais.ISO, user);
            this.dgw1.DataSource = ds.Tables["bl_list"];
            this.dgw1.DataBind();
            ViewState["dt"] = ds.Tables["bl_list"];
        }
        //pnlhbl_ModalPopupExtender.Show();
    }
    protected void dgw12_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        DataTable dt1;
        dt1 = (DataTable)ViewState["dt"];
        dgw12.DataSource = dt1;
        dgw12.PageIndex = e.NewPageIndex;
        dgw12.DataBind();
        //tb_cuenta2_ModalPopupExtender.Show();
    }
    protected void dgw12_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        string tipo = lb_tipo2.SelectedValue;
        if (e.CommandName == "Seleccionar2")
        {
            int index = Convert.ToInt32(e.CommandArgument);
            if (tipo.Equals("FCL") || tipo.Equals("LCL"))
            {
                tb_mbl.Text = dgw12.Rows[index].Cells[3].Text;
            }
            else if (tipo.Equals("PICKING"))
            {
                tb_mbl.Text = dgw12.Rows[index].Cells[3].Text;
            }
            else if (tipo.Equals("AEREO"))
            {
                tb_mbl.Text = dgw12.Rows[index].Cells[3].Text;
            }
            else if (tipo.Equals("TERRESTRE T"))
            {
                tb_mbl.Text = dgw12.Rows[index].Cells[3].Text;
            }
        }
    }
    protected void bt_search2_Click(object sender, EventArgs e)
    {
        ListItem item = null;
        UsuarioBean user = (UsuarioBean)Session["usuario"];
        int lista = 0;

        if (lb_tipo2.SelectedValue.Equals("LCL"))
        {
            DataSet ds = DB.getBL_LCL(lista, tb_criterio2.Text.Trim(), user);
            this.dgw12.DataSource = ds.Tables["bl_list"];
            this.dgw12.DataBind();
            ViewState["dt"] = ds.Tables["bl_list"];
        }
        else if (lb_tipo2.SelectedValue.Equals("FCL"))
        {
            DataSet ds = DB.getBL_FCL(lista, tb_criterio2.Text.Trim(), user);
            this.dgw12.DataSource = ds.Tables["bl_list"];
            this.dgw12.DataBind();
            ViewState["dt"] = ds.Tables["bl_list"];
        }
        else if (lb_tipo2.SelectedValue.Equals("ALMACENADORA"))
        {
            string codDistribucion = Utility.CodigoDistribucionPicking(user.PaisID);
            DataSet ds = DB.getBL_ALMACENADORA(lista, tb_criterio2.Text.Trim(), user.pais.schema, codDistribucion, user);
            this.dgw12.DataSource = ds.Tables["bl_list"];
            this.dgw12.DataBind();
            ViewState["dt"] = ds.Tables["bl_list"];
        }
        else if (lb_tipo2.SelectedValue.Equals("AEREO"))
        {
            DataSet ds = DB.getBL_AEREO(lista, tb_criterio2.Text.Trim(), user.pais.schema);
            this.dgw12.DataSource = ds.Tables["bl_list"];
            this.dgw12.DataBind();
            ViewState["dt"] = ds.Tables["bl_list"];
        }
        else if (lb_tipo2.SelectedValue.Equals("TERRESTRE T"))
        {
            string paisISO = Utility.InttoISOPais(user.PaisID);
            DataSet ds = DB.getBL_TERRESTRE(lista, tb_criterio2.Text.Trim(), paisISO, user);
            this.dgw12.DataSource = ds.Tables["bl_list"];
            this.dgw12.DataBind();
            ViewState["dt"] = ds.Tables["bl_list"];
        }
    }
    protected void lb_trans_SelectedIndexChanged(object sender, EventArgs e)
    {
        int tranID = 0;
        if (!lb_trans.SelectedItem.Text.Equals("Seleccione..."))
        {
            tranID = int.Parse(lb_trans.SelectedValue);
            if (tranID == 59) { lb_tipopersona.SelectedValue = "2"; }// agentes
            else if (tranID == 61) { lb_tipopersona.SelectedValue = "6"; }// linea aerea
            else if (tranID == 52) { lb_tipopersona.SelectedValue = "4"; }// proveedores
            else if (tranID == 60) { lb_tipopersona.SelectedValue = "5"; }// naviera
            else if (tranID == 106) { lb_tipopersona.SelectedValue = "10"; }//Intercompanys
        }
        else
        { 
            tranID = int.Parse(lb_trans.SelectedValue);
            if (tranID == 0) { lb_tipopersona.SelectedValue = "0"; }
        }

        #region Validaciones Aimar Costa Rica
        if (Page.IsPostBack)
        {
            if (user.PaisID == 5)
            {
                if (user.SucursalID == 117)
                {
                    if (lb_tipopersona.SelectedValue != "10")
                    {
                        WebMsgBox.Show("No es posible emitir.: " + lb_trans.SelectedItem.Text + " Electronica, por favor emita el cobro desde otro Departamento distinto al de Contabilidad.");
                        bt_Enviar.Enabled = false;
                        lb_trans.SelectedValue = "0";
                        return;
                    }
                }
                else if (user.SucursalID != 117)
                {
                    if (lb_tipopersona.SelectedValue == "10")
                    {
                        WebMsgBox.Show("No es posible emitir.: " + lb_trans.SelectedItem.Text + " desde este Departamento, por favor ingrese al Departamento Contabilidad.");
                        bt_Enviar.Enabled = false;
                        lb_trans.SelectedValue = "0";
                        return;
                    }
                }
            }
        }
        #endregion

        if (lb_trans.SelectedValue == "106")
        {
            //Si es Intercompany debe ser Excento
            lb_contribuyente.Enabled = false;
            lb_contribuyente.SelectedValue = "1";
            //Filtrar unicamente Servicio de Terceros cuando es Intercompany
            lb_servicio.Items.Clear();
            ListItem item = null;
            item = new ListItem("TERCEROS", "14");
            lb_servicio.Items.Add(item);
            if (lb_servicio.Items.Count > 0)
            {
                lb_servicio.SelectedValue = "14";
                ArrayList rubros = (ArrayList)DB.getAllRubrosXPaisXServ(user.PaisID, int.Parse(lb_servicio.SelectedValue), "");
                RE_GenericBean rubbean = null;
                for (int a = 0; a < rubros.Count; a++)
                {
                    rubbean = (RE_GenericBean)rubros[a];
                    item = new ListItem(rubbean.strC1, rubbean.intC1.ToString());
                    lb_rubro.Items.Add(item);
                }
            }
        }
        else
        {
            lb_contribuyente.Enabled = false;
            lb_contribuyente.SelectedValue = "1";

            lb_servicio.Items.Clear();
            ArrayList arr = null;
            ListItem item = null;
            if (DB.Validar_Restriccion_Activa(user, 26, 24) == true)
            {
                arr = (ArrayList)DB.Get_Tipos_Servicio_Permitidos(user.PaisID, user.contaID, 26, user.SucursalID);
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


        }
        #region Limpiar Tipo de Persona
        tbCliCod.Text = "0";
        tb_nombre.Text = "";
        tb_direccion.Text = "";
        tb_telefono.Text = "";
        tb_correoelectronico.Text = "";
        gv_detalle.DataBind();
        #endregion
    }

    protected void bt_nota_debito_virtual_Click(object sender, EventArgs e)
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
            string script = "window.open('../invoice/template_invoice.aspx?id=" + lb_facid.Text + "&transaccion=4','Imprimir','menubar=1,resizable=1,scrollbars=1,toolbar=1,width=750');";
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
            #region NO EMITIR PAGO MANUAL AGENTE ECONO
            if ((DB.Validar_Restriccion_Activa(user, 26, 26) == true) && (sender == "Validar_Econocaribe"))
            {
                resultado = false;
                if (lb_tipopersona.SelectedValue == "2")
                {
                    if (user.pais.Grupo_Empresas == 2)
                    {
                        int res = DB.Validar_Persona_Grupo_Econocaribe(user, int.Parse(tbCliCod.Text), 2);
                        if (res > 0)
                        {
                            WebMsgBox.Show("El Agente Econocaribe no puede ser utilizado en modulo.: " + user.pais.Nombre_Sistema + ", debe utilizar el Modulo de Aimar");
                            resultado = true;
                        }
                    }
                }
                return resultado;
            }
            #endregion
        }
        return resultado;
    }
    private void cargo_datos_BL()
    {
        #region Cargo Datos BL
        if ((Request.QueryString["bl_no"] != null) && (Request.QueryString["tipo"] != null))
        {
            string bl_no = Request.QueryString["bl_no"].ToString().Trim();
            string tipo = Request.QueryString["tipo"].ToString().Trim();
            string blID = Request.QueryString["blid"].ToString();
            string opID = Request.QueryString["opid"].ToString();
            string idRouting = "-1";
            if (tipo.Equals("LCL"))
            {
                RE_GenericBean rgb = (RE_GenericBean)DB.getInvoiceDataLCL(bl_no, user, blID);
                if (rgb == null || rgb.lonC1 == 0)
                {
                    WebMsgBox.Show("Existio un error al momento de cargar la informacion.");
                    return;
                }
                lbl_blID.Text = blID;
                lbl_tipoOperacionID.Text = opID;
                tb_hbl.Text = rgb.strC1;
                tb_mbl.Text = rgb.strC2;
                tb_contenedor.Text = rgb.strC4;
                idRouting = rgb.lonC10.ToString();
                tb_routing.Text = DB.getRouting(rgb.lonC10);
                lb_imp_exp.SelectedValue = rgb.strC5;
                lb_imp_exp.Enabled = false;
            }
            else if (tipo.Equals("FCL"))
            {
                RE_GenericBean rgb = (RE_GenericBean)DB.getInvoiceDataFCL(bl_no, user, blID);
                if (rgb == null || rgb.lonC1 == 0)
                {
                    WebMsgBox.Show("Existio un error al momento de cargar la informacion.");
                    return;
                }
                lbl_blID.Text = blID;
                lbl_tipoOperacionID.Text = opID;
                tb_hbl.Text = rgb.strC1;
                tb_mbl.Text = rgb.strC2;
                tb_contenedor.Text = rgb.strC4;
                idRouting = rgb.lonC10.ToString();
                tb_routing.Text = DB.getRouting(rgb.lonC10);
                lb_imp_exp.SelectedValue = rgb.strC5;
                lb_imp_exp.Enabled = false;
            }
            else if (tipo.Equals("TERRESTRE T"))
            {
                double QS_BLID = 0;
                string bldetailid = "";
                if (Request.QueryString["blid"] != null)
                {
                    QS_BLID = double.Parse(Request.QueryString["blid"].ToString());
                    bldetailid = Request.QueryString["bldetailid"].ToString();
                }
                RE_GenericBean rgb = (RE_GenericBean)DB.getDataTerrestre_Costos(QS_BLID, user, int.Parse(bldetailid));
                if (rgb == null || rgb.lonC1 == 0)
                {
                    WebMsgBox.Show("Existio un error al momento de cargar la informacion.");
                    return;
                }
                lbl_blID.Text = blID;
                lbl_tipoOperacionID.Text = opID;
                tb_hbl.Text = rgb.strC1;
                tb_mbl.Text = rgb.strC2;
                idRouting = DB.GetIDRoutingbyNumero(rgb.strC7);
                tb_routing.Text = rgb.strC7;
                tb_contenedor.Text = rgb.strC4;
            }
            else if (tipo.Equals("AEREO"))
            {
                int Tipo_Guia = 0;
                if (Request.QueryString["opid"] != null)
                {
                    Tipo_Guia = int.Parse(Request.QueryString["opid"].ToString());
                }
                RE_GenericBean rgb = (RE_GenericBean)DB.getInvoiceDataAereo(bl_no, 0, Tipo_Guia, blID);
                if (rgb == null || rgb.lonC1 == 0)
                {
                    WebMsgBox.Show("Existio un error al momento de cargar la informacion.");
                    return;
                }
                lbl_blID.Text = blID;
                lbl_tipoOperacionID.Text = opID;
                tb_hbl.Text = rgb.strC1;
                tb_mbl.Text = rgb.strC2;
                tb_contenedor.Text = rgb.strC4;
                idRouting = rgb.lonC8.ToString();
                tb_routing.Text = rgb.strC7;
                lb_imp_exp.SelectedValue = rgb.strC10;
                lb_imp_exp.Enabled = false;
            }
            else if (tipo.Equals("RO ADUANAS"))
            {
                RE_GenericBean rgb = (RE_GenericBean)DB.getInvoiceRO_Aduanas(user, bl_no, blID);
                if (rgb == null || rgb.lonC1 == 0)
                {
                    WebMsgBox.Show("Existio un error al momento de cargar la informacion.");
                    return;
                }
                lbl_blID.Text = blID;
                lbl_tipoOperacionID.Text = opID;
                tb_hbl.Text = rgb.strC1;
                tb_mbl.Text = rgb.strC2;
                tb_contenedor.Text = rgb.strC4;
                idRouting = lbl_blID.Text;
                tb_routing.Text = rgb.strC5;
                lb_imp_exp.Enabled = false;
            }
            else if (tipo.Equals("RO SEGUROS"))
            {
                RE_GenericBean rgb = (RE_GenericBean)DB.getInvoiceRO_Seguros(user, bl_no, blID);
                if (rgb == null || rgb.lonC1 == 0)
                {
                    WebMsgBox.Show("Existio un error al momento de cargar la informacion.");
                    return;
                }
                lbl_blID.Text = blID;
                lbl_tipoOperacionID.Text = opID;
                tb_hbl.Text = rgb.strC1;
                tb_mbl.Text = rgb.strC2;
                tb_contenedor.Text = rgb.strC4;
                idRouting = lbl_blID.Text;
                tb_routing.Text = rgb.strC5;
                lb_imp_exp.Enabled = false;
            }
        }
        Definir_Tipo_Operacion();
        #endregion
    }
    protected void Definir_Tipo_Operacion()
    {
        #region Definir Tipo Operacion
        if (lbl_operacion.Text != "0")
        {
            ArrayList Arr_Sistemas = DB.Get_Detalle_Sistemas(" and b.tto_id=" + lbl_tipoOperacionID.Text + "");
            foreach (RE_GenericBean Bean in Arr_Sistemas)
            {
                lbl_sistema.Text = Bean.strC1;
                lbl_operacion.Text = Bean.strC2;
            }
            pnl_operacion.Visible = true;
        }
        #endregion
    }
    protected void btn_borrar_hbl_Click(object sender, ImageClickEventArgs e)
    {
        lbl_hbl_eliminado.Text = tb_hbl.Text;
        tb_hbl.Text = "";
    }
    protected void lb_serie_factura_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (lb_serie_factura.Items.Count > 0)
        {
            if (lb_serie_factura.SelectedValue != "0")
            {
                int moneda_Serie = 0;
                moneda_Serie = DB.Get_Serie_Moneda_By_ID(int.Parse(lb_serie_factura.SelectedValue));
                lb_moneda.SelectedValue = moneda_Serie.ToString();//Moneda de la Transaccion
                lb_moneda2.SelectedValue = moneda_Serie.ToString();//Moneda del Rubro
                lb_moneda.Enabled = false;
                int impo_expo = 0;
                if (lb_imp_exp.Items.Count > 0)
                {
                    impo_expo = int.Parse(lb_imp_exp.SelectedValue);
                    Actualizar_Moneda_Rubros();
                }
            }
        }
    }
    protected void Actualizar_Moneda_Rubros()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("ID");
        dt.Columns.Add("NOMBRE");
        dt.Columns.Add("TYPE");
        dt.Columns.Add("MONEDATYPE");
        dt.Columns.Add("SUBTOTAL");
        dt.Columns.Add("IMPUESTO");
        dt.Columns.Add("TOTAL");
        dt.Columns.Add("TOTALD");
        dt.Clear();
        Label lb1, lb2, lb3, lb4, lb5, lb6, lb7, lb8;
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
            Rubros rubro = new Rubros();
            rubro.rubroID = long.Parse(lb1.Text);
            rubro.rubroName = lb2.Text;
            int cliID = int.Parse(tbCliCod.Text);
            rubro.rubroMoneda = Utility.TraducirMonedaStr(lb8.Text);
            rubro.rubtoType = lb3.Text;
            //rubro.rubroTypeID = Utility.TraducirServiciotoINT(lb3.Text);
            rubro.rubroTot = double.Parse(lb6.Text);
            Rubros rubtemp = (Rubros)rubro;
            rubtemp = (Rubros)DB.ExistRubroPais(rubro, user.PaisID);
            if (rubtemp == null)
            {
                WebMsgBox.Show("Error, El HBL no tiene todos los rubros registrados en contabilidad para este pais, por favor pongase en contacto con el departamento de contabilidad para agregarlos.");
                return;
            }
            int tipo_contabilidad = int.Parse(Session["Contabilidad"].ToString());

            if ((rubtemp.CobIva == 1) && (tipo_contabilidad != 2) && (!lb_contribuyente.SelectedValue.Equals("1")))
            {
                if (rubtemp.IvaInc == 1)
                {
                    rubtemp.rubroSubTot = Math.Round(rubtemp.rubroTot * (double)(1 / (1 + user.pais.Impuesto)), 2);
                    rubtemp.rubroImpuesto = Math.Round(rubtemp.rubroTot - rubtemp.rubroSubTot, 2);
                }
                else
                {
                    rubtemp.rubroImpuesto = Math.Round(rubtemp.rubroTot * (double)user.pais.Impuesto, 2);
                    rubtemp.rubroSubTot = Math.Round(rubtemp.rubroTot, 2);
                    rubtemp.rubroTot = Math.Round(rubtemp.rubroSubTot + rubtemp.rubroImpuesto, 2);
                }
            }
            else
            {
                rubtemp.rubroImpuesto = 0;
                rubtemp.rubroSubTot = rubtemp.rubroTot;
            }
            decimal tipoCambio = user.pais.TipoCambio;
            double totalD = rubtemp.rubroTot;
            if ((rubtemp.rubroMoneda == 8) && (lb_moneda.SelectedValue.Equals("8")))
            {
                totalD = Math.Round(rubtemp.rubroTot * (double)tipoCambio, 2);
            }
            else if ((rubtemp.rubroMoneda == 8) && (!lb_moneda.SelectedValue.Equals("8")))
            {
                rubtemp.rubroImpuesto = Math.Round(rubtemp.rubroImpuesto * (double)tipoCambio, 2);
                rubtemp.rubroSubTot = Math.Round(rubtemp.rubroSubTot * (double)tipoCambio, 2);
                rubtemp.rubroTot = Math.Round(rubtemp.rubroSubTot + rubtemp.rubroImpuesto, 2);
            }
            else if ((rubtemp.rubroMoneda != 8) && (lb_moneda.SelectedValue.Equals("8")))
            {
                rubtemp.rubroImpuesto = Math.Round(rubtemp.rubroImpuesto / (double)tipoCambio, 2);
                rubtemp.rubroSubTot = Math.Round(rubtemp.rubroSubTot / (double)tipoCambio, 2);
                rubtemp.rubroTot = Math.Round(rubtemp.rubroSubTot + rubtemp.rubroImpuesto, 2);
            }
            else if ((rubtemp.rubroMoneda != 8) && (!lb_moneda.SelectedValue.Equals("8")))
            {
                totalD = Math.Round(rubtemp.rubroTot / (double)tipoCambio, 2);
            }
            //object[] obj = { rubtemp.rubroID.ToString(), rubtemp.rubroName.ToString(), rubtemp.rubtoType.ToString(), Utility.TraducirMonedaInt(Convert.ToInt32(rubtemp.rubroMoneda)), rubtemp.rubroSubTot.ToString("#,#.00#;(#,#.00#)"), rubtemp.rubroImpuesto.ToString("#,#.00#;(#,#.00#)"), rubtemp.rubroTot.ToString("#,#.00#;(#,#.00#)"), totalD.ToString("#,#.00#;(#,#.00#)") };
            object[] obj = { rubtemp.rubroID.ToString(), rubtemp.rubroName.ToString(), rubtemp.rubtoType.ToString(), Utility.TraducirMonedaInt(Convert.ToInt32(lb_moneda.SelectedValue)), rubtemp.rubroSubTot.ToString("#,#.00#;(#,#.00#)"), rubtemp.rubroImpuesto.ToString("#,#.00#;(#,#.00#)"), rubtemp.rubroTot.ToString("#,#.00#;(#,#.00#)"), totalD.ToString("#,#.00#;(#,#.00#)") };
            dt.Rows.Add(obj);
        }
        gv_detalle.DataSource = dt;
        gv_detalle.DataBind();
    }
    protected void bt_nd_intrcompany_Click(object sender, EventArgs e)
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
            string script = "window.open('../invoice/template_intercompany.aspx?id=" + lb_facid.Text + "&transaccion=4','Imprimir','menubar=1,resizable=1,scrollbars=1,toolbar=1,width=750');";
            if (!cs2.IsStartupScriptRegistered(cstype, csname1))
            {
                cs2.RegisterStartupScript(cstype, csname1, script, true);
            }
        }
    }
    protected void bt_impresion_debitnote_Click(object sender, EventArgs e)
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
            string script = "window.open('../invoice/template_invoice_en.aspx?id=" + lb_facid.Text + "&transaccion=4','Imprimir','menubar=1,resizable=1,scrollbars=1,toolbar=1,width=750');";
            if (!cs2.IsStartupScriptRegistered(cstype, csname1))
            {
                cs2.RegisterStartupScript(cstype, csname1, script, true);
            }
        }
    }
    protected void Determinar_Tipo_Serie(int sucID, string serie)
    {
        int Doc_ID = DB.getDocumentoID(sucID, serie, 4, user);
        lbl_serie_id.Text = Doc_ID.ToString();
        RE_GenericBean Bean_Serie = (RE_GenericBean)DB.getFactura(Doc_ID, 4);
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

    protected void btBuscarFactura_Click(object sender, EventArgs e)
    {
        string seriefac = ddlSerieFacBusqueda.SelectedValue;
        if (seriefac == "")
        {
            WebMsgBox.Show("Debe indicar por lo menos la serie de la factura");
            return;
        }

        DataSet dsFacturas = (DataSet)DB.getFacturasElectronicas(ddlSerieFacBusqueda.SelectedItem.ToString(), tbCorrelativoFacBusqueda.Text, user);
        DataTable dtFacturas = dsFacturas.Tables["fact"];
        gv_facturas.DataSource = dtFacturas;
        gv_facturas.DataBind();
        ViewState["fact"] = dtFacturas;
        modalFacturas.Show();
    }

    protected void gv_facturas_SelectedIndexChanged(object sender, EventArgs e)
    {
        GridViewRow row = gv_facturas.SelectedRow;
        //tb_naviera.Text = row.Cells[2].Text;    PENDIENTES
        tbSerieFacturaRef.Text = row.Cells[2].Text;
        tbCorrelativoFacturaRef.Text = row.Cells[3].Text;
        hdIdFacturaRef.Value = row.Cells[1].Text;
        tbFechaFacturaRef.Text = row.Cells[4].Text; //2019-07-09
        tbDocFacturaRef.Text = row.Cells[13].Text; //2019-07-10 documento firmado gti

        ViewState["fact"] = null;
        gv_facturas.DataSource = null;
        gv_facturas.DataBind();
    }

    protected void gv_facturas_PageIndexChanged(object sender, EventArgs e)
    {
        if (Page.IsPostBack)
        {
            dt1 = (DataTable)ViewState["fact"];
        }
    }

    protected void gv_facturas_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        dt1 = (DataTable)ViewState["fact"];
        gv_facturas.DataSource = dt1;
        gv_facturas.PageIndex = e.NewPageIndex;
        gv_facturas.DataBind();
        modalFacturas.Show();
    }
}
