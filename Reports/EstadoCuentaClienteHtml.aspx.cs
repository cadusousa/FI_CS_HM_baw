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

public partial class Reports_EstadoCuentaClienteHtml : System.Web.UI.Page
{
    UsuarioBean user = null;
    public string html_facturas = "";
    public string html_notas_debito = "";
    public string html_recibos = "";
    decimal total_facturas = 0;
    decimal total_notas_debito = 0;
    decimal total_recibos = 0;
    decimal total_abono_facturas = 0;
    decimal total_abono_notas_debito = 0;
    decimal total_abono_recibos = 0;
    decimal total_saldo_facturas = 0;
    decimal total_saldo_notas_debito = 0;
    decimal total_saldo_recibos = 0;
    decimal total_saldo_cliente = 0;
    

    protected void Page_Load(object sender, EventArgs e)
    {
        user = (UsuarioBean)Session["usuario"];
        /*if (Session["usuario"] == null)
        {
            Response.Redirect("../default.aspx");
        }*/

        int paisID = 1;

        if (Session["usuario"] != null)
        {
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
            else if (user.PaisID == 30)
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

            paisID = user.PaisID;

        } 
            
            
        DB.parametros Params = DB.EmpresaParametros(paisID, "", "", "ESTADO DE CUENTA CLIENTE", ""); //pais, sistema, doc_id, titulo, edicion

        Label1.Text = Params.titulo;
        Image1.ImageUrl = "data:image;base64," + System.Convert.ToBase64String(Params.logo);
        Image1.Height = 63;
        Image1.Width = 237; 

        int solopendiente = int.Parse(Request.QueryString["solopendiente"].ToString());
        string consMon = Request.QueryString["consolida_moneda"].ToString();
        string pais = Request.QueryString["pais"].ToString();
        string fechaini = "", fechafin = "", clienteNombre = "";
        double clienteID = double.Parse(Request.QueryString["cliID"].ToString());
        int tpi = int.Parse(Request.QueryString["tpi"].ToString());

        if (Request.QueryString["fechaini"] != null && !Request.QueryString["fechaini"].ToString().Equals(""))
            fechaini = Request.QueryString["fechaini"].ToString();
        if (Request.QueryString["fechafin"] != null && !Request.QueryString["fechafin"].ToString().Equals(""))
            fechafin = Request.QueryString["fechafin"].ToString();
        if (Request.QueryString["clientenombre"] != null && !Request.QueryString["clientenombre"].ToString().Equals(""))
            clienteNombre = Request.QueryString["clientenombre"].ToString();
        int monID = int.Parse(Request.QueryString["moneda"].ToString());
        int contaID = int.Parse(Request.QueryString["conta"].ToString());

        #region Formatear Fechas
        //Fecha Inicio
        int fe_dia = int.Parse(fechaini.Substring(3, 2));
        int fe_mes = int.Parse(fechaini.Substring(0, 2));
        int fe_anio = int.Parse(fechaini.Substring(6, 4));
        fechaini = fe_anio.ToString() + "-";
        if (fe_mes < 10)
        {
            fechaini += "0" + fe_mes.ToString() + "-";
        }
        else
        {
            fechaini += fe_mes.ToString() + "-";
        }
        if (fe_dia < 10)
        {
            fechaini += "0" + fe_dia.ToString();
        }
        else
        {
            fechaini += fe_dia.ToString();
        }
        fechaini = fechaini + " 00:00:00";
        //Fecha Fin
        fe_dia = int.Parse(fechafin.Substring(3, 2));
        fe_mes = int.Parse(fechafin.Substring(0, 2));
        fe_anio = int.Parse(fechafin.Substring(6, 4));
        fechafin = fe_anio.ToString() + "-";
        if (fe_mes < 10)
        {
            fechafin += "0" + fe_mes.ToString() + "-";
        }
        else
        {
            fechafin += fe_mes.ToString() + "-";
        }
        if (fe_dia < 10)
        {
            fechafin += "0" + fe_dia.ToString();
        }
        else
        {
            fechafin += fe_dia.ToString();
        }
        fechafin = fechafin + " 23:59:59";
        #endregion
        string simbolomoneda = Utility.TraducirMonedaInt(monID);
        string where = "", where_1 = "", where2 = "", where2_1 = "", where3 = "", where3_1 = "", where4 = "", where4_1 = "", where4_2 = "", where5 = "", where5_1 = "", where5_2 = "", where5_3 = "", where5_4 = "", where5_5 = "", where6 = "", where6_1 = "", where7 = "", where7_1 = "", where8 = "", where8_1 = "", where5_6 = "", where8_2 = "", where7_2 = "", where9 = "", where9_1 = "", where9_2 = "", where10_1 = "";
        string where_cons = "", where_1_cons = "", where2_cons = "", where2_1_cons = "", where3_cons = "", where3_1_cons = "", where4_cons = "", where4_1_cons = "", where4_2_cons = "", where5_cons = "", where5_1_cons = "", where5_2_cons = "", where5_3_cons = "", where5_4_cons = "", where5_5_cons = "", where6_cons = "", where6_1_cons = "", where7_cons = "", where7_1_cons = "", where8_cons = "", where8_1_cons = "", where5_6_cons = "", where8_2_cons = "", where7_2_cons = "", where9_cons = "", where9_1_cons = "", where9_2_cons = "", where10_1_cons = "";
        string tipoPersona = "";
        if (!Page.IsPostBack)
        {


            if (Request.QueryString["tipo"].ToString().ToUpper() == "EXCEL")
            {
                string filename = "reporte.xls";
                Response.ContentType = "application/vnd.ms-excel";
                Response.AddHeader("Content-Disposition", "attachment; filename=" + filename + ";");
            }


            if (Session["usuario"] == null)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "Nombre", "<script> refresh(); </script>");
            }

            if (tpi == 3) tipoPersona = "Cliente";
            else if (tpi == 4) tipoPersona = "Proveedor";
            else if (tpi == 2) tipoPersona = "Agente";
            else if (tpi == 5) tipoPersona = "Naviera";
            else if (tpi == 6) tipoPersona = "Lineas Aereas";
            else if (tpi == 10) tipoPersona = "Intercompany";

            LibroDiarioDS ds = null;

            if (tpi == 3)
            {
                #region Parametros estado cuenta cliente
                //Facturas
                where = " and tfa_conta_id=" + contaID + " and tfa_moneda=" + monID + " and tfa_ted_id<>3 and tfa_tpi_id=" + tpi + " ";
                if (fechaini != null && !fechaini.Equals("")) where += " and tfa_fecha_emision>='" + fechaini + "'";
                if (fechafin != null && !fechafin.Equals("")) where += " and tfa_fecha_emision<='" + fechafin + "'";
                where += " and tfa_pai_id=" + pais + " ";
                //facturas anuladas
                where_1 = " and a.tfa_conta_id=" + contaID + " and a.tfa_moneda=" + monID + " and a.tfa_ted_id=3 and tfa_tpi_id=" + tpi + " ";
                if (fechaini != null && !fechaini.Equals("")) where_1 += " and a.tfa_fecha_emision>='" + fechaini + "'";
                if (fechafin != null && !fechafin.Equals("")) where_1 += " and a.tfa_fecha_emision<='" + fechafin + "'";
                where_1 += " and a.tfa_pai_id=" + pais + " ";

                //Notas Debito
                where2 = " and tnd_tcon_id=" + contaID + " and tnd_moneda=" + monID + " and tnd_tpi_id=" + tpi + " and tnd_ted_id<>3";
                if (fechaini != null && !fechaini.Equals("")) where2 += " and tnd_fecha_emision>='" + fechaini + "'";
                if (fechafin != null && !fechafin.Equals("")) where2 += " and tnd_fecha_emision<='" + fechafin + "'";
                where2 += " and tnd_pai_id=" + pais + " ";
                //Notas Credito
                // anuladas Nota Debito
                //Notas Debito
                where2_1 = " and a.tnd_tcon_id=" + contaID + " and a.tnd_moneda=" + monID + " and a.tnd_tpi_id=" + tpi + " and a.tnd_ted_id=3";
                if (fechaini != null && !fechaini.Equals("")) where2_1 += " and a.tnd_fecha_emision>='" + fechaini + "'";
                if (fechafin != null && !fechafin.Equals("")) where2_1 += " and a.tnd_fecha_emision<='" + fechafin + "'";
                where2_1 += " and a.tnd_pai_id=" + pais + " ";

                //where3 = " and tnc_tcon_id=" + contaID + " and tnc_mon_id=" + monID + " and tnc_tpi_id=" + tpi + " ";
                where3 = " and tnc_tcon_id=" + contaID + " and tnc_mon_id=" + monID + " and tnc_tpi_id=" + tpi + " and tnc_ted_id<>3 ";
                //if (solopendiente == 1) where3 += " and tnc_ted_id in (1,2)"; else where3 += " and tnc_ted_id in (1,2,4)";
                if (fechaini != null && !fechaini.Equals("")) where3 += " and tnc_fecha>='" + fechaini + "'";
                if (fechafin != null && !fechafin.Equals("")) where3 += " and tnc_fecha<='" + fechafin + "'";
                where3 += " and tnc_pai_id=" + pais + " ";
                // anuladas nc
                //where3 = " and tnc_tcon_id=" + contaID + " and tnc_mon_id=" + monID + " and tnc_tpi_id=" + tpi + " ";
                where3_1 = " and a.tnc_tcon_id=" + contaID + " and a.tnc_mon_id=" + monID + " and a.tnc_tpi_id=" + tpi + " and a.tnc_ted_id=3 ";
                //if (solopendiente == 1) where3 += " and tnc_ted_id in (1,2)"; else where3 += " and tnc_ted_id in (1,2,4)";
                if (fechaini != null && !fechaini.Equals("")) where3_1 += " and a.tnc_fecha>='" + fechaini + "'";
                if (fechafin != null && !fechafin.Equals("")) where3_1 += " and a.tnc_fecha<='" + fechafin + "'";
                where3_1 += " and a.tnc_pai_id=" + pais + " ";
                //Recibos
                //where4 = " and tre_id=" + contaID + " and tre_moneda_id=" + monID + " and tre_tpi_id=" + tpi + " ";
                //where4 = " and tre_tcon_id=" + contaID + " and tre_moneda_id=" + monID + " ";
                where4 = " and tre_tcon_id=" + contaID + " and tre_moneda_id=" + monID + " and tre_ted_id<>3 ";
                //if (solopendiente == 1) where4 += " and tre_ted_id in (0)"; else where4 += " and tre_ted_id in (0)";
                if (fechaini != null && !fechaini.Equals("")) where4 += " and tre_fecha>='" + fechaini + "'";
                if (fechafin != null && !fechafin.Equals("")) where4 += " and tre_fecha<='" + fechafin + "'";
                where4 += " and tre_pai_id=" + pais + " ";
                //Recibos anulados
                where4_1 = " and a.tre_tcon_id=" + contaID + " and a.tre_moneda_id=" + monID + " and a.tre_ted_id=3 ";
                //if (solopendiente == 1) where4 += " and tre_ted_id in (0)"; else where4 += " and tre_ted_id in (0)";
                if (fechaini != null && !fechaini.Equals("")) where4_1 += " and a.tre_fecha_emision>='" + fechaini + "'";
                if (fechafin != null && !fechafin.Equals("")) where4_1 += " and a.tre_fecha_emision<='" + fechafin + "'";
                where4_1 += " and a.tre_pai_id=" + pais + " ";

                //Cheques y Transferencias
                where5 = " and tcg_tcon_id=" + contaID + " and tcg_tpi_id=" + tpi + " and tcg_ted_id=4 ";
                if (fechaini != null && !fechaini.Equals("")) where5 += " and tcg_fecha_emision>='" + fechaini + "'";
                if (fechafin != null && !fechafin.Equals("")) where5 += " and tcg_fecha_emision<='" + fechafin + "'";
                where5 += " and tcg_pai_id=" + pais + " and tcg_ttm_id=" + monID + " ";

                //Cheques y Transferencias anuladas
                where5_1 = " and a.tcg_tcon_id=" + contaID + " and a.tcg_tpi_id=" + tpi + " and a.tcg_ted_id=3 ";
                if (fechaini != null && !fechaini.Equals("")) where5_1 += " and a.tcg_fecha_emision>='" + fechaini + "'";
                if (fechafin != null && !fechafin.Equals("")) where5_1 += " and a.tcg_fecha_emision<='" + fechafin + "'";
                where5_1 += " and a.tcg_pai_id=" + pais + " and tcg_ttm_id=" + monID + " ";
                #endregion

                if (pais == "1" && contaID == 1 && consMon == "si")
                {
                    //Facturas
                    where_cons = " and tfa_conta_id=" + contaID + " and tfa_moneda<>" + monID + " and tfa_ted_id<>3 and tfa_tpi_id=" + tpi + " ";
                    if (fechaini != null && !fechaini.Equals("")) where_cons += " and tfa_fecha_emision>='" + fechaini + "'";
                    if (fechafin != null && !fechafin.Equals("")) where_cons += " and tfa_fecha_emision<='" + fechafin + "'";
                    where_cons += " and tfa_pai_id=" + pais + " ";
                    //facturas anuladas
                    where_1_cons = " and a.tfa_conta_id=" + contaID + " and a.tfa_moneda<>" + monID + " and a.tfa_ted_id=3 and tfa_tpi_id=" + tpi + " ";
                    if (fechaini != null && !fechaini.Equals("")) where_1_cons += " and a.tfa_fecha_emision>='" + fechaini + "'";
                    if (fechafin != null && !fechafin.Equals("")) where_1_cons += " and a.tfa_fecha_emision<='" + fechafin + "'";
                    where_1_cons += " and a.tfa_pai_id=" + pais + " ";

                    //Notas Debito
                    where2_cons = " and tnd_tcon_id=" + contaID + " and tnd_moneda<>" + monID + " and tnd_tpi_id=" + tpi + " and tnd_ted_id<>3";
                    if (fechaini != null && !fechaini.Equals("")) where2_cons += " and tnd_fecha_emision>='" + fechaini + "'";
                    if (fechafin != null && !fechafin.Equals("")) where2_cons += " and tnd_fecha_emision<='" + fechafin + "'";
                    where2_cons += " and tnd_pai_id=" + pais + " ";
                    //Notas Credito
                    // anuladas Nota Debito
                    //Notas Debito
                    where2_1_cons = " and a.tnd_tcon_id=" + contaID + " and a.tnd_moneda<>" + monID + " and a.tnd_tpi_id=" + tpi + " and a.tnd_ted_id=3";
                    if (fechaini != null && !fechaini.Equals("")) where2_1_cons += " and a.tnd_fecha_emision>='" + fechaini + "'";
                    if (fechafin != null && !fechafin.Equals("")) where2_1_cons += " and a.tnd_fecha_emision<='" + fechafin + "'";
                    where2_1_cons += " and a.tnd_pai_id=" + pais + " ";


                    where3_cons = " and tnc_tcon_id=" + contaID + " and tnc_mon_id<>" + monID + " and tnc_tpi_id=" + tpi + " and tnc_ted_id<>3 ";
                    if (fechaini != null && !fechaini.Equals("")) where3_cons += " and tnc_fecha>='" + fechaini + "'";
                    if (fechafin != null && !fechafin.Equals("")) where3_cons += " and tnc_fecha<='" + fechafin + "'";
                    where3_cons += " and tnc_pai_id=" + pais + " ";
                    // anuladas nc
                    where3_1_cons = " and a.tnc_tcon_id=" + contaID + " and a.tnc_mon_id<>" + monID + " and a.tnc_tpi_id=" + tpi + " and a.tnc_ted_id=3 ";
                    if (fechaini != null && !fechaini.Equals("")) where3_1_cons += " and a.tnc_fecha>='" + fechaini + "'";
                    if (fechafin != null && !fechafin.Equals("")) where3_1_cons += " and a.tnc_fecha<='" + fechafin + "'";
                    where3_1_cons += " and a.tnc_pai_id=" + pais + " ";
                    //Recibos
                    where4_cons = " and tre_tcon_id=" + contaID + " and tre_moneda_id<>" + monID + " and tre_ted_id<>3 ";
                    if (fechaini != null && !fechaini.Equals("")) where4_cons += " and tre_fecha>='" + fechaini + "'";
                    if (fechafin != null && !fechafin.Equals("")) where4_cons += " and tre_fecha<='" + fechafin + "'";
                    where4_cons += " and tre_pai_id=" + pais + " ";
                    //Recibos anulados
                    where4_1_cons = " and a.tre_tcon_id=" + contaID + " and a.tre_moneda_id<>" + monID + " and a.tre_ted_id=3 ";
                    if (fechaini != null && !fechaini.Equals("")) where4_1_cons += " and a.tre_fecha_emision>='" + fechaini + "'";
                    if (fechafin != null && !fechafin.Equals("")) where4_1_cons += " and a.tre_fecha_emision<='" + fechafin + "'";
                    where4_1_cons += " and a.tre_pai_id=" + pais + " ";

                    //Cheques y Transferencias
                    where5_cons = " and tcg_tcon_id=" + contaID + " and tcg_tpi_id=" + tpi + " and tcg_ted_id=4 ";
                    if (fechaini != null && !fechaini.Equals("")) where5_cons += " and tcg_fecha_emision>='" + fechaini + "'";
                    if (fechafin != null && !fechafin.Equals("")) where5_cons += " and tcg_fecha_emision<='" + fechafin + "'";
                    where5_cons += " and tcg_pai_id=" + pais + " and tcg_ttm_id<>" + monID + " ";

                    //Cheques y Transferencias anuladas
                    where5_1_cons = " and a.tcg_tcon_id=" + contaID + " and a.tcg_tpi_id=" + tpi + " and a.tcg_ted_id=3 ";
                    if (fechaini != null && !fechaini.Equals("")) where5_1_cons += " and a.tcg_fecha_emision>='" + fechaini + "'";
                    if (fechafin != null && !fechafin.Equals("")) where5_1_cons += " and a.tcg_fecha_emision<='" + fechafin + "'";
                    where5_1_cons += " and a.tcg_pai_id=" + pais + " and tcg_ttm_id<>" + monID + " ";
                }


                ds = DB.getEstadoCuentaCliente_detallado(clienteID, where, where2, where3, where4, where5, fechafin, monID, solopendiente, where4_1, where_1, where2_1, where3_1, where5_1,
                                                         where_cons, where2_cons, where3_cons, where4_cons, where5_cons, where4_1_cons, where_1_cons, where2_1_cons, where3_1_cons, where5_1_cons, consMon, int.Parse(pais), contaID);
                //if (solopendiente == 0) ds = DB.getEstadoCuentaCliente_detallado(clienteID, where, where2, where5, fechafin, monID, solopendiente);
                //else ds = DB.getEstadoCuentaCliente(clienteID, where, fechafin, where2, where3, where4);
            }
            
            
            /*if (tpi == 3)
            {
                //rpt.Load(Server.MapPath("~/CR_EstadoCuentaCliente_detallado.rpt"));
                //RE_GenericBean datos_cliente = DB.getDataClient(clienteID);
                //clienteNombre = datos_cliente.strC2;

                string sql_clientes = "";
                sql_clientes = " a.id_cliente=" + clienteID.ToString();
                ArrayList Arr_Cliente = (ArrayList)DB.getClientes(sql_clientes, user, "REPORTES");
                foreach (RE_GenericBean Bean_Cliente in Arr_Cliente)
                {
                    clienteNombre = Bean_Cliente.strC1;
                }
            }*/

            lbl_codigo_cliente.Text = clienteID.ToString();
            lbl_nombre_cliente.Text = clienteNombre;
            lbl_fecha_desde.Text = fechaini.Substring(0, 10);
            lbl_fecha_hasta.Text = fechafin.Substring(0, 10);
            lbl_moneda.Text = simbolomoneda;
            int ver_header_facturas = 0;
            int ver_header_notas_debito = 0;
            int ver_header_recibos = 0;
            int id_factura = 0;
            int id_factura2 = 0;
            int id_nd = 0;
            int id_nd2 = 0;
            int id_recibo = 0;
            int id_recibo2 = 0;
            int alternar = 1;
            decimal saldo_facturas = 0;
            decimal saldo_notas_debito = 0;
            decimal saldo_recibos = 0;
            string color = "";

            foreach (DataRow dr in ds.Tables["estadocuenta_detallado_tbl"].Rows)
            {
                if (alternar == 2) 
                {
	                color = "Silver";
	                alternar = 1;
                }
                else 
                {
	                color = "White";
	                alternar = 2;
                }
                if (dr["tipo"].ToString() == "F")
                {
                    if (ver_header_facturas == 0)
                    {
                        html_facturas += "<tr style=\"background-color:Gray; color:Black\"><td colspan=\"16\"><b>F</b></td></tr>"; 
                    }

                    id_factura = int.Parse(dr["id_fact"].ToString());

                    if (ver_header_facturas > 0)
                    {
                        if (id_factura != id_factura2)
                        {
                            saldo_facturas += total_saldo_facturas;
                            total_saldo_facturas = 0;
                        }
                    }
                    html_facturas += "<tr style=\"background-color:"+color+"\"><td>" + dr["tipo"].ToString() + "</td>"
                                     
                                     //+ "<td>" + dr["serie_fact"].ToString() + "</td>"
                                     //+ "<td>" + dr["corr_fact"].ToString() + "</td>"

                                     + "<td colspan=2  style='white-space: nowrap'>" + dr["serie_fact"].ToString() + "-" + dr["corr_fact"].ToString() + dr["orden_rango"].ToString() + "</td>"

                                     + "<td>" + dr["fecha_fact"].ToString() + "</td>"
                                     + "<td>" + dr["fecha_pago"].ToString() + "</td>"
                                     + "<td align=\"right\">" + decimal.Parse(dr["total_fact"].ToString()).ToString("#,#.00") + "</td>"
                                     + "<td>" + dr["serie_rcpt"].ToString() + "</td>"
                                     + "<td>" + dr["corr_rcpt"].ToString() + "</td>"
                                     + "<td>" + dr["fecha_rcpt"].ToString() + "</td>"
                                     + "<td align=\"right\">" + decimal.Parse(dr["abono_rcpt"].ToString()).ToString("#,#.00") + "</td>"
                                     + "<td align=\"right\">" + decimal.Parse(dr["saldo"].ToString()).ToString("#,#.00") + "</td>"
                                     + "<td>" + dr["contenedor"].ToString() + "</td>"
                                     + "<td>" + dr["hbl"].ToString() + "</td>"
                                     + "<td>" + dr["servicio"].ToString() + "</td>"
                                     + "<td>" + dr["poliza"].ToString() + "</td>"
                                     + "<td>" + dr["usuario"].ToString() + "</td></tr>";

                    id_factura2 = int.Parse(dr["id_fact"].ToString());

                    ver_header_facturas++;
                    total_facturas += decimal.Parse(dr["total_fact"].ToString());
                    total_abono_facturas += decimal.Parse(dr["abono_rcpt"].ToString());
                    total_saldo_facturas = decimal.Parse(dr["saldo"].ToString());
                }
                if (dr["tipo"].ToString() == "ND")
                {
                    if (ver_header_notas_debito == 0)
                    {
                        html_notas_debito += "<tr style=\"background-color:Gray; color:Black\"><td colspan=\"16\"><b>ND</b></td></tr>";
                    }

                    id_nd = int.Parse(dr["id_fact"].ToString());

                    if (ver_header_notas_debito > 0)
                    {
                        if (id_nd != id_nd2)
                        {
                            saldo_notas_debito += total_saldo_notas_debito;
                            total_saldo_notas_debito = 0;
                        }
                    }

                    html_notas_debito += "<tr  style=\"background-color:"+color+"\"><td>" + dr["tipo"].ToString() + "</td>"
                                     + "<td>" + dr["serie_fact"].ToString() + "</td>"
                                     + "<td>" + dr["corr_fact"].ToString() + "</td>"
                                     + "<td>" + dr["fecha_fact"].ToString() + "</td>"
                                     + "<td>" + dr["fecha_pago"].ToString() + "</td>"
                                     + "<td align=\"right\">" + decimal.Parse(dr["total_fact"].ToString()).ToString("#,#.00") + "</td>"
                                     + "<td>" + dr["serie_rcpt"].ToString() + "</td>"
                                     + "<td>" + dr["corr_rcpt"].ToString() + "</td>"
                                     + "<td>" + dr["fecha_rcpt"].ToString() + "</td>"
                                     + "<td align=\"right\">" + decimal.Parse(dr["abono_rcpt"].ToString()).ToString("#,#.00") + "</td>"
                                     + "<td align=\"right\">" + decimal.Parse(dr["saldo"].ToString()).ToString("#,#.00") + "</td>"
                                     + "<td>" + dr["contenedor"].ToString() + "</td>"
                                     + "<td>" + dr["hbl"].ToString() + "</td>"
                                     + "<td>" + dr["servicio"].ToString() + "</td>"
                                     + "<td>" + dr["poliza"].ToString() + "</td>"
                                     + "<td>" + dr["usuario"].ToString() + "</td></tr>";

                    id_nd2 = int.Parse(dr["id_fact"].ToString());

                    ver_header_notas_debito++;
                    total_notas_debito += decimal.Parse(dr["total_fact"].ToString());
                    total_abono_notas_debito += decimal.Parse(dr["abono_rcpt"].ToString());
                    total_saldo_notas_debito = decimal.Parse(dr["saldo"].ToString());
                }
                if (dr["tipo"].ToString() == "R")
                {
                    if (ver_header_recibos == 0)
                    {
                        html_recibos += "<tr style=\"background-color:Gray; color:Black\"><td colspan=\"16\"><b>R</b></td></tr>";
                    }

                    id_recibo = int.Parse(dr["id_fact"].ToString());

                    if (ver_header_recibos > 0)
                    {
                        if (id_recibo != id_recibo2)
                        {
                            saldo_recibos += total_saldo_recibos;
                            total_saldo_recibos = 0;
                        }
                    }

                    html_recibos += "<tr  style=\"background-color:" + color + "\"><td>" + dr["tipo"].ToString() + "</td>"
                                     + "<td>" + dr["serie_fact"].ToString() + "</td>"
                                     + "<td>" + dr["corr_fact"].ToString() + "</td>"
                                     + "<td>" + dr["fecha_fact"].ToString() + "</td>"
                                     + "<td></td>"
                                     + "<td align=\"right\">" + decimal.Parse(dr["total_fact"].ToString()).ToString("#,#.00") + "</td>"
                                     + "<td>" + dr["serie_rcpt"].ToString() + "</td>"
                                     + "<td>" + dr["corr_rcpt"].ToString() + "</td>"
                                     + "<td>" + dr["fecha_rcpt"].ToString() + "</td>"
                                     + "<td align=\"right\">" + decimal.Parse(dr["abono_rcpt"].ToString()).ToString("#,#.00") + "</td>"
                                     + "<td align=\"right\">" + decimal.Parse(dr["saldo"].ToString()).ToString("#,#.00") + "</td>"
                                     + "<td>" + dr["contenedor"].ToString() + "</td>"
                                     + "<td>" + dr["hbl"].ToString() + "</td>"
                                     + "<td>" + dr["servicio"].ToString() + "</td>"
                                     + "<td>" + dr["poliza"].ToString() + "</td>"
                                     + "<td>" + dr["usuario"].ToString() + "</td></tr>";

                    id_recibo2 = int.Parse(dr["id_fact"].ToString());

                    ver_header_recibos++;
                    total_recibos += decimal.Parse(dr["total_fact"].ToString());
                    total_abono_recibos += decimal.Parse(dr["abono_rcpt"].ToString());
                    total_saldo_recibos = decimal.Parse(dr["saldo"].ToString());
                }
            }
            if (ver_header_facturas > 0)
            {
                saldo_facturas += total_saldo_facturas;
                html_facturas += "<tr><td colspan=\"3\"><b>Totales para: F</b></td><td></td><td></td><td align=\"right\"><b>" + total_facturas.ToString("#,#.00") + "</b></td><td></td><td></td><td></td><td align=\"right\"><b>" + total_abono_facturas.ToString("#,#.00") + "</b></td><td align=\"right\"><b>" + saldo_facturas.ToString("#,#.00") + "</b></td><td></td><td></td><td></td><td></td><td></td></tr>";
            }
            if (ver_header_notas_debito > 0)
            {
                saldo_notas_debito += total_saldo_notas_debito;
                html_notas_debito += "<tr><td colspan=\"3\"><b>Totales para: ND</b></td><td></td><td></td><td align=\"right\"><b>" + total_notas_debito.ToString("#,#.00") + "</b></td><td></td><td></td><td></td><td align=\"right\"><b>" + total_abono_notas_debito.ToString("#,#.00") + "</b></td><td align=\"right\"><b>" + saldo_notas_debito.ToString("#,#.00") + "</b></td><td></td><td></td><td></td><td></td><td></td></tr>";
            }
            if (ver_header_recibos > 0)
            {
                saldo_recibos += total_saldo_recibos;
                html_recibos += "<tr><td colspan=\"3\"><b>Totales para: R</b></td><td></td><td></td><td align=\"right\"><b>" + total_recibos.ToString("#,#.00") + "</b></td><td></td><td></td><td></td><td align=\"right\"><b>" + total_abono_recibos.ToString("#,#.00") + "</b></td><td align=\"right\"><b>" + saldo_recibos.ToString("#,#.00") + "</b></td><td></td><td></td><td></td><td></td><td></td></tr>";
            }

            total_saldo_cliente = saldo_facturas + saldo_notas_debito + saldo_recibos;
            lbl_total_saldo_cliente.Text = total_saldo_cliente.ToString("#,#.00");
            
        }
    }
}