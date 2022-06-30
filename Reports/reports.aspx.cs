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

public partial class Reports_reports : System.Web.UI.Page
{
    UsuarioBean user = null;
    LibroDiarioDS ds = new LibroDiarioDS();
    string fechainicial = "";
    string fechafinal = "";
    int monID = 0;
    string AGENTE = "";
    string DESTINO = "";
    string formato = "";
    CrystalDecisions.CrystalReports.Engine.ReportDocument rpt;
    protected void Page_Load(object sender, EventArgs e)
    {
        //if (Session["usuario"] == null)
        //{
        //    ClientScript.RegisterStartupScript(this.GetType(), "Nombre", "<script> refresh(); </script>");
        //}
        if (Session["usuario"] == null)
        {
            Response.Redirect("../default.aspx");
        }
        user = (UsuarioBean)Session["usuario"];
        if (Request.QueryString["fechaini"] != null)
        {
            fechainicial = Request.QueryString["fechaini"].ToString();
            fechafinal = Request.QueryString["fechafin"].ToString();
        }

        if (Request.QueryString["formato"] != null)
        {
            formato = Request.QueryString["formato"].ToString();
         }
        
        int.Parse(Request.QueryString["monID"].ToString());
        int reporttype = int.Parse(Request.QueryString["reptype"].ToString());
        if (reporttype == 1) {  // libro diario
            if (!Page.IsPostBack) {
                reporte_diario(user);
            } else {
                ds = (LibroDiarioDS)Session["librodiario"];
                rpt = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
                rpt.Load(Server.MapPath("~/CR_librodiario.rpt"));
                rpt.SetDataSource(ds.Tables["Librodiario_tbl"]);
                rpt.SetParameterValue("moneda", Utility.TraducirMonedaInt(monID));
                if (user.contaID == 1)
                {
                    rpt.SetParameterValue("contabilidad", "FISCAL");
                }
                else if (user.contaID == 2)
                {
                    rpt.SetParameterValue("contabilidad", "FINANCIERA");
                }
                else if (user.contaID == 3)
                {
                    rpt.SetParameterValue("contabilidad", "CONSOLIDADO");
                }
                rpt.SetParameterValue("pais", user.pais.Nombre);
                rpt.SetParameterValue("fecha_inicio", fechainicial);
                rpt.SetParameterValue("fecha_fin", fechafinal);
                rpt.SetParameterValue(5, Server.MapPath(user.pais.Imagepath));
                CrystalReportViewer1.ReportSource = rpt;
                CrystalReportViewer1.DataBind();
                Session["librodiario"] = ds;
            }   
        } else if (reporttype == 2) { //libro mayor

            string html = "";

            string fechainicial = Request.QueryString["fechaini"].ToString();
            string fechafinal = Request.QueryString["fechafin"].ToString();
            int monID = int.Parse(Request.QueryString["monID"].ToString());
            string consMon = Request.QueryString["consmonfiscal"].ToString();

            DateTime date1 = Convert.ToDateTime(fechainicial);
            DateTime date2 = Convert.ToDateTime(fechafinal);

            LibroDiarioDS ds = null;

            switch (formato)
            {
                case "1":

                    if (!Page.IsPostBack)
                    {
                        //reporte_mayor(user);

                        ds = (LibroDiarioDS)DB.getlibromayor(fechainicial, fechafinal, monID, user, consMon);
                        Session["libromayor"] = ds;
                    }
                    else
                    {
                        ds = (LibroDiarioDS)Session["libromayor"];

                    }

                    /*
                    rpt = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
                    rpt.Load(Server.MapPath("~/CR_libromayor.rpt"));
                    rpt.SetDataSource(ds.Tables["Libromayor_tbl"]);
                    rpt.SetParameterValue("moneda", Utility.TraducirMonedaInt(monID));
                    if (user.contaID == 1)
                    {
                        rpt.SetParameterValue("contabilidad", "FISCAL");
                    }
                    else if (user.contaID == 2)
                    {
                        rpt.SetParameterValue("contabilidad", "FINANCIERA");
                    }
                    else if (user.contaID == 3)
                    {
                        rpt.SetParameterValue("contabilidad", "CONSOLIDADO");
                    }
                    rpt.SetParameterValue("pais", user.pais.Nombre);
                    rpt.SetParameterValue("fecha_inicio", date1.ToString("dd/M/yyyy"));
                    rpt.SetParameterValue("fecha_fin", date2.ToString("dd/M/yyyy"));
                    //rpt.SetParameterValue(5, Server.MapPath(user.pais.Imagepath));
                    CrystalReportViewer1.ReportSource = rpt;
                    CrystalReportViewer1.DataBind();
                    */
                    break;

                
            


                case "2":  //resumido por dia formato TLA HTML
            
                    if (!Page.IsPostBack)
                    {
                        html = getlibromayorTLAhtml(fechainicial, fechafinal, monID, user, consMon);

                         Session["libromayor"] = html;
                    }
                    else
                    {
                        html = Session["libromayor"].ToString();

                    }

                    Response.Write(html);

                    break;
            

            
                case "3":  //resumido por dia formato TLA EXCEL

                    if (!Page.IsPostBack)
                    {
                        html = getlibromayorTLAhtml(fechainicial, fechafinal, monID, user, consMon);

                        Session["libromayor"] = html;
                    }
                    else
                    {
                        html = Session["libromayor"].ToString();

                    }

                    Response.Clear();
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("Content-Disposition", "attachment;filename=LibroMayor" + user.PaisID + ".xls");
                    Response.Write(html);
                    Response.End();

                    break;

            
            
                case "4":  //resumido por dia formato TLA Crystal R
            



                    if (!Page.IsPostBack)
                    {
                        //reporte_mayorTLA(user);

                        ds = (LibroDiarioDS) getlibromayorTLA(fechainicial, fechafinal, monID, user, consMon);
                    
                        Session["libromayor"] = ds;
                    }
                    else
                    {
                        ds = (LibroDiarioDS)Session["libromayor"];

                    }

                    /*
                    rpt = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
                    rpt.Load(Server.MapPath("~/CR_libromayorTLA.rpt"));
                    rpt.SetDataSource(ds.Tables["Libromayor_tbl"]);
                    rpt.SetParameterValue("moneda", Utility.TraducirMonedaInt(monID));
                    if (user.contaID == 1)
                    {
                        rpt.SetParameterValue("contabilidad", "FISCAL");
                    }
                    else if (user.contaID == 2)
                    {
                        rpt.SetParameterValue("contabilidad", "FINANCIERA");
                    }
                    else if (user.contaID == 3)
                    {
                        rpt.SetParameterValue("contabilidad", "CONSOLIDADO");
                    }

                    rpt.SetParameterValue("pais", user.pais.Nombre);
                    rpt.SetParameterValue("fecha_inicio", date1.ToString("dd/M/yyyy"));
                    rpt.SetParameterValue("fecha_fin", date2.ToString("dd/M/yyyy"));
                    //rpt.SetParameterValue(5, Server.MapPath(user.pais.Imagepath));
                    CrystalReportViewer1.ReportSource = rpt;
                    CrystalReportViewer1.DataBind();
                    */
                    break;

            }


            if (ds != null) { 

                rpt = new CrystalDecisions.CrystalReports.Engine.ReportDocument();


                switch (formato)
                {
                    case "1":
                        rpt.Load(Server.MapPath("~/CR_libromayor.rpt"));
                        break;

                    case "2":  //resumido por dia formato TLA HTML

                        break;

                    case "3":  //resumido por dia formato TLA EXCEL

                        break;

                    case "4":  //resumido por dia formato TLA Crystal R
                        rpt.Load(Server.MapPath("~/CR_libromayorTLA.rpt"));
                        break;
                }




                
                rpt.SetDataSource(ds.Tables["Libromayor_tbl"]);
                rpt.SetParameterValue("moneda", Utility.TraducirMonedaInt(monID));
                if (user.contaID == 1)
                {
                    rpt.SetParameterValue("contabilidad", "FISCAL");
                }
                else if (user.contaID == 2)
                {
                    rpt.SetParameterValue("contabilidad", "FINANCIERA");
                }
                else if (user.contaID == 3)
                {
                    rpt.SetParameterValue("contabilidad", "CONSOLIDADO");
                }
                rpt.SetParameterValue("pais", user.pais.Nombre);
                rpt.SetParameterValue("fecha_inicio", date1.ToString("dd/M/yyyy"));
                rpt.SetParameterValue("fecha_fin", date2.ToString("dd/M/yyyy"));
                //rpt.SetParameterValue(5, Server.MapPath(user.pais.Imagepath));
                CrystalReportViewer1.ReportSource = rpt;
                CrystalReportViewer1.DataBind();
            
            }
            

        } else if (reporttype == 3) { //reporte_profit()
            reporte_profit(user);
        } else if (reporttype == 4) { //antiguedad saldos
            antiguedad_saldos(user);
        } else if (reporttype == 5) { //reporte de recibos
            reporte_recibos(user);
        }
        else if (reporttype == 6)//reporte profit resumido
        {
            reporte_profit_resumido(user);
        }
        else if (reporttype == 7) // reporte de depositos no asociados a un recibo
        {
            reporte_depositos_pendientes(user);
        }
        else if (reporttype == 8)
        {
            reporte_routing_facturados(user);
        }
        else if (reporttype == 9)
        {
            reporte_routing_NO_facturados(user);
        }
        else if (reporttype == 10)
        {
            reporte_routing_NO_asociados(user);
        }
        else if (reporttype == 11)
        {
            integracion_bancaria(user);
        }
        else if (reporttype == 12)
        {
            reporte_cuentas_x_pagar(user);  
        }
        else if (reporttype == 13)
        {
            reporte_creditos_clientes(user);
        }
    }

    protected void reporte_recibos(UsuarioBean user)
    {
        string fecha_inicial = Request.QueryString["fechaini"].ToString();
        string fecha_final = Request.QueryString["fechafin"].ToString();
        int monID = int.Parse(Request.QueryString["monID"].ToString());
        int contaID = int.Parse(Request.QueryString["conta"].ToString());
        int sucID = int.Parse(Request.QueryString["sucID"].ToString());
        string serie = Request.QueryString["serie"].ToString();
        string r_ini = Request.QueryString["r_ini"].ToString();
        string r_fin = Request.QueryString["r_fin"].ToString();
        int depositado = int.Parse(Request.QueryString["solopendiente"].ToString());

        string path = "reporte_recibos";
        RE_GenericBean reg = DB.Traducir_reportes(user, path);

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
        if (!Page.IsPostBack)
        {
            LibroDiarioDS ds = (LibroDiarioDS)DB.getRecibos_report(monID, contaID, fecha_final, fecha_inicial, sucID, serie, depositado, r_ini, r_fin, user.PaisID, reg.strC10);
            ViewState["recibosDS"] = ds;
            rpt = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
            rpt.Load(Server.MapPath("~/CR_Recibos.rpt"));
            rpt.SetDataSource(ds.Tables["recibos_tbl"]);

            //***titulos traducidos***
            rpt.SetParameterValue("Tpais", reg.strC1);
            rpt.SetParameterValue("Tconta", reg.strC2);
            rpt.SetParameterValue("Tmoneda", reg.strC3);
            //rpt.SetParameterValue("Ttitulo", reg.strC10);
            rpt.SetParameterValue("Tdesde", reg.strC4);
            rpt.SetParameterValue("Thasta", reg.strC5);
            rpt.SetParameterValue("fecha_inicial", fecha_inicial);
            rpt.SetParameterValue("fecha_final", fecha_final);
            rpt.SetParameterValue("moneda", Utility.TraducirMonedaInt(monID));
            rpt.SetParameterValue("pais", user.pais.Nombre);
            rpt.SetParameterValue(5, Server.MapPath(user.pais.Imagepath));
            if (contaID == 1)
            {
                rpt.SetParameterValue("contabilidad", "FISCAL");
            }
            else
            {
                rpt.SetParameterValue("contabilidad", "FINANCIERA");
            }
            CrystalReportViewer1.ReportSource = rpt;
            CrystalReportViewer1.DataBind();
        }
        else
        {
            LibroDiarioDS ds = (LibroDiarioDS)ViewState["recibosDS"];
            rpt = new CrystalDecisions.CrystalReports.Engine.ReportDocument();

            rpt.Load(Server.MapPath("~/CR_Recibos.rpt"));
            rpt.SetDataSource(ds.Tables["recibos_tbl"]);
            //string path = "reporte_recibos";
            //RE_GenericBean reg = DB.Traducir_reportes(user, path);
            //***titulos traducidos***
            rpt.SetParameterValue("Tpais", reg.strC1);
            rpt.SetParameterValue("Tconta", reg.strC2);
            rpt.SetParameterValue("Tmoneda", reg.strC3);
            //rpt.SetParameterValue("Ttitulo", reg.strC10);
            rpt.SetParameterValue("Tdesde", reg.strC4);
            rpt.SetParameterValue("Thasta", reg.strC5);
            rpt.SetParameterValue("fecha_inicial", fecha_inicial);
            rpt.SetParameterValue("fecha_final", fecha_final);
            rpt.SetParameterValue("moneda", Utility.TraducirMonedaInt(monID));
            rpt.SetParameterValue("pais", user.pais.Nombre);
            if (contaID == 1)
            {
                rpt.SetParameterValue("contabilidad", "FISCAL");
            }
            else
            {
                rpt.SetParameterValue("contabilidad", "FINANCIERA");
            }

            CrystalReportViewer1.ReportSource = rpt;
            CrystalReportViewer1.DataBind();
        }
    }

    protected void antiguedad_saldos(UsuarioBean user)
    {
        LibroDiarioDS ds = null;
        string tb_Tipo = Request.QueryString["tb_Tipo"].ToString();
        string ant_rec = Request.QueryString["antrecibos"].ToString();
        string es_coloader = Request.QueryString["es_coloader"].ToString();
        string fechacorte = Request.QueryString["fechacorte"].ToString();
        fechacorte = fechacorte.Substring(6, 4) + "-" + fechacorte.Substring(0, 2) + "-" + fechacorte.Substring(3, 2);
        int tipopersona = int.Parse(Request.QueryString["tipopersona"].ToString());
        int monID = int.Parse(Request.QueryString["monID"].ToString());
        int contaID = int.Parse(Request.QueryString["contaID"].ToString());
        string tipo_contabilidad = "";
        int cliID = 0;
        int credito = 0;
        if (Request.QueryString["credito"].ToString().Equals("Credito"))
            credito = 1;
        if (Request.QueryString["credito"].ToString().Equals("Contado"))
            credito = 2;
        string cobrador = Request.QueryString["cobrador"].ToString();
        string orderby = Request.QueryString["orderby"].ToString();
        if (Request.QueryString["cliID"].ToString() != null && !Request.QueryString["cliID"].ToString().Equals(""))
            cliID = int.Parse(Request.QueryString["cliID"].ToString());


        //Filtros APL 
        string  sucursal = "";
        string serie_factura = "";
        string serie_recibo = "";
        string incluir_dias_credito = "";

        sucursal = Request.QueryString["sucursal_id"].ToString();
        serie_factura = Request.QueryString["serie_factura"].ToString();
        serie_recibo = Request.QueryString["serie_recibo"].ToString();
        incluir_dias_credito = Request.QueryString["incluir_dias_credito"].ToString();
        //Fin filtros APL

        string monstr = "", tipopersonastr = "";
        if (monID == 1) monstr = "QUETZALES [GTQ]";
        else if (monID == 2) monstr = "DOLARES [USD]";
        else if (monID == 3) monstr = "LEMPIRAS [HNL]";
        //else if (monID == 4) monstr = "COLONES [NIC]";
        else if (monID == 4) monstr = "COLONES [C$]";
        else if (monID == 5) monstr = "COLONES [CRC]";
        else if (monID == 6) monstr = "PANAMA [PAB]";
        else if (monID == 7) monstr = "DOLARES [BZD]";
        else if (monID == 8) monstr = "DOLARES [USD]";

        if (tipopersona == 3 && es_coloader == "si") tipopersonastr = "COLOADERS";
        else if (tipopersona == 3 && es_coloader == "no") tipopersonastr = "CLIENTES";
        else if (tipopersona == 4) tipopersonastr = "PROVEEDORES";
        else if (tipopersona == 2) tipopersonastr = "AGENTES";
        else if (tipopersona == 5) tipopersonastr = "NAVIERAS";
        else if (tipopersona == 6) tipopersonastr = "LINEAS AEREAS";
        else if (tipopersona == 6) tipopersonastr = "CAJA CHICA";
        else if (tipopersona == 10) tipopersonastr = "INTERCOMPANY";
        if (contaID == 1) tipo_contabilidad = "Fiscal";
        if (contaID == 2) tipo_contabilidad = "Financiera";
        if (!Page.IsPostBack)
        {
            //LibroDiarioDS ds = (LibroDiarioDS)DB.getSaldosVencidos(user.PaisID, monID, contaID, fechacorte, tipopersona, cliID, credito, cobrador, ant_rec);


            if (int.Parse(tb_Tipo) == 1)
            { //saldos cortados

                if (tipopersona == 3)
                {
                    ds = (LibroDiarioDS)DB.getAntiguedadDeSaldosCliente(user.PaisID, monID, contaID, fechacorte, tipopersona, cliID, credito, cobrador, ant_rec, sucursal, serie_factura, serie_recibo, incluir_dias_credito, es_coloader);
                }
                else
                {
                    ds = (LibroDiarioDS)DB.getAntiguedadDeSaldosProveedor(user, user.PaisID, monID, contaID, fechacorte, tipopersona, cliID, credito, cobrador, ant_rec, sucursal, serie_factura, serie_recibo, incluir_dias_credito);
                }
            }
            else
            {
                if (tipopersona == 3)
                {
                    ds = (LibroDiarioDS)DB.getAntiguedadDeSaldosCliente(user.PaisID, monID, contaID, fechacorte, tipopersona, cliID, credito, cobrador, ant_rec, sucursal, serie_factura, serie_recibo, incluir_dias_credito, es_coloader);
                }
                else
                {
                    ds = (LibroDiarioDS)DB.getAntiguedadDeSaldosProveedor2(user, user.PaisID, monID, contaID, fechacorte, tipopersona, cliID, credito, cobrador, ant_rec, sucursal, serie_factura, serie_recibo, incluir_dias_credito);
                }
            }

            ViewState["antiguedadDS"] = ds;
            rpt = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
            // rpt.Load(Server.MapPath("~/CR_antiguedadsaldos.rpt"));
            rpt.Load(Server.MapPath("~/CR_antiguedad.rpt"));
            //rpt.SetDataSource(ds.Tables["antiguedadsaldos_tbl"]);
            rpt.SetDataSource(ds.Tables["tbl_antiguedad_cliente"]);
            rpt.SetParameterValue("tipoconta", tipo_contabilidad);
            rpt.SetParameterValue("tipopersona", tipopersonastr);
            rpt.SetParameterValue("fechacorte", fechacorte);
            rpt.SetParameterValue("moneda", monstr);
            rpt.SetParameterValue("usuario", user.ID);
            rpt.SetParameterValue("empresa", Server.MapPath(user.pais.Imagepath));
            rpt.SetParameterValue("pais", user.pais.Nombre);
            CrystalReportViewer1.ReportSource = rpt;
            CrystalReportViewer1.DataBind();
        }
        else
        {
            ds = (LibroDiarioDS)ViewState["antiguedadDS"];
            rpt = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
            rpt.Load(Server.MapPath("~/CR_antiguedad.rpt"));
            rpt.SetDataSource(ds.Tables["tbl_antiguedad_cliente"]);
            rpt.SetParameterValue("tipoconta", tipo_contabilidad);
            rpt.SetParameterValue("tipopersona", tipopersonastr);
            rpt.SetParameterValue("fechacorte", fechacorte);
            rpt.SetParameterValue("moneda", monstr);
            rpt.SetParameterValue("usuario", user.ID);
            rpt.SetParameterValue("empresa", Server.MapPath(user.pais.Imagepath));
            CrystalReportViewer1.ReportSource = rpt;
            CrystalReportViewer1.DataBind();
        }
    }


    protected void reporte_diario(UsuarioBean user) {
        fechainicial = Request.QueryString["fechaini"].ToString();
        fechafinal = Request.QueryString["fechafin"].ToString();
        monID = int.Parse(Request.QueryString["monID"].ToString());
        string consMon = Request.QueryString["consmonfiscal"].ToString();
        LibroDiarioDS ds = new LibroDiarioDS();
        ds = DB.getlibrodiario(fechainicial, fechafinal, monID, user, consMon);
        rpt = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
        rpt.Load(Server.MapPath("~/CR_librodiario.rpt"));
        rpt.SetDataSource(ds.Tables["Librodiario_tbl"]);
        rpt.SetParameterValue("moneda", Utility.TraducirMonedaInt(monID));
        if (user.contaID == 1)
        {
            rpt.SetParameterValue("contabilidad", "FISCAL");
        }
        else if (user.contaID == 2)
        {
            rpt.SetParameterValue("contabilidad", "FINANCIERA");
        }
        else if (user.contaID == 3)
        {
            rpt.SetParameterValue("contabilidad", "CONSOLIDADO");
        }
        rpt.SetParameterValue("pais", user.pais.Nombre);
        rpt.SetParameterValue("fecha_inicio", fechainicial);
        rpt.SetParameterValue("fecha_fin", fechafinal);
        rpt.SetParameterValue(5, Server.MapPath(user.pais.Imagepath));
        CrystalReportViewer1.DisplayGroupTree = false;
        CrystalReportViewer1.ReportSource = rpt;
        CrystalReportViewer1.DataBind();
        Session["librodiario"] = ds;
    }

    /*
    protected void reporte_mayor(UsuarioBean user) {
        string fechainicial = Request.QueryString["fechaini"].ToString();
        string fechafinal = Request.QueryString["fechafin"].ToString();
        int monID = int.Parse(Request.QueryString["monID"].ToString());
        string consMon = Request.QueryString["consmonfiscal"].ToString();

        LibroDiarioDS ds = (LibroDiarioDS)DB.getlibromayor(fechainicial, fechafinal, monID, user, consMon);
        rpt = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
        rpt.Load(Server.MapPath("~/CR_libromayor.rpt"));
        rpt.SetDataSource(ds.Tables["Libromayor_tbl"]);
        rpt.SetParameterValue("moneda", Utility.TraducirMonedaInt(monID));
        if (user.contaID == 1)
        {
            rpt.SetParameterValue("contabilidad", "FISCAL");
        }
        else if (user.contaID == 2)
        {
            rpt.SetParameterValue("contabilidad", "FINANCIERA");
        }
        else if (user.contaID == 3)
        {
            rpt.SetParameterValue("contabilidad", "CONSOLIDADO");
        }
        rpt.SetParameterValue("pais", user.pais.Nombre);
        rpt.SetParameterValue("fecha_inicio", fechainicial);
        rpt.SetParameterValue("fecha_fin", fechafinal);
        rpt.SetParameterValue(5, Server.MapPath(user.pais.Imagepath));
        CrystalReportViewer1.ReportSource = rpt;
        CrystalReportViewer1.DataBind();
        Session["libromayor"] = ds;
    }

    

    protected void reporte_mayorTLA(UsuarioBean user)
    {
        string fechainicial = Request.QueryString["fechaini"].ToString();
        string fechafinal = Request.QueryString["fechafin"].ToString();
        int monID = int.Parse(Request.QueryString["monID"].ToString());
        string consMon = Request.QueryString["consmonfiscal"].ToString();

        LibroDiarioDS ds = (LibroDiarioDS)DB.getlibromayorTLA(fechainicial, fechafinal, monID, user, consMon);
        rpt = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
        rpt.Load(Server.MapPath("~/CR_libromayorTLA.rpt"));
        rpt.SetDataSource(ds.Tables["Libromayor_tbl"]);
        rpt.SetParameterValue("moneda", Utility.TraducirMonedaInt(monID));
        if (user.contaID == 1)
        {
            rpt.SetParameterValue("contabilidad", "FISCAL");
        }
        else if (user.contaID == 2)
        {
            rpt.SetParameterValue("contabilidad", "FINANCIERA");
        }
        else if (user.contaID == 3)
        {
            rpt.SetParameterValue("contabilidad", "CONSOLIDADO");
        }

        DateTime date1 = Convert.ToDateTime(fechainicial);

        DateTime date2 = Convert.ToDateTime(fechafinal);

        rpt.SetParameterValue("pais", user.pais.Nombre);
        rpt.SetParameterValue("fecha_inicio", date1.ToString("dd/M/yyyy"));
        rpt.SetParameterValue("fecha_fin", date2.ToString("dd/M/yyyy"));
        //rpt.SetParameterValue(5, Server.MapPath(user.pais.Imagepath));
        CrystalReportViewer1.ReportSource = rpt;
        CrystalReportViewer1.DataBind();
        Session["libromayor"] = ds;
    }


    protected string reporte_mayorTLAhtml(UsuarioBean user)
    {
        string fechainicial = Request.QueryString["fechaini"].ToString();
        string fechafinal = Request.QueryString["fechafin"].ToString();
        int monID = int.Parse(Request.QueryString["monID"].ToString());
        string consMon = Request.QueryString["consmonfiscal"].ToString();


        string html = getlibromayorTLAhtml(fechainicial, fechafinal, monID, user, consMon);

        Session["libromayor"] = html;

        return html;
        

        /*
        LibroDiarioDS ds = (LibroDiarioDS)DB.getlibromayorTLA(fechainicial, fechafinal, monID, user, consMon);
        rpt = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
        rpt.Load(Server.MapPath("~/CR_libromayorTLA.rpt"));
        rpt.SetDataSource(ds.Tables["Libromayor_tbl"]);
        rpt.SetParameterValue("moneda", Utility.TraducirMonedaInt(monID));
        if (user.contaID == 1)
        {
            rpt.SetParameterValue("contabilidad", "FISCAL");
        }
        else if (user.contaID == 2)
        {
            rpt.SetParameterValue("contabilidad", "FINANCIERA");
        }
        else if (user.contaID == 3)
        {
            rpt.SetParameterValue("contabilidad", "CONSOLIDADO");
        }
        rpt.SetParameterValue("pais", user.pais.Nombre);
        rpt.SetParameterValue("fecha_inicio", fechainicial);
        rpt.SetParameterValue("fecha_fin", fechafinal);
        rpt.SetParameterValue(5, Server.MapPath(user.pais.Imagepath));
        CrystalReportViewer1.ReportSource = rpt;
        CrystalReportViewer1.DataBind();
        Session["libromayor"] = ds;
        */
    //}

    /*
    public class Upload_Files_st
    {
        public string ruta { get; set; }
        public string filename_old { get; set; }
        public string user { get; set; }
        public string ip { get; set; }
        public string attachments { get; set; }
        public Boolean erase { get; set; }
    }
    */
    /*
    public static void uploadlogo(string filename, string base64)
    {
        try {
/*
            Upload_Files_st file = new Upload_Files_st
            {
                ruta = "C://inetpub//wwwroot//site//logos//",
                filename_old = "",
                user = "",
                ip = "",
                attachments = @"<?xml version = ""1.0""?>
				        <root>
				        <row NUM = ""1""><name>" + filename + @"</name><file><![CDATA[" + base64 + @"]]></file></row>
				        </root>",
                erase = false
            };

            System.Web.Script.Serialization.JavaScriptSerializer JavaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            string strRequest = JavaScriptSerializer.Serialize(file);
            */
/*

            string json = @"<?xml version = ""1.0""?><root><row NUM = ""1""><name>" + filename + @"</name><file><![CDATA[" + base64 + @"]]></file></row></root>";

            json = "";

            string strRequest = (@"<?xml version=""1.0"" encoding=""utf-8""?>
<soap:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"">
  <soap:Body>
    <Upload_Files xmlns=""http://tempuri.org/"">
      <ruta>C://inetpub//wwwroot//site//logos//</ruta>
      <filename_old></filename_old>
      <user></user>
      <ip></ip>
      <attachments>" + json + @"</attachments>
      <erase>false</erase>
    </Upload_Files>
  </soap:Body>
</soap:Envelope>");

            //string webServiceUrl = "http://10.10.1.21:7480/SendParametros.asmx?wsdl";
        
            string webServiceUrl = "http://10.10.1.32:54822/SendParametros.asmx?wsdl";

            System.Net.HttpWebRequest req = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(webServiceUrl);

            req.Method = "POST";
            
            req.ContentType = "application/soap+xml; charset=utf-8";

            //req.ContentType = "text/xml; charset=utf-8";

            req.Accept = "text/xml";

            req.Headers.Add("SOAPAction", "http://tempuri.org/Upload_Files");

            req.ContentLength = strRequest.Length;

            using (System.IO.Stream stm = req.GetRequestStream())
            {
                using (System.IO.StreamWriter stmw = new System.IO.StreamWriter(stm))
                {
                    stmw.Write(strRequest);
                }
            }

            System.Net.WebResponse response = req.GetResponse();

            System.IO.Stream responseStream = response.GetResponseStream();

        }
        catch (Exception e)
        {
            string x = e.Message;
            //log4net ErrLog = new log4net();
            //ErrLog.ErrorLog(e.Message);
            //return null;
        }

    }
    */ 

    /*
    // Metodo que obtiene todo el libro mayor 2020-10-27
    public static string getlibromayorTLAhtml2(string fechainicial, string fechafin, int monID, UsuarioBean user, string consMon)
    {

        Npgsql.NpgsqlConnection conn;
        Npgsql.NpgsqlCommand comm;
        Npgsql.NpgsqlDataReader reader;
        RE_GenericBean rgb = null;
        string html = "", conta_Str = "", td = "", moneda_str = "";
        Decimal debe = 0, haber = 0;

        DateTime date1 = Convert.ToDateTime(fechainicial);

        DateTime date2 = Convert.ToDateTime(fechafin);

        var arr = (ArrayList)DB.getMonedasbyCriterio(" and 	ttm_id=" + monID + " LIMIT 1 ");

        foreach (RE_GenericBean rgb2 in arr)
        {
            moneda_str = rgb2.strC2.Substring(0,3);  
        }


        switch (user.contaID)
        {
            case 1:
                conta_Str = "FISCAL";
                break;
            case 2:
                conta_Str = "FINANCIERA";
                break;
            case 3:
                conta_Str = "CONSOLIDADO";
                break;
        }
        //C:/Users/GT-IT05/Documents/
       //C:/Users/GT-IT05/Documents/LibroMayor_files/

        DB.parametros Params = DB.EmpresaParametros(user.PaisID, "", "", "LIBRO MAYOR", "FORMATO 2"); //pais, sistema, doc_id, titulo, edicion

        string path = @"C:\Users\GT-IT05\Downloads\";

        path += "LibroMayor.htm";

        //path += "we4co-clr1r.html";

        string text = System.IO.File.ReadAllText(path);

        html = text;
        

/*
        try
        {

            string filename = "LibroMayor_" + user.PaisID + ".jpg";

            Byte[] bytes = Params.logo;
            string base64String = Convert.ToBase64String(bytes, 0, bytes.Length);

            System.IO.Stream stream = new System.IO.MemoryStream(Params.logo);

            System.Drawing.Image imgPhoto = System.Drawing.Image.FromStream(stream);

            //imgPhoto.Save(filename);

            //uploadlogo(filename, base64String);

            string json = @"<?xml version = ""1.0""?><root><row NUM = ""1""><name>" + filename + @"</name><file><![CDATA[" + base64String + @"]]></file></row></root>";

            SendParametros.SendParametros t = new SendParametros.SendParametros();

            var res = t.Upload_Files("C://inetpub//wwwroot//site//logos//BAW//", "", "", "", json, false);

            filename = "http" + "://www.aimargroup.com/logos/BAW/" + filename;

            string imagepath = string.Format("<img src='{0}' width='{1}' height='{2}'/>", filename, imgPhoto.Width, imgPhoto.Height);          
   
            html = (@"
<table width=100% border=0>
<tr>
    <td rowspan=2>" + imagepath + @"</td>
    <td colspan=1><h2>" + Params.nombre_empresa + @"</h2></td>  
    <td colspan=3 align=center><h2>" + Params.titulo + @"</h2></td>
    <td colspan=2 align=right>" + Params.edicion + @"</td>
</tr>
<tr>
    <td colspan=1><h2>" + Params.direccion + @"</h2></td>  
</tr>
<tr>
  <td>Pais : " + Params.country + @"</td><td>Contabilidad : " + conta_Str + @"</td><td>Moneda : " + monID + " - " + moneda_str + @"</td>
</tr>
<tr>
   <td>Fecha Inicial : " + date1.ToString("dd/M/yyyy") + @"</td><td>Fecha Final : " + date2.ToString("dd/M/yyyy") + @"</td>
</tr>
            ");

            html += (@"
                <tr>
                    <th style='border-bottom:1px solid black'>No.  Cuenta</th>
                    <th style='border-bottom:1px solid black'>Nombre Cuenta</th>
                    <th style='border-bottom:1px solid black'>Fecha</th>
                    <th style='border-bottom:1px solid black'>Saldo Inicial</th>
                    <th style='border-bottom:1px solid black'>Debe</th>
                    <th style='border-bottom:1px solid black'>Haber</th>
                    <th style='border-bottom:1px solid black'>Saldo Final</th>
                </tr>                    
                ");

            conn = DB.OpenConnection();
            comm = new Npgsql.NpgsqlCommand();
            comm.Connection = conn;
            comm.CommandType = CommandType.Text;



            string query = (@"
select cue_id, cue_nombre, '' tdi_no_partida, '' tdi_fecha, saldo_inicial, tdi_debe, tdi_haber, saldo_inicial+tdi_debe-tdi_haber saldo_final, cast(cue_clasificacion as text), cue_id2, '1969-01-01' tdi_fecha2, 1 t from (

	select b.cue_id, b.cue_id as cue_id2, b.cue_nombre, SUM(COALESCE(a.tdi_debe,0)-COALESCE(a.tdi_haber,0)) saldo_inicial, b.cue_clasificacion,  

		COALESCE((select SUM(c.tdi_debe)  
		from tbl_libro_diario c 
		where c.tdi_pai_id=2 and c.tdi_tcon_id=1 and c.tdi_moneda_id=8 
		and c.tdi_fecha>='2020-09-01' and c.tdi_fecha<='2020-09-30' 
		and c.tdi_cue_id = b.cue_id),0) tdi_debe,
		
		COALESCE((select SUM(c.tdi_haber) 
		from tbl_libro_diario c 
		where c.tdi_pai_id=2 and c.tdi_tcon_id=1 and c.tdi_moneda_id=8 
		and c.tdi_fecha>='2020-09-01' and c.tdi_fecha<='2020-09-30' 
		and c.tdi_cue_id = b.cue_id),0) tdi_haber 
		

	from tbl_libro_diario a, tbl_cuenta b
	where a.tdi_pai_id=2 and a.tdi_tcon_id=1 and a.tdi_cue_id=b.cue_id and a.tdi_moneda_id=8 
	and a.tdi_fecha>='2020-01-01' and a.tdi_fecha<'2020-09-01' 
	GROUP BY b.cue_id, b.cue_nombre, b.cue_clasificacion
	order by b.cue_id

	 -- limit 24
) x

UNION 

select '' cue_id, '' cue_nombre, '' tdi_no_partida, to_char(tdi_fecha,'dd/MM/yyyy'), saldo_inicial, tdi_debe, tdi_haber, saldo_inicial+tdi_debe-tdi_haber saldo_final, '' cue_clasificacion, cue_id2, tdi_fecha2, 2 t from (

	select b.cue_id, b.cue_id as cue_id2, b.cue_nombre, a.tdi_fecha, 
COALESCE(
	(
		select SUM(COALESCE(c.tdi_debe,0)-COALESCE(c.tdi_haber,0))   
		from tbl_libro_diario c  
		where c.tdi_pai_id=2 and c.tdi_tcon_id=1 and c.tdi_moneda_id=8 
		and c.tdi_fecha >= '2020-01-01' and c.tdi_fecha < a.tdi_fecha
		and c.tdi_cue_id = b.cue_id 
	),0) saldo_inicial, 
		
	SUM(COALESCE(a.tdi_debe,0)) tdi_debe, SUM(COALESCE(a.tdi_haber,0)) tdi_haber, 

	b.cue_clasificacion, a.tdi_fecha as tdi_fecha2 

	from tbl_libro_diario a, tbl_cuenta b
	where a.tdi_pai_id=2 and a.tdi_tcon_id=1 and a.tdi_cue_id=b.cue_id and a.tdi_moneda_id=8 
	and a.tdi_fecha>='2020-09-01' and a.tdi_fecha<='2020-09-30' 
	GROUP BY b.cue_id, b.cue_nombre, a.tdi_fecha, b.cue_clasificacion
	order by b.cue_id, a.tdi_fecha

	-- limit 100

) y

order by cue_id2, t, tdi_fecha2
");
            fechainicial = fechainicial.Replace("/", "-");
            fechafin = fechafin.Replace("/", "-");

            query = query.Replace("tdi_moneda_id=8", "tdi_moneda_id=" + monID);
            query = query.Replace("tdi_fecha>='2020-09-01'", "tdi_fecha>='" + fechainicial + "'");
            query = query.Replace("tdi_fecha<='2020-09-30'", "tdi_fecha<='" + fechafin + "'");
            query = query.Replace("tdi_tcon_id=1", "tdi_tcon_id=" + user.contaID + "");
            query = query.Replace("tdi_pai_id=2", "tdi_pai_id=" + user.PaisID + "");

            //saldo inicial
            query = query.Replace("tdi_fecha>='2020-01-01'", "tdi_fecha>='" + fechainicial.Substring(0, 4) + "-01-01'");
            query = query.Replace("tdi_fecha<'2020-09-01'", "tdi_fecha<'" + fechainicial + "'");

            comm.CommandText = query;

            reader = comm.ExecuteReader();
            int c = 0;
            while (reader.Read())
            {
                rgb = new RE_GenericBean();
                if (!reader.IsDBNull(0)) rgb.strC1 = reader.GetString(0);
                if (!reader.IsDBNull(1)) rgb.strC2 = reader.GetString(1);
                //if (!reader.IsDBNull(2)) rgb.strC3 = reader.GetString(2);
                if (!reader.IsDBNull(3)) rgb.strC4 = reader.GetString(3); //reader.GetDateTime(3).ToShortDateString();
                if (!reader.IsDBNull(4)) rgb.decC1 = decimal.Parse(reader.GetValue(4).ToString());
                if (!reader.IsDBNull(5)) rgb.decC2 = decimal.Parse(reader.GetValue(5).ToString());
                if (!reader.IsDBNull(6)) rgb.decC3 = decimal.Parse(reader.GetValue(6).ToString());
                if (!reader.IsDBNull(7)) rgb.decC4 = decimal.Parse(reader.GetValue(7).ToString());
                if (!reader.IsDBNull(8)) rgb.strC5 = reader.GetValue(8).ToString();//Cue_Clasificacion
                c++;

                td = "td";
                if (rgb.strC4 == "")
                {
                    td = "th style='border-bottom:1px solid black' ";
                }

                html += (@"
                <tr>
                    <" + td + @">" + rgb.strC1 + @"</" + td + @">
                    <" + td + @">" + rgb.strC2 + @"</" + td + @">
                    <" + td + @" align=center>" + rgb.strC4 + @"</td>
                    <" + td + @" align=right>" + rgb.decC1.ToString("#,##0.00") + @"</" + td + @">
                    <" + td + @" align=right>" + rgb.decC2.ToString("#,##0.00") + @"</" + td + @">
                    <" + td + @" align=right>" + rgb.decC3.ToString("#,##0.00") + @"</" + td + @">
                    <" + td + @" align=right>" + rgb.decC4.ToString("#,##0.00") + @"</" + td + @">
                </tr>                    
                ");

                if (rgb.strC4 != "") {
                    debe += rgb.decC2;
                    haber += rgb.decC3;                
                }

            }

        }
        catch (Exception e)
        {
            log4net ErrLog = new log4net();
            ErrLog.ErrorLog(e.Message);
            return null;
        }


        html += (@"
                <tr>
                    <td>" + @"</td>
                    <td>" + @"</td>
                    <td align=center>" + @"</td>
                    <td align=right>" + @"</td>
                    <th align=right>" + debe.ToString("#,##0.00") + @"</th>
                    <th align=right>" + haber.ToString("#,##0.00") + @"</th>
                    <td align=right>" + @"</td>
                </tr>                    
                ");

        html += "</html>";
*/
    /*
        return html;
    }
*/

    // Metodo que obtiene todo el libro mayor 2020-10-27
    public static LibroDiarioDS getlibromayorTLA(string fechainicial, string fechafin, int monID, UsuarioBean user, string consMon)
    {
        LibroDiarioDS result = new LibroDiarioDS();
        Npgsql.NpgsqlConnection conn;
        Npgsql.NpgsqlCommand comm;
        Npgsql.NpgsqlDataReader reader;
        RE_GenericBean rgb = null;
        try
        {
            DB.parametros Params = DB.EmpresaParametros(user.PaisID, "39", "BAW", "LIBRO MAYOR", "FORMATO 2"); //pais, sistema, doc_id, titulo, edicion

            if (Params.country.Substring(0, 2) == "SV") {

                string default_image = "iVBORw0KGgoAAAANSUhEUgAAABwAAAAcCAIAAAD9b0jDAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAAAnSURBVEhL7cwxAQAACIAw+5fGEnjJAmw4UOor9ZX6Sn2lvlLf6xQW8mAnWMuiKs0AAAAASUVORK5CYII=";
                Params.logo = System.Convert.FromBase64String(default_image);
                
            }


            conn = DB.OpenConnection();
            comm = new Npgsql.NpgsqlCommand();
            comm.Connection = conn;
            comm.CommandType = CommandType.Text;

            /*
             * if (user.PaisID == 1 && user.contaID == 1 && consMon == "si")
            {
                comm.CommandText = "select cue_id, cue_nombre, tdi_no_partida, tdi_fecha, cast(tdi_debe as numeric), cast(tdi_haber as numeric), cue_clasificacion from tbl_libro_diario, tbl_cuenta where tdi_pai_id=" + user.PaisID + " and tdi_tcon_id=@contaID and tdi_cue_id=cue_id and tdi_moneda_id=@monID and tdi_fecha>=@fechainicial and tdi_fecha<=@fechafin";
                comm.CommandText += " union all";
                comm.CommandText += " select cue_id, cue_nombre, tdi_no_partida, tdi_fecha, cast(tdi_debe_equivalente as numeric), cast(tdi_haber_equivalente as numeric), cue_clasificacion from tbl_libro_diario, tbl_cuenta where tdi_pai_id=" + user.PaisID + " and tdi_tcon_id=@contaID and tdi_cue_id=cue_id and tdi_moneda_id<>@monID and tdi_fecha>=@fechainicial and tdi_fecha<=@fechafin";
                comm.CommandText += " order by cue_id, tdi_no_partida";
            }
            else
            {
                comm.CommandText = "select cue_id, cue_nombre, tdi_no_partida, tdi_fecha, cast(tdi_debe as numeric), cast(tdi_haber as numeric), cue_clasificacion from tbl_libro_diario, tbl_cuenta where tdi_pai_id=" + user.PaisID + " and tdi_tcon_id=@contaID and tdi_cue_id=cue_id and tdi_moneda_id=@monID and tdi_fecha>=@fechainicial and tdi_fecha<=@fechafin order by tdi_cue_id, tdi_no_partida";
            }
            comm.Parameters.Add("@monID", NpgsqlTypes.NpgsqlDbType.Integer).Value = monID;
            comm.Parameters.Add("@fechainicial", NpgsqlTypes.NpgsqlDbType.Date).Value = string.IsNullOrEmpty(fechainicial) ? (DateTime?)null : DateTime.Parse(fechainicial);
            comm.Parameters.Add("@fechafin", NpgsqlTypes.NpgsqlDbType.Date).Value = string.IsNullOrEmpty(fechafin) ? (DateTime?)null : DateTime.Parse(fechafin);
            comm.Parameters.Add("@contaID", NpgsqlTypes.NpgsqlDbType.Integer).Value = user.contaID;
            */


            string query = (@"
select cue_id, cue_nombre, '' tdi_no_partida, '' tdi_fecha, saldo_inicial, tdi_debe, tdi_haber, saldo_inicial+tdi_debe-tdi_haber saldo_final, cast(cue_clasificacion as text), cue_id2, '1969-01-01' tdi_fecha2, 1 t from (

	select b.cue_id, b.cue_id as cue_id2, b.cue_nombre, SUM(COALESCE(a.tdi_debe,0)-COALESCE(a.tdi_haber,0)) saldo_inicial, b.cue_clasificacion,  

		COALESCE((select SUM(c.tdi_debe)  
		from tbl_libro_diario c 
		where c.tdi_pai_id=2 and c.tdi_tcon_id=1 and c.tdi_moneda_id=8 
		and c.tdi_fecha>='2020-09-01' and c.tdi_fecha<='2020-09-30' 
		and c.tdi_cue_id = b.cue_id),0) tdi_debe,
		
		COALESCE((select SUM(c.tdi_haber) 
		from tbl_libro_diario c 
		where c.tdi_pai_id=2 and c.tdi_tcon_id=1 and c.tdi_moneda_id=8 
		and c.tdi_fecha>='2020-09-01' and c.tdi_fecha<='2020-09-30' 
		and c.tdi_cue_id = b.cue_id),0) tdi_haber 
		

	from tbl_libro_diario a, tbl_cuenta b
	where a.tdi_pai_id=2 and a.tdi_tcon_id=1 and a.tdi_cue_id=b.cue_id and a.tdi_moneda_id=8 
	and a.tdi_fecha>='2020-01-01' and a.tdi_fecha<'2020-09-01' 
	GROUP BY b.cue_id, b.cue_nombre, b.cue_clasificacion
	order by b.cue_id

	-- limit 24
) x

UNION 

select cue_id, cue_nombre, '' tdi_no_partida, to_char(tdi_fecha,'dd/MM/yyyy'), saldo_inicial, tdi_debe, tdi_haber, saldo_inicial+tdi_debe-tdi_haber saldo_final, '' cue_clasificacion, cue_id2, tdi_fecha2, 2 t from (

	select b.cue_id, b.cue_id as cue_id2, b.cue_nombre, a.tdi_fecha, 
COALESCE(
	(
		select SUM(COALESCE(c.tdi_debe,0)-COALESCE(c.tdi_haber,0))   
		from tbl_libro_diario c  
		where c.tdi_pai_id=2 and c.tdi_tcon_id=1 and c.tdi_moneda_id=8 
		and c.tdi_fecha >= '2020-01-01' and c.tdi_fecha < a.tdi_fecha
		and c.tdi_cue_id = b.cue_id 
	),0) saldo_inicial, 
		
	SUM(COALESCE(a.tdi_debe,0)) tdi_debe, SUM(COALESCE(a.tdi_haber,0)) tdi_haber, 

	b.cue_clasificacion, a.tdi_fecha as tdi_fecha2 

	from tbl_libro_diario a, tbl_cuenta b
	where a.tdi_pai_id=2 and a.tdi_tcon_id=1 and a.tdi_cue_id=b.cue_id and a.tdi_moneda_id=8 
	and a.tdi_fecha>='2020-09-01' and a.tdi_fecha<='2020-09-30' 
	GROUP BY b.cue_id, b.cue_nombre, a.tdi_fecha, b.cue_clasificacion
	order by b.cue_id, a.tdi_fecha

	-- limit 100

) y

  order by cue_id2, t, tdi_fecha2
");
            fechainicial = fechainicial.Replace("/", "-");
            fechafin = fechafin.Replace("/", "-");

            query = query.Replace("tdi_moneda_id=8", "tdi_moneda_id=" + monID);
            query = query.Replace("tdi_fecha>='2020-09-01'", "tdi_fecha>='" + fechainicial + "'");
            query = query.Replace("tdi_fecha<='2020-09-30'", "tdi_fecha<='" + fechafin + "'");
            query = query.Replace("tdi_tcon_id=1", "tdi_tcon_id=" + user.contaID + "");
            query = query.Replace("tdi_pai_id=2", "tdi_pai_id=" + user.PaisID + "");

            //saldo inicial
            query = query.Replace("tdi_fecha>='2020-01-01'", "tdi_fecha>='" + fechainicial.Substring(0, 4) + "-01-01'");
            query = query.Replace("tdi_fecha<'2020-09-01'", "tdi_fecha<'" + fechainicial + "'");

            comm.CommandText = query;

            reader = comm.ExecuteReader();
            int c = 0;
            while (reader.Read())
            {
                rgb = new RE_GenericBean();
                if (!reader.IsDBNull(0)) rgb.strC1 = reader.GetString(0);
                if (!reader.IsDBNull(1)) rgb.strC2 = reader.GetString(1);
                //if (!reader.IsDBNull(2)) rgb.strC3 = reader.GetString(2);
                if (!reader.IsDBNull(3)) rgb.strC4 = reader.GetString(3); //reader.GetDateTime(3).ToShortDateString();
                if (!reader.IsDBNull(4)) rgb.decC1 = decimal.Parse(reader.GetValue(4).ToString());
                if (!reader.IsDBNull(5)) rgb.decC2 = decimal.Parse(reader.GetValue(5).ToString());
                if (!reader.IsDBNull(6)) rgb.decC3 = decimal.Parse(reader.GetValue(6).ToString());
                if (!reader.IsDBNull(7)) rgb.decC4 = decimal.Parse(reader.GetValue(7).ToString());
                if (!reader.IsDBNull(8)) rgb.strC5 = reader.GetValue(8).ToString();//Cue_Clasificacion
                c++;
                if (c == 1)
                {
                    object[] objArr = { rgb.strC1, rgb.strC2, "", rgb.strC4, rgb.decC2.ToString(), rgb.decC3.ToString(), rgb.strC5, Params.logo, Params.titulo, Params.edicion, rgb.decC1.ToString(), rgb.decC4.ToString(), Params.nombre_empresa, Params.direccion, Params.nit };
                    result.Tables["Libromayor_tbl"].Rows.Add(objArr);
                }
                else
                {
                    object[] objArr = { rgb.strC1, rgb.strC2, "", rgb.strC4, rgb.decC2.ToString(), rgb.decC3.ToString(), rgb.strC5, null, "", "", rgb.decC1.ToString(), rgb.decC4.ToString(), "", "", "" };
                    result.Tables["Libromayor_tbl"].Rows.Add(objArr);
                }
            }

            if (c == 0)
            {
                object[] objArr = { "", "", "", "", 0, 0, "", Params.logo, Params.titulo, Params.edicion, 0, 0, Params.nombre_empresa, Params.direccion, Params.nit };
                result.Tables["Libromayor_tbl"].Rows.Add(objArr);
            }

        }
        catch (Exception e)
        {
            log4net ErrLog = new log4net();
            ErrLog.ErrorLog(e.Message);
            return null;
        }
        return result;
    }






    // Metodo que obtiene todo el libro mayor 2020-10-27
    public static string getlibromayorTLAhtml(string fechainicial, string fechafin, int monID, UsuarioBean user, string consMon)
    {

        Npgsql.NpgsqlConnection conn;
        Npgsql.NpgsqlCommand comm;
        Npgsql.NpgsqlDataReader reader;
        RE_GenericBean rgb = null;
        string html = "", conta_Str = "", td = "", moneda_str = "";
        Decimal debe = 0, haber = 0;

        DateTime date1 = Convert.ToDateTime(fechainicial);

        DateTime date2 = Convert.ToDateTime(fechafin);

        moneda_str = Utility.TraducirMonedaInt(monID);

        /*var arr = (ArrayList)DB.getMonedasbyCriterio(" and 	ttm_id=" + monID + " LIMIT 1 ");
        foreach (RE_GenericBean rgb2 in arr)
        {
            moneda_str = rgb2.strC2.Substring(0, 3);
        }*/


        switch (user.contaID)
        {
            case 1:
                conta_Str = "FISCAL";
                break;
            case 2:
                conta_Str = "FINANCIERA";
                break;
            case 3:
                conta_Str = "CONSOLIDADO";
                break;
        }

        try
        {
            DB.parametros Params = DB.EmpresaParametros(user.PaisID, "39", "BAW", "LIBRO MAYOR", "FORMATO 2"); //pais, sistema, doc_id, titulo, edicion

            /* solicitaron quitar los logos
            string filename = "LibroMayor_" + user.PaisID + ".jpg";

            Byte[] bytes = Params.logo;
            string base64String = Convert.ToBase64String(bytes, 0, bytes.Length);

            System.IO.Stream stream = new System.IO.MemoryStream(Params.logo);

            System.Drawing.Image imgPhoto = System.Drawing.Image.FromStream(stream);

            //imgPhoto.Save(filename);

            //uploadlogo(filename, base64String);

            string json = @"<?xml version = ""1.0""?><root><row NUM = ""1""><name>" + filename + @"</name><file><![CDATA[" + base64String + @"]]></file></row></root>";

            SendParametros.SendParametros t = new SendParametros.SendParametros();

            var res = t.Upload_Files("C://inetpub//wwwroot//site//logos//BAW//", "", "", "", json, false);

            filename = "http" + "://www.aimargroup.com/logos/BAW/" + filename;

            string imagepath = string.Format("<img src='{0}' width='{1}' height='{2}'/>", filename, imgPhoto.Width, imgPhoto.Height);
            */
            string imagepath = "";

            html = (@"
<table width=100% border=0>
<tr>
    <td rowspan=2>" + imagepath + @"</td>
    <td colspan=1><h2>" + Params.nombre_empresa + @"</h2></td>  
    <td colspan=3 align=center><h2>" + Params.titulo + @"</h2></td>
    <td colspan=2 align=left>" + Params.edicion + @"</td>
</tr>
<tr>
    <td colspan=4><h2>" + Params.direccion + @"</h2></td>  
    <td colspan=2 align=left>NIT:&nbsp;" + Params.nit + @"</td>
</tr>
<tr>
  <td>Pais : </td><td>" + user.pais.Nombre + @"</td><td>Contabilidad : " + conta_Str + @"</td><td>Moneda : " + monID + " - " + moneda_str + @"</td>
</tr>
<tr>
   <td>Fecha Inicial:</td><td>" + date1.ToString("dd/M/yyyy") + @"</td><td>Fecha Final : " + date2.ToString("dd/M/yyyy") + @"</td>
</tr>
            ");

            html += (@"
                <tr>
                    <th style='border-bottom:1px solid black'>No.  Cuenta</th>
                    <th style='border-bottom:1px solid black'>Nombre Cuenta</th>
                    <th style='border-bottom:1px solid black'>Fecha</th>
                    <th style='border-bottom:1px solid black'>Saldo Inicial</th>
                    <th style='border-bottom:1px solid black'>Debe</th>
                    <th style='border-bottom:1px solid black'>Haber</th>
                    <th style='border-bottom:1px solid black'>Saldo Final</th>
                </tr>                    
                ");

            conn = DB.OpenConnection();
            comm = new Npgsql.NpgsqlCommand();
            comm.Connection = conn;
            comm.CommandType = CommandType.Text;



            string query = (@"
select cue_id, cue_nombre, '' tdi_no_partida, '' tdi_fecha, saldo_inicial, tdi_debe, tdi_haber, saldo_inicial+tdi_debe-tdi_haber saldo_final, cast(cue_clasificacion as text), cue_id2, '1969-01-01' tdi_fecha2, 1 t from (

	select b.cue_id, b.cue_id as cue_id2, b.cue_nombre, SUM(COALESCE(a.tdi_debe,0)-COALESCE(a.tdi_haber,0)) saldo_inicial, b.cue_clasificacion,  

		COALESCE((select SUM(c.tdi_debe)  
		from tbl_libro_diario c 
		where c.tdi_pai_id=2 and c.tdi_tcon_id=1 and c.tdi_moneda_id=8 
		and c.tdi_fecha>='2020-09-01' and c.tdi_fecha<='2020-09-30' 
		and c.tdi_cue_id = b.cue_id),0) tdi_debe,
		
		COALESCE((select SUM(c.tdi_haber) 
		from tbl_libro_diario c 
		where c.tdi_pai_id=2 and c.tdi_tcon_id=1 and c.tdi_moneda_id=8 
		and c.tdi_fecha>='2020-09-01' and c.tdi_fecha<='2020-09-30' 
		and c.tdi_cue_id = b.cue_id),0) tdi_haber 
		

	from tbl_libro_diario a, tbl_cuenta b
	where a.tdi_pai_id=2 and a.tdi_tcon_id=1 and a.tdi_cue_id=b.cue_id and a.tdi_moneda_id=8 
	and a.tdi_fecha>='2020-01-01' and a.tdi_fecha<'2020-09-01' 
	GROUP BY b.cue_id, b.cue_nombre, b.cue_clasificacion
	order by b.cue_id

	 -- limit 24
) x

UNION 

select '' cue_id, '' cue_nombre, '' tdi_no_partida, to_char(tdi_fecha,'dd/MM/yyyy'), saldo_inicial, tdi_debe, tdi_haber, saldo_inicial+tdi_debe-tdi_haber saldo_final, '' cue_clasificacion, cue_id2, tdi_fecha2, 2 t from (

	select b.cue_id, b.cue_id as cue_id2, b.cue_nombre, a.tdi_fecha, 
COALESCE(
	(
		select SUM(COALESCE(c.tdi_debe,0)-COALESCE(c.tdi_haber,0))   
		from tbl_libro_diario c  
		where c.tdi_pai_id=2 and c.tdi_tcon_id=1 and c.tdi_moneda_id=8 
		and c.tdi_fecha >= '2020-01-01' and c.tdi_fecha < a.tdi_fecha
		and c.tdi_cue_id = b.cue_id 
	),0) saldo_inicial, 
		
	SUM(COALESCE(a.tdi_debe,0)) tdi_debe, SUM(COALESCE(a.tdi_haber,0)) tdi_haber, 

	b.cue_clasificacion, a.tdi_fecha as tdi_fecha2 

	from tbl_libro_diario a, tbl_cuenta b
	where a.tdi_pai_id=2 and a.tdi_tcon_id=1 and a.tdi_cue_id=b.cue_id and a.tdi_moneda_id=8 
	and a.tdi_fecha>='2020-09-01' and a.tdi_fecha<='2020-09-30' 
	GROUP BY b.cue_id, b.cue_nombre, a.tdi_fecha, b.cue_clasificacion
	order by b.cue_id, a.tdi_fecha

	-- limit 100

) y

order by cue_id2, t, tdi_fecha2
");
            fechainicial = fechainicial.Replace("/", "-");
            fechafin = fechafin.Replace("/", "-");

            query = query.Replace("tdi_moneda_id=8", "tdi_moneda_id=" + monID);
            query = query.Replace("tdi_fecha>='2020-09-01'", "tdi_fecha>='" + fechainicial + "'");
            query = query.Replace("tdi_fecha<='2020-09-30'", "tdi_fecha<='" + fechafin + "'");
            query = query.Replace("tdi_tcon_id=1", "tdi_tcon_id=" + user.contaID + "");
            query = query.Replace("tdi_pai_id=2", "tdi_pai_id=" + user.PaisID + "");

            //saldo inicial
            query = query.Replace("tdi_fecha>='2020-01-01'", "tdi_fecha>='" + fechainicial.Substring(0, 4) + "-01-01'");
            query = query.Replace("tdi_fecha<'2020-09-01'", "tdi_fecha<'" + fechainicial + "'");

            comm.CommandText = query;

            reader = comm.ExecuteReader();
            int c = 0;
            while (reader.Read())
            {
                rgb = new RE_GenericBean();
                if (!reader.IsDBNull(0)) rgb.strC1 = reader.GetString(0);
                if (!reader.IsDBNull(1)) rgb.strC2 = reader.GetString(1);
                //if (!reader.IsDBNull(2)) rgb.strC3 = reader.GetString(2);
                if (!reader.IsDBNull(3)) rgb.strC4 = reader.GetString(3); //reader.GetDateTime(3).ToShortDateString();
                if (!reader.IsDBNull(4)) rgb.decC1 = decimal.Parse(reader.GetValue(4).ToString());
                if (!reader.IsDBNull(5)) rgb.decC2 = decimal.Parse(reader.GetValue(5).ToString());
                if (!reader.IsDBNull(6)) rgb.decC3 = decimal.Parse(reader.GetValue(6).ToString());
                if (!reader.IsDBNull(7)) rgb.decC4 = decimal.Parse(reader.GetValue(7).ToString());
                if (!reader.IsDBNull(8)) rgb.strC5 = reader.GetValue(8).ToString();//Cue_Clasificacion
                c++;

                td = "td";
                if (rgb.strC4 == "")
                {
                    td = "th style='border-bottom:1px solid black' ";
                }

                html += (@"
                <tr>
                    <" + td + @">" + rgb.strC1 + @"</" + td + @">
                    <" + td + @">" + rgb.strC2 + @"</" + td + @">
                    <" + td + @" align=center>" + rgb.strC4 + @"</td>
                    <" + td + @" align=right>" + rgb.decC1.ToString("#,##0.00") + @"</" + td + @">
                    <" + td + @" align=right>" + rgb.decC2.ToString("#,##0.00") + @"</" + td + @">
                    <" + td + @" align=right>" + rgb.decC3.ToString("#,##0.00") + @"</" + td + @">
                    <" + td + @" align=right>" + rgb.decC4.ToString("#,##0.00") + @"</" + td + @">
                </tr>                    
                ");

                if (rgb.strC4 != "")
                {
                    debe += rgb.decC2;
                    haber += rgb.decC3;
                }

            }

        }
        catch (Exception e)
        {
            log4net ErrLog = new log4net();
            ErrLog.ErrorLog(e.Message);
            return null;
        }


        html += (@"
                <tr>
                    <td>" + @"</td>
                    <td>" + @"</td>
                    <td align=center>" + @"</td>
                    <td align=right>" + @"</td>
                    <th align=right>" + debe.ToString("#,##0.00") + @"</th>
                    <th align=right>" + haber.ToString("#,##0.00") + @"</th>
                    <td align=right>" + @"</td>
                </tr>                    
                ");

        html += "</html>";

        return html;
    }





    protected void reporte_profit(UsuarioBean user) {
        if (!Page.IsPostBack)
        {
            string HBL = Request.QueryString["HBL"].ToString();
            string MBL = Request.QueryString["MBL"].ToString();
            string ROUTING = Request.QueryString["ROUTING"].ToString();
            string CONTENEDOR = Request.QueryString["CONTENEDOR"].ToString();
            string fechaini = Request.QueryString["FechaIni"].ToString();
            string fechafin = Request.QueryString["FechaFin"].ToString();
            string tipopersona = Request.QueryString["tipopersona"].ToString();
            string codigopersona = Request.QueryString["codigopersona"].ToString();
            int monID = int.Parse(Request.QueryString["monID"].ToString());
            int tipo_conta = int.Parse(Request.QueryString["tipo_conta"].ToString());
            int i = 0;

            DB.parametros Params = DB.EmpresaParametros(user.PaisID, "", "", "PROFIT DETALLADO", ""); //pais, sistema, doc_id, titulo, edicion

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
            ArrayList Profit_Array = (ArrayList)DB.getDatos_For_Profit(user.pais.schema, MBL, HBL, ROUTING, CONTENEDOR);
            if (Profit_Array != null)
            {
                foreach (RE_GenericBean Bean in Profit_Array)
                {
                    AGENTE = Bean.strC5;
                    DESTINO = Bean.strC6;
                }
            }
            string where = " and tfa_ted_id<>3 ";
            if (!MBL.Trim().Equals("")) where += " and tfa_mbl='" + MBL + "'";
            if (!HBL.Trim().Equals("")) where += " and tfa_hbl='" + HBL + "'";
            if (!ROUTING.Trim().Equals("") && !ROUTING.Equals("&nbsp;")) where += " and tfa_routing='" + ROUTING + "'";
            if (!CONTENEDOR.Trim().Equals("")) where += " and tfa_contenedor='" + CONTENEDOR + "'";
            if (!fechaini.Trim().Equals("") && !fechafin.Equals("")) where += " and tfa_fecha_emision between '" + fechaini + " 00:00:00' and '" + fechafin + " 23:59:59'";
            if (!tipopersona.Trim().Equals("") && tipopersona.Equals("3") && !codigopersona.Equals("")) where += " and tfa_cli_id=" + codigopersona;
            if (!tipopersona.Trim().Equals("") && tipopersona.Equals("2") && !codigopersona.Equals("")) where += " and tfa_agent_id=" + codigopersona;
            where += " and tfa_moneda="+monID;
            where += " and tfa_pai_id=" + user.PaisID;
            //---------------------
            string where_1 = " and tfa_ted_id<>3 ";
            if (!MBL.Trim().Equals("")) where_1 += " and tfa_mbl='" + MBL + "'";
            if (!HBL.Trim().Equals("")) where_1 += " and tfa_hbl='" + HBL + "'";
            if (!ROUTING.Trim().Equals("") && !ROUTING.Equals("&nbsp;")) where_1 += " and tfa_routing='" + ROUTING + "'";
            if (!CONTENEDOR.Trim().Equals("")) where_1 += " and tfa_contenedor='" + CONTENEDOR + "'";
            if (!fechaini.Trim().Equals("") && !fechafin.Equals("")) where_1 += " and tfa_fecha_emision between '" + fechaini + " 00:00:00' and '" + fechafin + " 23:59:59'";
            if (!tipopersona.Trim().Equals("") && tipopersona.Equals("3") && !codigopersona.Equals("")) where_1 += " and tfa_cli_id=" + codigopersona;
            if (!tipopersona.Trim().Equals("") && tipopersona.Equals("2") && !codigopersona.Equals("")) where_1 += " and tfa_agent_id=" + codigopersona;
            where_1 += " and tfa_moneda<>" + monID;
            where_1 += " and tfa_pai_id=" + user.PaisID;
            ArrayList temp = null;
            //facturas
            temp = (ArrayList)DB.getFacturasForProfit(where,where_1,tipo_conta, user);//obtengo a partir de la referencia
            foreach (RE_GenericBean rgb in temp)
            {
                /*if (rgb.strC12.Equals("Ingreso"))
                {
                    object[] objArr = { rgb.strC1, rgb.strC2, rgb.strC3, rgb.strC4, rgb.strC5, rgb.strC6, rgb.strC7, rgb.strC8, rgb.strC9, Utility.TraducirServiciotoSTR(rgb.intC1), rgb.strC11, rgb.decC1, 0,rgb.strC13 };
                    ds.Tables["profit_data_tbl1"].Rows.Add(objArr);
                }
                else
                {
                    object[] objArr = { rgb.strC1, rgb.strC2, rgb.strC3, rgb.strC4, rgb.strC5, rgb.strC6, rgb.strC7, rgb.strC8, rgb.strC9, Utility.TraducirServiciotoSTR(rgb.intC1), rgb.strC11, 0, rgb.decC1,rgb.strC13 };
                    ds.Tables["profit_data_tbl1"].Rows.Add(objArr);
                }*/


                i++;
                object[] objArr = { rgb.strC1, rgb.strC2, rgb.strC3, rgb.strC4, rgb.strC5, rgb.strC6, rgb.strC7, rgb.strC8, rgb.strC9, Utility.TraducirServiciotoSTR(rgb.intC1), rgb.strC11, rgb.strC12.Equals("Ingreso") ? rgb.decC1 : 0, rgb.strC12.Equals("Ingreso") ? 0 : rgb.decC1, rgb.strC13, i == 1 ? Params.logo : null, i == 1 ? Params.titulo : "", i == 1 ? Params.edicion : "" };
                ds.Tables["profit_data_tbl1"].Rows.Add(objArr);

            }
            //Notas Credito
            where = " and tnc_ted_id<>3 ";
            if (!MBL.Trim().Equals("")) where += " and tnc_mbl='" + MBL + "'";
            if (!HBL.Trim().Equals("")) where += " and tnc_hbl='" + HBL + "'";
            if (!ROUTING.Trim().Equals("")) where += " and tnc_routing='" + ROUTING + "'";
            if (!CONTENEDOR.Trim().Equals("")) where += " and tnc_contenedor='" + CONTENEDOR + "'";
            if (!fechaini.Trim().Equals("") && !fechafin.Equals("")) where += " and tnc_fecha between '" + fechaini + " 00:00:00' and '" + fechafin + " 23:59:59'";
            if (!tipopersona.Trim().Equals("") && !codigopersona.Equals("")) where += " and tnc_cli_id=" + codigopersona + " and tnc_tpi_id=" + tipopersona;
            where += " and tnc_mon_id=" + monID;
            where += "and tnc_pai_id=" + user.PaisID;
            //-----
            where_1 = " and tnc_ted_id<>3 ";
            if (!MBL.Trim().Equals("")) where_1 += " and tnc_mbl='" + MBL + "'";
            if (!HBL.Trim().Equals("")) where_1 += " and tnc_hbl='" + HBL + "'";
            if (!ROUTING.Trim().Equals("")) where_1 += " and tnc_routing='" + ROUTING + "'";
            if (!CONTENEDOR.Trim().Equals("")) where_1 += " and tnc_contenedor='" + CONTENEDOR + "'";
            if (!fechaini.Trim().Equals("") && !fechafin.Equals("")) where_1 += " and tnc_fecha between '" + fechaini + " 00:00:00' and '" + fechafin + " 23:59:59'";
            if (!tipopersona.Trim().Equals("") && !codigopersona.Equals("")) where_1 += " and tnc_cli_id=" + codigopersona + " and tnc_tpi_id=" + tipopersona;
            where_1 += " and tnc_mon_id<>" + monID;
            where_1 += "and tnc_pai_id=" + user.PaisID;
            temp = (ArrayList)DB.getNCForProfit(where,where_1,tipo_conta,user);//obtengo a partir de la referencia
            foreach (RE_GenericBean rgb in temp)
            {
                /*if (rgb.strC12.Equals("Ingreso"))
                {
                    object[] objArr = { rgb.strC1, rgb.strC2, rgb.strC3, rgb.strC4, rgb.strC5, rgb.strC6, rgb.strC7, rgb.strC8, rgb.strC9, Utility.TraducirServiciotoSTR(rgb.intC1), rgb.strC11, rgb.decC1, 0,rgb.strC13 };
                    ds.Tables["profit_data_tbl1"].Rows.Add(objArr);
                }
                else
                {
                    object[] objArr = { rgb.strC1, rgb.strC2, rgb.strC3, rgb.strC4, rgb.strC5, rgb.strC6, rgb.strC7, rgb.strC8, rgb.strC9, Utility.TraducirServiciotoSTR(rgb.intC1), rgb.strC11, 0, rgb.decC1, rgb.strC13 };
                    ds.Tables["profit_data_tbl1"].Rows.Add(objArr);
                }*/

                i++;
                object[] objArr = { rgb.strC1, rgb.strC2, rgb.strC3, rgb.strC4, rgb.strC5, rgb.strC6, rgb.strC7, rgb.strC8, rgb.strC9, Utility.TraducirServiciotoSTR(rgb.intC1), rgb.strC11, rgb.strC12.Equals("Ingreso") ? rgb.decC1 : 0, rgb.strC12.Equals("Ingreso") ? 0 : rgb.decC1, rgb.strC13, i == 1 ? Params.logo : null, i == 1 ? Params.titulo : "", i == 1 ? Params.edicion : "" };
                ds.Tables["profit_data_tbl1"].Rows.Add(objArr);
            }

            //Notas Debito
            where = " and tnd_ted_id<>3 ";
            if (!MBL.Trim().Equals("")) where += " and tnd_mbl='" + MBL + "'";
            if (!HBL.Trim().Equals("")) where += " and tnd_hbl='" + HBL + "'";
            if (!ROUTING.Trim().Equals("")) where += " and tnd_routing='" + ROUTING + "'";
            if (!CONTENEDOR.Trim().Equals("")) where += " and tnd_contenedor='" + CONTENEDOR + "'";
            if (!fechaini.Trim().Equals("") && !fechafin.Equals("")) where += " and tnd_fecha_emision between '" + fechaini + " 00:00:00' and '" + fechafin + " 23:59:59'";
            if (!tipopersona.Trim().Equals("") && !codigopersona.Equals("")) where += " and tnd_cli_id=" + codigopersona + " and tnd_tpi_id=" + tipopersona;
            where += " and tnd_moneda=" + monID;
            where += " and tnd_pai_id=" + user.PaisID;
            //--------------
            where_1 = " and tnd_ted_id<>3 ";
            if (!MBL.Trim().Equals("")) where_1 += " and tnd_mbl='" + MBL + "'";
            if (!HBL.Trim().Equals("")) where_1 += " and tnd_hbl='" + HBL + "'";
            if (!ROUTING.Trim().Equals("")) where_1 += " and tnd_routing='" + ROUTING + "'";
            if (!CONTENEDOR.Trim().Equals("")) where_1 += " and tnd_contenedor='" + CONTENEDOR + "'";
            if (!fechaini.Trim().Equals("") && !fechafin.Equals("")) where_1 += " and tnd_fecha_emision between '" + fechaini + " 00:00:00' and '" + fechafin + " 23:59:59'";
            if (!tipopersona.Trim().Equals("") && !codigopersona.Equals("")) where_1 += " and tnd_cli_id=" + codigopersona + " and tnd_tpi_id=" + tipopersona;
            where_1 += " and tnd_moneda<>" + monID;
            where_1 += " and tnd_pai_id=" + user.PaisID;

            temp = (ArrayList)DB.getNDForProfit(where,where_1,tipo_conta,user);//obtengo a partir de la referencia
            foreach (RE_GenericBean rgb in temp)
            {
                /*if (rgb.strC12.Equals("Ingreso"))
                {
                    object[] objArr = { rgb.strC1, rgb.strC2, rgb.strC3, rgb.strC4, rgb.strC5, rgb.strC6, rgb.strC7, rgb.strC8, rgb.strC9, Utility.TraducirServiciotoSTR(rgb.intC1), rgb.strC11, rgb.decC1, 0,rgb.strC13 };
                    ds.Tables["profit_data_tbl1"].Rows.Add(objArr);
                }
                else
                {
                    object[] objArr = { rgb.strC1, rgb.strC2, rgb.strC3, rgb.strC4, rgb.strC5, rgb.strC6, rgb.strC7, rgb.strC8, rgb.strC9, Utility.TraducirServiciotoSTR(rgb.intC1), rgb.strC11, 0, rgb.decC1, rgb.strC13 };
                    ds.Tables["profit_data_tbl1"].Rows.Add(objArr);
                }*/

                i++;
                object[] objArr = { rgb.strC1, rgb.strC2, rgb.strC3, rgb.strC4, rgb.strC5, rgb.strC6, rgb.strC7, rgb.strC8, rgb.strC9, Utility.TraducirServiciotoSTR(rgb.intC1), rgb.strC11, rgb.strC12.Equals("Ingreso") ? rgb.decC1 : 0, rgb.strC12.Equals("Ingreso") ? 0 : rgb.decC1, rgb.strC13, i == 1 ? Params.logo : null, i == 1 ? Params.titulo : "", i == 1 ? Params.edicion : "" };
                ds.Tables["profit_data_tbl1"].Rows.Add(objArr);

            }

            //Provisiones
            where = " and tpr_ted_id<>3 ";
            if (!MBL.Trim().Equals("")) where += " and tpr_mbl='" + MBL + "'";
            if (!HBL.Trim().Equals("")) where += " and tpr_hbl='" + HBL + "'";
            if (!ROUTING.Trim().Equals("")) where += " and tpr_routing='" + ROUTING + "'";
            if (!CONTENEDOR.Trim().Equals("")) where += " and tpr_contenedor='" + CONTENEDOR + "'";
            if (!fechaini.Trim().Equals("") && !fechafin.Equals("")) where += " and tpr_fecha_emision between '" + fechaini + " 00:00:00' and '" + fechafin + " 23:59:59'";
            if (!tipopersona.Trim().Equals("") && !codigopersona.Equals("")) where += " and tpr_proveedor_id=" + codigopersona + " and tpr_tpi_id=" + tipopersona;
            where += " and tpr_mon_id=" + monID;
            where += " and tpr_pai_id=" + user.PaisID;
            //-------------------------------
            where_1 = " and tpr_ted_id<>3 ";
            if (!MBL.Trim().Equals("")) where_1 += " and tpr_mbl='" + MBL + "'";
            if (!HBL.Trim().Equals("")) where_1 += " and tpr_hbl='" + HBL + "'";
            if (!ROUTING.Trim().Equals("")) where_1 += " and tpr_routing='" + ROUTING + "'";
            if (!CONTENEDOR.Trim().Equals("")) where_1 += " and tpr_contenedor='" + CONTENEDOR + "'";
            if (!fechaini.Trim().Equals("") && !fechafin.Equals("")) where_1 += " and tpr_fecha_emision between '" + fechaini + " 00:00:00' and '" + fechafin + " 23:59:59'";
            if (!tipopersona.Trim().Equals("") && !codigopersona.Equals("")) where_1 += " and tpr_proveedor_id=" + codigopersona + " and tpr_tpi_id=" + tipopersona;
            where_1 += " and tpr_mon_id<>" + monID;
            where_1 += " and tpr_pai_id=" + user.PaisID;

            temp = (ArrayList)DB.getProvisionesForProfit(where,where_1,tipo_conta,user);//obtengo a partir de la referencia
            foreach (RE_GenericBean rgb in temp)
            {
                /*if (rgb.strC12.Equals("Ingreso"))
                {
                    object[] objArr = { rgb.strC1, rgb.strC2, rgb.strC3, rgb.strC4, rgb.strC5, rgb.strC6, rgb.strC7, rgb.strC8, rgb.strC9, Utility.TraducirServiciotoSTR(rgb.intC1), rgb.strC11, rgb.decC1, 0,rgb.strC13 };
                    ds.Tables["profit_data_tbl1"].Rows.Add(objArr);
                }
                else
                {
                    object[] objArr = { rgb.strC1, rgb.strC2, rgb.strC3, rgb.strC4, rgb.strC5, rgb.strC6, rgb.strC7, rgb.strC8, rgb.strC9, Utility.TraducirServiciotoSTR(rgb.intC1), rgb.strC11, 0, rgb.decC1, rgb.strC13 };
                    ds.Tables["profit_data_tbl1"].Rows.Add(objArr);
                }*/
                i++;
                object[] objArr = { rgb.strC1, rgb.strC2, rgb.strC3, rgb.strC4, rgb.strC5, rgb.strC6, rgb.strC7, rgb.strC8, rgb.strC9, Utility.TraducirServiciotoSTR(rgb.intC1), rgb.strC11, rgb.strC12.Equals("Ingreso") ? rgb.decC1 : 0, rgb.strC12.Equals("Ingreso") ? 0 : rgb.decC1, rgb.strC13, i == 1 ? Params.logo : null, i == 1 ? Params.titulo : "", i == 1 ? Params.edicion : "" };
                ds.Tables["profit_data_tbl1"].Rows.Add(objArr);
            }


            if (i == 0)
            {
                object[] objArr = { "", "", "", "", "", "", "", "", "", "", "", 0, 0, Params.logo, Params.titulo, Params.edicion };
                ds.Tables["profit_data_tbl1"].Rows.Add(objArr);
            }            
            
            ViewState["ds"] = ds;
            ViewState["Agente"] = AGENTE;
            ViewState["Destino"] = DESTINO;
        }
        ds = (LibroDiarioDS)ViewState["ds"];
        AGENTE = ViewState["Agente"].ToString();
        DESTINO = ViewState["Destino"].ToString();
        rpt = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
        rpt.Load(Server.MapPath("~/CR_profit2.rpt"));
        rpt.SetDataSource(ds.Tables["profit_data_tbl1"]);
        rpt.SetParameterValue("Agente", AGENTE);
        rpt.SetParameterValue("Destino", DESTINO);
        rpt.SetParameterValue(2, Server.MapPath(user.pais.Imagepath));
        //CrystalReportViewer1.DisplayGroupTree = false;
        CrystalReportViewer1.ReportSource = rpt;
        CrystalReportViewer1.DataBind();
    }
    protected void reporte_profit_resumido(UsuarioBean user)
    {
        string MBL = Request.QueryString["MBL"].ToString();
       // string CONTENEDOR = Request.QueryString["CONTENEDOR"].ToString();
        string fechaini = Request.QueryString["FechaIni"].ToString();
        string fechafin = Request.QueryString["FechaFin"].ToString();
        string tipopersona = Request.QueryString["tipopersona"].ToString();
        string codigopersona = Request.QueryString["codigopersona"].ToString();
        int monID = int.Parse(Request.QueryString["monID"].ToString());
        int tipoconta = int.Parse(Request.QueryString["TipoConta"].ToString());
        RE_GenericBean nombre  = null;
        string ID = "";
        int i = 0;
        if (!Page.IsPostBack)
        {
            DB.parametros Params = DB.EmpresaParametros(user.PaisID, "", "", "PROFIT RESUMIDO", ""); //pais, sistema, doc_id, titulo, edicion

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
            ArrayList Profit_Array = (ArrayList)DB.GetprofitResumido(user, fechaini, fechafin, user.PaisID, monID, tipoconta, user.pais.ISO,tipopersona,codigopersona,MBL);
            foreach (RE_GenericBean rgb in Profit_Array)
            {
                ID = rgb.strC3;
                nombre = new RE_GenericBean();
                if ((tipopersona == "2") && (ID != ""))
                {
                    nombre = DB.getAgenteData(int.Parse(ID), "REPORTES");
                }
                if ((tipopersona == "5") && (ID != ""))
                {
                    nombre = DB.getNavieraData(int.Parse(ID));
                }
                if (codigopersona != "")
                {
                    if (ID != "")
                    {
                        //object[] objArr = { rgb.strC1, nombre.strC1, rgb.decC1, rgb.decC2, rgb.strC2,rgb.strC4 };
                        object[] objArr = { rgb.strC1, nombre.strC1, rgb.decC1, rgb.decC2, rgb.strC2, rgb.strC4, i == 1 ? Params.logo : null, i == 1 ? Params.titulo : "", i == 1 ? Params.edicion : "" };
                        ds.Tables["profit_resumido"].Rows.Add(objArr);
                    }
                }
                else
                {                    
                    i++;
                    object[] objArr = { rgb.strC1, nombre.strC1, rgb.decC1, rgb.decC2, rgb.strC2, rgb.strC4, i == 1 ? Params.logo : null, i == 1 ? Params.titulo : "", i == 1 ? Params.edicion : "" };
                    ds.Tables["profit_resumido"].Rows.Add(objArr);
                }
                
            }

            if (i == 0)
            {
                object[] objArr = { "", "", 0, 0, "", "", Params.logo, Params.titulo, Params.edicion };
                ds.Tables["profit_resumido"].Rows.Add(objArr);
            }

            ViewState["ds"] = ds;
        }
        ds = (LibroDiarioDS)ViewState["ds"];

        rpt = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
        rpt.Load(Server.MapPath("~/CR_profitresumido.rpt"));
        rpt.SetDataSource(ds.Tables["profit_resumido"]);
        rpt.SetParameterValue("moneda", Utility.TraducirMonedaInt(monID));
        if (tipoconta == 1)
        {
            rpt.SetParameterValue("tipoconta", "FISCAL");
        }
        else if (tipoconta == 2)
        {
            rpt.SetParameterValue("tipoconta", "FINANCIERA");
        }
        else if (tipoconta == 3)
        {
            rpt.SetParameterValue("tipoconta", "CONSOLIDADO");
        }
        rpt.SetParameterValue("pais", user.pais.Nombre);
        rpt.SetParameterValue("fecha_inicio", fechaini);
        rpt.SetParameterValue("fecha_final", fechafin);
        rpt.SetParameterValue("empresa", Server.MapPath(user.pais.Imagepath));
        rpt.SetParameterValue("usuario", user.ID);
        //rpt.SetParameterValue(2, Server.MapPath(user.pais.Imagepath));
        CrystalReportViewer1.DisplayGroupTree = false;
        CrystalReportViewer1.ReportSource = rpt;
        CrystalReportViewer1.DataBind();
    }
    protected void reporte_depositos_pendientes(UsuarioBean user)
    {
        string fechaini = Request.QueryString["FechaIni"].ToString();
        string fechafin = Request.QueryString["FechaFin"].ToString();
        int monID = int.Parse(Request.QueryString["monID"].ToString());
        int tipo_reporte = int.Parse(Request.QueryString["tipo_reporte"].ToString());
        int paisID = user.PaisID;
        string monstr = "";
        if (monID == 1) monstr = "QUETZALES [GTQ]";
        else if (monID == 2) monstr = "DOLARES [USD]";
        else if (monID == 3) monstr = "LEMPIRAS [HNL]";
        //else if (monID == 4) monstr = "COLONES [NIC]";
        else if (monID == 4) monstr = "COLONES [C$]";
        else if (monID == 5) monstr = "COLONES [CRC]";
        else if (monID == 6) monstr = "PANAMA [PAB]";
        else if (monID == 7) monstr = "DOLARES [BZD]";
        else if (monID == 8) monstr = "DOLARES [USD]";

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
            ds = (LibroDiarioDS)DB.GetDepositosPendAsociar(user, paisID, monID, user.contaID, fechaini, fechafin,tipo_reporte);
            ViewState["Depositos_PendientesDs"] = ds;
        }
        ds = (LibroDiarioDS)ViewState["Depositos_PendientesDs"];
        rpt = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
        rpt.Load(Server.MapPath("~/CR_depositos.rpt"));
        rpt.SetDataSource(ds.Tables["Depositos_Pendientes"]);
        rpt.SetParameterValue("moneda", monstr);
        rpt.SetParameterValue("usuario", user.ID);
        rpt.SetParameterValue("fechaini",fechaini);
        rpt.SetParameterValue("fechafin", fechafin);
        rpt.SetParameterValue("empresa", Server.MapPath(user.pais.Imagepath));
        rpt.SetParameterValue("pais", user.pais.Nombre);
        CrystalReportViewer1.ReportSource = rpt;
        CrystalReportViewer1.DataBind();

    }
    protected void reporte_routing_facturados(UsuarioBean user)
    {
        string fechaini = Request.QueryString["f_ini"].ToString();
        string fechafin = Request.QueryString["f_fin"].ToString();
        int monID = int.Parse(Request.QueryString["monID"].ToString());
        int tconta = int.Parse(Request.QueryString["tcon"].ToString());
       // int tipo_reporte = int.Parse(Request.QueryString["tipo_reporte"].ToString());
        int paisID = user.PaisID;
        string monstr = "";
        if (monID == 1) monstr = "QUETZALES [GTQ]";
        else if (monID == 2) monstr = "DOLARES [USD]";
        else if (monID == 3) monstr = "LEMPIRAS [HNL]";
        //else if (monID == 4) monstr = "COLONES [NIC]";
        else if (monID == 4) monstr = "CORDOBAS [C$]";
        else if (monID == 5) monstr = "COLONES [CRC]";
        else if (monID == 6) monstr = "PANAMA [PAB]";
        else if (monID == 7) monstr = "DOLARES [BZD]";
        else if (monID == 8) monstr = "DOLARES [USD]";

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
            ds = (LibroDiarioDS)DB.GetRoutingFacturados(user,fechaini,fechafin,monID,tconta);
            ViewState["routing_facturadosDs"] = ds;
        }
        ds = (LibroDiarioDS)ViewState["routing_facturadosDs"];
        rpt = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
        rpt.Load(Server.MapPath("~/CR_routing_fact.rpt"));
        rpt.SetDataSource(ds.Tables["routing_facturados"]);
        rpt.SetParameterValue("moneda", monstr);
        rpt.SetParameterValue("usuario", user.ID);
        rpt.SetParameterValue("fechaini", fechaini);
        rpt.SetParameterValue("fechafin", fechafin);
        rpt.SetParameterValue("empresa", Server.MapPath(user.pais.Imagepath));
        rpt.SetParameterValue("pais", user.pais.Nombre);
        CrystalReportViewer1.ReportSource = rpt;
        CrystalReportViewer1.DataBind();

    }
    protected void reporte_routing_NO_facturados(UsuarioBean user)
    {
        string fechaini = Request.QueryString["f_ini"].ToString();
        string fechafin = Request.QueryString["f_fin"].ToString();
        string carga_ruteada = Request.QueryString["carga_ruteada"].ToString();
        string cargas_fhc = Request.QueryString["cargas_fhc"].ToString();

        string maritimo = Request.QueryString["maritimo"].ToString();
        string aereo = Request.QueryString["aereo"].ToString();
        string terrestre = Request.QueryString["terrestre"].ToString();
        string wms = Request.QueryString["wms"].ToString();
        string seguros = Request.QueryString["seguros"].ToString();
        string aduanas = Request.QueryString["aduanas"].ToString();
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

            int i = 0;
            DB.parametros Params = DB.EmpresaParametros(user.PaisID, "", "", "ROUTINGS NO FACTURADOS", ""); //pais, sistema, doc_id, titulo, edicion

            if (carga_ruteada == "si")
            {
                ArrayList arr_cargasrouteadas = (ArrayList)DB.GetCargarRuteadasNOFacturados(user, fechaini, fechafin);

                if (arr_cargasrouteadas != null)
                {
                    foreach (RE_GenericBean beanrout in arr_cargasrouteadas)
                    {
                        if (beanrout.strC2 == "Maritimo LCL" || beanrout.strC2 == "Maritimo FCL")
                        {
                            if (maritimo == "si")
                            {
                                i++;
                                object[] objArr1 = { beanrout.strC1, beanrout.strC2, beanrout.strC3, beanrout.strC4, beanrout.strC5, i == 1 ? Params.logo : null, i == 1 ? Params.titulo : "", i == 1 ? Params.edicion : "" };
                                ds.Tables["routing_no_facturados"].Rows.Add(objArr1);
                            }
                        }
                        if (beanrout.strC2 == "Aereo")
                        {
                            if (aereo == "si")
                            {
                                i++;
                                object[] objArr2 = { beanrout.strC1, beanrout.strC2, beanrout.strC3, beanrout.strC4, beanrout.strC5, i == 1 ? Params.logo : null, i == 1 ? Params.titulo : "", i == 1 ? Params.edicion : "" };
                                ds.Tables["routing_no_facturados"].Rows.Add(objArr2);
                            }
                        }
                        if (beanrout.strC2 == "Terrestre Local" || beanrout.strC2 == "Terrestre Expres")
                        {
                            if (terrestre == "si")
                            {
                                i++;
                                object[] objArr3 = { beanrout.strC1, beanrout.strC2, beanrout.strC3, beanrout.strC4, beanrout.strC5, i == 1 ? Params.logo : null, i == 1 ? Params.titulo : "", i == 1 ? Params.edicion : "" };
                                ds.Tables["routing_no_facturados"].Rows.Add(objArr3);
                            }
                        }
                        if (beanrout.strC2 == "Aduanas")
                        {
                            if (aduanas == "si")
                            {
                                i++;
                                object[] objArr4 = { beanrout.strC1, beanrout.strC2, beanrout.strC3, beanrout.strC4, beanrout.strC5, i == 1 ? Params.logo : null, i == 1 ? Params.titulo : "", i == 1 ? Params.edicion : "" };
                                ds.Tables["routing_no_facturados"].Rows.Add(objArr4);
                            }
                        }
                        if (beanrout.strC2 == "Seguros")
                        {
                            if (seguros == "si")
                            {
                                i++;
                                object[] objArr5 = { beanrout.strC1, beanrout.strC2, beanrout.strC3, beanrout.strC4, beanrout.strC5, i == 1 ? Params.logo : null, i == 1 ? Params.titulo : "", i == 1 ? Params.edicion : "" };
                                ds.Tables["routing_no_facturados"].Rows.Add(objArr5);
                            }
                        }
                    }
                }
            }
            

            if (cargas_fhc == "si")
            {
                if (maritimo == "si")
                {
                    ArrayList array_cif = (ArrayList)DB.GetCargasCIFNoFacturados(user, fechaini, fechafin);

                    if (array_cif != null)
                    {
                        foreach (RE_GenericBean bean1 in array_cif)
                        {
                            i++;
                            object[] obj1 = { bean1.strC1, bean1.strC2, bean1.strC3, bean1.strC4, bean1.strC5, i == 1 ? Params.logo : null, i == 1 ? Params.titulo : "", i == 1 ? Params.edicion : "" };
                            ds.Tables["routing_no_facturados"].Rows.Add(obj1);
                        }
                    }
                }

                if (aereo == "si")
                {
                    ArrayList array_cif_aereo = (ArrayList)DB.GetCargasCIFAereoNoFacturados(user, fechaini, fechafin);
                    if (array_cif_aereo != null)
                    {
                        foreach (RE_GenericBean bean2 in array_cif_aereo)
                        {
                            i++;
                            object[] obj2 = { bean2.strC1, bean2.strC2, bean2.strC3, bean2.strC4, bean2.strC5, i == 1 ? Params.logo : null, i == 1 ? Params.titulo : "", i == 1 ? Params.edicion : "" };
                            ds.Tables["routing_no_facturados"].Rows.Add(obj2);
                        }
                    }
                }

                if (terrestre == "si")
                {
                    ArrayList array_fhc_terr = (ArrayList)DB.GetCargasCIFTerrestreNoFacturados(user, fechaini, fechafin);
                    if (array_fhc_terr != null)
                    {
                        foreach (RE_GenericBean bean3 in array_fhc_terr)
                        {
                            i++;
                            object[] obj3 = { bean3.strC1, bean3.strC2, bean3.strC3, bean3.strC4, bean3.strC5, i == 1 ? Params.logo : null, i == 1 ? Params.titulo : "", i == 1 ? Params.edicion : "" };
                            ds.Tables["routing_no_facturados"].Rows.Add(obj3);
                        }
                    }
                }

                if (wms == "si")
                {
                    ArrayList array_fhc_wms = (ArrayList)DB.GetCargasWMSNoFacturados(user, fechaini, fechafin);
                    if (array_fhc_wms != null)
                    {
                        foreach (RE_GenericBean bean4 in array_fhc_wms)
                        {
                            i++;
                            object[] obj4 = { bean4.strC1, bean4.strC2, bean4.strC3, bean4.strC4, bean4.strC5, i == 1 ? Params.logo : null, i == 1 ? Params.titulo : "", i == 1 ? Params.edicion : "" };
                            ds.Tables["routing_no_facturados"].Rows.Add(obj4);
                        }
                    }
                }
            }

            if (i == 0)
            {
                object[] obj4 = { "", "", "", "", "", Params.logo, Params.titulo, Params.edicion };
                ds.Tables["routing_no_facturados"].Rows.Add(obj4);
            }

            ViewState["routing_no_facturadosDs"] = ds;
        }
        ds = (LibroDiarioDS)ViewState["routing_no_facturadosDs"];
        rpt = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
        rpt.Load(Server.MapPath("~/CR_routing_no_fact.rpt"));
        rpt.SetDataSource(ds.Tables["routing_no_facturados"]);
        rpt.SetParameterValue("usuario", user.ID);
        rpt.SetParameterValue("fechaini", fechaini);
        rpt.SetParameterValue("fechafin", fechafin);
        rpt.SetParameterValue("empresa", Server.MapPath(user.pais.Imagepath));
        rpt.SetParameterValue("pais", user.pais.Nombre);
        CrystalReportViewer1.ReportSource = rpt;
        CrystalReportViewer1.DataBind();

    }

    protected void reporte_cuentas_x_pagar(UsuarioBean user)
    {
        string fechafin = Request.QueryString["fechafin"].ToString();
        string moneda = Request.QueryString["monID"].ToString();
        string monstr = "";
        if (moneda == "1") monstr = "QUETZALES [GTQ]";
        else if (moneda == "2") monstr = "DOLARES [USD]";
        else if (moneda == "3") monstr = "LEMPIRAS [HNL]";
        else if (moneda == "4") monstr = "CORDOBAS [C$]";
        else if (moneda == "5") monstr = "COLONES [CRC]";
        else if (moneda == "6") monstr = "PANAMA [PAB]";
        else if (moneda == "7") monstr = "DOLARES [BZD]";
        else if (moneda == "8") monstr = "DOLARES [USD]";
        string conta = "";
        if (user.contaID == 1) conta = "FISCAL";
        else if (user.contaID == 2) conta = "FINANCIERA";
        else if (user.contaID == 3) conta = "CONSOLIDADO";

        if (!Page.IsPostBack)
        {
            ds = (LibroDiarioDS)DB.GetReporte_CuentasXPagar(user, fechafin, moneda);
            ViewState["cuentas_x_pagar_tblDS"] = ds;
        }
        ds = (LibroDiarioDS)ViewState["cuentas_x_pagar_tblDS"];
        rpt = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
        rpt.Load(Server.MapPath("~/CR_cuentas_por_pagar.rpt"));
        rpt.SetDataSource(ds.Tables["cuentas_x_pagar_tbl"]);
        rpt.SetParameterValue("usuario", user.ID);
        rpt.SetParameterValue("empresa", Server.MapPath(user.pais.Imagepath));
        rpt.SetParameterValue("pais", user.pais.Nombre);
        rpt.SetParameterValue("moneda", monstr);
        rpt.SetParameterValue("tipoconta", conta);
        rpt.SetParameterValue("fechafin", fechafin);


        CrystalReportViewer1.ReportSource = rpt;
        CrystalReportViewer1.DataBind();
    }

    protected void reporte_creditos_clientes(UsuarioBean user)
    {
        if (!Page.IsPostBack)
        {
            ds = (LibroDiarioDS)DB.GetReporte_CreditosClientes(user, user.pais.Nombre);
            ViewState["creditos_clientes_tblDS"] = ds;
        }
        ds = (LibroDiarioDS)ViewState["creditos_clientes_tblDS"];
        rpt = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
        rpt.Load(Server.MapPath("~/CR_Creditos_Clientes.rpt"));
        rpt.SetDataSource(ds.Tables["tbl_creditos_clientes"]);
        rpt.SetParameterValue("empresa", user.pais.Nombre);

        CrystalReportViewer1.ReportSource = rpt;
        CrystalReportViewer1.DataBind();
    }


    protected void reporte_routing_NO_asociados(UsuarioBean user)
    {
        string fechaini = Request.QueryString["f_ini"].ToString();
        string fechafin = Request.QueryString["f_fin"].ToString();
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
            ds = (LibroDiarioDS)DB.GetRoutingNOAsociados(user, fechaini, fechafin);
            ViewState["routing_no_asociadosDs"] = ds;
        }
        
        ds = (LibroDiarioDS)ViewState["routing_no_asociadosDs"];
        rpt = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
        rpt.Load(Server.MapPath("~/CR_ro_no_asociados.rpt"));
        rpt.SetDataSource(ds.Tables["routing_no_asociados"]);
        rpt.SetParameterValue("usuario", user.ID);
        rpt.SetParameterValue("fechaini", fechaini);
        rpt.SetParameterValue("fechafin", fechafin);
        rpt.SetParameterValue("empresa", Server.MapPath(user.pais.Imagepath)) ;
        rpt.SetParameterValue("pais", user.pais.Nombre);
        CrystalReportViewer1.ReportSource = rpt;
        CrystalReportViewer1.DataBind();

    }

    protected void integracion_bancaria(UsuarioBean user)
    {
        RE_GenericBean rgb = new RE_GenericBean();
        string fechaini = Request.QueryString["fechaini"].ToString();
        string fechafin = Request.QueryString["fechafin"].ToString();
        int tconta = int.Parse(Request.QueryString["conta"].ToString());
        int moneda = int.Parse(Request.QueryString["monID"].ToString());
        int banco = int.Parse(Request.QueryString["banco"].ToString());
        string cuenta = Request.QueryString["cuenta"].ToString();
        string cuentas = Request.QueryString["cuentas"].ToString();
        string bancos = Request.QueryString["bancos"].ToString();
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

            decimal tc = DB.getTipoCambioByDay(user, fechafin);

            ds = (LibroDiarioDS)DB.GetIntegracion_bancaria(user, fechaini, fechafin, banco, cuenta, moneda, tconta, cuentas, bancos, tc);
            ViewState["integracion_bancariaDS"] = ds;
        }
        string monstr = "";
        if (moneda == 1) monstr = "QUETZALES [GTQ]";
        else if (moneda == 2) monstr = "DOLARES [USD]";
        else if (moneda == 3) monstr = "LEMPIRAS [HNL]";
        else if (moneda == 4) monstr = "CORDOBAS [C$]";
        else if (moneda == 5) monstr = "COLONES [CRC]";
        else if (moneda == 6) monstr = "PANAMA [PN]";
        else if (moneda == 7) monstr = "DOLARES [BZD]";
        else if (moneda == 8) monstr = "DOLARES [USD]";


        ds = (LibroDiarioDS)ViewState["integracion_bancariaDS"];
        rpt = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
        rpt.Load(Server.MapPath("~/CR_integracion_bancaria.rpt"));
        rpt.SetDataSource(ds.Tables["integracion_bancaria"]);
        rpt.SetParameterValue("usuario", user.ID);
        rpt.SetParameterValue("desde", fechaini);
        rpt.SetParameterValue("hasta", fechafin);
        rpt.SetParameterValue("empresa", Server.MapPath(user.pais.Imagepath));
        rpt.SetParameterValue("pais", user.pais.ISO);
        rpt.SetParameterValue("moneda", monstr);
        rpt.SetParameterValue("banco", banco);

        string tc_ultimo = DB.getTipoCambioByDay(user, fechafin).ToString();
        rpt.SetParameterValue("tc_ultimo", tc_ultimo);
        if (tconta == 1)
        {
            rpt.SetParameterValue("conta", "FISCAL");
        }
        else if (tconta == 2)
        {
            rpt.SetParameterValue("conta", "FINANCIERA");
        }
        else if (tconta == 3)
        {
            rpt.SetParameterValue("conta", "CONSOLIDADO");
        }
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
