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
using CrystalDecisions.Shared;

public partial class ventas_reports : System.Web.UI.Page
{
    UsuarioBean user = null;
    LibroDiarioDS ds = new LibroDiarioDS();
    string fechainicial = "";
    string fechafinal = "";
    int monID = 0;
    string AGENTE = "";
    string DESTINO = "";
    CrystalDecisions.CrystalReports.Engine.ReportDocument rpt;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["usuario"] == null)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "Nombre", "<script> refresh(); </script>");
        }
        user = (UsuarioBean)Session["usuario"];
        if (Request.QueryString["fechaini"] != null)
        {
            fechainicial = Request.QueryString["fechaini"].ToString();
            fechafinal = Request.QueryString["fechafin"].ToString();
        }
        int.Parse(Request.QueryString["monID"].ToString());
        int reporttype = int.Parse(Request.QueryString["reptype"].ToString());
        if (reporttype == 2)
        {  

            reporte_comisiones(user);
        }
        else if (reporttype == 1)
        {
            corte_comision(user);
        }
        
    }

   

    protected void reporte_comisiones(UsuarioBean user)
    {
        string conta = "";
        if (user.contaID == 1)
        {
            conta = "Fiscal";
        }
        else if (user.contaID == 2)
        {
            conta = "Financiera";
        }
        string fechaini = Request.QueryString["f_ini"].ToString();
        string fechafin = Request.QueryString["f_fin"].ToString();
        int ted = int.Parse(Request.QueryString["ted"].ToString());
        if (!Page.IsPostBack)
        {
            #region Formatear Fechas
            //Fecha Inicio
            int fe_dia = int.Parse(fechaini.Substring(3, 2));
            int fe_mes = int.Parse(fechaini.Substring(0, 2));
            int fe_anio = int.Parse(fechaini.Substring(6, 4));
            fechaini = fe_anio.ToString() + "/";
            if (fe_mes < 10)
            {
                fechaini += "0" + fe_mes.ToString() + "/";
            }
            else
            {
                fechaini += fe_mes.ToString() + "/";
            }
            if (fe_dia < 10)
            {
                fechaini += "0" + fe_dia.ToString();
            }
            else
            {
                fechaini += fe_dia.ToString();
            }
            //Fecha Fin
            fe_dia = int.Parse(fechafin.Substring(3, 2));
            fe_mes = int.Parse(fechafin.Substring(0, 2));
            fe_anio = int.Parse(fechafin.Substring(6, 4));
            fechafin = fe_anio.ToString() + "/";
            if (fe_mes < 10)
            {
                fechafin += "0" + fe_mes.ToString() + "/";
            }
            else
            {
                fechafin += fe_mes.ToString() + "/";
            }
            if (fe_dia < 10)
            {
                fechafin += "0" + fe_dia.ToString();
            }
            else
            {
                fechafin += fe_dia.ToString();
            }
            #endregion
            ds = (LibroDiarioDS)DB.GetRepComisiones(user, fechaini,fechafin,ted);
            ViewState["ComisionesDS"] = ds;
        }
        ds = (LibroDiarioDS)ViewState["ComisionesDS"];
        decimal tp = DB.getTipoCambioByDay(user, fechafin);
        rpt = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
        rpt.Load(Server.MapPath("~/CR_comisiones.rpt"));
        rpt.SetDataSource(ds.Tables["comisiones"]);
        rpt.SetParameterValue("usuario", user.ID);
        rpt.SetParameterValue("fechaini", fechaini);
        rpt.SetParameterValue("fechafin", fechafin);
        rpt.SetParameterValue("empresa", Server.MapPath(user.pais.Imagepath));
        rpt.SetParameterValue("pais", user.pais.Nombre);
        rpt.SetParameterValue("conta", conta);
        rpt.SetParameterValue("tc", tp);
        CrystalReportViewer1.ReportSource = rpt;
        CrystalReportViewer1.DataBind();
    }


    protected void corte_comision(UsuarioBean user)
    {
        string conta = "";
        if (user.contaID == 1)
        {
            conta = "Fiscal";
        }
        else if (user.contaID == 2)
        {
            conta = "Financiera";
        }
        string fechaini = Request.QueryString["f_ini"].ToString();
        string fechafin = Request.QueryString["f_fin"].ToString();
        int  ted = int.Parse(Request.QueryString["ted"].ToString());
        int vendedor_id = int.Parse(Request.QueryString["vendedor"].ToString());
        string nombre = "";
        string criterio = " and tipo_usuario=1 and pais='" + user.pais.ISO + "' and pw_activo <> 0 and id_usuario="+ vendedor_id +" ";
        ArrayList arr = (ArrayList)DB.getProveedor_cajachica(criterio);
        foreach (RE_GenericBean vendedor in arr)
        {
            nombre = vendedor.strC2;
        }
        if (!Page.IsPostBack)
        {
            #region Formatear Fechas
            //Fecha Inicio
            int fe_dia = int.Parse(fechaini.Substring(3, 2));
            int fe_mes = int.Parse(fechaini.Substring(0, 2));
            int fe_anio = int.Parse(fechaini.Substring(6, 4));
            fechaini = fe_anio.ToString() + "/";
            if (fe_mes < 10)
            {
                fechaini += "0" + fe_mes.ToString() + "/";
            }
            else
            {
                fechaini += fe_mes.ToString() + "/";
            }
            if (fe_dia < 10)
            {
                fechaini += "0" + fe_dia.ToString();
            }
            else
            {
                fechaini += fe_dia.ToString();
            }
            //Fecha Fin
            fe_dia = int.Parse(fechafin.Substring(3, 2));
            fe_mes = int.Parse(fechafin.Substring(0, 2));
            fe_anio = int.Parse(fechafin.Substring(6, 4));
            fechafin = fe_anio.ToString() + "/";
            if (fe_mes < 10)
            {
                fechafin += "0" + fe_mes.ToString() + "/";
            }
            else
            {
                fechafin += fe_mes.ToString() + "/";
            }
            if (fe_dia < 10)
            {
                fechafin += "0" + fe_dia.ToString();
            }
            else
            {
                fechafin += fe_dia.ToString();
            }
            #endregion
            ds = (LibroDiarioDS)DB.getcorte_comision(user, fechaini, fechafin, ted, vendedor_id);
            ViewState["corte_comisionDS"] = ds;
        }
        ds = (LibroDiarioDS)ViewState["corte_comisionDS"];
        rpt = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
        rpt.Load(Server.MapPath("~/CR_corte_comision.rpt"));
        rpt.SetDataSource(ds.Tables["corte_comision"]);
        rpt.SetParameterValue("usuario", user.ID);
        rpt.SetParameterValue("fechaini", fechaini);
        rpt.SetParameterValue("fechafin", fechafin);
        rpt.SetParameterValue("moneda",Utility.TraducirMonedaInt(user.Moneda));
        //rpt.SetParameterValue("empresa", Server.MapPath(user.pais.Imagepath));
        rpt.SetParameterValue("pais", user.pais.Nombre);
        rpt.SetParameterValue("nombre", nombre);
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
