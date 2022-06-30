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

public partial class operations_notacreditoprov : System.Web.UI.Page
{
    UsuarioBean user;
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
        if (!user.Aplicaciones.Contains("6"))
            Response.Redirect("index.aspx");
        int permiso = int.Parse(user.Aplicaciones["6"].ToString());
        if (!((permiso & 8192) == 8192))
            Response.Redirect("index.aspx");

        //int tipo_contabilidad = int.Parse(Session["Contabilidad"].ToString());
        //obtengo_listas(tipo_contabilidad);
        //if (tipo_contabilidad == 2)
        //{
        //    #region Backup
        //    //lb_moneda.SelectedValue = "8";
        //    #endregion
        //    lb_moneda.SelectedValue = user.Moneda.ToString();
        //    lb_moneda.Enabled = false;
        //}
        //else
        //{
        //    #region Backup
        //    //lb_moneda.SelectedValue = user.pais.ID.ToString();
        //    #endregion
        //    lb_moneda.SelectedValue = user.Moneda.ToString();
        //    lb_moneda.Enabled = false;
        //}

        if (!IsPostBack)
        {
            int tipo_contabilidad = int.Parse(Session["Contabilidad"].ToString());
            obtengo_listas(tipo_contabilidad);
            int moneda_inicial = 0;
            moneda_inicial = DB.Get_Moneda_Inicial_By_Empresa(user, 3);
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
        
        int provID = 0;
        if (!Page.IsPostBack)
        {

            if (Request.QueryString["provID"] != null)
            {
                provID = int.Parse(Request.QueryString["provID"].ToString());
                RE_GenericBean provData = (RE_GenericBean)Utility.getDetalleProvision(provID);
                lb_fechacreacion.Text = provData.strC8;//fecha de creacion
                lb_proveedorID.Text = provData.intC3.ToString();
                lb_proveedorCCHID.Text = provData.intC4.ToString();
                lb_provID.Text = provData.intC1.ToString();
                tb_provID.Text = provID.ToString();
                tb_provID.Enabled = false;
                lb_moneda.SelectedValue = provData.intC5.ToString();
                lb_tipopersona.SelectedValue = provData.intC11.ToString();
                tb_hbl.Text = provData.strC13;
                tb_mbl.Text = provData.strC14;
                tb_routing.Text = provData.strC15;
                tb_contenedor.Text = provData.strC16;
                int proveedorCCHID = int.Parse(lb_proveedorCCHID.Text);
                RE_GenericBean proveedordata = null;
                int proveedorID = int.Parse(lb_proveedorID.Text);
                if (provData.intC11 == 2)
                { // es un agente
                    lb_transaccion.SelectedValue = "54";
                    proveedordata = (RE_GenericBean)DB.getAgenteData(proveedorID, "");// obtengo los datos del agente
                    lb_contribuyente.SelectedValue = DB.getProveedorRegimen(provData.intC11, proveedorID.ToString()).ToString();
                    tb_nit.Text = "";
                    if (proveedordata != null)
                    {
                        tb_nombre.Text = proveedordata.strC1;
                    }
                }
                else if (provData.intC11 == 4)
                {// es un proveedor 
                    lb_transaccion.SelectedValue = "25";
                    proveedordata = (RE_GenericBean)DB.getProveedorData(proveedorID, ""); //obtengo los datos del proveedor
                    lb_contribuyente.SelectedValue = DB.getProveedorRegimen(provData.intC11, proveedorID.ToString()).ToString();
                    tb_nit.Text = proveedordata.strC1;
                    tb_nombre.Text = proveedordata.strC2;
                }
                else if (provData.intC11 == 5)
                {// naviera
                    lb_transaccion.SelectedValue = "55";
                    proveedordata = (RE_GenericBean)DB.getNavieraData(proveedorID); //obtengo los datos del proveedor
                    lb_contribuyente.SelectedValue = DB.getProveedorRegimen(provData.intC11, proveedorID.ToString()).ToString();
                    tb_nit.Text = "";// proveedordata.strC1;
                    tb_nombre.Text = proveedordata.strC1;
                }
                else if (provData.intC11 == 6)
                {// Linea aerea
                    lb_transaccion.SelectedValue = "56";
                    proveedordata = (RE_GenericBean)DB.getCarriersData(proveedorID); //obtengo los datos del proveedor
                    lb_contribuyente.SelectedValue = DB.getProveedorRegimen(provData.intC11, proveedorID.ToString()).ToString();
                    tb_nit.Text = "";// proveedordata.strC1;
                    tb_nombre.Text = proveedordata.strC1;
                }
                else if (provData.intC11 == 8)
                {// Caja Chica
                    lb_transaccion.SelectedValue = "57";
                    proveedordata = (RE_GenericBean)DB.getProveedorData(proveedorCCHID, ""); //obtengo los datos del proveedor de caja chica
                    lb_contribuyente.SelectedValue = DB.getProveedorRegimen(4, proveedorCCHID.ToString()).ToString();
                    tb_nit.Text = proveedordata.strC1;
                    tb_nombre.Text = proveedordata.strC2;
                }
                else if (provData.intC11 == 10)
                {// Intercompany
                    lb_transaccion.SelectedValue = "107";
                    proveedordata = (RE_GenericBean)DB.getIntercompanyData(proveedorID);
                    lb_contribuyente.SelectedValue = DB.getProveedorRegimen(provData.intC11, proveedorID.ToString()).ToString();
                    tb_nit.Text = proveedordata.strC2;
                    tb_nombre.Text = proveedordata.strC1;
                }
                //DataTable rubdt = (DataTable)DB.getRubbyOC(provID, 5);//5=provision segun sys_tipo_referencia
                DataTable rubdt = (DataTable)DB.getRubByDoc_xNC(provID, 5, 12);
                gv_detalle.DataSource = rubdt;
                gv_detalle.DataBind();

                //Calculo de Dias del Documento para acreditar o no el IVA
                DateTime ahora = DateTime.Now;
                string[] f = lb_fechacreacion.Text.Split('/');
                DateTime fecha = new DateTime(int.Parse(f[2]), int.Parse(f[1]), int.Parse(f[0]));
                TimeSpan RangeTax = ahora.Subtract(fecha);
                LimitDays.Text = RangeTax.Days.ToString();
            }
        }
    }
    private void obtengo_listas(int tipo_contabilidad)
    {
        ListItem item = null;
        ArrayList arr = null;
        if (!Page.IsPostBack)
        {
            arr = (ArrayList)DB.getOpcionesMatriz("tbl_tipo_transaccion where ttt_template='notacreditoprov.aspx'");
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
            //arr = (ArrayList)DB.getSerieFactbySuc(user.SucursalID, 12, user, 0);//4 porque es el tipo de documento para nota debito segun sys_tipo_referencia
            //foreach (string valor in arr)
            //{
            //    item = new ListItem(valor, valor);
            //    lb_serie_factura.Items.Add(item);
            //}

            arr = (ArrayList)DB.Get_Series_By_Monedas_Conta(12, user, 1);
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
        }
    }    
    
    protected void bt_Enviar_Click(object sender, EventArgs e)
    {
        int transID = int.Parse(lb_transaccion.SelectedValue);//tipo de transaccion factura, invoice
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
        if (tb_fecha_libro_compras.Text.Trim().Equals("") && (bool.Parse(Rb_Documento.SelectedValue) == true))
        {
            WebMsgBox.Show("Debe seleccionar la Fecha del Libro de Compras");
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
        if ((tipopersona == 4) || (tipopersona == 8)) { transID = 8; } else { transID = 15; }//si tipopersona=4 (proveedores) provision proveedor sino provision agente
        decimal total = 0;
        total = decimal.Parse(tb_total.Text);

        if (total==0)
        {
            WebMsgBox.Show("Debe realizar un descuento en algun rubro.");
            return;
        }

        user = (UsuarioBean)Session["usuario"];
        RE_GenericBean rgb = new RE_GenericBean();
        rgb.Fecha_Hora = lb_fecha_hora.Text;
        rgb.intC1 = int.Parse(lb_transaccion.SelectedValue);//tipo transaccion
        rgb.intC2 = moneda;//tipo moneda
        rgb.intC3 = tipo_contabilidad;//tipo de contabilidad
        rgb.intC4 = int.Parse(tb_provID.Text);//Proveedor ID
        rgb.intC5 = 12;//sys tipo de transaccion
        rgb.intC6 = tipopersona;//tipo de persona
        rgb.intC7=int.Parse(lb_proveedorID.Text);
        rgb.decC1 = decimal.Parse(tb_total.Text.Trim());//monto a aplicar
        rgb.strC1 = tb_observaciones.Text;//observaciones
        rgb.strC2 = lb_serie_factura.SelectedItem.Text;
        rgb.strC3 = tb_referencia.Text; //Referencia
        rgb.decC3 = decimal.Parse(tb_iva.Text.Trim());//monto a aplicar
        rgb.strC9 = tb_hbl.Text;
        rgb.strC10 = tb_mbl.Text;
        rgb.strC11 = tb_contenedor.Text;
        rgb.strC12 = tb_routing.Text;
        rgb.Fiscal_No_Fiscal = bool.Parse(Rb_Documento.SelectedValue);
        rgb.Nombre_Cliente = tb_nombre.Text;//Nombre del Cliente
        rgb.strC14 = tb_nit.Text;
        rgb.IP_Address = Request.ServerVariables["REMOTE_ADDR"].ToString();
        string fecha_libro_compras = tb_fecha_libro_compras.Text;
        ArrayList Arr_Fechas = (ArrayList)DB.Formatear_Fechas(fecha_libro_compras, "");
        fecha_libro_compras = Arr_Fechas[0].ToString();
        rgb.strC13 = fecha_libro_compras;
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
        mat.tranID = 25;// int.Parse(lb_tipo_transaccion.SelectedValue);
        mat.monID = int.Parse(lb_moneda.SelectedValue);
        mat.contaID = rgb.intC3;
        mat.impexpID = int.Parse(lb_imp_exp.SelectedValue);
        mat.cobroID = tipo_cobro;
        mat.contriID = int.Parse(lb_contribuyente.SelectedValue);

        int matOpID = DB.getMatrizOperacionID(int.Parse(lb_transaccion.Text), mat.monID, user.PaisID, mat.contaID);
        ArrayList ctas_cargo = (ArrayList)DB.getMatrizConfiguracion(matOpID);


        ////recorro el datagrid para armar la factura
        Label lb1, lb2, lb3, lb6, lb7;
        TextBox tb1, tb2;
        Rubros rubro;
        foreach (GridViewRow row in gv_detalle.Rows)
        {
            tb1 = (TextBox)row.FindControl("tb_descuento");
            tb2 = (TextBox)row.FindControl("tb_descuento_impuesto");
            lb1 = (Label)row.FindControl("lb_codigo");
            lb2 = (Label)row.FindControl("lb_rubro");
            lb3 = (Label)row.FindControl("lb_tipo");
            lb6 = (Label)row.FindControl("lb_total");
            lb7 = (Label)row.FindControl("lb_totaldolares");
            rubro = new Rubros();
            rubro.rubroID = long.Parse(lb1.Text); ;
            rubro.rubroName = lb2.Text;
            rubro.rubtoType = lb3.Text;
            rubro.rubroTot = double.Parse(tb1.Text);
            rubro.rubroImpuesto = double.Parse(tb2.Text);
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
            //rubro.cta_debe = (ArrayList)DB.getCtaContablebyRubro("haber", (int)rubro.rubroID, user.PaisID, transID, contribuyente, moneda, imp_exp, tipo_cobro, tipo_contabilidad, servicio);
            rubro.cta_haber = (ArrayList)DB.getCtaContablebyRubro("debe", (int)rubro.rubroID, user.PaisID, transID, contribuyente, moneda, imp_exp, tipo_cobro, tipo_contabilidad, servicio);
            if (( rubro.cta_haber == null) || ( rubro.cta_haber.Count == 0))
            {
                WebMsgBox.Show("El rubro " + rubro.rubroName + " no tiene configurada ninguna cuenta contable para la combinacion de cobro que esta realizando, por favor pongase en contacto con el Contador");
                return;
            }
            if (rgb.arr1 == null) rgb.arr1 = new ArrayList();
            rgb.arr1.Add(rubro);
        }

        string Check_Existencia = DB.CheckExistDoc(rgb.Fecha_Hora, 12);
        if (Check_Existencia == "0")
        {
            ArrayList result = DB.InsertNotaCredito_provision(rgb, user, tipo_contabilidad, ctas_cargo);//4 ya que en la tabla tbl_tipo_persona 4=proveedor
            if (result != null && result.Count > 0)
            {
                WebMsgBox.Show("La Nota de Credito fue guardada exitosamente con Serie.: " + lb_serie_factura.SelectedItem.Text + " y Correlativo.: " + result[1].ToString());
                tb_NCprov.Text = result[0].ToString();
                bt_Enviar.Enabled = false;
                bt_imprimir.Enabled = true;

                //Mostrando la Partida contable generada
                gv_detalle_partida.DataSource = DB.getPolizaDiario(user, double.Parse(tb_NCprov.Text), 12, 0);
                gv_detalle_partida.DataBind();
                gv_detalle_partida.Rows[gv_detalle_partida.Rows.Count - 1].Font.Bold = true;
                return;
            }
            else
            {
                WebMsgBox.Show("Existió un problema al tratar de guardar la información, por favor intente de nuevo");
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

   
    protected void tb_descuento_TextChanged(object sender, EventArgs e)
    {
        double descuento = 0, subtotal = 0, total = 0, impuesto = 0, descuento_subtotal = 0, descuento_impuesto = 0;
        double porc_impuesto = double.Parse(user.pais.Impuesto.ToString());
        int tipo_contabilidad = int.Parse(Session["Contabilidad"].ToString());
        Label lb1, lb2, lb3, lb4, lb5, lb6, lb7; 
        TextBox tb1, tb2;
        foreach (GridViewRow row in gv_detalle.Rows)
        {
            tb1 = (TextBox)row.FindControl("tb_descuento");
            tb2 = (TextBox)row.FindControl("tb_descuento_impuesto");
            lb1 = (Label)row.FindControl("lb_subtotal");
            lb2 = (Label)row.FindControl("lb_impuesto");
            lb3 = (Label)row.FindControl("lb_total");
            lb4 = (Label)row.FindControl("lb_totaldolares");
            lb5 = (Label)row.FindControl("lb_monedatipo");
            lb6 = (Label)row.FindControl("lb_codigo");
            lb7 = (Label)row.FindControl("lb_saldo");
            if (lb5.Text.Equals("1")) lb5.Text = "GTQ";
            if (lb5.Text.Equals("2")) lb5.Text = "SVC";
            if (lb5.Text.Equals("3")) lb5.Text = "HNL";
            if (lb5.Text.Equals("4")) lb5.Text = "NIC";
            if (lb5.Text.Equals("5")) lb5.Text = "CRC";
            if (lb5.Text.Equals("6")) lb5.Text = "PAB";
            if (lb5.Text.Equals("7")) lb5.Text = "BZD";
            if (lb5.Text.Equals("8")) lb5.Text = "USD";
            #region Validar que el descuento sea menor o igual al total del rubro
            if (tb1.Text.Trim() == "")
            {
                tb1.Text = "0.00";
                tb2.Text = "0.00";
                return;
            }
            else
            {
                descuento = double.Parse(tb1.Text.Trim());
                if (descuento > double.Parse(lb7.Text))
                {
                    WebMsgBox.Show("El descuento no puede ser mayor a el monto de este rubro");
                    descuento = 0;
                    tb1.Text = "0.00";
                    tb2.Text = "0.00";
                    return;
                }
            }
            #endregion

            //*********************************************************************************************
            Rubros rub = new Rubros();
            //significa que si es contribuyente
            rub.rubroImpuesto = double.Parse(lb2.Text.Trim());

            //si se cobro Iva, y esta dentro del periodo de recuperacion permitido se descuenta el IVA
            if ((rub.rubroImpuesto > 0) && (descuento > 0) && (int.Parse(LimitDays.Text) <= user.pais.diasimpuesto))
            {
                descuento_subtotal = descuento / (1 + porc_impuesto);
                descuento_impuesto = Math.Round(descuento - descuento_subtotal, 2);
                tb2.Text = descuento_impuesto.ToString();
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
                tb1.Text = "0.00";
                tb2.Text = "0.00";
            }

        }
        tb_iva.Text = impuesto.ToString("#,#.00#;(#,#.00#)");
        tb_subtotal.Text = subtotal.ToString("#,#.00#;(#,#.00#)");
        tb_total.Text = total.ToString("#,#.00#;(#,#.00#)");
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
            else if (tranID == 107) { lb_tipopersona.SelectedValue = "10"; }//Intercompanys
        }
        #region Limpiar Tipo de Persona
        lb_proveedorID.Text = "";
        tb_nombre.Text = "";
        tb_nit.Text = "";
        #endregion
    }
    protected void lb_serie_factura_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (lb_serie_factura.Items.Count > 0)
        {
            if (lb_serie_factura.SelectedValue != "0")
            {
                int Moneda_Serie_Provision = 0;
                int Moneda_Serie_Nota_Credito = 0;
                //Moneda_Serie_Provision = DB.Get_Serie_Moneda_By_ID(int.Parse(lb_serie_factura.SelectedValue));
                Moneda_Serie_Provision = int.Parse(lb_moneda.SelectedValue);
                Moneda_Serie_Nota_Credito = DB.Get_Serie_Moneda_By_ID(int.Parse(lb_serie_factura.SelectedValue));
                if (Moneda_Serie_Provision != Moneda_Serie_Nota_Credito)
                {
                    lb_serie_factura.SelectedValue = "0";
                    WebMsgBox.Show("Por Favor seleccione una Serie con la misma Moneda de la Provision");
                    return;
                }
            }
        }
    }
}
