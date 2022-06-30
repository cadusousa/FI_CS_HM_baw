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

public partial class Reports_balance_saldos_clientes : System.Web.UI.Page
{
    UsuarioBean user = null;
    ArrayList arr = null;
    string fecha_inicial = "";
    string fecha_final = "";
    string fecha_saldo_inicial = "";
    int monID = 0;
    int contaID = 0;
    string estados = "";
    int ban_saldo = 0;
    decimal Saldo_Inicial = 0;
    decimal Saldo_Final = 0;
    decimal Total_Provisiones = 0;
    decimal Total_Notas_Debito = 0;
    decimal Total_Notas_Credito = 0;
    decimal Total_Cheques = 0;
    decimal Total_Transferencias = 0;
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
        monID = int.Parse(Request.QueryString["monID"].ToString());
        contaID = int.Parse(Request.QueryString["tcon"].ToString());
        estados = Request.QueryString["ted"].ToString();
        ban_saldo = int.Parse(Request.QueryString["ban_saldo"].ToString());
        fecha_inicial = DB.DateFormat(fecha_inicial);
        fecha_final = DB.DateFormat(fecha_final);
        fecha_saldo_inicial = DateTime.Parse(fecha_inicial).AddDays(-1).ToString("yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
        if (!IsPostBack)
        {
            DB.parametros Params = DB.EmpresaParametros(user.PaisID, "", "", "BALANCE DE DOCUMENTOS X PAGAR", ""); //pais, sistema, doc_id, titulo, edicion
            int i = 0;
            arr = DB.getBalance_Cuentas_X_Pagar(monID, contaID, user, fecha_inicial, fecha_final, estados);
            foreach (RE_GenericBean Bean in arr)
            {
                Saldo_Inicial = DB.getBalanceSaldos_X_Proveedor(monID, contaID, user, fecha_saldo_inicial, estados, Bean.intC1, Bean.strC1);
                Total_Provisiones = Bean.decC1;
                Total_Notas_Debito = Bean.decC2;
                Total_Notas_Credito = Bean.decC3;
                Total_Cheques = Bean.decC4;
                Total_Transferencias = Bean.decC5;
                Saldo_Final = Saldo_Inicial + ((Total_Provisiones) - (Total_Notas_Debito + Total_Notas_Credito+ Total_Cheques + Total_Transferencias));
                if (ban_saldo == 1)//Proveedor con Saldo
                {
			        i++;			
                    object[] ObjArr = { Bean.intC1, Bean.strC1, Saldo_Inicial, Total_Provisiones, Total_Notas_Debito, Total_Notas_Credito, Total_Cheques, Total_Transferencias, Saldo_Final, i == 1 ? Params.logo : null, i == 1 ? Params.titulo : "", i == 1 ? Params.edicion : "" };
                    ds.Tables["Balance_Saldos_Proveedores"].Rows.Add(ObjArr);
                }
                else
                {
                    if (Saldo_Final == 0)
                    {
			            i++;
                        object[] ObjArr = { Bean.intC1, Bean.strC1, Saldo_Inicial, Total_Provisiones, Total_Notas_Debito, Total_Notas_Credito, Total_Cheques, Total_Transferencias, Saldo_Final, i == 1 ? Params.logo : null, i == 1 ? Params.titulo : "", i == 1 ? Params.edicion : "" };
                        ds.Tables["Balance_Saldos_Proveedores"].Rows.Add(ObjArr);
                    }
                }
            }

            if (i == 0) {
                object[] ObjArr = { 0, "", 0, 0, 0, 0, 0, 0, 0, Params.logo, Params.titulo, Params.edicion };
                ds.Tables["Balance_Saldos_Proveedores"].Rows.Add(ObjArr);			                			
            }

            ViewState["Balance_Saldos_Proveedores"] = ds;//Definir ViewState
            #region Construir Crystal Report
            rpt = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
            rpt.Load(Server.MapPath("~/CR_BS_Proveedores.rpt"));
            rpt.SetDataSource(ds.Tables["Balance_Saldos_Proveedores"]);
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
            rpt.SetParameterValue("empresa", Server.MapPath(user.pais.Imagepath));
            CrystalReportViewer1.ReportSource = rpt;
            CrystalReportViewer1.DataBind();
            #endregion
        }
        else
        {
            #region Construir Crystal Report
            ds = (LibroDiarioDS)ViewState["Balance_Saldos_Proveedores"];
            rpt = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
            rpt.Load(Server.MapPath("~/CR_BS_Proveedores.rpt"));
            rpt.SetDataSource(ds.Tables["Balance_Saldos_Proveedores"]);
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
            rpt.SetParameterValue("empresa", Server.MapPath(user.pais.Imagepath));
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
}
