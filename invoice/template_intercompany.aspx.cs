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

public partial class invoice_template_intercompany : System.Web.UI.Page
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
    

    DataTable dt = null;
    public UsuarioBean user;
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


        //PIE DE DOCUMENTO HN
        if ((user.PaisID == 3 || user.PaisID == 23) && user.contaID == 2)
        {
            lb_observaciones_hn.Visible = true;
            lb_observaciones_hn.Text = "Please note, for the release of your goods and delivery to the original bill of lading and other documentation." +
            " We request the payment described above to our agents in Honduras." +
            " Dollar payments must be by cashier's check, wire transfer  payable to <b>AIMARGROUP</b>" +
            " Will also reiterate our desire to provide good service for import and export air shipments," +
            " Sea, land and Storage, also in carrying out their customs formalities, ground transportation, distribution and" +
            " inventory management, certified inspections of loading and unloading, storage and other services.";
        }
        //FIN PIE DE DOCUMENTO HN

        lbl_direccion_empresa.Text = user.pais.Direccion_Empresa;
        if ((Request.QueryString["id"] != null) && (Request.QueryString["transaccion"] != null))
        {
            ID = int.Parse(Request.QueryString["id"].ToString());
            tipo_transaccion = int.Parse(Request.QueryString["transaccion"].ToString());




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
                    lbl_tipo_transaccion.Text = "RECIBO";
                }
                lb_fecha_emision.Text = reg.strC7; //fecha emision
                lbl_vencimiento.Text = reg.strC8; //fecha vencimiento
                lb_codigo.Text = reg.strC9; //codigo
                lb_nombre.Text = reg.strC11; //nombre
                if (user.PaisID == 3 || user.PaisID == 23)
                {
                    lb_nit.Text = "R.T.N.";
                }
                else
                {
                    lb_nit.Text = reg.strC12; //nit
                }
                lb_razon.Text = reg.strC13; //razon
                lb_direccion.Text = reg.strC14; //direccion
                lbl_equivalente.Text = reg.strC21; //equivalente
                lb_impuesto.Text = reg.strC16; //impuesto
                lb_cifras.Text = reg.strC22; //cifras en
                if (user.PaisID == 7 && user.contaID == 2)
                {
                    lbl_equivalente.Text = "Equivalent BZE$"; //equivalente para bz
                }
                if ((user.PaisID == 3 || user.PaisID == 23) && user.contaID == 2)
                {
                    lbl_equivalente.Text = "Equivalent LP"; //equivalente para HN
                }
                if (user.PaisID == 3 || user.PaisID == 23)
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
                if (tipo_transaccion == 2)
                {
                    Datos_Factura = (RE_GenericBean)DB.getRcptData_forprint(ID);
                }
                cargar_Factura(Datos_Factura, ID, tipo_transaccion);
                if (((Datos_Factura.intC8 == 3) || (Datos_Factura.intC8 == 5) || (Datos_Factura.intC8 == 6)) && Datos_Factura.intC4 == 8) // si los paises son hn y cr no se muestra equivalente 
                {
                    lbl_total_equivalente.Visible = false;
                    lbl_equivalente.Visible = false;
                }
                if (Datos_Factura.intC8 == 1)
                {
                    if ((Datos_Factura.strC50 != "-") && (Datos_Factura.strC50 == "1"))
                    {
                        var fel = DB.DataFEL(user.PaisID, Datos_Factura.strC49, Datos_Factura.strC52);
                        lbl_firma_digital.Text = fel.firma;
                        /*
                        lbl_firma_comentario.Text = "Firma Electronica.: ";                        
                        if (DB.isFELDate(user.PaisID)) //2019-07-29
                            lbl_firma_digital.Text = DB.FirmaFEL("4", Datos_Factura.strC49, Datos_Factura.strC52); //, "", "Numero", "");
                        else
                            lbl_firma_digital.Text = Datos_Factura.strC49;*/
                        lbl_firma_comentario.Visible = true;
                        lbl_firma_digital.Visible = true;
                    }
                }
            }
            if (tipo_transaccion == 14)//Proforma
            {
                RE_GenericBean Datos_Factura_Proforma = (RE_GenericBean)DB.getFacturaData_proforma(ID);
                RE_GenericBean Datos_Factura_Proforma_Ordenados = Ordenar_Arrelgo(Datos_Factura_Proforma, tipo_transaccion);
                cargar_Factura(Datos_Factura_Proforma_Ordenados, ID, tipo_transaccion);
            }
            if (tipo_transaccion == 4)//Nota Debito
            {
                //***********traduccion nd
                string path = "nota_debito";
                RE_GenericBean reg = DB.Traducir_reportes(user, path);
                //***titulos traducidos***
                if (user.PaisID == 3 && user.contaID == 1)
                {
                    lbl_tipo_transaccion.Text = "Nota de Debito";
                    //Image1.ImageUrl = "~/img/aimar2.jpg";
                    lb_observaciones_hn.Visible = true;
                    lb_observaciones_hn.Text = "Por instrucciones de nuestros agentes en origen, le recordamos que para la liberación de su mercadería y para la entrega del conocimiento de embarque original y demás documentación.<br>Le solicitamos la cancelación de lo descrito anteriormente. <br> Pagos en LEMPIRAS deben ser con CHEQUE CERTIFICADO a nombre de AIMAR S.A. <br> Pagos en DOLARES deben ser con CHEQUE DE CAJA a nombre de AIMAR S.A. <br> Asimismo le reiteramos nuestro deseo por brindarle un buen servicio en  Importación y Exportación para embarques Aéreos, <br> Marítimos, Terrestres y de Almacenaje, También en la realización de sus trámites de Aduanas, transporte terrestre, Distribución y <br> administración de inventarios, inspecciones certificadas de carga y descarga, Almacenaje   y demás servicios.  ";
                }
                else
                {
                    lbl_tipo_transaccion.Text = reg.strC10; //titulo
                }

                lb_fecha_emision.Text = reg.strC7; //fecha emision
                lbl_vencimiento.Text = reg.strC8; //fecha vencimiento
                lb_codigo.Text = reg.strC9; //codigo
                lb_nombre.Text = reg.strC11; //nombre

                if (user.PaisID == 3 || user.PaisID == 23)
                {
                    lb_nit.Text = "R.T.N.";
                }
                else
                {
                    lb_nit.Text = reg.strC12; //nit
                }
                lb_razon.Text = reg.strC13; //razon
                lb_direccion.Text = reg.strC14; //direccion
                lbl_equivalente.Text = reg.strC21; //equivalente
                lb_impuesto.Text = reg.strC16; //impuesto
                lb_cifras.Text = reg.strC22; //cifras en


                RE_GenericBean Datos_Nota_Debito = (RE_GenericBean)DB.getNotaDebitoData(ID);
                RE_GenericBean Datos_Nota_Debito_Ordenados = Ordenar_Arrelgo(Datos_Nota_Debito, tipo_transaccion);
                cargar_Factura(Datos_Nota_Debito_Ordenados, ID, tipo_transaccion);
                lbl_usuario_crea.Text = Datos_Nota_Debito.strC5.ToString(); // usuario que creo la ND.
                if (((Datos_Nota_Debito.intC2 == 3) || (Datos_Nota_Debito.intC2 == 5)) && Datos_Nota_Debito.intC4 == 8) // si los paises son hn y cr no se muestra equivalente 
                {
                    lbl_total_equivalente.Visible = false;
                    lbl_equivalente.Visible = false;
                }

                if ((user.PaisID == 3 || user.PaisID == 23) && (user.SucursalID == 102 || user.SucursalID == 104) && (user.contaID == 1))
                {
                    lbl_nit_empresa.Text = user.pais.Pai_Numero_Identificacion_Tributaria;
                }
                if (user.PaisID == 4)
                {
                    lbl_nit_empresa.Text = "RUC.: " + user.pais.Pai_Numero_Identificacion_Tributaria;
                }
                else if (user.PaisID == 5)
                {
                    lbl_direccion_empresa.Text = user.pais.Direccion_Empresa;
                    lbl_nit_empresa.Text = user.pais.Pai_Numero_Identificacion_Tributaria;
                }
                if (user.PaisID == 1)
                {
                    if (user.contaID == 1)
                    {
                        lbl_direccion_empresa.Text = user.pais.Direccion_Empresa;
                        lbl_total_equivalente.Visible = false;
                        lbl_equivalente.Visible = false;
                        lbl_formas_pago.Visible = true;
                        //lbl_formas_pago.Text = "Cobro por cuenta Ajena";//Se comenta para agregar comentarios Fiscal USD solicitados por Esdras Mendez
                        lbl_formas_pago.Text = "El soporte de estos Cargos se encuentra Detallado en el pago de Impuestos amparado en la <strong>Poliza de Importacion.</strong><br>Forma de pago:   CHEQUE DE CAJA A NOMBRE DE AIMAR, S.A.<br>Gastos administrativos por cheque rechazado $25.00";
                        if (Datos_Nota_Debito.intC4 == 8)
                        {
                            //lbl_tipo_transaccion.Text = "Recordatorio de Pago";
                            lbl_formas_pago.Text += "<br>Exenta de IVA";
                        }
                        else
                        {
                            //lbl_tipo_transaccion.Text = "Recordatorio de Pago";
                            lbl_formas_pago.Text += "<br>Exenta de IVA";
                        }
                    }
                    else if (user.contaID == 2)
                    {
                        lbl_direccion_empresa.Text = "10087 NW 122nd St Medley, FL 3378";
                        lbl_total_equivalente.Visible = false;
                        lbl_equivalente.Visible = false;
                        lbl_formas_pago.Visible = true;
                        lbl_formas_pago.Text = "Cobro por cuenta Ajena <br>" + user.pais.Formas_Pago;

                        #region Cambio de Informacion por Cobro de DEMORAS APL
                        if ((user.SucursalID == 9) && ((lbl_serie.Text == "NDDAPLI") || (lbl_serie.Text == "NDDAPLE")))
                        {
                            lbl_direccion_empresa.Text = "";
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
                if (user.PaisID == 6)
                {
                    lbl_direccion_empresa.Visible = true;
                    lbl_direccion_empresa.Text = user.pais.Pai_Numero_Identificacion_Tributaria;
                    lbl_direccion_empresa2.Visible = true;
                    lbl_direccion_empresa2.Text = user.pais.Direccion_Empresa;
                }
            }
            if ((tipo_transaccion == 3) || (tipo_transaccion == 18))//Nota Credito o NC Ajuste 
            {
                //***********traduccion nd
                string path = "nota_credito";
                RE_GenericBean reg = DB.Traducir_reportes(user, path);
                //***titulos traducidos***
                lbl_tipo_transaccion.Text = reg.strC10; //titulo
                lb_fecha_emision.Text = reg.strC7; //fecha emision
                lbl_vencimiento.Text = reg.strC8; //fecha vencimiento
                lb_codigo.Text = reg.strC9; //codigo
                lb_nombre.Text = reg.strC11; //nombre
                if (user.PaisID == 3 || user.PaisID == 23)
                {
                    lb_nit.Text = "R.T.N.";
                }
                else
                {
                    lb_nit.Text = reg.strC12; //nit
                }
                lb_razon.Text = reg.strC13; //razon
                lb_direccion.Text = reg.strC14; //direccion
                lbl_equivalente.Text = reg.strC21; //equivalente
                lb_impuesto.Text = reg.strC16; //impuesto
                lb_cifras.Text = reg.strC22; //cifras en
                RE_GenericBean Datos_Nota_Credito;
                if (user.Operacion == 2)
                {
                    Datos_Nota_Credito = (RE_GenericBean)DB.getNotaCreditoAgenteData(ID);
                }
                else
                {
                    Datos_Nota_Credito = (RE_GenericBean)DB.getNotaCreditoData(ID);
                }
                RE_GenericBean Datos_Nota_Credito_Ordenados = Ordenar_Arrelgo(Datos_Nota_Credito, tipo_transaccion);
                cargar_Factura(Datos_Nota_Credito_Ordenados, ID, tipo_transaccion);
                if ((user.PaisID == 3 || user.PaisID == 23) && (user.SucursalID == 102 || user.SucursalID == 104) && (user.contaID == 1))
                {

                    lbl_nit_empresa.Text = user.pais.Pai_Numero_Identificacion_Tributaria;
                }

                if (user.PaisID == 5)
                {
                    lbl_direccion_empresa.Text = user.pais.Direccion_Empresa;
                    lbl_nit_empresa.Text = user.pais.Pai_Numero_Identificacion_Tributaria;
                }
                if (user.PaisID == 1)
                {
                    if (Datos_Nota_Credito.intC8 == 1)
                    {
                        if ((Datos_Nota_Credito.strC50 != "-") && (Datos_Nota_Credito.strC50 == "1"))
                        {
                            var fel = DB.DataFEL(user.PaisID, Datos_Nota_Credito.strC49, Datos_Nota_Credito.strC52);
                            lbl_firma_digital.Text = fel.firma;
                            /*
                            lbl_firma_comentario.Text = "Firma Electronica.: ";
                            if (DB.isFELDate(user.PaisID)) //2019-07-29
                                lbl_firma_digital.Text = DB.FirmaFEL("4", Datos_Nota_Credito.strC49, Datos_Nota_Credito.strC52); //, "", "Numero", "");
                            else
                                lbl_firma_digital.Text = Datos_Nota_Credito.strC49;*/
                            lbl_firma_comentario.Visible = true;
                            lbl_firma_digital.Visible = true;
                        }
                    }
                    if (user.contaID == 1)
                    {
                        lbl_direccion_empresa.Text = user.pais.Direccion_Empresa;
                        lbl_total_equivalente.Visible = true;
                        lbl_equivalente.Visible = true;
                    }
                    else if (user.contaID == 2)
                    {
                        lbl_direccion_empresa.Text = "10087 NW 122nd St Medley, FL 3378";
                        lbl_total_equivalente.Visible = false;
                        lbl_equivalente.Visible = false;
                        #region Cambio de Informacion por Cobro de DEMORAS APL
                        if ((user.SucursalID == 9) && ((Datos_Nota_Credito.strC32 == "NC-DI") || (Datos_Nota_Credito.strC32 == "NC-DE")))
                        {
                            lbl_direccion_empresa.Text = "";
                            lbl_direccion_empresa2.Text = "";
                            lbl_nit_empresa.Text = "";
                            lbl_formas_pago.Text = "*Por favor emitir Giro Bancario a nombre de AMERICAN PRESIDENT LINES";
                            lbl_formas_pago.Visible = true;
                            lbl_tipo_transaccion.Text = "Credit Note";
                            Image1.ImageUrl = "~/img/apl.png";
                            Image1.Width = 150;
                        }

                        #endregion
                    }
                }
                if (user.PaisID == 6)
                {
                    lbl_direccion_empresa.Visible = true;
                    lbl_direccion_empresa.Text = user.pais.Pai_Numero_Identificacion_Tributaria;
                    lbl_direccion_empresa2.Visible = true;
                    lbl_direccion_empresa2.Text = user.pais.Direccion_Empresa;
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
                if (user.PaisID == 7)
                {
                    lbl_tipo_transaccion.Text = "Invoice";
                }
                else if ((user.PaisID == 3 || user.PaisID == 23) && user.contaID == 2)
                {
                    lbl_tipo_transaccion.Text = "Invoice";
                }
                else if (user.PaisID == 1)
                {
                    if (user.contaID == 1)
                    {
                        lbl_tipo_transaccion.Text = "Factura";
                        if (factdata.intC4 == 8)
                        {
                            //Fiscal USD
                            lbl_tipo_transaccion.Text = "Recordatorio de Pago";
                            texto_recordatorio_pago = "</br>Agencia Internacional Marítima, S.A. (Aimar) no puede emitir una Factura, Invoice o Nota de Debito por los fletes marítimos, aéreos o terrestres que son generados y operados por nuestros agentes en el extranjero según la ley del IVA Articulo 3 donde ampara que el hecho generador de IVA es la prestación de servicios en el territorio nacional.</br></br>"
                                                      + "Dichos servicios son operados en el extranjero por nuestros agentes que no están domiciliadas en territorio nacional a los cuales solo servimos de intermediarios, el Conocimiento de embarque (Bill of Lading, Airway Bill y Carta de Porte) es el documento de soporte para sus gastos de flete que  está regulado por el Convenio de Bruselas de 1924, que ha sido modificado por las Reglas de La Haya-Visby de 1968 y, más recientemente por las Reglas de Hamburgo de 1978, éstas últimas elaboradas por UNCITRAL. Al cual debe de emitirse una retención del 5% al agente de carga según la Ley del Impuesto Sobre La Renta articulo 104 apéndice 1.</br>";
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
                            lbl_direccion_empresa.Text = "";
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
                            lbl_tipo_transaccion.Text = "Recordatorio de Pago";
                            texto_recordatorio_pago = "</br>Agencia Internacional Marítima, S.A. (Aimar) no puede emitir una Factura, Invoice o Nota de Debito por los fletes marítimos, aéreos o terrestres que son generados y operados por nuestros agentes en el extranjero según la ley del IVA Articulo 3 donde ampara que el hecho generador de IVA es la prestación de servicios en el territorio nacional.</br></br>"
                                                      + "Dichos servicios son operados en el extranjero por nuestros agentes que no están domiciliadas en territorio nacional a los cuales solo servimos de intermediarios, el Conocimiento de embarque (Bill of Lading, Airway Bill y Carta de Porte) es el documento de soporte para sus gastos de flete que  está regulado por el Convenio de Bruselas de 1924, que ha sido modificado por las Reglas de La Haya-Visby de 1968 y, más recientemente por las Reglas de Hamburgo de 1978, éstas últimas elaboradas por UNCITRAL. Al cual debe de emitirse una retención del 5% al agente de carga según la Ley del Impuesto Sobre La Renta articulo 104 apéndice 1.</br>";
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
            else if (tipo_transaccion == 3)
            {
                if (user.PaisID == 7)
                {
                    lbl_tipo_transaccion.Text = "Credit Note";
                }
                else if ((user.PaisID == 3 || user.PaisID == 23) && user.contaID == 2)
                {
                    lbl_tipo_transaccion.Text = "Credit Note";
                }
                else if (user.PaisID == 1)
                {
                    if (user.contaID == 1)
                    {
                        lbl_tipo_transaccion.Text = "Nota Credito";
                    }
                    else if (user.contaID == 2)
                    {
                        lbl_tipo_transaccion.Text = "Credit Note";
                        #region Cambio de Informacion por Cobro de DEMORAS APL
                        if ((user.SucursalID == 9) && ((factdata.strC28 == "NC-DI") || (factdata.strC28 == "NC-DE")))
                        {
                            lbl_direccion_empresa.Text = "";
                            lbl_direccion_empresa2.Text = "";
                            lbl_nit_empresa.Text = "";
                            lbl_formas_pago.Text = "*Por favor emitir Giro Bancario a nombre de AMERICAN PRESIDENT LINES";
                            lbl_tipo_transaccion.Text = "Credit Note";
                            Image1.ImageUrl = "~/img/apl.png";
                        }
                        #endregion
                    }
                }
                else
                {
                    lbl_tipo_transaccion.Text = "Nota Credito";
                }
            }
            else if (tipo_transaccion == 4)
            {
                if (user.PaisID == 7)
                {
                    lbl_tipo_transaccion.Text = "Debit Note";
                }
                else if ((user.PaisID == 3 || user.PaisID == 23) && user.contaID == 2)
                {
                    lbl_tipo_transaccion.Text = "Debit Note";
                }
                else if (user.PaisID == 1)
                {
                    if (user.contaID == 1)
                    {
                        if (factdata.intC4 == 8)
                        {
                            lbl_tipo_transaccion.Text = "Nota de Debito";
                            texto_recordatorio_pago = "</br>Agencia Internacional Marítima, S.A. (Aimar) no puede emitir una Factura, Invoice o Nota de Debito por los fletes marítimos, aéreos o terrestres que son generados y operados por nuestros agentes en el extranjero según la ley del IVA Articulo 3 donde ampara que el hecho generador de IVA es la prestación de servicios en el territorio nacional.</br></br>"
                                                          + "Dichos servicios son operados en el extranjero por nuestros agentes que no están domiciliadas en territorio nacional a los cuales solo servimos de intermediarios, el Conocimiento de embarque (Bill of Lading, Airway Bill y Carta de Porte) es el documento de soporte para sus gastos de flete que  está regulado por el Convenio de Bruselas de 1924, que ha sido modificado por las Reglas de La Haya-Visby de 1968 y, más recientemente por las Reglas de Hamburgo de 1978, éstas últimas elaboradas por UNCITRAL. Al cual debe de emitirse una retención del 5% al agente de carga según la Ley del Impuesto Sobre La Renta articulo 104 apéndice 1.</br>";
                        }
                        else
                        {
                            lbl_tipo_transaccion.Text = "Nota de Debito";
                            texto_recordatorio_pago = "";
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
                            lbl_tipo_transaccion.Text = "Nota de Debito";
                            texto_recordatorio_pago = "</br>Agencia Internacional Marítima, S.A. (Aimar) no puede emitir una Factura, Invoice o Nota de Debito por los fletes marítimos, aéreos o terrestres que son generados y operados por nuestros agentes en el extranjero según la ley del IVA Articulo 3 donde ampara que el hecho generador de IVA es la prestación de servicios en el territorio nacional.</br></br>"
                                                          + "Dichos servicios son operados en el extranjero por nuestros agentes que no están domiciliadas en territorio nacional a los cuales solo servimos de intermediarios, el Conocimiento de embarque (Bill of Lading, Airway Bill y Carta de Porte) es el documento de soporte para sus gastos de flete que  está regulado por el Convenio de Bruselas de 1924, que ha sido modificado por las Reglas de La Haya-Visby de 1968 y, más recientemente por las Reglas de Hamburgo de 1978, éstas últimas elaboradas por UNCITRAL. Al cual debe de emitirse una retención del 5% al agente de carga según la Ley del Impuesto Sobre La Renta articulo 104 apéndice 1.</br>";
                        }
                        else
                        {
                            lbl_tipo_transaccion.Text = "Nota de Debito";
                            texto_recordatorio_pago = "";
                        }
                    }
                    else if (user.contaID == 2)
                    {
                        lbl_tipo_transaccion.Text = "Debit Note";

                    }
                }
                else if (user.PaisID == 3)
                {
                    lbl_tipo_transaccion.Text = "Nota de Debito";
                }
                else
                {
                    lbl_tipo_transaccion.Text = "Nota Debito/Invoice";
                }
            }
            else if (tipo_transaccion == 14)
            {
                lbl_tipo_transaccion.Text = "Factura Proforma";
                #region Cambio de Informacion por Cobro de DEMORAS APL
                if ((user.SucursalID == 9) && ((factdata.strC28 == "DI") || (factdata.strC28 == "DE")))
                {
                    lbl_direccion_empresa.Text = "";
                    lbl_nit_empresa.Text = "";
                    lbl_formas_pago.Text = "*Por favor emitir Giro Bancario a nombre de AMERICAN PRESIDENT LINES";
                    lbl_tipo_transaccion.Text = "Invoice - Proforma";
                    Image1.ImageUrl = "~/img/apl.png";
                    Image1.Height = 100;
                    Image1.Width = 150;
                    lbl_formas_pago.Visible = true;
                }
                #endregion
            }
            else if (tipo_transaccion == 18)
            {
                lbl_tipo_transaccion.Text = "Ajuste Nota Credito";
            }
            #endregion
            lbl_serie.Text = factdata.strC28;
            //Formaterar correlativo honduras
            string strcorrelativo_doc = "";
            int correlativo_doc = 0;
            if ((user.PaisID == 3 || user.PaisID == 23) && (user.SucursalID == 102 || user.SucursalID == 104) && (user.contaID == 1))
            {
                if (factdata.strC1 != "")
                {
                    correlativo_doc = int.Parse(factdata.strC1);
                    strcorrelativo_doc = "-" + correlativo_doc.ToString("00000000.##");
                }
            }
            else
            {
                strcorrelativo_doc = "-" + factdata.strC1;
            }

            //////////////////////////////////////PARAMETROS///////////////////////////////////////////
            DB.parametros Params = DB.EmpresaParametros(user.PaisID, "", "", "** TITULO **", ""); //pais, sistema, doc_id, titulo, edicion        
            Image1.ImageUrl = "data:image;base64," + System.Convert.ToBase64String(Params.logo);
            Image1.Height = 63;
            Image1.Width = 237;
            lbl_direccion_empresa.Text = Params.direccion2;

		if (1 == 1)  //no habilitado temporalmente
        {
            #region 2021-01-11 getFacturabySerie
            if ((user.PaisID == 3 || user.PaisID == 23) && (user.contaID == 1) &&  
				(user.SucursalID == 102 || user.SucursalID == 104))  
            {
				rango_autorizado = "Rango autorizado:  Del <b>" + "</b> Al <b>" + "</b>";
                fecha_emision = "Fecha límite de emisión:  <b>" + "</b>"; ;
                lbl_cai.Text = "CAI.: - no se encontro numero -";

                RE_GenericBean dat2 = DB.getFacturabySerie(lbl_serie.Text, -1, user.SucursalID);

                if (dat2.strC5 != null)
                    if (dat2.strC5 != "")
                    if (int.Parse(dat2.strC5) > 0)
                    {
                        rango_autorizado = "Rango autorizado:  Del <b>" + lbl_serie.Text + "-" + int.Parse(dat2.strC5).ToString("00000000.##") + "</b> Al <b>" + lbl_serie.Text + "-" + int.Parse(dat2.strC6).ToString("00000000.##") + "</b>";
                    }

                if (dat2.strC4 != null)
                    if (dat2.strC4 != "")
                    {
                        fecha_emision = "Fecha límite de emisión:  <b>" + dat2.strC4 + "</b>";
                    }

                if (dat2.strC7 != null)
                    if (dat2.strC7 != "")
                    {
                        lbl_cai.Text = "CAI.: " + dat2.strC7;
                    }
            }
            #endregion getFacturabySerie
        }

        if (1 == 2)
        {
            //////////--VALIDACION DE RANGOS DE CORRELATIVOS POR DOCUMENTO PARA DETERMINAR QUE NUMERO DE CAI UTILIZAR--//////////
            #region 2021-01-27 
            if ((user.PaisID == 3) && (user.contaID == 1) && (user.SucursalID == 102))
            {
                if (factdata.intC4 == 8)
                {
                    if (tipo_transaccion == 1)
                    {
                        lbl_cai.Text = "CAI.: 69347A-EAEF9E-884681-62A9B6-3DB8F7-D2";
                    }
                    if (tipo_transaccion == 3)
                    {
                        lbl_cai.Text = "CAI.: C3E508-33A671-C64CB3-B74FD7-3AA9AC-BF";
                    }
                    if (tipo_transaccion == 4)
                    {
                        if (user.Operacion == 1)
                        {
                            lbl_cai.Text = "CAI.: 7FD142-6F22D5-144796-0E27C1-3DC75C-14";
                        }
                        if (user.Operacion == 2)
                        {
                            lbl_cai.Text = "CAI.: 50FDB5-29F5D8-4148BD-249DBA-581BEF-F8";
                        }
                    }
                }
                else
                {
                    if (user.Operacion == 1)
                    {
                        if ((Tipo_Transaccion == 1 && correlativo_doc <= 9000) || (Tipo_Transaccion == 3 && correlativo_doc <= 600) || (Tipo_Transaccion == 4 && correlativo_doc <= 1000))
                        {
                            lbl_cai.Text = "CAI.: 5890C1-026427-254380-AB3502-39D69C-88";
                        }
                        else
                        {
                            lbl_cai.Text = "CAI.: 6040EC-C37B99-0D43B1-B4A85F-E93234-39";
                        }
                    }
                    if (user.Operacion == 2)
                    {
                        lbl_cai.Text = "CAI.: 7D334A-A3EE3E-A54B80-7BE4FB-E3286E-8C";
                    }
                }
                lbl_email_empresa.Text = "hn-sales@aimargroup.com";
                factura_beneficio = "</br><b>LA FACTURA ES BENEFICIO DE TODOS,  EXIJALA!!</b></br></br>";
            }

            if ((user.PaisID == 23) && (user.SucursalID == 104) && (user.contaID == 1))
            {
                if (factdata.intC4 == 8)
                {
                    if (tipo_transaccion == 1)
                    {
                        lbl_cai.Text = "CAI.: 608DFD-FF1DC1-1D469C-F9E489-1CA777-05";
                    }
                    if (tipo_transaccion == 3)
                    {
                        lbl_cai.Text = "CAI.: 608DFD-FF1DC1-1D469C-F9E489-1CA777-05";
                    }
                    if (tipo_transaccion == 4)
                    {
                        if (user.Operacion == 1)
                        {
                            lbl_cai.Text = "CAI.: BB4FE0-603457-07408C-5531CE-2B6725-7D";
                        }
                        if (user.Operacion == 2)
                        {
                            lbl_cai.Text = "CAI.: BB4FE0-603457-07408C-5531CE-2B6725-7D";
                        }
                    }
                }
                else
                {
                    if (user.Operacion == 1)
                    {
                        if ((Tipo_Transaccion == 1 && correlativo_doc <= 2000) || (Tipo_Transaccion == 3 && correlativo_doc <= 200) || (Tipo_Transaccion == 4 && correlativo_doc <= 600))
                        {
                            lbl_cai.Text = "CAI.: C66F95-F93DD6-1D459C-3E4D35-046281-5B";
                        }
                        else
                        {
                            lbl_cai.Text = "CAI.: 593A1C-BD176C-4341A3-492C31-8B7349-F9";
                        }
                    }
                    if (user.Operacion == 2)
                    {
                        lbl_cai.Text = "CAI.: 9FCF29-0DEF9C-824AA5-1B3334-907305-B7";
                    }
                }
                lbl_email_empresa.Text = " operaciones.hn5@latinfreightneutral.com";
                factura_beneficio = "</br><b>LA FACTURA ES BENEFICIO DE TODOS,  EXIJALA!!</b></br></br>";
            }

            if ((user.PaisID == 3) && (user.contaID == 1) && (user.SucursalID == 102))
            {
                if (factdata.intC4 == 8)
                {
                    if (tipo_transaccion == 1)
                    {
                        fecha_emision = "Fecha límite de emisión:  <b>22/06/2017</b>";
                    }
                    if (tipo_transaccion == 3)
                    {
                        fecha_emision = "Fecha límite de emisión:  <b>22/06/2017</b>";
                    }
                    if (tipo_transaccion == 4)
                    {
                        if (user.Operacion == 1)
                        {
                            fecha_emision = "Fecha límite de emisión:  <b>22/06/2017</b>";
                        }
                        if (user.Operacion == 2)
                        {
                            fecha_emision = "Fecha límite de emisión:  <b>22/06/2017</b>";
                        }
                    }
                }
                else
                {
                    if (user.Operacion == 1)
                    {
                        if ((Tipo_Transaccion == 1 && correlativo_doc <= 9000) || (Tipo_Transaccion == 3 && correlativo_doc <= 600) || (Tipo_Transaccion == 4 && correlativo_doc <= 1000))
                        {
                            fecha_emision = "Fecha límite de emisión:  <b>20/03/2016</b>";
                        }
                        else
                        {
                            fecha_emision = "Fecha límite de emisión:  <b>23/03/2017</b>";
                        }
                    }
                    if (user.Operacion == 2)
                    {
                        fecha_emision = "Fecha límite de emisión:  <b>14/01/2017</b>";
                    }
                }

                if (tipo_transaccion == 1)//Factura
                {
                    if (factdata.intC4 == 8)
                    {
                        rango_autorizado = "Rango autorizado: Del <b>000-002-01-00000001</b> Al <b>000-002-01-00001000</b>";
                    }
                    else
                    {
                        if (correlativo_doc <= 9000)
                        {
                            rango_autorizado = "Rango autorizado: Del <b>000-001-01-00000001</b> Al <b>000-001-01-00009000</b>";
                        }
                        else
                        {
                            rango_autorizado = "Rango autorizado: Del <b>000-001-01-00009001</b> Al <b>000-001-01-00015000</b>";
                        }
                    }
                }

                if (tipo_transaccion == 3)//Nota Credito
                {
                    if (factdata.intC4 == 8)
                    {
                        rango_autorizado = "Rango autorizado: Del <b>000-002-06-00000001</b> Al <b>000-002-06-00000200</b>";
                    }
                    else
                    {
                        if (correlativo_doc <= 600)
                        {
                            rango_autorizado = "Rango autorizado: Del <b>000-001-06-00000001</b> Al <b>000-001-06-00000600</b>";
                        }
                        else
                        {
                            rango_autorizado = "Rango autorizado: Del <b>000-001-06-00000601</b> Al <b>000-001-06-00001100</b>";
                        }
                    }
                }

                if (tipo_transaccion == 4)//Nota Debito
                {
                    if (factdata.intC4 == 8)
                    {
                        if (user.Operacion == 1)
                        {
                            rango_autorizado = "Rango autorizado: Del <b>000-003-07-00000001</b> Al <b>000-003-07-00000300</b>";
                        }
                        if (user.Operacion == 2)
                        {
                            rango_autorizado = "Rango autorizado: Del <b>000-004-07-00000001</b> Al <b>000-004-07-00000250</b>";
                        }
                    }
                    else
                    {
                        if (user.Operacion == 1)
                        {
                            if (correlativo_doc <= 1000)
                            {
                                rango_autorizado = "Rango autorizado:  Del <b>000-001-07-00000001</b> Al <b>000-001-07-00001000</b>";
                            }
                            else
                            {
                                rango_autorizado = "Rango autorizado:  Del <b>000-001-07-00001001</b> Al <b>000-001-07-00001600</b>";
                            }
                        }
                        if (user.Operacion == 2)
                        {
                            rango_autorizado = "Rango autorizado:  Del <b>000-002-07-00000001</b> Al <b>000-002-07-00000500</b>";
                        }
                    }
                }
            }

            if ((user.PaisID == 23) && (user.SucursalID == 104) && (user.contaID == 1))
            {
                if (factdata.intC4 == 8)
                {
                    if (tipo_transaccion == 1)
                    {
                        fecha_emision = "Fecha límite de emisión:  <b>27/03/2017</b>";
                    }
                    if (tipo_transaccion == 3)
                    {
                        fecha_emision = "Fecha límite de emisión:  <b>27/03/2017</b>";
                    }
                    if (tipo_transaccion == 4)
                    {
                        if (user.Operacion == 1)
                        {
                            fecha_emision = "Fecha límite de emisión:  <b>27/06/2017</b>";
                        }
                        if (user.Operacion == 2)
                        {
                            fecha_emision = "Fecha límite de emisión:  <b>27/06/2017</b>";
                        }
                    }
                }
                else
                {
                    if (user.Operacion == 1)
                    {
                        if ((Tipo_Transaccion == 1 && correlativo_doc <= 2000) || (Tipo_Transaccion == 3 && correlativo_doc <= 200) || (Tipo_Transaccion == 4 && correlativo_doc <= 600))
                        {
                            fecha_emision = "Fecha límite de emisión:  <b>30/05/2016</b>";
                        }
                        else
                        {
                            fecha_emision = "Fecha límite de emisión:  <b>27/02/2017</b>";
                        }
                    }
                    if (user.Operacion == 2)
                    {
                        fecha_emision = "Fecha límite de emisión:  <b>25/01/2017</b>";
                    }
                }
                if (tipo_transaccion == 1)//Factura
                {
                    if (factdata.intC4 == 8)
                    {
                        rango_autorizado = "Rango autorizado: Del <b>000-002-01-00000001</b> Al <b>000-002-01-00000300</b>";
                    }
                    else
                    {
                        if (correlativo_doc <= 2000)
                        {
                            rango_autorizado = "Rango autorizado:  Del <b>000-001-01-00000001</b> Al <b>000-001-01-00002000</b>";
                        }
                        else
                        {
                            rango_autorizado = "Rango autorizado:  Del <b>000-001-01-00002001</b> Al <b>000-001-01-00002400</b>";
                        }
                    }
                }

                if (tipo_transaccion == 3)//Nota Credito
                {
                    if (factdata.intC4 == 8)
                    {
                        rango_autorizado = "Rango autorizado: Del <b>000-002-06-00000001</b> Al <b>000-002-06-00000050</b>";
                    }
                    else
                    {
                        if (correlativo_doc <= 200)
                        {
                            rango_autorizado = "Rango autorizado: Del <b>000-001-06-00000001</b> Al <b>000-001-06-00000200</b>";
                        }
                        else
                        {
                            rango_autorizado = "Rango autorizado: Del <b>000-001-06-00000201</b> Al <b>000-001-06-00000250</b>";
                        }
                    }
                }

                if (tipo_transaccion == 4)//Nota Debito
                {
                    if (factdata.intC4 == 8)
                    {
                        if (user.Operacion == 1)
                        {
                            rango_autorizado = "Rango autorizado: Del <b>000-003-07-00000001</b> Al <b>000-003-07-00000050</b>";
                        }
                        if (user.Operacion == 2)
                        {
                            rango_autorizado = "Rango autorizado: Del <b>000-004-07-00000001</b> Al <b>000-004-07-00000050</b>";
                        }
                    }
                    else
                    {
                        if (user.Operacion == 1)
                        {
                            if (correlativo_doc <= 600)
                            {
                                rango_autorizado = "Rango autorizado:  Del <b>000-001-07-00000001</b> Al <b>000-001-07-00000600</b>";
                            }
                            else
                            {
                                rango_autorizado = "Rango autorizado:  Del <b>000-001-07-00000601</b> Al <b>000-001-07-00000620</b>";
                            }
                        }
                        if (user.Operacion == 2)
                        {
                            rango_autorizado = "Rango autorizado:  Del <b>000-002-07-00000001</b> Al <b>000-002-07-00000300</b>";
                        }
                    }
                }
            }
			#endregion
            //////////FIN VALIDACION DE RANGOS DE CORRELATIVOS POR DOCUMENTO PARA DETERMINAR QUE NUMERO DE CAI UTILIZAR//////////
		}


			lbl_correlativo.Text = strcorrelativo_doc;

            var fel = DB.DataFEL(user.PaisID, factdata.strC49, "");
            if (fel.isFEL)
            {
                lbl_serie.Text = fel.Sign_Serie;
                lbl_correlativo.Text = fel.Sign_Numero;
                lbl_firma_digital.Text = fel.firma;
            } 
            /*
            if (DB.isFELDate(user.PaisID)) { //2019-07-29
                lbl_firma_digital.Text = DB.FirmaFEL("4", factdata.strC49, factdata.strC52); //, "", "Numero", "");
                string[] args = DB.DataFEL(factdata.strC49, lbl_serie.Text, lbl_correlativo.Text);
                lbl_serie.Text = args[1];
                lbl_correlativo.Text = args[2];
            }*/

            lbl_fecha_emision.Text = factdata.strC5;
            if ((Tipo_Transaccion == 1) || (Tipo_Transaccion == 4) || (Tipo_Transaccion == 14))
            {
                if (user.PaisID == 7)
                {
                    lbl_vencimiento.Text = "Due";
                }
                else if ((user.PaisID == 3 || user.PaisID == 23) && user.contaID == 2)
                {
                    lbl_vencimiento.Text = "Due";
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
                    if (tipo_transaccion == 4)
                    {
                        if ((user.PaisID == 3 || user.PaisID == 23) && user.contaID == 2)
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
            if (user.PaisID == 11)
            {
                lbl_moneda.Text = Utility.TraducirMonedaInt(8);
            }
            else
            {

                if ((user.PaisID == 3 || user.PaisID == 23) && (user.SucursalID == 102 || user.SucursalID == 104) && (user.contaID == 1))
                {
                    lbl_moneda.Text = "Lps.";
                }
                else
                {
                    lbl_moneda.Text = Utility.TraducirMonedaInt(factdata.intC4);
                }
            }
            lbl_direccion.Text = factdata.strC4;
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
                else if ((user.PaisID == 3 || user.PaisID == 23) && user.contaID == 2)
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
                else if ((user.PaisID == 3 || user.PaisID == 23) && user.contaID == 2 && tipo_transaccion == 3)
                {
                    html_factura_NC += "Note Credit Applied to Document: </td>";
                }
                else
                {
                    html_factura_NC += "Nota de Credito Aplicada a Documento: </td>";
                }

                //Formaterar correlativo honduras
                string strcorrelativo_nc = "";
                if ((user.PaisID == 3 || user.PaisID == 23) && (user.SucursalID == 102 || user.SucursalID == 104) && (user.contaID == 1))
                {
                    //string[] words = s.Split(' ');
                    if (factdata.strC1 != "")
                    {
                        string[] segmentos = factdata.strC40.Split('-');

                        correlativo_doc = int.Parse(segmentos[3].ToString().Trim());
                        strcorrelativo_nc = segmentos[0] + "-" + segmentos[1] + "-" + segmentos[2].Trim() + "-" + correlativo_doc.ToString("00000000.##");
                    }
                }
                else
                {
                    strcorrelativo_nc = factdata.strC40;
                }
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
                    if ((user.PaisID == 3 || user.PaisID == 23) && user.contaID == 2)
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
                if ((user.PaisID == 3 || user.PaisID == 23) && user.contaID == 2)
                {
                    string criterio = "agente = '" + factdata.strC41 + "'";
                    ArrayList datos_agente = new ArrayList();
                    datos_agente = DB.getAgente(criterio, "REPORTES");
                    foreach (RE_GenericBean dataA in datos_agente)
                    {
                        lb_nombre_agente.Visible = true;
                        lb_nombre_agente.Text = dataA.strC1;

                        if (dataA.strC2 == "")
                        {
                            lbl_direccion_empresa.Text = "11087 NW 122nd St, Miami, FL 33178, USA";
                        }
                        else
                        {
                            lbl_direccion_empresa.Text = dataA.strC2;
                        }
                    }
                }
                //
            }
            else
            {
                if ((user.PaisID == 3 || user.PaisID == 23) && user.contaID == 2)
                {
                    lb_nombre_agente.Visible = true;
                    lb_nombre_agente.Text = "International Agents";
                    lbl_direccion_empresa.Text = "11087 NW 122nd St, Miami, FL 33178, USA";
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
                if ((user.PaisID == 3 || user.PaisID == 23) && user.contaID == 2)
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
                    if ((user.PaisID == 3 || user.PaisID == 23) && user.contaID == 2)
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
                    if ((user.PaisID == 3 || user.PaisID == 23) && user.contaID == 2)
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
                    if ((user.PaisID == 3 || user.PaisID == 23) && user.contaID == 2)
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
                            if ((user.PaisID == 3 || user.PaisID == 23) && user.contaID == 2)
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
                        if ((user.PaisID == 3 || user.PaisID == 23) && user.contaID == 2)
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
                    if ((user.PaisID == 3 || user.PaisID == 23) && user.contaID == 2)
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
                    if ((user.PaisID == 3 || user.PaisID == 23) && user.contaID == 2)
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
                    if ((user.PaisID == 3 || user.PaisID == 23) && user.contaID == 2)
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
                    if ((user.PaisID == 3 || user.PaisID == 23) && user.contaID == 2)
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
                    if ((user.PaisID == 3 || user.PaisID == 23) && user.contaID == 2)
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
                    if (((user.PaisID == 3 || user.PaisID == 23) && (user.SucursalID == 102 || user.SucursalID == 104) && (user.contaID == 1)))
                    {

                        #region mostrar rubros honduras
                        ////toda la funcionalidad de cobro de terceros para aimar y latin freight Hondudras
                        string cargos_locales = "";
                        string cargos_terceros = "";
                        string titulo_cargos_locales = "";
                        string titulo_cargos_terceros = "";
                        decimal total_terceros = 0;
                        decimal total_terceros_equivalente = 0;
                        decimal total_locales = 0;
                        decimal total_locales_equivalente = 0;
                        decimal impuestos_locales = 0;
                        decimal total_fact_hn = 0;
                        decimal tipo_cambio_hn = 0;
                        decimal terceros_usd_hn = 0;
                        decimal locales_usd_hn = 0;
                        decimal impuestos_locales_usd_hn = 0;

                        if (user.PaisID == 3 || user.PaisID == 23)
                        {
                            titulo_cargos_locales = "DETALLE CARGOS LOCALES";
                            titulo_cargos_terceros = "REEMBOLSO DE  CARGOS A CUENTA DE CLIENTE  / TERCEROS";
                        }
                        else if (user.PaisID == 5 || user.PaisID == 21)
                        {
                            titulo_cargos_locales = "SERVICIOS";
                            titulo_cargos_terceros = "PAGOS A TERCEROS";
                        }

                        if (factdata.intC4 == 8)
                        {
                            moneda = "US$";
                            if (user.PaisID == 3 || user.PaisID == 23)
                            {
                                moneda_equivalente = "Lps.";
                            }
                            else if (user.PaisID == 5 || user.PaisID == 21)
                            {
                                moneda_equivalente = "Cl.";
                            }
                        }
                        else
                        {
                            if (user.PaisID == 3 || user.PaisID == 23)
                            {
                                moneda = "Lps.";
                            }
                            else if (user.PaisID == 5 || user.PaisID == 21)
                            {
                                moneda = "Cl.";
                            }
                            moneda_equivalente = "US$";
                        }

                        html_Rubros = "<table align='center' cellpadding='0' cellspacing='3' class='style3'>";

                        foreach (DataRow dr in dt.Rows)
                        {
                            if (dr[2].ToString() == "TERCEROS")
                            {
                                cargos_terceros += "<tr>"
                                + "<td width='500px'>"
                                    + dr[1].ToString()
                                + "</td>"
                                + "<td width='100px' align='right'>"
                                    + decimal.Parse(dr[4].ToString()).ToString("#,#.00")
                                + "</td>"
                                + "<td width='100px' align='right'>"
                                    + decimal.Parse(dr[5].ToString()).ToString("#,#.00")
                                + "</td>"
                                + "</tr>";

                                if ((dr[8].ToString() != null) && (dr[8].ToString() != ""))
                                {
                                    cargos_terceros += "<tr>"
                                    + "<td colspan='2' style='width: 600px' width='300px'>"
                                        + "         --->" + dr[8].ToString()
                                    + "</td>"
                                + "</tr>";
                                }

                                total_terceros += decimal.Parse(dr[5].ToString());
                                total_terceros_equivalente += decimal.Parse(dr[4].ToString());
                            }

                            else
                            {
                                cargos_locales += "<tr>"
                                + "<td width='500px'>"
                                    + dr[1].ToString()
                                + "</td>"
                                + "<td width='100px' align='right'>"
                                    + decimal.Parse(dr[4].ToString()).ToString("#,#.00")
                                + "</td>"
                                + "<td width='100px' align='right'>"
                                    + decimal.Parse(dr[5].ToString()).ToString("#,#.00")
                                + "</td>"
                                + "</tr>";

                                if ((dr[8].ToString() != null) && (dr[8].ToString() != ""))
                                {
                                    cargos_locales += "<tr>"
                                    + "<td colspan='2' style='width: 600px' width='300px'>"
                                        + "         --->" + dr[8].ToString()
                                    + "</td>"
                                + "</tr>";
                                }

                                total_locales += decimal.Parse(dr[5].ToString());
                                total_locales_equivalente += decimal.Parse(dr[4].ToString());
                                impuestos_locales += decimal.Parse(dr[6].ToString());
                            }

                        }

                        cargos_terceros += "<tr><td align='center'><b>Total</b></td><td align='right'><b>" + total_terceros_equivalente.ToString("#,#.00") + "</b></td><td align='right'><b>" + total_terceros.ToString("#,#.00") + "</b></td></tr>";
                        cargos_locales += "<tr><td align='center'><b>Total</b></td><td align='right'><b>" + total_locales_equivalente.ToString("#,#.00") + "</b></td><td align='right'><b>" + total_locales.ToString("#,#.00") + "</b></td></tr>";

                        if (cargos_locales != "")
                        {
                            html_Rubros += "<tr>"
                                + "<td width='500px' class='style6'>"
                                    + "<b>" + titulo_cargos_locales + "</b>"
                                + "</td>"
                                + "<td width='100px' align='right' class='style6'>"
                                    + "<b>" + moneda_equivalente + "</b>"
                                + "</td>"
                                + "<td width='100px' align='right' class='style6'>"
                                    + "<b>" + moneda + "</b>"
                                + "</td>"
                            + "</tr>";
                            html_Rubros += cargos_locales;
                        }
                        if (cargos_terceros != "")
                        {
                            html_Rubros += "<tr><td colspan='3'>&nbsp;</td></tr>";
                            html_Rubros += "<tr>"
                                + "<td width='500px' class='style6'>"
                                    + "<b>" + titulo_cargos_terceros + "</b>"
                                + "</td>"
                                + "<td width='100px' align='right' class='style6'>"
                                    + "<b>" + moneda_equivalente + "</b>"
                                + "</td>"
                                + "<td width='100px' align='right' class='style6'>"
                                    + "<b>" + moneda + "</b>"
                                + "</td>"
                            + "</tr>";
                            html_Rubros += cargos_terceros;
                        }



                        html_Rubros += "</table>";

                        //impuestos_locales = total_locales * user.pais.Impuesto;

                        if (factdata.intC4 == 8)
                        {
                            if (factdata.strC1 != "")
                            {
                                total_fact_hn = total_terceros + total_locales + impuestos_locales;
                                tipo_cambio_hn = factdata.decC4 / total_fact_hn;
                                terceros_usd_hn = total_terceros * tipo_cambio_hn;
                                locales_usd_hn = total_locales * tipo_cambio_hn;
                                impuestos_locales_usd_hn = impuestos_locales * tipo_cambio_hn;
                            }
                        }
                        else
                        {
                            if (factdata.strC1 != "")
                            {
                                total_fact_hn = total_terceros + total_locales + impuestos_locales;
                                tipo_cambio_hn = total_fact_hn / factdata.decC4;
                                terceros_usd_hn = total_terceros / tipo_cambio_hn;
                                locales_usd_hn = total_locales / tipo_cambio_hn;
                                impuestos_locales_usd_hn = impuestos_locales / tipo_cambio_hn;
                            }
                        }

                        lbl_terceros.Text = total_terceros.ToString("#,#.00");
                        lbl_locales.Text = total_locales.ToString("#,#.00");
                        lbl_impuesto_locales.Text = impuestos_locales.ToString("#,#.00");
                        lbl_total_pagar.Text = total_fact_hn.ToString("#,#.00");
                        lbl_tipo_cambio_hn.Text = tipo_cambio_hn.ToString("#,#.0000");
                        lbl_total_usd_hn.Text = factdata.decC4.ToString("#,#.00");

                        lbl_terceros_usd_hn.Text = terceros_usd_hn.ToString("#,#.00");
                        lbl_locales_usd_hn.Text = locales_usd_hn.ToString("#,#.00");
                        lbl_impuesto_locales_usd_hn.Text = impuestos_locales_usd_hn.ToString("#,#.00");


                        ////
                        #endregion
                    }
                    else if ((user.PaisID == 5 || user.PaisID == 21) && (Tipo_Transaccion == 1))
                    {
                        #region mostrar rubros Aimar y Latin Freight Costa Rica

                        //////CARGAR RUBROS AIMAR Y LATIN CR
                        string cargos_locales = "";
                        string cargos_terceros = "";
                        string titulo_cargos_locales = "";
                        string titulo_cargos_terceros = "";
                        decimal total_terceros = 0;
                        decimal total_terceros_equivalente = 0;
                        decimal total_locales = 0;
                        decimal total_locales_equivalente = 0;
                        decimal impuestos_locales = 0;
                        decimal total_fact_hn = 0;
                        decimal tipo_cambio_hn = 0;
                        decimal terceros_usd_hn = 0;
                        decimal locales_usd_hn = 0;
                        decimal impuestos_locales_usd_hn = 0;


                        if (user.PaisID == 5 || user.PaisID == 21)
                        {
                            titulo_cargos_locales = "SERVICIOS";
                            titulo_cargos_terceros = "PAGOS A TERCEROS";
                        }

                        if (factdata.intC4 == 8)
                        {
                            moneda = "US$";
                            moneda_equivalente = "";
                        }
                        else
                        {
                            moneda = "Cl.";
                            moneda_equivalente = "";
                        }

                        html_Rubros = "<table align='center' cellpadding='0' cellspacing='3' class='style3'>";

                        foreach (DataRow dr in dt.Rows)
                        {
                            if (dr[2].ToString() == "TERCEROS")
                            {
                                cargos_terceros += "<tr>"
                                + "<td width='500px'>"
                                    + dr[1].ToString()
                                + "</td>"
                                + "<td width='100px' align='right'>"

                                + "</td>"
                                + "<td width='100px' align='right'>"
                                    + decimal.Parse(dr[5].ToString()).ToString("#,#.00")
                                + "</td>"
                                + "</tr>";

                                if ((dr[8].ToString() != null) && (dr[8].ToString() != ""))
                                {
                                    cargos_terceros += "<tr>"
                                    + "<td colspan='2' style='width: 600px' width='300px'>"
                                        + "         --->" + dr[8].ToString()
                                    + "</td>"
                                + "</tr>";
                                }

                                total_terceros += decimal.Parse(dr[5].ToString());
                                total_terceros_equivalente += decimal.Parse(dr[4].ToString());
                            }

                            else
                            {
                                cargos_locales += "<tr>"
                                + "<td width='500px'>"
                                    + dr[1].ToString()
                                + "</td>"
                                + "<td width='100px' align='right'>"

                                + "</td>"
                                + "<td width='100px' align='right'>"
                                    + decimal.Parse(dr[5].ToString()).ToString("#,#.00")
                                + "</td>"
                                + "</tr>";

                                if ((dr[8].ToString() != null) && (dr[8].ToString() != ""))
                                {
                                    cargos_locales += "<tr>"
                                    + "<td colspan='2' style='width: 600px' width='300px'>"
                                        + "         --->" + dr[8].ToString()
                                    + "</td>"
                                + "</tr>";
                                }

                                total_locales += decimal.Parse(dr[5].ToString());
                                total_locales_equivalente += decimal.Parse(dr[4].ToString());
                                impuestos_locales += decimal.Parse(dr[6].ToString());
                            }

                        }

                        cargos_terceros += "<tr><td align='center'><b>Total</b></td><td align='right'></td><td align='right'><b>" + total_terceros.ToString("#,#.00") + "</b></td></tr>";
                        cargos_locales += "<tr><td align='center'><b>Total</b></td><td align='right'></td><td align='right'><b>" + total_locales.ToString("#,#.00") + "</b></td></tr>";

                        if (cargos_locales != "")
                        {
                            html_Rubros += "<tr>"
                                + "<td width='500px' class='style6'>"
                                    + "<b>" + titulo_cargos_locales + "</b>"
                                + "</td>"
                                + "<td width='100px' align='right' class='style6'>"
                                    + "<b>" + moneda_equivalente + "</b>"
                                + "</td>"
                                + "<td width='100px' align='right' class='style6'>"
                                    + "<b>" + moneda + "</b>"
                                + "</td>"
                            + "</tr>";
                            html_Rubros += cargos_locales;
                        }
                        if (cargos_terceros != "")
                        {
                            html_Rubros += "<tr><td colspan='3'>&nbsp;</td></tr>";
                            html_Rubros += "<tr>"
                                + "<td width='500px' class='style6'>"
                                    + "<b>" + titulo_cargos_terceros + "</b>"
                                + "</td>"
                                + "<td width='100px' align='right' class='style6'>"
                                    + "<b>" + moneda_equivalente + "</b>"
                                + "</td>"
                                + "<td width='100px' align='right' class='style6'>"
                                    + "<b>" + moneda + "</b>"
                                + "</td>"
                            + "</tr>";
                            html_Rubros += cargos_terceros;
                        }



                        html_Rubros += "</table>";

                        //impuestos_locales = total_locales * user.pais.Impuesto;

                        if (factdata.intC4 == 8)
                        {
                            if (factdata.strC1 != "")
                            {
                                total_fact_hn = total_terceros + total_locales + impuestos_locales;
                                tipo_cambio_hn = factdata.decC4 / total_fact_hn;
                                terceros_usd_hn = total_terceros * tipo_cambio_hn;
                                locales_usd_hn = total_locales * tipo_cambio_hn;
                                impuestos_locales_usd_hn = impuestos_locales * tipo_cambio_hn;
                            }
                        }
                        else
                        {
                            if (factdata.strC1 != "")
                            {
                                total_fact_hn = total_terceros + total_locales + impuestos_locales;
                                tipo_cambio_hn = total_fact_hn / factdata.decC4;
                                terceros_usd_hn = total_terceros / tipo_cambio_hn;
                                locales_usd_hn = total_locales / tipo_cambio_hn;
                                impuestos_locales_usd_hn = impuestos_locales / tipo_cambio_hn;
                            }
                        }

                        lbl_terceros.Text = total_terceros.ToString("#,#.00");
                        lbl_locales.Text = total_locales.ToString("#,#.00");
                        lbl_impuesto_locales.Text = impuestos_locales.ToString("#,#.00");
                        lbl_total_pagar.Text = total_fact_hn.ToString("#,#.00");
                        lbl_tipo_cambio_hn.Text = tipo_cambio_hn.ToString("#,#.0000");
                        lbl_total_usd_hn.Text = factdata.decC4.ToString("#,#.00");

                        lbl_terceros_usd_hn.Text = terceros_usd_hn.ToString("#,#.00");
                        lbl_locales_usd_hn.Text = locales_usd_hn.ToString("#,#.00");
                        lbl_impuesto_locales_usd_hn.Text = impuestos_locales_usd_hn.ToString("#,#.00");
                        #endregion
                    }
                    else
                    {
                        string detalle = "";
                        string value = "";
                        string impuesto = "";
                        if (user.PaisID == 7)
                        {
                            detalle = "Detail";
                            value = "Value";
                            impuesto = "Tax";
                        }
                        else if ((user.PaisID == 3 || user.PaisID == 23) && user.contaID == 2)
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
                    }
                }//arriba
                else
                {
                    string detalle = "";
                    string value = "";
                    string impuesto = "";
                    if (user.PaisID == 7)
                    {
                        detalle = "Detail";
                        value = "Value";
                        impuesto = "Tax";
                    }
                    else if ((user.PaisID == 3 || user.PaisID == 23) && user.contaID == 2)
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
                if (user.PaisID == 7)
                {
                    lbl_total_letras.Text = "TOTAL IN LETTERS.:    " + c.enletras(factdata.decC3.ToString(), 8);
                }
                else if ((user.PaisID == 3 || user.PaisID == 23) && user.contaID == 2)
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