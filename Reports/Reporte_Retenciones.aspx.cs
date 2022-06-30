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

public partial class Reports_Reporte_Retenciones : System.Web.UI.Page
{
    UsuarioBean user = null;
    int contaID = 0;
    int monID = 0;
    int t_rep = 0;
    int t_ret = 0;
    string t_tpi = "";
    string fecha_inicial = "";
    string fecha_final = "";
    string Join = "";
    CrystalDecisions.CrystalReports.Engine.ReportDocument rpt;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["usuario"] == null)
        {
            Response.Redirect("../default.aspx");
        }
        user = (UsuarioBean)Session["usuario"];
        contaID = int.Parse(Request.QueryString["t_con"].ToString());
        monID = int.Parse(Request.QueryString["t_mon"].ToString());
        t_rep = int.Parse(Request.QueryString["t_rep"].ToString());
        t_ret = int.Parse(Request.QueryString["t_ret"].ToString());
        t_tpi = Request.QueryString["t_tpi"].ToString();
        fecha_inicial = Request.QueryString["f_inicial"].ToString();
        fecha_final = Request.QueryString["f_final"].ToString();
        #region Formatear Fechas
        //Fecha Inicio
        int fe_dia = int.Parse(fecha_inicial.Substring(3, 2));
        int fe_mes = int.Parse(fecha_inicial.Substring(0, 2));
        int fe_anio = int.Parse(fecha_inicial.Substring(6, 4));
        fecha_inicial = fe_anio.ToString() + "/";
        if (fe_mes < 10)
        {
            fecha_inicial += "0" + fe_mes.ToString() + "/";
        }
        else
        {
            fecha_inicial += fe_mes.ToString() + "/";
        }
        if (fe_dia < 10)
        {
            fecha_inicial += "0" + fe_dia.ToString();
        }
        else
        {
            fecha_inicial += fe_dia.ToString();
        }
        //Fecha Fin
        fe_dia = int.Parse(fecha_final.Substring(3, 2));
        fe_mes = int.Parse(fecha_final.Substring(0, 2));
        fe_anio = int.Parse(fecha_final.Substring(6, 4));
        fecha_final = fe_anio.ToString() + "/";
        if (fe_mes < 10)
        {
            fecha_final += "0" + fe_mes.ToString() + "/";
        }
        else
        {
            fecha_final += fe_mes.ToString() + "/";
        }
        if (fe_dia < 10)
        {
            fecha_final += "0" + fe_dia.ToString();
        }
        else
        {
            fecha_final += fe_dia.ToString();
        }
        #endregion
        if (t_rep == 3)
        {
            #region Reporte de Clientes
            Join = " and a.tfa_pai_id=" + user.PaisID + " and d.tre_pai_id=" + user.PaisID + " and a.tfa_conta_id=" + contaID + " and a.tfa_moneda=" + monID + "" +
                //" and a.tfa_fecha_emision>='" + fecha_inicial + " 00:00:00' and a.tfa_fecha_emision<='" + fecha_final + " 23:59:59'";
                " and d.tre_fecha>='" + fecha_inicial + "' and d.tre_fecha<='" + fecha_final + "'";
            if (t_ret == 0)
            {
                Join += " and c.tpr_tipo_pago in (5,6) ";
            }
            else
            {
                Join += " and c.tpr_tipo_pago in (" + t_ret + ") ";
            }
           // Join += " group by c.tpr_doc_referencia,d.tre_fecha, a.tfa_cli_id, a.tfa_nombre, a.tfa_moneda, e.ttp_nombre,a.tfa_serie, a.tfa_correlativo  ";
            #endregion
        }
        else if (t_rep == 4)
        {
            #region Reporte de Proveedores
            Join = " and a.tpr_pai_id=" + user.PaisID + " and a.tpr_tcon_id=" + contaID + " and a.tpr_mon_id=" + monID + ""+
            //" and a.tpr_fecha_acepta>='" + fecha_inicial + "' and a.tpr_fecha_acepta<='" + fecha_final + "' ";
            " and b.trp_fecha_generacion>='" + fecha_inicial + "' and b.trp_fecha_generacion<='" + fecha_final + "' ";
            if (t_ret > 0)
            {
                Join += " and b.trp_trt_id="+t_ret+" ";
            }
            Join += " and a.tpr_tpi_id in (" + t_tpi + ") ";
            #endregion
        }
        LibroDiarioDS ds = null;
        if (!Page.IsPostBack)
        {
            ds = (LibroDiarioDS)DB.getReporte_Retenciones(user, t_rep, Join);
            ViewState["ds_Retenciones"] = ds;
        }
        else
        {
            ds = (LibroDiarioDS)ViewState["ds_Retenciones"];
        }
        rpt = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
        if (t_rep == 3)
        {
            rpt.Load(Server.MapPath("~/CR_Retencion_Clientes.rpt"));
        }
        else
        {
            rpt.Load(Server.MapPath("~/CR_Retencion_Proveedores.rpt"));
        }
        rpt.SetDataSource(ds.Tables["Reporte_Retenciones"]);
        rpt.SetParameterValue("fechaini", fecha_inicial);
        rpt.SetParameterValue("fechafin", fecha_final);
        rpt.SetParameterValue("moneda", Utility.TraducirMonedaInt(monID));
        rpt.SetParameterValue("pais", user.pais.Nombre);
        string contabilidad = "";
        if (contaID == 1) { contabilidad = "FISCAL"; }
        if (contaID == 2) { contabilidad = "FINANCIERA"; }
        rpt.SetParameterValue("contabilidad", contabilidad);
        rpt.SetParameterValue("empresa", Server.MapPath(user.pais.Imagepath));
        rpt.SetParameterValue("usuario", user.ID);
        CrystalReportViewer1.ReportSource = rpt;
        CrystalReportViewer1.DataBind();
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
