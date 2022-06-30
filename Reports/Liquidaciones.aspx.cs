using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;

public partial class Reports_Liquidaciones : System.Web.UI.Page
{
    UsuarioBean user = null;
    LibroDiarioDS ds = new LibroDiarioDS();
    CrystalDecisions.CrystalReports.Engine.ReportDocument rpt;
    ArrayList Arr_Liquidacion = new ArrayList();
    string sql = "";
    string Tipo_Persona = "";
    string Nombre = "";
    string Codigo = "";
    string Banco = "";
    string cta = "";
    string Fecha_Inicial = "";
    string Fecha_Final = "";
    int monID = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["usuario"] == null)
        {
            Response.Redirect("../default.aspx");
        }
        user = (UsuarioBean)Session["usuario"];
        Tipo_Persona = Request.QueryString["tpi"].ToString();
        Codigo = Request.QueryString["codigo"].ToString();
        Banco = Request.QueryString["banco"].ToString();
        cta = Request.QueryString["cta"].ToString();
        Fecha_Inicial = Request.QueryString["fi"].ToString();
        Fecha_Final = Request.QueryString["ff"].ToString();
        monID = int.Parse(Request.QueryString["monid"].ToString());
        #region Contruir SQL
        sql = " and a.tli_pai_id=" + user.PaisID + " and a.tli_conta_id="+user.contaID+" ";
        if (Tipo_Persona != "")
        {
            sql += " and a.tli_tpi_id=" + Tipo_Persona+ " ";
        }
        if (Codigo != "")
        {
            sql += " and a.tli_proveedor_id=" + Codigo + " ";
        }
        if ((Banco != "") && (cta != "0"))
        {
            sql += " and a.tli_tba_id=" + Banco + " ";
        }
        if ((Banco != "") && (cta != "0"))
        {
            sql += " and a.tli_cuenta='" + cta + "' ";
        }
        if (Fecha_Inicial != "")
        {
            sql += " and a.tli_fecha>='" + DB.DateFormat(Fecha_Inicial) + "' ";
        }
        if (Fecha_Final != "")
        {
            sql += " and a.tli_fecha<='" + DB.DateFormat(Fecha_Final) + "' ";
        }
        #endregion
        Arr_Liquidacion = (ArrayList)DB.Get_Liquidaciones(user, sql);
        if (!Page.IsPostBack)
        {
            int i = 0;
            DB.parametros Params = DB.EmpresaParametros(user.PaisID, "", "", "LIQUIDACIONES", ""); //pais, sistema, doc_id, titulo, edicion

            foreach (RE_GenericBean Bean in Arr_Liquidacion)
            {
                Tipo_Persona = Bean.strC3;
                Codigo = Bean.strC4;
                Nombre = Bean.strC5;
			    i++;
                object[] objArr = { Bean.strC1, "", "", "", Bean.strC6, Bean.strC7, Bean.strC8, Bean.strC9, decimal.Parse(Bean.strC10), Bean.strC14, Bean.strC16, Bean.strC17, i == 1 ? Params.logo : null, i == 1 ? Params.titulo : "", i == 1 ? Params.edicion : "" };
                ds.Tables["Liquidaciones"].Rows.Add(objArr);
            }

            if (i == 0) {			
                object[] objArr = { "", "", "", "", "", "", "", "", 0, "", "", "", Params.logo, Params.titulo, Params.edicion  };
                ds.Tables["Liquidaciones"].Rows.Add(objArr);                			
            }

            ViewState["VS_Liquidaciones"] = ds;
        }
        else
        {
            ds = (LibroDiarioDS)ViewState["VS_Liquidaciones"];
        }
        rpt = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
        rpt.Load(Server.MapPath("~/CR_Liquidaciones.rpt"));
        rpt.SetDataSource(ds.Tables["Liquidaciones"]);
        rpt.SetParameterValue("empresa", Server.MapPath(user.pais.Imagepath));
        rpt.SetParameterValue("pais", user.pais.Nombre);
        if (user.contaID.ToString() == "1")
        {
            rpt.SetParameterValue("contabilidad", "FISCAL");
        }
        if (user.contaID.ToString() == "2")
        {
            rpt.SetParameterValue("contabilidad", "FINANCIERA");
        }
        if (user.contaID.ToString() == "3")
        {
            rpt.SetParameterValue("contabilidad", "CONSOLIDADO");
        }
        rpt.SetParameterValue("moneda", Utility.TraducirMonedaInt(monID));
        rpt.SetParameterValue("tipo_persona", Tipo_Persona.ToUpper());
        rpt.SetParameterValue("codigo", Codigo);
        rpt.SetParameterValue("nombre", Nombre);
        rpt.SetParameterValue("usuario", user.ID);
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