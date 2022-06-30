using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Reports_WhiteList : System.Web.UI.Page
{
    UsuarioBean user = null;
    CrystalDecisions.CrystalReports.Engine.ReportDocument rpt;
    LibroDiarioDS ds = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["usuario"] == null)
        {
            Response.Redirect("../default.aspx");
        }
        else
        {
            user = (UsuarioBean)Session["usuario"];
        }
        int empresaID = int.Parse(Request.QueryString["eid"].ToString());
        int sucID = int.Parse(Request.QueryString["sucid"].ToString());
        int Tipo_Persona = int.Parse(Request.QueryString["tpiid"].ToString());
        if (!Page.IsPostBack)
        {
            ds = (LibroDiarioDS)DB.GetReporte_WhiteList(user, empresaID, sucID, Tipo_Persona);
            ViewState["Whitelist"] = ds;
        }
        else
        {
            ds = (LibroDiarioDS)ViewState["Whitelist"];
        }
        SucursalBean SucBean = (SucursalBean)DB.getSucursal(sucID);
        rpt = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
        rpt.Load(Server.MapPath("~/CR_WhiteList.rpt"));
        rpt.SetDataSource(ds.Tables["WhiteList"]);
        rpt.SetParameterValue("pais", user.pais.Nombre);
        rpt.SetParameterValue("usuario", user.ID);
        rpt.SetParameterValue("sucursal", SucBean.Nombre);
        rpt.SetParameterValue("empresa", Server.MapPath(user.pais.Imagepath));
        rpt.SetParameterValue("tipo_persona", Tipo_Persona.ToString());
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