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

public partial class Reports_viewProfit2 : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["usuario"] == null)
        {
            Response.Redirect("../default.aspx");
        }
        UsuarioBean user = null;
        LibroDiarioDS ds = new LibroDiarioDS();
        if (!Page.IsPostBack)
        {
            string BL = "MBL PRUEBA";// Request.QueryString["BL"].ToString();
            ArrayList temp = null;
            //facturas
           // temp = (ArrayList)DB.getFacturasForProfit(BL,"",1,);//obtengo a partir de la referencia
            foreach (RE_GenericBean rgb in temp)
            {
                if (rgb.strC12.Equals("Ingreso"))
                {
                    object[] objArr = { rgb.strC1, rgb.strC2, rgb.strC3, rgb.strC4, rgb.strC5, rgb.strC6, rgb.strC7, rgb.strC8, rgb.strC9, Utility.TraducirServiciotoSTR(rgb.intC1), rgb.strC11, rgb.decC1, 0 };
                    ds.Tables["profit_data_tbl1"].Rows.Add(objArr);
                }
                else
                {
                    object[] objArr = { rgb.strC1, rgb.strC2, rgb.strC3, rgb.strC4, rgb.strC5, rgb.strC6, rgb.strC7, rgb.strC8, rgb.strC9, Utility.TraducirServiciotoSTR(rgb.intC1), rgb.strC11, 0, rgb.decC1 };
                    ds.Tables["profit_data_tbl1"].Rows.Add(objArr);
                }

            }

            //Notas Credito
           // temp = (ArrayList)DB.getNCForProfit(BL,"",1);//obtengo a partir de la referencia
            foreach (RE_GenericBean rgb in temp)
            {
                if (rgb.strC12.Equals("Ingreso"))
                {
                    object[] objArr = { rgb.strC1, rgb.strC2, rgb.strC3, rgb.strC4, rgb.strC5, rgb.strC6, rgb.strC7, rgb.strC8, rgb.strC9, Utility.TraducirServiciotoSTR(rgb.intC1), rgb.strC11, rgb.decC1, 0 };
                    ds.Tables["profit_data_tbl1"].Rows.Add(objArr);
                }
                else
                {
                    object[] objArr = { rgb.strC1, rgb.strC2, rgb.strC3, rgb.strC4, rgb.strC5, rgb.strC6, rgb.strC7, rgb.strC8, rgb.strC9, Utility.TraducirServiciotoSTR(rgb.intC1), rgb.strC11, 0, rgb.decC1 };
                    ds.Tables["profit_data_tbl1"].Rows.Add(objArr);
                }
            }

            //Notas Debito
          //  temp = (ArrayList)DB.getNDForProfit(BL,"",1);//obtengo a partir de la referencia
            foreach (RE_GenericBean rgb in temp)
            {
                if (rgb.strC12.Equals("Ingreso"))
                {
                    object[] objArr = { rgb.strC1, rgb.strC2, rgb.strC3, rgb.strC4, rgb.strC5, rgb.strC6, rgb.strC7, rgb.strC8, rgb.strC9, Utility.TraducirServiciotoSTR(rgb.intC1), rgb.strC11, rgb.decC1, 0 };
                    ds.Tables["profit_data_tbl1"].Rows.Add(objArr);
                }
                else
                {
                    object[] objArr = { rgb.strC1, rgb.strC2, rgb.strC3, rgb.strC4, rgb.strC5, rgb.strC6, rgb.strC7, rgb.strC8, rgb.strC9, Utility.TraducirServiciotoSTR(rgb.intC1), rgb.strC11, 0, rgb.decC1 };
                    ds.Tables["profit_data_tbl1"].Rows.Add(objArr);
                }

            }

            //Provisiones
         //   temp = (ArrayList)DB.getProvisionesForProfit(BL,"",1);//obtengo a partir de la referencia
            foreach (RE_GenericBean rgb in temp)
            {
                if (rgb.strC12.Equals("Ingreso"))
                {
                    object[] objArr = { rgb.strC1, rgb.strC2, rgb.strC3, rgb.strC4, rgb.strC5, rgb.strC6, rgb.strC7, rgb.strC8, rgb.strC9, Utility.TraducirServiciotoSTR(rgb.intC1), rgb.strC11, rgb.decC1, 0 };
                    ds.Tables["profit_data_tbl1"].Rows.Add(objArr);
                }
                else
                {
                    object[] objArr = { rgb.strC1, rgb.strC2, rgb.strC3, rgb.strC4, rgb.strC5, rgb.strC6, rgb.strC7, rgb.strC8, rgb.strC9, Utility.TraducirServiciotoSTR(rgb.intC1), rgb.strC11, 0, rgb.decC1 };
                    ds.Tables["profit_data_tbl1"].Rows.Add(objArr);
                }

            }
            ViewState["ds"] = ds;
        }

        ds = (LibroDiarioDS)ViewState["ds"];
        CrystalDecisions.CrystalReports.Engine.ReportDocument rpt = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
        rpt.Load(Server.MapPath("/AIMAR/CR_profit2.rpt"));
        rpt.SetDataSource(ds.Tables["profit_data_tbl1"]);
        CrystalReportViewer1.DisplayGroupTree = false;
        CrystalReportViewer1.ReportSource = rpt;
        CrystalReportViewer1.DataBind();
    }
}
