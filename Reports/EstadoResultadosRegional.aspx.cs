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
using System.Net.Mail;
using System.Net;
using System.Net.Mime;
using System.IO;
public partial class Reports_EstadoResultadosRegional : System.Web.UI.Page
    {
        string fecha_inicial = "";
        string fecha_final = "";
        string archivo = "C:/BAWReportBSyER/Estado_Resultados_Regional.xls";
        CrystalDecisions.CrystalReports.Engine.ReportDocument rpt;
        protected void Page_Load(object sender, EventArgs e)
        {

            if (Session["usuario"] == null)
            {
                Response.Redirect("../default.aspx");
            }
            UsuarioBean user = (UsuarioBean)Session["usuario"];
            fecha_inicial = Request.QueryString["f_ini"].ToString();
            fecha_final = Request.QueryString["f_fin"].ToString();
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
            //--
            decimal tipocambio = DB.getTipoCambioHoy(user.PaisID);
            LibroDiarioDS ds = new LibroDiarioDS();
            ArrayList arr = (ArrayList)DB.getEstadoResultadosRegional(fecha_inicial,fecha_final);
            //ArrayList arr = (ArrayList)DB.getEstadoResultadosRegional(fecha1, fecha2);
            decimal total = 0, total2 = 0;
            decimal total_GT = 0, total_CR = 0, total_PR = 0, total_HN = 0, total_ESL = 0, total_NIC = 0, total_ESL2 = 0, total_GRH = 0, total_ISI = 0, total_MAYAN = 0, total_BEL = 0;
            decimal totalactivo = 0, totalactivo2 = 0;
            decimal totalpasivo = 0, totalpasivo2 = 0;
            string tipo = "";
            int i = 0;
            DB.parametros Params = DB.EmpresaParametros(user.PaisID, "", "", "ESTADO DE RESULTADOS REGIONAL", ""); //pais, sistema, doc_id, titulo, edicion

            foreach (RE_GenericBean rgb in arr)
            {
                if (rgb.intC2 == 3)// Ingresos
                {
                    total = rgb.decC2 - rgb.decC1;
                    total_GT = rgb.decC4 - rgb.decC3;
                    total_CR = rgb.decC6 - rgb.decC5;
                    total_PR = rgb.decC8 - rgb.decC7;
                    total_HN = rgb.decC10 - rgb.decC9;
                    total_ESL = rgb.decC12 - rgb.decC11;
                    total_NIC = rgb.decC14 - rgb.decC13;
                    total_ESL2 = rgb.decC16 - rgb.decC15;
                    total_GRH = rgb.decC18 - rgb.decC17;
                    total_ISI = rgb.decC20 - rgb.decC19;
                    total_MAYAN = rgb.decC22 - rgb.decC21;
                    total_BEL = rgb.decC24 - rgb.decC23;
                    totalactivo += total;
                }
                else if (rgb.intC2 == 4) //Egresos
                {
                    total = rgb.decC1 - rgb.decC2;
                    total_GT = rgb.decC3 - rgb.decC4;
                    total_CR = rgb.decC5 - rgb.decC6;
                    total_PR = rgb.decC7 - rgb.decC8;
                    total_HN = rgb.decC9 - rgb.decC10;
                    total_ESL = rgb.decC11 - rgb.decC12;
                    total_NIC = rgb.decC13 - rgb.decC14;
                    total_ESL2 = rgb.decC15 - rgb.decC16;
                    total_GRH = rgb.decC17 - rgb.decC18;
                    total_ISI = rgb.decC19 - rgb.decC20;
                    total_MAYAN = rgb.decC21 - rgb.decC22;
                    total_BEL = rgb.decC23 - rgb.decC24;
                    totalpasivo += total;
                }
                if (rgb.intC1 == 1)
                {
                    i++;
                    object[] objArr = { rgb.strC1, rgb.strC2, "", "", rgb.intC2, rgb.intC1, total, rgb.strC3, total_GT, total_CR, total_PR, total_HN, total_ESL, total_NIC, total_ESL2, total_GRH, total_ISI, total_MAYAN, total_BEL, i == 1 ? Params.logo : null, i == 1 ? Params.titulo : "", i == 1 ? Params.edicion : "" };
                    ds.Tables["Estado_Resultados_regional"].Rows.Add(objArr);
                }
                else if (rgb.intC1 == 2)
                {
                    i++;
                    object[] objArr = { rgb.strC1, "", rgb.strC2, "", rgb.intC2, rgb.intC1, total, rgb.strC3, total_GT, total_CR, total_PR, total_HN, total_ESL, total_NIC, total_ESL2, total_GRH, total_ISI, total_MAYAN, total_BEL, i == 1 ? Params.logo : null, i == 1 ? Params.titulo : "", i == 1 ? Params.edicion : "" };
                    ds.Tables["Estado_Resultados_regional"].Rows.Add(objArr);
                }
                else if (rgb.intC1 == 3)
                {
                    i++;
                    object[] objArr = { rgb.strC1, "", "", rgb.strC2, rgb.intC2, rgb.intC1, total, rgb.strC3, total_GT, total_CR, total_PR, total_HN, total_ESL, total_NIC, total_ESL2, total_GRH, total_ISI, total_MAYAN, total_BEL, i == 1 ? Params.logo : null, i == 1 ? Params.titulo : "", i == 1 ? Params.edicion : "" };
                    ds.Tables["Estado_Resultados_regional"].Rows.Add(objArr);
                }			
			
                if (i == 0) {
                    object[] objArr = { "", "", "", "", 0, 0, 0, "", 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, Params.logo, Params.titulo, Params.edicion };
                    ds.Tables["Estado_Resultados_regional"].Rows.Add(objArr);			                			    
                }

            }
            #region Backup
            /*
        decimal totalotros = 0;
        tipo = "OTROS INGRESOS Y EGRESOS";
        RE_GenericBean rgb3 = (RE_GenericBean)DB.getDatos("Gastos Adminitrativos", "'4.3.0.0.0000', '4.3.0.1.0000', '4.3.0.1.0001', '4.3.0.1.0002', '4.3.0.1.0003', '4.3.0.1.0004', '4.3.0.1.0005', '4.3.0.1.0006', '4.3.0.1.0007', '4.3.0.1.0008', '4.3.0.1.0009', '4.3.0.2.0000', '4.3.0.2.0001', '4.3.0.2.0002', '4.3.0.2.0003', '4.3.0.2.0004', '4.3.0.2.0005', '4.3.0.3.0000', '4.3.0.3.0001', '4.3.0.3.0002', '4.3.0.4.0000', '4.3.0.4.0001', '4.3.0.4.0002', '4.3.0.4.0003', '4.3.0.4.0004', '4.3.0.4.0005', '4.3.0.4.0006', '4.3.0.4.0007', '4.3.0.5.0000', '4.3.0.5.0001', '4.3.0.5.0002', '4.3.0.5.0003', '4.3.0.5.0004', '4.3.0.5.0005', '4.3.0.5.0006', '4.3.0.5.0007', '4.3.0.5.0008', '4.3.0.5.0009', '4.3.0.6.0000', '4.3.0.6.0001', '4.3.0.6.0002', '4.3.0.6.0003', '4.3.0.6.0004', '4.3.0.6.0005', '4.3.0.6.0006', '4.3.0.7.0000', '4.3.0.7.0001', '4.3.0.7.0002', '4.3.0.7.0003', '4.3.0.7.0004', '4.3.0.7.0005', '4.3.0.7.0006', '4.3.0.7.0007', '4.3.0.7.0008', '4.3.0.7.0009', '4.3.0.7.0010', '4.3.0.7.0011'", user.PaisID, monID, tconID, tipocambio, fecha_inicial, fecha_final);
        if (rgb3 != null)
        {
            object[] objArr1 = { rgb3.strC1, rgb3.strC2, rgb3.decC1, rgb3.decC2, tipo, rgb3.decC1 + rgb3.decC2 };
            ds.Tables["EstadoResultado_tbl"].Rows.Add(objArr1);
            totalotros += rgb3.decC1;
        }

        rgb3 = (RE_GenericBean)DB.getDatos("Gastos y productos financieros", "'4.4.0.0.0000', '4.4.0.1.0000', '4.4.0.1.0001', '4.4.0.1.0002', '4.4.0.1.0003', '4.4.0.1.0004', '3.2.0.2.0000', '3.2.0.2.0001', '3.2.0.2.0002', '3.2.0.2.0003', '3.2.0.2.0004', '3.2.0.2.0005', '3.2.0.2.0006', '3.2.0.2.0007'", user.PaisID, monID, tconID, tipocambio, fecha_inicial,fecha_final);
        if (rgb3 != null)
        {
            object[] objArr2 = { rgb3.strC1, rgb3.strC2, rgb3.decC1, rgb3.decC2, tipo, rgb3.decC1 + rgb3.decC2 };
            ds.Tables["EstadoResultado_tbl"].Rows.Add(objArr2);
            totalotros += rgb3.decC1;
        }
        rgb3 = (RE_GenericBean)DB.getDatos("Otros ingresos", "'3.2.0.0.0000', '3.2.0.1.0000', '3.2.0.1.0001', '3.2.0.1.0002', '3.2.0.1.0003'", user.PaisID, monID, tconID, tipocambio, fecha_inicial,fecha_final);
        if (rgb3 != null)
        {
            object[] objArr3 = { rgb3.strC1, rgb3.strC2, rgb3.decC1, rgb3.decC2, tipo, rgb3.decC1 + rgb3.decC2 };
            ds.Tables["EstadoResultado_tbl"].Rows.Add(objArr3);
            totalotros += rgb3.decC1;
        }
        decimal GranTotal = totalactivo + totalotros - totalpasivo;
         */
            #endregion
            if (!Page.IsPostBack)
            {
                ViewState["dsEstado_ResultadosRegional"] = ds;
            }
            else
            {
                ds = (LibroDiarioDS)ViewState["dsEstado_ResultadosRegional"];
            }
            decimal GranTotal = totalactivo - totalpasivo;

            rpt = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
            rpt.Load(Server.MapPath("~/CR_ERregional.rpt"));
            rpt.SetDataSource(ds.Tables["Estado_Resultados_regional"]);
            //rpt.SetParameterValue("moneda", "USD");
            //rpt.SetParameterValue("contabilidad", "CONSOLIDADO");
            rpt.SetParameterValue("fecha_inicial", fecha_inicial);
            rpt.SetParameterValue("fecha_final", fecha_final);
            //rpt.SetParameterValue("empresa", Server.MapPath(user.pais.Imagepath));
            rpt.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.Excel, "C:/BAWReportBSyER/Estado_Resultados_Regional.xls");
           DB.CreateMessageWithAttachment(archivo,0);
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