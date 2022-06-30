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

public partial class invoice_rep_contrasenas : System.Web.UI.Page
{
    UsuarioBean user = null;
    CrystalDecisions.CrystalReports.Engine.ReportDocument rpt;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["usuario"] == null)
        {
            Response.Redirect("../default.aspx");
        }

        user = (UsuarioBean)Session["usuario"];
        string fechainicial = "";
        string fechafinal = "";
        string tipo_persona = "";
        string codigo_cliente = "";
        string nocontrasena = "";

        string ordenar_por = Request.QueryString["ordenar_por"].ToString();
      
        if (Request.QueryString["fecha_inicio"] != "")
        {
            fechainicial = Request.QueryString["fecha_inicio"].ToString();
        }
        if (Request.QueryString["fecha_fin"] != "")
        {
            fechafinal = Request.QueryString["fecha_fin"].ToString();
        }
        if (Request.QueryString["tipo_persona"] != "")
        {
            tipo_persona = Request.QueryString["tipo_persona"].ToString();
        }
        if (Request.QueryString["codigo_cliente"] != "")
        {
            codigo_cliente = Request.QueryString["codigo_cliente"].ToString();
        }
        if (Request.QueryString["nocontrasena"] != "")
        {
            nocontrasena = Request.QueryString["nocontrasena"].ToString();
        }


        #region Formatear Fechas
        string Fecha_Inicial = "";
        string Fecha_Final = "";
        int fe_dia = 0;
        int fe_mes = 0;
        int fe_anio = 0;

        if (fechainicial != "")
        {
            //Fecha Inicio
            fe_dia = int.Parse(fechainicial.Substring(0, 2));
            fe_mes = int.Parse(fechainicial.Substring(3, 2));
            fe_anio = int.Parse(fechainicial.Substring(6, 4));
            Fecha_Inicial = fe_anio.ToString() + "/";
            if (fe_mes < 10)
            {
                Fecha_Inicial += "0" + fe_mes.ToString() + "/";
            }
            else
            {
                Fecha_Inicial += fe_mes.ToString() + "/";
            }
            if (fe_dia < 10)
            {
                Fecha_Inicial += "0" + fe_dia.ToString();
            }
            else
            {
                Fecha_Inicial += fe_dia.ToString();
            }
        }
        if (fechafinal != "")
        {
            //Fecha Fin
            fe_dia = int.Parse(fechafinal.Substring(0, 2));
            fe_mes = int.Parse(fechafinal.Substring(3, 2));
            fe_anio = int.Parse(fechafinal.Substring(6, 4));
            Fecha_Final = fe_anio.ToString() + "/";
            if (fe_mes < 10)
            {
                Fecha_Final += "0" + fe_mes.ToString() + "/";
            }
            else
            {
                Fecha_Final += fe_mes.ToString() + "/";
            }
            if (fe_dia < 10)
            {
                Fecha_Final += "0" + fe_dia.ToString();
            }
            else
            {
                Fecha_Final += fe_dia.ToString();
            }
        }
        #endregion

        if (!Page.IsPostBack)
        {
            LibroDiarioDS ds = (LibroDiarioDS)DB.getContrasenasCliente(Fecha_Inicial, Fecha_Final, tipo_persona, codigo_cliente, nocontrasena, user, ordenar_por);
            ViewState["contrasenasDS"] = ds;
            rpt = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
            rpt.Load(Server.MapPath("~/CR_Contrasenas_Cliente.rpt"));
            rpt.SetDataSource(ds.Tables["tbl_contrasenas_cliente"]);

            CrystalReportViewer1.ReportSource = rpt;
            CrystalReportViewer1.DataBind();
        }
        else
        {
            LibroDiarioDS ds = (LibroDiarioDS)ViewState["contrasenasDS"];
            rpt = new CrystalDecisions.CrystalReports.Engine.ReportDocument();

            rpt.Load(Server.MapPath("~/CR_Contrasenas_Cliente.rpt"));
            rpt.SetDataSource(ds.Tables["tbl_contrasenas_cliente"]);
            
            CrystalReportViewer1.ReportSource = rpt;
            CrystalReportViewer1.DataBind();
        }
    }
}