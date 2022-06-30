using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Reports_liquidacion : System.Web.UI.Page
{
    UsuarioBean user = null;
    LibroDiarioDS ds = new LibroDiarioDS();
    CrystalDecisions.CrystalReports.Engine.ReportDocument rpt;
    string liquidacionID = "0";
    ArrayList Arr_Liquidacion = new ArrayList();
    ArrayList Arr_Liquidacion_Detalle = new ArrayList();
    string sql = "";
    RE_GenericBean Bean_Data_Liquidacion = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["usuario"] == null)
        {
            Response.Redirect("../default.aspx");
        }
        user = (UsuarioBean)Session["usuario"];

        int i = 0;
        DB.parametros Params = DB.EmpresaParametros(user.PaisID, "", "", "LIQUIDACION", ""); //pais, sistema, doc_id, titulo, edicion

        liquidacionID = Request.QueryString["id"].ToString();
        sql = " and a.tli_id=" + liquidacionID + " and a.tli_pai_id=" + user.PaisID + " ";
        Arr_Liquidacion = (ArrayList)DB.Get_Liquidaciones(user, sql);
        foreach (RE_GenericBean Bean in Arr_Liquidacion)
        {
            Bean_Data_Liquidacion = new RE_GenericBean();
            Bean_Data_Liquidacion = Bean;
        }
        if (!Page.IsPostBack)
        {
            
            Arr_Liquidacion_Detalle = (ArrayList)DB.Get_Detalle_Liquidacion(user, int.Parse(liquidacionID));
            foreach (RE_GenericBean Bean in Arr_Liquidacion_Detalle)
            {
                i++;
                object[] objArr = { Bean.strC1, Bean.strC2, Bean.strC3, Bean.strC4, Bean.strC5, Bean.decC1, 0, Bean.decC2, i == 1 ? Params.logo : null, i == 1 ? Params.titulo : "", i == 1 ? Params.edicion : "" };
                ds.Tables["Liquidacion"].Rows.Add(objArr);
            }

            if (i == 0) {
                object[] objArr = { "", "", "", "", "", 0, 0, 0, Params.logo, Params.titulo, Params.edicion };
                ds.Tables["Liquidacion"].Rows.Add(objArr);			                			
            }

            ViewState["vs_liquidacion"] = ds;
        }
        else
        {
            ds = (LibroDiarioDS)ViewState["vs_liquidacion"];
        }
        rpt = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
        rpt.Load(Server.MapPath("~/CR_Liquidacion.rpt"));
        rpt.SetDataSource(ds.Tables["Liquidacion"]);
        rpt.SetParameterValue("empresa", Server.MapPath(user.pais.Imagepath));
        rpt.SetParameterValue("pais", Bean_Data_Liquidacion.strC12);
        if (Bean_Data_Liquidacion.strC13 == "1")
        {
            rpt.SetParameterValue("contabilidad", "FISCAL");
        }
        else if (Bean_Data_Liquidacion.strC13 == "2")
        {
            rpt.SetParameterValue("contabilidad", "FINANCIERA");
        }
        else if (Bean_Data_Liquidacion.strC13 == "3")
        {
            rpt.SetParameterValue("contabilidad", "CONSOLIDADO");
        }
        rpt.SetParameterValue("moneda", Utility.TraducirMonedaInt(int.Parse(Bean_Data_Liquidacion.strC18)));
        rpt.SetParameterValue("banco", Bean_Data_Liquidacion.strC16);
        rpt.SetParameterValue("cuenta_bancaria", Bean_Data_Liquidacion.strC17);
        rpt.SetParameterValue("tipo_persona", Bean_Data_Liquidacion.strC3.ToUpper());
        rpt.SetParameterValue("codigo", Bean_Data_Liquidacion.strC4);
        rpt.SetParameterValue("nombre", Bean_Data_Liquidacion.strC5);
        rpt.SetParameterValue("serie", Bean_Data_Liquidacion.strC6);
        rpt.SetParameterValue("correlativo", Bean_Data_Liquidacion.strC7);
        rpt.SetParameterValue("fecha", Bean_Data_Liquidacion.strC8);
        rpt.SetParameterValue("observaciones",Bean_Data_Liquidacion.strC14);
        rpt.SetParameterValue("usuario", Bean_Data_Liquidacion.strC9);
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