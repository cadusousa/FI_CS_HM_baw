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

public partial class invoice_template_oc : System.Web.UI.Page
{
    RE_GenericBean oc = null;
    int oc_ID = 0;
    int tipo = 0;
    UsuarioBean user;
    DataTable dtPolizaDiario = null;
    DataTable dtDetalleProvision = null;
    ArrayList arrayProvision = null;
    public string html_datos = "";
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
        if (Request.QueryString["id"] != null)
        {
            oc_ID = int.Parse(Request.QueryString["id"].ToString());
            tipo = int.Parse(Request.QueryString["tipo"].ToString());
            Cargar(oc_ID,tipo);
        }


    }

    protected void Cargar(int ocID, int tipo)
    {
        if (tipo == 7)
        {
            oc = (RE_GenericBean)DB.getOCData_forprint(ocID);
            arrayProvision = (ArrayList)DB.getProvisionesListbyOC(" tpr_toc_id = "+ocID.ToString());
            foreach (RE_GenericBean arrProv in arrayProvision)
            {
                lbl_provision.Text = arrProv.strC7 + "-" + arrProv.intC4.ToString();
                switch (arrProv.intC6)
                {
                    case 1:
                        lbl_estado_provision.Text = "Activa";
                        break;
                    case 5:
                        lbl_estado_provision.Text = "Autorizada";
                        break;
                    case 3:
                        lbl_estado_provision.Text = "Anulada";
                        break;
                    case 4:
                        lbl_estado_provision.Text = "Pagada";
                        break;
                    case 9:
                        lbl_estado_provision.Text = "Cortada";
                        break;
                    default:
                        lbl_estado_provision.Text = "-";
                        break;
                }
            }
            lbl_tipo_documento.Text = "Orden de Compra";
            lbl_documento_asociado.Text = "Número de Cotizacion";
            lbl_codigo_proveedor.Text = oc.intC1.ToString();
            html_iva_subtotal = "<tr>"
                            + "<td class='style8' width='150px'></td>"
                            + "<td class='style8' width='200px'></td>"
                            + "<td class='style8'></td>"
                            + "<td class='style8'></td>"
                            + "<td class='style8'></td>"
                            + "<td class='style8'></td>"
                            + "<td width='50px' class='style5'>Subtotal</td>"
                            + "<td width='200px' align='right'>" + oc.decC6.ToString() + "</td>"
                        + "</tr>"
                        + "<tr>"
                            + "<td class='style8' width='150px'></td>"
                            + "<td class='style8' width='200px'></td>"
                            + "<td class='style8'></td>"
                            + "<td class='style8'></td>"
                            + "<td class='style8'></td>"
                            + "<td class='style8'></td>"
                            + "<td width='50px' class='style5'>Impuesto</td>"
                            + "<td width='200px' align='right'>" + oc.decC5.ToString() + "</td>"
                        + "</tr>";
        }
        if (tipo == 5)
        {
            oc = (RE_GenericBean)DB.getProvisionData_forprint(ocID);
            lbl_tipo_documento.Text = "Provision";
            lbl_documento_asociado.Text = "Correlativo";

            dtPolizaDiario = DB.getPolizaDiario(user, double.Parse(ocID.ToString()), 5, 0);
            html_poliza_diario = "<table align='center' cellpadding='0' cellspacing='0' width='100%'>";
            html_poliza_diario += "<tr>"
                    + "<td width='12%' class='style9'>"
                        + "<b>Cuenta</b>"
                    + "</td>"
                    + "<td width='38%' align='left'  class='style9'>"
                        + "<b>Descripcion de Cuenta</b>"
                    + "</td>"
                    + "<td width='25%' align='left' class='style9'>"
                        + "<b>Cargos</b>"
                    + "</td>"
                    + "<td width='25%' align='left' class='style9'>"
                        + "<b>Abonos</b>"
                    + "</td>"
                + "</tr>";
            foreach (DataRow dr in dtPolizaDiario.Rows)
            {
                html_poliza_diario += "<tr>"
                                + "<td>"
                                    + dr[0].ToString()
                                + "</td>";
                    if (dr[1].ToString() == "TOTAL")
                    {
                        html_poliza_diario += "<td><b>" + dr[1].ToString() + "</b></td>";
                    }
                    else {
                        html_poliza_diario += "<td>" + dr[1].ToString() + "</td>";
                    }
                
                if((dr[2].ToString() == " ") || (dr[3].ToString() == " "))
                {
                    html_poliza_diario += "<td>"
                                    + dr[2].ToString()
                                + "</td>"
                                + "<td>"
                                    + dr[3].ToString()
                                + "</td>"
                            + "</tr>";
                }
                else
                {
                    html_poliza_diario += "<td>"
                                        + decimal.Parse(dr[2].ToString()).ToString("#,#.00#;(#,#.00#)")
                                    + "</td>"
                                    + "<td>"
                                        + decimal.Parse(dr[3].ToString()).ToString("#,#.00#;(#,#.00#)")
                                    + "</td>"
                                + "</tr>";
                }
            }
            html_poliza_diario += "</table>";

            dtDetalleProvision = (DataTable)DB.getRubbyOC(ocID, 5);

            html_detalle_provision = "<table align='center' cellpadding='0' cellspacing='0' width='100%'>";
            html_detalle_provision += "<tr>"
                    + "<td width='12.5%' class='style9'>"
                        + "<b>Código</b>"
                    + "</td>"
                    + "<td width='12.5%' align='left' class='style9'>"
                        + "<b>Rubro</b>"
                    + "</td>"
                    + "<td width='12.5%' align='left' class='style9'>"
                        + "<b>Tipo</b>"
                    + "</td>"
                    + "<td width='12.5%' align='left' class='style9'>"
                        + "<b>Moneda</b>"
                    + "</td>"
                    + "<td width='12.5%' align='left' class='style9'>"
                        + "<b>Equivalente</b>"
                    + "</td>"
                    + "<td width='12.5%' align='left' class='style9'>"
                        + "<b>Subtotal</b>"
                    + "</td>"
                    + "<td width='12.5%' align='left' class='style9'>"
                        + "<b>Impuesto</b>"
                    + "</td>"
                    + "<td width='12.5%' align='left' class='style9'>"
                        + "<b>Total</b>"
                    + "</td>"
                + "</tr>";
            foreach (DataRow dr in dtDetalleProvision.Rows)
            {
                string tipo_moneda = "";
                if (dr[3].ToString().Equals("1")) tipo_moneda = "GTQ";
                if (dr[3].ToString().Equals("2")) tipo_moneda = "SVC";
                if (dr[3].ToString().Equals("3")) tipo_moneda = "HNL";
                if (dr[3].ToString().Equals("4")) tipo_moneda = "NIC";
                if (dr[3].ToString().Equals("5")) tipo_moneda = "CRC";
                if (dr[3].ToString().Equals("6")) tipo_moneda = "PAB";
                if (dr[3].ToString().Equals("7")) tipo_moneda = "BZD";
                if (dr[3].ToString().Equals("8")) tipo_moneda = "USD";
                
                html_detalle_provision += "<tr>"
                                 + "<td>"
                                     + dr[0].ToString()
                                 + "</td>"
                                 + "<td>"
                                    + dr[1].ToString()
                                + "</td>"
                                + "<td>"
                                    + dr[2].ToString()
                                + "</td>"
                                + "<td align='center'>"
                                    + tipo_moneda
                                + "</td>"
                                + "<td>"
                                    + decimal.Parse(dr[4].ToString()).ToString("#,#.00#;(#,#.00#)")
                                + "</td>"
                                + "<td>"
                                    + decimal.Parse(dr[5].ToString()).ToString("#,#.00#;(#,#.00#)")
                                + "</td>"
                                + "<td>"
                                    + decimal.Parse(dr[6].ToString()).ToString("#,#.00#;(#,#.00#)")
                                + "</td>"
                                + "<td>"
                                    + decimal.Parse(dr[7].ToString()).ToString("#,#.00#;(#,#.00#)")
                                + "</td>"
                            + "</tr>"; 
            }
            html_detalle_provision += "</table>";
        }

        DB.parametros Params = DB.EmpresaParametros(user.PaisID, "", "", "** TITULO **", ""); //pais, sistema, doc_id, titulo, edicion
        Image1.ImageUrl = "data:image;base64," + System.Convert.ToBase64String(Params.logo);
        Image1.Height = 63;
        Image1.Width = 237;

        var fel = DB.DataFEL(user.PaisID , oc.strC49, "");
        if (fel.isFEL) {
            lbl_serie.Text = fel.Sign_Serie;
            lbl_correlativo.Text = fel.Sign_Numero;
		} else {
	        lbl_serie.Text = oc.strC32;//Serie
	        lbl_correlativo.Text = oc.strC33;//Correlativo
		}        
        lbl_fecha_emision.Text = oc.strC5;//Fecha de Emision
        lbl_proveedor.Text = oc.strC3;//Proveedor
        lbl_direccion.Text = oc.strC4;//Direccion
        lbl_numero_cotizacion.Text = oc.strC37;//Numero de Cotizacion
        lbl_departamento.Text = oc.strC38;//Departamento
        lbl_solicitada_por.Text = oc.strC40;//Solicitada Por
        lbl_autorizada_por.Text = oc.strC41;//Autorizada Por
        lbl_descripcion.Text = oc.strC35;//Descripcion
        lbl_terminos.Text = oc.strC36;//Terminos y Condiciones
        lbl_observaciones.Text = oc.strC7;//Observaciones
        lbl_contacto.Text = oc.strC39;//Contacto
        lbl_hbl.Text = oc.strC9;//HBL
        lbl_mbl.Text = oc.strC10;//MBL
        lbl_moneda.Text = Utility.TraducirMonedaInt(oc.intC4);
        if (user.PaisID == 11)//GRH
        {
            lbl_total.Text = oc.decC4.ToString();
            lbl_total_equivalente.Text = "   " + oc.decC3.ToString();
        }
        else
        {
            lbl_total.Text = oc.decC3.ToString();
            lbl_total_equivalente.Text = "   " + oc.decC4.ToString();
        }

        Conv c = new Conv();
        if (user.PaisID == 7)
        {
            lbl_total_letras.Text = "TOTAL IN LETTERS.:    " + c.enletras(oc.decC3.ToString(), 8);
        }
        else if (user.PaisID == 1)
        {
            if (user.contaID == 1)
            {
                lbl_total_letras.Text = "TOTAL EN LETRAS.:    " + c.enletras(oc.decC3.ToString(), 1);
            }
            else if (user.contaID == 2)
            {
                lbl_total_letras.Text = "TOTAL IN LETTERS.:    " + c.enletras(oc.decC3.ToString(), 8);
            }
        }
        else
        {
            lbl_total_letras.Text = "TOTAL EN LETRAS.:    " + c.enletras(oc.decC3.ToString(), 1);
        }

        if ((user.PaisID == 23) || (user.PaisID == 3))
        {
            html_datos = "<table width='100%'>"
                + "<tr>"
                + "    <td class='style11'>Dirección:</td><td>Col.Brisas de la Mesa contiguo a la Base Aerea,</br>Aeropuerto Ramon Villeda Morales</td>"
                + "</tr>"
                + "<tr>"
                + "    <td class='style11'>Teléfonos:</td><td>504-2668-0121/504-2564-0099, fax 504-2668-0353</td>"
                + "</tr>"
                + "<tr>"
                + "    <td class='style11'>R.T.N.</td><td>05019000044051</td>"
                + "</tr>"
            + "</table>";

            html_datos = Params.direccion2;

            html_pie_pagina = "<table width='100%'>"
                + "<tr>"
                + "    <td class='style5' width='50%'>Recibi conforme:</td><td class='style5' width='50%'>Firma autorizada:</td>"
                + "</tr>"
            + "</table>";
        }
    }
}
