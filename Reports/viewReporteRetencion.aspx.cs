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

public partial class Reports_viewReporteRetencion : System.Web.UI.Page
{
    UsuarioBean user = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["usuario"] == null)
        {
            Response.Redirect("../default.aspx");
        }
        user = (UsuarioBean)Session["usuario"];


        string fechaini = "", fechafin = "", clienteNombre = "", retencionNombre = "", usuario = "";
        int clienteID = 0, retID=0;
        int tpi = 0;

        if (Request.QueryString["cliID"] != null && !Request.QueryString["cliID"].ToString().Equals(""))
            clienteID = int.Parse(Request.QueryString["cliID"].ToString());
        if (Request.QueryString["tpi"] != null && !Request.QueryString["tpi"].ToString().Equals(""))
            tpi = int.Parse(Request.QueryString["tpi"].ToString());
        if (Request.QueryString["fechaini"] != null && !Request.QueryString["fechaini"].ToString().Equals(""))
            fechaini = Request.QueryString["fechaini"].ToString();
        if (Request.QueryString["fechafin"] != null && !Request.QueryString["fechafin"].ToString().Equals(""))
            fechafin = Request.QueryString["fechafin"].ToString();
        if (Request.QueryString["clientenombre"] != null && !Request.QueryString["clientenombre"].ToString().Equals(""))
            clienteNombre = Request.QueryString["clientenombre"].ToString();
        if (Request.QueryString["retencionNombre"] != null && !Request.QueryString["retencionNombre"].ToString().Equals(""))
            retencionNombre= Request.QueryString["retencionNombre"].ToString();
        if (Request.QueryString["retencionID"] != null && !Request.QueryString["retencionID"].ToString().Equals(""))
            retID = int.Parse(Request.QueryString["retencionID"].ToString());
        int monID = int.Parse(Request.QueryString["moneda"].ToString());
        int contaID = int.Parse(Request.QueryString["conta"].ToString());
        
        usuario = user.ID;

        LibroDiarioDS ds = null;
        ds = DB.generaReporteRetencion(retID, fechaini, fechafin, user.PaisID, clienteID, tpi, retencionNombre);

        CrystalDecisions.CrystalReports.Engine.ReportDocument rpt = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
        //rpt.Load(Server.MapPath("/AIMAR/CR_retencion.rpt")); 2020-01-21
        rpt.Load(Server.MapPath("~/CR_retencion.rpt"));
        rpt.SetDataSource(ds.Tables["Retencion"]);
        //rpt.SetParameterValue("retencionNombre", retencionNombre);
        rpt.SetParameterValue("fechaini", fechaini);
        rpt.SetParameterValue("fechafin", fechafin);
        rpt.SetParameterValue("usuario", usuario);
        //CrystalReportViewer1.DisplayGroupTree = false;
        CrystalReportViewer1.ReportSource = rpt;
        CrystalReportViewer1.DataBind();

    }
}
