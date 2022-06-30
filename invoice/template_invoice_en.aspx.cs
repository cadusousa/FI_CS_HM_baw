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

public partial class invoice_template_invoice_en : System.Web.UI.Page
{
    int ID = 0;
    int tipo_transaccion = 0;
    int ban_BL = 0;
    public string html_observaciones = "";
    public string html_factura_NC = "";
    public string html_bl = "";
    public string html_Rubros = "";
    public string factura_beneficio = "";
    public string fecha_emision = "";
    public string rango_autorizado = "";
    public string moneda = "";
    public string moneda_equivalente = "";
    public string texto_recordatorio_pago = "";
    public string texto_reclamo_documento = "";

    DataTable dt = null;
    public UsuarioBean user;
    protected void Page_Load(object sender, EventArgs e)
    {
        string nombre_grupo_empres = "";
        if (Session["usuario"] == null)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "Nombre", "<script> refresh(); </script>");
        }
        user = (UsuarioBean)Session["usuario"];
        RE_GenericBean Datos_Factura = null;

        
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

        lbl_direccion_empresa.Text = "10087 NW 122nd St. Medley, FL 3378";
        if ((Request.QueryString["id"] != null) && (Request.QueryString["transaccion"] != null))
        {
            ID = int.Parse(Request.QueryString["id"].ToString());
            tipo_transaccion = int.Parse(Request.QueryString["transaccion"].ToString());




            if (tipo_transaccion == 1)//Factura y Recibo
            {
                //***********traduccion fatura
                string path = "factura";
                RE_GenericBean reg = DB.Traducir_reportes(user, path);
                //***titulos traducidos***
                lbl_tipo_transaccion.Text = "Invoice"; //titulo
                lb_fecha_emision.Text = "Issue Date"; //fecha emision
                lbl_vencimiento.Text = "Due"; //fecha vencimiento
                lb_codigo.Text = "Code"; //codigo
                lb_nombre.Text = "Name"; //nombre
                lb_nit.Text = "TIN"; //nit
                lb_razon.Text = "Bussines Name"; //razon
                lb_direccion.Text = "Address"; //direccion
                lbl_equivalente.Text = "Equivalent"; //equivalente
                lb_impuesto.Text = "Tax"; //impuesto
                lb_cifras.Text = "Amount in"; //cifras en
                if (user.PaisID == 1)
                {
                    if (user.contaID == 1)
                    {
                        lbl_direccion_empresa.Text = "10087 NW 122nd St. Medley, FL 3378";
                        lbl_total_equivalente.Visible = true;
                        lbl_equivalente.Visible = true;
                    }
                    else if (user.contaID == 2)
                    {
                        lbl_direccion_empresa.Text = "10087 NW 122nd St Medley, FL 3378";
                        lbl_total_equivalente.Visible = false;
                        lbl_equivalente.Visible = false;
                        lbl_formas_pago.Text = user.pais.Formas_Pago;
                        lbl_formas_pago.Visible = true;
                    }
                }
                if (tipo_transaccion == 1)
                {
                    Datos_Factura = (RE_GenericBean)DB.getFacturaData(ID);
                }
                cargar_Factura(Datos_Factura, ID, tipo_transaccion);
                if (((Datos_Factura.intC8 == 3) || (Datos_Factura.intC8 == 5) || (Datos_Factura.intC8 == 6)) && Datos_Factura.intC4 == 8) // si los paises son hn y cr no se muestra equivalente 
                {
                    lbl_total_equivalente.Visible = false;
                    lbl_equivalente.Visible = false;
                }

                //if (Datos_Factura.intC8 == 1)
                //{
                if ((Datos_Factura.strC50 != "-") && (Datos_Factura.strC50 == "1"))
                {
                    if ((user.PaisID == 1) || (user.PaisID == 15))
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
                    else if (user.PaisID == 5)
                    {
                        lbl_firma_comentario.Text = "Codigo Unico de Consulta.: ";
                        lbl_firma_digital.Text = Datos_Factura.strC49;
                        lbl_firma_comentario.Visible = true;
                        lbl_firma_digital.Visible = true;
                    }
                }
                //}

                
                if (user.pais.Grupo_Empresas == 2)
                {
                    nombre_grupo_empres = "LATIN FREIGHT";
                }
                else
                {
                    nombre_grupo_empres = "AIMAR";
                }

                if (Datos_Factura.intC4 == 8)
                {
                    lbl_formas_pago.Visible = true;
                    lbl_formas_pago.Text = "The support of these charges is detailed in the payment of taxes covered by the <strong>policy of import.</strong><br>Method of payment:   CASHIER'S CHECK PAYABLE TO " + nombre_grupo_empres + "<br>Administrative fees for dishonored check $30.00";
                }
                else
                {
                    lbl_formas_pago.Text = "El soporte de estos Cargos se encuentra Detallado en el pago de Impuestos amparado en la <strong>Poliza de Importacion.</strong><br>Forma de pago:   CHEQUE DE CAJA A NOMBRE DE " + nombre_grupo_empres + "<br>Gastos administrativos por cheque rechazado $30.00";
                }
            }
            if (tipo_transaccion == 4)//Nota Debito
            {
                //***********traduccion nd
                string path = "nota_debito";
                RE_GenericBean reg = DB.Traducir_reportes(user, path);
                //***titulos traducidos***

                lbl_tipo_transaccion.Text = "Invoice"; //titulo
                lb_fecha_emision.Text = "Issue Date"; //fecha emision
                lbl_vencimiento.Text = "Due"; //fecha vencimiento
                lb_codigo.Text = "Code"; //codigo
                lb_nombre.Text = "Name"; //nombre
                lb_nit.Text = "TIN"; //nit
                lb_razon.Text = "Bussines Name"; //razon
                lb_direccion.Text = "Address"; //direccion
                lbl_equivalente.Text = "Equivalent"; //equivalente
                lb_impuesto.Text = "Tax"; //impuesto
                lb_cifras.Text = "Amount in"; //cifras en


                RE_GenericBean Datos_Nota_Debito = (RE_GenericBean)DB.getNotaDebitoData(ID);
                RE_GenericBean Datos_Nota_Debito_Ordenados = Ordenar_Arrelgo(Datos_Nota_Debito, tipo_transaccion);
                cargar_Factura(Datos_Nota_Debito_Ordenados, ID, tipo_transaccion);
                lbl_usuario_crea.Text = Datos_Nota_Debito.strC5.ToString(); // usuario que creo la ND.
                if (user.PaisID == 1)
                {
                    if (user.contaID == 1)
                    {
                        lbl_total_equivalente.Visible = false;
                        lbl_equivalente.Visible = false;
                        lbl_formas_pago.Visible = true;
                        //lbl_formas_pago.Text = "Cobro por cuenta Ajena";//Se comenta para agregar comentarios Fiscal USD solicitados por Esdras Mendez
                        
                        if (Datos_Nota_Debito.intC4 == 8)
                        {
                            lbl_tipo_transaccion.Text = "Debit Note";
                            lbl_formas_pago.Text += "<br>Tax Free";
                            lbl_formas_pago.Text = "The support of these charges is detailed in the payment of taxes covered by the <strong>policy of import.</strong><br>Method of payment:   CASHIER'S CHECK PAYABLE TO AIMAR, S.A.<br>Administrative fees for dishonored check $30.00";
                        }
                        else
                        {
                            lbl_tipo_transaccion.Text = "Nota de Debito";
                            lbl_formas_pago.Text += "<br>Exenta de IVA";
                            lbl_formas_pago.Text = "El soporte de estos Cargos se encuentra Detallado en el pago de Impuestos amparado en la <strong>Poliza de Importacion.</strong><br>Forma de pago:   CHEQUE DE CAJA A NOMBRE DE AIMAR, S.A.<br>Gastos administrativos por cheque rechazado $30.00";
                        }
                    }
                    else if (user.contaID == 2)
                    {
                        lbl_total_equivalente.Visible = false;
                        lbl_equivalente.Visible = false;
                        lbl_formas_pago.Visible = true;
                        lbl_formas_pago.Text = "Cobro por cuenta Ajena <br>" + user.pais.Formas_Pago;

                        #region Cambio de Informacion por Cobro de DEMORAS APL
                        if ((user.SucursalID == 9) && ((lbl_serie.Text == "NDDAPLI") || (lbl_serie.Text == "NDDAPLE")))
                        {
                            lbl_nit_empresa.Text = "";
                            lbl_formas_pago.Text = "*Por favor emitir Giro Bancario a nombre de AMERICAN PRESIDENT LINES";
                            lbl_tipo_transaccion.Text = "Debit Note";
                            Image1.ImageUrl = "~/img/apl.png";
                            Image1.Height = 100;
                            Image1.Width = 150;
                        }
                        #endregion
                    }
                }
                
            }
        }

    }
    protected void cargar_Factura(RE_GenericBean factdata, int ID, int Tipo_Transaccion)
    {
        if (factdata != null)
        {
            #region Definir el Tipo de Documento
            if (tipo_transaccion == 1)
            {
                if (user.PaisID == 1)
                {
                    if (user.contaID == 1)
                    {
                        lbl_tipo_transaccion.Text = "Factura";
                        if (factdata.intC4 == 8)
                        {
                            //Fiscal USD
                            lbl_tipo_transaccion.Text = "Invoice";
                            texto_recordatorio_pago = "";
                        }
                        else
                        {
                            lbl_tipo_transaccion.Text = "Factura";
                        }
                    }
                    else if (user.contaID == 2)
                    {
                        lbl_tipo_transaccion.Text = "Invoice";
                        #region Cambio de Informacion por Cobro de DEMORAS APL
                        if ((user.SucursalID == 9) && ((factdata.strC28 == "DI") || (factdata.strC28 == "DE")))
                        {
                            lbl_direccion_empresa.Text = "10087 NW 122nd St. Medley, FL 3378";
                            lbl_nit_empresa.Text = "";
                            lbl_formas_pago.Text = "*Por favor emitir Giro Bancario a nombre de AMERICAN PRESIDENT LINES";
                            lbl_tipo_transaccion.Text = "Invoice";
                            Image1.ImageUrl = "~/img/apl.png";
                            Image1.Height = 100;
                            Image1.Width = 150;
                        }
                        #endregion
                    }
                }
                else if (user.PaisID == 15)
                {
                    if (user.contaID == 1)
                    {
                        lbl_tipo_transaccion.Text = "Factura";
                        if (factdata.intC4 == 8)
                        {
                            //Fiscal USD
                            lbl_tipo_transaccion.Text = "Invoice";
                            texto_recordatorio_pago = "";
                        }
                        else
                        {
                            lbl_tipo_transaccion.Text = "Factura";
                        }
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
            else if (tipo_transaccion == 4)
            {
                if (user.PaisID == 1)
                {
                    if (user.contaID == 1)
                    {
                        if (factdata.intC4 == 8)
                        {
                            lbl_tipo_transaccion.Text = "Debit Note";
                            texto_recordatorio_pago = "";
                        }
                        else
                        {
                            lbl_tipo_transaccion.Text = "Nota de Debito";
                        }
                        
                    }
                    else if (user.contaID == 2)
                    {
                        lbl_tipo_transaccion.Text = "Debit Note";

                    }
                }
                else if (user.PaisID == 15)
                {
                    if (user.contaID == 1)
                    {
                        if (factdata.intC4 == 8)
                        {
                            lbl_tipo_transaccion.Text = "Debit Note";
                            texto_recordatorio_pago = "";
                        }
                        else
                        {
                            lbl_tipo_transaccion.Text = "Nota de Debito";
                        }
                    }
                    else if (user.contaID == 2)
                    {
                        lbl_tipo_transaccion.Text = "Debit Note";

                    }
                }
                else
                {
                    lbl_tipo_transaccion.Text = "Nota Debito/Invoice";
                }
            }
            #endregion



            //////////////////////////////////////PARAMETROS///////////////////////////////////////////
            DB.parametros Params = DB.EmpresaParametros(user.PaisID, "", "", "** TITULO **", ""); //pais, sistema, doc_id, titulo, edicion        
            Image1.ImageUrl = "data:image;base64," + System.Convert.ToBase64String(Params.logo);
            Image1.Height = 63;
            Image1.Width = 237; 
            lbl_direccion_empresa.Text = Params.direccion2;





            lbl_serie.Text = factdata.strC28;
            //Formaterar correlativo honduras
            string strcorrelativo_doc = "";
            int correlativo_doc = 0;
            strcorrelativo_doc = "-" + factdata.strC1;
            
            lbl_correlativo.Text = strcorrelativo_doc;			

            var fel = DB.DataFEL(user.PaisID, factdata.strC49, "");
            if (fel.isFEL) {
                lbl_serie.Text = fel.Sign_Serie;
                lbl_correlativo.Text = fel.Sign_Numero;            
			}

            lbl_fecha_emision.Text = factdata.strC5;
            if (Tipo_Transaccion == 1)
            {
                texto_reclamo_documento = "Dear Customer, the amount of this invoice is considered correct and payable as is, if no errors are claimed within 15 calendar days of the issuance date of this invoice";
            }
            else if (Tipo_Transaccion == 4)
            {
                texto_reclamo_documento = "Dear Customer, the amount of this debit note is considered correct and payable as is, if no errors are claimed within 15 calendar days of the issuance date of this debit note";
            }
            
            if ((Tipo_Transaccion == 1) || (Tipo_Transaccion == 4) || (Tipo_Transaccion == 14))
            {
                 if (factdata.intC4 == 8)
                 {
                     lbl_vencimiento.Text = "Due";
                 }
                 else
                 {
                     lbl_vencimiento.Text = "Fecha de Vencimiento ";
                 }

                lbl_fecha_vencimiento.Text = factdata.strC6;
            }
            lbl_codigo_cliente.Text = factdata.intC3.ToString();
            lbl_nombre_cliente.Text = factdata.strC3;
            lbl_nit.Text = factdata.strC2;
            lbl_razon.Text = factdata.strC26;
            if ((factdata.Tipo_Operacion != 10) && (factdata.Tipo_Operacion != 11) && (factdata.Tipo_Operacion != 12))
            {
                ArrayList Arr_Sistemas = DB.Get_Detalle_Sistemas(" and b.tto_id=" + factdata.Tipo_Operacion + "");
                foreach (RE_GenericBean Bean in Arr_Sistemas)
                {
                    string buscar = "";
                    string reemplazar = "";
                    buscar = Bean.strC1.Substring(8, (Bean.strC1.Length - 8));
                    reemplazar = buscar;
                    if (tipo_transaccion == 1 || tipo_transaccion == 4)
                    {
                        if ((user.PaisID == 1 || user.PaisID == 15))
                        {
                            if (factdata.Tipo_Operacion == 1 || factdata.Tipo_Operacion == 2 || factdata.Tipo_Operacion == 17 || factdata.Tipo_Operacion == 18 || factdata.Tipo_Operacion == 19)
                            {
                                buscar = "MARITIMO";
                                reemplazar = "SEA";
                            }
                            if (factdata.Tipo_Operacion == 5 || factdata.Tipo_Operacion == 6 || factdata.Tipo_Operacion == 7)
                            {
                                buscar = "TERRESTRE";
                                reemplazar = "LAND";
                            }
                            if (factdata.Tipo_Operacion == 3 || factdata.Tipo_Operacion == 4)
                            {
                                buscar = "AEREO";
                                reemplazar = "AIR";
                            }

                        }
                    }


                    lbl_tipo_operacion.Text = Bean.strC1.Substring(8, (Bean.strC1.Length - 8)).Replace(buscar, reemplazar) + " " + Bean.strC2;
                }
            }
            lbl_moneda.Text = Utility.TraducirMonedaInt(factdata.intC4);
            lbl_direccion.Text = factdata.strC4;
            #region Observaciones
            if ((factdata.strC7 != null) && (factdata.strC7 != ""))
            {
                html_observaciones = "<tr>";
                html_observaciones += "<td>";
                html_observaciones += "<table align='center' cellpadding='0' cellspacing='0' class='style4'>";
                html_observaciones += "<tr>";
                html_observaciones += "<td width='100px' style='font-weight: 700'>";
                if (user.PaisID == 1 || user.PaisID == 15)
                {
                    if (factdata.intC4 == 8)
                    {
                        html_observaciones += "Observations</td>";
                    }
                    else 
                    {
                        html_observaciones += "Observaciones</td>";
                    }
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
                if ((user.PaisID == 1 || user.PaisID == 15) && tipo_transaccion == 3)
                {
                    if (factdata.intC4 == 8)
                    {
                        html_factura_NC += "Note Credit Applied to Document: </td>";
                    }
                    else
                    {
                        html_factura_NC += "Nota de Credito Aplicada a Documento: </td>";
                    }
                }
                //Formaterar correlativo honduras
                string strcorrelativo_nc = "";
                strcorrelativo_nc = factdata.strC40;
                //

                html_factura_NC += "<td>";
                html_factura_NC += strcorrelativo_nc;
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
                if ((factdata.Tipo_Operacion != 5) && (factdata.Tipo_Operacion != 6) && (factdata.Tipo_Operacion != 7))
                {
                    html_bl += "<tr>"
                                                    + "<td width='100px'>";
                    if(factdata.intC4 == 8)
                    {
                        html_bl += "<b>Agent</b></td>";
                    }
                    else
                    {
                        html_bl += "<b>Agente</b></td>";
                    }
                    html_bl += "<td colspan='5' width='100px;'>"
                        + factdata.strC41
                    + "</td>"
                + "</tr>";
                    ban_BL++;
                }
                //Insertar Nombre y direccion de agente
                //if ((user.PaisID == 1 || user.PaisID == 15))
                //{
                //    string criterio = "agente = '" + factdata.strC41 + "'";
                //    ArrayList datos_agente = new ArrayList();
                //    datos_agente = DB.getAgente(criterio, "REPORTES");
                //    foreach (RE_GenericBean dataA in datos_agente)
                //    {
                //        lb_nombre_agente.Visible = true;
                //        lb_nombre_agente.Text = dataA.strC1;

                //        //lbl_direccion_empresa.Text = "10087 NW 122nd St. Medley, FL 3378";
                        
                //    }
                //}
                //
            }
            else
            {
                if ((user.PaisID == 1 || user.PaisID == 15))
                {
                    lb_nombre_agente.Visible = true;
                    lb_nombre_agente.Text = "";
                    lbl_direccion_empresa.Text = "10087 NW 122nd St. Medley, FL 3378";
                }
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
                                                + "<td width='100px'>";
                if (factdata.intC4 == 8)
                {
                    html_bl += "<b>Packages</b></td>";
                }
                else
                {
                    html_bl += "<b>Paquetes</b></td>";
                }
                html_bl += "<td colspan='5' width='100px;'>"
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
                    html_bl += "<td width='100px'>";
                    if (factdata.intC4 == 8)
                    {
                        html_bl += "<b>Weight</b></td>";
                    }
                    else
                    {
                        html_bl += "<b>Peso</b></td>";
                    }
                    html_bl += "<td width='100px;'>"
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
                    html_bl += "<td width='100px'>";
                    if (factdata.intC4 == 8)
                    {
                        html_bl += "<b>Bulk</b></td>";
                    }
                    else
                    {
                        html_bl += "<b>Volumen</b></td>";
                    }
                    html_bl += "<td width='100px'>"
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
            ///Validar tipo de operacion y colocar 
            if (((factdata.strC10 != null) && (factdata.strC10 != "")) || ((factdata.strC9 != null) && (factdata.strC9 != "")))
            {
                string master_document = "";
                string child_document = "";
                if ((factdata.Tipo_Operacion == 5) || (factdata.Tipo_Operacion == 6) || (factdata.Tipo_Operacion == 7))
                {
                    master_document = "CPM";
                    child_document = "CPH";
                }
                else
                {
                    master_document = "MBL";
                    child_document = "HBL";
                }

                ban_BL++;
                html_bl += "<tr>";
                if ((factdata.strC10 != null) && (factdata.strC10 != ""))
                {
                    html_bl += "<td width='100px'>"
                        + "<b>" + master_document + "</b></td>"
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
                        + "<b>" + child_document + "</b></td>"
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
                    html_bl += "<td width='100px'>";
                    if (factdata.intC4 == 8)
                    {
                        html_bl += "<b>Container</b></td>";
                    }
                    else
                    {
                        html_bl += "<b>Contenedor</b></td>";
                    }
                    html_bl += "<td width='100px'>"
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
                if ((factdata.Tipo_Operacion != 5) && (factdata.Tipo_Operacion != 6) && (factdata.Tipo_Operacion != 7))
                {
                    ban_BL++;
                    html_bl += "<tr>";


                    if ((factdata.strC13 != null) && (factdata.strC13 != ""))//Cambio para No Mostrar la Naviera en Recordatorios en Guatemala
                    {
                        if ((user.PaisID == 1) && (user.contaID == 1) && (tipo_transaccion == 4))
                        {
                            //Cambio para No Mostrar la Naviera en Recordatorios en Guatemala, Solicitado en Proyecto Fiscal USD
                            html_bl += "<td width='100px'>"
                            + "</td>"
                            + "<td width='100px;'>"
                            + "</td>";
                        }
                        else
                        {
                            html_bl += "<td width='100px'>";
                            if (factdata.intC4 == 8)
                            {
                                html_bl += "<b>Shipping</b></td>";
                            }
                            else
                            {
                                html_bl += "<b>Naviera</b></td>";
                            }
                            html_bl += "<td width='100px;'>"
                                + factdata.strC13
                            + "</td>";
                        }
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
                        html_bl += "<td width='100px'>";
                        if (factdata.intC4 == 8)
                        {
                            html_bl += "<b>Steam</b></td>";
                        }
                        else
                        {
                            html_bl += "<b>Vapor</b></td>";
                        }
                        html_bl += "<td width='100px'>"
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
                ////
            }
            if (((factdata.strC22 != null) && (factdata.strC22 != "")) || ((factdata.strC23 != null) && (factdata.strC23 != "")))
            {
                ban_BL++;
                html_bl += "<tr>";
                if ((factdata.strC22 != null) && (factdata.strC22 != ""))
                {
                    html_bl += "<td width='100px'>";
                    if (factdata.intC4 == 8)
                    {
                        html_bl += "<b>Inconming Dua</b></td>";
                    }
                    else
                    {
                        html_bl += "<b>Dua Ingreso</b></td>";
                    }

                    html_bl += "<td width='100px;'>"
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
                    html_bl += "<td width='100px'>";
                    if (factdata.intC4 == 8)
                    {
                        html_bl += "<b>Outgoing Dua</b></td>";
                    }
                    else
                    {
                        html_bl += "<b>Dua Salida</b></td>";
                    }
                    html_bl += "<td width='100px'>"
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
                    html_bl += "<td width='100px'>";
                    if (factdata.intC4 == 8)
                    {
                        html_bl += "<b>Customs Receipt</b></td>";
                    }
                    else
                    {
                        html_bl += "<b>Recibo Aduanal</b></td>";
                    }
                    html_bl += "<td width='100px;'>"
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
                    html_bl += "<td width='100px'>";
                    if (factdata.intC4 == 8)
                    {
                        html_bl += "<b>Customs Policy</b></td>";
                    }
                    else
                    {
                        html_bl += "<b>Poliza Aduanal</b></td>";
                    }
                    html_bl += "<td width='100px'>"
                        + factdata.strC16
                    + "</td>";
                    html_bl += "<td width='100px'>";
                    if (factdata.intC4 == 8)
                    {
                        html_bl += "<b>Regime</b></td>";
                    }
                    else
                    {
                        html_bl += "<b>Regimen</b></td>";
                    }
                    html_bl += "<td width='100px'>"
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
            if (user.PaisID == 11)
            {

                dt = (DataTable)DB.getRubbyFact_For_Print(ID, tipo_transaccion, user);
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
                dt = (DataTable)DB.getRubbyFact(ID, Tipo_Transaccion);
                if ((dt != null) && (factdata.strC30 == ""))
                {
                        string detalle = "";
                        string value = "";
                        string impuesto = "";

                        if (factdata.intC4 == 8)
                        {
                            detalle = "Detail";
                            value = "Value";
                            impuesto = "Tax";
                        }
                        else
                        {
                            detalle = "Detalle";
                            value = "Valor";
                            impuesto = "Impuesto";
                        }
                        html_Rubros = "<table align='center' cellpadding='0' cellspacing='3' class='style3'>";
                        html_Rubros += "<tr>"
                                + "<td width='300px' class='style6'>"
                                    + "<b><u>" + detalle + "</u></b>"
                                + "</td>"
                                + "<td width='200px' align='right' class='style6'>"
                                    + "<b><u>" + impuesto + "</u></b>"
                                + "</td>"
                                + "<td width='200px' align='right' class='style6'>"
                                    + "<b><u>" + value + "</u></b>"
                                + "</td>"
                            + "</tr>";
                        foreach (DataRow dr in dt.Rows)
                        {
                            html_Rubros += "<tr>"
                                + "<td width='300px'>"
                                    + dr[1].ToString()
                                + "</td>"
                                + "<td width='200px' align='right'>"
                                    + decimal.Parse(dr[6].ToString()).ToString("#,#.00")
                                + "</td>"
                                + "<td width='200px' align='right'>"
                                    + decimal.Parse(dr[5].ToString()).ToString("#,#.00")
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
                    
                }//arriba
                else
                {
                    string detalle = "";
                    string value = "";
                    string impuesto = "";
                    if (factdata.intC4 == 8)
                    {
                        detalle = "Detail";
                        value = "Value";
                        impuesto = "Tax";
                    }
                    else
                    {
                        detalle = "Detalle";
                        value = "Valor";
                        impuesto = "Impuesto";
                    }
                    html_Rubros = "<table align='center' cellpadding='0' cellspacing='3' class='style3'>";
                    html_Rubros += "<tr>"
                            + "<td width='300px' class='style6'>"
                                + "<b><u>" + detalle + "</u></b>"
                            + "</td>"
                            + "<td width='200px' align='right' class='style6'>"
                                + "<b><u>" + impuesto + "</u></b>"
                            + "</td>"
                            + "<td width='200px' align='right' class='style6'>"
                                + "<b><u>" + value + "</u></b>"
                            + "</td>"
                        + "</tr>";


                    html_Rubros += "<tr>"
                                + "<td width='300px'>"
                                    + factdata.strC30
                                + "</td>"
                                + "<td width='200px' align='right'>"
                                    + factdata.decC2.ToString("#,#.00")
                                + "</td>"
                                + "<td width='200px' align='right'>"
                                    + factdata.decC1.ToString("#,#.00")
                                + "</td>"
                            + "</tr>";


                    html_Rubros += "</table>";

                }
            }
            #endregion
            if (user.PaisID == 11)
            {
                #region GRH
                lbl_total.Text = factdata.decC3.ToString("#,#.00");
                lbl_total_equivalente.Text = factdata.decC4.ToString("#,#.00");
                lbl_subtotal.Text = factdata.decC1.ToString("#,#.00");
                lbl_impuesto.Text = factdata.decC2.ToString("#,#.00");
                #endregion
            }
            else
            {
                #region Todas las Empresas excepto GRH
                string strMoneda = "";
                string strMonedaEquivalente = "";
                if (user.PaisID == 1)
                {
                    if (factdata.intC4 == 1)
                    {
                        strMoneda = "<b>Q.</b>";
                        strMonedaEquivalente = "<b>USD</b>";
                    }
                    if (factdata.intC4 == 8)
                    {
                        strMoneda = "<b>USD</b>";

                        strMonedaEquivalente = "<b>Q.</b>";
                        lbl_equivalente.Visible = false;
                        lbl_total_equivalente.Visible = false;
                    }
                }

                lbl_total.Text = strMoneda + " " + factdata.decC3.ToString("#,#.00");
                lbl_total_equivalente.Text = strMonedaEquivalente + " " + factdata.decC4.ToString("#,#.00");
                Conv c = new Conv();
                if (user.PaisID == 1)
                {
                    if (factdata.intC4 == 8)
                    {
                        lbl_total_letras.Text = "TOTAL IN LETTERS.:    " + c.enletras(factdata.decC3.ToString(), 8);
                    }
                    else 
                    {
                        lbl_total_letras.Text = "TOTAL EN LETRAS.:    " + c.enletras(factdata.decC3.ToString(), 1);
                    }
                }
                else
                {
                    if ((user.PaisID == 3) && (user.contaID == 1) && (user.SucursalID == 102))
                    {
                        string tipo_moneda = "";
                        if (factdata.intC4 == 8)
                        {
                            tipo_moneda = " Dolares";
                        }
                        else
                        {
                            tipo_moneda = " Lempiras";
                        }

                        lbl_total_letras.Text = "TOTAL EN LETRAS.:    " + c.enletras(factdata.decC3.ToString(), 1) + tipo_moneda;
                    }
                    else
                    {
                        lbl_total_letras.Text = "TOTAL EN LETRAS.:    " + c.enletras(factdata.decC3.ToString(), 1);
                    }
                }
                lbl_subtotal.Text = factdata.decC1.ToString("#,#.00");
                lbl_impuesto.Text = factdata.decC2.ToString("#,#.00");
                if (user.PaisID == 5)
                {
                    if ((lbl_serie.Text == "NDFA") || (lbl_serie.Text == "F1"))
                    {
                        decimal CR_Total_Anticipos = 0;
                        decimal CR_Total = 0;
                        decimal CR_Saldo_Pagar = 0;
                        CR_Total_Anticipos = DB.getTotalAbonadoNotaDebito_For_Print(ID);
                        lbl_anticipos.Text = (CR_Total_Anticipos).ToString("#,#.00");
                        CR_Total = factdata.decC3;
                        CR_Saldo_Pagar = CR_Total - CR_Total_Anticipos;
                        lbl_saldo_pagar.Text = (CR_Saldo_Pagar).ToString("#,#.00");
                        lbl_anticipos.Visible = true;
                        lb_anticipos.Visible = true;
                        lb_saldo_pagar.Visible = true;
                        lbl_saldo_pagar.Visible = true;
                        lb_total.Text = "Total General";
                        lb_saldo_pagar.Text = "Total a Pagar";
                    }


                    //Validar pais para mostrar iva y anticipos y total
                    if (tipo_transaccion == 1)
                    {
                        decimal CR_Total_Anticipos = 0;
                        decimal CR_Total = 0;
                        decimal CR_Saldo_Pagar = 0;
                        CR_Total_Anticipos = DB.getAbonosByttr_id(ID, 2, 1);//DB.getTotalAbonadoNotaDebito_For_Print(ID);
                        lbl_anticipos.Text = (CR_Total_Anticipos).ToString("#,#.00");
                        CR_Total = factdata.decC3;
                        CR_Saldo_Pagar = CR_Total - CR_Total_Anticipos;
                        lbl_saldo_pagar.Text = (CR_Saldo_Pagar).ToString("#,#.00");
                        lbl_anticipos.Visible = true;
                        lb_anticipos.Visible = true;
                        lb_saldo_pagar.Visible = true;
                        lbl_saldo_pagar.Visible = true;
                        lb_impuesto.Text = "IVA";
                        lbl_tc.Visible = false;
                        lbl_equivalente.Visible = false;
                        lbl_total_equivalente.Visible = false;
                    }
                }
                //Separar Notas de Debito a Agentes de Honduras
                if (tipo_transaccion == 4)
                {
                    if ((user.PaisID == 3 || user.PaisID == 23) && user.contaID == 2)
                    {
                        if (lbl_serie.Text == "NDPD")
                        {
                            Image1.Visible = true;
                            Image1.ImageUrl = "~/img/aimar_en.jpg";
                            lb_nombre_agente.Visible = false;
                            lb_nombre_agente.Text = "";
                            lbl_direccion_empresa.Text = user.pais.Direccion_Empresa;
                        }
                    }
                }
                //fin
                #endregion
            }
            if (factdata.intC8 == 9 || factdata.intC8 == 2) // muestra retencion si el pais es el salvador2 o el salvador
            {
                lbl_retencion.Visible = true;
                lbl_Titulo_ret.Visible = true;
                decimal retencion = 0;
                retencion = DB.getRetencioByFactID(factdata.intC1);
                lbl_retencion.Text = retencion.ToString("#,#.00");
                if (retencion == 0)
                {
                    lbl_retencion.Visible = false;
                    lbl_Titulo_ret.Visible = false;
                }
            }


            ///////Colocamos direccion del agente
            string criterio = "agente = '" + factdata.strC41 + "'";

            ArrayList datos_agente = new ArrayList();
            datos_agente = DB.getAgente(criterio, "REPORTES");
            foreach (RE_GenericBean dataA in datos_agente)
            {
                lb_nombre_agente.Visible = true;
                lb_nombre_agente.Text = dataA.strC1;

                if (dataA.strC2 == "")
                {
                    lbl_direccion_empresa.Text = "";
                }
                else
                {
                    lbl_direccion_empresa.Text = dataA.strC2;
                }
            }
            ////////////////////////////////////////

        }


 
    }
    public RE_GenericBean Ordenar_Arrelgo(RE_GenericBean Arreglo, int Tipo_Transaccion)
    {
        RE_GenericBean result = new RE_GenericBean();
        try
        {
            if (Tipo_Transaccion == 14)//Proforma
            {
                result.strC28 = Arreglo.strC28;//Serie
                result.strC1 = Arreglo.strC1;//Correlativo
                result.strC5 = Arreglo.strC5;//Fecha de Emision
                result.strC6 = Arreglo.strC6;//Fecha de Pago
                result.intC3 = Arreglo.intC3;//Codigo de Cliente
                result.strC3 = Arreglo.strC3;//Nombre del Cliente
                result.strC2 = Arreglo.strC2;//Nit
                result.strC26 = Arreglo.strC26;//Razon
                result.intC4 = Arreglo.intC4;//Moneda
                result.strC4 = Arreglo.strC4;//Direccion
                result.strC7 = Arreglo.strC7;//Observaciones
                result.strC41 = Arreglo.strC41;//Agente
                result.strC15 = Arreglo.strC15;//Shipper
                result.strC17 = Arreglo.strC17;//Consignee
                result.strC18 = Arreglo.strC18;//Comodity
                result.strC19 = Arreglo.strC32;//Cantidad de Paquetes
                result.strC32 = Arreglo.strC19;//Nombre del Paquete
                result.strC20 = Arreglo.strC20;//Peso
                result.strC21 = Arreglo.strC21;//Volumen
                result.strC10 = Arreglo.strC10;//MBL
                result.strC9 = Arreglo.strC9;//HBL
                result.strC12 = Arreglo.strC12;//Routing
                result.strC11 = Arreglo.strC11;//Contenedor
                result.strC13 = Arreglo.strC13;//Naviera
                result.strC14 = Arreglo.strC14;//Vapor
                result.strC22 = Arreglo.strC22;//Dua Ingreso
                result.strC23 = Arreglo.strC23;//Dua Salida
                result.strC31 = Arreglo.strC31;//Recibo Aduanal
                result.strC16 = Arreglo.strC16;//Poliza Aduanal
                result.decC1 = Arreglo.decC1;//Subtotal
                result.decC2 = Arreglo.decC2;//Impuesto
                result.decC3 = Arreglo.decC3;//Total
                result.decC4 = Arreglo.decC4;//Total Equivalente
                result.decC5 = Arreglo.decC5;//Impuesto Equivalente
                result.decC6 = Arreglo.decC6;//Subtotal Equivalente
            }
            else if (Tipo_Transaccion == 4)//Nota de Debito
            {
                result.strC28 = Arreglo.strC28;//Serie
                result.strC1 = Arreglo.intC6.ToString();//Correlativo
                result.strC5 = Arreglo.strC3;//Fecha de Emision
                result.strC6 = Arreglo.strC45;//Fecha de Pago
                result.intC3 = Convert.ToInt32(Arreglo.douC1);//Codigo de Cliente
                result.strC3 = Arreglo.strC2;//Nombre del Cliente
                result.strC2 = Arreglo.strC1;//Nit
                result.strC26 = Arreglo.strC34;//Razon
                result.intC4 = Arreglo.intC4;//Moneda
                result.strC4 = Arreglo.strC6;//Direccion
                result.strC7 = Arreglo.strC4;//Observaciones
                result.strC41 = Arreglo.strC33;//Agente
                result.strC15 = Arreglo.strC15;//Shipper
                result.strC17 = Arreglo.strC17;//Consignee
                result.strC18 = Arreglo.strC18;//Comodity
                result.strC19 = Arreglo.strC32;//Cantidad de Paquetes
                result.strC32 = Arreglo.strC19;//Nombre del Paquete
                result.strC20 = Arreglo.strC20;//Peso
                result.strC21 = Arreglo.strC21;//Volumen
                result.strC10 = Arreglo.strC8;//MBL
                result.strC9 = Arreglo.strC7;//HBL
                result.strC12 = Arreglo.strC12;//Routing
                result.strC11 = Arreglo.strC9;//Contenedor
                result.strC13 = Arreglo.strC13;//Naviera
                result.strC14 = Arreglo.strC14;//Vapor
                result.strC22 = Arreglo.strC22;//Dua Ingreso
                result.strC23 = Arreglo.strC23;//Dua Salida
                result.strC31 = Arreglo.strC31;//Recibo Aduanal
                result.strC16 = Arreglo.strC16;//Poliza Aduanal
                result.decC1 = Arreglo.decC2;//Subtotal
                result.decC2 = Arreglo.decC3;//Impuesto
                result.decC3 = Arreglo.decC1;//Total
                result.decC4 = Arreglo.decC4;//Total Equivalente
                result.decC5 = Arreglo.decC5;//Impuesto Equivalente
                result.decC6 = Arreglo.decC6;//Subtotal Equivalente
                result.Tipo_Operacion = Arreglo.Tipo_Operacion;
                //result.Tipo_Contribuyente = Arreglo.Tipo_Operacion;
            }
            else if ((Tipo_Transaccion == 3) || (Tipo_Transaccion == 18))
            {
                result.strC28 = Arreglo.strC32;//Serie
                result.strC1 = Arreglo.strC33;//Correlativo
                result.strC5 = Arreglo.strC5;//Fecha Emision
                result.strC6 = "";//Fecha de Pago
                result.intC3 = Arreglo.intC3;//Codigo Cliente
                result.strC3 = Arreglo.strC3;//Nombre del Cliente
                result.strC2 = Arreglo.strC2;//Nit
                result.strC26 = Arreglo.strC26;//Razon
                result.intC4 = Arreglo.intC4;//Moneda
                result.strC4 = Arreglo.strC4;//Direccion
                result.strC7 = Arreglo.strC7;//Observaciones
                result.strC15 = Arreglo.strC15;//Shipper
                result.strC17 = Arreglo.strC17;//Consignee
                result.strC18 = Arreglo.strC18;//Comodity
                //if (user.Operacion == 1)
                //{
                //    result.strC19 = Arreglo.strC32;//Cantidad de Paquetes
                //}
                //else
                //{
                result.strC19 = "";//Cantidad de Paquetes
                //}
                result.strC32 = Arreglo.strC19;//Nombre del Paquete
                result.strC20 = Arreglo.strC20;//Peso
                result.strC21 = Arreglo.strC21;//Volumen
                result.strC10 = Arreglo.strC10;//MBL
                result.strC9 = Arreglo.strC9;//HBL
                result.strC12 = Arreglo.strC12;//Routing
                result.strC11 = Arreglo.strC11;//Contenedor
                result.strC13 = Arreglo.strC13;//Naviera
                result.strC14 = Arreglo.strC14;//Vapor
                result.strC22 = Arreglo.strC22;//Dua Ingreso
                result.strC23 = Arreglo.strC23;//Dua Salida
                result.strC16 = Arreglo.strC34;//Poliza Aduanal
                result.decC1 = Arreglo.decC4;//Subtotal
                result.decC2 = Arreglo.decC5;//Impuesto
                result.decC3 = Arreglo.decC3;//Total
                result.decC4 = Arreglo.decC6;//Total Equivalente
                result.strC40 = Arreglo.strC28 + " - " + Arreglo.strC1;//Serie, Correlativo de la Factura
                if (tipo_transaccion == 3)
                {
                    Arreglo.decC9 = DB.Convertir_ATipo_Cambio_Libro_Diario(ID, 3, user, Arreglo.decC5);//Impuesto Equivalente
                    Arreglo.decC10 = DB.Convertir_ATipo_Cambio_Libro_Diario(ID, 3, user, Arreglo.decC4);//Subtotal Equivalente
                }
                else if (tipo_transaccion == 18)
                {
                    Arreglo.decC9 = DB.Convertir_ATipo_Cambio_Libro_Diario(ID, 18, user, Arreglo.decC5);//Impuesto Equivalente
                    Arreglo.decC10 = DB.Convertir_ATipo_Cambio_Libro_Diario(ID, 18, user, Arreglo.decC4);//Subtotal Equivalente
                }
                result.decC5 = Arreglo.decC9;//Impuesto Equivalente
                result.decC6 = Arreglo.decC10;//Subtotal Equivalente
            }
        }
        catch (Exception e)
        {
            log4net ErrLog = new log4net();
            ErrLog.ErrorLog(e.Message);
            return null;
        }
        return result;
    }
}