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

public partial class invoice_viewrcpt : System.Web.UI.Page
{
    int factID = 0;
    UsuarioBean user = null;

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
        if (!((permiso & 128) == 128))
        {
            Response.Redirect("index.aspx");
        }

        int impo_expo = 0;
        int tipo_contabilidad = int.Parse(Session["Contabilidad"].ToString());
        obtengo_listas(tipo_contabilidad, impo_expo, user.PaisID);
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
        if (lb_imp_exp.Items.Count > 0)
            impo_expo = int.Parse(lb_imp_exp.SelectedValue);

        if (lb_serieNC.SelectedItem == null)
        {
            bt_Enviar.Enabled = false;
            WebMsgBox.Show("No existe Serie configurada para esta sucursal");
            return;
        }


    }
    private void obtengo_listas(int tipoconta, int impo_expo, int paiID)
    {
        ListItem item = null;
        ArrayList arr = null;
        if (!Page.IsPostBack)
        {
            arr = (ArrayList)DB.getOpcionesMatriz("tbl_tipo_transaccion where ttt_template='nc_ajustecontable.aspx'");
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
            if (user.SucursalID == 15)
            {
                arr = (ArrayList)DB.getSerieFactCreditos(user.SucursalID, 1, user,0);//1 porque es el tipo de documento para facturacion
            }
            else
            {
                arr = (ArrayList)DB.getSerieFactbySuc(user.SucursalID, 1, user,0);//1 porque es el tipo de documento para facturacion
            }
            foreach (string valor in arr)
            {
                item = new ListItem(valor, valor);
                lb_serie_factura.Items.Add(item);
            }

            arr = null;
            arr = (ArrayList)DB.getSerieFactbySuc(user.SucursalID, 18, user, 1);//1 porque es el tipo de documento para facturacion
            foreach (string valor in arr)
            {
                item = new ListItem(valor, valor);
                lb_serieNC.Items.Add(item);
            }
            
        }
    }
    protected void TextBox1_TextChanged(object sender, EventArgs e)
    {
        DateTime ahora = DateTime.Now;
        string[] f = tb_fecha.Text.Split('/');
        //DateTime fecha = new DateTime(int.Parse(f[2]), int.Parse(f[1]), int.Parse(f[0]));
        DateTime fecha = new DateTime(int.Parse(f[0]), int.Parse(f[1]), int.Parse(f[2]));
        TimeSpan tm = new TimeSpan(user.pais.diasimpuesto, 0, 0, 0);
        int x = ahora.CompareTo(fecha.Add(tm));


        GridViewRow row;
        decimal descuento = 0, total = 0;

        decimal impuesto = 0;
        int tipo_contabilidad = int.Parse(Session["Contabilidad"].ToString());
        TextBox t1;
        for (int i = 0; i < dgw.Rows.Count; i++)
        {
            row = dgw.Rows[i];
            if (row.RowType == DataControlRowType.DataRow)
            {
                t1 = (TextBox)row.FindControl("TextBox1");
                if (t1.Text.Trim() == "")
                {
                    t1.Text = "0.00";
                }
                else
                {
                    descuento = decimal.Parse(t1.Text.Trim());
                    if (descuento > decimal.Parse(row.Cells[8].Text.Trim()))
                    {
                        WebMsgBox.Show("El descuento no puede ser mayor a el monto de este rubro");
                        descuento = 0;
                        t1.Text = "0.00";
                    }

                    //*********************************************************************************************
                    Rubros rubtemp = new Rubros();
                    Rubros rub = new Rubros();
                    rub.rubroID = int.Parse(row.Cells[1].Text.Trim());
                    rub.rubroSubTot = double.Parse(row.Cells[6].Text.Trim());
                    rub.rubroTot = double.Parse(row.Cells[8].Text.Trim());
                    rubtemp = (Rubros)DB.ExistRubroPais(rub, user.PaisID);
                    if (rubtemp == null)
                    {
                        WebMsgBox.Show("Error, El HBL no tiene todos los rubros registrados en contabilidad para este pais, por favor pongase en contacto con el departamento de contabilidad para agregarlos.");
                        return;
                    }
                    if ((double.Parse(row.Cells[7].Text.Trim()) > 0) && (rubtemp.CobIva == 1) && (tipo_contabilidad != 2) && x <= 0)//si debe cobrar iva y el rubro no esta en dolares y no es excento
                    {
                        impuesto += (descuento * user.pais.Impuesto);
                        total += descuento;
                    }
                    else
                    {
                        total += descuento;
                    }
                    #region Backup
                    //if (lb_contribuyente.SelectedValue.Equals("2"))
                    //{  //significa que si es contribuyente
                    //    rub.rubroID = int.Parse(row.Cells[1].Text.Trim());
                    //    rub.rubroSubTot = double.Parse(row.Cells[6].Text.Trim());
                    //    rub.rubroTot = double.Parse(row.Cells[8].Text.Trim());
                    //    rubtemp = (Rubros)DB.ExistRubroPais(rub, user.PaisID);
                    //    if (rubtemp == null)
                    //    {
                    //        WebMsgBox.Show("Error, El HBL no tiene todos los rubros registrados en contabilidad para este pais, por favor pongase en contacto con el departamento de contabilidad para agregarlos.");
                    //        return;
                    //    }
                    //    if ((rubtemp.CobIva == 1) && (tipo_contabilidad != 2) && x <= 0)//si debe cobrar iva y el rubro no esta en dolares y no es excento
                    //    {
                    //        impuesto += (descuento * user.pais.Impuesto);
                    //        //total += descuento - impuesto;
                    //        total += descuento;
                    //    }
                    //    else
                    //    {
                    //        total += descuento;
                    //    }
                    //}
                    //else
                    //{
                    //    total += descuento;
                    //}
                    #endregion
                }
            }
        }
        impuesto = Math.Round(impuesto, 2);
        total = Math.Round(total, 2);
        tb_iva.Text = impuesto.ToString("#,#.00#;(#,#.00#)");
        tb_total.Text = total.ToString("#,#.00#;(#,#.00#)");
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            if (Request.QueryString["factID"] != null && !Request.QueryString["factID"].ToString().Equals(""))
            {
                factID = int.Parse(Request.QueryString["factID"].ToString().Trim());
                //Obtengo los rubros de la factura
                RE_GenericBean factdata = (RE_GenericBean)DB.getFacturaData(factID);
                lb_serie_factura.SelectedValue = factdata.strC28;//Serie
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
                lb_serie_factura.SelectedValue = factdata.strC28;
                lb_facid.Text = factdata.strC1;
                lb_contribuyente.SelectedValue = factdata.strC55;
                dgw.DataSource = (DataTable)DB.getRubbyFact(factID,1);
                dgw.DataBind();
                #region Definir Moneda y serie del Documento en Base al tipo de Contabilidad
                int tipo_contabilidad = int.Parse(Session["Contabilidad"].ToString());
                if (tipo_contabilidad == 2)
                {
                    #region Backup
                    //lb_moneda.SelectedValue = "8";
                    #endregion
                    lb_moneda.SelectedValue = user.Moneda.ToString();
                }
                else
                {
                    #region Backup
                    //lb_moneda.SelectedValue = user.pais.ID.ToString();
                    #endregion
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
            }
        }
    }

    protected void bt_Enviar_Click(object sender, EventArgs e)
    {
        if (tb_total.Text.Trim().Equals(""))
        {
            WebMsgBox.Show("Debe Indicar el credito para aplicar.");
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
            //88888888888888888888888888888888888888888888888888888888888888888888888888888888888
            user = (UsuarioBean)Session["usuario"];
            ////Pendiente el tema de si es menor de 2 meses
            RE_GenericBean rgb = new RE_GenericBean();
            rgb.Fecha_Hora = lb_fecha_hora.Text;
            rgb.intC1 = int.Parse(lb_tipo_transaccion.SelectedValue);//tipo transaccion
            rgb.intC2 = int.Parse(lb_moneda.SelectedValue);//tipo moneda
            rgb.intC3 = int.Parse(Session["Contabilidad"].ToString());//tipo de contabilidad
            rgb.intC4 = int.Parse(Request.QueryString["factID"].ToString().Trim());
            rgb.intC5 = 18;//tipo de transaccion
            rgb.intC6 = 3;//tipo de persona
            rgb.intC7 = int.Parse(tbCliCod.Text.Trim());//Cliente ID
            rgb.decC1 = decimal.Parse(tb_total.Text.Trim());//monto a aplicar
            rgb.strC1 = tb_observaciones.Text;//observaciones
            rgb.strC2 = lb_serie_factura.SelectedValue;
            rgb.strC20 = lb_serieNC.SelectedValue;
            //Parametros Agregados
            rgb.strC9 = tb_hbl.Text;
            rgb.strC10 = tb_mbl.Text;
            rgb.strC11 = tb_contenedor.Text;
            rgb.strC12 = tb_routing.Text;
            rgb.decC3 = decimal.Parse(tb_iva.Text);
            rgb.Nombre_Cliente = tb_nombre.Text.Trim();//Nombre del Cliente
            //
            if (lb_moneda.SelectedValue.Equals("8"))
            {
                rgb.decC2 = Math.Round((rgb.decC1 * user.pais.TipoCambio), 2);
            }
            else
            {
                rgb.decC2 = Math.Round((rgb.decC1 / user.pais.TipoCambio), 2);
            }

            ////************************************************** obtengo las cuentas *****************
            MatOpBean mat = new MatOpBean();
            mat.paiID = user.PaisID;
            mat.monID = int.Parse(lb_moneda.SelectedValue);
            mat.contaID = rgb.intC3;
            if (mat.contaID == 1) {
                mat.tranID = 1;// Fiscal Factura
            }
            else{
                mat.tranID = 7;// Financiera Invoice
            }
            mat.impexpID = int.Parse(lb_imp_exp.SelectedValue);
            mat.cobroID = int.Parse(lb_tipocobro.Text);
            mat.contriID = int.Parse(lb_contribuyente.SelectedValue);



            int matOpID = DB.getMatrizOperacionID(48, mat.monID, user.PaisID, mat.contaID);
            ArrayList ctas_cargo = (ArrayList)DB.getMatrizConfiguracion(matOpID);
            
            #region Definir Estado de Factura
            if ((chk_anulacion.Visible == true)&&(chk_anulacion.Checked==true))
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
            //recorro el datagrid para aramar la factura
            Label lb1 = new Label();
            Label lb2 = new Label();
            Label lb3 = new Label();
            Label lb4 = new Label();
            Label lb5 = new Label();
            Label lb6 = new Label();
            TextBox t1 = new TextBox();
            Rubros rubro;
            foreach (GridViewRow row in dgw.Rows)
            {
                lb1.Text = row.Cells[1].Text.Trim();
                lb2.Text = row.Cells[2].Text.Trim();
                lb6.Text = row.Cells[3].Text.Trim();
                lb3.Text = row.Cells[6].Text.Trim();
                lb4.Text = row.Cells[7].Text.Trim();
                lb5.Text = row.Cells[8].Text.Trim();
                t1 = (TextBox)row.FindControl("TextBox1");
                if (t1.Text == "")
                { t1.Text = "0"; }
                rubro = new Rubros();
                rubro.rubroID = long.Parse(lb1.Text); ;
                rubro.rubroName = lb2.Text;
                rubro.rubroSubTot = double.Parse(lb3.Text);
                rubro.rubroImpuesto = double.Parse(lb4.Text);
                rubro.rubroTot = double.Parse(t1.Text);
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
            rgb.intC10 = 1;
            DateTime fecha = DateTime.Parse(tb_fecha.Text);
            if (fecha.CompareTo(DateTime.Now) < 0) rgb.boolC2 = true;
            string Check_Existencia = DB.CheckExistDoc(rgb.Fecha_Hora, 3);
            if (Check_Existencia == "0")
            {
                ArrayList result = DB.InsertNC_AjusteContable(rgb, user, rgb.intC3, ctas_cargo);
                if (result == null || result.Count == 0 || int.Parse(result[1].ToString()) == 0 || int.Parse(result[0].ToString()) == -100)
                {
                    WebMsgBox.Show("Existió un problema al tratar de guardar la informacion, por favor intente de nuevo");
                    return;
                }
                else
                {
                    WebMsgBox.Show("El ajuste contable se grabo exitosamente con el numero " + lb_serieNC.SelectedValue + result[0].ToString());
                    tb_corrNC.Text = result[0].ToString();
                    bt_Enviar.Enabled = false;
                    bt_Imprimir.Enabled = true;
                    bt_factura_virtual.Enabled = true;
                    factID = int.Parse(Request.QueryString["factID"].ToString().Trim());
                    FacturaBean Estado_Factura = (FacturaBean)DB.getEstadoFactura(factID);
                    lbl_total_factura.Text = Estado_Factura.Total.ToString();
                    lbl_total_abonos.Text = Estado_Factura.SubTot.ToString();
                    lbl_saldo.Text = Math.Round((Estado_Factura.Total - Estado_Factura.SubTot), 2).ToString();

                    //Mostrando la Partida contable generada
                    gv_detalle_partida.DataSource = DB.getPolizaDiario(user, double.Parse(result[1].ToString()), 18, 0);
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
        int ncID = DB.getCorrelativoDoc(user.SucursalID, 18, lb_serieNC.SelectedItem.Text, tb_corrNC.Text);
        String csname1 = "PopupScript";
        Type cstype = this.GetType();
        // Get a ClientScriptManager reference from the Page class.
        ClientScriptManager cs2 = Page.ClientScript;
        string script = "window.open('print.aspx?fac_id=" + ncID.ToString() + "&tipo=18','Imprimir','toolbar=0, status=1, resizable=1, scrollbars=1, top=0, left=0, width=' + (window.screen.width -10) + ', height=' + (window.screen.height -75))";

        if (!cs2.IsStartupScriptRegistered(cstype, csname1))
        {
            cs2.RegisterStartupScript(cstype, csname1, script, true);
        }
        bt_Imprimir.Enabled = false;
    }
    protected void lb_serieNC_SelectedIndexChanged(object sender, EventArgs e)
    {
        int tipo_contabilidad = int.Parse(Session["Contabilidad"].ToString());
        if (tipo_contabilidad != 2)
        {
            RE_GenericBean rgb = (RE_GenericBean)DB.getFacturabySerie(lb_serieNC.SelectedValue, 18, user.SucursalID);
            lb_moneda.SelectedValue = rgb.intC5.ToString();
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
            lb1.Text = row.Cells[8].Text.Trim();
            if (chk_anulacion.Checked == true)
            {
                t1.Text = Math.Round(decimal.Parse(lb1.Text), 2).ToString("#,#.00#;(#,#.00#)");
                descuento = decimal.Parse(t1.Text.Trim());
            }
            else
            {
                t1.Text = "0.00";
                descuento = decimal.Parse(t1.Text.Trim());
            }
            total += descuento;
        }
        //tb_total.Text = total.ToString();
        TextBox1_TextChanged(sender, e);
    }
    protected void bt_factura_virtual_Click(object sender, EventArgs e)
    {
        int ncID = DB.getCorrelativoDoc(user.SucursalID, 18, lb_serieNC.SelectedItem.Text, tb_corrNC.Text);
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
            string script = "window.open('../invoice/template_invoice.aspx?id=" + ncID.ToString() + "&transaccion=18','Imprimir','menubar=1,resizable=1,scrollbars=1,toolbar=1,width=750');";
            if (!cs2.IsStartupScriptRegistered(cstype, csname1))
            {
                cs2.RegisterStartupScript(cstype, csname1, script, true);
            }
        }
    }
}
