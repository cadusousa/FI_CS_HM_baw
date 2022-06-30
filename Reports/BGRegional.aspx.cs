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

public partial class Reports_solicita_BGR : System.Web.UI.Page
{

    decimal total = 0;
    decimal totalgt = 0;
    decimal totales = 0;
    decimal totalhn = 0;
    decimal totalnic = 0;
    decimal totalcr = 0;
    decimal totalpn = 0;
    decimal totalbc = 0;
    decimal totalgrh = 0;
    decimal totalisi = 0;
    decimal totalmayan = 0;
    decimal totales2 = 0;
    decimal totalactivo = 0;
    decimal totalpasivo = 0;
    decimal totalingreso = 0;
    decimal totalegreso = 0;
    decimal totalcapital = 0;
    decimal pasivo_capital = 0;
    decimal Resultado_Ejercicio = 0,  Resultado_Ejerciciogt = 0, Resultado_Ejercicioes = 0;
    decimal Resultado_Ejercicioes2 = 0, Resultado_Ejerciciohn = 0, Resultado_Ejercicionic = 0;
    decimal Resultado_Ejerciciocr = 0, Resultado_Ejerciciopn = 0, Resultado_Ejerciciobs = 0;
    decimal Resultado_Ejerciciogrh = 0, Resultado_Ejercicioisi = 0, Resultado_Ejerciciomayan = 0;
    string Tipo_Reporte = "";
    UsuarioBean user = null;
    string fecha_corte = "";
    string fecha_inicial = "";
    string fecha_final = "";
    string tipo_reporte = "";
    int monID = 0;
    int contaID = 0;
    int tipo = 0;
    string archivo = "";
    ArrayList cuentas = null;
    ArrayList arr = null;
    CrystalDecisions.CrystalReports.Engine.ReportDocument rpt;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["usuario"] == null)
        {
            Response.Redirect("../default.aspx");
        }
        user = (UsuarioBean)Session["usuario"];
        LibroDiarioDS ds = new LibroDiarioDS();
        Tipo_Reporte = Request.QueryString["t_rep"].ToString();
        tipo = int.Parse(Tipo_Reporte.ToString());
        if (Tipo_Reporte == "1")
        {
            fecha_final = Request.QueryString["f_corte"].ToString();
            archivo = " C:/BAWReportBSyER/Balance_General_Regional_Completo.xls";
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
            archivo = " C:/BAWReportBSyER/Balance_General_Regional_Acotado.xls";
   
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

        int i = 0;

        DB.parametros Params = DB.EmpresaParametros(user.PaisID, "", "", "BALANCE GENERAL REGIONAL", ""); //pais, sistema, doc_id, titulo, edicion
        
        //decimal tipocambio = DB.getTipoCambioHoy(5);
            #region Obtener Resultado del Ejercicio
            Resultado_Ejercicio = DB.getResultado_Ejercicio_Regional(fecha_inicial, fecha_final, 8, tipo);  //toda la region
            Resultado_Ejerciciogt = DB.getResultado_Ejercicio_Regional(fecha_inicial, fecha_final, 1, tipo); //guatemala
            Resultado_Ejercicioes = DB.getResultado_Ejercicio_Regional(fecha_inicial, fecha_final, 2, tipo); //El salvador
            Resultado_Ejercicioes2 = DB.getResultado_Ejercicio_Regional(fecha_inicial, fecha_final, 9, tipo); //El salvador 2
            Resultado_Ejerciciohn = DB.getResultado_Ejercicio_Regional(fecha_inicial, fecha_final, 3, tipo); //honduras
            Resultado_Ejercicionic = DB.getResultado_Ejercicio_Regional(fecha_inicial, fecha_final, 4, tipo); //nicaragua
            Resultado_Ejerciciocr = DB.getResultado_Ejercicio_Regional(fecha_inicial, fecha_final, 5, tipo); //Costa Rica
            Resultado_Ejerciciopn = DB.getResultado_Ejercicio_Regional(fecha_inicial, fecha_final, 6, tipo); //Panama
            Resultado_Ejerciciobs = DB.getResultado_Ejercicio_Regional(fecha_inicial, fecha_final, 7, tipo); //belise
            Resultado_Ejerciciogrh = DB.getResultado_Ejercicio_Regional(fecha_inicial, fecha_final, 11, tipo); //grh
            Resultado_Ejercicioisi = DB.getResultado_Ejercicio_Regional(fecha_inicial, fecha_final, 12, tipo); //isi
            Resultado_Ejerciciomayan = DB.getResultado_Ejercicio_Regional(fecha_inicial, fecha_final, 13, tipo); //isi
           // lb_resultado_ejercicio.Text = Resultado_Ejercicio.ToString();
            #endregion
            arr = (ArrayList)DB.getBG_Regional(fecha_inicial, fecha_final, tipo);
            foreach (ReportBean Reporte in arr)
            {
                if (Reporte.Cue_Clasificacion == 1)
                {
                    if (Reporte.Cue_Nivel == 1)
                    {
                        total = 0;
            			i++;
                        object[] objArr = { Reporte.Cue_Id, Reporte.Nombre, "", "", "", total, "ACTIVO", Reporte.Cue_Nivel, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, i == 1 ? Params.logo : null, i == 1 ? Params.titulo : "", i == 1 ? Params.edicion : "" };
                        ds.Tables["balancegeneralregional"].Rows.Add(objArr);
                    }
                    if (Reporte.Cue_Nivel == 2)
                    {
                        total = 0;
                        object[] objArr = { Reporte.Cue_Id, "", Reporte.Nombre, "", "", total, "ACTIVO", Reporte.Cue_Nivel, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, i == 1 ? Params.logo : null, i == 1 ? Params.titulo : "", i == 1 ? Params.edicion : "" };
                        ds.Tables["balancegeneralregional"].Rows.Add(objArr);
                    }
                    if (Reporte.Cue_Nivel == 3)
                    {
                        total = 0;
                        totalgt = 0; totales = 0; totales2 = 0; totalhn = 0; totalnic = 0; totalgrh = 0; totalisi = 0; totalmayan = 0;
                        totalcr = 0; totalpn = 0; totalbc = 0;
                        total = Reporte.Debe - Reporte.Haber;
                        totalgt = Reporte.Debegt - Reporte.Habergt;
                        totales = Reporte.DebeEs - Reporte.HaberEs;
                        totales2 = Reporte.DebeEs2 - Reporte.HaberEs2;
                        totalhn = Reporte.DebeHn - Reporte.HaberHn;
                        totalnic = Reporte.Debenic - Reporte.Habernic;
                        totalgrh = Reporte.Debegrh - Reporte.Habergrh;
                        totalisi = Reporte.Debeisi - Reporte.Haberisi;
                        totalmayan = Reporte.Debemayan - Reporte.Habermayan;
                        totalcr = Reporte.DebeCr - Reporte.HaberCr;
                        totalpn = Reporte.DebePr - Reporte.HaberPr;
                        totalbc = Reporte.Debebc - Reporte.Haberbc;
                        object[] objArr = { Reporte.Cue_Id, "", "", Reporte.Nombre, "", total, "ACTIVO", Reporte.Cue_Nivel, totalgt, totales, totales2, totalhn, totalnic, totalgrh, totalisi, totalmayan, totalcr, totalpn, totalbc, i == 1 ? Params.logo : null, i == 1 ? Params.titulo : "", i == 1 ? Params.edicion : "" };
                        ds.Tables["balancegeneralregional"].Rows.Add(objArr);
                    }
                    if (Reporte.Cue_Nivel == 4)
                    {
                        ArrayList Arr_Hijas = (ArrayList)DB.getHijosRegional(Reporte.Cue_Id,fecha_inicial, fecha_final);
                        foreach (RE_GenericBean Re_Hijas in Arr_Hijas)
                        {
                            Reporte.Debe += Re_Hijas.decC1;
                            Reporte.Haber += Re_Hijas.decC2;
                            Reporte.Debegt += Re_Hijas.decC3;
                            Reporte.Habergt += Re_Hijas.decC4;
                            Reporte.DebeCr += Re_Hijas.decC5;
                            Reporte.HaberCr += Re_Hijas.decC6;
                            Reporte.DebePr += Re_Hijas.decC7;
                            Reporte.HaberPr += Re_Hijas.decC8;
                            Reporte.DebeHn += Re_Hijas.decC9;
                            Reporte.HaberHn += Re_Hijas.decC10;
                            Reporte.DebeEs += Re_Hijas.decC11;
                            Reporte.HaberEs += Re_Hijas.decC12;
                            Reporte.Debenic += Re_Hijas.decC13;
                            Reporte.Habernic += Re_Hijas.decC14;
                            Reporte.DebeEs2 += Re_Hijas.decC15;
                            Reporte.HaberEs2 += Re_Hijas.decC16;
                            Reporte.Debegrh += Re_Hijas.decC17;
                            Reporte.Habergrh += Re_Hijas.decC18;
                            Reporte.Debeisi += Re_Hijas.decC19;
                            Reporte.Haberisi += Re_Hijas.decC20;
                            Reporte.Debemayan += Re_Hijas.decC21;
                            Reporte.Habermayan += Re_Hijas.decC22;
                            Reporte.Debebc += Re_Hijas.decC23;
                            Reporte.Haberbc += Re_Hijas.decC24;
                        }
                        total = Reporte.Debe - Reporte.Haber;
                        totalgt = Reporte.Debegt - Reporte.Habergt;
                        totales = Reporte.DebeEs - Reporte.HaberEs;
                        totales2 = Reporte.DebeEs2 - Reporte.HaberEs2;
                        totalhn = Reporte.DebeHn - Reporte.HaberHn;
                        totalnic = Reporte.Debenic - Reporte.Habernic;
                        totalgrh = Reporte.Debegrh - Reporte.Habergrh;
                        totalisi = Reporte.Debeisi - Reporte.Haberisi;
                        totalmayan = Reporte.Debemayan - Reporte.Habermayan;
                        totalcr = Reporte.DebeCr - Reporte.HaberCr;
                        totalpn = Reporte.DebePr - Reporte.HaberPr;
                        totalbc = Reporte.Debebc - Reporte.Haberbc;
                        object[] objArr = { Reporte.Cue_Id, "", "", "", Reporte.Nombre, total, "ACTIVO", Reporte.Cue_Nivel, totalgt, totales, totales2, totalhn, totalnic, totalgrh, totalisi, totalmayan, totalcr, totalpn, totalbc, i == 1 ? Params.logo : null, i == 1 ? Params.titulo : "", i == 1 ? Params.edicion : "" };
                        ds.Tables["balancegeneralregional"].Rows.Add(objArr);
                    }
                }
                if (Reporte.Cue_Clasificacion == 2)
                {
                    if (Reporte.Cue_Nivel == 1)
                    {
                        total = 0;
                        object[] objArr = { Reporte.Cue_Id, Reporte.Nombre, "", "", "", total, "PASIVO", Reporte.Cue_Nivel, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, i == 1 ? Params.logo : null, i == 1 ? Params.titulo : "", i == 1 ? Params.edicion : "" };
                        ds.Tables["balancegeneralregional"].Rows.Add(objArr);
                    }
                    if (Reporte.Cue_Nivel == 2)
                    {
                        total = 0;
                        object[] objArr = { Reporte.Cue_Id, "", Reporte.Nombre, "", "", total, "PASIVO", Reporte.Cue_Nivel, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, i == 1 ? Params.logo : null, i == 1 ? Params.titulo : "", i == 1 ? Params.edicion : "" };
                        ds.Tables["balancegeneralregional"].Rows.Add(objArr);
                    }
                    if (Reporte.Cue_Nivel == 3)
                    {
                        total = 0;
                        totalgt = 0; totales = 0; totales2 = 0; totalhn = 0; totalnic = 0; totalgrh = 0; totalisi = 0; totalmayan = 0;
                        totalcr = 0; totalpn = 0; totalbc = 0;
                        total = Reporte.Haber - Reporte.Debe;
                        totalgt = Reporte.Habergt - Reporte.Debegt;
                        totales = Reporte.HaberEs - Reporte.DebeEs;
                        totales2 =Reporte.HaberEs2 -  Reporte.DebeEs2;
                        totalhn = Reporte.HaberHn - Reporte.DebeHn;
                        totalnic =Reporte.Habernic - Reporte.Debenic;
                        totalgrh = Reporte.Habergrh - Reporte.Debegrh;
                        totalisi =Reporte.Haberisi - Reporte.Debeisi;
                        totalmayan = Reporte.Habermayan - Reporte.Debemayan;
                        totalcr =  Reporte.HaberCr - Reporte.DebeCr;
                        totalpn =Reporte.HaberPr -  Reporte.DebePr;
                        totalbc =Reporte.Haberbc -  Reporte.Debebc;
                        object[] objArr = { Reporte.Cue_Id, "", "", Reporte.Nombre, "", total, "PASIVO", Reporte.Cue_Nivel, totalgt, totales, totales2, totalhn, totalnic, totalgrh, totalisi, totalmayan, totalcr, totalpn, totalbc, i == 1 ? Params.logo : null, i == 1 ? Params.titulo : "", i == 1 ? Params.edicion : "" };
                        ds.Tables["balancegeneralregional"].Rows.Add(objArr);
                    }
                    if (Reporte.Cue_Nivel == 4)
                    {
                        ArrayList Arr_Hijas = (ArrayList)DB.getHijosRegional(Reporte.Cue_Id, fecha_inicial, fecha_final);
                        foreach (RE_GenericBean Re_Hijas in Arr_Hijas)
                        {
                            Reporte.Debe += Re_Hijas.decC1;
                            Reporte.Haber += Re_Hijas.decC2;
                            Reporte.Debegt += Re_Hijas.decC3;
                            Reporte.Habergt += Re_Hijas.decC4;
                            Reporte.DebeCr += Re_Hijas.decC5;
                            Reporte.HaberCr += Re_Hijas.decC6;
                            Reporte.DebePr += Re_Hijas.decC7;
                            Reporte.HaberPr += Re_Hijas.decC8;
                            Reporte.DebeHn += Re_Hijas.decC9;
                            Reporte.HaberHn += Re_Hijas.decC10;
                            Reporte.DebeEs += Re_Hijas.decC11;
                            Reporte.HaberEs += Re_Hijas.decC12;
                            Reporte.Debenic += Re_Hijas.decC13;
                            Reporte.Habernic += Re_Hijas.decC14;
                            Reporte.DebeEs2 += Re_Hijas.decC15;
                            Reporte.HaberEs2 += Re_Hijas.decC16;
                            Reporte.Debegrh += Re_Hijas.decC17;
                            Reporte.Habergrh += Re_Hijas.decC18;
                            Reporte.Debeisi += Re_Hijas.decC19;
                            Reporte.Haberisi += Re_Hijas.decC20;
                            Reporte.Debemayan += Re_Hijas.decC21;
                            Reporte.Habermayan += Re_Hijas.decC22;
                            Reporte.Debebc += Re_Hijas.decC23;
                            Reporte.Haberbc += Re_Hijas.decC24;
                        }
                        total = Reporte.Haber - Reporte.Debe;
                        totalgt =Reporte.Habergt -  Reporte.Debegt;
                        totales =Reporte.HaberEs -  Reporte.DebeEs;
                        totales2 = Reporte.HaberEs2 - Reporte.DebeEs2;
                        totalhn = Reporte.HaberHn - Reporte.DebeHn;
                        totalnic = Reporte.Habernic - Reporte.Debenic;
                        totalgrh =  Reporte.Habergrh - Reporte.Debegrh;
                        totalisi =  Reporte.Haberisi - Reporte.Debeisi;
                        totalmayan =  Reporte.Habermayan - Reporte.Debemayan;
                        totalcr = Reporte.HaberCr - Reporte.DebeCr;
                        totalpn = Reporte.HaberPr - Reporte.DebePr;
                        totalbc = Reporte.Haberbc - Reporte.Debebc;
                        totalpasivo += total;
                        object[] objArr = { Reporte.Cue_Id, "", "", "", Reporte.Nombre, total, "PASIVO", Reporte.Cue_Nivel, totalgt, totales, totales2, totalhn, totalnic, totalgrh, totalisi, totalmayan, totalcr, totalpn, totalbc, i == 1 ? Params.logo : null, i == 1 ? Params.titulo : "", i == 1 ? Params.edicion : "" };
                        ds.Tables["balancegeneralregional"].Rows.Add(objArr);
                    }
                }
                else if (Reporte.Cue_Clasificacion == 5)
                {
                    if (Reporte.Cue_Nivel == 1)
                    {
                        total = 0;
                        object[] objArr = { Reporte.Cue_Id, Reporte.Nombre, "", "", "", total, "CAPITAL", Reporte.Cue_Nivel, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, i == 1 ? Params.logo : null, i == 1 ? Params.titulo : "", i == 1 ? Params.edicion : "" };
                        ds.Tables["balancegeneralregional"].Rows.Add(objArr);
                    }
                    if (Reporte.Cue_Nivel == 2)
                    {
                        total = 0;
                        object[] objArr = { Reporte.Cue_Id, "", Reporte.Nombre, "", "", total, "CAPITAL", Reporte.Cue_Nivel, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, i == 1 ? Params.logo : null, i == 1 ? Params.titulo : "", i == 1 ? Params.edicion : "" };
                        ds.Tables["balancegeneralregional"].Rows.Add(objArr);
                    }
                    if (Reporte.Cue_Nivel == 3)
                    {
                        total = 0;
                        totalgt = 0; totales = 0; totales2 = 0; totalhn = 0; totalnic = 0; totalgrh = 0; totalisi = 0; totalmayan = 0;
                        totalcr = 0; totalpn = 0; totalbc = 0;
                        total = Reporte.Haber - Reporte.Debe;
                        totalgt = Reporte.Habergt - Reporte.Debegt;
                        totales = Reporte.HaberEs - Reporte.DebeEs;
                        totales2 = Reporte.HaberEs2 - Reporte.DebeEs2;
                        totalhn = Reporte.HaberHn - Reporte.DebeHn;
                        totalnic = Reporte.Habernic - Reporte.Debenic;
                        totalgrh = Reporte.Habergrh - Reporte.Debegrh;
                        totalisi = Reporte.Haberisi - Reporte.Debeisi;
                        totalmayan = Reporte.Habermayan - Reporte.Debemayan;
                        totalcr = Reporte.HaberCr - Reporte.DebeCr;
                        totalpn = Reporte.HaberPr - Reporte.DebePr;
                        totalbc = Reporte.Haberbc - Reporte.Debebc;
                        object[] objArr = { Reporte.Cue_Id, "", "", Reporte.Nombre, "", total, "CAPITAL", Reporte.Cue_Nivel, totalgt, totales, totales2, totalhn, totalnic, totalgrh, totalisi, totalmayan, totalcr, totalpn, totalbc, i == 1 ? Params.logo : null, i == 1 ? Params.titulo : "", i == 1 ? Params.edicion : "" };
                        ds.Tables["balancegeneralregional"].Rows.Add(objArr);
                    }
                    if (Reporte.Cue_Nivel == 4)
                    {
                        ArrayList Arr_Hijas = (ArrayList)DB.getHijosRegional(Reporte.Cue_Id, fecha_inicial, fecha_final);
                        foreach (RE_GenericBean Re_Hijas in Arr_Hijas)
                        {
                            Reporte.Debe += Re_Hijas.decC1;
                            Reporte.Haber += Re_Hijas.decC2;
                            Reporte.Debegt += Re_Hijas.decC3;
                            Reporte.Habergt += Re_Hijas.decC4;
                            Reporte.DebeCr += Re_Hijas.decC5;
                            Reporte.HaberCr += Re_Hijas.decC6;
                            Reporte.DebePr += Re_Hijas.decC7;
                            Reporte.HaberPr += Re_Hijas.decC8;
                            Reporte.DebeHn += Re_Hijas.decC9;
                            Reporte.HaberHn += Re_Hijas.decC10;
                            Reporte.DebeEs += Re_Hijas.decC11;
                            Reporte.HaberEs += Re_Hijas.decC12;
                            Reporte.Debenic += Re_Hijas.decC13;
                            Reporte.Habernic += Re_Hijas.decC14;
                            Reporte.DebeEs2 += Re_Hijas.decC15;
                            Reporte.HaberEs2 += Re_Hijas.decC16;
                            Reporte.Debegrh += Re_Hijas.decC17;
                            Reporte.Habergrh += Re_Hijas.decC18;
                            Reporte.Debeisi += Re_Hijas.decC19;
                            Reporte.Haberisi += Re_Hijas.decC20;
                            Reporte.Debemayan += Re_Hijas.decC21;
                            Reporte.Habermayan += Re_Hijas.decC22;
                            Reporte.Debebc += Re_Hijas.decC23;
                            Reporte.Haberbc += Re_Hijas.decC24;
                        }
                        total = Reporte.Haber - Reporte.Debe;
                        totalgt = Reporte.Habergt - Reporte.Debegt;
                        totales = Reporte.HaberEs - Reporte.DebeEs;
                        totales2 = Reporte.HaberEs2 - Reporte.DebeEs2;
                        totalhn = Reporte.HaberHn - Reporte.DebeHn;
                        totalnic = Reporte.Habernic - Reporte.Debenic;
                        totalgrh = Reporte.Habergrh - Reporte.Debegrh;
                        totalisi = Reporte.Haberisi - Reporte.Debeisi;
                        totalmayan = Reporte.Habermayan - Reporte.Debemayan;
                        totalcr = Reporte.HaberCr - Reporte.DebeCr;
                        totalpn = Reporte.HaberPr - Reporte.DebePr;
                        totalbc = Reporte.Haberbc - Reporte.Debebc;
                        totalcapital += total;
                        object[] objArr = { Reporte.Cue_Id, "", "", "", Reporte.Nombre, total, "CAPITAL", Reporte.Cue_Nivel, totalgt, totales, totales2, totalhn, totalnic, totalgrh, totalisi, totalmayan, totalcr, totalpn, totalbc, i == 1 ? Params.logo : null, i == 1 ? Params.titulo : "", i == 1 ? Params.edicion : "" };
                        ds.Tables["balancegeneralregional"].Rows.Add(objArr);
                    }
                }
            }


            if (i == 0) {
                object[] objArr = { "", "", "", "", "", 0, "", "", 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, Params.logo, Params.titulo, Params.edicion };                
                ds.Tables["balancegeneralregional"].Rows.Add(objArr);
            }


            //Definir ViewState
            ViewState["balancegeneralregional"] = ds;
            pasivo_capital = totalpasivo + totalcapital;
            lbl_pasivo_capital.Text = pasivo_capital.ToString();
            rpt = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
            rpt.Load(Server.MapPath("~/CR_BGRegional.rpt"));
            rpt.SetDataSource(ds.Tables["balancegeneralregional"]);
            //rpt.SetParameterValue("moneda", Utility.TraducirMonedaInt(monID));
            //if (contaID == 1)
            //{
            //    rpt.SetParameterValue("contabilidad", "FISCAL");
            //}
            //else if (contaID == 2)
            //{
            //    rpt.SetParameterValue("contabilidad", "FINANCIERA");
            //}
            //else if (contaID == 3)
            //{
            //    rpt.SetParameterValue("contabilidad", "CONSOLIDADO");
            //}
            //rpt.SetParameterValue("pais", user.pais.Nombre);
            //rpt.SetParameterValue("Tipo_Reporte", Tipo_Reporte);
            //if (Tipo_Reporte == "1")
            //{
            //    rpt.SetParameterValue("fecha_inicio", fecha_inicial);
            //    rpt.SetParameterValue("fecha_fin", fecha_final);
            //}
            //else
            //{
            //    rpt.SetParameterValue("fecha_inicio", fecha_inicial);
            //    rpt.SetParameterValue("fecha_fin", fecha_final);
            //}
            rpt.SetParameterValue("Resultado_del_Ejercicio", Resultado_Ejercicio);
            rpt.SetParameterValue("Resultado_del_Ejerciciogt", Resultado_Ejerciciogt);
            rpt.SetParameterValue("Resultado_del_Ejercicioes", Resultado_Ejercicioes);
            rpt.SetParameterValue("Resultado_del_Ejercicioes2", Resultado_Ejercicioes2);
            rpt.SetParameterValue("Resultado_del_Ejerciciohn", Resultado_Ejerciciohn);
            rpt.SetParameterValue("Resultado_del_Ejercicionic", Resultado_Ejercicionic);
            rpt.SetParameterValue("Resultado_del_Ejerciciocr", Resultado_Ejerciciocr);
            rpt.SetParameterValue("Resultado_del_Ejerciciopn", Resultado_Ejerciciopn);
            rpt.SetParameterValue("Resultado_del_Ejerciciobs", Resultado_Ejerciciobs);
            rpt.SetParameterValue("Resultado_del_Ejerciciogrh", Resultado_Ejerciciogrh);
            rpt.SetParameterValue("Resultado_del_Ejercicioisi", Resultado_Ejercicioisi);
            rpt.SetParameterValue("Resultado_del_Ejerciciomayan", Resultado_Ejerciciomayan);
            rpt.SetParameterValue("fecha_inicial", fecha_inicial);
            rpt.SetParameterValue("fecha_final", fecha_final);
            if (tipo == 1)
            {
                tipo_reporte = "ACUMULADO";
            }
            else
            {
                tipo_reporte = "ACOTADO";
            }
            rpt.SetParameterValue("Tipo_Reporte", tipo_reporte);
            //rpt.SetParameterValue("contaID", contaID);
            //rpt.SetParameterValue("monID", monID);
            //rpt.SetParameterValue("empresa", Server.MapPath(user.pais.Imagepath));
        if (tipo == 1)
        {
            rpt.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.Excel, "C:/BAWReportBSyER/Balance_General_Regional_Completo.xls");
        }
        else if (tipo == 2)
        {
            rpt.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.Excel, "C:/BAWReportBSyER/Balance_General_Regional_Acotado.xls");
        }
            DB.CreateMessageWithAttachment(archivo,0);
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