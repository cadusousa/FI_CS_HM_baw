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

public partial class Reports_balance_saldos : System.Web.UI.Page
{
    UsuarioBean user = null;
    ArrayList arr = null;
    ArrayList Arr_Balance = new ArrayList();
    ArrayList Arr_Aux = new ArrayList();
    RE_GenericBean Balance_Bean = null;
    RE_GenericBean Bean_aux = null;
    string fecha_inicial = "";
    string fecha_final = "";
    string consmonedafiscal = "";
    string consolidarrep = "";
    int monID = 0;
    int contaID = 0;
    int anio = 0;
    int mes = 0;
    decimal Saldo_Inicial = 0, Saldo_Final = 0;
    CrystalDecisions.CrystalReports.Engine.ReportDocument rpt;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["usuario"] == null)
        {
            Response.Redirect("../default.aspx");
        }
        user = (UsuarioBean)Session["usuario"];
        LibroDiarioDS ds = new LibroDiarioDS();
        fecha_inicial = Request.QueryString["fecha_inicial"].ToString();
        fecha_final = Request.QueryString["fecha_final"].ToString();
        consmonedafiscal = Request.QueryString["consmonefis"].ToString();
        monID = int.Parse(Request.QueryString["monID"].ToString());
        contaID = int.Parse(Request.QueryString["tcon"].ToString());

        if (consmonedafiscal == "si")
        {
            consolidarrep = "CONSOLIDADO";
        }

        #region Fechas Saldo Inicial
        mes = int.Parse(fecha_inicial.Substring(0, 2));
        anio = int.Parse(fecha_final.Substring(6, 4));
        if (mes == 1)
        {
            anio = anio - 1;
            mes = 12;
        }
        else
        {
            mes = mes - 1;
        }
        #endregion
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
        decimal tc = DB.getTipoCambioXFecha(user.PaisID, fecha_final);
        if (!IsPostBack)
        {
            #region Generar Reporte
            arr = (ArrayList)DB.getBalance_Saldos(monID, contaID, user, mes, anio, fecha_inicial, fecha_final, consmonedafiscal);
            foreach (ReportBean Reporte in arr)
            {
                if (Reporte.Cue_Clasificacion == 1)
                {
                    #region Activo
                    if (Reporte.Cue_Nivel == 1)
                    {
                        Saldo_Inicial = Reporte.Saldo_Inicial_Debe - Reporte.Saldo_Inicial_Haber;
                        Saldo_Final = (Reporte.Debe - Reporte.Haber)+ (Saldo_Inicial);
                        //object[] ObjArr = { Reporte.Cue_Id, Reporte.Nombre, "", "", "", "", Saldo_Inicial, Reporte.Debe, Reporte.Haber, Saldo_Final, "Activo", Reporte.Cue_Nivel };
                        //ds.Tables["Balance_Saldos"].Rows.Add(ObjArr);
                        #region Crear Balance Bean
                        Balance_Bean = new RE_GenericBean();
                        Balance_Bean.strC1 = Reporte.Cue_Id;
                        Balance_Bean.strC2 = Reporte.Nombre;
                        Balance_Bean.strC3 = "";
                        Balance_Bean.strC4 = "";
                        Balance_Bean.strC5 = "";
                        Balance_Bean.strC6 = "";
                        Balance_Bean.decC1 = Saldo_Inicial;
                        Balance_Bean.decC2 = Reporte.Debe;
                        Balance_Bean.decC3 = Reporte.Haber;
                        Balance_Bean.decC4 = Saldo_Final;
                        Balance_Bean.strC7 = "Activo";
                        Balance_Bean.intC1 = Reporte.Cue_Nivel;
                        Balance_Bean.strC8 = Reporte.Cue_Madre;
                        Arr_Balance.Add(Balance_Bean);
                        #endregion
                    }
                    else if (Reporte.Cue_Nivel == 2)
                    {
                        Saldo_Inicial = Reporte.Saldo_Inicial_Debe - Reporte.Saldo_Inicial_Haber;
                        Saldo_Final = (Reporte.Debe - Reporte.Haber) + (Saldo_Inicial);
                        //object[] ObjArr = { Reporte.Cue_Id, "", Reporte.Nombre, "", "", "", Saldo_Inicial, Reporte.Debe, Reporte.Haber, Saldo_Final, "Activo", Reporte.Cue_Nivel };
                        //ds.Tables["Balance_Saldos"].Rows.Add(ObjArr);
                        #region Crear Balance Bean
                        Balance_Bean = new RE_GenericBean();
                        Balance_Bean.strC1 = Reporte.Cue_Id;
                        Balance_Bean.strC2 = "";
                        Balance_Bean.strC3 = Reporte.Nombre;
                        Balance_Bean.strC4 = "";
                        Balance_Bean.strC5 = "";
                        Balance_Bean.strC6 = "";
                        Balance_Bean.decC1 = Saldo_Inicial;
                        Balance_Bean.decC2 = Reporte.Debe;
                        Balance_Bean.decC3 = Reporte.Haber;
                        Balance_Bean.decC4 = Saldo_Final;
                        Balance_Bean.strC7 = "Activo";
                        Balance_Bean.intC1 = Reporte.Cue_Nivel;
                        Balance_Bean.strC8 = Reporte.Cue_Madre;
                        Arr_Balance.Add(Balance_Bean);
                        #endregion
                    }
                    else if (Reporte.Cue_Nivel == 3)
                    {
                        Saldo_Inicial = Reporte.Saldo_Inicial_Debe - Reporte.Saldo_Inicial_Haber;
                        Saldo_Final = (Reporte.Debe - Reporte.Haber) + (Saldo_Inicial);
                        //object[] ObjArr = { Reporte.Cue_Id, "", "", Reporte.Nombre, "", "", Saldo_Inicial, Reporte.Debe, Reporte.Haber, Saldo_Final, "Activo", Reporte.Cue_Nivel };
                        //ds.Tables["Balance_Saldos"].Rows.Add(ObjArr);
                        #region Crear Balance Bean
                        Balance_Bean = new RE_GenericBean();
                        Balance_Bean.strC1 = Reporte.Cue_Id;
                        Balance_Bean.strC2 = "";
                        Balance_Bean.strC3 = "";
                        Balance_Bean.strC4 = Reporte.Nombre;
                        Balance_Bean.strC5 = "";
                        Balance_Bean.strC6 = "";
                        Balance_Bean.decC1 = Saldo_Inicial;
                        Balance_Bean.decC2 = Reporte.Debe;
                        Balance_Bean.decC3 = Reporte.Haber;
                        Balance_Bean.decC4 = Saldo_Final;
                        Balance_Bean.strC7 = "Activo";
                        Balance_Bean.intC1 = Reporte.Cue_Nivel;
                        Balance_Bean.strC8 = Reporte.Cue_Madre;
                        Arr_Balance.Add(Balance_Bean);
                        #endregion
                    }
                    else if (Reporte.Cue_Nivel == 4)
                    {
                        Saldo_Inicial = Reporte.Saldo_Inicial_Debe - Reporte.Saldo_Inicial_Haber;
                        Saldo_Final = (Reporte.Debe - Reporte.Haber) + (Saldo_Inicial);
                        //object[] ObjArr = { Reporte.Cue_Id, "", "", "", Reporte.Nombre, "", Saldo_Inicial, Reporte.Debe, Reporte.Haber, Saldo_Final, "Activo", Reporte.Cue_Nivel };
                        //ds.Tables["Balance_Saldos"].Rows.Add(ObjArr);
                        #region Crear Balance Bean
                        Balance_Bean = new RE_GenericBean();
                        Balance_Bean.strC1 = Reporte.Cue_Id;
                        Balance_Bean.strC2 = "";
                        Balance_Bean.strC3 = "";
                        Balance_Bean.strC4 = "";
                        Balance_Bean.strC5 = Reporte.Nombre;
                        Balance_Bean.strC6 = "";
                        Balance_Bean.decC1 = Saldo_Inicial;
                        Balance_Bean.decC2 = Reporte.Debe;
                        Balance_Bean.decC3 = Reporte.Haber;
                        Balance_Bean.decC4 = Saldo_Final;
                        Balance_Bean.strC7 = "Activo";
                        Balance_Bean.intC1 = Reporte.Cue_Nivel;
                        Balance_Bean.strC8 = Reporte.Cue_Madre;
                        Arr_Balance.Add(Balance_Bean);
                        #endregion
                    }
                    else if (Reporte.Cue_Nivel == 5)
                    {
                        Saldo_Inicial = Reporte.Saldo_Inicial_Debe - Reporte.Saldo_Inicial_Haber;
                        Saldo_Final = (Reporte.Debe - Reporte.Haber) + (Saldo_Inicial);
                        //object[] ObjArr = { Reporte.Cue_Id, "", "", "", "", Reporte.Nombre, Saldo_Inicial, Reporte.Debe, Reporte.Haber, Saldo_Final, "Activo", Reporte.Cue_Nivel };
                        //ds.Tables["Balance_Saldos"].Rows.Add(ObjArr);
                        #region Crear Balance Bean
                        Balance_Bean = new RE_GenericBean();
                        Balance_Bean.strC1 = Reporte.Cue_Id;
                        Balance_Bean.strC2 = "";
                        Balance_Bean.strC3 = "";
                        Balance_Bean.strC4 = "";
                        Balance_Bean.strC5 = "";
                        Balance_Bean.strC6 = Reporte.Nombre;
                        Balance_Bean.decC1 = Saldo_Inicial;
                        Balance_Bean.decC2 = Reporte.Debe;
                        Balance_Bean.decC3 = Reporte.Haber;
                        Balance_Bean.decC4 = Saldo_Final;
                        Balance_Bean.strC7 = "Activo";
                        Balance_Bean.intC1 = Reporte.Cue_Nivel;
                        Balance_Bean.strC8 = Reporte.Cue_Madre;
                        Arr_Balance.Add(Balance_Bean);
                        #endregion
                    }
                    #endregion
                }
                else if (Reporte.Cue_Clasificacion == 2)//Pasivo
                {
                    #region Pasivo
                    if (Reporte.Cue_Nivel == 1)
                    {
                        Saldo_Inicial = Reporte.Saldo_Inicial_Haber - Reporte.Saldo_Inicial_Debe;
                        Saldo_Final = (Reporte.Haber - Reporte.Debe) + (Saldo_Inicial);
                        //object[] ObjArr = { Reporte.Cue_Id, Reporte.Nombre, "", "", "", "", Saldo_Inicial, Reporte.Debe, Reporte.Haber, Saldo_Final, "Pasivo", Reporte.Cue_Nivel };
                        //ds.Tables["Balance_Saldos"].Rows.Add(ObjArr);
                        #region Crear Balance Bean
                        Balance_Bean = new RE_GenericBean();
                        Balance_Bean.strC1 = Reporte.Cue_Id;
                        Balance_Bean.strC2 = Reporte.Nombre;
                        Balance_Bean.strC3 = "";
                        Balance_Bean.strC4 = "";
                        Balance_Bean.strC5 = "";
                        Balance_Bean.strC6 = "";
                        Balance_Bean.decC1 = Saldo_Inicial;
                        Balance_Bean.decC2 = Reporte.Debe;
                        Balance_Bean.decC3 = Reporte.Haber;
                        Balance_Bean.decC4 = Saldo_Final;
                        Balance_Bean.strC7 = "Pasivo";
                        Balance_Bean.intC1 = Reporte.Cue_Nivel;
                        Balance_Bean.strC8 = Reporte.Cue_Madre;
                        Arr_Balance.Add(Balance_Bean);
                        #endregion
                    }
                    else if (Reporte.Cue_Nivel == 2)
                    {
                        Saldo_Inicial = Reporte.Saldo_Inicial_Haber - Reporte.Saldo_Inicial_Debe;
                        Saldo_Final = (Reporte.Haber - Reporte.Debe) + (Saldo_Inicial);
                        //object[] ObjArr = { Reporte.Cue_Id, "", Reporte.Nombre, "", "", "", Saldo_Inicial, Reporte.Debe, Reporte.Haber, Saldo_Final, "Pasivo", Reporte.Cue_Nivel };
                        //ds.Tables["Balance_Saldos"].Rows.Add(ObjArr);
                        #region Crear Balance Bean
                        Balance_Bean = new RE_GenericBean();
                        Balance_Bean.strC1 = Reporte.Cue_Id;
                        Balance_Bean.strC2 = "";
                        Balance_Bean.strC3 = Reporte.Nombre;
                        Balance_Bean.strC4 = "";
                        Balance_Bean.strC5 = "";
                        Balance_Bean.strC6 = "";
                        Balance_Bean.decC1 = Saldo_Inicial;
                        Balance_Bean.decC2 = Reporte.Debe;
                        Balance_Bean.decC3 = Reporte.Haber;
                        Balance_Bean.decC4 = Saldo_Final;
                        Balance_Bean.strC7 = "Pasivo";
                        Balance_Bean.intC1 = Reporte.Cue_Nivel;
                        Balance_Bean.strC8 = Reporte.Cue_Madre;
                        Arr_Balance.Add(Balance_Bean);
                        #endregion
                    }
                    else if (Reporte.Cue_Nivel == 3)
                    {
                        Saldo_Inicial = Reporte.Saldo_Inicial_Haber - Reporte.Saldo_Inicial_Debe;
                        Saldo_Final = (Reporte.Haber - Reporte.Debe) + (Saldo_Inicial);
                        //object[] ObjArr = { Reporte.Cue_Id, "", "", Reporte.Nombre, "", "", Saldo_Inicial, Reporte.Debe, Reporte.Haber, Saldo_Final, "Pasivo", Reporte.Cue_Nivel };
                        //ds.Tables["Balance_Saldos"].Rows.Add(ObjArr);
                        #region Crear Balance Bean
                        Balance_Bean = new RE_GenericBean();
                        Balance_Bean.strC1 = Reporte.Cue_Id;
                        Balance_Bean.strC2 = "";
                        Balance_Bean.strC3 = "";
                        Balance_Bean.strC4 = Reporte.Nombre;
                        Balance_Bean.strC5 = "";
                        Balance_Bean.strC6 = "";
                        Balance_Bean.decC1 = Saldo_Inicial;
                        Balance_Bean.decC2 = Reporte.Debe;
                        Balance_Bean.decC3 = Reporte.Haber;
                        Balance_Bean.decC4 = Saldo_Final;
                        Balance_Bean.strC7 = "Pasivo";
                        Balance_Bean.intC1 = Reporte.Cue_Nivel;
                        Balance_Bean.strC8 = Reporte.Cue_Madre;
                        Arr_Balance.Add(Balance_Bean);
                        #endregion
                    }
                    else if (Reporte.Cue_Nivel == 4)
                    {
                        Saldo_Inicial = Reporte.Saldo_Inicial_Haber - Reporte.Saldo_Inicial_Debe;
                        Saldo_Final = (Reporte.Haber - Reporte.Debe) + (Saldo_Inicial);
                        //object[] ObjArr = { Reporte.Cue_Id, "", "", "", Reporte.Nombre, "", Saldo_Inicial, Reporte.Debe, Reporte.Haber, Saldo_Final, "Pasivo", Reporte.Cue_Nivel };
                        //ds.Tables["Balance_Saldos"].Rows.Add(ObjArr);
                        #region Crear Balance Bean
                        Balance_Bean = new RE_GenericBean();
                        Balance_Bean.strC1 = Reporte.Cue_Id;
                        Balance_Bean.strC2 = "";
                        Balance_Bean.strC3 = "";
                        Balance_Bean.strC4 = "";
                        Balance_Bean.strC5 = Reporte.Nombre;
                        Balance_Bean.strC6 = "";
                        Balance_Bean.decC1 = Saldo_Inicial;
                        Balance_Bean.decC2 = Reporte.Debe;
                        Balance_Bean.decC3 = Reporte.Haber;
                        Balance_Bean.decC4 = Saldo_Final;
                        Balance_Bean.strC7 = "Pasivo";
                        Balance_Bean.intC1 = Reporte.Cue_Nivel;
                        Balance_Bean.strC8 = Reporte.Cue_Madre;
                        Arr_Balance.Add(Balance_Bean);
                        #endregion
                    }
                    else if (Reporte.Cue_Nivel == 5)
                    {
                        Saldo_Inicial = Reporte.Saldo_Inicial_Haber - Reporte.Saldo_Inicial_Debe;
                        Saldo_Final = (Reporte.Haber - Reporte.Debe) + (Saldo_Inicial);
                        //object[] ObjArr = { Reporte.Cue_Id, "", "", "", "", Reporte.Nombre, Saldo_Inicial, Reporte.Debe, Reporte.Haber, Saldo_Final, "Pasivo", Reporte.Cue_Nivel };
                        //ds.Tables["Balance_Saldos"].Rows.Add(ObjArr);
                        #region Crear Balance Bean
                        Balance_Bean = new RE_GenericBean();
                        Balance_Bean.strC1 = Reporte.Cue_Id;
                        Balance_Bean.strC2 = "";
                        Balance_Bean.strC3 = "";
                        Balance_Bean.strC4 = "";
                        Balance_Bean.strC5 = "";
                        Balance_Bean.strC6 = Reporte.Nombre;
                        Balance_Bean.decC1 = Saldo_Inicial;
                        Balance_Bean.decC2 = Reporte.Debe;
                        Balance_Bean.decC3 = Reporte.Haber;
                        Balance_Bean.decC4 = Saldo_Final;
                        Balance_Bean.strC7 = "Pasivo";
                        Balance_Bean.intC1 = Reporte.Cue_Nivel;
                        Balance_Bean.strC8 = Reporte.Cue_Madre;
                        Arr_Balance.Add(Balance_Bean);
                        #endregion
                    }
                    #endregion
                }
                else if (Reporte.Cue_Clasificacion == 3)//Ingreso
                {
                    #region Ingresos
                    if (Reporte.Cue_Nivel == 1)
                    {
                        Saldo_Inicial = Reporte.Saldo_Inicial_Haber - Reporte.Saldo_Inicial_Debe;
                        Saldo_Final = (Reporte.Haber - Reporte.Debe) + (Saldo_Inicial);
                        //object[] ObjArr = { Reporte.Cue_Id, Reporte.Nombre, "", "", "", "", Saldo_Inicial, Reporte.Debe, Reporte.Haber, Saldo_Final, "Ingresos", Reporte.Cue_Nivel };
                        //ds.Tables["Balance_Saldos"].Rows.Add(ObjArr);
                        #region Crear Balance Bean
                        Balance_Bean = new RE_GenericBean();
                        Balance_Bean.strC1 = Reporte.Cue_Id;
                        Balance_Bean.strC2 = Reporte.Nombre;
                        Balance_Bean.strC3 = "";
                        Balance_Bean.strC4 = "";
                        Balance_Bean.strC5 = "";
                        Balance_Bean.strC6 = "";
                        Balance_Bean.decC1 = Saldo_Inicial;
                        Balance_Bean.decC2 = Reporte.Debe;
                        Balance_Bean.decC3 = Reporte.Haber;
                        Balance_Bean.decC4 = Saldo_Final;
                        Balance_Bean.strC7 = "Ingresos";
                        Balance_Bean.intC1 = Reporte.Cue_Nivel;
                        Balance_Bean.strC8 = Reporte.Cue_Madre;
                        Arr_Balance.Add(Balance_Bean);
                        #endregion
                    }
                    else if (Reporte.Cue_Nivel == 2)
                    {
                        Saldo_Inicial = Reporte.Saldo_Inicial_Haber - Reporte.Saldo_Inicial_Debe;
                        Saldo_Final = (Reporte.Haber - Reporte.Debe) + (Saldo_Inicial);
                        //object[] ObjArr = { Reporte.Cue_Id, "", Reporte.Nombre, "", "", "", Saldo_Inicial, Reporte.Debe, Reporte.Haber, Saldo_Final, "Ingresos", Reporte.Cue_Nivel };
                        //ds.Tables["Balance_Saldos"].Rows.Add(ObjArr);
                        #region Crear Balance Bean
                        Balance_Bean = new RE_GenericBean();
                        Balance_Bean.strC1 = Reporte.Cue_Id;
                        Balance_Bean.strC2 = "";
                        Balance_Bean.strC3 = Reporte.Nombre;
                        Balance_Bean.strC4 = "";
                        Balance_Bean.strC5 = "";
                        Balance_Bean.strC6 = "";
                        Balance_Bean.decC1 = Saldo_Inicial;
                        Balance_Bean.decC2 = Reporte.Debe;
                        Balance_Bean.decC3 = Reporte.Haber;
                        Balance_Bean.decC4 = Saldo_Final;
                        Balance_Bean.strC7 = "Ingresos";
                        Balance_Bean.intC1 = Reporte.Cue_Nivel;
                        Balance_Bean.strC8 = Reporte.Cue_Madre;
                        Arr_Balance.Add(Balance_Bean);
                        #endregion
                    }
                    else if (Reporte.Cue_Nivel == 3)
                    {
                        Saldo_Inicial = Reporte.Saldo_Inicial_Haber - Reporte.Saldo_Inicial_Debe;
                        Saldo_Final = (Reporte.Haber - Reporte.Debe) + (Saldo_Inicial);
                        //object[] ObjArr = { Reporte.Cue_Id, "", "", Reporte.Nombre, "", "", Saldo_Inicial, Reporte.Debe, Reporte.Haber, Saldo_Final, "Ingresos", Reporte.Cue_Nivel };
                        //ds.Tables["Balance_Saldos"].Rows.Add(ObjArr);
                        #region Crear Balance Bean
                        Balance_Bean = new RE_GenericBean();
                        Balance_Bean.strC1 = Reporte.Cue_Id;
                        Balance_Bean.strC2 = "";
                        Balance_Bean.strC3 = "";
                        Balance_Bean.strC4 = Reporte.Nombre;
                        Balance_Bean.strC5 = "";
                        Balance_Bean.strC6 = "";
                        Balance_Bean.decC1 = Saldo_Inicial;
                        Balance_Bean.decC2 = Reporte.Debe;
                        Balance_Bean.decC3 = Reporte.Haber;
                        Balance_Bean.decC4 = Saldo_Final;
                        Balance_Bean.strC7 = "Ingresos";
                        Balance_Bean.intC1 = Reporte.Cue_Nivel;
                        Balance_Bean.strC8 = Reporte.Cue_Madre;
                        Arr_Balance.Add(Balance_Bean);
                        #endregion
                    }
                    else if (Reporte.Cue_Nivel == 4)
                    {
                        Saldo_Inicial = Reporte.Saldo_Inicial_Haber - Reporte.Saldo_Inicial_Debe;
                        Saldo_Final = (Reporte.Haber - Reporte.Debe) + (Saldo_Inicial);
                        //object[] ObjArr = { Reporte.Cue_Id, "", "", "", Reporte.Nombre, "", Saldo_Inicial, Reporte.Debe, Reporte.Haber, Saldo_Final, "Ingresos", Reporte.Cue_Nivel };
                        //ds.Tables["Balance_Saldos"].Rows.Add(ObjArr);
                        #region Crear Balance Bean
                        Balance_Bean = new RE_GenericBean();
                        Balance_Bean.strC1 = Reporte.Cue_Id;
                        Balance_Bean.strC2 = "";
                        Balance_Bean.strC3 = "";
                        Balance_Bean.strC4 = "";
                        Balance_Bean.strC5 = Reporte.Nombre;
                        Balance_Bean.strC6 = "";
                        Balance_Bean.decC1 = Saldo_Inicial;
                        Balance_Bean.decC2 = Reporte.Debe;
                        Balance_Bean.decC3 = Reporte.Haber;
                        Balance_Bean.decC4 = Saldo_Final;
                        Balance_Bean.strC7 = "Ingresos";
                        Balance_Bean.intC1 = Reporte.Cue_Nivel;
                        Balance_Bean.strC8 = Reporte.Cue_Madre;
                        Arr_Balance.Add(Balance_Bean);
                        #endregion
                    }
                    else if (Reporte.Cue_Nivel == 5)
                    {
                        Saldo_Inicial = Reporte.Saldo_Inicial_Haber - Reporte.Saldo_Inicial_Debe;
                        Saldo_Final = (Reporte.Haber - Reporte.Debe) + (Saldo_Inicial);
                        //object[] ObjArr = { Reporte.Cue_Id, "", "", "", "", Reporte.Nombre, Saldo_Inicial, Reporte.Debe, Reporte.Haber, Saldo_Final, "Ingresos", Reporte.Cue_Nivel };
                        //ds.Tables["Balance_Saldos"].Rows.Add(ObjArr);
                        #region Crear Balance Bean
                        Balance_Bean = new RE_GenericBean();
                        Balance_Bean.strC1 = Reporte.Cue_Id;
                        Balance_Bean.strC2 = "";
                        Balance_Bean.strC3 = "";
                        Balance_Bean.strC4 = "";
                        Balance_Bean.strC5 = "";
                        Balance_Bean.strC6 = Reporte.Nombre;
                        Balance_Bean.decC1 = Saldo_Inicial;
                        Balance_Bean.decC2 = Reporte.Debe;
                        Balance_Bean.decC3 = Reporte.Haber;
                        Balance_Bean.decC4 = Saldo_Final;
                        Balance_Bean.strC7 = "Ingresos";
                        Balance_Bean.intC1 = Reporte.Cue_Nivel;
                        Balance_Bean.strC8 = Reporte.Cue_Madre;
                        Arr_Balance.Add(Balance_Bean);
                        #endregion
                    }
                    #endregion
                }
                else if (Reporte.Cue_Clasificacion == 4)//Egreso
                {
                    #region Egresos
                    if (Reporte.Cue_Nivel == 1)
                    {
                        Saldo_Inicial = Reporte.Saldo_Inicial_Debe - Reporte.Saldo_Inicial_Haber;
                        Saldo_Final = (Reporte.Debe - Reporte.Haber) + (Saldo_Inicial);
                        //object[] ObjArr = { Reporte.Cue_Id, Reporte.Nombre, "", "", "", "", Saldo_Inicial, Reporte.Debe, Reporte.Haber, Saldo_Final, "Egresos", Reporte.Cue_Nivel };
                        //ds.Tables["Balance_Saldos"].Rows.Add(ObjArr);
                        #region Crear Balance Bean
                        Balance_Bean = new RE_GenericBean();
                        Balance_Bean.strC1 = Reporte.Cue_Id;
                        Balance_Bean.strC2 = Reporte.Nombre;
                        Balance_Bean.strC3 = "";
                        Balance_Bean.strC4 = "";
                        Balance_Bean.strC5 = "";
                        Balance_Bean.strC6 = "";
                        Balance_Bean.decC1 = Saldo_Inicial;
                        Balance_Bean.decC2 = Reporte.Debe;
                        Balance_Bean.decC3 = Reporte.Haber;
                        Balance_Bean.decC4 = Saldo_Final;
                        Balance_Bean.strC7 = "Egresos";
                        Balance_Bean.intC1 = Reporte.Cue_Nivel;
                        Balance_Bean.strC8 = Reporte.Cue_Madre;
                        Arr_Balance.Add(Balance_Bean);
                        #endregion
                    }
                    else if (Reporte.Cue_Nivel == 2)
                    {
                        Saldo_Inicial = Reporte.Saldo_Inicial_Debe - Reporte.Saldo_Inicial_Haber;
                        Saldo_Final = (Reporte.Debe - Reporte.Haber) + (Saldo_Inicial);
                        //object[] ObjArr = { Reporte.Cue_Id, "", Reporte.Nombre, "", "", "", Saldo_Inicial, Reporte.Debe, Reporte.Haber, Saldo_Final, "Egresos", Reporte.Cue_Nivel };
                        //ds.Tables["Balance_Saldos"].Rows.Add(ObjArr);
                        #region Crear Balance Bean
                        Balance_Bean = new RE_GenericBean();
                        Balance_Bean.strC1 = Reporte.Cue_Id;
                        Balance_Bean.strC2 = "";
                        Balance_Bean.strC3 = Reporte.Nombre;
                        Balance_Bean.strC4 = "";
                        Balance_Bean.strC5 = "";
                        Balance_Bean.strC6 = "";
                        Balance_Bean.decC1 = Saldo_Inicial;
                        Balance_Bean.decC2 = Reporte.Debe;
                        Balance_Bean.decC3 = Reporte.Haber;
                        Balance_Bean.decC4 = Saldo_Final;
                        Balance_Bean.strC7 = "Egresos";
                        Balance_Bean.intC1 = Reporte.Cue_Nivel;
                        Balance_Bean.strC8 = Reporte.Cue_Madre;
                        Arr_Balance.Add(Balance_Bean);
                        #endregion
                    }
                    else if (Reporte.Cue_Nivel == 3)
                    {
                        Saldo_Inicial = Reporte.Saldo_Inicial_Debe - Reporte.Saldo_Inicial_Haber;
                        Saldo_Final = (Reporte.Debe - Reporte.Haber) + (Saldo_Inicial);
                        //object[] ObjArr = { Reporte.Cue_Id, "", "", Reporte.Nombre, "", "", Saldo_Inicial, Reporte.Debe, Reporte.Haber, Saldo_Final, "Egresos", Reporte.Cue_Nivel };
                        //ds.Tables["Balance_Saldos"].Rows.Add(ObjArr);
                        #region Crear Balance Bean
                        Balance_Bean = new RE_GenericBean();
                        Balance_Bean.strC1 = Reporte.Cue_Id;
                        Balance_Bean.strC2 = "";
                        Balance_Bean.strC3 = "";
                        Balance_Bean.strC4 = Reporte.Nombre;
                        Balance_Bean.strC5 = "";
                        Balance_Bean.strC6 = "";
                        Balance_Bean.decC1 = Saldo_Inicial;
                        Balance_Bean.decC2 = Reporte.Debe;
                        Balance_Bean.decC3 = Reporte.Haber;
                        Balance_Bean.decC4 = Saldo_Final;
                        Balance_Bean.strC7 = "Egresos";
                        Balance_Bean.intC1 = Reporte.Cue_Nivel;
                        Balance_Bean.strC8 = Reporte.Cue_Madre;
                        Arr_Balance.Add(Balance_Bean);
                        #endregion
                    }
                    else if (Reporte.Cue_Nivel == 4)
                    {
                        Saldo_Inicial = Reporte.Saldo_Inicial_Debe - Reporte.Saldo_Inicial_Haber;
                        Saldo_Final = (Reporte.Debe - Reporte.Haber) + (Saldo_Inicial);
                        //object[] ObjArr = { Reporte.Cue_Id, "", "", "", Reporte.Nombre, "", Saldo_Inicial, Reporte.Debe, Reporte.Haber, Saldo_Final, "Egresos", Reporte.Cue_Nivel };
                        //ds.Tables["Balance_Saldos"].Rows.Add(ObjArr);
                        #region Crear Balance Bean
                        Balance_Bean = new RE_GenericBean();
                        Balance_Bean.strC1 = Reporte.Cue_Id;
                        Balance_Bean.strC2 = "";
                        Balance_Bean.strC3 = "";
                        Balance_Bean.strC4 = "";
                        Balance_Bean.strC5 = Reporte.Nombre;
                        Balance_Bean.strC6 = "";
                        Balance_Bean.decC1 = Saldo_Inicial;
                        Balance_Bean.decC2 = Reporte.Debe;
                        Balance_Bean.decC3 = Reporte.Haber;
                        Balance_Bean.decC4 = Saldo_Final;
                        Balance_Bean.strC7 = "Egresos";
                        Balance_Bean.intC1 = Reporte.Cue_Nivel;
                        Balance_Bean.strC8 = Reporte.Cue_Madre;
                        Arr_Balance.Add(Balance_Bean);
                        #endregion
                    }
                    else if (Reporte.Cue_Nivel == 5)
                    {
                        Saldo_Inicial = Reporte.Saldo_Inicial_Debe - Reporte.Saldo_Inicial_Haber;
                        Saldo_Final = (Reporte.Debe - Reporte.Haber) + (Saldo_Inicial);
                        //object[] ObjArr = { Reporte.Cue_Id, "", "", "", "", Reporte.Nombre, Saldo_Inicial, Reporte.Debe, Reporte.Haber, Saldo_Final, "Egresos", Reporte.Cue_Nivel };
                        //ds.Tables["Balance_Saldos"].Rows.Add(ObjArr);
                        #region Crear Balance Bean
                        Balance_Bean = new RE_GenericBean();
                        Balance_Bean.strC1 = Reporte.Cue_Id;
                        Balance_Bean.strC2 = "";
                        Balance_Bean.strC3 = "";
                        Balance_Bean.strC4 = "";
                        Balance_Bean.strC5 = "";
                        Balance_Bean.strC6 = Reporte.Nombre;
                        Balance_Bean.decC1 = Saldo_Inicial;
                        Balance_Bean.decC2 = Reporte.Debe;
                        Balance_Bean.decC3 = Reporte.Haber;
                        Balance_Bean.decC4 = Saldo_Final;
                        Balance_Bean.strC7 = "Egresos";
                        Balance_Bean.intC1 = Reporte.Cue_Nivel;
                        Balance_Bean.strC8 = Reporte.Cue_Madre;
                        Arr_Balance.Add(Balance_Bean);
                        #endregion
                    }
                    #endregion
                }
                else if (Reporte.Cue_Clasificacion == 5)//Capital
                {
                    #region Capital
                    if (Reporte.Cue_Nivel == 1)
                    {
                        Saldo_Inicial = Reporte.Saldo_Inicial_Haber - Reporte.Saldo_Inicial_Debe;
                        Saldo_Final = (Reporte.Haber - Reporte.Debe) + (Saldo_Inicial);
                        //object[] ObjArr = { Reporte.Cue_Id, Reporte.Nombre, "", "", "", "", Saldo_Inicial, Reporte.Debe, Reporte.Haber, Saldo_Final, "Capital", Reporte.Cue_Nivel };
                        //ds.Tables["Balance_Saldos"].Rows.Add(ObjArr);
                        #region Crear Balance Bean
                        Balance_Bean = new RE_GenericBean();
                        Balance_Bean.strC1 = Reporte.Cue_Id;
                        Balance_Bean.strC2 = Reporte.Nombre;
                        Balance_Bean.strC3 = "";
                        Balance_Bean.strC4 = "";
                        Balance_Bean.strC5 = "";
                        Balance_Bean.strC6 = "";
                        Balance_Bean.decC1 = Saldo_Inicial;
                        Balance_Bean.decC2 = Reporte.Debe;
                        Balance_Bean.decC3 = Reporte.Haber;
                        Balance_Bean.decC4 = Saldo_Final;
                        Balance_Bean.strC7 = "Capital";
                        Balance_Bean.intC1 = Reporte.Cue_Nivel;
                        Balance_Bean.strC8 = Reporte.Cue_Madre;
                        Arr_Balance.Add(Balance_Bean);
                        #endregion
                    }
                    else if (Reporte.Cue_Nivel == 2)
                    {
                        Saldo_Inicial = Reporte.Saldo_Inicial_Haber - Reporte.Saldo_Inicial_Debe;
                        Saldo_Final = (Reporte.Haber - Reporte.Debe) + (Saldo_Inicial);
                        //object[] ObjArr = { Reporte.Cue_Id, "", Reporte.Nombre, "", "", "", Saldo_Inicial, Reporte.Debe, Reporte.Haber, Saldo_Final, "Capital", Reporte.Cue_Nivel };
                        //ds.Tables["Balance_Saldos"].Rows.Add(ObjArr);
                        #region Crear Balance Bean
                        Balance_Bean = new RE_GenericBean();
                        Balance_Bean.strC1 = Reporte.Cue_Id;
                        Balance_Bean.strC2 = "";
                        Balance_Bean.strC3 = Reporte.Nombre;
                        Balance_Bean.strC4 = "";
                        Balance_Bean.strC5 = "";
                        Balance_Bean.strC6 = "";
                        Balance_Bean.decC1 = Saldo_Inicial;
                        Balance_Bean.decC2 = Reporte.Debe;
                        Balance_Bean.decC3 = Reporte.Haber;
                        Balance_Bean.decC4 = Saldo_Final;
                        Balance_Bean.strC7 = "Capital";
                        Balance_Bean.intC1 = Reporte.Cue_Nivel;
                        Balance_Bean.strC8 = Reporte.Cue_Madre;
                        Arr_Balance.Add(Balance_Bean);
                        #endregion
                    }
                    else if (Reporte.Cue_Nivel == 3)
                    {
                        Saldo_Inicial = Reporte.Saldo_Inicial_Haber - Reporte.Saldo_Inicial_Debe;
                        Saldo_Final = (Reporte.Haber - Reporte.Debe) + (Saldo_Inicial);
                        //object[] ObjArr = { Reporte.Cue_Id, "", "", Reporte.Nombre, "", "", Saldo_Inicial, Reporte.Debe, Reporte.Haber, Saldo_Final, "Capital", Reporte.Cue_Nivel };
                        //ds.Tables["Balance_Saldos"].Rows.Add(ObjArr);
                        #region Crear Balance Bean
                        Balance_Bean = new RE_GenericBean();
                        Balance_Bean.strC1 = Reporte.Cue_Id;
                        Balance_Bean.strC2 = "";
                        Balance_Bean.strC3 = "";
                        Balance_Bean.strC4 = Reporte.Nombre;
                        Balance_Bean.strC5 = "";
                        Balance_Bean.strC6 = "";
                        Balance_Bean.decC1 = Saldo_Inicial;
                        Balance_Bean.decC2 = Reporte.Debe;
                        Balance_Bean.decC3 = Reporte.Haber;
                        Balance_Bean.decC4 = Saldo_Final;
                        Balance_Bean.strC7 = "Capital";
                        Balance_Bean.intC1 = Reporte.Cue_Nivel;
                        Balance_Bean.strC8 = Reporte.Cue_Madre;
                        Arr_Balance.Add(Balance_Bean);
                        #endregion
                    }
                    else if (Reporte.Cue_Nivel == 4)
                    {
                        Saldo_Inicial = Reporte.Saldo_Inicial_Haber - Reporte.Saldo_Inicial_Debe;
                        Saldo_Final = (Reporte.Haber - Reporte.Debe) + (Saldo_Inicial);
                        //object[] ObjArr = { Reporte.Cue_Id, "", "", "", Reporte.Nombre, "", Saldo_Inicial, Reporte.Debe, Reporte.Haber, Saldo_Final, "Capital", Reporte.Cue_Nivel };
                        //ds.Tables["Balance_Saldos"].Rows.Add(ObjArr);
                        #region Crear Balance Bean
                        Balance_Bean = new RE_GenericBean();
                        Balance_Bean.strC1 = Reporte.Cue_Id;
                        Balance_Bean.strC2 = "";
                        Balance_Bean.strC3 = "";
                        Balance_Bean.strC4 = "";
                        Balance_Bean.strC5 = Reporte.Nombre;
                        Balance_Bean.strC6 = "";
                        Balance_Bean.decC1 = Saldo_Inicial;
                        Balance_Bean.decC2 = Reporte.Debe;
                        Balance_Bean.decC3 = Reporte.Haber;
                        Balance_Bean.decC4 = Saldo_Final;
                        Balance_Bean.strC7 = "Capital";
                        Balance_Bean.intC1 = Reporte.Cue_Nivel;
                        Balance_Bean.strC8 = Reporte.Cue_Madre;
                        Arr_Balance.Add(Balance_Bean);
                        #endregion
                    }
                    else if (Reporte.Cue_Nivel == 5)
                    {
                        Saldo_Inicial = Reporte.Saldo_Inicial_Haber - Reporte.Saldo_Inicial_Debe;
                        Saldo_Final = (Reporte.Haber - Reporte.Debe) + (Saldo_Inicial);
                        //object[] ObjArr = { Reporte.Cue_Id, "", "", "", "", Reporte.Nombre, Saldo_Inicial, Reporte.Debe, Reporte.Haber, Saldo_Final, "Capital", Reporte.Cue_Nivel };
                        //ds.Tables["Balance_Saldos"].Rows.Add(ObjArr);
                        #region Crear Balance Bean
                        Balance_Bean = new RE_GenericBean();
                        Balance_Bean.strC1 = Reporte.Cue_Id;
                        Balance_Bean.strC2 = "";
                        Balance_Bean.strC3 = "";
                        Balance_Bean.strC4 = "";
                        Balance_Bean.strC5 = "";
                        Balance_Bean.strC6 = Reporte.Nombre;
                        Balance_Bean.decC1 = Saldo_Inicial;
                        Balance_Bean.decC2 = Reporte.Debe;
                        Balance_Bean.decC3 = Reporte.Haber;
                        Balance_Bean.decC4 = Saldo_Final;
                        Balance_Bean.strC7 = "Capital";
                        Balance_Bean.intC1 = Reporte.Cue_Nivel;
                        Balance_Bean.strC8 = Reporte.Cue_Madre;
                        Arr_Balance.Add(Balance_Bean);
                        #endregion
                    }
                    #endregion
                }
            }
            #endregion
            #region Generar Totales nivel 4
            //Arr_Aux = Arr_Balance;
            //for (int i = 0; i < Arr_Balance.Count;i++ )
            //{
            //    Bean_aux = (RE_GenericBean)Arr_Balance[i];
            //    if (Bean_aux.intC1 == 4)
            //    {
            //        Bean_aux.decC5 = Totalizar_Hijas(Bean_aux.strC1, Bean_aux.decC4, 4);
            //        Arr_Balance[i] = Bean_aux;
            //    }
            //}
            #endregion
            #region Generar Totales nivel 3
            //Arr_Aux = Arr_Balance;
            //for (int i = 0; i < Arr_Balance.Count; i++)
            //{
            //    Bean_aux = (RE_GenericBean)Arr_Balance[i];
            //    if (Bean_aux.intC1 == 3)
            //    {
            //        Bean_aux.decC6 = Totalizar_Hijas(Bean_aux.strC1, Bean_aux.decC4, 3);
            //        Arr_Balance[i] = Bean_aux;
            //    }
            //}
            #endregion

            string path = "balance_saldos";
            RE_GenericBean reg = DB.Traducir_reportes(user, path);

            DB.parametros Params = DB.EmpresaParametros(user.PaisID, "", "", reg.strC10, ""); //pais, sistema, doc_id, titulo, edicion
            int i = 0;
            #region Generar DataSet
            foreach (RE_GenericBean Bean in Arr_Balance)
            {
    			i++;
                object[] ObjArr = { Bean.strC1, Bean.strC2, Bean.strC3, Bean.strC4, Bean.strC5, Bean.strC6, Bean.decC1, Bean.decC2, Bean.decC3, Bean.decC4, Bean.strC7, Bean.intC1, Bean.strC8, Bean.decC5, Bean.decC6, i == 1 ? Params.logo : null, i == 1 ? Params.titulo : "", i == 1 ? Params.edicion : "" };
                ds.Tables["Balance_Saldos"].Rows.Add(ObjArr);
            }
            #endregion

            if (i == 0) {
                object[] ObjArr = { "", "", "", "", "", "", 0, 0, 0, 0, "", 0, "", 0, 0, Params.logo, Params.titulo, Params.edicion  };
                ds.Tables["Balance_Saldos"].Rows.Add(ObjArr);
            }

            ViewState["Balance_Saldos"] = ds;//Definir ViewState
            #region Construir Crystal Report
            rpt = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
            rpt.Load(Server.MapPath("~/CR_BS.rpt"));
            rpt.SetDataSource(ds.Tables["Balance_Saldos"]);

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
            rpt.SetParameterValue("fecha_inicial", fecha_inicial);
            rpt.SetParameterValue("fecha_final", fecha_final);
            rpt.SetParameterValue("tc", tc);
            rpt.SetParameterValue("pais", user.pais.Nombre);
            rpt.SetParameterValue("contaID", contaID);
            rpt.SetParameterValue("monID", monID);
            rpt.SetParameterValue("usuario", user.ID);
            rpt.SetParameterValue("empresa", Server.MapPath(user.pais.Imagepath));
            //***titulos traducidos***
            rpt.SetParameterValue("Tpais", reg.strC1);
            rpt.SetParameterValue("Tconta", reg.strC2);
            rpt.SetParameterValue("Tmoneda", reg.strC3);
            //rpt.SetParameterValue("Ttitulo", reg.strC10);
            rpt.SetParameterValue("Tdesde", reg.strC4);
            rpt.SetParameterValue("Thasta", reg.strC5);
            rpt.SetParameterValue("moneda_fiscal_consolidada", consolidarrep);
            CrystalReportViewer1.ReportSource = rpt;
            CrystalReportViewer1.DataBind();
            rpt.SetParameterValue("vinculo", "testchar61tess");
            #endregion
        }
        else
        {
            #region Construir Crystal Report
            ds = (LibroDiarioDS)ViewState["Balance_Saldos"];
            rpt = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
            rpt.Load(Server.MapPath("~/CR_BS.rpt"));
            rpt.SetDataSource(ds.Tables["Balance_Saldos"]);
            string path = "balance_saldos";
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
            rpt.SetParameterValue("fecha_inicial", fecha_inicial);
            rpt.SetParameterValue("fecha_final", fecha_final);
            rpt.SetParameterValue("pais", user.pais.Nombre);
            rpt.SetParameterValue("contaID", contaID);
            rpt.SetParameterValue("monID", monID);
            rpt.SetParameterValue("empresa", Server.MapPath(user.pais.Imagepath));
            //***titulos traducidos***
            rpt.SetParameterValue("Tpais", reg.strC1);
            rpt.SetParameterValue("Tconta", reg.strC2);
            rpt.SetParameterValue("Tmoneda", reg.strC3);
            //rpt.SetParameterValue("Ttitulo", reg.strC10);
            rpt.SetParameterValue("Tdesde", reg.strC4);
            rpt.SetParameterValue("Thasta", reg.strC5);
            rpt.SetParameterValue("moneda_fiscal_consolidada", consolidarrep);
            CrystalReportViewer1.ReportSource = rpt;
            CrystalReportViewer1.DataBind();
            #endregion
        }
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
    protected decimal Totalizar_Hijas(string cue_madre, decimal total, int nivel)
    {
        #region Totalizar Hijas
        foreach (RE_GenericBean Bean in Arr_Balance)
        {
            if (Bean.strC8 == cue_madre)
            {
                if (nivel == 4)
                {
                    total += Bean.decC4;
                }
                else if (nivel == 3)
                {
                    total += Bean.decC5;
                }
            }
        }
        return total;
        #endregion
    }
}
