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

public partial class invoice_template_retencion : System.Web.UI.Page
{
    //RE_GenericBean oc = null;
    //RE_GenericBean ret_prov = null;
    int cheque_ID = 0;
    int tipo = 0;
    UsuarioBean user;
    DataTable dtPolizaDiario = null;
    DataTable dtDetalleProvision = null;
    public string html_datos = "";
    public string html_cai_honduras = "";
    public string html_descripcion_honduras = "";
    public string html_pie_pagina = "";
    public string html_poliza_diario = "";
    public string html_detalle_provision = "";
    public string html_iva_subtotal = "";
    

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["usuario"] == null)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "Nombre", "<script> refresh(); </script>");
        }
        user = (UsuarioBean)Session["usuario"];


        if (user.pais.Grupo_Empresas == 2)
        {
            Image1.ImageUrl = user.pais.Imagepath;
        }
        else if (user.PaisID == 11)
        {
            Image1.ImageUrl = user.pais.Imagepath;
        }
        else if (user.PaisID == 12)
        {
            Image1.ImageUrl = user.pais.Imagepath;
        }
        else if (user.PaisID == 13)
        {
            Image1.ImageUrl = user.pais.Imagepath;
        }
        else if ((user.PaisID == 3 || user.PaisID == 23) && user.contaID == 2)
        {
            Image1.ImageUrl = "";
            Image1.Visible = false;
        }
        else
        {
            if (user.contaID == 1)
            {
                Image1.ImageUrl = "~/img/aimar.jpg";
            }
            else
            {
                Image1.ImageUrl = "~/img/aimar_en.jpg";
            }
        }
        


        
        if (user.PaisID == 3 || user.PaisID == 23)
        {
            lbl_tipo_documento.Text = "Comprobante de Retención";
            lbl_documento_asociado.Text = "R.T.N.:";
        }
        else
        {
            lbl_tipo_documento.Text = "Provision";
            lbl_documento_asociado.Text = "Correlativo";
        }

        if (user.PaisID == 3)
        {
            html_datos = "<table width='100%'>"
                + "<tr>"
                + "   <td>Agencia Internacional Maritima S.A.</td>"
                + "</tr>"
                + "<tr>"
                + "   <td>Col. Brisas de la Mesa contiguo a la Base Aerea,</br>Aeropuerto Ramón Villeda Morales</td>"
                + "</tr>"
                + "<tr>"
                + "   <td>PBX (504)2564-0099 / 2668-0121, fax (504)2668-0353</td>"
                + "</tr>"
                + "<tr>"
                + "   <td>La Lima Cortes, Honduras</td>"
                + "</tr>"
                + "<tr>"
                + "   <td>info@aimargroup.com</td>"
                + "</tr>"
                + "<tr>"
                + "    <td class='style11'>R.T.N.:05019000044051</td>"
                + "</tr>"
            + "</table>";
        }

        if (user.PaisID == 23)
        {
            html_datos = "<table width='100%'>"
                + "<tr>"
                + "   <td>Latin Freight S.A.</td>"
                + "</tr>"
                + "<tr>"
                + "   <td>Col. Brisas de la Mesa contiguo a la Base Aerea,</br>Aeropuerto Ramón Villeda Morales</td>"
                + "</tr>"
                + "<tr>"
                + "   <td>PBX (504)2564-0099 / 2668-0121, fax (504)2668-0353</td>"
                + "</tr>"
                + "<tr>"
                + "   <td>La Lima Cortes, Honduras</td>"
                + "</tr>"
                + "<tr>"
                + "   <td>operaciones.hn5@latinfreightneutral.com</td>"
                + "</tr>"
                + "<tr>"
                + "    <td class='style11'>R.T.N.:05019013575155</td>"
                + "</tr>"
            + "</table>";
        }

        if ((user.PaisID == 23) || (user.PaisID == 3))
        {
            html_pie_pagina = "<table width='100%'>"
                + "<tr>"
                + "    <td class='style5' width='50%'>Recibi conforme:</td><td class='style5' width='50%'>Firma autorizada:</td>"
                + "</tr>"
            + "</table>";
        }

        
        if (user.PaisID == 11)
        {
            //Image1.ImageUrl = "~/img/GRH.jpg";
            Image1.ImageUrl = user.pais.Imagepath;
        }
        else if (user.PaisID == 12)
        {
            //Image1.ImageUrl = "~/img/ISI.bmp";
            Image1.ImageUrl = user.pais.Imagepath;
        }
        else if (user.PaisID == 13)
        {
            //Image1.ImageUrl = "~/img/Mayan Logistics.jpg";
            Image1.ImageUrl = user.pais.Imagepath;
        }
        else
        {
            //Image1.ImageUrl = "~/img/aimar.jpg";
            Image1.ImageUrl = user.pais.Imagepath;
        }
        if (Request.QueryString["chequeID"] != null)
        {
            cheque_ID = int.Parse(Request.QueryString["chequeID"].ToString());
            tipo = int.Parse(Request.QueryString["tipo"].ToString());
            Cargar(cheque_ID, tipo);
        }



    }

    protected void Cargar(int chequeID, int tipo)
    {
        string nombre = ""; 
        string direccion = ""; 
        string telefono = "";
        string nit = ""; 
        string ruc = "";
          
        //oc = (RE_GenericBean)DB.getProvisionData_forprint(ocID);
        //ret_prov = (RE_GenericBean)DB.getRetencion_forprint(" b.trp_tpr_prov_id = "+ ocID);
        ArrayList array = new ArrayList();
        RE_GenericBean rgb_cheque = (RE_GenericBean)DB.getChequeDataforView(chequeID);
        decimal tipo_cambio = Math.Round(DB.getTipoCambioByDay(user, rgb_cheque.strC3),4);
        if (rgb_cheque.intC7 == 4)//proveedor
        {
            RE_GenericBean Proveedor_Bean = DB.getProveedorData(rgb_cheque.intC6, "");
            nombre = Proveedor_Bean.strC2;
            nit = Proveedor_Bean.strC1;
            direccion = Proveedor_Bean.strC5;
            telefono = Proveedor_Bean.strC7;
            ruc = Proveedor_Bean.strC11;
        }
        else if (rgb_cheque.intC7 == 2)//agente
        {
            RE_GenericBean Agente_Bean = DB.getAgenteData(rgb_cheque.intC6, "");
            nombre = Agente_Bean.strC1;
            direccion = Agente_Bean.strC2;
            telefono = Agente_Bean.strC3;
            nit = Agente_Bean.strC6;
            ruc = Agente_Bean.strC7;
        }
        else if (rgb_cheque.intC7 == 5)//naviera
        {
            RE_GenericBean Naviera_Bean = DB.getNavieraData(rgb_cheque.intC6);
            nombre = Naviera_Bean.strC1;
            nit = Naviera_Bean.strC2;
            ruc = Naviera_Bean.strC3;
        }
        else if (rgb_cheque.intC7 == 6)//linea aerea
        {
            RE_GenericBean Carrier_Bean = DB.getCarriersData(rgb_cheque.intC6);
            nombre = Carrier_Bean.strC1;
            nit = Carrier_Bean.strC2;
            ruc = Carrier_Bean.strC3;
        }
        else if (rgb_cheque.intC7 == 8)//Caja Chica
        {
            RE_GenericBean CajaChica_Bean = (RE_GenericBean)DB.getUsuarioEmpresabyID(rgb_cheque.intC6);
            nombre = CajaChica_Bean.strC1;
            nit = CajaChica_Bean.strC2;
        }

        ArrayList Provisiones = (ArrayList)DB.getProvisionesPagadasPorCheque(chequeID);
        decimal montoRetenidoTotal = 0;
        decimal montoRetenidoTotalUsd = 0;
        decimal montoRetenido1 = 0;
        decimal montoRetenido1_usd = 0;
        decimal montoRetenido12 = 0;
        decimal montoRetenido12_usd = 0;
        decimal montoRetenido25 = 0;
        decimal montoRetenido25_usd = 0;
        decimal totalFactura = 0;
        decimal totalFactura_usd = 0;
        decimal valorSujeto = 0;
        decimal valorSujeto_usd = 0;
        decimal inpuesto_venta = 0;
        int tipoRetencion = 0;
        string facturas_proveedor = "";
        foreach (RE_GenericBean rgb in Provisiones)
        {
            int bandera = 0;
            string facturas_retencion = "";
            decimal totalFacturaRetencion = 0;
            decimal inpuesto_venta_retencion = 0;
            ArrayList Retenciones = (ArrayList)DB.getRetencionesDeProvision(chequeID, rgb.intC1);
            foreach (RE_GenericBean rgb_retencion in Retenciones)
            {
                tipoRetencion = rgb_retencion.intC2;
                if (tipoRetencion == 25 || tipoRetencion == 114)
                {
                    montoRetenido1 += rgb_retencion.decC1;
                }
                if (tipoRetencion == 50 || tipoRetencion == 115)
                {
                    montoRetenido12 += rgb_retencion.decC1;
                }
                if (tipoRetencion == 117 || tipoRetencion == 118)
                {
                    montoRetenido25 += rgb_retencion.decC1;
                }
                totalFacturaRetencion  = rgb_retencion.decC2;
                inpuesto_venta_retencion = rgb_retencion.decC5;
                facturas_retencion = " "+ rgb_retencion.strC3 + "-" + rgb_retencion.strC4;
            }
            totalFactura += totalFacturaRetencion;
            inpuesto_venta += inpuesto_venta_retencion;
            facturas_proveedor += facturas_retencion;
        }
        valorSujeto = totalFactura - inpuesto_venta;

        if (rgb_cheque.intC4 == 8)
        {
            montoRetenidoTotal = montoRetenido1 + montoRetenido12 + montoRetenido25;
            montoRetenidoTotalUsd = montoRetenidoTotal * tipo_cambio;
        }
        else
        {
            montoRetenidoTotal = montoRetenido1 + montoRetenido12 + montoRetenido25;
            montoRetenidoTotalUsd = montoRetenidoTotal / tipo_cambio;
        }

        //////////////////////////////////////PARAMETROS///////////////////////////////////////////
        DB.parametros Params = DB.EmpresaParametros(user.PaisID, "", "", "** TITULO **", ""); //pais, sistema, doc_id, titulo, edicion
        Image1.ImageUrl = "data:image;base64," + System.Convert.ToBase64String(Params.logo);
        Image1.Height = 63;
        Image1.Width = 237;
        html_datos = Params.direccion2;


        RE_GenericBean dat1 = DB.GetCorrelativoRetencion(chequeID, user, 6);

        int correlativo_doc = dat1.intC1;

        string strcorrelativo_doc = "-" + correlativo_doc.ToString("00000000.##");
        /*string strcorrelativo_doc = DB.GetCorrelativoRetencion(chequeID, user);
        string[] arrRetencion = strcorrelativo_doc.Split('-');
        string puntoEmision = arrRetencion[1];*/

        //lbl_serie.Text = "";//Numero retencion
        lbl_serie.Text = dat1.strC1.Trim();


        if (1 == 1) //no habilitado temporalmente
        {
            ///////////////////////////////////////////////////////////
            #region 2021-01-11 getFacturabySerie

            if ((user.PaisID == 3 || user.PaisID == 23) && (user.contaID == 1) &&  
				(user.SucursalID == 102 || user.SucursalID == 104))  
            {

                lbl_rango_inicial.Text = "";
                lbl_rango_final.Text = "";
                lbl_fecha_limite_emision.Text = " - ";
                html_cai_honduras = "CAI.:  - no se encontro numero -";

                RE_GenericBean dat2 = DB.getFacturabySerie(lbl_serie.Text, -1, user.SucursalID);

                if (dat2.strC5 != null)
                    if (dat2.strC5 != "")
                    if (int.Parse(dat2.strC5) > 0)
                    {
                        lbl_rango_inicial.Text = lbl_serie.Text + "-" + int.Parse(dat2.strC5).ToString("00000000.##");
                        lbl_rango_final.Text = lbl_serie.Text + "-" + int.Parse(dat2.strC6).ToString("00000000.##");
                    }

                if (dat2.strC4 != null)
                    if (dat2.strC4 != "")
                    {
                        lbl_fecha_limite_emision.Text = dat2.strC4;
                    }

                if (dat2.strC7 != null)
                    if (dat2.strC7 != "")
                    {
                        html_cai_honduras = "CAI.: " + dat2.strC7;
                    }
            }
            #endregion getFacturabySerie
            ///////////////////////////////////////////////////////////
        }


        lbl_serie.Text += "-" + correlativo_doc.ToString("00000000.##");

        if (rgb_cheque.intC4 == 8)
        {
            #region convertir a moneda local
            lbl_fecha_emision.Text = rgb_cheque.strC3;//Fecha de Emision
            lbl_proveedor.Text = nombre;//Proveedor
            lbl_direccion.Text = direccion;//Direccion
            lbl_numero_cotizacion.Text = nit;//Numero de Cotizacion
            /*lbl_moneda.Text = Utility.TraducirMonedaInt(oc.intC4);*/
            lbl_total_factura.Text = totalFactura.ToString("#,#.00");
            totalFactura_usd = totalFactura * tipo_cambio;
            lbl_total_factura_usd.Text = totalFactura_usd.ToString("#,#.00");

            lbl_valor_sujeto.Text = valorSujeto.ToString("#,#.00");
            valorSujeto_usd = valorSujeto * tipo_cambio;
            lbl_valor_sujeto_usd.Text = valorSujeto_usd.ToString("#,#.00");

            if (Provisiones != null)
            {
                lbl_retenido_1_hn.Text = montoRetenido1.ToString("#,#.00");
                montoRetenido1_usd = montoRetenido1 * tipo_cambio;
                lbl_retenido_1_usd.Text = montoRetenido1_usd.ToString("#,#.00");

                lbl_retenido_12_hn.Text = montoRetenido12.ToString("#,#.00");
                montoRetenido12_usd = montoRetenido12 * tipo_cambio;
                lbl_retenido_12_usd.Text = montoRetenido12_usd.ToString("#,#.00");

                lbl_retenido_25_hn.Text = montoRetenido25.ToString("#,#.00");
                montoRetenido25_usd = montoRetenido25 * tipo_cambio;
                lbl_retenido_25_usd.Text = montoRetenido25_usd.ToString("#,#.00");
            }
            else
            {
                lbl_retenido_1_hn.Text = "0.00";
                lbl_retenido_1_usd.Text = "0.00";

                lbl_retenido_12_hn.Text = "0.00";
                lbl_retenido_12_usd.Text = "0.00";

                lbl_retenido_25_hn.Text = "0.00";
                lbl_retenido_25_usd.Text = "0.00";
            }

            decimal total_neto = rgb_cheque.decC1;
            decimal togal_neto_usd = total_neto * tipo_cambio;

            lbl_neto_recibir_hn.Text = "<b>$</b> " + total_neto.ToString("#,#.00");
            lbl_neto_recibir_usd.Text = "<b>L</b> " + togal_neto_usd.ToString("#,#.00");
            #endregion
        }
        else
        {
            #region convertir a dolares
            lbl_fecha_emision.Text = rgb_cheque.strC3;//Fecha de Emision
            lbl_proveedor.Text = nombre;//Proveedor
            lbl_direccion.Text = direccion;//Direccion
            lbl_numero_cotizacion.Text = nit;//Numero de Cotizacion
            /*lbl_moneda.Text = Utility.TraducirMonedaInt(oc.intC4);*/
            lbl_total_factura.Text = totalFactura.ToString("#,#.00");
            totalFactura_usd = totalFactura / tipo_cambio;
            lbl_total_factura_usd.Text = totalFactura_usd.ToString("#,#.00");

            lbl_valor_sujeto.Text = valorSujeto.ToString("#,#.00");
            valorSujeto_usd = valorSujeto / tipo_cambio;
            lbl_valor_sujeto_usd.Text = valorSujeto_usd.ToString("#,#.00");

            if (Provisiones != null)
            {
                lbl_retenido_1_hn.Text = montoRetenido1.ToString("#,#.00");
                montoRetenido1_usd = montoRetenido1 / tipo_cambio;
                lbl_retenido_1_usd.Text = montoRetenido1_usd.ToString("#,#.00");

                lbl_retenido_12_hn.Text = montoRetenido12.ToString("#,#.00");
                montoRetenido12_usd = montoRetenido12 / tipo_cambio;
                lbl_retenido_12_usd.Text = montoRetenido12_usd.ToString("#,#.00");

                lbl_retenido_25_hn.Text = montoRetenido25.ToString("#,#.00");
                montoRetenido25_usd = montoRetenido25 / tipo_cambio;
                lbl_retenido_25_usd.Text = montoRetenido25_usd.ToString("#,#.00");
            }
            else
            {
                lbl_retenido_1_hn.Text = "0.00";
                lbl_retenido_1_usd.Text = "0.00";

                lbl_retenido_12_hn.Text = "0.00";
                lbl_retenido_12_usd.Text = "0.00";

                lbl_retenido_25_hn.Text = "0.00";
                lbl_retenido_25_usd.Text = "0.00";
            }

            decimal total_neto = rgb_cheque.decC1;
            decimal togal_neto_usd = total_neto / tipo_cambio;

            lbl_neto_recibir_hn.Text = "<b>L</b> " + total_neto.ToString("#,#.00");
            lbl_neto_recibir_usd.Text = "<b>$</b> " + togal_neto_usd.ToString("#,#.00");
            #endregion
        }
        

        Conv c = new Conv();
          
            
        lbl_total_letras.Text = c.enletras(montoRetenidoTotal.ToString(), 1);
        lbl_total_letras_usd.Text = c.enletras(montoRetenidoTotalUsd.ToString(), 1);

        if (user.PaisID == 3 || user.PaisID == 23)
        {
            html_descripcion_honduras = "<table>"
                + "<tr>"
                    + "<td><b>Descripción:</b></td>"
                    + "<td>Constancia de Retención del 12.5% por servicios</br>Constancia de Retención del 1% Según Acuerdo DEI 217-2010</br>Retención Art.5 Ley ISR a No Domiciliados</td>"
                + "</tr>"
                + "<tr>"
                    + "<td><b>Observaciones:</b></td>"
                    + "<td>Segun factura # " + facturas_proveedor + "</td>"
                + "</tr>"
            + "</table>";
        }

		if (1 == 2) {
        ///////////////////////////////////////////////////////////
		#region 2021-01-27

        if (user.PaisID == 3)
        {
            if (rgb_cheque.intC4 == 8)//moneda
            {
                lbl_serie.Text = "000-002-05" + strcorrelativo_doc;//Numero retencion

                if (correlativo_doc <= 100)
                {
                    lbl_rango_inicial.Text = "000-002-05-00000001";
                    lbl_rango_final.Text = "000-002-05-00000100";
                    lbl_fecha_limite_emision.Text = "05 de Abril del 2017";
                    html_cai_honduras = "CAI.: A941CE-C58587-8846B0-C6BF20-9886C2-28";
                }
                else if (correlativo_doc >= 101 && correlativo_doc <= 120) //Ticket#2020091031000101 — NUEVA SERIE FISCAL// COMPROBANTES DE RETENCION // DOLARES 
                {
                    lbl_rango_inicial.Text = "000-002-05-00000101";
                    lbl_rango_final.Text = "000-002-05-00000120";
                    lbl_fecha_limite_emision.Text = "10 de Marzo del 2021";
                    html_cai_honduras = "CAI.: DFF44B-D54462-FC4AA0-8A3820-76AB52-EC";
                }
            }
            else
            {
                lbl_serie.Text = "000-001-05" + strcorrelativo_doc;//Numero retencion

                if (correlativo_doc <= 600)
                {
                    lbl_rango_inicial.Text = "000-001-05-00000401";
                    lbl_rango_final.Text = "000-001-05-00000600";
                    lbl_fecha_limite_emision.Text = "15 de Diciembre del 2016";
                    html_cai_honduras = "CAI.: 1502E6-73ACB2-6F4680-B2E722-4B14C1-60";
                }
                else if (correlativo_doc >= 601 && correlativo_doc <= 900)
                {
                    lbl_rango_inicial.Text = "000-001-05-00000601";
                    lbl_rango_final.Text = "000-001-05-00000900";
                    lbl_fecha_limite_emision.Text = "04 de Abril del 2017";
                    html_cai_honduras = "CAI.: 3FDA50-E53078-1A48A6-3B94BB-6C48C3-9F";
                }
                else if (correlativo_doc >= 901 && correlativo_doc <= 1700)
                {
                    lbl_rango_inicial.Text = "000-001-05-00000901";
                    lbl_rango_final.Text = "000-001-05-00001700";
                    lbl_fecha_limite_emision.Text = "19 de Agosto del 2017";
                    html_cai_honduras = "CAI.: 6E0673-CC66D7-1A469D-697B9E-F8610C-FF";
                }
                else if (correlativo_doc >= 1701 && correlativo_doc <= 2300)
                {
                    lbl_rango_inicial.Text = "000-001-05-00001701";
                    lbl_rango_final.Text = "000-001-05-00002300";
                    lbl_fecha_limite_emision.Text = "03 de Agosto del 2018";
                    html_cai_honduras = "CAI.: B2AABB-7C9380-1448A7-C5C6DF-619DDC-37";
                }
                else if (correlativo_doc >= 2301 && correlativo_doc <= 2700)
                {
                    lbl_rango_inicial.Text = "000-001-05-00002301";
                    lbl_rango_final.Text = "000-001-05-00002700";
                    lbl_fecha_limite_emision.Text = "13 de Agosto del 2019";
                    html_cai_honduras = "CAI.: EFBC43-2954D9-A345A3-055C04-676474-38";
                }
                else if (correlativo_doc >= 2701 && correlativo_doc <= 3000)
                {
                    lbl_rango_inicial.Text = "000-001-05-00002701";
                    lbl_rango_final.Text = "000-001-05-00003000";
                    lbl_fecha_limite_emision.Text = "16 de Agosto del 2020";
                    html_cai_honduras = "CAI.: 041C7E-FCCE39-F041A6-F06522-3C111B-67";
                }
                else if (correlativo_doc >= 3001 && correlativo_doc <= 3200) //Ticket#2020081731000019 — NUEVA SERIE FISCAL// COMPROBANTES DE RETENCION 
                {
                    lbl_rango_inicial.Text = "000-001-05-00003001";
                    lbl_rango_final.Text = "000-001-05-00003200";
                    lbl_fecha_limite_emision.Text = "17 de Febrero del 2021";
                    html_cai_honduras = "CAI.: 8B0C1A-B772CA-AA46B6-057DB6-3E8BBB-83";
                }
            }
        }
        if (user.PaisID == 23)
        {
            if (rgb_cheque.intC4 == 8)
            {
                lbl_serie.Text = "000-002-05" + strcorrelativo_doc;//Numero retencion

                lbl_rango_inicial.Text = "000-002-05-00000001";
                lbl_rango_final.Text = "000-002-05-00000050";
                lbl_fecha_limite_emision.Text = "27 de Marzo del 2017";
                html_cai_honduras = "CAI.: 608DFD-FF1DC1-1D469C-F9E489-1CA777-05";
            }
            else
            {
                lbl_serie.Text = "000-001-05" + strcorrelativo_doc;//Numero retencion

                if (correlativo_doc <= 200)
                {
                    lbl_rango_inicial.Text = "000-001-05-00000001";
                    lbl_rango_final.Text = "000-001-05-00000200";
                    lbl_fecha_limite_emision.Text = "30 de Mayo del 2016";
                    html_cai_honduras = "CAI.: C66F95-F93DD6-1D459C-3E4D35-046281-5B";
                }
                else if (correlativo_doc >= 201 && correlativo_doc <= 270)
                {
                    lbl_rango_inicial.Text = "000-001-05-00000201";
                    lbl_rango_final.Text = "000-001-05-00000270";
                    lbl_fecha_limite_emision.Text = "27 de Febrero del 2017";
                    html_cai_honduras = "CAI.: 593A1C-BD176C-4341A3-492C31-8B7349-F9";
                }
                else if (correlativo_doc >= 271 && correlativo_doc <= 370)
                {
                    lbl_rango_inicial.Text = "000-001-05-00000271";
                    lbl_rango_final.Text = "000-001-05-00000370";
                    lbl_fecha_limite_emision.Text = "12 de Diciembre del 2017";
                    html_cai_honduras = "CAI.: 9CB960-47B442-7247A8-A5E23C-7352D9-81";
                }
                else if (correlativo_doc >= 371 && correlativo_doc <= 400)
                {
                    lbl_rango_inicial.Text = "000-001-05-00000371";
                    lbl_rango_final.Text = "000-001-05-00000400";
                    lbl_fecha_limite_emision.Text = "14 de Diciembre del 2018";
                    html_cai_honduras = "CAI.: A1A891-8BB1FB-BA469E-69294A-1D1FB5-A0";
                }
                else if (correlativo_doc >= 401 && correlativo_doc <= 420)
                {
                    lbl_rango_inicial.Text = "000-001-05-00000401";
                    lbl_rango_final.Text = "000-001-05-00000420";
                    lbl_fecha_limite_emision.Text = "26 de Diciembre del 2019";
                    html_cai_honduras = "CAI.: 34F2A8-AA302D-0E4BB3-AE3866-1AB715-A5";
                }
            }
        }

		#endregion
		///////////////////////////////////////////////////////////
		}
 
    }
}