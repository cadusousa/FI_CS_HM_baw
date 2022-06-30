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

public partial class Reports_balance : System.Web.UI.Page
{
    CrystalDecisions.CrystalReports.Engine.ReportDocument rpt;
    string fecha_inicial = "";
    string fecha_final = "";
    string consmonfiscal = "";    
    string consmonRep = "";
    
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["usuario"] == null)
        {
            Response.Redirect("../default.aspx");
        }
        UsuarioBean user = (UsuarioBean)Session["usuario"];
        int monID = int.Parse(Request.QueryString["monID"].ToString());
        int tconID = int.Parse(Request.QueryString["tcon"].ToString());
        decimal tc = 0;
        fecha_inicial = Request.QueryString["f_ini"].ToString();
        fecha_final = Request.QueryString["f_fin"].ToString();
        consmonfiscal = Request.QueryString["consmonfiscal"].ToString();
        if (consmonfiscal == "si")
        {
            consmonRep = "CONSOLIDADO";
        }
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
        LibroDiarioDS ds = new LibroDiarioDS();
        decimal total = 0, total2 = 0;
        decimal totalactivo = 0, totalactivo2 = 0;
        decimal totalpasivo = 0, totalpasivo2 = 0;
        string tipo = "";

        string path = "estado_resultados";
        RE_GenericBean reg = DB.Traducir_reportes(user, path);

        if (!Page.IsPostBack)
        {
            string conta_nombre = "";
            switch (tconID)
            {
                case 1: conta_nombre = "FISCAL"; break;
                case 2: conta_nombre = "FINANCIERA"; break;
                case 3: conta_nombre = "CONSOLIDADO"; break;
            }


            string moneda_nombre = "";
            if (monID == 8) moneda_nombre = "USD$"; else moneda_nombre = "LOCAL";
            
            ArrayList arr = (ArrayList)DB.getEstadoResultados(user, monID, tconID, tipocambio, fecha_inicial, fecha_final, consmonfiscal);
            tc = DB.getTipoCambioXFecha(user.PaisID, fecha_final);
            
            /*Response.Write ("<table border=1 width=100%>");
            Response.Write("<tr><td><h1>ESTADO RESULTADOS</h1></td></tr>");
            Response.Write("<tr><td>Pais : " + user.pais.Nombre + "</td><td>Contabilidad : " + conta_nombre + "</td><td> Moneda : " + moneda_nombre + "</td></tr>");
            Response.Write("<tr><td>Del : " + fecha_inicial + "</td><td>Al : " + fecha_final + "</td></tr>");                
            Response.Write("</table>");

            Response.Write("<table border=1 width=100%>");  */

            DB.parametros Params = DB.EmpresaParametros(user.PaisID, "", "", reg.strC10, ""); //pais, sistema, doc_id, titulo, edicion
            int i = 0;
            
            foreach (RE_GenericBean rgb in arr)
            {
                if (rgb.intC2 == 3)// Ingresos
                {
                    total = rgb.decC2 - rgb.decC1;
                    totalactivo += total;
                }
                else if (rgb.intC2 == 4) //Egresos
                {
                    total = rgb.decC1 - rgb.decC2;
                    totalpasivo += total;
                }
                if (rgb.intC1 == 1)
                {
        			i++;		        	
                    object[] objArr = { rgb.strC1, rgb.strC2, "", "", rgb.intC2, rgb.intC1, total, rgb.strC3, 0, i == 1 ? Params.logo : null, i == 1 ? Params.titulo : "", i == 1 ? Params.edicion : "" };
                    ds.Tables["Estado_Resultados"].Rows.Add(objArr);
                    //Response.Write("<tr><td>" + rgb.strC1 + "</td><td>" + rgb.strC2 + "</td><td>" + "" + "</td><td>" + "" + "</td><td>" + rgb.intC2 + "</td><td>" + rgb.intC1 + "</td><td>" + total + "</td><td><a href='SubReporte_ER.aspx?" + fecha_inicial + "?" + fecha_final + "?" + rgb.strC1 + "?" + tconID + "?" + monID + "?' target=_blank>Detalle</a></td></tr>");
                }
                else if (rgb.intC1 == 2)
                {
        			i++;		        	
                    object[] objArr = { rgb.strC1, "", rgb.strC2, "", rgb.intC2, rgb.intC1, total, rgb.strC3, 0, i == 1 ? Params.logo : null, i == 1 ? Params.titulo : "", i == 1 ? Params.edicion : "" };
                    ds.Tables["Estado_Resultados"].Rows.Add(objArr);
                    //Response.Write("<tr><td>" + rgb.strC1 + "</td><td>" + "" + "</td><td>" + rgb.strC2 + "</td><td>" + "" + "</td><td>" + rgb.intC2 + "</td><td>" + rgb.intC1 + "</td><td>" + total + "</td><td><a href='SubReporte_ER.aspx?" + fecha_inicial + "?" + fecha_final + "?" + rgb.strC1 + "?" + tconID + "?" + monID + "?' target=_blank>Detalle</a></td></tr>");
                }
                else if (rgb.intC1 == 3)
                {
        			i++;		        	
                    object[] objArr = { rgb.strC1, "", "", rgb.strC2, rgb.intC2, rgb.intC1, total, rgb.strC3, 0, i == 1 ? Params.logo : null, i == 1 ? Params.titulo : "", i == 1 ? Params.edicion : "" };
                    ds.Tables["Estado_Resultados"].Rows.Add(objArr);
                    //Response.Write("<tr><td>" + rgb.strC1 + "</td><td>" + "" + "</td><td>" + "" + "</td><td>" + rgb.strC2 + "</td><td>" + rgb.intC2 + "</td><td>" + rgb.intC1 + "</td><td>" + total + "</td><td><a href='SubReporte_ER.aspx?" + fecha_inicial + "?" + fecha_final + "?" + rgb.strC1 + "?" + tconID + "?" + monID + "?' target=_blank>Detalle</a></td></tr>");                
                }
            }
            //Response.Write("</table>");
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
			
			
            if (i == 0) {
                object[] objArr = { "", "", "", "", 0, 0, 0, "", 0, Params.logo, Params.titulo, Params.edicion };
                ds.Tables["Estado_Resultados"].Rows.Add(objArr);			                			
            }
            
            ViewState["dsEstado_Resultados"] = ds;
        }
        else
        {
            ds = (LibroDiarioDS)ViewState["dsEstado_Resultados"];
        }
        decimal GranTotal = totalactivo - totalpasivo;
        
        rpt = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
        rpt.Load(Server.MapPath("~/CR_Estado_Resultados.rpt"));
        rpt.SetDataSource(ds.Tables["Estado_Resultados"]);

        rpt.SetParameterValue("moneda", Utility.TraducirMonedaInt(monID));
        if (tconID == 1)
        {
            rpt.SetParameterValue("contabilidad", "FISCAL");
        }
        else if (tconID == 2)
        {
            rpt.SetParameterValue("contabilidad", "FINANCIERA");
        }
        else if (tconID == 3)
        {
            rpt.SetParameterValue("contabilidad", "CONSOLIDADO");
        }

        rpt.SetParameterValue("contaID", tconID.ToString());
        rpt.SetParameterValue("monID", monID.ToString());
        rpt.SetParameterValue("pais", user.pais.Nombre);
        rpt.SetParameterValue("fecha_inicial", fecha_inicial);
        rpt.SetParameterValue("fecha_final", fecha_final);
        rpt.SetParameterValue("empresa", Server.MapPath(user.pais.Imagepath));
        rpt.SetParameterValue("tipo_cambio", tc);
        rpt.SetParameterValue("usuario", user.ID);
        //***titulos traducidos***
        rpt.SetParameterValue("Tpais", reg.strC1);
        rpt.SetParameterValue("Tconta", reg.strC2);
        rpt.SetParameterValue("Tmoneda", reg.strC3);
        //rpt.SetParameterValue("Ttitulo", reg.strC10);
        rpt.SetParameterValue("Tdesde", reg.strC4);
        rpt.SetParameterValue("Thasta", reg.strC5);
        rpt.SetParameterValue("consolida_moneda", consmonRep);
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