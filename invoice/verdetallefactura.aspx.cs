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
using System.IO;

public partial class invoice_viewrcpt : System.Web.UI.Page
{
    int factID = 0;
    UsuarioBean user = null;
    public string simboloequivalente = "USD";
    public string simbolomoneda = "GTQ";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["usuario"] == null)
        {
            Response.Redirect("../default.aspx");
        }

        user = (UsuarioBean)Session["usuario"];
        int impo_expo = 0;
        int tipo_contabilidad = int.Parse(Session["Contabilidad"].ToString());
        obtengo_listas(tipo_contabilidad, impo_expo, user.PaisID);
        #region Backup Seteo de Moneda
        /*
        if (tipo_contabilidad == 2)
        {
            lb_moneda.SelectedValue = "8";
        }

        else
        {
            lb_moneda.SelectedValue = user.pais.ID.ToString();
        }
         */ 
        #endregion
        //lb_moneda.SelectedValue = user.Moneda.ToString();
        //if (lb_imp_exp.Items.Count > 0)
        //    impo_expo = int.Parse(lb_imp_exp.SelectedValue);


        //if (lb_moneda.SelectedValue.Equals("1")) { simbolomoneda = "GTQ"; simboloequivalente = "USD"; }
        //if (lb_moneda.SelectedValue.Equals("2")) { simbolomoneda = "SVC"; simboloequivalente = "USD"; }
        //if (lb_moneda.SelectedValue.Equals("3")) { simbolomoneda = "HNL"; simboloequivalente = "USD"; }
        //if (lb_moneda.SelectedValue.Equals("4")) { simbolomoneda = "NIC"; simboloequivalente = "USD"; }
        //if (lb_moneda.SelectedValue.Equals("5")) { simbolomoneda = "CRC"; simboloequivalente = "USD"; }
        //if (lb_moneda.SelectedValue.Equals("6")) { simbolomoneda = "PAB"; simboloequivalente = "USD"; }
        //if (lb_moneda.SelectedValue.Equals("7")) { simbolomoneda = "BZD"; simboloequivalente = "USD"; }
        //if (lb_moneda.SelectedValue.Equals("8")) { simbolomoneda = "USD"; simboloequivalente = "Local"; }
        //gv_detalle.Columns[5].HeaderText = "Subtotal " + simbolomoneda;
        //gv_detalle.Columns[6].HeaderText = "Impuesto " + simbolomoneda;
        //gv_detalle.Columns[7].HeaderText = "Total " + simbolomoneda;
        //gv_detalle.Columns[8].HeaderText = "Equivalente " + simboloequivalente;


        #region Bloqueos Facturacion Electronica Costa Rica
        if ((user.PaisID == 5) || (user.PaisID == 21))
        {
            if (user.SucursalID == 117)
            {
                btn_re_imprimir.Visible = false;
            }
        }
        #endregion
         
        
        /* modulo de pruebas datos de terrestre para validar el ws baw 2020-04-15
        BAW_Provisionar_BL t = new BAW_Provisionar_BL ();
        int c = t.Provisionar_Costos(48817, 3, 6, "3", "patricia-inestroza");
        //BLID - 48817 - SysID 3 OP - 6 CT_BAW - 3 USU - patricia-inestroza
        string s = t.Validar_Cobros_Pendientes(48817, 6, "3");
        s = t.Get_Alerta(48817);
        */
    }

    private void obtengo_listas(int tipoconta, int impo_expo, int paiID)
    {
        ListItem item = null;
        ArrayList arr = null;
        if (!Page.IsPostBack)
        {
            arr = (ArrayList)DB.getOpcionesMatriz("tbl_tipo_transaccion where ttt_template='newinvoice.aspx'");
            foreach (RE_GenericBean rgb in arr)
            {
                item = new ListItem(rgb.strC1, rgb.intC1.ToString());
                lb_tipo_transaccion.Items.Add(item);
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
            if ((user.SucursalID == 15) || (user.SucursalID == 38) || (user.SucursalID == 26) || (user.SucursalID == 47)) 
            {
                arr = (ArrayList)DB.getSerieFactCreditos(user.SucursalID, 1, user, 0);//1 porque es el tipo de documento para facturacion
            }
            else
            {
                arr = (ArrayList)DB.getSerieFactbySuc(user.SucursalID, 1, user, 0);//1 porque es el tipo de documento para facturacion
            }
            foreach (string valor in arr)
            {
                item = new ListItem(valor, valor);
                lb_serie_factura.Items.Add(item);
            }
            
            //Desplegando Panel de Datos de Mayan
            if (user.PaisID == 13)
            {
                Pnl_Mayan_Logistics.Visible = true;
            }

            arr = null;
            arr = (ArrayList)DB.Get_Regimen_Aduanero_XPais(user.PaisID);
            drp_regimen_aduanero.Items.Clear();
            item = new ListItem(" - ", "0");
            drp_regimen_aduanero.Items.Add(item);
            foreach (RE_GenericBean rgb in arr)
            {
                item = new ListItem(rgb.strC2, rgb.strC1);
                drp_regimen_aduanero.Items.Add(item);
            }
        }
    }


    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            if (Request.QueryString["factID"] != null && !Request.QueryString["factID"].ToString().Equals(""))
            {
                factID = int.Parse(Request.QueryString["factID"].ToString().Trim());
                lb_facid.Text = factID.ToString();
                //Obtengo los rubros de la factura
                RE_GenericBean factdata = (RE_GenericBean)DB.getFacturaData(factID);
                tb_agente_nombre.Text = factdata.strC41;//Agente
                lb_serie_factura.SelectedValue = factdata.strC28;//Serie de la Factura
                tb_fecha_emision.Text = factdata.strC5;
                tb_correlativo.Text = factdata.strC1;
                tbCliCod.Text=factdata.intC3.ToString();
                tb_nombre.Text=factdata.strC3;
                tb_nit.Text=factdata.strC2;
                tb_razon.Text = factdata.strC26;
                lb_moneda.SelectedValue = factdata.intC4.ToString();
                tb_direccion.Text = factdata.strC4;
                tb_observaciones.Text = factdata.strC7;
                tb_observaciones2.Text = factdata.strC46;
                tb_referencia.Text = factdata.strC27;
                tb_naviera.Text = factdata.strC13;
                tb_vapor.Text = factdata.strC14;
                tb_contenedor.Text = factdata.strC11;
                tb_hbl.Text = factdata.strC9;
                tb_mbl.Text = factdata.strC10;
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
                tb_total.Text = factdata.decC3.ToString();
                tb_orden.Text = factdata.strC12;
                lb_ruta_pais.SelectedValue = factdata.strC47;
                lb_ruta.SelectedValue = factdata.strC48;
                lbl_tipo_serie.Text = factdata.strC50;
                lbl_internal_reference.Text = factdata.strC51;
                lbl_documentGUID.Text = factdata.strC52;
                lbl_correo_documento_electronico.Text = factdata.strC53;
                tb_referencia_correo.Text = factdata.strC54;
                tb_allin.Text = factdata.strC30;
                lb_contribuyente.SelectedValue = factdata.strC55;
                tb_poliza_aduanal.Text = factdata.strC16;
                drp_regimen_aduanero.SelectedValue = factdata.intC11.ToString();
                tb_reciboaduanal.Text = factdata.strC31.ToString();
                tb_valor_aduanero.Text = factdata.strC43.ToString();
                tb_recibo_agencia.Text = factdata.strC42.ToString();
                tb_no_factura_aduana.Text = factdata.strC56.ToString();
                tb_no_embarque.Text = factdata.strC57.ToString();
                drp_tipopersona.SelectedValue = factdata.strC58.ToString();
                tb_poliza_seguros.Text = factdata.strC59.ToString();
                if (factdata.strC58 == "10")
                {
                    #region Asignar Nombre Intercompany y Nombre Fiscal
                    RE_GenericBean Intercompany_Bean = (RE_GenericBean)DB.Get_Intercompany_Data(factdata.intC3);
                    tb_nombre.Text = "";
                    tb_nombre.Text = Intercompany_Bean.strC5 + "   (" + Intercompany_Bean.strC1 + ")";
                    #endregion
                }
                if (lbl_tipo_serie.Text == "1")
                {
                    var fel = DB.DataFEL(user.PaisID, factdata.strC49, factdata.strC52);
                    tb_resultado_transmision.Text = fel.firma;
                    if (fel.isFEL)
                    {
                        lbl_internal_reference.Text = factdata.strC49; //debe enviar el json FEL
                    }
                    else if (user.PaisID == 5 || user.PaisID == 21)
/*
                    if ((user.PaisID == 1) || (user.PaisID == 15))
                    {
                        if (DB.isFELDate(user.PaisID))
                        {//2019-07-29
                            tb_resultado_transmision.Text = DB.FirmaFEL("3", factdata.strC49, factdata.strC52); //, "", "Numero", "");
                            if (DB.isFEL("1",factdata.strC49))
                                lbl_internal_reference.Text = factdata.strC49;
                        } 
                        else
                            tb_resultado_transmision.Text = "Firma Electronica.: " + factdata.strC49;
                    }
                    else if (user.PaisID == 5)*/
                    {
                        tb_resultado_transmision.Text = "La Factura fue transmitida y guardada exitosamente con el Correlativo: " + tb_correlativo.Text + " y Código Único de Consulta.: " + factdata.strC49;
                    }
                    pnl_transmision_electronica.Visible = true;
                    pnl_documento_electronico.Visible = true;
                }

                if (lb_moneda.SelectedValue.Equals("1")) { simbolomoneda = "GTQ"; simboloequivalente = "USD"; }
                if (lb_moneda.SelectedValue.Equals("2")) { simbolomoneda = "SVC"; simboloequivalente = "USD"; }
                if (lb_moneda.SelectedValue.Equals("3")) { simbolomoneda = "HNL"; simboloequivalente = "USD"; }
                if (lb_moneda.SelectedValue.Equals("4")) { simbolomoneda = "NIC"; simboloequivalente = "USD"; }
                if (lb_moneda.SelectedValue.Equals("5")) { simbolomoneda = "CRC"; simboloequivalente = "USD"; }
                if (lb_moneda.SelectedValue.Equals("6")) { simbolomoneda = "PAB"; simboloequivalente = "USD"; }
                if (lb_moneda.SelectedValue.Equals("7")) { simbolomoneda = "BZD"; simboloequivalente = "USD"; }
                if (lb_moneda.SelectedValue.Equals("8")) { simbolomoneda = "USD"; simboloequivalente = "Local"; }
                gv_detalle.Columns[5].HeaderText = "Subtotal en " + simbolomoneda;
                gv_detalle.Columns[6].HeaderText = "Impuesto en " + simbolomoneda;
                gv_detalle.Columns[7].HeaderText = "Total en " + simbolomoneda;
                gv_detalle.Columns[8].HeaderText = "Equivalente en " + simboloequivalente;
                gv_detalle.DataSource = (DataTable)DB.getRubbyFact(factID, 1);
                gv_detalle.DataBind();

                gv_detalle_partida.DataSource = DB.getPolizaDiario(user, double.Parse(lb_facid.Text), 1, 0);
                gv_detalle_partida.DataBind();
                gv_detalle_partida.Rows[gv_detalle_partida.Rows.Count-1].Font.Bold = true;
                Determinar_Tipo_Serie(factdata.intC2, factdata.strC28);

                #region Automatizacion Intercompany Administrativo
                if (factdata.strC58 == "10")
                {
                    string mensaje = "";
                    ArrayList Arr_Transacciones_Hijas = (ArrayList)DB.Get_Transacciones_Encadenadas_Hijas(1, factID);
                    if (Arr_Transacciones_Hijas == null)
                    {
                        WebMsgBox.Show("Existio un error al Tratar de Obtener las Transacciones Encadenadas Hijas");
                        return;
                    }
                    foreach (RE_GenericBean Hija in Arr_Transacciones_Hijas)
                    {
                        ArrayList Arr_Temp = DB.getSerieCorrAndObsByDoc(int.Parse(Hija.strC2), int.Parse(Hija.strC3));
                        mensaje += "Documento Serie.: " + Arr_Temp[1].ToString() + " Correlativo.: " + Arr_Temp[2].ToString() + " ";
                    }
                    if (Arr_Transacciones_Hijas.Count > 0)
                    {
                        string Nombre_Empresa_Destino = "";
                        string Nombre_Empresa_Origen = "";
                        RE_GenericBean Intercompany_Bean = (RE_GenericBean)DB.Get_Intercompany_Data(Convert.ToInt32(factdata.intC3));
                        PaisBean Pais_Temporal = (PaisBean)DB.getPais(Intercompany_Bean.intC3);
                        Nombre_Empresa_Destino = Pais_Temporal.Nombre_Sistema;
                        Pais_Temporal = (PaisBean)DB.getPais(user.PaisID);
                        Nombre_Empresa_Origen = Pais_Temporal.Nombre_Sistema;
                        tb_resultado_automatizacion.Text = "Se ha creado Automaticamente un documento Por Pagar.: " + "\r\n";
                        tb_resultado_automatizacion.Text += "    * En Sistema BAW - " + Nombre_Empresa_Destino + "\r\n";
                        tb_resultado_automatizacion.Text += "    * A " + Nombre_Empresa_Origen + "\r\n";
                        tb_resultado_automatizacion.Text += mensaje;
                        pnl_intercompanys.Visible = true;
                    }
                }
                #endregion

                if (factdata.strC58 == "10")
                {
                    lb_tipo_transaccion.SelectedValue = "108";
                }
                else
                {
                    if (factdata.intC9 == 2)
                    {
                        lb_tipo_transaccion.SelectedValue = "7";
                    }
                    else
                    {
                        lb_tipo_transaccion.SelectedValue = "1";
                    }
                }
            }
        }
        if ((user.PaisID == 1 || user.PaisID == 15) && lb_moneda.SelectedValue == "8")
        {
            bt_impresion_invoice.Visible = true;
        }
    }
    protected void btn_re_imprimir_Click(object sender, EventArgs e)
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
            user.ImpresionBean.Operacion = "2";
            user.ImpresionBean.Tipo_Documento = "1";
            user.ImpresionBean.Id = lb_facid.Text;
            user.ImpresionBean.Impreso = true;
            Session["usuario"] = user;
            string script = "";
            String csname1 = "PopupScript";
            Type cstype = this.GetType();
            // Get a ClientScriptManager reference from the Page class.
            ClientScriptManager cs2 = Page.ClientScript;

            if (((user.PaisID == 6) || (user.PaisID == 25)) && (lbl_tipo_serie.Text == "1"))
            { //En el caso de Panama utiliza la impresion especial para Impresora Fiscal Tally1125
                script = "window.open('printersettingsPA.aspx?fac_id=" + lb_facid.Text + "&tipo=1','Imprimir','toolbar=0, status=1, resizable=1, scrollbars=1, top=0, left=0, width=' + (window.screen.width -10) + ', height=' + (window.screen.height -75))";
            }
            else
            {
                #region Impresion Electronica
                if (((user.PaisID == 1) || (user.PaisID == 15)) && ((lbl_tipo_serie.Text == "1") || (lbl_tipo_serie.Text == "2")))
                {
                    #region Validar Si el Documento esta Firmado
                    if (lbl_documentGUID.Text == "0")
                    {
						//if (user.PaisID == 1) 
    	                    WebMsgBox.Show("Este documento no fue procesado ni firmado por FEL");
						//else
	                    //    WebMsgBox.Show("Este documento no fue procesado ni firmado por el GFACE");
                        btn_re_imprimir.Enabled = false;
                        return;
                    }
                    #endregion

                    //if (DB.isFELDate(user.PaisID))
                    { //2019-07-29
                        if (!DB.DownloadFEL(lb_facid.Text, "1", "PDF", Response, lbl_internal_reference.Text, user.PaisID, true, true))
                            DB.DownloadGFACE(lbl_internal_reference.Text, Response, user, lbl_documentGUID.Text);
                    } 
                    //else
                    {
                        //DB.DownloadGFACE(lbl_internal_reference.Text, Response, user, lbl_documentGUID.Text);
                        /*#region Obtener Documento Electronico
                        string FilePath = "D:\\EINVOICE\\" + lbl_internal_reference.Text + ".pdf";
                        if (File.Exists(FilePath) == true)
                        {
                            #region Descargar Archivo
                            Response.Clear();
                            Response.ContentType = "application/octet-stream";
                            Response.AddHeader("Content-Disposition", "attachment; filename=" + lbl_internal_reference.Text + ".pdf");
                            Response.Flush();
                            Response.WriteFile(FilePath);
                            Response.End();
                            #endregion
                        }
                        else if (File.Exists(FilePath) == false)
                        {
                            GFACEWEBSERVICE.TransactionTag Tag = new GFACEWEBSERVICE.TransactionTag();
                            Tag = (GFACEWEBSERVICE.TransactionTag)DB.ObtenerDocumentoTransmitido(user, lbl_documentGUID.Text, lbl_internal_reference.Text);
                            if (Tag == null)
                            {
                                WebMsgBox.Show("No se pudo establecer conexion con el GFACE");
                            }
                            else if (Tag.Response.Result == false)
                            {
                                WebMsgBox.Show("Este documento no fue procesado ni firmado por el GFACE");
                            }
                            else if (Tag.Response.Result == true)
                            {
                                if (File.Exists(FilePath) == true)
                                {
                                    #region Descargar Archivo
                                    Response.Clear();
                                    Response.ContentType = "application/octet-stream";
                                    Response.AddHeader("Content-Disposition", "attachment; filename=" + lbl_internal_reference.Text + ".pdf");
                                    Response.Flush();
                                    Response.WriteFile(FilePath);
                                    Response.End();
                                    #endregion
                                }
                                else
                                {
                                    WebMsgBox.Show("Ruta de acceso al archivo de impresion invalida");
                                }
                            }
                        }
                        #endregion*/
                    }
                }
                else
                {
                    script = "window.open('re_print.aspx?fac_id=" + lb_facid.Text + "&s=" + lb_serie_factura.SelectedItem.Text + "&c=" + tb_correlativo.Text.Trim() + "&tipo=1','Imprimir','menubar=1,resizable=1,scrollbars=1,toolbar=1,width=750');";
                }
                #endregion
            }
            if (!cs2.IsStartupScriptRegistered(cstype, csname1))
            {
                cs2.RegisterStartupScript(cstype, csname1, script, true);
            }
        }
    }
    protected void gv_detalle_DataBound(object sender, EventArgs e)
    {
        decimal subtotal = 0;
        decimal impuesto = 0;
        decimal total = 0;
        decimal totalD = 0;
        Label lb1, lb2, lb3, lb4;
        foreach (GridViewRow row in gv_detalle.Rows)
        {
            lb1 = (Label)row.FindControl("lb_subtotal");
            lb2 = (Label)row.FindControl("lb_impuesto");
            lb3 = (Label)row.FindControl("lb_total");
            lb4 = (Label)row.FindControl("lb_totaldolares");
            subtotal += Math.Round(decimal.Parse(lb1.Text), 2);
            impuesto += Math.Round(decimal.Parse(lb2.Text), 2);
            total += Math.Round(decimal.Parse(lb3.Text), 2);
            totalD += Math.Round(decimal.Parse(lb4.Text), 2);
        }
        tb_subtotal.Text = subtotal.ToString("#,#.00#;(#,#.00#)");
        tb_impuesto.Text = impuesto.ToString("#,#.00#;(#,#.00#)");
        tb_total.Text = total.ToString("#,#.00#;(#,#.00#)");
        tb_totaldolares.Text = totalD.ToString("#,#.00#;(#,#.00#)");
    }
    protected void bt_factura_virtual_Click(object sender, EventArgs e)
    {
        user = (UsuarioBean)Session["usuario"];
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
            ClientScriptManager cs2 = Page.ClientScript;
            string script = "window.open('../invoice/template_invoice.aspx?id=" + lb_facid.Text + "&transaccion=1','Imprimir','menubar=1,resizable=1,scrollbars=1,toolbar=1,width=750');";
            if (!cs2.IsStartupScriptRegistered(cstype, csname1))
            {
                cs2.RegisterStartupScript(cstype, csname1, script, true);
            }
        }
    }
    protected void Determinar_Tipo_Serie(int sucID, string serie)
    {
        int Doc_ID = DB.getDocumentoID(sucID, serie, 1, user);
        RE_GenericBean Bean_Serie = (RE_GenericBean)DB.getFactura(Doc_ID, 1);
        if (Bean_Serie == null)
        {
            WebMsgBox.Show("Existio un error tratando de determinar el Tipo de Documento, por lo cual no fue transmitido");
            return;
        }
        else
        {
            lbl_tipo_serie_caption.Font.Bold = true;
            if (user.PaisID == 1)
            {
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
    protected void bt_impresion_invoice_Click(object sender, EventArgs e)
    {
        user = (UsuarioBean)Session["usuario"];
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
            ClientScriptManager cs2 = Page.ClientScript;
            string script = "window.open('../invoice/template_invoice_en.aspx?id=" + lb_facid.Text + "&transaccion=1','Imprimir Invoice','menubar=1,resizable=1,scrollbars=1,toolbar=1,width=750');";
            if (!cs2.IsStartupScriptRegistered(cstype, csname1))
            {
                cs2.RegisterStartupScript(cstype, csname1, script, true);
            }
        }
    }
}
