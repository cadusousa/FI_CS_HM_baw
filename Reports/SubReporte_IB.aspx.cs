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

public partial class Reports_SubReporte_IB : System.Web.UI.Page
{
    UsuarioBean user = null;
    ArrayList arr = null;
    string fecha_inicial = "";
    string fecha_final = "";
    string cuenta_bancaria = "";
    string banco = "";
    string banco_id = "";
    decimal saldo_inicial = 0;
    decimal saldo_actual = 0;
    decimal debe = 0;
    decimal haber = 0;
    decimal total = 0;
    decimal debe_temp = 0;
    decimal haber_temp = 0;
    decimal saldo_temp = 0;
    decimal saldo = 0;

    CrystalDecisions.CrystalReports.Engine.ReportDocument rpt;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["usuario"] == null)
        {
            Response.Redirect("../default.aspx");
        }
        user = (UsuarioBean)Session["usuario"];
        LibroDiarioDS ds = new LibroDiarioDS();

        fecha_inicial = Request.QueryString["fecha_inicio"].ToString();
        fecha_final = Request.QueryString["fecha_fin"].ToString();
        cuenta_bancaria = Request.QueryString["cuenta_bancaria"].ToString();
        banco_id = Request.QueryString["banco_id"].ToString();
        banco = Request.QueryString["banco"].ToString();


        if (!IsPostBack)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("TIPO");
            dt.Columns.Add("DOCUMENTO");
            dt.Columns.Add("FECHA");
            dt.Columns.Add("DESCRIPCION");
            dt.Columns.Add("DEBE");
            dt.Columns.Add("HABER");
            dt.Columns.Add("SALDO");

            saldo_inicial = DB.getSaldoInicialMovimientoBancario(user, int.Parse(banco_id), cuenta_bancaria, "1.1.1.2.0000", fecha_inicial, user.Moneda);
            saldo_temp = saldo_inicial;
            arr = (ArrayList)DB.getMovimientoBancario(user, int.Parse(banco_id), cuenta_bancaria, fecha_inicial, fecha_final, "1.1.1.2.0000", user.Moneda);
            foreach (RE_GenericBean rgb in arr)
            {
                object[] objArr = { rgb.strC5, rgb.strC1, rgb.strC2, rgb.strC6, decimal.Parse(rgb.strC3), decimal.Parse(rgb.strC4), decimal.Parse("0.00") };
                dt.Rows.Add(objArr);
                //ds.Tables["SR_IntegracionBancaria"].Rows.Add(objArr);
                debe += decimal.Parse(rgb.strC3);
                haber += decimal.Parse(rgb.strC4);
            }

            //
            foreach (DataRow row in dt.Rows)
            {
                debe_temp = 0;
                haber_temp = 0;
                debe_temp += decimal.Parse(row["DEBE"].ToString());
                haber_temp += decimal.Parse(row["HABER"].ToString());
                saldo = debe_temp - haber_temp;
                saldo_temp += saldo;

                object[] objDatos = { row["TIPO"].ToString(), row["DOCUMENTO"].ToString(), row["FECHA"].ToString(), row["DESCRIPCION"].ToString(), decimal.Parse(row["DEBE"].ToString()), decimal.Parse(row["HABER"].ToString()), saldo_temp.ToString("#,#.00#;(#,#.00#)") };
                ds.Tables["SR_IntegracionBancaria"].Rows.Add(objDatos);
            }
            //

            ViewState["SR_IntegracionBancariaDS"] = ds;//Definir ViewState
        }

        //saldo_inicial = DB.getSaldoInicialMovimientoBancario(user, int.Parse(banco_id), cuenta_bancaria, "1.1.1.2.0000", fecha_inicial, user.Moneda);
        saldo_actual = debe - haber;
        total = saldo_inicial + saldo_actual;


        ds = (LibroDiarioDS)ViewState["SR_IntegracionBancariaDS"];
        rpt = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
        rpt.Load(Server.MapPath("~/CR_SubReporte_IB.rpt"));
        rpt.SetDataSource(ds.Tables["SR_IntegracionBancaria"]);

        rpt.SetParameterValue("usuario", user.ID);
        rpt.SetParameterValue("empresa", Server.MapPath(user.pais.Imagepath));
        rpt.SetParameterValue("banco", banco);
        rpt.SetParameterValue("cuenta_bancaria", cuenta_bancaria);
        rpt.SetParameterValue("fecha_inicial", fecha_inicial);
        rpt.SetParameterValue("fecha_final", fecha_final);
        rpt.SetParameterValue("pais", user.pais.Nombre);

        rpt.SetParameterValue("saldo_inicial", saldo_inicial.ToString("#,#.00#;(#,#.00#)"));
        rpt.SetParameterValue("saldo_actual", saldo_actual.ToString("#,#.00#;(#,#.00#)"));
        rpt.SetParameterValue("total", total.ToString("#,#.00#;(#,#.00#)"));
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