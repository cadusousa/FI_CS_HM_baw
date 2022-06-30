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

public partial class Reports_viewSOA : System.Web.UI.Page
{
    UsuarioBean user = null;
    LibroDiarioDS ds = new LibroDiarioDS();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["usuario"] == null)
        {
            Response.Redirect("../default.aspx");
        }

        user = (UsuarioBean)Session["usuario"];
        if (!Page.IsPostBack)
        {
            string mbl = Request.QueryString["mbl"].ToString();
            //ArrayList prov_arr = (ArrayList)DB.getProvisionesbyMBL(mbl);//2 porque es tipopersona=agente segun tbl_tipo_persona
            //ArrayList nd_arr = (ArrayList)DB.getndbyMBL(mbl);//2 porque es tipopersona=agente segun tbl_tipo_persona
            //ArrayList nc_arr = (ArrayList)DB.getncbyMBL(mbl);//2 porque es tipopersona=agente segun tbl_tipo_persona
            //ArrayList fact_arr = (ArrayList)DB.getfactbyMBL(mbl);//2 porque es tipopersona=agente segun tbl_tipo_persona

            CrystalDecisions.CrystalReports.Engine.ReportDocument rpt = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
            rpt.Load(Server.MapPath("/BAW/CR_SOA.rpt"));
            rpt.SetDataSource(ds.Tables["soa_tbl"]);
            //rpt.SetParameterValue("fecha_inicial", fechaini);
            //rpt.SetParameterValue("fecha_final", fechafin);
            rpt.SetParameterValue("prepara", user.Nombre);
            //rpt.ParameterFields["fecha_inicial"] = "13/05/2010";
            //rpt.ParameterFields["fecha_final"] = "14/05/2010";
            CrystalReportViewer1.DisplayGroupTree = false;
            CrystalReportViewer1.ReportSource = rpt;
            CrystalReportViewer1.DataBind();
        }
    }
}
