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
    long agente_id = 0;// Codigo del agente
    public string simboloequivalente = "USD";
    public string simbolomoneda = "GTQ";
    protected void Page_Load(object sender, EventArgs e)
    {
        int tipo_contabilidad = 0;
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
        if (!((permiso & 1) == 1) && !((permiso & 2) == 2) && !((permiso & 4) == 4) && !((permiso & 8) == 8))
        {
            Response.Redirect("index.aspx");
        }
        
        if (!Page.IsPostBack)
        {
            int impo_expo = 0;
            tipo_contabilidad = int.Parse(Session["Contabilidad"].ToString());
            obtengo_listas(tipo_contabilidad, impo_expo, user.PaisID);
            if (tipo_contabilidad == 2)
            {
                lb_moneda.SelectedValue = user.Moneda.ToString();
                lb_contribuyente.SelectedValue = "1";
                lb_contribuyente.Enabled = false;
            }
            else
            {
                lb_moneda.SelectedValue = user.Moneda.ToString();
                
            }
            if (lb_imp_exp.Items.Count > 0)
                impo_expo = int.Parse(lb_imp_exp.SelectedValue);
            if (lb_moneda.SelectedValue.Equals("1")) { simbolomoneda = "GTQ"; simboloequivalente = "USD"; }
            if (lb_moneda.SelectedValue.Equals("2")) { simbolomoneda = "SVC"; simboloequivalente = "USD"; }
            if (lb_moneda.SelectedValue.Equals("3")) { simbolomoneda = "HNL"; simboloequivalente = "USD"; }
            if (lb_moneda.SelectedValue.Equals("4")) { simbolomoneda = "NIC"; simboloequivalente = "USD"; }
            if (lb_moneda.SelectedValue.Equals("5")) { simbolomoneda = "CRC"; simboloequivalente = "USD"; }
            if (lb_moneda.SelectedValue.Equals("6")) { simbolomoneda = "PAB"; simboloequivalente = "USD"; }
            if (lb_moneda.SelectedValue.Equals("7")) { simbolomoneda = "BZD"; simboloequivalente = "USD"; }
            if (lb_moneda.SelectedValue.Equals("8")) { simbolomoneda = "USD"; simboloequivalente = "Local"; }
        }
        tipo_contabilidad = int.Parse(Session["Contabilidad"].ToString());
        if (tipo_contabilidad == 2)
        {
            lb_contribuyente.SelectedValue = "1";
            lb_contribuyente.Enabled = false;
        }
       

        if (lb_moneda.SelectedValue.Equals("1")) { simbolomoneda = "GTQ"; simboloequivalente = "USD"; }
        if (lb_moneda.SelectedValue.Equals("2")) { simbolomoneda = "SVC"; simboloequivalente = "USD"; }
        if (lb_moneda.SelectedValue.Equals("3")) { simbolomoneda = "HNL"; simboloequivalente = "USD"; }
        if (lb_moneda.SelectedValue.Equals("4")) { simbolomoneda = "NIC"; simboloequivalente = "USD"; }
        if (lb_moneda.SelectedValue.Equals("5")) { simbolomoneda = "CRC"; simboloequivalente = "USD"; }
        if (lb_moneda.SelectedValue.Equals("6")) { simbolomoneda = "PAB"; simboloequivalente = "USD"; }
        if (lb_moneda.SelectedValue.Equals("7")) { simbolomoneda = "BZD"; simboloequivalente = "USD"; }
        if (lb_moneda.SelectedValue.Equals("8")) { simbolomoneda = "USD"; simboloequivalente = "Local"; }
        gv_detalle.Columns[4].HeaderText = "Subtotal " + simbolomoneda;
        gv_detalle.Columns[5].HeaderText = "Impuesto " + simbolomoneda;
        gv_detalle.Columns[6].HeaderText = "Total " + simbolomoneda;
        gv_detalle.Columns[7].HeaderText = "Equivalente " + simboloequivalente;

    }

    private void obtengo_listas(int tipoconta, int impo_expo, int paiID)
    {
        ListItem item = null;
        ArrayList arr = null;
        if (!Page.IsPostBack)
        {
            arr = (ArrayList)DB.getOpcionesMatriz("tbl_tipo_transaccion where ttt_template='notadebito.aspx' and ttt_id<>6 order by ttt_nombre");
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
            arr = (ArrayList)DB.getSerieFactbySuc(user.SucursalID, 4, user, 0);//1 porque es el tipo de documento para facturacion
            foreach (string valor in arr)
            {
                item = new ListItem(valor, valor);
                lb_serie_factura.Items.Add(item);
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
        gv_detalle.Columns[4].HeaderText = "Subtotal en " + simbolomoneda;
        gv_detalle.Columns[5].HeaderText = "Impuesto en " + simbolomoneda;
        gv_detalle.Columns[6].HeaderText = "Total en " + simbolomoneda;
        gv_detalle.Columns[7].HeaderText = "Equivalente en " + simboloequivalente;
        Label2.Text = "Equivalente en " + simboloequivalente;
        Label6.Text = "Sub total en " + simbolomoneda;
        Label7.Text = "Impuesto en " + simbolomoneda;
        Label8.Text = "Total en " + simbolomoneda;
        tb_subtotal.Text = subtotal.ToString("#,#.00#;(#,#.00#)");
        tb_impuesto.Text = impuesto.ToString("#,#.00#;(#,#.00#)");
        tb_total.Text = total.ToString("#,#.00#;(#,#.00#)");
        tb_totaldolares.Text = totalD.ToString("#,#.00#;(#,#.00#)");
    }
    protected void bt_imprimir_Click(object sender, EventArgs e)
    {
        String csname1 = "PopupScript";
        Type cstype = this.GetType();
        // Get a ClientScriptManager reference from the Page class.
        ClientScriptManager cs2 = Page.ClientScript;
        //string script = "window.open('re_print.aspx?fac_id=" + lb_facid.Text + "&s=" + lb_serie_factura.SelectedItem.Text + "&c=" + tb_correlativo.Text.Trim() + "&tipo=4','Imprimir','menubar=1,resizable=1,scrollbars=1,toolbar=1,width=750');";
        string script = "window.open('../invoice/re_print.aspx?fac_id=" + lb_facid.Text + "&s=" + lb_serie_factura.SelectedItem.Text + "&c=" + tb_correlativo.Text.Trim() + "&tipo=4','Imprimir','menubar=1,resizable=1,scrollbars=1,toolbar=1,width=750');";

        if (!cs2.IsStartupScriptRegistered(cstype, csname1))
        {
            cs2.RegisterStartupScript(cstype, csname1, script, true);
        }
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        int ndID = 0;
        if (!Page.IsPostBack)
        {
            if (Request.QueryString["ndID"] != null && !Request.QueryString["ndID"].ToString().Equals(""))
            {
                ndID = int.Parse(Request.QueryString["ndID"].ToString().Trim());
                lb_facid.Text = ndID.ToString();
                //Obtengo los rubros de la factura
                RE_GenericBean factdata = (RE_GenericBean)DB.getNotaDebitoData(ndID);
                tb_correlativo.Text = factdata.intC6.ToString();
                tbCliCod.Text = factdata.douC1.ToString();
                tb_nombre.Text = factdata.strC2;
                tb_nit.Text = factdata.strC1;
                tb_razon.Text = factdata.strC2;
                lb_moneda.SelectedValue = factdata.intC4.ToString();
                tb_direccion.Text = factdata.strC6;
                tb_observaciones.Text = factdata.strC4;

                tb_routing.Text = factdata.strC12;
                tb_naviera.Text = factdata.strC13;
                tb_vapor.Text = factdata.strC14;
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
                tb_allin.Text = factdata.strC30;
                tb_reciboaduanal.Text = factdata.strC31;
                tb_factura_referencia.Text = factdata.strC11;

                tb_hbl.Text = factdata.strC7;
                tb_mbl.Text = factdata.strC8;
                //tb_routing.Text = factdata.strC10;
                tb_contenedor.Text = factdata.strC9;

                lb_serie_factura.SelectedValue = factdata.strC28;
                tb_correlativo.Text = factdata.intC6.ToString();
                tb_agente_nombre.Text = factdata.strC33;
                tb_poliza_seguros.Text = factdata.strC56;


                if (lb_moneda.SelectedValue.Equals("1")) { simbolomoneda = "GTQ"; simboloequivalente = "USD"; }
                if (lb_moneda.SelectedValue.Equals("2")) { simbolomoneda = "SVC"; simboloequivalente = "USD"; }
                if (lb_moneda.SelectedValue.Equals("3")) { simbolomoneda = "HNL"; simboloequivalente = "USD"; }
                if (lb_moneda.SelectedValue.Equals("4")) { simbolomoneda = "NIC"; simboloequivalente = "USD"; }
                if (lb_moneda.SelectedValue.Equals("5")) { simbolomoneda = "CRC"; simboloequivalente = "USD"; }
                if (lb_moneda.SelectedValue.Equals("6")) { simbolomoneda = "PAB"; simboloequivalente = "USD"; }
                if (lb_moneda.SelectedValue.Equals("7")) { simbolomoneda = "BZD"; simboloequivalente = "USD"; }
                if (lb_moneda.SelectedValue.Equals("8")) { simbolomoneda = "USD"; simboloequivalente = "Local"; }


                gv_detalle.Columns[4].HeaderText = "Subtotal en " + simbolomoneda;
                gv_detalle.Columns[5].HeaderText = "Impuesto en " + simbolomoneda;
                gv_detalle.Columns[6].HeaderText = "Total en " + simbolomoneda;
                gv_detalle.Columns[7].HeaderText = "Equivalente en " + simboloequivalente;
                gv_detalle.DataSource = (DataTable)DB.getRubbyFact(ndID, 4);
                gv_detalle.DataBind();


                //Mostrando la Partida contable generada
                gv_detalle_partida.DataSource = DB.getPolizaDiario(user, double.Parse(ndID.ToString()), 4, 0);
                gv_detalle_partida.DataBind();
                gv_detalle_partida.Rows[gv_detalle_partida.Rows.Count - 1].Font.Bold = true;

                #region Automatizacion Intercompany Administrativo
                if (factdata.intC7 == 10)
                {

                    #region Asignar Nombre Intercompany y Nombre Fiscal
                    RE_GenericBean Intercompany_Bean = null;
                    Intercompany_Bean = (RE_GenericBean)DB.Get_Intercompany_Data(Convert.ToInt32(factdata.douC1.ToString()));
                    tb_nombre.Text = Intercompany_Bean.strC5 + "   (" + Intercompany_Bean.strC1 + ")";
                    #endregion

                    string mensaje = "";
                    ArrayList Arr_Transacciones_Hijas = (ArrayList)DB.Get_Transacciones_Encadenadas_Hijas(4, ndID);
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

                    Intercompany_Bean = (RE_GenericBean)DB.Get_Intercompany_Data(Convert.ToInt32(factdata.douC1));

                    if (Intercompany_Bean.intC3 > 0)
                    {
                        string Nombre_Empresa_Destino = "";
                        string Nombre_Empresa_Origen = "";
                        PaisBean Pais_Temporal = (PaisBean)DB.getPais(Intercompany_Bean.intC3);
                        Nombre_Empresa_Destino = Pais_Temporal.Nombre_Sistema;
                        Pais_Temporal = (PaisBean)DB.getPais(user.PaisID);
                        Nombre_Empresa_Origen = Pais_Temporal.Nombre_Sistema;
                        tb_resultado_automatizacion.Text = "Se ha creado Automaticamente un documento Por Pagar.: " + "\r\n";
                        tb_resultado_automatizacion.Text += "    * En Sistema BAW - " + Nombre_Empresa_Destino + "\r\n";
                        tb_resultado_automatizacion.Text += "    * A " + Nombre_Empresa_Origen + "\r\n";
                        tb_resultado_automatizacion.Text += mensaje;
                    }
                    pnl_intercompanys.Visible = true;
                    if (user.PaisID == 1 || user.PaisID == 15)
                    {
                        bt_nd_intercompany.Visible = true;
                    }
                }
                #endregion
                if (factdata.intC4 == 8)
                {
                    if (user.PaisID == 1 || user.PaisID == 15)
                    {
                        bt_impresion_debit_note.Visible = true;
                    }
                }
            }
        }
    }
    protected void bt_nota_debito_virtual_Click(object sender, EventArgs e)
    {
        user = (UsuarioBean)Session["usuario"];
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
    protected void bt_nd_intercompany_Click(object sender, EventArgs e)
    {
        user = (UsuarioBean)Session["usuario"];
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
    protected void bt_impresion_debit_note_Click(object sender, EventArgs e)
    {
        user = (UsuarioBean)Session["usuario"];
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
}
