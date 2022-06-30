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

public partial class manager_addfactura : System.Web.UI.Page
{
    UsuarioBean user;
    public ArrayList camposfac_arr = null;
    public int suc_id=0;
    public int fac_id = 0;
    public int tipo = 1;
    
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["usuario"] == null)
        {
            Response.Redirect("../default.aspx");
        }
        user = (UsuarioBean)Session["usuario"];
        SucursalBean suc_bean=null;
        if (!Page.IsPostBack)
        {
            ArrayList arr = (ArrayList)DB.getOpcionesMatriz("tbl_tipo_moneda");
            ListItem itemito;
            foreach (RE_GenericBean rgb in arr)
            {
                itemito = new ListItem(rgb.strC1, rgb.intC1.ToString());
                lb_moneda.Items.Add(itemito);
            }
            arr = null;
            arr = (ArrayList)DB.getOpcionesMatriz("tbl_tipo_conta");
            ListItem item = null;
            foreach (RE_GenericBean rgb in arr)
            {
                item = new ListItem(rgb.strC1, rgb.intC1.ToString());
                lb_tipo_contabilidad.Items.Add(item);
            }
            arr = null;
            arr = (ArrayList)DB.getTipoFactura();
            item = null;
            foreach (RE_GenericBean rgb in arr)
            {
                item = new ListItem(rgb.strC1, rgb.intC1.ToString());
                lb_tipo_factura.Items.Add(item);
            }
            arr = null;
            arr = (ArrayList)DB.getFormatoDocumentos();
            item = null;
            foreach (RE_GenericBean rgb in arr)
            {
                item = new ListItem(rgb.strC1 + " -- " + rgb.strC2 + "x" + rgb.strC3, rgb.intC1.ToString());
                lb_formato_documento.Items.Add(item);
            }
            if (Request.QueryString["suc_id"] != null)
            {
                suc_id = int.Parse(Request.QueryString["suc_id"].ToString().Trim());
                if (Request.QueryString["tipo"] != null)
                {
                    tipo = int.Parse(Request.QueryString["tipo"].ToString().Trim());
                }
                lb_tipo_doc.SelectedValue = tipo.ToString();
                suc_bean = (SucursalBean)DB.getSucursal(suc_id);
                if (suc_bean == null)
                {
                    Session["msg"] = "Error, No es posible asociar un documento sin especificar un departamento valido";
                    Session["url"] = "searchsuc.aspx";
                    Response.Redirect("message.aspx");
                }
                ListItem ite = new ListItem(suc_bean.Nombre.ToString(), suc_bean.ID.ToString());
                lb_suc.Items.Add(ite);
                if (Request.QueryString["id"] != null)
                {
                    fac_id = int.Parse(Request.QueryString["id"].ToString().Trim());
                    if (fac_id > 0)
                    {
                        RE_GenericBean fac_bean = (RE_GenericBean)DB.getFactura(fac_id, tipo);
                        if (fac_bean == null)
                        {
                            Session["msg"] = "Error, No es posible obtener datos del documento especificado";
                            Session["url"] = "addsuc.aspx?id=" + suc_id;
                            Response.Redirect("message.aspx");
                        }
                        tb_facid.Text = fac_bean.intC1.ToString();
                        tb_serie.Text = fac_bean.strC1;
                        tb_NoInicial.Text = fac_bean.intC3.ToString();
                        lb_moneda.SelectedValue = fac_bean.intC5.ToString();
                        tb_size.Text = fac_bean.intC7.ToString();
                        tb_font.Text = fac_bean.strC2;
                        tb_interlineado.Text = fac_bean.intC6.ToString();
                        tb_corrimiento.Text = fac_bean.intC8.ToString();
                        tb_simbolo.Text = fac_bean.strC3;
                        lb_tipo_contabilidad.SelectedValue = fac_bean.intC9.ToString();
                        lb_tipo_operacion.SelectedValue = fac_bean.intC10.ToString();
                        if (fac_bean.intC11 > 0)
                        {
                            lb_tipo_factura.SelectedValue = fac_bean.intC11.ToString();
                        }
                        if (fac_bean.intC12 > 0)
                        {
                            lb_formato_documento.SelectedValue = fac_bean.intC12.ToString();
                        }
                        lb_tipo_documento.SelectedValue = fac_bean.intC14.ToString();
                        tb_numero_autorizacion.Text = fac_bean.strC7;
                        tb_fecha_resolucion.Text = fac_bean.strC4;
                        tb_rango_inicial.Text = fac_bean.strC5;
                        tb_rango_final.Text = fac_bean.strC6;
                        drp_estado.SelectedValue = fac_bean.intC13.ToString();
                        tb_dispositivo_electronico.Text = fac_bean.strC8;
                        drp_ImprimirTotalLetraAmbasMonedas.SelectedValue = Convert.ToInt32(fac_bean.boolC1).ToString();
                    }
                    else
                    {
                        tb_facid.Text = "0";
                        tb_serie.Text = "";
                        tb_NoInicial.Text = "0";
                    }
                }
                else
                {
                    tb_facid.Text = "0";
                    tb_serie.Text = "";
                    tb_NoInicial.Text = "0";
                }
                llenar_gridView();
            }
            else
            {
                Session["msg"] = "Error, No es posible asociar un documento sin especificar un departamento valido";
                Session["url"] = "searchsuc.aspx";
                Response.Redirect("message.aspx");
            }

        }
        if (lb_tipo_doc.SelectedValue == "1")
        { 
            Panel1.Visible = true;
        }
        else { Panel1.Visible = false; }
        if ((lb_tipo_doc.SelectedValue == "1") || (lb_tipo_doc.SelectedValue == "2") || (lb_tipo_doc.SelectedValue == "3") || (lb_tipo_doc.SelectedValue == "4"))
        {
            Panel2.Visible = true;
            Panel3.Visible = true;
            Panel4.Visible = true;
        }
        else
        {
            Panel2.Visible = false;
            Panel3.Visible = false;
            Panel4.Visible = false;
        }
    }
    protected void bt_Enviar_Click(object sender, EventArgs e)
    {
        RE_GenericBean fac_bean = new RE_GenericBean();
        fac_bean.intC1 = int.Parse(tb_facid.Text.Trim());
        fac_bean.intC2 = int.Parse(lb_suc.SelectedValue);
        fac_bean.strC1 = tb_serie.Text.Trim().ToUpper();
        fac_bean.intC3 = int.Parse(tb_NoInicial.Text.Trim());
        fac_bean.intC4 = int.Parse(lb_tipo_doc.SelectedValue);
        fac_bean.intC5 = int.Parse(lb_moneda.SelectedValue);
        fac_bean.strC2 = tb_font.Text.Trim();
        fac_bean.intC9 = int.Parse(lb_tipo_contabilidad.SelectedValue);
        fac_bean.intC10 = int.Parse(lb_tipo_operacion.SelectedValue);
        fac_bean.intC13 = int.Parse(lb_tipo_documento.SelectedValue);
        if ((lb_tipo_doc.SelectedValue == "1") || (lb_tipo_doc.SelectedValue == "14"))//Tipo de Factura
        {
            fac_bean.intC11 = int.Parse(lb_tipo_factura.SelectedValue);//Tipo de Factura
        }
        else
        {
            fac_bean.intC11 = 0;
        }
        if ((lb_tipo_doc.SelectedValue == "1") || (lb_tipo_doc.SelectedValue == "2") || (lb_tipo_doc.SelectedValue == "3") || (lb_tipo_doc.SelectedValue == "4"))
        {
            fac_bean.intC12 = int.Parse(lb_formato_documento.SelectedValue);//Formato del Documento
            fac_bean.intC13 = int.Parse(lb_tipo_documento.SelectedValue);//Marca si el Documento es electronico o no
        }
        else
        {
            fac_bean.intC12 = 0;
            fac_bean.intC13 = 0;
        }
        fac_bean.strC4 = tb_numero_autorizacion.Text.Trim();
        if (tb_fecha_resolucion.Text.Trim() == "")
        {
            fac_bean.strC5 = DateTime.Now.ToString("MM/dd/yyyy");
        }
        else
        {
            fac_bean.strC5 = tb_fecha_resolucion.Text;
        }
        fac_bean.strC6 = tb_rango_inicial.Text.Trim();
        fac_bean.strC7 = tb_rango_final.Text.Trim();
        fac_bean.strC8 = tb_dispositivo_electronico.Text.Trim();
        fac_bean.strC9 = tb_doc_impresion.Text.Trim();
        fac_bean.intC14 = int.Parse(drp_estado.SelectedValue.ToString());
        fac_bean.boolC1 = drp_ImprimirTotalLetraAmbasMonedas.SelectedValue.ToString() == "0" ? false : true;
        if (!tb_interlineado.Text.Equals("")) fac_bean.intC6 = int.Parse(tb_interlineado.Text);
        if (!tb_size.Text.Equals("")) fac_bean.intC7 = int.Parse(tb_size.Text);
        if (!tb_corrimiento.Text.Equals("")) fac_bean.intC8 = int.Parse(tb_corrimiento.Text);
        if (!tb_simbolo.Text.Equals("")) fac_bean.strC3 = tb_simbolo.Text;
        RE_GenericBean camposbean = null;
        TextBox t1, t2, t3;
        int x = 0, y = 0;
        Label lb;
        GridViewRow row;
        for (int i = 0; i < gv_conf_cuentas.Rows.Count; i++)
        {
            row = gv_conf_cuentas.Rows[i];
            if (row.RowType == DataControlRowType.DataRow) {
                t1 = (TextBox)row.FindControl("tb_posx");
                t2 = (TextBox)row.FindControl("tb_posy");
                t3 = (TextBox)row.FindControl("tb_notas");
                t3.Text = t3.Text.Replace("|", "<br/>");
                if (t1 != null && !t1.Text.Equals("")) x = int.Parse(t1.Text); else x = 0;
                if (t2 != null && !t2.Text.Equals("")) y = int.Parse(t2.Text); else y = 0;
                if (x > 0 && y > 0) { // si tiene configurada mas de alguna posicion
                    camposbean = new RE_GenericBean();
                    lb = (Label)row.FindControl("Label1");
                    camposbean.strC1 = lb.Text;//Nombre del campo
                    camposbean.intC1 = x;
                    camposbean.intC2 = y;
                    camposbean.strC2 = t3.Text;
                    if (fac_bean.arr1 == null) fac_bean.arr1 = new ArrayList();
                    fac_bean.arr1.Add(camposbean);
                }
                t3.Text = t3.Text.Replace("<br/>", "|");
            }
        }
        int result = DB.InsertUpdateFactura(fac_bean, user);
        if ((result == 0) || (result==-100))
        {
            WebMsgBox.Show("Error, No se pudieron grabar los datos que ingreso, por favor intente de nuevo.");
            return;
        }
        tb_facid.Text = result.ToString();
        
    }
    private void llenar_gridView() {
        DataTable dt_temp = new DataTable();
        dt_temp.Columns.Add("Campo");
        dt_temp.Columns.Add("PosicionX");
        dt_temp.Columns.Add("PosicionY");
        dt_temp.Columns.Add("Notas");
        dt_temp.Clear();

        if (lb_tipo_doc.SelectedValue.Equals("1"))
        { // factura
            string campos = "CODIGO CLIENTE|NIT|NOMBRE|CONDICIONES_PAGO|DIRECCION|FECHA EMISION|FECHA (D)|FECHA (M)|FECHA (M) LETRAS|FECHA (A)|FECHA PAGO|SUB TOTAL|SERIE FACTURA|CORRELATIVO FACTURA";
            campos += "|IMPUESTO|TOTAL|TOTAL LETRAS|OBSERVACIONES|RAZON|MONEDA|USUARIO|HBL|MBL";
            campos += "|CONTENEDOR|ROUTING|NAVIERA|VAPOR|SHIPPER|POLIZA ADUANAL|CONSIGNATARIO|AGENTE|AGENTE_ID";
            campos += "|COMODITY|PAQUETES|PESO|VOLUMEN|DUA INGRESO|DUA SALIDA|VENDEDOR 1";
            campos += "|VENDEDOR 2|REFERENCIA|RUBRO_NOMBRE|RUBRO_SUBTOTAL|RUBRO_IMPUESTO|RUBRO_TOTAL|RUBRO_COMENTARIO|NOTA DOCUMENTO|TIPO CAMBIO|FECHA IMPRESION|HORA IMPRESION";
            campos += "|MARCA CONTADO|MARCA CREDITO|RECIBO ADUANA|SUBTOTAL EXCENTOS|SUBTOTAL NORMALES|RUBRO TOTAL EQUIVALENTE|RUBRO MONEDA|RUBRO MONEDA EQ";
            campos += "|VALOR ADUANERO|ADUANA|RECIBO AGENCIA|SUB TOTAL ALMACEN|IMPUESTO VENTA ALMACEN|TOTAL ALMACEN|ANTICIPOS ALMACEN|SALDO A PAGAR ALMACEN|SALDO A PAGAR ALMACEN NO.2|RECIBOS";
            campos += "|RUC|GIRO|VENTAS_AFECTAS|VENTAS_NO_SUJETAS|RUBRO_ID|TOTAL_VENTAS_AFECTAS|TOTAL_VENTAS_AFECTAS_CON_IVA|TOTAL_VENTAS_NO_SUJETAS|TOTAL_IVA_VENTAS_AFECTAS|SUB_TOTAL_SV|TOTAL_VENTAS_EXENTAS|TOTAL_VENTAS_EXENTAS_CON_IVA|VENTAS_EXENTAS|CANTIDAD_SV|PRECIO_UNITARIO_SV";
            campos += "|SUBTOTAL_HN|IMPUESTO_HN|TOTAL_HN|TOTAL_EQUIVALENTE|TOTAL_LETRAS_EQUIVALENTE";
            campos += "|SUBTOTAL_NI_EQA|IMPUESTO_NI_EQA|TOTAL_NI_EQA|RUBRO_SUBTOTAL_NI_EQA|SUBTOTAL_EQUIVALENTE|TOTAL_LOCAL|OTRAS_OBSERVACIONES|RETENCION|RETENCIONIVA15|RETENCIONIVA1|RUBRO TOTAL EQUIVALENTE ERIAL|RUBRO MONEDA EQ ERIAL|NUMERO_FACTURA_ADUANA|NO_DE_EMBARQUE|TEXTO_RECLAMO";
            string[] camparr = campos.Split('|');
            foreach (string campo in camparr)
            {
                object[] obj = { campo, "0", "0", "" };
                dt_temp.Rows.Add(obj);
            }
        }
        else if (lb_tipo_doc.SelectedValue.Equals("2"))
        {  //recibo
            string campos = "CODIGO PERSONA|NOMBRE|FECHA EMISION|FECHA (D)|FECHA (M)|FECHA (M) LETRAS|FECHA (A)|TOTAL|OBSERVACIONES|MONEDA|USUARIO|HBL|MBL";
            campos += "|CONTENEDOR|ROUTING|REFERENCIA|SERIE|CORRELATIVO|TOTAL LETRAS|DOC. REFERENCIA (CHEQUE/DEPOSITO)|BANCO|NOTA DOCUMENTO|TIPO CAMBIO|FECHA IMPRESION|HORA IMPRESION";
            campos += "|NOMBRE TIPO PAGO|REFEENCIA TIPO PAGO|MONTO TIPO PAGO|DIRECCION|DETALLE PAGOS|TOTAL_HN|TOTAL_EQUIVALENTE|TOTAL_LETRAS_EQUIVALENTE|POR_CUENTA_DE|CONCEPTO|CONCEPTO_DETALLADO|COMENTARIO1|COMENTARIO2";
            string[] camparr = campos.Split('|');
            foreach (string campo in camparr){
                object[] obj = { campo, "0", "0", "" };
                dt_temp.Rows.Add(obj);
            }
        } else if (lb_tipo_doc.SelectedValue.Equals("3")) {  //Nota Credito
            //string campos = "NUMERO|USUARIO|MONTO|NOMBRE|FECHA EMISION|FECHA (D)|FECHA (M)|FECHA (M) LETRAS|FECHA (A)|OBSERVACIONES|NOTA DOCUMENTO|TIPO CAMBIO";
            string campos = "TIPO DOCUMENTO|CODIGO CLIENTE|NIT|NOMBRE|DIRECCION|FECHA EMISION|FECHA (D)|FECHA (M)|FECHA (M) LETRAS|FECHA (A)|FECHA PAGO|SUB TOTAL|SERIE FACTURA|CORRELATIVO FACTURA";
            campos += "|IMPUESTO|TOTAL|TOTAL LETRAS|OBSERVACIONES|RAZON|MONEDA|USUARIO|HBL|MBL";
            campos += "|CONTENEDOR|ROUTING|NAVIERA|VAPOR|SHIPPER|POLIZA ADUANAL|CONSIGNATARIO";
            campos += "|COMODITY|PAQUETES|PESO|VOLUMEN|DUA INGRESO|DUA SALIDA|VENDEDOR 1";
            campos += "|VENDEDOR 2|REFERENCIA|SERIE|CORRELATIVO|RUBRO_NOMBRE|RUBRO_SUBTOTAL|RUBRO_IMPUESTO|RUBRO_TOTAL|NOTA DOCUMENTO|TIPO CAMBIO|FECHA IMPRESION|HORA IMPRESION";
            campos += "|MARCA CONTADO|MARCA CREDITO|RECIBO ADUANA|TOTAL_DESCUENTO|NOMBRE_USUARIO|NIT_USUARIO|TOTAL_EQUIVALENTE|TOTAL_LETRAS_EQUIVALENTE|RUBRO_ID|RUBRO MONEDA|NOTA_DE";
            campos += "|SIRVASE_DE|CIUDAD|COMENTARIO1|COMENTARIO2|RUBRO TOTAL EQUIVALENTE|RUBRO MONEDA|RUBRO_ID|RUBRO MONEDA EQ ";
            string[] camparr = campos.Split('|');
            foreach (string campo in camparr)
            {
                object[] obj = { campo, "0", "0", "" };
                dt_temp.Rows.Add(obj);
            }
        } else if (lb_tipo_doc.SelectedValue.Equals("4")) {  //Nota Debito
            string campos = "TIPO DOCUMENTO|CODIGO CLIENTE|NIT|NOMBRE|DIRECCION|FECHA EMISION|FECHA (D)|FECHA (M)|FECHA (M) LETRAS|FECHA (A)|FECHA PAGO|SUB TOTAL";
            campos += "|IMPUESTO|TOTAL|TOTAL LETRAS|OBSERVACIONES|RAZON|MONEDA|USUARIO|HBL|MBL";
            campos += "|CONTENEDOR|ROUTING|NAVIERA|VAPOR|SHIPPER|POLIZA ADUANAL|CONSIGNATARIO";
            campos += "|COMODITY|PAQUETES|PESO|VOLUMEN|DUA INGRESO|DUA SALIDA|VENDEDOR 1";
            campos += "|VENDEDOR 2|REFERENCIA|SERIE|CORRELATIVO|RUBRO_NOMBRE|RUBRO_SUBTOTAL|RUBRO_IMPUESTO|RUBRO_TOTAL|NOTA DOCUMENTO|TIPO CAMBIO|FECHA IMPRESION|HORA IMPRESION";
            campos += "|MARCA CONTADO|MARCA CREDITO|RECIBO ADUANA|TOTAL_EQUIVALENTE|TOTAL_LETRAS_EQUIVALENTE|NOTA_DE|SIRVASE_DE|CIUDAD|COMENTARIO1|COMENTARIO2|RUBRO TOTAL EQUIVALENTE|RUBRO MONEDA EQ|RUBRO MONEDA|RUBRO_ID|DETALLE_ABONOS_ND|TOTAL_ABONOS_ND|TEXTO_RECLAMO";
            string[] camparr = campos.Split('|');
            foreach (string campo in camparr)
            {
                object[] obj = { campo, "0", "0", "" };
                dt_temp.Rows.Add(obj);
            }
        }
        else if (lb_tipo_doc.SelectedValue.Equals("5"))//provisiones
        {
            string campos = "NIT|NOMBRE|DIRECCION|FECHA EMISION|FECHA (D)|FECHA (M)|FECHA (M) LETRAS|FECHA (A)|SUB TOTAL|IMPUESTO|TOTAL|OBSERVACIONES|MONEDA|HBL|MBL|CONTENEDOR|ROUTING|SERIE FACTURA|CORRELATIVO FACTURA|SERIE|CORRELATIVO|RUBRO_NOMBRE|RUBRO_SUBTOTAL|RUBRO_IMPUESTO|RUBRO_TOTAL|NOTA DOCUMENTO|TIPO CAMBIO";
            string[] camparr = campos.Split('|');
            foreach (string campo in camparr)
            {
                object[] obj = { campo, "0", "0", "" };
                dt_temp.Rows.Add(obj);
            }
        }
        else if (lb_tipo_doc.SelectedValue.Equals("7"))//Ordenes de compra
        {  
            //string campos = "NIT|NOMBRE|DIRECCION|FECHA EMISION|TOTAL|OBSERVACIONES|MONEDA|REFERENCIA|SERIE|CORRELATIVO|NOTA DOCUMENTO|TIPO CAMBIO";
            string campos = "NOMBRE|DIRECCION|FECHA EMISION|TOTAL|DESCRIPCION|MONEDA|NO DE COTIZACION|SERIE|CORRELATIVO|TERMINOS|DEPARTAMENTO|CONTACTO|HBL|MBL|OBSERVACIONES|CREADOR|TOTAL_EQUIVALENTE|TOTAL_LETRAS_EQUIVALENTE";
            string[] camparr = campos.Split('|');
            foreach (string campo in camparr)
            {
                object[] obj = { campo, "0", "0", "" };
                dt_temp.Rows.Add(obj);
            }
        }
        else if (lb_tipo_doc.SelectedValue.Equals("8"))// contrasenia de factura
        {
            string campos = "ENCABEZADO|NOMBRE|FECHA EMISION|TOTAL|OBSERVACIONES|SERIE|CORRELATIVO|NOTA DOCUMENTO|SERIE FACTURA|CORRELATIVO FACTURA";
            string[] camparr = campos.Split('|');
            foreach (string campo in camparr)
            {
                object[] obj = { campo, "0", "0", "" };
                dt_temp.Rows.Add(obj);
            }
        }
        else if (lb_tipo_doc.SelectedValue.Equals("20")) {  //retenciones
            string campos = "CODIGO_CLIENTE|NOMBRE|DIRECCION|TELEFONO|VENDEDOR|FECHA (D)|FECHA (M)|FECHA (M) LETRAS|FECHA (A)|NIT|RUC|PROVISIONES|RUBRO_TOTAL|TOTAL_LETRAS|SUMAS|IVA|SUB_TOTAL|TOTAL_COMPRA|IVA_RETENIDO|VENTAS_EXENTAS|RENTA|TOTAL|RUBRO_IMPUESTO|RUBRO_RETENCION|FECHA_IMPRESION|USUARIO|HORA|MOTIVO";
            campos += "|PROVISION_SERIE|PROVISION_CORRELATIVO|PROVISION_TOTAL|RT_10%_SOBRE_BASE|RT_1%_SOBRE_BASE|RT_5%_SOBRE_TOTAL|RT_13%_SOBRE_BASE|SERIE|CORRELATIVO";
            string[] camparr = campos.Split('|');
            foreach (string campo in camparr)
            {
                object[] obj = { campo, "0", "0", "" };
                dt_temp.Rows.Add(obj);
            }
        } else if (lb_tipo_doc.SelectedValue.Equals("14")) {  //Factura proforma
            string campos = "CODIGO CLIENTE|NIT|NOMBRE|DIRECCION|FECHA EMISION|FECHA (D)|FECHA (M)|FECHA (M) LETRAS|FECHA (A)|FECHA PAGO|SUB TOTAL|SERIE FACTURA|CORRELATIVO FACTURA";
            campos += "|IMPUESTO|TOTAL|TOTAL LETRAS|OBSERVACIONES|RAZON|MONEDA|USUARIO|HBL|MBL";
            campos += "|CONTENEDOR|ROUTING|NAVIERA|VAPOR|SHIPPER|POLIZA ADUANAL|CONSIGNATARIO|AGENTE";
            campos += "|COMODITY|PAQUETES|PESO|VOLUMEN|DUA INGRESO|DUA SALIDA|VENDEDOR 1";
            //campos += "|VENDEDOR 2|REFERENCIA|SERIE|RUBRO_NOMBRE|RUBRO_SUBTOTAL|RUBRO_IMPUESTO|RUBRO_TOTAL|NOTA DOCUMENTO|TIPO CAMBIO|FECHA IMPRESION|HORA IMPRESION";
            campos += "|VENDEDOR 2|REFERENCIA|RUBRO_NOMBRE|RUBRO_SUBTOTAL|RUBRO_IMPUESTO|RUBRO_TOTAL|NOTA DOCUMENTO|TIPO CAMBIO|FECHA IMPRESION|HORA IMPRESION";
            campos += "|MARCA CONTADO|MARCA CREDITO|RECIBO ADUANA|SUBTOTAL EXCENTOS|SUBTOTAL NORMALES|TOTAL_EQUIVALENTE|TOTAL_LETRAS_EQUIVALENTE";
            string[] camparr = campos.Split('|');
            foreach (string campo in camparr)
            {
                object[] obj = { campo, "0", "0", "" };
                dt_temp.Rows.Add(obj);
            }
        }
        else if (lb_tipo_doc.SelectedValue.Equals("15"))
        {  //Factura proforma
            string campos = "FECHA EMISION|FECHA (D)|FECHA (M)|FECHA (M) LETRAS|FECHA (A)|SERIE FACTURA|CORRELATIVO FACTURA";
            campos += "|TOTAL|TOTAL LETRAS|OBSERVACIONES|MONEDA|USUARIO|HBL|MBL";
            campos += "|CONTENEDOR|ROUTING|CUENTA ID|CUENTA NOMBRE|TOTAL CARGO|TOTAL ABONO|FECHA AJUSTAR|TOTAL_EQUIVALENTE|TOTAL_LETRAS_EQUIVALENTE";
            string[] camparr = campos.Split('|');
            foreach (string campo in camparr)
            {
                object[] obj = { campo, "0", "0", "" };
                dt_temp.Rows.Add(obj);
            }
        }

        
        gv_conf_cuentas.DataSource = dt_temp;
        gv_conf_cuentas.DataBind();
    }
    protected void gv_conf_cuentas_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        int facid = int.Parse(tb_facid.Text);
        if (facid>0){
            Label lb;
            TextBox t1, t2, t3;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                lb=(Label)e.Row.FindControl("Label1");
                t1 = (TextBox)e.Row.FindControl("tb_posx");
                t2 = (TextBox)e.Row.FindControl("tb_posy");
                t3 = (TextBox)e.Row.FindControl("tb_notas");
                RE_GenericBean rgb = DB.getDatosCampoPlantilla(facid, lb.Text);
                if (rgb != null) {
                    t1.Text = rgb.intC1.ToString();
                    t2.Text = rgb.intC2.ToString();
                    t3.Text = rgb.strC2.Replace("<br/>", "|");
                }
            }
        }
    }
    protected void bt_preview_Click(object sender, EventArgs e)
    {
        String csname1 = "PopupScript";
        Type cstype = this.GetType();
        // Get a ClientScriptManager reference from the Page class.
        ClientScriptManager cs2 = Page.ClientScript;
        string script = "window.open('../invoice/preview.aspx?suc_id=" + lb_suc.SelectedValue + "&tipo=" + lb_tipo_doc.SelectedValue + "&serie=" + tb_serie.Text.Trim().ToUpper() + "','Imprimir','menubar=1,resizable=1,scrollbars=1,toolbar=1,width=750');";

        if (!cs2.IsStartupScriptRegistered(cstype, csname1))
        {
            cs2.RegisterStartupScript(cstype, csname1, script, true);
        }
    }
}
