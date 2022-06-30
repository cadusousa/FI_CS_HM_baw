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

public partial class Reports_SubReporte_BS : System.Web.UI.Page
{
    UsuarioBean user = null;
    ArrayList arr = null;
    ArrayList Datos = null;
    string fecha_inicial = "";
    string consMon = "";
    string fecha_final = "";
    string cue_nombre = "";
    string cue_id = "";
    int contaID = 0;
    int monID = 0;
    CrystalDecisions.CrystalReports.Engine.ReportDocument rpt;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["usuario"] == null)
        {
            Response.Redirect("../default.aspx");
        }
        user = (UsuarioBean)Session["usuario"];
        LibroDiarioDS ds = new LibroDiarioDS();
        if (Request.QueryString["typerep"] != null)
        {
            fecha_inicial = Request.QueryString["fecha_inicial"].ToString();
            fecha_final = Request.QueryString["fecha_final"].ToString();
            cue_id = Request.QueryString["cue_id"].ToString();
            contaID = int.Parse(Request.QueryString["contaID"].ToString());
            monID = int.Parse(Request.QueryString["monID"].ToString());
            consMon = "";
        }
        else
        {
            string QUERY_STRING = Request.ServerVariables["QUERY_STRING"].ToString();
            QUERY_STRING = Page.Server.UrlDecode(QUERY_STRING);
            string[] Variables = QUERY_STRING.Split('?');
            fecha_inicial = Variables[0];
            fecha_final = Variables[1];
            cue_id = Variables[2];
            contaID = int.Parse(Variables[3]);
            monID = int.Parse(Variables[4]);
            int num = Variables.Count();
            if (num < 6)
            {
                consMon = "";
            }
            else
            {
                consMon = Variables[5];
            }
        }


        RE_GenericBean Cuenta_Bean = DB.getCtabyCtaID(cue_id);
        if (!IsPostBack)
        {

            #region Registrar Log de Reportes
            RE_GenericBean Bean_Log = new RE_GenericBean();
            Bean_Log.intC1 = user.PaisID;
            Bean_Log.strC1 = "SUBREPORTE BALANCE DE SALDOS";
            Bean_Log.strC2 = user.ID;
            Bean_Log.strC3 = Request.ServerVariables["REMOTE_ADDR"].ToString();
            string mensaje_log = "";
            mensaje_log += "Contabilidad.: " + contaID + " Moneda.: " + monID + " ,";
            mensaje_log += "Cuenta ID.: " + cue_id + " ,";
            mensaje_log += "Fecha Inicial.: " + fecha_inicial + " Fecha Final.: " + fecha_final + " ,";
            Bean_Log.strC4 = mensaje_log;
            DB.Insertar_Log_Reportes(Bean_Log);
            #endregion

            int i = 0;
            DB.parametros Params = DB.EmpresaParametros(user.PaisID, "", "", "MOVIMIENTO CONTABLE " + Cuenta_Bean.strC2, ""); //pais, sistema, doc_id, titulo, edicion

            object[] ObjArr2 = { "", "", "", "", 0, 0, "", "", "", "", "", "", "", "", "", "", "", Params.logo, Params.titulo, Params.edicion };
            ds.Tables["SubReporte_BS"].Rows.Add(ObjArr2);                			

            arr = (ArrayList)DB.getPartidasByCuentaContable(cue_id, user, fecha_inicial, fecha_final, contaID, monID, consMon);
            foreach (RE_GenericBean Partidas in arr)
            {
                cue_nombre= Partidas.strC3;
                #region Si es recibo traer Pago
                if (Partidas.strC15 == "RE")
                {
                    string Detalle_Pagos = "";
                    Detalle_Pagos = DB.getPagosRecibo_STR(Partidas.intC2);
                    Partidas.strC16 += "      " + Detalle_Pagos;
                }
                if (Partidas.strC15 == "RCP")
                {
                    string Detalle_Pagos_Proveedor = "";
                    Detalle_Pagos_Proveedor = DB.getPagosReciboCorte_STR(Partidas.intC2);
                    Partidas.strC16 += "      " + Detalle_Pagos_Proveedor;
                }
                #endregion
			    i++;
                object[] ObjArr = { Partidas.strC1, Partidas.strC15, Partidas.strC13, Partidas.strC14, Partidas.decC1, Partidas.decC2, Partidas.strC2.Substring(0, 10), Partidas.strC5, Partidas.strC16, Partidas.strC6, Partidas.strC7, Partidas.strC8, Partidas.strC9, Partidas.strC10, Partidas.strC11, Partidas.strC12, "", i == 1 ? Params.logo : null, i == 1 ? Params.titulo : "", i == 1 ? Params.edicion : "" };
                ds.Tables["SubReporte_BS"].Rows.Add(ObjArr);
            }
            
            /*if (i == 0) {                
                object[] ObjArr = { "", "", "", "", 0, 0, "", "", "", "", "", "", "", "", "", "", "", Params.logo, Params.titulo, Params.edicion };
                ds.Tables["SubReporte_BS"].Rows.Add(ObjArr);                			
            }*/

            ViewState["SubReport_BS"] = ds;//Definir ViewState
            lbl_cue_nombre.Text = Cuenta_Bean.strC2;//Definir ViewState para mantener el nombre de la cuenta
            #region Construir Crystal Report
            rpt = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
            rpt.Load(Server.MapPath("~/CR_SubReporte_BS.rpt"));
            rpt.SetDataSource(ds.Tables["SubReporte_BS"]);
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
            //rpt.SetParameterValue("cue_nombre",Cuenta_Bean.strC2 );
            rpt.SetParameterValue("cue_id",cue_id);
            rpt.SetParameterValue("empresa", Server.MapPath(user.pais.Imagepath));
            CrystalReportViewer1.ReportSource = rpt;
            CrystalReportViewer1.DataBind();
            #endregion
        }
        else
        {
            #region Construir Crystal Report
            ds = (LibroDiarioDS)ViewState["SubReport_BS"];
            cue_nombre = lbl_cue_nombre.Text;
            rpt = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
            rpt.Load(Server.MapPath("~/CR_SubReporte_BS.rpt"));
            rpt.SetDataSource(ds.Tables["SubReporte_BS"]);
            if (user.contaID == 1)
            {
                rpt.SetParameterValue("contabilidad", "FISCAL");
            }
            else if (user.contaID == 2)
            {
                rpt.SetParameterValue("contabilidad", "FINANCIERA");
            }
            else if (user.contaID == 3)
            {
                rpt.SetParameterValue("contabilidad", "CONSOLIDADO");
            }
            rpt.SetParameterValue("fecha_inicial", fecha_inicial);
            rpt.SetParameterValue("fecha_final", fecha_final);
            rpt.SetParameterValue("pais", user.pais.Nombre);
            //rpt.SetParameterValue("cue_nombre", cue_nombre);
            rpt.SetParameterValue("cue_id", cue_id);
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
