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

public partial class Reports_EstadoCuentaProveedorHtml : System.Web.UI.Page
{
    UsuarioBean user = null;
    public string html_sincorte = "";
    public string html_cortes = "";
    decimal total_sincorte = 0;
    decimal total_cortes = 0;
    decimal total_abono_sincorte = 0;
    decimal total_abono_cortes = 0;
    //decimal total_saldo_facturas = 0;
    decimal total_saldo_cortes = 0;
    decimal total_saldo_cliente = 0;
    decimal total_valor_cortes = 0;
    decimal total_valor_sincortes = 0;
    decimal total_abonos_proveedor = 0;
    decimal total_saldo_proveedor = 0;

    decimal total_saldo_fixed = 0; //2020-09-02

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

        DB.parametros Params = DB.EmpresaParametros(paisID, "", "", "ESTADO DE CUENTA", ""); //pais, sistema, doc_id, titulo, edicion
        Label1.Text = Params.titulo;
        Image1.ImageUrl = "data:image;base64," + System.Convert.ToBase64String(Params.logo);
        Image1.Height = 63;
        Image1.Width = 237;

        int solopendiente = int.Parse(Request.QueryString["solopendiente"].ToString());
        string pais = Request.QueryString["pais"].ToString();
        string consMon = Request.QueryString["consolida_moneda"].ToString();
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

            if (tpi != 3)
            {
                //Cortes
                where = " and tcp_conta_id=" + contaID + " and tcp_mon_id=" + monID + " and tcp_tpi_id=" + tpi + " and tcp_ted_id<>3";
                if (fechaini != null && !fechaini.Equals("")) where += " and tcp_fecha_creacion>='" + fechaini + "'";
                if (fechafin != null && !fechafin.Equals("")) where += " and tcp_fecha_creacion<='" + fechafin + "'";
                where += " and tcp_pai_id=" + pais + " ";
                //Notas Debito
                where2 = " and tnd_tcon_id=" + contaID + " and tnd_moneda=" + monID + " and tnd_tpi_id=" + tpi + " and tnd_ted_id<>3";
                if (fechaini != null && !fechaini.Equals("")) where2 += " and tnd_fecha_emision>='" + fechaini + "'";
                if (fechafin != null && !fechafin.Equals("")) where2 += " and tnd_fecha_emision<='" + fechafin + "'";
                where2 += " and tnd_pai_id=" + pais + " ";
                //Notas Credito
                where3 = " and tnc_tcon_id=" + contaID + " and tnc_mon_id=" + monID + " and tnc_tpi_id=" + tpi + " and tnc_ted_id<>3 ";
                if (fechaini != null && !fechaini.Equals("")) where3 += " and tnc_fecha>='" + fechaini + "'";
                if (fechafin != null && !fechafin.Equals("")) where3 += " and tnc_fecha<='" + fechafin + "'";
                where3 += " and tnc_pai_id=" + pais + " ";
                //Recibos de Soas
                where4 = " and trc_tcon_id=" + contaID + " and trc_moneda_id=" + monID + " and trc_tpi_id=" + tpi + " and trc_ted_id<>3 ";
                if (fechaini != null && !fechaini.Equals("")) where4 += " and trc_fecha>='" + fechaini + "'";
                if (fechafin != null && !fechafin.Equals("")) where4 += " and trc_fecha<='" + fechafin + "'";
                where4 += " and trc_pai_id=" + pais + " ";
                //Cheques y Transferencias
                where5 = " and tcg_tcon_id=" + contaID + " and tcg_tpi_id=" + tpi + " and tcg_ted_id=4 ";
                if (fechaini != null && !fechaini.Equals("")) where5 += " and tcg_fecha_emision>='" + fechaini + "'";
                if (fechafin != null && !fechafin.Equals("")) where5 += " and tcg_fecha_emision<='" + fechafin + "'";
                where5 += " and tcg_pai_id=" + pais + " and tcg_ttm_id=" + monID + " ";
                //-provisiones sin corte
                where6 = " and to_char(tpr_fecha_acepta,'yyyy-mm-dd')>='" + fechaini + "' and to_char(tpr_fecha_acepta,'yyyy-mm-dd')<='" + fechafin + "' and tpr_mon_id=" + monID + " and tpr_tcon_id=" + contaID + " ";
                where6 += " and tpr_pai_id=" + pais + " and tpr_ted_id not in (1,3) ";
                where6 += " and tpr_prov_id not in (select b.tcpd_docto_id from tbl_corte_proveedor a join tbl_corte_proveedor_detalle b  on b.tcpd_tcp_id = a.tcp_id and b.tcpd_docto_id = tpr_prov_id and a.tcp_mon_id =" + monID + "  ";
                where6 += " and a.tcp_conta_id =" + contaID + " and tcp_pai_id =" + pais + " and tcp_cli_id =" + clienteID + " and a.tcp_fecha_creacion>= '" + fechaini + "' and a.tcp_fecha_creacion<= '" + fechafin + "' ";
                where6 += " and a.tcp_tpi_id =" + tpi + " and a.tcp_ted_id <>3  )  ";
                where6 += " and tpr_tpi_id=" + tpi + " group by tpr_prov_id, tpr_correlativo, ";
                where6 += " tpr_serie, tpr_fecha_acepta,tpr_contenedor,tpr_hbl, tpr_valor,poliza, usuario having (tpr_valor-coalesce((select sum(abono) from (  ";
                where6 += "  select sum(b.tcg_valor) as abono    from tbl_cheques_generados b, tbl_cheque_corte c, tbl_corte_proveedor_detalle d   ";
                where6 += " where b.tcg_id=c.corte_tcg_id and c.corte_tcp_id=d.tcpd_tcp_id and d.tcpd_docto_id=tpr_prov_id    ";
                where6 += " and to_char(b.tcg_fecha_emision,'yyyy-mm-dd')<='" + fechafin + "' and b.tcg_ted_id<>3    and d.tcpd_str_id in (5,10)   union all ";
                where6 += " select sum(d.trp_monto) as abono    from tbl_retencion_provision d    where d.trp_tpr_prov_id=tpr_prov_id    ";
                where6 += " and to_char(d.trp_fecha_generacion,'yyyy-mm-dd')<='" + fechafin + "' and d.trp_ted_id<>3 ) abonos),0))>0 and tpr_prov_id not in (       ";
                where6 += " select tpr_prov_id from aux_prov_id where tpr_proveedor_id=" + clienteID + " and tpr_tpi_id=" + tpi + " and tpr_pai_id=" + pais + " and tpr_mon_id=" + monID + " and ";
                where6 += " tpr_tcon_id=" + contaID + " and  tcg_fecha_emision<='" + fechafin + "'   ) ";
                //provisioens sin corte anuladas
                //-provisiones sin corte
                where6_1 = " and to_char(tpr_fecha_acepta,'yyyy-mm-dd')>='" + fechaini + "' and to_char(tpr_fecha_acepta,'yyyy-mm-dd')<='" + fechafin + "' and tpr_mon_id=" + monID + " and tpr_tcon_id=" + contaID + " ";
                where6_1 += " and tpr_pai_id=" + pais + " and tpr_ted_id =3 ";
                where6_1 += " and tpr_prov_id not in (select b.tcpd_docto_id from tbl_corte_proveedor a join tbl_corte_proveedor_detalle b  on b.tcpd_tcp_id = a.tcp_id and b.tcpd_docto_id = tpr_prov_id and a.tcp_mon_id =" + monID + "  ";
                where6_1 += " and a.tcp_conta_id =" + contaID + " and tcp_pai_id =" + pais + " and tcp_cli_id =" + clienteID + " and a.tcp_fecha_creacion>= '" + fechaini + "' and a.tcp_fecha_creacion<= '" + fechafin + "' ";
                where6_1 += " and a.tcp_tpi_id =" + tpi + " and a.tcp_ted_id <>3  )  ";
                where6_1 += " and tpr_tpi_id=" + tpi + " group by tpr_prov_id, tpr_correlativo, ";
                where6_1 += " tpr_serie, tpr_fecha_acepta,tpr_contenedor,tpr_hbl, tpr_valor,poliza, usuario having (tpr_valor-coalesce((select sum(abono) from (  ";
                where6_1 += "  select sum(b.tcg_valor) as abono    from tbl_cheques_generados b, tbl_cheque_corte c, tbl_corte_proveedor_detalle d   ";
                where6_1 += " where b.tcg_id=c.corte_tcg_id and c.corte_tcp_id=d.tcpd_tcp_id and d.tcpd_docto_id=tpr_prov_id    ";
                where6_1 += " and to_char(b.tcg_fecha_emision,'yyyy-mm-dd')<='" + fechafin + "' and b.tcg_ted_id<>3    and d.tcpd_str_id in (5,10)   union all ";
                where6_1 += " select sum(d.trp_monto) as abono    from tbl_retencion_provision d    where d.trp_tpr_prov_id=tpr_prov_id    ";
                where6_1 += " and to_char(d.trp_fecha_generacion,'yyyy-mm-dd')<='" + fechafin + "' and d.trp_ted_id<>3 ) abonos),0))>0 and tpr_prov_id not in (       ";
                where6_1 += " select tpr_prov_id from aux_prov_id where tpr_proveedor_id=" + clienteID + " and tpr_tpi_id=" + tpi + " and tpr_pai_id=" + pais + " and tpr_mon_id=" + monID + " and ";
                where6_1 += " tpr_tcon_id=" + contaID + " and  tcg_fecha_emision<='" + fechafin + "'   ) ";

                //notas debito
                where7 = "  and  to_char(a.tnd_fecha_emision,'yyyy-mm-dd')>='" + fechaini + "' and  to_char(a.tnd_fecha_emision,'yyyy-mm-dd')<='" + fechafin + "' and a.tnd_pai_id=" + pais + "  and a.tnd_moneda=" + monID + " and a.tnd_tcon_id=" + contaID + " and a.tnd_ted_id<>3 and a.tnd_id not in  ";
                where7 += " (select b.tcpd_docto_id from tbl_corte_proveedor a join tbl_corte_proveedor_detalle b  on b.tcpd_tcp_id = a.tcp_id and b.tcpd_docto_id = tnd_id and a.tcp_mon_id =" + monID + " and b.tcpd_str_id=4   ";
                where7 += " and a.tcp_conta_id =" + contaID + " and tcp_pai_id =" + pais + " and tcp_cli_id =" + clienteID + " and a.tcp_fecha_creacion>= '" + fechaini + "' and a.tcp_fecha_creacion<= '" + fechafin + "'  and a.tcp_tpi_id =" + tpi + " and a.tcp_ted_id <>3  )  ";
                where7 += "  and a.tnd_tpi_id=" + tpi + " and tnd_id not in   ( select tnd_id from aux_nd_id where   tnd_cli_id=" + clienteID + " and   tnd_tpi_id=" + tpi + " and tnd_pai_id=" + pais + " and tnd_moneda=" + monID + " and tnd_tcon_id=" + contaID + "  and fecha<='" + fechafin + "'    )   group by serie,correlavito,fecha2,tnd_contenedor,hbl,poliza, usuario ";

                //anuladas notas debito

                where7_1 = "  and  to_char(a.tnd_fecha_emision,'yyyy-mm-dd')>='" + fechaini + "' and  to_char(a.tnd_fecha_emision,'yyyy-mm-dd')<='" + fechafin + "' and a.tnd_pai_id=" + pais + "  and a.tnd_moneda=" + monID + " and a.tnd_tcon_id=" + contaID + " and a.tnd_ted_id=3 and a.tnd_id not in  ";
                where7_1 += " (select b.tcpd_docto_id from tbl_corte_proveedor a join tbl_corte_proveedor_detalle b  on b.tcpd_tcp_id = a.tcp_id and b.tcpd_docto_id = tnd_id and a.tcp_mon_id =" + monID + " and b.tcpd_str_id=4   ";
                where7_1 += " and a.tcp_conta_id =" + contaID + " and tcp_pai_id =" + pais + " and tcp_cli_id =" + clienteID + " and a.tcp_fecha_creacion>= '" + fechaini + "' and a.tcp_fecha_creacion<= '" + fechafin + "'  and a.tcp_tpi_id =" + tpi + " and a.tcp_ted_id <>3  )  ";
                where7_1 += "  and a.tnd_tpi_id=" + tpi + " and tnd_id not in   ( select tnd_id from aux_nd_id where   tnd_cli_id=" + clienteID + " and   tnd_tpi_id=" + tpi + " and tnd_pai_id=" + pais + " and tnd_moneda=" + monID + " and tnd_tcon_id=" + contaID + "  and fecha<='" + fechafin + "'    )   group by serie,correlavito,fecha2,tnd_contenedor,hbl,poliza,usuario ";

                where7_2 = "  and  to_char(tfr_fecha_abono,'yyyy-mm-dd')>'" + fechafin + "'  and tnd_pai_id=" + pais + "  and tnd_moneda=" + monID + " and tnd_tcon_id=" + contaID + " and tnd_ted_id=3 and tnd_fecha_emision <='" + fechafin + "'  and tnd_id not in  ";
                where7_2 += " (select b.tcpd_docto_id from tbl_corte_proveedor a join tbl_corte_proveedor_detalle b  on b.tcpd_tcp_id = a.tcp_id and b.tcpd_docto_id = tnd_id and a.tcp_mon_id =" + monID + " and b.tcpd_str_id=4   ";
                where7_2 += " and a.tcp_conta_id =" + contaID + " and tcp_pai_id =" + pais + " and tcp_cli_id =" + clienteID + " and a.tcp_fecha_creacion>= '" + fechaini + "' and a.tcp_fecha_creacion<= '" + fechafin + "'  and a.tcp_tpi_id =" + tpi + " and a.tcp_ted_id <>3  )  ";
                where7_2 += "  and tnd_tpi_id=" + tpi + " and tnd_id not in   ( select tnd_id from aux_nd_id where   tnd_cli_id=" + clienteID + " and   tnd_tpi_id=" + tpi + " and tnd_pai_id=" + pais + " and tnd_moneda=" + monID + " and tnd_tcon_id=" + contaID + "  and fecha<='" + fechafin + "'    )    ";

                //notas credito
                where8 = "  and  to_char(a.tnc_fecha_emision,'yyyy-mm-dd')>='" + fechaini + "' and  to_char(a.tnc_fecha_emision,'yyyy-mm-dd')<='" + fechafin + "' and a.tnc_pai_id=" + pais + "  and a.tnc_mon_id=" + monID + " and a.tnc_tcon_id=" + contaID + " and a.tnc_ted_id<>3 and a.tnc_id not in  ";
                where8 += " (select b.tcpd_docto_id from tbl_corte_proveedor a join tbl_corte_proveedor_detalle b  on b.tcpd_tcp_id = a.tcp_id and b.tcpd_docto_id = tnc_id and a.tcp_mon_id =" + monID + " and b.tcpd_str_id in (12,31)   ";
                where8 += " and a.tcp_conta_id =" + contaID + " and tcp_pai_id =" + pais + " and tcp_cli_id =" + clienteID + " and a.tcp_fecha_creacion>= '" + fechaini + "' and a.tcp_fecha_creacion<= '" + fechafin + "'  and a.tcp_tpi_id =" + tpi + " and a.tcp_ted_id <>3  )  ";
                where8 += "  and a.tnc_tpi_id=" + tpi + " and tnc_id not in   ( select tnc_id from aux_nc_id where   tnc_cli_id=" + clienteID + " and   tnc_tpi_id=" + tpi + " and tnc_pai_id=" + pais + " and tnc_mon_id=" + monID + " and tnc_tcon_id=" + contaID + "  and fecha<='" + fechafin + "'    )   group by serie,correlavito,fecha2,tnc_contenedor,hbl,poliza,usuario ";

                //nc directa
                where8_2 = "  and  to_char(a.tnc_fecha_emision,'yyyy-mm-dd')>='" + fechaini + "' and  to_char(a.tnc_fecha_emision,'yyyy-mm-dd')<='" + fechafin + "' and a.tnc_pai_id=" + pais + "  and a.tnc_mon_id=" + monID + " and a.tnc_tcon_id=" + contaID + " and a.tnc_ted_id<>3 and a.tnc_id not in  ";
                where8_2 += " (select b.tcpd_docto_id from tbl_corte_proveedor a join tbl_corte_proveedor_detalle b  on b.tcpd_tcp_id = a.tcp_id and b.tcpd_docto_id = tnc_id and a.tcp_mon_id =" + monID + " and b.tcpd_str_id=12   ";
                where8_2 += " and a.tcp_conta_id =" + contaID + " and tcp_pai_id =" + pais + " and tcp_cli_id =" + clienteID + " and a.tcp_fecha_creacion>= '" + fechaini + "' and a.tcp_fecha_creacion<= '" + fechafin + "'  and a.tcp_tpi_id =" + tpi + " and a.tcp_ted_id <>3  )  ";
                where8_2 += "  and a.tnc_tpi_id=" + tpi + " and tnc_id not in   ( select tnc_id from aux_nc_id where   tnc_cli_id=" + clienteID + " and   tnc_tpi_id=" + tpi + " and tnc_pai_id=" + pais + " and tnc_mon_id=" + monID + " and tnc_tcon_id=" + contaID + "  and fecha<='" + fechafin + "'    )   group by a.tnc_id ";

                //anuladas notas credito pv
                where8_1 = "  and  to_char(a.tnc_fecha_emision,'yyyy-mm-dd')>='" + fechaini + "' and  to_char(a.tnc_fecha_emision,'yyyy-mm-dd')<='" + fechafin + "' and a.tnc_pai_id=" + pais + "  and a.tnc_mon_id=" + monID + " and a.tnc_tcon_id=" + contaID + " and a.tnc_ted_id=3 and a.tnc_id not in  ";
                where8_1 += " (select b.tcpd_docto_id from tbl_corte_proveedor a join tbl_corte_proveedor_detalle b  on b.tcpd_tcp_id = a.tcp_id and b.tcpd_docto_id = tnc_id and a.tcp_mon_id =" + monID + " and b.tcpd_str_id in (12,31)   ";
                where8_1 += " and a.tcp_conta_id =" + contaID + " and tcp_pai_id =" + pais + " and tcp_cli_id =" + clienteID + " and a.tcp_fecha_creacion>= '" + fechaini + "' and a.tcp_fecha_creacion<= '" + fechafin + "'  and a.tcp_tpi_id =" + tpi + " and a.tcp_ted_id <>3  )  ";
                where8_1 += "  and a.tnc_tpi_id=" + tpi + " and tnc_id not in   ( select tnc_id from aux_nc_id where   tnc_cli_id=" + clienteID + " and   tnc_tpi_id=" + tpi + " and tnc_pai_id=" + pais + " and tnc_mon_id=" + monID + " and tnc_tcon_id=" + contaID + "  and fecha<='" + fechafin + "'    )   group by serie,correlavito,fecha2,tnc_contenedor,hbl,poliza,usuario,a.tnc_id ";

                //anuladas cheques
                where5_1 = " and    a.tcg_ted_id =3  and a.tcg_tcon_id =" + contaID + " and a.tcg_pai_id =" + pais + " and a.tcg_ttm_id =" + monID + "  and a.tcg_tpi_id=" + tpi + "  and a.tcg_fecha_emision >='" + fechaini + "'  and  a.tcg_fecha_emision <='" + fechafin + "'  group by tcg_cuenta,tcg_numero ,tcg_fecha_emision,tcg_valor,poliza,usuario ";
                //recibos soas anulados
                where4_1 = "  and a.trc_ted_id = 3  and a.trc_tcon_id =" + contaID + " and a.trc_pai_id =" + pais + " and  a.trc_moneda_id =" + monID + "  and a.trc_tpi_id=" + tpi + " and a.trc_fecha_emision>='" + fechaini + "' and a.trc_fecha_emision<='" + fechafin + "'   group by trc_serie,trc_correlativo,trc_fecha_emision,trc_contenedor,trc_hbl,poliza,usuario ";
                //recibos a favor proveedor
                where4_2 = "  and a.trc_ted_id <> 3  and a.trc_tcon_id =" + contaID + " and a.trc_pai_id =" + pais + " and  a.trc_moneda_id =" + monID + "  and a.trc_tpi_id=" + tpi + " and a.trc_fecha_emision>='" + fechaini + "' and a.trc_fecha_emision<='" + fechafin + "'   group by trc_serie,trc_correlativo,trc_fecha_emision,trc_contenedor,trc_hbl,poliza,usuario ";
                where4_2 += " having  coalesce(sum(cast(trc_monto as numeric) - coalesce((select sum(tcr_abono) from (select sum(tcr_abono) as tcr_abono  from tbl_corte_abono where tcr_tre_id=trc_id and tcr_sysref_id = 22 and to_char(tcr_fecha_abono,'yyyy-mm-dd')<='" + fechafin + " 23:59:59')abono),0)))  > 0 ";
                //Cheques liquidaciones
                where5_2 = " and tcg_tcon_id=" + contaID + " and tcg_tpi_id=" + tpi + (solopendiente == 1 ? " and tcg_ted_id not in(4,3) " : " and tcg_ted_id not in(3) ");
                if (fechaini != null && !fechaini.Equals("")) where5_2 += " and tcg_fecha_emision>='" + fechaini + "'";
                if (fechafin != null && !fechafin.Equals("")) where5_2 += " and tcg_fecha_emision<='" + fechafin + "'";
                where5_2 += " and tcg_pai_id=" + pais + " and tcg_ttm_id=" + monID + " and tcg_no_transferencia is null ";
                where5_3 = " and tcg_tcon_id=" + contaID + " and tcg_tpi_id=" + tpi + " and tcg_ted_id=14 ";
                if (fechaini != null && !fechaini.Equals("")) where5_3 += " and tcg_fecha_emision>='" + fechaini + "'";
                if (fechafin != null && !fechafin.Equals("")) where5_3 += " and tcg_fecha_emision<='" + fechafin + "'";
                where5_3 += " and tcg_pai_id=" + pais + " and tcg_ttm_id=" + monID + " and tcg_no_transferencia is null ";
                //transferencia liquidaciones
                where5_4 = " and tcg_tcon_id=" + contaID + " and tcg_tpi_id=" + tpi + " and tcg_ted_id<>3 ";
                if (fechaini != null && !fechaini.Equals("")) where5_4 += " and tcg_fecha_emision>='" + fechaini + "'";
                if (fechafin != null && !fechafin.Equals("")) where5_4 += " and tcg_fecha_emision<='" + fechafin + "'";
                where5_4 += " and tcg_pai_id=" + pais + " and tcg_ttm_id=" + monID + " and tcg_no_transferencia is not null ";
                where5_5 = " and tcg_tcon_id=" + contaID + " and tcg_tpi_id=" + tpi + " and tcg_ted_id=14 ";
                if (fechaini != null && !fechaini.Equals("")) where5_5 += " and tcg_fecha_emision>='" + fechaini + "'";
                if (fechafin != null && !fechafin.Equals("")) where5_5 += " and tcg_fecha_emision<='" + fechafin + "'";
                where5_5 += " and tcg_pai_id=" + pais + " and tcg_ttm_id=" + monID + " and tcg_no_transferencia is not null ";
                //depositos liquidaciones
                where5_6 = " and c.tli_ted_id <>3    and tli_conta_id=" + contaID + " and tli_tpi_id=" + tpi + (solopendiente == 1 ? " and tmb_ted_id not in(4,3) " : " and tmb_ted_id<>3 ") + " and tli_pai_id=" + pais + " and tmb_mon_id=" + monID + " ";
                if (fechaini != null && !fechaini.Equals("")) where5_6 += " and tmb_fecha>='" + fechaini + "'";
                if (fechafin != null && !fechafin.Equals("")) where5_6 += " and tmb_fecha<='" + fechafin + "'";


                //Facturas intercompnay
                where9 = "  and  to_char(a.tfa_fecha_emision,'yyyy-mm-dd')>='" + fechaini + "' and  to_char(a.tfa_fecha_emision,'yyyy-mm-dd')<='" + fechafin + "' and a.tfa_pai_id=" + pais + "  and a.tfa_moneda=" + monID + " and a.tfa_conta_id=" + contaID + " and a.tfa_ted_id<>3 and a.tfa_id not in  ";
                where9 += " (select b.tcpd_docto_id from tbl_corte_proveedor a join tbl_corte_proveedor_detalle b  on b.tcpd_tcp_id = a.tcp_id and b.tcpd_docto_id = tfa_id and a.tcp_mon_id =" + monID + " and b.tcpd_str_id=1   ";
                where9 += " and a.tcp_conta_id =" + contaID + " and tcp_pai_id =" + pais + " and tcp_cli_id =" + clienteID + " and a.tcp_fecha_creacion>= '" + fechaini + "' and a.tcp_fecha_creacion<= '" + fechafin + "'  and a.tcp_tpi_id =" + tpi + " and a.tcp_ted_id <>3  )  ";
                where9 += "  and a.tfa_tpi_id=" + tpi + " and tfa_id not in   ( select tfa_id from aux_fa_id where   tfa_cli_id=" + clienteID + " and   tfa_tpi_id=" + tpi + " and tfa_pai_id=" + pais + " and tfa_moneda=" + monID + " and tfa_conta_id=" + contaID + "  and fecha<='" + fechafin + "'    )   group by serie,correlavito,fecha2,tfa_contenedor,hbl,poliza, usuario ";

                //anuladas facturas intercompany

                where9_1 = "  and  to_char(a.tfa_fecha_emision,'yyyy-mm-dd')>='" + fechaini + "' and  to_char(a.tfa_fecha_emision,'yyyy-mm-dd')<='" + fechafin + "' and a.tfa_pai_id=" + pais + "  and a.tfa_moneda=" + monID + " and a.tfa_conta_id=" + contaID + " and a.tfa_ted_id=3 and a.tfa_id not in  ";
                where9_1 += " (select b.tcpd_docto_id from tbl_corte_proveedor a join tbl_corte_proveedor_detalle b  on b.tcpd_tcp_id = a.tcp_id and b.tcpd_docto_id = tfa_id and a.tcp_mon_id =" + monID + " and b.tcpd_str_id=1   ";
                where9_1 += " and a.tcp_conta_id =" + contaID + " and tcp_pai_id =" + pais + " and tcp_cli_id =" + clienteID + " and a.tcp_fecha_creacion>= '" + fechaini + "' and a.tcp_fecha_creacion<= '" + fechafin + "'  and a.tcp_tpi_id =" + tpi + " and a.tcp_ted_id <>3  )  ";
                where9_1 += "  and a.tfa_tpi_id=" + tpi + " and tfa_id not in   ( select tfa_id from aux_fa_id where   tfa_cli_id=" + clienteID + " and   tfa_tpi_id=" + tpi + " and tfa_pai_id=" + pais + " and tfa_moneda=" + monID + " and tfa_conta_id=" + contaID + "  and fecha<='" + fechafin + "'    )   group by serie,correlavito,fecha2,tfa_contenedor,hbl,poliza,usuario ";

                where9_2 = "  and  to_char(tfr_fecha_abono,'yyyy-mm-dd')>'" + fechafin + "'  and tfa_pai_id=" + pais + "  and tfa_moneda=" + monID + " and tfa_conta_id=" + contaID + " and tfa_ted_id=3 and tfa_fecha_emision <='" + fechafin + "'  and tfa_id not in  ";
                where9_2 += " (select b.tcpd_docto_id from tbl_corte_proveedor a join tbl_corte_proveedor_detalle b  on b.tcpd_tcp_id = a.tcp_id and b.tcpd_docto_id = tfa_id and a.tcp_mon_id =" + monID + " and b.tcpd_str_id=1   ";
                where9_2 += " and a.tcp_conta_id =" + contaID + " and tcp_pai_id =" + pais + " and tcp_cli_id =" + clienteID + " and a.tcp_fecha_creacion>= '" + fechaini + "' and a.tcp_fecha_creacion<= '" + fechafin + "'  and a.tcp_tpi_id =" + tpi + " and a.tcp_ted_id <>3  )  ";
                where9_2 += "  and tfa_tpi_id=" + tpi + " and tfa_id not in   ( select tfa_id from aux_fa_id where   tfa_cli_id=" + clienteID + " and   tfa_tpi_id=" + tpi + " and tfa_pai_id=" + pais + " and tfa_moneda=" + monID + " and tfa_conta_id=" + contaID + "  and fecha<='" + fechafin + "'    )    ";

                //anuladas notas credito pv
                where10_1 = "  and  to_char(a.tnc_fecha_emision,'yyyy-mm-dd')>='" + fechaini + "' and  to_char(a.tnc_fecha_emision,'yyyy-mm-dd')<='" + fechafin + "' and a.tnc_pai_id=" + pais + "  and a.tnc_mon_id=" + monID + " and a.tnc_tcon_id=" + contaID + " and a.tnc_ted_id=3 and a.tnc_id not in  ";
                where10_1 += " (select b.tcpd_docto_id from tbl_corte_proveedor a join tbl_corte_proveedor_detalle b  on b.tcpd_tcp_id = a.tcp_id and b.tcpd_docto_id = tnc_id and a.tcp_mon_id =" + monID + " and b.tcpd_str_id in (3)   ";
                where10_1 += " and a.tcp_conta_id =" + contaID + " and tcp_pai_id =" + pais + " and tcp_cli_id =" + clienteID + " and a.tcp_fecha_creacion>= '" + fechaini + "' and a.tcp_fecha_creacion<= '" + fechafin + "'  and a.tcp_tpi_id =" + tpi + " and a.tcp_ted_id <>3  )  ";
                where10_1 += "  and a.tnc_tpi_id=" + tpi + " and tnc_id not in   ( select tnc_id from aux_nc_id where   tnc_cli_id=" + clienteID + " and   tnc_tpi_id=" + tpi + " and tnc_pai_id=" + pais + " and tnc_mon_id=" + monID + " and tnc_tcon_id=" + contaID + "  and fecha<='" + fechafin + "'    )  and tnc_ttr_id=3   group by serie,correlavito,fecha2,tnc_contenedor,hbl,poliza,usuario,a.tnc_id ";



                ds = DB.getEstadoCuentaProveedor(clienteID, where, where2, where3, where4, where5, fechafin, monID, solopendiente, tpi, where6, where7, where6_1, where7_1, where8, where8_1, where5_1, where4_1, where4_2, where5_2, where5_3, where5_4, where5_5, where5_6, where8_2, where7_2, where9, where9_1, where9_2, where10_1, user.PaisID);

            }

            lbl_tipo_persona.Text = tipoPersona;
            lbl_codigo_cliente.Text = clienteID.ToString();
            lbl_nombre_cliente.Text = clienteNombre;
            lbl_fecha_desde.Text = fechaini.Substring(0, 10);
            lbl_fecha_hasta.Text = fechafin.Substring(0, 10);
            lbl_moneda.Text = simbolomoneda;
            int ver_header_sincorte = 0;
            int ver_header_cortes = 0;
            int alternar = 1;
            string color = "";
            int id_corte = 0;
            int id_corte2 = 0;

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

                if (dr["tipo"].ToString() == "CT")
                {
                    if (ver_header_cortes == 0)
                    {
                        html_cortes += "<tr style=\"background-color:White; color:Black\"><td colspan=\"16\"><b>CT</b></td></tr>";
                    }
                    id_corte = int.Parse(dr["id_fact"].ToString());

                    if (ver_header_cortes > 0)
                    {
                        if (id_corte != id_corte2)
                        {
                            html_cortes += "<tr><td colspan=\"3\"></td><td></td><td align=\"right\"><b>" + total_cortes.ToString("#,#.00") + "</b></td><td></td><td></td><td></td><td></td><td align=\"right\"><b>" + total_abono_cortes.ToString("#,#.00") + "</b></td><td></td><td></td><td></td><td></td><td></td><td></td></tr>";
                            total_cortes = 0;
                            total_abono_cortes = 0;
                        }
                    }

                    html_cortes += "<tr style=\"background-color:" + color + "\"><td>" + dr["tipo"].ToString() + "</td>"
                                     + "<td>" + dr["serie_fact"].ToString() + "</td>"
                                     + "<td>" + dr["corr_fact"].ToString() + "</td>"
                                     + "<td>" + dr["fecha_fact"].ToString() + "</td>"
                                     + "<td align=\"right\">" + decimal.Parse(dr["total_fact"].ToString()).ToString("#,#.00") + "</td>"
                                     + "<td>" + dr["serie_rcpt"].ToString() + "</td>"
                                     + "<td>" + dr["corr_rcpt"].ToString() + "</td>"
                                     + "<td>" + dr["fecha_rcpt"].ToString() + "</td>"
                                     + "<td align=\"right\">" + decimal.Parse(dr["valor_fact"].ToString()).ToString("#,#.00") + "</td>"
                                     + "<td align=\"right\">" + decimal.Parse(dr["abono_rcpt"].ToString()).ToString("#,#.00") + "</td>"
                                     + "<td align=\"right\">" + decimal.Parse(dr["saldo"].ToString()).ToString("#,#.00") + "</td>"
                                     + "<td>" + dr["contenedor"].ToString() + "</td>"
                                     + "<td>" + dr["hbl"].ToString() + "</td>"
                                     + "<td>" + dr["servicio"].ToString() + "</td>"
                                     + "<td>" + dr["poliza"].ToString() + "</td>"
                                     + "<td>" + dr["usuario"].ToString() + "</td></tr>";

                    id_corte2 = int.Parse(dr["id_fact"].ToString());

                    total_cortes += decimal.Parse(dr["total_fact"].ToString());
                    total_abono_cortes += decimal.Parse(dr["abono_rcpt"].ToString());
                    ver_header_cortes++;
                    //total_saldo_cortes += decimal.Parse(dr["saldo"].ToString());
                    total_valor_cortes += decimal.Parse(dr["valor_fact"].ToString());
                }

                if (dr["tipo"].ToString() == "SC")
                {
                    if (ver_header_sincorte == 0)
                    {
                        html_sincorte += "<tr style=\"background-color:White; color:Black\"><td colspan=\"16\"><b>SC</b></td></tr>";
                    }
                    html_sincorte += "<tr style=\"background-color:" + color + "\"><td>" + dr["tipo"].ToString() + "</td>"
                                     + "<td>" + dr["serie_fact"].ToString() + "</td>"
                                     + "<td>" + dr["corr_fact"].ToString() + "</td>"
                                     + "<td>" + dr["fecha_fact"].ToString() + "</td>"
                                     + "<td align=\"right\">" + decimal.Parse(dr["total_fact"].ToString()).ToString("#,#.00") + "</td>"
                                     + "<td>" + dr["serie_rcpt"].ToString() + "</td>"
                                     + "<td>" + dr["corr_rcpt"].ToString() + "</td>"
                                     + "<td>" + dr["fecha_rcpt"].ToString() + "</td>"
                                     + "<td align=\"right\">" + decimal.Parse(dr["valor_fact"].ToString()).ToString("#,#.00") + "</td>"
                                     + "<td align=\"right\">" + decimal.Parse(dr["abono_rcpt"].ToString()).ToString("#,#.00") + "</td>"
                                     + "<td align=\"right\">" + decimal.Parse(dr["saldo"].ToString()).ToString("#,#.00") + "</td>"
                                     + "<td>" + dr["contenedor"].ToString() + "</td>"
                                     + "<td>" + dr["hbl"].ToString() + "</td>"
                                     + "<td>" + dr["servicio"].ToString() + "</td>"
                                     + "<td>" + dr["poliza"].ToString() + "</td>"
                                     + "<td>" + dr["usuario"].ToString() + "</td></tr>";
                    ver_header_sincorte++;
                    total_sincorte += decimal.Parse(dr["total_fact"].ToString());
                    total_abono_sincorte += decimal.Parse(dr["abono_rcpt"].ToString());
                    total_valor_sincortes += decimal.Parse(dr["valor_fact"].ToString());
                    //total_saldo_facturas += decimal.Parse(dr["saldo"].ToString());
                }

                total_saldo_fixed += decimal.Parse(dr[14].ToString()); //2020-09-02

                total_abonos_proveedor += decimal.Parse(dr["abono_rcpt"].ToString());
            }

            total_abonos_proveedor = total_abonos_proveedor * -1;

            if (ver_header_cortes > 0)
            {
                html_cortes += "<tr><td colspan=\"3\"></td><td></td><td align=\"right\"><b>" + total_cortes.ToString("#,#.00") + "</b></td><td></td><td></td><td></td><td align=\"right\"><b>" + total_valor_cortes.ToString("#,#.00") + "</b></td><td align=\"right\"><b>" + total_abono_cortes.ToString("#,#.00") + "</b></td><td></td><td></td><td></td><td></td><td></td><td></td></tr>";
            }
            if (ver_header_sincorte > 0)
            {
                html_sincorte += "<tr><td colspan=\"3\"></td><td></td><td align=\"right\"><b>" + total_sincorte.ToString("#,#.00") + "</b></td><td></td><td></td><td></td><td align=\"right\"><b>" + total_valor_sincortes.ToString("#,#.00") + "</b></td><td align=\"right\"><b>" + total_abono_sincorte.ToString("#,#.00") + "</b></td><td></td><td></td><td></td><td></td><td></td><td></td></tr>";
            }
            resumen_cortes.Text = total_valor_cortes.ToString("#,#.00");
            resumen_sincortes.Text = total_valor_sincortes.ToString("#,#.00");
            resumen_abonos.Text = total_abonos_proveedor.ToString("#,#.00");

            total_saldo_proveedor = total_valor_cortes + total_valor_sincortes + total_abonos_proveedor;
            lbl_total_saldo_proveedor.Text = total_saldo_proveedor.ToString("#,#.00");
            //lbl_total_saldo_proveedor.Text = total_saldo_fixed.ToString("#,#.00"); //2020-09-02 //2020-11-20 hoy se comento de nuevo Ticket#2020111631000051 — Error en reporte Intercompany 

        }
    }
}