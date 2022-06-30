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

public partial class invoice_template_recibo : System.Web.UI.Page
{
    int ID = 0;
    int tipo_transaccion = 0;
    int ban_BL = 0;
    public string html_observaciones = "";
    public string html_factura_NC = "";
    public string html_bl = "";
    public string html_Rubros = "";
    DataTable dt = null;
    UsuarioBean user;
    public string nombre, recibo;
    public decimal totalrecibo, totalpagado, saldo;
    public ArrayList facturas = new ArrayList();
    public string listado_facturas = "";
    

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["usuario"] == null)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "Nombre", "<script> refresh(); </script>");
        }
        user = (UsuarioBean)Session["usuario"];
        RE_GenericBean Datos_Factura = null;

        if (user.pais.Grupo_Empresas == 2)
        {
            Image1.ImageUrl = user.pais.Imagepath;
            Image1.Width = 175;
            Image1.Height = 175;
        }
        else if (user.PaisID == 11)
        {
            Image1.ImageUrl = user.pais.Imagepath;
        }
        else if (user.PaisID == 7)
        {
            Image1.ImageUrl = "~/img/aimar_en.jpg";
        }
        else if (user.PaisID == 12)
        {
            Image1.ImageUrl = user.pais.Imagepath;
        }
        else if (user.PaisID == 13)
        {
            Image1.ImageUrl = user.pais.Imagepath;
        }
        else if (user.PaisID == 30)
        {
            Image1.ImageUrl = user.pais.Imagepath;
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
        lbl_direccion_empresa.Text = user.pais.Direccion_Empresa;
        if ((Request.QueryString["id"] != null) && (Request.QueryString["transaccion"] != null))
        {
            ID = int.Parse(Request.QueryString["id"].ToString());
            tipo_transaccion = int.Parse(Request.QueryString["transaccion"].ToString());
            //if ((tipo_transaccion == 4) && (user.PaisID == 1))
            //{
            //    lbl_formas_pago.Text = "Cobro por cuenta Ajena <br>" + user.pais.Formas_Pago;
            //    lbl_formas_pago.Visible = true;
            //}
            if ((tipo_transaccion == 1) || (tipo_transaccion == 2))//Factura y Recibo
            {
                //***********traduccion fatura
                string path = "factura";
                RE_GenericBean reg = DB.Traducir_reportes(user, path);
                //***titulos traducidos***
                if (tipo_transaccion == 1)
                {
                    lbl_tipo_transaccion.Text = reg.strC10; //titulo
                }
                if (tipo_transaccion == 2)
                {
                    if (user.PaisID == 7)
                    {
                        lbl_tipo_transaccion.Text = "RECEIPT";
                    }
                    else
                    {
                        lbl_tipo_transaccion.Text = "RECIBO";
                    }
                }
                lb_fecha_emision.Text = reg.strC7; //fecha emision
                lbl_vencimiento.Text = reg.strC8; //fecha vencimiento
                lb_nombre.Text = reg.strC11; //nombre
                lb_cifras.Text = reg.strC22; //cifras en
                if (user.PaisID == 7 && user.contaID == 2)
                {
                }
                if (user.PaisID == 3)
                {
                    lbl_nit_empresa.Text = user.pais.Pai_Numero_Identificacion_Tributaria;
                }
                if (user.PaisID == 5)
                {
                    lb_cedula_juridica.Visible = true;
                    lbl_nit_CR.Visible = true;
                    lbl_nit_CR.Text = user.pais.Pai_Numero_Identificacion_Tributaria;
                    lb_cedula_juridica.Text = "Cedula Juridica: ";
                }
                if (user.PaisID == 21)
                {
                    lb_cedula_juridica.Visible = true;
                    lbl_nit_CR.Visible = true;
                    lbl_nit_CR.Text = user.pais.Pai_Numero_Identificacion_Tributaria;
                    lb_cedula_juridica.Text = "Cedula Juridica: ";
                }
                if (user.PaisID == 6)
                {
                    //lb_cedula_juridica.Visible = true;
                    //lb_cedula_juridica.Text = " Vía Tocumen Parque Sur, Edificio Flexspace 2, Bodega 12 y 13 ";
                    //lbl_direccion_empresa2.Visible = true;
                    //lbl_direccion_empresa2.Text = " Panamá, República de Panamá ";
                    //lbl_direccion_empresa.Visible = true;
                    //lbl_direccion_empresa.Text = " RUC: 1297764-1-604683 D.V. 02 ";
                    //lbl_ruc.Visible = true;
                    //lbl_ruc.Text = " Tel. (507) 270-0466 Fax: (507) 270-1614 ";

                    lbl_direccion_empresa.Visible = true;
                    lbl_direccion_empresa.Text = user.pais.Pai_Numero_Identificacion_Tributaria;
                    lbl_direccion_empresa2.Visible = true;
                    lbl_direccion_empresa2.Text = user.pais.Direccion_Empresa;
                }
                if (user.PaisID == 1)
                {
                    if (user.contaID == 1)
                    {
                        lbl_direccion_empresa.Text = user.pais.Direccion_Empresa;
                    }
                    else if (user.contaID == 2)
                    {
                        lbl_direccion_empresa.Text = "10087 NW 122nd St Medley, FL 3378";
                        lbl_formas_pago.Text = user.pais.Formas_Pago;
                        lbl_formas_pago.Visible = true;
                    }
                }
                if (tipo_transaccion == 1)
                {
                    Datos_Factura = (RE_GenericBean)DB.getFacturaData(ID);
                }
                if (tipo_transaccion == 2)
                {
                    Datos_Factura = (RE_GenericBean)DB.getRcptData_forprint(ID);
                }
                cargar_Factura(Datos_Factura, ID, tipo_transaccion);
                if (((Datos_Factura.intC8 == 3) || (Datos_Factura.intC8 == 5) || (Datos_Factura.intC8 == 6)) && Datos_Factura.intC4 == 8) // si los paises son hn y cr no se muestra equivalente 
                {
                }
                if (Datos_Factura.intC8 == 1)
                {
                    if ((Datos_Factura.strC50 != "-") && (Datos_Factura.strC50 == "1"))
		            {
                        var fel = DB.DataFEL(user.PaisID, Datos_Factura.strC49, Datos_Factura.strC52);
                        lbl_firma_comentario.Text = "";
                        lbl_firma_digital.Text = fel.firma;
                        /*
		                lbl_firma_comentario.Text = "Firma Electronica.: ";
                        if (DB.isFELDate(user.PaisID)) //2019-07-29
		                    lbl_firma_digital.Text = DB.FirmaFEL("5", Datos_Factura.strC49, Datos_Factura.strC52); //, Datos_Factura.strC28, "Correlativo", Datos_Factura.strC1);
						else
                        	lbl_firma_digital.Text = Datos_Factura.strC49;
                        */
                        lbl_firma_comentario.Visible = true;
                        lbl_firma_digital.Visible = true;
                    }
                }
            }


            DB.parametros Params = DB.EmpresaParametros(user.PaisID, "", "", "** TITULO **", ""); //pais, sistema, doc_id, titulo, edicion
            Image1.ImageUrl = "data:image;base64," + System.Convert.ToBase64String(Params.logo);
            Image1.Height = 63;
            Image1.Width = 237;
            lbl_direccion_empresa.Text = Params.direccion2;
            
        }

        //Aca se pone todo lo referente al detalle de fc y nd del recibo
        if (Request.QueryString["id"] != null)
        {
            int rcpt = int.Parse(Request.QueryString["id"].ToString().Trim());
            RE_GenericBean rgb = (RE_GenericBean)DB.getRcptData(rcpt); //obtengo los datos del recibo
            rgb.decC1 = DB.getTotalAbonadoXRcpt(rcpt, 2);//obtengo todo lo abonado por el recibo tipo2 porque es recibo
            // Obtengo el arreglo de facturas que el recibo mato o aplico
            DataTable dt = (DataTable)DB.getRcptFacturas(rcpt);
            RE_GenericBean datos = null;
            RE_GenericBean cliente = (RE_GenericBean)DB.getDataClient(rgb.douC1);
            nombre = cliente.strC2;
            recibo = rgb.strC1;
            totalrecibo = Math.Round(rgb.decC3, 2);
            totalpagado = Math.Round(rgb.decC1, 2);
            saldo = Math.Round(totalrecibo - totalpagado, 2);
            DataRow row = null;
            #region Dibujar Facturas
            string strFactura = "";
            string strNotaDebito = "";
            if (user.PaisID == 7)
            {
                strFactura = "INVOICE";
                strNotaDebito = "DEBIT NOTE";
            }
            else
            {
                strFactura = "Factura";
                strNotaDebito = "Nota Debito";
            }
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                row = dt.Rows[i];
                listado_facturas += "<tr>";
                listado_facturas += "<td>" + strFactura + " No. " + row[4].ToString() + "-" + row[5].ToString() + "</td>";
                listado_facturas += "<td align='right'>" + decimal.Parse(row[6].ToString()).ToString("#,#.00") + "</td>";
                listado_facturas += "</tr>";
                datos = new RE_GenericBean();
                datos.strC1 = row[1].ToString();
                datos.decC1 = decimal.Parse(row[6].ToString());
                facturas.Add(datos);
            }
            #endregion
            #region Dibujar ND
            DataTable dt_nd = (DataTable)DB.getRcptNotaDebito(rcpt);
            for (int i = 0; i < dt_nd.Rows.Count; i++)
            {
                row = dt_nd.Rows[i];
                listado_facturas += "<tr>";
                listado_facturas += "<td>" + strNotaDebito + " No. " + row[4].ToString() + "-" + row[5].ToString() + "</td>";
                listado_facturas += "<td align='right'>" + decimal.Parse(row[6].ToString()).ToString("#,#.00") + "</td>";
                listado_facturas += "</tr>";
                datos = new RE_GenericBean();
                datos.strC1 = row[1].ToString();
                datos.decC1 = decimal.Parse(row[6].ToString());
                facturas.Add(datos);
            }
            #endregion
        }
        //Fin de detalle de fc y nd del recibo
    }

    protected string traducir_texto(string texto)
    {
        string traduccion = "";
        if (user.PaisID == 7)
        {
            if (texto.Trim() == "Cheque")
            {
                traduccion = "CHECK";
            }
            if (texto.Trim() == "Efectivo")
            {
                traduccion = "CASH";
            }
        }
        else
        {
            traduccion = texto;
        }
        return traduccion;
    }

    protected void cargar_Factura(RE_GenericBean factdata, int ID, int Tipo_Transaccion)
    {
        if (factdata != null)
        {
            lbl_usuario_crea.Text = factdata.strC8;
            #region Definir el Tipo de Documento
            if (tipo_transaccion == 1)
            {
                if (user.PaisID == 7)
                {
                    lbl_tipo_transaccion.Text = "Invoice";
                }
                else if (user.PaisID == 1)
                {
                    if (user.contaID == 1)
                    {
                        lbl_tipo_transaccion.Text = "Factura";
                    }
                    else if (user.contaID == 2)
                    {
                        lbl_tipo_transaccion.Text = "Invoice";
                    }
                }
                else
                {
                    lbl_tipo_transaccion.Text = "Factura";
                }

            }
            #endregion                        
            var fel = DB.DataFEL(user.PaisID, factdata.strC49, "");
            if (fel.isFEL) {
                lbl_serie.Text = fel.Sign_Serie;
                lbl_correlativo.Text = fel.Sign_Numero;
			} else {
		        lbl_serie.Text = factdata.strC28;
		        lbl_correlativo.Text = factdata.strC1;              
			}
		    
            lbl_fecha_emision.Text = factdata.strC5;
            if ((Tipo_Transaccion == 1) || (Tipo_Transaccion == 2))
            {
                if (user.PaisID == 7)
                {
                    lbl_vencimiento.Text = "Due";
                    lbl_pagos_recibidos.Text = "PAYMENTS RECEIVED";
                    lbl_detalle_pagos.Text = "PAYMENT DETAILS";
                    lbl_total_recibo.Text = "Total Received:";
                    lbl_total_pagado.Text = "Total Paid:";
                    lbl_saldo_favor.Text = "Balance:";
                }
                else
                {
                    lbl_vencimiento.Text = "Fecha de Vencimiento ";
                }

                lbl_fecha_vencimiento.Text = factdata.strC6;
                if (user.PaisID == 3) //imprimiendo tC solo para HN
                {
                    if (user.contaID == 1)
                    {
                        lbl_tc.Visible = true;
                        decimal tc = Math.Round(DB.getTipoCambioByDay(user, factdata.strC5), 4);
                        lbl_tc.Text = "TC: " + Convert.ToString(tc) + ", ";
                    }
                }

                if ((user.PaisID == 1) && (user.contaID == 2) && (user.SucursalID == 9))
                {
                    if ((factdata.strC28 == "DI") || (factdata.strC28 == "DE") || (factdata.strC28 == "RD"))
                    {
                        lbl_direccion_empresa.Text = "";
                        lbl_nit_empresa.Text = "";
                        Image1.ImageUrl = "~/img/apl.png";
                        Image1.Width = 150;
                        lbl_formas_pago.Visible = false;
                    }
                }
            }
            
            lbl_nombre_cliente.Text = factdata.strC3;
            if ((factdata.Tipo_Operacion != 10) && (factdata.Tipo_Operacion != 11) && (factdata.Tipo_Operacion != 12))
            {
                ArrayList Arr_Sistemas = DB.Get_Detalle_Sistemas(" and b.tto_id=" + factdata.Tipo_Operacion + "");
                foreach (RE_GenericBean Bean in Arr_Sistemas)
                {
                    lbl_tipo_operacion.Text = Bean.strC1.Substring(8, (Bean.strC1.Length - 8)) + " " + Bean.strC2;
                }
            }
            if (user.PaisID == 11)
            {
                lbl_moneda.Text = Utility.TraducirMonedaInt(8);
            }
            else
            {
                lbl_moneda.Text = Utility.TraducirMonedaInt(factdata.intC4);
            }
            #region Observaciones
            if ((factdata.strC7 != null) && (factdata.strC7 != ""))
            {
                html_observaciones = "<tr>";
                html_observaciones += "<td>";
                html_observaciones += "<table align='center' cellpadding='0' cellspacing='0' class='style4'>";
                html_observaciones += "<tr>";
                html_observaciones += "<td width='100px' style='font-weight: 700'>";
                if (user.PaisID == 7)
                {
                    html_observaciones += "Observations</td>";
                }
                else
                {
                    html_observaciones += "Observaciones</td>";
                }
                html_observaciones += "<td>";
                html_observaciones += factdata.strC7;
                html_observaciones += "</td>";
                html_observaciones += "</tr>";
                html_observaciones += "<tr>";
                html_observaciones += "<td>";
                html_observaciones += factdata.strC46;
                html_observaciones += "</td>";
                html_observaciones += "</tr>";


                html_observaciones += "</table>";
                html_observaciones += "</td>";
                html_observaciones += "</tr>";
                html_observaciones += "<tr>";
                html_observaciones += "<td>";
                html_observaciones += "&nbsp;</td>";
                html_observaciones += "</tr>";
            }
            #endregion
            #region Factura asociada a la NC
            if ((factdata.strC40 != null) && (factdata.strC40 != ""))
            {
                html_factura_NC = "<tr>";
                html_factura_NC += "<td>";
                html_factura_NC += "<table align='center' cellpadding='0' cellspacing='0' class='style4'>";
                html_factura_NC += "<tr>";
                html_factura_NC += "<td width='100px' style='font-weight: 700'>";
                if (user.PaisID == 7 && tipo_transaccion == 3)
                {
                    html_factura_NC += "Note Credit Applied to Document: </td>";
                }
                else
                {
                    html_factura_NC += "Nota de Credito Aplicada a Documento: </td>";
                }
                html_factura_NC += "<td>";
                html_factura_NC += factdata.strC40;
                html_factura_NC += "</td>";
                html_factura_NC += "</tr>";
                html_factura_NC += "</table>";
                html_factura_NC += "</td>";
                html_factura_NC += "</tr>";
                html_factura_NC += "<tr>";
                html_factura_NC += "<td>";
                html_factura_NC += "&nbsp;</td>";
                html_factura_NC += "</tr>";
            }
            #endregion
            #region Cargar BL
            html_bl = "<tr>"
            + "<td>"
            + "<table align='center' cellpadding='0' cellspacing='0' class='style4'>"
                    + "<tr>"
                        + "<td>"
                            + "<table align='center' cellpadding='0' cellspacing='0' class='style3'>";
            if ((factdata.strC41 != null) && (factdata.strC41 != ""))
            {
                html_bl += "<tr>"
                                                + "<td width='100px'>"
                                                    + "<b>Agente</b></td>"
                                                + "<td colspan='5' width='100px;'>"
                                                    + factdata.strC41
                                                + "</td>"
                                            + "</tr>";
                ban_BL++;
            }
            if ((factdata.strC15 != null) && (factdata.strC15 != ""))
            {
                html_bl += "<tr>"
                                                + "<td width='100px'>"
                                                    + "<b>Shipper</b></td>"
                                                + "<td colspan='5' width='100px;'>"
                                                    + factdata.strC15
                                                + "</td>"
                                            + "</tr>";
                ban_BL++;
            }
            if ((factdata.strC17 != null) && (factdata.strC17 != ""))
            {
                html_bl += "<tr>"
                                                + "<td width=100px'>"
                                                    + "<b>Consignee</b></td>"
                                                + "<td colspan='5' width='100px;'>"
                                                    + factdata.strC17
                                                + "</td>"
                                            + "</tr>";
                ban_BL++;
            }
            if ((factdata.strC18 != null) && (factdata.strC18 != ""))
            {
                html_bl += "<tr>"
                                                + "<td width='100px'>"
                                                    + "<b>Comodity</b></td>"
                                                + "<td colspan='5' width='100px;'>"
                                                    + factdata.strC18
                                                + "</td>"
                                            + "</tr>";
                ban_BL++;
            }
            if (((factdata.strC19 != null) && (factdata.strC19 != "")) || ((factdata.strC32 != null) && (factdata.strC32 != "")))
            {
                html_bl += "<tr>"
                                                + "<td width='100px'>"
                                                    + "<b>Paquetes</b></td>"
                                                + "<td colspan='5' width='100px;'>"
                                                    + factdata.strC19
                                                    + "&nbsp;"
                                                    + factdata.strC32
                                                + "</td>"
                                            + "</tr>";
                ban_BL++;
            }
            if (((factdata.strC20 != null) && (factdata.strC20 != "")) || ((factdata.strC21 != null) && (factdata.strC21 != "")))
            {
                ban_BL++;
                html_bl += "<tr>";
                if ((factdata.strC20 != null) && (factdata.strC20 != ""))
                {
                    html_bl += "<td width='100px'>"
                        + "<b>Peso</b></td>"
                    + "<td width='100px;'>"
                        + factdata.strC20
                    + "</td>";
                }
                else
                {
                    html_bl += "<td width='100px'>"
                        + "</td>"
                    + "<td width='100px;'>"
                    + "</td>";
                }
                if ((factdata.strC21 != null) && (factdata.strC21 != ""))
                {
                    html_bl += "<td width='100px'>"
                        + "<b>Volumen</b></td>"
                    + "<td width='100px'>"
                        + factdata.strC21
                    + "</td>";
                }
                else
                {
                    html_bl += "<td width='100px'>"
                        + "</td>"
                    + "<td width='100px;'>"
                    + "</td>";
                }
                html_bl += "<td width='100px'>"
                    + "&nbsp;</td>"
                + "<td width='100px'>"
                    + "&nbsp;</td>"
            + "</tr>";
            }
            if (((factdata.strC10 != null) && (factdata.strC10 != "")) || ((factdata.strC9 != null) && (factdata.strC9 != "")))
            {
                ban_BL++;
                html_bl += "<tr>";
                if ((factdata.strC10 != null) && (factdata.strC10 != ""))
                {
                    html_bl += "<td width='100px'>"
                        + "<b>MBL</b></td>"
                    + "<td width='300px;'>"
                        + factdata.strC10
                    + "</td>";
                }
                else
                {
                    html_bl += "<td width='100px'>"
                        + "</td>"
                    + "<td width='100px;'>"
                    + "</td>";
                }
                if ((factdata.strC9 != null) && (factdata.strC9 != ""))
                {
                    html_bl += "<td width='100px'>"
                        + "<b>HBL</b></td>"
                    + "<td width='300px'>"
                        + factdata.strC9
                    + "</td>";
                }
                else
                {
                    html_bl += "<td width='100px'>"
                        + "</td>"
                    + "<td width='100px;'>"
                    + "</td>";
                }
                html_bl += "<td width='100px'>"
                    + "&nbsp;</td>"
                + "<td width='100px'>"
                    + "&nbsp;</td>"
            + "</tr>";
            }
            if (((factdata.strC12 != null) && (factdata.strC12 != "")) || ((factdata.strC11 != null) && (factdata.strC11 != "")))
            {
                ban_BL++;
                html_bl += "<tr>";
                if ((factdata.strC12 != null) && (factdata.strC12 != ""))
                {
                    html_bl += "<td width='100px'>"
                        + "<b>Routing</b></td>"
                    + "<td width='100px;'>"
                        + factdata.strC12
                    + "</td>";
                }
                else
                {
                    html_bl += "<td width='100px'>"
                        + "</td>"
                    + "<td width='100px;'>"
                    + "</td>";
                }
                if ((factdata.strC11 != null) && (factdata.strC11 != ""))
                {
                    html_bl += "<td width='100px'>"
                        + "<b>Contenedor</b></td>"
                    + "<td width='100px'>"
                        + factdata.strC11
                    + "</td>";
                }
                else
                {
                    html_bl += "<td width='100px'>"
                        + "</td>"
                    + "<td width='100px;'>"
                    + "</td>";
                }
                html_bl += "<td width='100px'>"
                    + "&nbsp;</td>"
                + "<td width='100px'>"
                    + "&nbsp;</td>"
            + "</tr>";
            }
            if (((factdata.strC13 != null) && (factdata.strC13 != "")) || ((factdata.strC14 != null) && (factdata.strC14 != "")))
            {
                ban_BL++;
                html_bl += "<tr>";
                if ((factdata.strC13 != null) && (factdata.strC13 != ""))
                {
                    html_bl += "<td width='100px'>"
                        + "<b>Naviera</b></td>"
                    + "<td width='100px;'>"
                        + factdata.strC13
                    + "</td>";
                }
                else
                {
                    html_bl += "<td width='100px'>"
                        + "</td>"
                    + "<td width='100px;'>"
                    + "</td>";
                }
                if ((factdata.strC14 != null) && (factdata.strC14 != ""))
                {
                    html_bl += "<td width='100px'>"
                        + "<b>Vapor</b></td>"
                    + "<td width='100px'>"
                        + factdata.strC14
                    + "</td>";
                }
                else
                {
                    html_bl += "<td width='100px'>"
                        + "</td>"
                    + "<td width='100px;'>"
                    + "</td>";
                }
                html_bl += "<td width='100px'>"
                    + "&nbsp;</td>"
                + "<td width='100px'>"
                    + "&nbsp;</td>"
            + "</tr>";
            }
            if (((factdata.strC22 != null) && (factdata.strC22 != "")) || ((factdata.strC23 != null) && (factdata.strC23 != "")))
            {
                ban_BL++;
                html_bl += "<tr>";
                if ((factdata.strC22 != null) && (factdata.strC22 != ""))
                {
                    html_bl += "<td width='100px'>"
                        + "<b>Dua Ingreso</b></td>"
                    + "<td width='100px;'>"
                        + factdata.strC22
                    + "</td>";
                }
                else
                {
                    html_bl += "<td width='100px'>"
                        + "</td>"
                    + "<td width='100px;'>"
                    + "</td>";
                }
                if ((factdata.strC23 != null) && (factdata.strC23 != ""))
                {
                    html_bl += "<td width='100px'>"
                        + "<b>Dua Salida</b></td>"
                    + "<td width='100px'>"
                        + factdata.strC23
                    + "</td>";
                }
                else
                {
                    html_bl += "<td width='100px'>"
                        + "</td>"
                    + "<td width='100px;'>"
                    + "</td>";
                }
                html_bl += "<td width='100px'>"
                    + "&nbsp;</td>"
                + "<td width='100px'>"
                    + "&nbsp;</td>"
            + "</tr>";
            }
            if (((factdata.strC31 != null) && (factdata.strC31 != "")) || ((factdata.strC16 != null) && (factdata.strC16 != "")))
            {
                ban_BL++;
                html_bl += "<tr>";
                if ((factdata.strC31 != null) && (factdata.strC31 != ""))
                {
                    html_bl += "<td width='100px'>"
                        + "<b>Recibo Aduanal</b></td>"
                    + "<td width='100px;'>"
                        + factdata.strC31
                    + "</td>";
                }
                else
                {
                    html_bl += "<td width='100px'>"
                        + "</td>"
                    + "<td width='100px;'>"
                    + "</td>";
                }
                if ((factdata.strC16 != null) && (factdata.strC16 != ""))
                {
                    string regimen = DB.Get_Regimen_Aduanero_XID(user.PaisID, factdata.intC11);
                    html_bl += "<td width='100px'>"
                        + "<b>Poliza Aduanal</b></td>"
                    + "<td width='100px'>"
                        + factdata.strC16
                    + "</td>";
                    html_bl += "<td width='100px'>"
                       + "<b>Regimen</b></td>"
                   + "<td width='100px'>"
                       + regimen
                   + "</td>";
                }
                else
                {
                    html_bl += "<td width='100px'>"
                        + "</td>"
                    + "<td width='100px;'>"
                    + "</td>";
                }
                html_bl += "<td width='100px'>"
                    + "&nbsp;</td>"
                + "<td width='100px'>"
                    + "&nbsp;</td>"
            + "</tr>";
            }
            html_bl += "</table>"
                        + "</td>"
                    + "</tr>"
                + "</table>"
            + "</td>"
            + "</tr>";
            if (ban_BL == 0)
            {
                html_bl = "";
            }
            #endregion
            #region Cargar Rubros
            if (user.PaisID == 0)
            {

                dt = (DataTable)DB.getPagosRecibo(ID);
                if ((dt != null) && (factdata.strC30 == ""))
                {

                    html_Rubros = "<table align='center' cellpadding='0' cellspacing='3' class='style3'>";
                    html_Rubros += "<tr>"
                            + "<td width='300px' class='style6'>"
                                + "<b><u>Detalle</u></b>"
                            + "</td>"
                            + "<td width='200px' align='right' class='style6'>"
                                + "<b><u>Valor</u></b>"
                            + "</td>"
                        + "</tr>";
                    foreach (DataRow dr in dt.Rows)
                    {
                        html_Rubros += "<tr>"
                            + "<td width='500px'>"
                                + dr[1].ToString()
                            + "</td>"
                            + "<td width='100px' align='right'>"
                                + decimal.Parse(dr[9].ToString()).ToString("#,#.00")
                            + "</td>"
                        + "</tr>";
                        if ((dr[8].ToString() != null) && (dr[8].ToString() != ""))
                        {
                            html_Rubros += "<tr>"
                            + "<td colspan='2' style='width: 600px' width='300px'>"
                                + "         --->" + dr[8].ToString()
                            + "</td>"
                        + "</tr>";
                        }
                    }
                    html_Rubros += "</table>";
                }
            }
            else
            {
                dt = (DataTable)DB.getPagosRecibo(ID);
                if ((dt != null) && (factdata.strC30 == ""))
                {

                    string tipo = "";
                    string banco = "";
                    string cuenta = "";
                    string referencia = "";
                    string monto = "";

                    if (user.PaisID == 7)
                    {
                        tipo = "Type";
                        banco = "Bank";
                        cuenta = "Account";
                        referencia = "Reference";
                        monto = "Amount";
                    }
                    else
                    {
                        tipo = "Tipo";
                        banco = "Banco";
                        cuenta = "Cuenta";
                        referencia = "Referencia";
                        monto = "Monto";
                    }
                    html_Rubros = "<table align='center' cellpadding='0' cellspacing='3' class='style3'>";
                    html_Rubros += "<tr>"
                            + "<td width='300px' class='style6'>"
                                + "<b><u>" + tipo+ "</u></b>"
                            + "</td>"
                            + "<td width='200px'class='style6'>"
                                + "<b><u>" + banco + "</u></b>"
                            + "</td>"
                            + "<td width='200px'class='style6'>"
                                + "<b><u>" + cuenta + "</u></b>"
                            + "</td>"
                            + "<td width='200px'class='style6'>"
                                + "<b><u>" + referencia + "</u></b>"
                            + "</td>"
                            + "<td width='200px'class='style6'>"
                                + "<b><u>" + monto + "</u></b>"
                            + "</td>"
                        + "</tr>";
                    int contador = 0;
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (contador == 0)
                        {
                            html_Rubros += "<tr>"
                                + "<td width='300px'>"
                                    + traducir_texto(dr[0].ToString())
                                + "</td>"
                                + "<td width='200px'>"
                                    + dr[1].ToString()
                                + "</td>"
                                + "<td width='200px'>"
                                    + dr[2].ToString()
                                + "</td>"
                                + "<td width='200px'>"
                                    + dr[3].ToString()
                                + "</td>"
                                + "<td width='200px'>"
                                    + dr[4].ToString()
                                + "</td>"
                            + "</tr>";
                        }
                        contador += 1;
                    }
                    html_Rubros += "</table>";
                }
            }
            #endregion
            if (user.PaisID == 0)
            {
                #region GRH
                lbl_total.Text = factdata.decC4.ToString("#,#.00");
                #endregion
            }
            else
            {
                #region Todas las Empresas excepto GRH
                lbl_total.Text = factdata.decC3.ToString("#,#.00");
                Conv c = new Conv();
                if (user.PaisID == 7)
                {
                    lbl_total_letras.Text = "TOTAL IN LETTERS.:    " + c.enletras(factdata.decC3.ToString(), 8);
                }
                else if (user.PaisID == 1)
                {
                    if (user.contaID == 1)
                    {
                        lbl_total_letras.Text = "TOTAL EN LETRAS.:    " + c.enletras(factdata.decC3.ToString(), 1);
                    }
                    else if (user.contaID == 2)
                    {
                        lbl_total_letras.Text = "TOTAL IN LETTERS.:    " + c.enletras(factdata.decC3.ToString(), 8);
                    }
                }
                else
                {
                    lbl_total_letras.Text = "TOTAL EN LETRAS.:    " + c.enletras(factdata.decC3.ToString(), 1);
                }
                
                if (user.PaisID == 5)
                {
                    if ((lbl_serie.Text == "NDFA") || (lbl_serie.Text == "F1"))
                    {
                        decimal CR_Total_Anticipos = 0;
                        decimal CR_Total = 0;
                        decimal CR_Saldo_Pagar = 0;
                        CR_Total_Anticipos = DB.getTotalAbonadoNotaDebito_For_Print(ID);
                        CR_Total = factdata.decC3;
                        CR_Saldo_Pagar = CR_Total - CR_Total_Anticipos;
                        
                    }
                }
                #endregion
            }
            if (factdata.intC8 == 9 || factdata.intC8 == 2) // muestra retencion si el pais es el salvador2 o el salvador
            {
                
                decimal retencion = 0;
                retencion = DB.getRetencioByFactID(factdata.intC1);
                if (retencion == 0)
                {
                    
                }
            }
        }
        /*
        DB.parametros Params = DB.EmpresaParametros(user.PaisID, "", "", "** TITULO **", ""); //pais, sistema, doc_id, titulo, edicion
        Image1.ImageUrl = "data:image;base64," + System.Convert.ToBase64String(Params.logo);
        Image1.Height = 63;
        Image1.Width = 237; 
        */
    }
    
}