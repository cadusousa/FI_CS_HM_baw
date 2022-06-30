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

public partial class Reports_estadoctaagente : System.Web.UI.Page
{
    UsuarioBean user = null;
    LibroDiarioDS ds = new LibroDiarioDS();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["usuario"] == null)
        {
            Response.Redirect("../default.aspx");
        }

        user = (UsuarioBean)Session["usuario"];
        int id = int.Parse(Request.QueryString["id"].ToString());
        int correlativo = int.Parse(Request.QueryString["correlativo"].ToString());
        string serie = Request.QueryString["serie"].ToString();
        string usuario = Request.QueryString["user"].ToString();
        if (!Page.IsPostBack)
        {
            int i = 0;
            DB.parametros Params = DB.EmpresaParametros(user.PaisID, "", "", "RESUMEN DE CAJA CHICA", ""); //pais, sistema, doc_id, titulo, edicion

            ArrayList transacciones = (ArrayList)DB.getTransactions(id);//2 porque es tipopersona=agente segun tbl_tipo_persona
            RE_GenericBean operacion = null;
            foreach (RE_GenericBean rgb in transacciones)
            {
                if (rgb.intC4 == 3)
                { //NC
                    ArrayList temp = (ArrayList)DB.getNotasCreditoData(rgb.intC1);//obtengo a partir de la referencia
                    operacion = (RE_GenericBean)temp[0];
                    RE_GenericBean viaje = (RE_GenericBean)DB.getViaje(operacion.strC16, user.pais.schema);//obtengo los datos del viaje a partir del contenedor
                    operacion.strC17 = viaje.strC1;//vapor (barco)
                    operacion.strC18 = viaje.strC2;//No de viaje
			        i++;
                    object[] objArr = { operacion.strC2, rgb.strC1, "Nota Credito", operacion.strC17 + " " + operacion.strC18, operacion.decC1, 0, i == 1 ? Params.logo : null, i == 1 ? Params.titulo : "", i == 1 ? Params.edicion : "" };
                    ds.Tables["estadoctaagente_tbl"].Rows.Add(objArr);
                }
                if (rgb.intC4 == 4)
                { //ND
                    operacion = (RE_GenericBean)DB.getNotaDebitoData(rgb.intC1);
                    RE_GenericBean viaje = (RE_GenericBean)DB.getViaje(operacion.strC9, user.pais.schema);//obtengo los datos del viaje a partir del contenedor
                    operacion.strC17 = viaje.strC1;//vapor (barco)
                    operacion.strC18 = viaje.strC2;//No de viaje
                    i++;
                    object[] objArr = { operacion.strC3, rgb.strC1, "Nota Debito", operacion.strC17 + " " + operacion.strC18, 0, operacion.decC1, operacion.strC8, operacion.strC7, operacion.strC9, i == 1 ? Params.logo : null, i == 1 ? Params.titulo : "", i == 1 ? Params.edicion : "" };
                    ds.Tables["estadoctaagente_tbl"].Rows.Add(objArr);
                }
                if (rgb.intC4 == 5)
                { //provision
                    string where = " and b.tpr_prov_id=" + rgb.intC1;
                    ArrayList temp = (ArrayList)DB.getProvision(where);//obtengo a partir de la referencia
                    operacion = (RE_GenericBean)temp[0];
                    RE_GenericBean viaje = (RE_GenericBean)DB.getViaje(operacion.strC16, user.pais.schema);//obtengo los datos del viaje a partir del contenedor
                    i++;        			
                    object[] objArr = { operacion.strC1, rgb.strC1, "Provision", operacion.strC17, 0, operacion.decC1, operacion.strC13, operacion.strC14, operacion.strC16, i == 1 ? Params.logo : null, i == 1 ? Params.titulo : "", i == 1 ? Params.edicion : "" };
                    ds.Tables["estadoctaagente_tbl"].Rows.Add(objArr);
                }
                operacion = null;
            }

            if (i == 0) {			                			
                object[] objArr = { "", "", "", "", 0, 0, "", "", "", Params.logo, Params.titulo, Params.edicion };
                ds.Tables["estadoctaagente_tbl"].Rows.Add(objArr);
            }

            ViewState["EstadoCtaAgente_tbl"] = ds;
        }
        else
        {
            ds = (LibroDiarioDS)ViewState["EstadoCtaAgente_tbl"];
        }
        CrystalDecisions.CrystalReports.Engine.ReportDocument rpt = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
        rpt.Load(Server.MapPath("~/CR_CajaChica.rpt"));
        rpt.SetDataSource(ds.Tables["EstadoCtaAgente_tbl"]);
        rpt.SetParameterValue("fecha_inicial", "2010-05-10");
        rpt.SetParameterValue("fecha_final", "2010-05-14");
        rpt.SetParameterValue("prepara", user.ID);
        rpt.SetParameterValue("refNo", serie+""+correlativo);
        rpt.SetParameterValue("provName", usuario);
        rpt.SetParameterValue("country", user.pais.Nombre);
        rpt.SetParameterValue("autoriza", "");
        CrystalReportViewer1.DisplayGroupTree = false;
        CrystalReportViewer1.ReportSource = rpt;
        CrystalReportViewer1.DataBind();
    }
}
