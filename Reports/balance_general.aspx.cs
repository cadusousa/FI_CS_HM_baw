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
    decimal total = 0;
    decimal totalactivo = 0;
    decimal totalpasivo = 0;
    decimal totalingreso = 0;
    decimal totalegreso = 0;
    decimal totalcapital = 0;
    decimal pasivo_capital = 0;
    decimal Resultado_Ejercicio = 0;
    string Tipo_Reporte = "";
    UsuarioBean user = null;
    string fecha_corte = "";
    string fecha_inicial = "";
    string fecha_final = "";
    string consmon = "";
    string consmon_rep = "";
    int monID = 0;
    int contaID = 0;
    ArrayList cuentas = null;
    ArrayList arr = null;
    CrystalDecisions.CrystalReports.Engine.ReportDocument rpt;
    protected void Page_Load(object sender, EventArgs e)
    {
        
        if (Session["usuario"] == null)
        {
            Response.Redirect("../default.aspx");
        }
        user=(UsuarioBean)Session["usuario"];
        LibroDiarioDS ds = new LibroDiarioDS();
        Tipo_Reporte = Request.QueryString["t_rep"].ToString();
        if (Tipo_Reporte == "1")
        {
            fecha_final = Request.QueryString["f_corte"].ToString();
            #region Formatear Fecha Corte
            //Fecha Corte
            int fe_dia = int.Parse(fecha_final.Substring(3, 2));
            int fe_mes = int.Parse(fecha_final.Substring(0, 2));
            int fe_anio = int.Parse(fecha_final.Substring(6, 4));
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
        }
        else if (Tipo_Reporte == "2")
        {
            fecha_inicial = Request.QueryString["f_inicial"].ToString();
            fecha_final = Request.QueryString["f_final"].ToString();
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
        }
        monID = int.Parse(Request.QueryString["monID"].ToString());
        contaID = int.Parse(Request.QueryString["tcon"].ToString());
        consmon = Request.QueryString["consmonfiscal"].ToString();

        if (consmon == "si")
        {
            consmon_rep = "CONSOLIDADO";
        }
        else
        {
            consmon_rep = "";
        }

        
        decimal tc = DB.getTipoCambioXFecha(user.PaisID, fecha_final);
        if (!IsPostBack)
        {
            decimal tipocambio = DB.getTipoCambioHoy(5);

            string path = "balance_general";
            RE_GenericBean reg = DB.Traducir_reportes(user, path);

            int i = 0;
            DB.parametros Params = DB.EmpresaParametros(user.PaisID, "", "", reg.strC10, ""); //pais, sistema, doc_id, titulo, edicion

            #region Obtener Resultado del Ejercicio
            Resultado_Ejercicio = DB.getResultado_Ejercicio(user, monID, contaID, fecha_inicial, fecha_final, Tipo_Reporte, consmon);
            lb_resultado_ejercicio.Text = Resultado_Ejercicio.ToString();
            #endregion
            arr = (ArrayList)DB.getBG(monID, tipocambio, contaID, user, fecha_final, fecha_inicial, fecha_final, Tipo_Reporte, consmon);
            foreach (ReportBean Reporte in arr)
            {
                if (Reporte.Cue_Clasificacion == 1)
                {
                    if (Reporte.Cue_Nivel == 1)
                    {
                        total = 0;
                        i++;
                        object[] objArr = { Reporte.Cue_Id, Reporte.Nombre, "", "", "", total, "ACTIVO", Reporte.Cue_Nivel, i == 1 ? Params.logo : null, i == 1 ? Params.titulo : "", i == 1 ? Params.edicion : "" };
                        ds.Tables["balancegeneral2"].Rows.Add(objArr);		                
                    }
                    if (Reporte.Cue_Nivel == 2)
                    {
                        total = 0;
                        i++;
                        object[] objArr = { Reporte.Cue_Id, "", Reporte.Nombre, "", "", total, "ACTIVO", Reporte.Cue_Nivel, i == 1 ? Params.logo : null, i == 1 ? Params.titulo : "", i == 1 ? Params.edicion : "" };
                        ds.Tables["balancegeneral2"].Rows.Add(objArr);
                    }
                    if (Reporte.Cue_Nivel == 3)
                    {
                        total = 0;
                        total = Reporte.Debe- Reporte.Haber;
                        i++;
                        object[] objArr = { Reporte.Cue_Id, "", "", Reporte.Nombre, "", total, "ACTIVO", Reporte.Cue_Nivel, i == 1 ? Params.logo : null, i == 1 ? Params.titulo : "", i == 1 ? Params.edicion : "" };
                        ds.Tables["balancegeneral2"].Rows.Add(objArr);
                    }
                    if (Reporte.Cue_Nivel == 4)
                    {
                        ArrayList Arr_Hijas = (ArrayList)DB.getHijos(Reporte.Cue_Id, monID, contaID, fecha_inicial, fecha_final, user, consmon);
                        foreach (RE_GenericBean Re_Hijas in Arr_Hijas)
                        {
                            Reporte.Debe += Re_Hijas.decC1;
                            Reporte.Haber += Re_Hijas.decC2;
                        }
                        total = Reporte.Debe - Reporte.Haber;
                        i++;
                        object[] objArr = { Reporte.Cue_Id, "", "", "", Reporte.Nombre, total, "ACTIVO", Reporte.Cue_Nivel, i == 1 ? Params.logo : null, i == 1 ? Params.titulo : "", i == 1 ? Params.edicion : "" };
                        ds.Tables["balancegeneral2"].Rows.Add(objArr);
                    }
                }
                if (Reporte.Cue_Clasificacion == 2)
                {
                    if (Reporte.Cue_Nivel == 1)
                    {
                        total = 0;
                        i++;
                        object[] objArr = { Reporte.Cue_Id, Reporte.Nombre, "", "", "", total, "PASIVO", Reporte.Cue_Nivel, i == 1 ? Params.logo : null, i == 1 ? Params.titulo : "", i == 1 ? Params.edicion : "" };
                        ds.Tables["balancegeneral2"].Rows.Add(objArr);
                    }
                    if (Reporte.Cue_Nivel == 2)
                    {
                        total = 0;
                        i++;
                        object[] objArr = { Reporte.Cue_Id, "", Reporte.Nombre, "", "", total, "PASIVO", Reporte.Cue_Nivel, i == 1 ? Params.logo : null, i == 1 ? Params.titulo : "", i == 1 ? Params.edicion : "" };
                        ds.Tables["balancegeneral2"].Rows.Add(objArr);
                    }
                    if (Reporte.Cue_Nivel == 3)
                    {
                        total = 0;
                        total = Reporte.Haber - Reporte.Debe;
                        i++;
                        object[] objArr = { Reporte.Cue_Id, "", "", Reporte.Nombre, "", total, "PASIVO", Reporte.Cue_Nivel, i == 1 ? Params.logo : null, i == 1 ? Params.titulo : "", i == 1 ? Params.edicion : "" };
                        ds.Tables["balancegeneral2"].Rows.Add(objArr);
                    }
                    if (Reporte.Cue_Nivel == 4)
                    {
                        ArrayList Arr_Hijas = (ArrayList)DB.getHijos(Reporte.Cue_Id, monID, contaID, fecha_inicial, fecha_final, user, consmon);
                        foreach (RE_GenericBean Re_Hijas in Arr_Hijas)
                        {
                            Reporte.Debe += Re_Hijas.decC1;
                            Reporte.Haber += Re_Hijas.decC2;
                        }
                        total = Reporte.Haber - Reporte.Debe;
                        totalpasivo += total;
                        i++;
                        object[] objArr = { Reporte.Cue_Id, "", "", "", Reporte.Nombre, total, "PASIVO", Reporte.Cue_Nivel, i == 1 ? Params.logo : null, i == 1 ? Params.titulo : "", i == 1 ? Params.edicion : "" };
                        ds.Tables["balancegeneral2"].Rows.Add(objArr);
                    }
                }
                else if (Reporte.Cue_Clasificacion == 5)
                {
                    if (Reporte.Cue_Nivel == 1)
                    {
                        total = 0;
                        i++;
                        object[] objArr = { Reporte.Cue_Id, Reporte.Nombre, "", "", "", total, "CAPITAL", Reporte.Cue_Nivel, i == 1 ? Params.logo : null, i == 1 ? Params.titulo : "", i == 1 ? Params.edicion : "" };
                        ds.Tables["balancegeneral2"].Rows.Add(objArr);
                    }
                    if (Reporte.Cue_Nivel == 2)
                    {
                        total = 0;
                        i++;
                        object[] objArr = { Reporte.Cue_Id, "", Reporte.Nombre, "", "", total, "CAPITAL", Reporte.Cue_Nivel, i == 1 ? Params.logo : null, i == 1 ? Params.titulo : "", i == 1 ? Params.edicion : "" };
                        ds.Tables["balancegeneral2"].Rows.Add(objArr);
                    }
                    if (Reporte.Cue_Nivel == 3)
                    {
                        total = 0;
                        total = Reporte.Haber - Reporte.Debe;
                        i++;
                        object[] objArr = { Reporte.Cue_Id, "", "", Reporte.Nombre, "", total, "CAPITAL", Reporte.Cue_Nivel, i == 1 ? Params.logo : null, i == 1 ? Params.titulo : "", i == 1 ? Params.edicion : "" };
                        ds.Tables["balancegeneral2"].Rows.Add(objArr);
                    }
                    if (Reporte.Cue_Nivel == 4)
                    {
                        ArrayList Arr_Hijas = (ArrayList)DB.getHijos(Reporte.Cue_Id, monID, contaID, fecha_inicial, fecha_final, user, consmon);
                        foreach (RE_GenericBean Re_Hijas in Arr_Hijas)
                        {
                            Reporte.Debe += Re_Hijas.decC1;
                            Reporte.Haber += Re_Hijas.decC2;
                        }
                        total = Reporte.Haber - Reporte.Debe;
                        totalcapital += total;
                        i++;
                        object[] objArr = { Reporte.Cue_Id, "", "", "", Reporte.Nombre, total, "CAPITAL", Reporte.Cue_Nivel, i == 1 ? Params.logo : null, i == 1 ? Params.titulo : "", i == 1 ? Params.edicion : "" };
                        ds.Tables["balancegeneral2"].Rows.Add(objArr);
                    }
                }
            }

            if (i == 0) {			                			
                object[] objArr = { "", "", "", "", "", 0, "", "", Params.logo, Params.titulo, Params.edicion };
                ds.Tables["balancegeneral2"].Rows.Add(objArr);
            }



            ViewState["balancegeneral2"] = ds;

            /*
            //Definir ViewState
            pasivo_capital  = totalpasivo + totalcapital;
            lbl_pasivo_capital.Text = pasivo_capital.ToString();
            rpt = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
            rpt.Load(Server.MapPath("~/CR_BG.rpt"));
            rpt.SetDataSource(ds.Tables["balancegeneral2"]);

            rpt.SetParameterValue("moneda", Utility.TraducirMonedaInt(monID));
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
            rpt.SetParameterValue("pais", user.pais.Nombre);
            rpt.SetParameterValue("Tipo_Reporte", Tipo_Reporte);
            if (Tipo_Reporte == "1")
            {
                rpt.SetParameterValue("fecha_inicio", fecha_inicial);
                rpt.SetParameterValue("fecha_fin", fecha_final);
            }
            else
            {
                rpt.SetParameterValue("fecha_inicio", fecha_inicial);
                rpt.SetParameterValue("fecha_fin", fecha_final);
            }
            rpt.SetParameterValue("Resultado_Ejercicio", Resultado_Ejercicio);
            rpt.SetParameterValue("contaID", contaID);
            rpt.SetParameterValue("monID", monID);
            rpt.SetParameterValue("tc", tc);
            rpt.SetParameterValue("usuario", user.ID);
            rpt.SetParameterValue("empresa", Server.MapPath(user.pais.Imagepath));
            //***titulos traducidos***
            rpt.SetParameterValue("Tpais", reg.strC1);
            rpt.SetParameterValue("Tconta", reg.strC2);
            rpt.SetParameterValue("Tmoneda", reg.strC3);
            //rpt.SetParameterValue("Ttitulo", reg.strC10);
            rpt.SetParameterValue("Tdesde", reg.strC4);
            rpt.SetParameterValue("Thasta", reg.strC5);
            rpt.SetParameterValue("Tfechacorte", reg.strC6);
            rpt.SetParameterValue("consolida_moneda", consmon_rep);
            CrystalReportViewer1.ReportSource = rpt;
            CrystalReportViewer1.DataBind();
            */
        }
        else
        {
            ds = (LibroDiarioDS)ViewState["balancegeneral2"];
            /*
            rpt = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
            rpt.Load(Server.MapPath("~/CR_BG.rpt"));
            rpt.SetDataSource(ds.Tables["balancegeneral2"]);
            string path = "balance_general";
            RE_GenericBean reg = DB.Traducir_reportes(user, path);
            rpt.SetParameterValue("moneda", Utility.TraducirMonedaInt(monID));
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
            rpt.SetParameterValue("pais", user.pais.Nombre);
            rpt.SetParameterValue("fecha_inicio", fecha_corte);
            rpt.SetParameterValue("Resultado_Ejercicio", decimal.Parse(lb_resultado_ejercicio.Text));
            rpt.SetParameterValue("Tipo_Reporte", Tipo_Reporte);
            if (Tipo_Reporte == "1")
            {
                rpt.SetParameterValue("fecha_inicio", fecha_inicial);
                rpt.SetParameterValue("fecha_fin", fecha_final);
            }
            else
            {
                rpt.SetParameterValue("fecha_inicio", fecha_inicial);
                rpt.SetParameterValue("fecha_fin", fecha_final);
            }
            rpt.SetParameterValue("contaID", contaID);
            rpt.SetParameterValue("monID", monID);
            rpt.SetParameterValue("tc", tc);
            rpt.SetParameterValue("usuario", user.ID);
            rpt.SetParameterValue("empresa", Server.MapPath(user.pais.Imagepath));
            //***titulos traducidos***
            rpt.SetParameterValue("Tpais", reg.strC1);
            rpt.SetParameterValue("Tconta", reg.strC2);
            rpt.SetParameterValue("Tmoneda", reg.strC3);
            //rpt.SetParameterValue("Ttitulo", reg.strC10);
            rpt.SetParameterValue("Tdesde", reg.strC4);
            rpt.SetParameterValue("Thasta", reg.strC5);
            rpt.SetParameterValue("Tfechacorte", reg.strC6);
            rpt.SetParameterValue("consolida_moneda", consmon_rep);
            CrystalReportViewer1.ReportSource = rpt;
            CrystalReportViewer1.DataBind();
             * */
        }








        /*
        string filtro = "http://localhost:54310/BAW/Reports/SubReporte_BG.aspx";

        if (Tipo_Reporte == "1")
        {
            filtro += "?2010/01/01";
        }
        else if (Tipo_Reporte == "2")
        {
            filtro += "?" + fecha_inicial;
        }
        filtro += "?" + fecha_final;
        filtro += "?#*cueid*#";
        filtro += "?" + contaID;
        filtro += "?" + monID;
        filtro += "?" + consmon;

        string filename = @"C:/BAW_REPORTS/BG_Reporte.htm";
        DB.GetHtmlData(ds.Tables["balancegeneral2"], filtro, 0, "#*cueid*#", filename, "");

        int counter = 0;
        string line;

        // Read the file and display it line by line.  
        System.IO.StreamReader file = new System.IO.StreamReader(filename);
        while ((line = file.ReadLine()) != null)
        {
            Response.Write(line);
            counter++;
        }
        file.Close();
        */



        
        rpt = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
        rpt.Load(Server.MapPath("~/CR_BG.rpt"));
        rpt.SetDataSource(ds.Tables["balancegeneral2"]);
        //string path = "balance_general";
        RE_GenericBean reg2 = DB.Traducir_reportes(user, "balance_general");
        rpt.SetParameterValue("moneda", Utility.TraducirMonedaInt(monID));
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
        rpt.SetParameterValue("pais", user.pais.Nombre);
        rpt.SetParameterValue("fecha_inicio", fecha_corte);
        rpt.SetParameterValue("Resultado_Ejercicio", decimal.Parse(lb_resultado_ejercicio.Text));
        rpt.SetParameterValue("Tipo_Reporte", Tipo_Reporte);
        if (Tipo_Reporte == "1")
        {
            rpt.SetParameterValue("fecha_inicio", fecha_inicial);
            rpt.SetParameterValue("fecha_fin", fecha_final);
        }
        else
        {
            rpt.SetParameterValue("fecha_inicio", fecha_inicial);
            rpt.SetParameterValue("fecha_fin", fecha_final);
        }
        rpt.SetParameterValue("contaID", contaID);
        rpt.SetParameterValue("monID", monID);
        rpt.SetParameterValue("tc", tc);
        rpt.SetParameterValue("usuario", user.ID);
        rpt.SetParameterValue("empresa", Server.MapPath(user.pais.Imagepath));
        //***titulos traducidos***
        rpt.SetParameterValue("Tpais", reg2.strC1);
        rpt.SetParameterValue("Tconta", reg2.strC2);
        rpt.SetParameterValue("Tmoneda", reg2.strC3);
        //rpt.SetParameterValue("Ttitulo", reg.strC10);
        rpt.SetParameterValue("Tdesde", reg2.strC4);
        rpt.SetParameterValue("Thasta", reg2.strC5);
        rpt.SetParameterValue("Tfechacorte", reg2.strC6);
        rpt.SetParameterValue("consolida_moneda", consmon_rep);
        CrystalReportViewer1.ReportSource = rpt;
        CrystalReportViewer1.DataBind();
        
    }

    protected ArrayList TotalPorNivel(string cue_madre, decimal nivel4_debe, decimal nivel4_haber)
    {
        ArrayList result = new ArrayList();
        decimal debe = 0;
        decimal haber = 0;
        foreach (ReportBean Arreglo in arr)
        {
            if (Arreglo.Cue_Madre == cue_madre)
            {
                debe += Arreglo.Debe;
                haber += Arreglo.Haber;
            }
        }
        debe += nivel4_debe;
        haber += nivel4_haber;
        result.Add(debe);
        result.Add(haber);
        return result;
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
