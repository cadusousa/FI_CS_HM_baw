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
    int ncID = 0;
    UsuarioBean user = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["usuario"] == null)
        {
            Response.Redirect("../default.aspx");
        }

        user = (UsuarioBean)Session["usuario"];

        ArrayList Arr_Perfiles = (ArrayList)DB.getPerfilesxUsu(user.ID, user.PaisID);
        int b_permiso = 0;
        foreach (PerfilesBean Bean in Arr_Perfiles)
        {
            if (Bean.ID == 33)
            {
                b_permiso = 1;
            }
        }

        if (b_permiso == 0)
        {
            if (!user.Aplicaciones.Contains("5"))
            {
                Response.Redirect("index.aspx");
            }
            int permiso = int.Parse(user.Aplicaciones["5"].ToString());
            if (!((permiso & 128) == 128))
            {
                Response.Redirect("index.aspx");
            }
        }

        int impo_expo = 0;
        int tipo_contabilidad = int.Parse(Session["Contabilidad"].ToString());
        obtengo_listas(tipo_contabilidad, impo_expo, user.PaisID);
        if (tipo_contabilidad == 2)
        {
            lb_moneda.SelectedValue = user.Moneda.ToString();
        }
        else
        {
            #region Backup
            //lb_moneda.SelectedValue = user.pais.ID.ToString();
            #endregion
            lb_moneda.SelectedValue = user.Moneda.ToString();
        }
        if (lb_imp_exp.Items.Count > 0)
            impo_expo = int.Parse(lb_imp_exp.SelectedValue);


        #region Bloqueos Facturacion Electronica Costa Rica
        if ((user.PaisID == 5) || (user.PaisID == 21))
        {
            if (user.SucursalID == 117)
            {
                bt_Enviar.Visible = false;
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
            //llena serie de factura
            #region llega combo 
            //arr = null;
            //arr = (ArrayList)DB.getSerieFactbySuc(user.SucursalID, 1, user);//1 porque es el tipo de documento para facturacion
            //foreach (string valor in arr)
            //{
            //    item = new ListItem(valor, valor);
            //    lb_serie_factura.Items.Add(item);
            //}
            #endregion
            arr = null;
            arr = (ArrayList)DB.getSerieFactbySuc(user.SucursalID, 3, user, 1);//1 porque es el tipo de documento para facturacion
            foreach (string valor in arr)
            {
                item = new ListItem(valor, valor);
                lb_serieNC.Items.Add(item);
            }
            
        }
    }
    
    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            if (Request.QueryString["ncID"] != null && !Request.QueryString["ncID"].ToString().Equals(""))
            {
                ncID = int.Parse(Request.QueryString["ncID"].ToString().Trim());
                //Obtengo los rubros de la factura
                RE_GenericBean factdata = (RE_GenericBean)DB.getNotaCreditoData(ncID);
                factID = factdata.intC1;
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
                tb_bl.Text = factdata.strC9;
                tb_shipper.Text = factdata.strC15;
                tb_orden.Text = factdata.strC16;
                tb_consignee.Text = factdata.strC17;
                tb_comodity.Text = factdata.strC18;
                tb_paquetes1.Text = factdata.strC32;
                tb_paquetes2.Text = factdata.strC19;
                tb_peso.Text = factdata.strC20;
                tb_vol.Text = factdata.strC21;
                tb_dua_ingreso.Text = factdata.strC22;
                tb_dua_salida.Text = factdata.strC23;
                tb_vendedor1.Text = factdata.strC24;
                tb_vendedor2.Text = factdata.strC25;
                tb_fecha.Text = factdata.strC5;
                lb_serie_factura.Items.Add(factdata.strC28);
                lb_facid.Text = factdata.strC1;
                tb_corrNC.Text = factdata.strC33;
                lb_serieNC.SelectedValue = factdata.strC32;
                lb_serieNC.Enabled = false;
                lb_serie_factura.Enabled = false;
                tb_poliza.Text = factdata.strC34;//Poliza Aduanal
                lbl_internal_reference.Text = factdata.strC51;
                lbl_documentGUID.Text = factdata.strC52;
                lbl_correo_documento_electronico.Text = factdata.strC53;
                tb_referencia_correo.Text = factdata.strC54;
                lbl_tipo_serie.Text = factdata.strC50;
                tb_allin.Text = factdata.strC30;
                dgw.DataSource = (DataTable)DB.getRubbyNC(ncID,3);
                dgw.DataBind();

                //Mostrando la Partida contable generada
                gv_detalle_partida.DataSource = DB.getPolizaDiario(user, double.Parse(ncID.ToString()), 3, 0);
                gv_detalle_partida.DataBind();
                gv_detalle_partida.Rows[gv_detalle_partida.Rows.Count - 1].Font.Bold = true;
                Determinar_Tipo_Serie(factdata.intC2, factdata.strC32);

                if (lbl_tipo_serie.Text == "1")
                {                    
                    var fel = DB.DataFEL(user.PaisID, factdata.strC49, factdata.strC52);                      
                    tb_resultado_transmision.Text = fel.firma;
                    if (fel.isFEL)
                    {
                        lbl_internal_reference.Text = factdata.strC49; //debe enviar el json FEL
                    }
                    else if (user.PaisID == 5 || user.PaisID == 21)
                    {
                        tb_resultado_transmision.Text = "La Nota de Credito fue transmitida y guardada exitosamente con el Correlativo: " + tb_corrNC.Text + " y Código Único de Consulta.: " + factdata.strC49;
                    }
                    pnl_transmision_electronica.Visible = true;
                    pnl_documento_electronico.Visible = true;
                }
            }
        }
    }

    protected void bt_Enviar_Click(object sender, EventArgs e)
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
            ncID = int.Parse(Request.QueryString["ncID"].ToString().Trim());
            user.ImpresionBean.Operacion = "2";
            user.ImpresionBean.Tipo_Documento = "3";
            user.ImpresionBean.Id = ncID.ToString();
            user.ImpresionBean.Impreso = true;
            Session["usuario"] = user;
            string script = "";
            if (((user.PaisID == 6) || (user.PaisID == 25)) && (lbl_tipo_serie.Text == "1"))
            {
                //En el caso de Panama utiliza la impresion especial para Impresora Fiscal Tally1125, parametro op=0=impresion
                script = "window.open('printersettingsPA.aspx?fac_id=" + ncID + "&tipo=3','Imprimir','toolbar=0, status=1, resizable=1, scrollbars=1, top=0, left=0, width=' + (window.screen.width -10) + ', height=' + (window.screen.height -75))";
            }
            else
            {
                #region Impresion Electronica
                if (((user.PaisID == 1) || (user.PaisID == 15)) && ((lbl_tipo_serie.Text == "1") || (lbl_tipo_serie.Text == "2")))
                {
                    //if (DB.isFELDate(user.PaisID))
                    { //2019-07-29
                        if (!DB.DownloadFEL(Request.QueryString["ncID"].ToString().Trim(), "3", "PDF", Response, lbl_internal_reference.Text, user.PaisID, true, true)) 
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
                                    //System.Diagnostics.Process proc = new System.Diagnostics.Process();
                                    //proc.StartInfo.FileName = FilePath;
                                    //proc.Start();
                                    //proc.Close();
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
                    script = "window.open('re_print.aspx?fac_id=" + ncID + "&s=" + lb_serieNC.SelectedItem.Text + "&c=" + tb_corrNC.Text.Trim() + "&tipo=3','Imprimir','menubar=1,resizable=1,scrollbars=1,toolbar=1,width=750');";
                }
                #endregion
            }

            if (!cs2.IsStartupScriptRegistered(cstype, csname1))
            {
                cs2.RegisterStartupScript(cstype, csname1, script, true);
            }
        }
    }
    protected void dgw_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        GridViewRow row;
        decimal descuento = 0, total = 0;
        TextBox t1;
        for (int i = 0; i < dgw.Rows.Count; i++)
        {
            row = dgw.Rows[i];
            if (row.RowType == DataControlRowType.DataRow)
            {
                t1 = (TextBox)row.FindControl("TextBox1");
                descuento = decimal.Parse(t1.Text.Trim());
                total += descuento;
            }
        }
        tb_total.Text = total.ToString();
    }
    protected void bt_nota_credito_virtual_Click(object sender, EventArgs e)
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
            // Get a ClientScriptManager reference from the Page class.
            ClientScriptManager cs2 = Page.ClientScript;
            string script = "window.open('../invoice/template_invoice.aspx?id=" + Request.QueryString["ncID"].ToString() + "&transaccion=3','Imprimir','menubar=1,resizable=1,scrollbars=1,toolbar=1,width=750');";
            if (!cs2.IsStartupScriptRegistered(cstype, csname1))
            {
                cs2.RegisterStartupScript(cstype, csname1, script, true);
            }
        }
    }
    protected void Determinar_Tipo_Serie(int sucID, string serie)
    {
        if (user.PaisID == 1)
        {
            int Doc_ID = DB.getDocumentoID(sucID, serie, 3, user);
            RE_GenericBean Bean_Serie = (RE_GenericBean)DB.getFactura(Doc_ID, 3);
            if (Bean_Serie == null)
            {
                WebMsgBox.Show("Existio un error tratando de determinar el Tipo de Documento");
                return;
            }
            else
            {
                lbl_tipo_serie.Text = Bean_Serie.intC14.ToString();
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
    }
}
