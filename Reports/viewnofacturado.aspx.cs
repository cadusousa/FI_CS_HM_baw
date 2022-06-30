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

public partial class Reports_viewnofacturado : System.Web.UI.Page
{
    string fecha_inicial = "";
    string fecha_final = "";
    CrystalDecisions.CrystalReports.Engine.ReportDocument rpt;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["usuario"] == null)
        {
            Response.Redirect("../default.aspx");
        }
            UsuarioBean user = (UsuarioBean)Session["usuario"];
            //int monID = int.Parse(Request.QueryString["monID"].ToString());
            //int tconID = int.Parse(Request.QueryString["tcon"].ToString());
            fecha_inicial = Request.QueryString["f_ini"].ToString();
            fecha_final = Request.QueryString["f_fin"].ToString();
            string maritimo = Request.QueryString["maritimo"].ToString();
            string aereo = Request.QueryString["aereo"].ToString();
            string terrestre = Request.QueryString["terrestre"].ToString();
            string wms = Request.QueryString["wms"].ToString();
            string seguros = Request.QueryString["seguros"].ToString();
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
            decimal tipocambio = DB.getTipoCambioXFecha(user.PaisID, fecha_final);

            LibroDiarioDS ds = null;
            if (!Page.IsPostBack)
            {
                ds = DB.getNoFacturado(user,fecha_inicial,fecha_final, maritimo, terrestre, aereo,wms,seguros);
                ViewState["dsnofac"] = ds;
            }
            else
            {
                ds = (LibroDiarioDS)ViewState["dsnofac"];
            }
            rpt = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
            rpt.Load(Server.MapPath("~/CR_noFacturado.rpt"));
            rpt.SetDataSource(ds.Tables["no_facturado"]);
            rpt.SetParameterValue("desde", fecha_inicial.Substring(0,10));
            rpt.SetParameterValue("hasta", fecha_final.Substring(0,10));
            rpt.SetParameterValue("usuario", user.ID);
            rpt.SetParameterValue("pais", user.pais.Nombre);
            rpt.SetParameterValue("empresa", Server.MapPath(user.pais.Imagepath));
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