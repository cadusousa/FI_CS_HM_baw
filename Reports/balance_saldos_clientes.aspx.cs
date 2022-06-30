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
    decimal Total_Facturas = 0;
    decimal Total_Recibos = 0;
    decimal Total_Nota_Credito = 0;
    decimal Total_Nota_Debito = 0;
    decimal Total_Retenido = 0;
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
            DB.parametros Params = DB.EmpresaParametros(user.PaisID, "", "", "BALANCE DE DOCUMENTOS X COBRAR", ""); //pais, sistema, doc_id, titulo, edicion
            int i = 0;
            arr = DB.getBalance_Saldos_Clientes(monID, contaID, user, fecha_inicial, fecha_final, estados);
            foreach (RE_GenericBean Bean in arr)
            {
                Saldo_Inicial = DB.getBalanceSaldos_X_Cliente(monID, contaID, user, fecha_saldo_inicial, estados, Bean.intC1, Bean.strC1);
                Total_Facturas = Bean.decC1;
                Total_Recibos = Bean.decC2;
                Total_Nota_Credito = Bean.decC3;
                Total_Nota_Debito = Bean.decC4;
                Total_Retenido = Bean.decC5;
                Saldo_Final = Saldo_Inicial + ((Total_Facturas + Total_Nota_Debito) - (Total_Recibos + Total_Nota_Credito));
                if (ban_saldo == 1)//Clientes con Saldo
                {
                    i++;
                    object[] ObjArr = { Bean.intC1, Bean.strC1, Saldo_Inicial, Total_Facturas, Total_Recibos, Total_Nota_Credito, Total_Nota_Debito, Total_Retenido, Saldo_Final, i == 1 ? Params.logo : null, i == 1 ? Params.titulo : "", i == 1 ? Params.edicion : "" };
                    ds.Tables["Balance_Saldos_Clientes"].Rows.Add(ObjArr);
                }
                else
                {
                    if (Saldo_Final == 0)
                    {                        
			            i++;
                        object[] ObjArr = { Bean.intC1, Bean.strC1, Saldo_Inicial, Total_Facturas, Total_Recibos, Total_Nota_Credito, Total_Nota_Debito, Total_Retenido, Saldo_Final, i == 1 ? Params.logo : null, i == 1 ? Params.titulo : "", i == 1 ? Params.edicion : "" };
                        ds.Tables["Balance_Saldos_Clientes"].Rows.Add(ObjArr);
                    }
                }
            }

            if (i == 0) {
                object[] ObjArr = { 0, "", 0, 0, 0, 0, 0, 0, 0, Params.logo, Params.titulo, Params.edicion };                
                ds.Tables["Balance_Saldos_Clientes"].Rows.Add(ObjArr);                
            }

            ViewState["Balance_Saldos_Clientes"] = ds;//Definir ViewState
            #region Construir Crystal Report
            rpt = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
            rpt.Load(Server.MapPath("~/CR_BS_Clientes.rpt"));
            rpt.SetDataSource(ds.Tables["Balance_Saldos_Clientes"]);
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
            ds = (LibroDiarioDS)ViewState["Balance_Saldos_Clientes"];
            rpt = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
            rpt.Load(Server.MapPath("~/CR_BS_Clientes.rpt"));
            rpt.SetDataSource(ds.Tables["Balance_Saldos_Clientes"]);
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
}
