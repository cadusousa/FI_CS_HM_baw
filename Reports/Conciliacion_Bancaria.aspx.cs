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

public partial class Reports_Conciliacion_Bancaria : System.Web.UI.Page
{
    LibroDiarioDS ds = new LibroDiarioDS();
    LibroDiarioDS ds_Resumen = new LibroDiarioDS();
    LibroDiarioDS ds_Documentos_Circulacion = new LibroDiarioDS();
    UsuarioBean user = null;
    RE_GenericBean Conciliacion_Bean = null;
    string fecha_inicial = "";
    string fecha_final = "";
    int monID = 0;
    int contaID = 0;
    int bancoID = 0;
    string Cuenta = "";
    CrystalDecisions.CrystalReports.Engine.ReportDocument rpt;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["usuario"] == null)
        {
            Response.Redirect("../default.aspx");
        }
        user = (UsuarioBean)Session["usuario"];
        fecha_inicial = Request.QueryString["fecha_inicial"].ToString();
        fecha_final = Request.QueryString["fecha_final"].ToString();
        monID = int.Parse(Request.QueryString["monID"].ToString());
        contaID = user.contaID;
        int Anio_Consultado = DateTime.Parse(DB.DateFormat(fecha_inicial)).Year;
        int Mes_Consultado = DateTime.Parse(DB.DateFormat(fecha_final)).Month;
        fecha_inicial = DB.DateFormat(fecha_inicial);
        fecha_final = DB.DateFormat(fecha_final);
        bancoID = int.Parse(Request.QueryString["bancoID"].ToString());
        Cuenta = Request.QueryString["cuenta"].ToString();
        if (!Page.IsPostBack)
        {
            string Query = "tc_pai_id=" + user.PaisID + " and tc_tba_id=" + bancoID + " and tc_tbc_cuenta_bancaria='" + Cuenta + "' and tc_tbc_mon_id=" + monID + " and tc_fecha_inicial='" + fecha_inicial + "' and tc_fecha_final='" + fecha_final + "' and tc_ted_id=1";
            int ID_CONCILIACION = DB.Validar_Periodo_Conciliado(user, Query);


            int i = 0;
            DB.parametros Params = DB.EmpresaParametros(user.PaisID, "", "", "CONCILIACION BANCARIA", ""); //pais, sistema, doc_id, titulo, edicion
            
            #region Obtener Detalle Conciliacion
            ArrayList Arr_Conciliacion = (ArrayList)DB.getDetalleConciliacion(user, ID_CONCILIACION);
            foreach (RE_GenericBean Documento in Arr_Conciliacion)
            {
                if ((Documento.strC9 == "13") && (Documento.strC10 == "T"))
                {
                    i++;
                    object[] objArr = { Documento.strC1, Documento.strC2, Documento.strC3, Documento.strC4, decimal.Parse(Documento.strC5), decimal.Parse(Documento.strC6), decimal.Parse(Documento.strC7), Documento.strC8, Documento.strC9, Documento.strC10, 0, 0, i == 1 ? Params.logo : null, i == 1 ? Params.titulo : "", i == 1 ? Params.edicion : "" };
                    ds.Tables["Conciliaciones_Bancarias"].Rows.Add(objArr);
                }
                else
                {
                    i++;
                    object[] objArr = { Documento.strC1, Documento.strC2, Documento.strC3, Documento.strC4, decimal.Parse(Documento.strC5), decimal.Parse(Documento.strC6), decimal.Parse(Documento.strC7), Documento.strC8, Documento.strC9, Documento.strC10, decimal.Parse(Documento.strC5), decimal.Parse(Documento.strC6), i == 1 ? Params.logo : null, i == 1 ? Params.titulo : "", i == 1 ? Params.edicion : "" };
                    ds.Tables["Conciliaciones_Bancarias"].Rows.Add(objArr);
                }
            }
            #endregion
            
            if (i == 0) {
                object[] objArr = { "", "", "", "", 0, 0, 0, "", "", "", 0, 0, Params.logo, Params.titulo, Params.edicion };
                ds.Tables["Conciliaciones_Bancarias"].Rows.Add(objArr);                			
            }
            
            ViewState["Conciliaciones_Bancarias"] = ds;
            ds_Resumen = (LibroDiarioDS)DB.get_Resumen_Conciliacion(user, ID_CONCILIACION);
            ViewState["Resumen_Conciliaciones_Bancarias"] = ds_Resumen;
            ds_Documentos_Circulacion = (LibroDiarioDS)DB.getReporte_Documentos_Circulacion(user, ID_CONCILIACION);
            ViewState["Resumen_Documentos_Circulacion"] = ds_Documentos_Circulacion;
        }
        else
        {
            ds = (LibroDiarioDS)ViewState["Conciliaciones_Bancarias"];
            ds_Resumen = (LibroDiarioDS)ViewState["Resumen_Conciliaciones_Bancarias"];
            ds_Documentos_Circulacion = (LibroDiarioDS)ViewState["Resumen_Documentos_Circulacion"];
        }
        rpt = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
        rpt.Load(Server.MapPath("~/CR_Conciliaciones_Bancarias.rpt"));
        rpt.SetDataSource(ds.Tables["Conciliaciones_Bancarias"]);
        rpt.Subreports["CR_Resumen_Conciliaciones.rpt"].SetDataSource(ds_Resumen);
        rpt.Subreports["CR_Resumen_Documentos_Circulacion.rpt"].SetDataSource(ds_Documentos_Circulacion);
        Conciliacion_Bean = (RE_GenericBean)DB.Get_Conciliacion_Data(user, fecha_inicial, fecha_final, bancoID, Cuenta, monID);
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
        rpt.SetParameterValue("banco", Conciliacion_Bean.strC1);
        rpt.SetParameterValue("cuenta_bancaria",Conciliacion_Bean.strC2);
        RE_GenericBean Usuario_Bean =  (RE_GenericBean)DB.getUsuarioEmpresa(Conciliacion_Bean.strC6);
        rpt.SetParameterValue("usuario_realiza", Usuario_Bean.strC1);
        Usuario_Bean =  (RE_GenericBean)DB.getUsuarioEmpresa(user.ID);
        rpt.SetParameterValue("usuario_imprime", Usuario_Bean.strC1);
        rpt.SetParameterValue("fecha_conciliacion", Conciliacion_Bean.strC5);
        rpt.SetParameterValue("Total_Creditos_Contabilidad", Conciliacion_Bean.decC4);
        rpt.SetParameterValue("Total_Debitos_Contabilidad", Conciliacion_Bean.decC5);
        rpt.SetParameterValue("Total_Creditos_Circulacion", Conciliacion_Bean.decC6);
        rpt.SetParameterValue("Total_Debitos_Circulacion", Conciliacion_Bean.decC7);
        rpt.SetParameterValue("Saldo_Final_Circulacion", Conciliacion_Bean.decC2);
        rpt.SetParameterValue("Saldo_Final_Contabilidad", Conciliacion_Bean.decC3);
        rpt.SetParameterValue("Saldo_Final_Banco", Conciliacion_Bean.decC1);
        rpt.SetParameterValue("Saldo_Inicial", Conciliacion_Bean.decC8);
        rpt.SetParameterValue("Descripcion_Saldo_Banco", "Saldo en Banco al " + fecha_final+".:");
        rpt.SetParameterValue("empresa", Server.MapPath(user.pais.Imagepath));
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
