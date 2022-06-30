using System;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Text;
using System.Globalization;

public partial class Reports_SolicitaER_2 : System.Web.UI.Page
{
    UsuarioBean user = null;
    Dictionary<int, string> listaTipoConta = new Dictionary<int, string>();
    Dictionary<int, int> listaAnios = new Dictionary<int, int>();
    Dictionary<int, string> listaPaises = new Dictionary<int, string>();
    
    enum TipoReporte
    {
        BalanceSaldo = 0,
        BalanceGeneral = 1,
        EstadosDeResultados = 2
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["usuario"] == null) {
            Response.Redirect("../default.aspx");
        }
        user = (UsuarioBean)Session["usuario"];

        obtengo_lista();
    }

    protected void obtengo_lista()
    {
        //Se crea la lista de los tipo de contabilidad
        listaTipoConta.Add(1, "FISCAL");
        listaTipoConta.Add(2, "FINANCIERA");

        //se crea lista de años
        listaAnios.Add(1, 2016);
        //listaAnios.Add(1, 2017);
        //listaAnios.Add(2, 2018);

        //Se obtienen paises
        listaPaises = ObtenerPaises();
    }

    protected Dictionary<int, string> ObtenerMonedas(int idPais, int idConta)
    {
        ArrayList arr = null;
        var listaMoneda = new Dictionary<int, string>();
        arr = (ArrayList)DB.getMonedasbyPais(idPais, idConta);
        foreach (RE_GenericBean rgb in arr)
        {
            listaMoneda.Add(rgb.intC1, rgb.strC1.Substring(0, 3));
        }
        return listaMoneda;
    }

    protected Dictionary<int, string> ObtenerPaises()
    {
        listaPaises.Add(1,	"GUATEMALA");
        listaPaises.Add(2,	"EL SALVADOR");
        listaPaises.Add(3,	"HONDURAS");
        listaPaises.Add(4,	"NICARAGUA");
        listaPaises.Add(5,	"COSTA RICA");
        listaPaises.Add(6,	"PANAMA");
        listaPaises.Add(7,	"BELICE");
        listaPaises.Add(9,	"SALVADOR 2");
        listaPaises.Add(11,	"GRH");
        listaPaises.Add(12,	"ISI SURVEYOR");
        listaPaises.Add(13,	"MAYAN LOGISTIC");
        listaPaises.Add(15, "LATIN FREIGHT - GUATEMALA");
        listaPaises.Add(16,	"MAYAN LOGISTICS GT");
        listaPaises.Add(18,	"ISI SURVEYOR GT");
        listaPaises.Add(21,	"LATIN FREIGHT - COSTA RICA");
        listaPaises.Add(23,	"LATIN FREIGHT - HONDURAS");
        listaPaises.Add(24,	"LATIN FREIGHT - NICARAGUA");
        listaPaises.Add(25,	"LATIN FREIGHT - PANAMA");
        listaPaises.Add(26,	"LATIN FREIGHT - EL SALVADOR");
        listaPaises.Add(29,	"EQUITRANS COSTA RICA");
        listaPaises.Add(30,	"WORLD MARITIME TRANSPORT, LTD");
        return listaPaises;
    }

    protected void btn_generar_Click(object sender, EventArgs e)
    {
        //decimal tp = 0;
        string consolidar_moneda_fiscal = "no";
        //tp = DB.getTipoCambioByDay(user, DB.DateFormat(tb_fechafinal.Text.Trim()));
        
        foreach (var anio in listaAnios)
        {
            var cantidadMeses = anio.Value == 2018 ? 9 : 12;
            for (int mes = 1;  mes <= cantidadMeses;  mes++)
			{
                var fechaInicial = new DateTime(anio.Value, mes, 1);
                var fechaFinal = new DateTime(anio.Value, mes, DateTime.DaysInMonth(anio.Value, mes));
                foreach (var pais in listaPaises)
                {
                    user.PaisID = pais.Key;
                    user.pais = (PaisBean)DB.getPais(pais.Key);
                    Session["usuario"] = user;
                    foreach (var tipoConta in listaTipoConta)
                    {
                        var listaMonedas = ObtenerMonedas(pais.Key, tipoConta.Key);
                        foreach (var tmoneda in listaMonedas)
                        {
                            foreach (TipoReporte tReporte in ((TipoReporte[])Enum.GetValues(typeof(TipoReporte))))
                            {
                                var titulo = string.Empty;
                                var path = string.Concat(this.CrearPathArchivo(pais.Value, anio.Value.ToString(), CultureInfo.InvariantCulture.TextInfo.ToTitleCase(new CultureInfo("es-GT", false).DateTimeFormat.GetMonthName(mes)), tipoConta.Value)
                                                        , this.CrearNombreArchivo(tReporte, tmoneda.Value));
                                if (!System.IO.File.Exists(path))
                                {
                                    switch (tReporte)
                                    {
                                        case TipoReporte.BalanceSaldo:
                                            {
                                                titulo = "BALANCE DE SALDOS";
                                                this.EjecutarBalanceSaldos(tmoneda.Key, tipoConta.Key
                                                       , fechaInicial.ToString("MM/dd/yyyy"), fechaFinal.ToString("MM/dd/yyyy"), consolidar_moneda_fiscal, path);
                                                break;
                                            }
                                        case TipoReporte.BalanceGeneral:
                                            {
                                                titulo = "BALANCE GENERAL";
                                                this.EjecutarBalanceGeneral("2", tmoneda.Key, tipoConta.Key
                                                        , fechaInicial.ToString("MM/dd/yyyy"), fechaFinal.ToString("MM/dd/yyyy"), consolidar_moneda_fiscal, path);
                                                break;
                                            }
                                        case TipoReporte.EstadosDeResultados:
                                            {
                                                titulo = "ESTADOS DE RESULTADOS";
                                                this.EjecutarEstadoResultado(tmoneda.Key, tipoConta.Key
                                                        , fechaInicial.ToString("MM/dd/yyyy"), fechaFinal.ToString("MM/dd/yyyy"), consolidar_moneda_fiscal, path);
                                                break;
                                            }
                                        default:
                                            break;
                                    }
                                    if (!string.IsNullOrEmpty(titulo))
                                        this.GuardarLog(pais.Key, titulo, tmoneda.Value, tipoConta.Value, fechaInicial.ToString("MM/dd/yyyy"), fechaFinal.ToString("MM/dd/yyyy"));
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    private void EjecutarEstadoResultado(int monedaId, int contaId, string fechaIni, string fechaFin, string consolidarMoneda, string savePath)
    {
        #region Variables
        string fecha_inicial = "";
        string fecha_final = "";
        string consmonfiscal = "";
        string consmonRep = "";
        CrystalDecisions.CrystalReports.Engine.ReportDocument rpt;
        var user = (UsuarioBean)Session["usuario"];
        int monID = monedaId;
        int tconID = contaId;
        decimal tc = 0;
        #endregion

        fecha_inicial = fechaIni;
        fecha_final = fechaFin;
        consmonfiscal = consolidarMoneda;
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
        decimal total = 0;
        decimal totalactivo = 0;
        decimal totalpasivo = 0;

        ArrayList arr = (ArrayList)DB.getEstadoResultados(user, monID, tconID, tipocambio, fecha_inicial, fecha_final, consmonfiscal);
        tc = DB.getTipoCambioXFecha(user.PaisID, fecha_final);

        string path = "estado_resultados";
        RE_GenericBean reg = DB.Traducir_reportes(user, path);

        int i = 0;
        DB.parametros Params = DB.EmpresaParametros(user.PaisID, "", "", reg.strC10, ""); //pais, sistema, doc_id, titulo, edicion

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
                object[] objArr = { rgb.strC1, rgb.strC2, "", "", rgb.intC2, rgb.intC1, total, rgb.strC3, i == 1 ? Params.logo : null, i == 1 ? Params.titulo : "", i == 1 ? Params.edicion : "" };
                ds.Tables["Estado_Resultados"].Rows.Add(objArr);
            }
            else if (rgb.intC1 == 2)
            {
                i++;
                object[] objArr = { rgb.strC1, "", rgb.strC2, "", rgb.intC2, rgb.intC1, total, rgb.strC3, i == 1 ? Params.logo : null, i == 1 ? Params.titulo : "", i == 1 ? Params.edicion : "" };
                ds.Tables["Estado_Resultados"].Rows.Add(objArr);
            }
            else if (rgb.intC1 == 3)
            {
                i++;
                object[] objArr = { rgb.strC1, "", "", rgb.strC2, rgb.intC2, rgb.intC1, total, rgb.strC3, i == 1 ? Params.logo : null, i == 1 ? Params.titulo : "", i == 1 ? Params.edicion : "" };
                ds.Tables["Estado_Resultados"].Rows.Add(objArr);
            }
        }

        if (i == 0) {
                object[] objArr = { "", "", "", "", 0, 0, 0, "", Params.logo, Params.titulo, Params.edicion  };
                ds.Tables["Estado_Resultados"].Rows.Add(objArr);			                
        }

        ViewState["dsEstado_Resultados"] = ds;

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

        rpt.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.Excel, savePath);
                //, string.Format(@"{0}/{1}{2}_{3}_{4}{5}", savePath, "estados_resultados", fe_anio.ToString(), fe_mes.ToString(), monID.ToString(), ".xls"));

        
        #region destruir objeto CR
        if (rpt != null)
        {
            rpt.Close();
            rpt.Dispose();
            GC.Collect();
        }
        #endregion

    }

    private void EjecutarBalanceGeneral(string tipoReporte, int monedaId, int contaId, string fechaIni, string fechaFin, string consolidarMoneda, string savePath)
    {
        #region Variables
        decimal total = 0;
        decimal totalpasivo = 0;
        decimal totalcapital = 0;
        decimal pasivo_capital = 0;
        decimal Resultado_Ejercicio = 0;
        string Tipo_Reporte = "";
        UsuarioBean user = null;
        string fecha_inicial = "";
        string fecha_final = "";
        string consmon = "";
        string consmon_rep = "";
        int monID = 0;
        int contaID = 0;
        ArrayList arr = null;
        int fe_dia = 0;
        int fe_mes = 0;
        int fe_anio = 0;
        CrystalDecisions.CrystalReports.Engine.ReportDocument rpt;
        #endregion

        user = (UsuarioBean)Session["usuario"];
        LibroDiarioDS ds = new LibroDiarioDS();
        Tipo_Reporte = tipoReporte;
        if (Tipo_Reporte == "1")
        {
            fecha_final = fechaFin;
            #region Formatear Fecha Corte
            //Fecha Corte
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
        else if (Tipo_Reporte == "2")
        {
            fecha_inicial = fechaIni;
            fecha_final = fechaFin;
            #region Formatear Fechas
            //Fecha Inicio
            fe_dia = int.Parse(fecha_inicial.Substring(3, 2));
            fe_mes = int.Parse(fecha_inicial.Substring(0, 2));
            fe_anio = int.Parse(fecha_inicial.Substring(6, 4));
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
        monID = monedaId;
        contaID = contaId;
        consmon = consolidarMoneda;

        if (consmon == "si")
        {
            consmon_rep = "CONSOLIDADO";
        }
        else
        {
            consmon_rep = "";
        }

        decimal tc = DB.getTipoCambioXFecha(user.PaisID, fecha_final);
        decimal tipocambio = DB.getTipoCambioHoy(5);

        string path = "balance_general";
        RE_GenericBean reg = DB.Traducir_reportes(user, path);

        int i = 0;
        DB.parametros Params = DB.EmpresaParametros(user.PaisID, "", "", reg.strC10, ""); //pais, sistema, doc_id, titulo, edicion

        #region Obtener Resultado del Ejercicio
        Resultado_Ejercicio = DB.getResultado_Ejercicio(user, monID, contaID, fecha_inicial, fecha_final, Tipo_Reporte, consmon);
        //lb_resultado_ejercicio.Text = Resultado_Ejercicio.ToString();
        #endregion
        arr = (ArrayList)DB.getBG(monID, tipocambio, contaID, user, fecha_final, fecha_inicial, fecha_final, Tipo_Reporte, consmon);
        foreach (ReportBean Reporte in arr)
        {
            if (Reporte.Cue_Clasificacion == 1)
            {
                #region Calsificacion1
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
                    total = Reporte.Debe - Reporte.Haber;
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
                #endregion
            }
            if (Reporte.Cue_Clasificacion == 2)
            {
                #region Clasificacion2
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
                #endregion
            }
            else if (Reporte.Cue_Clasificacion == 5)
            {
                #region Clasificacion5
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
                #endregion
            }
        }

        if (i == 0) {
            object[] objArr = { "", "", "", "", "", 0, "", "", Params.logo, Params.titulo, Params.edicion  };
            ds.Tables["balancegeneral2"].Rows.Add(objArr);                	
        }

        //Definir ViewState
        ViewState["balancegeneral2"] = ds;
        pasivo_capital = totalpasivo + totalcapital;
        // lbl_pasivo_capital.Text = pasivo_capital.ToString();
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

        rpt.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.Excel, savePath);
                //, string.Format(@"{0}/{1}{2}_{3}_{4}{5}", savePath, "balance_general", fe_anio.ToString(), fe_mes.ToString(), monID.ToString(), ".xls"));

        #region destruir objeto CR
        if (rpt != null)
        {
            rpt.Close();
            rpt.Dispose();
            GC.Collect();
        }
        #endregion
    }

    private void EjecutarBalanceSaldos(int monedaId, int contaId, string fechaIni, string fechaFin, string consolidarMoneda, string savePath)
    {
        #region Variables
        UsuarioBean user = null;
        ArrayList arr = null;
        ArrayList Arr_Balance = new ArrayList();
        ArrayList Arr_Aux = new ArrayList();
        RE_GenericBean Balance_Bean = null;
        int anio = 0;
        int mes = 0;
        decimal Saldo_Inicial = 0, Saldo_Final = 0;
        CrystalDecisions.CrystalReports.Engine.ReportDocument rpt;
        string consolidarrep = "";

        user = (UsuarioBean)Session["usuario"];
        LibroDiarioDS ds = new LibroDiarioDS();
        var fecha_inicial = fechaIni;
        var fecha_final = fechaFin;
        var consmonedafiscal = consolidarMoneda;
        var monID = monedaId;
        var contaID = contaId;
        #endregion

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
                    Saldo_Final = (Reporte.Debe - Reporte.Haber) + (Saldo_Inicial);
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

        int i = 0;
        DB.parametros Params = DB.EmpresaParametros(user.PaisID, "", "", reg.strC10, ""); //pais, sistema, doc_id, titulo, edicion

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
        rpt.SetParameterValue("vinculo", "testchar61tess");
        rpt.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.Excel, savePath);
                //, string.Format(@"{0}/{1}{2}_{3}_{4}{5}", savePath, "balance_saldos", fe_anio.ToString(), fe_mes.ToString(), monID.ToString(), ".xls"));
        
        #endregion

        #region destruir objeto CR
        if (rpt != null)
        {
            rpt.Close();
            rpt.Dispose();
            GC.Collect();
        }
        #endregion
    }

    private string CrearPathArchivo(string empresa, string anio, string nombreMes, string tipoConta)
    {
        var sb = new StringBuilder();
        var slash = "\\";
        sb.Append("D:\\ReportesContabilidad\\");
        sb.Append(empresa);
        sb.Append(slash);
        sb.Append(anio);
        sb.Append(slash);
        sb.Append(nombreMes);
        sb.Append(slash);
        sb.Append(tipoConta);
        sb.Append(slash);

        if (!System.IO.Directory.Exists(sb.ToString()))
            System.IO.Directory.CreateDirectory(sb.ToString());

        return sb.ToString();
    }

    private string CrearNombreArchivo(TipoReporte tipoReporte, string moneda, string extencion = "xls")
    {
        var nombreArchivo = string.Empty;
        
        switch (tipoReporte)
        {
            case TipoReporte.BalanceSaldo:
                nombreArchivo = string.Concat("BalanceSaldos_", moneda);
                break;
            case TipoReporte.BalanceGeneral:
                nombreArchivo = string.Concat("BalanceGeneral_", moneda);
                break;
            case TipoReporte.EstadosDeResultados:
                nombreArchivo = string.Concat("EstadosDeResultados_", moneda);
                break;
            default:
                break;
        }

        return string.Concat(nombreArchivo, ".", extencion);
    }

    private void GuardarLog(int idPais, string titulo, string tmoneda, string tConta, string fechaIni, string fechaFin)
    {
        #region Registrar Log de Reportes
        RE_GenericBean Bean_Log = new RE_GenericBean();
        Bean_Log.intC1 = idPais;
        Bean_Log.strC1 = titulo;
        Bean_Log.strC2 = user.ID;
        Bean_Log.strC3 = Request.ServerVariables["REMOTE_ADDR"].ToString();
        string mensaje_log = "";
        mensaje_log += "Moneda.: " + tmoneda + " Contabilidad.: " + tConta + " ,";
        mensaje_log += "Fecha Inicial.: " + fechaIni + " Fecha Final.: " + fechaFin + " ,";
        mensaje_log += "Consolidar Moneda.: no ";
        Bean_Log.strC4 = mensaje_log;
        DB.Insertar_Log_Reportes(Bean_Log);
        #endregion
    }

}
