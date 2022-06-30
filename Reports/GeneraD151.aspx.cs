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


public partial class Reports_GeneraD151 : System.Web.UI.Page
{
    UsuarioBean user = null;

    CrystalDecisions.CrystalReports.Engine.ReportDocument rpt;
    CrystalDecisions.CrystalReports.Engine.ReportDocument rpt1;
    protected void Page_Load(object sender, EventArgs e)
    {
        user = (UsuarioBean)Session["usuario"];
        if (Session["usuario"] == null)
        {
            Response.Redirect("../default.aspx");
        }
        LibroDiarioDS ds = new LibroDiarioDS();
        string fechaini = "", fechafin = "", clienteNombre = "";
        if (Request.QueryString["f_ini"] != null && !Request.QueryString["f_ini"].ToString().Equals(""))
            fechaini = Request.QueryString["f_ini"].ToString();
        if (Request.QueryString["f_fin"] != null && !Request.QueryString["f_fin"].ToString().Equals(""))
            fechafin = Request.QueryString["f_fin"].ToString();
        int monID = int.Parse(Request.QueryString["monID"].ToString());
        int contaID = int.Parse(Request.QueryString["tcon"].ToString());
        int tpi = int.Parse(Request.QueryString["t_persona"].ToString());
        int tipo_reporte = int.Parse(Request.QueryString["tipo"].ToString());
        string tipoPersona = "";
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
        #endregion
     
            //if (Session["usuario"] == null)
            //{
            //    ClientScript.RegisterStartupScript(this.GetType(), "Nombre", "<script> refresh(); </script>");
            //}

            if (tpi == 1) tipoPersona = "Ventas";
            else if (tpi == 2) tipoPersona = "Compras";

            
               //Definir ViewState
            if (!Page.IsPostBack)
            {
                ds = DB.getD151(fechaini, fechafin, monID, contaID, tpi, user, tipo_reporte);
                ViewState["D151_tbl"] = ds;
            }
            ds = (LibroDiarioDS)ViewState["D151_tbl"];
            rpt = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
            rpt1 = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
            if (tipo_reporte == 1)//detallado
            {
                if (tpi == 1)//venta
                {
                    rpt.Load(Server.MapPath("~/CR_D151_detallado.rpt"));
                    rpt.SetDataSource(ds.Tables["D151_tbl"]);
                    rpt1.Load(Server.MapPath("~/CR_sub_D151.rpt"));
                    rpt1.SetDataSource(ds.Tables["D151_poliza"]);

                    rpt.Subreports[0].SetDataSource(ds.Tables["D151_poliza"]);

                    //--
                    //crP.Subreports[0].SetDataSource(dsEgresos1); //Primer subreporte
                    //crvReporteador.ReportSource = crP;
                }
                else //compra
                {
                    rpt.Load(Server.MapPath("~/CR_D151.rpt"));
                }
            }
            else if (tipo_reporte == 2)//resumido
            {
                rpt.Load(Server.MapPath("~/CR_D151.rpt"));
                rpt.SetDataSource(ds.Tables["D151_tbl"]);
            }
            if (contaID == 1)
            {
                rpt.SetParameterValue("contabilidad", "FISCAL");
            }
            else if (contaID == 2)
            {
                rpt.SetParameterValue("contabilidad", "FINANCIERA");
            }
            else if (contaID == 3)
            {
                rpt.SetParameterValue("contabilidad", "CONSOLIDADO");
            }

            rpt.SetParameterValue("moneda", Utility.TraducirMonedaInt(monID));
            rpt.SetParameterValue("pais", user.pais.Nombre);
            rpt.SetParameterValue("fecha_inicial", fechaini);
            rpt.SetParameterValue("fecha_final", fechafin);
            rpt.SetParameterValue("empresa", Server.MapPath(user.pais.Imagepath));
            rpt.SetParameterValue("usuario", user.ID);
            //if (tpi == 1) { rpt.SetParameterValue("titulo", "HISTORICO DE VENTAS"); }
            //if (tpi == 2) { rpt.SetParameterValue("titulo", "HISTORICO DE COMPRAS"); }

            // rpt.SetParameterValue("empresa", Server.MapPath(user.pais.Imagepath));
            //CrystalReportViewer1.DisplayGroupTree = false;
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