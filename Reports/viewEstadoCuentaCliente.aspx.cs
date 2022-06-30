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

public partial class Reports_viewEstadoCuentaCliente : System.Web.UI.Page
{
    UsuarioBean user = null;
    CrystalDecisions.CrystalReports.Engine.ReportDocument rpt;
    protected void Page_Load(object sender, EventArgs e)
    {
        user = (UsuarioBean)Session["usuario"];
        if (Session["usuario"] == null)
        {
            Response.Redirect("../default.aspx");
        }
        int solopendiente = int.Parse(Request.QueryString["solopendiente"].ToString());
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
                where += " and tfa_pai_id=" + user.PaisID + " ";
                //facturas anuladas
                where_1 = " and a.tfa_conta_id=" + contaID + " and a.tfa_moneda=" + monID + " and a.tfa_ted_id=3 and tfa_tpi_id=" + tpi + " ";
                if (fechaini != null && !fechaini.Equals("")) where_1 += " and a.tfa_fecha_emision>='" + fechaini + "'";
                if (fechafin != null && !fechafin.Equals("")) where_1 += " and a.tfa_fecha_emision<='" + fechafin + "'";
                where_1 += " and a.tfa_pai_id=" + user.PaisID + " ";

                //Notas Debito
                where2 = " and tnd_tcon_id=" + contaID + " and tnd_moneda=" + monID + " and tnd_tpi_id=" + tpi + " and tnd_ted_id<>3";
                if (fechaini != null && !fechaini.Equals("")) where2 += " and tnd_fecha_emision>='" + fechaini + "'";
                if (fechafin != null && !fechafin.Equals("")) where2 += " and tnd_fecha_emision<='" + fechafin + "'";
                where2 += " and tnd_pai_id=" + user.PaisID + " ";
                //Notas Credito
                // anuladas Nota Debito
                //Notas Debito
                where2_1 = " and a.tnd_tcon_id=" + contaID + " and a.tnd_moneda=" + monID + " and a.tnd_tpi_id=" + tpi + " and a.tnd_ted_id=3";
                if (fechaini != null && !fechaini.Equals("")) where2_1 += " and a.tnd_fecha_emision>='" + fechaini + "'";
                if (fechafin != null && !fechafin.Equals("")) where2_1 += " and a.tnd_fecha_emision<='" + fechafin + "'";
                where2_1 += " and a.tnd_pai_id=" + user.PaisID + " ";

                //where3 = " and tnc_tcon_id=" + contaID + " and tnc_mon_id=" + monID + " and tnc_tpi_id=" + tpi + " ";
                where3 = " and tnc_tcon_id=" + contaID + " and tnc_mon_id=" + monID + " and tnc_tpi_id=" + tpi + " and tnc_ted_id<>3 ";
                //if (solopendiente == 1) where3 += " and tnc_ted_id in (1,2)"; else where3 += " and tnc_ted_id in (1,2,4)";
                if (fechaini != null && !fechaini.Equals("")) where3 += " and tnc_fecha>='" + fechaini + "'";
                if (fechafin != null && !fechafin.Equals("")) where3 += " and tnc_fecha<='" + fechafin + "'";
                where3 += " and tnc_pai_id=" + user.PaisID + " ";
                // anuladas nc
                //where3 = " and tnc_tcon_id=" + contaID + " and tnc_mon_id=" + monID + " and tnc_tpi_id=" + tpi + " ";
                where3_1 = " and a.tnc_tcon_id=" + contaID + " and a.tnc_mon_id=" + monID + " and a.tnc_tpi_id=" + tpi + " and a.tnc_ted_id=3 ";
                //if (solopendiente == 1) where3 += " and tnc_ted_id in (1,2)"; else where3 += " and tnc_ted_id in (1,2,4)";
                if (fechaini != null && !fechaini.Equals("")) where3_1 += " and a.tnc_fecha>='" + fechaini + "'";
                if (fechafin != null && !fechafin.Equals("")) where3_1 += " and a.tnc_fecha<='" + fechafin + "'";
                where3_1 += " and a.tnc_pai_id=" + user.PaisID + " ";
                //Recibos
                //where4 = " and tre_id=" + contaID + " and tre_moneda_id=" + monID + " and tre_tpi_id=" + tpi + " ";
                //where4 = " and tre_tcon_id=" + contaID + " and tre_moneda_id=" + monID + " ";
                where4 = " and tre_tcon_id=" + contaID + " and tre_moneda_id=" + monID + " and tre_ted_id<>3 ";
                //if (solopendiente == 1) where4 += " and tre_ted_id in (0)"; else where4 += " and tre_ted_id in (0)";
                if (fechaini != null && !fechaini.Equals("")) where4 += " and tre_fecha>='" + fechaini + "'";
                if (fechafin != null && !fechafin.Equals("")) where4 += " and tre_fecha<='" + fechafin + "'";
                where4 += " and tre_pai_id=" + user.PaisID + " ";
                //Recibos anulados
                where4_1= " and a.tre_tcon_id=" + contaID + " and a.tre_moneda_id=" + monID + " and a.tre_ted_id=3 ";
                //if (solopendiente == 1) where4 += " and tre_ted_id in (0)"; else where4 += " and tre_ted_id in (0)";
                if (fechaini != null && !fechaini.Equals("")) where4_1 += " and a.tre_fecha_emision>='" + fechaini + "'";
                if (fechafin != null && !fechafin.Equals("")) where4_1 += " and a.tre_fecha_emision<='" + fechafin + "'";
                where4_1 += " and a.tre_pai_id=" + user.PaisID + " ";

                //Cheques y Transferencias
                where5 = " and tcg_tcon_id=" + contaID + " and tcg_tpi_id=" + tpi + " and tcg_ted_id=4 ";
                if (fechaini != null && !fechaini.Equals("")) where5 += " and tcg_fecha_emision>='" + fechaini + "'";
                if (fechafin != null && !fechafin.Equals("")) where5 += " and tcg_fecha_emision<='" + fechafin + "'";
                where5 += " and tcg_pai_id=" + user.PaisID + " and tcg_ttm_id=" + monID + " ";

                //Cheques y Transferencias anuladas
                where5_1 = " and a.tcg_tcon_id=" + contaID + " and a.tcg_tpi_id=" + tpi + " and a.tcg_ted_id=3 ";
                if (fechaini != null && !fechaini.Equals("")) where5_1 += " and a.tcg_fecha_emision>='" + fechaini + "'";
                if (fechafin != null && !fechafin.Equals("")) where5_1 += " and a.tcg_fecha_emision<='" + fechafin + "'";
                where5_1 += " and a.tcg_pai_id=" + user.PaisID + " and tcg_ttm_id=" + monID + " ";
                #endregion 

                if (user.PaisID == 1 && contaID == 1 && consMon == "si")
                {
                    //Facturas
                    where_cons = " and tfa_conta_id=" + contaID + " and tfa_moneda<>" + monID + " and tfa_ted_id<>3 and tfa_tpi_id=" + tpi + " ";
                    if (fechaini != null && !fechaini.Equals("")) where_cons += " and tfa_fecha_emision>='" + fechaini + "'";
                    if (fechafin != null && !fechafin.Equals("")) where_cons += " and tfa_fecha_emision<='" + fechafin + "'";
                    where_cons += " and tfa_pai_id=" + user.PaisID + " ";
                    //facturas anuladas
                    where_1_cons = " and a.tfa_conta_id=" + contaID + " and a.tfa_moneda<>" + monID + " and a.tfa_ted_id=3 and tfa_tpi_id=" + tpi + " ";
                    if (fechaini != null && !fechaini.Equals("")) where_1_cons += " and a.tfa_fecha_emision>='" + fechaini + "'";
                    if (fechafin != null && !fechafin.Equals("")) where_1_cons += " and a.tfa_fecha_emision<='" + fechafin + "'";
                    where_1_cons += " and a.tfa_pai_id=" + user.PaisID + " ";

                    //Notas Debito
                    where2_cons = " and tnd_tcon_id=" + contaID + " and tnd_moneda<>" + monID + " and tnd_tpi_id=" + tpi + " and tnd_ted_id<>3";
                    if (fechaini != null && !fechaini.Equals("")) where2_cons += " and tnd_fecha_emision>='" + fechaini + "'";
                    if (fechafin != null && !fechafin.Equals("")) where2_cons += " and tnd_fecha_emision<='" + fechafin + "'";
                    where2_cons += " and tnd_pai_id=" + user.PaisID + " ";
                    //Notas Credito
                    // anuladas Nota Debito
                    //Notas Debito
                    where2_1_cons = " and a.tnd_tcon_id=" + contaID + " and a.tnd_moneda<>" + monID + " and a.tnd_tpi_id=" + tpi + " and a.tnd_ted_id=3";
                    if (fechaini != null && !fechaini.Equals("")) where2_1_cons += " and a.tnd_fecha_emision>='" + fechaini + "'";
                    if (fechafin != null && !fechafin.Equals("")) where2_1_cons += " and a.tnd_fecha_emision<='" + fechafin + "'";
                    where2_1_cons += " and a.tnd_pai_id=" + user.PaisID + " ";


                    where3_cons = " and tnc_tcon_id=" + contaID + " and tnc_mon_id<>" + monID + " and tnc_tpi_id=" + tpi + " and tnc_ted_id<>3 ";
                    if (fechaini != null && !fechaini.Equals("")) where3_cons += " and tnc_fecha>='" + fechaini + "'";
                    if (fechafin != null && !fechafin.Equals("")) where3_cons += " and tnc_fecha<='" + fechafin + "'";
                    where3_cons += " and tnc_pai_id=" + user.PaisID + " ";
                    // anuladas nc
                    where3_1_cons = " and a.tnc_tcon_id=" + contaID + " and a.tnc_mon_id<>" + monID + " and a.tnc_tpi_id=" + tpi + " and a.tnc_ted_id=3 ";
                    if (fechaini != null && !fechaini.Equals("")) where3_1_cons += " and a.tnc_fecha>='" + fechaini + "'";
                    if (fechafin != null && !fechafin.Equals("")) where3_1_cons += " and a.tnc_fecha<='" + fechafin + "'";
                    where3_1_cons += " and a.tnc_pai_id=" + user.PaisID + " ";
                    //Recibos
                    where4_cons = " and tre_tcon_id=" + contaID + " and tre_moneda_id<>" + monID + " and tre_ted_id<>3 ";
                    if (fechaini != null && !fechaini.Equals("")) where4_cons += " and tre_fecha>='" + fechaini + "'";
                    if (fechafin != null && !fechafin.Equals("")) where4_cons += " and tre_fecha<='" + fechafin + "'";
                    where4_cons += " and tre_pai_id=" + user.PaisID + " ";
                    //Recibos anulados
                    where4_1_cons = " and a.tre_tcon_id=" + contaID + " and a.tre_moneda_id<>" + monID + " and a.tre_ted_id=3 ";
                    if (fechaini != null && !fechaini.Equals("")) where4_1_cons += " and a.tre_fecha_emision>='" + fechaini + "'";
                    if (fechafin != null && !fechafin.Equals("")) where4_1_cons += " and a.tre_fecha_emision<='" + fechafin + "'";
                    where4_1_cons += " and a.tre_pai_id=" + user.PaisID + " ";

                    //Cheques y Transferencias
                    where5_cons = " and tcg_tcon_id=" + contaID + " and tcg_tpi_id=" + tpi + " and tcg_ted_id=4 ";
                    if (fechaini != null && !fechaini.Equals("")) where5_cons += " and tcg_fecha_emision>='" + fechaini + "'";
                    if (fechafin != null && !fechafin.Equals("")) where5_cons += " and tcg_fecha_emision<='" + fechafin + "'";
                    where5_cons += " and tcg_pai_id=" + user.PaisID + " and tcg_ttm_id<>" + monID + " ";

                    //Cheques y Transferencias anuladas
                    where5_1_cons = " and a.tcg_tcon_id=" + contaID + " and a.tcg_tpi_id=" + tpi + " and a.tcg_ted_id=3 ";
                    if (fechaini != null && !fechaini.Equals("")) where5_1_cons += " and a.tcg_fecha_emision>='" + fechaini + "'";
                    if (fechafin != null && !fechafin.Equals("")) where5_1_cons += " and a.tcg_fecha_emision<='" + fechafin + "'";
                    where5_1_cons += " and a.tcg_pai_id=" + user.PaisID + " and tcg_ttm_id<>" + monID + " ";   
                }


                ds = DB.getEstadoCuentaCliente_detallado(clienteID, where, where2, where3, where4, where5, fechafin, monID, solopendiente, where4_1, where_1, where2_1, where3_1, where5_1,
                                                         where_cons, where2_cons, where3_cons, where4_cons, where5_cons, where4_1_cons, where_1_cons, where2_1_cons, where3_1_cons, where5_1_cons, consMon, user.PaisID, contaID);
                //if (solopendiente == 0) ds = DB.getEstadoCuentaCliente_detallado(clienteID, where, where2, where5, fechafin, monID, solopendiente);
                //else ds = DB.getEstadoCuentaCliente(clienteID, where, fechafin, where2, where3, where4);
            }
            else
            {
                //Cortes
                where = " and tcp_conta_id=" + contaID + " and tcp_mon_id=" + monID + " and tcp_tpi_id=" + tpi + " and tcp_ted_id<>3";
                if (fechaini != null && !fechaini.Equals("")) where += " and tcp_fecha_creacion>='" + fechaini + "'";
                if (fechafin != null && !fechafin.Equals("")) where += " and tcp_fecha_creacion<='" + fechafin + "'";
                where += " and tcp_pai_id=" + user.PaisID + " ";
                //Notas Debito
                where2 = " and tnd_tcon_id=" + contaID + " and tnd_moneda=" + monID + " and tnd_tpi_id=" + tpi + " and tnd_ted_id<>3";
                if (fechaini != null && !fechaini.Equals("")) where2 += " and tnd_fecha_emision>='" + fechaini + "'";
                if (fechafin != null && !fechafin.Equals("")) where2 += " and tnd_fecha_emision<='" + fechafin + "'";
                where2 += " and tnd_pai_id=" + user.PaisID + " ";
                //Notas Credito
                where3 = " and tnc_tcon_id=" + contaID + " and tnc_mon_id=" + monID + " and tnc_tpi_id=" + tpi + " and tnc_ted_id<>3 ";
                if (fechaini != null && !fechaini.Equals("")) where3 += " and tnc_fecha>='" + fechaini + "'";
                if (fechafin != null && !fechafin.Equals("")) where3 += " and tnc_fecha<='" + fechafin + "'";
                where3 += " and tnc_pai_id=" + user.PaisID + " ";
                //Recibos de Soas
                where4 = " and trc_tcon_id=" + contaID + " and trc_moneda_id=" + monID + " and trc_tpi_id=" + tpi + " and trc_ted_id<>3 ";
                if (fechaini != null && !fechaini.Equals("")) where4 += " and trc_fecha>='" + fechaini + "'";
                if (fechafin != null && !fechafin.Equals("")) where4 += " and trc_fecha<='" + fechafin + "'";
                where4 += " and trc_pai_id=" + user.PaisID + " ";
                //Cheques y Transferencias
                where5 = " and tcg_tcon_id=" + contaID + " and tcg_tpi_id=" + tpi + " and tcg_ted_id=4 ";
                if (fechaini != null && !fechaini.Equals("")) where5 += " and tcg_fecha_emision>='" + fechaini + "'";
                if (fechafin != null && !fechafin.Equals("")) where5 += " and tcg_fecha_emision<='" + fechafin + "'";
                where5 += " and tcg_pai_id=" + user.PaisID + " and tcg_ttm_id=" + monID + " ";
                //-provisiones sin corte
                where6 = " and to_char(tpr_fecha_acepta,'yyyy-mm-dd')>='" + fechaini + "' and to_char(tpr_fecha_acepta,'yyyy-mm-dd')<='" + fechafin + "' and tpr_mon_id=" + monID + " and tpr_tcon_id=" + contaID + " ";
                where6 += " and tpr_pai_id=" + user.PaisID + " and tpr_ted_id not in (1,3) ";
                where6 += " and tpr_prov_id not in (select b.tcpd_docto_id from tbl_corte_proveedor a join tbl_corte_proveedor_detalle b  on b.tcpd_tcp_id = a.tcp_id and b.tcpd_docto_id = tpr_prov_id and a.tcp_mon_id =" + monID + "  ";
                where6 += " and a.tcp_conta_id =" + contaID + " and tcp_pai_id =" + user.PaisID + " and tcp_cli_id =" + clienteID + " and a.tcp_fecha_creacion>= '" + fechaini + "' and a.tcp_fecha_creacion<= '" + fechafin + "' ";
                where6 += " and a.tcp_tpi_id =" + tpi + " and a.tcp_ted_id <>3  )  ";
                where6 += " and tpr_tpi_id=" + tpi + " group by tpr_prov_id, tpr_correlativo, ";
                where6 += " tpr_serie, tpr_fecha_acepta,tpr_contenedor,tpr_hbl, tpr_valor,poliza, usuario having (tpr_valor-coalesce((select sum(abono) from (  ";
                where6 += "  select sum(b.tcg_valor) as abono    from tbl_cheques_generados b, tbl_cheque_corte c, tbl_corte_proveedor_detalle d   ";
                where6 += " where b.tcg_id=c.corte_tcg_id and c.corte_tcp_id=d.tcpd_tcp_id and d.tcpd_docto_id=tpr_prov_id    ";
                where6 += " and to_char(b.tcg_fecha_emision,'yyyy-mm-dd')<='" + fechafin + "' and b.tcg_ted_id<>3    and d.tcpd_str_id in (5,10)   union all ";
                where6 += " select sum(d.trp_monto) as abono    from tbl_retencion_provision d    where d.trp_tpr_prov_id=tpr_prov_id    ";
                where6 += " and to_char(d.trp_fecha_generacion,'yyyy-mm-dd')<='" + fechafin + "' and d.trp_ted_id<>3 ) abonos),0))>0 and tpr_prov_id not in (       ";
                where6 += " select tpr_prov_id from aux_prov_id where tpr_proveedor_id=" + clienteID + " and tpr_tpi_id=" + tpi + " and tpr_pai_id=" + user.PaisID + " and tpr_mon_id=" + monID + " and ";
                where6 += " tpr_tcon_id=" + contaID + " and  tcg_fecha_emision<='" + fechafin + "'   ) ";
                //provisioens sin corte anuladas
                //-provisiones sin corte
                where6_1 = " and to_char(tpr_fecha_acepta,'yyyy-mm-dd')>='" + fechaini + "' and to_char(tpr_fecha_acepta,'yyyy-mm-dd')<='" + fechafin + "' and tpr_mon_id=" + monID + " and tpr_tcon_id=" + contaID + " ";
                where6_1 += " and tpr_pai_id=" + user.PaisID + " and tpr_ted_id =3 ";
                where6_1 += " and tpr_prov_id not in (select b.tcpd_docto_id from tbl_corte_proveedor a join tbl_corte_proveedor_detalle b  on b.tcpd_tcp_id = a.tcp_id and b.tcpd_docto_id = tpr_prov_id and a.tcp_mon_id =" + monID + "  ";
                where6_1 += " and a.tcp_conta_id =" + contaID + " and tcp_pai_id =" + user.PaisID + " and tcp_cli_id =" + clienteID + " and a.tcp_fecha_creacion>= '" + fechaini + "' and a.tcp_fecha_creacion<= '" + fechafin + "' ";
                where6_1 += " and a.tcp_tpi_id =" + tpi + " and a.tcp_ted_id <>3  )  ";
                where6_1 += " and tpr_tpi_id=" + tpi + " group by tpr_prov_id, tpr_correlativo, ";
                where6_1 += " tpr_serie, tpr_fecha_acepta,tpr_contenedor,tpr_hbl, tpr_valor,poliza, usuario having (tpr_valor-coalesce((select sum(abono) from (  ";
                where6_1 += "  select sum(b.tcg_valor) as abono    from tbl_cheques_generados b, tbl_cheque_corte c, tbl_corte_proveedor_detalle d   ";
                where6_1 += " where b.tcg_id=c.corte_tcg_id and c.corte_tcp_id=d.tcpd_tcp_id and d.tcpd_docto_id=tpr_prov_id    ";
                where6_1 += " and to_char(b.tcg_fecha_emision,'yyyy-mm-dd')<='" + fechafin + "' and b.tcg_ted_id<>3    and d.tcpd_str_id in (5,10)   union all ";
                where6_1 += " select sum(d.trp_monto) as abono    from tbl_retencion_provision d    where d.trp_tpr_prov_id=tpr_prov_id    ";
                where6_1 += " and to_char(d.trp_fecha_generacion,'yyyy-mm-dd')<='" + fechafin + "' and d.trp_ted_id<>3 ) abonos),0))>0 and tpr_prov_id not in (       ";
                where6_1 += " select tpr_prov_id from aux_prov_id where tpr_proveedor_id=" + clienteID + " and tpr_tpi_id=" + tpi + " and tpr_pai_id=" + user.PaisID + " and tpr_mon_id=" + monID + " and ";
                where6_1 += " tpr_tcon_id=" + contaID + " and  tcg_fecha_emision<='" + fechafin + "'   ) ";

                //notas debito
                where7 = "  and  to_char(a.tnd_fecha_emision,'yyyy-mm-dd')>='" + fechaini + "' and  to_char(a.tnd_fecha_emision,'yyyy-mm-dd')<='" + fechafin + "' and a.tnd_pai_id=" + user.PaisID + "  and a.tnd_moneda=" + monID + " and a.tnd_tcon_id=" + contaID + " and a.tnd_ted_id<>3 and a.tnd_id not in  ";
                where7 += " (select b.tcpd_docto_id from tbl_corte_proveedor a join tbl_corte_proveedor_detalle b  on b.tcpd_tcp_id = a.tcp_id and b.tcpd_docto_id = tnd_id and a.tcp_mon_id =" + monID + " and b.tcpd_str_id=4   ";
                where7 += " and a.tcp_conta_id =" + contaID + " and tcp_pai_id =" + user.PaisID + " and tcp_cli_id =" + clienteID + " and a.tcp_fecha_creacion>= '" + fechaini + "' and a.tcp_fecha_creacion<= '" + fechafin + "'  and a.tcp_tpi_id =" + tpi + " and a.tcp_ted_id <>3  )  ";
                where7 += "  and a.tnd_tpi_id=" + tpi + " and tnd_id not in   ( select tnd_id from aux_nd_id where   tnd_cli_id=" + clienteID + " and   tnd_tpi_id=" + tpi + " and tnd_pai_id=" + user.PaisID + " and tnd_moneda=" + monID + " and tnd_tcon_id=" + contaID + "  and fecha<='" + fechafin + "'    )   group by serie,correlavito,fecha2,tnd_contenedor,hbl,poliza, usuario ";

                //anuladas notas debito

                where7_1 = "  and  to_char(a.tnd_fecha_emision,'yyyy-mm-dd')>='" + fechaini + "' and  to_char(a.tnd_fecha_emision,'yyyy-mm-dd')<='" + fechafin + "' and a.tnd_pai_id=" + user.PaisID + "  and a.tnd_moneda=" + monID + " and a.tnd_tcon_id=" + contaID + " and a.tnd_ted_id=3 and a.tnd_id not in  ";
                where7_1 += " (select b.tcpd_docto_id from tbl_corte_proveedor a join tbl_corte_proveedor_detalle b  on b.tcpd_tcp_id = a.tcp_id and b.tcpd_docto_id = tnd_id and a.tcp_mon_id =" + monID + " and b.tcpd_str_id=4   ";
                where7_1 += " and a.tcp_conta_id =" + contaID + " and tcp_pai_id =" + user.PaisID + " and tcp_cli_id =" + clienteID + " and a.tcp_fecha_creacion>= '" + fechaini + "' and a.tcp_fecha_creacion<= '" + fechafin + "'  and a.tcp_tpi_id =" + tpi + " and a.tcp_ted_id <>3  )  ";
                where7_1 += "  and a.tnd_tpi_id=" + tpi + " and tnd_id not in   ( select tnd_id from aux_nd_id where   tnd_cli_id=" + clienteID + " and   tnd_tpi_id=" + tpi + " and tnd_pai_id=" + user.PaisID + " and tnd_moneda=" + monID + " and tnd_tcon_id=" + contaID + "  and fecha<='" + fechafin + "'    )   group by serie,correlavito,fecha2,tnd_contenedor,hbl,poliza,usuario ";

                where7_2 = "  and  to_char(tfr_fecha_abono,'yyyy-mm-dd')>'" + fechafin + "'  and tnd_pai_id=" + user.PaisID + "  and tnd_moneda=" + monID + " and tnd_tcon_id=" + contaID + " and tnd_ted_id=3 and tnd_fecha_emision <='" + fechafin + "'  and tnd_id not in  ";
                where7_2 += " (select b.tcpd_docto_id from tbl_corte_proveedor a join tbl_corte_proveedor_detalle b  on b.tcpd_tcp_id = a.tcp_id and b.tcpd_docto_id = tnd_id and a.tcp_mon_id =" + monID + " and b.tcpd_str_id=4   ";
                where7_2 += " and a.tcp_conta_id =" + contaID + " and tcp_pai_id =" + user.PaisID + " and tcp_cli_id =" + clienteID + " and a.tcp_fecha_creacion>= '" + fechaini + "' and a.tcp_fecha_creacion<= '" + fechafin + "'  and a.tcp_tpi_id =" + tpi + " and a.tcp_ted_id <>3  )  ";
                where7_2 += "  and tnd_tpi_id=" + tpi + " and tnd_id not in   ( select tnd_id from aux_nd_id where   tnd_cli_id=" + clienteID + " and   tnd_tpi_id=" + tpi + " and tnd_pai_id=" + user.PaisID + " and tnd_moneda=" + monID + " and tnd_tcon_id=" + contaID + "  and fecha<='" + fechafin + "'    )    ";

                //notas credito
                where8 = "  and  to_char(a.tnc_fecha_emision,'yyyy-mm-dd')>='" + fechaini + "' and  to_char(a.tnc_fecha_emision,'yyyy-mm-dd')<='" + fechafin + "' and a.tnc_pai_id=" + user.PaisID + "  and a.tnc_mon_id=" + monID + " and a.tnc_tcon_id=" + contaID + " and a.tnc_ted_id<>3 and a.tnc_id not in  ";
                where8 += " (select b.tcpd_docto_id from tbl_corte_proveedor a join tbl_corte_proveedor_detalle b  on b.tcpd_tcp_id = a.tcp_id and b.tcpd_docto_id = tnc_id and a.tcp_mon_id =" + monID + " and b.tcpd_str_id in (12,31)   ";
                where8 += " and a.tcp_conta_id =" + contaID + " and tcp_pai_id =" + user.PaisID + " and tcp_cli_id =" + clienteID + " and a.tcp_fecha_creacion>= '" + fechaini + "' and a.tcp_fecha_creacion<= '" + fechafin + "'  and a.tcp_tpi_id =" + tpi + " and a.tcp_ted_id <>3  )  ";
                where8 += "  and a.tnc_tpi_id=" + tpi + " and tnc_id not in   ( select tnc_id from aux_nc_id where   tnc_cli_id=" + clienteID + " and   tnc_tpi_id=" + tpi + " and tnc_pai_id=" + user.PaisID + " and tnc_mon_id=" + monID + " and tnc_tcon_id=" + contaID + "  and fecha<='" + fechafin + "'    )   group by serie,correlavito,fecha2,tnc_contenedor,hbl,poliza,usuario ";

                //nc directa
                where8_2 = "  and  to_char(a.tnc_fecha_emision,'yyyy-mm-dd')>='" + fechaini + "' and  to_char(a.tnc_fecha_emision,'yyyy-mm-dd')<='" + fechafin + "' and a.tnc_pai_id=" + user.PaisID + "  and a.tnc_mon_id=" + monID + " and a.tnc_tcon_id=" + contaID + " and a.tnc_ted_id<>3 and a.tnc_id not in  ";
                where8_2 += " (select b.tcpd_docto_id from tbl_corte_proveedor a join tbl_corte_proveedor_detalle b  on b.tcpd_tcp_id = a.tcp_id and b.tcpd_docto_id = tnc_id and a.tcp_mon_id =" + monID + " and b.tcpd_str_id=12   ";
                where8_2 += " and a.tcp_conta_id =" + contaID + " and tcp_pai_id =" + user.PaisID + " and tcp_cli_id =" + clienteID + " and a.tcp_fecha_creacion>= '" + fechaini + "' and a.tcp_fecha_creacion<= '" + fechafin + "'  and a.tcp_tpi_id =" + tpi + " and a.tcp_ted_id <>3  )  ";
                where8_2 += "  and a.tnc_tpi_id=" + tpi + " and tnc_id not in   ( select tnc_id from aux_nc_id where   tnc_cli_id=" + clienteID + " and   tnc_tpi_id=" + tpi + " and tnc_pai_id=" + user.PaisID + " and tnc_mon_id=" + monID + " and tnc_tcon_id=" + contaID + "  and fecha<='" + fechafin + "'    )   group by a.tnc_id ";

                //anuladas notas credito pv
                where8_1 = "  and  to_char(a.tnc_fecha_emision,'yyyy-mm-dd')>='" + fechaini + "' and  to_char(a.tnc_fecha_emision,'yyyy-mm-dd')<='" + fechafin + "' and a.tnc_pai_id=" + user.PaisID + "  and a.tnc_mon_id=" + monID + " and a.tnc_tcon_id=" + contaID + " and a.tnc_ted_id=3 and a.tnc_id not in  ";
                where8_1 += " (select b.tcpd_docto_id from tbl_corte_proveedor a join tbl_corte_proveedor_detalle b  on b.tcpd_tcp_id = a.tcp_id and b.tcpd_docto_id = tnc_id and a.tcp_mon_id =" + monID + " and b.tcpd_str_id in (12,31)   ";
                where8_1 += " and a.tcp_conta_id =" + contaID + " and tcp_pai_id =" + user.PaisID + " and tcp_cli_id =" + clienteID + " and a.tcp_fecha_creacion>= '" + fechaini + "' and a.tcp_fecha_creacion<= '" + fechafin + "'  and a.tcp_tpi_id =" + tpi + " and a.tcp_ted_id <>3  )  ";
                where8_1 += "  and a.tnc_tpi_id=" + tpi + " and tnc_id not in   ( select tnc_id from aux_nc_id where   tnc_cli_id=" + clienteID + " and   tnc_tpi_id=" + tpi + " and tnc_pai_id=" + user.PaisID + " and tnc_mon_id=" + monID + " and tnc_tcon_id=" + contaID + "  and fecha<='" + fechafin + "'    )   group by serie,correlavito,fecha2,tnc_contenedor,hbl,poliza,usuario,a.tnc_id ";

                //anuladas cheques
                where5_1 = " and    a.tcg_ted_id =3  and a.tcg_tcon_id =" + contaID + " and a.tcg_pai_id =" + user.PaisID + " and a.tcg_ttm_id =" + monID + "  and a.tcg_tpi_id=" + tpi + "  and a.tcg_fecha_emision >='" + fechaini + "'  and  a.tcg_fecha_emision <='" + fechafin + "'  group by tcg_cuenta,tcg_numero ,tcg_fecha_emision,tcg_valor,poliza,usuario ";
                //recibos soas anulados
                where4_1 = "  and a.trc_ted_id = 3  and a.trc_tcon_id =" + contaID + " and a.trc_pai_id =" + user.PaisID + " and  a.trc_moneda_id =" + monID + "  and a.trc_tpi_id=" + tpi + " and a.trc_fecha_emision>='" + fechaini + "' and a.trc_fecha_emision<='" + fechafin + "'   group by trc_serie,trc_correlativo,trc_fecha_emision,trc_contenedor,trc_hbl,poliza,usuario ";
                //recibos a favor proveedor
                where4_2 = "  and a.trc_ted_id <> 3  and a.trc_tcon_id =" + contaID + " and a.trc_pai_id =" + user.PaisID + " and  a.trc_moneda_id =" + monID + "  and a.trc_tpi_id=" + tpi + " and a.trc_fecha_emision>='" + fechaini + "' and a.trc_fecha_emision<='" + fechafin + "'   group by trc_serie,trc_correlativo,trc_fecha_emision,trc_contenedor,trc_hbl,poliza,usuario ";
                where4_2 += " having  coalesce(sum(cast(trc_monto as numeric) - coalesce((select sum(tcr_abono) from (select sum(tcr_abono) as tcr_abono  from tbl_corte_abono where tcr_tre_id=trc_id and tcr_sysref_id = 22 and to_char(tcr_fecha_abono,'yyyy-mm-dd')<='"+ fechafin +" 23:59:59')abono),0)))  > 0 ";
                //Cheques liquidaciones
                where5_2 = " and tcg_tcon_id=" + contaID + " and tcg_tpi_id=" + tpi + " and tcg_ted_id<>3 ";
                if (fechaini != null && !fechaini.Equals("")) where5_2 += " and tcg_fecha_emision>='" + fechaini + "'";
                if (fechafin != null && !fechafin.Equals("")) where5_2 += " and tcg_fecha_emision<='" + fechafin + "'";
                where5_2 += " and tcg_pai_id=" + user.PaisID + " and tcg_ttm_id=" + monID + " and tcg_no_transferencia is null ";
                where5_3 = " and tcg_tcon_id=" + contaID + " and tcg_tpi_id=" + tpi + " and tcg_ted_id=14 ";
                if (fechaini != null && !fechaini.Equals("")) where5_3 += " and tcg_fecha_emision>='" + fechaini + "'";
                if (fechafin != null && !fechafin.Equals("")) where5_3 += " and tcg_fecha_emision<='" + fechafin + "'";
                where5_3 += " and tcg_pai_id=" + user.PaisID + " and tcg_ttm_id=" + monID + " and tcg_no_transferencia is null ";
                //transferencia liquidaciones
                where5_4 = " and tcg_tcon_id=" + contaID + " and tcg_tpi_id=" + tpi + " and tcg_ted_id<>3 ";
                if (fechaini != null && !fechaini.Equals("")) where5_4 += " and tcg_fecha_emision>='" + fechaini + "'";
                if (fechafin != null && !fechafin.Equals("")) where5_4 += " and tcg_fecha_emision<='" + fechafin + "'";
                where5_4 += " and tcg_pai_id=" + user.PaisID + " and tcg_ttm_id=" + monID + " and tcg_no_transferencia is not null ";
                where5_5 = " and tcg_tcon_id=" + contaID + " and tcg_tpi_id=" + tpi + " and tcg_ted_id=14 ";
                if (fechaini != null && !fechaini.Equals("")) where5_5 += " and tcg_fecha_emision>='" + fechaini + "'";
                if (fechafin != null && !fechafin.Equals("")) where5_5 += " and tcg_fecha_emision<='" + fechafin + "'";
                where5_5 += " and tcg_pai_id=" + user.PaisID + " and tcg_ttm_id=" + monID + " and tcg_no_transferencia is not null ";
                //depositos liquidaciones
                where5_6 = " and c.tli_ted_id <>3    and tli_conta_id="+ contaID +" and tli_tpi_id="+ tpi +" and tmb_ted_id<>3   and tli_pai_id="+ user.PaisID +" and tmb_mon_id="+ monID +" ";
                if (fechaini != null && !fechaini.Equals("")) where5_6 += " and tmb_fecha>='" + fechaini + "'";
                if (fechafin != null && !fechafin.Equals("")) where5_6 += " and tmb_fecha<='" + fechafin + "'";


                //Facturas intercompnay
                where9 = "  and  to_char(a.tfa_fecha_emision,'yyyy-mm-dd')>='" + fechaini + "' and  to_char(a.tfa_fecha_emision,'yyyy-mm-dd')<='" + fechafin + "' and a.tfa_pai_id=" + user.PaisID + "  and a.tfa_moneda=" + monID + " and a.tfa_conta_id=" + contaID + " and a.tfa_ted_id<>3 and a.tfa_id not in  ";
                where9 += " (select b.tcpd_docto_id from tbl_corte_proveedor a join tbl_corte_proveedor_detalle b  on b.tcpd_tcp_id = a.tcp_id and b.tcpd_docto_id = tfa_id and a.tcp_mon_id =" + monID + " and b.tcpd_str_id=1   ";
                where9 += " and a.tcp_conta_id =" + contaID + " and tcp_pai_id =" + user.PaisID + " and tcp_cli_id =" + clienteID + " and a.tcp_fecha_creacion>= '" + fechaini + "' and a.tcp_fecha_creacion<= '" + fechafin + "'  and a.tcp_tpi_id =" + tpi + " and a.tcp_ted_id <>3  )  ";
                where9 += "  and a.tfa_tpi_id=" + tpi + " and tfa_id not in   ( select tfa_id from aux_fa_id where   tfa_cli_id=" + clienteID + " and   tfa_tpi_id=" + tpi + " and tfa_pai_id=" + user.PaisID + " and tfa_moneda=" + monID + " and tfa_conta_id=" + contaID + "  and fecha<='" + fechafin + "'    )   group by serie,correlavito,fecha2,tfa_contenedor,hbl,poliza, usuario ";

                //anuladas facturas intercompany

                where9_1 = "  and  to_char(a.tfa_fecha_emision,'yyyy-mm-dd')>='" + fechaini + "' and  to_char(a.tfa_fecha_emision,'yyyy-mm-dd')<='" + fechafin + "' and a.tfa_pai_id=" + user.PaisID + "  and a.tfa_moneda=" + monID + " and a.tfa_conta_id=" + contaID + " and a.tfa_ted_id=3 and a.tfa_id not in  ";
                where9_1 += " (select b.tcpd_docto_id from tbl_corte_proveedor a join tbl_corte_proveedor_detalle b  on b.tcpd_tcp_id = a.tcp_id and b.tcpd_docto_id = tfa_id and a.tcp_mon_id =" + monID + " and b.tcpd_str_id=1   ";
                where9_1 += " and a.tcp_conta_id =" + contaID + " and tcp_pai_id =" + user.PaisID + " and tcp_cli_id =" + clienteID + " and a.tcp_fecha_creacion>= '" + fechaini + "' and a.tcp_fecha_creacion<= '" + fechafin + "'  and a.tcp_tpi_id =" + tpi + " and a.tcp_ted_id <>3  )  ";
                where9_1 += "  and a.tfa_tpi_id=" + tpi + " and tfa_id not in   ( select tfa_id from aux_fa_id where   tfa_cli_id=" + clienteID + " and   tfa_tpi_id=" + tpi + " and tfa_pai_id=" + user.PaisID + " and tfa_moneda=" + monID + " and tfa_conta_id=" + contaID + "  and fecha<='" + fechafin + "'    )   group by serie,correlavito,fecha2,tfa_contenedor,hbl,poliza,usuario ";

                where9_2 = "  and  to_char(tfr_fecha_abono,'yyyy-mm-dd')>'" + fechafin + "'  and tfa_pai_id=" + user.PaisID + "  and tfa_moneda=" + monID + " and tfa_conta_id=" + contaID + " and tfa_ted_id=3 and tfa_fecha_emision <='" + fechafin + "'  and tfa_id not in  ";
                where9_2 += " (select b.tcpd_docto_id from tbl_corte_proveedor a join tbl_corte_proveedor_detalle b  on b.tcpd_tcp_id = a.tcp_id and b.tcpd_docto_id = tfa_id and a.tcp_mon_id =" + monID + " and b.tcpd_str_id=1   ";
                where9_2 += " and a.tcp_conta_id =" + contaID + " and tcp_pai_id =" + user.PaisID + " and tcp_cli_id =" + clienteID + " and a.tcp_fecha_creacion>= '" + fechaini + "' and a.tcp_fecha_creacion<= '" + fechafin + "'  and a.tcp_tpi_id =" + tpi + " and a.tcp_ted_id <>3  )  ";
                where9_2 += "  and tfa_tpi_id=" + tpi + " and tfa_id not in   ( select tfa_id from aux_fa_id where   tfa_cli_id=" + clienteID + " and   tfa_tpi_id=" + tpi + " and tfa_pai_id=" + user.PaisID + " and tfa_moneda=" + monID + " and tfa_conta_id=" + contaID + "  and fecha<='" + fechafin + "'    )    ";

                //anuladas notas credito pv
                where10_1 = "  and  to_char(a.tnc_fecha_emision,'yyyy-mm-dd')>='" + fechaini + "' and  to_char(a.tnc_fecha_emision,'yyyy-mm-dd')<='" + fechafin + "' and a.tnc_pai_id=" + user.PaisID + "  and a.tnc_mon_id=" + monID + " and a.tnc_tcon_id=" + contaID + " and a.tnc_ted_id=3 and a.tnc_id not in  ";
                where10_1 += " (select b.tcpd_docto_id from tbl_corte_proveedor a join tbl_corte_proveedor_detalle b  on b.tcpd_tcp_id = a.tcp_id and b.tcpd_docto_id = tnc_id and a.tcp_mon_id =" + monID + " and b.tcpd_str_id in (3)   ";
                where10_1 += " and a.tcp_conta_id =" + contaID + " and tcp_pai_id =" + user.PaisID + " and tcp_cli_id =" + clienteID + " and a.tcp_fecha_creacion>= '" + fechaini + "' and a.tcp_fecha_creacion<= '" + fechafin + "'  and a.tcp_tpi_id =" + tpi + " and a.tcp_ted_id <>3  )  ";
                where10_1 += "  and a.tnc_tpi_id=" + tpi + " and tnc_id not in   ( select tnc_id from aux_nc_id where   tnc_cli_id=" + clienteID + " and   tnc_tpi_id=" + tpi + " and tnc_pai_id=" + user.PaisID + " and tnc_mon_id=" + monID + " and tnc_tcon_id=" + contaID + "  and fecha<='" + fechafin + "'    )  and tnc_ttr_id=3   group by serie,correlavito,fecha2,tnc_contenedor,hbl,poliza,usuario,a.tnc_id ";



                ds = DB.getEstadoCuentaProveedor(clienteID, where, where2, where3, where4, where5, fechafin, monID, solopendiente, tpi, where6, where7, where6_1, where7_1, where8, where8_1, where5_1, where4_1, where4_2, where5_2, where5_3, where5_4, where5_5, where5_6, where8_2, where7_2, where9, where9_1, where9_2, where10_1, user.PaisID);

            }
            ViewState["estadocuenta"] = ds;
            rpt = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
            if (tpi == 3)
            {
                rpt.Load(Server.MapPath("~/CR_EstadoCuentaCliente_detallado.rpt"));
                //RE_GenericBean datos_cliente = DB.getDataClient(clienteID);
                //clienteNombre = datos_cliente.strC2;

                string sql_clientes = "";
                sql_clientes = " a.id_cliente=" + clienteID.ToString();
                ArrayList Arr_Cliente = (ArrayList)DB.getClientes(sql_clientes, user, "REPORTES");
                foreach (RE_GenericBean Bean_Cliente in Arr_Cliente)
                {
                    clienteNombre = Bean_Cliente.strC1;
                }
            }
            else
            {
                rpt.Load(Server.MapPath("~/CR_EstadoCuentaProveedor.rpt"));
            }
            rpt.SetDataSource(ds.Tables["estadocuenta_detallado_tbl"]);
            
            rpt.SetParameterValue("clienteID", clienteID);
            rpt.SetParameterValue("clienteNombre", clienteNombre);
            rpt.SetParameterValue("tipoPersona", tipoPersona);
            rpt.SetParameterValue("simbolomoneda", simbolomoneda);
            rpt.SetParameterValue("inicio", fechaini.Substring(0,10) );
            rpt.SetParameterValue("fin", fechafin.Substring(0, 10));
            rpt.SetParameterValue("empresa", Server.MapPath(user.pais.Imagepath));
            //CrystalReportViewer1.DisplayGroupTree = false;
            CrystalReportViewer1.ReportSource = rpt;
            CrystalReportViewer1.DataBind();
        }
        else
        {
            LibroDiarioDS ds = (LibroDiarioDS)ViewState["estadocuenta"];
            rpt = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
            if (tpi == 3)
            {
                rpt.Load(Server.MapPath("~/CR_EstadoCuentaCliente_detallado.rpt"));
            }
            else
            {
                rpt.Load(Server.MapPath("~/CR_EstadoCuentaProveedor.rpt"));
            }
            rpt.SetDataSource(ds.Tables["estadocuenta_detallado_tbl"]);
            
            rpt.SetParameterValue("clienteID", clienteID);
            rpt.SetParameterValue("clienteNombre", clienteNombre);
            rpt.SetParameterValue("tipoPersona", tipoPersona);
            rpt.SetParameterValue("simbolomoneda", simbolomoneda);
            rpt.SetParameterValue("empresa", Server.MapPath(user.pais.Imagepath));
            //CrystalReportViewer1.DisplayGroupTree = false;
            CrystalReportViewer1.ReportSource = rpt;
            CrystalReportViewer1.DataBind();
        }
    }
    private void Page_Unload(object sender, EventArgs e)
    {
        #region Clear Report Objects
        if (rpt != null)
        {
            rpt.Close();
            rpt.Dispose();
            GC.Collect();
        }
        #endregion
    }
}
