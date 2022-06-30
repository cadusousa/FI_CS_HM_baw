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
using System.Drawing;
using System.Drawing.Printing;
using System.Security.Principal;

public partial class invoice_print_invoice : System.Web.UI.Page
{
    UsuarioBean user;
    decimal retencion = 0;
    decimal retencionIVA1 = 0;
    decimal retencionIVA15 = 0;
    int fac_id = 0, tipo = 1, tpi=0;//se agrego tpi para distinguir entre documento intercompany o cliente 
    int xnombre_ini = 0, ynombre_ini = 0;
    int xsubt_ini = 0, ysubt_ini = 0;
    int ximp_ini = 0, yimp_ini = 0;
    int xtot_ini = 0, ytot_ini = 0;
    int xcomentario_ini = 0, ycomentario_ini = 0;
    int xrubroequivalente_ini = 0, yrubroequivalente_ini = 0;
    int xrubromoneda_ini = 0, yrubromoneda_ini = 0;
    int xrubromonedaeq_ini = 0, yrubromonedaeq_ini = 0;
    int imp_id = 0;
    int x, y = 0;
    decimal subtotalnormal = 0, subtotalexcento = 0, abonofactura = 0;
    string texto;
    string detalle_pagos = "";
    string espacios = "";
    string monto_auxiliar = "";
    int DOC_ID = 0;
    const string Retorno_De_Carro = "\n";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["usuario"] == null)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "Nombre", "<script> refresh(); </script>");
        }
        DataTable dt = null;
        RE_GenericBean datosfactura = null;
        RE_GenericBean font_interlineado = null;
        RE_GenericBean rgb_cliente = null;
        bool IsCredito = false;
        string serieDoc = "";
        if (Request.QueryString["fac_id"] != null)
        {
            fac_id = int.Parse(Request.QueryString["fac_id"].ToString());
            tipo = int.Parse(Request.QueryString["tipo"].ToString());
        }
        user = (UsuarioBean)Session["usuario"];
        //user.PrinterName = "Canon iP1200";
        //Imprimir_Documento();
        if (tipo == 1)
        { //factura
            datosfactura = (RE_GenericBean)DB.getFacturaData(fac_id);
            tpi = int.Parse(datosfactura.strC58);
            if (user.PaisID == 9)
            {
                //datosfactura.decC1 = datosfactura.decC3;  2020-06-22 vuelve asignar total a subtotal SV
            }
            retencion = DB.getRetencioByFactID(datosfactura.intC1);
            serieDoc = datosfactura.strC28;//Factura
            if (tpi == 3)
            {
                rgb_cliente = (RE_GenericBean)DB.getDataClient((int)datosfactura.intC3);
            }
            else if (tpi == 10)
            {
                rgb_cliente = (RE_GenericBean)DB.getIntercompanyData((int)datosfactura.intC3);
                rgb_cliente.intC2 = 0;
            }
            dt = (DataTable)DB.getRubbyFact(fac_id, tipo);
            abonofactura = (decimal)DB.getTotalAbonadoXFact(fac_id, 2);//indica que la referencia es de recibo
            IsCredito = DB.EsClienteCredito(datosfactura.intC3, user.PaisID);
            int excento = 0;
            foreach (DataRow dr in dt.Rows)
            {
                excento = DB.getRubroExcento(datosfactura.intC8, long.Parse(dr[0].ToString()));
                if (excento == 0) subtotalnormal += decimal.Parse(dr[5].ToString());
                else if (excento > 0) subtotalexcento += decimal.Parse(dr[5].ToString());
            }
        }
        else if (tipo == 4)
        { //Nota Debito
            datosfactura = (RE_GenericBean)DB.getNotaDebitoData_forprint(fac_id);
            dt = (DataTable)DB.getRubbyFact(fac_id, tipo);
            serieDoc = datosfactura.strC28;
        }
        else if (tipo == 2)
        {  // Recibos
            datosfactura = (RE_GenericBean)DB.getRcptData(fac_id);
            serieDoc = datosfactura.strC32;
            dt = null;
            dt = (DataTable)DB.getPagosRecibo(fac_id);
            foreach (DataRow dr in dt.Rows)
            {
                detalle_pagos += "<tr>";
                detalle_pagos += "<td>" + dr[0].ToString() + "</td>";
                detalle_pagos += "<td>" + dr[1].ToString() + "</td>";
                detalle_pagos += "<td>" + dr[2].ToString() + "</td>";
                detalle_pagos += "<td>" + dr[3].ToString() + "</td>";
                detalle_pagos += "<td>" + dr[4].ToString() + "</td>";
                detalle_pagos += "</tr>";
            }
            detalle_pagos = "<table border=0 cellspacing=3 cellpadding=0 >" + detalle_pagos + "</table>";


        }
        else if (tipo == 14)
        { //factura proforma
            datosfactura = (RE_GenericBean)DB.getFacturaData_proforma(fac_id);
            serieDoc = datosfactura.strC28;
            rgb_cliente = (RE_GenericBean)DB.getDataClient((int)datosfactura.intC3);
            dt = (DataTable)DB.getRubbyFact(fac_id, tipo);
            IsCredito = DB.EsClienteCredito(datosfactura.intC3, user.PaisID);
        }
        else if ((tipo == 3) || (tipo == 18))
        { //Nota Credito
            datosfactura = (RE_GenericBean)DB.getNotaCreditoData(fac_id);
            //serieDoc = datosfactura.strC29;
            serieDoc = datosfactura.strC32;
            //Ir a traer el Total de la Nota de Credito en el query porque no regresa ese dato el que regresa es el de la factura
            dt = (DataTable)DB.getRubbyFact(fac_id, tipo);
        }
        else if (tipo == 15)
        { //Ajuste Contable
            datosfactura = (RE_GenericBean)DB.getFacturaData_proforma(fac_id);
            rgb_cliente = (RE_GenericBean)DB.getDataClient((int)datosfactura.intC3);
            dt = (DataTable)DB.getRubbyFact(fac_id, tipo);
            IsCredito = DB.EsClienteCredito(datosfactura.intC3, user.PaisID);
        }
        else if (tipo == 7)
        { //Orden de Compra
            datosfactura = (RE_GenericBean)DB.getOCData_forprint(fac_id);
            serieDoc = datosfactura.strC28;
        }
        else if (tipo == 8)
        { //Contrasenias Factura
            datosfactura = (RE_GenericBean)DB.getContrasenaData_forprint(fac_id);
            serieDoc = datosfactura.strC32;
        }
        int doc_id = DB.getDocumentoID(datosfactura.intC2, serieDoc, tipo, user);
        DOC_ID = doc_id;
        font_interlineado = DB.getFactura(doc_id, tipo);
        ArrayList camposfact = (ArrayList)DB.getCamposDoc(tipo, datosfactura.intC2, serieDoc);//parametros 1=factura (tipo doc); 2=sucursal de la factura; 3=serie de factura
        Label lb1 = null;
        Imprimir_Documento();
        foreach (RE_GenericBean rgb in camposfact)
        {
            // dibujo el campo
            lb1 = new Label();
            lb1.Text = "";
            if (rgb.strC1.Trim().Equals("RETENCION")) lb1.Text = rgb.strC2.Trim() + " " + retencion;
            if (rgb.strC1.Trim().Equals("FECHA EMISION")) lb1.Text = rgb.strC2.Trim() + " " + datosfactura.strC5;
            if (rgb.strC1.Trim().Equals("FECHA (D)")) lb1.Text = rgb.strC2.Trim() + " " + datosfactura.strC5.Substring(8, 2);
            if (rgb.strC1.Trim().Equals("FECHA (M)")) lb1.Text = rgb.strC2.Trim() + " " + datosfactura.strC5.Substring(5, 2);
            if (rgb.strC1.Trim().Equals("FECHA (M) LETRAS"))
                if (user.Idioma == 2)//Ingles
                {
                    //lb1.Text = rgb.strC2.Trim() + " " + Utility.MesfromIntToStr_english(int.Parse(datosfactura.strC5.Substring(3, 2)));
                    lb1.Text = rgb.strC2.Trim() + " " + Utility.MesfromIntToStr_english(DateTime.Parse(datosfactura.strC5).Month);
                }
                else
                {
                    //lb1.Text = rgb.strC2.Trim() + " " + Utility.MesfromIntToStr(int.Parse(datosfactura.strC5.Substring(3, 2)));
                    lb1.Text = rgb.strC2.Trim() + " " + Utility.MesfromIntToStr(DateTime.Parse(datosfactura.strC5).Month);
                }

            //if (rgb.strC1.Trim().Equals("FECHA (A)")) lb1.Text = rgb.strC2.Trim() + " " + datosfactura.strC5.Substring(6, 4);
            if (rgb.strC1.Trim().Equals("FECHA (A)")) lb1.Text = rgb.strC2.Trim() + " " + DateTime.Parse(datosfactura.strC5).Year.ToString();
            if (rgb.strC1.Trim().Equals("NOMBRE")) lb1.Text = rgb.strC2.Trim() + " " + datosfactura.strC3;
            if (rgb.strC1.Trim().Equals("DIRECCION")) lb1.Text = rgb.strC2.Trim() + " " + datosfactura.strC4;
            if (rgb.strC1.Trim().Equals("NIT")) lb1.Text = rgb.strC2.Trim() + " " + datosfactura.strC2;
            if (rgb.strC1.Trim().Equals("SUB TOTAL")) lb1.Text = rgb.strC2.Trim() + " " + font_interlineado.strC3 + datosfactura.decC1.ToString("#,#.00");
            if (rgb.strC1.Trim().Equals("IMPUESTO")) lb1.Text = rgb.strC2.Trim() + " " + font_interlineado.strC3 + datosfactura.decC2.ToString("#,#.00");
            if (rgb.strC1.Trim().Equals("TOTAL")) lb1.Text = rgb.strC2.Trim() + " " + font_interlineado.strC3 + datosfactura.decC3.ToString("#,#.00");
            if (rgb.strC1.Trim().Equals("MONTO")) lb1.Text = rgb.strC2.Trim() + " " + font_interlineado.strC3 + datosfactura.decC3.ToString("#,#.00");
            if (rgb.strC1.Trim().Equals("FECHA PAGO")) lb1.Text = rgb.strC2.Trim() + " " + datosfactura.strC6;
            if (rgb.strC1.Trim().Equals("OBSERVACIONES")) lb1.Text = rgb.strC2.Trim() + " " + datosfactura.strC7;
            if (rgb.strC1.Trim().Equals("RAZON")) lb1.Text = rgb.strC2.Trim() + " " + datosfactura.strC26;
            //if (rgb.strC1.Trim().Equals("MONEDA")) lb1.Text = rgb.strC2.Trim() + " " + datosfactura.intC4.ToString();
            if (rgb.strC1.Trim().Equals("MONEDA")) lb1.Text = rgb.strC2.Trim() + " " + Utility.TraducirMonedaInt(datosfactura.intC4);
            if (rgb.strC1.Trim().Equals("USUARIO")) lb1.Text = rgb.strC2.Trim() + " " + datosfactura.strC8;
            if (rgb.strC1.Trim().Equals("HBL")) lb1.Text = rgb.strC2.Trim() + " " + datosfactura.strC9;
            if (rgb.strC1.Trim().Equals("MBL")) lb1.Text = rgb.strC2.Trim() + " " + datosfactura.strC10;
            if (rgb.strC1.Trim().Equals("CONTENEDOR")) lb1.Text = rgb.strC2.Trim() + " " + datosfactura.strC11;
            if (rgb.strC1.Trim().Equals("ROUTING")) lb1.Text = rgb.strC2.Trim() + " " + datosfactura.strC12;
            if (rgb.strC1.Trim().Equals("NAVIERA")) lb1.Text = rgb.strC2.Trim() + " " + datosfactura.strC13;
            if (rgb.strC1.Trim().Equals("VAPOR")) lb1.Text = rgb.strC2.Trim() + " " + datosfactura.strC14;
            if (rgb.strC1.Trim().Equals("SHIPPER")) lb1.Text = rgb.strC2.Trim() + " " + datosfactura.strC15;
            if (rgb.strC1.Trim().Equals("AGENTE")) lb1.Text = rgb.strC2.Trim() + " " + datosfactura.strC41;
            if (rgb.strC1.Trim().Equals("POLIZA ADUANAL")) lb1.Text = rgb.strC2.Trim() + " " + datosfactura.strC16;
            if (rgb.strC1.Trim().Equals("CONSIGNATARIO")) lb1.Text = rgb.strC2.Trim() + " " + datosfactura.strC17;
            if (rgb.strC1.Trim().Equals("COMODITY")) lb1.Text = rgb.strC2.Trim() + " " + datosfactura.strC18;
            if (rgb.strC1.Trim().Equals("PAQUETES")) lb1.Text = rgb.strC2.Trim() + " " + datosfactura.strC19;
            if (rgb.strC1.Trim().Equals("PESO")) lb1.Text = rgb.strC2.Trim() + " " + datosfactura.strC20;
            if (rgb.strC1.Trim().Equals("VOLUMEN")) lb1.Text = rgb.strC2.Trim() + " " + datosfactura.strC21;
            if (rgb.strC1.Trim().Equals("DUA INGRESO")) lb1.Text = rgb.strC2.Trim() + " " + datosfactura.strC22;
            if (rgb.strC1.Trim().Equals("DUA SALIDA")) lb1.Text = rgb.strC2.Trim() + " " + datosfactura.strC23;
            if (rgb.strC1.Trim().Equals("VENDEDOR 1")) lb1.Text = rgb.strC2.Trim() + " " + datosfactura.strC24;
            if (rgb.strC1.Trim().Equals("VENDEDOR 2")) lb1.Text = rgb.strC2.Trim() + " " + datosfactura.strC25;
            if (rgb.strC1.Trim().Equals("REFERENCIA")) lb1.Text = rgb.strC2.Trim() + " " + datosfactura.strC27;
            //
            if (rgb.strC1.Trim().Equals("SERIE")) lb1.Text = rgb.strC2.Trim() + " " + datosfactura.strC32;
            if (rgb.strC1.Trim().Equals("CORRELATIVO")) lb1.Text = rgb.strC2.Trim() + " " + datosfactura.strC33;
            if (rgb.strC1.Trim().Equals("DETALLE PAGOS")) lb1.Text = rgb.strC2.Trim() + " " + detalle_pagos;
            if (rgb.strC1.Trim().Equals("DESCRIPCION")) lb1.Text = rgb.strC2.Trim() + " " + datosfactura.strC35;
            if (rgb.strC1.Trim().Equals("TERMINOS")) lb1.Text = rgb.strC2.Trim() + " " + datosfactura.strC36;
            if (rgb.strC1.Trim().Equals("NO DE COTIZACION")) lb1.Text = rgb.strC2.Trim() + " " + datosfactura.strC37;
            if (rgb.strC1.Trim().Equals("DEPARTAMENTO")) lb1.Text = rgb.strC2.Trim() + " " + datosfactura.strC38;
            if (rgb.strC1.Trim().Equals("CONTACTO")) lb1.Text = rgb.strC2.Trim() + " " + datosfactura.strC39;
            if (rgb.strC1.Trim().Equals("CREADOR")) lb1.Text = rgb.strC2.Trim() + " " + datosfactura.strC40;
            if (rgb.strC1.Trim().Equals("ENCABEZADO")) lb1.Text = rgb.strC2.Trim();//Encabezado Contrasenas
            if (rgb.strC1.Trim().Equals("TOTAL LETRAS")) lb1.Text = rgb.strC2.Trim() + " " + datosfactura.strC29;
            if (rgb.strC1.Trim().Equals("DOC. REFERENCIA (CHEQUE/DEPOSITO)")) lb1.Text = rgb.strC2.Trim() + " " + datosfactura.strC30;
            if (rgb.strC1.Trim().Equals("BANCO")) lb1.Text = rgb.strC2.Trim() + " " + datosfactura.strC31;
            if (rgb.strC1.Trim().Equals("TIPO CAMBIO")) lb1.Text = rgb.strC2.Trim() + " " + user.pais.TipoCambio;
            if (rgb.strC1.Trim().Equals("NOTA DOCUMENTO")) lb1.Text = rgb.strC2.Trim();
            if (rgb.strC1.Trim().Equals("FECHA IMPRESION")) lb1.Text = rgb.strC2.Trim() + " " + DateTime.Now.ToShortDateString();
            if (rgb.strC1.Trim().Equals("HORA IMPRESION")) lb1.Text = rgb.strC2.Trim() + " " + DateTime.Now.ToShortTimeString();
            if (rgb.strC1.Trim().Equals("CODIGO CLIENTE")) lb1.Text = rgb.strC2.Trim() + " " + datosfactura.intC3;
            if (rgb.strC1.Trim().Equals("SERIE FACTURA")) lb1.Text = rgb.strC2.Trim() + " " + datosfactura.strC28;
            if (rgb.strC1.Trim().Equals("CORRELATIVO FACTURA")) lb1.Text = rgb.strC2.Trim() + " " + datosfactura.strC1;
            if (rgb.strC1.Trim().Equals("RECIBO ADUANA")) lb1.Text = rgb.strC2.Trim() + " " + datosfactura.strC31;
            if (rgb.strC1.Trim().Equals("SUBTOTAL NORMALES")) lb1.Text = rgb.strC2.Trim() + " " + subtotalnormal;
            if (rgb.strC1.Trim().Equals("SUBTOTAL EXCENTOS")) lb1.Text = rgb.strC2.Trim() + " " + subtotalexcento;
            if (rgb.strC1.Trim().Equals("ENCABEZADO")) lb1.Text = rgb.strC2.Trim();
            if (rgb.strC1.Trim().Equals("GIRO")) lb1.Text = rgb.strC2.Trim() + " " + datosfactura.strC45;
            if (rgb.strC1.Trim().Equals("RUC")) lb1.Text = rgb.strC2.Trim() + " " + datosfactura.strC44;
            #region Datos Almacen
            double almacen_subtotal = 0;
            double almacen_iva = 0;
            double almacen_total = 0;
            double almacen_anticipos = 0;
            double almacen_saldo_pagar = 0;
            string recibos = "";
            string valor_aduanero = "";
            string aduana = "";
            string recibo_agencia = "";
            string dua_salida = "";
            dua_salida = datosfactura.strC23;
            if (dua_salida.Length >= 3)
            {
                if ((dua_salida != "") && (dua_salida != null))
                {
                    if (dua_salida.Substring(0, 3) == "001")
                    {
                        aduana = "CENTRAL";
                    }
                    else if (dua_salida.Substring(0, 3) == "002")
                    {
                        aduana = "CALDERAS";
                    }
                    else if (dua_salida.Substring(0, 3) == "003")
                    {
                        aduana = "PENAS BLANCAS";
                    }
                    else if (dua_salida.Substring(0, 3) == "005")
                    {
                        aduana = "SANTAMARIA";
                    }
                    else if (dua_salida.Substring(0, 3) == "006")
                    {
                        aduana = "LIMON";
                    }
                    else if (dua_salida.Substring(0, 3) == "007")
                    {
                        aduana = "PASO CANOAS";
                    }
                }
            }
            DataTable dt_anticipos = new DataTable();
            dt_anticipos = DB.getRecibosAbonanFacturas(fac_id);
            foreach (DataRow dr in dt_anticipos.Rows)
            {
                almacen_anticipos += double.Parse(dr[0].ToString());
                recibos += dr[1].ToString() + " " + dr[2].ToString() + ", ";
            }
            recibo_agencia = datosfactura.strC42;
            valor_aduanero = datosfactura.strC43;
            almacen_subtotal = double.Parse(datosfactura.decC1.ToString());
            almacen_iva = double.Parse(datosfactura.decC2.ToString());
            almacen_total = almacen_subtotal + almacen_iva;
            almacen_saldo_pagar = almacen_total - almacen_anticipos;
            if (rgb.strC1.Trim().Equals("VALOR ADUANERO")) lb1.Text = rgb.strC2.Trim() + " " + valor_aduanero;
            if (rgb.strC1.Trim().Equals("ADUANA")) lb1.Text = rgb.strC2.Trim() + " " + aduana;
            if (rgb.strC1.Trim().Equals("RECIBO AGENCIA")) lb1.Text = rgb.strC2.Trim() + " " + recibo_agencia;
            if (rgb.strC1.Trim().Equals("SUB TOTAL ALMACEN")) lb1.Text = rgb.strC2.Trim() + " " + Utility.TraducirMonedaInt(datosfactura.intC4) + " " + almacen_subtotal;
            if (rgb.strC1.Trim().Equals("IMPUESTO VENTA ALMACEN")) lb1.Text = rgb.strC2.Trim() + " " + Utility.TraducirMonedaInt(datosfactura.intC4) + " " + almacen_iva;
            if (rgb.strC1.Trim().Equals("TOTAL ALMACEN")) lb1.Text = rgb.strC2.Trim() + " " + Utility.TraducirMonedaInt(datosfactura.intC4) + " " + almacen_total;
            if (rgb.strC1.Trim().Equals("ANTICIPOS ALMACEN")) lb1.Text = rgb.strC2.Trim() + " " + Utility.TraducirMonedaInt(datosfactura.intC4) + " " + almacen_anticipos.ToString();
            if (rgb.strC1.Trim().Equals("SALDO A PAGAR ALMACEN")) lb1.Text = rgb.strC2.Trim() + " " + Utility.TraducirMonedaInt(datosfactura.intC4) + " " + almacen_saldo_pagar.ToString();
            if (rgb.strC1.Trim().Equals("SALDO A PAGAR ALMACEN NO.2")) lb1.Text = rgb.strC2.Trim() + " " + Utility.TraducirMonedaInt(datosfactura.intC4) + " " + almacen_saldo_pagar.ToString();
            if (rgb.strC1.Trim().Equals("RECIBOS")) lb1.Text = rgb.strC2.Trim() + " " + recibos;
            #endregion
            if (!IsCredito)
            {
                if (rgb.strC1.Trim().Equals("MARCA CONTADO")) lb1.Text = rgb.strC2.Trim() + " X";
            }
            else
            {
                if (rgb.strC1.Trim().Equals("MARCA CREDITO")) lb1.Text = rgb.strC2.Trim() + " X";
            }
            if ((tipo == 1 || tipo == 4) || (tipo == 14) || (tipo == 3) || (tipo == 18))
            {
                if (rgb.strC1.Trim().Equals("RUBRO_NOMBRE"))
                {
                    xnombre_ini = rgb.intC1;
                    ynombre_ini = rgb.intC2;
                }
                if (rgb.strC1.Trim().Equals("RUBRO_SUBTOTAL"))
                {
                    xsubt_ini = rgb.intC1;
                    ysubt_ini = rgb.intC2;
                }
                if (rgb.strC1.Trim().Equals("RUBRO_IMPUESTO"))
                {
                    ximp_ini = rgb.intC1;
                    yimp_ini = rgb.intC2;
                }
                if (rgb.strC1.Trim().Equals("RUBRO_TOTAL"))
                {
                    xtot_ini = rgb.intC1;
                    ytot_ini = rgb.intC2;
                }
                if (rgb.strC1.Trim().Equals("RUBRO_COMENTARIO"))
                {
                    xcomentario_ini = rgb.intC1;
                    ycomentario_ini = rgb.intC2;
                }
                if (rgb.strC1.Trim().Equals("RUBRO MONEDA EQ"))
                {
                    xrubromonedaeq_ini = rgb.intC1;
                    yrubromonedaeq_ini = rgb.intC2;
                }
            }
            lb1.Attributes.Add("Style", "left: " + rgb.intC1 + "px; top: " + rgb.intC2 + "px; position: absolute;");
            if (font_interlineado.strC2 != null && !font_interlineado.strC2.Equals("")) lb1.Font.Name = font_interlineado.strC2;
            if (font_interlineado.intC7 > 0) lb1.Font.Size = font_interlineado.intC7;
            Page.Controls.Add(lb1);

        }
        //************************************************************************
        if ((tipo == 1 || tipo == 4 || tipo == 14 || tipo == 3 || tipo == 18) && (dt != null && dt.Rows.Count > 0))
        {
            int interlineado = 18;
            if (font_interlineado.intC6 != 0) interlineado = font_interlineado.intC6;
            Label lb = null;
            Label lb_comentario = null;
            if (datosfactura.strC30 == null || datosfactura.strC30.Equals(""))
            {
                foreach (DataRow dr in dt.Rows)
                {
                    if (xnombre_ini > 0 && ynombre_ini > 0)
                    {
                        // dibujo el nombre del rubro
                        lb = new Label();
                        lb_comentario = new Label();
                        if (rgb_cliente != null)
                        {
                            if (rgb_cliente.intC2 == 0)
                            {
                                lb.Text = dr[1].ToString();
                                lb_comentario.Text = dr[8].ToString();
                            }
                            else
                            {
                                lb.Text = DB.getAliasRubro(datosfactura.intC8, long.Parse(dr[0].ToString()), datosfactura.intC3, dr[1].ToString());
                                lb_comentario.Text = dr[8].ToString();
                            }
                        }
                        else
                        {
                            lb.Text = dr[1].ToString();
                            lb_comentario.Text = dr[8].ToString();
                        }
                        lb.Attributes.Add("Style", "left: " + xnombre_ini + "px; top: " + ynombre_ini + "px; position: absolute;");
                        if (lb_comentario.Text == "")//No Hay Comentario
                        {
                            ynombre_ini += interlineado;
                        }
                        else// Hay Comentario
                        {
                            ynombre_ini += interlineado;
                            lb_comentario.Attributes.Add("Style", "left: " + xnombre_ini + "px; top: " + ynombre_ini + "px; position: absolute;");
                            ynombre_ini += interlineado;
                        }
                        if (font_interlineado.strC2 != null && !font_interlineado.strC2.Equals("")) lb.Font.Name = font_interlineado.strC2;
                        if (font_interlineado.strC2 != null && !font_interlineado.strC2.Equals("")) lb_comentario.Font.Name = font_interlineado.strC2;
                        if (font_interlineado.intC7 > 0) lb.Font.Size = font_interlineado.intC7;
                        if (font_interlineado.intC7 > 0) lb_comentario.Font.Size = font_interlineado.intC7;
                        Page.Controls.Add(lb);
                        Page.Controls.Add(lb_comentario);
                    }
                    if (xsubt_ini > 0 && ysubt_ini > 0)
                    {
                        //dibujo el subtotal
                        lb = new Label();
                        texto = decimal.Parse(dr[5].ToString()).ToString("#,#.00");
                        lb.Text = font_interlineado.strC3 + texto.ToString() + "<br>";
                        lb.Attributes.Add("Style", "left: " + xsubt_ini + "px; top: " + ysubt_ini + "px; position: absolute;");
                        if (dr[8].ToString() == "")//No Hay Comentario
                        {
                            ysubt_ini += interlineado;
                        }
                        else// Hay Comentario
                        {
                            ysubt_ini += interlineado;
                            ysubt_ini += interlineado;
                        }
                        if (font_interlineado.strC2 != null && !font_interlineado.strC2.Equals("")) lb.Font.Name = font_interlineado.strC2;
                        if (font_interlineado.intC7 > 0) lb.Font.Size = font_interlineado.intC7;
                        Page.Controls.Add(lb);
                    }
                    if (ximp_ini > 0 && yimp_ini > 0)
                    {
                        //dibujo el impuestox
                        lb = new Label();
                        texto = decimal.Parse(dr[6].ToString()).ToString("#,#.00");
                        lb.Text = font_interlineado.strC3 + texto.ToString() + "<br>";
                        lb.Attributes.Add("Style", "left: " + ximp_ini + "px; top: " + yimp_ini + "px; position: absolute;");
                        if (dr[8].ToString() == "")//No Hay Comentario
                        {
                            yimp_ini += interlineado;
                        }
                        else// Hay Comentario
                        {
                            yimp_ini += interlineado;
                            yimp_ini += interlineado;
                        }
                        if (font_interlineado.strC2 != null && !font_interlineado.strC2.Equals("")) lb.Font.Name = font_interlineado.strC2;
                        if (font_interlineado.intC7 > 0) lb.Font.Size = font_interlineado.intC7;
                        Page.Controls.Add(lb);
                    }
                    if (xtot_ini > 0 && ytot_ini > 0)
                    {
                        //dibujo el Total
                        lb = new Label();
                        texto = decimal.Parse(dr[7].ToString()).ToString("#,#.00");
                        lb.Text = font_interlineado.strC3 + texto.ToString() + "<br>";
                        //verifico si el rubro es excento o no pero antes veo si hay configurado corrimiento
                        int excento = 0;
                        if (font_interlineado.intC8 != 0) excento = DB.getRubroExcento(datosfactura.intC8, long.Parse(dr[0].ToString()));
                        // si es excento le sumo el corrimiento sino, lo dejo igual
                        if (excento == 0) lb.Attributes.Add("Style", "left: " + (xtot_ini + font_interlineado.intC8) + "px; top: " + ytot_ini + "px; position: absolute;");
                        else lb.Attributes.Add("Style", "left: " + xtot_ini + "px; top: " + ytot_ini + "px; position: absolute;");
                        if (dr[8].ToString() == "")//No Hay Comentario
                        {
                            ytot_ini += interlineado;
                        }
                        else// Hay Comentario
                        {
                            ytot_ini += interlineado;
                            ytot_ini += interlineado;
                        }
                        if (font_interlineado.strC2 != null && !font_interlineado.strC2.Equals("")) lb.Font.Name = font_interlineado.strC2;
                        if (font_interlineado.intC7 > 0) lb.Font.Size = font_interlineado.intC7;
                        Page.Controls.Add(lb);
                    }
                    if (xcomentario_ini > 0 && ycomentario_ini > 0)
                    {
                        //dibujo Comentarios
                        lb = new Label();
                        lb.Text = dr[8].ToString();
                        lb.Attributes.Add("Style", "left: " + xcomentario_ini + "px; top: " + ycomentario_ini + "px; position: absolute;");
                        if (font_interlineado.strC2 != null && !font_interlineado.strC2.Equals("")) lb.Font.Name = font_interlineado.strC2;
                        if (font_interlineado.intC7 > 0) lb.Font.Size = font_interlineado.intC7;
                        ycomentario_ini += interlineado;
                        Page.Controls.Add(lb);
                    }
                    if (xrubroequivalente_ini > 0 && yrubroequivalente_ini > 0)
                    {
                        //dibujo Rubro Total Equivalente
                        lb = new Label();
                        lb.Text = dr[4].ToString();
                        //e.Graphics.DrawString(lb.Text, Fuente, Brushes.Black, xrubroequivalente_ini, yrubroequivalente_ini);
                        lb.Attributes.Add("Style", "left: " + xrubroequivalente_ini + "px; top: " + yrubroequivalente_ini + "px; position: absolute;");
                        if (dr[8].ToString() == "")//No Hay Comentario
                        {
                            yrubroequivalente_ini += interlineado;
                        }
                        else// Hay Comentario
                        {
                            yrubroequivalente_ini += interlineado;
                            yrubroequivalente_ini += interlineado;
                        }
                        if (font_interlineado.strC2 != null && !font_interlineado.strC2.Equals("")) lb.Font.Name = font_interlineado.strC2;
                        if (font_interlineado.intC7 > 0) lb.Font.Size = font_interlineado.intC7;
                    }
                    if (xrubromoneda_ini > 0 && yrubromoneda_ini > 0)
                    {
                        //dibujo Moneda del Rubro
                        lb = new Label();
                        lb.Text = dr[3].ToString();
                        //e.Graphics.DrawString(lb.Text, Fuente, Brushes.Black, xrubromoneda_ini, yrubromoneda_ini);
                        lb.Attributes.Add("Style", "left: " + xrubromoneda_ini + "px; top: " + xrubromoneda_ini + "px; position: absolute;");
                        if (dr[8].ToString() == "")//No Hay Comentario
                        {
                            yrubromoneda_ini += interlineado;
                        }
                        else// Hay Comentario
                        {
                            yrubromoneda_ini += interlineado;
                            yrubromoneda_ini += interlineado;
                        }
                        if (font_interlineado.strC2 != null && !font_interlineado.strC2.Equals("")) lb.Font.Name = font_interlineado.strC2;
                        if (font_interlineado.intC7 > 0) lb.Font.Size = font_interlineado.intC7;
                    }
                    if (xrubromonedaeq_ini > 0 && yrubromonedaeq_ini > 0)
                    {
                        //Dibujo la Moneda Equivalente
                        string mnd_eq = "";
                        if (datosfactura.intC4 == 8)
                        {
                            mnd_eq = Utility.TraducirMonedaInt(user.PaisID);
                        }
                        else
                        {
                            mnd_eq = Utility.TraducirMonedaInt(datosfactura.intC4);
                        }
                        lb = new Label();
                        lb.Text = mnd_eq;
                        //e.Graphics.DrawString(lb.Text, Fuente, Brushes.Black, xrubromonedaeq_ini, yrubromonedaeq_ini);
                        lb.Attributes.Add("Style", "left: " + xrubromonedaeq_ini + "px; top: " + yrubromonedaeq_ini + "px; position: absolute;");
                        if (dr[8].ToString() == "")//No Hay Comentario
                        {
                            yrubromonedaeq_ini += interlineado;
                        }
                        else// Hay Comentario
                        {
                            yrubromonedaeq_ini += interlineado;
                            yrubromonedaeq_ini += interlineado;
                        }
                        if (font_interlineado.strC2 != null && !font_interlineado.strC2.Equals("")) lb.Font.Name = font_interlineado.strC2;
                        if (font_interlineado.intC7 > 0) lb.Font.Size = font_interlineado.intC7;
                    }
                }
            }
            else
            {
                // dibujo el nombre del rubro
                lb = new Label();
                lb.Text = datosfactura.strC30;
                lb.Attributes.Add("Style", "left: " + xnombre_ini + "px; top: " + ynombre_ini + "px; position: absolute;");
                if (font_interlineado.strC2 != null && !font_interlineado.strC2.Equals("")) lb.Font.Name = font_interlineado.strC2;
                if (font_interlineado.intC7 > 0) lb.Font.Size = font_interlineado.intC7;
                ynombre_ini += interlineado;
                Page.Controls.Add(lb);

                //dibujo el Total
                lb = new Label();
                lb.Text = font_interlineado.strC3 + datosfactura.decC3.ToString("#,#.00");
                lb.Attributes.Add("Style", "left: " + xtot_ini + "px; top: " + ytot_ini + "px; position: absolute;");
                if (font_interlineado.strC2 != null && !font_interlineado.strC2.Equals("")) lb.Font.Name = font_interlineado.strC2;
                if (font_interlineado.intC7 > 0) lb.Font.Size = font_interlineado.intC7;
                ytot_ini += interlineado;
                Page.Controls.Add(lb);

            }
            // dibujo los anticipos si es que tiene
            if (user.PaisID == 1)
            {
                if (abonofactura > 0)
                {
                    lb = new Label();
                    lb.Text = "Total de recibos de anticipo: " + abonofactura.ToString("#,#.00");
                    lb.Attributes.Add("Style", "left: " + xtot_ini + "px; top: " + ytot_ini + "px; position: absolute;");
                    if (font_interlineado.strC2 != null && !font_interlineado.strC2.Equals("")) lb.Font.Name = font_interlineado.strC2;
                    if (font_interlineado.intC7 > 0) lb.Font.Size = font_interlineado.intC7;
                    ytot_ini += interlineado;
                    Page.Controls.Add(lb);
                }
            }
        }
    }

    #region Impresion de Documentos
    private void Imprimir_Documento()
    {
        //ImpersonationSettings settings = new ImpersonationSettings();
        //UserImpersonation userImpersonation = new UserImpersonation(settings);
        WindowsImpersonationContext wic = WindowsIdentity.Impersonate(IntPtr.Zero);
        try
        {
            //userImpersonation.Impersonate();
            //Actividad que deseemos realizar con mayores permisos
            string PrinterName = "";
            try
            {
                PrintDocument pd = new PrintDocument();
                PrinterName = user.PrinterName;
                if (PrinterName == "0")
                {
                    WebMsgBox.Show("El documento que esta tratando de Imprimir no tiene ninguna impresora asignada");
                    return;
                }
                RE_GenericBean Paper_Bean = (RE_GenericBean)DB.getPaperName(DOC_ID);
                if (user.ImpresionBean.Operacion == "1")
                {
                    #region Impresion
                    if ((user.ImpresionBean.Tipo_Documento == tipo.ToString()) && (user.ImpresionBean.Id == fac_id.ToString())&&(user.ImpresionBean.Impreso==false))
                    {
                        pd.PrinterSettings.PrinterName = PrinterName;
                        #region Definir Tamanio del Papel
                        if (Paper_Bean.intC1 > 0)
                        {
                            foreach (PaperSize Tamanio_Papel in pd.PrinterSettings.PaperSizes)
                                if (Tamanio_Papel.PaperName.Equals(Paper_Bean.strC1))
                                    pd.DefaultPageSettings.PaperSize = Tamanio_Papel;
                        }
                        else
                        {
                            foreach (PaperSize Tamanio_Papel in pd.PrinterSettings.PaperSizes)
                                if (Tamanio_Papel.PaperName.StartsWith("A4"))
                                    pd.DefaultPageSettings.PaperSize = Tamanio_Papel;
                        }
                        #endregion
                        pd.PrintPage += new PrintPageEventHandler(this.pd_PrintPage);
                        pd.Print();
                        user = (UsuarioBean)Session["usuario"];
                        user.ImpresionBean.Impreso = true;
                        Session["usuario"] = user;
                    }
                    #endregion
                }
                else if (user.ImpresionBean.Operacion == "2")
                {
                    #region Re-Impresion
                    pd.PrinterSettings.PrinterName = PrinterName;
                    #region Definir Tamanio del Papel
                    if (Paper_Bean.intC1 > 0)
                    {
                        foreach (PaperSize Tamanio_Papel in pd.PrinterSettings.PaperSizes)
                            if (Tamanio_Papel.PaperName.Equals(Paper_Bean.strC1))
                                pd.DefaultPageSettings.PaperSize = Tamanio_Papel;
                    }
                    else
                    {
                        foreach (PaperSize Tamanio_Papel in pd.PrinterSettings.PaperSizes)
                            if (Tamanio_Papel.PaperName.StartsWith("A4"))
                                pd.DefaultPageSettings.PaperSize = Tamanio_Papel;
                    }
                    #endregion 
                    pd.PrintPage += new PrintPageEventHandler(this.pd_PrintPage);
                    pd.Print();
                    #endregion
                }
            }
            catch (Exception ex)
            {
                log4net ErrLog = new log4net();
                ErrLog.ErrorLog(ex.Message);
            }
        }
        finally
        {
            //userImpersonation.UndoImpersonation();
            wic.Undo();
        }
    }
    public DataSet oset = new DataSet();
    private void pd_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
    {
        try
        {
            string serieDoc = "";
            int xnombre_ini = 0, ynombre_ini = 0;
            int xrubro_id_ini = 0, yrubro_id_ini = 0;
            int x_ventas_no_sujetas_ini = 0, y_ventas_no_sujetas_ini = 0;
            int x_ventas_afectas_ini = 0, y_ventas_afectas_ini = 0;
            int x_ventas_exentas_ini = 0, y_ventas_exentas_ini = 0;
            int xsubt_ini = 0, ysubt_ini = 0;
            int ximp_ini = 0, yimp_ini = 0;
            int xtot_ini = 0, ytot_ini = 0;
            int xcomentario_ini = 0, ycomentario_ini = 0;
            int xpago_ini = 0, ypago_ini = 0;
            int xdocumentos_ini = 0, ydocumentos_ini = 0;
            int xrubroequivalente_ini = 0, yrubroequivalente_ini = 0;
            int xrubro_subtotal_equivalente_ini = 0, yrubro_subtotal_equivalente_ini = 0;
            int xrubromoneda_ini = 0, yrubromoneda_ini = 0;
            int xrubromonedaeq_ini = 0, yrubromonedaeq_ini = 0;

            int xrubroequivalente_erial_ini = 0, yrubroequivalente_erial_ini = 0;
            int xrubromonedaeq_erial_ini = 0, yrubromonedaeq_erial_ini = 0;
            decimal subtotalnormal = 0, subtotalexcento = 0, abonofactura = 0;
            string texto;
            string detalle_pagos = "";
            bool IsCredito = false;
            Font Fuente = new Font("Courier New", 10);
            DataTable dt = null;
            RE_GenericBean datosfactura = null;
            RE_GenericBean font_interlineado = null;
            RE_GenericBean rgb_cliente = null;
            ArrayList retenciones = new ArrayList();
            DataTable dt_pagos = null;
            Double ban1 = 0.90;
            Double ban2 =1.20 ;
            if (tipo == 1)
            { //factura
                datosfactura = (RE_GenericBean)DB.getFacturaData(fac_id);
                tpi = int.Parse(datosfactura.strC58);
                if ((user.PaisID == 9))
                {
                    datosfactura.decC1 = datosfactura.decC3;
                }
                if ((user.PaisID == 9) || (user.PaisID == 2))
                {
                    //retencion = DB.getRetencioByFactID(datosfactura.intC1);
                    retenciones = (ArrayList)DB.getRetencioIVAByFactID(datosfactura.intC1);
                    foreach (RE_GenericBean arr in retenciones)
                    {
                        arr.decC3 = Math.Round(((arr.decC1 / datosfactura.decC6) * 100), 2);
                        if ((arr.decC3 >= decimal.Parse(ban1.ToString())) && (arr.decC3 <= decimal.Parse(ban2.ToString())))  //SI ES IVA RETENIDO 1%
                        {
                            retencionIVA1 = arr.decC1;

                        }
                        else
                        {
                            retencionIVA15 = arr.decC1;
                        }
                    }
                }
                
                if (user.PaisID == 9)
                {
                    datosfactura.decC3 = datosfactura.decC3 - retencion;
                }
                if (user.PaisID == 2)
                {
                    datosfactura.decC3 = datosfactura.decC3 - retencionIVA1;
                }
                serieDoc = datosfactura.strC28;//Factura
                if (tpi == 3)
                {
                    rgb_cliente = (RE_GenericBean)DB.getDataClient((int)datosfactura.intC3);
                }
                else if (tpi == 10)
                {
                    rgb_cliente = (RE_GenericBean)DB.getIntercompanyData((int)datosfactura.intC3);
                    rgb_cliente.intC2 = 0;
                }
                dt = (DataTable)DB.getRubbyFact_For_Print(fac_id, tipo,user);
                abonofactura = (decimal)DB.getTotalAbonadoXFact(fac_id, 2);//indica que la referencia es de recibo
                IsCredito = DB.EsClienteCredito(datosfactura.intC3, user.PaisID);
                int excento = 0;
                foreach (DataRow dr in dt.Rows)
                {
                    excento = DB.getRubroExcento(datosfactura.intC8, long.Parse(dr[0].ToString()));
                    if (excento == 0) subtotalnormal += decimal.Parse(dr[5].ToString());
                    else if (excento > 0) subtotalexcento += decimal.Parse(dr[5].ToString());
                }
            }
            else if (tipo == 4)
            { //Nota Debito
                datosfactura = (RE_GenericBean)DB.getNotaDebitoData_forprint(fac_id);
                dt = (DataTable)DB.getRubbyFact_For_Print(fac_id, tipo,user);
                serieDoc = datosfactura.strC28;
            }
            else if (tipo == 2)
            {  // Recibos
                datosfactura = (RE_GenericBean)DB.getRcptData(fac_id);
                serieDoc = datosfactura.strC32;
            }
            else if (tipo == 14)
            { //factura proforma
                datosfactura = (RE_GenericBean)DB.getFacturaData_proforma(fac_id);
                serieDoc = datosfactura.strC28;
                rgb_cliente = (RE_GenericBean)DB.getDataClient((int)datosfactura.intC3);
                dt = (DataTable)DB.getRubbyFact_For_Print(fac_id, tipo,user);
                IsCredito = DB.EsClienteCredito(datosfactura.intC3, 1);
            }
            else if ((tipo == 3) || (tipo == 18))
            { //Nota Credito
                datosfactura = (RE_GenericBean)DB.getNotaCreditoData(fac_id);
                serieDoc = datosfactura.strC32;
                //Ir a traer el Total de la Nota de Credito en el query porque no regresa ese dato el que regresa es el de la factura
                dt = (DataTable)DB.getRubbyFact_For_Print(fac_id, tipo, user);
            }
            else if (tipo == 15)
            { //Ajuste Contable
                datosfactura = (RE_GenericBean)DB.getFacturaData_proforma(fac_id);
                rgb_cliente = (RE_GenericBean)DB.getDataClient((int)datosfactura.intC3);
                dt = (DataTable)DB.getRubbyFact_For_Print(fac_id, tipo, user);
                IsCredito = DB.EsClienteCredito(datosfactura.intC3, user.PaisID);
            }
            else if (tipo == 7)
            { //Orden de Compra
                datosfactura = (RE_GenericBean)DB.getOCData_forprint(fac_id);
                serieDoc = datosfactura.strC28;
            }
            else if (tipo == 8)
            { //Contrasenias Factura
                datosfactura = (RE_GenericBean)DB.getContrasenaData_forprint(fac_id);
                serieDoc = datosfactura.strC32;
            }
            int doc_id = DB.getDocumentoID(datosfactura.intC2, serieDoc, tipo, user);
            font_interlineado = DB.getFactura(doc_id, tipo);
            ArrayList camposfact = (ArrayList)DB.getCamposDoc(tipo, datosfactura.intC2, serieDoc);//parametros 1=factura (tipo doc); 2=sucursal de la factura; 3=serie de factura
            Label lb1 = null;
            foreach (RE_GenericBean rgb in camposfact)
            {
                // dibujo el campo
                lb1 = new Label();
                lb1.Text = "";
                if (rgb.strC1.Trim().Equals("RETENCION")) lb1.Text = rgb.strC2.Trim() + " " + retencion;
                if (user.PaisID == 2)
                {
                    if (retencionIVA1 != 0)
                    {
                        if (rgb.strC1.Trim().Equals("RETENCIONIVA1")) lb1.Text = rgb.strC2.Trim() + " (-)IVA RETENIDO 1%  $" + retencionIVA1;
                    }
                    if (retencionIVA15 != 0)
                    {
                        if (rgb.strC1.Trim().Equals("RETENCIONIVA15")) lb1.Text = rgb.strC2.Trim() + "(+)IVA PERCIBIDO  $" + retencionIVA15;
                    }
                }
                if (user.PaisID == 9)
                {
                    if (retencionIVA15 != 0)
                    {
                        if (rgb.strC1.Trim().Equals("RETENCIONIVA15")) lb1.Text = rgb.strC2.Trim() + " $" + retencionIVA15;
                    }
                    if (retencionIVA1 != 0)
                    {
                        if (rgb.strC1.Trim().Equals("RETENCIONIVA1")) lb1.Text = rgb.strC2.Trim() + " $" + retencionIVA1;
                    }

                }
                if (rgb.strC1.Trim().Equals("FECHA EMISION")) lb1.Text = rgb.strC2.Trim() + " " + datosfactura.strC5;
                if (rgb.strC1.Trim().Equals("FECHA (D)")) lb1.Text = rgb.strC2.Trim() + " " + DateTime.Parse(datosfactura.strC5).Day.ToString();
                if (rgb.strC1.Trim().Equals("FECHA (M)")) lb1.Text = rgb.strC2.Trim() + " " + DateTime.Parse(datosfactura.strC5).Month.ToString();
                if (rgb.strC1.Trim().Equals("FECHA (M) LETRAS"))
                {
                    if (user.Idioma == 2)
                    {
                        lb1.Text = rgb.strC2.Trim() + " " + Utility.MesfromIntToStr_english(DateTime.Parse(datosfactura.strC5).Month);
                    }
                    else
                    {
                        lb1.Text = rgb.strC2.Trim() + " " + Utility.MesfromIntToStr(DateTime.Parse(datosfactura.strC5).Month);
                    }
                }
                if (rgb.strC1.Trim().Equals("FECHA (A)")) lb1.Text = rgb.strC2.Trim() + " " + DateTime.Parse(datosfactura.strC5).Year.ToString();
                if (rgb.strC1.Trim().Equals("NOMBRE")) lb1.Text = rgb.strC2.Trim() + " " + datosfactura.strC3;
                if (rgb.strC1.Trim().Equals("POR_CUENTA_DE")) lb1.Text = rgb.strC2.Trim() + " " + datosfactura.strC3;
                if (rgb.strC1.Trim().Equals("CONCEPTO"))
                {
                    if (user.PaisID != 11)
                    {
                        lb1.Text = rgb.strC2.Trim() + " " + DB.getDocsByRec(fac_id);
                    }
                    else 
                    {
                        lb1.Text = Segmentar_Cadena_generica(rgb.strC2.Trim() + " " + DB.getDocsByRec(fac_id), 90);
                    }
                }
                if (rgb.strC1.Trim().Equals("CONCEPTO_DETALLADO"))
                {
                    xdocumentos_ini = rgb.intC1;
                    ydocumentos_ini = rgb.intC2;
                }
                if (rgb.strC1.Trim().Equals("NOTA_DE")) lb1.Text = rgb.strC2.Trim();
                if (rgb.strC1.Trim().Equals("SIRVASE_DE")) lb1.Text = rgb.strC2.Trim();
                if (rgb.strC1.Trim().Equals("CIUDAD")) lb1.Text = rgb.strC2.Trim();
                if (rgb.strC1.Trim().Equals("COMENTARIO1")) lb1.Text = rgb.strC2.Trim();
                if (rgb.strC1.Trim().Equals("COMENTARIO2")) lb1.Text = rgb.strC2.Trim();
                if (rgb.strC1.Trim().Equals("DIRECCION"))
                {
                    if ((user.PaisID != 2) && (user.PaisID != 9))
                    {
                        lb1.Text = rgb.strC2.Trim() + " " + datosfactura.strC4;
                    }
                    else
                    {
                        lb1.Text = Segmentar_Cadena_Con_Retorno(rgb.strC2.Trim() + " " + datosfactura.strC4, tipo);
                    }
                }
                if (rgb.strC1.Trim().Equals("NIT")) lb1.Text = rgb.strC2.Trim() + " " + datosfactura.strC2;
                if (rgb.strC1.Trim().Equals("SUB TOTAL")) lb1.Text = rgb.strC2.Trim() + " " + font_interlineado.strC3 + datosfactura.decC1.ToString("#,#.00");
                if (rgb.strC1.Trim().Equals("IMPUESTO")) lb1.Text = rgb.strC2.Trim() + " " + font_interlineado.strC3 + datosfactura.decC2.ToString("#,#.00");
                if (rgb.strC1.Trim().Equals("TOTAL")) lb1.Text = rgb.strC2.Trim() + " " + font_interlineado.strC3 + datosfactura.decC3.ToString("#,#.00");
                if (rgb.strC1.Trim().Equals("TOTAL_LOCAL")) lb1.Text = rgb.strC2.Trim() + " " + datosfactura.decC3.ToString("#,#.00");
                if (rgb.strC1.Trim().Equals("TOTAL_EQUIVALENTE")) lb1.Text = rgb.strC2.Trim() + " " +  datosfactura.decC4.ToString("#,#.00");
                if (rgb.strC1.Trim().Equals("TOTAL_HN")) lb1.Text = rgb.strC2.Trim() + " " + font_interlineado.strC3 + datosfactura.decC3.ToString("#,#.00");
                if ((rgb.strC1.Trim().Equals("SUBTOTAL_HN")) || (rgb.strC1.Trim().Equals("IMPUESTO_HN")) || (rgb.strC1.Trim().Equals("TOTAL_HN")))
                {
                    #region Totales Formateados Honduras
                    StringFormat drawFormat = new StringFormat();
                    drawFormat.FormatFlags = StringFormatFlags.DirectionRightToLeft;
                    if (rgb.strC1.Trim().Equals("SUBTOTAL_HN")) lb1.Text = rgb.strC2.Trim() + " " + font_interlineado.strC3 + datosfactura.decC1.ToString("#,#.00");
                    if (rgb.strC1.Trim().Equals("IMPUESTO_HN")) lb1.Text = rgb.strC2.Trim() + " " + font_interlineado.strC3 + datosfactura.decC2.ToString("#,#.00");
                    if (rgb.strC1.Trim().Equals("TOTAL_HN")) lb1.Text = rgb.strC2.Trim() + " " + font_interlineado.strC3 + datosfactura.decC3.ToString("#,#.00");
                    e.Graphics.DrawString(lb1.Text, Fuente, Brushes.Black, rgb.intC1, rgb.intC2, drawFormat);
                    #endregion
                }
                if ((rgb.strC1.Trim().Equals("IMPUESTO_NC")) || (rgb.strC1.Trim().Equals("SUB_TOTAL_NC")) || (rgb.strC1.Trim().Equals("TOTAL_NC"))|| (rgb.strC1.Trim().Equals("VENTAS_AFECTAS_NC")))
                {
                    #region Toles Formateados NC de El Salvador
                    StringFormat drawFormat = new StringFormat();
                    drawFormat.FormatFlags = StringFormatFlags.DirectionRightToLeft;
                    if (rgb.strC1.Trim().Equals("IMPUESTO_NC")) lb1.Text = rgb.strC2.Trim() + " " + font_interlineado.strC3 + datosfactura.decC5.ToString("#,#.00");
                    if (rgb.strC1.Trim().Equals("SUB_TOTAL_NC")) lb1.Text = rgb.strC2.Trim() + " " + font_interlineado.strC3 + datosfactura.decC4.ToString("#,#.00");
                    if (rgb.strC1.Trim().Equals("TOTAL_NC")) lb1.Text = rgb.strC2.Trim() + " " + font_interlineado.strC3 + datosfactura.decC3.ToString("#,#.00");
                    if (rgb.strC1.Trim().Equals("VENTAS_AFECTAS_NC")) lb1.Text = rgb.strC2.Trim() + " " + font_interlineado.strC3 + datosfactura.decC4.ToString("#,#.00");
                    e.Graphics.DrawString(lb1.Text, Fuente, Brushes.Black, rgb.intC1, rgb.intC2, drawFormat);
                    #endregion
                }
                if ((rgb.strC1.Trim().Equals("SUBTOTAL_NI_EQA")) || rgb.strC1.Trim().Equals("IMPUESTO_NI_EQA") || rgb.strC1.Trim().Equals("TOTAL_NI_EQA") || rgb.strC1.Trim().Equals("SUBTOTAL_EQUIVALENTE"))
                {
                    #region Impresiones Nicaragua
                    StringFormat drawFormat = new StringFormat();
                    drawFormat.FormatFlags = StringFormatFlags.DirectionRightToLeft;
                    if (rgb.strC1.Trim().Equals("SUBTOTAL_NI_EQA")) lb1.Text = rgb.strC2.Trim() + " " + font_interlineado.strC3 + datosfactura.decC6.ToString("#,#.00");
                    if (rgb.strC1.Trim().Equals("SUBTOTAL_EQUIVALENTE")) lb1.Text = rgb.strC2.Trim() + " " + font_interlineado.strC3 + datosfactura.decC6.ToString("#,#.00");
                    if (rgb.strC1.Trim().Equals("IMPUESTO_NI_EQA")) lb1.Text = rgb.strC2.Trim() + " " + font_interlineado.strC3 + datosfactura.decC5.ToString("#,#.00");
                    if (rgb.strC1.Trim().Equals("TOTAL_NI_EQA")) lb1.Text = rgb.strC2.Trim() + " " + font_interlineado.strC3 + datosfactura.decC4.ToString("#,#.00");
                    e.Graphics.DrawString(lb1.Text, Fuente, Brushes.Black, rgb.intC1, rgb.intC2, drawFormat);
                    #endregion
                }
                if (rgb.strC1.Trim().Equals("MONTO")) lb1.Text = rgb.strC2.Trim() + " " + font_interlineado.strC3 + datosfactura.decC3.ToString("#,#.00");
                if (rgb.strC1.Trim().Equals("FECHA PAGO")) lb1.Text = rgb.strC2.Trim() + " " + datosfactura.strC6;
                if (rgb.strC1.Trim().Equals("OBSERVACIONES")) lb1.Text = rgb.strC2.Trim() + " " + datosfactura.strC7;
                if (rgb.strC1.Trim().Equals("OTRAS_OBSERVACIONES")) lb1.Text = rgb.strC2.Trim() + " " + datosfactura.strC46;
                if (rgb.strC1.Trim().Equals("RAZON")) lb1.Text = rgb.strC2.Trim() + " " + datosfactura.strC26;
                if (rgb.strC1.Trim().Equals("MONEDA")) lb1.Text = rgb.strC2.Trim() + " " + datosfactura.intC4.ToString();
                if (rgb.strC1.Trim().Equals("MONEDA")) lb1.Text = rgb.strC2.Trim() + " " + Utility.TraducirMonedaInt(datosfactura.intC4);
                if (rgb.strC1.Trim().Equals("USUARIO")) lb1.Text = rgb.strC2.Trim() + " " + datosfactura.strC8;
                if (rgb.strC1.Trim().Equals("HBL")) lb1.Text = rgb.strC2.Trim() + " " + datosfactura.strC9;
                if (rgb.strC1.Trim().Equals("MBL")) lb1.Text = rgb.strC2.Trim() + " " + datosfactura.strC10;
                if (rgb.strC1.Trim().Equals("CONTENEDOR")) lb1.Text = rgb.strC2.Trim() + " " + datosfactura.strC11;
                if (rgb.strC1.Trim().Equals("ROUTING")) lb1.Text = rgb.strC2.Trim() + " " + datosfactura.strC12;
                if (rgb.strC1.Trim().Equals("NAVIERA")) lb1.Text = rgb.strC2.Trim() + " " + datosfactura.strC13;
                if (rgb.strC1.Trim().Equals("VAPOR")) lb1.Text = rgb.strC2.Trim() + " " + datosfactura.strC14;
                if (rgb.strC1.Trim().Equals("SHIPPER")) lb1.Text = rgb.strC2.Trim() + " " + datosfactura.strC15;
                if (rgb.strC1.Trim().Equals("AGENTE")) lb1.Text = rgb.strC2.Trim() + " " + datosfactura.strC41;
                if (rgb.strC1.Trim().Equals("AGENTE_ID")) lb1.Text = rgb.strC2.Trim() + " " + datosfactura.intC10.ToString();
                if (rgb.strC1.Trim().Equals("POLIZA ADUANAL")) lb1.Text = rgb.strC2.Trim() + " " + datosfactura.strC16;
                if (rgb.strC1.Trim().Equals("CONSIGNATARIO")) lb1.Text = rgb.strC2.Trim() + " " + datosfactura.strC17;
                if (rgb.strC1.Trim().Equals("COMODITY"))
                {
                    if ((datosfactura.strC18.Length > 63) && (user.PaisID == 3))
                    {
                        lb1.Text = rgb.strC2.Trim() + " " + datosfactura.strC18.Substring(0, 63);
                    }
                    else
                    {
                        lb1.Text = rgb.strC2.Trim() + " " + datosfactura.strC18;
                    }
                }
                if (rgb.strC1.Trim().Equals("PAQUETES")) lb1.Text = rgb.strC2.Trim() + " " + datosfactura.strC19;
                if (rgb.strC1.Trim().Equals("PESO")) lb1.Text = rgb.strC2.Trim() + " " + datosfactura.strC20;
                if (rgb.strC1.Trim().Equals("VOLUMEN")) lb1.Text = rgb.strC2.Trim() + " " + datosfactura.strC21;
                if (rgb.strC1.Trim().Equals("DUA INGRESO")) lb1.Text = rgb.strC2.Trim() + " " + datosfactura.strC22;
                if (rgb.strC1.Trim().Equals("DUA SALIDA")) lb1.Text = rgb.strC2.Trim() + " " + datosfactura.strC23;
                if (rgb.strC1.Trim().Equals("VENDEDOR 1")) lb1.Text = rgb.strC2.Trim() + " " + datosfactura.strC24;
                if (rgb.strC1.Trim().Equals("VENDEDOR 2")) lb1.Text = rgb.strC2.Trim() + " " + datosfactura.strC25;
                if (rgb.strC1.Trim().Equals("REFERENCIA")) lb1.Text = rgb.strC2.Trim() + " " + datosfactura.strC27;
                if (rgb.strC1.Trim().Equals("SERIE")) lb1.Text = rgb.strC2.Trim() + " " + datosfactura.strC32;
                if (rgb.strC1.Trim().Equals("CORRELATIVO")) lb1.Text = rgb.strC2.Trim() + " " + datosfactura.strC33;
                if (rgb.strC1.Trim().Equals("DETALLE PAGOS"))
                {
                    xpago_ini = rgb.intC1;
                    ypago_ini = rgb.intC2;
                }
                if (rgb.strC1.Trim().Equals("DESCRIPCION")) lb1.Text = rgb.strC2.Trim() + " " + datosfactura.strC35;
                if (rgb.strC1.Trim().Equals("TERMINOS")) lb1.Text = rgb.strC2.Trim() + " " + datosfactura.strC36;
                if (rgb.strC1.Trim().Equals("NO DE COTIZACION")) lb1.Text = rgb.strC2.Trim() + " " + datosfactura.strC37;
                if (rgb.strC1.Trim().Equals("DEPARTAMENTO")) lb1.Text = rgb.strC2.Trim() + " " + datosfactura.strC38;
                if (rgb.strC1.Trim().Equals("CONTACTO")) lb1.Text = rgb.strC2.Trim() + " " + datosfactura.strC39;
                if (rgb.strC1.Trim().Equals("CREADOR")) lb1.Text = rgb.strC2.Trim() + " " + datosfactura.strC40;
                if (rgb.strC1.Trim().Equals("ENCABEZADO")) lb1.Text = rgb.strC2.Trim();//Encabezado Contrasenas
                //if (rgb.strC1.Trim().Equals("DETALLE_ABONOS_ND")) lb1.Text = rgb.strC2.Trim();
                //if (rgb.strC1.Trim().Equals("TOTAL_ABONOS_ND")) 
                //{
                //    lb1.Text = DB.getTotalAbonadoNotaDebito_For_Print(fac_id).ToString();
                //}
                #region Datos Almacen
                double almacen_subtotal = 0;
                double almacen_iva = 0;
                double almacen_total = 0;
                double almacen_anticipos = 0;
                double almacen_saldo_pagar = 0;
                string recibos = "";
                string valor_aduanero = "";
                string aduana = "";
                string recibo_agencia = "";
                string dua_salida = "";
                dua_salida = datosfactura.strC23;
                if (dua_salida.Length >= 3)
                {
                    if ((dua_salida != "") && (dua_salida != null))
                    {
                        if (dua_salida.Substring(0, 3) == "001")
                        {
                            aduana = "CENTRAL";
                        }
                        else if (dua_salida.Substring(0, 3) == "002")
                        {
                            aduana = "CALDERAS";
                        }
                        else if (dua_salida.Substring(0, 3) == "003")
                        {
                            aduana = "PENAS BLANCAS";
                        }
                        else if (dua_salida.Substring(0, 3) == "005")
                        {
                            aduana = "SANTAMARIA";
                        }
                        else if (dua_salida.Substring(0, 3) == "006")
                        {
                            aduana = "LIMON";
                        }
                        else if (dua_salida.Substring(0, 3) == "007")
                        {
                            aduana = "PASO CANOAS";
                        }
                    }
                }
                DataTable dt_anticipos = new DataTable();
                dt_anticipos = DB.getRecibosAbonanFacturas(fac_id);
                foreach (DataRow dr in dt_anticipos.Rows)
                {
                    almacen_anticipos += double.Parse(dr[0].ToString());
                    recibos += dr[1].ToString() + " " + dr[2].ToString() + ", ";
                }
                recibo_agencia = datosfactura.strC42;
                valor_aduanero = datosfactura.strC43;
                almacen_subtotal = double.Parse(datosfactura.decC1.ToString());
                almacen_iva = double.Parse(datosfactura.decC2.ToString());
                almacen_total = almacen_subtotal + almacen_iva;
                almacen_saldo_pagar = almacen_total - almacen_anticipos;
                if (rgb.strC1.Trim().Equals("VALOR ADUANERO")) lb1.Text = rgb.strC2.Trim() + " " + valor_aduanero;
                if (rgb.strC1.Trim().Equals("ADUANA")) lb1.Text = rgb.strC2.Trim() + " " + aduana;
                if (rgb.strC1.Trim().Equals("RECIBO AGENCIA")) lb1.Text = rgb.strC2.Trim() + " " + recibo_agencia;
                if (rgb.strC1.Trim().Equals("SUB TOTAL ALMACEN"))
                {
                    lb1.Text = rgb.strC2.Trim() + " " + Utility.TraducirMonedaInt(datosfactura.intC4) + " " + almacen_subtotal;
                    espacios = "";
                    espacios = DB.Rellenar_Espacios(rgb.strC2.Trim(), almacen_subtotal.ToString("#,#.00#;(#,#.00#)"), datosfactura.intC4);
                    monto_auxiliar = "";
                    monto_auxiliar = espacios + almacen_subtotal.ToString("#,#.00#;(#,#.00#)");
                    monto_auxiliar = DB.Invertir(monto_auxiliar);
                    char[] Monto_Subtotal = monto_auxiliar.ToCharArray();
                    x = rgb.intC1;
                    y = rgb.intC2;
                    foreach (char caracter in Monto_Subtotal)
                    {
                        Fuente = new Font(font_interlineado.strC2, font_interlineado.intC7);
                        e.Graphics.DrawString(caracter.ToString(), Fuente, Brushes.Black, x, y);
                        x = x - 9;
                    }
                }
                if (rgb.strC1.Trim().Equals("IMPUESTO VENTA ALMACEN"))
                {
                    lb1.Text = rgb.strC2.Trim() + " " + Utility.TraducirMonedaInt(datosfactura.intC4) + " " + almacen_iva;
                    espacios = "";
                    espacios = DB.Rellenar_Espacios(rgb.strC2.Trim(), almacen_iva.ToString("#,#.00#;(#,#.00#)"), datosfactura.intC4);
                    monto_auxiliar = "";
                    monto_auxiliar = espacios + almacen_iva.ToString("#,#.00#;(#,#.00#)");
                    monto_auxiliar = DB.Invertir(monto_auxiliar);
                    char[] Monto_Impuesto = monto_auxiliar.ToCharArray();
                    x = rgb.intC1;
                    y = rgb.intC2;
                    foreach (char caracter in Monto_Impuesto)
                    {
                        Fuente = new Font(font_interlineado.strC2, font_interlineado.intC7);
                        e.Graphics.DrawString(caracter.ToString(), Fuente, Brushes.Black, x, y);
                        x = x - 9;
                    }
                }
                if (rgb.strC1.Trim().Equals("TOTAL ALMACEN"))
                {
                    lb1.Text = rgb.strC2.Trim() + " " + Utility.TraducirMonedaInt(datosfactura.intC4) + " " + almacen_total;
                    espacios = "";
                    espacios = DB.Rellenar_Espacios(rgb.strC2.Trim(), almacen_total.ToString("#,#.00#;(#,#.00#)"), datosfactura.intC4);
                    monto_auxiliar = "";
                    monto_auxiliar = espacios + almacen_total.ToString("#,#.00#;(#,#.00#)");
                    monto_auxiliar = DB.Invertir(monto_auxiliar);
                    char[] Monto_Total = monto_auxiliar.ToCharArray();
                    x = rgb.intC1;
                    y = rgb.intC2;
                    foreach (char caracter in Monto_Total)
                    {
                        Fuente = new Font(font_interlineado.strC2, font_interlineado.intC7);
                        e.Graphics.DrawString(caracter.ToString(), Fuente, Brushes.Black, x, y);
                        x = x - 9;
                    }
                }
                if (rgb.strC1.Trim().Equals("ANTICIPOS ALMACEN"))
                {
                    lb1.Text = rgb.strC2.Trim() + " " + Utility.TraducirMonedaInt(datosfactura.intC4) + " " + almacen_anticipos.ToString();
                    espacios = "";
                    espacios = DB.Rellenar_Espacios(rgb.strC2.Trim(), almacen_anticipos.ToString("#,#.00#;(#,#.00#)"), datosfactura.intC4);
                    monto_auxiliar = "";
                    monto_auxiliar = espacios + almacen_anticipos.ToString("#,#.00#;(#,#.00#)");
                    monto_auxiliar = DB.Invertir(monto_auxiliar);
                    char[] Monto_Anticipos = monto_auxiliar.ToCharArray();
                    x = rgb.intC1;
                    y = rgb.intC2;
                    foreach (char caracter in Monto_Anticipos)
                    {
                        Fuente = new Font(font_interlineado.strC2, font_interlineado.intC7);
                        e.Graphics.DrawString(caracter.ToString(), Fuente, Brushes.Black, x, y);
                        x = x - 9;
                    }
                }
                if (rgb.strC1.Trim().Equals("SALDO A PAGAR ALMACEN"))
                {
                    lb1.Text = rgb.strC2.Trim() + " " + Utility.TraducirMonedaInt(datosfactura.intC4) + " " + almacen_saldo_pagar.ToString();
                    espacios = "";
                    espacios = DB.Rellenar_Espacios(rgb.strC2.Trim(), almacen_saldo_pagar.ToString("#,#.00#;(#,#.00#)"), datosfactura.intC4);
                    monto_auxiliar = "";
                    monto_auxiliar = espacios + almacen_saldo_pagar.ToString("#,#.00#;(#,#.00#)");
                    monto_auxiliar = DB.Invertir(monto_auxiliar);
                    char[] Monto_Anticipos = monto_auxiliar.ToCharArray();
                    x = rgb.intC1;
                    y = rgb.intC2;
                    foreach (char caracter in Monto_Anticipos)
                    {
                        Fuente = new Font(font_interlineado.strC2, font_interlineado.intC7);
                        e.Graphics.DrawString(caracter.ToString(), Fuente, Brushes.Black, x, y);
                        x = x - 9;
                    }
                }
                if (rgb.strC1.Trim().Equals("RECIBOS")) lb1.Text = rgb.strC2.Trim() + " " + recibos;
                #endregion
                if (rgb.strC1.Trim().Equals("TOTAL LETRAS"))
                {
                    Conv c = new Conv();
                    if (user.Idioma == 2)//Ingles
                        lb1.Text = rgb.strC2.Trim() + " " + c.enletras(datosfactura.decC3.ToString(), 8);
                    else
                        lb1.Text = rgb.strC2.Trim() + " " + c.enletras(datosfactura.decC3.ToString(), 1);
                }
                if (rgb.strC1.Trim().Equals("TOTAL_LETRAS_EQUIVALENTE"))
                {
                    Conv c = new Conv();
                    if (user.Idioma == 2)//Ingles
                        lb1.Text = rgb.strC2.Trim() + " " + c.enletras(datosfactura.decC4.ToString(), 8);
                    else
                        lb1.Text = rgb.strC2.Trim() + " " + c.enletras(datosfactura.decC4.ToString(), 1);
                }
                if (rgb.strC1.Trim().Equals("DOC. REFERENCIA (CHEQUE/DEPOSITO)")) lb1.Text = rgb.strC2.Trim() + " " + datosfactura.strC30;
                if (rgb.strC1.Trim().Equals("BANCO")) lb1.Text = rgb.strC2.Trim() + " " + datosfactura.strC31;
                if (rgb.strC1.Trim().Equals("TIPO CAMBIO")) lb1.Text = rgb.strC2.Trim() + " " + user.pais.TipoCambio;
                if (rgb.strC1.Trim().Equals("NOTA DOCUMENTO")) lb1.Text = rgb.strC2.Trim();
                if (rgb.strC1.Trim().Equals("FECHA IMPRESION")) lb1.Text = rgb.strC2.Trim() + " " + DateTime.Now.ToShortDateString();
                if (rgb.strC1.Trim().Equals("HORA IMPRESION")) lb1.Text = rgb.strC2.Trim() + " " + DateTime.Now.ToShortTimeString();
                if (rgb.strC1.Trim().Equals("CODIGO CLIENTE")) lb1.Text = rgb.strC2.Trim() + " " + datosfactura.intC3;
                if (rgb.strC1.Trim().Equals("SERIE FACTURA")) lb1.Text = rgb.strC2.Trim() + " " + datosfactura.strC28;
                if (rgb.strC1.Trim().Equals("CORRELATIVO FACTURA")) lb1.Text = rgb.strC2.Trim() + " " + datosfactura.strC1;
                if (rgb.strC1.Trim().Equals("RECIBO ADUANA")) lb1.Text = rgb.strC2.Trim() + " " + datosfactura.strC31;
                if (rgb.strC1.Trim().Equals("SUBTOTAL NORMALES")) lb1.Text = rgb.strC2.Trim() + " " + subtotalnormal;
                if (rgb.strC1.Trim().Equals("SUBTOTAL EXCENTOS")) lb1.Text = rgb.strC2.Trim() + " " + subtotalexcento;
                if (rgb.strC1.Trim().Equals("ENCABEZADO")) lb1.Text = rgb.strC2.Trim();
                if (rgb.strC1.Trim().Equals("GIRO"))
                {
                    lb1.Text = rgb.strC2.Trim() + " " + datosfactura.strC45;
                }
                if (rgb.strC1.Trim().Equals("TOTAL_DESCUENTO"))
                {
                    lb1.Text = rgb.strC2.Trim() + " " + font_interlineado.strC3 + datosfactura.decC1.ToString("#,#.00");
                }
                if (rgb.strC1.Trim().Equals("NOMBRE_USUARIO"))
                {
                    RE_GenericBean Usuario_Bean = (RE_GenericBean)DB.getUsuarioEmpresa(datosfactura.strC8);
                    lb1.Text = rgb.strC2.Trim() + " " + Usuario_Bean.strC1;
                }
                if (rgb.strC1.Trim().Equals("NIT_USUARIO"))
                {
                    RE_GenericBean User_Bean = (RE_GenericBean)DB.getUsuarioEmpresa(datosfactura.strC8);
                    lb1.Text = rgb.strC2.Trim() + " " + User_Bean.strC2;
                }

                if (rgb.strC1.Trim().Equals("RUC")) lb1.Text = rgb.strC2.Trim() + " " + datosfactura.strC44;
                #region CALCULOS CENTRAL SV
                if (tipo == 1)//Factura
                {
                    #region Obtener Regimen del Cliente
                    if (tpi == 3)
                    {
                        rgb_cliente = (RE_GenericBean)DB.getDataClient(double.Parse(datosfactura.intC3.ToString()));
                    }
                    else if (tpi == 10)
                    {
                        rgb_cliente = (RE_GenericBean)DB.getIntercompanyData(datosfactura.intC3);
                        rgb_cliente.intC2 = 0;
                    }
                    #endregion
                    if ((font_interlineado.intC11 == 3) || (font_interlineado.intC11 == 4) || (font_interlineado.intC11 == 2))//Credito Fiscal o Factura Exportacion o Consumidor Final
                    {
                        decimal tot_ventas_afectas = 0;
                        decimal tot_ventas_afectas_con_iva = 0;
                        decimal tot_ventas_no_sujetas = 0;
                        decimal tot_iva_ventas_afectas = 0;
                        decimal tot_ventas_exentas = 0;
                        decimal tot_ventas_exentas_con_iva = 0;
                        decimal subtotal_sv = 0;

                        if ((rgb.strC1.Trim().Equals("TOTAL_VENTAS_AFECTAS")) || (rgb.strC1.Trim().Equals("TOTAL_IVA_VENTAS_AFECTAS")) || (rgb.strC1.Trim().Equals("SUB_TOTAL_SV")) || (rgb.strC1.Trim().Equals("TOTAL_VENTAS_NO_SUJETAS")) || (rgb.strC1.Trim().Equals("TOTAL_VENTAS_EXENTAS")))
                        {
                            foreach (DataRow dr in dt.Rows)
                            {
                                //Error
                                Rubros rubro = new Rubros();
                                rubro.rubroID = long.Parse(dr[0].ToString());
                                Rubros Rubro_Detalle = (Rubros)rubro;
                                Rubro_Detalle = (Rubros)DB.ExistRubroPais(rubro, user.PaisID);
                                if (tpi == 3)
                                {                             
                                    if (rgb_cliente.intC1 == 1)//Excento
                                    {
                                        #region Total Ventas Exentas
                                        tot_ventas_exentas += decimal.Parse(dr[5].ToString());
                                        #endregion
                                    }
                                    else//Contribuyente
                                    {
                                        if (Rubro_Detalle.CobIva == 0)//Excento
                                        {
                                            #region Total Ventas No Sujetas
                                            //Monto Sin IVA
                                            tot_ventas_no_sujetas += decimal.Parse(dr[5].ToString());
                                            #endregion
                                        }
                                        else if (Rubro_Detalle.CobIva == 1)//No Excento
                                        {
                                            #region Total Ventas Afectas
                                            //Monto Sin IVA
                                            tot_ventas_afectas += decimal.Parse(dr[5].ToString());
                                            tot_iva_ventas_afectas += decimal.Parse(dr[6].ToString());
                                            #endregion
                                        }
                                    }
                                }
                                else if (tpi == 10)
                                {
                                    if (rgb_cliente.strC3 == "1")//Excento
                                    {
                                        #region Total Ventas Exentas
                                        tot_ventas_exentas += decimal.Parse(dr[5].ToString());
                                        #endregion
                                    }
                                    else//Contribuyente
                                    {
                                        if (Rubro_Detalle.CobIva == 0)//Excento
                                        {
                                            #region Total Ventas No Sujetas
                                            //Monto Sin IVA
                                            tot_ventas_no_sujetas += decimal.Parse(dr[5].ToString());
                                            #endregion
                                        }
                                        else if (Rubro_Detalle.CobIva == 1)//No Excento
                                        {
                                            #region Total Ventas Afectas
                                            //Monto Sin IVA
                                            tot_ventas_afectas += decimal.Parse(dr[5].ToString());
                                            tot_iva_ventas_afectas += decimal.Parse(dr[6].ToString());
                                            #endregion
                                        }
                                    }
 
                                }
                               
                            } 
                        }
                        else if (rgb.strC1.Trim().Equals("TOTAL_VENTAS_EXENTAS_CON_IVA"))
                        {
                            foreach (DataRow dr in dt.Rows)
                            {
                                #region Total Ventas Exentas con Iva
                                tot_ventas_exentas_con_iva += decimal.Parse(dr[7].ToString());
                                #endregion
                            }
                        }
                        else if (rgb.strC1.Trim().Equals("TOTAL_VENTAS_AFECTAS_CON_IVA"))
                        {
                            #region Total Ventas Afectas con Iva
                            foreach (DataRow dr in dt.Rows)
                            {
                                Rubros rubro = new Rubros();
                                rubro.rubroID = long.Parse(dr[0].ToString());
                                Rubros Rubro_Detalle = (Rubros)rubro;
                                Rubro_Detalle = (Rubros)DB.ExistRubroPais(rubro, user.PaisID);
                                if (Rubro_Detalle.CobIva == 1)//No Excento
                                {
                                    #region Total Ventas Afectas
                                    //Monto Sin IVA
                                    tot_ventas_afectas_con_iva += decimal.Parse(dr[5].ToString());
                                    tot_ventas_afectas_con_iva += decimal.Parse(dr[6].ToString());
                                    #endregion
                                }
                            }
                            #endregion
                        }
                        subtotal_sv = tot_ventas_afectas + tot_iva_ventas_afectas;
                        if (rgb.strC1.Trim().Equals("TOTAL_VENTAS_AFECTAS")) lb1.Text = rgb.strC2.Trim() + " " + tot_ventas_afectas.ToString("#,#.00");
                        if (rgb.strC1.Trim().Equals("TOTAL_VENTAS_AFECTAS_CON_IVA")) lb1.Text = rgb.strC2.Trim() + " " + (tot_ventas_afectas_con_iva).ToString("#,#.00");
                        if (rgb.strC1.Trim().Equals("TOTAL_VENTAS_NO_SUJETAS")) lb1.Text = rgb.strC2.Trim() + " " + tot_ventas_no_sujetas.ToString("#,#.00");
                        if (rgb.strC1.Trim().Equals("TOTAL_IVA_VENTAS_AFECTAS")) lb1.Text = rgb.strC2.Trim() + " " + tot_iva_ventas_afectas.ToString("#,#.00");
                        if (rgb.strC1.Trim().Equals("SUB_TOTAL_SV")) lb1.Text = rgb.strC2.Trim() + " " + subtotal_sv.ToString("#,#.00");
                        if (rgb.strC1.Trim().Equals("TOTAL_VENTAS_EXENTAS")) lb1.Text = rgb.strC2.Trim() + " " + tot_ventas_exentas.ToString("#,#.00");
                        if (rgb.strC1.Trim().Equals("TOTAL_VENTAS_EXENTAS_CON_IVA")) lb1.Text = rgb.strC2.Trim() + " " + tot_ventas_exentas_con_iva.ToString("#,#.00");
                    }
                }
                #endregion
                if (IsCredito == false)
                {
                    if (rgb.strC1.Trim().Equals("MARCA CONTADO")) lb1.Text = rgb.strC2.Trim() + " X";
                }
                else if (IsCredito == true)
                {
                    if (rgb.strC1.Trim().Equals("MARCA CREDITO")) lb1.Text = rgb.strC2.Trim() + " X";
                }
                if ((tipo == 1 || tipo == 4) || (tipo == 14) || (tipo == 3) || (tipo == 18))
                {
                    if (rgb.strC1.Trim().Equals("RUBRO_NOMBRE"))
                    {
                        xnombre_ini = rgb.intC1;
                        ynombre_ini = rgb.intC2;
                    }
                    if (rgb.strC1.Trim().Equals("RUBRO_SUBTOTAL"))
                    {
                        xsubt_ini = rgb.intC1;
                        ysubt_ini = rgb.intC2;
                    }
                    if (rgb.strC1.Trim().Equals("RUBRO_IMPUESTO"))
                    {
                        ximp_ini = rgb.intC1;
                        yimp_ini = rgb.intC2;
                    }
                    if (rgb.strC1.Trim().Equals("RUBRO_TOTAL"))
                    {
                        xtot_ini = rgb.intC1;
                        ytot_ini = rgb.intC2;
                    }
                    if (rgb.strC1.Trim().Equals("RUBRO_COMENTARIO"))
                    {
                        xcomentario_ini = rgb.intC1;
                        ycomentario_ini = rgb.intC2;
                    }
                    if (rgb.strC1.Trim().Equals("RUBRO TOTAL EQUIVALENTE"))
                    {
                        xrubroequivalente_ini = rgb.intC1;
                        yrubroequivalente_ini = rgb.intC2;
                    }
                    if (rgb.strC1.Trim().Equals("RUBRO_SUBTOTAL_NI_EQA"))
                    {
                        xrubro_subtotal_equivalente_ini = rgb.intC1;
                        yrubro_subtotal_equivalente_ini = rgb.intC2;
                    }
                    if (rgb.strC1.Trim().Equals("RUBRO MONEDA"))
                    {
                        xrubromoneda_ini = rgb.intC1;
                        yrubromoneda_ini = rgb.intC2;
                    }
                    if (rgb.strC1.Trim().Equals("RUBRO MONEDA EQ"))
                    {
                        xrubromonedaeq_ini = rgb.intC1;
                        yrubromonedaeq_ini = rgb.intC2;
                    }
                    if (rgb.strC1.Trim().Equals("VENTAS_AFECTAS"))
                    {
                        x_ventas_afectas_ini = rgb.intC1;
                        y_ventas_afectas_ini = rgb.intC2;
                    }
                    if (rgb.strC1.Trim().Equals("VENTAS_NO_SUJETAS"))
                    {
                        x_ventas_no_sujetas_ini = rgb.intC1;
                        y_ventas_no_sujetas_ini = rgb.intC2;
                    }
                    if (rgb.strC1.Trim().Equals("RUBRO_ID"))
                    {
                        xrubro_id_ini = rgb.intC1;
                        yrubro_id_ini = rgb.intC2;
                    }
                    if (rgb.strC1.Trim().Equals("VENTAS_EXENTAS"))
                    {
                        x_ventas_exentas_ini = rgb.intC1;
                        y_ventas_exentas_ini = rgb.intC2;
                    }
                    if ((rgb.strC1.Trim().Equals("RUBRO TOTAL EQUIVALENTE ERIAL")) && (datosfactura.strC28 == "A2") && (datosfactura.intC3 == 24660))
                    {
                        xrubroequivalente_erial_ini = rgb.intC1;
                        yrubroequivalente_erial_ini = rgb.intC2;
                    }
                    if ((rgb.strC1.Trim().Equals("RUBRO MONEDA EQ ERIAL")) && (datosfactura.strC28 == "A2") && (datosfactura.intC3 == 24660))
                    {
                        xrubromonedaeq_erial_ini = rgb.intC1;
                        yrubromonedaeq_erial_ini = rgb.intC2;
                    }
                }
                Fuente = new Font(font_interlineado.strC2, font_interlineado.intC7);
                if ((!(rgb.strC1.Trim().Equals("SUB TOTAL ALMACEN"))) && (!(rgb.strC1.Trim().Equals("SALDO A PAGAR ALMACEN"))) && (!(rgb.strC1.Trim().Equals("ANTICIPOS ALMACEN"))) && (!(rgb.strC1.Trim().Equals("TOTAL ALMACEN"))) && (!(rgb.strC1.Trim().Equals("IMPUESTO VENTA ALMACEN"))) && (!(rgb.strC1.Trim().Equals("SUBTOTAL_HN"))) && (!(rgb.strC1.Trim().Equals("IMPUESTO_HN"))) && (!(rgb.strC1.Trim().Equals("TOTAL_HN"))) && (!(rgb.strC1.Trim().Equals("IMPUESTO_NC"))) && (!(rgb.strC1.Trim().Equals("SUB_TOTAL_NC"))) && (!(rgb.strC1.Trim().Equals("TOTAL_NC"))) && (!(rgb.strC1.Trim().Equals("VENTAS_AFECTAS_NC"))) && (!(rgb.strC1.Trim().Equals("SUBTOTAL_NI_EQA"))) && (!(rgb.strC1.Trim().Equals("IMPUESTO_NI_EQA"))) && (!(rgb.strC1.Trim().Equals("TOTAL_NI_EQA"))) && (!(rgb.strC1.Trim().Equals("SUBTOTAL_EQUIVALENTE"))))
                {
                    e.Graphics.DrawString(lb1.Text, Fuente, Brushes.Black, rgb.intC1, rgb.intC2);
                }
            }
            //**********************Recibos*******************************************

            if (tipo == 2)
            {  // Recibos
                int interlineado_pagos = 18;
                if (user.PaisID == 3)
                {
                    interlineado_pagos = 10;
                }
                dt_pagos = null;
                dt_pagos = (DataTable)DB.getPagosRecibo(fac_id);
                foreach (DataRow dr_pagos in dt_pagos.Rows)
                {
                    detalle_pagos = "";
                    detalle_pagos += dr_pagos[0].ToString() + " ";
                    detalle_pagos += dr_pagos[1].ToString() + " ";
                    detalle_pagos += dr_pagos[2].ToString() + " ";
                    detalle_pagos += dr_pagos[3].ToString() + " ";
                    detalle_pagos += decimal.Parse(dr_pagos[4].ToString()).ToString("#,#.00") + " ";
                    e.Graphics.DrawString(detalle_pagos, Fuente, Brushes.Black, xpago_ini, ypago_ini);
                    ypago_ini += interlineado_pagos;
                }
                //Cancelacion y/o Abono de los siguientes Documentos.:
                if ((xdocumentos_ini > 0) && (ydocumentos_ini > 0))
                {
                    Font Fuente2 = new Font("Arial", 9, FontStyle.Bold);
                    ArrayList Arr_Documentos = (ArrayList)DB.getDocsByRec_Detallado(fac_id);
                    foreach (RE_GenericBean Bean in Arr_Documentos)
                    {
                        string Campo = "[     " + Bean.strC3 + "     " + Bean.strC2 + "     " + Bean.strC1 + "     " + Bean.strC5 + "     " + decimal.Parse(Bean.strC4).ToString("#,#.00") + "    ]";
                        e.Graphics.DrawString(Campo, Fuente2, Brushes.Black, xdocumentos_ini, ydocumentos_ini);
                        ydocumentos_ini += 13;
                    }
                }
            }

            //************************************************************************
            if ((tipo == 1 || tipo == 4 || tipo == 14 || tipo == 3 || tipo == 18) && (dt != null && dt.Rows.Count > 0))
            {

                int interlineado = 18;
                if (font_interlineado.intC6 != 0) interlineado = font_interlineado.intC6;
                Label lb = null;
                Label lb_comentario = null;
                if (((tipo == 1) && (font_interlineado.intC11 == 1)) || (tipo == 4) || (tipo == 14) || (tipo == 3) || (tipo == 18))//Documento Natural
                {//Font_Interlineadi.intC11=1 es porque es Factura Natural
                    if (datosfactura.strC30 == null || datosfactura.strC30.Equals(""))//All In
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            if (xrubro_id_ini > 0 && yrubro_id_ini > 0)
                            {
                                //dibujo el ID del Rubro
                                lb = new Label();
                                texto = dr[0].ToString();
                                //lb.Text = font_interlineado.strC3 + texto.ToString();
                                lb.Text = texto.ToString();
                                e.Graphics.DrawString(lb.Text, Fuente, Brushes.Black, xrubro_id_ini, yrubro_id_ini);
                                if (dr[8].ToString() == "")//No Hay Comentario
                                {
                                    yrubro_id_ini += interlineado;
                                }
                                else// Hay Comentario
                                {
                                    yrubro_id_ini += interlineado;
                                    yrubro_id_ini += interlineado;
                                }
                                if (font_interlineado.strC2 != null && !font_interlineado.strC2.Equals("")) lb.Font.Name = font_interlineado.strC2;
                                if (font_interlineado.intC7 > 0) lb.Font.Size = font_interlineado.intC7;
                            }
                            if (xnombre_ini > 0 && ynombre_ini > 0)
                            {
                                // dibujo el nombre del rubro
                                lb = new Label();
                                lb_comentario = new Label();
                                if (rgb_cliente != null)
                                {
                                    if (rgb_cliente.intC2 == 0)
                                    {
                                        lb.Text = dr[1].ToString();
                                        lb_comentario.Text = dr[8].ToString();
                                    }
                                    else
                                    {
                                        lb.Text = DB.getAliasRubro(datosfactura.intC8, long.Parse(dr[0].ToString()), datosfactura.intC3, dr[1].ToString());
                                        lb_comentario.Text = dr[8].ToString();
                                    }
                                }
                                else
                                {
                                    lb.Text = dr[1].ToString();
                                    lb_comentario.Text = dr[8].ToString();
                                }
                                #region Marcar Rubro con un * si Cobra ITBMS
                                if (user.PaisID == 6)
                                {
                                    Rubros rubro = new Rubros();
                                    rubro.rubroID = long.Parse(dr[0].ToString());
                                    Rubros rubtemp = (Rubros)rubro;
                                    rubtemp = (Rubros)DB.ExistRubroPais(rubro, user.PaisID);
                                    if (rubtemp.CobIva == 1)
                                    {
                                        lb.Text = lb.Text + " *";
                                    }
                                }
                                #endregion
                                e.Graphics.DrawString(lb.Text, Fuente, Brushes.Black, xnombre_ini, ynombre_ini);
                                if (lb_comentario.Text == "")//No Hay Comentario
                                {
                                    ynombre_ini += interlineado;
                                }
                                else// Hay Comentario
                                {
                                    ynombre_ini += interlineado;
                                    e.Graphics.DrawString(lb_comentario.Text, Fuente, Brushes.Black, xnombre_ini, ynombre_ini);
                                    ynombre_ini += interlineado;
                                }
                                if (font_interlineado.strC2 != null && !font_interlineado.strC2.Equals("")) lb.Font.Name = font_interlineado.strC2;
                                if (font_interlineado.strC2 != null && !font_interlineado.strC2.Equals("")) lb_comentario.Font.Name = font_interlineado.strC2;
                                if (font_interlineado.intC7 > 0) lb.Font.Size = font_interlineado.intC7;
                                if (font_interlineado.intC7 > 0) lb_comentario.Font.Size = font_interlineado.intC7;
                            }
                            if (xsubt_ini > 0 && ysubt_ini > 0)
                            {
                                //dibujo el subtotal
                                lb = new Label();
                                texto = decimal.Parse(dr[5].ToString()).ToString("#,#.00");
                                lb.Text = texto.ToString();

                                espacios = "";
                                espacios = DB.Rellenar_Espacios("", decimal.Parse(dr[5].ToString()).ToString("#,#.00#;(#,#.00#)"), 0);
                                monto_auxiliar = "";
                                monto_auxiliar = espacios + decimal.Parse(dr[5].ToString()).ToString("#,#.00#;(#,#.00#)");
                                monto_auxiliar = DB.Invertir(monto_auxiliar);
                                char[] Monto_Subtotal = monto_auxiliar.ToCharArray();
                                x = xsubt_ini;
                                y = ysubt_ini;
                                foreach (char caracter in Monto_Subtotal)
                                {
                                    Fuente = new Font(font_interlineado.strC2, font_interlineado.intC7);
                                    e.Graphics.DrawString(caracter.ToString(), Fuente, Brushes.Black, x, y);
                                    x = x - 9;
                                }

                                if (dr[8].ToString() == "")//No Hay Comentario
                                {
                                    ysubt_ini += interlineado;
                                }
                                else// Hay Comentario
                                {
                                    ysubt_ini += interlineado;
                                    ysubt_ini += interlineado;
                                }
                                if (font_interlineado.strC2 != null && !font_interlineado.strC2.Equals("")) lb.Font.Name = font_interlineado.strC2;
                                if (font_interlineado.intC7 > 0) lb.Font.Size = font_interlineado.intC7;
                            }
                            if (ximp_ini > 0 && yimp_ini > 0)
                            {
                                //dibujo el impuestox
                                lb = new Label();
                                texto = decimal.Parse(dr[6].ToString()).ToString("#,#.00");
                                lb.Text = font_interlineado.strC3 + texto.ToString() + "<br>";
                                e.Graphics.DrawString(lb.Text, Fuente, Brushes.Black, ximp_ini, yimp_ini);
                                if (dr[8].ToString() == "")//No Hay Comentario
                                {
                                    yimp_ini += interlineado;
                                }
                                else// Hay Comentario
                                {
                                    yimp_ini += interlineado;
                                    yimp_ini += interlineado;
                                }
                                if (font_interlineado.strC2 != null && !font_interlineado.strC2.Equals("")) lb.Font.Name = font_interlineado.strC2;
                                if (font_interlineado.intC7 > 0) lb.Font.Size = font_interlineado.intC7;
                                //Page.Controls.Add(lb);
                            }
                            if (xtot_ini > 0 && ytot_ini > 0)
                            {
                                //dibujo el Total
                                lb = new Label();
                                texto = decimal.Parse(dr[7].ToString()).ToString("#,#.00");
                                lb.Text = font_interlineado.strC3 + texto.ToString();
                                //verifico si el rubro es excento o no pero antes veo si hay configurado corrimiento
                                int excento = 0;
                                //if (font_interlineado.intC8 != 0) excento = DB.getRubroExcento(datosfactura.intC8, long.Parse(dr[0].ToString()));
                                // si es excento le sumo el corrimiento sino, lo dejo igual

                                //if (excento == 0)
                                //e.Graphics.DrawString(lb.Text, Fuente, Brushes.Black, xtot_ini + font_interlineado.intC8, ytot_ini);
                                //else e.Graphics.DrawString(lb.Text, Fuente, Brushes.Black, xtot_ini, ytot_ini);


                                espacios = "";
                                espacios = DB.Rellenar_Espacios("", decimal.Parse(dr[7].ToString()).ToString("#,#.00#;(#,#.00#)"), 0);
                                monto_auxiliar = "";
                                monto_auxiliar = espacios + decimal.Parse(dr[7].ToString()).ToString("#,#.00#;(#,#.00#)");
                                monto_auxiliar = DB.Invertir(monto_auxiliar);
                                char[] Monto_Total = monto_auxiliar.ToCharArray();
                                if (excento == 0)
                                {
                                    x = xtot_ini + font_interlineado.intC8;
                                    y = ytot_ini;
                                }
                                else
                                {
                                    x = xtot_ini;
                                    y = ytot_ini;
                                }
                                foreach (char caracter in Monto_Total)
                                {
                                    Fuente = new Font(font_interlineado.strC2, font_interlineado.intC7);
                                    e.Graphics.DrawString(caracter.ToString(), Fuente, Brushes.Black, x, y);
                                    x = x - 9;
                                }
                                if (dr[8].ToString() == "")//No Hay Comentario
                                {
                                    ytot_ini += interlineado;
                                }
                                else// Hay Comentario
                                {
                                    ytot_ini += interlineado;
                                    ytot_ini += interlineado;
                                }
                                if (font_interlineado.strC2 != null && !font_interlineado.strC2.Equals("")) lb.Font.Name = font_interlineado.strC2;
                                if (font_interlineado.intC7 > 0) lb.Font.Size = font_interlineado.intC7;
                            }
                            /*
                            if (xcomentario_ini > 0 && ycomentario_ini > 0)
                            {
                                //dibujo Comentarios
                                lb = new Label();
                                lb.Text = dr[8].ToString();
                                e.Graphics.DrawString(lb.Text, Fuente, Brushes.Black, xcomentario_ini, ycomentario_ini);
                                if (font_interlineado.strC2 != null && !font_interlineado.strC2.Equals("")) lb.Font.Name = font_interlineado.strC2;
                                if (font_interlineado.intC7 > 0) lb.Font.Size = font_interlineado.intC7;
                                ycomentario_ini += interlineado;
                            }
                            */
                            if (xrubroequivalente_ini > 0 && yrubroequivalente_ini > 0)
                            {
                                //dibujo Rubro Total Equivalente
                                lb = new Label();
                                lb.Text = dr[4].ToString();
                                //e.Graphics.DrawString(lb.Text, Fuente, Brushes.Black, xrubroequivalente_ini, yrubroequivalente_ini);

                                espacios = "";
                                espacios = DB.Rellenar_Espacios("", decimal.Parse(dr[4].ToString()).ToString("#,#.00#;(#,#.00#)"), 0);
                                monto_auxiliar = "";
                                monto_auxiliar = espacios + decimal.Parse(dr[4].ToString()).ToString("#,#.00#;(#,#.00#)");
                                monto_auxiliar = DB.Invertir(monto_auxiliar);
                                char[] Monto_Equivalente = monto_auxiliar.ToCharArray();
                                x = xrubroequivalente_ini;
                                y = yrubroequivalente_ini;
                                foreach (char caracter in Monto_Equivalente)
                                {
                                    Fuente = new Font(font_interlineado.strC2, font_interlineado.intC7);
                                    e.Graphics.DrawString(caracter.ToString(), Fuente, Brushes.Black, x, y);
                                    x = x - 9;
                                }
                                if (dr[8].ToString() == "")//No Hay Comentario
                                {
                                    yrubroequivalente_ini += interlineado;
                                }
                                else// Hay Comentario
                                {
                                    yrubroequivalente_ini += interlineado;
                                    yrubroequivalente_ini += interlineado;
                                }
                                if (font_interlineado.strC2 != null && !font_interlineado.strC2.Equals("")) lb.Font.Name = font_interlineado.strC2;
                                if (font_interlineado.intC7 > 0) lb.Font.Size = font_interlineado.intC7;
                            }
                            if (xrubro_subtotal_equivalente_ini > 0 && yrubro_subtotal_equivalente_ini > 0)
                            {
                                //dibujo Rubro Total Subtotal Equivalente
                                StringFormat drawFormat = new StringFormat();
                                drawFormat.FormatFlags = StringFormatFlags.DirectionRightToLeft;
                                lb = new Label();
                                lb.Text = dr[9].ToString();
                                x = xrubro_subtotal_equivalente_ini;
                                y = yrubro_subtotal_equivalente_ini;
                                e.Graphics.DrawString(lb.Text, Fuente, Brushes.Black, x, y, drawFormat);
                                if (dr[8].ToString() == "")//No Hay Comentario
                                {
                                    yrubro_subtotal_equivalente_ini += interlineado;
                                }
                                else// Hay Comentario
                                {
                                    yrubro_subtotal_equivalente_ini += interlineado;
                                    yrubro_subtotal_equivalente_ini += interlineado;
                                }
                                if (font_interlineado.strC2 != null && !font_interlineado.strC2.Equals("")) lb.Font.Name = font_interlineado.strC2;
                                if (font_interlineado.intC7 > 0) lb.Font.Size = font_interlineado.intC7;
                            }
                            if (xrubromoneda_ini > 0 && yrubromoneda_ini > 0)
                            {
                                //dibujo Moneda del Rubro
                                lb = new Label();
                                lb.Text = dr[3].ToString();
                                e.Graphics.DrawString(lb.Text, Fuente, Brushes.Black, xrubromoneda_ini, yrubromoneda_ini);
                                if (dr[8].ToString() == "")//No Hay Comentario
                                {
                                    yrubromoneda_ini += interlineado;
                                }
                                else// Hay Comentario
                                {
                                    yrubromoneda_ini += interlineado;
                                    yrubromoneda_ini += interlineado;
                                }
                                if (font_interlineado.strC2 != null && !font_interlineado.strC2.Equals("")) lb.Font.Name = font_interlineado.strC2;
                                if (font_interlineado.intC7 > 0) lb.Font.Size = font_interlineado.intC7;
                            }
                            if (xrubromonedaeq_ini > 0 && yrubromonedaeq_ini > 0)
                            {
                                //Dibujo la Moneda Equivalente
                                int paiID = user.PaisID;
                                if ((user.PaisID == 11) || (user.PaisID == 12) || (user.PaisID == 13))
                                {
                                    paiID = 4;
                                }
                                string mnd_eq = "";
                                if (datosfactura.intC4 == paiID)
                                {
                                    mnd_eq = "USD";
                                }
                                else
                                {
                                    mnd_eq = Utility.TraducirMonedaInt(user.PaisID);
                                }
                                lb = new Label();
                                lb.Text = mnd_eq;
                                e.Graphics.DrawString(lb.Text, Fuente, Brushes.Black, xrubromonedaeq_ini, yrubromonedaeq_ini);
                                if (dr[8].ToString() == "")//No Hay Comentario
                                {
                                    yrubromonedaeq_ini += interlineado;
                                }
                                else// Hay Comentario
                                {
                                    yrubromonedaeq_ini += interlineado;
                                    yrubromonedaeq_ini += interlineado;
                                }
                                if (font_interlineado.strC2 != null && !font_interlineado.strC2.Equals("")) lb.Font.Name = font_interlineado.strC2;
                                if (font_interlineado.intC7 > 0) lb.Font.Size = font_interlineado.intC7;
                            }
                            #region Caso Erial
                            if (xrubroequivalente_erial_ini > 0 && yrubroequivalente_erial_ini > 0)
                            {
                                //dibujo Rubro Total Equivalente
                                lb = new Label();
                                lb.Text = dr[4].ToString();
                                //e.Graphics.DrawString(lb.Text, Fuente, Brushes.Black, xrubroequivalente_ini, yrubroequivalente_ini);

                                espacios = "";
                                espacios = DB.Rellenar_Espacios("", decimal.Parse(dr[4].ToString()).ToString("#,#.00#;(#,#.00#)"), 0);
                                monto_auxiliar = "";
                                monto_auxiliar = espacios + decimal.Parse(dr[4].ToString()).ToString("#,#.00#;(#,#.00#)");
                                monto_auxiliar = DB.Invertir(monto_auxiliar);
                                char[] Monto_Equivalente = monto_auxiliar.ToCharArray();
                                x = xrubroequivalente_erial_ini;
                                y = yrubroequivalente_erial_ini;
                                foreach (char caracter in Monto_Equivalente)
                                {
                                    Fuente = new Font(font_interlineado.strC2, font_interlineado.intC7);
                                    e.Graphics.DrawString(caracter.ToString(), Fuente, Brushes.Black, x, y);
                                    x = x - 9;
                                }
                                if (dr[8].ToString() == "")//No Hay Comentario
                                {
                                    yrubroequivalente_erial_ini += interlineado;
                                }
                                else// Hay Comentario
                                {
                                    yrubroequivalente_erial_ini += interlineado;
                                    yrubroequivalente_erial_ini += interlineado;
                                }
                                if (font_interlineado.strC2 != null && !font_interlineado.strC2.Equals("")) lb.Font.Name = font_interlineado.strC2;
                                if (font_interlineado.intC7 > 0) lb.Font.Size = font_interlineado.intC7;
                            }
                            if (xrubromonedaeq_erial_ini > 0 && yrubromonedaeq_erial_ini > 0)
                            {
                                //Dibujo la Moneda Equivalente
                                int paiID = user.PaisID;
                                if ((user.PaisID == 11) || (user.PaisID == 12) || (user.PaisID == 13))
                                {
                                    paiID = 4;
                                }
                                string mnd_eq = "";
                                if (datosfactura.intC4 == paiID)
                                {
                                    mnd_eq = "USD";
                                }
                                else
                                {
                                    mnd_eq = Utility.TraducirMonedaInt(user.PaisID);
                                }
                                lb = new Label();
                                lb.Text = mnd_eq;
                                e.Graphics.DrawString(lb.Text, Fuente, Brushes.Black, xrubromonedaeq_erial_ini, yrubromonedaeq_erial_ini);
                                if (dr[8].ToString() == "")//No Hay Comentario
                                {
                                    yrubromonedaeq_erial_ini += interlineado;
                                }
                                else// Hay Comentario
                                {
                                    yrubromonedaeq_erial_ini += interlineado;
                                    yrubromonedaeq_erial_ini += interlineado;
                                }
                                if (font_interlineado.strC2 != null && !font_interlineado.strC2.Equals("")) lb.Font.Name = font_interlineado.strC2;
                                if (font_interlineado.intC7 > 0) lb.Font.Size = font_interlineado.intC7;
                            }
                            #endregion
                        }
                    }
                    else
                    {
                        // dibujo el nombre del rubro
                        lb = new Label();
                        lb.Text = datosfactura.strC30;
                        e.Graphics.DrawString(lb.Text, Fuente, Brushes.Black, xnombre_ini, ynombre_ini);
                        if (font_interlineado.strC2 != null && !font_interlineado.strC2.Equals("")) lb.Font.Name = font_interlineado.strC2;
                        if (font_interlineado.intC7 > 0) lb.Font.Size = font_interlineado.intC7;
                        ynombre_ini += interlineado;
                        //dibujo el Total
                        lb = new Label();
                        lb.Text = font_interlineado.strC3 + datosfactura.decC3.ToString("#,#.00");
                        e.Graphics.DrawString(lb.Text, Fuente, Brushes.Black, xtot_ini, ytot_ini);
                        if (font_interlineado.strC2 != null && !font_interlineado.strC2.Equals("")) lb.Font.Name = font_interlineado.strC2;
                        if (font_interlineado.intC7 > 0) lb.Font.Size = font_interlineado.intC7;
                        ytot_ini += interlineado;
                    }
                    // dibujo los anticipos si es que tiene
                    if (user.PaisID == 100)
                    {
                        if (abonofactura > 0)
                        {
                            lb = new Label();
                            lb.Text = "Total de recibos de anticipo: " + abonofactura.ToString("#,#.00");
                            e.Graphics.DrawString(lb.Text, Fuente, Brushes.Black, xtot_ini, ytot_ini);
                            if (font_interlineado.strC2 != null && !font_interlineado.strC2.Equals("")) lb.Font.Name = font_interlineado.strC2;
                            if (font_interlineado.intC7 > 0) lb.Font.Size = font_interlineado.intC7;
                            ytot_ini += interlineado;
                        }
                    }
                }
                else
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        #region Facturacion SV
                        if (font_interlineado.intC11 == 2)//Facturacion Consumidor Final
                        {
                            #region Dibujo ID_RUBRO
                            if (xrubro_id_ini > 0 && yrubro_id_ini > 0)
                            {
                                lb = new Label();
                                lb.Text = dr[0].ToString();
                                e.Graphics.DrawString(lb.Text, Fuente, Brushes.Black, xrubro_id_ini, yrubro_id_ini);
                                if (dr[8].ToString() == "")//No Hay Comentario
                                {
                                    yrubro_id_ini += interlineado;
                                }
                                else// Hay Comentario
                                {
                                    yrubro_id_ini += interlineado;
                                    yrubro_id_ini += interlineado;
                                }
                                if (font_interlineado.strC2 != null && !font_interlineado.strC2.Equals("")) lb.Font.Name = font_interlineado.strC2;
                                if (font_interlineado.intC7 > 0) lb.Font.Size = font_interlineado.intC7;
                            }
                            #endregion
                            #region Dibujo RUBRO_NOMBRE
                            if (xnombre_ini > 0 && ynombre_ini > 0)
                            {
                                // dibujo el nombre del rubro
                                lb = new Label();
                                lb_comentario = new Label();
                                if (rgb_cliente != null)
                                {
                                    if (rgb_cliente.intC2 == 0)
                                    {
                                        lb.Text = dr[1].ToString();
                                        lb_comentario.Text = dr[8].ToString();
                                    }
                                    else
                                    {
                                        lb.Text = DB.getAliasRubro(datosfactura.intC8, long.Parse(dr[0].ToString()), datosfactura.intC3, dr[1].ToString());
                                        lb_comentario.Text = dr[8].ToString();
                                    }
                                }
                                else
                                {
                                    lb.Text = dr[1].ToString();
                                    lb_comentario.Text = dr[8].ToString();
                                }
                                e.Graphics.DrawString(lb.Text, Fuente, Brushes.Black, xnombre_ini, ynombre_ini);
                                if (lb_comentario.Text == "")//No Hay Comentario
                                {
                                    ynombre_ini += interlineado;
                                }
                                else// Hay Comentario
                                {
                                    ynombre_ini += interlineado;
                                    e.Graphics.DrawString(lb_comentario.Text, Fuente, Brushes.Black, xnombre_ini, ynombre_ini);
                                    ynombre_ini += interlineado;
                                }
                                if (font_interlineado.strC2 != null && !font_interlineado.strC2.Equals("")) lb.Font.Name = font_interlineado.strC2;
                                if (font_interlineado.strC2 != null && !font_interlineado.strC2.Equals("")) lb_comentario.Font.Name = font_interlineado.strC2;
                                if (font_interlineado.intC7 > 0) lb.Font.Size = font_interlineado.intC7;
                                if (font_interlineado.intC7 > 0) lb_comentario.Font.Size = font_interlineado.intC7;
                            }
                            #endregion
                            Rubros rubro = new Rubros();
                            rubro.rubroID = long.Parse(dr[0].ToString());
                            Rubros Rubro_Detalle = (Rubros)rubro;
                            Rubro_Detalle = (Rubros)DB.ExistRubroPais(rubro, user.PaisID);
                            if (Rubro_Detalle.NoSujeto == 1)//No Sujeto
                            {
                                #region Ventas No Sujetas
                                //Monto Sin IVA
                                lb = new Label();
                                texto = decimal.Parse(dr[5].ToString()).ToString("#,#.00");//tdf_montosinimpuesto
                                lb.Text = font_interlineado.strC3 + texto.ToString();
                                e.Graphics.DrawString(lb.Text, Fuente, Brushes.Black, x_ventas_no_sujetas_ini, y_ventas_no_sujetas_ini);
                                if (dr[8].ToString() == "")//No Hay Comentario
                                {
                                    y_ventas_no_sujetas_ini += interlineado;
                                    y_ventas_afectas_ini += interlineado;
                                }
                                else// Hay Comentario
                                {
                                    y_ventas_no_sujetas_ini += interlineado;
                                    y_ventas_no_sujetas_ini += interlineado;
                                    y_ventas_afectas_ini += interlineado;
                                    y_ventas_afectas_ini += interlineado;
                                }
                                if (font_interlineado.strC2 != null && !font_interlineado.strC2.Equals("")) lb.Font.Name = font_interlineado.strC2;
                                if (font_interlineado.intC7 > 0) lb.Font.Size = font_interlineado.intC7;
                                #endregion
                            }
                            if (Rubro_Detalle.CobIva == 1)//No Excento
                            {
                                #region Ventas Afectas
                                //Monto Sin IVA
                                lb = new Label();
                                texto = decimal.Parse(dr[7].ToString()).ToString("#,#.00");//tdf_monto
                                lb.Text = font_interlineado.strC3 + texto.ToString();
                                e.Graphics.DrawString(lb.Text, Fuente, Brushes.Black, x_ventas_afectas_ini, y_ventas_afectas_ini);
                                if (dr[8].ToString() == "")//No Hay Comentario
                                {
                                    y_ventas_afectas_ini += interlineado;
                                    y_ventas_no_sujetas_ini += interlineado;
                                }
                                else// Hay Comentario
                                {
                                    y_ventas_afectas_ini += interlineado;
                                    y_ventas_afectas_ini += interlineado;
                                    y_ventas_no_sujetas_ini += interlineado;
                                    y_ventas_no_sujetas_ini += interlineado;
                                }
                                if (font_interlineado.strC2 != null && !font_interlineado.strC2.Equals("")) lb.Font.Name = font_interlineado.strC2;
                                if (font_interlineado.intC7 > 0) lb.Font.Size = font_interlineado.intC7;
                                #endregion
                            }
                        }
                        else if (font_interlineado.intC11 == 3)//Facturacion Credito Fiscal
                        {
                            #region Dibujo ID_RUBRO
                            if (xrubro_id_ini > 0 && yrubro_id_ini > 0)
                            {
                                lb = new Label();
                                lb.Text = dr[0].ToString();
                                e.Graphics.DrawString(lb.Text, Fuente, Brushes.Black, xrubro_id_ini, yrubro_id_ini);
                                if (dr[8].ToString() == "")//No Hay Comentario
                                {
                                    yrubro_id_ini += interlineado;
                                }
                                else// Hay Comentario
                                {
                                    yrubro_id_ini += interlineado;
                                    yrubro_id_ini += interlineado;
                                }
                                if (font_interlineado.strC2 != null && !font_interlineado.strC2.Equals("")) lb.Font.Name = font_interlineado.strC2;
                                if (font_interlineado.intC7 > 0) lb.Font.Size = font_interlineado.intC7;
                            }
                            #endregion
                            #region Dibujo RUBRO_NOMBRE
                            if (xnombre_ini > 0 && ynombre_ini > 0)
                            {
                                // dibujo el nombre del rubro
                                lb = new Label();
                                lb_comentario = new Label();
                                if (rgb_cliente != null)
                                {
                                    if (rgb_cliente.intC2 == 0)
                                    {
                                        lb.Text = dr[1].ToString();
                                        lb_comentario.Text = dr[8].ToString();
                                    }
                                    else
                                    {
                                        lb.Text = DB.getAliasRubro(datosfactura.intC8, long.Parse(dr[0].ToString()), datosfactura.intC3, dr[1].ToString());
                                        lb_comentario.Text = dr[8].ToString();
                                    }
                                }
                                else
                                {
                                    lb.Text = dr[1].ToString();
                                    lb_comentario.Text = dr[8].ToString();
                                }
                                e.Graphics.DrawString(lb.Text, Fuente, Brushes.Black, xnombre_ini, ynombre_ini);
                                if (lb_comentario.Text == "")//No Hay Comentario
                                {
                                    ynombre_ini += interlineado;
                                }
                                else// Hay Comentario
                                {
                                    ynombre_ini += interlineado;
                                    e.Graphics.DrawString(lb_comentario.Text, Fuente, Brushes.Black, xnombre_ini, ynombre_ini);
                                    ynombre_ini += interlineado;
                                }
                                if (font_interlineado.strC2 != null && !font_interlineado.strC2.Equals("")) lb.Font.Name = font_interlineado.strC2;
                                if (font_interlineado.strC2 != null && !font_interlineado.strC2.Equals("")) lb_comentario.Font.Name = font_interlineado.strC2;
                                if (font_interlineado.intC7 > 0) lb.Font.Size = font_interlineado.intC7;
                                if (font_interlineado.intC7 > 0) lb_comentario.Font.Size = font_interlineado.intC7;
                            }
                            #endregion
                            Rubros rubro = new Rubros();
                            rubro.rubroID = long.Parse(dr[0].ToString());
                            Rubros Rubro_Detalle = (Rubros)rubro;
                            Rubro_Detalle = (Rubros)DB.ExistRubroPais(rubro, user.PaisID);
                            if (rgb_cliente.intC1 == 1)//Excento
                            {
                                #region Ventas Exentas
                                //Monto Sin IVA
                                lb = new Label();
                                texto = decimal.Parse(dr[5].ToString()).ToString("#,#.00");
                                lb.Text = font_interlineado.strC3 + texto.ToString();
                                e.Graphics.DrawString(lb.Text, Fuente, Brushes.Black, x_ventas_exentas_ini, y_ventas_exentas_ini);
                                if (dr[8].ToString() == "")//No Hay Comentario
                                {
                                    y_ventas_exentas_ini += interlineado;
                                }
                                else// Hay Comentario
                                {
                                    y_ventas_exentas_ini += interlineado;
                                    y_ventas_exentas_ini += interlineado;
                                }
                                if (font_interlineado.strC2 != null && !font_interlineado.strC2.Equals("")) lb.Font.Name = font_interlineado.strC2;
                                if (font_interlineado.intC7 > 0) lb.Font.Size = font_interlineado.intC7;
                                #endregion
                            }
                            else//Contribuyente
                            {
                                if (Rubro_Detalle.CobIva == 0)//Excento
                                {
                                    #region Ventas No Sujetas
                                    //Monto Sin IVA
                                    lb = new Label();
                                    texto = decimal.Parse(dr[5].ToString()).ToString("#,#.00");
                                    lb.Text = font_interlineado.strC3 + texto.ToString();
                                    e.Graphics.DrawString(lb.Text, Fuente, Brushes.Black, x_ventas_no_sujetas_ini, y_ventas_no_sujetas_ini);
                                    if (dr[8].ToString() == "")//No Hay Comentario
                                    {
                                        y_ventas_no_sujetas_ini += interlineado;
                                        y_ventas_afectas_ini += interlineado;
                                    }
                                    else// Hay Comentario
                                    {
                                        y_ventas_no_sujetas_ini += interlineado;
                                        y_ventas_no_sujetas_ini += interlineado;
                                        y_ventas_afectas_ini += interlineado;
                                        y_ventas_afectas_ini += interlineado;
                                    }
                                    if (font_interlineado.strC2 != null && !font_interlineado.strC2.Equals("")) lb.Font.Name = font_interlineado.strC2;
                                    if (font_interlineado.intC7 > 0) lb.Font.Size = font_interlineado.intC7;
                                    #endregion
                                }
                                else if (Rubro_Detalle.CobIva == 1)//No Excento
                                {
                                    #region Ventas Afectas
                                    //Monto Sin IVA
                                    lb = new Label();
                                    texto = decimal.Parse(dr[5].ToString()).ToString("#,#.00");
                                    lb.Text = font_interlineado.strC3 + texto.ToString();
                                    e.Graphics.DrawString(lb.Text, Fuente, Brushes.Black, x_ventas_afectas_ini, y_ventas_afectas_ini);
                                    if (dr[8].ToString() == "")//No Hay Comentario
                                    {
                                        y_ventas_afectas_ini += interlineado;
                                        y_ventas_no_sujetas_ini += interlineado;
                                    }
                                    else// Hay Comentario
                                    {
                                        y_ventas_afectas_ini += interlineado;
                                        y_ventas_afectas_ini += interlineado;
                                        y_ventas_no_sujetas_ini += interlineado;
                                        y_ventas_no_sujetas_ini += interlineado;
                                    }
                                    if (font_interlineado.strC2 != null && !font_interlineado.strC2.Equals("")) lb.Font.Name = font_interlineado.strC2;
                                    if (font_interlineado.intC7 > 0) lb.Font.Size = font_interlineado.intC7;
                                    #endregion
                                }
                            }
                        }
                        else if (font_interlineado.intC11 == 4)//Facturacion Exportacion
                        {
                            #region Dibujo ID_RUBRO
                            if (xrubro_id_ini > 0 && yrubro_id_ini > 0)
                            {
                                lb = new Label();
                                lb.Text = dr[0].ToString();
                                e.Graphics.DrawString(lb.Text, Fuente, Brushes.Black, xrubro_id_ini, yrubro_id_ini);
                                if (dr[8].ToString() == "")//No Hay Comentario
                                {
                                    yrubro_id_ini += interlineado;
                                }
                                else// Hay Comentario
                                {
                                    yrubro_id_ini += interlineado;
                                    yrubro_id_ini += interlineado;
                                }
                                if (font_interlineado.strC2 != null && !font_interlineado.strC2.Equals("")) lb.Font.Name = font_interlineado.strC2;
                                if (font_interlineado.intC7 > 0) lb.Font.Size = font_interlineado.intC7;
                            }
                            #endregion
                            #region Dibujo RUBRO_NOMBRE
                            if (xnombre_ini > 0 && ynombre_ini > 0)
                            {
                                // dibujo el nombre del rubro
                                lb = new Label();
                                lb_comentario = new Label();
                                if (rgb_cliente != null)
                                {
                                    if (rgb_cliente.intC2 == 0)
                                    {
                                        lb.Text = dr[1].ToString();
                                        lb_comentario.Text = dr[8].ToString();
                                    }
                                    else
                                    {
                                        lb.Text = DB.getAliasRubro(datosfactura.intC8, long.Parse(dr[0].ToString()), datosfactura.intC3, dr[1].ToString());
                                        lb_comentario.Text = dr[8].ToString();
                                    }
                                }
                                else
                                {
                                    lb.Text = dr[1].ToString();
                                    lb_comentario.Text = dr[8].ToString();
                                }
                                e.Graphics.DrawString(lb.Text, Fuente, Brushes.Black, xnombre_ini, ynombre_ini);
                                if (lb_comentario.Text == "")//No Hay Comentario
                                {
                                    ynombre_ini += interlineado;
                                }
                                else// Hay Comentario
                                {
                                    ynombre_ini += interlineado;
                                    e.Graphics.DrawString(lb_comentario.Text, Fuente, Brushes.Black, xnombre_ini, ynombre_ini);
                                    ynombre_ini += interlineado;
                                }
                                if (font_interlineado.strC2 != null && !font_interlineado.strC2.Equals("")) lb.Font.Name = font_interlineado.strC2;
                                if (font_interlineado.strC2 != null && !font_interlineado.strC2.Equals("")) lb_comentario.Font.Name = font_interlineado.strC2;
                                if (font_interlineado.intC7 > 0) lb.Font.Size = font_interlineado.intC7;
                                if (font_interlineado.intC7 > 0) lb_comentario.Font.Size = font_interlineado.intC7;
                            }
                            #endregion
                            Rubros rubro = new Rubros();
                            rubro.rubroID = long.Parse(dr[0].ToString());
                            Rubros Rubro_Detalle = (Rubros)rubro;
                            Rubro_Detalle = (Rubros)DB.ExistRubroPais(rubro, user.PaisID);
                            //if (Rubro_Detalle.CobIva == 1)//No Excento
                            //{
                            #region Ventas Afectas
                            //Monto Sin IVA
                            lb = new Label();
                            texto = decimal.Parse(dr[5].ToString()).ToString("#,#.00");
                            lb.Text = font_interlineado.strC3 + texto.ToString();
                            e.Graphics.DrawString(lb.Text, Fuente, Brushes.Black, x_ventas_afectas_ini, y_ventas_afectas_ini);
                            if (dr[8].ToString() == "")//No Hay Comentario
                            {
                                y_ventas_afectas_ini += interlineado;
                            }
                            else// Hay Comentario
                            {
                                y_ventas_afectas_ini += interlineado;
                                y_ventas_afectas_ini += interlineado;
                            }
                            if (font_interlineado.strC2 != null && !font_interlineado.strC2.Equals("")) lb.Font.Name = font_interlineado.strC2;
                            if (font_interlineado.intC7 > 0) lb.Font.Size = font_interlineado.intC7;
                            #endregion
                        }
                        //}
                        #endregion
                    }
                }
            }
        }
        catch (Exception ex)
        {
            log4net ErrLog = new log4net();
            ErrLog.ErrorLog(ex.Message);
        }
    }
    #endregion
    protected string Segmentar_Cadena_Con_Retorno(string Cadena, int Tipo)
    {
        int longitud = Cadena.Length;
        int longitud2 = 0;
        int indice = 0;
        int indice1 = 0;
        string Sub_Cadena = "";
        string Sub_Cadena1 = "";
        string Sub_Segmento = "";
        string Sub_Segmento1 = "";
        string Complemento = "";
        string complemento2 = "";
        if (Tipo == 1)//Factura
        {
            if (longitud > 40)
            {
                Sub_Cadena = Cadena.Substring(0, 40);
                indice = Sub_Cadena.LastIndexOf(" ");
                Sub_Cadena = Sub_Cadena.Substring(0, indice);
                Sub_Segmento = Cadena.Substring(indice, 40 - indice);
                Complemento = Cadena.Substring(40, longitud - 40);
                longitud2 = Complemento.Length;
                if (longitud2 > 40)
                {
                    //---nuevo---//
                    Sub_Cadena1 = Complemento.Substring(0, 40);
                    indice1 = Sub_Cadena1.LastIndexOf(" ");
                    Sub_Cadena1 = Sub_Cadena1.Substring(0, indice1);
                    Sub_Segmento1 = Complemento.Substring(indice1, 40 - indice1);
                    complemento2 = Complemento.Substring(40, longitud2 - 40);
                    Cadena = Sub_Cadena + Retorno_De_Carro + Sub_Segmento + Sub_Cadena1 + Retorno_De_Carro + Sub_Segmento1 + complemento2;
                }
                else
                {
                    Cadena = Sub_Cadena + Retorno_De_Carro + Sub_Segmento + Complemento;
                }
                // Cadena = Sub_Cadena + Retorno_De_Carro + Sub_Segmento + Complemento + Retorno_De_Carro + complemento2;

            }
        }
        else if ((Tipo == 3) || (Tipo == 18))//NC
        {
            if (longitud > 30)
            {
                Sub_Cadena = Cadena.Substring(0, 30);
                indice = Sub_Cadena.LastIndexOf(" ");
                Sub_Cadena = Sub_Cadena.Substring(0, indice);
                Sub_Segmento = Cadena.Substring(indice, 30 - indice);
                Complemento = Cadena.Substring(30, longitud - 30);
                Cadena = Sub_Cadena + Retorno_De_Carro + Sub_Segmento + Complemento;
            }
        }
        else if ((Tipo == 2))//RC
        {
            if (longitud > 150)
            {
                Sub_Cadena = Cadena.Substring(0, 150);
                indice = Sub_Cadena.LastIndexOf(" ");
                Sub_Cadena = Sub_Cadena.Substring(0, indice);
                Sub_Segmento = Cadena.Substring(indice, 150 - indice);
                Complemento = Cadena.Substring(150, longitud - 150);
                Cadena = Sub_Cadena + Retorno_De_Carro + Sub_Segmento + Complemento;
            }
        }
        return Cadena;
    }
    protected string Segmentar_Cadena_generica(string Cadena, int longitud_total)
    {
        int longitud = Cadena.Length;
        int indice = 0;
        string Sub_Cadena = "";
        string Sub_Segmento = "";
        string Complemento = "";
        if (longitud > longitud_total)
        {
            Sub_Cadena = Cadena.Substring(0, longitud_total);
            indice = Sub_Cadena.LastIndexOf(" ");
            Sub_Cadena = Sub_Cadena.Substring(0, indice);
            Sub_Segmento = Cadena.Substring(indice, longitud_total - indice);
            Complemento = Cadena.Substring(longitud_total, longitud - longitud_total);
            Cadena = Sub_Cadena + Retorno_De_Carro + Sub_Segmento + Complemento;
        }
        return Cadena;
    }
}