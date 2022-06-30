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
    CrystalDecisions.CrystalReports.Engine.ReportDocument rpt;
    UsuarioBean user = null;
    ArrayList arr = null;
    ArrayList Datos = null;
    string fecha_inicial = "";
    string fecha_final = "";
    string cue_nombre = "";
    string cue_id = "";
    string consMoneda = "";
   
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["usuario"] == null)
        {
            Response.Redirect("../default.aspx");
        }
        user = (UsuarioBean)Session["usuario"];
        LibroDiarioDS ds = new LibroDiarioDS();
        #region Backup 2012-03-27
        //int monID = int.Parse(Request.QueryString["monID"].ToString());
        //int contaID = int.Parse(Request.QueryString["contaID"].ToString());
        //fecha_inicial = Request.QueryString["fecha_inicial"].ToString();
        //fecha_final = Request.QueryString["fecha_final"].ToString();
        //cue_id = Request.QueryString["cue_id"].ToString();
        #endregion
        string QUERY_STRING = Request.ServerVariables["QUERY_STRING"].ToString();
        QUERY_STRING = Page.Server.UrlDecode(QUERY_STRING);
        string[] Variables = QUERY_STRING.Split('?');
        fecha_inicial = Variables[0].ToString();
        fecha_final = Variables[1].ToString();
        cue_id = Variables[2].ToString();
        int contaID = int.Parse(Variables[3].ToString());
        int monID = int.Parse(Variables[4].ToString());
        int num = Variables.Count();
        if (num < 6)
        {
            consMoneda = "";
        }
        else
        {
            consMoneda = Variables[5].ToString();
        }
        //consMoneda = Variables[5].ToString();
        RE_GenericBean Cuenta_Bean = DB.getCtabyCtaID(cue_id);
        if (!IsPostBack)
        {

            #region Registrar Log de Reportes
            RE_GenericBean Bean_Log = new RE_GenericBean();
            Bean_Log.intC1 = user.PaisID;
            Bean_Log.strC1 = "SUBREPORTE ESTADO DE RESULTADOS";
            Bean_Log.strC2 = user.ID;
            Bean_Log.strC3 = Request.ServerVariables["REMOTE_ADDR"].ToString();
            string mensaje_log = "";
            mensaje_log += "Contabilidad.: " + contaID + " Moneda.: " + monID + " ,";
            mensaje_log += "Cuenta ID.: " + cue_id + " ,";
            mensaje_log += "Fecha Inicial.: " + fecha_inicial + " Fecha Final.: " + fecha_final + " ,";
            Bean_Log.strC4 = mensaje_log;
            DB.Insertar_Log_Reportes(Bean_Log);
            #endregion
            //ds = DB.getPartidasByCuentaContableForER2(cue_id, user, fecha_inicial, fecha_final, contaID, monID, consMoneda, Response);

            int i = 0;
            DB.parametros Params = DB.EmpresaParametros(user.PaisID, "", "", "MOVIMIENTO CONTABLE " + Cuenta_Bean.strC2, ""); //pais, sistema, doc_id, titulo, edicion

            arr = (ArrayList)DB.getPartidasByCuentaContableForER(cue_id, user, fecha_inicial, fecha_final, contaID, monID, consMoneda);
            foreach (RE_GenericBean Partidas in arr)
            {
                cue_nombre= Partidas.strC3;
                Datos = (ArrayList)DB.getSerieCorrAndObsByDoc(Partidas.intC1, Partidas.intC2);
                if (Datos != null)
                {
                    if (Datos.Count > 0)
                    {
                        i++;
                        object[] ObjArr = { Partidas.strC1, Datos[0].ToString(), Datos[1].ToString(), Datos[2].ToString(), Partidas.decC1, Partidas.decC2, Partidas.strC2.Substring(0, 10), Partidas.strC5, Datos[3].ToString(), Partidas.strC6, "", Datos[4].ToString(), "", "", "", "", Datos[6].ToString(), i == 1 ? Params.logo : null, i == 1 ? Params.titulo : "", i == 1 ? Params.edicion : "" };
                        ds.Tables["SubReporte_BS"].Rows.Add(ObjArr);
                    }
                    else
                    {
                        i++;
                        object[] ObjArr2 = { Partidas.strC1, "Pnd.", "Pnd.", "Pnd.", Partidas.decC1, Partidas.decC2, Partidas.strC2.Substring(0, 10), Partidas.strC5, "Pnd.", Partidas.strC6, "Pnd.", "", "", "", "", "", "", i == 1 ? Params.logo : null, i == 1 ? Params.titulo : "", i == 1 ? Params.edicion : "" };
                        ds.Tables["SubReporte_BS"].Rows.Add(ObjArr2);
                    }

                }
                else
                {
                    i++;
                    object[] ObjArr2 = { Partidas.strC1, "Pnd.", "Pnd.", "Pnd.", Partidas.decC1, Partidas.decC2, Partidas.strC2.Substring(0, 10), Partidas.strC5, "Pnd.", Partidas.strC6, "Pnd.", "", "", "", "", "", "", i == 1 ? Params.logo : null, i == 1 ? Params.titulo : "", i == 1 ? Params.edicion : "" };
                    ds.Tables["SubReporte_BS"].Rows.Add(ObjArr2);
                }

            }

            if (i == 0)
            {
                object[] ObjArr = { "", "", "", "", 0, 0, "", "", "", "", "", "", "", "", "", "", "", Params.logo, Params.titulo, Params.edicion };
                ds.Tables["SubReporte_BS"].Rows.Add(ObjArr);
            }

            
            ViewState["SubReport_ER"] = ds;//Definir ViewState
            lbl_cue_nombre.Text = Cuenta_Bean.strC2;//Definir ViewState para mantener el nombre de la cuenta
            #region Construir Crystal Report
            rpt = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
            rpt.Load(Server.MapPath("~/CR_SubReporte_ER.rpt"));
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
            ds = (LibroDiarioDS)ViewState["SubReport_ER"];
            cue_nombre = lbl_cue_nombre.Text;
            rpt = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
            rpt.Load(Server.MapPath("~/CR_SubReporte_ER.rpt"));
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
            rpt.SetParameterValue("cue_nombre", cue_nombre);
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
