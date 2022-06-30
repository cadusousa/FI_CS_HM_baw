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

public partial class Reports_Libro_Compras : System.Web.UI.Page
{
    UsuarioBean user = null;
    CrystalDecisions.CrystalReports.Engine.ReportDocument rpt;
    char[] delimit = new char[] { ',' };
    int contaID = 0;
    int monedaID = 0;
    string consolidar = "";
    string Documentos = "";
    string Serie = "";
    string fecha_inicial = "";
    string fecha_final = "";
    string Join_Facturacion = "";
    string Join_Facturacion2 = "";
    string Join_NC = "";
    string Join_NC2 = "";
    string Join_NCA = "";
    string Join_NC2A = "";
    string Join_ND = "";
    string Join_ND2 = "";
    string Join_NDP = "";
    string Join_NDP2 = "";
    int grupoSAT = 0;
    string Tipo_Doc = "";
    string ventas_no_sujetas = "Excento";
    int tipo = 0;
    decimal serv_no_afecto = 0;
    decimal serv_afecto = 0;
    decimal impuesto = 0;
    decimal no_sujeto = 0;
    int docs = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["usuario"] == null)
        {
            Response.Redirect("../default.aspx");
        }
        user = (UsuarioBean)Session["usuario"];
        contaID = int.Parse(Request.QueryString["t_con"].ToString());
        monedaID =int.Parse(Request.QueryString["t_mon"].ToString());
        Documentos = Request.QueryString["docts"].ToString();
        Serie = Request.QueryString["serie"].ToString();
        fecha_inicial = Request.QueryString["f_inicial"].ToString();
        fecha_final = Request.QueryString["f_final"].ToString();
        grupoSAT = int.Parse(Request.QueryString["grupo"].ToString());
        tipo = int.Parse(Request.QueryString["tipo"].ToString());
        consolidar = Request.QueryString["consolidar"].ToString();
        ArrayList Arr_Fechas = (ArrayList)DB.Formatear_Fechas(fecha_inicial, fecha_final);
        fecha_inicial = Arr_Fechas[0].ToString();
        fecha_final = Arr_Fechas[1].ToString();
        foreach (string Tipo in Documentos.Split(delimit))
        {
            if (Tipo != "")
            {
                if (int.Parse(Tipo) == 1)//Facturas
                {
                    if (contaID == 3)
                    {
                        Join_Facturacion = "tfa_pai_id=" + user.PaisID + " " +
                        "and tfa_fecha_emision >= '" + fecha_inicial + " 00:00:00' and tfa_fecha_emision <='" + fecha_final + " 23:59:59' ";
                        Join_Facturacion2 = " and " + Join_Facturacion;
                    }
                    else if (consolidar == "si")
                    {
                        Join_Facturacion = "tfa_pai_id=" + user.PaisID + " and tfa_conta_id=" + contaID + " " +
                        "and tfa_fecha_emision >= '" + fecha_inicial + " 00:00:00' and tfa_fecha_emision <='" + fecha_final + " 23:59:59' ";
                        Join_Facturacion2 = " and " + Join_Facturacion;
                    }
                    else
                    {
                        Join_Facturacion = "tfa_pai_id=" + user.PaisID + " and tfa_conta_id=" + contaID + " and tfa_moneda=" + monedaID.ToString()
                        + " and tfa_fecha_emision >= '" + fecha_inicial + " 00:00:00' and tfa_fecha_emision <='" + fecha_final + " 23:59:59' ";
                        Join_Facturacion2 = " and " + Join_Facturacion;
                    }
                    if (Serie != "")
                    {
                        Join_Facturacion+= "and tfa_serie='"+Serie+"'";
                    }
                    if (grupoSAT >0)
                    {
                        Join_Facturacion += " and tfa_suc_id in (select tes_suc_id from tbl_empresa_sucursal, tbl_empresa where tes_tem_id=tem_id and tem_grupo_sat='" + grupoSAT + "')";
                    }
                }
                if (int.Parse(Tipo) == 3)//Notas de Credito
                {
                    if (contaID == 3)
                    {
                        Join_NC = " tnc_pai_id=" + user.PaisID + " and tnc_tpi_id in (3,10) and tnc_ttr_id=3 " +
                        " and tnc_fecha>='" + fecha_inicial + "' and tnc_fecha<='" + fecha_final + "'";
                        Join_NC2 = " and tpr_pai_id=" + user.PaisID + " ";
                    }
                    else if (consolidar == "si")
                    {
                        Join_NC = " tnc_pai_id=" + user.PaisID + " and tnc_tcon_id=" + contaID + " and tnc_tpi_id in (3,10) and tnc_ttr_id=3 " +
                        " and tnc_fecha>='" + fecha_inicial + "' and tnc_fecha<='" + fecha_final + "'";
                        Join_NC2 = " and tpr_pai_id=" + user.PaisID + " and tpr_tcon_id=" + contaID + " ";
                    }
                    else
                    {
                        Join_NC = " tnc_pai_id=" + user.PaisID + " and tnc_tcon_id=" + contaID + " and tnc_tpi_id in (3,10) and tnc_ttr_id=3 and tnc_mon_id=" + monedaID.ToString()
                        + " and tnc_fecha>='" + fecha_inicial + "' and tnc_fecha<='" + fecha_final + "'";
                        Join_NC2 = " and tpr_pai_id=" + user.PaisID + " and tpr_tcon_id=" + contaID + " and tpr_mon_id =" + monedaID.ToString();
                    }
                    if (Serie != "")
                    {
                        Join_NC += " and tnc_serie='" + Serie + "' ";
                    }
                    if (grupoSAT > 0)
                    {
                        Join_NC += " and tnc_suc_id in (select tes_suc_id from tbl_empresa_sucursal, tbl_empresa where tes_tem_id=tem_id and tem_grupo_sat='" + grupoSAT + "')";
                    }
                }
                if (int.Parse(Tipo) == 4)//Notas de Debito
                {
                    if (contaID == 3)
                    {
                        Join_ND = " tnd_pai_id=" + user.PaisID + " and tnd_tpi_id in (3,10) " +
                        " and tnd_fecha_emision>='" + fecha_inicial + " 00:00:00' and tnd_fecha_emision<='" + fecha_final + " 23:59:59'";
                        Join_ND2 = " and tpr_pai_id=" + user.PaisID + " ";
                    }
                    else if (consolidar == "si")
                    {
                        Join_ND = " tnd_pai_id=" + user.PaisID + " and tnd_tcon_id=" + contaID + " and tnd_tpi_id in (3,10) " +
                        " and tnd_fecha_emision>='" + fecha_inicial + " 00:00:00' and tnd_fecha_emision<='" + fecha_final + " 23:59:59'";
                        Join_ND2 = " and tpr_pai_id=" + user.PaisID + " and tpr_tcon_id=" + contaID + " ";
                    }
                    else
                    {
                        Join_ND = " tnd_pai_id=" + user.PaisID + " and tnd_tcon_id=" + contaID + " and tnd_tpi_id in (3,10) and tnd_moneda=" + monedaID.ToString()
                        + " and tnd_fecha_emision>='" + fecha_inicial + " 00:00:00' and tnd_fecha_emision<='" + fecha_final + " 23:59:59'";
                        Join_ND2 = " and tpr_pai_id=" + user.PaisID + " and tpr_tcon_id=" + contaID + " and tpr_mon_id =" + monedaID.ToString();
                    }
                    if (Serie != "")
                    {
                        Join_ND += " and tnd_serie='" + Serie + "' ";
                    }
                    if (grupoSAT > 0)
                    {
                        Join_ND += " and tnd_suc_id in (select tes_suc_id from tbl_empresa_sucursal, tbl_empresa where tes_tem_id=tem_id and tem_grupo_sat='" + grupoSAT + "')";
                    }
                }
                if (int.Parse(Tipo) == 44)//Notas de Debito Proveedor
                {
                    if (contaID == 3)
                    {
                        Join_NDP = " tnd_pai_id=" + user.PaisID + " and tnd_tpi_id<>3 " +
                        " and tnd_fecha_emision>='" + fecha_inicial + " 00:00:00' and tnd_fecha_emision<='" + fecha_final + " 23:59:59'";
                        Join_NDP2 = " and tpr_pai_id=" + user.PaisID + " ";
                    }
                    else if (consolidar == "si")
                    {
                        Join_NDP = " tnd_pai_id=" + user.PaisID + " and tnd_tcon_id=" + contaID + " and tnd_tpi_id<>3 " +
                        " and tnd_fecha_emision>='" + fecha_inicial + " 00:00:00' and tnd_fecha_emision<='" + fecha_final + " 23:59:59'";
                        Join_NDP2 = " and tpr_pai_id=" + user.PaisID + " and tpr_tcon_id=" + contaID + " ";
                    }
                    else
                    {
                        Join_NDP = " tnd_pai_id=" + user.PaisID + " and tnd_tcon_id=" + contaID + " and tnd_tpi_id<>3 and tnd_moneda=" + monedaID.ToString()
                        + " and tnd_fecha_emision>='" + fecha_inicial + " 00:00:00' and tnd_fecha_emision<='" + fecha_final + " 23:59:59'";
                        Join_NDP2 = " and tpr_pai_id=" + user.PaisID + " and tpr_tcon_id=" + contaID + " and tpr_mon_id =" + monedaID.ToString();
                    }
                    if (Serie != "")
                    {
                        Join_NDP += " and tnd_serie='" + Serie + "' ";
                    }
                    if (grupoSAT > 0)
                    {
                        Join_NDP += " and tnd_suc_id in (select tes_suc_id from tbl_empresa_sucursal, tbl_empresa where tes_tem_id=tem_id and tem_grupo_sat='" + grupoSAT + "')";
                    }
                }
                if (int.Parse(Tipo) == 18)//Notas de Credito Ajuste Contable
                {
                    if (contaID == 3)
                    {
                        Join_NCA = " tnc_pai_id=" + user.PaisID + " and tnc_tpi_id in (3,10) and tnc_ttr_id=18 " +
                        " and tnc_fecha>='" + fecha_inicial + "' and tnc_fecha<='" + fecha_final + "'";
                        Join_NC2A = " and tpr_pai_id=" + user.PaisID + " ";
                    }
                    else if (consolidar == "si")
                    {
                        Join_NCA = " tnc_pai_id=" + user.PaisID + " and tnc_tcon_id=" + contaID + " and tnc_tpi_id in (3,10) and tnc_ttr_id=18 " +
                        " and tnc_fecha>='" + fecha_inicial + "' and tnc_fecha<='" + fecha_final + "'";
                        Join_NC2A = " and tpr_pai_id=" + user.PaisID + " and tpr_tcon_id=" + contaID + " ";
                    }
                    else
                    {
                        Join_NCA = " tnc_pai_id=" + user.PaisID + " and tnc_tcon_id=" + contaID + " and tnc_tpi_id in (3,10) and tnc_ttr_id=18 and tnc_mon_id = " + monedaID.ToString()
                        + " and tnc_fecha>='" + fecha_inicial + "' and tnc_fecha<='" + fecha_final + "'";
                        Join_NC2A = " and tpr_pai_id=" + user.PaisID + " and tpr_tcon_id=" + contaID + " and tpr_mon_id =" + monedaID.ToString();
                    }

                    if (Serie != "")
                    {
                        Join_NCA += " and tnc_serie='" + Serie + "' ";
                    }
                    if (grupoSAT > 0)
                    {
                        Join_NCA += " and tnc_suc_id in (select tes_suc_id from tbl_empresa_sucursal, tbl_empresa where tes_tem_id=tem_id and tem_grupo_sat='" + grupoSAT + "')";
                    }
                }
            }
        }

        string path = "libro_ventas";
        RE_GenericBean reg = DB.Traducir_reportes(user, path);                

        LibroDiarioDS ds = null;
        ArrayList arr = null;
        if (!Page.IsPostBack)
        {
            string grupoSATStr = "";
            switch (grupoSAT) {
                case 1: grupoSATStr = "CENTRAL GT"; break;
                case 2: grupoSATStr = "COMBEX"; break;
                case 3: grupoSATStr = "ALSERSA"; break;
                case 4: grupoSATStr = "PUERTO QUETZAL"; break;
                case 5: grupoSATStr = "PUERTO BARRIOS"; break;
            }

            if (tipo == 0)//resumido
            {
                ds = (LibroDiarioDS)DB.getLibro_Ventas(Join_Facturacion, Join_ND, Join_NC, Documentos, Join_NC2, Join_ND2, Join_Facturacion2, monedaID, contaID, user, Join_NDP, Join_NDP2, Join_NCA, Join_NC2A, reg.strC10 + " " + grupoSATStr);
                ViewState["dscompras"] = ds;
            }
            else if (tipo == 1)//detallado
            {
                arr = (ArrayList)DB.getLibro_Ventas_detallado(Join_Facturacion, Join_ND, Join_NC, Documentos, Join_NC2, Join_ND2, Join_Facturacion2, monedaID, contaID, user, Join_NDP, Join_NDP2, Join_NCA, Join_NC2A, reg.strC10 + " DETALLADO " + grupoSATStr);
                ds = (LibroDiarioDS)arr[0];
                serv_no_afecto = (Decimal)arr[1];
                serv_afecto = (Decimal)arr[2];
                impuesto = (Decimal)arr[3];
                no_sujeto = (Decimal)arr[4];
                docs = (int)arr[5];
                ViewState["dscompras"] = ds;
            }

            if (tipo == 0)
            {
                rpt = new CrystalDecisions.CrystalReports.Engine.ReportDocument();

                if (user.PaisID == 1)
                    rpt.Load(Server.MapPath("~/CR_Ventas_GT.rpt"));
                else
                    rpt.Load(Server.MapPath("~/CR_Ventas.rpt"));

                rpt.SetDataSource(ds.Tables["Compras"]);

                //***titulos traducidos***
                rpt.SetParameterValue("Tpais", reg.strC1);
                rpt.SetParameterValue("Tconta", reg.strC2);
                rpt.SetParameterValue("Tmoneda", reg.strC3);
                //rpt.SetParameterValue("Ttitulo", reg.strC10);
                rpt.SetParameterValue("Tdesde", reg.strC4);
                rpt.SetParameterValue("Thasta", reg.strC5);
                rpt.SetParameterValue("fechaini", fecha_inicial);
                rpt.SetParameterValue("fechafin", fecha_final);
                rpt.SetParameterValue("moneda", Utility.TraducirMonedaInt(monedaID));
                rpt.SetParameterValue("pais", user.pais.Nombre);
                if (user.PaisID == 2)
                {
                    ventas_no_sujetas = "Ventas No Sujetas";
                }
                rpt.SetParameterValue("Ventas_No_Sujetas", ventas_no_sujetas);
                string contabilidad = "";
                if (contaID == 1) { contabilidad = "FISCAL"; }
                if (contaID == 2) { contabilidad = "FINANCIERA"; }
                if (contaID == 3) { contabilidad = "CONSOLIDADO"; }
                rpt.SetParameterValue("contabilidad", contabilidad);
                rpt.SetParameterValue("empresa", Server.MapPath(user.pais.Imagepath));
                CrystalReportViewer1.ReportSource = rpt;
                CrystalReportViewer1.DataBind();
            }
            else if (tipo == 1)
            {
                rpt = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
                rpt.Load(Server.MapPath("~/CR_Ventas_detallado.rpt"));
                rpt.SetDataSource(ds.Tables["Compras"]);
                //string path = "libro_ventas";
                //RE_GenericBean reg = DB.Traducir_reportes(user, path);
                //***titulos traducidos***
                rpt.SetParameterValue("Tpais", reg.strC1);
                rpt.SetParameterValue("Tconta", reg.strC2);
                rpt.SetParameterValue("Tmoneda", reg.strC3);
                //rpt.SetParameterValue("Ttitulo", reg.strC10);
                rpt.SetParameterValue("Tdesde", reg.strC4);
                rpt.SetParameterValue("Thasta", reg.strC5);
                rpt.SetParameterValue("fechaini", fecha_inicial);
                rpt.SetParameterValue("fechafin", fecha_final);
                rpt.SetParameterValue("moneda", Utility.TraducirMonedaInt(monedaID));
                rpt.SetParameterValue("pais", user.pais.Nombre);
                if (user.PaisID == 2)
                {
                    ventas_no_sujetas = "Ventas No Sujetas";
                }
                rpt.SetParameterValue("Ventas_No_Sujetas", ventas_no_sujetas);
                string contabilidad = "";
                if (contaID == 1) { contabilidad = "FISCAL"; }
                if (contaID == 2) { contabilidad = "FINANCIERA"; }
                if (contaID == 3) { contabilidad = "CONSOLIDADO"; }
                rpt.SetParameterValue("contabilidad", contabilidad);
                rpt.SetParameterValue("empresa", Server.MapPath(user.pais.Imagepath));
                rpt.SetParameterValue("usuario", user.ID);
                rpt.SetParameterValue("nafservicio", serv_no_afecto);
                rpt.SetParameterValue("afservicio", serv_afecto);
                rpt.SetParameterValue("impuesto", impuesto);
                rpt.SetParameterValue("nosujeto", no_sujeto);
                rpt.SetParameterValue("nodocs", docs);
                CrystalReportViewer1.ReportSource = rpt;
                CrystalReportViewer1.DataBind();
            }
        }
        else
        {
            //ds = (LibroDiarioDS)ViewState["dscompras"];
            ds = (LibroDiarioDS)ViewState["dscompras"];

            if (tipo == 0)
            {
                rpt = new CrystalDecisions.CrystalReports.Engine.ReportDocument();                

                if (user.PaisID == 1)
                    rpt.Load(Server.MapPath("~/CR_Ventas_GT.rpt"));
                else
                    rpt.Load(Server.MapPath("~/CR_Ventas.rpt"));

                rpt.SetDataSource(ds.Tables["Compras"]);
                //string path = "libro_ventas";
                //RE_GenericBean reg = DB.Traducir_reportes(user, path);
                //***titulos traducidos***
                rpt.SetParameterValue("Tpais", reg.strC1);
                rpt.SetParameterValue("Tconta", reg.strC2);
                rpt.SetParameterValue("Tmoneda", reg.strC3);
                //rpt.SetParameterValue("Ttitulo", reg.strC10);
                rpt.SetParameterValue("Tdesde", reg.strC4);
                rpt.SetParameterValue("Thasta", reg.strC5);
                rpt.SetParameterValue("fechaini", fecha_inicial);
                rpt.SetParameterValue("fechafin", fecha_final);
                rpt.SetParameterValue("moneda", Utility.TraducirMonedaInt(monedaID));
                rpt.SetParameterValue("pais", user.pais.Nombre);
                if (user.PaisID == 2)
                {
                    ventas_no_sujetas = "Ventas No Sujetas";
                }
                rpt.SetParameterValue("Ventas_No_Sujetas", ventas_no_sujetas);
                string contabilidad = "";
                if (contaID == 1) { contabilidad = "FISCAL"; }
                if (contaID == 2) { contabilidad = "FINANCIERA"; }
                if (contaID == 3) { contabilidad = "CONSOLIDADO"; }
                rpt.SetParameterValue("contabilidad", contabilidad);
                rpt.SetParameterValue("empresa", Server.MapPath(user.pais.Imagepath));
                CrystalReportViewer1.ReportSource = rpt;
                CrystalReportViewer1.DataBind();
            }
            else if (tipo == 1)
            {
                rpt = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
                rpt.Load(Server.MapPath("~/CR_Ventas_detallado.rpt"));
                rpt.SetDataSource(ds.Tables["Compras"]);
                //string path = "libro_ventas";
                //RE_GenericBean reg = DB.Traducir_reportes(user, path);
                //***titulos traducidos***
                rpt.SetParameterValue("Tpais", reg.strC1);
                rpt.SetParameterValue("Tconta", reg.strC2);
                rpt.SetParameterValue("Tmoneda", reg.strC3);
                //rpt.SetParameterValue("Ttitulo", reg.strC10);
                rpt.SetParameterValue("Tdesde", reg.strC4);
                rpt.SetParameterValue("Thasta", reg.strC5);
                rpt.SetParameterValue("fechaini", fecha_inicial);
                rpt.SetParameterValue("fechafin", fecha_final);
                rpt.SetParameterValue("moneda", Utility.TraducirMonedaInt(monedaID));
                rpt.SetParameterValue("pais", user.pais.Nombre);
                if (user.PaisID == 2)
                {
                    ventas_no_sujetas = "Ventas No Sujetas";
                }
                rpt.SetParameterValue("Ventas_No_Sujetas", ventas_no_sujetas);
                string contabilidad = "";
                if (contaID == 1) { contabilidad = "FISCAL"; }
                if (contaID == 2) { contabilidad = "FINANCIERA"; }
                if (contaID == 3) { contabilidad = "CONSOLIDADO"; }
                rpt.SetParameterValue("contabilidad", contabilidad);
                rpt.SetParameterValue("empresa", Server.MapPath(user.pais.Imagepath));
                rpt.SetParameterValue("usuario", user.ID);
                rpt.SetParameterValue("nafservicio", serv_no_afecto);
                rpt.SetParameterValue("afservicio", serv_afecto);
                rpt.SetParameterValue("impuesto", impuesto);
                rpt.SetParameterValue("nosujeto", no_sujeto);
                rpt.SetParameterValue("nodocs", docs);
                CrystalReportViewer1.ReportSource = rpt;
                CrystalReportViewer1.DataBind();
            }
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

