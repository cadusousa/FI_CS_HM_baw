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

public partial class Reports_rep_soa_regional : System.Web.UI.Page
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
        if (reporttype == 1)
        {  // soa regional
            soa_regional(user);
        }

    }

    protected void soa_regional(UsuarioBean user)
    {
        string fechafin = Request.QueryString["fechafin"].ToString();
        string tpi = Request.QueryString["tpi"].ToString();
        string cliID = Request.QueryString["cliID"].ToString();
        string nombre = Request.QueryString["nombre"].ToString();
        if (!Page.IsPostBack)
        {
            #region Formatear Fechas
            //Fecha Inicio

            //Fecha Fin
            int fe_dia = int.Parse(fechafin.Substring(3, 2));
            int fe_mes = int.Parse(fechafin.Substring(0, 2));
            int fe_anio = int.Parse(fechafin.Substring(6, 4));
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
            ds = (LibroDiarioDS)DB.Get_Soa_Regional(user, fechafin, int.Parse(cliID), int.Parse(tpi));
            ViewState["soaRegionalDS"] = ds;
        }


        ds = (LibroDiarioDS)ViewState["soaRegionalDS"];
        rpt = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
        rpt.Load(Server.MapPath("~/CR_soa_regional.rpt"));
        rpt.SetDataSource(ds.Tables["soa_regional"]);

        rpt.SetParameterValue("usuario", user.ID);
        rpt.SetParameterValue("hasta", fechafin);
        rpt.SetParameterValue("empresa", Server.MapPath(user.pais.Imagepath));
        if (tpi == "4")
        {
            rpt.SetParameterValue("persona", "Proveedor");
        }
        else if (tpi == "2")
        {
            rpt.SetParameterValue("persona", "Agente");
        }
        else if (tpi == "5")
        {
            rpt.SetParameterValue("persona", "Naviera");
        }
        else if (tpi == "6")
        {
            rpt.SetParameterValue("persona", "Linea Aerea");
        }
        else if (tpi == "10")
        {
            rpt.SetParameterValue("persona", "Intercompany");
        }
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