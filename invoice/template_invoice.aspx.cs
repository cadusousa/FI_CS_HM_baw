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

public partial class invoice_template_invoice : System.Web.UI.Page
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
    public string texto_copia = string.Empty;
    

    DataTable dt = null;
    public UsuarioBean user;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["usuario"] == null)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "Nombre", "<script> refresh(); </script>");
        }
        user = (UsuarioBean)Session["usuario"];

        DB.parametros Params = DB.EmpresaParametros(user.PaisID, "", "", "** TITULO **", ""); //pais, sistema, doc_id, titulo, edicion

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
        

        //PIE DE DOCUMENTO HN
        if ((user.PaisID == 3) && user.contaID == 2)
        {
            lb_observaciones_hn.Visible = true;
            lb_observaciones_hn.Text = "Please note, for the release of your goods and delivery to the original bill of lading and other documentation."+
            " We request the payment described above to our agents in Honduras."+
            " Dollar payments must be by cashier's check, wire transfer  payable to <b>AIMAR GROUP</b>"+
            " Will also reiterate our desire to provide good service for import and export air shipments,"+
            " Sea, land and Storage, also in carrying out their customs formalities, ground transportation, distribution and"+
            " inventory management, certified inspections of loading and unloading, storage and other services.";
        }
        if ((user.PaisID == 23) && user.contaID == 2)
        {
            lb_observaciones_hn.Visible = true;
            lb_observaciones_hn.Text = "Please note, for the release of your goods and delivery to the original bill of lading and other documentation." +
            " We request the payment described above to our agents in Honduras." +
            " Dollar payments must be by cashier's check, wire transfer  payable to <b>LATIN FREIGHT</b>" +
            " Will also reiterate our desire to provide good service for import and export air shipments," +
            " Sea, land and Storage, also in carrying out their customs formalities, ground transportation, distribution and" +
            " inventory management, certified inspections of loading and unloading, storage and other services.";
        }
        //FIN PIE DE DOCUMENTO HN

        lbl_direccion_empresa.Text = user.pais.Direccion_Empresa;
        if (Params.direccion2.Trim() != "")
            lbl_direccion_empresa.Text = Params.direccion2; //2021-01-28

        if ((Request.QueryString["id"] != null)&&(Request.QueryString["transaccion"]!=null))
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

                    //lbl_direccion_empresa2.Text = user.pais.Direccion_Empresa;  //2021-01-28
                    if (Params.direccion2 != "")
                        lbl_direccion_empresa2.Text = Params.direccion2; //2021-01-28

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
                if (((Datos_Factura.intC8 == 3) || (Datos_Factura.intC8 == 5) ||(Datos_Factura.intC8 == 6)) && Datos_Factura.intC4 == 8) // si los paises son hn y cr no se muestra equivalente 
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
                                lbl_firma_digital.Text = Datos_Factura.strC49;                                */
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
                    //lbl_direccion_empresa.Text = user.pais.Direccion_Empresa; 2020-03-04
                    lbl_nit_empresa.Text = user.pais.Pai_Numero_Identificacion_Tributaria;
                }

                if ((Datos_Nota_Debito.strC50 != "-") && (Datos_Nota_Debito.strC50 == "1"))
                {
                    if (user.PaisID == 5)
                    {
                        lbl_firma_comentario.Text = "Codigo Unico de Consulta.: ";
                        lbl_firma_digital.Text = Datos_Nota_Debito.strC49;
                        lbl_firma_comentario.Visible = true;
                        lbl_firma_digital.Visible = true;
                    }
                }

                if (user.PaisID == 1)
                {
                    if (user.contaID == 1)
                    {
                        lbl_direccion_empresa.Text = user.pais.Direccion_Empresa;
                        if (Params.direccion2 != "")
                            lbl_direccion_empresa.Text = Params.direccion2; //2020-03-04

                        lbl_total_equivalente.Visible = false;
                        lbl_equivalente.Visible = false;
                        lbl_formas_pago.Visible = true;

                        string nombre_grupo_empres = "";
                        if (user.pais.Grupo_Empresas == 2)
                        {
                            nombre_grupo_empres = "LATIN FREIGHT";
                        }
                        else
                        {
                            nombre_grupo_empres = "AIMAR";
                        }

                        nombre_grupo_empres = Params.nombre_empresa;

                        //lbl_formas_pago.Text = "Cobro por cuenta Ajena";//Se comenta para agregar comentarios Fiscal USD solicitados por Esdras Mendez
                        lbl_formas_pago.Text = "El soporte de estos Cargos se encuentra Detallado en el pago de Impuestos amparado en la <strong>Poliza de Importacion.</strong><br>Forma de pago:   CHEQUE DE CAJA A NOMBRE DE "+nombre_grupo_empres+", S.A.<br>Gastos administrativos por cheque rechazado $25.00";
                        if (Datos_Nota_Debito.intC4 == 8)
                        {
                            lbl_tipo_transaccion.Text = "Recordatorio de Pago";
                            lbl_formas_pago.Text += "<br>Exenta de IVA";
                        }
                        else
                        {
                            lbl_tipo_transaccion.Text = "Recordatorio de Pago";
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
                    if (Params.direccion2 != "")
                        lbl_direccion_empresa2.Text = Params.direccion2; //2020-03-04
                }
            }
            if ((tipo_transaccion == 3) || (tipo_transaccion == 18) || (tipo_transaccion == 12) || (tipo_transaccion == 31))//Nota Credito o NC Ajuste 
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
                lb_razon.Text = "Razon"; //razon
                lb_direccion.Text = "Dirección"; //direccion
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
                lbl_direccion.Text = Datos_Nota_Credito.strC4;
                if ((user.PaisID == 3 || user.PaisID == 23) && (user.SucursalID == 102 || user.SucursalID == 104) && (user.contaID == 1))
                {                    
                    lbl_nit_empresa.Text = user.pais.Pai_Numero_Identificacion_Tributaria;
                }
                
                if (user.PaisID == 5)
                {
                    lbl_direccion_empresa.Text = user.pais.Direccion_Empresa;                    
                    if (Params.direccion2 != "")
                        lbl_direccion_empresa.Text = Params.direccion2; //2020-03-04

                    lbl_nit_empresa.Text = user.pais.Pai_Numero_Identificacion_Tributaria;
                }

                if ((Datos_Nota_Credito.strC50 != "-") && (Datos_Nota_Credito.strC50 == "1"))
                {
                    lbl_firma_comentario.Text = "Firma Electronica.: ";
                    lbl_firma_digital.Text = Datos_Nota_Credito.strC49;
                    lbl_firma_comentario.Visible = true;
                    lbl_firma_digital.Visible = true;

                    if ((user.PaisID == 1) || (user.PaisID == 15))
                    {
                        lbl_firma_comentario.Text = "";
                        var fel = DB.DataFEL(user.PaisID, Datos_Nota_Credito.strC49, Datos_Nota_Credito.strC52);
                        lbl_firma_digital.Text = fel.firma;
                        /*
                        lbl_firma_comentario.Text = "Firma Electronica.: ";	                    
                        if (DB.isFELDate(user.PaisID)) //2019-07-29
							lbl_firma_digital.Text = DB.FirmaFEL("5", Datos_Nota_Credito.strC49, Datos_Nota_Credito.strC52); //, Datos_Nota_Credito.strC32, "Correlativo", Datos_Nota_Credito.strC33);
						else
	                        lbl_firma_digital.Text = Datos_Nota_Credito.strC49;                        
                        */
                        lbl_firma_comentario.Visible = true;
                        lbl_firma_digital.Visible = true;
                    }
                    else if (user.PaisID == 5 || user.PaisID == 21)
                    {
                        lbl_firma_comentario.Text = "Codigo Unico de Consulta.: ";
                        lbl_firma_digital.Text = Datos_Nota_Credito.strC49;
                        lbl_firma_comentario.Visible = true;
                        lbl_firma_digital.Visible = true;
                    }

                }

                if (user.PaisID == 1)
                {
                    if (Datos_Nota_Credito.intC8 == 1)
                    {
                        //if ((Datos_Nota_Credito.strC50 != "-") && (Datos_Nota_Credito.strC50 == "1"))
                        //{
                        //    lbl_firma_comentario.Text = "Firma Electronica.: ";
                        //    lbl_firma_digital.Text = Datos_Nota_Credito.strC49;
                        //    lbl_firma_comentario.Visible = true;
                        //    lbl_firma_digital.Visible = true;
                        //}
                    }
                    if (user.contaID == 1)
                    {
                        lbl_direccion_empresa.Text = user.pais.Direccion_Empresa;
                        if (Params.direccion2 != "")
                            lbl_direccion_empresa.Text = Params.direccion2; //2020-03-04

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
                    if (Params.direccion2 != "")
                        lbl_direccion_empresa2.Text = Params.direccion2; //2020-03-04
                }
            }
        }

        Image1.ImageUrl = "data:image;base64," + System.Convert.ToBase64String(Params.logo);
        Image1.Height = 63;
        Image1.Width = 237; 
        lbl_email_empresa.Text = "";
 
        //lbl_nit_empresa.Text = "";
    }
    protected void cargar_Factura(RE_GenericBean factdata, int ID, int Tipo_Transaccion)
    {
        if (factdata != null)
        {
            #region Definir el Tipo de Documento
            if (tipo_transaccion == 1)
            {
                if (user.PaisID == 7 || user.PaisID == 30)
                {
                    texto_reclamo_documento = "Dear Customer, the amount of this invoice is considered correct and payable as is, if no errors are claimed within 15 calendar days of the issuance date of this invoice";
                }
                else if (user.PaisID == 3 || user.PaisID == 23)
                {
                    texto_reclamo_documento = "Estimado Cliente, a partir de la fecha de emision de la factura tiene 15 dias calendario para realizar cualquier reclamo sobre la misma";
                    texto_copia = "<tr><td align=\"center\"><p>Original Cliente - Copia Contabilidad</p><td><tr>";
                }
                else
                {
                    texto_reclamo_documento = "Estimado Cliente, a partir de la fecha de emision de la factura tiene 15 dias calendario para realizar cualquier reclamo sobre la misma";
                }
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
                            texto_recordatorio_pago = "</br>Latin Freight, S.A. no puede emitir una Factura, Invoice o Nota de Debito por los fletes marítimos, aéreos o terrestres que son generados y operados por nuestros agentes en el extranjero según la ley del IVA Articulo 3 donde ampara que el hecho generador de IVA es la prestación de servicios en el territorio nacional.</br></br>"
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
                else if (user.PaisID == 30)
                {
                    lbl_tipo_transaccion.Text = "INVOICE";
                    texto_recordatorio_pago = "Dear customer, for payment of the service provided, please make an international bank draft / international check / wire transfer to the order of <b>\"World Maritime Transport, Ltd.\"</b>";
                    texto_recordatorio_pago += "</br></br>Estimado Cliente, para el pago de los servicios proporcionados , favor de realizar giro bancario internacional / cheque internacional / transferencia bancaria a nombre de <b>\"World Maritime Transport, Ltd.\"</b> Para los cargos Internacionales.";
                }
                else
                {
                    lbl_tipo_transaccion.Text = "Factura";
                }

                
                
            }
            else if (tipo_transaccion == 3 || tipo_transaccion == 12 || tipo_transaccion == 31)
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
                if (user.PaisID == 7 || user.PaisID == 30)
                {
                    texto_reclamo_documento = "Dear Customer, the amount of this debit note is considered correct and payable as is, if no errors are claimed within 15 calendar days of the issuance date of this debit note";
                }
                else if (user.PaisID == 3 || user.PaisID == 23)
                {
                    texto_reclamo_documento = "Estimado Cliente, a partir de la fecha de emision de la nota de debito tiene 15 dias calendario para realizar cualquier reclamo sobre la misma";
                    texto_copia = "<tr><td align=\"center\"><p>Original Cliente - Copia Contabilidad</p><td><tr>";
                }
                else
                {
                    texto_reclamo_documento = "Estimado Cliente, a partir de la fecha de emision de la nota de debito tiene 15 dias calendario para realizar cualquier reclamo sobre la misma";
                }
                if (user.PaisID == 7)
                {
                    lbl_tipo_transaccion.Text = "Debit Note";
                }
                else if ((user.PaisID == 3 || user.PaisID == 23) && user.contaID == 2)
                {
                    lbl_tipo_transaccion.Text = "Debit Note";
                }
                else if (user.PaisID == 30)
                {
                    
                    texto_recordatorio_pago = "Dear customer, for payment of the service provided, please make an international bank draft / international check / wire transfer to the order of <b>\"World Maritime Transport, Ltd.\"</b>";
                    texto_recordatorio_pago += "</br></br>Estimado Cliente, para el pago de los servicios proporcionados , favor de realizar giro bancario internacional / cheque internacional / transferencia bancaria a nombre de <b>\"World Maritime Transport, Ltd.\"</b> Para los cargos Internacionales.";
                }
                else if (user.PaisID == 1)
                {
                    if (user.contaID == 1)
                    {
                        if (factdata.intC4 == 8)
                        {
                            lbl_tipo_transaccion.Text = "Recordatorio de Pago";
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
                            lbl_tipo_transaccion.Text = "Recordatorio de Pago";
                            texto_recordatorio_pago = "</br>Latin Freight, S.A. (Aimar) no puede emitir una Factura, Invoice o Nota de Debito por los fletes marítimos, aéreos o terrestres que son generados y operados por nuestros agentes en el extranjero según la ley del IVA Articulo 3 donde ampara que el hecho generador de IVA es la prestación de servicios en el territorio nacional.</br></br>"
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
                if (user.PaisID == 3 && (tipo_transaccion == 1 || tipo_transaccion == 3 || tipo_transaccion == 4) && factdata.intC4 == 8 && lbl_serie.Text != "000-004-07" && lbl_serie.Text != "000-002-01" && lbl_serie.Text != "000-003-07" && lbl_serie.Text != "000-002-06")
                {
                    //2021-01-28 bl_direccion_empresa.Text = "Agencia Internacional Maritima, S.A. de C.V.<br>Barrio El Centro, 1-2 calle oeste 2 avenida<br>PBX (504) 2564-0099/ 2668-0121  FAX (504) 2668-0353<br>Puerto Cortes, Cortes, Honduras, C.A.";
                }
            }
            else
            {
                strcorrelativo_doc = "-"+factdata.strC1;
            }

            if (1 == 1) //no habilitado temporalmente
            {
                /////////////////////////////////////////////
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
                /////////////////////////////////////////////
            }


			if (1 == 2) {
            //////////--VALIDACION DE RANGOS DE CORRELATIVOS POR DOCUMENTO PARA DETERMINAR QUE NUMERO DE CAI UTILIZAR--//////////
            #region Facturacion Honduras SAR
            if ((user.PaisID == 3) && (user.contaID == 1) && (user.SucursalID == 102))
            {
                if (factdata.intC4 == 8)
                {
                    if (tipo_transaccion == 1)
                    {
                        if (lbl_serie.Text =="000-002-01")
                        {
                            if (correlativo_doc >= 1001 && correlativo_doc <= 2500)
                            {
                                lbl_cai.Text = "CAI.: 08CFA0-BB7A0A-9B4A87-35D21C-A6A9C5-51";
                            }
                            else if (correlativo_doc >= 2501 && correlativo_doc <= 5000)
                            {
                                lbl_cai.Text = "CAI.: 43330C-B14DDB-F045B3-99951B-F0494A-44";
                            }
                            else if (correlativo_doc >= 5001 && correlativo_doc <= 7000)
                            {
                                lbl_cai.Text = "CAI.: 82E64A-45C8B5-4D49B3-416A9C-E3D127-B1";
                            }
                            else if (correlativo_doc >= 7001 && correlativo_doc <= 8500)
                            {
                                lbl_cai.Text = "CAI.: 594193-F0F5AF-C04C9C-B3D60B-68D7E2-EB";
                            }
                            else if (correlativo_doc >= 8501 && correlativo_doc <= 9200)
                            {
                                lbl_cai.Text = "CAI.: B2E130-6A8A3D-3D479E-6DD810-F3D17B-4A";
                            }
                            else
                            {
                                lbl_cai.Text = "CAI.: 000000000";
                            }
                        }
                        if (lbl_serie.Text == "001-001-01")
                        {
                            if (correlativo_doc >= 1 && correlativo_doc <= 50)
                            {
                                lbl_cai.Text = "CAI.: 696553-01C920-E54BB9-5FF522-1F72EA-8B";
                            }
                        }
                    }
                    if (tipo_transaccion == 3)
                    {
                        if (lbl_serie.Text == "000-002-06")
                        {
                            if (correlativo_doc >= 201 && correlativo_doc <= 500)
                            {
                                lbl_cai.Text = "CAI.: 481E9A-A8EDC1-1F4E9E-9AD2CA-A305C0-B4";
                            }
                            else if (correlativo_doc >= 501 && correlativo_doc <= 750)
                            {
                                lbl_cai.Text = "CAI.: 3FEEF0-89CB2C-084FBF-0160C1-C0E40B-49";
                            }
                            else if (correlativo_doc >= 751 && correlativo_doc <= 1750)
                            {
                                lbl_cai.Text = "CAI.: 5E7FA0-C97EDE-254484-458EFD-876B15-AF";
                            }
                            else if (correlativo_doc >= 1751 && correlativo_doc <= 1950)
                            {
                                lbl_cai.Text = "CAI.: EF5DA1-299F70-6E4EB6-774393-7924BF-4D";
                            }
                            else if (correlativo_doc >= 1951 && correlativo_doc <= 2100)
                            {
                                lbl_cai.Text = "CAI.: FE9800-6DBE6A-074C90-C37F3F-F1F86C-17";
                            }
                            else if (correlativo_doc >= 2101 && correlativo_doc <= 2300)
                            {
                                lbl_cai.Text = "CAI.: B2E130-6A8A3D-3D479E-6DD810-F3D17B-4A";
                            }
                            else
                            {
                                lbl_cai.Text = "CAI.: 0000000";
                            }
                        }
                        if (lbl_serie.Text == "001-001-06")
                        {
                            if (correlativo_doc >= 1 && correlativo_doc <= 50)
                            {
                                lbl_cai.Text = "CAI.: 340AE8-F0462E-D44F83-725B0D-507589-50";
                            }
                        }
                    }
                    if (tipo_transaccion == 4)
                    {
                        if (user.Operacion == 1)
                        {
                            if (lbl_serie.Text == "000-003-07")
                            {
                                if (correlativo_doc >= 301 && correlativo_doc <= 500)
                                {
                                    lbl_cai.Text = "CAI.: 7AF220-066584-8D44BF-0D8F4B-2B2E9F-B7";
                                }
                                else if (correlativo_doc >= 501 && correlativo_doc <= 1100)
                                {
                                    lbl_cai.Text = "CAI.: 1A1FFE-087698-E94A9A-B9679D-5EAC49-1B";
                                }
                                else if (correlativo_doc >= 1101 && correlativo_doc <= 1600)
                                {
                                    lbl_cai.Text = "CAI.: EF5DA1-299F70-6E4EB6-774393-7924BF-4D";
                                }
                                else if (correlativo_doc >= 1601 && correlativo_doc <= 2000)
                                {
                                    lbl_cai.Text = "CAI.: FE9800-6DBE6A-074C90-C37F3F-F1F86C-17";
                                }
                                else if (correlativo_doc >= 2001 && correlativo_doc <= 2400)
                                {
                                    lbl_cai.Text = "CAI.: B2E130-6A8A3D-3D479E-6DD810-F3D17B-4A";
                                }
                                else
                                {
                                    lbl_cai.Text = "CAI.: 000000";
                                }
                            }
                            if (lbl_serie.Text == "001-001-07")
                            {
                                if (correlativo_doc >= 1 && correlativo_doc <= 50)
                                {
                                    lbl_cai.Text = "CAI.: 340AE8-F0462E-D44F83-725B0D-507589-50";
                                }
                            }
                        }
                        if (user.Operacion == 2)
                        {
                            if (correlativo_doc >= 251 && correlativo_doc <= 750)
                            {
                                lbl_cai.Text = "CAI.: 1383CD-85FB9F-E140B7-D728C5-964F05-E2";
                            }
                            else if (correlativo_doc >= 751 && correlativo_doc <= 1350)
                            {
                                lbl_cai.Text = "CAI.: 1A1FFE-087698-E94A9A-B9679D-5EAC49-1B";
                            }
                            else if (correlativo_doc >= 1351 && correlativo_doc <= 1950)
                            {
                                lbl_cai.Text = "CAI.: EF5DA1-299F70-6E4EB6-774393-7924BF-4D";
                            }
                            else if (correlativo_doc >= 1951 && correlativo_doc <= 2250)
                            {
                                lbl_cai.Text = "CAI.: FE9800-6DBE6A-074C90-C37F3F-F1F86C-17";
                            }
                            else if (correlativo_doc >= 2251 && correlativo_doc <= 2400) //000-004-07
                            {
                                lbl_cai.Text = "CAI.: B2E130-6A8A3D-3D479E-6DD810-F3D17B-4A";
                            }
                            else
                            {
                                lbl_cai.Text = "CAI.: 0000000";
                            }
                        }
                    }
                    if (tipo_transaccion == 12)
                    {
                        if (user.Operacion == 2)
                        {
                            if (correlativo_doc >= 1 && correlativo_doc <= 50)
                            {
                                lbl_cai.Text = "CAI.: 08E867-59E7AA-E24CA7-47DB2D-C5F984-9D";
                            }
                            else if (correlativo_doc >= 51 && correlativo_doc <= 100)
                            {
                                lbl_cai.Text = "CAI.: 609027-00F9A1-6346A3-856477-817784-96";
                            }
                        }
                    }
                    if (tipo_transaccion == 31)
                    {
                        if (user.Operacion == 2)
                        {
                            if (correlativo_doc >= 1 && correlativo_doc <= 50)
                            {
                                lbl_cai.Text = "CAI.: C70F09-156C46-0643BB-962446-CD381C-10";
                            }
                            else if (correlativo_doc >= 51 && correlativo_doc <= 100)
                            {
                                lbl_cai.Text = "CAI.: 609027-00F9A1-6346A3-856477-817784-96";
                            }
                            else if (correlativo_doc >= 101 && correlativo_doc <= 140)
                            {
                                lbl_cai.Text = "CAI.: B79486-154DD4-B94185-5F0ABB-084129-CF";
                            }
                            else if (correlativo_doc >= 141 && correlativo_doc <= 190)
                            {
                                lbl_cai.Text = "CAI.: 806432-D708E2-6C4985-A9794B-41E1AE-E4";
                            }
                            else if (correlativo_doc >= 191 && correlativo_doc <= 220)
                            {
                                lbl_cai.Text = "CAI.: ABAF15-F02CF0-FD44A8-E690B3-CFB5EA-CF";
                            }
                            else if (correlativo_doc >= 221 && correlativo_doc <= 250) //000-004-06
                            {
                                lbl_cai.Text = "CAI.: B2E130-6A8A3D-3D479E-6DD810-F3D17B-4A";
                            }
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
                        else if ((Tipo_Transaccion == 1 && (correlativo_doc >= 15001 && correlativo_doc <= 19500)) || (Tipo_Transaccion == 3 && (correlativo_doc >= 1101 && correlativo_doc <= 1600)) || (Tipo_Transaccion == 4 && (correlativo_doc >= 2201 && correlativo_doc <= 2800)))
                        {
                            lbl_cai.Text = "CAI .: 481758-33D416-4D4282-6CDBFA-794662-7A";
                        }
                        else if (Tipo_Transaccion == 1 && (correlativo_doc >= 19501 && correlativo_doc <= 22500))
                        {
                            lbl_cai.Text = "CAI .: BB3FC6-BFD640-22458B-EBE37D-BB1DFF-6D";
                        }
						else if (Tipo_Transaccion == 1 && (correlativo_doc >= 22501 && correlativo_doc <= 25000))
                        {
                            lbl_cai.Text = "CAI .: FAF59D-970E11-11468D-AF1E21-89889F-D3 "; 
                        }
                        else if (Tipo_Transaccion == 1 && (correlativo_doc >= 25001 && correlativo_doc <= 27500))
                        {
                            lbl_cai.Text = "CAI .: 004BBD-13769B-1843B7-E40BDD-F80155-E2 ";
                        }
                        else if (Tipo_Transaccion == 3 && correlativo_doc >= 1601 && correlativo_doc <= 1850)
                        {
                            lbl_cai.Text = "CAI .: 63357D-3D1FCC-0E4881-B63A9D-917248-62";
                        }
                        else if (Tipo_Transaccion == 3 && correlativo_doc >= 1851 && correlativo_doc <= 1950)
                        {
                            lbl_cai.Text = "CAI .: 5D3953-67C87E-3E4686-374234-70176B-DB";
                        }
                        else if (Tipo_Transaccion == 3 && correlativo_doc >= 1951 && correlativo_doc <= 2000)
                        {
                            lbl_cai.Text = "CAI .: 9CE157-A12DED-0C4291-622470-2E7732-F7";
                        }
                        else if (Tipo_Transaccion == 3 && correlativo_doc >= 2001 && correlativo_doc <= 2100) //000-001-06
                        {
                            lbl_cai.Text = "CAI .: E917B2-42B0E4-C24DB3-9E217E-A8536A-8C";
                        }
                        else if (Tipo_Transaccion == 3 && correlativo_doc >= 2101 && correlativo_doc <= 2300) //000-001-06
                        {
                            lbl_cai.Text = "CAI .: B2E130-6A8A3D-3D479E-6DD810-F3D17B-4A";
                        }
                        else
                        {
                            if ((Tipo_Transaccion == 4) && (correlativo_doc >= 1001) && (correlativo_doc <= 1600))
                            {
                                lbl_cai.Text = "CAI.: 6040EC-C37B99-0D43B1-B4A85F-E93234-39";
                            }
                            else if ((Tipo_Transaccion == 4) && (correlativo_doc >= 1601) && (correlativo_doc <= 2200))
                            {
                                lbl_cai.Text = "CAI.: 87A0AA-1E022E-EF48A9-F12E51-2C95D3-C0";
                            }
                            else if ((Tipo_Transaccion == 4) && (correlativo_doc >= 2801) && (correlativo_doc <= 3200))
                            {
                                lbl_cai.Text = "CAI.: EA7E6E-54C984-06408B-859591-5131BD-35";
                            }
                            else if ((Tipo_Transaccion == 4) && (correlativo_doc >= 3201) && (correlativo_doc <= 3400))
                            {
                                lbl_cai.Text = "CAI.: 5D3953-67C87E-3E4686-374234-70176B-DB";
                            }
                            else if ((Tipo_Transaccion == 4) && (correlativo_doc >= 3401) && (correlativo_doc <= 3600))
                            {
                                lbl_cai.Text = "CAI.: 234C1F-D0143B-764FAB-50429A-524996-33";
                            }
                            else if (Tipo_Transaccion == 4 && correlativo_doc >= 3601 && correlativo_doc <= 4000) //000-001-07
                            {
                                lbl_cai.Text = "CAI.: B2E130-6A8A3D-3D479E-6DD810-F3D17B-4A";
                            }
                            else
                            {
                                lbl_cai.Text = "CAI.: 0000000000";
                            }
                        }
                    }
                    if (user.Operacion == 2)
                    {
                        lbl_cai.Text = "CAI.: 7D334A-A3EE3E-A54B80-7BE4FB-E3286E-8C";
                    }
                }
                lbl_email_empresa.Text = "info@aimargroup.com";
                factura_beneficio = "</br><b>LA FACTURA ES BENEFICIO DE TODOS,  EXIJALA!!</b></br></br>";
            }

            if ((user.PaisID == 23) && (user.SucursalID == 104) && (user.contaID == 1))
            {
                if (factdata.intC4 == 8)
                {
                    if (tipo_transaccion == 1)
                    {
                        if (correlativo_doc >= 301 && correlativo_doc <= 550)
                        {
                            lbl_cai.Text = "CAI.: EB45CC-689A96-4442BB-F36423-45376B-5A";
                        }
                        else if (correlativo_doc >= 551 && correlativo_doc <= 1550)
                        {
                            lbl_cai.Text = "CAI.: 1E5B80-9A11C7-7E4C8F-54744A-82A0A3-1C";
                        }
                        else if (correlativo_doc >= 1551 && correlativo_doc <= 1900)
                        {
                            lbl_cai.Text = "CAI.: DB4B4C-C689C9-EF4DAF-361ED9-22A5F4-1F";
                        }
                        else if (correlativo_doc >= 1901 && correlativo_doc <= 2200) //000-002-01
                        {
                            lbl_cai.Text = "CAI.: 7B3509-821FCA-494AB9-0E45CC-EFAA48-4E";
                        }
                        else if (correlativo_doc >= 2201 && correlativo_doc <= 2350) //000-002-01
                        {
                            lbl_cai.Text = "CAI.: 05CFCB-3CBB6A-554796-4E9534-B26DF4-4D";
                        }
                        else
                        {
                            lbl_cai.Text = "CAI.: 000000";
                        }
                    }

                    if (tipo_transaccion == 3)
                    {

                        if (lbl_serie.Text == "000-002-06")
                        {

                            if (correlativo_doc >= 101 && correlativo_doc <= 150)
                            {
                                lbl_cai.Text = "CAI.: D474BD-C067F5-B94780-60B7DA-5224AB-B8";
                            }
                            else if (correlativo_doc >= 51 && correlativo_doc <= 100)
                            {
                                lbl_cai.Text = "CAI.: EA7C28-9AD479-3441AE-3C12F3-0F38A4-77";
                            }
                            else if (correlativo_doc >= 151 && correlativo_doc <= 170) //2019-07-24 NC LTF HN 104
                            {
                                lbl_cai.Text = "CAI.: 44DAC3-107DE6-CF4E9E-F49DD1-9BB04B-13";
                            }

                        }


                        if (lbl_serie.Text == "000-003-06") //serie nueva se crea 2021-01-06
                        {
                            if (correlativo_doc >= 1 && correlativo_doc <= 30) //000-003-06
                            {
                                lbl_cai.Text = "CAI.: 05CFCB-3CBB6A-554796-4E9534-B26DF4-4D";
                            }
                        }


                    }
                    if (tipo_transaccion == 4)
                    {
                        if (user.Operacion == 1)
                        {
                            if (correlativo_doc >= 51 && correlativo_doc <= 60)
                            {
                                lbl_cai.Text = "CAI.: BD5443-9808C0-EE4EA2-5D907E-4C1DCF-5A";
                            }
                            else if (correlativo_doc >= 61 && correlativo_doc <= 70)
                            {
                                lbl_cai.Text = "CAI.: F4EF1D-0F77BF-FE4098-29BA78-63B766-78";
                            }
                            else if (correlativo_doc >= 71 && correlativo_doc <= 80) //000-003-07
                            {
                                lbl_cai.Text = "CAI.: 7B3509-821FCA-494AB9-0E45CC-EFAA48-4E";
                            }
                            else if (correlativo_doc >= 81 && correlativo_doc <= 110) //000-003-07
                            {
                                lbl_cai.Text = "CAI.: 05CFCB-3CBB6A-554796-4E9534-B26DF4-4D";
                            }
                            else
                            {
                                lbl_cai.Text = "CAI.: 0000000";
                            }
                        }
                        if (user.Operacion == 2)
                        {
                            if (correlativo_doc >= 51 && correlativo_doc <= 120)
                            {
                                lbl_cai.Text = "CAI.: EB45CC-689A96-4442BB-F36423-45376B-5A";
                            }
                            else if (correlativo_doc >= 121 && correlativo_doc <= 220)
                            {
                                lbl_cai.Text = "CAI.: 1E5B80-9A11C7-7E4C8F-54744A-82A0A3-1C";
                            }
                            else if (correlativo_doc >= 221 && correlativo_doc <= 240)
                            {
                                lbl_cai.Text = "CAI.: F6F7C1-45F45B-37409C-0B36D1-BA7902-97";
                            }
                            else if (correlativo_doc >= 241 && correlativo_doc <= 290) //2019-07-24 ND LTF HN 104
                            {
                                lbl_cai.Text = "CAI.: DB4B4C-C689C9-EF4DAF-361ED9-22A5F4-1F";
                            }
                            else if (correlativo_doc >= 291 && correlativo_doc <= 305) 
                            {
                                lbl_cai.Text = "CAI.: 44DAC3-107DE6-CF4E9E-F49DD1-9BB04B-13";
                            }
                            else if (correlativo_doc >= 306 && correlativo_doc <= 315) //000-004-07
                            {
                                lbl_cai.Text = "CAI.: 05CFCB-3CBB6A-554796-4E9534-B26DF4-4D";
                            }
                            else
                            {
                                lbl_cai.Text = "CAI.: 0000000";
                            }
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
                        else if ((Tipo_Transaccion == 1) && (correlativo_doc >= 2401) && (correlativo_doc <= 2900))
                        {
                            lbl_cai.Text = "CAI.: FD875B-F27806-9F41A4-039BB4-A4806A-BD";
                        }
                        else if ((Tipo_Transaccion == 1) && (correlativo_doc >= 2901) && (correlativo_doc <= 3000))
                        {
                            lbl_cai.Text = "CAI.: 0C39AA-7874BF-014D8C-05452F-267F47-C9";
                        }
                        else if ((Tipo_Transaccion == 1) && (correlativo_doc >= 3001) && (correlativo_doc <= 3050))
                        {
                            lbl_cai.Text = "CAI.: 666489-6363F3-F94184-D32956-560212-54";
                        }
                        else if ((Tipo_Transaccion == 1) && (correlativo_doc >= 3051) && (correlativo_doc <= 3150)) //000-001-01
                        {
                            lbl_cai.Text = "CAI.: 7B3509-821FCA-494AB9-0E45CC-EFAA48-4E";
                        }
                        else if ((Tipo_Transaccion == 1) && (correlativo_doc >= 3151) && (correlativo_doc <= 3300)) //000-001-01
                        {
                            lbl_cai.Text = "CAI.: 05CFCB-3CBB6A-554796-4E9534-B26DF4-4D";
                        }
                        else if (Tipo_Transaccion == 3)
                        {
                            if (correlativo_doc >= 251 && correlativo_doc <= 300)
                            {
                                lbl_cai.Text = "CAI.: 39353B-2C995C-8649B0-0C76CE-B13F5B-2D";
                            }
                            else if (correlativo_doc >= 301 && correlativo_doc <= 330)
                            {
                                lbl_cai.Text = "CAI.: A1A891-8BB1FB-BA469E-69294A-1D1FB5-A0";
                            }
                            else if (correlativo_doc >= 331 && correlativo_doc <= 340)
                            {
                                lbl_cai.Text = "CAI.: 34F2A8-AA302D-0E4BB3-AE3866-1AB715-A5";
                            }
                            else if (correlativo_doc >= 341 && correlativo_doc <= 360)
                            {
                                lbl_cai.Text = "CAI.: 0FCA23-99A10-8D40A2-OFA020-6282EA-94";
                            }
                            else if (correlativo_doc >= 361 && correlativo_doc <= 400) //000-001-06
                            {
                                lbl_cai.Text = "CAI.: 05CFCB-3CBB6A-554796-4E9534-B26DF4-4D";
                            }
                            else
                            {
                                lbl_cai.Text = "CAI.: 0000000";
                            }
                        }
                        else if (Tipo_Transaccion == 4 && (correlativo_doc >= 621 && correlativo_doc <= 650))
                        {
                            lbl_cai.Text = "CAI.: BF135F-0686C8-5E468D-2F5999-5085E7-A0";
                        }
                        else if (Tipo_Transaccion == 4 && (correlativo_doc >= 651 && correlativo_doc <= 670))
                        {
                            lbl_cai.Text = "CAI.: F6F7C1-45F45B-37409C-0B36D1-BA7902-97";
                        }
                        else if (Tipo_Transaccion == 4 && (correlativo_doc >= 671 && correlativo_doc <= 685)) //2019-07-24 ND LTF HN 104
                        {
                            lbl_cai.Text = "CAI.: 44DAC3-107DE6-CF4E9E-F49DD1-9BB04B-13";
                        }
                        else if (Tipo_Transaccion == 4 && (correlativo_doc >= 686 && correlativo_doc <= 700)) //000-001-07
                        {
                            lbl_cai.Text = "CAI.: 05CFCB-3CBB6A-554796-4E9534-B26DF4-4D";
                        }
                        else
                        {
                            lbl_cai.Text = "CAI.: 000000";
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
                        if (lbl_serie.Text == "000-002-01")
                        {
                            if (correlativo_doc >= 1001 && correlativo_doc <= 2500)
                            {
                                fecha_emision = "Fecha límite de emisión:  <b>30/03/2018</b>";
                            }
                            else if (correlativo_doc >= 2501 && correlativo_doc <= 5000)
                            {
                                fecha_emision = "Fecha límite de emisión:  <b>06/12/2018</b>";
                            }
                            else if (correlativo_doc >= 5001 && correlativo_doc <= 7000)
                            {
                                fecha_emision = "Fecha límite de emisión:  <b>06/12/2019</b>";
                            }
                            else if (correlativo_doc >= 7001 && correlativo_doc <= 8500)
                            {
                                fecha_emision = "Fecha límite de emisión:  <b>12/12/2020</b>";
                            }
                            else if (correlativo_doc >= 8501 && correlativo_doc <= 9200)
                            {
                                fecha_emision = "Fecha límite de emisión:  <b>04/01/2022</b>";
                            }
                            else
                            {
                                fecha_emision = "Fecha límite de emisión:  <b>00/00/0000</b>";
                            }
                        }
                        if (lbl_serie.Text == "001-001-01")
                        {
                            if (correlativo_doc >= 1 && correlativo_doc <= 50)
                            {
                                fecha_emision = "Fecha límite de emisión: <b>17/08/2019</b>";
                            }
                        }
                    }
                    if (tipo_transaccion == 3)
                    {
                        if (lbl_serie.Text == "000-002-06")
                        {
                            if (correlativo_doc >= 201 && correlativo_doc <= 500)
                            {
                                fecha_emision = "Fecha límite de emisión:  <b>12/04/2018</b>";
                            }
                            else if (correlativo_doc >= 501 && correlativo_doc <= 750)
                            {
                                fecha_emision = "Fecha límite de emisión: <b>22/04/2018</b>";
                            }
                            else if (correlativo_doc >= 751 && correlativo_doc <= 1750)
                            {
                                fecha_emision = "Fecha límite de emisión: <b>11/10/2018</b>";
                            }
                            else if (correlativo_doc >= 1751 && correlativo_doc <= 1950)
                            {
                                fecha_emision = "Fecha límite de emisión: <b>17/10/2019</b>";
                            }
                            else if (correlativo_doc >= 1951 && correlativo_doc <= 2100)
                            {
                                fecha_emision = "Fecha límite de emisión: <b>18/10/2020</b>";
                            }
                            else if (correlativo_doc >= 2101 && correlativo_doc <= 2300)
                            {
                                fecha_emision = "Fecha límite de emisión: <b>04/01/2022</b>";
                            }
                            else
                            {
                                fecha_emision = "Fecha límite de emisión:  <b>00/00/0000</b>";
                            }
                        }
                        if (lbl_serie.Text == "001-001-06")
                        {
                            if (correlativo_doc >=1 && correlativo_doc <= 50)
                            {
                                fecha_emision = "Fecha límite de emisión:  <b>27/08/2019</b>";
                            }
                        }

                    }
                    if (tipo_transaccion == 4)
                    {
                        if (user.Operacion == 1)
                        {
                            if (lbl_serie.Text == "000-003-07")
                            {
                                if (correlativo_doc >= 301 && correlativo_doc <= 500)
                                {
                                    fecha_emision = "Fecha límite de emisión:  <b>22/06/2018</b>";
                                }
                                else if (correlativo_doc >= 501 && correlativo_doc <= 1100)
                                {
                                    fecha_emision = "Fecha límite de emisión:  <b>13/10/2018</b>";
                                }
                                else if (correlativo_doc >= 1101 && correlativo_doc <= 1600)
                                {
                                    fecha_emision = "Fecha límite de emisión:  <b>17/10/2019</b>";
                                }
                                else if (correlativo_doc >= 1601 && correlativo_doc <= 2000)
                                {
                                    fecha_emision = "Fecha límite de emisión:  <b>18/10/2020</b>";
                                }
                                else if (correlativo_doc >= 2001 && correlativo_doc <= 2400)
                                {
                                    fecha_emision = "Fecha límite de emisión:  <b>04/01/2022</b>";
                                }
                                else
                                {
                                    fecha_emision = "Fecha límite de emisión:  <b>00/00/0000</b>";
                                }
                            }
                            if (lbl_serie.Text == "001-001-07")
                            {
                                if (correlativo_doc >= 1 && correlativo_doc <= 50)
                                {
                                    fecha_emision = "Fecha límite de emisión:  <b>27/08/2019</b>";
                                }
                            }
                        }
                        if (user.Operacion == 2)
                        {
                            if (correlativo_doc >= 251 && correlativo_doc <= 750)
                            {
                                fecha_emision = "Fecha límite de emisión:  <b>26/01/2018</b>";
                            }
                            else if (correlativo_doc >= 751 && correlativo_doc <= 1350)
                            {
                                fecha_emision = "Fecha límite de emisión:  <b>13/10/2018</b>";
                            }
                            else if (correlativo_doc >= 1351 && correlativo_doc <= 1950)
                            {
                                fecha_emision = "Fecha límite de emisión:  <b>17/10/2019</b>";
                            }
                            else if (correlativo_doc >= 1951 && correlativo_doc <= 2250)
                            {
                                fecha_emision = "Fecha límite de emisión:  <b>18/10/2020</b>";
                            }
                            else if (correlativo_doc >= 2251 && correlativo_doc <= 2400) //000-004-07
                            {
                                fecha_emision = "Fecha límite de emisión:  <b>04/01/2022</b>";
                            }
                            else
                            {
                                fecha_emision = "Fecha límite de emisión:  <b>00/00/0000</b>";
                            }
                        }
                    }
                    if (tipo_transaccion == 12)
                    {
                        if (user.Operacion == 2)
                        {
                            if (correlativo_doc >= 1 && correlativo_doc <= 50)
                            {
                                fecha_emision = "Fecha límite de emisión:  <b>24/10/2017</b>";
                            }
                            else if (correlativo_doc >= 51 && correlativo_doc <= 100)
                            {
                                fecha_emision = "Fecha límite de emisión:  <b>26/01/2018</b>";
                            }
                        }
                    }
                    if (tipo_transaccion == 31)
                    {
                        if (user.Operacion == 2)
                        {
                            if (correlativo_doc >= 1 && correlativo_doc <= 50)
                            {
                                fecha_emision = "Fecha límite de emisión:  <b>24/10/2017</b>";
                            }
                            else if (correlativo_doc >= 51 && correlativo_doc <= 100)
                            {
                                fecha_emision = "Fecha límite de emisión:  <b>26/01/2018</b>";
                            }
                            else if (correlativo_doc >= 101 && correlativo_doc <= 140)
                            {
                                fecha_emision = "Fecha límite de emisión:  <b>30/01/2019</b>";
                            }
                            else if (correlativo_doc >= 141 && correlativo_doc <= 190)
                            {
                                fecha_emision = "Fecha límite de emisión:  <b>27/08/2019</b>";
                            }
                            else if (correlativo_doc >= 191 && correlativo_doc <= 220)
                            {                                
                                fecha_emision = "Fecha límite de emisión:  <b>22/10/2020</b>";
                            }
                            else if (correlativo_doc >= 221 && correlativo_doc <= 250) //000-004-06
                            {
                                fecha_emision = "Fecha límite de emisión:  <b>04/01/2022</b>";
                            }
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
                        else if ((Tipo_Transaccion == 1 && (correlativo_doc >= 15001 && correlativo_doc <= 19500)) || (Tipo_Transaccion == 3 && (correlativo_doc >= 1101 && correlativo_doc <= 1600)) || (Tipo_Transaccion == 4 && (correlativo_doc >= 2201 && correlativo_doc <= 2800)))
                        {
                            fecha_emision = "Fecha límite de emisión:  <b>20/03/2018</b>";
                        }
                        else if (tipo_transaccion == 1 && (correlativo_doc >= 19501 && correlativo_doc <= 22500))
                        {
                            fecha_emision = "Fecha límite de emisión: <b>20/03/2019</b>";
                        }
						else if (tipo_transaccion == 1 && (correlativo_doc >= 22501 && correlativo_doc <= 25000))
                        {
                            fecha_emision = "Fecha límite de emisión: <b>21/03/2020</b>";
                        }
                        else if (tipo_transaccion == 1 && (correlativo_doc >= 25001 && correlativo_doc <= 27500))
                        {
                            fecha_emision = "Fecha límite de emisión: <b>25/03/2021</b>";
                        }
                        else if (tipo_transaccion == 3 && (correlativo_doc >= 1601 && correlativo_doc <= 1850))
                        {
                            fecha_emision = "Fecha límite de emisión: <b>27/03/2019</b>";
                        }
                        else if (tipo_transaccion == 3 && (correlativo_doc >= 1851 && correlativo_doc <= 1950))
                        {
                            fecha_emision = "Fecha límite de emisión: <b>28/03/2020</b>";
                        }                        
                        else if (tipo_transaccion == 3 && (correlativo_doc >= 1951 && correlativo_doc <= 2000))
                        {
                            fecha_emision = "Fecha límite de emisión: <b>08/10/2020</b>";
                        }
                        else if (tipo_transaccion == 3 && (correlativo_doc >= 2001 && correlativo_doc <= 2100)) //000-001-06
                        {
                            fecha_emision = "Fecha límite de emisión: <b>10/01/2021</b>";
                        }
                        else if (tipo_transaccion == 3 && correlativo_doc >= 2101 && correlativo_doc <= 2300) //000-001-06
                        {
                            fecha_emision = "Fecha límite de emisión: <b>04/01/2022</b>";
                        }
                        else
                        {
                            if ((Tipo_Transaccion == 4) && (correlativo_doc >= 1001) && (correlativo_doc <= 1600))
                            {
                                fecha_emision = "Fecha límite de emisión:  <b>23/03/2017</b>";
                            }
                            else if ((Tipo_Transaccion == 4) && (correlativo_doc >= 1601) && (correlativo_doc <= 2200))
                            {
                                fecha_emision = "Fecha límite de emisión:  <b>06/01/2018</b>";
                            }
                            else if ((Tipo_Transaccion == 4) && (correlativo_doc >= 2801) && (correlativo_doc <= 3200))
                            {
                                fecha_emision = "Fecha límite de emisión:  <b>27/03/2019</b>";
                            }
                            else if ((Tipo_Transaccion == 4) && (correlativo_doc >= 3201) && (correlativo_doc <= 3400))
                            {
                                fecha_emision = "Fecha límite de emisión:  <b>28/03/2020</b>";
                            }
                            else if ((Tipo_Transaccion == 4) && (correlativo_doc >= 3401) && (correlativo_doc <= 3600))
                            {
                                fecha_emision = "Fecha límite de emisión:  <b>12/12/2020</b>";
                            }
                            else if (Tipo_Transaccion == 4 && correlativo_doc >= 3601 && correlativo_doc <= 4000) //000-001-07
                            {
                                fecha_emision = "Fecha límite de emisión:  <b>04/01/2022</b>";
                            }
                            else
                            {
                                fecha_emision = "Fecha límite de emisión:  <b>00/00/0000</b>";
                            }
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
                        if (lbl_serie.Text == "001-001-01" && correlativo_doc >= 1 && correlativo_doc <= 50)
                        {
                            rango_autorizado = "Rango autorizado: Del <b>001-001-01-00000001</b> Al <b>001-001-01-00000050</b>";
                        }
                        else if (correlativo_doc >= 1001 && correlativo_doc <= 2500)
                        {
                            rango_autorizado = "Rango autorizado: Del <b>000-002-01-00001001</b> Al <b>000-002-01-00002500</b>";
                        }
                        else if (correlativo_doc >= 2501 && correlativo_doc <= 5000)
                        {
                            rango_autorizado = "Rango autorizado: Del <b>000-002-01-00002501</b> Al <b>000-002-01-00005000</b>";
                        }
                        else if (correlativo_doc >= 5001 && correlativo_doc <= 7000)
                        {
                            rango_autorizado = "Rango autorizado: Del <b>000-002-01-00005001</b> Al <b>000-002-01-00007000</b>";
                        }
                        else if (correlativo_doc >= 7001 && correlativo_doc <= 8500)
                        {
                            rango_autorizado = "Rango autorizado: Del <b>000-002-01-00007001</b> Al <b>000-002-01-00008500</b>";
                        }
                        else if (correlativo_doc >= 8501 && correlativo_doc <= 9200)
                        {
                            rango_autorizado = "Rango autorizado: Del <b>000-002-01-00008501</b> Al <b>000-002-01-00009200</b>";
                        }
                        else
                        {
                            rango_autorizado = "Rango autorizado: Del <b>000</b> Al <b>000</b>";
                        }
                    }
                    else
                    {
                        if (correlativo_doc <= 9000)
                        {
                            rango_autorizado = "Rango autorizado: Del <b>000-001-01-00000001</b> Al <b>000-001-01-00009000</b>";
                        }
                        else if (correlativo_doc >= 15001 && correlativo_doc <= 19500)
                        {
                            rango_autorizado = "Rango autorizado: Del <b>000-001-01-00015001</b> Al <b>000-001-01-00019500</b>";
                        }
                        else if (correlativo_doc >= 19501 && correlativo_doc <= 22500)
                        {
                            rango_autorizado = "Rango autorizado: Del <b>000-001-01-00019501</b> Al <b>000-001-01-00022500</b>";
                        }
                        else if (correlativo_doc >= 22501 && correlativo_doc <= 25000)
                        {
                            rango_autorizado = "Rango autorizado: Del <b>000-001-01-00022501</b> Al <b>000-001-01-00025000</b>";
                        }
                        else if (correlativo_doc >= 25001 && correlativo_doc <= 27500)
                        {
                            rango_autorizado = "Rango autorizado: Del <b>000-001-01-00025001</b> Al <b>000-001-01-00027500</b>";
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
                        if (lbl_serie.Text == "000-002-06")
                        {
                            if (correlativo_doc >= 201 && correlativo_doc <= 500)
                            {
                                rango_autorizado = "Rango autorizado: Del <b>000-002-06-00000201</b> Al <b>000-002-06-00000500</b>";
                            }
                            else if (correlativo_doc >= 501 && correlativo_doc <= 750)
                            {
                                rango_autorizado = "Rango autorizado: Del <b>000-002-06-00000501</b> Al <b>000-002-06-00000750</b>";
                            }
                            else if (correlativo_doc >= 751 && correlativo_doc <= 1750)
                            {
                                rango_autorizado = "Rango autorizado: Del <b>000-002-06-00000751</b> Al <b>000-002-06-00001750</b>";
                            }
                            else if (correlativo_doc >= 1751 && correlativo_doc <= 1950)
                            {
                                rango_autorizado = "Rango autorizado: Del <b>000-002-06-00001751</b> Al <b>000-002-06-00001950</b>";
                            }
                            else if (correlativo_doc >= 1951 && correlativo_doc <= 2100)
                            {
                                rango_autorizado = "Rango autorizado: Del <b>000-002-06-00001951</b> Al <b>000-002-06-00002100</b>";
                            }
                            else if (correlativo_doc >= 2101 && correlativo_doc <= 2300)
                            {
                                rango_autorizado = "Rango autorizado: Del <b>000-002-06-00002101</b> Al <b>000-002-06-00002300</b>";
                            }
                            else
                            {
                                rango_autorizado = "Rango autorizado: Del <b>000</b> Al <b>000</b>";
                            }
                        }
                        if (lbl_serie.Text == "001-001-06")
                        {
                            if (correlativo_doc >= 1 && correlativo_doc <= 50)
                            {
                                rango_autorizado = "Rango autorizado: Del <b>001-001-06-00000001</b> Al <b>001-001-06-00000050</b>";
                            }
                        }
                    }
                    else
                    {
                        if (correlativo_doc <= 600)
                        {
                            rango_autorizado = "Rango autorizado: Del <b>000-001-06-00000001</b> Al <b>000-001-06-00000600</b>";
                        }
                        else if(correlativo_doc >= 1101 && correlativo_doc <= 1600)
                        {
                            rango_autorizado = "Rango autorizado: Del <b>000-001-06-00001101</b> Al <b>000-001-06-00001600</b>";
                        }
                        else if (correlativo_doc >= 1601 && correlativo_doc <= 1850)
                        {
                            rango_autorizado = "Rango autorizado: Del <b>000-001-06-00001601</b> Al <b>000-001-06-00001850</b>";
                        }
                        else if (correlativo_doc >= 1851 && correlativo_doc <= 1950)
                        {
                            rango_autorizado = "Rango autorizado: Del <b>000-001-06-00001851</b> Al <b>000-001-06-00001950</b>";
                        }
                        else if (correlativo_doc >= 1951 && correlativo_doc <= 2000)
                        {
                            rango_autorizado = "Rango autorizado: Del <b>000-001-06-00001951</b> Al <b>000-001-06-00002000</b>";
                        }
                        else if (correlativo_doc >= 2001 && correlativo_doc <= 2100)
                        {
                            rango_autorizado = "Rango autorizado: Del <b>000-001-06-00002001</b> Al <b>000-001-06-00002100</b>";
                        }
                        else if (correlativo_doc >= 2101 && correlativo_doc <= 2300) //000-001-06
                        {
                            rango_autorizado = "Rango autorizado: Del <b>000-001-06-00002101</b> Al <b>000-001-06-00002300</b>";
                        }
                        else
                        {
                            rango_autorizado = "Rango autorizado: Del <b>000-001-06-00000000</b> Al <b>000-001-06-00000000</b>";
                        } 
                    }
                }
                if (tipo_transaccion == 12)//Nota Credito Directa a Proveedores
                {
                    if (factdata.intC4 == 8)
                    {
                        if (correlativo_doc >= 1 && correlativo_doc <= 50)
                        {
                            rango_autorizado = "Rango autorizado: Del <b>000-003-06-00000001</b> Al <b>000-003-06-00000050</b>";
                        }
                        else if (correlativo_doc >= 51 && correlativo_doc <= 100)
                        {
                            rango_autorizado = "Rango autorizado: Del <b>000-003-06-00000051</b> Al <b>000-003-06-00000100</b>";
                        }
                    }
                }
                if (tipo_transaccion == 31)//Nota Credito a Nota Debito a Proveedores
                {
                    if (factdata.intC4 == 8)
                    {
                        if (correlativo_doc >= 1 && correlativo_doc <= 50)
                        {
                            rango_autorizado = "Rango autorizado: Del <b>000-004-06-00000001</b> Al <b>000-004-06-00000050</b>";
                        }
                        else if (correlativo_doc >= 51 && correlativo_doc <= 100)
                        {
                            rango_autorizado = "Rango autorizado: Del <b>000-004-06-00000051</b> Al <b>000-004-06-00000100</b>";
                        }
                        else if (correlativo_doc >= 101 && correlativo_doc <= 140)
                        {
                            rango_autorizado = "Rango autorizado: Del <b>000-004-06-00000101</b> Al <b>000-004-06-00000140</b>";
                        }
                        else if (correlativo_doc >= 141 && correlativo_doc <= 190)
                        {
                            rango_autorizado = "Rango autorizado: Del <b>000-004-06-00000141</b> Al <b>000-004-06-00000190</b>";
                        }
                        else if (correlativo_doc >= 191 && correlativo_doc <= 220)
                        {
                            rango_autorizado = "Rango autorizado: Del <b>000-004-06-00000191</b> Al <b>000-004-06-00000220</b>";
                        }
                        else if (correlativo_doc >= 221 && correlativo_doc <= 250) //000-004-06
                        {
                            rango_autorizado = "Rango autorizado: Del <b>000-004-06-00000221</b> Al <b>000-004-06-00000250</b>";
                        }
                    }
                }
                if (tipo_transaccion == 4)//Nota Debito
                {
                    if (factdata.intC4 == 8)
                    {
                        if (user.Operacion == 1)
                        {
                            if (lbl_serie.Text == "000-003-07")
                            {
                                if (correlativo_doc >= 301 && correlativo_doc <= 500)
                                {
                                    rango_autorizado = "Rango autorizado: Del <b>000-003-07-00000301</b> Al <b>000-003-07-00000500</b>";
                                }
                                else if (correlativo_doc >= 501 && correlativo_doc <= 1100)
                                {
                                    rango_autorizado = "Rango autorizado: Del <b>000-003-07-00000501</b> Al <b>000-003-07-00001100</b>";
                                }
                                else if (correlativo_doc >= 1101 && correlativo_doc <= 1600)
                                {
                                    rango_autorizado = "Rango autorizado: Del <b>000-003-07-00001101</b> Al <b>000-003-07-00001600</b>";
                                }
                                else if (correlativo_doc >= 1601 && correlativo_doc <= 2000)
                                {
                                    rango_autorizado = "Rango autorizado: Del <b>000-003-07-00001601</b> Al <b>000-003-07-00002000</b>";
                                }
                                else if (correlativo_doc >= 2001 && correlativo_doc <= 2400)
                                {
                                    rango_autorizado = "Rango autorizado: Del <b>000-003-07-00002001</b> Al <b>000-003-07-00002400</b>";
                                }
                                else
                                {
                                    rango_autorizado = "Rango autorizado: Del <b>0000</b> Al <b>000</b>";
                                }
                            }
                            if (lbl_serie.Text == "001-001-07")
                            {
                                if (correlativo_doc >= 1 && correlativo_doc <= 50)
                                {
                                    rango_autorizado = "Rango autorizado: Del <b>001-001-07-00000001</b> Al <b>001-001-07-00000050</b>";
                                }
                            }
                        }
                        if (user.Operacion == 2)
                        {
                            if (correlativo_doc >= 251 && correlativo_doc <= 750)
                            {
                                rango_autorizado = "Rango autorizado: Del <b>000-004-07-00000251</b> Al <b>000-004-07-00000750</b>";
                            }
                            else if (correlativo_doc >= 751 && correlativo_doc <= 1350)
                            {
                                rango_autorizado = "Rango autorizado: Del <b>000-004-07-00000751</b> Al <b>000-004-07-00001350</b>";
                            }
                            else if (correlativo_doc >= 1351 && correlativo_doc <= 1950)
                            {
                                rango_autorizado = "Rango autorizado: Del <b>000-004-07-00001351</b> Al <b>000-004-07-00001950</b>";
                            }
                            else if (correlativo_doc >= 1951 && correlativo_doc <= 2250)
                            {
                                rango_autorizado = "Rango autorizado: Del <b>000-004-07-00001951</b> Al <b>000-004-07-00002250</b>";
                            }
                            else if (correlativo_doc >= 2251 && correlativo_doc <= 2400)
                            {
                                rango_autorizado = "Rango autorizado: Del <b>000-004-07-00002251</b> Al <b>000-004-07-00002400</b>";
                            }
                            else
                            {
                                rango_autorizado = "Rango autorizado: Del <b>000</b> Al <b>000</b>";
                            }
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
                                if ((Tipo_Transaccion == 4) && (correlativo_doc >= 1001) && (correlativo_doc <= 1600))
                                {
                                    rango_autorizado = "Rango autorizado:  Del <b>000-001-07-00001001</b> Al <b>000-001-07-00001600</b>";
                                }
                                else if ((Tipo_Transaccion == 4) && (correlativo_doc >= 1601) && (correlativo_doc <= 2200))
                                {
                                    rango_autorizado = "Rango autorizado:  Del <b>000-001-07-00001601</b> Al <b>000-001-07-00002200</b>";
                                }
                                else if ((Tipo_Transaccion == 4) && (correlativo_doc >= 2201) && (correlativo_doc <= 2800))
                                {
                                    rango_autorizado = "Rango autorizado:  Del <b>000-001-07-00002201</b> Al <b>000-001-07-00002800</b>";
                                }
                                else if ((Tipo_Transaccion == 4) && (correlativo_doc >= 2801) && (correlativo_doc <= 3200))
                                {
                                    rango_autorizado = "Rango autorizado:  Del <b>000-001-07-00002801</b> Al <b>000-001-07-00003200</b>";
                                }
                                else if ((Tipo_Transaccion == 4) && (correlativo_doc >= 3201) && (correlativo_doc <= 3400))
                                {
                                    rango_autorizado = "Rango autorizado:  Del <b>000-001-07-00003201</b> Al <b>000-001-07-00003400</b>";
                                }
                                else if ((Tipo_Transaccion == 4) && (correlativo_doc >= 3401) && (correlativo_doc <= 3600))
                                {
                                    rango_autorizado = "Rango autorizado:  Del <b>000-001-07-00003401</b> Al <b>000-001-07-00003600</b>";
                                }
                                else if (Tipo_Transaccion == 4 && correlativo_doc >= 3601 && correlativo_doc <= 4000) //000-001-07
                                {
                                    rango_autorizado = "Rango autorizado:  Del <b>000-001-07-00003601</b> Al <b>000-001-07-00004000</b>";
                                }
                                else
                                {
                                    rango_autorizado = "Rango autorizado:  Del <b>000-001-07-00001001</b> Al <b>000-001-07-00001600</b>";
                                }
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
                        if (correlativo_doc >= 301 && correlativo_doc <= 550)
                        {
                            fecha_emision = "Fecha límite de emisión:  <b>20/03/2018</b>";
                        }
                        else if (correlativo_doc >= 551 && correlativo_doc <= 1550)
                        {
                            fecha_emision = "Fecha límite de emisión:  <b>16/10/2018</b>";
                        }
                        else if (correlativo_doc >= 1551 && correlativo_doc <= 1900)
                        {
                            fecha_emision = "Fecha límite de emisión: <b>17/10/2019</b>";
                        }
                        else if (correlativo_doc >= 1901 && correlativo_doc <= 2200)
                        {
                            fecha_emision = "Fecha límite de emisión: <b>18/10/2020</b>";
                        }
                        else if (correlativo_doc >= 2201 && correlativo_doc <= 2350) //000-002-01
                        {
                            fecha_emision = "Fecha límite de emisión: <b>04/01/2022</b>";
                        }
                        else
                        {
                            fecha_emision = "Fecha límite de emisión:  <b>00/00/0000</b>";
                        }
                    }
                    if (tipo_transaccion == 3)
                    {
                        if (lbl_serie.Text == "000-002-06")
                        {

                            if (correlativo_doc >= 101 && correlativo_doc <= 150)
                            {
                                fecha_emision = "Fecha límite de emisión:  <b>12/04/2019</b>";
                            }
                            else if (correlativo_doc >= 51 && correlativo_doc <= 100)
                            {
                                fecha_emision = "Fecha límite de emisión:  <b>12/04/2018</b>";
                            }
                            else if (correlativo_doc >= 151 && correlativo_doc <= 170) //2019-07-24 NC LTF HN 104
                            {
                                fecha_emision = "Fecha límite de emisión:  <b>22/07/2020</b>";
                            }
                        }

                        if (lbl_serie.Text == "000-003-06") //serie nueva se crea 2021-01-06
                        {
                            if (correlativo_doc >= 1 && correlativo_doc <= 30) //000-003-06
                            {
                                fecha_emision = "Fecha límite de emisión:  <b>04/01/2022</b>";
                            }
                        }


                        else
                        {
                            fecha_emision = "Fecha límite de emisión:  <b>27/03/2017</b>";
                        }
                    }
                    if (tipo_transaccion == 4)
                    {
                        if (user.Operacion == 1)
                        {
                            if (correlativo_doc >= 51 && correlativo_doc <= 60)
                            {
                                fecha_emision = "Fecha límite de emisión:  <b>27/06/2018</b>";
                            }
                            else if (correlativo_doc >= 61 && correlativo_doc <= 70)
                            {
                                fecha_emision = "Fecha límite de emisión:  <b>17/10/2019</b>";
                            }
                            else if (correlativo_doc >= 71 && correlativo_doc <= 80)
                            {
                                fecha_emision = "Fecha límite de emisión:  <b>18/10/2020</b>";
                            }
                            else if (correlativo_doc >= 81 && correlativo_doc <= 110) //000-003-07
                            {
                                fecha_emision = "Fecha límite de emisión:  <b>04/01/2022</b>";
                            }
                            else
                            {
                                fecha_emision = "Fecha límite de emisión:  <b>27/06/2017</b>";
                            }
                        }
                        if (user.Operacion == 2)
                        {
                            if(correlativo_doc >= 51 && correlativo_doc <= 120)
                            {
                                fecha_emision = "Fecha límite de emisión:  <b>20/03/2018</b>";
                            }
                            else if (correlativo_doc >= 121 && correlativo_doc <= 220)
                            {
                                fecha_emision = "Fecha límite de emisión:  <b>16/10/2018</b>";
                            }
                            else if (correlativo_doc >= 221 && correlativo_doc <= 240)
                            {
                                fecha_emision = "Fecha límite de emisión:  <b>28/02/2019</b>";
                            }
                            else if (correlativo_doc >= 241 && correlativo_doc <= 290) //2019-07-24 ND LTF HN 104
                            {
                                fecha_emision = "Fecha límite de emisión:  <b>17/10/2019</b>";
                            }
                            else if (correlativo_doc >= 291 && correlativo_doc <= 305) 
                            {
                                fecha_emision = "Fecha límite de emisión:  <b>22/07/2020</b>";
                            }
                            else if (correlativo_doc >= 306 && correlativo_doc <= 315) //000-004-07
                            {
                                fecha_emision = "Fecha límite de emisión:  <b>04/01/2022</b>";
                            }
                            else
                            {
                                fecha_emision = "Fecha límite de emisión:  <b>00/00/0000</b>";
                            }
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
                        else if ((Tipo_Transaccion == 1) && (correlativo_doc >= 2401) && (correlativo_doc <= 2900))
                        {
                            fecha_emision = "Fecha límite de emisión:  <b>22/02/2018</b>";
                        }
                        else if ((Tipo_Transaccion == 1) && (correlativo_doc >= 2901) && (correlativo_doc <= 3000))
                        {
                            fecha_emision = "Fecha límite de emisión:  <b>23/02/2019</b>";
                        }
                        else if ((Tipo_Transaccion == 1) && (correlativo_doc >= 3001) && (correlativo_doc <= 3050))
                        {
                            fecha_emision = "Fecha límite de emisión:  <b>25/02/2020</b>";
                        }
                        else if ((Tipo_Transaccion == 1) && (correlativo_doc >= 3051) && (correlativo_doc <= 3150))
                        {
                            fecha_emision = "Fecha límite de emisión:  <b>18/10/2020</b>";
                        }
                        else if ((Tipo_Transaccion == 1) && (correlativo_doc >= 3151) && (correlativo_doc <= 3300)) //000-001-01
                        {
                            fecha_emision = "Fecha límite de emisión:  <b>04/01/2022</b>";
                        }
                        else if (Tipo_Transaccion == 3 && (correlativo_doc >= 251 && correlativo_doc <= 300))
                        {
                            fecha_emision = "Fecha límite de emisión:  <b>12/12/2017</b>";
                        }
                        else if (Tipo_Transaccion == 3 && (correlativo_doc >= 301 && correlativo_doc <= 330))
                        {
                            fecha_emision = "Fecha límite de emisión:  <b>14/12/2018</b>";
                        }
                        else if (Tipo_Transaccion == 3 && (correlativo_doc >= 331 && correlativo_doc <= 340))
                        {
                            fecha_emision = "Fecha límite de emisión:  <b>26/12/2019</b>";
                        }
                        else if (Tipo_Transaccion == 3 && (correlativo_doc >= 341 && correlativo_doc <= 360))
                        {
                            fecha_emision = "Fecha límite de emisión:  <b>16/08/2020</b>";
                        }
                        else if (Tipo_Transaccion == 3 && (correlativo_doc >= 361 && correlativo_doc <= 400)) //000-001-06
                        {
                            fecha_emision = "Fecha límite de emisión:  <b>04/01/2022</b>";
                        }
                        else if (Tipo_Transaccion == 4 && (correlativo_doc >= 621 && correlativo_doc <= 650))
                        {
                            fecha_emision = "Fecha límite de emisión: <b>27/02/2018</b>";
                        }
                        else if (Tipo_Transaccion == 4 && (correlativo_doc >= 651 && correlativo_doc <= 670))
                        {
                            fecha_emision = "Fecha límite de emisión: <b>28/02/2019</b>";
                        }
                        else if (Tipo_Transaccion == 4 && (correlativo_doc >= 671 && correlativo_doc <= 685)) //2019-07-24 ND LTF HN 104
                        {
                            fecha_emision = "Fecha límite de emisión: <b>22/07/2020</b>";
                        }
                        else if (Tipo_Transaccion == 4 && (correlativo_doc >= 686 && correlativo_doc <= 700)) //000-001-07
                        {
                            fecha_emision = "Fecha límite de emisión: <b>04/01/2022</b>";
                        }
                        else
                        {
                            fecha_emision = "Fecha límite de emisión:  <b>00/00/0000</b>";
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
                        if(correlativo_doc >= 301 && correlativo_doc <= 550)
                        {
                            rango_autorizado = "Rango autorizado: Del <b>000-002-01-00000301</b> Al <b>000-002-01-00000550</b>";
                        }
                        else if (correlativo_doc >= 551 && correlativo_doc <= 1550)
                        {
                            rango_autorizado = "Rango autorizado: Del <b>000-002-01-00000551</b> Al <b>000-002-01-00001550</b>";
                        }
                        else if (correlativo_doc >= 1551 && correlativo_doc <= 1900)
                        {
                            rango_autorizado = "Rango autorizado: Del <b>000-002-01-00001551</b> Al <b>000-002-01-00001900</b>";
                        }
                        else if (correlativo_doc >= 1901 && correlativo_doc <= 2200)
                        {
                            rango_autorizado = "Rango autorizado: Del <b>000-002-01-00001901</b> Al <b>000-002-01-00002200</b>";
                        }
                        else if (correlativo_doc >= 2201 && correlativo_doc <= 2350) //000-002-01
                        {
                            rango_autorizado = "Rango autorizado: Del <b>000-002-01-00002201</b> Al <b>000-002-01-00002350</b>";
                        }
                        else
                        {
                            rango_autorizado = "Rango autorizado: Del <b>000</b> Al <b>000</b>";
                        }
                    }
                    else
                    {
                        if (correlativo_doc <= 2000)
                        {
                            rango_autorizado = "Rango autorizado:  Del <b>000-001-01-00000001</b> Al <b>000-001-01-00002000</b>";
                        }
                        else if ((correlativo_doc >= 2401) && (correlativo_doc <= 2900))
                        {
                            rango_autorizado = "Rango autorizado:  Del <b>000-001-01-00002401</b> Al <b>000-001-01-00002900</b>";
                        }
                        else if ((correlativo_doc >= 2901) && (correlativo_doc <= 3000))
                        {
                            rango_autorizado = "Rango autorizado:  Del <b>000-001-01-00002901</b> Al <b>000-001-01-00003000</b>";
                        }
                        else if ((correlativo_doc >= 3001) && (correlativo_doc <= 3050))
                        {
                            rango_autorizado = "Rango autorizado:  Del <b>000-001-01-00003001</b> Al <b>000-001-01-00003050</b>";
                        }
                        else if ((correlativo_doc >= 3051) && (correlativo_doc <= 3150))
                        {
                            rango_autorizado = "Rango autorizado:  Del <b>000-001-01-00003051</b> Al <b>000-001-01-00003150</b>";
                        }
                        else if ((correlativo_doc >= 3151) && (correlativo_doc <= 3300))
                        {
                            rango_autorizado = "Rango autorizado:  Del <b>000-001-01-00003151</b> Al <b>000-001-01-00003300/b>";
                        }
                        else
                        {
                            rango_autorizado = "Rango autorizado:  Del <b>000</b> Al <b>000</b>";
                        }
                    }
                }

                if (tipo_transaccion == 3)//Nota Credito
                {
                    if (factdata.intC4 == 8)
                    {

                        if (lbl_serie.Text == "000-002-06")
                        {
                            if (correlativo_doc >= 101 && correlativo_doc <= 150)
                            {
                                rango_autorizado = "Rango autorizado: Del <b>000-002-06-00000101</b> Al <b>000-002-06-00000150</b>";
                            }
                            else if (correlativo_doc >= 51 && correlativo_doc <= 100)
                            {
                                rango_autorizado = "Rango autorizado: Del <b>000-002-06-00000051</b> Al <b>000-002-06-00000100</b>";
                            }
                            else if (correlativo_doc >= 151 && correlativo_doc <= 170) //2019-07-24 NC LTF HN 104
                            {
                                rango_autorizado = "Rango autorizado: Del <b>000-002-06-00000151</b> Al <b>000-002-06-00000170</b>";
                            }
                            else
                            {
                                rango_autorizado = "Rango autorizado: Del <b>000-002-06</b> Al <b>000-002-06</b>";
                            }
                        }

                        if (lbl_serie.Text == "000-003-06") //serie nueva se crea 2021-01-06
                        {
                            if (correlativo_doc >= 1 && correlativo_doc <= 30) //000-003-06
                            {
                                rango_autorizado = "Rango autorizado: Del <b>000-003-06-00000001</b> Al <b>000-003-06-00000030</b>";
                            }
                        }
                    }
                    else
                    {
                        if (lbl_serie.Text == "000-001-06")
                        {

                            if (correlativo_doc <= 200)
                            {
                                rango_autorizado = "Rango autorizado: Del <b>000-001-06-00000001</b> Al <b>000-001-06-00000200</b>";
                            }
                            else if (correlativo_doc >= 251 && correlativo_doc <= 300)
                            {
                                rango_autorizado = "Rango autorizado: Del <b>000-001-06-00000251</b> Al <b>000-001-06-00000300</b>";
                            }
                            else if (correlativo_doc >= 301 && correlativo_doc <= 330)
                            {
                                rango_autorizado = "Rango autorizado: Del <b>000-001-06-00000301</b> Al <b>000-001-06-00000330</b>";
                            }
                            else if (correlativo_doc >= 331 && correlativo_doc <= 340)
                            {
                                rango_autorizado = "Rango autorizado: Del <b>000-001-06-00000331</b> Al <b>000-001-06-00000340</b>";
                            }
                            else if (correlativo_doc >= 341 && correlativo_doc <= 360)
                            {
                                rango_autorizado = "Rango autorizado: Del <b>000-001-06-00000341</b> Al <b>000-001-06-00000360</b>";
                            }
                            else if (correlativo_doc >= 361 && correlativo_doc <= 400) //000-001-06
                            {
                                rango_autorizado = "Rango autorizado: Del <b>000-001-06-00000361</b> Al <b>000-001-06-00000400</b>";
                            }

                        }
                        else
                        {
                            rango_autorizado = "Rango autorizado: Del <b>000</b> Al <b>000</b>";
                        }
                    }
                }

                if (tipo_transaccion == 4)//Nota Debito
                {
                    if (factdata.intC4 == 8)
                    {
                        if (user.Operacion == 1)
                        {
                            if (correlativo_doc >= 51 && correlativo_doc <= 60)
                            {
                                rango_autorizado = "Rango autorizado: Del <b>000-003-07-00000051</b> Al <b>000-003-07-00000060</b>";
                            }
                            else if (correlativo_doc >= 61 && correlativo_doc <= 70)
                            {
                                rango_autorizado = "Rango autorizado: Del <b>000-003-07-00000061</b> Al <b>000-003-07-00000070</b>";
                            }
                            else if (correlativo_doc >= 71 && correlativo_doc <= 80)
                            {
                                rango_autorizado = "Rango autorizado: Del <b>000-003-07-00000071</b> Al <b>000-003-07-00000080</b>";
                            }
                            else if (correlativo_doc >= 81 && correlativo_doc <= 110) //000-003-07
                            {
                                rango_autorizado = "Rango autorizado: Del <b>000-003-07-00000081</b> Al <b>000-003-07-00000110</b>";
                            }
                            else
                            {
                                rango_autorizado = "Rango autorizado: Del <b>000</b> Al <b>000</b>";
                            }
                        }
                        if (user.Operacion == 2)
                        {
                            if (correlativo_doc >= 51 && correlativo_doc <= 120)
                            {
                                rango_autorizado = "Rango autorizado: Del <b>000-004-07-00000051</b> Al <b>000-004-07-00000120</b>";
                            }
                            else if (correlativo_doc >= 121 && correlativo_doc <= 220)
                            {
                                rango_autorizado = "Rango autorizado: Del <b>000-004-07-00000121</b> Al <b>000-004-07-00000220</b>";
                            }
                            else if (correlativo_doc >= 221 && correlativo_doc <= 240)
                            {
                                rango_autorizado = "Rango autorizado: Del <b>000-004-07-00000221</b> Al <b>000-004-07-00000240</b>";
                            }
                            else if (correlativo_doc >= 241 && correlativo_doc <= 290) //2019-07-24 ND LTF HN 104
                            {
                                rango_autorizado = "Rango autorizado: Del <b>000-004-07-00000241</b> Al <b>000-004-07-00000290</b>";
                            }
                            else if (correlativo_doc >= 291 && correlativo_doc <= 305) 
                            {
                                rango_autorizado = "Rango autorizado: Del <b>000-004-07-00000291</b> Al <b>000-004-07-00000305</b>";
                            }
                            else if (correlativo_doc >= 306 && correlativo_doc <= 315) //000-004-07
                            {
                                rango_autorizado = "Rango autorizado: Del <b>000-004-07-00000306</b> Al <b>000-004-07-00000315</b>";
                            }
                            else
                            {
                                rango_autorizado = "Rango autorizado: Del <b>000</b> Al <b>000</b>";
                            }
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
                            else if (correlativo_doc >= 621 && correlativo_doc <= 650)
                            {
                                rango_autorizado = "Rango autorizado:  Del <b>000-001-07-00000621</b> Al <b>000-001-07-00000650</b>";
                            }
                            else if (correlativo_doc >= 651 && correlativo_doc <= 670)
                            {
                                rango_autorizado = "Rango autorizado:  Del <b>000-001-07-00000651</b> Al <b>000-001-07-00000670</b>";
                            }
                            else if (correlativo_doc >= 671 && correlativo_doc <= 685) //2019-07-24 ND LTF HN 104
                            {
                                rango_autorizado = "Rango autorizado:  Del <b>000-001-07-00000671</b> Al <b>000-001-07-00000685</b>";
                            }
                            else if (correlativo_doc >= 686 && correlativo_doc <= 700) //000-001-07
                            {
                                rango_autorizado = "Rango autorizado:  Del <b>000-001-07-00000686</b> Al <b>000-001-07-00000700</b>";
                            }
                            else
                            {
                                rango_autorizado = "Rango autorizado:  Del <b>000-001-07-00000651</b> Al <b>000-001-07-00000670</b>";
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
			////////////////////////////////////////////
			}
            lbl_correlativo.Text = strcorrelativo_doc;

            var fel = DB.DataFEL(user.PaisID, factdata.strC49, "");
            if (fel.isFEL) {
                lbl_serie.Text = fel.Sign_Serie;
                lbl_correlativo.Text = fel.Sign_Numero;
		    } 
            /*
            if (DB.isFELDate(user.PaisID)) { //2019-07-29
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
                       
              
                    lbl_tipo_operacion.Text = Bean.strC1.Substring(8, (Bean.strC1.Length - 8)).Replace(buscar,reemplazar) + " " + Bean.strC2;
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
                    if (factdata.intC4 == 8)
                        lbl_moneda.Text = "USD.";
                    else
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
                    /*if (factdata.strC1 != "")
                    {
                        string[] segmentos = factdata.strC40.Split('-');

                        correlativo_doc = int.Parse(segmentos[3].ToString().Trim());
                        strcorrelativo_nc = segmentos[0] + "-" + segmentos[1] + "-" + segmentos[2].Trim() + "-" + correlativo_doc.ToString("00000000.##");
                    }*/
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
                string[] separators = { ",", ".", "!", "?", ";", ":", " " };
                string value = factdata.strC18;
                string[] words = value.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                int cont = 0;
                html_bl += "<tr>"
                                                + "<td width='100px'>"
                                                    + "<b>Comodity</b></td>"
                                                + "<td colspan='5' width='100px;'>";
                                                    foreach (string word in words)
                                                    {
                                                        string regreso = "";
                                                        if (cont % 4 == 0) { regreso = "\n"; } else { regreso = ""; }
                                                        
                                                        html_bl += word+","+regreso;
                                                        cont++;
                                                    }
                                               html_bl += "</td>"
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
                                                }else
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
                        + "<b>"+master_document+"</b></td>"
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
                        + "<b>"+child_document+"</b></td>"
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
                                +"         --->"+ dr[8].ToString()
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
						decimal total_exentos_usd = 0;
						decimal total_exonerado_usd = 0;
						decimal total_exentos = 0;
						decimal total_exonerado = 0;
						decimal total_exentos_equivalente = 0;
						decimal total_exonerado_equivalente = 0;
						decimal total_locales_exentos = 0;
						decimal total_locales_exentos_equivalente = 0;
                        decimal total_rebajas_usd_hn = 0;
                        decimal total_rebajas = 0;

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
                            decimal tc = 0;
                            decimal subTotal = 0;
                            if (factdata.decC4 > 0)
                            {
                                tc = factdata.decC3 / factdata.decC4;
                                subTotal = decimal.Parse(dr[5].ToString()) / tc;
                            }
                            
                            if (dr[2].ToString() == "TERCEROS")
                            {
                                cargos_terceros += "<tr>"
                                + "<td width='500px'>"
                                    + dr[1].ToString()
                                + "</td>"
                                + "<td width='100px' align='right'>"
                                    //+ decimal.Parse(dr[4].ToString()).ToString("#,#.00")
                                    + subTotal.ToString("#,#.00")
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
                                //total_terceros_equivalente += decimal.Parse(dr[4].ToString());
                                total_terceros_equivalente += subTotal;
                            }                            
                            else
                            {
								
								//reader.GetInt32(0), reader.GetString(1), servicio, moneda, Math.Round(double.Parse(reader.GetValue(4).ToString()), 2), //Math.Round(double.Parse(reader.GetValue(5).ToString()), 2), Math.Round(double.Parse(reader.GetValue(6).ToString()), 2), //Math.Round(double.Parse(reader.GetValue(7).ToString()), 2), reader.GetValue(8).ToString(), reader.GetValue(9).ToString() 
                                cargos_locales += "<tr>"
                                + "<td width='500px'>"
                                    + dr[1].ToString()
                                + "</td>"
                                + "<td width='100px' align='right'>"
                                    //+ decimal.Parse(dr[4].ToString()).ToString("#,#.00")
                                    + subTotal.ToString("#,#.00")
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
								
								//Si no hay impuesto el rubro se manda a casilla de exentos
								if (decimal.Parse(dr[6].ToString()) == 0) {
									total_exentos += decimal.Parse(dr[5].ToString());
									//total_locales_equivalente += decimal.Parse(dr[4].ToString());
									total_exentos_equivalente += subTotal;
									
								} else {
									total_locales += decimal.Parse(dr[5].ToString());
									//total_locales_equivalente += decimal.Parse(dr[4].ToString());
									total_locales_equivalente += subTotal;
									impuestos_locales += decimal.Parse(dr[6].ToString());
								}

                                //total_locales += decimal.Parse(dr[5].ToString());
                                ////total_locales_equivalente += decimal.Parse(dr[4].ToString());
                                //total_locales_equivalente += subTotal;
                                //impuestos_locales += decimal.Parse(dr[6].ToString());
                            }
                            
                        }
						total_locales_exentos = total_locales + total_exentos;
						total_locales_exentos_equivalente = total_locales_equivalente + total_exentos_equivalente;
						
                        cargos_terceros += "<tr><td align='center'><b>Total</b></td><td align='right'><b>" + total_terceros_equivalente.ToString("#,#.00") + "</b></td><td align='right'><b>" + total_terceros.ToString("#,#.00") + "</b></td></tr>";
                        cargos_locales += "<tr><td align='center'><b>Total</b></td><td align='right'><b>" + total_locales_exentos_equivalente.ToString("#,#.00") + "</b></td><td align='right'><b>" + total_locales_exentos.ToString("#,#.00") + "</b></td></tr>";
                                                
                        if (cargos_locales != "")
                        {
                            html_Rubros += "<tr>"
                                + "<td width='500px' class='style6'>"
                                    + "<b>"+titulo_cargos_locales+"</b>"
                                + "</td>"
                                + "<td width='100px' align='right' class='style6'>"
                                    + "<b>"+moneda_equivalente+"</b>"
                                + "</td>"
                                + "<td width='100px' align='right' class='style6'>"
                                    + "<b>"+moneda+"</b>"
                                + "</td>"
                            + "</tr>";
                            html_Rubros += cargos_locales;
                        }
                        if (cargos_terceros != "")
                        {
                            html_Rubros += "<tr><td colspan='3'>&nbsp;</td></tr>";
                            html_Rubros += "<tr>"
                                + "<td width='500px' class='style6'>"
                                    + "<b>"+titulo_cargos_terceros+"</b>"
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
                                total_fact_hn = total_terceros + total_locales + total_exentos + total_exonerado + impuestos_locales;
                                tipo_cambio_hn = factdata.decC4 / total_fact_hn;
                                terceros_usd_hn = total_terceros * tipo_cambio_hn;
                                locales_usd_hn = total_locales * tipo_cambio_hn;
                                total_exentos_usd = total_exentos * tipo_cambio_hn;
                                total_exonerado_usd = total_exonerado * tipo_cambio_hn;
                                impuestos_locales_usd_hn = impuestos_locales * tipo_cambio_hn;
                            }
                        }
                        else
                        {
                            if (factdata.strC1 != "")
                            {
                                if (factdata.decC4 == 0)
                                {
                                    total_fact_hn = total_terceros + total_locales + total_exentos + total_exonerado + impuestos_locales;
                                    tipo_cambio_hn = 0;
                                    terceros_usd_hn = 0;
                                    locales_usd_hn = 0;
									total_exentos_usd = 0;
									total_exonerado_usd = 0;
                                    impuestos_locales_usd_hn = 0;
                                }
                                else
                                {
                                    total_fact_hn = total_terceros + total_locales + total_exentos + total_exonerado + impuestos_locales;
                                    tipo_cambio_hn = total_fact_hn / factdata.decC4;
                                    terceros_usd_hn = total_terceros / tipo_cambio_hn;
                                    locales_usd_hn = total_locales / tipo_cambio_hn;
									total_exentos_usd = total_exentos / tipo_cambio_hn;
									total_exonerado_usd = total_exonerado / tipo_cambio_hn;
                                    impuestos_locales_usd_hn = impuestos_locales / tipo_cambio_hn;
                                }
                            }
                        }

                        lbl_rebajas_usd_hn.Text = total_rebajas_usd_hn.ToString("#,#.00");
                        lbl_rebajas.Text = total_rebajas.ToString("#,#.00");

                        lbl_terceros.Text = total_terceros.ToString("#,#.00");
                        lbl_locales.Text = total_locales.ToString("#,#.00");
                        lbl_impuesto_locales.Text = impuestos_locales.ToString("#,#.00");
                        lbl_total_pagar.Text = total_fact_hn.ToString("#,#.00");
                        lbl_tipo_cambio_hn.Text = tipo_cambio_hn.ToString("#,#.0000");
                        lbl_total_usd_hn.Text = factdata.decC4.ToString("#,#.00");

                        lbl_terceros_usd_hn.Text = terceros_usd_hn.ToString("#,#.00");
                        lbl_locales_usd_hn.Text = locales_usd_hn.ToString("#,#.00");
                        lbl_impuesto_locales_usd_hn.Text = impuestos_locales_usd_hn.ToString("#,#.00");

						//se agrego la separacion de subtotal de rubros exentos y exonerados
                        lbl_subtotal_usd_exento.Text = total_exentos_usd.ToString("#,#.00");
                        lbl_subtotal_exento.Text = total_exentos.ToString("#,#.00");
                        lbl_subtotal_usd_exonerado.Text = total_exonerado_usd.ToString("#,#.00");
                        lbl_subtotal_exonerado.Text = total_exonerado.ToString("#,#.00");
                        

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

                lbl_total.Text = strMoneda+" "+factdata.decC3.ToString("#,#.00");
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

                        var imprimeAmbasMonedas = DB.ObtenerValorCampoBAW(string.Format("select fac_imprimir_moneda_local_y_equivalente::int::varchar from tbl_factura where fac_serie = '{0}' and fac_suc_id = {1}", lbl_serie.Text.Trim(), user.SucursalID.ToString()));
                        if (imprimeAmbasMonedas == "1")
                        {
                            lbl_total_letras.Text = string.Format("TOTAL EN LETRAS.:    {0}{1}{4}TOTAL EN LETRAS.:    {2}{3}", c.enletras(factdata.decC4.ToString(), 1), " Lempiras"
                                , c.enletras(factdata.decC3.ToString(), 1), " Dolares", "<br/>");
                        }
                        else
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
                            string temp1 = lbl_direccion_empresa.Text;
                            //lbl_direccion_empresa.Text = user.pais.Direccion_Empresa; 2020-03-04 esto deberia traer el valor
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
                retencion =  DB.getRetencioByFactID(factdata.intC1);
                lbl_retencion.Text = retencion.ToString("#,#.00");
                if (retencion == 0)
                {
                    lbl_retencion.Visible = false;
                    lbl_Titulo_ret.Visible = false;
                }
            }
        }

        /* esto ya realizado en page load
        DB.parametros Params = DB.EmpresaParametros(user.PaisID, "", "", "** TITULO **", ""); //pais, sistema, doc_id, titulo, edicion        
        Image1.ImageUrl = "data:image;base64," + System.Convert.ToBase64String(Params.logo);
        Image1.Height = 63;
        Image1.Width = 237; 
        lbl_direccion_empresa.Text = Params.direccion2;
        string str1 = lbl_email_empresa.Text;
        */
        lbl_email_empresa.Text = lbl_email_empresa.Text;
        lbl_direccion_empresa.Text = lbl_direccion_empresa.Text;

    }
    public  RE_GenericBean Ordenar_Arrelgo(RE_GenericBean Arreglo, int Tipo_Transaccion)
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
            }else if (Tipo_Transaccion == 4)//Nota de Debito
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
            else if ((Tipo_Transaccion == 3) || (Tipo_Transaccion == 18) || (Tipo_Transaccion == 12) || (Tipo_Transaccion == 31))
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

                result.strC49 = Arreglo.strC49; //ESignature    2019-05-24
                result.strC52 = Arreglo.strC52; //tnc_guid      2019-05-24

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
                if (tipo_transaccion == 12)
                {
                    Arreglo.decC9 = DB.Convertir_ATipo_Cambio_Libro_Diario(ID, 12, user, Arreglo.decC5);//Impuesto Equivalente
                    Arreglo.decC10 = DB.Convertir_ATipo_Cambio_Libro_Diario(ID, 12, user, Arreglo.decC4);//Subtotal Equivalente
                }
                if (tipo_transaccion == 31)
                {
                    Arreglo.decC9 = DB.Convertir_ATipo_Cambio_Libro_Diario(ID, 31, user, Arreglo.decC5);//Impuesto Equivalente
                    Arreglo.decC10 = DB.Convertir_ATipo_Cambio_Libro_Diario(ID, 31, user, Arreglo.decC4);//Subtotal Equivalente
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


