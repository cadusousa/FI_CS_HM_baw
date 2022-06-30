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

public partial class invoice_print_invoice : System.Web.UI.Page
{
    UsuarioBean user;
    int fac_id = 0, tipo = 1;
    int xnombre_ini = 0, ynombre_ini = 0;
    int xsubt_ini = 0, ysubt_ini = 0;
    int ximp_ini = 0, yimp_ini = 0;
    int xtot_ini = 0, ytot_ini = 0;
    int xcomentario_ini = 0, ycomentario_ini = 0;
    decimal subtotalnormal = 0, subtotalexcento = 0, abonofactura=0;
    string texto;
    string detalle_pagos = "";
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
        bool IsContado = false;
        string serieDoc = "";
        if (Request.QueryString["fac_id"] != null)
        {
            fac_id = int.Parse(Request.QueryString["fac_id"].ToString());
            tipo = int.Parse(Request.QueryString["tipo"].ToString());
        }
        user = (UsuarioBean)Session["usuario"];
        if (tipo == 1)
        { //factura
            datosfactura = (RE_GenericBean)DB.getFacturaData(fac_id);
            serieDoc = datosfactura.strC28;//Factura
            rgb_cliente = (RE_GenericBean)DB.getDataClient((int)datosfactura.intC3);
            dt = (DataTable)DB.getRubbyFact(fac_id, tipo);
            abonofactura = (decimal)DB.getTotalAbonadoXFact(fac_id, 2);//indica que la referencia es de recibo
            IsContado = DB.EsClienteCredito(datosfactura.intC3, user.PaisID);
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
            IsContado = DB.EsClienteCredito(datosfactura.intC3, user.PaisID);
        }
        else if (tipo == 3)
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
            IsContado = DB.EsClienteCredito(datosfactura.intC3, user.PaisID);
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
            //ArrayList camposfact = (ArrayList)DB.getCamposDoc(tipo, datosfactura.intC2, datosfactura.strC28);//parametros 1=factura (tipo doc); 2=sucursal de la factura; 3=serie de factura
            ArrayList camposfact = (ArrayList)DB.getCamposDoc(tipo, datosfactura.intC2, serieDoc);//parametros 1=factura (tipo doc); 2=sucursal de la factura; 3=serie de factura
            Label lb1 = null;
            foreach (RE_GenericBean rgb in camposfact)
            {
                // dibujo el campo
                lb1 = new Label();
                lb1.Text = "";
                if (rgb.strC1.Trim().Equals("FECHA EMISION")) lb1.Text = rgb.strC2.Trim() + " " + datosfactura.strC5;
                if (rgb.strC1.Trim().Equals("FECHA (D)")) lb1.Text = rgb.strC2.Trim() + " " + datosfactura.strC5.Substring(0, 2);
                if (rgb.strC1.Trim().Equals("FECHA (M)")) lb1.Text = rgb.strC2.Trim() + " " + datosfactura.strC5.Substring(3, 2);

                if (rgb.strC1.Trim().Equals("FECHA (M) LETRAS"))
                    if (font_interlineado.intC5 == 8)
                        lb1.Text = rgb.strC2.Trim() + " " + Utility.MesfromIntToStr_english(int.Parse(datosfactura.strC5.Substring(3, 2)));
                    else
                        lb1.Text = rgb.strC2.Trim() + " " + Utility.MesfromIntToStr(int.Parse(datosfactura.strC5.Substring(3, 2)));

                if (rgb.strC1.Trim().Equals("FECHA (A)")) lb1.Text = rgb.strC2.Trim() + " " + datosfactura.strC5.Substring(6, 4);
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
                if (rgb.strC1.Trim().Equals("MONEDA")) lb1.Text = rgb.strC2.Trim() + " " + datosfactura.intC4.ToString();
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
                //
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
                if (IsContado)
                {
                    if (rgb.strC1.Trim().Equals("MARCA CONTADO")) lb1.Text = rgb.strC2.Trim() + " X";
                }
                else
                {
                    if (rgb.strC1.Trim().Equals("MARCA CREDITO")) lb1.Text = rgb.strC2.Trim() + " X";
                }
                if ((tipo == 1 || tipo == 4) || (tipo == 14) || (tipo == 3))
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
                }
                lb1.Attributes.Add("Style", "left: " + rgb.intC1 + "px; top: " + rgb.intC2 + "px; position: absolute;");
                if (font_interlineado.strC2 != null && !font_interlineado.strC2.Equals("")) lb1.Font.Name = font_interlineado.strC2;
                if (font_interlineado.intC7 > 0) lb1.Font.Size = font_interlineado.intC7;
                Page.Controls.Add(lb1);

            }
            //************************************************************************
            if ((tipo == 1 || tipo == 4 || tipo == 14 || tipo == 3) && (dt != null && dt.Rows.Count > 0))
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
                            lb.Text = font_interlineado.strC3 + texto.ToString()+"<br>";
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
